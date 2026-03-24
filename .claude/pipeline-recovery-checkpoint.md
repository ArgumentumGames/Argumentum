# Pipeline Recovery Checkpoint

**Dernière mise à jour**: 2026-03-03 (Session 10 - Opus 4.6 sur myia-ai-01)
**Branche**: fix/recovery-october-2025
**Objectif**: PDFs format "1 Dos + N Faces" conformes à la référence RU 2024
**Référence 2024**: `G:\Mon Drive\MyIA\Argumentum\Fallacies\Matériel\Argumentum_ru`

## État Actuel - PIPELINE FR VALIDÉ

### Corrections Appliquées (Commits récents)

| Commit | Description | Status |
|--------|-------------|--------|
| `be1a529e` | **Fix Print&Play recto-verso + A0 12 colonnes** | Fait |
| `37600e4a` | CSV injection sans modification (Golden Master) | Fait |
| `f0b1cd35` | CSS argumentsVertueux pour Virtues racine | Fait |
| `09b427ef` | Chemins assets Scenarii en URLs GitHub | Fait |
| `30483257` | CSS Virtues et nommage Rules | Fait |
| `9b19d5e8` | Fix RowsetNb=14 pour Scenarii | Fait |

### CardSets - État de Génération (FR)

| CardSet | Images | PDFs | Notes |
|---------|--------|------|-------|
| Fallacies Tarot FR | 177 | Fait | TarotCards_fr-1.pdf, TarotCards_fr-2.pdf |
| Fallacies-Web FR | 176 | Fait | A0 (99MB), A4 (98MB), Thumbnails |
| Virtues Tarot FR | 113 | Fait | TarotCards_Virtues_fr-FacesOnly.pdf (37MB) |
| **Scenarii Poker FR** | **97** | **Fait** | PokerCards_fr-1.pdf (12MB) |
| Rules Tarot FR | 24 | Fait | Dans TarotCards |
| Memo Tarot FR | 1 | Fait | Dans TarotCards |
| Print&Play A4 | 27+7 | Fait | Poker + Tarot |

### PDFs Générés (2026-02-01)

```
Target/fr/Documents/density-0/
├── Argumentum_Fallacies_Web_A0_fr.pdf (99 MB)
├── Argumentum_Fallacies_Web_A4_fr.pdf (98 MB)
├── Argumentum_Fallacies_Web_Thumbnails_A4_fr.pdf (8 MB)
├── Argumentum_PokerCards_fr-1.pdf (12 MB) ← SCENARII!
├── Argumentum_PokerCards_Print&Play_A4_fr.pdf (3.6 MB)
├── Argumentum_TarotCards_fr-1.pdf (685 KB)
├── Argumentum_TarotCards_fr-2.pdf (70 MB)
├── Argumentum_TarotCards_fr-FacesOnly.pdf (3.3 MB)
├── Argumentum_TarotCards_Print&Play_A4_fr.pdf (1.7 MB)
└── Argumentum_TarotCards_Virtues_fr-FacesOnly.pdf (37 MB)
```

### Langues

| Langue | Code | État |
|--------|------|------|
| Français | fr | COMPLET |
| Anglais | en | À faire |
| Russe | ru | À faire |
| Portugais | pt | À faire |

## Prochaines Étapes (Phase 3)

**Priorité CRITIQUE**: Corriger l'assemblage PDF avant multilingue.

### 1. Investigation Git - Système Dos/Faces

Le système recto-verso est cassé. Formats attendus:
- **Format A**: Alternance dos/face (1 PDF, recto-verso)
- **Format B**: 1 dos + N faces par famille (multiple PDFs)

### 2. Problèmes PDF identifiés (2026-02-02)

| PDF | Problème |
|-----|----------|
| `Fallacies_Web_A0` | 2 pages au lieu de 1 → passer à 12 colonnes |
| `PokerCards_fr-1` | Dimensions dos≠faces, format dos/faces incorrect |
| `PokerCards_Print&Play` | Pas de dos en alternance |
| `TarotCards_fr-2` | Dimensions dos≠faces |
| `TarotCards_FacesOnly` | Rules sans styles (fond, police, couleurs) |
| `TarotCards_Print&Play` | Rules sans styles, recto-verso cassé, Memo incomplet |
| `TarotCards_Virtues` | Couleurs manquantes, texte trop petit |

### 3. Régénérer Scenarii

CSV mis à jour avec nouvelles cartes → régénérer images et PDF.

### 4. Multilingue (après corrections PDF)

Activer génération EN/RU/PT

## Problèmes Connus Non Résolus

| Problème | Fichier | Impact | Priorité |
|----------|---------|--------|----------|
| DPI FallaciesWeb | WebBasedGeneratorConfig.cs | Résolution web | Moyenne |
| Tests SVG crashent | SvgConversionTests | Tests unitaires | Basse |
| MmGenerator deadlock | MmGeneratorTests | Mind maps | Basse |
| Backs manquants | CardSetConfig | Warning "Default back not found" | À évaluer |

## Bug Fix Clé de Cette Session

### RowsetNb vs rscount (CRITIQUE)

**Symptôme**: Scenarii harvest vide ou cartes sans contenu

**Cause**: `RowsetNb=14` dans WebBasedGeneratorConfig.cs groupait 14 lignes CSV par carte, alors que le template Scenarii attend **1 ligne par carte** (champs `{{titre}}`, `{{contexte}}`, `{{enjeu}}`).

**Fix**: Supprimer `RowsetNb=14` pour que CardPen utilise `rscount=1` du template JSON.

