# #498 — tranche documentaire `Abus de langage` : **faite**, et ce qui reste vraiment

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `874a5d98`
**Mesuré à** `21a72385`, **reporté tel quel à** `874a5d98` : entre les deux, master n'a reçu que **3 ajouts `.md`** (`#1360`, `#1358`, `#1357`, +568/−0) — `Cards/` et `docs/ontology/` ne sont pas touchés, donc recensement identique par construction, pas par supposition.
**Statut** : **MESURE / DOSSIER** — lecture seule. ⛔ `0` écriture CSV, `0` OWL, aucune modélisation appliquée.
**Dispatch** : ai-01, deep-queue #458 c.5656689863, grain **④** (`[idle] #498 P3, tranche documentaire `Abus de langage` sans écrire le CSV`).

> ⚠️ **Écart de spec déclaré avant de courir.** La ligne du dispatch nomme deux objets que la mesure sépare : la **tranche documentaire `Abus de langage`** est **déjà faite et appliquée** (§0), et **P3** est un lot distinct, gelé depuis le 29/08 (§2). Plutôt que de re-modéliser une tranche close — ou d'inventer un livrable sur une prémisse périmée — ce dossier **établit l'état des deux** et chiffre ce qui reste. Aucun travail de modélisation n'est proposé ici : le GO d'écriture et le contenu des propositions sont des gestes séparés.

---

## §0 La tranche `Abus de langage` est faite — documentée **et** appliquée

Le commentaire du 2026-08-29 portait encore « *le write est toujours gated GO owner, le CSV prod n'est pas modifié* ». **Ce n'est plus vrai.** Le write est arrivé.

Contrôle inverse sur le commit qui l'a porté, `49ef5fad` = PR **#1234** (« P2-A production write — deep-serialize skos on the 46 'Abus de langage' attack-only rows ») :

| révision | lignes | skos (≥1 cellule) | attack-only |
|---|---:|---:|---:|
| `49ef5fad^` | 1 408 | **70** | **75** |
| `49ef5fad` | 1 408 | **116** | **29** |
| `874a5d98` (master du jour) | 1 408 | **116** | **29** |

Les deux arithmétiques tombent juste : `70 + 46 = 116` et `75 − 46 = 29`. **La tranche `Abus de langage` représente exactement les 46 lignes qui ont bougé**, et rien d'autre n'a bougé depuis.

- **Documentation** : PR **#1219** (mergée `2026-08-29T08:57:26Z`) — docs + `--apply` dry-run.
- **Application** : PR **#1234** (`49ef5fad`) — write de production.
- **État au HEAD** : `attack-only` restant dans la famille `Abus de langage` = **0**.

⇒ **Il n'y a pas de « tranche documentaire `Abus de langage` » à produire.** Elle est livrée.

---

## §1 Ce que la famille `Abus de langage` porte encore (mesuré)

| | n |
|---|---:|
| lignes de la famille | 89 |
| dont ≥1 cellule skos | **57** |
| dont **aucun** AIF (ni skos, ni attackType) | **32** |

Les 32 restantes ne sont **pas** des lignes à modéliser **en bloc** : ce sont les **nœuds sans AIF** de la famille, et cet ensemble n'est **pas homogène**.

| forme | n |
|---|---:|
| **feuilles** (aucun descendant) | **24** |
| **nœuds intérieurs** | **8** |
| dont portent une `carte` | **6** — `798` · `809` · `813` · `837` · `848` · `869` |

⚠️ **Correction du 14/09 (re-review).** La version antérieure généralisait : « les 32 sont les **nœuds intérieurs** de la famille, dont les feuilles sont déjà couvertes ». C'est faux — **24 des 32 sont des feuilles**, `816` compris. La mesure exige une descendance **stricte** sur `path` (préfixe `p.`, séparateur compris) : une sonde par préfixe nu lit `5.1.2.3.2.24` comme descendant de `5.1.2.3.2.2` et gonfle artificiellement les nœuds intérieurs (23 feuilles au lieu de 24). Les 32 se lisent donc comme **32 nœuds sans AIF**, feuille/intérieur distingués — jamais comme une classe unique.

| profondeur | 4 | 5 | 6 | 7 | 1 |
|---|---:|---:|---:|---:|---:|
| nœuds | 8 | 8 | 11 | 4 | 1 |

**Deux** d'entre eux portent un `nom_vulgarisé` à profondeur 6 — pas un :

| pk | nom | position | `carte` | voie |
|---|---|---|---:|---|
| **`816`** | « Argument Bush » (`Définition inexacte > Acception arbitraire`) | feuille | — | **non dérivé — rangé en tranche PR-13** |
| **`869`** | « chosification » (`Ambiguïté > Equivoque`) | intérieur | **2** | **déjà close** |

`869` n'est pas un candidat ouvert : elle appartient au cluster **ratifié** `498-aif-equivoque-reification-cluster.md` (PR-12, **level-confusion → FAIL-LOUD**), qui la déclare `unmapped` avec `868` et `870`–`874`. Elle se cite donc **au même titre que les quatre verdicts ratifiés du §2.2** — une ligne close par verdict, à ne pas re-modéliser. Le `depth 1` est la racine de famille elle-même.

⇒ **`816` reste le seul candidat encore non dérivé de la famille** — 1 ligne, pas 32 — et il est **déjà rangé dans la tranche différée PR-13** : le dossier `498-aif-arbitrary-definition-cluster.md` couvre la famille `pk 804–825` et y diffère le mécanisme *dichotomy* sur `808–818` (ancré sur le mappé `808`), donc `816` tombe dans **les deux plages**. Le travail neuf se réduit à cette seule ligne ; `869` n'en fait pas partie parce qu'elle est **déjà close par verdict**, et non parce qu'elle serait un nœud intérieur.

