# Card sets and templates — lessons learned

> **Read on demand — not loaded by default.** Read this before adding or changing a card set, a dataset declaration or a card template.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Classes CSS Familles (Virtues/Fallacies)* names a heading that exists here.

## Leçons Apprises (Pipeline Recovery 2025-2026)

### CardPen Template Configuration

| Parameter | Description | Example |
|-----------|-------------|---------|
| `rscount` | Nombre de lignes CSV groupées par carte | Memo=200 (1 carte avec toutes les données) |
| `rsstyle` | Mode de groupement | "bunch", "cycle", "random" |
| `{{cardIndex}}` | Variable auto-injectée (1-based) | Utilisable pour numérotation de page |

**Règle critique**: Ne JAMAIS forcer `rscount=0` dans le code C# - préserver la valeur du template JSON.

### Calcul du nombre d'images attendues

Avec `rsstyle="bunch"` et `rscount >= N`:
```
expectedImageCount = ceil(cardIds.Count / rscount)
```

Exemple: Memo avec 200 lignes CSV et `rscount=200` → génère 1 seule image.

### CSS Card Templates

```css
/* TOUJOURS inclure pour éviter problèmes d'affichage des caractères */
-webkit-font-variant-ligatures: no-common-ligatures;
font-variant-ligatures: no-common-ligatures;
```

**À éviter**: `flex-flow: column wrap` - cause des débordements de texte. Préférer `column nowrap` avec gestion overflow.

### domtoimage (frame.js)

Configuration Golden Master restaurée:
```javascript
const options = {
    height: height,
    width: width,
    scale: dpi / 96,      // Important pour résolution correcte
    cachedFonts: true,    // Utilise polices préchargées
    imagePlaceholder: "..." // Fallback pour images manquantes
};

// Appeler AVANT génération:
domtoimage.getFontsBefore();
```

### Chemins d'assets relatifs

Les chemins `../../Cards/...` ne fonctionnent pas avec CardPen local (IIS).

**Solutions**:
1. Réécrire en URLs absolues GitHub: `https://raw.githubusercontent.com/ArgumentumGames/Argumentum/master/Cards/...`
2. Ou configurer IIS pour servir `/Cards/` depuis le répertoire local

### Debugging Tips

| Symptôme | Cause probable | Solution |
|----------|---------------|----------|
| Harvest vide (`Images: {}`) | Erreur JS dans CardPen | Vérifier console Playwright |
| Harvest vide + Dpi=0 | CsvType manquant dans DataSet | Ajouter `CsvType = typeof(Entity)` |
| Images blanches/vides | Chemins assets relatifs | Réécrire en URLs absolues |
| `data:,` (empty dataUrl) | Timeout ou erreur fonts | Augmenter timeout à 120s |
| Mismatch image count | rscount mal calculé | Vérifier formule expectedImageCount |

### CsvType et DataSets

**CRITIQUE**: Chaque DataSet doit avoir un `CsvType` défini dans `AssetConverterConfig.cs` pour que le harvesting fonctionne.

```csharp
// ✅ CORRECT - CsvType défini
new DataSetInfo() {
    Name = KnownDataSets.Scenarii,
    CsvType = typeof(Scenario),  // ← OBLIGATOIRE
    DebugFilePath = @"..\..\Cards\Scenarii\..."
}

// ❌ INCORRECT - génère harvest vide
new DataSetInfo() {
    Name = KnownDataSets.Scenarii,
    // CsvType manquant → early return dans HarvestManager ligne ~494
    DebugFilePath = @"..\..\Cards\Scenarii\..."
}
```

### Dimensions FallaciesWeb A0

Pour tenir sur 1 page A0 (841×1189mm):
- HeightMM = 69mm (pas 72mm)
- WidthMM = 69mm
- NbColumns = 12
- Padding = 2mm

### RowsetNb vs rscount (CRITIQUE)

**Règle**: Si le template JSON attend 1 ligne CSV par carte (variables simples comme `{{titre}}`), ne PAS définir `RowsetNb` dans la config C#.

```csharp
// ✅ CORRECT - template avec rscount=1 utilise 1 ligne par carte
FaceCardSetInfo = new CardSetInfo()
{
    DataSet = KnownDataSets.Scenarii,
    JsonFilePathDebug = @"...\Argumentum_Scenarii_Face_fr.json",
    // RowsetNb non défini → utilise rscount du template JSON
}

// ❌ INCORRECT - force 14 lignes par carte mais template attend 1 ligne
FaceCardSetInfo = new CardSetInfo()
{
    ...
    RowsetNb = 14  // Casse le template !
}
```

