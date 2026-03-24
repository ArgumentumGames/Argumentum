# Skill: Pipeline Recovery

Tu reprends le travail de reconstruction du pipeline Argumentum.

## Contexte Automatique

Lis immédiatement ces fichiers dans l'ordre:

1. `.claude/pipeline-recovery-checkpoint.md` - État actuel et prochaines étapes
2. `CLAUDE.md` - Règles du projet et leçons apprises
3. `.claude/plans/giggly-knitting-yao.md` - Plan détaillé (si existant)

## Actions à Effectuer

### 1. Diagnostic Initial

```bash
# État git
git status
git log --oneline -5

# Vérifier dernière génération
ls -la "Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target/fr/Harvest/"
```

### 2. Vérifier les Outputs

Examine visuellement quelques images générées pour valider:
- Fallacies: `Target/fr/Images/density-0/Fallacies/*.png`
- Virtues: `Target/fr/Images/density-0/Virtues/*.png`
- Scenarii: `Target/fr/Images/density-0/Scenarii/*.png`

### 3. Identifier la Prochaine Tâche

Selon l'état du checkpoint:
- Si images manquantes → régénérer avec `dotnet run`
- Si images OK mais PDFs manquants → activer QuestPdfGeneration
- Si FR OK → passer aux autres langues

### 4. Mettre à Jour le Checkpoint

Après chaque session, mets à jour:
- `.claude/pipeline-recovery-checkpoint.md`
- Ajoute une entrée dans "Historique des Sessions"

## Objectif Final

Pipeline fonctionnel pour:
- 4 langues: FR, EN, RU, PT
- Tous les CardSets: Fallacies, FallaciesWeb, Virtues, Scenarii, Rules, Memo
- Tous les formats: Tarot, Poker, A0, Print&Play
- PDFs générés et validés

## Rappels Critiques

- **SkipConfigFile** doit être `false` dans AssetConverterConfig.cs
- **CSV**: Ne JAMAIS modifier avant injection dans CardPen
- **rscount**: Ne JAMAIS forcer à 0 dans le code C#
- **CsvType**: Obligatoire dans chaque DataSet pour harvesting
