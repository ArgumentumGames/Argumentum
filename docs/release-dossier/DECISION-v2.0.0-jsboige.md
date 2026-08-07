# v2.0.0 — Brief de décision pour jsboige

> **Objet** : ce qu'il reste à décider pour **tagger `v2.0.0`**, synthèse exécutive.
> **Auteur** : Claude Code @ myia-po-2023 (worker), 2026-08-07.
> **Base** : master `7abf4c50`.
> **Statut** : **BRIEF DE DÉCISION** — prépare ta validation week-end. Ne remplace pas les dossiers
> détaillés (indexés §4), il les actualise en 2 pages.

Ce brief ne **tag rien**, ne **publie rien**. C'est le document unique à lire pour décider.

---

## 1. Ce qui a changé depuis ton dernier dossier (07-04)

Le dossier v4.1 ([`docs/RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md)) date du 04/07.
Six faits ont bougé depuis, et **aucun ne dégrade** la release :

| # | Fait | État dossier v4.1 (07-04) | État réel (07-08) |
|---|------|---------------------------|-------------------|
| 1 | **Verdict visuel** | en attente | ✅ **PASS** (ai-01 2026-07-01 : géométrie + contenu + micro-RU résolu) |
| 2 | **Numérotation** | v0.9.0 | ✅ **v2.0.0** décidé (toi, 2026-08-06, #999) |
| 3 | **Tests** | 578 pass | ✅ **638 pass / 0 fail / 5 skip** (master `caefa695`) |
| 4 | **Mindmaps 8 lang** | es/ar/fa/zh BLOCKED | ✅ **shipped** (PR #565, 20 SVGs × 8 lang) + harnais #830 **28/28 vert** |
| 5 | **OWL** | FR only | ✅ **5,13 Mo SKOS+AIF**, #499 Phase 2 (Virtues) merged |
| 6 | **Bundle v3 CMYK** | en cours | ✅ **GDrive `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/`, 80 PDFs DeviceCMYK + SWOP** (post-Ghostscript #632/#652) |

⚠️ Le **dossier README 17-06** ([`docs/release-dossier/README.md`](README.md)) est **superseded et
stale** (mindmaps es/ar/fa/zh « BLOCKED » = faux, OWL « FR only » = faux, tests 359 = faux). Ne t'y
fie pas — ce brief et le v4.1 sont les références vivantes.

---

## 2. La séquence exacte vers le tag `v2.0.0`

Trois verrous, tous chez po-2024, tous **mécaniques et chiffrés**. La régén mindmap (#983) attend la
fin de ces trois-là **puis le feu vert explicite d'ai-01** (porte fixée cycle 45, ne bouge plus).

```
PR A  libellés FR (254 subst. + pk 88 remark_zh)    ── po-2024, DoD dur #1007 FR → 0
PR B  apostrophes EN (102 subst., non ambiguës)      ── po-2024, moitié EN du GO
PR C  #994 (12 exemples + coquille pk 1361)          ── po-2024, diff éditorial SÉPARÉ du mécanique
        │
        ▼  merge des 3  →  feu vert ai-01
#983   régén mindmaps Vertus 8 langues (FreeMind/Batik, worktree master prêt `.prep-983-worktree`)
        │  ⚠️ env var ARGUMENTUM_FREEMIND_PATH = FreeMind.exe obligatoire (sinon DoD SHA-diff fail SILENCIEUX, cf #983 body)
        ▼
verdict visuel ai-01  →  tag v2.0.0 (#999)  →  packaging release (#134)
```

Rien de tout cela ne demande ta présence avant le tag. Tu interviens à la fin.

---

## 3. Tes décisions (week-end)

### D1 — Confirmer le packaging de distribution (#134)
Le bundle v3 existe déjà sur GDrive (80 PDFs CMYK). Trois options pour les rendre téléchargeables :

| Option | Avantage | Coût |
|--------|----------|------|
| **A. GitHub Release assets** (recommandé) | canonique, lien permanent, visible sur le repo | 80 fichiers × ~60 Mo = **4,8 Go** vs limite asset 2 Go/fichier ; OK en plusieurs assets |
| B. git-lfs | versionné avec le code | alourdit le clone ; non pertinent pour des binaires de release |
| C. Lien GDrive externe dans le README | zéro effort, déjà uploadé | non-canonique, dépend de ton compte GDrive |

**Mon appel** : **A** (assets GitHub, groupés par langue ou par type), avec le README qui pointe dessus.
Décision : **toi**.

### D2 — Finding « titre PT Manche »
Sur la cover PT, le titre affiche **« Roll of the English Channel »** (homonyme Manche=Manche de
vêtement) au lieu du PT attendu. Fix prêt chez po-2024 (gated). Deux options :
- **corriger avant le tag** (recommandé — une cellule CSV, fix po-2024 déjà préparé),
- ou **reporter en post-tag** (serait un v2.0.1).

Décision : **toi**.

### D3 — Couplage DNN (#131/#132)
Le site `dnn.argumentum.myia.io` est **LIVE et stable** depuis le fix #131 (07-08, probe
FirstChanceException → `Default.aspx` 10.x). Reco po-2023/ai-01 : **dé-coupler** — la release cards
ship **indépendamment** du go-live DNN (ops VPS = ta tâche ops, pas un gate release).
- Confirmer : **la release v2.0.0 ne gate pas sur le DNN go-live** ?

### D4 — Scope mindmaps (confirmé, peu de marge)
Les mindmaps **8 langues sont déjà shipped** (PR #565). La régén #983 met juste à jour les **libellés
de familles** (apostrophes + #1007). Confirme : **8-lang mindmaps = exigence v2.0.0** (pas 4-lang
différé).

---

## 4. Où trouver le détail (ne pas relire ce brief pour ça)

| Sujet | Document |
|-------|----------|
| Bundle v3, comment valider en 15 min (1 PDF/lang) | [`docs/publication/release-v090-bundle-v3-8lang-verification-guide.md`](../publication/release-v090-bundle-v3-8lang-verification-guide.md) |
| Dossier validation complet (v4.1) | [`docs/RELEASE-VALIDATION-v0.9.0.md`](../RELEASE-VALIDATION-v0.9.0.md) |
| Renumérotation v2.0.0 (tag, milestone, docs) | [issue #999](https://github.com/ArgumentumGames/Argumentum/issues/999) |
| Scénario QA par type de PDF | [`docs/publication/qa-scenario-8langues-release.md`](../publication/qa-scenario-8langues-release.md) |
| Gate checklist (ce qui doit passer avant tag) | [`docs/release-dossier/README.md`](README.md) §4 — **actualisé par ce brief §1** |
| Notes de version | [`docs/RELEASE-NOTES-v0.9.0.md`](../RELEASE-NOTES-v0.9.0.md) (à renommer v2.0.0, #999) |

---

## 5. Ce que ce brief ne fait pas

- ❌ Ne tag pas, ne publie pas (gate jsboige).
- ❌ Ne décide pas le packaging, le finding PT, le couplage DNN — les **surface en D1/D2/D3**.
- ❌ Ne déclare pas de verdict visuel (ai-01 only).
- ❌ Ne régénère rien (la régén #983 attend son feu vert).

---

## TL;DR pour jsboige

> **La release est prête sauf 3 PR mécaniques (po-2024, en cours) → régén mindmaps → verdict visuel
> → tag `v2.0.0`.** Tu as **4 décisions** (packaging, finding PT, couplage DNN, scope mindmaps —
> D1-D4 ci-dessus). Le bundle v3 (80 PDFs CMYK) est déjà sur GDrive, verdict visuel PASS, tests
> 638/0/5. Le chemin est court.

🤖 Worker po-2023 — 2026-08-07
