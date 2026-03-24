# Analyse de l'état Git - Phase 1

**Date** : 2025-12-11
**Auteur** : Roo
**Branche** : `feature/fix-cardpen-generation-issue`
**Dernier Commit** : `d324bd3b` "feat(pipeline): Stabilize visual asset generation pipeline"

---

## 1. Résumé Exécutif

Le dépôt est dans un état de **chantier avancé** suite aux opérations de restauration d'octobre 2025. Les travaux de restauration des assets (LFS/PNG) et de stabilisation du pipeline de génération semblent avoir été effectués mais **non finalisés au niveau Git**.

Il y a un mélange de :
- Restauration d'assets graphiques (PNG).
- Nettoyage technique (suppression des `.REMOVED.git-id`).
- Corrections de code fonctionnelles (C# et JS).
- Artefacts d'investigation et de test (JSON, PS1, MD).

## 2. Analyse Détaillée

### 2.1 Assets Graphiques (Cards/Fallacies/Assets)
Une opération de restauration massive a eu lieu.
- **Staged (New File)** : De nombreux fichiers `.png` sont prêts à être commités (ex: `Cards/Fallacies/Assets/Fallacy-front/1.2.2.3.1.png`).
- **Untracked** : D'autres fichiers `.png` sont présents mais non suivis.
- **Deleted (Not Staged)** : Les fichiers marqueurs `.REMOVED.git-id` correspondants ont été supprimés du disque, ce qui confirme le remplacement des placeholders par les vrais fichiers.

### 2.2 Code Source (Modifications non stagées)
Des modifications critiques ont été apportées au moteur de génération.

#### CardPen (Génération Visuelle)
- **Frontend (JS)** : Modifications lourdes dans `const.js` (+432/-0 lignes ?), `main.js`, `frame.js`.
- **Backend (C#)** : `Program.cs` modifié.
- **Config** : `package.json`, `appsettings.json`.

#### AssetConverter (Logique Métier)
- **Cœur** : `CardSetDocument.cs` (+178/-178 lignes), `HarvestManager.cs` (+305/-5 lignes).
- **Config** : `WebBasedGeneratorConfig.cs`, `AssetConverterConfig.cs`.
- **Sortie** : `PdfManager.cs`.

### 2.3 Fichiers Critiques et Configuration
De nombreux fichiers de configuration et de sauvegarde polluent la racine de `Generation/Converters/...` :
- `AssetConverterConfig.backup.json`
- `AssetConverterConfig_CORRUPTED_...json`
- `AssetConverterConfig.test.json`
- `AssetConverterConfig.validation.json`

Ces fichiers doivent être triés : soit ignorés (.gitignore), soit supprimés, soit archivés.

### 2.4 Documentation et Scripts
Le dossier `docs/investigations/` contient tout l'historique des opérations d'octobre (du 15/10 au 23/10) qui n'a jamais été commité.
- C'est une **dette documentaire positive** : il faut absolument commiter ces fichiers pour conserver la trace des travaux.

## 3. Risques Identifiés

1.  **Perte de contexte** : Si un `git reset` est effectué par erreur, tout le travail de restauration des assets et les corrections du générateur seront perdus.
2.  **Commit "Fourre-tout"** : Le volume de changements incite à faire un gros commit "WIP", ce qui rendrait l'historique illisible.
3.  **Pollution** : Les fichiers de config temporaires risquent d'être commités par erreur.

## 4. Recommandations pour la Phase 2

Il est impératif de procéder par étapes pour assainir l'état Git :

1.  **Sécurisation des Assets** : Commiter d'abord les ajouts de PNG et suppressions de `.git-id` (Commit: "chore(assets): restore missing LFS files").
2.  **Sauvegarde Documentation** : Commiter tous les fichiers `docs/investigations/` (Commit: "docs: add investigation reports from Oct 2025").
3.  **Nettoyage** : Supprimer ou ignorer les fichiers `*.backup.json`, `*.CORRUPTED.json`.
4.  **Review Code** : Analyser finement les changements C#/JS avant de les commiter (Commit: "feat(generator): update CardPen and AssetConverter logic").

---
*Fin du rapport.*
