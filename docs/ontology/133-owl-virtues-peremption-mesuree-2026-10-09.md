# OWL Vertus — péremption mesurée de l'artefact committé (et le coût caché d'une régénération)

**Grain** : pool #458, item *« OWL Vertus »* (scoping — l'item était un **nom**, pas un geste).
**Date** : 2026-10-09 · **Base** : `master` `37aae03a` · **Nature** : **mesure, lecture seule** —
aucune écriture, aucune régénération, aucun `--generate-owl` exécuté.

L'item de pool demandait « OWL Vertus ». Mesurer d'abord était nécessaire : **le geste évident
— régénérer — n'est pas un geste mécanique**, et il porte une rupture d'identifiant que rien
dans l'item ne signalait.

---

## 1. L'artefact est **bilingue par construction** — et un comptage naïf fabrique un faux défaut massif

`docs/ontology/argumentum_virtues.owl` (1 074 816 caractères, dernier geste `c1491de9` du
30/09, « regen x8 post-19w + OWL reconcile ») porte **446 triplets `prefLabel`** :

| Langue | Libellés dans l'OWL | Colonnes de titre au CSV |
|---|---:|---:|
| `fr` | **223** | 223 |
| `en` | **223** | 223 |
| `ru` · `pt` · `es` · `ar` · `fa` · `zh` | **0** | 223 chacune |

⇒ L'artefact **n'embarque que deux langues**. Un instrument qui confronte les 8 colonnes du
corpus à l'OWL rend **1 338 « absents »** sur six langues — un chiffre qui mesure la
**conception de l'artefact**, pas sa fraîcheur.

⚠️ **Mon premier instrument a rendu exactement ce faux massif**, et pire : il comptait
`prefLabel` sous forme **Turtle** (`skos:prefLabel`) alors que l'artefact est en dialecte
**OWL/XML** (`<AnnotationProperty IRI="…#prefLabel"/>` + `<Literal xml:lang="fr">`) — il rendait
**0 partout**, y compris là où l'artefact porte bien des libellés. Les deux erreurs vont dans le
même sens : *un compte n'est un fait que si l'instrument pouvait voir l'unité qu'il compte.*
Le contrôle qui a sauvé la mesure est le **témoin positif** : `« Argument valable »` (fr, pk 0)
retrouvé — sans lui, « 0 en fr » aurait pu passer pour « l'OWL n'a pas de libellés ».

**Portée déclarée de cette mesure** : `prefLabel` uniquement (`fr`/`en`). ⛔ **Non mesurés** ici :
`broader`/`narrower`, les trois couches relationnelles, et les arêtes AIF — une péremption y
serait invisible à cet instrument.

---

## 2. Le delta réel : **2 libellés anglais** en retard sur le corpus

Confrontation des 223 titres `fr` et 223 titres `en` du CSV courant à l'ensemble des libellés de
l'OWL (appariement par sujet via le libellé `fr`, identique des deux côtés) :

| Langue | Titres du CSV absents de l'OWL |
|---|---:|
| `fr` | **0 / 223** ✅ |
| `en` | **2 / 223** |

Les deux divergences, nommées :