```csharp
// WebBasedGeneratorConfig.cs ligne 125-141
new CardSetConfig(){
    Name = KnownCardSets.Scenarii,
    FaceCardSetInfo = new CardSetInfo()
    {
        DataSet = KnownDataSets.Scenarii,
        JsonFilePathRelease = "...",
        JsonFilePathDebug = @"...",
        // RowsetNb NON DÉFINI → utilise rscount=1 du template JSON
    },
    ...
}
```

## Fichiers Clés

```
# Config principale
Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs
  → SkipConfigFile DOIT être false

# CardSet configs
Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/WebBasedGeneratorConfig.cs
  → RowsetNb NON DÉFINI pour Scenarii (utilise rscount du template)

# Templates JSON
Cards/Scenarii/Argumentum_Scenarii_Face_fr.json (rscount=1)
Cards/Fallacies/Argumentum_Virtues_Face_fr.json

# Outputs
bin/Debug/net9.0/Target/fr/Harvest/*.json
bin/Debug/net9.0/Target/fr/Images/density-0/*
bin/Debug/net9.0/Target/fr/Documents/density-0/*
```

## Commande pour Reprendre

```bash
# Vérifier état git
git status
git log --oneline -5

# Lancer pipeline
cd "Generation/Converters/Argumentum.AssetConverter"
dotnet run

# Vérifier outputs
ls bin/Debug/net9.0/Target/fr/Harvest/
ls bin/Debug/net9.0/Target/fr/Images/density-0/
ls bin/Debug/net9.0/Target/fr/Documents/density-0/
```

## Instructions pour Claude

Quand tu reprends cette tâche:

1. **Lis ce fichier** pour comprendre l'état actuel
2. **Vérifie git status** pour voir s'il y a des changements non commités
3. **Consulte CLAUDE.md** pour les règles et leçons apprises
4. **Consulte le plan** dans `.claude/plans/giggly-knitting-yao.md`
5. **Mets à jour ce checkpoint** après chaque session de travail

## Historique des Sessions

### Session 2026-03-03 (Handover à Opus 4.6)

- **État**: PokerCards format "1 Dos + N Faces" validé ✅
- **Problème identifié**: TarotCards ne suit PAS le format "1 Dos + N Faces" (commence par Face)
- **Changements en attente** (non commités):
  - `WebBasedGeneratorConfig.cs`: Rules Back config avec DPI 300, LocalCardpenUrl en https
  - `HarvestManager.cs`: Amélioration extraction card IDs depuis CSV
  - `Argumentum_Rules_Back_fr.json`: Chemins absolus GitHub, colonne 'card' ajoutée
  - `Argumentum_Rules_fr.json`: Chemins absolus GitHub, CSS variantes
- **Fichiers référence ajoutés**: `reference-ru-2-rules.png`, `rules_back_default.png`, `rules_face_01.png`
- **Outil ajouté**: `compare_pdfs.py` pour extraction PDF → PNG
- **Debrief complet**: `.claude/debrief-pipeline-recovery-20260303.md`

### Session 2026-02-02 (Corrections PDF recto-verso)

- **Diagnostic système dos/faces** : Investigation Git complète
  - Comparé Golden Master (avril 2024) vs code actuel
  - Identifié régression dans commit `6bd802f4` (PrintAndPlayDocument)
  - Cause: Filtrage `validBackImages.Where(b => b != null)` désynchronise dos/faces
- **Corrections appliquées** (commit `be1a529e`):
  - PrintAndPlayDocument: Restauré gestion null pour synchronisation dos/faces
  - FallaciesWeb A0: Passé à 12 colonnes (au lieu de 11) pour tenir sur 1 page
- **Format par défaut**: Identifié `BackFirstOneDocPerBack` (1 dos + N faces par famille)
- **CardGridComponent**: Vérifié gestion correcte des null (cellules vides)

### Session 2026-02-02 (Revue PDFs)

- Revue détaillée des 10 PDFs générés par l'utilisateur
- **8 problèmes critiques identifiés** (voir plan Phase 3)
- Système dos/faces cassé → nécessite investigation Git
- Affiche A0 sur 2 pages → passer à 12 colonnes
- Rules sans styles CSS
- Virtues: texte trop petit, couleurs manquantes
- CSV Scenarii mis à jour → régénération nécessaire
- Plan mis à jour: [giggly-knitting-yao.md](file:///C:/Users/jsboi/.claude/plans/giggly-knitting-yao.md)

### Session 2026-02-01 (Suite - Validation Complète)
- **Pipeline FR complètement fonctionnel**
- Régénéré Target complet après suppression
- Validé visuellement carte Scenarii "La mère de César et Cléopâtre"
- 11 PDFs générés avec succès
- Tous les CardSets générés: Fallacies, Fallacies-Web, Virtues, Scenarii, Rules, Memo

### Session 2026-02-01
- Corrigé CSV injection (commit 37600e4a)
- Ajouté CSS argumentsVertueux (commit f0b1cd35)
- Corrigé chemins Scenarii (commit 09b427ef)
- **Fix critique: RowsetNb=14 → non défini** (commit 9b19d5e8)
  - Cause: RowsetNb=14 groupait 14 lignes CSV par carte
  - Template Scenarii attend 1 ligne par carte ({{titre}}, {{contexte}}, etc.)
- Mis à jour CLAUDE.md avec nouvelles leçons
- Créé système de reprise de tâche (.claude/commands/pipeline-recovery.md)
