#!/usr/bin/env python3
"""Contrôles du jour J (#1066 / #2132 / #1502 / #457 / #1049) — lecture seule.

Un seul point d'entrée pour les 4 contrôles que la bascule doit passer, et pour la
**ligne de base** qu'on mesure sur la préprod avant le jour J : sans ligne de base,
un « 0 » le jour J ne distingue pas « propre » d'« instrument aveugle ».

    ROUTES     8 cultures : accueil, /terms, /privacy, /Règles  -> 200 + <html lang>
    LEGAL-EN   /en/terms + /en/privacy : porte « ARGUMENTUM SAS », ET NE PORTE PAS
               « AGREEMENT BETWEEN USER AND » (gabarit DNN) — les DEUX sens, toujours
    RULES-D1   compteurs de joueurs servis par langue : formes d'intervalle et
               min==max (les 16 formes du commutateur D1 des deux gabarits
               RulesExplorer), nombres lus, et repérage du 3->10 (#1502)
    EVENTLOG   journal Application : FileLoadException / « Could not load file or
               assembly » / exceptions 2sxc sur la fenêtre demandée (#1049/#2122)
               ⚠️ le journal lu est celui de la MACHINE LOCALE ($env:COMPUTERNAME), et
               le script nomme cette machine dans sa sortie. Le jour J il doit tourner
               SUR le VPS de la prod : lancé depuis un autre siège, un
               « 0 FileLoadException » décrit la mauvaise machine — vert à tort.
    WEBCONFIG  le web.config de la CIBLE (fichier local, `--webconfig`) : quatre comptes
               qui séparent le fichier vivant du fichier versionné piégé (#1049), plus
               la taille. ⛔ Comptes seulement : le fichier porte la chaîne de connexion
               et les machineKey — aucune ligne n'est jamais affichée.
               ⚠️ Le contrôle de référence du runbook s'écrit en `grep -c`, mais sur le
               VPS de la prod `grep` n'est PAS sur le PATH de PowerShell et `bash` y est
               WSL sans distribution : d'où ce contrôle **dans le script**, où le shell
               ne décide plus de rien.

⛔ Lecture seule : uniquement des GET et des lectures de journal. Aucune écriture,
aucun geste IIS, aucune sonde de connexion.

Instruments (payés en flotte, ne pas les retirer) :
  · cache-buster `?cb=` sur chaque sonde (le cache prod dure un an) ;
  · `-L` systématique — sans lui, une page redirigeante rend un corps vide qu'on lit
    à tort comme « absent » ;
  · entités HTML **décodées** avant tout comptage (`&agrave;` compte sinon 0) ;
  · espaces insécables normalisés (`\\xa0`, `\\u202f`) — les gabarits en posent, et
    un motif littéral les rate ;
  · témoin par contrôle : une forme dont on SAIT qu'elle est présente (contre-preuve
    que l'instrument voit le 1) et une forme dont on sait qu'elle est absente.

Usage :
    python tools/1105-cutover-day-controls.py --label preprod-baseline
    python tools/1105-cutover-day-controls.py --base-url https://www.argumentum.games \\
        --label prod-jour-j --out docs/quality/controles-jour-j-prod-2026-11-05.md
    python tools/1105-cutover-day-controls.py --webconfig "<copie>\\web.config" \\
        --skip-web --skip-eventlog --label jour-j-webconfig   # le contrôle #1049, seul

Sortie : tableau markdown sur stdout (+ fichier avec --out) ; code de sortie 1 si un
contrôle DUR échoue (route non 200, legal EN inversé, FileLoadException présente,
web.config de la cible qui ne porte pas les marqueurs du fichier vivant).
"""

from __future__ import annotations

import argparse
import datetime as _dt
import html
import json
import re
import subprocess
import sys

# ── Les 8 cultures servies, et le <html lang> attendu ────────────────────────────────
CULTURES = {
    "fr": ("", "fr-FR"),
    "en": ("en/", "en-US"),
    "es": ("es/", "es-ES"),
    "pt": ("pt/", "pt-BR"),
    "ru": ("ru/", "ru-RU"),
    "zh": ("zh/", "zh-CN"),
    "ar": ("ar/", "ar-SA"),
    "fa": ("fa/", "fa-IR"),
}

