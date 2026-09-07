#!/usr/bin/env bash
# Détecte les demandes HUMAINES nouvelles sur GitHub (issues + commentaires).
#
# Raison d'être : relire tout le backlog à chaque cycle coûte des dizaines de
# milliers de tokens au coordinateur. Ce script rend la même information pour
# ~1 appel API et une poignée de lignes.
#
# ⚠️ Le login N'EST PAS un discriminateur. Les agents du cluster poussent avec
# le token partagé `jsboige` : sur 30 jours, 351 commentaires portent ce login
# et ~239 sont des rapports d'agents. Filtrer par auteur ne sépare rien.
#
# Trois filets indépendants, aux angles morts différents :
#   A. court (<700 car.) et hors vocabulaire de ménage d'agent  → la demande brève
#   B. mentionne @myia-*                                        → la demande longue adressée
#   C. issue OUVERTE sans bannière d'agent                      → le nouveau sujet
#
# Usage :
#   ./human-requests.sh [SINCE_ISO]     # défaut : il y a 26 h
#   ./human-requests.sh --self-test     # contrôle inverse permanent (CI)
set -uo pipefail

REPO="${TRIAGE_REPO:-ArgumentumGames/Argumentum}"
MAXLEN="${TRIAGE_MAXLEN:-700}"
# Identités sous lesquelles le cluster publie. Le token partagé fait que
# `jsboige` couvre l'owner ET les trois agents — ce login n'est donc PAS une
# preuve d'agent. L'implication inverse, elle, tient : un auteur HORS de cet
# ensemble n'est pas un agent du cluster, et son issue mérite d'être vue
# quelle que soit sa longueur (cf. filet C).
CLUSTER_LOGINS="${TRIAGE_CLUSTER_LOGINS:-jsboige,myia-ai-01,myia-po-2023,myia-po-2024,app/dependabot}"
# Vocabulaire de ménage d'agent — SANS ancre ^ : la sortie jq porte un préfixe,
# une ancre ne matcherait jamais (le bug qui a produit 2 faux positifs au test).
HOUSEKEEPING='Superseded|Closing in favor|Clos : le DoD|Sans objet|Dispatché|Rebase sur|delivered as|livre les|🤖|Fermeture sur|Merged as|Closed by'

api() { gh api "repos/$REPO/$1" --paginate 2>/dev/null; }

scan() {
  local since="$1"
  echo "### Filet A — demandes brèves (<${MAXLEN} car., hors ménage d'agent)"
  # Le tri se fait DANS jq : grep filtre des lignes, or un enregistrement en fait
  # deux — un grep -v laissait l'en-tête orphelin de la ligne rejetée.
  api "issues/comments?since=${since}&per_page=100"     | jq -r --arg hk "$HOUSEKEEPING" '.[]|select(.user.login|test("dependabot")|not)
             |select(.body|length < '"$MAXLEN"')
             |select(.body|test($hk)|not)
             |"  #\(.issue_url|split("/")|last) \(.created_at|.[0:16]) <\(.html_url)>
    \(.body|gsub("
";" "))"'     | grep . || echo "  (aucune)"

  echo "### Filet B — commentaires adressés à un agent (toute longueur)"
  api "issues/comments?since=${since}&per_page=100" \
    | jq -r '.[]|select(.body|test("@myia-(ai-01|po-20[0-9]{2})"))
             |"  #\(.issue_url|split("/")|last) \(.created_at|.[0:16]) <\(.html_url)>\n    \(.body|gsub("\n";" ")|.[0:300])"' \
    || echo "  (aucun)"

  echo
  echo "### Filet C — issues ouvertes sans bannière d'agent"
  # ⚠️ Le plafond de longueur était un PROXY de « écrite par un agent ». Il
  # corrèle avec « détaillée », donc il rendait l'organe aveugle aux rapports
  # les mieux documentés — exactement les plus utiles. Incident fondateur :
  # #1293 (consommateur aval, 2600 car., sourcé), resté 44 h sans réponse
  # alors que les trois filets rendaient vert à chaque cycle.
  # Le plafond ne s'applique donc qu'aux auteurs DU CLUSTER ; un auteur externe
  # est remonté quelle que soit la longueur.
  gh issue list --repo "$REPO" --state all --limit 60 \
      --json number,title,body,createdAt,url,author 2>/dev/null \
    | jq -r --arg s "$since" --arg cl "$CLUSTER_LOGINS" '($cl|split(",")) as $cluster
             |.[]|. as $it
             |select($it.createdAt > $s)
             |select(($it.body // "")|ascii_downcase|test("agent `myia|coordinator ai-01|⚠️ agent|ouverte par l.agent")|not)
             |select((($cluster|index($it.author.login))==null) or ((($it.body // "")|length) < 1200))
             |"  #\($it.number) \($it.createdAt|.[0:16]) [\($it.author.login)] \($it.title)\n    \($it.url)"' \
    || echo "  (aucune)"
}

self_test() {
  # Fenêtre figée d'août 2026 : deux demandes humaines connues sur #802
  # (« Revue du deck tarot anglais », « on a acté avec Thomas et Adeline »).
  # L'organe doit les VOIR, et doit REJETER le ménage d'agent de la même fenêtre.
  local out rc=0
  out="$(scan 2026-08-26T14:00:00Z)"
  grep -q "Revue du deck tarot anglais"      <<<"$out" || { echo "FAIL: demande humaine brève manquée"; rc=1; }
  grep -q "on a acté avec Thomas et Adeline" <<<"$out" || { echo "FAIL: demande humaine adressée manquée"; rc=1; }
  # contrôle inverse : le filet A ne doit pas ramasser le ménage d'agent
  local a; a="$(sed -n '/Filet A/,/Filet B/p' <<<"$out")"
  grep -qiE 'Superseded|Dispatché|Rebase sur' <<<"$a" && { echo "FAIL: ménage d'agent capté par le filet A"; rc=1; }
  # Contrôle inverse ajouté le 07/09 — la panne que les deux précédents ne
  # voyaient pas. #1293 (auteur externe `jsboigeEpita`, 2600 caractères, sourcé)
  # est resté 44 h sans réponse pendant que l'organe rendait vert : le filet C
  # plafonnait le corps à 1200 car., donc il était aveugle AUX RAPPORTS LES PLUS
  # DÉTAILLÉS. Un organe qui ne peut pas voir le défaut qu'il prétend couvrir
  # n'est pas un contrôle — d'où cette assertion, qui échoue si le plafond revient.
  grep -q "#1293" <<<"$out" || { echo "FAIL: demande externe LONGUE manquée (filet C plafonne-t-il encore ?)"; rc=1; }
  [ $rc -eq 0 ] && echo "OK — l'organe voit les 2 demandes humaines et rejette le ménage d'agent"
  return $rc
}

case "${1:-}" in
  --self-test) self_test ;;
  "")          scan "$(date -u -d '26 hours ago' +%Y-%m-%dT%H:%M:%SZ)" ;;
  *)           scan "$1" ;;
esac
