# #994 — Grain A+B : les 2 cellules Tournesol sont écrites, la branche A est **arrêtée sur delta** (47 → 71)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `874a5d98`
**Révision** : **2** (la révision 1 portait des chiffres périphériques faux — voir §6)
**Statut** : **1 mutation CSV (B, 2 cellules)** + **1 delta déclaré AVANT écriture (A)**.
⛔ `0` cellule du volet voix écrite · ⛔ aucune régénération · ⛔ aucune branche choisie ici.
**Dispatch** : ai-01, deep-queue #458 c.5657210653, grain **④** (« #994 A+B — une PR CSV Scenarii »).
**GO amont** : commentaire owner sur #994 (2026-09-14, publié par ai-01 sous le token partagé).
**Doc amont** : `994-arbitrages-AB-reestampe-2026-09-14.md` (PR #1362, révision 2).

---

## §0 Le résultat en une phrase

**B est écrit** — les 2 cellules Tournesol certaines, exactement celles du GO, byte à byte.
**A ne l'est pas**, et c'est volontaire : le chiffre du GO (**47**) est re-dérivé **exact**, mais
**47 ne réalise pas l'objectif énoncé** (« normaliser l'EN à la troisième personne »). Le périmètre
complet est **71 cellules** ; écrire les 47 seules résout **16** cartes et en casse **9**.

---

## §1 Ce qui a été écrit — branche B, 2 cellules

Fichier : `Cards/Scenarii/Argumentum Scenarii - Cards.csv`, rangée `path = 5.1.1`
(`coordonnées = 5,0101`), **une seule ligne, deux champs** :

| colonne | idx | avant | après |
|---|---:|---|---|
| `drawer` (EN) | 15 | `Sunflower` | **`Cuthbert Calculus`** |
| `drawer_ru` | 23 | `Подсолнух` | **`профессор Лакмус`** |

Le FR canonique est `Tournesol` — le **professeur Tournesol** de Tintin. EN et RU portaient une
traduction **littérale du nom français** (tournesol = sunflower = подсолнух) au lieu du nom
canonique de chaque langue.

**Méthode — splice par offsets d'octets, pas de round-trip CSV.** Le fichier est lu en binaire, la
ligne cible découpée par un splitter **quote-aware** qui rend les bornes d'octets de chaque champ ;
seules les deux plages visées sont remplacées. Aucun ré-encodage, aucun re-quoting.

