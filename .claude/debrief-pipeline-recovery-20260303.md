# Debrief: Pipeline Recovery - 3 Mars 2026

## Objectif de la Session

L'utilisateur souhaite générer des PDFs conformes au jeu de référence russe de 2024, avec le format suivant:
- **1 Dos par PDF**: Première page = dos, pages suivantes = faces associées
- **7 PDFs poker**: Un par famille de scénarios
- **PDFs règles recto-verso**: Plusieurs PDFs
- **1 gros PDF fallacies**: Toutes les fallacies

## Références Clés

### 1. Série Russe 2024 (GOLDEN REFERENCE)
**Localisation**: `d:\Dev\Argumentum\` (fichiers de référence fournis par l'utilisateur)
- `reference-ru-2-rules.png` - Règles russes de référence
- `rules_back_default.png` - Dos des règles
- `rules_face_01.png` - Face des règles
- `current-ru-1-page1.png` - Page 1 du RU-1 actuel
- `current-ru-2-page1.png` - Page 1 du RU-2 actuel

**Format attendu** (d'après description utilisateur):
```
Format PDF "1 Dos + N Faces":
- Page 1: Dos (commun à la famille)
- Pages 2-N: Faces des cartes de cette famille

Structure des outputs:
├── PokerCards/
│   ├── famille-1.pdf (Dos + 14 faces)
│   ├── famille-2.pdf (Dos + 14 faces)
│   └── ... (7 PDFs total pour 7 familles)
├── Rules/
│   ├── rules-set-1.pdf (Recto-verso)
│   └── rules-set-2.pdf (Recto-verso)
└── Fallacies/
    └── fallacies-all.pdf (Toutes les fallacies)
```

### 2. État Git Début 2025 (Workflows)
**Branche actuelle**: `fix/recovery-october-2025`

**Commits clés à investiguer**:
- Chercher les workflows de génération PDF dans l'historique git
- Le Golden Master d'avril 2024 (`0087f0ec`) avait le bon format
- L'état début 2025 contenait les workflows corrects

## État Actuel du Pipeline

### PDFs Générés (RU - Current)
**Localisation**: `Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/ru/Documents/density-0/`

| PDF | Pages | Format Actuel | Problème |
|-----|-------|---------------|----------|
| Argumentum_TarotCards_ru-1.pdf | 2 | Cover + Rules | ? |
| Argumentum_PokerCards_ru-1.pdf | 15 | 97 cartes × 2 | Ordre Face/Dos incorrect |

### Problèmes Identifiés (Phase 0 du plan)

1. **Ordre Recto-Verso Inversé** (Issue #1)
   - Fichier: `PdfManager.cs` lignes 56-57
   - Actuel: Face, Dos, Face, Dos...
   - Attendu: Dos, Face, Dos, Face...

2. **Dimensions Dos Scenarii** (Issue #2)
   - Fichier: `Argumentum_Scenarii_Back_fr.json` lignes 18-19
   - Actuel: `blsize: 0` (pas de bleed)
   - Attendu: `blsize: 5, blunit: "mm"`

3. **Dos Identiques** (Issue #3)
   - Fichier: `WebBasedGeneratorConfig.cs` lignes 134-140
   - Manque: `RowsetNb = 14`
   - Résultat: Tous les dos montrent "POP CULTURE"

## Travaux Effectués Cette Session

### Créé: compare_pdfs.py
**Localisation**: `d:\Dev\Argumentum\compare_pdfs.py`

Script Python pour extraire les pages PDF en PNG:
```python
#!/usr/bin/env python3
import fitz  # PyMuPDF
from pathlib import Path

def extract_page_as_image(pdf_path, page_num, output_dir):
    doc = fitz.open(pdf_path)
    page = doc[page_num]
    pix = page.get_pixmap()  # Default resolution
    output_path = Path(output_dir) / f"{Path(pdf_path).stem}_page{page_num}.png"
    pix.save(output_path)
    return output_path
