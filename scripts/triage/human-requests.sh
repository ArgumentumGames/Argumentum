#!/usr/bin/env bash
# Détecte les demandes HUMAINES nouvelles sur GitHub (issues + commentaires).
#
# Raison d'être : relire tout le backlog à chaque cycle coûte des dizaines de
# milliers de tokens au coordinateur. Ce script rend la même information pour
# quelques appels API et une poignée de lignes.
#
# ⚠️ Le login N'EST PAS un discriminateur. Les agents du cluster poussent avec
# le token partagé `jsboige` : sur 30 jours, 351 commentaires portent ce login
# et ~239 sont des rapports d'agents. Filtrer par auteur ne sépare rien.
#
# ⚠️ Un appel API qui échoue rend un flux VIDE — donc « (aucune) » est
# indiscernable d'un backlog propre. Incident fondateur : 2026-09-13, le quota
# GraphQL du token (partagé par TOUTE la machine, pas par cette lane) a été
# épuisé par une lane voisine. `gh issue list` — alors le seul appel GraphQL du
# script — rendait une sortie vide sous `2>/dev/null`, et le filet C a affiché
# « (aucune) » deux exécutions de suite. Mesure du jour : le filtre jq était
# CORRECT (rejoué sur les mêmes données servies par REST, il rend bien #1293,
# auteur externe, 2600 caractères) ; seul le canal était mort. D'où deux
# remèdes, tous deux nécessaires :
#   1. tout passe par REST (`gh api`), qui a répondu pendant toute la panne ;
#   2. un appel qui échoue CRIE (« ORGANE AVEUGLE ») et fait sortir le script
#      en 2 — il ne dégrade jamais en « (aucune) ».
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

# Lit un endpoint REST et écrit le JSON sur stdout. rc=1 + cri sur stderr si
# l'appel échoue ou ne rend pas du JSON. ⛔ Jamais de `2>/dev/null` ici : c'est
# exactement ce qui a rendu la panne du 13/09 invisible.
fetch() {
  local out rc
  out="$(gh api "repos/$REPO/$1" --paginate 2>&1)"; rc=$?
  if [ $rc -ne 0 ] || ! jq -e . >/dev/null 2>&1 <<<"$out"; then
    {
      echo "  ⛔ ORGANE AVEUGLE — l'appel API a échoué, AUCUNE conclusion possible."
      echo "     endpoint : $1"
      head -2 <<<"$out" | sed 's/^/     /'
    } >&2
    return 1
  fi
  printf '%s' "$out"
}

scan() {
  local since="$1"
  local comments issues
  # Assignation sur sa propre ligne : `local x="$(cmd)"` rendrait le rc de
  # `local`, pas celui de la commande — la garde ne verrait jamais l'échec.
  comments="$(fetch "issues/comments?since=${since}&per_page=100")" || return 2
  issues="$(fetch "issues?state=all&per_page=100&sort=created&direction=desc")" || return 2

  echo "### Filet A — demandes brèves (<${MAXLEN} car., hors ménage d'agent)"
  # Le tri se fait DANS jq : grep filtre des lignes, or un enregistrement en fait
  # deux — un grep -v laissait l'en-tête orphelin de la ligne rejetée.
  jq -r --arg hk "$HOUSEKEEPING" '.[]|select(.user.login|test("dependabot")|not)
             |select(.body|length < '"$MAXLEN"')
             |select(.body|test($hk)|not)
             |"  #\(.issue_url|split("/")|last) \(.created_at|.[0:16]) <\(.html_url)>
    \(.body|gsub("
";" "))"' <<<"$comments" | grep . || echo "  (aucune)"

  echo "### Filet B — commentaires adressés à un agent (toute longueur)"
  jq -r '.[]|select(.body|test("@myia-(ai-01|po-20[0-9]{2})"))
             |"  #\(.issue_url|split("/")|last) \(.created_at|.[0:16]) <\(.html_url)>\n    \(.body|gsub("\n";" ")|.[0:300])"' \
    <<<"$comments" | grep . || echo "  (aucun)"

  echo
  echo "### Filet C — issues ouvertes sans bannière d'agent"
  # ⚠️ Le plafond de longueur était un PROXY de « écrite par un agent ». Il
  # corrèle avec « détaillée », donc il rendait l'organe aveugle aux rapports
  # les mieux documentés — exactement les plus utiles. Incident fondateur :
  # #1293 (consommateur aval, 2600 car., sourcé), resté 44 h sans réponse
  # alors que les trois filets rendaient vert à chaque cycle.
  # Le plafond ne s'applique donc qu'aux auteurs DU CLUSTER ; un auteur externe
  # est remonté quelle que soit la longueur.
  # ⚠️ REST et non `gh issue list` : cet endpoint mêle issues et PR, d'où le
  # `select(.pull_request==null)` — une PR n'est pas une demande humaine.
  jq -r -n --arg s "$since" --arg cl "$CLUSTER_LOGINS" '[inputs]|add
             |($cl|split(",")) as $cluster
             |[.[]|select(.pull_request==null)]|.[0:60]|.[]|. as $it
             |select($it.created_at > $s)
             |select(($it.body // "")|ascii_downcase|test("agent `myia|coordinator ai-01|⚠️ agent|ouverte par l.agent")|not)
             |select((($cluster|index($it.user.login))==null) or ((($it.body // "")|length) < 1200))
             |"  #\($it.number) \($it.created_at|.[0:16]) [\($it.user.login)] \($it.title)\n    \($it.html_url)"' \
    <<<"$issues" | grep . || echo "  (aucune)"
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
  grep -q "#1293" <<<"$out" || { echo "FAIL: demande externe LONGUE manquée (filet C)"; rc=1; }
  # Contrôle inverse ajouté le 13/09 — la panne que les TROIS précédents ne
  # voyaient pas : un organe aveugle rendait « (aucune) », c'est-à-dire la même
  # chose qu'un backlog propre. On vérifie donc que l'organe CRIE quand l'appel
  # échoue. Dépôt inexistant = panne d'API reproductible sans attendre un quota.
  local dead
  dead="$(REPO=ArgumentumGames/__triage_selftest_no_such_repo__ scan 2026-08-26T14:00:00Z 2>&1)"
  grep -q "ORGANE AVEUGLE" <<<"$dead" || { echo "FAIL: une panne d'API se déguise en backlog propre"; rc=1; }
  grep -q "(aucune)"       <<<"$dead" && { echo "FAIL: une panne d'API rend encore '(aucune)'"; rc=1; }
  [ $rc -eq 0 ] && echo "OK — l'organe voit les 3 demandes humaines, rejette le ménage d'agent, et crie quand l'API tombe"
  return $rc
}

case "${1:-}" in
  --self-test) self_test ;;
  "")          scan "$(date -u -d '26 hours ago' +%Y-%m-%dT%H:%M:%SZ)" ;;
  *)           scan "$1" ;;
esac