**Contrôles passés** (assertions, pas d'inspection visuelle) :

| contrôle | résultat |
|---|---|
| BOM | **absent** avant **et** après |
| fins de ligne | **168 CRLF**, **0 LF nu** — inchangé |
| lignes du fichier | 169 avant / 169 après |
| lignes **modifiées** | **1** (index 112) — assertion bloquante |
| champs modifiés sur cette ligne | **exactement [15, 23]** |
| taille | 556 796 → 556 817 o (**+21**) |
| `git diff --numstat` | `1 insertion, 1 suppression` |

**Les 6 autres langues sont byte-identiques** : `Tornasol` (es), `Girassol` (pt), `向日葵教授` (zh),
`تورنسول` (ar), `پروفسور تورنسل` (fa), `Tournesol` (fr).
**Les 3 cellules Schtroumpfs PT** (`smoothTalker_pt`, `suggestion_pt`, `context_pt`) restent
**hors périmètre** — aucune n'a été écrite.

> ⚠️ Non-collision : la carte `5,0101` porte aussi un `context` EN en 2ᵉ personne, donc elle figure
> dans la worklist A. Le champ écrit ici (`drawer`) **n'est pas** l'un des 10 `drawer` de A (le sien
> valait `Sunflower`) : les deux volets sont **orthogonaux**.

---

## §2 Branche A — le delta, déclaré **avant** d'écrire

### A.0 L'instrument, d'abord — parce qu'il a menti deux fois

Les colonnes **rendues** en anglais sont fixées par `ScenariiLocalizationTests.cs:60` :
`title`, `category`, **`context`**, **`issue`**, **`smoothTalker`**, **`drawer`**. Le template FR
(`Argumentum_Scenarii_Face_fr.json`) porte `contexte`/`enjeu`/`baratineur`/`piocheur`/`titre` ; la
couche de localisation les rebinde vers ces colonnes EN — `issue` **est donc bien rendu sur la
carte**, et le normaliser a un effet visible.

**Sonde retenue — le pronom explicite.** Une cellule est comptée 2ᵉ personne si elle contient un
pronom 2ᵉ personne explicite (`you/your/yours/yourself`). Cette sonde est **non ambiguë**, et rend
sur les trois colonnes de voix **exactement 25 / 12 / 10 = 47** — le chiffre du GO, au chiffre près.

**Pourquoi pas « toute cellule à l'impératif ».** Une sonde par mot-clé de verbe, ou par « absence
de sujet 3ᵉ personne », a été essayée et **se casse sur deux populations** : les colonnes de **noms
propres** (`drawer` = « Cuthbert Calculus » n'a aucun sujet → classé 2ᵉ personne à tort) et les
impératifs **mentionnant un tiers** (« Convince **her** to stay » → le pronom tiers fait classer 3ᵉ
à tort). Les deux passes ont donné 135 puis 162 cellules « 2ᵉ personne » sur les 3 colonnes de voix,
contre 47 pour le GO : **un facteur 3, entièrement dû à l'instrument.** La classification
« personne » d'une cellule n'est pas mécaniquement décidable par expression régulière.

### A.1 Le chiffre du GO, re-dérivé à l'unité

| surface | GO | **re-mesure (pronom explicite)** |
|---|---:|---:|
| `context` | 25 | **25** ✅ |
| `smoothTalker` | 12 | **12** ✅ |
| `drawer` | 10 | **10** ✅ |
| **total** | **47** | **47** ✅ |

**Le chiffre du GO est exact.** Le problème n'est pas le nombre — c'est sa **suffisance**.

### A.2 Pourquoi 47 ne suffit pas — la quatrième colonne

La 2ᵉ personne de l'anglais de Scenarii ne vit pas dans trois colonnes mais dans **quatre**. La
quatrième, `issue`, est **rendue** (cf. A.0) et **absente des 47** :

| colonne | cellules 2ᵉ pers. |  |
|---|---:|---|
| `context` | 25 | dans les 47 |
| `smoothTalker` | 12 | dans les 47 |
| `drawer` | 10 | dans les 47 |
| **`issue`** | **24** | ❌ **hors des 47** |
| **total** | **71** | |

Les **24 cellules `issue`** se répartissent en deux populations, et c'est ce partage qui décide :

| population | n | effet d'écrire les 47 seules |
|---|---:|---|
| cartes **dans** les 25 `context` | **9** | ⚠️ **bascule neuve** : narration à la 3ᵉ, consigne toujours à la 2ᵉ |
| cartes **hors** des 25 (contexte déjà à la 3ᵉ) | **15** | inchangées — elles basculent **déjà** aujourd'hui |

**Les 9 bascules neuves, vérifiées carte par carte** (FR uniformément à la 3ᵉ, EN `context` à la 2ᵉ,
EN `issue` à l'impératif) :

| `pk` | FR `contexte` / FR `enjeu` | EN `context` / EN `issue` |
|---|---|---|
| `3,0104` | *Le baratineur se réveille… / Il doit la convaincre d'aller prendre un café…* | `You wake up… / Try to convince her to go for coffee…` |
| `4,0301` | *Le baratineur est une licorne. / Il doit convaincre un éminent zoologue…* | `You're a unicorn. / You must convince an eminent zoologist…` |
| `7,0102` | *Le baratineur a poussé un camarade… / Le baratineur doit se justifier auprès de la maîtresse.* | `You're a kid at school… / Justify the incident to the teacher.` |
| `7,0103` | *Le baratineur est le père Noël… / Il doit se justifier auprès de l'enfant.* | `You're Santa Claus… / Justify the lack of a present to the child.` |
| `7,0104` | *Le baratineur est la mère d'un nouveau-né… / …doit éviter d'être accusé d'adultère.* | `You are the mother of a newborn… / Avoid being accused of adultery.` |
| `7,0105` | *Après une énorme gueule de bois… / Il doit expliquer à son enfant…* | `Following a massive hangover… / Explain to the child that attending the fair is impossible.` |
| `7,0107` | *Le baratineur est un chat au régime. / Il essaie de convaincre son maître…* | `You're a cat on a diet. / Convince your master to open a tin of sardines in oil.` |
| `7,0201` | *Le baratineur a donné par inadvertance un muffin au cannabis… / Il doit s'en expliquer…* | `You inadvertently gave a cannabis muffin… / Explain the situation to the nephew's parents.` |
| `7,0206` | *Le baratineur produit du foie gras en Périgord. / Il demande à son voisin végétarien…* | `You produce foie gras in the Périgord. / Ask the vegetarian neighbor to taste the new recipe.` |

### A.3 Le bilan chiffré — les 47 **améliorent** sans **achever**

Le décompte des cartes **mixtes** (colonnes de voix rendues en désaccord de personne **à
l'intérieur d'une même carte**), par la même sonde déterministe :

| état | cartes mixtes |
|---|---:|
| **aujourd'hui, sans écriture** | **31** — 16 (narration 2ᵉ + consigne 3ᵉ) + 15 (narration 3ᵉ + consigne 2ᵉ) |
| après avoir écrit **les 47** | **24** — les 16 résolues, les **9** nouvelles, les 15 inchangées |
| après avoir écrit **les 71** | **0** |

⇒ Les 47 sont un **progrès net (−7)**, pas une régression. Mais elles **laissent 24 cartes mixtes**
et en **créent 9** d'un type neuf. L'objectif énoncé — « normaliser l'EN à la troisième personne » —
**n'est pas atteint.**

### A.4 La worklist préparée — **71 cellules**

| colonne | n | cartes (`coordonnées`) |
|---|---:|---|
| `context` | 25 | `3,0104` `3,0305` `3,0306` `3,0311` `4,0101` `4,0103` `4,0202` `4,0203` `4,0301` `4,0302` `5,0101` `5,0102` `5,0201` `6,0101` `6,0201` `6,0301` `7,0102` `7,0103` `7,0104` `7,0105` `7,0107` `7,0201` `7,0206` `7,0301` `7,0302` |
| `smoothTalker` | 12 | `3,0101` `3,0102` `3,0103` `3,0104` `3,0203` `3,0306` `3,0311` `4,0202` `4,0208` `4,0302` `7,0207` `7,0301` |
| `drawer` | 10 | `3,0102` `3,0103` `3,0202` `3,0203` `3,0306` `3,0311` `4,0208` `4,0302` `7,0106` `7,0207` |
| `issue` | 24 | les **9** du tableau A.2 **+** `1,0301` `2,0101` `3,0102` `3,0106` `3,0107` `3,0108` `3,0202` `3,0203` `4,0102` `5,0203` `7,0106` `7,0203` `7,0204` `7,0205` `7,0303` |
| **total** | **71** | |

**La cible n'est pas à inventer : le FR canonique est la référence 3ᵉ personne de chaque carte.**
Sur les 71 cellules, le FR est à la 3ᵉ personne dans **71 cas sur 71** — la réécriture EN est donc
mécanique et corroborable ligne à ligne, exactement comme le GO le justifie.

---

## §3 Pourquoi je n'ai pas écrit A

**Parce que le GO me l'ordonne, noir sur blanc.** Le commentaire owner porte sa propre clause
d'arrêt :

> « La re-mesure détaillée reste portée par la PR #1362 ; **si son chiffre diffère avant écriture,
> le worker doit publier le delta et arrêter plutôt que forcer l'attendu**. »

Le chiffre 47 **est confirmé** ; ce que la re-mesure met au jour, c'est que **47 ne réalise pas
l'attendu**. Écrire 58 de ma propre initiative serait l'erreur symétrique : **dépasser le périmètre
autorisé**. Le GO autorise 47 ; il n'autorise pas 71.

⇒ **Le geste juste était de s'arrêter et de publier le delta.** C'est ce qui est fait ici.

---

## §4 Ce qu'il faut de l'owner

Une décision, sur un écran :

| | |
|---|---|
| **La question** | La branche A est-elle étendue des **47** cellules du GO aux **71** réellement nécessaires (ajout des **24 `issue`**) ? |
| **Option 1 — 71** *(recommandée)* | Seul périmètre qui atteint l'objectif : EN entièrement à la 3ᵉ, **0** carte mixte. Ajoute 24 cellules, toutes mécaniques depuis le FR (71/71). |
| **Option 2 — 47** | Progrès réel (31 → 24 cartes mixtes) mais **partiel** : 24 cartes restent mixtes, dont **9** d'un type neuf. Il faut l'assumer explicitement. |
| **Option 3 — 62** (47 + les 15 `issue` hors-25) | Corrige les cartes qui basculent **déjà**, laisse les **9** en bascule neuve. Peu cohérent : c'est choisir de casser 9 cartes tout en réparant 15. |
| **Ce qui ne change pas** | `smoothTalker`/`drawer`/`context`/`issue` sont **rendus** ⇒ le lot invalide les cartes EN. |
| **Ordre — impératif** | A **puis** les apostrophes (PR #1363, §6) : l'ordre inverse réécrirait deux fois les mêmes cellules. |

---

## §5 Périmètre tenu

⛔ `0` cellule du volet **voix** écrite · ⛔ `0` cellule Schtroumpfs PT · ⛔ `0` OWL ·
⛔ aucune régénération · ⛔ aucune branche choisie · ⛔ `link_*` non touché (URLs).
**Une seule** mutation dans cette PR : les 2 cellules du §1, explicites dans le GO
(« corriger **immédiatement** les deux cellules Tournesol certaines »).

---

## §6 Correction — ce que la révision 1 de ce document disait de faux

La révision 1 annonçait **11** cellules `issue`, **58** de périmètre, **18** cartes basculant
aujourd'hui, et présentait les 47 comme une **régression** (« dégrade 9 cartes »). **Quatre chiffres
faux**, tous du même instrument.

- **Le fond tenait** : les **9** cartes en bascule neuve, et **la liste exacte de ces 9**, sont
  identiques dans les deux révisions — c'était la mesure qui portait la décision, et elle a survécu.
- **La périphérie était fausse** : la sonde `issue` de la révision 1 ne balayait qu'un **sous-ensemble**
  — d'où 11 au lieu de **24**. C'est exactement le motif « la portée de la sonde doit couvrir la
  portée du claim » : le claim portait sur la **colonne entière**, la sonde sur une tranche.
- **Le sens était faux** : 47 n'est pas une régression mais un **progrès partiel** (31 → 24 cartes
  mixtes ; 16 résolues, 9 cassées). La révision 1 ne comptait que les 9 cassées.
- **Ce qui a changé la mesure** : compter les cartes **mixtes** (désaccord **à l'intérieur** d'une
  carte) plutôt que les seules bascules neuves, et sonder la **colonne entière**.

*Leçon d'instrument, à porter au dossier* : une sonde par expression régulière sur la « personne »
grammaticale **n'est pas fiable** ici — les colonnes de noms propres et les impératifs mentionnant un
tiers la font diverger d'un facteur 3. Seule la sonde par **pronom explicite** est non ambiguë, et
elle seule doit servir à **délimiter** un périmètre ; le reste se lit cellule par cellule.

---

*po-2024 — grain ④ du dispatch #458 c.5657210653. Le worker mesure et signale ; l'extension de
périmètre est une décision owner.*