# ── Commutateur D1 : les 16 formes des DEUX gabarits RulesExplorer ───────────────────
# Source unique : DNNPlatform/Portals/1/2sxc/Argumentum/_RulesExplorer_RuleDetail.cshtml
# (et _RuleList.cshtml, à l'identique — la garde RulesExplorerD1PlayerCountGuardTests
# prouve la synchronisation). Recopier ici est un choix assumé : ce script mesure le
# SERVI, pas le gabarit ; si une forme change, la garde rougit avant ce script.
INTERVAL_FORMS = {
    "en": "{0} to {1} players",
    "es": "de {0} a {1} jugadores",
    "pt": "de {0} a {1} jogadores",
    "ru": "от {0} до {1} игроков",
    "zh": "{0}至{1}名玩家",
    "ar": "من {0} إلى {1} لاعبين",
    "fa": "از {0} تا {1} بازیکن",
    "fr": "de {0} à {1} joueurs",
}
SINGLE_FORMS = {
    "en": "{0} players",
    "es": "{0} jugadores",
    "pt": "{0} jogadores",
    "ru": "{0} игрока",
    "zh": "{0}名玩家",
    "ar": "{0} لاعبين",
    "fa": "{0} بازیکن",
    "fr": "{0} joueurs",
}
# Chiffres acceptés : ASCII et persans/arabes (fa convertit, les 7 autres non).
DIGITS = "0-9۰-۹٠-٩"

# Marqueurs calibrés au 2026-11-05 sur les valeurs du DÉPÔT (master) — un marqueur
# qu'aucune valeur saine ne porte est un contrôle qui ne peut pas passer.
#
#   MESSAGE_PORTAL_TERMS.Text    neutre : « ARGUMENTUM SAS » ×1, SIREN oui
#   MESSAGE_PORTAL_PRIVACY.Text  neutre : « ARGUMENTUM SAS » ×0, SIREN non  ← mesuré
#
# ⚠️ Le texte parlait d'« ARGUMENTUM SAS » sur les DEUX pages : vrai pour TERMS,
# **faux pour PRIVACY** (0 occurrence dans le neutre comme dans le fr-FR). La
# politique de confidentialité ne nomme pas la société ; son marqueur propre est la
# phrase de primauté du français, présente dans les deux valeurs du dépôt.
#   Négatif commun : « AGREEMENT BETWEEN USER AND » = le texte STOCK de DNN, que
#   l'installeur écrit quand il écrase le neutre.
LEGAL_PAGES = {
    "terms": ("ARGUMENTUM SAS",),
    "privacy": ("The French version of this page is the authoritative version",),
}
LEGAL_DNN_TEMPLATE = "AGREEMENT BETWEEN USER AND"

EVENT_MARKERS = {
    "FileLoadException": r"FileLoadException",
    "Could not load file or assembly": r"Could not load file or assembly",
    "2sxc": r"ToSic|2sxc",
}

# ── web.config de la cible : ce qui sépare le fichier VIVANT du VERSIONNÉ (#1049) ────
# Mesuré le 05/11/2026 sur les deux mondes, avec le même instrument :
#   fichier vivant    (C:\Production\Argumentum\wwwroot\web.config, 83 211 o) → 0·0·1·1
#   fichier versionné (DNNPlatform/web.config d'avant 167fbd33, 79 286 o)     → 1·1·0·0
#
# ⚠️ Le runbook portait « ~176 `dependentAssembly` (pas 2) ». Mesuré : 181 lignes et 2
# balises nues dans les DEUX fichiers — un marqueur égal dans les deux mondes ne mesure
# rien, et il donnait un feu vert sur la panne garantie. Ce qui discrimine, ce sont les
# VALEURS de remplacement, pas un décompte de balises.
WEBCONFIG_MARKERS = (
    ("Data Source=REPLACE", 0, "chaîne de connexion factice ⇒ fichier versionné"),
    ('validationKey="REPLACE"', 0, "machineKey factice ⇒ fichier versionné"),
    ("Initial Catalog=ArgumentumGames", 1, "la base de la prod est nommée"),
    ('fileExtension=".owl"', 1, "mimeMap qui sert l'OWL en application/xml (#133)"),
    # Filet le plus large : n'importe quel placeholder survivant, toutes clés confondues.
    ("REPLACE", 0, "aucune valeur de remplacement, quelle que soit la clé"),
)
WEBCONFIG_EXPECTED_SIZE = 83_211


