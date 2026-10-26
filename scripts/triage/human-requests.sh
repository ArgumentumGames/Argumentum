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
# Quatre filets indépendants, aux angles morts différents :
#   A. court (<700 car.) et hors vocabulaire de ménage d'agent  → la demande brève
#   B. mentionne @myia-*                                        → la demande longue adressée
#   C. issue OUVERTE sans bannière d'agent                      → le nouveau sujet
#   D. auteur HORS CLUSTER, quelle que soit la longueur         → l'associé qu'on ne connaît pas
#
# ⚠️ Trou MESURÉ que D comble (06/10, DoD c.6005102887) : 2 des 5 commentaires
# d'Adeline (`addinette`, 865 et 1440 car.) ont échappé au filet A — dont le
# plafond de longueur est un proxy de « court », jamais de « humain » — et ne
# mentionnaient aucun agent. Seul C les a vus, et seulement parce que l'issue
# était ouverte : sur une issue fermée ou sur une PR, ils étaient invisibles.
# D ne regarde QUE l'auteur, donc la longueur cesse d'être un filtre.
#
# ⚠️ Contrepartie déclarée, mesurée les 04/10 et 07/10 : `jsboigeEpita` N'EST PAS
# dans CLUSTER_LOGINS — c'est précisément là que le filet C attrape la demande
# externe #1293 — mais des RAPPORTS D'AGENT y sont publiés (po-2023 : livraison
# du 04/10 sur #458, et sur #1781 « Message de l'agent technique (po-2023), pas
# de Jesse »). D les remonte donc comme des demandes. Sortir `jsboigeEpita` de D
# veut dire l'entrer dans CLUSTER_LOGINS, ce qui ÉTEINT la détection de #1293 par
# C : l'arbitrage appartient au coordinateur, pas au script. Le self-test épingle
# le comportement ACTUEL pour que ce choix ne change pas en silence.
#
# Usage :
#   ./human-requests.sh [SINCE_ISO [UNTIL_ISO]]   # défaut : il y a 26 h, sans borne haute
#   ./human-requests.sh --self-test               # contrôle inverse permanent (CI)
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
# ⚠️ Sensible à la casse par design : passer test($hk;"i") avalerait une demande
# humaine contenant « superseded », « merged as »… en prose (faux négatif = le
# sens dangereux). La casse du vocabulaire n'est PAS le discriminateur des
# rapports d'agent — la bannière ci-dessous l'est (incident 18/09 : self-test
# rouge sur « superseded » minuscule, corrigé par AGENT_BANNER, pas par "i").
HOUSEKEEPING='Superseded|Closing in favor|Clos : le DoD|Sans objet|Dispatché|Rebase sur|delivered as|livre les|🤖|Fermeture sur|Merged as|Closed by'
# Bannière STRUCTURELLE d'agent (18/09, piste instruite ai-01 c.5731110146) : ce
# que les gabarits du cluster génèrent mécaniquement — jamais de la prose qu'un
# humain écrit naturellement. Bloc crocheté ouvrant sur une machine du cluster
# (ex. « [po-2024 — pool #458 grain ③] »), signature de pied des workers et du
# coordinateur. Un nom de machine NU (ex. « @myia-ai-01 » en prose) ne matche
# PAS : c'est le contrôle inverse du self-test (demande humaine #802 26/08 qui
# mentionne une machine reste vue par le filet A).
# ⚠️ Pied italique (03/10, pool #458 item 5) : les corps du cluster signent
# désormais d'un pied « *po-2024* » / « *ai-01* » / « *myia-web2* » (PR bodies,
# commentaires de session). Incident réel #1677 c.5917959959 (30/09) : rapport
# court, sans crochet ni « (worker lane », capté par le filet A — son
# « (rebase sur `3bd84bc1`) » a fait rougir le self-test. Le pied ASTÉRISQUÉ
# matche donc la bannière ; le nom nu SANS astérisques ne matche toujours pas
# (le faux négatif reste le sens dangereux).
AGENT_BANNER='\[(myia-)?(ai-01|po-20[0-9]{2})[^\]]*\]|\(worker lane[,)]|Coordinator ai-01|\*(ai-01|po-20[0-9]{2}|myia-web2)\*'

