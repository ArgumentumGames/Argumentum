# #498 grain 1 — l'instrument lit la hiérarchie dans `path` : re-mesure, contrôle inverse, et trois lecteurs de `decimal_path` passés au crible

**Date :** 2026-11-06 · **Lane :** po-2024 · **Grain :** pool #458 c.6665295882 item 2, détail [#498 c.6665070534](https://github.com/ArgumentumGames/Argumentum/issues/498#issuecomment-6665070534)
**Base :** `origin/master` `5740727b` · **Instrument :** [`tools/498-aif-cadrage-measure.py`](../../tools/498-aif-cadrage-measure.py) — lecture seule

---

## 1. Le défaut corrigé

La version précédente de l'instrument reconstruisait les ancêtres en découpant `decimal_path`
**aux virgules**, et sa docstring enseignait ce découpage comme la méthode correcte. Mesuré sur
l'arbre vivant :

- `decimal_path` porte **une seule virgule** sur 1 400 rangées (8 n'en ont aucune), juste après
  la famille (`2,1151251`) ;
- les niveaux suivants sont **collés**, et un rang monte jusqu'à **24**, donc ils ne peuvent pas
  y être retrouvés ;
- l'instrument ne voyait d'ancêtre que la **tête de famille** — aucun héritage depuis un nœud
  intermédiaire n'a jamais été mesuré par lui.

La vraie hiérarchie est la colonne **`path`** (`2.1.1.5.1.2.5.1`), un segment par niveau :
**1 408 chemins uniques, aucun orphelin** (mesuré). Les ancêtres se découpent aux points.

## 2. Les deux tableaux, re-dérivés (et indépendamment confirmés)

Chaque élément du triplet hérite du **plus proche ancêtre porteur**. Définitions exactes,
portées par l'instrument :

- **(a)** schéma légitime imité — `AIF_skosExceptionRef` non vide, **ou** `AIF_skosDirectRef`
  dont une cible finit en `_Inference`/`_Scheme` ;
- **(b)** élément attaqué — `AIF_attackType` non vide ;
- **(c)** question sautée nommée par un schéma de conflit AIF — une cible de l'une des deux
  colonnes de refs finissant en `_Conflict`. ⚠️ mesure **par approximation** : AIF ne nomme
  qu'une vingtaine de conflits ; la question sautée écrite en prose est invisible ici.

⚠️ La nature Inference/Scheme/Conflict ne vit **pas** dans `AIF_skosMappingType` (qui porte des
prédicats SKOS : `skos:broadMatch`…) mais dans le **suffixe de la cible IRI**. Prendre la
colonne de mapping type pour la nature fabrique un (c) à zéro — mesuré.

### 2.1 Le triplet

| Élément | Porteurs | Nœuds couverts | Cartes |
| --- | ---: | ---: | ---: |
| (a) schéma légitime imité | 97 | **1 064 / 1 408** | 135 / 175 |
| (b) élément attaqué | 145 | **1 400 / 1 408** | 168 / 175 |
| (c) question sautée nommée | 58 | **959 / 1 408** | 133 / 175 |
| les trois (intersection) | | **731 / 1 408** | 108 / 175 |
| au moins un (union) | | 1 400 / 1 408 | 168 / 175 |

### 2.2 Les groupes où (a) ou (c) manque — le chantier réel

| Groupe | Nœuds | sans (a) | sans (c) | sans (a) ou (c) |
| --- | ---: | ---: | ---: | ---: |
| 2.1 Procédé rhétorique | 123 | 81 | 81 | 81 |
| 2.2 Appel à l'émotion | 57 | 1 | 54 | 54 |
| 2.3 Manipulation mentale | 239 | 156 | 0 | 156 |
| 3.1 Généralisation abusive | 37 | 0 | 19 | 19 |
| 3.2 Mauvaise interprétation | 34 | 0 | 23 | 23 |
| 3.3 Résultat invalide | 30 | 29 | 29 | 29 |
| 5.2 Comparaison fallacieuse | 13 | 4 | 0 | 4 |
| 6.1 Arranger les faits | 85 | 0 | 65 | 65 |
| 6.2 Changement de cap | 51 | 49 | 0 | 49 |
| 6.3 Raisonnement biaisé | 257 | 0 | 173 | 173 |
| 7.2 Sabotage du débat | 48 | 16 | 0 | 16 |

**669** nœuds dans ces groupes ; **677** au total manquent (a) ou (c) ; les **8** hors groupes
sont les nœuds de tête `0,1,2,3,4,5,6,7`. Ces deux tableaux, re-dérivés de zéro par
l'instrument corrigé, coïncident **à l'unité près** avec ceux de la correction d'ai-01
(c.6665070534) : deux instruments, deux écritures, mêmes chiffres.

## 3. Le contrôle inverse (exigé par le grain)

`--self-test`, 14 contrôles, arbre **synthétique** (aucune lecture du corpus) :

1. **Le contrôle du dispatch** : un enfant de porteur **intermédiaire** est compté couvert via
   `path` — et l'ancienne méthode (découpage aux virgules, conservée dans l'instrument sous
   étiquette « DEFECTUEUSE ») ne rend que la tête de famille : le défaut est **reproduit**,
   pas raconté.