def _count_lines(text: str, pattern: str) -> int:
    """Lignes portant `pattern` — la sémantique de `grep -c`, celle que le runbook
    prescrit. Les marqueurs ci-dessus vivent chacun sur leur propre ligne dans le
    fichier mesuré, donc lignes et occurrences y coïncident."""
    return sum(1 for line in text.splitlines() if pattern in line)


def control_webconfig(path: str, expected_size: int = WEBCONFIG_EXPECTED_SIZE) -> list[dict]:
    """Le web.config de la CIBLE porte-t-il les marqueurs du fichier VIVANT ?

    Lecture seule et locale : c'est le seul contrôle du script qui ne sonde pas le
    réseau, et le seul qui doive tourner **avant que le site soit servi** — dernier
    point où reculer ne coûte rien.

    ⛔ Aucune ligne du fichier n'est jamais affichée : il porte la chaîne de connexion
    (avec son mot de passe) et les `machineKey`. On ne rend que des comptes.

    Un fichier illisible rend `ok=False` : l'absence de mesure n'est pas une mesure.
    """
    try:
        with open(path, "rb") as fh:
            raw = fh.read()
    except OSError as exc:
        return [{"error": f"web.config illisible : {exc}", "ok": False}]
    text = raw.decode("utf-8", errors="replace")
    rows = [
        {
            "path": path,
            "size": len(raw),
            "expected_size": expected_size,
            "ok": len(raw) == expected_size,
        }
    ]
    for pattern, expected, why in WEBCONFIG_MARKERS:
        got = _count_lines(text, pattern)
        rows.append(
            {"marker": pattern, "count": got, "expected": expected, "why": why, "ok": got == expected}
        )
    return rows


def form_pattern(fmt: str) -> re.Pattern[str]:
    """Motif d'une forme servie : {0}/{1} -> nombres, le reste littéral."""
    out = []
    i = 0
    while i < len(fmt):
        if fmt[i] == "{" and i + 2 < len(fmt) and fmt[i + 1] in "01" and fmt[i + 2] == "}":
            out.append(f"(?P<n{fmt[i + 1]}>[{DIGITS}]+)")
            i += 3
        else:
            out.append(re.escape(fmt[i]))
            i += 1
    return re.compile("".join(out))


def normalize(text: str) -> str:
    """HTML décodé + espaces insécables rendus à l'espace : un motif littéral ne
    survit ni à `&agrave;` ni à `\\xa0`."""
    text = html.unescape(text)
    return text.replace(" ", " ").replace(" ", " ").replace("‏", "").replace("‎", "")


def to_int(digits: str) -> int | None:
    """Nombres persans/arabes -> entier, ASCII -> entier. None si illisible."""
    out = []
    for ch in digits:
        if "0" <= ch <= "9":
            out.append(ch)
        elif "۰" <= ch <= "۹":
            out.append(str(ord(ch) - 0x06F0))
        elif "٠" <= ch <= "٩":
            out.append(str(ord(ch) - 0x0660))
        else:
            return None
    return int("".join(out)) if out else None


def fetch(url: str, resolve: str | None, cb: str) -> tuple[int, str]:
    """GET avec cache-buster et -L. Renvoie (statut, corps décodé)."""
    sep = "&" if "?" in url else "?"
    target = f"{url}{sep}cb={cb}"
    cmd = ["curl", "-sL", "--max-time", "60", "-w", "\n%{http_code}", target]
    if resolve:
        host = re.sub(r"^https?://", "", url).split("/")[0]
        scheme = "https" if url.startswith("https") else "http"
        cmd[1:1] = ["--resolve", f"{host}:{443 if scheme == 'https' else 80}:{resolve}"]
    proc = subprocess.run(cmd, capture_output=True)
    raw = proc.stdout
    # `-w "\n%{http_code}"` : le corps puis le statut sur la dernière ligne.
    split = raw.rsplit(b"\n", 1)
    if len(split) != 2:
        return 0, ""
    body, code = split[0], split[1].strip()
    try:
        status = int(code)
    except ValueError:
        status = 0
    return status, body.decode("utf-8", errors="replace")