# Sélecteur du filet D — extrait de `scan` pour être ÉPROUVÉ par le self-test
# sur un jeu SYNTHÉTIQUE (un contrôle qui ne peut pas voir le défaut qu'il
# prétend couvrir n'est pas un contrôle). Lit le JSON des commentaires sur
# stdin ; $1 = borne haute facultative (vide = sans borne).
# Trois exclusions, dans l'ordre : le type Bot (GitHub le marque), les logins de
# bots par nom (un `app/…` entré dans CLUSTER_LOGINS ne doit pas rouvrir la
# porte), puis l'appartenance au cluster — après strip du préfixe `app/`.
d_selector() {
  jq -r --arg u "${1:-}" --arg cl "$CLUSTER_LOGINS" '
    ($cl|split(",")|map(sub("^app/";""))) as $cluster
    |.[]|select($u == "" or .created_at < $u)
    |. as $c
    |select((($c.user.type // "User") != "Bot")
            and (($c.user.login|test("dependabot|github-actions|\\[bot\\]"))|not)
            and (($cluster|index($c.user.login)) == null))
    |"  #\($c.issue_url|split("/")|last) \($c.created_at|.[0:16]) [\($c.user.login)] <\($c.html_url)>\n    \($c.body|gsub("\n";" ")|.[0:300])"'
}

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
  # UNTIL facultatif : borne HAUTE de la fenêtre, côté client (l'API ne sait
  # fenêtrer que par `since`). Vide = sans borne (comportement de production,
  # « dernières 26 h »). Le self-test passe une borne pour FIGER son jeu
  # d'entrées — sans elle, chaque commentaire/issue créé depuis entre dans la
  # fenêtre et le test dérive (03/10 : filet A rougissant sur des rapports
  # d'agent postérieurs aux faits qu'il prétend couvrir).
  local since="$1" until="${2:-}"
  local comments issues
  # Assignation sur sa propre ligne : `local x="$(cmd)"` rendrait le rc de
  # `local`, pas celui de la commande — la garde ne verrait jamais l'échec.
  comments="$(fetch "issues/comments?since=${since}&per_page=100")" || return 2
  issues="$(fetch "issues?state=all&per_page=100&sort=created&direction=desc")" || return 2

  echo "### Filet A — demandes brèves (<${MAXLEN} car., hors ménage d'agent)"
  # Le tri se fait DANS jq : grep filtre des lignes, or un enregistrement en fait
  # deux — un grep -v laissait l'en-tête orphelin de la ligne rejetée.
  jq -r --arg hk "$HOUSEKEEPING" --arg ab "$AGENT_BANNER" --arg u "$until" '.[]|select(.user.login|test("dependabot")|not)
             |select(.body|length < '"$MAXLEN"')
             |select($u == "" or .created_at < $u)
             |select(.body|test($hk) or test($ab)|not)
             |"  #\(.issue_url|split("/")|last) \(.created_at|.[0:16]) <\(.html_url)>
    \(.body|gsub("
";" "))"' <<<"$comments" | grep . || echo "  (aucune)"

  echo "### Filet B — commentaires adressés à un agent (toute longueur)"
  jq -r --arg u "$until" '.[]|select($u == "" or .created_at < $u)
             |select(.body|test("@myia-(ai-01|po-20[0-9]{2})"))
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
  # ⚠️ Fenêtre AVANT la tranche .[0:60] (03/10) : la tranche sur données non
  # fenêtrées glisse au fil des créations — #1293 finirait par en sortir et le
  # self-test rougirait pour rien. Fenêtrer d'abord fige le jeu candidat.
  jq -r -n --arg s "$since" --arg u "$until" --arg cl "$CLUSTER_LOGINS" '[inputs]|add
             |($cl|split(",")) as $cluster
             |[.[]|select(.pull_request==null)]
             |map(select(.created_at > $s and ($u == "" or .created_at < $u)))
             |.[0:60]|.[]|. as $it
             |select(($it.body // "")|ascii_downcase|test("agent `myia|coordinator ai-01|⚠️ agent|ouverte par l.agent")|not)
             |select((($cluster|index($it.user.login))==null) or ((($it.body // "")|length) < 1200))
             |"  #\($it.number) \($it.created_at|.[0:16]) [\($it.user.login)] \($it.title)\n    \($it.html_url)"' \
    <<<"$issues" | grep . || echo "  (aucune)"

  echo
  echo "### Filet D — commentaires d'un auteur hors cluster (toute longueur)"
  # L'angle mort de A était la LONGUEUR (proxy), celui de B l'ADRESSE explicite,
  # celui de C le fait que l'issue soit ouverte. D ne regarde qu'une chose :
  # l'auteur n'est ni le token partagé (jsboige), ni une machine myia-*, ni un
  # bot — donc c'est un humain qu'on n'a pas déjà entendu, et sa demande mérite
  # d'être vue même longue, même sur une PR, même sur une issue fermée.
  d_selector "$until" <<<"$comments" | grep . || echo "  (aucun)"
}

self_test() {
  # Fenêtre FIGÉE des deux côtés (03/10) : les deux demandes humaines connues
  # de #802 (26/08) et l'issue externe #1293 (05/09, 2600 car.) — tout ce qui
  # est créé après le 06/09 n'entre JAMAIS dans ce test, sinon chaque rapport
  # d'agent récent le fait dériver (rouge du 30/09 : #1677, filet A).
  local out rc=0
  out="$(scan 2026-08-26T14:00:00Z 2026-09-06T00:00:00Z)"
  grep -q "Revue du deck tarot anglais"      <<<"$out" || { echo "FAIL: demande humaine brève manquée"; rc=1; }
  grep -q "on a acté avec Thomas et Adeline" <<<"$out" || { echo "FAIL: demande humaine adressée manquée"; rc=1; }
  # contrôle inverse : le filet A ne doit pas ramasser le ménage d'agent
  local a; a="$(sed -n '/Filet A/,/Filet B/p' <<<"$out")"
  grep -qiE 'Superseded|Dispatché|Rebase sur' <<<"$a" && { echo "FAIL: ménage d'agent capté par le filet A"; rc=1; }
  # Contrôle inverse du discriminateur bannière (18/09) : la demande humaine
  # #802 26/08 mentionne une machine EN PROSE (« @myia-ai-01 on a acté… »)
  # sans bannière crochetée. Le filet A doit la garder — si cette assertion
  # devient rouge, AGENT_BANNER a dégénéré en « match nom de machine nu » et
  # avale des demandes humaines : le faux négatif, sens dangereux.
  grep -q "on a acté avec Thomas et Adeline" <<<"$a" || { echo "FAIL: bannière agent avale une demande humaine mentionnant une machine (filet A)"; rc=1; }
  # Contrôle local du pattern bannière (19/09, retour ai-01) : les corps des
  # workers portent « (worker lane, `myia-po-2024`) » — la virgule doit
  # matcher, pas seulement la forme fermée « (worker lane) ». Assertion
  # unitaire : la fenêtre GitHub du self-test ne contient pas la forme
  # étendue (postérieure au 26/08), on teste donc le pattern lui-même.
  grep -qE "$AGENT_BANNER" <<<"— po-2024 (worker lane, \`myia-po-2024\`) fin de session" \
    || { echo "FAIL: bannière agent ne matche pas la forme étendue « (worker lane, …) »"; rc=1; }
  grep -qE "$AGENT_BANNER" <<<"— po-2023 (worker lane) tick :41" \
    || { echo "FAIL: bannière agent ne matche plus la forme fermée « (worker lane) »"; rc=1; }
  # Contrôle du pied italique (03/10) — l'incident réel : #1677 c.5917959959
  # (30/09), rapport court sans crochet ni « (worker lane », signé « *po-2024* »,
  # capté par le filet A dont il a fait rougir ce self-test. Le pied ASTÉRISQUÉ
  # doit matcher ; le nom nu SANS astérisques ne doit PAS (faux négatif = le
  # sens dangereux : une demande humaine qui cite une lane en prose reste vue).
  grep -qE "$AGENT_BANNER" <<<'## Nouvelle tête (rebase sur abc123) — les deux BOM retirés, le diff se réduit aux 2 fichiers.

*po-2024*' \
    || { echo "FAIL: pied de signature « *po-2024* » non reconnu comme bannière"; rc=1; }
  grep -qE "$AGENT_BANNER" <<<"signé po-2024 en prose, sans astérisques" \
    && { echo "FAIL: nom de lane NU (sans astérisques) capté comme bannière"; rc=1; }
  # Contrôle de la borne haute (03/10) : aucune ligne d'en-tête du scan ne doit
  # porter une date postérieure à UNTIL — si cette assertion rougit, la fenêtre
  # s'est rouverte côté droit et le test dérive à nouveau avec le temps.
  grep -qE '^  #[0-9]+ 2026-(09-0[6-9]|09-[12][0-9]|10-[0-9]{2})T' <<<"$out" \
    && { echo "FAIL: la fenêtre du self-test n'est pas bornée côté droit (entrée postérieure au 06/09 visible)"; rc=1; }
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

  # ---- Filet D (07/10, DoD c.6005102887) -------------------------------------
  # (a) Contrôle UNITAIRE du sélecteur sur un jeu SYNTHÉTIQUE — indépendant de
  #     toute fenêtre GitHub : un long commentaire d'`addinette` sort, un long
  #     rapport publié sous `jsboige` ne sort pas, un worker et un bot non plus.
  #     Le corps d'addinette dépasse VOLONTAIREMENT le plafond de A (750 + 56
  #     car. de texte utile) : le contrôle prouve que D ne regarde pas la
  #     longueur, et le marqueur « lesinne » est dans les 300 premiers caractères
  #     parce que D tronque à 300.
  local fix du long_body
  long_body="concernant la tache de comparaison : lesinne --> lesine. $(printf 'a%.0s' $(seq 1 750))"
  fix='[
    {"user":{"login":"addinette","type":"User"},"created_at":"2026-10-06T10:00:00Z","issue_url":"https://api.github.com/repos/o/r/issues/1781","html_url":"https://github.com/o/r/issues/1781","body":"__LONG__"},
    {"user":{"login":"jsboige","type":"User"},"created_at":"2026-10-06T10:01:00Z","issue_url":"https://api.github.com/repos/o/r/issues/458","html_url":"https://github.com/o/r/issues/458","body":"Rapport long publie sous le login de l owner par un agent du cluster : il ne doit PAS sortir du filet D."},
    {"user":{"login":"myia-po-2023","type":"User"},"created_at":"2026-10-06T10:02:00Z","issue_url":"https://api.github.com/repos/o/r/issues/458","html_url":"https://github.com/o/r/issues/458","body":"Rapport de worker, long lui aussi."},
    {"user":{"login":"dependabot[bot]","type":"Bot"},"created_at":"2026-10-06T10:03:00Z","issue_url":"https://api.github.com/repos/o/r/issues/1633","html_url":"https://github.com/o/r/issues/1633","body":"Bump de dependance."}
  ]'
  fix="${fix/__LONG__/$long_body}"
  du="$(d_selector "" <<<"$fix")"
  grep -q "\[addinette\]" <<<"$du" || { echo "FAIL: filet D ne voit pas un commentaire LONG d'auteur hors cluster (unitaire)"; rc=1; }
  grep -q "lesinne"       <<<"$du" || { echo "FAIL: le marqueur du commentaire long n'est pas rendu (troncature à revoir)"; rc=1; }
  grep -q "\[jsboige\]"   <<<"$du" && { echo "FAIL: filet D ramasse un rapport publié sous jsboige (unitaire)"; rc=1; }
  grep -q "myia-po-2023"  <<<"$du" && { echo "FAIL: filet D ramasse un worker du cluster (unitaire)"; rc=1; }
  grep -q "dependabot"    <<<"$du" && { echo "FAIL: filet D ramasse un bot (unitaire)"; rc=1; }
  # (b) le MÉCANISME du trou, à l'unitaire : ce même corps dépasse le plafond
  #     du filet A — c'est exactement pourquoi les deux commentaires d'Adeline
  #     du 06/10 (865 et 1440 car.) lui ont échappé. A ne peut PAS le voir par
  #     construction ; D le voit par (a). Déterministe, sans dépendre du quota.
  jq -e --argjson m "$MAXLEN" 'length < $m' <<<"\"$long_body\"" >/dev/null \
    && { echo "FAIL: le corps de contrôle ne dépasse pas le plafond du filet A — le contrôle ne prouve plus rien"; rc=1; }
  # (c) Fenêtre FIGÉE contenant #1781 — les faits MESURÉS du 06/10 : les
  #     commentaires d'Adeline y sont, dont les longs (1440 et 865 car.), et le
  #     rapport long de myia-ai-01 (5425 car. sur #458, 06/10 11:01) y est.
  #     Borne haute 01:00Z le 07/10 : elle inclut le commentaire de 00:03
  #     (`jsboigeEpita`, cf. témoin (f) ci-dessous).
  local outd dsec
  outd="$(scan 2026-10-05T00:00:00Z 2026-10-07T01:00:00Z)"
  dsec="$(sed -n '/Filet D/,$p' <<<"$outd")"
  # (d) le commentaire LONG RÉEL (1440 car.) est vu par D — repère « lesinne »,
  #     choisi dans les 300 premiers caractères du corps réel (D tronque).
  grep -q "lesinne" <<<"$dsec" || { echo "FAIL: commentaire long hors cluster manqué (filet D, #1781)"; rc=1; }
  # (e) contrôle INVERSE : un rapport LONG du cluster (myia-ai-01, 5425 car.)
  #     ne sort pas de D — D n'est pas « tout ce qui est long ». Et le bot reste
  #     dehors (dependabot a commenté dans la fenêtre).
  grep -q "\[myia-ai-01\]" <<<"$dsec" && { echo "FAIL: filet D ramasse un rapport d'agent du cluster"; rc=1; }
  grep -q "dependabot"    <<<"$dsec" && { echo "FAIL: filet D ramasse un bot"; rc=1; }
  # (f) TÉMOIN de la contrepartie déclarée (cf. tête du script) : `jsboigeEpita`
  #     sort de D aujourd'hui — il porte à la fois la demande externe #1293 (que
  #     C ne détecte QUE parce qu'il est hors cluster) et des rapports po-2023.
  #     Si on l'entre un jour dans CLUSTER_LOGINS, cette assertion ROUGIT : le
  #     rouge nommera ce qu'on achète (le silence sur #1293 côté C).
  grep -q "\[jsboigeEpita\]" <<<"$dsec" || { echo "FAIL: le comportement déclaré de jsboigeEpita a changé — relire la contrepartie en tête de script"; rc=1; }
  [ $rc -eq 0 ] && echo "OK — l'organe voit les 3 demandes humaines, rejette le ménage d'agent (crochets, worker lane, pied italique), fenêtre figée, et crie quand l'API tombe ; filet D : auteur hors cluster vu à toute longueur (unitaire + #1781 réel), rapports du cluster et bots rejetés, contrepartie jsboigeEpita épinglée"
  return $rc
}

case "${1:-}" in
  --self-test) self_test ;;
  "")          scan "$(date -u -d '26 hours ago' +%Y-%m-%dT%H:%M:%SZ)" ;;
  *)           scan "$1" "${2:-}" ;;
esac
