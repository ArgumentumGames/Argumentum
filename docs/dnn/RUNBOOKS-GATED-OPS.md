# Runbooks gated-ops — Index (pré-armement au GO jsboige)

> **But** : au GO jsboige, exécution des ops gated = **copier-coller, zéro réflexion**. Cet index consolide les **6** runbooks gated-ops DNN — compte dérivé de la table de mesure ci-dessous.
> **Contrainte** : rien d'exécuté sans GO jsboige explicite. DNN publish écrase `web.config` prod — ne pas publier. Verdict QA visuelle = ai-01.
> **Auteur** : po-2023 (dispatch ai-01 `msg-20260722T144728-h7msce`) · critère d'appartenance : arbitrage ai-01 `msg-20260912T210143-7y49cx`, mesuré po-2024 le 13/09/2026.

## Critère d'appartenance (les 2 conditions, pas l'une)

Un **runbook gated-ops DNN** = une procédure dont l'exécution **exige un GO owner** ET **opère un geste d'administration ou de mutation sur une machine servante** (prod ou préprod).

Deux précisions que la mesure du 13/09 a imposées (les 9 documents runbook-type ouverts un par un — table de mesure ci-dessous) :

- « Toucher » s'entend au sens **administratif** : une procédure de **lecture client** (navigation, capture, comparaison visuelle) ne compte pas.
- Une **machine de dev** (localhost, LocalDB, IIS Express) n'est pas une machine servante.

---

## Index des runbooks