**Symptôme si RowsetNb incorrect**: Cartes générées avec contenu vide (seuls éléments statiques visibles).

### CSV Injection dans CardPen (CRITIQUE)

**Règle absolue**: NE JAMAIS modifier le contenu CSV avant injection dans CardPen.

```csharp
// ✅ CORRECT - Golden Master (avril 2024)
cardSetDocumentWrapper.CardSetDocument.csv = csvContent;

// ❌ INCORRECT - Casse le parsing PapaParse
cardSetDocumentWrapper.CardSetDocument.csv = csvContent.Replace("\n", "\\n");
```

**Pourquoi**: PapaParse gère correctement les newlines dans les cellules CSV entre guillemets.
L'échappement transforme les vrais newlines en chaînes littérales "\\n", cassant le parsing.

**Symptôme**: Cartes générées avec contenu vide (seules les icônes statiques visibles).

### Classes CSS Familles (Virtues/Fallacies)

La classe racine de la carte est **la valeur de la colonne CSV** désignée par `cardClass` dans le template — `family_fr_camelcase` pour Virtues, `Famille_camelCase` pour Fallacies. Chaque valeur doit avoir sa règle `card.<valeur>` dans la clé `css` du template JSON (⚠️ pas dans `mustache`, qui ne contient que le HTML).

Liste complète pour Virtues, **vérifiée contre `Argumentum Virtues - Taxonomy.csv` + la clé `css` du template** (master `c99fc4b3`, 2026-08-17) — 8 familles, 223 nœuds :

| Classe CSS (= `family_fr_camelcase`) | Famille (`family_fr`) | Nœuds | Couleur | Alias hérités encore déclarés |
|------------|---------|---:|---------|---------|
| `argumentValable` | Argument valable (racine) | 1 | Gris #555555 | `argumentsVertueux` |
| `argumentPertinent` | Argument pertinent | 33 | Violet #811da3 | — |
| `présentationIntègre` | Présentation intègre | 25 | Rose #ff66eb | — |
| `sensQuantitatif` | Sens quantitatif | 20 | Turquoise #08af93 | `rigueurMathématique`, `exactitudeMathématique` |
| `inférenceMaîtrisée` | Inférence maîtrisée | 55 | Vert #8dc801 | `raisonnementValide` |
| `justesseLexicale` | Justesse lexicale | 18 | Bleu #0054a4 | `langageExact`, `langageRigoureux` |
| `honnêtetéIntellectuelle` | Honnêteté intellectuelle | 27 | Jaune #ffc307ff | — |
| `échangeEnrichissant` | Échange enrichissant | 44 | Rouge #dc0f0a | `débatRespectueux` |

> ⚠️ **Trois familles ont été renommées les 6-7 août 2026** (#981/#982a → `6d22f79a`, #998 → `5631bf3c`, #1002 → `19b9c9d1`) : `Raisonnement valide` → **`Inférence maîtrisée`**, `Rigueur mathématique` → **`Sens quantitatif`**, `Langage exact` → **`Justesse lexicale`**. Les anciens noms **survivent partout dans les artefacts figés** — bodies d'issues, rapports, commentaires. Une recherche sur un ancien nom rend **0 résultat** dans le CSV, ce qui se lit à tort comme « rien à faire » : c'est une panne d'instrument, pas une absence. **Toujours re-mesurer le libellé courant contre le CSV avant d'agir sur une famille.** (Le dossier #985 a ainsi coûté un cycle : son tableau porte encore l'ancien nom *et* des comptes de cartes périmés.)

**Renommage additif** — les renommages ci-dessus ont été faits selon ce motif, et le template le conserve : tous les noms successifs cohabitent **dans le même bloc**, séparés par des virgules (`card.exactitudeMathématique, card.rigueurMathématique, card.sensQuantitatif { … }` — trois générations). Pour renommer une famille : ajouter le nouveau nom au bloc existant **d'abord**, renommer le CSV **ensuite**. L'ordre inverse produit des cartes sans couleur de famille entre les deux merges.

**Second référentiel à synchroniser** — les glossaires en dur des prompts `DatasetUpdater/Resources/` portent aussi les noms de familles : un renommage CSV non répercuté y est **ré-injecté à la passe de traduction suivante**. Les 8 `PromptVirtues*User.txt` sont additifs (ancien + nouveau) et donc corrects ; vérifier ce point à chaque renommage.

**Symptôme si classe manquante**: Carte avec fond blanc au lieu de la couleur de famille.
