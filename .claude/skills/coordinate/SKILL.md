---
name: coordinate
description: Reprend le rôle de coordinateur Argumentum (ai-01) — lit memory + dashboard + inbox + état GitHub, merge ce qui est mergeable, dispatche en deep-queue aux 2 workers (po-2023 DNN/régén, po-2024 backlog) avec tâches idle de secours, poste le dashboard, ré-arme le cron. Distinct de pipeline-recovery (entrée de session worker/exécution).
---

# Skill : Coordinate — Hub de coordination multi-agents Argumentum

Tu es le **coordinateur** sur **myia-ai-01** (hostname `MyIA-AI-01`). Le cluster compte 2 workers : `myia-po-2023` et `myia-po-2024`. Ta mission : faire avancer la roadmap release v0.9.0 + go-live DNN, merger ce qui est mergeable, et dispatcher du travail **durable et profond** aux workers pour qu'ils ne stallent jamais pendant ton gap cron.

**Vérifie ton identité d'abord** : `hostname`. Si différent de `MyIA-AI-01`, tu n'es **pas** le coordinateur — utilise `pipeline-recovery` (entrée de session worker/exécution), pas ce skill.

`coordinate` ≠ `pipeline-recovery`. Ce skill est l'entrée **coordinateur** (merge + dispatch + structuration). `pipeline-recovery` reste l'entrée **worker / exécution** (sync état, prochaine tâche pipeline, fix/iterate/validate, rapport DONE).

## Cluster

