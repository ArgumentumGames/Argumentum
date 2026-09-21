# #498 — Cadrage : les sophismes comme **exceptions à des schemes** (paradigme AIF/Walton)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `6694d702`
**Grain** : pool #458 **v8 ⑥** — *« cadrage docs-only. Sophismes comme exceptions à des schemes. **Aucune écriture corpus.** »*
**Instrument** : [`tools/498-aif-cadrage-measure.py`](../../tools/498-aif-cadrage-measure.py) — lecture seule, rejouable, **n'écrit rien**.
**Statut du chantier** : le **write** reste **POST-TAG**, sous **gate owner nommé** (statut du 19/09, reconfirmé ici par la mesure). Ce document ne change pas ce gate ; il le rend **chiffrable**.

> ⛔ **0 écriture CSV, 0 mutation de taxonomie.** Aucun code C# touché.

---

## 1. Le fait central : **trois chiffres de « couverture » circulent, et ils ne mesurent pas le même objet**

C'est la contribution de ce cadrage, et elle vient d'un échec de ma part : j'ai commencé par vouloir « re-mesurer la couverture », et j'ai obtenu un nombre qui ne correspondait à rien de publié — parce qu'il n'existe pas **une** couverture, il en existe **trois**, sous trois règles différentes.

| Définition | Règle | Mesuré ici | Où elle est publiée |
|---|---|---:|---|
| **Porteurs** | nœud portant lui-même ≥ 1 `DirectRef`/`ExceptionRef` | **116** (8,2 %) | recensement 3 états, commentaire du **14/09** — **adossé à un test** (`FallacySkosCensusTests`) |
| **Héritage, refs seules** | porteur **ou descendant** d'un porteur | **488** (34,7 %) | — |
| **Héritage, refs-ou-attack** | idem, `attackType`/`attackedNode` comptant comme marqueur | **505** (35,9 %) | — |
| *Héritage, corps de l'issue (juin)* | idem, règle non écrite | *442 (31,4 %)* | **corps de #498** — ⚠️ **périmé** |

⚠️ **Les 442/31,4 % du corps de l'issue ne sont reproduits par aucune définition rejouable.** L'écart (+46 à +63 nœuds) est cohérent avec les écritures atterries depuis — dont **P2-A** (46 lignes « Abus de langage », `49ef5fad`, 30/08) — mais la règle d'origine n'étant pas consignée, **on ne peut pas le prouver** ; on peut seulement constater que le chiffre a bougé.

⛔ **Conséquence directe pour tout dispatch** : « la couverture est de ~31 % » est **inverifiable** sans sa définition. Un grain bâti sur ce chiffre chiffre un objet non nommé. C'est la famille « le compte suit la définition publiée », appliquée ici à un chiffre qui n'a **pas** de définition écrite.

⇒ **La première tâche du chantier post-tag n'est pas d'écrire des cellules, c'est d'écrire la définition** — et de la faire porter par un test, comme le recensement du 14/09 l'a fait pour les 3 états.

## 2. Confirmation indépendante du recensement du 14/09

L'instrument reproduit **à l'unité** les chiffres du recensement testé (`FallacySkosCensusTests` + `FallacyAifVocabularyLockTests`, 4/4 verts) :

| | Recensement 14/09 | Mesuré 21/09 | |
|---|---:|---:|---|
| porteurs (mappée) | 116 | **116** | ✅ |
| `AIF_attackType`/`attackedNode` | 145 | **145** | ✅ |
| **attack-only** (attack sans ref) | 29 | **29** | ✅ |
| somme 3 états | 1408 | **1408** nœuds | ✅ |

C'est un résultat utile en soi : **deux instruments indépendants, deux dates, même chiffre** — et le second ne lit aucun artefact produit par le premier (il relit le CSV brut). Le recensement du 14/09 est donc **confirmé sur la base vivante**, pas seulement « vert dans son test ».

