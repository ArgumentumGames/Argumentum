# #497 — Substrat de comparaison grappe-à-grappe (lane ai-01, chantier multi-sessions)

> **Statut : proposition GATED sous `docs/taxonomy/`.** Aucune écriture dans le CSV de prod.
> L'écriture des cellules `crossLink_*` validées dans `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv`
> est l'**étape finale gated** (spot-check ai-01 + nod jsboige sur échantillon).
>
> **Mandat jsboige (VÉRIFIÉ, interactif 2026-06-17)** : « il faudra se donner la possibilité de
> **comparer des grappes entières**, et **remonter jusqu'aux nœuds qui se répondent, les plus généraux** »
> — à tâtons, sur de nombreuses sessions, comme le regroupement manuel en grappes de la taxonomie actuelle.
> Direction validée ; #497 est un **chantier multi-sessions**, pas une génération leaf-by-leaf.

Ce document livre le **substrat outillé** qui rend ces comparaisons faisables session après session.
Il ne propose pas (encore) de liens à écrire : il établit la **carte de travail** sur laquelle la
curation va s'appuyer. Le générateur `497-grappe-substrate.py` (commité ici) re-produit l'ensemble à
partir du CSV — reproductible, zéro état caché.

---

## 1. Méthode — pourquoi « grappe », pourquoi « le plus général d'abord »

La taxonomie Fallacies = **1408 nœuds** sur un arbre de profondeur 0→10 (`path` = numérotation
hiérarchique, ex `2.3.1.1.4.1`). Comparer **feuille à feuille** (1366 feuilles) est combinatoirement
intenable *et* contraire à la méthode de jsboige. On compare donc des **sous-arbres (« grappes »)**.

- **Grappe = sous-arbre raciné à un nœud de profondeur 2** (le niveau « sous-famille majeure »).
  Il y a **21 grappes depth-2** — le bon grain pour « les nœuds les plus généraux ». (Le niveau
  depth-3 = 63 grappes plus fines, disponible pour la descente ultérieure « à tâtons ».)
- **Trois couches d'analyse**, par fiabilité décroissante :
  1. **Inventaire** (§2) — squelette factuel des 21 grappes.
  2. **Grappes qui se répondent — preuve empirique** (§3) — on fait **remonter** les crossLinks
     feuille-à-feuille *déjà existants* jusqu'au niveau grappe. **Zéro fabrication** : c'est de la
     donnée réelle ré-agrégée. C'est le signal de confiance.
  3. **Candidats d'affinité lexicale** (§4) — heuristique exploratoire (vocabulaire partagé), à
     **trier en curation**. Ne vaut que comme générateur de pistes.

Workflow cible (§5) : valider la **correspondance générale** d'une paire de grappes, puis **descendre
à tâtons** en mappant les nœuds fins ; chaque session ajoute les liens généraux validés puis raffine.

---

## 2. Inventaire des 21 grappes depth-2

(`size` = nœuds du sous-arbre ; `cl` = crossLinks déjà posés dans le sous-arbre. Source machine :
`497-grappe-inventory.csv`.)

| path | Famille | nom de la grappe | size | depth | cl |
|------|---------|------------------|-----:|-------|---:|
| 1.1 | Insuffisance | Généralisation hâtive | 68 | 2–7 | 2 |
| 1.2 | Insuffisance | Préjugé | 63 | 2–7 | 0 |
| 1.3 | Insuffisance | Surinterprétation | 42 | 2–7 | 3 |
| 2.1 | Influence | Technique rhétorique | 123 | 2–8 | 0 |
| 2.2 | Influence | Appel à l'émotion | 57 | 2–7 | 2 |
| **2.3** | **Influence** | **Manipulation mentale** | **239** | 2–8 | **7** |
| 3.1 | Erreur mathématique | Généralisation abusive | 37 | 2–6 | 0 |
| 3.2 | Erreur mathématique | Interprétation quantitative erronée | 34 | 2–6 | 0 |
| 3.3 | Erreur mathématique | Conclusion mathématique invalide | 30 | 2–6 | 1 |
| 4.1 | Erreur de raisonnement | Causalité douteuse | 29 | 2–6 | 0 |
| 4.2 | Erreur de raisonnement | Composition fautive | 32 | 2–6 | 0 |
| 4.3 | Erreur de raisonnement | Déduction invalide | 40 | 2–7 | 0 |
| 5.1 | Abus de langage | Définition biaisée | 34 | 2–7 | 2 |
| 5.2 | Abus de langage | Comparaison fallacieuse | 13 | 2–4 | 0 |
| 5.3 | Abus de langage | Ambiguïté | 41 | 2–7 | 0 |
| 6.1 | Tricherie | Présentation trompeuse des faits | 85 | 2–8 | 0 |
| 6.2 | Tricherie | Déplacement des critères | 51 | 2–7 | 2 |
| **6.3** | **Tricherie** | **Raisonnement biaisé** | **257** | 2–10 | 0 |
| 7.1 | Obstruction | Refus du débat | 31 | 2–7 | 1 |
| 7.2 | Obstruction | Sabotage du débat | 48 | 2–7 | 0 |
| 7.3 | Obstruction | Ad hominem | 46 | 2–6 | 2 |