| # | Runbook | Couverture | Statut |
|---|---------|-----------|--------|
| 1 | **Apply Manifests Δ #490/#682** | [`go-live-turnkey-checklist.md`](go-live-turnkey-checklist.md) | ✅ existant (prêt) — B4 (prod VPS go-live) en attente du GO |
| 2 | **Option C connection-string** | [`option-c-connection-string-runbook.md`](option-c-connection-string-runbook.md) | ✅ **APPLIQUÉ + CERTIFIÉ** (17/07) — cf. note doublon ci-dessous |
| 3 | **Fix skin `tabid=138` (Opt 1)** | [`skin-tabid138-diagnostic-runbook.md`](skin-tabid138-diagnostic-runbook.md) | ⚠ **diagnostic-only** (fix diff deferred au GO, nécessite stack trace live) |
| 4 | **Redéploiement mindmaps servi (#830)** | [`redeploy-mindmaps-runbook.md`](redeploy-mindmaps-runbook.md) | ✅ existant (prêt) — ops serveur seul, geste additif fichier par fichier, ⛔ `/MIR` |
| 5 | **Rotation machineKey + scrub** | [`machinekey-rotation-scrub-runbook.md`](machinekey-rotation-scrub-runbook.md) | ⚠ staged — **ZERO exécution sans GO jsboige** (ops server, #415 git-rewrite gated) ; nuance §0.1 dans la table de mesure |
| 6 | **Go-live gate (jour J, top-to-bottom)** | [`go-live-gate-runbook.md`](go-live-gate-runbook.md) | ✅ existant (prêt) — Step A = rotation machineKey (prod `web.config`) |

> **Doublon Option C tranché (13/09/2026)** : deux documents décrivaient la même op. Le **document opérateur** = [`option-c-connection-string-runbook.md`](option-c-connection-string-runbook.md) (GO interactif jsboige 08:52, prod only, **APPLIQUÉ + CERTIFIÉ** 17/07 13:01 — test idle-long ai-01, verdict PASS 13:38). [`dnn-hang-option-c-connection-string-prep.md`](dnn-hang-option-c-connection-string-prep.md) est requalifié **document de préparation** (analyse pré-arbitrage, « Not applied ») et ne paraît plus dans l'index : un opérateur au GO n'a qu'un document à ouvrir. ⚠️ Le runbook référençait un sibling `app-pool-idle-hang-runbook.md` **jamais committé** — référence qualifiée inline le 13/09/2026 (renvoi vers le récap §7 du runbook ; la procédure d'application détaillée n'est pas versionnée).

## Table de mesure — les 9 documents runbook-type passés au critère (13/09/2026)

Mesure, pas déduction depuis le nom de fichier : chaque document a été ouvert et lu.

| Document | GO owner ? | Machine servante ? | Verdict | Motif |
|---|---|---|---|---|
| `go-live-turnkey-checklist.md` | ✓ | ✓ | **IN** (ligne 1) | B4 = prod VPS go-live « awaits the owner GO » ; « requires jsboige's interactive RDP session » ; B1-B3 historiques |
| `option-c-connection-string-runbook.md` | ✓ | ✓ | **IN** (ligne 2) | GO interactif jsboige, prod only, `web.config` — le document opérateur de l'op appliquée et certifiée |
| `skin-tabid138-diagnostic-runbook.md` | ✓ | ✓ | **IN** (ligne 3) | « à exécuter au GO jsboige avec accès serveur VPS » ; le fix aboutit à une mutation (template 2sxc, contenu en DB) — statut diagnostic-only tant que le stack trace live n'est pas capturé |
| `redeploy-mindmaps-runbook.md` | ✓ | ✓ | **IN** (ligne 4) | Ops serveur, geste additif fichier par fichier dans le webroot prod, ⛔ `/MIR`, sentinelles §5 |
| `machinekey-rotation-scrub-runbook.md` | ✓ | ✓ | **IN** (ligne 5) | « ZERO execution without explicit GO from jsboige » ; ops server + #415 git-rewrite. §0.1 : l'urgence compromis (clé exposée) était sandbox-scoped et résolue (mesuré 25/07) — la rotation **prod** reste planifiée comme Step A du runbook 6 |
| `go-live-gate-runbook.md` | ✓ | ✓ | **IN** (ligne 6) | « the single top-to-bottom procedure jsboige executes on go-live day » ; Step A = rotation machineKey sur prod |
| `dnn-hang-option-c-connection-string-prep.md` | — | — | **HORS — préparation** | « Prep-only analysis… for jsboige's arbitration… Not applied » : aucune procédure à exécuter au GO ; périmé par le runbook appliqué ; conservé comme archive de décision |
| `sandbox-bootstrap-runbook.md` | ✗ | ✗ | **HORS famille** | « Bootstrap a **local** DNN sandbox… **without touching production** » : LocalDB + IIS Express `:8090` sur machine de dev po-2023, machineKey throwaway DEV — aucune des deux conditions |
| `visual-diff-runbook.md` | ✗ | ✗ | **HORS famille** | « Lecture seule stricte » : « toute mutation » et « tout login » explicitement hors périmètre, exécutable à tout moment sans GO. C'est ce document qui a imposé la précision « toucher = administrer/muter, pas charger une page » |

Le compte de l'en-tête (**6**) se dérive de cette table — il ne s'asserte plus.

---

## Pré-requis commun aux ops DNN mutantes `web.config` / `bin/` / DB (runbooks 1, 2, 3, 5, 6)

- **Sauvegarde pré-op** horodatée + hashée (`web.config`, `bin/`, DB). Cf `[[reference-dnn-bin-restore-surgical]]` (restore chirurgical, pas rsync brutal).
- **Backup naming hygiene** : ne JAMAIS restaurer un backup sur la base du nom/date seul — **vérifier la connection string + le count de `dependentAssembly` AVANT** (`[[feedback-dnn-webconfig-bak-trap]]`). `web.config.bak-20260717` = pré-migration IIS, trompeur.
- **CS verification** : la connection string actuelle est `localhost\SQLEXPRESS` (correcte). Ne pas restaurer un backup LocalDB par erreur (`[[feedback-dnn-webconfig-secret-tracked]]`).
- **Runtime** : .NET Framework 4.8 (pas .NET 8/9 — les redirects 9.0.0.0 sont BCL 2sxc-21, cf `[[reference-dnn-2sxc-net48-bcl-stack]]`).

> ⚠️ **Le runbook 4 ne relève pas de ces pré-requis.** Le redéploiement mindmaps est une copie de fichiers additive dans le webroot : il ne touche ni `web.config`, ni `bin/`, ni la DB — donc ni connection string ni runtime à vérifier. Ses pré-requis propres (sauvegarde préalable, instantané des **deux sentinelles orphelines**) sont au §3 de [`redeploy-mindmaps-runbook.md`](redeploy-mindmaps-runbook.md).

---

## Décisions jsboige en attente (bloqueurs DNN)

1. **Rotation machineKey** (urgent — clés exposées sur branche feature `4b0297ee`). Ops serveur seul.
2. **Skin `tabid=138` Opt 1/2** (reco Opt 1 = v0.9.1, tag non bloqué).
3. **#681 2sxc App export** (hard-unblocker DNN i18n #682-#685). Contenu vit dans DB 2sxc live, aucun worker ne peut le faire.

---

## Post-exec smoke

[`go-live-smoke-test.md`](go-live-smoke-test.md) — homepage 200, `/Argumentum` + `/Règles` rendent 2sxc content, 0 `JsonOptions`/conn-string error, `tabid=138` (post-fix Opt 1).
