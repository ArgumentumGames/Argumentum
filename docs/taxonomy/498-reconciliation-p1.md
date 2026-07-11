# #498 — AIF two-layer reconciliation, P1 (skos-only → attack columns)

**Worker** po-2024 · **Date** 2026-07-10 (addendum §2bis audit 2026-07-11) · **Base** master `e748735b` · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 `msg-20260710T180845-5i1v03` (réconciliation P1), couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

`498-coverage-status.md` (#768) a établi que le mapping AIF a **deux couches jamais réconciliées** : la couche **attack** (`AIF_attackType`+`AIF_attackedNode`, 93 lignes) et la couche **skos** (`AIF_skosDirectRef`/`ExceptionRef`/`MappingType`, 70 lignes), avec **52 lignes skos-only** (skos vetté, colonnes attack vides). P1 = back-fill de la couche attack pour ces 52.

Ce PR livre :

1. **Une correction load-bearing** au cadrage #768 : il **n'existe pas** de sous-ensemble « inherit mécanique 0-risque » parmi les 52. La classification initiale « 19 inherit du sous-sous anchor » est un **artefact** — la vérification montre **0/19** dont la signature skos correspond à celle de son anchor (les anchors du même sous-sous sont eux-mêmes **skos-vides** : ils appartiennent à la couche attack-only, modélisée par #753/#760 sur des arguments **différents**). Hériter leur `attackType` serait une **fabrication** (§1).
2. **La méthode de dérivation** rigoureuse : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos, ancrée sur les **18 lignes fully-modeled** (les seules qui portent skos **et** attack — ground truth « cette signature → ce type »). `attackedNode` suit déterministiquement (#707§4 (a)) (§2).
3. **Tranche 1 — 14 lignes PRECEDENT** (token exact d'un précédent fully-modeled + contrôle sémantique) : proposition ready-to-serialize, distribution **11 undermine / 2 undercut / 1 rebut** (§3–§4).
4. **Le reste (38) scopé** : 2 PREC-TIE (votes de tokens divergents) + 36 SUFFIX-ONLY (aucun précédent de token ; le « prior de suffixe » par défaut vers *undermine* est **démontrablement non fiable**) → modélisation Walton **au cas par cas**, pas d'auto-dérivation (§5).
5. **Audit des 18 précédents ground-truth** (addendum 2026-07-11) : la méthode ne vaut que si les 18 anchors sont eux-mêmes correctement typés. Audit main-Walton → **16/18 CLEAN, 2/18 SOFT (défendables), 0 erreur franche, node-map 18/18 propre** ; l'ancrage est **validé** et l'audit **durcit** la méthode (§2bis).

Compléter la tranche 1 porte le fully-modeled **18 → 32** lignes (1.3% → 2.3%). Le reste demande un vrai travail de modélisation, tranché en sous-lots.

---

## 1. Correction : pas d'« inherit » mécanique (0/52)

Le premier passage classait les 52 en « 19 inherit du sous-sous anchor » (le sous-sous porte une ligne attack-typée → hériter son type) vs « 33 à modéliser ». **Contrôle de rigueur** (`498-p1-inherit-verify.py`, code=truth) : pour chacune des 19, la signature skos de la feuille est-elle alignée sur celle de son anchor ?

**Résultat : 0/19 alignées (19 DIVERGE).** Les anchors ont, dans la quasi-totalité des cas, une signature skos **vide** : ce sont des lignes de la couche **attack-only** (#753/#760), qui portent un `attackType` mais **pas** de skos, et modélisent des arguments **distincts** dans le même sous-sous. Exemples :

| Feuille skos-only | son skos | anchor même sous-sous | skos de l'anchor |
|---|---|---|---|
| pk677 « Pente glissante » | `WeakestLink_Conflict` + SlipperySlope schemes | pk667 (undermine) | **vide** |
| pk705 « Pente glissante » | `RequiredSteps_Conflict` + SlipperySlope schemes | pk698 (undermine) | **vide** |
| pk337 « Appel à la terreur » | `IrrationalFearAppeal_Conflict` + `FearAppeal_Inference` | pk322 (undermine) | `NegativeConsequences_Inference` (**autre** scheme) |

Hériter le type de l'anchor propagerait un type **non fondé sur la modélisation propre de la feuille** — et, pour pk677/705, probablement **faux** : leurs CQ (`WeakestLink`/`RequiredSteps`) contestent la **tenue de la chaîne causale** → penchent **undercut**, alors que l'anchor est typé undermine. **Conclusion : l'`attackType` est un jugement neuf** (les tokens existent = 0 fabrication de token ; mais le type d'attaque n'est **pas** dans le skos). Ceci raffine le « 0 fabrication risk » de #768 : 0-risque **token**, pas 0-risque **modélisation**.

---

## 2. Méthode de dérivation (ancrée sur les 18 fully-modeled)

Les **18 lignes fully-modeled** (skos **et** attack) sont le seul ground truth « signature skos → attackType assigné par le modélisateur ». On construit depuis elles une carte **token → attackType** (`498-p1-precedent.py`), puis pour chaque ligne skos-only on vote par ses tokens :

- **PRECEDENT** — au moins un token de la ligne a un précédent, vote **unique et cohérent**. `attackType` = ce vote. Contrôle sémantique par ligne (§4). **14 lignes.**
- **PREC-TIE** — des tokens votent pour des types **différents** → jugement requis. **2 lignes** (777, 633).
- **SUFFIX-ONLY** — aucun token de la ligne n'a de précédent ; seul le **suffixe** (`_Conflict`/`_Inference`/`_Scheme`) donne un signal. Or la carte de suffixe est `_Conflict{undermine:5,undercut:1}`, `_Inference{undercut:6,undermine:9,rebut:2}`, `_Scheme{undermine:1,rebut:1}` : elle **défaut tout vers undermine** (pluralité), ce qui est un **prior**, pas une dérivation. **36 lignes.**

`attackedNode` est **déterministe** (#707§4 Option a, ratifié) — la sérialisation prod le confirme exactement (90/93 lignes) :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion opposée) |

---

## 2bis. Audit des 18 précédents ground-truth (validation de l'ancrage) — addendum 2026-07-11

Toute la méthode §2 repose sur une hypothèse tacite : **les 18 anchors sont eux-mêmes correctement typés.** Si un anchor est mal typé, l'erreur se propage dans chaque dérivation (#769) puis, à terme, en prod. Cet addendum audite les 18 à la main (sémantique Walton : que défait le CQ — prémisse → undermine, inférence → undercut, conclusion → rebut) et croise le résultat au code=truth.

Machine-readable : [`498-audit-18-anchors.csv`](498-audit-18-anchors.csv) (10 colonnes, BOM+CRLF, colonnes factuelles lues du CSV master, `verdict`+`note` = revue authored). Reproductible : [`tools/498-audit-anchors.py`](../../tools/498-audit-anchors.py) (read-only ; re-dérive le set depuis prod, vérifie le node-map, croise la CSV d'audit).

**Résultat : ancrage validé.**

| contrôle | résultat |
|---|---|
| census des couches (code=truth) | fully-modeled **18** / attack-only 75 / skos-only 52 — concorde #768 exactement |
| node-map #707§4a (undercut→RA / undermine→I / rebut→CA) | **18/18 propre, 0 violation** |
| cohérence skos↔attackType (Walton, main) | **16 CLEAN** (textbook ou clairement défendable) · **2 SOFT** (défendables mais discutables) · **0 erreur franche** |

**Les 2 SOFT** — pas des erreurs, mais des choix de modélisation à signaler :

- **pk1313 Évasion** [rebut/CA] — l'évasion n'attaque ni une prémisse ni une inférence précise ; elle est modélisée comme un **conflit global au niveau du dialogue** (`Dialogue_Scheme`). Choix résiduel, uniforme pour la famille « saboter le débat » ; défendable mais pas un rebut-contre-conclusion canonique. **C'est l'anchor de pk1281** (tranche 1) — la même logique de nucleus rebut s'y applique, donc la softness est cohérente, pas transférée à tort.
- **pk1361 Procès en incohérence** [rebut/CA] — relever les incohérences (`InconsistentCommitment_Inference`) → contre-conclusion « ta position est intenable » → rebut. **Vrai jugement** rebut vs undercut/undermine. **C'est l'anchor sur lequel s'appuie 777→rebut** (§5) : sa softness confirme qu'il faut **trancher 777 par lecture de `desc_fr` + co-token**, pas mécaniquement.

**Trois trouvailles qui *durcissent* la méthode** (au-delà de la simple validation) :

1. **Le suffixe `_Conflict`/`_Inference` ne détermine PAS l'attackType — preuve in-set.** pk804 Acception arbitraire porte `PropertyNotExistant_Conflict` et est typé **undercut** (la définition arbitraire corrompt le *warrant*, pas une prémisse), alors que d'autres tokens `_Conflict` (177, 953, 1297…) sont undermine. C'est le contre-exemple **dans les 18** qui démontre l'affirmation §2/§5 « le prior de suffixe est non fiable » — jusqu'ici argumentée seulement via 677/705.
2. **Slippery-slope penche undercut — ancre in-set.** pk858 Pente glissante sémantique (`VerbalSlipperySlope_Inference`) est typé **undercut** (la chaîne « un concept mène nécessairement à un autre » est une inférence contestée). Ceci **ancre** le contre-exemple 677/705 « Pente glissante » du §1/§5 (jusqu'ici purement sémantique-CQ) sur un précédent réel.
3. **`CircumstantialAdHominem_Inference` n'est pas décisif.** Il apparaît dans **1361 (rebut)** *et* **1371 (undermine)** ; c'est le **co-token** (InconsistentCommitment vs SignFromOtherEvents) + la `desc_fr` qui tranchent. Règle de tie-break pour 777/633 : ne jamais voter sur un token partagé isolé.

Bilan : l'audit ne contredit pas §2, il le **confirme et l'outille**. Les 14 dérivations de la tranche 1 s'appuient toutes sur des anchors **CLEAN** (953, 177, 357, 322, 300, 750, 804, 340, 1371, 1313) — la seule dépendance à un anchor SOFT est **pk1281←pk1313**, cohérente (même famille, même nucleus rebut).

---

## 3. Tranche 1 — 14 lignes PRECEDENT (proposition)

Machine-readable : [`498-reconciliation-p1-annotations.csv`](498-reconciliation-p1-annotations.csv) (12 colonnes, BOM+CRLF). Distribution **11 undermine / 2 undercut / 1 rebut**.

| pk | famille | sophisme | skos (existant) | → type | node | précédent | conf. |
|---:|---|---|---|---|---|---|---|
| 1198 | Cheating | Essentialisme | `Preference_Scheme` | undermine | I | 953 | HIGH |
| 1083 | Cheating | Apophénie | `Sign_Inference` | undermine | I | 357 | HIGH |
| 1090 | Cheating | Biais émotionnels | `OppositeConsequences_Conflict`+`Preference_Scheme` | undermine | I | 953 | MED |
| 1092 | Cheating | Biais de négativité | `NegativeConsequences_Inference` | undermine | I | 322 | HIGH |
| 1104 | Cheating | Biais d'autocomplaisance | `PositiveConsequences_Inference` | undermine | I | 300 | HIGH |
| 1 | Insufficiency | Insuffisance | `GeneralAcceptanceDoubt_Conflict` | undermine | I | 953+177 | HIGH |
| 3 | Insufficiency | Argument vide | `OppositeConsequences_Conflict`+`PopularOpinion_Inference` | undermine | I | 177 | MED |
| 70 | Insufficiency | Préjugé | `Bias_Inference`+6 schemes | undermine | I | 177+340+953 | HIGH |
| 133 | Insufficiency | Surinterprétation | `LackOfCompleteKnowledge_Conflict`+`Sign_Inference` | undermine | I | 357 | HIGH |
| 4 | Insufficiency | Appel à l'ignorance | `Ignorance_Inference` | undercut | RA | 750 | HIGH |
| 799 | Misleading language | Définition biaisée | `ArbitraryVerbalClassification_Inference`+… | undermine | I | 177 | HIGH |
| 846 | Misleading language | Ambiguïté | `ArbitraryVerbalClassification`+`SignFromOtherEvents` | undermine | I | 177+357+1371 | HIGH |
| 621 | Mathematical error | Transfert illicite | `PropertyNotExistant_Conflict` | undercut | RA | 804 | HIGH |
| 1281 | Obstruction | Refus du débat | `Dialogue_Scheme` | rebut | CA | 1313 | HIGH |

Tous les précédents (953, 177, 357, 322, 300, 750, 804, 340, 1371, 1313) sont dans le set des 18 fully-modeled. **2 lignes MED** (3, 1090) : le vote vient d'un token de scheme **secondaire** (le CQ primaire `OppositeConsequences_Conflict` n'a pas de précédent direct) mais `undermine` reste cohérent avec la sémantique du sophisme — flaggé, à confirmer par ai-01.

---

## 4. Justification par ligne

Détail complet en colonne `justification` du CSV. Synthèse :

- **Undermine — biais/prémisse déformée** (1092 négativité←`NegativeConsequences` pk322 ; 1104 autocomplaisance←`PositiveConsequences` pk300 ; 1083 apophénie←`Sign` pk357 ; 1198 essentialisme←`Preference` pk953) : mêmes tokens que leurs précédents, la prémisse (conséquences/signe/préférence/essence) est déformée → I-node.
- **Undermine — insuffisance de prémisse** (1 insuffisance←`GeneralAcceptanceDoubt` ; 70 préjugé←6 schemes présomptifs ; 3 argument vide←`PopularOpinion` ; 133 surinterprétation←`Sign`+CQ connaissance incomplète) : le défaiteur conteste l'**acceptabilité/suffisance** d'une prémisse.
- **Undermine — classification biaisée** (799 définition biaisée, 846 ambiguïté ← `ArbitraryVerbalClassification` pk177) : la prémisse de catégorisation est construite pour biaiser.
- **Undercut — la règle d'inférence ne tient pas** (4 appel à l'ignorance ← `Ignorance_Inference` pk750 : « non-prouvé-faux ⇒ vrai » est une inférence invalide ; 621 transfert illicite ← `PropertyNotExistant_Conflict` pk804 : la propriété ne se transfère pas) → RA-node.
- **Rebut — contre-conclusion qui bloque l'échange** (1281 refus du débat ← `Dialogue_Scheme` pk1313 Évasion, même famille Obstruction, nucleus rebut) → CA-node.

---

## 5. Reste (38) — modélisation au cas par cas (P1 tranche 2+)

**Ne PAS auto-sérialiser.** Le prior de suffixe est non fiable (contre-exemple : pk705/677 « Pente glissante » ont des CQ qui contestent la chaîne → penchent **undercut**, pas le `~undermine` du prior).

**PREC-TIE (2) — arbitrage de token requis :**
- pk777 « Inconsistance » : `OpposedCommitment_Conflict`→undermine vs `InconsistentCommitment_Inference`→rebut (précédent pk1361 Procès en incohérence = rebut). Nœud parent → probablement **rebut**, à trancher.
- pk633 « Relation infondée » : `PropertyNotExistant_Conflict`→undercut vs `Sign_Inference`→undermine. À trancher.

**SUFFIX-ONLY (36) — modéliser depuis la sémantique du sophisme + son skos :**
- Cheating (8) : 1023, 888, 973, 1175, 1066, 1087, 1148, 1020
- Insufficiency (5) : 2, 71, 33, 34, 43
- Misleading language (7) : 833, 808, 814, 800, 876, 856, 839
- Faulty logics (7) : 696, 697, 726, 758, 759, 719, 705
- Mathematical error (4) : 595, 632, 677, 614
- Influence (3) : 356, 432, 337
- Obstruction (2) : 1280, 1360

Chacune demande la lecture de `desc_fr` + l'analyse « que défait le CQ » (prémisse → undermine, inférence → undercut, conclusion → rebut). Sous-lots par famille aux prochains ticks, chacun une proposition gated comme celle-ci.

---

## 6. Sérialisation (flow #753/#760)

`tools/498-p1-apply.py` — **gated, dry-run par défaut**, mirroir de `tools/498-phase13-apply.py` (#757) :
- lit `498-reconciliation-p1-annotations.csv` et **re-vérifie** que sa carte interne concorde 14/14 (assertion load-bearing) ;
- splitters byte-exact (guillemets doublés + LF encadrés), cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 14 PK ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 14 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/` avant écriture pour vérif indépendante.

```
python tools/498-p1-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1-apply.py --write    # APPLY 14 cellules (GATÉ — relais ai-01)
```

---

## 7. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on **type** des lignes qui portent déjà un skos vetté.
- ✅ Code=truth : tous les chiffres/tokens/labels lus du CSV master `e748735b`. Précédents = les 18 fully-modeled.
- ✅ Discipline rigueur : correction du cadrage « inherit » avant toute sérialisation ; tranche limitée aux 14 défendables ; 38 restantes scopées, pas devinées.
- ❌ #674/#666/#596 non touchés. Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche 1 (GO ai-01 `msg-20260710T180845-5i1v03`).
