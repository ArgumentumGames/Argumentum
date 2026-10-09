# #1781 I3 — vérification de la graine EN de la zone 2 (2026-10-09)

Volet **vérification** d'I3 (« vérifier + injecter »), en **lecture seule** (SELECT sur
`ArgumentumGames`, préprod). Le volet écriture n'est pas ouvert : la mesure ci-dessous
suffit à poser la décision qui le conditionne (cf. § Décision).

Suite d'[`1781-i2-zone3-inventory.md`](1781-i2-zone3-inventory.md), dont elle **corrige
un chiffre** (§ 2).

## 1. L'appariement est total — et c'est le seul lien exact

App 60 (zone 3, CT `Fallacy` = `ce2041a3-…`, id 376) ↔ app 29 (zone 2, même StaticName,
id 208) : **1 013 / 1 013 entités appariées par `EntityGuid`** (100 %).

| Contrôle | Résultat |
| --- | --- |
| Attributs de part et d'autre | **14 / 14 identiques** — mêmes `StaticName`, mêmes `SortOrder` |
| Porteurs de texte | `Name` (0) · `Description` (1) · `Example` (2) ; les 11 autres = liens/techniques (Entity, Boolean, Number, DateTime) |
| `KeyString` / `KeyNumber` / `Version` (app 60) | `NULL` / `1` / `1` sur **1 013/1 013** ⇒ **aucune clé métier** |

⇒ L'`EntityGuid` est le **seul** appariement exact disponible entre les deux bases.
Tout autre pivot (nom, ordre, identifiant) est une heuristique.

## 2. La graine EN est à **47 %**, pas « complète »

Valeurs d'app 29 (CT 208) sur les 1 013 entités appariées, par dimension :

| Attribut | dim 3 = **en-US** (lignes / **non vides**) | dim 4 = fr-FR (lignes / non vides) |
| --- | --- | --- |
| `Name` | 1 013 / **601** | 3 / 3 |
| `Description` | 1 013 / **607** | 1 013 / 922 |
| `Example` | 1 013 / **247** | 1 011 / 797 |
| **Total EN** | 3 039 / **1 455 = 47 %** | — |

⚠️ **Correction d'I2** : « 3 046 valeurs en-US = tout le triplet » comptait des **lignes**,
pas des valeurs non vides. Un rang existe pour chaque cellule ; il est souvent vide.
Le chiffre « 3 046 » reste vrai comme compte de rangs — il ne dit rien de la complétude.

**Contrôle de langue** (échantillon) : les valeurs dim 3 sont bien de l'anglais
(`Insufficiency`, `Slovenly argument`…), dim 4 bien du français — la graine n'est pas
une copie croisée.

## 3. Le défaut d'app 60 est un **mélange**, et le gain réel n'est que de 855 cellules

État de la valeur **par défaut** (non dimensionnée = ce que sert aujourd'hui un visiteur
en `fr-FR` **comme** en `en-US`) confrontée à la graine :

| Attribut | défaut = EN (déjà) | défaut = FR, EN disponible | défaut vide | EN absente |
| --- | ---: | ---: | ---: | ---: |
| `Name` | **598** | 3 | 412 | 412 |
| `Description` | 0 | **607** | 91 | 406 |
| `Example` | 2 | **245** | 214 | 766 |
| **Total** | **600** | **855** | 717 | 1 584 |

Trois conséquences :

1. **Injection ≠ traduction visible sur 600 cellules** : le nom affiché est **déjà**
   l'anglais (`Anecdotal evidence`, `Argument from hearsay`…). Les injecter rend la
   langue *explicite* sans changer un caractère.
2. **Le gain visible porte sur 855 cellules** (Description 607 + Example 245 + Name 3) —
   aujourd'hui servies en français sur le site EN.
3. **706 cellules n'ont rien** (ni défaut ni EN) : `Name` 412 · `Example` 203 ·
   `Description` 91. Une injection ne les remplit pas ; il faudra une source (cf. § 4).

## 4. La source alternative (corpus du dépôt) n'est pas substituable mécaniquement

`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (1 408 lignes) :

| Colonne | Couverture |
| --- | --- |
| `text_en` / `desc_en` | **1 408 / 1 408 (100 %)** |
| `example_en` | 1 401 / 1 408 (99,5 %) |

Corpus **complet** — mais **l'appariement aux 1 013 entités du site ne couvre que 490
(48 %)** par nom normalisé, dont **473 via le nom *anglais*** et 17 via le `text_fr`.
Les 523 restants nomment autrement (« Fallacies » côté site pour « Argument
fallacieux » côté corpus, etc.).

⇒ Le corpus est la **seule** source capable de couvrir les 1 584 cellules sans EN, mais
l'y brancher suppose un appariement flou **plus une validation humaine** (une erreur
d'appariement écrirait la description d'une autre notion). Ce n'est pas un geste
mécanique, c'est un grain.

## 5. Ce que l'écriture devra savoir (mesuré, pas supposé)

- **`TsDynDataEntity.Json` est vide sur 4 050 / 4 050** entités (CT 376 et 208) ⇒
  aucun cache JSON à tenir cohérent ; `TsDynDataValue` + `TsDynDataValueDimension`
  sont la source unique. (L'écriture d'I3 est un **INSERT** + une ligne de
  dimension, pas un UPDATE comme R-site : identité `ValueId` à vérifier à ce moment-là.)
- Zone 3 : dim 7 = en-US **active, 0 valeur** ⇒ rien à fusionner, l'injection est pure.
- Recycle app-domain requis après écriture + contrôle **sur la page servie** en `fr-FR`
  **et** `en-US` (les deux cultures lisent le même défaut aujourd'hui).

## Décision à porter à l'owner (registre)

| Branche | Contenu | Coût | Effet visible |
| --- | --- | --- | --- |
| **(a) recommandée** | Injecter les **1 455** valeurs EN disponibles (appariement GUID, exact) | 1 geste, réversible (backup) | 855 cellules passent FR → EN ; 600 rendues explicites ; 706 restent vides |
| (b) | Construire d'abord l'appariement corpus ↔ entités, puis injecter l'EN complet | grain à part + validation humaine | site EN complet, mais différé |
| (c) | Ne rien faire | 0 | le site EN reste majoritairement français |

Les deux premières ne s'excluent pas : (a) est un état intermédiaire **strictement
meilleur** que (c), et n'empêche pas (b) ensuite (les valeurs de (b) écraseraient
celles de (a) sur les cellules concernées).

⚠️ **Tension de doctrine, à trancher par l'owner — pas par la lane.** L'arbitrage
#1502 a posé « **les cartes font foi** » : le texte *publié* suit la source versionnée,
pas la copie d'un magasin historique. Appliqué à l'anglais, ce principe désigne le
**corpus** (§ 4, branche b), pas la base de l'ancien portail 0 (branche a) — dont le
millésime est inconnu (valeurs `Date` = 2022) et dont 40 % des cellules sont vides.
La mesure ne départage pas les deux : elle dit que (a) est disponible **maintenant**,
exactement apparié et réversible, et que (b) est complet mais coûteux. La reco
ci-dessus privilégie (a) parce qu'elle sert la priorité owner du 08/10 (« site EN :
PRIORITÉ 1 ») sans fermer (b) ; elle doit céder si l'owner veut que l'EN du site
reflète le corpus dès le premier jour.

## Gates

❌ lecture seule (SELECT) · ❌ aucune écriture DB/portal · ❌ zéro donnée personnelle
(noms et textes de taxonomie) · ❌ pas de verdict QA (ai-01).
