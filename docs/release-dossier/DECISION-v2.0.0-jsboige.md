# v2.0.0 — Brief de décision pour jsboige

> **Objet** : ce qu'il reste à décider pour **tagger `v2.0.0`**, synthèse exécutive.
> **Auteur** : Claude Code @ myia-po-2023 (worker), 2026-08-07.
> **Base** : master `7abf4c50`.
> **Statut** : **BRIEF DE DÉCISION** — prépare ta validation week-end. Ne remplace pas les dossiers
> détaillés (indexés §4), il les actualise en 2 pages.
>
> **⚠️ ÉTAMPÉ 2026-09-13 — les 4 questions de ce brief ont été RÉPONDUES le 2026-08-08** (jsboige,
> canal interactif). Chaque réponse figure **sous sa question** au §3, datée et sourcée. Les questions
> sont **conservées** : un brief dont on ne voit plus la question devient illisible.
> **Lire §3 comme un compte rendu, pas comme une liste d'arbitrages ouverts.**

Ce brief ne **tag rien**, ne **publie rien**. C'est le document unique à lire pour décider.

> **Pourquoi cette étampe existe** (13/09) : ce brief est un **questionnaire sans étampe de réponse**.
> Ses 4 questions ont été tranchées le 08/08, mais **aucune** réponse n'y était inscrite — et la seule
> trace durable vivait dans un commentaire de #458, que rien ici ne pointait. Un lecteur à froid y a
> vu **4 arbitrages ouverts alors qu'il en restait zéro**, et a mis une lane en attente dessus pendant
> un cycle. Le §5 (« ne décide pas le couplage DNN ») **affirmait encore l'inverse de la décision**.

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

## 3. Tes décisions (week-end) — ✅ **TOUTES TRANCHÉES le 2026-08-08**

> Les 4 questions sont conservées ci-dessous, **chacune suivie de sa réponse** datée et sourcée.
> **Ne pas les relire comme des arbitrages ouverts.**

### D1 — Confirmer le packaging de distribution (#134)
Le bundle v3 existe déjà sur GDrive (80 PDFs CMYK). Trois options pour les rendre téléchargeables :

| Option | Avantage | Coût |
|--------|----------|------|
| **A. GitHub Release assets** (recommandé) | canonique, lien permanent, visible sur le repo | 80 fichiers × ~60 Mo = **4,8 Go** vs limite asset 2 Go/fichier ; OK en plusieurs assets |
| B. git-lfs | versionné avec le code | alourdit le clone ; non pertinent pour des binaires de release |
| C. Lien GDrive externe dans le README | zéro effort, déjà uploadé | non-canonique, dépend de ton compte GDrive |

**Mon appel** : **A** (assets GitHub, groupés par langue ou par type), avec le README qui pointe dessus.
Décision : **toi**.

