# #498 — AIF two-layer reconciliation, P1 tranche-1e (SUFFIX-ONLY, famille « Insuffisance »)

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `c1ed77d2` (post-#777 ; tranches 1+1b écrites en prod → attack 114 ; 1c #779 + 1d #778 en attente de write) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 « suite SUFFIX-ONLY par sous-lots famille jusqu'à épuisement » (tertiaire GO autonome), couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

Suite de la réconciliation P1 (back-fill de la couche attack pour les 52 lignes skos-only). Tranches **1** (14 PRECEDENT), **1b** (2 PREC-TIE + 5 SUFFIX-ONLY), **1c** (7 SUFFIX-ONLY « Erreur de raisonnement »), **1d** (4 SUFFIX-ONLY « Erreur mathématique ») déjà livrées. Ce PR livre la **tranche-1e = 5 lignes SUFFIX-ONLY**, la **famille « Insuffisance » complète** :

| pk | sophisme (text_fr) | sous-famille |
|---:|---|---|
| 2 | Généralisation hâtive | Argument bâclé |
| 33 | Justification triviale | Argument bâclé / Justification triviale |
| 34 | Preuve anecdotique | Argument bâclé / Justification triviale |
| 43 | Pratique courante | Argument bâclé / Justification triviale |
| 71 | Argument d'autorité | Préjugé / Argument d'autorité |

**Aucun** token de ces 5 lignes n'a de précédent exact in-set (precedent-check : tous tokens novel — `LackOfPTKReliability_Scheme`, `AlternativeMeans_Conflict`, `Example_Inference`/`InductiveInference_Scheme`, `PopularPractice_Inference`, `ExpertOpinion_Inference`/`ExpertiseInconsistency_Conflict`). Modélisation Walton **au cas par cas** depuis le scheme propre + `desc_fr` (« que défait le CQ »). Le **prior de suffixe est proscrit** (contre-preuve in-set pk804 `_Conflict`→undercut, audit #770).

Distribution : **5 undercut / 0 undermine / 0 rebut**. Cette **uniformité est la signature attendue d'une famille d'insuffisance épistémique** — le défaut porte par nature sur le **lien inférentiel** (la preuve citée est trop faible pour soutenir la conclusion ; §3 : ce n'est pas un défaut par défaut, chaque ligne est modélisée et deux nuances sont flaguées MED). `attackType` = **jugement neuf** ; **0 fabrication de token** (#677). `attackedNode` déterministe (#707§4 a) → tous **RA-node**.

Back-fill **+5** : attack-typed **114 → 119** (baseline dynamique, §5). Reste après ce sous-lot : **15 SUFFIX-ONLY** (Tricherie 8, Abus de langage 7 — distributions MIXTES, prochains sous-lots).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos, ancrée sur les **lignes fully-modeled** (ground truth token→type). Pas d'héritage mécanique du sous-sous anchor (0/19 alignées, #769 §1).
- **Audit des 18 anchors** (#770) : **16 CLEAN / 2 SOFT / 0 erreur franche, node-map 18/18 propre**. Deux trouvailles **ancrent** ce sous-lot :
  1. **suffixe non décisif** (pk804 `PropertyNotExistant_Conflict`→undercut) → le prior de suffixe est proscrit ;
  2. les **défauts d'inférence in-set sont undercut** (pk707 `CauseToEffect_Inference`, pk727 `ModusPonens_Inference`, pk858 `VerbalSlipperySlope_Inference`) → contrôle de cohérence pour les schemes inférentiels de cette famille.
- **Tension utile** (precedent-check) : `PopularOpinion_Inference` est typé **undermine (3/3, pks 3/70/177)**. Ce sous-lot différencie explicitement **PopularPractice** (pk43) de **PopularOpinion** (§2, §3, flag MED).

Node déterministe (tous undercut ici) :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. SUFFIX-ONLY — famille « Insuffisance », modélisation Walton (5 lignes)

Aucun token de ces lignes n'a de précédent in-set typé. On modélise **depuis le scheme propre + `desc_fr`** (« que défait le CQ »). **Prior de suffixe proscrit.**

| pk | sophisme | scheme propre (signature skos) | `desc_fr` (abrégé) | → type | node | conf |
|---:|---|---|---|---|---|---|
| 2 | Généralisation hâtive | `LackOfPTKReliability_Scheme` | « argument à partir d'impressions/anecdotes, sans preuve solide » | **undercut** | RA | HIGH |
| 33 | Justification triviale | `AlternativeMeans_Conflict` | « accorder à une habitude/impression/exemple la valeur d'une preuve » | **undercut** | RA | MED |
| 34 | Preuve anecdotique | `Example_Inference` + `InductiveInference_Scheme` | « événement isolé, qui ne suffit pas à démontrer une règle générale » | **undercut** | RA | HIGH |
| 43 | Pratique courante | `PopularPractice_Inference` | « comportement justifié parce qu'il est communément adopté » | **undercut** | RA | MED |
| 71 | Argument d'autorité | `ExpertiseInconsistency_Conflict` + `ExpertOpinion_Inference` | « vraie uniquement parce qu'une autorité la soutient, sans examiner les faits » | **undercut** | RA | HIGH |

- **2** — généralisation hâtive (parente sous-famille « Argument bâclé »). La source (anecdote/impression) n'est pas en position de savoir : le CQ Walton du PositionToKnow défait le **lien** [témoin/source rapporte X] → [X est vrai] → RA. `LackOfPTKReliability_Scheme` = textbook undercutter du testimony scheme.
- **33** — le défaut = un exemple/habitude isolé ne soutient pas une règle → le pas inférentiel (induction) est trop faible → RA. **Flag MED** : **DISCONNECT skos/desc** — le token `AlternativeMeans_Conflict` (« il existe d'autres moyens d'atteindre Y ») ne matche pas la sémantique d'induction de la `desc_fr` (mappeur a assigné un token approximatif). Les deux lectures convergent vers undercut : (a) desc = induction faible = undercutter ; (b) `AlternativeMeans_Conflict` = undercutter canonique du means-end/practical-reasoning scheme. Alternative undermine écartée (aucune prémisse attaquée comme fausse).
- **34** — cas d'école du hasty generalization / undercutter de l'induction : un exemple unique ne soutient pas une règle → RA. **Parallèle exact tranche-1d pk595** (Généralisation abusive, mêmes tokens `Example_Inference`+`InductiveInference_Scheme`, → undercut, PR #778).
- **43** — appeal to common practice : la commonalité ne transfère pas la justification, le pas [commun → justifié] est invalide → RA. **Flag MED** : **TENSION** avec le precedent `PopularOpinion_Inference`→undermine (3/3, pks 3/70/177). Nuance : PopularOpinion = « vrai car beaucoup y croivent » (CQ attaque la prémisse « beaucoup y croient » → I-node) ; PopularPractice = « justifié car commun » (défaut dans le raisonnement, pas dans le fait → RA-node). Schemes distincts (practice/opinion, justification/vérité), mais ai-01 pourra préférer aligner sur le pattern « Popular* → undermine ».
- **71** — appeal to authority : l'autorité ne prouve pas la vérité, le pas [autorité soutient X] → [X est vrai] est invalide → RA. `ExpertOpinion_Inference` = l'inférence d'expertise est le défaut. **Distinction explicite avec pk1360** `Ethotic_Inference` (Ad hominem, → undermine, tranche-1b) : pk1360 **attaque** la crédibilité de l'adversaire (prémisse d'éthos, I-node) ; pk71 est l'**inverse** (l'argueur s'appuie sur une autorité, le CQ défait l'inférence). « sans examiner les faits » cible le raisonnement, pas une prémisse fausse.

Détail complet en colonne `justification` de [`498-reconciliation-p1e-annotations.csv`](498-reconciliation-p1e-annotations.csv).

---

## 3. Uniformité undercut — signature de famille, pas défaut par défaut

Ce sous-lot ressort **5/5 undercut**. C'est **attendu et cohérent**, non un artefact de facilité :

- **Sémantique** : « Insuffisance » regroupe les arguments **épistémiquement faibles** (généralisation hâtive, justification triviale, preuve anecdotique, pratique courante, appel à l'autorité). Le défaut porte structurellement sur le **lien entre la preuve citée et la conclusion** : la preuve (anecdote, exemple isolé, commonalité, autorité) est trop faible pour soutenir la conclusion → la **règle d'inférence** ne tient pas (undercut/RA), pas l'acceptabilité d'une prémisse (undermine/I) ni une contre-conclusion (rebut/CA).
- **Ancrage** : `ExpertOpinion_Inference`, `Example_Inference`, `LackOfPTKReliability_Scheme` sont les schemes Walton classiques dont les CQ sont des **undercutters** (le témoignage/l'exemple/l'autorité ne transfèrent pas la force épistémique). Aucun token n'a de precedent typé in-set (tokens novel) — l'uniformité vient de la **sémantique vérifiée par ligne**, pas d'un prior de suffixe (proscrit) ni d'un héritage d'anchor.
- **Garde-fou** : chaque ligne est modélisée séparément et **deux nuances sont flaguées MED** avec leur alternative explicite — **33** (disconnect skos/desc ; alternative undermine non applicable) et **43** (tension PopularOpinion→undermine ; alternative alignement pattern « Popular* » laissée à ai-01). Les trois autres (2/34/71) sont HIGH.

**Les 2 MED-flag load-bearing à revoir en priorité par ai-01 : 33 et 43.**

---

## 4. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Mécanisme unique de ce sous-lot : **la preuve citée est trop faible pour soutenir la conclusion — la règle d'inférence ne tient pas (undercut/RA)** —

- **source non fiable / pas en position de savoir** : 2 (généralisation hâtive depuis impressions/anecdotes) ;
- **induction trop faible** : 33 (exemple/habitude comme preuve), 34 (preuve anecdotique) ;
- **commonauté ⇏ justification** : 43 (pratique courante) ;
- **autorité ⇏ vérité** : 71 (argument d'autorité).

---

## 5. Sérialisation (flow #753/#760)

`tools/498-p1e-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1c-apply.py` :

- lit `498-reconciliation-p1e-annotations.csv` et **re-vérifie** que sa carte interne concorde 5/5 (assertion load-bearing) ;
- splitters byte-exact (guillemets doublés + LF encadrés), cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 5 PK ;
- pre-state : les 5 PK **vides** (fill, pas overwrite) + **portent une skos** (back-fill skos-only) ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 5 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1e.csv` avant écriture pour vérif indépendante.

**Baseline dynamique (pilotée par les `--write`, pas par les merges).** Les 5 PK sont **disjoints** des 14 de la tranche-1, des 7 de la tranche-1b, des 7 de la tranche-1c et des 4 de la tranche-1d. La baseline dépend des **sérialisations prod `--write` effectuées**, pas des PR mergées. Le script lit le compte **réel** et rapporte `total → total+5` sans jamais coder en dur la baseline :

- si seuls les `--write` 1 + 1b ont tourné (master actuel `c1ed77d2`) : `114 → 119` ;
- si le `--write` 1c (#779) a aussi tourné : `121 → 126` ;
- si les `--write` 1c + 1d ont aussi tourné : `125 → 130`.

L'ordre est donc sans impact sur la correction (cellules disjointes). Dry-run actuel (master `c1ed77d2`, baseline 114) : `114 → 119`, delta **+75 B**, **0 mismatch**, **1409×104**, CRLF+BOM préservés.

```bash
python tools/498-p1e-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1e-apply.py --write    # APPLY 5 cellules (GATÉ — relais ai-01)
```

---

## 6. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement — `git diff c1ed77d2 -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `c1ed77d2` ; précédents = les fully-modeled (audités CLEAN, #770) ; SUFFIX-ONLY confirmé par précédent-check (tous tokens novel).
- ✅ Prior de suffixe **proscrit** ; uniformité undercut = signature de famille vérifiée (insuffisance épistémique), chaque ligne modélisée, 2 MED flaguées (33, 43).
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche-1e (famille « Insuffisance », 5 SUFFIX-ONLY).
