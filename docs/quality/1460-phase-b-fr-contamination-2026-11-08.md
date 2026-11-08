# #1460 phase B — organe 2 : aucun mot-outil français ne survit dans une colonne non française

Organe : `Generation/Converters/Argumentum.AssetConverter.Tests/Localization/FrenchContaminationGuardTests.cs`
(10 tests, mesurés verts le 08/11/2026 sur `db037c78` + le fix de corpus de cette PR).
Contrat de phase (corps de #1460) : « FR dans du non-FR (sondes par langue, liste nommée) ».

## 1. Ce que l'organe assert

Un mot-outil français dans une colonne localisée non française est un défaut, **zéro toléré** —
assertion structurelle, pas une proportion (la proportion est l'organe 3). C'est la famille de
défaillance de #216 (PDFs EN/RU/PT livrés en français : la carte de conversion nommait des champs
que le gabarit n'utilisait pas) et du bug Scenarii pré-`37600e4a` (colonnes non traduites).

Sept tests nommés par langue (en, ru, pt, es, ar, fa, zh) + anti-vacuité + contrôles inverses.

## 2. Pourquoi cette source, et pas une autre

Même source que l'organe 1 (`PlaceholderResidueGuardTests`) : **gabarit × CSV**, jamais le PDF
(0 couche texte) ni le `.harvest.json` (pixels). Concrètement deux couches :

1. **Corpus** — toutes les colonnes localisées (`_en _ru _pt _es _ar _fa _zh`) des CSV des DataSets
   déclarés par les CardSets activés, **plus** les trois colonnes nues anglaises de la taxonomie
   (`Family`, `Subfamily`, `Subsubfamily` — 4 187 cellules mesurées, 0 atteinte), **moins** les
   colonnes `link_*` (une URL est latine par construction : mesuré 08/11, tous les suspects
   « latin-script » des colonnes ar/fa/zh de Fallacies sont des URL légitimes). Le corpus est sondé
   en entier, pas seulement les colonnes référencées par les gabarits : les colonnes circulent par
   la synchro GSheet et un gabarit futur peut en consommer une — le vrai positif trouvé à la
   calibration vivait exactement là.
2. **Embarqué** — pour les CardSets sans DataSet (les dos), le `csv` embarqué EST la source de
   données (note #813/#812 de l'organe 1) ; colonnes localisées sondées quand leur radical est
   référencé par le mustache (`Argumentum_Fallacies_Back_fr.json` référence `tagline_fr`, la carte
   de conversion y injecte `tagline_<lang>`). Les csv embarqués des ensembles QUI déclarent un
   DataSet sont des snapshots périmés et ne sont PAS sondés : mesuré 08/11, ils portent encore une
   note de traducteur 2022 en français dans `example_en` (ligne 49) alors que le jumeau corpus est
   propre.

## 3. Calibration — mesurée, pas supposée

Balayage initial (Python, 6 CSV du corpus, colonnes localisées hors `link_*`) :

| Sonde brute | Atteintes | Verdict |
|---|---|---|
| `les` en es | 17 | **espagnol légitime** (pronom datif : « les permitía », « les gusta ») |
| `une` en es | 1 | espagnol légitime (3ᵉ pers. de *unar/unir* : « que nos une ») |
| `une` en pt | 2 | portugais légitime (« que nos une », « Você une ») |
| `faux` en en | 1 | **VRAI POSITIF** : PK 753 `Simple_name_en` = « Confondre le faux et l'impossible » |
| toutes les autres | 0 | propre |

**Le balayage C# (frontières par lookarounds Unicode, pas par espaces)** a rattrapé un angle mort du
balayage Python : « Déjà vu » en `title_es` et `title_pt` d'une carte Scenarii — emprunt
international, sondé « déjà » retiré du calibre par **exemption nominale d'expression** (« déjà vu »),
pas par retrait du mot (un « déjà » hors de cette expression reste une atteinte).

### Listes nommées

- **Couche accentuée** (sans homographe dans les 7 langues, appliquée partout) : très, après, être,
  était, même, déjà (hors emprunt).
- **Couche base** (mots-outils sans accent) : avec, dans, aux, dont, cette, moins, depuis, toujours,
  ainsi, chez, selon, faut, peut, doit, reste, contre, rien, des, du, alors, et, les, une, pour,
  pas, chose, faux, dire, jamais.
- **Exclusions par langue** (chaque retrait mesuré ou lexical, voir la doc du code) :
  - **en** : pour (verbe anglais), pas (« pas de deux »), chose (prétérit de *choose*), dire
    (adjectif anglais). *Conservés malgré un risque d'emprunt, 0 usage légitime mesuré* : faux
    (« faux pas »), et (latin « et cetera »), les/une/des/du.
  - **es** : les (17 cellules), une (1).
  - **pt** : une (2), jamais (mot portugais).
  - **ru/ar/fa/zh** : aucune — écriture différente, tout mot-outil latin est étranger.

### Le vrai positif, réparé dans la même PR

PK 753 (« Erreur de modalité aléthique ») portait « Confondre le faux et l'impossible » dans
`Simple_name_en` — la seule cellule française d'une colonne anglaise de 61 cellules (les 60 autres
sont anglaises : « Bold guess », « Manifest Destiny »…). Remplacée par « Confusing the false with
the impossible », formulation alignée sur le `desc_en` de la même ligne. La colonne n'est consommée
par aucun gabarit vivant (Archive/2022 et exemples CardPen seulement — mesuré) : **zéro impact de
rendu**. Édition chirurgicale au niveau octet (une occurrence, diff = 1 cellule).

## 4. Ce que les contrôles inverses prouvent

- **Réintroduction historique** : la cellule française de PK 753 est remise dans une copie en
  mémoire → le jeu en doit la détecter (par « faux »/« et »). De même une phrase française fraîche
  injectée en colonne russe → détectée par la couche accentuée. L'organe détecte, il ne transpose pas.
- **Exclusions porteuses** : le balayage BRUT (sans filtrage) doit retrouver ≥10 « les » espagnols
  et ≥1 « une » portugais — les exclusions sont calibrées sur des cellules qui existent. Si une
  passe future réécrit ces prose légitimes, le contrôle passe rouge et dit de re-mesurer, pas de
  supprimer l'exclusion. Même discipline pour l'emprunt « déjà vu » (≥1 cellule).
- **Anti-vacuité** : ≥5 tables lues, ≥8 000 cellules sondées par langue (mesuré : en 9 990,
  autres 11 063) — un DataSet qui cesserait de charger fait tomber le plancher, pas les gardes.

## 5. Ce que cet organe n'établit pas

- Il ne sonde pas les **gabarits** eux-mêmes (HTML/CSS du mustache) — c'est le rôle du garde
  sosies cyrilliques (`CyrilliqueSosieGabaritsGuardTests`) et, pour le texte mort des gabarits,
  de l'organe 1. Frontière citée pour ne pas doubler.
- Il ne mesure pas les **proportions d'écritures** (cyrillique/CJK/arabe) — organe 3 de la phase,
  avec seuil nommé.
- Les csv embarqués périmés des ensembles à DataSet ne sont pas sondés (§2) : leur contenu n'est
  jamais rendu ; le jour où un ensemble bascule à `DataSet = None`, il entre dans le périmètre.

## 6. Mesure de la suite

L'organe 3 (proportion d'écritures, seuil nommé et justifié) complète la phase B ; la réécriture de
la phase B est déjà faite (#2185/#2186) et ne se rejoue pas.
