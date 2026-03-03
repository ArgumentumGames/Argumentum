# Plan de Comparaison: Référence 2023 vs Génération Actuelle

## Objectif
Comparer les PDFs générés avec la référence 2023 (Argumentum_ru.zip) pour valider la qualité de la génération.

## Étape 1: Génération Russe (Clean Comparison)

### Pourquoi le Russe ?
- La référence 2023 est en russe
- Permet une comparaison directe sans barrière linguistique
- Les dimensions et layouts doivent être identiques

### Commandes
```bash
# Activer la localisation
# LocalizationConfig.Enabled = true (déjà fait)

# Nettoyer les outputs russes existants
rm -rf Target/ru/*

# Régénérer
dotnet run --project Generation/Converters/Argumentum.AssetConverter/Argumentum.AssetConverter.csproj
```

## Étape 2: Extraction des Pages pour Comparaison

### Fichiers de Référence (2023)
- `Argumentum_TarotCards_ru-1.pdf` → `tarot_ru-1_back.png` (page 1)
- `Argumentum_TarotCards_ru-1.pdf` → `tarot_ru-1_faces.png` (page 2)
- `Argumentum_PokerCards_ru-1.pdf` → `poker_ru-1_back.png` (page 1)
- `Argumentum_PokerCards_ru-1.pdf` → `poker_ru-1_faces.png` (page 2)
- `Argumentum_TarotCards_Print&Play_A4_ru.pdf` → `tarot_pp_ru.png` (page 1)
- `Argumentum_PokerCards_Print&Play_A4_ru.pdf` → `poker_pp_ru.png` (page 1)

### Fichiers Actuels (Générés)
- `Argumentum_TarotCards_ru-1.pdf` → `tarot_curr_back.png`, `tarot_curr_faces.png`
- `Argumentum_PokerCards_ru-1.pdf` → `poker_curr_back.png`, `poker_curr_faces.png`
- `Argumentum_TarotCards_Print&Play_A4_ru.pdf` → `tarot_pp_curr.png`
- `Argumentum_PokerCards_Print&Play_A4_ru.pdf` → `poker_pp_curr.png`

## Étape 3: Comparaison avec Agents MCP Parallélisés

### Métriques à Vérifier
1. **Dimensions physiques** (mm) - doivent correspondre exactement
2. **Layout** - nombre de cartes par page, grille
3. **Qualité visuelle** - netteté, couleurs
4. **Contenu** - texte lisible, pas de troncature
5. **Structure PDF** - nombre de pages, ordre recto/verso

### Script de Comparaison (Python)
```python
# comparison.py
import asyncio
from concurrent.futures import ThreadPoolExecutor

async def compare_with_agent(ref_image, curr_image, category):
    """Utilise sk-agent pour comparer deux images"""
    prompt = f"""
    Compare these two card game PDF pages:

    REFERENCE: {ref_image}
    CURRENT: {curr_image}

    Category: {category}

    Analyze:
    1. Card dimensions in mm (width x height)
    2. Number of cards per page
    3. Grid layout (rows x columns)
    4. Visual quality (sharpness, colors)
    5. Any obvious differences or issues

    Return JSON format:
    {{
        "match": true/false,
        "dimensions_ref": "WxH mm",
        "dimensions_curr": "WxH mm",
        "cards_per_page_ref": N,
        "cards_per_page_curr": N,
        "issues": ["list of problems"]
    }}
    """
    # Appel à sk-agent MCP
    result = await mcp_sk_agent_call(prompt, curr_image)
    return result

# Comparaisons parallélisées
comparisons = [
    ("Tarot Back", tarot_ref_back, tarot_curr_back),
    ("Tarot Faces", tarot_ref_faces, tarot_curr_faces),
    ("Poker Back", poker_ref_back, poker_curr_back),
    ("Poker Faces", poker_ref_faces, poker_curr_faces),
    ("Tarot P&P", tarot_pp_ref, tarot_pp_curr),
    ("Poker P&P", poker_pp_ref, poker_pp_curr),
]

with ThreadPoolExecutor(max_workers=4) as executor:
    results = executor.map(lambda x: compare_with_agent(*x), comparisons)
```

## Étape 4: Rapport de Validation

### Format de Rapport
```markdown
# Rapport de Comparaison Argumentum RU

## Résumé
- ✅ Match Dimensions: X/Y
- ❌ Issues Found: Z

## Détails par Document

### TarotCards ru-1
| Métrique | Référence | Actuel | Status |
|----------|-----------|--------|--------|
| Pages | 2 | 2 | ✅ |
| Carte dimensions | 60x113mm | ? | ⚠️ |
| ...
```

## Étape 5: Correction des Problèmes Identifiés

### Problèmes Courants
1. **Dimensions incorrectes** → Ajuster DPI ou configuration
2. **Pagination** → Corriger `PrintAndPlayDocument.cs`
3. **Couleurs** → Vérifier profils CMYK
4. **Contenu tronqué** → Ajuster marges/layout

## Étape 6: Itération jusqu'à Validation Complète

Répéter les étapes 1-5 jusqu'à ce que toutes les métriques correspondent à la référence.
