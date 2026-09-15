#!/usr/bin/env bash
# #133 — post-merge smoke for the OWL endpoint exposed by .github/workflows/static.yml.
#
# The workflow mounts docs/ontology/ into the GitHub Pages artifact, so the bytes served
# must be the committed master blobs. This script measures exactly that: HTTP status,
# Content-Type and the sha256 of the body, against a live control (200) and a dead control
# (404) so that a green run cannot come from a broken instrument.
#
# The sha256 check is what makes a 200 unambiguous. Before the workflow landed, the two
# OWL URLs returned the Pages "file not found" page — a 9 379-byte HTML body, identical in
# size to any other missing path on that host. A size check, or an "does it look like XML"
# check, discriminates nothing; only the hash separates served-artifact from error-page.
#
# Expected to FAIL before the workflow is merged: that pre-merge failure is the 404 half of
# the proof. Run it before and after to get both halves.
#
# Usage: tools/133-pages-owl-smoke.sh [BASE_URL] [GIT_REF]
#   BASE_URL  default https://argumentumgames.github.io/Argumentum
#   GIT_REF   oracle for the expected hashes, default origin/master
#
# Exit: 0 all expectations met · 1 at least one failed · 2 environment problem.
set -u

BASE="${1:-https://argumentumgames.github.io/Argumentum}"
REF="${2:-origin/master}"

command -v curl >/dev/null 2>&1 || { echo "FATAL: curl not found on PATH" >&2; exit 2; }
command -v sha256sum >/dev/null 2>&1 || { echo "FATAL: sha256sum not found on PATH" >&2; exit 2; }
git rev-parse --verify --quiet "$REF" >/dev/null || { echo "FATAL: git ref '$REF' not found (run from the repo)" >&2; exit 2; }

TMP="$(mktemp -d)"
trap 'rm -rf "$TMP"' EXIT

fails=0
pass() { printf '  %-34s %s\n' "$1" "$2"; }
verdict() { # <ok:0|1> <label>
  if [ "$1" -eq 0 ]; then printf '  %-34s \033[32mPASS\033[0m\n' "$2"
  else printf '  %-34s \033[31mFAIL\033[0m\n' "$2"; fails=$((fails + 1)); fi
}

# committed_sha <repo-path> -> sha256 of the blob at $REF
# MSYS_NO_PATHCONV: Git-Bash rewrites "ref:path" into a Windows path, which makes git
# report "ambiguous argument 'origin\\master;...'". The env var is inert on Linux/macOS.
committed_sha() {
  MSYS_NO_PATHCONV=1 git cat-file -p "$REF:$1" 2>/dev/null | sha256sum | cut -d' ' -f1
}

# served <slug> <url> -> "code|ctype|sha256"
served() {
  local body="$TMP/$1"
  local meta sha
  meta="$(curl -sS -m 30 -o "$body" -w '%{http_code}|%{content_type}' "$2" 2>/dev/null)" || meta="ERR|"
  sha="-"
  [ -s "$body" ] && sha="$(sha256sum "$body" | cut -d' ' -f1)"
  printf '%s|%s' "$meta" "$sha"
}

echo "=== #133 Pages OWL smoke ==="
echo "base   : $BASE"
echo "oracle : $REF ($(MSYS_NO_PATHCONV=1 git rev-parse --short "$REF"))"
echo

# --- targets: the two published artefacts must be the committed blobs ---
for pair in "argumentum.owl:docs/ontology/argumentum.owl" \
            "argumentum_virtues.owl:docs/ontology/argumentum_virtues.owl"; do
  name="${pair%%:*}"
  path="${pair#*:}"
  IFS='|' read -r code ctype got <<<"$(served "$name" "$BASE/$path")"
  want="$(committed_sha "$path")"

  echo "[target] $path"
  pass "http"         "$code"
  pass "content-type" "$ctype"
  pass "served sha256"    "$got"
  pass "committed sha256" "$want"

  [ "$code" = "200" ] && verdict 0 "status is 200" || verdict 1 "status is 200 (got $code)"
  [ "$got" = "$want" ] && verdict 0 "served bytes == committed blob" \
                       || verdict 1 "served bytes == committed blob"
  echo
done

# --- controls: the instrument must see a live page and a dead path ---
echo "[control] live  /  (CardPen root must stay 200 — its URLs are a host contract)"
IFS='|' read -r code _ctype sha <<<"$(served "root" "$BASE/")"
pass "http" "$code"
pass "served sha256" "$sha"
[ "$code" = "200" ] && verdict 0 "CardPen root still served" || verdict 1 "CardPen root still served (got $code)"
echo

echo "[control] dead  /docs/ontology/__witness-404__.owl  (missing path must stay 404)"
IFS='|' read -r code _ctype sha <<<"$(served "witness" "$BASE/docs/ontology/__witness-404__.owl")"
pass "http" "$code"
pass "served sha256" "$sha"
[ "$code" = "404" ] && verdict 0 "host answers 404 for a missing path" \
                    || verdict 1 "host answers 404 for a missing path (got $code)"
echo

if [ "$fails" -eq 0 ]; then
  echo "VERDICT: PASS — the endpoint serves the committed OWL, and the instrument discriminates."
  exit 0
fi
echo "VERDICT: FAIL — $fails expectation(s) unmet. Before the workflow merge this is the expected"
echo "         pre-pose state (both OWL URLs 404); after it, treat as a real defect."
exit 1
