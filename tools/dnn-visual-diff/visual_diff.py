#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""dnn-visual-diff — visual comparison of the production vs preprod DNN hosts.

Companion of docs/dnn/visual-diff-runbook.md. Design provenance (measured,
issue #1180 comments, Aug 2026): dual diff metric (% differing pixels AND %
with per-channel |delta| >= 16/255 — a ~5000x discriminator between an
identical render and genuinely different pages), mandatory synthetic inverse
control, known-delta classification (D1..D7) BEFORE pixel diff, cache-buster
discipline with marker-based build identification.

Hard rules (runbook §3-4):
- READ-ONLY client: navigate + screenshot only. No login, no server mutation,
  no server-side diagnostics (Default.aspx/assembly audit = po-2023 queue).
- NO mask by default; --double-capture (same-host A/B) yields an explicit
  NO-VERDICT on nondeterminism, never a masked PASS.
- Dimension differences are explicit (ECART-DIMENSIONS); images are NEVER
  resized for comparison. Display copies in the montage are never an input
  to a verdict.
- Memory guard BEFORE decoding and BEFORE the screenshot: page dimensions
  are probed in the browser; beyond --max-mp the screenshot is not taken and
  the route yields NO-VERDICT (memory-limit). A route whose capture tripped
  the guard is never compared.
- Freshness is always "unknown". Declared marker expectations are checked
  separately (matched / mismatched / none-declared); a mismatch is a route
  error. Even matching markers do not prove that a deployment is current.
- HTTP errors (>=400) always yield an explicit failing verdict — even when
  the status was declared expected (separate verdict ATTENDU-HTTP keeps the
  expected state distinct from unexpected HTTP-ERROR). An error page that
  happens to render identically on both hosts can NEVER produce an
  IDENTICAL verdict.
- A low strong-delta is reported against a DESCRIPTIVE threshold only: the
  tool does not assert the cause (antialiasing or otherwise).
- Uniform (single-colour) or tiny captures are flagged as suspicious — an
  error page or a blank render is never silently treated as a valid render.
- NO verdict of this tool is an ergonomics/aesthetics PASS (report banner).

Modes: --mode full (default) = capture + report; capture = capture only
(manifest with entries); report = rebuild the report OFFLINE from an existing
manifest (fingerprints re-verified against the PNG files).

