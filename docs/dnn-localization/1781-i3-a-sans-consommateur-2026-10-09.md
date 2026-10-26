# #1781 I3-(a) — arrêt avant écriture : la graine EN des 1 013 fallacies n'a **aucun consommateur** sur le site (2026-10-09)

Le volet **écriture** d'I3 a été ouvert le 09/10 (décision ai-01 « (a) puis (b) »), le plan
d'insertion a été construit — puis **le contrôle qui manquait** (« qui affiche ces
entités ? ») a été fait **avant** d'écrire. Il réfute le fondement de (a). Aucune écriture
n'a été faite ; l'objet de ce document est la mesure, pas un geste.

Suite d'[`1781-i3-en-seed-verification-2026-10-09.md`](1781-i3-en-seed-verification-2026-10-09.md)
(lecture seule, PR #1836).

## 1. Le plan d'insertion était prêt, propre, et sans surprise

| Contrôle | Mesure |
| --- | --- |
| Lignes à insérer | **1 455** — `Name` 601 · `Description` 607 · `Example` 247 |
| Appariement | `EntityGuid`, **1 013 / 1 013** |
| Cellule cible **sans** valeur par défaut | **0** ⇒ aucune insertion ne crée de trou |
| Valeur `en-US` (dim 7) déjà présente sur ces cellules | **0** ⇒ aucune collision |
| Défauts portant déjà une dimension | **0** ⇒ tous dimensionless, donc aucun écrasement |
| Valeur EN vide | **0** (par construction) |

Dispositif : `ValueId` est **IDENTITY** ⇒ la sauvegarde est la **liste des `ValueId` créés**
et le retour arrière une suppression ciblée ; dimension **7 = en-US, `Active = 1`, 0 valeur**
(injection pure) ; `ReadOnly = 0`, la convention des 5 181 liens de dimension existants.

## 2. Le contrôle qui manquait : **qui rend ces entités ?**

- **Un seul module de toute la base est lié à l'app 60** : `ModuleID 602`, onglet **152
  « Règles »**, visible. Requête `TabModules × ModuleSettings × TsDynDataApp` sur
  **tous les portails**, y compris onglets **invisibles et supprimés** — pas seulement la
  première page.
- Ce module rend `_RulesExplorer_RuleList.cshtml` / `_RulesExplorer_RuleDetail.cshtml`, qui
  lisent `Data` (les entités) et affichent `EntityTitle`, `MinNbPlayers`, `MaxNbPlayers`,
  `Summary`, `Material`, `Installation`, `Content`, `Variants`, `Memo` — la **forme d'un
  Game Rule**. Il affiche les **5 entités Game Rule**, jamais les 1 013 fallacies.
- La seule vue qui affiche des fallacies, `_FallacyExplorer_Root.cshtml`, **ne lit aucune
  entité** : elle lit `App.Query["FallaciesFromCSV"]` et les champs `text_<lang>`,
  `desc_<lang>`, `link_<lang>`, `path` — **le CSV**, avec sa **propre** cascade
  `lang → en → fr` (miroir de `Fallacy.LinkXxxFallback`). Un magasin d'entités n'y est
  jamais consulté.
- Cette vue n'est **liée à aucun module**.

⇒ Les 1 013 entités `Fallacy` de l'app 60 sont un **catalogue sans surface de rendu** sur
le site tel qu'il est déployé.

## 3. Contrôle d'instrument — le `0` est une absence, pas une panne

| Contrôle | Résultat |
| --- | --- |
| 4 noms distincts pris dans les défauts d'app 60, sondés sur les **22 onglets visibles** | **0** occurrence |
| **Contrôle positif, même page (152)** : « cole des menteurs » (contenu Game Rule) | **2** ⇒ l'app 60 s'affiche bien là, et l'instrument voit ce qui est là |
| Bascule de langue : `?language=en-US` change `<html lang>` (`fr-FR` → `en-US`) | ✅ l'anglais est **actif**, la sonde porte sur la bonne surface |
| Les 7 `fallacy/` de la page 170 | **URLs du SVG** (`onegoodmove.org/fallacy/…`), pas la route de l'app |

## 4. Ce que ça change : le vrai écart anglais est ailleurs — et (a) ne le ferme pas

La page Règles **servie en `en-US`** est mesurée **entièrement française** :
`joueurs` **16** · `players` **0**. Et elle **ne peut pas** être réparée par I3-(a) : le
content-type `Game Rule` **n'a aucune entité en zone 2** (CT `a9d3420a…`, app 29 = **0**),
donc **aucune graine anglaise n'existe** pour les 5 règles. Leur anglais ne peut venir que
des **cartes** — c'est exactement la branche (b).

| Surface | Type | Rendue ? | Graine EN en zone 2 | Effet de I3-(a) |
| --- | --- | --- | --- | --- |
| Fallacy × 1 013 | entités app 60 | **non** — aucun module lié | 1 455 valeurs (47 %) | **aucun effet visible** |
| Game Rule × 5 | entités app 60 | **oui** — onglet 152 | **0** | aucun (pas de graine) |
| app 33 `Content` / app 52 `News5` | entités app 33 / 52 | oui | — | aucun (hors périmètre) |

## 5. Décision : ne pas écrire

- Le fondement de (a) était « **855 cellules changeront visiblement de langue** ». C'est
  **mesuré faux** : le magasin n'est lu par **aucun** gabarit.
- Écrire serait une mutation d'un CMS vivant **sans effet observable**, pour un gain
  purement interne (rendre la dimension explicite).
- Le geste est **déterministe et scripté** : le refaire plus tard coûte un tick. L'écrire
  maintenant ne fait gagner que si un consommateur apparaît — et il n'y en a pas.
- Risque non nul et non nécessaire : une valeur dimensionnée peut changer la **résolution**
  de 2sxc (défaut ↔ dimension). On ne prend pas ce risque pour zéro bénéfice.

⇒ **I3-(a) est suspendu avant tout INSERT.** Le plan du § 1 reste valable et rejouable tel
quel si une surface de rendu est créée un jour — c'est précisément ce qui rend le report
gratuit.

## 6. Ce qu'il faut à la place

- **(b) recentré sur le visible** : l'anglais de la page Règles = **Game Rules × 5**, dont
  la source est le corpus des cartes (aucune graine en base) — l'arbitrage « les cartes
  font foi » et la mesure convergent ici.
- La carte des surfaces du § 4 est le préalable direct de **T2** (app 33, lane po-2024).
- Question **au registre owner**, pas tranchée ici : l'état de l'anglais **partant en
  production** (déjà listé « I3 Go-live » au status).

## Gates

❌ lecture seule (SELECT + GET) · ❌ **aucune écriture DB ni portal** · ❌ zéro donnée
personnelle · ❌ pas de verdict QA (ai-01) · ❌ aucune modification CSV/gabarit/CardPen.