```

### Fichiers Extraits
**Localisation**: `d:\Dev\Argumentum\comparison_output\`
- Argumentum_TarotCards_ru-1_page0.png
- Argumentum_TarotCards_ru-1_page1.png
- Argumentum_PokerCards_ru-1_page0.png
- Argumentum_PokerCards_ru-1_page1.png
- Argumentum_PokerCards_ru-1_page2.png

## Prochaines Étapes (Pipeline Recovery)

### Phase 1: Investigation Workflows 2024-2025
1. **Explorer l'historique git** pour trouver les workflows de génération PDF
   ```bash
   git log --oneline --all --grep="pdf" --grep="workflow" --since="2024-01-01" --until="2025-03-01"
   ```

2. **Comparer avec la série russe 2024**
   - Analyser les images de référence avec vision AI
   - Identifier le format exact (1 Dos + N Faces)

3. **Localiser les configurations CardSetDocuments**
   - Fichier: `WebBasedGeneratorConfig.cs` ou `AssetConverterConfig.cs`
   - Chercher la structure qui génère 7 PDFs poker + règles + fallacies

### Phase 2: Corrections Structurelles
1. **Corriger l'ordre recto-verso** (PdfManager.cs)
2. **Corriger les dimensions dos** (Scenarii_Back_fr.json)
3. **Restaurer RowsetNb=14** (WebBasedGeneratorConfig.cs)

### Phase 3: Régénération et Validation
1. `dotnet build` puis `dotnet run`
2. Utiliser `compare_pdfs.py` pour extraire les pages
3. Comparer visuellement avec la série russe 2024
4. Valider le format "1 Dos + N Faces"

## Points d'Attention

### Règles Absolues (CLAUDE.md)
1. **INSPECTION VISUELLE OBLIGATOIRE** avant de présenter les résultats
2. Ne JAMAIS démarrer de serveur HTTP sans permission explicite
3. Utiliser `pdf_preview.py` ou `compare_pdfs.py` pour extraction
4. Le Golden Master est `0087f0ec` (avril 2024)

### Outils de Validation
- `compare_pdfs.py` - Extraction pages PDF → PNG
- `pdf_preview.py` - Autre script d'extraction
- MCP `analyze_image` - Comparaison vision AI (peut avoir des erreurs de format)
- Read tool natif - Lecture directe d'images PNG

## Commandes Utiles

```bash
# Build et run pipeline
cd "Generation/Converters/Argumentum.AssetConverter"
dotnet build
dotnet run

# Extraire pages PDF
python "d:/Dev/Argumentum/compare_pdfs.py"

# Vérifier outputs
ls -la "bin/Debug/net9.0/Target/ru/Documents/density-0/"

# Git archaeologie
git log --oneline --since="2024-01-01" --until="2025-03-01" -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/*.cs"
```

## Fichiers Critiques à Modifier

| Fichier | Changement | Issue |
|---------|------------|-------|
| `PdfManager.cs` (56-57) | Inverser ordre Front/Back | #1 |
| `Argumentum_Scenarii_Back_fr.json` (18-19) | Ajouter bleed 5mm | #2 |
| `WebBasedGeneratorConfig.cs` (134-140) | Ajouter RowsetNb=14 | #3 |

## Message Final pour Pipeline-Recovery

Le skill pipeline-recovery doit:

1. **D'ABORD**: Investiguer l'historique git 2024-2025 pour trouver les workflows qui généraient le format "1 Dos + N Faces" par famille
2. **ENSUITE**: Comparer les outputs actuels avec la série russe 2024 (fichiers de référence dans le root du repo)
3. **CORRIGER**: Les 3 issues documentées (ordre, dimensions, variation dos)
4. **VALIDER**: Inspection visuelle OBLIGATOIRE avant de présenter les résultats

**Point de départ recommandé**:
```bash
git log --oneline --since="2024-04-01" --until="2025-02-01" -- "Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/"
```

---
*Debrief créé le 3 mars 2026 - Session context saturé*