Détail des porteurs : `DirectRef` **86** nœuds · `ExceptionRef` **71** nœuds · **union 116** (donc **41 nœuds portent les deux**), et l'union **coïncide exactement** avec `AIF_skosMappingType` = 116 — les deux colonnes sont **redondantes**, ce qui est un point de cohérence, pas deux mesures.

## 3. Où le travail se trouve réellement : une asymétrie par famille

Couverture par héritage (refs-ou-attack) ventilée par famille :

| Famille | Couverts | Nœuds | % |
|---|---:|---:|---:|
| **Influence** | 12 | 420 | **2,9 %** |
| **Tricherie** | 22 | 394 | **5,6 %** |
| Erreur mathématique | 12 | 102 | 11,8 % |
| Abus de langage | 57 | 89 | 64,0 % |
| Insuffisance | 174 | 174 | **100 %** |
| Obstruction | 126 | 126 | **100 %** |
| Erreur de raisonnement | 102 | 102 | **100 %** |
| *Argument fallacieux* (racine) | 0 | 1 | 0 % |

⇒ **917 nœuds — 65 % de la taxonomie — vivent dans des familles couvertes à moins de 15 %**, et cette masse tient à **deux familles** : Influence + Tricherie = **814 nœuds, 58 % du corpus**, à 2,9 % et 5,6 %.

Trois familles sont **entièrement** couvertes, quatre sont quasi vides. La distribution n'est pas un dégradé : c'est un **partage net**. Un chantier « modéliser à grande échelle » qui se planifie « par tranches régulières » se trompe de forme — le travail est **concentré**, pas étalé.

⚠️ **Ne pas confondre avec les « subtrees 11/8/7/11/11/14/8 » du corps de l'issue** (somme 70) : cet objet-là compte des **grappes par famille**, pas des pourcentages de couverture. Je ne l'ai **pas re-mesuré** ici (voir §7).

## 4. Les deux périmètres — et ils ne coïncident pas

| Périmètre | Couvert | Total | % |
|---|---:|---:|---:|
| **Taxonomie** (nœuds) | 505 | 1408 | 35,9 % |
| **Deck** (cartes imprimées) | 127 | 175 | **72,6 %** |
| **Deck sans structure d'exception** | **131** | 175 | **74,9 %** |

Les deux perimeters ne se déduisent pas l'un de l'autre : le deck est couvert à **72,6 %** là où la taxonomie l'est à **35,9 %**, parce que les cartes sont concentrées dans les familles bien servies. Et la **worklist du DoD** (« pour chaque sophisme traité : un scheme cible + une CQ explicite ») porte sur **131 des 175 cartes** — pas sur 1292 nœuds.

⚠️ **C'est un chiffre de travail, pas un chiffre d'effort** : 131 cartes à rédiger est très différent de « 1292 nœuds non traités », et les deux **circulent** comme s'ils décrivaient le même chantier. Le premier est le livrable ; le second est une décision de périmètre owner.

## 5. Le critère génératif — ce que le chantier doit produire

Le corps de l'issue le pose et je ne le modifie pas ; je le rends **rejouable**, parce qu'un critère non rejouable est un critère qu'on applique différemment à chaque tranche :

> Un sophisme est décrit comme **une exception défaisable à un scheme argumentatif légitime** : **scheme visé S** + **question critique Q violée** (ou défaiseur). ⛔ **Un nom latin seul ne satisfait pas le critère.**

Ce que le corpus porte déjà, mesuré — les cibles **distinctes** des deux colonnes de ref :

| Colonne | Cibles distinctes | Répartition par suffixe |
|---|---:|---|
| `AIF_skosDirectRef` | 28 | 19 `*_Conflict` · 8 `*_Inference` · 1 `*_Scheme` |
| `AIF_skosExceptionRef` | 38 | **32 `*_Inference`** · 5 `*_Scheme` · 1 `*_Conflict` |

La séparation est **cohérente avec le paradigme** : `DirectRef` pointe majoritairement des **conflits** (le type d'attaque), `ExceptionRef` pointe majoritairement des **schemes légitimes** (ce dont on s'exclut). Les deux colonnes font deux métiers différents.

