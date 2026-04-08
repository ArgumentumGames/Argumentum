---
name: pipeline-fix
description: Corrige un probleme specifique identifie dans un rapport de validation du pipeline Argumentum, regenere les medias affectes, et valide jusqu'a resolution. Max 3 iterations par probleme.
---

# Skill : Pipeline Fix

Corrige un probleme cible (CSS, dimensions, assets, sync, CSV), regenere les medias affectes, valide. Max 3 iterations par probleme avant escalade.

---

## Usage

```
/pipeline-fix <problem-description> [validation-report-path]
```

- `/pipeline-fix "Virtues CSS argumentsVertueux manquant" Target/fr/validation-report-20260202.md`
- `/pipeline-fix "TarotCards_fr-2.pdf page 12 blanche"`

---

## Workflow

### Phase 1 — Analyse

1. Lire le rapport de validation (si fourni)
2. Identifier :
   - CardSet affecte
   - Type de probleme
   - Fichiers source a modifier
   - Impact (images / PDFs / les deux)

3. Strategie :

| Type probleme | Fichiers a modifier | Regeneration |
|---------------|---------------------|--------------|
| CSS manquant | Template JSON CardPen | Images + PDFs |
| Dimension incorrecte | WebBasedGeneratorConfig.cs | Images + PDFs |
| Asset non charge | Template JSON (URLs) | Images + PDFs |
| Sync dos/faces | PrintAndPlayDocument.cs | PDFs seulement |
| Nb colonnes PDF | CardSetDocumentConfig | PDFs seulement |
| CSV contenu | Fichier CSV source | Images + PDFs |

### Phase 2 — Correction

1. `Read` les fichiers concernes
2. `Edit` avec la correction
3. Verifier syntaxe :
   - JSON : parser
   - C# : `dotnet build`
   - CSV : pas de validation auto

4. Commit (si demande) avec message conventionnel + `Co-Authored-By`.

### Phase 3 — Regeneration ciblee

#### Si Images + PDFs

```bash
cd "Generation/Converters/Argumentum.AssetConverter"
dotnet run -- --non-interactive
```

Surveiller logs du CardSet affecte.

#### Si PDFs seulement

Modifier temporairement `Mode` dans `AssetConverterConfig.cs` :
```csharp
Mode = ConverterMode.QuestPdfGeneration | ConverterMode.PdfAuditor;
```

Relancer, puis **restaurer le Mode**.

### Phase 4 — Validation

1. Images regenerees (echantillon 5-10) :
   - Probleme corrige ?
   - Pas de regression ?
   - Autres cartes non affectees ?

2. PDFs regeneres (lecture complete) :
   - Probleme corrige toutes pages ?
   - Pas de nouvelles pages blanches ?
   - Qualite maintenue ?

3. Logs : grep warnings CardSet, pas de nouvelles erreurs.

### Phase 5 — Iteration (max 3)

**Si probleme persiste** :
1. Analyse approfondie : template complet + config C# + Golden Master (`0087f0ec`) + CLAUDE.md
2. Hypothese alternative + nouvelle correction
3. Limite 3 tentatives → escalade humaine

**Si resolu** : rapport de fix + mise a jour checkpoint.

---

## Rapport de fix

```markdown
# Fix Report: [Probleme]

**Date:** YYYY-MM-DD HH:MM
**CardSet:** ...
**Probleme:** ...

## Diagnostic
- Fichier source: ...
- Cause: ...
- Impact: ...

## Correction
**Fichier:** ...
**Changement:**
```diff
- ...
+ ...
```
**Commit:** `abc123`

## Regeneration
- Commande: ...
- Duree: ...
- Fichiers: ...

## Validation
- Images: ...
- PDFs: ...

## Resultat
**Status:** RESOLU / ECHEC
**Iterations:** N/3
```

---

## Rappels

- Toujours lire avant de modifier
- Valider visuellement (pas de "ça devrait marcher")
- Documenter la cause racine, pas juste le symptome
- Max 3 iterations avant escalade
- Pas de regression sur autres CardSets