| PK | `title_fr` (identique dans l'OWL) | CSV `title_en` (courant) | OWL `en` (artefact, 30/09) | IRI actuel de l'artefact |
|---:|---|---|---|---|
| **128** | Logique informelle solide | `Solid informal logic` | `Acceptable informal logic` | `#acceptableInformalLogic` |
| **176** | Tenir compte des biais idéologiques | `Taking the parties' ideological biases into account` | `Taking the opponent's ideological biases into account` | `#takingTheOpponentsIdeologicalBiasesIntoAccount` |

Les deux portent sur des titres **anglais édités en octobre**, après le dernier geste sur
l'artefact (30/09) — cohérent avec les grains de corpus de la période (#1795, #1806-#1808,
#1814, #1834 touchent le CSV Vertus ; 9 commits depuis le 30/09).

**Cause unique, datée** — `git log -S` sur chaque libellé : un **seul** commit a introduit les
deux valeurs courantes *et* retiré les anciennes, `371dd730` (grain 10, **2026-10-07 22:45**,
#1795 : *« 28 cellules de fidélité (pk 54, 162, **128**, **176** …) »*). L'artefact ayant été
touché pour la dernière fois le **30/09 15:36**, il **précède de 7 jours** l'unique geste qui l'a
périmé, et il est en retard **exactement** sur les deux cellules que ce geste a déplacées. Une
péremption à cause unique et nommable — pas une dérive diffuse.

⚠️ **Ce que cette table ne dit pas** : **lequel des deux états est juste**. Elle constate une
divergence entre le **corpus versionné** et l'**artefact committé**. L'arbitrage de contenu
n'appartient pas à cette mesure — et la ligne « pk 128 : *solide* → *acceptable* » du dashboard
montre que la direction du mouvement a déjà été lue dans les deux sens selon l'instrument.

---

## 3. Le coût que l'item ne nommait pas : **régénérer renomme des IRIs**

`VirtueOwlGeneratorConfig.cs:334` :

```csharp
var virtueId = GetId(targetVirtue.TitleEn);      // GetId = Camelize() + strip "'-," et espaces
var virtueUri = $"{OntologyNamespace}{virtueId}";
```

L'identifiant d'une vertu est donc **dérivé de son titre anglais**. Conséquence mécanique : les
deux divergences du §2 sont aussi **deux renommages d'IRI** si l'on régénère.

| Sujet | IRI aujourd'hui (artefact) | IRI après régénération (dérivé du CSV) |
|---|---|---|
| pk 128 | `#acceptableInformalLogic` | `#solidInformalLogic` |
| pk 176 | `#takingTheOpponentsIdeologicalBiasesIntoAccount` | `#takingThePartiesIdeologicalBiasesIntoAccount` |

Et l'identifiant **n'est pas local à l'OWL** — il vit dans un **second artefact committé** :
`docs/ontology/aif-export/aif-virtues-good-tenor.csv` porte `takingTheOpponentsIdeologicalBiases…`
(mesuré : la chaîne y est présente). L'OWL est par ailleurs **servi publiquement**
(GitHub Pages — cf. `README.md` du dossier), et ses concepts sont déréférençables ⇒ un
renommage est une **rupture pour les consommateurs**, pas une retouche de libellé.

⚠️ **Le générateur ne « met à jour » pas un libellé : il change le sujet.** Régénérer sans traiter
ce point produirait un artefact dont les IRIs ne correspondent plus à ceux que les consommateurs
ont enregistrés — et les deux artefacts committés divergeraient entre eux si l'export AIF n'était
pas régénéré dans le même geste.

---

## 4. Ce que devient l'item de pool

| Branche | Contenu | Coût | Effet |
|---|---|---|---|
| **(a)** | **Ne rien régénérer** ; consigner les 2 divergences de libellé | 0 | l'artefact reste en retard de 2 libellés EN ; **aucun IRI ne bouge** |
| (b) | Régénérer l'OWL **et** l'export AIF, en assumant 2 renommages d'IRI | 1 geste + propagation | artefact aligné sur le corpus au prix d'une rupture d'identifiants |
| (c) | Découpler : stabiliser d'abord l'IRI (identifiant figé par PK), **puis** régénérer | grain à part | alignement **sans** rupture, mais différé |

La mesure ne tranche pas (a)/(b)/(c) : elle dit que **(b) est le seul qui casse quelque chose**, et
que la question « un IRI de vertu peut-il être renommé ? » est une **décision**, à porter au
registre / à l'owner, pas une conséquence technique à absorber en silence.

---

## Gates

- ❌ Lecture seule : aucun `--generate-owl` exécuté, aucune écriture, aucun artefact touché.
- ❌ Aucun verdict sur la justesse des libellés (contenu = lane corpus / owner).
- ❌ Pas de verdict QA (ai-01).

## Sources

- `docs/ontology/argumentum_virtues.owl` (`c1491de9`, 30/09) · `docs/ontology/README.md`
  (procédure `--generate-owl`, `Program.cs:397`)
- `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (`37aae03a`, 223 rangées, 8 colonnes `title_*`)
- `Generation/Converters/Argumentum.AssetConverter/Ontology/VirtueOwlGeneratorConfig.cs:334`
  (`GetId(TitleEn)`) et `:193` (nom de schéma, même dérivation)
- `docs/ontology/aif-export/aif-virtues-good-tenor.csv` (second porteur de l'identifiant)