> ### ✅ RÉPONSE — owner (jsboige), **2026-08-08**, canal interactif
>
> **D1 = A. GitHub Release assets** (la reco est suivie).
> **« à exécuter au packaging »** — non exécuté à la date de l'étampe : le packaging (#134) est
> postérieur au tag.
>
> *Source* : [issue #458, c.5227104283](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5227104283)
> — tableau « les 4 arbitrages jsboige sont TRANCHÉS (VÉRIFIÉ interactif 2026-08-08) », ligne **D1**.
> *Grade de provenance* : **VÉRIFIÉ** (décision gravée sur GitHub, pas relayée).

### D2 — Finding « titre PT Manche »
Sur la cover PT, le titre affiche **« Roll of the English Channel »** (homonyme Manche=Manche de
vêtement) au lieu du PT attendu. Fix prêt chez po-2024 (gated). Deux options :
- **corriger avant le tag** (recommandé — une cellule CSV, fix po-2024 déjà préparé),
- ou **reporter en post-tag** (serait un v2.0.1).

Décision : **toi**.

> ### ✅ RÉPONSE — owner (jsboige), **2026-08-08**, canal interactif — **décidée, exécutée, et CLOSE le jour même**
>
> **D2 = corriger AVANT le tag** (la reco est suivie ; la voie « reporter en post-tag / v2.0.1 » est
> **écartée**).
>
> **Et l'exécution a rendu NO-DEFECT, pas un fix.** Issue **#1034** ouverte le 08/08 à 09:50,
> **fermée le 08/08 à 23:40** par jsboige sur investigation po-2024 : *« aucun défaut vivant »* —
> la chaîne **n'a jamais atteint une carte**, elle n'a **jamais été versionnée** dans `Cards/`.
>
> ⚠️ **Re-mesuré indépendamment à l'étampe (2026-09-13, `origin/master`)** — reproduit le verdict :
> `git grep "English Channel" -- Cards/` → **0**. La chaîne ne subsiste que dans **13 fichiers, tous
> documentaires** (CHANGELOG, dossiers de release, investigations) — **aucun CSV, aucun template**.
> Le §3.6 de `RELEASE-VALIDATION-v0.9.0.md` l'avait établi dès le **2026-07-28** : le défaut résidait
> dans une clé `csv` **embarquée et ignorée à l'exécution** (écrasée par `HarvestManager.cs`),
> nettoyée par hygiène en #803.
>
> ⇒ **D2 est une décision d'ordre sur un objet éteint *avant* d'être décidée.** Rien à corriger,
> rien en attente : « corriger avant le tag » est satisfait par construction. **D2 se lit comme
> historique, pas comme travail restant.**
>
> *Sources* : [issue #458, c.5227104283](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5227104283)
> (ligne **D2**, **VÉRIFIÉ**) · [issue #1034](https://github.com/ArgumentumGames/Argumentum/issues/1034)
> — corps (décision) + commentaire de fermeture NO-DEFECT du 2026-08-08 23:40, **VÉRIFIÉ**.
> *Contre-mesure* : la re-mesure `Cards/` ci-dessus, faite à l'étampe, **VÉRIFIÉ**.

### D3 — Couplage DNN (#131/#132)
Le site `dnn.argumentum.myia.io` est **LIVE et stable** depuis le fix #131 (07-08, probe
FirstChanceException → `Default.aspx` 10.x). Reco po-2023/ai-01 : **dé-coupler** — la release cards
ship **indépendamment** du go-live DNN (ops VPS = ta tâche ops, pas un gate release).
- Confirmer : **la release v2.0.0 ne gate pas sur le DNN go-live** ?

> ### 🔴 RÉPONSE — owner (jsboige), **2026-08-08**, canal interactif — **la reco ci-dessus est ÉCARTÉE**
>
> **D3 = ON COUPLE.** La release v2.0.0 **gate sur le go-live DNN**. La recommandation de découplage
> portée par ce brief (et par le §8 du dossier de validation) est **explicitement overridée**.
>
> **Verbatim jsboige** :
> - **2026-08-08** — « **On couple**, et je te rappelle que le site hébergé sur po-2023 … n'est que le
>   site de **préprod**. c'est **myia-web1** qui est sur le serveur VPS de **prod** »
> - **2026-08-20** — « **On continue à coupler stp** … **celle de DNN n'est pas négociable** »
>
> ⚠️ **Correction de topologie, et elle est structurante** : ce que ce brief appelait « le site LIVE »
> — `dnn.argumentum.myia.io`, hébergé sur **po-2023** — est la **PRÉPROD**. La **PROD** est
> `www.argumentum.games` sur le VPS **myia-web1**.
>
> **Séquence release révisée** : valider la préprod (po-2023) → **cutover prod (web1 VPS)** → tag
> `v2.0.0`. Les **assets avancent en prérelease** en parallèle (régén, packaging D1, #1034) ; le
> **tag formel attend le go-live prod**.
>
> *Sources* : [issue #458, c.5227104283](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5227104283)
> (ligne **D3**, **VÉRIFIÉ**) · la réaffirmation du 20/08 est **citée** dans
> [c.5652005725](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5652005725)
> — *grade* **RAPPORTÉ** pour cette seule citation, le verbatim vivant en session interactive.
>
> ⚠️ **Note d'étampe** : cette ligne est celle qui a coûté un cycle le 13/09. Un lecteur froid a lu
> le §D3 **et** `README.md:106` comme deux prémisses opposées, en a conclu « arbitrage owner non
> posé », et a mis une lane en attente d'une décision vieille d'un mois. **Le README était le côté
> sourcé** (il cite la décision du 08/08) ; **c'est le §D3 qui était le document vieux**. La cause
> n'est pas la lecture, c'est l'absence d'étampe — corrigée ici.

### D4 — Scope mindmaps (confirmé, peu de marge)
Les mindmaps **8 langues sont déjà shipped** (PR #565). La régén #983 met juste à jour les **libellés
de familles** (apostrophes + #1007). Confirme : **8-lang mindmaps = exigence v2.0.0** (pas 4-lang
différé).

> ### ✅ RÉPONSE — owner (jsboige), **2026-08-08**, canal interactif
>
> **D4 = 8 langues confirmé.** Le scope 8-langues est une **exigence v2.0.0**, pas un 4-langues
> différé. La décision porte aussi l'ouverture d'un **forensic prod (#1033)** — le texte qui ne rentre
> plus sur `fallacies_fr.html` en prod, causé par un wrapper **pré-06/08** servi : **aucun gabarit à
> corriger**, l'action résolvante est de **déployer master** (chantier DNN, op web1 VPS).
>
> **Verbatim jsboige** (antérieur, même portée, sur le scope langues *et* DNN) : « ça fait plusieurs
> fois que j'ai dit scope 8 langues, garde la décision stp **pareil pour dnn** ».
>
> *Source* : [issue #458, c.5227104283](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5227104283),
> ligne **D4** — *grade* **VÉRIFIÉ**. Le verbatim « scope 8 langues » est lui aussi gravé sur #458
> (commentaire du 2026-08-06, *grade* **VÉRIFIÉ**).

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
- ⚠️ ~~Ne décide pas le packaging, le finding PT, le couplage DNN — les **surface en D1/D2/D3**.~~
  **CADUC depuis le 2026-08-08** : les 4 questions ont été **tranchées** (§3, étampes). Ce brief
  **exposait** des arbitrages ; il les **enregistre** désormais. *(Cette ligne est conservée barrée
  plutôt que supprimée : c'est elle qui affirmait l'inverse de la décision et qui a induit un lecteur
  en erreur le 13/09.)*
- ❌ Ne déclare pas de verdict visuel (ai-01 only).
- ❌ Ne régénère rien (la régén #983 attend son feu vert).

---

## TL;DR pour jsboige

> **La release est prête sauf 3 PR mécaniques (po-2024, en cours) → régén mindmaps → verdict visuel
> → tag `v2.0.0`.** ~~Tu as **4 décisions**~~ → **tu n'en as plus aucune : D1-D4 sont TRANCHÉES
> depuis le 2026-08-08** (§3, étampes). Le bundle v3 (80 PDFs CMYK) est déjà sur GDrive, verdict
> visuel PASS, tests 638/0/5. Le chemin est court.
>
> **Ce que la décision change au chemin** : **D3 = ON COUPLE** ⇒ le tag n'est plus « la fin du
> chemin » mais vient **après** le cutover prod (web1 VPS). Séquence : préprod validée → **cutover
> prod** → tag.

🤖 Worker po-2023 — 2026-08-07