def control_routes(base: str, resolve: str | None, cb: str) -> list[dict]:
    """8 cultures × 4 surfaces. Une route 404 est un échec DUR : c'est exactement ce
    que la bascule peut casser sans le dire (le /en/ 404 de la prod gelée)."""
    rows = []
    for lang, (prefix, expect_lang) in CULTURES.items():
        for surface, path in (
            ("accueil", f"{prefix}"),
            ("terms", f"{prefix}terms"),
            ("privacy", f"{prefix}privacy"),
            ("règles", f"{prefix}Règles"),
        ):
            status, body = fetch(f"{base}/{path}", resolve, cb)
            served = None
            m = re.search(r'<html[^>]*\blang="([^"]+)"', body)
            if m:
                served = m.group(1)
            rows.append(
                {
                    "lang": lang,
                    "surface": surface,
                    "url": f"/{path}",
                    "status": status,
                    "html_lang": served,
                    "lang_ok": (served == expect_lang) if surface == "accueil" else None,
                    "bytes": len(body.encode("utf-8", errors="replace")),
                    # Un accueil qui rend la langue d'une AUTRE culture est un échec
                    # dur : c'est le repli silencieux (une langue tombe sur le défaut
                    # du portail sans rien dire). Mesuré : une page 404 rend elle
                    # aussi `lang="en-US"` quelle que soit l'URL — d'où le test de
                    # statut d'abord, la langue ne juge que ce qui est servi.
                    "ok": status == 200 and (surface != "accueil" or served == expect_lang),
                }
            )
    return rows


def control_legal_en(base: str, resolve: str | None, cb: str) -> list[dict]:
    """Les DEUX sens, page par page : marqueur propre à la page PRÉSENT, texte stock
    de DNN ABSENT. Un seul sens ne distingue pas « texte restauré » de « page vide »."""
    rows = []
    for page, markers in LEGAL_PAGES.items():
        status, body = fetch(f"{base}/en/{page}", resolve, cb)
        text = normalize(body)
        counts = {m: text.count(m) for m in markers}
        template = text.count(LEGAL_DNN_TEMPLATE)
        rows.append(
            {
                "page": f"/en/{page}",
                "status": status,
                "markers": counts,
                "mark": sum(counts.values()),
                "dnn_template": template,
                "ok": status == 200 and all(c > 0 for c in counts.values()) and template == 0,
            }
        )
    return rows


def control_rules_d1(base: str, resolve: str | None, cb: str) -> list[dict]:
    """Compteurs D1 servis par langue. On lit les NOMBRES au passage : c'est eux qui
    portent #1502 (« de 3 à 10 joueurs » côté cartes vs autre chose côté site)."""
    rows = []
    for lang, (prefix, expect_lang) in CULTURES.items():
        status, body = fetch(f"{base}/{prefix}Règles", resolve, cb)
        text = normalize(body)
        intervals = []
        for m in form_pattern(INTERVAL_FORMS[lang]).finditer(text):
            intervals.append((to_int(m.group("n0")), to_int(m.group("n1"))))
        singles = [to_int(m.group("n0")) for m in form_pattern(SINGLE_FORMS[lang]).finditer(text)]
        three_to_ten = sum(1 for lo, hi in intervals if lo == 3 and hi == 10)
        four_to_ten = sum(1 for lo, hi in intervals if lo == 4 and hi == 10)
        four_to_eight = sum(1 for lo, hi in intervals if lo == 4 and hi == 8)
        rows.append(
            {
                "lang": lang,
                "status": status,
                "intervals": len(intervals),
                "distinct_pairs": sorted({p for p in intervals if p[0] is not None}),
                "singles": len(singles),
                "single_values": sorted({v for v in singles if v is not None}),
                "three_to_ten": three_to_ten,
                "four_to_ten": four_to_ten,
                "four_to_eight": four_to_eight,
                # Témoin : une page Règles servie DOIT porter au moins un compteur.
                "ok": status == 200 and (len(intervals) + len(singles)) > 0,
            }
        )
    return rows


