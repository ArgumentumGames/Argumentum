---
name: pipeline-iterate
description: Orchestre un cycle complet validate-fix-validate sur le pipeline Argumentum, jusqu'a resolution ou limite d'iterations. Chef d'orchestre des corrections. Appelle pipeline-validate puis pipeline-fix en boucle.
---

# Skill : Pipeline Iterate

Orchestre un cycle validate → fix → validate jusqu'a ce que tous les problemes soient resolus ou que la limite d'iterations soit atteinte.

---

## Usage

```
/pipeline-iterate [language] [max-iterations] [auto-commit]
```

- `/pipeline-iterate` — FR, 5 iterations max, commits manuels
- `/pipeline-iterate en 10 true` — EN, 10 iterations max, auto-commit
- `/pipeline-iterate fr 3 false` — FR, 3 max, demande confirmation

**Parametres** :
- `language` (defaut: fr)
- `max-iterations` (defaut: 5)
- `auto-commit` (defaut: false)

---

## Workflow

### Initialisation

1. Lire `CLAUDE.md` et `.claude/pipeline-recovery-checkpoint.md`
2. Determiner objectif : langue, CardSets, seuil de qualite (0 critiques, <3 mineurs)

### Boucle principale

```
POUR iteration = 1 A max-iterations:

  ETAPE 1 — Validation
    Appelle pipeline-validate {language}
    Extrait: nb critiques, majeurs, mineurs + liste

  ETAPE 2 — Decision
    SI critiques == 0 ET majeurs == 0 ET mineurs <= 3:
      SUCCES → sortie boucle
    SI iteration >= max-iterations:
      ECHEC → sortie boucle

  ETAPE 3 — Priorisation
    Trier: critiques > majeurs > mineurs
    Selectionner le probleme #1

  ETAPE 4 — Correction
    Appelle pipeline-fix "<probleme>" <report-path>
    Lit fix-report
    SI auto-commit ET resolu: commit fait par pipeline-fix
    SINON: demande confirmation

  ETAPE 5 — Mise a jour etat
    Ajoute entree dans pipeline-recovery-checkpoint.md
    Note le probleme corrige

  iteration++
```

### Finalisation

Generer `iteration-report-{timestamp}.md` :

```markdown
# Pipeline Iteration Report

**Date:** ...
**Language:** ...
**Iterations:** N/max
**Duree:** ...
**Resultat:** SUCCES / PARTIEL / ECHEC

## Progression par iteration

### Iteration 1
- Validation initiale: C=X, M=Y, m=Z
- Correction: "..."
- Fichier: ...
- Status: RESOLU/ECHEC (n/3 tentatives)
- Commit: abc123
- Post-fix: C=X-1, M=Y, m=Z

### Iteration 2
...

## Problemes resolus (N)

1. ...
2. ...

## Problemes restants

### Majeurs (N)
1. ...

### Mineurs (N)
1. ...

## Metriques

| Metrique | Valeur | Cible | Status |
|----------|--------|-------|--------|
| Critiques | 0 | 0 | OK |
| Majeurs | ... | 0 | ... |
| Mineurs | ... | <3 | ... |
| Iterations | N/max | - | ... |
| Taux resolution | X% | >80% | ... |

## Commits

```bash
abc123 - fix(...): ...
```

## Recommandations

1. ...
2. ...
```

---

## Gestion des echecs

### Fix echoue (3 tentatives)
1. Documenter dans iteration-report
2. Marquer "BLOQUE"
3. Continuer avec probleme suivant
4. Note intervention humaine en fin

### Dotnet run echoue
1. Capturer logs
2. Analyser : compilation / runtime / timeout
3. Si non recuperable : STOP + rapport d'echec + escalade

### Limite iterations atteinte
1. Rapport final "PARTIEL"
2. Lister resolus ET restants
3. Recommander : augmenter limite / intervention / nouvelle strategie

---

## Modes d'usage

| Mode | Commande | Usage |
|------|----------|-------|
| Auto complet | `/pipeline-iterate fr 10 true` | Lance et oublie |
| Semi-auto | `/pipeline-iterate fr 5 false` | Valide + propose, confirme |
| Debug | `/pipeline-iterate fr 1 false` | Une iteration, controle total |

---

## Rappels

- **Un probleme a la fois** : pas de correction parallele
- **Valider apres CHAQUE fix** : pas de batch sans validation
- **Documenter tous les echecs** : pattern recurrent → leçon CLAUDE.md
- **Respecter max-iterations** : eviter boucles infinies
- **Commits frequents** : un fix resolu = un commit
- **Checkpoint** : mis a jour apres chaque iteration, pas juste a la fin