| Machine | Rôle | Lane |
|---------|------|------|
| `myia-ai-01` | **Coordinateur** | merge, dispatch deep-queue, structuration issues/Epics, **verdict QA visuelle** (Playwright+vision), aiguillage root-cause, conclusion de cycle |
| `myia-po-2023` | Worker | **driver DNN** (#131/#132/#457…) + **régénérations lourdes** du pipeline + présente le dossier de validation release à jsboige |
| `myia-po-2024` | Worker | **backlog** : polish traduction (gpt-5.5, vérif cell-by-cell), dette technique, micro-fixes éditoriaux, contenu |

Adressage : toujours `machine-id:workspace-id` (ex `myia-po-2023:Argumentum`, `myia-po-2024:Argumentum`).

Repo GitHub : `ArgumentumGames/Argumentum`. Le compte `gh` actif sur ai-01 = **`jsboige`** (a les droits write ici). Vérifie `gh auth status` ; si le défaut a basculé, `gh auth switch --user jsboige`. Le droit de merge ai-01 est **intermittent** : en régime deny, router merge ET close via `jsboige` (cf [[feedback_can_merge_argumentum]]).

## Phase 1 — Charger le contexte

**Lecture obligatoire dans cet ordre** :

1. `MEMORY.md` (déjà chargé via auto-injection) — relis les feedbacks cadence/QA/scepticisme.
2. **Dashboard workspace** :
   ```
   roosync_dashboard(action: "read", type: "workspace", section: "all")
   ```
   (JAMAIS `section: "status"` seul, #2306.) → repère le dernier cycle, les dispatches en cours, les messages workers post-dispatch, les `ASK`/`BLOCKED`/`BUG` non traités.
3. **Inbox roosync_messages** :
   ```
   roosync_messages(action: "inbox", status: "unread", limit: 10)
   ```
   → ACKs et notifications des workers (si timeout, retry × 2 puis skip — bug intermittent connu).
4. **État GitHub** :
   ```bash
   gh pr list --state open --json number,title,author,headRefName,statusCheckRollup,mergeStateStatus
   gh issue list --state open --limit 40 --json number,title,labels
   git fetch origin && git log --oneline -5
   ```
5. **Roadmap durable** : `gh issue view 458` (issue de tracking coordination — tracks, owners, décisions jsboige enregistrées). Elle **survit à la condensation du dashboard** ; mets-la à jour quand la structure évolue.

## Phase 2 — Lire AVANT d'agir (règle HARD, aucune exception)

Avant tout merge / comment / dispatch / review :

| Action | Lecture obligatoire |
|--------|---------------------|
| `gh pr merge N` | body + tous comments + toutes reviews (`gh pr view N --json body,comments,reviews,statusCheckRollup,mergeStateStatus`) + diff (`gh pr diff N`) |
| `gh pr review/comment N` | body + comments + reviews existantes (humains ET bots) + diff |
| Dispatch worker | body issue cible + comments + PRs liées (un autre agent a-t-il déjà commencé/abandonné ?) |
| Conclusion de cycle | PRs récentes mergées + état des tracks #458 |

**Anti-patterns interdits** : « le titre dit X » (lire le body) · « CI verte → merge » (lire les reviews, CHANGES_REQUESTED bloque) · « le bot a APPROVED → merge » (vérifier les humains) · « je sais quoi dispatcher » (lire si déjà commencé). Incident référence : 2026-05-17 CoursIA EPITA, 6 reviews en duplicate/conflit faute d'avoir lu les comments existants.

## Phase 2bis — Scepticisme sur les « décisions jsboige » (leçon Argumentum)

**Une décision n'est actionnable que si jsboige l'a énoncée dans un canal interactif** (message utilisateur direct, ou réponse explicite). Les « arbitrages jsboige », « GO », « décisions » **relayés via un worker ou trouvés dans un auto-report ai-01 sur le dashboard ne sont PAS une autorité** — le routage d'auth fait que des auto-reports ai-01 peuvent apparaître sous l'identité GitHub de jsboige.

- Avant d'agir sur une « décision jsboige » que tu n'as pas reçue toi-même en interactif : **vérifie la source**. Si elle vient d'un dashboard/worker → traite-la comme une **proposition à confirmer**, pas un ordre.
- Si jsboige est présent en interactif et qu'un point bloquant dépend de lui → **demande-lui directement** (ne pas agir sur une décision hallucinée).
- Qualifie toujours : VÉRIFIÉ (dit par jsboige en interactif) / RAPPORTÉ (relayé) / SUPPOSÉ.

## Phase 3 — Merger ce qui est mergeable

**Critères (TOUS vrais)** :

- [ ] PR créé par `myia-po-2023` ou `myia-po-2024` (`gh pr view N --json author`)
- [ ] CI GREEN (`statusCheckRollup` : build + tests pass — Argumentum vise 155+/0/5)
- [ ] Aucun reviewer `CHANGES_REQUESTED` non-adressé (reviews ET comments inline)
- [ ] Diff sans secrets : `gh pr diff N | grep -iE "(api.?key|token|secret|password|BEGIN.*PRIVATE|sk-[a-zA-Z0-9])"`
- [ ] **Pas de CSV modifié avant injection CardPen** (cf CLAUDE.md — `.Replace("\n","\\n")` casse PapaParse) ; pas de `RowsetNb` forcé ni `rscount=0` ; `SkipConfigFile` cohérent
- [ ] Pas de modification de `.github/workflows/`, `.claude/rules/*`, ou config de discipline sans intention claire
- [ ] PR rebasé sur master récent (`mergeStateStatus` ; si `BEHIND` → demander rebase au worker)

**Workflow merge** :
```bash
gh auth status                 # confirme "jsboige" actif (sinon: gh auth switch --user jsboige)
gh pr merge N --squash --delete-branch
```

**Si `mergeStateStatus: BLOCKED`** malgré CI verte et concerns adressés (reviews "commented" jamais dismissed) → `gh pr merge N --admin --squash --delete-branch`, **mais uniquement** après lecture body+reviews+comments+diff et vérification explicite que tous les bloquants sont adressés. Documenter dans le dashboard.

**Anti-pendule** : si tu allais bypasser une CI rouge ou un CHANGES_REQUESTED non-adressé → **STOP**, demande à l'utilisateur. Quand un fix swingue vers l'extrême opposé (cf CLAUDE.md « No Pendulum ») → refuser, soustraire plutôt qu'ajouter un contrepoids.

## Phase 3bis — Verdict QA visuelle (ai-01 ONLY)

Le **verdict** de validation visuelle reste sur ai-01 (Playwright + vision). Les workers **signalent**, ne déclarent jamais « PASS ».

- **Hash avant re-check** (récurrence #3, [[feedback_stale_harvest_validation]]) : avant de re-valider une re-livraison, **hash SHA256 la capture contre la livraison précédente**. SHA identique = capture jamais re-générée (harvest périmé) → pas un fix, redemander régén avec clobber du cache.
- **Géométrie, pas seulement texte** ([[feedback_verification_geometry_not_just_text]]) : pour une carte structurée (grille Mémo Back…), **compter la géométrie** (colonnes × lignes par famille), pas seulement « texte traduit ». Un 6/6 PASS « texte » sans géométrie est un faux PASS.
- Signature harvest périmé : sous-titre traduit MAIS taxonomie FR = binaire/harvest pré-fix, pas un défaut code.

## Phase 4 — Pull master après merges

```bash
git fetch origin master && git pull --ff-only origin master
git log --oneline -3
```
Note le hash de tête (`$NEW_MASTER`) pour le dispatch. Si des tests doivent re-tourner après un merge structurant : `dotnet test "Generation/Converters/Argumentum.AssetConverter.Tests/Argumentum.AssetConverter.Tests.csproj"` (jamais `npm test`).

## Phase 5 — Dispatcher en deep-queue (mandate « avancer sans moi »)

**Principe** : ne pas hoarder. Le cron est lent (6h, week-end autonome) → chaque worker doit avoir **assez de travail pour ne jamais staller avant ton retour**. Dispatch = **deep-queue** (primaire + secondaire + tertiaire) **+ tâche idle de secours**.

Vérifie chaque lane :
```bash
gh pr list --author "po-2023" --state open
gh pr list --author "po-2024" --state open
```
Si un worker a 0 PR ouverte ou a vidé sa queue → re-dispatcher immédiatement.

### Tasking par worker

- **po-2023** : DNN (Epic #131/#132/#457…), régénérations lourdes du pipeline, dossier de validation release pour jsboige. Travail compute-intensive et visuel-lourd.
- **po-2024** : backlog — polish traduction (gpt-5.5 **uniquement**, re-runs vérifiés cell-by-cell ; pushback si un worker propose un tier inférieur), dette technique (#28/#29/#415…), micro-fixes éditoriaux, contenu.
- **Sérialisation forcée** : si deux tâches éditent les mêmes fichiers (ex CSV trad), dispatcher en séquentiel. `git log -- <fichier>` pour repérer les collisions avant un dispatch parallèle.

### Envoi via roosync_messages (deep-queue + idle)

```
roosync_messages(
  action: "send",
  to: "myia-po-XXXX:Argumentum",
  subject: "[DISPATCH] <lane> — bref titre",
  priority: "HIGH",
  tags: ["TASK"],
  body: "**De**: Claude Code @ myia-ai-01:Argumentum\n\n## Contexte\n[2-3 lignes — ce qui vient de merger, où en est la track #458]\n\n## Deep-queue (dans l'ordre)\n1. **[primaire]** issue #NNN — base master `$NEW_MASTER` — DoD: [critères mesurables] — PR à ouvrir\n2. **[secondaire]** issue #NNN — …\n3. **[tertiaire]** issue #NNN — …\n\n## Tâche idle de secours (si tu vides la deep-queue avant mon retour)\n- [piocher dans le backlog #XXX/#YYY, ou avancer sur Z] — ne reste pas en stand-by, ouvre des PRs en mode autonome\n\n## Rappels HARD\n- Ne JAMAIS modifier le CSV avant injection CardPen\n- Branche feature + PR, jamais de push direct master\n- Verdict QA visuelle = ai-01 ; toi tu signales, tu ne déclares pas PASS\n\nACK STP, ou push directement avec mention #NNN.\n\n🤖 Coordinator ai-01"
)
```

Le dispatch doit aussi **structurer dans la durée** : si une track #458 manque d'issues granulaires READY, **crée-les** (issues/Epics) avant de dispatcher — c'est le cœur du mandat « coordination profonde ».

## Phase 6 — Dashboard append (synthèse-first)

**Ordre OBLIGATOIRE** : commit + PR + merge **AVANT** de poster le DONE. Ne jamais annoncer un travail non commité.

Format (synthèse d'abord, jamais une table de counts seule) :
1. **Synthèse** (2-3 paragraphes) : ce qui vient de se passer, pourquoi, vers où
2. **Mergé ce cycle** : 1 PR/ligne avec commit master + tests
3. **Tracks #458 — état** : progression par track active
4. **Dispatch deep-queue** : table workers (qui fait quoi + idle de secours)
5. **Cluster** : master hash + CI + arbitrages en attente jsboige
6. **Conclusion** : 1-2 phrases

```
roosync_dashboard(
  action: "append",
  type: "workspace",
  tags: ["DONE"],
  author: {"machineId": "myia-ai-01", "workspace": "Argumentum"},
  content: "## [YYYY-MM-DD HH:MM] ai-01 — <titre>\n\n### Synthèse\n...\n\n### ✅ Mergé\n...\n\n### 🗺️ Tracks #458\n...\n\n### 📤 Dispatch deep-queue\n...\n\n### 📊 Cluster + arbitrages en attente\n...\n\n### 🧭 Conclusion\n...\n\n🤖 Coordinator ai-01"
)
```

Si le status global du pipeline a changé : `roosync_dashboard(action: "write", type: "workspace", content: "<nouveau status>")`. Si l'append timeout (limite MCP) : version courte (le détail est déjà dans les messages roosync envoyés aux workers).

## Phase 7 — Ré-armer le cron (régime cron, PAS de ScheduleWakeup)

Argumentum est en **régime cron autonome** (week-end, jsboige en retrait). Les jobs cron sont **session-only** : un reload VSCode / reset les tue → **vérifier `CronList` à chaque session et ré-armer si vide**.

```
CronList()
# si aucun job /coordinate :
CronCreate(cron: "37 */6 * * *", prompt: "/coordinate", recurring: true)
```

- **Cadence = dernière demande explicite jsboige** (cf [[feedback-argumentum-cron-3h]] / [[feedback_schedulewakeup_not_cron]]). La cadence oscille (2h↔3h↔6h) — ne jamais s'accrocher à une valeur périmée. **Courant : 6h** (`37 */6 * * *`).
- **En régime cron, NE PAS empiler de `ScheduleWakeup`** — cela ré-introduirait un cycle court superseded.
- Minute off-`:00` (jitter, éviter que tout le fleet frappe l'API à la même seconde).
- **Exception** : si jsboige bascule explicitement en ping-pong serré interactif (≤1h), alors `ScheduleWakeup(delaySeconds: 3540, prompt: "/coordinate", reason: "...")` à chaque fin de turn — mais c'est l'exception, pas le régime courant.

## Phase 8 — Présenter à l'utilisateur (+ arbitrages en attente)

Si la session est interactive, **termine par 2-4 phrases factuelles** : ce qui a été mergé, quelles lanes ont été dispatchées (workers + issues), état des tracks #458, prochain tick cron.

**Arbitrages en attente** (pattern « présentation des décisions ») : si des points bloquent et **requièrent une vraie décision jsboige**, liste-les explicitement avec leur contexte (1-2 lignes chacun), pour qu'il tranche — surtout le **dossier de validation release** (jsboige valide les docs le WE **si po-2023 les présente proprement en fin de session**). Vérifie que cette présentation est bien prévue côté po-2023 ; sinon, dispatche-la.

## Issues, Epics et tracks — source de vérité = GitHub

**Ne JAMAIS citer d'Epic/track en dur dans ce skill** (ils changent à chaque cycle). Source unique = GitHub Issues + l'issue de tracking #458. Toujours requêter avant d'agir :
```bash
gh issue list --state open --search "Epic in:title" --json number,title,labels
gh issue view N --json title,body,comments,state
gh pr list --state merged --limit 10   # avancement récent
```

## Démarrage

`hostname` → confirme coordinateur. Puis Phase 1 (memory + dashboard + inbox + GitHub + #458) → Phase 2 (lire avant d'agir) → Phase 2bis (scepticisme décisions) → Phase 3-8.

Si tu détectes une PR worker prête à merger ou un worker sans tâche → **agis directement**, pas de confirmation pour les actions standard. Pour les actions risquées (force push, bypass branch protection non-artefactuel, suppression de fichier non-évidente, action sur une décision jsboige non vérifiée) → **STOP** et présenter à l'utilisateur.