def control_eventlog(hours: int) -> list[dict]:
    """Journal Application sur la fenêtre. Instrument qui seul voit une redirection
    ratée (HTTP 200 avec 32 FileLoadException, mesuré 02/11). CIM plutôt que
    Get-WinEvent seul : lisible sans élévation sur Application."""
    ps = (
        "$ErrorActionPreference='Stop';"
        f"$since=(Get-Date).AddHours(-{hours});"
        "$ev=Get-WinEvent -FilterHashtable @{LogName='Application';StartTime=$since} -ErrorAction SilentlyContinue;"
        "$out=@();foreach($e in $ev){$m=$e.Message;if($m){$out+=[pscustomobject]@{Time=$e.TimeCreated.ToString('s');"
        "Provider=$e.ProviderName;Id=$e.Id;Level=$e.LevelDisplayName;Message=$m}}};"
        # La machine est dans la sortie, pas dans une note : un « 0 » n'a de sens que
        # rapporté au journal dont il vient (le VPS prod, pas le siège qui pilote).
        "[pscustomobject]@{Machine=$env:COMPUTERNAME;Events=$out}|ConvertTo-Json -Compress -Depth 3"
    )
    try:
        proc = subprocess.run(
            ["powershell", "-NoProfile", "-NonInteractive", "-Command", ps],
            capture_output=True,
            timeout=300,
        )
    except Exception as exc:  # journal inaccessible = contrôle NON concluant, pas vert
        return [{"error": f"journal illisible : {exc}", "ok": False}]
    raw = proc.stdout.decode("utf-8", errors="replace").strip()
    if not raw:
        return [{"error": (proc.stderr.decode("utf-8", errors="replace") or "journal vide")[:400], "ok": False}]
    try:
        payload = json.loads(raw)
    except json.JSONDecodeError as exc:
        return [{"error": f"JSON illisible : {exc}", "ok": False}]
    if isinstance(payload, dict) and "Events" in payload:
        machine = payload.get("Machine") or "?"
        events = payload.get("Events") or []
    elif isinstance(payload, dict):  # un dict nu = un seul événement (ancien format)
        machine, events = "?", [payload]
    else:
        machine, events = "?", payload
    if isinstance(events, dict):
        events = [events]
    rows = []
    for name, pattern in EVENT_MARKERS.items():
        rx = re.compile(pattern, re.IGNORECASE)
        hits = [e for e in events if rx.search(e.get("Message") or "")]
        rows.append(
            {
                "marker": name,
                "hits": len(hits),
                "sample": [
                    {
                        "time": h.get("Time"),
                        "id": h.get("Id"),
                        "excerpt": (h.get("Message") or "")[:160].replace("\r", " ").replace("\n", " "),
                    }
                    for h in hits[:3]
                ],
                # Seul FileLoadException est un échec DUR : c'est la signature d'une
                # redirection ratée. Les 2 autres sont des COMPTES à comparer à la
                # ligne de base (un 0 n'y prouve rien — il peut manquer d'événements).
                "ok": hits == [] if name == "FileLoadException" else None,
            }
        )
    rows.append(
        {"window_hours": hours, "events_in_window": len(events), "machine": machine, "ok": True}
    )
    return rows


