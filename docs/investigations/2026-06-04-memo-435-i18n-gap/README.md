# Mémo Tarot — régression i18n (#435) : diagnostic + correctif — 2026-06-04

Carte **Mémo Face** (le glossaire des 7 familles de sophismes, une grande carte de
référence). La carte est correcte en FR mais **vide ou partielle dans les autres langues**
depuis l'introduction de la localisation runtime. C'est le problème Mémo signalé par
jsboige (et resté non précisé sur #435 jusqu'ici).

## Symptôme — preuve par rendu (build Release du 2026-06-02)

| Langue | Familles affichées | Taille PNG | Capture |
|--------|--------------------|-----------|---------|
| FR | **7 / 7** (toutes, avec pastilles couleur + descriptions) | 165 KB | `memo_face_fr_BEFORE.png` |
| EN | **2 / 7** — seulement **INFLUENCE** + **OBSTRUCTION** | 63 KB | `memo_face_en_BEFORE.png` |
| PT | **0 / 7** — carte vide, seul le titre « MEMO » | 16,5 KB | `memo_face_pt_BEFORE.png` |
| RU | **0 / 7** — carte vide, seul le titre | 16,5 KB | `memo_face_ru_BEFORE.png` |

Le **gradient de taille** corrobore : FR 165 KB ≫ EN 63 KB ≫ PT 16,5 KB **=** RU 16,5 KB
(PT et RU sont byte-identiques : deux cartes « titre seul »).

Les 2 familles qui survivent en EN — **Influence** et **Obstruction** — sont précisément
les **mots identiques FR/EN** (cognats). C'est la signature du bug.

## Cause racine — le sélecteur de famille n'est pas localisé symétriquement

Le template `Cards/Memo/Argumentum_Memo_Face_fr.json` détecte les lignes « en-tête de
famille » avec :

```handlebars
{{#each rowset}}
  {{#ifCond Famille "==" text_fr}}        <!-- ← le sélecteur -->
    <div class="familyContainer {{Famille_camelCase}}">
      ...
      <div class="familyName">{{Famille}}</div>      <!-- affichage -->
      <div class="familyDesc">{{desc_fr}}</div>      <!-- affichage -->
  {{/ifCond}}
{{/each}}
```

La localisation runtime
([`CardSetLocalization.cs`](../../../Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/Localization/CardSetLocalization.cs)) :

```csharp
public string FormatField(string fieldName) => $"{fieldName}}}";   // → "champ}" (UNE accolade)
...
template = template.Replace(sourceFieldPattern, destFieldPattern); // remplacement de sous-chaîne brut
```

Le motif de recherche est `champ}` (le nom + **une** accolade fermante, **sans** `{{`
ouvrant). Donc un opérande n'est localisé **que s'il est immédiatement suivi de `}}`** :

- Dans `{{#ifCond Famille "==" text_fr}}`, **`text_fr`** est suivi de `}}` → motif `text_fr}`
  trouvé → **localisé** en `text_en` / `text_pt` / `text_ru`.
- **`Famille`** est suivi d'une **espace** (`Famille "=="`) → motif `Famille}` **absent** →
  **PAS localisé**, reste la valeur FR.

Le sélecteur devient donc, en EN, `Famille(FR) == text_en` : il compare le **nom de
famille FR** au **texte EN** → ne matche que les **cognats**.

- EN : `Insuffisance==Insufficiency` ✗, `Influence==Influence` ✓, `Obstruction==Obstruction` ✓
  → **2/7** (exactement les renders observés).
- PT/RU : aucun cognat (`Insuffisance==Insuficiência` ✗, cyrillique ✗) → **0/7**.

### Ce n'est PAS un trou de traduction

Les traductions **existent** dans la taxonomie. Vérifié cellule par cellule sur
`Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` : les lignes-racines de famille
(pk=1 Insuffisance, pk=175 Influence, …) ont `Family` / `desc_en` / `desc_ru` / `desc_pt`
toutes peuplées. C'est un bug de **sélecteur / configuration**, pas de contenu — donc
**pas de campagne de traduction**, juste un correctif template + régén.

## Correctif — ancrer `text_fr` sur une espace (`text_fr }}`)

Une seule espace avant `}}` suffit : `text_fr` devient suivi d'une espace, donc le motif
`text_fr}` du localiseur ne le trouve plus → **aucun** des deux opérandes n'est localisé →
le sélecteur compare **FR contre FR dans toutes les langues** = sélection de lignes
**byte-identique au comportement FR actuel** (zéro changement FR), désormais
**indépendante de la langue**. Les placeholders d'**affichage** (`{{Famille}}`,
`{{desc_fr}}`, suivis de `}}` sans espace) **continuent** de se localiser correctement.
`text_fr` n'est **jamais affiché** dans les templates Mémo (sélecteur uniquement : Face 1×,
Back 3×), donc le garder en FR n'a **aucun** effet visible.

```diff
- {{#ifCond Famille "==" text_fr}}
+ {{#ifCond Famille "==" text_fr }}
```

Appliqué aux **4 sélecteurs** : Face (1, niveau famille) + Back (3, niveaux
famille / sous-famille / sous-sous-famille — même classe de bug). `git diff --stat` :
2 fichiers, 2 lignes (la seule valeur `mustache` de chaque template).

### Preuve d'équivalence (les bonnes lignes restent sélectionnées)

Sur la taxonomie complète, le sélecteur `Famille == text_fr` sélectionne **exactement**
les 8 mêmes lignes que le test structurel `Sous-Famille == ""` (super-racine « Argument
fallacieux » + 7 familles). Le correctif ne **change pas** cette sélection — il la rend
seulement insensible à la langue.

## Validation

1. **Sélection** (prouvée par les données, ce jour) : le sélecteur corrigé sélectionne les
   8 lignes-racines en EN/PT/RU comme en FR.
2. **Affichage** (prouvé par le build) : les 2 cartes EN qui se rendent **déjà**
   (Influence, Obstruction) prouvent que `{{Famille}}`→`Family` et `{{desc_fr}}`→`desc_en`
   fonctionnent en EN ; seule la **sélection** était cassée.

∴ Par composition, le correctif rend les 7 familles dans toutes les langues.

⏳ **Confirmation visuelle autoritaire** = régén Mémo (Face + Back) sur les 8 langues, puis
inspection des `memo_face_face.png` / `memo_back.png` par langue (lane ai-01). Tenu jusqu'à
la régén (comme PR #438). 🔒 Pas de merge avant le sign-off visuel #140.

Refs #435.