---

## §2 P3 — le lot réellement ouvert, gelé depuis le 29/08

**P3 = les 29 lignes `attack-only` hors `Abus de langage`.** Elles n'ont pas bougé d'une unité entre le recensement du 2026-08-29 et aujourd'hui — j'ai vérifié les **PK un par un**, ils sont identiques :

| famille | n | PK |
|---|---:|---|
| Tricherie | 8 | 889 942 974 992 1011 1024 1174 1242 |
| Insuffisance | 5 | 55 96 112 134 165 |
| Erreur mathématique | 5 | 596 644 667 681 690 |
| Influence | 4 | 219 247 420 511 |
| Obstruction | 4 | 1282 1287 1345 1352 |
| Erreur de raisonnement | 3 | 698 735 784 |

### §2.1 Combien sont déjà documentées — **10 sur 29**, et pas par la même voie

| voie | n | lignes |
|---|---:|---|
| **`498-reconciliation-p3a-palier1.md`** (proposition, **non appliquée**) | 6 | 1024 · 1174 · 698 · 420 · 1011 · 667 |
| **Docs de cluster vettés** (antérieurs à P3) | 4 | 690 · 1282 · 1345 · 1352 |
| **Aucun document** | **19** | le reste |

### §2.2 ⚠️ Le piège : 4 des 29 sont **ratifiées skos-vides**

`690` (`Opération inappropriée`), `1282` (`Relativisme abusif`), `1345` (`Complication exagérée`), `1352` (`Empoisonner le puits`) ont chacune un doc de cluster vetté qui a **conclu à l'absence de scheme AIF natif** et les a sérialisées en `attack-columns-only` (all skos empty) — c'est-à-dire **exactement l'état où elles sont aujourd'hui**.

Une re-modélisation « fraîche » de ces 4 lignes **contredirait un verdict ratifié**. Toute passe P3 doit donc partir de la liste des **19 non documentées**, jamais de la liste des 29 brutes.

---

## §3 Feuille de décision (une feuille)

| | |
|---|---|
| **La question** | Que faire du lot P3 ? |
| **Le fait mesuré** | La tranche `Abus de langage` est **close** (documentée + appliquée). P3 = **29 lignes**, inchangées depuis 24 jours, dont **10 déjà documentées** — 6 par une proposition non appliquée, 4 par des verdicts ratifiés qui les déclarent **sans scheme natif**. Reste **19 lignes sans aucun document**. |
| **(A) Appliquer palier 1** | **6 lignes** (1024 1174 698 420 1011 667). La proposition existe déjà (`498-reconciliation-p3a-palier1.md`) et a été arbitrée par ai-01 le 30/08. Rien à modéliser — juste un GO. ⚠️ 2 d'entre elles (`698`, `667`) sont des **FAIL-LOUD** : aucune ligne de circularité ni de défaut-de-précision n'existe dans les 116 modélisées, donc leur note serait une **forme nouvelle** — c'est le vrai point d'arbitrage. |
| **(B) Documenter les 19 restantes** | **19 lignes**. C'est la « tranche documentaire » au sens où `Abus de langage` l'a été (#1219 : modélisation triple-AIF, proposition + dry-run, **puis** write séparé). Coût réel : 19 dérivations `scheme + attack-type + CA-node/CQ` contre le vocabulaire verrouillé (`FallacyAifVocabularyLockTests`, 60 tokens). ⚠️ Étendre à la famille ajoute **deux** candidats `nom_vulgarisé` de profondeur 6, pas un : `816` « Argument Bush » (**non dérivé, rangé en tranche PR-13**) et `869` « chosification » (**déjà close** par le verdict ratifié `level-confusion → FAIL-LOUD` — à citer au même titre que les quatre verdicts ratifiés du §2.2). Le travail neuf de cette extension se réduit donc à **`816` seul**. |
| **(C) Ne rien faire** | 0 écriture. Le corpus reste à **116/145** skos et **29** attack-only, c'est-à-dire l'équilibre stable depuis le 29/08. Aucun défaut *publié* n'en dépend : l'`attackType` non plus n'est pas dans le deck — il alimente l'OWL (#133, gatée). |
| **Nature** | Modélisation ontologique — chaque ligne est une **affirmation vérifiable** contre le standard Walton/AIF, pas une reformulation. Le DoD de #498 l'exige explicitement : *si un sophisme ne se décrit pas honnêtement comme exception à un scheme, le documenter comme tel — ne pas fabriquer un scheme factice.* |
| **Ce qu'il faut savoir avant de trancher** | Le lot (A) est **déjà écrit** quelque part : le blocker n'est pas le travail, c'est le GO. Le lot (B) est le seul vrai travail neuf, et il est **plus petit que la ligne du dispatch ne le laisse croire** (19, pas « P3 » en bloc) — parce que 4 des 29 sont closes par verdict et 6 par proposition. |

---

## §4 Ce que ce grain n'a pas fait

⛔ aucune ligne CSV écrite · ⛔ aucune proposition AIF rédigée · ⛔ aucun OWL régénéré · ⛔ aucune branche (A)/(B)/(C) choisie · ⛔ les 4 lignes ratifiées skos-vides n'ont **pas** été re-modélisées.

Mesure uniquement.

---

*po-2024 — grain ④ du dispatch #458 c.5656689863. Le worker mesure et signale ; la décision reste à l'owner.*
