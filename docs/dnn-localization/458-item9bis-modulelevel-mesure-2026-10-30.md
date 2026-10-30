# #458 — grain suivant, famille **module-level** : mesure réelle (ar-SA / fa-IR / zh-CN)

> **Statut : document de mesure + script de reproduction.** Aucun `.resx` n'est modifié,
> aucun `.resx` de préprod ni de prod n'est touché. Précédent : PR #2065 (même méthode pour la
> famille AGR cœur du Tier 2).

---

## 1. Ce que le dispatch annonce, et ce qu'il ne dit pas

Le dispatch d'ai-01 du 29/10 22:23Z (commentaire #458, §5 et §7) nomme le grain suivant :

> §7 — « Portée : fichiers `App_GlobalResources` uniquement — les ~94 fichiers de module sans
> version ar/fa/zh sont un grain **suivant**, non ce grain »
>
> §5 — « Module-level : ~94 fichiers de module sur 97 « cœur » n'ont pas de version ar/fa/zh.
> **Grain suivant, après le Tier 2.** »

Le Tier 2 (grain ⑨-bis, famille AGR cœur) est **livré** : PR #2067 (zh-CN), #2068 (ar-SA),
#2070 (fa-IR), 358 clés par langue. Le grain suivant est donc celui-ci.

**Ce que le dispatch ne dit pas** — et sans quoi on ne peut ni chiffrer ni slicer :

