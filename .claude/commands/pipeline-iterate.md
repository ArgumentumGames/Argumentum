# Skill: Pipeline Iterate

Orchestre un cycle complet de validation-correction-validation jusqu'à ce que tous les problèmes soient résolus ou qu'une limite soit atteinte.

## Usage

```
/pipeline-iterate [language] [max-iterations] [auto-commit]
```

**Exemples:**
- `/pipeline-iterate` - Itère sur FR, 5 itérations max, commits manuels
- `/pipeline-iterate en 10 true` - Itère sur EN, 10 itérations max, auto-commit
- `/pipeline-iterate fr 3 false` - Itère sur FR, 3 itérations max, demande confirmation avant commit

**Paramètres:**
- `language` (défaut: fr) - Langue à traiter
- `max-iterations` (défaut: 5) - Nombre max d'itérations validation-fix
- `auto-commit` (défaut: false) - Commiter automatiquement les corrections

## Workflow Automatique

### Initialisation

**Lis le contexte:**
1. `.claude/pipeline-recovery-checkpoint.md` - État actuel
2. `CLAUDE.md` - Règles et leçons apprises
3. `.claude/plans/giggly-knitting-yao.md` - Plan détaillé (si existe)

**Détermine l'objectif:**
- Langue cible: FR/EN/RU/PT
- CardSets à valider: Tous ou subset spécifique
- Seuil de qualité: 0 problèmes critiques, <3 problèmes mineurs

### Boucle Principale

```
POUR iteration = 1 À max-iterations:

  ┌─────────────────────────────────────────┐
  │ ÉTAPE 1: Validation                     │
  └─────────────────────────────────────────┘

  Appelle /pipeline-validate {language}
  → Génère rapport: validation-report-{timestamp}.md

  Lis le rapport et extrait:
  - Nombre de problèmes critiques
  - Nombre de problèmes majeurs
  - Nombre de problèmes mineurs
  - Liste détaillée des problèmes

  ┌─────────────────────────────────────────┐
  │ ÉTAPE 2: Décision                       │
  └─────────────────────────────────────────┘

  SI (problèmes critiques == 0 ET problèmes majeurs == 0):
    SI (problèmes mineurs <= 3):
      → SUCCÈS: Qualité acceptable atteinte
      → SORTIE de la boucle
    SINON:
      → Continue (corrige mineurs)

  SI (iteration >= max-iterations):
    → ÉCHEC: Limite d'itérations atteinte
    → SORTIE de la boucle

  ┌─────────────────────────────────────────┐
  │ ÉTAPE 3: Priorisation                   │
  └─────────────────────────────────────────┘

  Trie problèmes par priorité:
  1. Critiques (bloquants production)
  2. Majeurs (qualité dégradée)
  3. Mineurs (cosmétiques)

  Sélectionne le problème #1 de priorité max

  ┌─────────────────────────────────────────┐
  │ ÉTAPE 4: Correction                     │
  └─────────────────────────────────────────┘

  Appelle /pipeline-fix "{problème}" validation-report-{timestamp}.md
  → Génère fix-report-{timestamp}.md

  Lis le fix-report et extrait:
  - Statut de la correction (RÉSOLU/ÉCHEC)
  - Nombre d'itérations utilisées
  - Fichiers modifiés

  SI auto-commit == true ET statut == RÉSOLU:
    → Commit automatique (déjà fait par pipeline-fix)
  SINON SI auto-commit == false ET statut == RÉSOLU:
    → Demande confirmation utilisateur pour commit

  ┌─────────────────────────────────────────┐
  │ ÉTAPE 5: Mise à Jour État               │
  └─────────────────────────────────────────┘

  Mets à jour .claude/pipeline-recovery-checkpoint.md:
  - Ajoute entrée dans "Historique des Sessions"
  - Note le problème corrigé
  - Met à jour tableau "CardSets - État de Génération"

  iteration++

FIN BOUCLE
```

### Finalisation

**Génère rapport final:** `iteration-report-{timestamp}.md`

