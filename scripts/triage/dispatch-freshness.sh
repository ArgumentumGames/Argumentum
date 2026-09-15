#!/usr/bin/env bash
# dispatch-freshness.sh — garde anti-dispatch-périmé (ai-01)
#
# POURQUOI CET ORGANE EXISTE
#   Récurrence mesurée : le coordinateur dispatche depuis le TITRE et le CORPS d'une
#   issue, sans relire ses commentaires récents — et redemande un travail déjà livré.
#   Constaté 3 fois (po-2023 sur #1183/#965 le 29/08 ; po-2024 sur #965 puis sur les
#   4 bullets du dispatch zev80v). Le worker perd un tick à corriger la prémisse.
#
#   La règle Phase 2 du skill `coordinate` l'exige déjà (« Dispatch worker → body issue
#   cible + comments »). Une règle qu'on oublie 3 fois n'est pas une règle, c'est un
#   vœu. Cet organe la rend mécanique.
#
# USAGE
#   scripts/triage/dispatch-freshness.sh 965 1145 1187      # avant de dispatcher
#   scripts/triage/dispatch-freshness.sh --self-test        # l'organe voit-il encore ?
#
# SORTIE : par issue, l'état + les 3 derniers commentaires (date, auteur, extrait).
#   ⚠ = l'issue porte des commentaires — LIS-LES avant de rédiger le dispatch.
#   ✓ = aucun commentaire, le corps suffit.
#
# ⛔ CE QUE CET ORGANE NE FAIT PAS
#   Il ne juge pas si le travail est fait — il rend seulement la matière visible.
#   `author.login` ne discrimine PAS humain/agent (token partagé `jsboige`), donc ne
#   déduis rien de l'auteur : lis le contenu.

set -uo pipefail

REPO="${ARGUMENTUM_REPO:-ArgumentumGames/Argumentum}"
NB_COMMENTS=3

die() { printf '%s\n' "$*" >&2; exit 1; }

command -v gh >/dev/null 2>&1 || die "gh introuvable — l'organe est aveugle."

render_issue() {
  local num="$1" json
  json=$(gh issue view "$num" --repo "$REPO" \
           --json number,title,state,updatedAt,comments 2>/dev/null) || {
    printf '  ✗ #%s — illisible (inexistante, ou droits insuffisants)\n\n' "$num"
    return
  }

  printf '%s' "$json" | jq -r --argjson n "$NB_COMMENTS" '
    (.comments | length) as $c
    | (if $c > 0 then "⚠" else "✓" end) as $flag
    | "\($flag) #\(.number) [\(.state)] \(.title[0:78])",
      "    maj \(.updatedAt[0:16]) · \($c) commentaire(s)",
      ( if $c == 0 then
          "    (aucun commentaire — le corps de l'\''issue suffit)"
        else
          ( .comments[-$n:][]
            | "    ├─ \(.createdAt[0:16])  \(.author.login // "?")",
              "    │  \((.body // "") | gsub("\r";"") | gsub("\n";" ") | .[0:150])"
          )
        end ),
      ""
  '
}

# --- self-test : l'organe doit voir des commentaires là où il y en a, et pas ailleurs
if [ "${1:-}" = "--self-test" ]; then
  # #1187 porte le verdict BAT (donc des commentaires) ; on vérifie que le flag ⚠ sort.
  probe=$(render_issue 1187 2>/dev/null)
  case "$probe" in
    *"⚠ #1187"*) ;;
    *) die "ÉCHEC — l'organe ne voit pas les commentaires de #1187. Il est aveugle." ;;
  esac
  case "$probe" in
    *"├─"*) ;;
    *) die "ÉCHEC — aucun commentaire rendu pour #1187." ;;
  esac
  echo "OK — l'organe voit les commentaires de #1187 et les rend."
  exit 0
fi

[ "$#" -gt 0 ] || die "usage: $(basename "$0") <numéro-issue> [<numéro-issue>...] | --self-test"

echo "### Fraîcheur des issues à dispatcher — lis les commentaires AVANT de rédiger"
echo
for n in "$@"; do
  case "$n" in
    ''|*[!0-9]*) printf '  ✗ %s — pas un numéro d'\''issue\n\n' "$n"; continue ;;
  esac
  render_issue "$n"
done