Exit codes (modes full/report): 0 = every route has a comparable,
non-failing verdict; 1 = global failure (zero pairs, instrument error,
>=1 route in ERROR / NO-VERDICT / HTTP-ERROR / ATTENDU-HTTP).
DIFF-A-CLASSER and ECART-DIMENSIONS are results, not tool failures.
In capture mode, exit 0 only means the manifest was written — NOT that the
captures succeeded (per-route errors are recorded in the manifest and get
qualified by the next report run).
"""

from __future__ import annotations

import argparse
import hashlib
import html as html_mod
import json
import os
import re
import subprocess
import sys
import time
from dataclasses import dataclass, asdict
from datetime import datetime
from typing import Dict, List, Optional, Tuple

try:
    from PIL import Image, ImageChops, ImageDraw
except ImportError:  # pragma: no cover - environment guard, self-test needs PIL
    Image = None  # type: ignore

# ---------------------------------------------------------------------------
# Constants (measured or deliberate; see module docstring and runbook)
# ---------------------------------------------------------------------------

DEFAULT_VIEWPORT = (1440, 900)      # matches the Aug-2026 E2 measurements
STRONG_DELTA_THRESHOLD = 16         # per-channel |delta| >= 16/255 (E2 metric)
LOW_DELTA_MAX_STRONG_PCT = 0.05     # DESCRIPTIVE threshold only: pairs below it
                                    # are "IDENTIQUE-SOUS-SEUIL" — the tool does
                                    # NOT assert the cause of the residual delta
                                    # (E2 measured 0.0039% strong on a pair
                                    # rendered identically, but that was ONE
                                    # measurement, not a law)
INVERSE_CONTROL_FLOOR_PCT = 1.0     # synthetic known difference must be seen
                                    # at >= 1% strong or the engine is blind
DEFAULT_MAX_MP = 40.0               # per-image megapixel guard (memory limit)
DEFAULT_TIMEOUT_MS = 60_000
DEFAULT_SETTLE_MS = 800             # post-networkidle settle before screenshot
MIN_SUSPECT_DIM = 48                # captures smaller than this = suspicious

VERDICT_IDENTICAL = "IDENTIQUE-SOUS-SEUIL"
VERDICT_KNOWN_DELTA = "DELTA-CONNU"
VERDICT_DIFF = "DIFF-A-CLASSER"
VERDICT_DIM_GAP = "ECART-DIMENSIONS"
VERDICT_ERROR = "ERROR"
VERDICT_NO_VERDICT = "NO-VERDICT"
VERDICT_HTTP_ERROR = "HTTP-ERROR"
VERDICT_EXPECTED_HTTP = "ATTENDU-HTTP"

FAILING_VERDICTS = {VERDICT_ERROR, VERDICT_NO_VERDICT,
                    VERDICT_HTTP_ERROR, VERDICT_EXPECTED_HTTP}

AESTHETIC_BANNER = (
    "Aucun verdict de cet outil ne vaut une validation ergonomique ou "
    "esthétique. Les métriques pixel classifient des écarts techniques ; "
    "le jugement de goût reste au regard humain (cf. runbook §4)."
)


# ---------------------------------------------------------------------------
# Errors
# ---------------------------------------------------------------------------

class VisualDiffError(Exception):
    """Base class for explicit, reportable tool errors."""


class RouteConfigError(VisualDiffError):
    """routes.json is structurally invalid."""


class CaptureError(VisualDiffError):
    """A capture could not be produced (navigation, HTTP, empty file...)."""


class EmptyCaptureError(CaptureError):
    """The screenshot file is empty / not a decodable image."""


class CorruptImageError(VisualDiffError):
    """An image file exists but cannot be decoded."""


class MemoryLimitError(VisualDiffError):
    """An image exceeds the megapixel guard (probed before decoding)."""


class InstrumentError(VisualDiffError):
    """The inverse control proved the diff engine blind — verdicts are void."""


# ---------------------------------------------------------------------------
# Pure helpers (no playwright import required — unit-tested offline)
# ---------------------------------------------------------------------------

def slugify(name: str) -> str:
    """Filesystem-safe, stable slug for a route name (also used in the report).
    Accents are transliterated (NFKD) so 'Règles' -> 'regles' — and so that
    'Règles' vs 'regles' COLLIDES, which load_routes rejects: two route names
    must never silently map to the same artifact files."""
    import unicodedata
    decomposed = unicodedata.normalize("NFKD", name.strip().lower())
    ascii_only = decomposed.encode("ascii", "ignore").decode("ascii")
    s = re.sub(r"[^A-Za-z0-9._-]+", "-", ascii_only)
    return s.strip("-") or "route"


def sha256_bytes(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def sha256_file(path: str) -> str:
    h = hashlib.sha256()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(1 << 20), b""):
            h.update(chunk)
    return h.hexdigest()


def now_iso() -> str:
    """Timezone-aware timestamp (local offset included)."""
    return datetime.now().astimezone().isoformat(timespec="seconds")


def source_ref() -> Dict:
    """Identify the running tool: file path + short git rev when available.
    Recorded in the manifest so a report can name the exact source that
    produced its captures."""
    rev = "unknown"
    try:
        rev = subprocess.run(
            ["git", "rev-parse", "--short", "HEAD"],
            capture_output=True, text=True, timeout=10,
            cwd=os.path.dirname(os.path.abspath(__file__))).stdout.strip() or "unknown"
    except Exception:
        pass
    return {"tool": os.path.abspath(__file__), "git_rev": rev}


# --- routes configuration ---------------------------------------------------

@dataclass
class RoutePair:
    name: str
    prod_path: str
    preprod_path: Optional[str] = None      # defaults to prod_path
    enabled: bool = True
    expect_login_redirect: bool = False     # account/checkout pages
    expected_status_both: Optional[int] = None  # e.g. 404 for Activity-Feed*
    expected_markers: Optional[Dict[str, Dict[str, str]]] = None
    freshness_note: str = ""
    note: str = ""


def load_routes(path: str) -> Tuple[Dict, List[RoutePair]]:
    """Load and validate routes.json. Raises RouteConfigError with an explicit
    message on any structural problem — a config that silently yields zero
    pairs is a tool failure, never a quiet empty run."""
    try:
        with open(path, "r", encoding="utf-8") as fh:
            cfg = json.load(fh)
    except (OSError, json.JSONDecodeError) as exc:
        raise RouteConfigError(f"cannot read routes file {path!r}: {exc}") from exc

    if not isinstance(cfg, dict):
        raise RouteConfigError("routes file root must be an object")
    for key in ("hosts", "pairs"):
        if key not in cfg:
            raise RouteConfigError(f"routes file missing required key {key!r}")
    hosts = cfg["hosts"]
    if not isinstance(hosts, dict) or not hosts.get("prod") or not hosts.get("preprod"):
        raise RouteConfigError("hosts must define non-empty 'prod' and 'preprod'")

    raw_pairs = cfg["pairs"]
    if not isinstance(raw_pairs, list) or not raw_pairs:
        raise RouteConfigError("pairs must be a non-empty list (zero pairs is a config error)")

    seen_slugs: Dict[str, str] = {}
    pairs: List[RoutePair] = []
    for i, raw in enumerate(raw_pairs):
        if not isinstance(raw, dict) or not raw.get("name") or not raw.get("prod_path"):
            raise RouteConfigError(
                f"pair #{i} must be an object with at least 'name' and 'prod_path'")
        em = raw.get("expected_markers")
        if em is not None and not isinstance(em, dict):
            raise RouteConfigError(f"pair {raw['name']!r}: expected_markers must be an object")
        rp = RoutePair(
            name=str(raw["name"]),
            prod_path=str(raw["prod_path"]),
            preprod_path=(str(raw["preprod_path"]) if raw.get("preprod_path") else None),
            enabled=bool(raw.get("enabled", True)),
            expect_login_redirect=bool(raw.get("expect_login_redirect", False)),
            expected_status_both=(int(raw["expected_status_both"])
                                  if raw.get("expected_status_both") is not None else None),
            expected_markers=em,
            freshness_note=str(raw.get("freshness_note", "")),
            note=str(raw.get("note", "")),
        )
        slug = slugify(rp.name)
        if slug in seen_slugs:
            raise RouteConfigError(
                f"pair {rp.name!r}: slug {slug!r} collides with {seen_slugs[slug]!r}")
        seen_slugs[slug] = rp.name
        pairs.append(rp)
    return hosts, pairs


# --- cache buster -----------------------------------------------------------

def append_cache_buster(url: str, token: str) -> str:
    """Append cb=<token> as a query parameter. A fragment (#), when present,
    stays LAST (query params inserted before it); an existing query is
    extended with '&'."""
    if not token:
        return url
    fragment = ""
    if "#" in url:
        url, fragment = url.split("#", 1)
        fragment = "#" + fragment
    sep = "&" if ("?" in url and not url.rstrip().endswith("?")) else "?"
    return f"{url}{sep}cb={token}{fragment}"


_BUSTER_PATH_RE = re.compile(r"/cb/\d+(/|$)")
_BUSTER_QUERY_RE = re.compile(r"[?&]cb=\d+")


def detect_buster_outcome(requested_url: str, final_url: str) -> str:
    """Classify what happened to the cb parameter: 'kept-query' |
    'rewritten-to-path' | 'lost'. A rewrite is not a failure by itself
    (measured on the DNN root) but must be traced — the build markers, not
    the buster, identify what answered."""
    if _BUSTER_QUERY_RE.search(final_url):
        return "kept-query"
    if _BUSTER_PATH_RE.search(final_url):
        return "rewritten-to-path"
    return "lost"


# --- build markers & freshness semantics -------------------------------------

_GTID_RE = re.compile(r"GTM-[A-Z0-9]{5,}")
_CDV_RE = re.compile(r"[?&]cdv=([0-9]+)")
_JQUERY_RE = re.compile(r"jquery[.-](\d+\.\d+\.\d+(?:\.\d+)?)\.min\.js", re.IGNORECASE)


def parse_freshness_markers(page_html: str) -> Dict:
    """Extract BUILD markers from served HTML. These identify WHICH build
    answered (preprod 2sxc 21.x emits 'tosic.sxc', prod 15.2
    'tosic_sexycontent'; cdv 181 vs 325 measured; GTM id + <noscript> track
    D4). Marker presence is NOT proof of currency — an OLD page carries old
    markers. Explicit expectations (check_marker_expectations) only verify
    the declared BUILD IDENTITY (matched / mismatched); they NEVER establish
    currency or deployment freshness, which stays "unknown" client-side."""
    return {
        "tosic_sxc": page_html.count("tosic.sxc"),
        "tosic_sexycontent": page_html.count("tosic_sexycontent"),
        "gtm_ids": sorted(set(_GTID_RE.findall(page_html))),
        "gtm_noscript": page_html.count("googletagmanager.com/ns.html"),
        "cdv": (m.group(1) if (m := _CDV_RE.search(page_html)) else None),
        "jquery": (m.group(1) if (m := _JQUERY_RE.search(page_html)) else None),
        "jquery_migrate": page_html.count("jquery-migrate"),
        "onlinewebfonts_http": page_html.count("http://db.onlinewebfonts.com"),
        "html_len": len(page_html),
    }


def markers_present(markers: Dict) -> bool:
    """At least one build marker in the served HTML. This says a DNN build
    answered — it does NOT say it is the current build."""
    return bool(
        markers.get("tosic_sxc")
        or markers.get("tosic_sexycontent")
        or markers.get("gtm_ids")
        or markers.get("cdv")
        or markers.get("jquery")
    )


def check_expected_markers(markers: Dict, expected: Dict[str, str]) -> Tuple[bool, List[str]]:
    """Check the declared marker expectations against the observed markers.
    Key syntax: "field" (default op >, for counts), "field>", "field>=",
    "field=", "field!=" / "field<=" / "field>=". Value = the operand.
    Examples: {"tosic_sexycontent>": "0"}, {"cdv=": "325"}, {"cdv!=": "181"}.
    Returns (all_ok, details)."""
    details: List[str] = []
    ok_all = True
    for spec, want in expected.items():
        m = re.match(r"^([A-Za-z0-9_]+)(>=|<=|!=|=|>)?$", spec)
        if m is None:
            details.append(f"{spec!r}: unparsable spec -> VIOLATED")
            ok_all = False
            continue
        field, op = m.group(1), (m.group(2) or ">")
        got = markers.get(field)
        if op == ">":
            ok = (got is not None and isinstance(got, int) and got > int(want))
        elif op == ">=":
            ok = (got is not None and isinstance(got, int) and got >= int(want))
        elif op == "<":
            ok = (got is not None and isinstance(got, int) and got < int(want))
        elif op == "<=":
            ok = (got is not None and isinstance(got, int) and got <= int(want))
        elif op == "!=":
            ok = (str(got) != str(want))
        else:  # "="
            ok = (str(got) == str(want))
        details.append(f"{spec} {want}: got {got!r} -> {'OK' if ok else 'VIOLATED'}")
        ok_all = ok_all and ok
    return ok_all, details


def check_marker_expectations(markers: Dict,
                              expected: Optional[Dict[str, str]]) -> Tuple[str, List[str]]:
    """Compare observed build markers against the per-route declared
    expectations. Outcomes: 'none-declared' (no expectation configured —
    INCLUDING an empty dict, which declares nothing), 'matched', or
    'mismatched' (explicit route error: the wrong build is suspected).

    IMPORTANT — this is NOT a freshness protocol. Marker expectations
    matching says the served HTML carries the markers that were declared;
    it does NOT prove the deployment is current (an old page carries its
    old markers, and no independent deployment proof exists client-side).
    Freshness is therefore always reported 'unknown'."""
    if not expected:
        return "none-declared", []
    ok, details = check_expected_markers(markers, expected)
    return ("matched" if ok else "mismatched"), details


KNOWN_DELTA_REGISTRY: List[Tuple[str, str, str, str]] = [
    # (id, label, condition on (prod_markers, preprod_markers), direction note)
    ("D1", "2sxc 15.x (tosic_sexycontent) vs 21.x (tosic.sxc)",
     lambda p, q: (p.get("tosic_sexycontent", 0) > 0 and q.get("tosic_sxc", 0) > 0)
                  or (p.get("tosic_sxc", 0) > 0 and q.get("tosic_sexycontent", 0) > 0),
     "attendu au cutover (montée 15.2 -> 21.7)"),
    ("D2", "jquery-migrate: present on one host only",
     lambda p, q: p.get("jquery_migrate", 0) != q.get("jquery_migrate", 0),
     "mesure #1180: prod seulement, page / — validé au chargement"),
    ("D4", "GTM <noscript> fallback absent côté preprod",
     lambda p, q: p.get("gtm_noscript", 0) > 0 and q.get("gtm_noscript", 0) == 0,
     "seul manque réel côté preprod (mesuré #1180 E6)"),
    ("D5", "cdv cache-versioning differs",
     lambda p, q: p.get("cdv") != q.get("cdv"),
     "versionnage cache décidé 10/08 — présent en preprod, arrive au go-live"),
    ("D6", "jQuery version differs",
     lambda p, q: p.get("jquery") != q.get("jquery"),
     "mesuré 3.5.1 (prod) vs 3.7.1 (preprod)"),
    ("D7", "dead http:// @font-face (onlinewebfonts) count differs",
     lambda p, q: p.get("onlinewebfonts_http", 0) != q.get("onlinewebfonts_http", 0),
     "bloc mort retiré côté preprod — préprod meilleure"),
]


def classify_known_deltas(prod_markers: Dict, preprod_markers: Dict) -> List[str]:
    out = []
    for did, label, cond, note in KNOWN_DELTA_REGISTRY:
        try:
            if cond(prod_markers, preprod_markers):
                out.append(f"{did}: {label} ({note})")
        except Exception:  # defensive: a broken condition must not void the run
            out.append(f"{did}: registry condition error")
    return out


# --- image probes & diff engine ----------------------------------------------

def probe_dimensions(path: str) -> Tuple[int, int]:
    """Read image dimensions from the header WITHOUT decoding pixel data
    (PIL opens lazily until .load()). The memory guard must never need the
    full decode to trip."""
    if not os.path.exists(path):
        raise CorruptImageError(f"image not found: {path}")
    if os.path.getsize(path) == 0:
        raise EmptyCaptureError(f"empty capture file: {path}")
    try:
        img = Image.open(path)
        size = img.size
        img.close()
        return size
    except Exception as exc:
        raise CorruptImageError(f"cannot read header of {path}: {exc}") from exc


def _load_image(path: str) -> "Image.Image":
    if not os.path.exists(path):
        raise CorruptImageError(f"image not found: {path}")
    if os.path.getsize(path) == 0:
        raise EmptyCaptureError(f"empty capture file: {path}")
    try:
        img = Image.open(path)
        img.load()
    except Exception as exc:
        raise CorruptImageError(f"cannot decode {path}: {exc}") from exc
    return img.convert("RGB")


def _max_channel_gray(diff_rgb: "Image.Image") -> "Image.Image":
    r, g, b = diff_rgb.split()
    return ImageChops.lighter(ImageChops.lighter(r, g), b)


@dataclass
class DiffMetrics:
    width_a: int
    height_a: int
    width_b: int
    height_b: int
    dims_match: bool
    pct_raw: float
    pct_strong: float
    max_delta: int


def compute_diff(path_a: str, path_b: str,
                 strong_threshold: int = STRONG_DELTA_THRESHOLD) -> DiffMetrics:
    """Pixel metrics of two images. NO RESIZING EVER: on a dimension mismatch,
    metrics are computed over the common area only and dims_match=False — the
    caller must classify the pair as ECART-DIMENSIONS, never 'identical'."""
    if Image is None:
        raise VisualDiffError("PIL/Pillow is required for the diff engine")
    a = _load_image(path_a)
    b = _load_image(path_b)
    dims_match = a.size == b.size
    cw = min(a.width, b.width)
    ch = min(a.height, b.height)
    if cw == 0 or ch == 0:
        raise CorruptImageError(f"no common area: sizes {a.size} vs {b.size}")
    ca = a.crop((0, 0, cw, ch))
    cb = b.crop((0, 0, cw, ch))
    diff = ImageChops.difference(ca, cb)
    gray = _max_channel_gray(diff)
    hist = gray.histogram()
    total = cw * ch
    raw = sum(hist[1:]) / total * 100.0
    strong = sum(hist[strong_threshold:]) / total * 100.0
    ext = gray.getextrema()
    return DiffMetrics(
        width_a=a.width, height_a=a.height,
        width_b=b.width, height_b=b.height,
        dims_match=dims_match,
        pct_raw=round(raw, 4),
        pct_strong=round(strong, 4),
        max_delta=int(ext[1]),
    )


def is_suspicious_capture(path: str) -> Tuple[bool, str]:
    """Detect captures that are technically non-empty but cannot be a valid
    page render: uniform single-colour image, or tiny dimensions. A blank
    error page must be flagged, not silently compared."""
    try:
        img = _load_image(path)
    except VisualDiffError as exc:
        return True, f"undecodable: {exc}"
    if img.width < MIN_SUSPECT_DIM or img.height < MIN_SUSPECT_DIM:
        return True, f"tiny capture {img.width}x{img.height} < {MIN_SUSPECT_DIM}px"
    extrema = [band.getextrema() for band in img.split()]
    if all(lo == hi for lo, hi in extrema):
        return True, f"uniform capture (single colour {[(lo) for lo, _ in extrema]})"
    return False, ""


def render_heatmap(path_a: str, path_b: str, out_path: str,
                   display_max_width: int = 1440) -> str:
    """Diff heatmap (black = 0, red channel = per-pixel max |delta|). May be
    scaled DOWN for display only; never an input to a verdict."""
    a = _load_image(path_a)
    b = _load_image(path_b)
    cw, ch = min(a.width, b.width), min(a.height, b.height)
    diff = ImageChops.difference(a.crop((0, 0, cw, ch)), b.crop((0, 0, cw, ch)))
    gray = _max_channel_gray(diff)
    heat = Image.merge("RGB", (gray, Image.new("L", gray.size, 0),
                               Image.new("L", gray.size, 0)))
    if heat.width > display_max_width:
        ratio = display_max_width / heat.width
        heat = heat.resize((display_max_width, max(1, int(heat.height * ratio))))
    heat.save(out_path)
    return out_path


def render_side_by_side(path_a: str, path_b: str, label_a: str, label_b: str,
                        out_path: str, display_max_width: int = 720) -> str:
    """Side-by-side montage with host labels. Display copies are scaled for
    the montage only; the raw captures stay untouched and fingerprinted in
    the manifest. A scaled display copy is never compared."""
    a = _load_image(path_a)
    b = _load_image(path_b)

    def fit(img: "Image.Image") -> "Image.Image":
        if img.width > display_max_width:
            ratio = display_max_width / img.width
            return img.resize((display_max_width, max(1, int(img.height * ratio))))
        return img

    ca, cb = fit(a), fit(b)
    label_h = 28
    canvas = Image.new("RGB", (ca.width + cb.width + 12,
                               max(ca.height, cb.height) + label_h + 8), (24, 24, 24))
    draw = ImageDraw.Draw(canvas)
    draw.text((6, 6), label_a[:80], fill=(255, 210, 120))
    draw.text((ca.width + 18, 6), label_b[:80], fill=(120, 210, 255))
    canvas.paste(ca, (0, label_h + 8))
    canvas.paste(cb, (ca.width + 12, label_h + 8))
    canvas.save(out_path)
    return out_path


# --- verdict ----------------------------------------------------------------

def classify_pair(metrics: Optional[DiffMetrics], known_deltas: List[str],
                  errors: List[str]) -> Tuple[str, List[str]]:
    """Route verdict from pixel metrics. Ordering: errors, dimension gap
    (explicit, never resampled away), then the strong-delta DESCRIPTIVE
    threshold — a low delta is 'sous le seuil descriptif', the tool does not
    assert its cause."""
    if errors:
        return VERDICT_ERROR, errors
    if metrics is None:
        return VERDICT_NO_VERDICT, ["no metrics (no comparison performed)"]
    reasons: List[str] = []
    if not metrics.dims_match:
        reasons.append(
            f"dimensions differ: {metrics.width_a}x{metrics.height_a} vs "
            f"{metrics.width_b}x{metrics.height_b} — explicit gap, NOT resampled "
            "(metrics above are over the common area only)")
    if known_deltas:
        reasons.extend(known_deltas)
    if not metrics.dims_match:
        return VERDICT_DIM_GAP, reasons
    if metrics.pct_strong > LOW_DELTA_MAX_STRONG_PCT:
        if not known_deltas:
            reasons.append(
                f"strong delta {metrics.pct_strong:.4f}% > descriptive ceiling "
                f"{LOW_DELTA_MAX_STRONG_PCT}% with NO registered known delta -> to re-verify")
        return VERDICT_DIFF, reasons
    reasons.append(
        f"strong delta {metrics.pct_strong:.4f}% <= descriptive ceiling "
        f"{LOW_DELTA_MAX_STRONG_PCT}% (raw {metrics.pct_raw:.4f}%) — DESCRIPTIVE "
        "threshold: the tool does NOT establish the cause of the residual delta")
    return (VERDICT_KNOWN_DELTA if known_deltas else VERDICT_IDENTICAL), reasons


def http_gate(prod_status, pre_status, expected: Optional[int]) -> Tuple[Optional[str], List[str]]:
    """HTTP-level gate, evaluated BEFORE any pixel comparison. Any HTTP error
    (>=400) yields a failing verdict — even when it matches the declared
    expectation (kept distinct as ATTENDU-HTTP). An error page can therefore
    NEVER be compared into an IDENTIQUE verdict."""
    reasons: List[str] = []
    if expected is not None and prod_status == expected and pre_status == expected:
        reasons.append(f"HTTP {expected} on BOTH hosts as declared expected — "
                       "recorded as ATTENDU-HTTP (still a global failure: the route "
                       "has no render to compare)")
        return VERDICT_EXPECTED_HTTP, reasons
    errs = []
    if isinstance(prod_status, int) and prod_status >= 400:
        errs.append(f"prod HTTP {prod_status}")
    if isinstance(pre_status, int) and pre_status >= 400:
        errs.append(f"preprod HTTP {pre_status}")
    if errs:
        reasons.append("; ".join(errs) + " — error page, comparison not performed")
        return VERDICT_HTTP_ERROR, reasons
    if prod_status != pre_status:
        reasons.append(f"status asymmetry prod={prod_status} vs preprod={pre_status}")
        return VERDICT_HTTP_ERROR, reasons
    return None, reasons  # no HTTP problem; reasons may carry informational notes


# --- synthetic inverse control (MANDATORY) ----------------------------------

def synthetic_images() -> Tuple["Image.Image", "Image.Image"]:
    """Two 240x160 images differing by one 60x60 block position:
    ~2*3600/38400 = 18.75% strong delta expected."""
    base = Image.new("RGB", (240, 160), (200, 200, 200))
    a = base.copy()
    ImageDraw.Draw(a).rectangle([20, 20, 80, 80], fill=(20, 20, 200))
    b = base.copy()
    ImageDraw.Draw(b).rectangle([140, 70, 200, 130], fill=(20, 20, 200))
    return a, b


def inverse_control(tmpdir: str) -> Tuple[bool, float, str]:
    """Prove the engine can see a real difference. If not: INSTRUMENT-ERROR —
    every verdict of the run is void. Synthetic, offline, mandatory."""
    pa = os.path.join(tmpdir, "_ic_a.png")
    pb = os.path.join(tmpdir, "_ic_b.png")
    a, b = synthetic_images()
    a.save(pa)
    b.save(pb)
    m = compute_diff(pa, pb)
    ok = m.pct_strong >= INVERSE_CONTROL_FLOOR_PCT
    return ok, m.pct_strong, (
        f"inverse control synthetic pair: strong={m.pct_strong:.4f}% "
        f"(floor {INVERSE_CONTROL_FLOOR_PCT}%) -> "
        + ("ENGINE SEES REAL DIFFERENCES" if ok else "ENGINE BLIND — verdicts void"))


# --- memory guard (header-level, before decode) ------------------------------

def memory_guard(path: str, max_mp: float) -> None:
    """Raise MemoryLimitError when an image exceeds the megapixel guard.
    Probes dimensions from the header WITHOUT decoding. The caller turns
    this into NO-VERDICT (memory-limit) — never a downscale and never an
    OOM. A capture that tripped the guard is never compared."""
    w, h = probe_dimensions(path)
    mpx = (w * h) / 1_000_000.0
    if mpx > max_mp:
        raise MemoryLimitError(f"{path}: {w}x{h} = {mpx:.1f} MPx > guard {max_mp} MPx")


# ---------------------------------------------------------------------------
# Capture (playwright — lazy import so self-tests run without it)
# ---------------------------------------------------------------------------

def _capture_one(context, url: str, out_png: str, settle_ms: int,
                 timeout_ms: int, max_mp: float) -> Dict:
    """Capture one host. READ-ONLY by construction: navigate + screenshot.
    The fullPage size is bounded BEFORE the screenshot: the document
    dimensions are measured in the browser and a page beyond the megapixel
    guard is NOT screenshotted (sidecar flag memory_limit=True)."""
    from playwright.sync_api import TimeoutError as PwTimeout

    console_errors: List[str] = []
    page = context.new_page()
    page.on("console", lambda m: console_errors.append(m.text) if m.type == "error" else None)
    page.on("pageerror", lambda e: console_errors.append(f"pageerror: {e}"))
    try:
        resp = page.goto(url, wait_until="domcontentloaded", timeout=timeout_ms)
        status = resp.status if resp is not None else None
        try:
            page.wait_for_load_state("networkidle", timeout=timeout_ms)
        except PwTimeout:
            console_errors.append("networkidle timeout (settled on domcontentloaded)")
        if settle_ms:
            page.wait_for_timeout(settle_ms)
        final_url = page.url
        html = page.content()
        markers = parse_freshness_markers(html)

        # Memory bound BEFORE the screenshot: probe document dimensions in
        # the browser, skip the capture entirely when beyond the guard.
        doc_w, doc_h = page.evaluate(
            "() => [document.documentElement.scrollWidth,"
            " document.documentElement.scrollHeight]")
        mpx = (doc_w * doc_h) / 1_000_000.0
        if mpx > max_mp:
            return {
                "requested_url": url, "final_url": final_url,
                "http_status": status, "captured_at": now_iso(),
                "console_error_count": len(console_errors),
                "console_errors_first": console_errors[:5],
                "markers": markers, "screenshot": None, "memory_limit": True,
                "page_dims": [doc_w, doc_h],
            }

        page.screenshot(path=out_png, full_page=True)
        if not os.path.exists(out_png) or os.path.getsize(out_png) == 0:
            raise EmptyCaptureError(f"screenshot produced an empty file for {url}")
        return {
            "requested_url": url, "final_url": final_url,
            "http_status": status, "captured_at": now_iso(),
            "console_error_count": len(console_errors),
            "console_errors_first": console_errors[:5],
            "markers": markers,
            "screenshot": os.path.basename(out_png), "memory_limit": False,
            "page_dims": [doc_w, doc_h],
        }
    except CaptureError:
        raise
    except Exception as exc:  # navigation failure, timeout, crash -> explicit
        raise CaptureError(f"{type(exc).__name__}: {exc}") from exc
    finally:
        page.close()


def record_nondeterminism(entry: Dict, host_key: str, m: DiffMetrics,
                          second: Dict, out_dir: str) -> bool:
    """ACCUMULATE same-host A/B results per host (never overwrite). Returns
    the nondeterminism verdict. A dimension mismatch between the two
    same-host loads (e.g. 64x64 vs 64x128) makes the page nondeterministic
    EVEN IF the common area is pixel-identical — a stability claim on the
    common area alone would be invalid. Traces the second capture's
    filename, dimensions, fingerprint, status and timestamp; the second
    capture's 'screenshot' is a BASENAME resolved against out_dir (never
    the process cwd) so report mode can re-verify the fingerprint later."""
    name2 = second.get("screenshot")
    p2 = os.path.join(out_dir, name2) if name2 else None
    nondet = (m.pct_strong > LOW_DELTA_MAX_STRONG_PCT) or (not m.dims_match)
    entry.setdefault("nondeterminism", {})[host_key] = {
        "pct_strong_same_host": m.pct_strong,
        "dims_match": m.dims_match,
        "nondeterministic": nondet,
        "second_capture": {
            "screenshot": name2,
            "dims": (list(probe_dimensions(p2)) if p2 and os.path.exists(p2)
                     else second.get("page_dims")),
            "sha256": (sha256_file(p2) if p2 and os.path.exists(p2) else None),
            "http_status": second.get("http_status"),
            "captured_at": second.get("captured_at"),
        },
    }
    return nondet


def assess_second_capture(out_dir: str, second: Dict, max_mp: float) -> Tuple[bool, List[str]]:
    """Decide whether a same-host A/B second capture is usable for the
    nondeterminism comparison. Checks IN ORDER: (1) the screenshot exists
    (a memory_limit second capture or a None screenshot is NOT silently
    ignored — it makes the route NO-VERDICT); (2) HTTP status; (3) header
    memory guard; (4) suspicious-capture screen. compute_diff happens only
    if every check passes. Returns (usable, reasons)."""
    if second.get("memory_limit") or not second.get("screenshot"):
        return False, [f"second capture unusable "
                       f"(memory_limit={second.get('memory_limit')}, "
                       f"screenshot={second.get('screenshot')})"]
    status = second.get("http_status")
    if isinstance(status, int) and status >= 400:
        return False, [f"second capture HTTP {status}"]
    p2 = os.path.join(out_dir, second["screenshot"])
    if not os.path.exists(p2):
        return False, [f"second capture file missing on disk: {second['screenshot']}"]
    try:
        memory_guard(p2, max_mp)
    except MemoryLimitError as exc:
        return False, [str(exc)]
    susp, detail = is_suspicious_capture(p2)
    if susp:
        return False, [f"second capture suspicious — {detail}"]
    return True, []


def flag_ab_no_verdict(entry: Dict, host_key: str, reasons: List[str]) -> None:
    """Record a STRUCTURED same-host A/B failure for this host. The state is
    honored by build_report_rows: the route returns NO-VERDICT BEFORE the
    prod/preprod compute_diff — an A/B that failed to establish determinism
    must never be turned into a pixel verdict, and must not live only in a
    free-text error string."""
    entry.setdefault("ab_verdict", {}).setdefault(host_key, []).extend(reasons)


def run_capture(hosts: Dict, pairs: List[RoutePair], out_dir: str,
                only: Optional[List[str]], viewport: Tuple[int, int],
                timeout_ms: int, settle_ms: int, max_mp: float,
                double_capture: bool) -> List[Dict]:
    """Capture every enabled (and selected) pair on both hosts. Per-route
    failures are recorded and the run CONTINUES; the report aggregates them
    into an explicit global failure."""
    from playwright.sync_api import sync_playwright

    wanted = set(only) if only else None
    selected = [p for p in pairs if p.enabled and (wanted is None or p.name in wanted)]
    if not selected:
        raise RouteConfigError(
            "zero route pairs selected (empty routes file or --only filter "
            "matched nothing) — refusing to produce an empty report")

    os.makedirs(out_dir, exist_ok=True)
    token = str(int(time.time()))
    results: List[Dict] = []

    with sync_playwright() as pw:
        browser = pw.chromium.launch(headless=True)
        try:
            context = browser.new_context(
                viewport={"width": viewport[0], "height": viewport[1]},
                device_scale_factor=1)
            for pair in selected:
                slug = slugify(pair.name)
                entry: Dict = {"name": pair.name, "note": pair.note,
                               "expect_login_redirect": pair.expect_login_redirect,
                               "expected_status_both": pair.expected_status_both,
                               "expected_markers": pair.expected_markers,
                               "hosts": {}, "errors": [],
                               "nondeterminism": {}}
                for host_key in ("prod", "preprod"):
                    path = (pair.prod_path if host_key == "prod"
                            else (pair.preprod_path or pair.prod_path))
                    url = hosts[host_key].rstrip("/") + "/" + path.lstrip("/")
                    busted = append_cache_buster(url, token)
                    png = os.path.join(out_dir, f"{slug}_{host_key}.png")
                    try:
                        sidecar = _capture_one(context, busted, png, settle_ms,
                                               timeout_ms, max_mp)
                        sidecar["buster_outcome"] = detect_buster_outcome(
                            busted, sidecar["final_url"])
                        exp_state, exp_details = check_marker_expectations(
                            sidecar["markers"],
                            (pair.expected_markers or {}).get(host_key))
                        sidecar["freshness"] = {
                            # freshness is NEVER claimed: no independent
                            # deployment proof exists client-side
                            "state": "unknown",
                            "marker_expectations": exp_state,
                            "details": exp_details,
                        }
                        if exp_state == "mismatched":
                            entry["errors"].append(
                                f"{host_key}: marker expectations MISMATCHED — "
                                "the served build markers violate the declared "
                                f"expectation ({'; '.join(exp_details)})")
                        if sidecar.get("memory_limit"):
                            entry["errors"].append(
                                f"{host_key}: memory limit {sidecar.get('page_dims')} "
                                f"beyond {max_mp} MPx — screenshot skipped, "
                                "route will NOT be compared")
                        # header-level guard on the written file too
                        if sidecar.get("screenshot"):
                            try:
                                memory_guard(png, max_mp)
                            except MemoryLimitError as exc:
                                sidecar["memory_limit"] = True
                                sidecar["screenshot"] = None
                                entry["errors"].append(f"{host_key}: {exc}")
                            except CorruptImageError as exc:
                                # contained PER ROUTE: an unreadable capture
                                # header must not kill the run — the route is
                                # errored, the manifest and the FOLLOWING
                                # routes are still produced
                                sidecar["screenshot"] = None
                                entry["errors"].append(
                                    f"{host_key}: capture header unreadable "
                                    f"({exc}) — route will NOT be compared")
                        entry["hosts"][host_key] = sidecar
                        if double_capture and sidecar.get("screenshot"):
                            # per-route containment: a failure of the A/B
                            # comparison must not kill the run
                            try:
                                png2 = os.path.join(out_dir, f"{slug}_{host_key}_b.png")
                                second = _capture_one(context, busted, png2, settle_ms,
                                                      timeout_ms, max_mp)
                                usable, ab_reasons = assess_second_capture(
                                    out_dir, second, max_mp)
                                if not usable:
                                    entry["errors"].append(
                                        f"{host_key}: same-host A/B not performed — "
                                        + "; ".join(ab_reasons)
                                        + " — NO-VERDICT")
                                    flag_ab_no_verdict(entry, host_key, ab_reasons)
                                else:
                                    m = compute_diff(png, os.path.join(
                                        out_dir, second["screenshot"]))
                                    nondet = record_nondeterminism(
                                        entry, host_key, m, second, out_dir)
                                    if nondet:
                                        if m.dims_match:
                                            ab_reason = (
                                                f"same-host A/B strong delta "
                                                f"{m.pct_strong:.4f}% > descriptive "
                                                f"ceiling {LOW_DELTA_MAX_STRONG_PCT}%")
                                        else:
                                            ab_reason = (
                                                f"same-host A/B dimension instability "
                                                f"{m.width_a}x{m.height_a} vs "
                                                f"{m.width_b}x{m.height_b} — an "
                                                "identical common area is NOT a "
                                                "stability proof")
                                        entry["errors"].append(
                                            f"{host_key}: {ab_reason} — page is "
                                            "nondeterministic, NO mask applied, "
                                            "NO-VERDICT (mask policy runbook §3)")
                                        flag_ab_no_verdict(entry, host_key,
                                                           [ab_reason])
                            except (VisualDiffError, OSError) as exc:
                                ab_reason = (f"double-capture error "
                                             f"{type(exc).__name__}: {exc}")
                                entry["errors"].append(
                                    f"{host_key}: {ab_reason} — NO-VERDICT")
                                flag_ab_no_verdict(entry, host_key, [ab_reason])
                    except CaptureError as exc:
                        entry["errors"].append(f"{host_key}: CAPTURE ERROR {exc}")
                        entry["hosts"][host_key] = {"requested_url": busted,
                                                    "error": str(exc)}
                    except (VisualDiffError, OSError) as exc:
                        # per-route containment: an unexpected error in the
                        # post-capture bookkeeping (marker expectations,
                        # header guards, I/O...) must not kill the run — the
                        # manifest and the FOLLOWING routes are still produced
                        entry["errors"].append(
                            f"{host_key}: CAPTURE ROUTE ERROR "
                            f"{type(exc).__name__}: {exc}")
                        entry["hosts"].setdefault(
                            host_key, {"requested_url": busted,
                                       "error": str(exc)})
                results.append(entry)
        finally:
            browser.close()
    return results


# ---------------------------------------------------------------------------
# Report (works offline from the manifest — --mode report)
# ---------------------------------------------------------------------------

def _esc(value) -> str:
    return html_mod.escape(str(value), quote=True)


def _resolve_png(out_dir: str, sidecar: Dict) -> Optional[str]:
    name = sidecar.get("screenshot")
    if not name:
        return None
    p = name if os.path.isabs(name) else os.path.join(out_dir, name)
    return p if os.path.exists(p) else None


def verify_fingerprints(entries: List[Dict], out_dir: str,
                        stored: Dict[str, Dict[str, str]]) -> List[Tuple[str, str]]:
    """In report mode, re-hash the PNGs and compare with the fingerprints
    stored at capture time. Returns (route_name, problem) pairs. Every
    capture WITH a screenshot MUST have a stored fingerprint — a missing one
    (including for same-host A/B second captures) is an explicit failure,
    never silently accepted. Per-file hashing errors are contained (they
    become problems, not crashes)."""
    problems: List[Tuple[str, str]] = []
    for entry in entries:
        slug = slugify(entry["name"])
        for hk in ("prod", "preprod"):
            sc = entry.get("hosts", {}).get(hk, {})
            fp = (stored.get(slug, {}) or {}).get(hk)
            p = _resolve_png(out_dir, sc)
            if p is None:
                if fp is not None:
                    problems.append((entry["name"],
                                     f"{hk}: fingerprint recorded but screenshot "
                                     "missing on disk"))
                elif not sc.get("memory_limit") and sc.get("error") is None:
                    problems.append((entry["name"],
                                     f"{hk}: no screenshot and no guard/error flag "
                                     "— inconsistent capture state"))
                continue
            if fp is None:
                problems.append((entry["name"],
                                 f"{hk}: NO stored fingerprint for an existing "
                                 "capture — explicit failure"))
                continue
            try:
                if sha256_file(p) != fp:
                    problems.append((entry["name"], f"{hk}: FINGERPRINT MISMATCH — "
                                                    "file changed since capture"))
            except OSError as exc:
                problems.append((entry["name"], f"{hk}: cannot re-hash ({exc})"))
        # same-host A/B second captures: their fingerprints live in the
        # nondeterminism trace. The disk is scanned for EVERY expected
        # host/capture pair EVEN WHEN nondeterminism IS EMPTY — an orphan
        # {slug}_{host}_b.png without any trace is an explicit failure (an
        # unauditable capture nobody accounted for must never be invisible)
        nondet = entry.get("nondeterminism") or {}
        for hk in ("prod", "preprod"):
            on_disk_path = os.path.join(out_dir, f"{slug}_{hk}_b.png")
            on_disk = os.path.exists(on_disk_path)
            block = nondet.get(hk)
            if block is None:
                if on_disk:
                    problems.append((entry["name"],
                                     f"{hk}: second capture on disk "
                                     f"({slug}_{hk}_b.png) WITHOUT any "
                                     "nondeterminism trace — explicit failure"))
                continue
            second = (block or {}).get("second_capture") or {}
            name2 = second.get("screenshot")
            p2 = os.path.join(out_dir, name2) if name2 else None
            target = p2 if (p2 and os.path.exists(p2)) else (
                on_disk_path if on_disk else None)
            if target is None:
                problems.append((entry["name"],
                                 f"{hk}: second-capture trace refers to a file "
                                 "missing on disk"))
                continue
            sha = second.get("sha256")
            if not sha:
                problems.append((entry["name"],
                                 f"{hk}: second capture on disk WITHOUT recorded "
                                 "fingerprint — explicit failure"))
                continue
            try:
                if sha256_file(target) != sha:
                    problems.append((entry["name"],
                                     f"{hk}: second-capture FINGERPRINT MISMATCH"))
            except OSError as exc:
                problems.append((entry["name"],
                                 f"{hk}: cannot re-hash second capture ({exc})"))
    return problems


def build_report_rows(entries: List[Dict], out_dir: str,
                      max_mp: float = DEFAULT_MAX_MP) -> List[Dict]:
    """For each captured pair: HTTP gate, guards, diff metrics, side-by-side,
    heatmap, verdict, fingerprints. Guards are REVALIDATED against the files
    (header probe, no decode) — the manifest flags are never trusted alone.
    Every comparison or I/O error is caught PER ROUTE and turned into an
    ERROR row — the following routes always run."""
    rows: List[Dict] = []
    for entry in entries:
        slug = slugify(entry["name"])
        row = {"name": entry["name"], "errors": list(entry.get("errors", [])),
               "note": entry.get("note", ""), "metrics": None,
               "fingerprints": {}, "artifacts": {}, "verdict": None,
               "reasons": [], "sidecars": entry.get("hosts", {}),
               "nondeterminism": entry.get("nondeterminism", {}),
               "ab_verdict": entry.get("ab_verdict", {})}
        try:
            prod = entry.get("hosts", {}).get("prod", {})
            pre = entry.get("hosts", {}).get("preprod", {})
            png_prod = _resolve_png(out_dir, prod)
            png_pre = _resolve_png(out_dir, pre)

            # 1. HTTP gate — BEFORE any pixel logic. An error page can never
            #    be compared into an IDENTIQUE verdict.
            gate, gate_reasons = http_gate(prod.get("http_status"),
                                           pre.get("http_status"),
                                           entry.get("expected_status_both"))
            row["reasons"].extend(gate_reasons)
            if gate is not None:
                row["verdict"] = gate
                rows.append(row)
                continue

            # 2. guard flags (memory) — never compare a guarded capture
            if prod.get("memory_limit") or pre.get("memory_limit"):
                row["verdict"] = VERDICT_NO_VERDICT
                row["reasons"].append("memory-limit guard tripped on at least one "
                                      "host — comparison not performed")
                rows.append(row)
                continue

            # 3. both captures present
            if not (png_prod and png_pre):
                row["errors"].append("missing one or both captures")
                row["verdict"] = VERDICT_ERROR
                rows.append(row)
                continue

            # 3b. REVALIDATE dimensions from the file headers before ANY
            #     decode — the manifest flags are not trusted (report mode
            #     may run against a tampered or stale manifest)
            for hk, p in (("prod", png_prod), ("preprod", png_pre)):
                try:
                    memory_guard(p, max_mp)
                except MemoryLimitError as exc:
                    row["verdict"] = VERDICT_NO_VERDICT
                    row["reasons"].append(
                        f"{hk}: header revalidation tripped the memory guard "
                        f"({exc}) — comparison not performed")
                    rows.append(row)
                    break
            if row["verdict"] is not None:
                continue

            # 3c. STRUCTURED same-host A/B state: an A/B that could not
            #     be performed (or proved the page nondeterministic) voids
            #     the pixel verdict — return BEFORE the prod/preprod
            #     compute_diff
            ab = entry.get("ab_verdict") or {}
            if ab:
                row["reasons"].append(
                    "same-host A/B unusable/nondeterministic on "
                    + ", ".join(f"{hk} ({'; '.join(rs)})"
                                for hk, rs in sorted(ab.items()))
                    + " — determinism not established, pixel verdict withheld")
                row["verdict"] = VERDICT_NO_VERDICT
                rows.append(row)
                continue

            # 4. suspicious captures (uniform / tiny) — flagged, not compared
            for hk, p in (("prod", png_prod), ("preprod", png_pre)):
                susp, detail = is_suspicious_capture(p)
                if susp:
                    row["errors"].append(f"{hk}: SUSPICIOUS CAPTURE — {detail}")
            if row["errors"] != list(entry.get("errors", [])):
                row["verdict"] = VERDICT_ERROR
                rows.append(row)
                continue

            # 5. fingerprint + compare
            metrics = compute_diff(png_prod, png_pre)
            row["metrics"] = asdict(metrics)
            row["fingerprints"] = {"prod": sha256_file(png_prod),
                                   "preprod": sha256_file(png_pre)}
            sbs = os.path.join(out_dir, f"{slug}_sbs.png")
            hm = os.path.join(out_dir, f"{slug}_heat.png")
            render_side_by_side(png_prod, png_pre,
                                f"PROD {prod.get('final_url', '')[:70]}",
                                f"PREPROD {pre.get('final_url', '')[:70]}", sbs)
            render_heatmap(png_prod, png_pre, hm)
            row["artifacts"] = {"side_by_side": os.path.basename(sbs),
                                "heatmap": os.path.basename(hm),
                                # links to the RAW captures (never downscaled):
                                # the montage is a reduced display copy only
                                "raw_prod": os.path.basename(png_prod),
                                "raw_preprod": os.path.basename(png_pre)}
            kd = classify_known_deltas(prod.get("markers", {}),
                                       pre.get("markers", {}))
            verdict, reasons = classify_pair(metrics, kd, row["errors"])
            row["verdict"], row["reasons"] = verdict, row["reasons"] + reasons
        except (VisualDiffError, OSError) as exc:
            # per-route containment: comparison I/O errors must not stop the
            # following routes
            row["errors"].append(f"COMPARISON ERROR: {type(exc).__name__}: {exc}")
            row["verdict"] = VERDICT_ERROR
        rows.append(row)
    return rows


def render_html_report(rows: List[Dict], meta: Dict, inverse: Dict,
                       out_path: str) -> str:
    """Local HTML report. Every dynamic string goes through _esc
    (html.escape) — tested with hostile labels. The aesthetic banner is
    mandatory and unconditional."""
    parts: List[str] = []
    parts.append("<!doctype html><html lang='fr'><head><meta charset='utf-8'>")
    parts.append("<title>dnn-visual-diff report</title><style>")
    parts.append("body{font-family:system-ui,sans-serif;margin:16px;background:#fafafa;color:#111}"
                 "table{border-collapse:collapse;width:100%;margin:12px 0}"
                 "td,th{border:1px solid #bbb;padding:6px 8px;vertical-align:top;font-size:14px}"
                 "img{max-width:100%;height:auto}"
                 ".banner{background:#fff3cd;border:2px solid #b8860b;padding:10px;font-weight:600}"
                 ".err{background:#fdd}.ok{background:#dfd}.warn{background:#ffe9b3}"
                 "code{font-size:12px;word-break:break-all}")
    parts.append("</style></head><body>")
    parts.append(f"<h1>dnn-visual-diff — rapport local — {_esc(meta.get('generated_at', ''))}</h1>")
    src = meta.get("source") or {}
    parts.append(f"<div class='banner'>{_esc(AESTHETIC_BANNER)}</div>")
    parts.append(f"<p>Viewports identiques {_esc(meta.get('viewport'))} — captures fullPage — "
                 f"aucun masque — aucun redimensionnement pour le verdict — "
                 f"limite mémoire CAPTURE {_esc(meta.get('max_mp'))} MPx "
                 f"(limite fixée à la capture ; le report applique "
                 f"min(--max-mp opérateur, manifest)) — "
                 f"seuil {_esc(LOW_DELTA_MAX_STRONG_PCT)}% DESCRIPTIF (aucune causalité affirmée). "
                 f"Source: <code>{_esc(src.get('tool', ''))}</code> @ <code>"
                 f"{_esc(src.get('git_rev', ''))}</code></p>")
    inv_cls = "ok" if inverse.get("ok") else "err"
    parts.append(f"<h2>Contrôle inverse (obligatoire)</h2>"
                 f"<p class='{inv_cls}'>{_esc(inverse.get('detail', ''))}</p>")
    if not inverse.get("ok"):
        parts.append("<p class='err'><b>INSTRUMENT-ERROR: tous les verdicts de ce run sont "
                     "non avenus (moteur aveugle prouvé par le contrôle inverse).</b></p>")
    parts.append("<h2>Verdicts par route</h2><table><tr><th>route</th><th>verdict</th>"
                 "<th>% brut / % fort (≥16/255)</th><th>HTTP · fraîcheur · marqueurs</th>"
                 "<th>raisons / erreurs</th><th>artefacts</th></tr>")
    for row in rows:
        cls = ("err" if row["verdict"] in FAILING_VERDICTS
               else "warn" if row["verdict"] in (VERDICT_DIFF, VERDICT_DIM_GAP) else "ok")
        m = row.get("metrics") or {}
        met_txt = (_esc(m.get("pct_raw")) + " / " + _esc(m.get("pct_strong"))
                   + ("" if m.get("dims_match", True)
                      else f" — DIMS {m.get('width_a')}x{m.get('height_a')} vs "
                           f"{m.get('width_b')}x{m.get('height_b')}"))
        cells = []
        for hk in ("prod", "preprod"):
            sc = row.get("sidecars", {}).get(hk, {})
            fr = sc.get("freshness", {})
            mk = sc.get("markers", {})
            cells.append(
                f"{hk}: HTTP {_esc(sc.get('http_status'))} · "
                f"final {_esc(sc.get('final_url', '?')[:90])} · "
                f"buster {_esc(sc.get('buster_outcome', '?'))} · "
                f"fraîcheur {_esc(fr.get('state', 'unknown'))} (aucune preuve de "
                f"déploiement indépendante) · attentes marqueurs: "
                f"{_esc(fr.get('marker_expectations', 'n/a'))}"
                + (f" [{'; '.join(_esc(d) for d in fr.get('details', [])[:3])}]"
                   if fr.get("details") else "")
                + f" · à {_esc(sc.get('captured_at', '?'))}"
                + f" · tosic.sxc/sexy {_esc(mk.get('tosic_sxc'))}/"
                  f"{_esc(mk.get('tosic_sexycontent'))} · cdv {_esc(mk.get('cdv'))}"
                + (f" · CONSOLE ERR {_esc(sc.get('console_error_count'))}"
                   if sc.get("console_error_count") else ""))
        reasons = "<br>".join(_esc(r) for r in (row.get("reasons") or []) + (row.get("errors") or []))
        arts = ""
        if row.get("artifacts"):
            a = row["artifacts"]
            arts = (f"<a href='{_esc(a['side_by_side'])}'>côte à côte</a> · "
                    f"<a href='{_esc(a['heatmap'])}'>heatmap</a> · "
                    f"PNG bruts: <a href='{_esc(a.get('raw_prod', ''))}'>prod</a> · "
                    f"<a href='{_esc(a.get('raw_preprod', ''))}'>préprod</a> "
                    "(le montage est une copie d'affichage réduite)")
        fps = row.get("fingerprints") or {}
        fp_txt = "<br>".join(f"{_esc(k)}: <code>{_esc(v[:16])}…</code>" for k, v in fps.items())
        parts.append(
            f"<tr class='{cls}'><td>{_esc(row['name'])}<br><small>{_esc(row.get('note',''))}</small></td>"
            f"<td><b>{_esc(row['verdict'])}</b></td><td>{met_txt}</td>"
            f"<td><small>{'<br>'.join(cells)}</small></td>"
            f"<td><small>{reasons}</small><br><small>{fp_txt}</small></td>"
            f"<td>{arts}</td></tr>")
    parts.append("</table>")
    parts.append("<h2>Légende</h2><ul>"
                 "<li><b>IDENTIQUE-SOUS-SEUIL</b>: écart fort sous le seuil DESCRIPTIF — "
                 "l'outil n'établit pas la cause du résidu."
                 "<li><b>DELTA-CONNU</b>: écarts du registre D1–D7 (runbook §5)."
                 "<li><b>DIFF-A-CLASSER</b>: écart fort supérieur au seuil descriptif — y "
                 "compris AVEC des deltas connus (D1–D7 ne couvrent pas cet écart) → à revérifier."
                 "<li><b>ECART-DIMENSIONS</b>: dimensions différentes — écart explicite, "
                 "jamais rééchantillonné."
                 "<li><b>HTTP-ERROR / ATTENDU-HTTP</b>: erreur HTTP (mêmes déclarée attendue) — "
                 "aucune comparaison pixel, échec global."
                 "<li><b>ERROR / NO-VERDICT</b>: capture impossible/suspecte, attentes de marqueurs non satisfaites, "
                 "nondéterminisme même-hôte, empreinte modifiée ou limite mémoire."
                 "</ul>")
    parts.append("<p><small>Rapport local — ne publier que le résumé (dashboard / issue), "
                 "pas ce fichier (convention runbook §6).</small></p>")
    parts.append("</body></html>")
    with open(out_path, "w", encoding="utf-8") as fh:
        fh.write("\n".join(parts))
    return out_path


def aggregate(rows: List[Dict], inverse_ok: bool) -> Tuple[bool, Dict]:
    """A run is clean only if: inverse control OK, at least one route was
    produced (an empty row set is a FAILURE, not a success), and no route is
    in a failing verdict. DIFF-A-CLASSER / ECART-DIMENSIONS are results."""
    counts: Dict[str, int] = {}
    for row in rows:
        counts[row["verdict"]] = counts.get(row["verdict"], 0) + 1
    clean = (inverse_ok
             and bool(rows)
             and not any(counts.get(v, 0) for v in FAILING_VERDICTS))
    return clean, counts


# ---------------------------------------------------------------------------
# CLI
# ---------------------------------------------------------------------------

def _load_manifest(out_dir: str) -> Dict:
    path = os.path.join(out_dir, "manifest.json")
    if not os.path.exists(path):
        raise VisualDiffError(
            f"no manifest.json in {out_dir!r} — run --mode capture first")
    with open(path, "r", encoding="utf-8") as fh:
        return json.load(fh)


def validate_params(args: argparse.Namespace) -> Tuple[int, int]:
    """Validate operator parameters up front: viewport = two strictly positive
    ints, max_mp > 0, timeout > 0, settle >= 0. Bad parameters are explicit
    errors, never silent defaults."""
    try:
        w, h = (int(x) for x in str(args.viewport).lower().split("x"))
    except ValueError:
        raise VisualDiffError(f"viewport must be WxH, got {args.viewport!r}")
    if w <= 0 or h <= 0:
        raise VisualDiffError(f"viewport dimensions must be > 0, got {w}x{h}")
    if not (args.max_mp > 0):
        raise VisualDiffError(f"max-mp must be > 0, got {args.max_mp}")
    if args.timeout <= 0:
        raise VisualDiffError(f"timeout must be > 0, got {args.timeout}")
    if args.settle < 0:
        raise VisualDiffError(f"settle must be >= 0, got {args.settle}")
    return w, h


def run(args: argparse.Namespace) -> int:
    out_dir = os.path.abspath(args.out)
    os.makedirs(out_dir, exist_ok=True)

    if args.self_test:
        return self_test()

    viewport = validate_params(args)

    if args.mode in ("full", "capture"):
        # Run preservation: a previous manifest is NEVER overwritten — each
        # run must use a fresh --out directory (captures and manifests of
        # earlier runs, even failing ones, stay auditable).
        if os.path.exists(os.path.join(out_dir, "manifest.json")):
            raise VisualDiffError(
                f"manifest.json already exists in {out_dir!r} — previous run "
                "preserved; use a new --out directory")
        hosts, pairs = load_routes(args.routes)
        entries = run_capture(hosts, pairs, out_dir, args.only, viewport,
                              args.timeout * 1000, args.settle, args.max_mp,
                              args.double_capture)
        meta = {"generated_at": now_iso(), "viewport": args.viewport,
                "max_mp": args.max_mp, "double_capture": args.double_capture,
                "hosts": load_routes(args.routes)[0], "source": source_ref()}
        # fingerprints taken at capture time (report mode re-verifies them)
        stored_fps: Dict[str, Dict[str, str]] = {}
        for entry in entries:
            slug = slugify(entry["name"])
            for hk in ("prod", "preprod"):
                p = _resolve_png(out_dir, entry["hosts"].get(hk, {}))
                if p:
                    try:
                        stored_fps.setdefault(slug, {})[hk] = sha256_file(p)
                    except OSError as exc:
                        # contained PER ROUTE (e.g. PermissionError): the
                        # manifest is still written; the route carries an
                        # explicit error and report mode flags the missing
                        # fingerprint as a verification failure
                        entry.setdefault("errors", []).append(
                            f"{hk}: fingerprint could not be collected "
                            f"({type(exc).__name__}: {exc}) — stored "
                            "fingerprint missing, route will FAIL verification")
        with open(os.path.join(out_dir, "manifest.json"), "w", encoding="utf-8") as fh:
            json.dump({"meta": meta, "entries": entries,
                       "stored_fingerprints": stored_fps},
                      fh, ensure_ascii=False, indent=2)
        if args.mode == "capture":
            print(f"capture done: {len(entries)} entries -> "
                  f"{os.path.join(out_dir, 'manifest.json')}")
            return 0

    # report / full: offline rebuild from the manifest
    manifest = _load_manifest(out_dir)
    meta = manifest["meta"]
    entries = manifest["entries"]
    if not entries:
        print("GLOBAL FAILURE: manifest has zero entries", file=sys.stderr)
        return 1
    ic_ok, ic_val, ic_detail = inverse_control(out_dir)
    print(ic_detail)
    for route_name, problem in verify_fingerprints(entries, out_dir,
                                                   manifest.get("stored_fingerprints", {})):
        target = next((e for e in entries if e["name"] == route_name), None)
        if target is not None:
            target.setdefault("errors", []).append(f"FINGERPRINT: {problem}")
        else:
            entries[0].setdefault("errors", []).append(
                f"FINGERPRINT (unattached): {route_name}: {problem}")
    manifest_max_mp = float(meta.get("max_mp", DEFAULT_MAX_MP))
    # the OPERATOR limit (--max-mp) always applies; when stricter than the
    # capture-time guard recorded in the manifest it WINS (min of both)
    op_max_mp = float(args.max_mp)
    max_mp = min(op_max_mp, manifest_max_mp)
    if op_max_mp < manifest_max_mp:
        print(f"note: operator --max-mp {op_max_mp} stricter than the "
              f"capture-time guard {manifest_max_mp} — using {max_mp}")
    rows = build_report_rows(entries, out_dir, max_mp=max_mp)
    if not ic_ok:
        for row in rows:
            if row["verdict"] not in FAILING_VERDICTS:
                row["verdict"] = VERDICT_NO_VERDICT
                row["reasons"].append("voided by INSTRUMENT-ERROR (inverse control failed)")
    manifest["rows"] = rows
    manifest["inverse_control"] = {"ok": ic_ok, "strong_pct": ic_val,
                                   "detail": ic_detail}
    with open(os.path.join(out_dir, "manifest.json"), "w", encoding="utf-8") as fh:
        json.dump(manifest, fh, ensure_ascii=False, indent=2)
    report_path = render_html_report(rows, meta, {"ok": ic_ok, "detail": ic_detail},
                                     os.path.join(out_dir, "report.html"))
    clean, counts = aggregate(rows, ic_ok)
    print(f"routes: {counts} -> report: {report_path} "
          f"manifest: {os.path.join(out_dir, 'manifest.json')}")
    print("GLOBAL:", "OK (every route has a comparable verdict)"
          if clean else "FAILURE (errors / no-verdict / http-error / instrument) — see report")
    return 0 if clean else 1


# ---------------------------------------------------------------------------
# Offline self-test (also runnable without playwright installed)
# ---------------------------------------------------------------------------

def self_test() -> int:
    import tempfile
    ok_all = True

    def check(name: str, cond: bool, detail: str = ""):
        nonlocal ok_all
        print(("PASS " if cond else "FAIL ") + name
              + (f" — {detail}" if detail and not cond else ""))
        ok_all = ok_all and cond

    with tempfile.TemporaryDirectory() as tmp:
        # 1. identical pair -> zero diff
        img = Image.new("RGB", (64, 64), (100, 100, 100))
        pa, pb = os.path.join(tmp, "a.png"), os.path.join(tmp, "b.png")
        img.save(pa); img.save(pb)
        m = compute_diff(pa, pb)
        check("identical->0%", m.pct_raw == 0 and m.pct_strong == 0 and m.dims_match)
        # 2. low-amplitude noise (+-1) -> raw ~50%, strong 0
        noisy = Image.new("RGB", (64, 64))
        noisy.putdata([(100, 100, 100) if (x + y) % 2 == 0 else (101, 101, 101)
                       for y in range(64) for x in range(64)])
        pn = os.path.join(tmp, "n.png")
        noisy.save(pn)
        m = compute_diff(pa, pn)
        check("noise->raw/strong split", 49 <= m.pct_raw <= 51 and m.pct_strong == 0)
        # 3. real difference -> strong high
        block = img.copy()
        ImageDraw.Draw(block).rectangle([8, 8, 24, 24], fill=(0, 0, 0))
        pbl = os.path.join(tmp, "blk.png")
        block.save(pbl)
        m = compute_diff(pa, pbl)
        check("real diff seen", m.pct_strong >= 5, f"strong={m.pct_strong}")
        # 4. dimension mismatch explicit; verdict ECART-DIMENSIONS
        ps = os.path.join(tmp, "s.png")
        Image.new("RGB", (64, 32), (100, 100, 100)).save(ps)
        m = compute_diff(pa, ps)
        v, _ = classify_pair(m, [], [])
        check("dim mismatch explicit", not m.dims_match and v == VERDICT_DIM_GAP)
        # 5. error detectors (empty, corrupt, missing)
        empty = os.path.join(tmp, "e.png")
        open(empty, "wb").close()
        try:
            compute_diff(empty, empty); check("empty capture detected", False)
        except EmptyCaptureError:
            check("empty capture detected", True)
        corrupt = os.path.join(tmp, "c.png")
        with open(corrupt, "wb") as fh:
            fh.write(b"\x89PNG\r\n\x1a\nNOTAPNG")
        try:
            compute_diff(corrupt, corrupt); check("corrupt image detected", False)
        except CorruptImageError:
            check("corrupt image detected", True)
        # 6. inverse control on healthy engine
        ok, val, _ = inverse_control(tmp)
        check("inverse control healthy", ok, f"strong={val}")
        # 7. heatmap + side-by-side artifacts
        hm = render_heatmap(pa, pbl, os.path.join(tmp, "h.png"))
        sbs = render_side_by_side(pa, pbl, "PROD <x>", "PREPROD", os.path.join(tmp, "sbs.png"))
        check("artifacts written", os.path.getsize(hm) > 0 and os.path.getsize(sbs) > 0)
        # 8. html escaping in report
        hostile = "<script>alert(1)</script> & 'quote'"
        rows = [{"name": hostile, "verdict": VERDICT_DIFF, "errors": [hostile],
                 "reasons": [hostile], "note": hostile, "metrics": None,
                 "fingerprints": {"prod": "0" * 64}, "artifacts": {},
                 "sidecars": {}, "nondeterminism": {}}]
        rp = render_html_report(rows, {"viewport": "1440x900"},
                                {"ok": True, "detail": hostile}, os.path.join(tmp, "r.html"))
        with open(rp, encoding="utf-8") as fh:
            content = fh.read()
        check("report escapes html", "<script>alert" not in content and "&lt;script&gt;" in content)
        # 9. zero-pairs detection
        try:
            load_routes(os.path.join(tmp, "missing.json")); check("zero/missing routes detected", False)
        except RouteConfigError:
            check("zero/missing routes detected", True)
        # 10. memory guard trips on header BEFORE decode
        big = os.path.join(tmp, "big.png")
        Image.new("RGB", (4200, 4200), (0, 0, 0)).save(big)
        try:
            memory_guard(big, 10.0); check("memory guard trips", False)
        except MemoryLimitError:
            check("memory guard trips", True)
        # 11. buster outcomes
        check("buster kept", detect_buster_outcome("/x?cb=1", "/x?cb=1") == "kept-query")
        check("buster path rewrite", detect_buster_outcome("/?cb=1", "/Argumentum/cb/1") == "rewritten-to-path")
        check("buster lost", detect_buster_outcome("/?cb=1", "/y") == "lost")
        # 12. markers, marker-expectation semantics, known deltas
        prod_html = "tosic_sexycontent jquery-3.5.1.min.js jquery-migrate.min.js GTM-TZBQ57M googletagmanager.com/ns.html ?cdv=181 http://db.onlinewebfonts.com/t/x.eot"
        pre_html = "tosic.sxc tosic.sxc jquery-3.7.1.min.js GTM-TZBQ57M ?cdv=325"
        mp_, mq = parse_freshness_markers(prod_html), parse_freshness_markers(pre_html)
        check("markers present", markers_present(mp_) and markers_present(mq))
        st, _ = check_marker_expectations(mp_, None)
        check("no expectation -> none-declared", st == "none-declared")
        st, _ = check_marker_expectations(mp_, {})
        check("EMPTY expectation dict -> none-declared (declares nothing)",
              st == "none-declared")
        st, _ = check_marker_expectations(mp_, {"tosic_sexycontent>": "0"})
        check("expectation satisfied -> matched (freshness stays unknown)",
              st == "matched")
        st, det = check_marker_expectations(mq, {"tosic_sexycontent>": "0"})
        check("expectation violated -> mismatched", st == "mismatched")
        kd = classify_known_deltas(mp_, mq)
        ids = [k.split(":")[0] for k in kd]
        check("known deltas D1,D2,D4,D5,D6,D7",
              all(d in ids for d in ("D1", "D2", "D4", "D5", "D6", "D7")), str(ids))
        # 13. HTTP gate: error page can never be IDENTIQUE
        g, _ = http_gate(500, 500, None)
        check("http 500 -> HTTP-ERROR", g == VERDICT_HTTP_ERROR)
        g, _ = http_gate(404, 404, 404)
        check("expected 404 -> ATTENDU-HTTP", g == VERDICT_EXPECTED_HTTP)
        g, _ = http_gate(200, 500, None)
        check("asymmetry -> HTTP-ERROR", g == VERDICT_HTTP_ERROR)
        g, _ = http_gate(200, 200, None)
        check("200/200 -> no gate", g is None)
        # 14. aggregate semantics
        check("aggregate([]) fails", aggregate([], True)[0] is False)
        clean, _ = aggregate([{"verdict": VERDICT_IDENTICAL}], True)
        check("healthy rows pass", clean)
        clean, _ = aggregate([{"verdict": VERDICT_EXPECTED_HTTP}], True)
        check("ATTENDU-HTTP still global failure", clean is False)
        # 15. suspicious captures
        uni = os.path.join(tmp, "uni.png")
        Image.new("RGB", (200, 200), (255, 255, 255)).save(uni)
        susp, _ = is_suspicious_capture(uni)
        check("uniform capture flagged", susp)
        grad = Image.new("RGB", (64, 64))
        grad.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        pgrad = os.path.join(tmp, "grad.png")
        grad.save(pgrad)
        susp, _ = is_suspicious_capture(pgrad)
        check("textured capture not flagged", not susp)
        # 16. guarded capture never compared (build_report_rows)
        entries = [{"name": "x", "note": "", "errors": [],
                    "expect_login_redirect": False, "expected_status_both": None,
                    "expected_markers": None,
                    "hosts": {"prod": {"http_status": 200, "memory_limit": False,
                                       "screenshot": "a.png"},
                              "preprod": {"http_status": 200, "memory_limit": True,
                                          "screenshot": None}},
                    "nondeterminism": {}}]
        rows = build_report_rows(entries, tmp)
        check("memory-limited route -> NO-VERDICT, not compared",
              rows[0]["verdict"] == VERDICT_NO_VERDICT and rows[0]["metrics"] is None)
        # 17. per-route I/O containment: one bad PNG does not stop the next route
        g1 = os.path.join(tmp, "g1.png")
        g2 = os.path.join(tmp, "g2.png")
        grad2 = Image.new("RGB", (64, 64))
        grad2.putdata([(x * 4 % 256, y * 4 % 256, 7) for y in range(64) for x in range(64)])
        grad2.save(g1); grad2.save(g2)
        entries2 = [
            {"name": "bad", "note": "", "errors": [],
             "hosts": {"prod": {"http_status": 200, "memory_limit": False,
                                "screenshot": "ghost.png"},
                       "preprod": {"http_status": 200, "memory_limit": False,
                                   "screenshot": "ghost2.png"}},
             "nondeterminism": {}},
            {"name": "good", "note": "", "errors": [],
             "hosts": {"prod": {"http_status": 200, "memory_limit": False,
                                "screenshot": os.path.basename(g1)},
                       "preprod": {"http_status": 200, "memory_limit": False,
                                   "screenshot": os.path.basename(g2)}},
             "nondeterminism": {}}]
        rows = build_report_rows(entries2, tmp)
        check("I/O failure contained per route",
              rows[0]["verdict"] == VERDICT_ERROR and rows[1]["verdict"] == VERDICT_IDENTICAL)
    print("SELF-TEST:", "PASS" if ok_all else "FAIL")
    return 0 if ok_all else 1


def main(argv: Optional[List[str]] = None) -> int:
    ap = argparse.ArgumentParser(
        description="Visual diff prod <-> preprod DNN (read-only, client-side). "
                    "Runbook: docs/dnn/visual-diff-runbook.md")
    ap.add_argument("--mode", choices=("full", "capture", "report"), default="full",
                    help="full=capture+report; capture=capture only; "
                         "report=offline rebuild from an existing manifest "
                         "(fingerprints re-verified)")
    ap.add_argument("--routes", default=os.path.join(
        os.path.dirname(os.path.abspath(__file__)), "routes.json"),
        help="routes.json (pairs + hosts)")
    ap.add_argument("--out", default="dnn-visual-diff-out", help="output directory")
    ap.add_argument("--only", nargs="*", default=None,
                    help="restrict to route names (else all enabled pairs)")
    ap.add_argument("--viewport", default=f"{DEFAULT_VIEWPORT[0]}x{DEFAULT_VIEWPORT[1]}",
                    help="viewport WxH, identical for both hosts (default 1440x900, E2)")
    ap.add_argument("--timeout", type=int, default=DEFAULT_TIMEOUT_MS // 1000,
                    help="navigation timeout seconds per host")
    ap.add_argument("--settle", type=int, default=DEFAULT_SETTLE_MS,
                    help="settle ms after networkidle before screenshot")
    ap.add_argument("--max-mp", type=float, default=DEFAULT_MAX_MP,
                    help="per-capture megapixel guard, probed BEFORE the "
                         "screenshot (NO-VERDICT beyond, never a downscale)")
    ap.add_argument("--double-capture", action="store_true",
                    help="same-host A/B per host to detect nondeterminism (traced; "
                         "produces NO-VERDICT, never a masked PASS)")
    ap.add_argument("--self-test", action="store_true", help="offline self-tests, no network")
    args = ap.parse_args(argv)
    try:
        return run(args)
    except (RouteConfigError, VisualDiffError) as exc:
        print(f"GLOBAL FAILURE: {type(exc).__name__}: {exc}", file=sys.stderr)
        return 1


if __name__ == "__main__":
    sys.exit(main())