1. combien de **chaînes** traduire par langue (le dispatch compte des *fichiers*) ;
2. combien de ces chaînes portent un **placeholder** `{n}` (donc quels multisets de placeholders
   doivent être préservés à l'identique) ;
3. si la référence **ru-RU** peut servir de miroir du jeu de clés (c'est elle qui a servi de
   référence de sens dans les packs Tier 1).

Ce document répond à ces trois questions **par mesure sur les fichiers**, pas par estimation.

## 2. Méthode

Script **lecture seule** : [`458-item9bis-modulelevel-extract.py`](458-item9bis-modulelevel-extract.py)
— il n'écrit rien, ne crée aucun fichier de langue, ne refuse rien. Sortie texte + rapport JSON
optionnel (`--json`).

```bash
python docs/dnn-localization/458-item9bis-modulelevel-extract.py --root .
```

Ce que fait le script :

- parcourt `DNNPlatform/**/*.resx` ;
- regroupe les fichiers par **famille** (chemin sans culture, sans le suffixe `.resx` — cf. §6.1) ;
- classe chaque famille : `agr-core` (`App_GlobalResources`), `module-level` (`App_LocalResources`),
  `autre` ;
- pour chaque famille : nombre de `<data>` du neutre, valeurs **non vides**, valeurs portant un
  `{n}`, et comparaison du **jeu de clés** avec la référence ru-RU.

## 3. Le dénominateur réel

### 3.1 Vue d'ensemble

| Périmètre | familles | avec neutre | clés | non vides |
|---|---:|---:|---:|---:|
| module-level (`App_LocalResources`) | 279 | 197 | 8 020 | 7 763 |
| … dont **cœur** (ru+pt+es+fr) | **100** | 100 | 2 421 | 2 414 |
| … **cœur sans ar/fa/zh** | **96** | 96 | **2 362** | **2 355** |

⇒ **2 355 clés à produire par langue** (3 langues ⇒ 7 065), contre **358** pour le Tier 2 :
ce grain est **6,6× plus gros**.

### 3.2 Le « cœur » — définition mesurée

Le « cœur » = les familles portant **ru-RU + pt-BR + es-ES + fr-FR**, c'est-à-dire celles que
**dnnsoftware** livre localisées (le pack maison ru-RU est le seul pack amont complet disponible).
Définition **opérationnelle**, reproductible par le script, et non une sélection à l'œil.

4 familles de ce cœur portent **déjà** ar/fa/zh (le Tier 1 #1995) et sont donc exclues du reste :
`DesktopModules/Admin/Authentication/…/Login.ascx`,
`DesktopModules/AuthenticationServices/DNN/…/Login.ascx`, `admin/Skins/…/Login.ascx` et
`Portals/_default/Skins/2shineBS5/…/2shine`.

### 3.3 Placeholders

**59 clés** du périmètre à produire portent un `{n}`. Le DoD doit donc comparer le **multiset**
des placeholders au neutre, clé par clé — la même règle que le Tier 2, où le contrôle inverse
(amputer un `{0}`) doit rougir le test.

### 3.4 La référence ru-RU n'est **pas** un miroir du neutre

| | fichiers |
|---|---:|
| jeu de clés **identique** au ru-RU | **81 / 96** |
| jeu de clés **différent** | **15 / 96** |

Les 15 divergents (neutral vs ru-RU) :

| Fichier | neutre | ru-RU |
|---|---:|---:|
| `DesktopModules/Admin/Dnn.PersonaBar/Modules/Dnn.AdminLogs/…/AdminLogs` | 116 | 97 |
| `DesktopModules/Admin/SearchResults/…/SearchableModules` | 10 | 9 |
| `DesktopModules/Admin/Security/…/Profile.ascx` | 93 | 89 |
| `DesktopModules/Admin/Security/…/ProfileDefinitions.ascx` | 22 | 18 |
| `DesktopModules/CoreMessaging/…/SharedResources` | 28 | 26 |
| `DesktopModules/Journal/…/SharedResources` | 95 | 87 |
| `DesktopModules/RazorModules/RazorHost/…/CreateModule.ascx` | 25 | 23 |
| `DesktopModules/SocialGroups/…/SharedResources` | 56 | 55 |
| `Install/…/Installwizard.aspx` | 140 | 133 |
| `Install/…/UpgradeWizard.aspx` | 47 | 33 |
| `admin/Modules/…/ModuleSettings.ascx` | 114 | **117** |
| `admin/Skins/…/Copyright.ascx` | 1 | **0** |
| `admin/Skins/…/Language.ascx` | 9 | **1** |
| `admin/Skins/…/User.ascx` | 12 | 11 |
| `controls/…/TextEditor.ascx` | 13 | 12 |

⇒ **le DoD se dérive du fichier neutre**, pas du ru-RU : une garde qui prendrait ru pour
référence accepterait des clés manquantes sur ces 15 fichiers (et le cas `Copyright.ascx`,
où ru n'a **aucune** clé, illustre le risque).

### 3.5 Recoupement avec les chiffres du dispatch

Les deux chiffres du dispatch se retrouvent dans la mesure :

- « **97 « cœur »** » = 100 familles du cœur **− 3** `Login.ascx` (déjà en Tier 1) ;
- « **~94 fichiers** » = 96 fichiers du cœur sans ar/fa/zh **− 2** fichiers d'installation
  (jamais affichés sur un site en service, cf. §4).

La seconde lecture est une **cohérence arithmétique**, pas une preuve de la méthode d'ai-01 ;
la première est une égalité exacte.

## 4. Découpe proposée (par dossier)

| Dossier | fichiers | clés à produire / langue | dont placeholders |
|---|---:|---:|---:|
| `DesktopModules/` | 56 | 1 700 | 31 |
| `admin/` | 27 | 335 | 11 |
| `controls/` | 11 | 133 | 5 |
| `Install/` | 2 | **187** | 12 |
| **total** | **96** | **2 355** | **59** |

⚠️ **`Install/` (2 fichiers, 187 clés) : assistant d'installation et de mise à niveau de DNN.**
Ces écrans ne sont affichés qu'au **premier déploiement** ; sur un site en service ils ne le sont
jamais. **Recommandation : les exclure du grain** — décision à ai-01. Le périmètre devient alors
**94 fichiers / 2 168 clés / langue**.

Les 15 plus gros fichiers (pour l'ordre de grandeur des lots) :

| Fichier | clés non vides | placeholders | parité ru |
|---|---:|---:|---|
| `DesktopModules/Admin/HtmlEditorManager/…/ProviderConfiguration.ascx` | 142 | 1 | oui |
| `Install/…/Installwizard.aspx` | 140 | 9 | **non** |
| `DesktopModules/Admin/Dnn.PersonaBar/…/AdminLogs` | 116 | 3 | **non** |
| `DesktopModules/CoreMessaging/…/View.ascx` | 114 | 1 | oui |
| `admin/Modules/…/ModuleSettings.ascx` | 114 | 2 | **non** |
| `DesktopModules/Journal/…/SharedResources` | 95 | 2 | **non** |
| `DesktopModules/Admin/Security/…/Profile.ascx` | 93 | 1 | **non** |
| `DesktopModules/MemberDirectory/…/Settings.ascx` | 70 | 0 | oui |
| `DesktopModules/Admin/Security/…/Users.ascx` | 61 | 1 | oui |
| `DesktopModules/Admin/Security/…/EditProfileDefinition.ascx` | 56 | 0 | oui |

## 5. Ce qui n'est **pas** du cœur (à ne pas y mêler)

| Ensemble | familles | clés | locales présentes | statut |
|---|---:|---:|---|---|
| `Dnn.PersonaBar` (admin DNN) | 81 | 5 351 | fr-FR + ru-RU | pack DNN propre, **pas** le pack 6 langues du cœur |
| `NBright` / `NBrightBuy` (e-commerce) | ~25 | ~1 700 | es/fr/nl/uk | **module tiers**, son propre jeu de locales |
| sans aucune locale de référence | 16 | 248 | (aucune ou de-DE) | sans miroir : hors périmètre d'un pack de langue |

Le reste du module-level (8 020 clés) n'est **pas** le grain nommé par le dispatch : c'est une
découpe à trancher par ai-01, dans un grain distinct.

## 6. Pièges d'instrument (mesurés pendant cette mesure)

1. **Le neutre doit être indexé sans son suffixe `.resx`.** Sinon la famille ne retrouve pas ses
   versions culturelles et l'instrument affiche « 0 locale » partout — un zéro qui prouve le
   contraire de la réalité. Corrigé dans le script livré ; c'est ce qui a produit la première
   lecture fausse « 0 fichier du cœur ».
2. **Comparer à un neutre vide ne prouve rien** (piège `List_Country` du Tier 2). Ici : 7 clés
   vides sur le périmètre, et `Copyright.ascx` dont le ru-RU n'a **aucune** clé.
3. **Un fichier déjà traduit n'est pas à retraduire** : 4 familles du cœur portent ar/fa/zh.
4. **82 familles module-level n'ont pas de fichier neutre** (locale seule) : sans neutre, pas de
   référence de clés — les exclure du dénominateur.
5. **Un compte de fichiers n'est pas un dénominateur** : « ~94 fichiers » ne dit rien du volume de
   travail ; 94 fichiers peuvent valoir 200 clés comme 2 000.

## 7. Portée

- **Lecture seule** : aucun `.resx` modifié, aucun fichier de langue créé, aucune écriture hors
  du dépôt.
- Préprod et prod **non touchées** ; le webroot servi (`D:\Dev\Argumentum`) reste sur `master`.
- Aucun secret, aucune donnée membre, aucun envoi de formulaire.

## 8. Suite

Si ai-01 valide le périmètre (2 168 clés/langue hors installation, ou 2 355 avec), la découpe
**par dossier puis par culture** donne des PR de l'ordre de **500–700 clés** — granularité
comparable au Tier 2 (358 clés/lot). Le DoD reprend celui du dispatch : parité exacte des clés
(neutral → culture), **0** valeur identique à l'anglais, **0** identique au français (sauf
verbatim déclarés), **0** clé manquante, plus un **contrôle inverse** (une valeur ramenée à
l'anglais doit rougir la garde).
