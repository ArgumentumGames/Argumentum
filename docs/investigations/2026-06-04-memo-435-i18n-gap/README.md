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

## ✅ Confirmation visuelle — régén du 2026-06-04 (lane ai-01)

Régén `dotnet run -c Debug` sur une branche combinant les deux correctifs (#438 Rules +
#439 Mémo), harvest Mémo régénéré pour les 4 langues. Rendus extraits du harvest frais
(`Memo_harvest_{lang}.json` → dataURL base64 → PNG), source de vérité du rendu.

| Langue | AVANT | APRÈS | Taille PNG (avant → après) | Capture APRÈS |
|--------|-------|-------|----------------------------|---------------|
| FR | 7 / 7 | **7 / 7** | 165 KB → 137 KB | `memo_face_fr_AFTER.png` |
| EN | 2 / 7 | **7 / 7** ✅ | 63 KB → **130 KB** | `memo_face_en_AFTER.png` |
| PT | 0 / 7 | **7 / 7** ✅ | 16,5 KB → **145 KB** | `memo_face_pt_AFTER.png` |
| RU | 0 / 7 | **7 / 7** ✅ | 16,5 KB → **146 KB** | `memo_face_ru_AFTER.png` |

Les 4 langues rendent désormais **les 7 familles**, chacune avec son **texte localisé** (et
non un clone du FR — les tailles PNG diffèrent toutes, signature d'un contenu propre à
chaque langue). Le bug est **résolu**.

### Guide de vérification (langues non lues) — noms de famille par langue

Pour contrôler RU/PT sans les lire : les 7 pastilles couleur apparaissent dans le **même
ordre** et la **même couleur** dans les 4 langues. Table de correspondance des en-têtes :

| # | Couleur | FR | EN | PT | RU |
|---|---------|----|----|----|----|
| 1 | violet | Insuffisance | Insufficiency | Insuficiência | Недостаток |
| 2 | rose | Influence | Influence | Influência | Влияние |
| 3 | turquoise | Erreur mathématique | Mathematical Error | Erro Matemático | Математическая ошибка |
| 4 | vert | Erreur de raisonnement | Faulty Logics | Lógicas Defeituosas | Ошибка рассуждения |
| 5 | bleu | Abus de langage | Misleading Language | Abuso da Linguagem | Злоупотребление языком |
| 6 | jaune | Tricherie | Cheating | Trapaça | Лукавство |
| 7 | rouge | Obstruction | Obstruction | Obstrução | Обструкция |

### Réserve mineure — RU, 7ᵉ famille (Обструкция) en bas de carte

En RU, la 7ᵉ famille (Обструкция, rouge) tombe en pied de carte (verbosité cyrillique) :
l'en-tête et le début de description rendent, mais la carte est plus dense qu'en FR/EN/PT.
Non bloquant pour le correctif i18n (les 7 familles rendent) ; à surveiller si débordement
au format imprimé.

### Carte Mémo **Back** — observation séparée (pré-existante, hors #435)

Le correctif des 3 sélecteurs du Back s'applique : le Back **rend** désormais la taxonomie
complète dans les 4 langues (au lieu d'un cadre vide en EN/PT/RU). MAIS le Back **affiche
les clés de taxonomie** (`{{Famille}}` ×14, `{{Sous-Famille}}` ×3, `{{Soussousfamille}}` ×3
— aucun champ `desc_*` ni variante `_en/_ru/_pt`), donc il s'affiche **en français dans les
4 langues** (rendus byte-identiques, 327 KB ; `memo_back_AFTER.png`). Le correctif (espace
sur le sélecteur) **ne touche aucun champ d'affichage** → ce comportement FR du Back est
**pré-existant**, pas une régression. **Question éditoriale ouverte pour jsboige** : le Back
dense (« ARGUMENTUM — L'art de jamais avoir tort ») doit-il rester une référence FR, ou ses
clés de taxonomie doivent-elles être localisées (tâche contenu/config distincte du bug
sélecteur) ?

🔒 Pas de merge avant le sign-off visuel #140 (couplé go-live site DNN).

Refs #435.
