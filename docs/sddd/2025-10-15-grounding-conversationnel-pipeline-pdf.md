# Documentation SDDD : Grounding Pipeline PDF - Régénération Assets Poker & Tarot

*   **Date** : 15 Octobre 2025 (Clôture Mission)
*   **Auteur** : Roo (Architecte)
*   **Statut** : Validé & Clôturé
*   **Version** : 1.0

---

## 1. Contexte & Objectifs

Cette mission visait à restaurer l'intégrité du pipeline de génération des assets PDF du projet Argumentum, spécifiquement pour les jeux de cartes **Poker** et **Tarot**, suite à un constat d'échec silencieux (fichiers générés vides ou manquants).

L'objectif principal était de garantir que le code source C# (`WebBasedGeneratorConfig.cs`) serve de source unique de vérité ("Code First"), éliminant toute dépendance à des fichiers de configuration JSON générés manuellement ou corrompus.

## 2. Chronologie des Événements & Diagnostic

1.  **Constat Initial** :
    *   Le fichier PDF Poker était généré avec une taille anormale (~14 KB) et vide.
    *   Le fichier PDF Tarot était correct, mais le processus global semblait instable.
    *   Suspicion de corruption LFS (Large File Storage) sur les assets sources.

2.  **Investigation LFS** :
    *   Vérification des pointeurs LFS et restauration des fichiers binaires.
    *   Confirmation que les assets sources étaient présents, écartant la piste LFS comme cause unique du fichier vide Poker.

3.  **Analyse Code Source (La Révélation)** :
    *   L'audit du fichier `WebBasedGeneratorConfig.cs` a révélé que la configuration pour le `CardSetId = "poker"` était explicitement désactivée : `Enabled = false`.
    *   Découverte d'un problème de "contamination d'état" en Javascript côté CardPen (`rscount` global) affectant potentiellement le rendu si plusieurs decks étaient générés séquentiellement sans reset propre.

4.  **Correction "Code First"** :
    *   Réactivation du Poker dans le code C#.
    *   Réintégration du CardSet "Memo" manquant.
    *   Suppression du fichier `AssetConverterConfig.json` du suivi Git (ajout au `.gitignore`) pour forcer l'utilisation de la config C# compilée.

## 3. Analyse Technique Approfondie

### 3.1. Le Piège de la Double Configuration
Le système permettait une surcharge de la configuration via un fichier JSON externe. Ce fichier, généré lors de runs précédents, pouvait contenir des états obsolètes (ex: Poker désactivé) qui prévalaient sur le code C# fraîchement modifié.
**Solution** : Le fichier JSON a été banni du contrôle de source. La vérité est dans le code compilé.

### 3.2. Contamination d'État JS (CardPen)
Le moteur de rendu HTML/JS (CardPen) utilisait des variables globales pour compter les ressources. Lors de générations par lots (Tarot puis Poker), l'état n'était pas parfaitement nettoyé, risquant de fausser la pagination ou le rendu du second document.
**Contournement** : Le script de génération redémarre le processus ou assure une isolation suffisante (bien que la correction C# ait suffi pour le problème "vide", ce point reste sous surveillance).

## 4. État Final Validé (13/12/2025)

La génération via le pipeline corrigé produit les artefacts suivants, validés par taille et inspection visuelle :

| Artefact | Taille Validée | Contenu |
| :--- | :--- | :--- |
| **Argumentum-TarotCards-Restored_fr-FacesOnly.pdf** | **12.1 MB** | 78 cartes + Dos (Complet) |
| **Argumentum-PokerCards-Restored_fr-FacesOnly.pdf** | **12.1 MB** | 54 cartes + Dos (Complet) |
| **Argumentum-Fallacies-Web-A0-Restored_fr.pdf** | **3.6 MB** | Poster A0 (Complet) |

## 5. Recommandations & Maintenance

1.  **Principe Code First** : Ne jamais modifier manuellement les JSON de config dans `bin/`. Toujours passer par `WebBasedGeneratorConfig.cs` et recompiler.
2.  **Monitoring LFS** : Vérifier périodiquement que les gros assets (images HD) ne sont pas convertis en pointeurs texte par erreur lors de merges.
3.  **Script de Référence** : Utiliser exclusivement `Generation/Converters/scripts/01-run-pipeline-success-config.ps1` pour toute régénération future afin de garantir l'environnement (nettoyage processus CardPen, démarrage serveur, compilation Release).

---
*Ce document sert de référence SDDD pour l'état stable du pipeline PDF au 15/10/2025.*