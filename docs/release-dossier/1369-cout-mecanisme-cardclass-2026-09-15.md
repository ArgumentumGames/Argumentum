# #1369 — Coût du choix de mécanisme : chiffrage des 3 options en fichiers touchés + régressions possibles

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `608a0fd9`
**Instrument** : lecture du code vivant — `Rule.cs` (`RuleClassMap`, mapping **par nom**),
`main.js:1387` (hook `cardClass`), `HarvestManager.cs` (injection Golden Master
`CardSetDocument.csv = dataSet.GetContent(...)`), des 3 tests consommant le CSV Rules
(`CardSetExpectedCardCountContractTests`, `PromptRulesGlossaryConsistencyTests`,
`GSheetSync/RulesPullTests`). **Sans écrire de code** — complément chiffré du dossier
#1390 (décisions §5) ; les indices/plages cités sont ceux de l'**instrument vivant**
(15 rangées, covers 1/7/9/11/13, plages 1-6 · 7-8 · 9-10 · 11-12 · 13-15).

---

## §0 La table de chiffrage (fichiers touchés × surface de régression)

| | **1a — colonne CSV + `cardClass`** | **1b — plage d'index** | **1c — champ template dédié** |
|---|---|---|---|
| **Fichiers touchés** | **2** : `Argumentum Rules - Cards.csv` (+1 colonne × 15 rangées, 5 valeurs non vides, insertion byte-exacte) + `Argumentum_Rules_fr.json` (`cardClass: "variant_class"` + ~5 blocs CSS `card.cover-<x>`) | **0 à 1** : les plages existent déjà dans le CSS (`[class~="N"]` 1-15) — le geste = les **documenter** (ce dossier + le dossier #1390) ; garde optionnelle = 1 fichier de test | **~4** : `CardSetInfo.cs` (+propriété), le chemin d'injection C# (wrapper/HarvestManager), le template JSON (+champ), le CSS |
| **C# touché** | **0** — `RuleClassMap` mappe **par nom** (`Rule.cs:32-41`) : une colonne non mappée est ignorée à la lecture ; l'injection CardPen passe le CSV **brut** (Golden Master inchangé) | 0 | **2 fichiers binaires** — et le chemin d'injection est **partagé par tous les CardSets** (zone Golden Master : « ne jamais modifier le CSV avant injection ») |
| **Tests impactés** | `Rules_Has_15_Faces_Per_Language` : épingle **15 rangées** — une colonne ne change pas le compte (reste vert) · `PromptRulesGlossaryConsistencyTests` : lit **uniquement** les colonnes `Text_<lang>` par nom (colonne nouvelle invisible) · ⚠️ `GSheetSync/RulesPullTests` : **reconstruit** le CSV depuis le GDrive (schéma attendu `pk,Text,Text_en,…`) — la colonne nouvelle doit être **ajoutée au GSheet ou exemptée** de la comparaison (test réseau, typiquement skippé en CI, mais le contrat de schéma est réel) | aucun (0 write) — c'est précisément le risque : **aucune garde n'existe** sur la correspondance index↔variante | tous les tests du chemin d'injection (Golden Master harvesting) à rejouer ; `CardSetExpectedCardCountContractTests` a un équivalent par CardSet |
| **Surface de régression** | **Rules seul** (1 CardSet, 8 langues via colonnes — la classe CSS est langue-neutre) | nulle introduite ; **risque résiduel fort** : toute édition de contenu du CSV décale les plages **silencieusement** (la pente qui a fait diverger l'embarqué 15→18 en est le précédent mesuré) | **tous les CardSets** (chemin commun) — la plus large |
| **Validation pré-merge** | rendu **Debug** obligatoire (template sur branche invisible en Release, #1225/#1228) ; jamais de write dans le `csv` **embarqué** du template (clé STALE, ignorée au runtime) | n/a | Debug + suite complète harvesting |
| **Extensibilité** | une nouvelle variante = 1 valeur de colonne + 1 bloc CSS | une nouvelle variante = **ré-équilibrer toutes les plages à la main** | une nouvelle variante = 1 entrée de config |

## §1 Lecture du chiffrage

- **1a est le moins cher ET le moins risqué** : 2 fichiers, 0 C#, la colonne traverse
  CsvHelper sans warning de champ manquant (mapping par nom), CardPen la lit via le hook
  `data.cardClass` existant (`main.js:1387`). Les deux seuls points d'attention sont **nommés** :
  le contrat GSheet (§0) et la validation Debug-only pré-merge.
- **1b coûte 0 mais n'achète rien** : le découpage existe déjà dans le CSS ; ce qui manque est
  une **garde** — sans elle, la prochaine édition de contenu du CSV décale les plages sans
  qu'aucun instrument ne rougisse. Si 1b était retenu, le chiffrage honnête = 1 fichier de test
  de contrat (« la rangée N porte le titre de la variante V », **dérivé du CSV vivant**, jamais
  de l'embarqué — la leçon du 15/09 s'applique mot pour mot).
- **1c est le plus cher et le plus large** : 2 fichiers C# dans le chemin d'injection partagé,
  un 2ᵉ mécanisme parallèle à `cardClass` à maintenir — à réserver si 1a est rejeté pour une
  raison de fond (aucune identifiée à ce jour).

## §2 Ce que ce chiffrage n'établit pas

⛔ Aucun code écrit, aucun CSV/template modifié (le gel tient ; la décision 1 reste à cocher sur
le dossier #1390 §5) · ⛔ le coût **rédactionnel** (rédiger les blocs CSS `card.cover-<x>`,
choisir les 5 valeurs de classe) est compté en « ~5 blocs » mais pas estimé en temps · ⛔ le
coût GSheet de 1a (ajouter la colonne au sheet source ou exempter le pull) est **nommé**, pas
chiffré — le sheet vit hors dépôt · ⛔ aucune recommandation visuelle (le rendu final reste
verdict ai-01) · ⛔ les régressions « possibles » de 1a/1c sont dérivées de la lecture du code,
pas d'une exécution (aucune mutation jouée — sans code, pas de mutation).

---
*po-2024 — pool #458 (c.5666260217), grain ⑩. Chiffrage en fichiers + régressions des 3 options
de la décision 1 du dossier #1390 ; les 4 coches restent sur la feuille §5 de ce dossier.*
