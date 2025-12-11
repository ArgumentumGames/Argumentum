# 📜 Rapport d'Archéologie Git : Régression DocumentConfigurations

**Date de génération** : 2025-10-21 03:17:22  
**Répertoire d'analyse** : `d:\Dev\Argumentum`  
**Fichiers analysés** : 3

---

## 🎯 Objectif

Retrouver le commit qui a introduit la régression dans les DocumentConfigurations, causant la perte des CardSets dans les PDFs Poker.

---

## 📊 Résultats

### Dernier Commit Fonctionnel

- **Hash** : `d324bd3b`
- **Message** : feat(pipeline): Stabilize visual asset generation pipeline
- **Fichier extrait** : `d:\Dev\Argumentum\docs\investigations\archeologie-git\WebBasedGeneratorConfig_d324bd3b_20251021_031720.cs`
- **CardSets détectés** : ✅ OUI
- **DocumentConfigurations détectés** : ✅ OUI

### Premier Commit Cassé

⚠️ **AUCUN commit cassé identifié**

---

## 📁 Fichiers Générés

| Fichier | Description |
|---------|-------------|
| `cardset_analysis_20251021_031720.json` | 954 octets |
| `commits_history_AssetConverterConfig.cs_20251021_031720.txt` | 1108 octets |
| `commits_history_CardSetDocument.cs_20251021_031720.txt` | 78 octets |
| `commits_history_WebBasedGeneratorConfig.cs_20251021_031720.txt` | 1023 octets |
| `WebBasedGeneratorConfig_0391bf2b_20251021_031720.cs` | 25193 octets |
| `WebBasedGeneratorConfig_04cff567_20251021_031720.cs` | 26240 octets |
| `WebBasedGeneratorConfig_082073ec_20251021_031720.cs` | 2024 octets |
| `WebBasedGeneratorConfig_0a515939_20251021_031720.cs` | 26228 octets |
| `WebBasedGeneratorConfig_1aad1696_20251021_031720.cs` | 26228 octets |
| `WebBasedGeneratorConfig_2749dc14_20251021_031720.cs` | 25157 octets |
| `WebBasedGeneratorConfig_44093efb_20251021_031720.cs` | 42608 octets |
| `WebBasedGeneratorConfig_6723d628_20251021_031720.cs` | 26228 octets |
| `WebBasedGeneratorConfig_6edf683c_20251021_031720.cs` | 26274 octets |
| `WebBasedGeneratorConfig_8c085c91_20251021_031720.cs` | 42608 octets |
| `WebBasedGeneratorConfig_ad019445_20251021_031720.cs` | 42551 octets |
| `WebBasedGeneratorConfig_ad6c8c45_20251021_031720.cs` | 25157 octets |
| `WebBasedGeneratorConfig_c8e7dd2f_20251021_031720.cs` | 25193 octets |
| `WebBasedGeneratorConfig_d324bd3b_20251021_031720.cs` | 20540 octets |
| `WebBasedGeneratorConfig_f7641878_20251021_031720.cs` | 20519 octets |
| `WebBasedGeneratorConfig_REFERENCE_FUNCTIONAL_20251021_031720.cs` | 20540 octets |

---

## 🔧 Prochaines Étapes

1. **Examiner le diff** entre le dernier commit fonctionnel et le premier commit cassé
2. **Restaurer la configuration** depuis le commit fonctionnel
3. **Vérifier** si le problème vient du code C# ou de la logique de génération JSON
4. **Tester** la génération de PDFs après restauration

---

## 📋 Versions Extraites

| Commit | Message | CardSets | DocConfigs | Taille |
|--------|---------|----------|------------|--------|
| `d324bd3b` | feat(pipeline): Stabilize visual asset generation pipeline | ✅ | ✅ | 734 |
| `f7641878` | refactor(core): General improvements to asset generation logic | ✅ | ✅ | 733 |
| `082073ec` | feat(generation): Enhance asset converter and add PDF auditor | ❌ | ❌ | 100 |
| `6723d628` | feat: Clean up repository and update documentation | ✅ | ✅ | 910 |
| `6edf683c` | feat: Refactor MindMap generation for Virtues and Fallacies | ✅ | ✅ | 910 |
| `04cff567` | Fix: Mise à jour des configurations et des classes du convertisseur d'assets | ✅ | ✅ | 912 |
| `0a515939` | Updated default documents | ✅ | ✅ | 910 |
| `1aad1696` | Updated default documents | ✅ | ✅ | 910 |
| `c8e7dd2f` | Updated images, fallacy taxonomy, introduced cross links | ✅ | ✅ | 872 |
| `0391bf2b` | Updated images, fallacy taxonomy, introduced cross links | ✅ | ✅ | 872 |
| `ad6c8c45` | Updating images and promoting main config parameters | ✅ | ✅ | 871 |
| `2749dc14` | Updating images and promoting main config parameters | ✅ | ✅ | 871 |
| `44093efb` | include Html override | ✅ | ✅ | 1264 |
| `8c085c91` | include Html override | ✅ | ✅ | 1264 |
| `ad019445` | Mindmap virtues | ✅ | ✅ | 1262 |

---

*Rapport généré automatiquement par le script d'archéologie Git*