2. Un rang à deux chiffres (`2.1.24`) n'est pas déchiré : son ancêtre est `2.1`, jamais `2.1.2`.
3. Monotonie et cohérence : porteurs ⊆ couverts ; l'intersection incluse dans chaque élément ;
   une famille sans porteur n'est pas couverte (pas de faux positif).
4. Classification par suffixe : cibles coupées proprement, vide ≠ cible, `_Inference` ≠
   `_Conflict` ; `DirectRef _Inference` compte pour (a) ; `DirectRef _Conflict` compte pour
   (c) et pas pour (a).

## 4. Les autres lecteurs de `decimal_path` — deux trouvailles

Le grain demandait de vérifier les jumeaux `docs/taxonomy/141-aif-*.py`. Faits mesures :

**Jumeaux sains, à une rangée près.** Leur `comma2dot`/`parent_of` éclate le reste collé
**chiffre par chiffre** — la bonne mécanique. `comma2dot(decimal_path) == path` sur
**1 407 / 1 408** rangées. L'exception : `decimal_path='5,123224'`, vrai `path='5.1.2.3.2.24'`
— un rang **24**, que l'éclatement en chiffres simples corrompt en `5.1.2.3.2.2.4` : le parent
calculé de cette rangée **n'existe pas dans l'arbre** (silencieusement absent du pool de
candidats des jumeaux).

**Un export committé est une étoile, pas un arbre.** `tools/aif-attack-graph-export.py`
(`export_taxonomy_tree`) calcule `depth = virgules + 1` et `parent = découpe aux virgules`.
Mesuré sur le fichier committé `docs/ontology/aif-export/taxonomy-tree-edges.csv` :
**1 408 lignes, depth ∈ {1, 2} seulement, 8 parents distincts** — chaque nœud pend
directement de sa tête de famille, alors que le README de l'export et le rapport
d'investigation du 19/07 le décrivent comme « l'arbre taxonomique complet (backbone) ».
Aucun code du dépôt ne consomme ce fichier (grep : le producteur et deux mentions
documentaires seulement), mais tout lecteur qui le croirait hériterait de l'étoile.

Les deux corrections (jumeaux : lire `path` ; export : arêtes depuis `path`) sont **volontairement
hors de ce grain** — une instrument par PR, et la re-génération de l'export touche d'autres
fichiers. Proposées comme grain suivant.

## 5. Ce qui n'a pas changé

- **Les porteurs bruts** (§1 de l'instrument, §2 du cadrage) : 86 / 71 / union 116 ==
  `AIF_skosMappingType` / attack 145 / attack-only 29 — aucun ancêtre là-dedans, confirmés.
- **0 écriture corpus**, 0 code C#, 0 cellule : le chantier reste gated ; ce grain ne fait que
  mesurer.

## 6. Contrôles exécutés

- `tools/498-aif-cadrage-measure.py --self-test` — **14/14 verts** (arbre synthétique).
- `tools/498-aif-cadrage-measure.py` — sortie complète : sections 1-5, les deux tableaux
  ci-dessus re-dérivés à l'exécution.
- Recoupement indépendant : sondes de définition (suffixes des cibles, équivalence
  `comma2dot`/`path`, comptes par groupe) exécutées **avant** l'écriture de l'instrument —
  les chiffres de §2 ne sont pas recopiés de c.6665070534, ils sont tombés égaux.

*po-2024*