⚠️ Le corps annonce « **41** schemes légitimes référencés comme cibles d'exception » ; je mesure **37** (32 + 5). Écart de **4**, non résolu ici — les deux chiffres peuvent compter des objets différents (dédupliqué entre les deux colonnes ? `*_Scheme` inclus ou non ?) et **je ne tranche pas** : c'est exactement le motif de §1, et le trancher sans la règle d'origine fabriquerait le chiffre.

**Règle fail-loud** (DoD, conservée telle quelle) : si un sophisme **ne se décrit pas honnêtement** comme exception à un scheme, le **documenter comme tel** — ⛔ **ne pas fabriquer un scheme factice**. Le corpus en porte déjà la trace : `AIF_skosOther` contient des mentions explicites du type *« No native AIF Inference scheme for non-faulty comparison »*, *« Divergence flag (ai-01 arbitration 2026-08-30) »*, *« swap deferred to owner GO »*. C'est la règle **appliquée**, pas seulement énoncée.

## 6. Worklist (post-tag, **gate owner**)

**131 cartes sans structure d'exception** (ni directe ni héritée). Les 20 premières : PK `1, 2, 33, 51, 55, 78, 79, 98, 104, 105, 108, 112, 121, 128, 134, 154, 165, 175, 176, 179`.

⛔ **Aucune n'est ouverte par ce document.** Le statut du 19/09 tient : le write est post-tag, sous gate owner nommé, et les **29 attack-only** hors « Abus de langage » exigent « discipline #677 + arbitrage distinct ». Les événements de reprise sont ceux du 19/09 : **tag v2.0.0 posé**, ou **GO owner explicite**.

## 7. Ce que ce cadrage n'établit **pas**

⛔ **La règle exacte du 442/31,4 %** — je constate qu'il a bougé, je ne démontre pas *comment* ; la règle n'est pas consignée · ⛔ les **« subtrees 11/8/7/11/11/14/8 »** du corps ne sont **pas re-mesurés** (objet différent : grappes par famille) · ⛔ l'écart **41 vs 37** cibles d'exception n'est **pas tranché** · ⛔ la **liste des 131 PK** est un périmètre **mécanique** : elle dit « pas de structure d'exception lisible dans ces colonnes », ⛔ **pas** « ce sophisme n'a pas de structure d'exception » — un sophisme peut être bien modélisé sous une autre forme, ou la structure peut vivre dans un CSV d'export non relu ici · ⛔ **rien n'est dit sur la qualité** des 505 nœuds couverts : couverture ≠ justesse, et un `broadMatch` posé sur un scheme non pertinent reste couvert · ⛔ **aucune estimation d'effort** : 131 cartes à rédiger ne dit rien du temps ni de la difficulté · ⛔ les CSV de `docs/ontology/aif-export/` sont des **instantanés**, non re-mesurés ici.

## 8. Ce qui est livré, et ce qui ne change pas

- **Instrument rejouable** : [`tools/498-aif-cadrage-measure.py`](../../tools/498-aif-cadrage-measure.py) — lecture seule, sort 0, rejoue §1-§5.
- **Ce document** : nomme les **trois définitions** de la couverture, mesure l'**asymétrie par famille**, sépare les **deux périmètres**, et rend la **worklist** chiffrée.
- **Inchangé** : le gate owner, le statut POST-TAG, l'arbitrage Option B du 30/08, les 3 états du recensement (116 / 0 / 1292). Les organes de test (`FallacySkosCensusTests`, `FallacyAifVocabularyLockTests`) restent l'autorité sur l'état ; ce document la **recoupe**, il ne la remplace pas.

---

*Un cadrage qui n'écrit rien n'est utile que s'il rend le write post-tag **chiffrable et vérifiable**. C'est ce qu'il tente : trois définitions nommées au lieu d'un chiffre flottant, une worklist de 131 cartes au lieu de 1292 nœuds, et une règle fail-loud déjà à l'œuvre dans le corpus.*
