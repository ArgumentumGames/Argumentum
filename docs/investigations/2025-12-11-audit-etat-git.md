# Audit État Git - 11 Décembre 2025

## 1. Synthèse Exécutive

*   **État** : Divergent (153 modifications).
*   **Diagnostic** : Le dépôt est au milieu d'une opération de correction de bug (Fix C# + Restauration Images LFS) interrompue en Octobre 2025.
*   **Verdict** : NE PAS faire de `git reset --hard`.

## 2. Détail des Modifications (Groupes A/B/C)

### *Groupe A (À Sauvegarder)*
*   **Fix C#** : `Generation/Converters/Argumentum.AssetConverter/WebBasedGenerator/PdfManager.cs` (Correction NullRef sur cartes sans dos).
*   **Images** : `Cards/Fallacies/Assets/*.png` (Restauration confirmée vs git-id).
*   **Scripts** : `run-full-pipeline.ps1`, `prepare-environment.ps1` (Automatisation validée).

### *Groupe B (À Nettoyer)*
*   Logs, backups, fichiers `_CORRUPTED`.
*   `Argumentum_Scenarii_Face_fr.json` (Artefact généré à reverter).

### *Groupe C (À Renommer)*
*   `cleanup.ps1` -> `cleanup-output.ps1`.

## 3. Plan d'Action Immédiat (Next Steps)

1.  Créer branche `fix/recovery-october-2025`.
2.  Stager Groupe A (Code + Images + Scripts déplacés vers `Generation/Scripts`).
3.  Renommer `cleanup.ps1`.
4.  Discard Groupe B.
5.  Valider via `run-full-pipeline.ps1`.