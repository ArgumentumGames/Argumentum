# #1369 — Cover illustrée par variante de jeu : dossier owner tranchable

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `c089d526`
**Instrument** : re-mesure du CSV **vivant** du DataSet `Cards/Rules/Argumentum Rules - Cards.csv`
(15 lignes — c'est lui que le runtime rend : `HarvestManager.cs` fait
`CardSetDocument.csv = dataSet.GetContent(...)`, config `AssetConverterConfig.cs:74`), du
template `Cards/Rules/Argumentum_Rules_fr.json` (CSS 16 391 car. ; sa clé `csv` embarquée est
**STALE** par sa propre note `_csv_note` : *« overridden at runtime by DataSet='Rules' … this key
is ignored »*), du mécanisme `cardClass` (`Generation/CardPen/js/main.js:1387`), des assets
`Cards/Rules/Assets/`. **Aucune image générée, aucune ligne de code écrite** — ce
dossier est le support de décision, pas son exécution (conforme au body #1369).
**Corrigé le 15/09 après review** (c.5677149869 + c.5677200800) : la première édition publiait
« 18 lignes » pris sur le CSV embarqué stale — cf. §0.
**Statut** : **PROPOSITION TRANCHABLE** — 4 décisions, chacune avec objet visible, branches
chiffrées et coût. Qualification MESURÉ / RAPPORTÉ / DÉRIVÉ / SUPPOSÉ par ligne.

---

## §0 Ce qui est mesuré (et ce que la première édition avait faux)

Le body #1369 posait le bon diagnostic structurel **et le bon chiffre de cartes**. La première
édition de ce dossier le « corrigeait » à tort : sa mesure « 18 lignes » était prise sur le CSV
**embarqué** du gabarit, que sa propre clé `_csv_note` déclare STALE et que le runtime écrase
(`HarvestManager.cs` : `CardSetDocument.csv = dataSet.GetContent(...)`). Un seul point de
correction subsiste :

| point | le body dit | mesuré ce jour (instrument vivant) | conséquence sur la décision |
|---|---|---|---|
| **Nombre de cartes** | « 15 cartes », sélecteurs `[class~="1"]…[class~="15"]` | **exact** : le CSV vivant du DataSet (`Argumentum Rules - Cards.csv`) tient **15 lignes** (MESURÉ) ; les 5 covers sont aux index **1, 7, 9, 11, 13** ; les sélecteurs 14/15 existent (5 occ. chacun). La piste « 18 lignes » de la 1ʳᵉ édition = mesure sur le csv **embarqué stale** (instantané d'avant édition de contenu, mêmes 5 variantes, pagination différente — pas une troncature). | aucune « carte sans sélecteur » ; les plages de la branche 1b sont **propres** sur 15 lignes : **1-6 · 7-8 · 9-10 · 11-12 · 13-15**. |
| **Portée multilingue** | « 8 langues » / « 40 images » (5×8) | **un seul template de règles vivant** : `Argumentum_Rules_fr.json` (MESURÉ, `ls Cards/Rules/*.json`) ; les autres langues sont des **colonnes** `Text_en`/`Text_ru`/… du **même** CSV. (Le 2ᵉ fichier `…_Francais_edition_fevrier_2022_Print_and_Play.json` est une relique figée de 2022.) | il n'y a **pas** 8 gabarits à dupliquer — il y a **un** gabarit dont le texte est localisé. Une cover **sans texte** est partagée par les 8 langues ; une cover **avec texte** suppose une surcharge par langue (coût ×N). |

La structure réelle (MESURÉ sur le CSV vivant) :

```
1-6   École des menteurs   (1 = COVER)  ┐ variante 1
7-8   Bingo mixologie      (7 = COVER)  ┐ variante 2
9-10  Dernier beau parleur (9 = COVER)  ┐ variante 3
11-12 Moulin à baratin    (11 = COVER)  ┐ variante 4
13-15 Parlote coinchée    (13 = COVER)  ┐ variante 5
```

**5 covers**, 5 variantes, adressées aujourd'hui **par index numérique** (CSS `[class~="N"]` +
`::before` portant le libellé), `cardClass` **vide**. Le mécanisme « adressé par donnée » existe
déjà dans CardPen : `main.js:1387` lit `data.cardClass` = **le nom d'une colonne CSV**, et ajoute
la **valeur** de cette colonne comme classe CSS de la carte. Le gabarit manquant est donc une
**colonne**, pas une invention.

---

## §1 Décision 1 — Mécanisme de gabarit (comment une cover devient adressable par donnée)

**Objet visible** : la carte 1 (cover « L'école des menteurs ») rendue aujourd'hui par le bloc
CSS `[class~="1"]`, et le champ `cardClass` vide dans le template.

Trois branches, diffs **incompatibles** (c'est pourquoi rien ne peut être codé avant) :

| branche | mécanisme | diff type | coût / risque |
|---|---|---|---|
| **1a — colonne `variant_class` + `cardClass="variant_class"`** *(reco)* | ajouter une colonne au **CSV vivant du DataSet** (`Argumentum Rules - Cards.csv`) portant `cover-ecole`, `cover-bingo`, … sur les 5 covers (vide ailleurs) ; renseigner `cardClass` dans le template ; écrire les règles `card.cover-<x>` | **CSV + template** (JSON) | réutilise le mécanisme **existant** de CardPen ; extensible (une nouvelle variante = une valeur de colonne) ; ⚠️ l'insertion de colonne dans le CSV vivant doit rester **byte-exacte** (mémoire `csv-byte-exact-column-insertion`) ; ⚠️ ne PAS écrire la colonne dans le `csv` embarqué du template — clé STALE, ignorée au runtime |
| **1b — plage d'index par variante** | garder l'adressage par index, documenter les plages **1-6 · 7-8 · 9-10 · 11-12 · 13-15** | **CSS seul** | zéro changement de donnée mais **fragile** : tout réordonnancement ou édition de contenu du CSV (la pente qui a fait diverger l'embarqué : 15 → 18 lignes) casse les plages silencieusement. Ne résout pas « adressé par donnée » (le but de #1369). |
| **1c — champ dédié au template** | un paramètre `coverIndices: [1,7,9,11,13]` lu par le pipeline pour injecter une classe | **config C# + template** | le plus lourd ; crée un 2ᵉ mécanisme parallèle à `cardClass`. À réserver si 1a est rejeté. |

**Recommandation worker : 1a.** C'est le seul qui rende la cover « adressable par la donnée »
(l'intention explicite du body), en réutilisant le hook CardPen existant, sans toucher aux 15
cartes. Coût d'exécution : 1 colonne CSV (5 valeurs) + `cardClass` + 5 blocs CSS.

> ⚠️ **Prérequis quelle que soit la branche** : le gel du corpus jusqu'au tag interdit l'écriture
> CSV/gabarit **avant le tag**. La décision peut être prise maintenant ; l'exécution attend le tag.

---

## §2 Décision 2 — Style / direction artistique (décrire avant de générer)

**Objet visible** : les 3 assets existants de `Cards/Rules/Assets/` (MESURÉ) :

| asset | taille | rôle actuel |
|---|---:|---|
| `bg-rules.jpg` | 65 Ko | fond rouge texturé commun |
| `rules-kids.png` | 123 Ko | illustration (enfants) |
| `rules-board.png` | 28 Ko | illustration (plateau) |

La cover actuelle n'est **pas illustrée** — c'est un titre typographique sur le fond texturé. Il
n'existe donc **pas encore de référence de style « cover illustrée »** : seule une charte partielle
(fond rouge, deux illustrations à plat). Deux branches :

| branche | objet | coût |
|---|---|---|
| **2a — brief de style écrit d'abord** *(reco)* | échantillonner `rules-kids.png` / `rules-board.png` / cover FR, en tirer une **fiche de style** (palette dominante, trait, cadrage, présence/absence de texte) **avant** toute génération ; la fiche est le contrat de la passe IA | 1 doc, 0 image. Évite de payer des générations incohérentes avec le deck. |
| **2b — génération exploratoire** | produire 2-3 visuels tests pour « voir » | coût déjà payé même si rejeté ; sans référence, fort risque d'incohérence (le body le signale). |

**Recommandation worker : 2a.** Le coût de 2b est « déjà payé » dès la première génération ratée ;
2a le paie une fois en documentation. La fiche de style est aussi le livrable qui rendra la passe
**reproductible** (DoD #1369 : « appliqué de façon reproductible, pas un one-off »).

---

## §3 Décision 3 — Budget (modèle, nombre d'images, coût)

**Objet visible** : le nombre d'images à produire, qui dépend directement de la décision 4. Le
body pose la fourchette « 5 variantes × 8 langues = 40, ou 5 partagées ». La mesure §0 **réduit
cette fourchette** : comme il n'y a qu'**un** gabarit à texte localisé, une cover **sans texte**
est partagée par les 8 langues → le vrai choix est :

| branche | images | hypothèse |
|---|---:|---|
| **3a — 5 images, sans texte, partagées 8 langues** *(reco)* | **5** | la cover porte son titre via le CSS existant (localisé), l'illustration est muette → 1 image/variante, 0 surcharge par langue |
| **3b — 5 images + surcharge texte par langue** | 5 à 40 | si l'illustration **intègre** du texte, il faut une version par langue (×8) — le texte d'une image n'est pas localisable par CSS |
| **3c — pilote 1 image** | **1** | une seule variante illustrée pour valider le pipeline (DoD minimal du body) avant d'étendre |

Le **modèle d'image** reste à nommer par l'owner (hors périmètre worker : politique de coût API).
À titre d'ordre de grandeur SUPPOSÉ : une passe de 5 images sur un modèle d'image courant est de
l'ordre du dollar ; ×8 langues (branche 3b) multiplie d'autant. **Le coût dominant n'est pas le
modèle, c'est le facteur langue** — d'où la reco 3a (texte hors image).

**Recommandation worker : 3a + 3c en rampe** — générer **1** cover (pilote, DoD minimal), valider
le rendu Debug + verdict ai-01, **puis** les 4 autres si le pilote passe.

---

## §4 Décision 4 — Périmètre (quelles variantes, quelles langues)

**Objet visible** : les **5 covers** aux index 1, 7, 9, 11, 13 (MESURÉ sur le CSV vivant, titres de variante).

| branche | variantes | langues |
|---|---|---|
| **4a — les 5 variantes, image muette partagée** *(reco)* | 5 covers | les 8 langues partagent la même image (texte en CSS localisé) |
| **4b — sous-ensemble** | 1 à 4 covers | idem — utile si certaines variantes ne doivent pas être mises en avant |
| **4c — image distincte par langue** | 5 × 8 | choix éditorial lourd ; le body le pose, la mesure §0 le rend **inutile** si l'image est muette |

**Recommandation worker : 4a**, alignée sur 3a. Le périmètre « 5 variantes » est le découpage
naturel du CSV (5 titres de variante). Le périmètre « langues » se réduit à « l'image est-elle
langue-neutre ? » — si oui (3a), 4c disparaît.

---

## §5 Les 4 décisions se réduisent à 2 vraies questions owner

La mesure §0 (un seul gabarit, texte localisé en colonnes) fait **collapser** le tableau 4×N du
body en deux questions indépendantes :

1. **Mécanisme** (Déc. 1) — `cardClass` par colonne (1a, reco) vs plage d'index (1b) vs champ
   dédié (1c). *Détermine le diff, bloque tout code.*
2. **L'illustration porte-t-elle du texte ?** — si **non** (reco) : Déc. 3 → 5 images partagées
   (3a), Déc. 4 → 5 variantes/8 langues sans surcharge (4a) ; si **oui** : tout passe en ×8 (3b/4c).
   La Déc. 2 (style) est **orthogonale** et recommandée quelle que soit la réponse (2a).

**Feuille à cocher par jsboige** (chaque ligne = une coche) :

| décision | choix owner |
|---|---|
| **1 — mécanisme** | ☐ 1a colonne+`cardClass` *(reco)* · ☐ 1b plage d'index · ☐ 1c champ dédié |
| **2 — style** | ☐ 2a fiche de style d'abord *(reco)* · ☐ 2b génération exploratoire |
| **3 — budget/nombre** | ☐ 3a 5 images muettes *(reco)* · ☐ 3b ×8 langues · ☐ 3c pilote 1 seule |
| **4 — périmètre** | ☐ 4a 5 variantes, image partagée *(reco)* · ☐ 4b sous-ensemble · ☐ 4c ×8 |

---

## §6 Ce que ce dossier n'établit pas

⛔ **Aucune image générée, aucune ligne de code, aucun CSV/template modifié** — le gel du corpus
jusqu'au tag tient ; ce dossier ne fait que rendre les 4 arbitrages **tranchables**. ⛔ Le **modèle
d'image** n'est pas choisi (politique de coût = owner) — seul le **facteur langue** est chiffré.
⛔ Aucun **verdict visuel** n'est rendu ni annoncé (réservé à ai-01). ⛔ La correction publiée
(« 1 gabarit vivant, pas 8 ») est une **mesure** ; elle ne préjuge pas du choix, elle chiffre
les branches — et le chiffre « 15 cartes » du body était **juste** (la « correction à 18 » de la
1ʳᵉ édition était un artefact d'instrument, cf. §0). ⛔ L'exécution (post-décision) reste gated
sur le **tag** + verdict **ai-01** +
respect des contraintes #1225/#1228 (asset neuf validable seulement en Debug ou après merge).

---

*po-2024 — pool #458, grain ⑫ / pool n°9. Le worker mesure les branches et les coûts ; les 4 coches sont à l'owner.*