**Observation structurelle** : les deux **plus grosses** grappes de tout l'arbre — `2.3 Manipulation
mentale` (239) et `6.3 Raisonnement biaisé` (257) — concentrent l'essentiel de la masse. Couverture
relationnelle actuelle = **22 crossLinks / 1408 nœuds (1,6 %)**, très concentrée (7 des 22 partent de `2.3`).

---

## 3. Grappes qui se répondent — preuve empirique (bubble-up des crossLinks)

On prend les **22 crossLinks feuille-à-feuille existants** et on remonte **chaque extrémité jusqu'à sa
grappe depth-2**. Résultat : **16 arêtes grappe↔grappe**, dont **14 inter-familles**. Source machine :
`497-responding-grappes.csv`.

### Arête dominante (le point d'ancrage du chantier)

> **`2.3 Manipulation mentale` [Influence]  →  `6.3 Raisonnement biaisé` [Tricherie]  — poids 7**

Sept crossLinks `PredatesOn` (« prédate sur ») relient des **techniques de manipulation** au **biais
cognitif** qu'elles exploitent : Cadrage→Effet de cadrage, Ancrage→Effet d'ancrage, Leurre→Effet de
leurre, Coûts irrécupérables→Biais des coûts irrécupérables, etc. La relation générale qui « se
répond » est donc : **`Manipulation mentale` *exploite* `Raisonnement biaisé`**. Ce sont précisément
**les deux nœuds les plus généraux** qui se correspondent — exactement la cible de la méthode jsboige.
Une fois ce lien général posé, la descente est *systématique* : chaque technique de manipulation ↔ le
biais qu'elle exploite (mapping 1-à-1 partiellement déjà tracé, à compléter à tâtons).

### Les autres arêtes (poids 1, déjà attestées)

| src grappe | type(s) | tgt grappe | inter-fam |
|-----------|---------|-----------|:--------:|
| 7.3 Ad hominem [Obstruction] | PredatesOn | 6.3 Raisonnement biaisé [Tricherie] | ✓ |
| 7.3 Ad hominem [Obstruction] | Leverages | 5.2 Comparaison fallacieuse [Abus de langage] | ✓ |
| 7.1 Refus du débat [Obstruction] | Leverages | 5.3 Ambiguïté [Abus de langage] | ✓ |
| 6.2 Déplacement des critères [Tricherie] | IsRelatedTo | 1.1 Généralisation hâtive [Insuffisance] | ✓ |
| 5.1 Définition biaisée [Abus de langage] | Mirrors | 3.3 Conclusion math. invalide [Erreur math.] | ✓ |
| 2.2 Appel à l'émotion [Influence] | Allows | 1.3 Surinterprétation [Insuffisance] | ✓ |
| 2.2 Appel à l'émotion [Influence] | PredatesOn | 1.1 Généralisation hâtive [Insuffisance] | ✓ |
| 1.3 Surinterprétation [Insuffisance] | Leverages | 5.3 Ambiguïté [Abus de langage] | ✓ |
| 1.3 Surinterprétation [Insuffisance] | Denounces | 5.1 Définition biaisée [Abus de langage] | ✓ |
| 1.3 Surinterprétation [Insuffisance] | Leverages | 4.1 Causalité douteuse [Erreur de raisonnement] | ✓ |
| 1.1 Généralisation hâtive [Insuffisance] | IsRelatedTo | 7.1 Refus du débat [Obstruction] | ✓ |
| (2 arêtes intra-famille `6.2→6.2`, `5.1→5.1` — bruit attendu, ignorées en priorité) | | | |

Ces 14 arêtes inter-familles **sont la première carte des grappes qui se répondent** — entièrement
fondée sur des liens déjà validés à la main, donc sûres comme points de départ de la curation.

---

## 4. Candidats d'affinité lexicale (heuristique — à TRIER en curation)

Pour suggérer des paires **pas encore liées**, on mesure le vocabulaire distinctif partagé entre
grappes inter-familles (Jaccard sur les ~22 termes saillants de `text_fr`+`desc_fr`, stopwords
argumentatifs génériques retirés). **C'est un générateur de pistes, pas une preuve** : le bruit
résiduel (verbes de second-personne, « votre », « entre ») reste présent. Top pistes :

| Jaccard | grappe A | grappe B | termes partagés notables |
|--------:|----------|----------|--------------------------|
| 0.19 | Généralisation hâtive [Insuff.] | Refus du débat [Obstr.] | preuve, prétendez, rejetez, fournir |
| 0.16 | Interprétation quantitative [Err.math] | Causalité douteuse [Err.rais.] | causalité, cause, résultat, attribuez, tort |
| 0.16 | Appel à l'émotion [Infl.] | Ad hominem [Obstr.] | adversaire, discréditer, interlocuteur, proposition |
| 0.16 | Conclusion math. invalide [Err.math] | Ambiguïté [Abus] | argumentation, plusieurs, faites, utilisez |
| 0.13 | Généralisation hâtive [Insuff.] | Raisonnement biaisé [Tricherie] | croyances, monde, pensez, considérez |

Lecture curatoriale : « Interprétation quantitative ↔ Causalité douteuse » (les deux erreurs causales,
familles différentes) et « Appel à l'émotion ↔ Ad hominem » (attaquer via la personne / l'affect)
sont des **ponts plausibles** ; les autres mélangent du vocabulaire de surface. Liste complète
re-générable via le script (`cands2`, 65 paires ≥ 3 termes).

---

## 5. Workflow curatorial proposé (multi-sessions, « le général d'abord »)

1. **Session-type** : prendre **une** paire de grappes « qui se répondent » (commencer par l'ancre
   §3 : `2.3 ↔ 6.3`), poser/valider le **lien au niveau général** (le nœud depth-2/3 le plus général
   de chaque côté), avec son type (`PredatesOn`, `Leverages`, `Opposes`…).
2. **Descendre à tâtons** : sous ce lien général, apparier les nœuds fins (technique ↔ biais exploité),
   un sous-ensemble par session — pas d'exhaustivité forcée.
3. **Réciprocité** : rappel convention — `crossLink_*` stocke le **`path`** de la cible (PAS le PK) ;
   un lien réciproque = paths croisés dans les deux nœuds.
4. **Accumulation** : chaque session ajoute ses liens généraux validés au substrat (re-run du script
   met à jour le bubble-up — les nouveaux liens remontent automatiquement et raffinent la carte).
5. **Gate** : tout passe d'abord en proposition `docs/taxonomy/` ; l'écriture dans le CSV de prod =
   spot-check ai-01 + nod jsboige sur échantillon.

### Premières paires-cibles recommandées (ordre proposé)
1. **`2.3 Manipulation mentale` *exploite (PredatesOn)* `6.3 Raisonnement biaisé`** — déjà 7 liens, descente quasi-mécanique.
2. **`7.3 Ad hominem` *exploite* `6.3 Raisonnement biaisé`** (effet de mode, etc.) + **`2.2 Appel à l'émotion`** : les trois voies « attaque/émotion/mode → biais ».
3. **`3.2 Interprétation quantitative` ↔ `4.1 Causalité douteuse`** (pont causal inter-familles, piste §4 à valider).

---

## 6. Fichiers livrés (gate-safe, docs/ only)

| Fichier | Rôle |
|---------|------|
| `497-grappe-substrate.md` | ce document (méthode + carte de travail) |
| `497-grappe-inventory.csv` | les 21 grappes depth-2 (squelette factuel) |
| `497-responding-grappes.csv` | les 16 arêtes empiriques (bubble-up), machine-consumable |
| `497-grappe-substrate.py` | générateur reproductible (lit le CSV, écrit les sorties) |

**Aucune** modification de CSV de prod, OWL, EPITA, cartes, mindmaps. Je livre le substrat, je ne
déclare pas de lien à écrire : la curation grappe-à-grappe démarre sur ce socle, session après session.

🤖 ai-01 — lane #497 (curation outillée)
