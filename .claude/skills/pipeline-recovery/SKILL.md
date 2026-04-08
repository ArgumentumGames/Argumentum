---
name: pipeline-recovery
description: Entree de session pour la recovery du pipeline Argumentum. Lit le dashboard workspace (coordination ai-01 / po-2023), fait le point git, identifie la prochaine etape, et rapporte en fin de session. A utiliser au demarrage de toute nouvelle conversation sur le projet.
---

# Skill : Pipeline Recovery

Tu reprends le travail de reconstruction du pipeline Argumentum. Ce skill est l'ENTREE DE SESSION : il synchronise l'etat (dashboard + git + checkpoint), identifie la prochaine tache, et garantit qu'en fin de session le travail est rapporte sur le dashboard workspace.

Deux agents collaborent sur ce projet :
- **ai-01** (cette machine) : validation visuelle, debug interactif, merges, publication
- **po-2023** (machine distante) : pipelines lourds, builds multi-langues, taches mecaniques

La coordination passe par le **dashboard workspace** du MCP `roo-state-manager`.

---

## Workflow obligatoire

### Phase 0 — Lecture du dashboard (OBLIGATOIRE, tout debut de session)

```
roosync_dashboard(action: "read", type: "workspace")
```

Extraire du resultat :
1. **Status** (section `status.markdown`) : etat actuel du pipeline
2. **Intercom** (section `intercom.messages`) : messages recents par ordre chronologique
3. **Messages non traites** de po-2023 : bugs signales, taches completees, questions ouvertes (tags `ASK`, `BLOCKED`, `BUG`)
4. **Derniere action ai-01** : verifier qu'on ne fait pas doublon

**Si po-2023 a pose une question (tag `ASK`)** → y repondre via `append` avec tag `REPLY` avant de continuer.
**Si po-2023 a signale un bug (`BUG`/`BLOCKED`)** → l'integrer dans le plan de la session.

### Phase 1 — Etat git + sync

```bash
git fetch origin
git status
git log --oneline -10
gh pr list --state open --limit 20
```

- Si `behind` : `git pull --ff-only origin master`
- Si PRs ouvertes non-dependabot : les review en priorite
- Si fichiers non commites non attendus : investiguer avant d'agir

### Phase 2 — Lecture du contexte projet

Lire dans l'ordre :
1. `CLAUDE.md` (racine) : regles projet, leçons apprises, etat recovery
2. `.claude/pipeline-recovery-checkpoint.md` : snapshot de la derniere session (si existe)
3. Memoire : `C:\Users\MYIA\.claude\projects\d--Argumentum\memory\MEMORY.md` (chargee auto)

### Phase 3 — Diagnostic des outputs

Verifier l'etat des artefacts generes :

```bash
# PDFs par langue
ls Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/{fr,en,ru,pt}/Documents/density-0/

# Mind maps SVG
ls Cards/Fallacies/Mindmaps/{fr,en,ru,pt}/

# OWL
ls docs/ontology/ 2>/dev/null
```

Ne pas regenerer si les fichiers existent deja et que le dashboard rapporte le cycle comme complet.

### Phase 4 — Identification de la prochaine tache

Priorites par ordre decroissant :
1. **Bugs actifs** rapportes par po-2023 sur le dashboard
2. **PRs en attente** de review
3. **Validation visuelle** des documents generes (prerequis publication)
4. **Issues ouvertes prioritaires** : #183 (traduction), #134 (release), #140 (QA multilingue), #131/#132 (DNN)
5. **Nettoyages** : dependabot, stale branches

Si la session est bloquee sur une decision → poster `ASK` sur le dashboard et s'arreter.

### Phase 5 — Execution

Selon la nature de la tache :
- **Validation visuelle** : utiliser Playwright + screenshots, delegation restreinte a ai-01
- **Corrections code** : utiliser `pipeline-fix` skill
- **Cycle complet valide+fix** : utiliser `pipeline-iterate` skill
- **Taches compute-intensive** : deleguer a po-2023 via message dashboard (tag `TASK`), ne pas executer localement

**Regle po-2023** : ne jamais lancer de regeneration lourde sans coordination prealable. La validation visuelle reste ai-01.

### Phase 6 — Commits + PR

Pour toute modification de code :
1. Branche feature : `git checkout -b <type>/<scope>`
2. Commits conventionnels avec `Co-Authored-By: Claude Opus 4.6 <noreply@anthropic.com>`
3. PR via `gh pr create` — jamais de push direct sur master
4. Review + merge apres validation (tests, build)

### Phase 7 — Rapport de fin de session (OBLIGATOIRE)

**Aucune exception** : toute session doit se terminer par un message sur le dashboard workspace.

```
roosync_dashboard(
  action: "append",
  type: "workspace",
  tags: ["DONE"],
  content: "## [YYYY-MM-DD HH:MM] ai-01 — <titre>\n\n### Actions\n- ...\n\n### PRs/commits\n- ...\n\n### Bloqueurs / handoff po-2023\n- ...\n\n### Prochaine etape\n- ..."
)
```

Si un PR a ete cree + merge pendant la session, l'annoncer AVANT de poster le DONE (ordre : commit → push → merge → dashboard).

Si le status global du pipeline a change, aussi mettre a jour :
```
roosync_dashboard(action: "write", type: "workspace", content: "<nouveau status markdown>")
```

---

## Etat recovery (Avril 2026)

Pipeline operationnel sur les 4 langues. Artefacts generes :
- 82 PDFs (FR/EN/RU/PT)
- 4502 images
- 20 SVGs mind maps (FreeMind Batik)
- OWL ontology 664 KB avec SKOS + AIF
- 27/27 tests pass

**Issues en cours** : #119 (layout Rules), #131/#132 (DNN), #133 (OWL publication), #134 (GitHub release), #140 (QA multilingue), #183 (upgrade SDK traduction + Virtues i18n).

**Prochaine grande etape** : validation complete de TOUS les documents generes avant publication.

---

## Rappels critiques

- `SkipConfigFile` doit etre `true` dans `AssetConverterConfig.cs` (tuple serialization casse Translations)
- **CSV** : ne JAMAIS modifier avant injection dans CardPen
- **rscount** : ne JAMAIS forcer a 0 dans le code C#
- **CsvType** : obligatoire dans chaque DataSet pour harvesting
- **QuestPDF non thread-safe** : lock global, ne pas paralleliser
- **Validation visuelle** : toujours cote ai-01 (Playwright + vision), jamais deleguee a po-2023
- **Ordre dashboard** : commit + PR AVANT de poster DONE. Ne jamais annoncer un travail non commite.