def render_md(label: str, base: str, stamp: str, routes, legal, rules, events, webconfig) -> str:
    out = [
        f"# Contrôles du jour J — ligne de base `{label}`",
        "",
        f"- Cible : `{base}`",
        f"- Mesuré : {stamp} (lecture seule, cache-buster `?cb=` sur chaque sonde, `-L`)",
        "",
        "## 1. Routes (8 cultures × 4 surfaces)",
        "",
    ]
    if not routes:
        out += [
            "⚠️ **Non mesuré** (`--skip-web`) — normal **avant que le site soit servi** : "
            "aucune route n'a de sens à ce moment-là, et un 503 lu comme un échec ferait "
            "reculer à tort.",
        ]
    else:
        out += ["| Langue | Surface | URL | Statut | `<html lang>` | Octets |", "|---|---|---|---|---|---|"]
        for r in routes:
            mark = "✅" if r["status"] == 200 else "❌"
            out.append(
                f"| {r['lang']} | {r['surface']} | `{r['url']}` | {mark} {r['status']} | "
                f"{r['html_lang'] or '—'} | {r['bytes']} |"
            )
    out += ["", "## 2. Texte légal anglais (les deux sens)", ""]
    if not legal:
        out.append("⚠️ **Non mesuré** (`--skip-web`).")
    else:
        out += [
            "| Page | Statut | Marqueur propre à la page | Texte stock DNN | Verdict |",
            "|---|---|---|---|---|",
        ]
        for r in legal:
            marks = " · ".join(f"« {m} » ×{c}" for m, c in r["markers"].items())
            out.append(
                f"| `{r['page']}` | {r['status']} | {marks} | « AGREEMENT BETWEEN USER AND » ×{r['dnn_template']} | "
                f"{'✅' if r['ok'] else '❌'} |"
            )
    out += [
        "",
        "Lecture : le marqueur de la page doit être **présent** (≥ 1) et le texte **stock de DNN** absent (0).",
        "Un marqueur à 0 avec un stock à 0 est une page vide, pas un succès.",
        "",
        "⚠️ Le marqueur de `terms` est « ARGUMENTUM SAS » ; celui de `privacy` est la phrase de primauté du",
        "français — la politique de confidentialité **ne nomme pas la société**, ni dans le neutre ni dans le",
        "fr-FR (mesuré 05/11 sur les valeurs du dépôt). Un contrôle qui exigerait « ARGUMENTUM SAS » sur",
        "`privacy` ne pourrait pas passer.",
        "",
        "## 3. Compteurs de joueurs servis (#1502 / D1)",
        "",
    ]
    if not rules:
        out.append("⚠️ **Non mesuré** (`--skip-web`).")
    else:
        out += [
            "| Langue | Statut | Intervalles | Nombres lus | Min==max | 3→10 | 4→10 | 4→8 |",
            "|---|---|---|---|---|---|---|---|",
        ]
        for r in rules:
            pairs = ", ".join(f"{lo}→{hi}" for lo, hi in r["distinct_pairs"]) or "—"
            out.append(
                f"| {r['lang']} | {r['status']} | {r['intervals']} | {pairs} | {r['singles']} | "
                f"{r['three_to_ten']} | {r['four_to_ten']} | {r['four_to_eight']} |"
            )
    out += [
        "",
        "Témoin : une page Règles servie porte au moins un compteur ; 0 est un signal d'instrument",
        "avant d'être un résultat. La colonne 3→10 est la cible #1502 côté cartes.",
        "",
        "## 4. Journal Application (EventLog)",
        "",
    ]
    if not events:
        out.append(
            "⚠️ **Non mesuré** (`--skip-eventlog`) — à ne pas confondre avec "
            "« 0 FileLoadException » : l'un est une absence de mesure, l'autre une mesure."
        )
    for r in events:
        if "error" in r:
            out.append(f"- ❌ {r['error']}")
        elif "window_hours" in r:
            out.append(
                f"- Fenêtre : {r['window_hours']} h · **{r['events_in_window']}** événements lus"
                f" · machine dont le journal est lu : **{r.get('machine') or '?'}**"
            )
        else:
            verdict = "✅ 0" if r["ok"] else ("—" if r["ok"] is None else "❌")
            out.append(f"- {verdict} **{r['marker']}** : {r['hits']} occurrence(s)")
            for s in r["sample"]:
                out.append(f"  - `{s['time']}` id {s['id']} — {s['excerpt']}")
    out += [
        "",
        "⚠️ Ce journal est celui de la machine qui exécute le script. Le jour J, lancez-le "
        "**sur le VPS de la prod** : depuis un autre siège, un « 0 FileLoadException » décrit "
        "la mauvaise machine et le contrôle est vert à tort.",
        "",
        "## 5. web.config de la cible (#1049)",
        "",
    ]
    if not webconfig:
        out += [
            "⚠️ **Non mesuré** — aucun `--webconfig` fourni. Ce contrôle est **obligatoire le "
            "jour J** : c'est lui qui dit si le fichier versionné a atteint la copie, avant que "
            "le site soit servi. Une absence de mesure n'est pas un vert.",
        ]
    elif "error" in webconfig[0]:
        out.append(f"- ❌ {webconfig[0]['error']}")
    else:
        size = webconfig[0]
        verdict_size = "✅" if size["ok"] else "❌"
        out += [
            f"- Fichier : `{size['path']}` — **{size['size']}** o "
            f"({verdict_size} attendu {size['expected_size']} o)",
            "",
            "| Marqueur | Compte | Attendu | Ce qu'il dit | Verdict |",
            "|---|---:|---:|---|---|",
        ]
        for r in webconfig[1:]:
            out.append(
                f"| `{r['marker']}` | {r['count']} | {r['expected']} | {r['why']} | "
                f"{'✅' if r['ok'] else '❌'} |"
            )
        out += [
            "",
            "Lecture : **un seul** compte qui dévie ⇒ le fichier **versionné** a atteint la copie.",
            "Reculer — remettre le `web.config` vivant sauvegardé — ne pas « réparer ».",
            "⛔ Comptes seulement, jamais les lignes trouvées : le fichier porte la chaîne de "
            "connexion et les `machineKey`.",
        ]
    hard_fail = (
        [r for r in routes if not r["ok"]]
        + [r for r in legal if not r["ok"]]
        + [r for r in rules if not r["ok"]]
        + [r for r in events if r.get("ok") is False]
        + [r for r in webconfig if r.get("ok") is False]
    )
    verdict = "❌ " + str(len(hard_fail)) + " échec(s) dur(s)" if hard_fail else "✅ tous les contrôles durs passent"
    if not webconfig:
        # Une absence de mesure ne doit pas se lire dans la même ligne qu'un « tout passe ».
        verdict += " · ⚠️ **web.config non mesuré** (obligatoire le jour J)"
    out += [
        "",
        f"**Verdict** : {verdict}.",        "",
        "*po-2023 · script : `tools/1105-cutover-day-controls.py`*",
        "",
    ]
    return "\n".join(out)