```markdown
# Pipeline Iteration Report

**Date:** 2026-02-02 16:00:00
**Language:** fr
**Iterations:** 3/5
**Durée totale:** 47m 18s
**Résultat:** ✅ SUCCÈS

## Progression par Itération

### Itération 1

**Validation initiale:**
- Problèmes critiques: 2
- Problèmes majeurs: 3
- Problèmes mineurs: 5

**Correction appliquée:**
- Problème: TarotCards_fr-2.pdf page 12 blanche
- Fichier: WebBasedGeneratorConfig.cs (ImageDensity)
- Statut: ✅ RÉSOLU (1/3 tentatives)
- Commit: `abc123de`

**Validation post-fix:**
- Problèmes critiques: 1 (-1)
- Problèmes majeurs: 3 (=)
- Problèmes mineurs: 5 (=)

### Itération 2

**Correction appliquée:**
- Problème: Virtues CSS argumentsVertueux manquant
- Fichier: Argumentum_Virtues_Face_fr.json
- Statut: ✅ RÉSOLU (1/3 tentatives)
- Commit: `def456gh`

**Validation post-fix:**
- Problèmes critiques: 0 (-1) ✅
- Problèmes majeurs: 2 (-1)
- Problèmes mineurs: 5 (=)

### Itération 3

**Correction appliquée:**
- Problème: Rules sans styles CSS
- Fichier: Argumentum_Rules_Face_fr.json
- Statut: ✅ RÉSOLU (1/3 tentatives)
- Commit: `ghi789jk`

**Validation post-fix:**
- Problèmes critiques: 0 (=) ✅
- Problèmes majeurs: 1 (-1)
- Problèmes mineurs: 4 (-1)

**→ Seuil de qualité atteint:** 0 critiques, 1 majeur (<3 mineurs)

## Problèmes Résolus (3)

1. ✅ TarotCards_fr-2.pdf page 12 blanche
2. ✅ Virtues CSS argumentsVertueux manquant
3. ✅ Rules sans styles CSS

## Problèmes Restants (5)

### Majeurs (1)

1. ⚠️ **Scenarii assets non chargés**
   - CardSet: Scenarii Poker FR
   - Impact: 12 cartes avec icônes manquantes
   - Recommandation: Valider URLs GitHub

### Mineurs (4)

1. ⚠️ TarotCards_Virtues texte 9pt (cible: 10pt)
2. ⚠️ Print&Play Memo incomplet (190/200 lignes)
3. ⚠️ A0 padding 2mm (recommandé: 1mm)
4. ⚠️ PDF metadata vide (titre, auteur)

## Métriques Finales

| Métrique | Valeur | Cible | Status |
|----------|--------|-------|--------|
| Problèmes critiques | 0 | 0 | ✅ |
| Problèmes majeurs | 1 | 0 | ⚠️ |
| Problèmes mineurs | 4 | <3 | ⚠️ |
| Itérations utilisées | 3 | <5 | ✅ |
| Taux de résolution | 75% (3/4) | >80% | ⚠️ |
| Durée moyenne/fix | 15m 46s | <20m | ✅ |

## Commits Créés

```bash
abc123de - fix(tarot): resolve blank page 12 in TarotCards_fr-2.pdf
def456gh - fix(virtues): add argumentsVertueux CSS class
ghi789jk - fix(rules): add CSS styles (fonts, colors, backgrounds)
```

## Recommandations

1. **Traiter problème majeur restant** (Scenarii assets)
   - Valider URLs GitHub dans template
   - Vérifier accessibilité depuis CardPen local

2. **Considérer problèmes mineurs:**
   - Virtues texte: Augmenter à 10pt si lisibilité dégradée
   - Memo: Investiguer pourquoi 10 lignes manquantes
   - A0 padding: Tester 1mm pour optimiser espace
   - PDF metadata: Ajouter dans config QuestPDF

3. **Prochaine itération:**
   - Lancer `/pipeline-iterate fr 2 true` pour finir FR
   - Puis passer à EN: `/pipeline-iterate en 5 true`

## État du Pipeline

**Langue FR:** 95% complète
- Fallacies Tarot: ✅ 100%
- FallaciesWeb: ✅ 100%
- Virtues Tarot: ✅ 100%
- Scenarii Poker: ⚠️ 90% (assets manquants)
- Rules Tarot: ✅ 100%
- Memo Tarot: ⚠️ 95% (10 lignes manquantes)

**Prochaines étapes:**
1. Corriger Scenarii assets
2. Investiguer Memo incomplet
3. Passer à langue EN
```

## Gestion des Échecs

### Échec de Correction (Fix Report = ÉCHEC)

**Si un problème ne se résout pas après 3 tentatives dans pipeline-fix:**

1. **Documente l'échec** dans iteration-report
2. **Marque le problème comme "BLOQUÉ"**
3. **Continue avec le problème suivant** dans la liste
4. **Note pour intervention humaine** en fin de rapport

### Échec de Validation (Génération échoue)

**Si dotnet run échoue:**

1. **Capture logs complets**
2. **Analyse l'erreur:**
   - Erreur compilation C# → Fix code
   - Erreur runtime → Analyse stack trace
   - Timeout → Augmente timeout ou investigation perf

3. **Si erreur non récupérable:**
   - STOP l'itération
   - Génère rapport d'échec
   - Demande intervention humaine

### Limite d'Itérations Atteinte

**Si max-iterations atteint sans résoudre tous les problèmes:**

1. **Génère rapport final** avec statut "PARTIEL"
2. **Liste problèmes résolus ET restants**
3. **Recommande:**
   - Augmenter max-iterations si progression constante
   - Intervention humaine si problèmes récurrents
   - Nouvelle stratégie si corrections inefficaces

## Utilisation dans le Workflow

Cet agent est le **chef d'orchestre** des corrections pipeline:

1. **Mode automatique complet:**
   ```
   /pipeline-iterate fr 10 true
   ```
   → Lance et oublie, reviens quand c'est fini

2. **Mode semi-automatique:**
   ```
   /pipeline-iterate fr 5 false
   ```
   → Valide et propose corrections, demande confirmation

3. **Mode debug:**
   ```
   /pipeline-iterate fr 1 false
   ```
   → Une itération à la fois, contrôle total

## Sorties Réutilisables

- **iteration-report-{timestamp}.md:** Traçabilité complète du cycle
- **Tous les validation-report-*.md:** Progression itération par itération
- **Tous les fix-report-*.md:** Détails de chaque correction
- **Checkpoint mis à jour:** État synchronisé en temps réel

## Rappels Critiques

- **Ne pas corriger en parallèle** - Un problème à la fois pour éviter conflits
- **Valider après CHAQUE fix** - Pas de batch correction sans validation
- **Documenter TOUS les échecs** - Même pattern d'échec = leçon pour CLAUDE.md
- **Respecter max-iterations** - Éviter boucles infinies
- **Commiter fréquemment** - Chaque fix résolu = commit (si auto-commit)
- **Mettre à jour checkpoint** - Après CHAQUE itération, pas juste à la fin