def main() -> int:
    # Le rapport est du markdown UTF-8 (flèches `→`, `⇒`, `≥`). Sur une console Windows
    # non-UTF-8, `print(md)` mourait en `UnicodeEncodeError: 'charmap' codec …` dès que
    # stdout était redirigé vers un fichier — au pire moment, à l'étape 5 du jour J.
    # `reconfigure` existe depuis 3.7 ; en cas d'absence on garde le comportement d'origine.
    try:
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    except (AttributeError, ValueError):
        pass

    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--base-url", default="https://dnn.argumentum.myia.io")
    ap.add_argument("--resolve", default=None, help="IP pour --resolve (hairpin : 127.0.0.1)")
    ap.add_argument("--label", default="baseline")
    ap.add_argument("--event-log-hours", type=int, default=24)
    ap.add_argument("--skip-eventlog", action="store_true", help="hors Windows / sans journal")
    ap.add_argument(
        "--skip-web",
        action="store_true",
        help="sauter les 3 contrôles HTTP — étape 5, avant que le site soit servi",
    )
    ap.add_argument(
        "--webconfig",
        default=None,
        help="chemin du web.config de la CIBLE : contrôle #1049 (comptes + taille, jamais les lignes)",
    )
    ap.add_argument(
        "--webconfig-size",
        type=int,
        default=WEBCONFIG_EXPECTED_SIZE,
        help=f"taille attendue du web.config en octets (défaut {WEBCONFIG_EXPECTED_SIZE})",
    )
    ap.add_argument("--out", default=None, help="fichier markdown à écrire")
    args = ap.parse_args()

    base = args.base_url.rstrip("/")
    cb = _dt.datetime.now().strftime("%Y%m%d%H%M%S")
    stamp = _dt.datetime.now().strftime("%Y-%m-%d %H:%M")

    routes = [] if args.skip_web else control_routes(base, args.resolve, cb)
    legal = [] if args.skip_web else control_legal_en(base, args.resolve, cb)
    rules = [] if args.skip_web else control_rules_d1(base, args.resolve, cb)
    events = [] if args.skip_eventlog else control_eventlog(args.event_log_hours)
    webconfig = [] if not args.webconfig else control_webconfig(args.webconfig, args.webconfig_size)

    md = render_md(args.label, base, stamp, routes, legal, rules, events, webconfig)
    print(md)
    if args.out:
        with open(args.out, "w", encoding="utf-8", newline="\n") as fh:
            fh.write(md)
        print(f"\n[écrit] {args.out}", file=sys.stderr)

    hard_fail = (
        [r for r in routes if not r["ok"]]
        + [r for r in legal if not r["ok"]]
        + [r for r in rules if not r["ok"]]
        + [r for r in events if r.get("ok") is False]
        + [r for r in webconfig if r.get("ok") is False]
    )
    return 1 if hard_fail else 0


if __name__ == "__main__":
    sys.exit(main())
