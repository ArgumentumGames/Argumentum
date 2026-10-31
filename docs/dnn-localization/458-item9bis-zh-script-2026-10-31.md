# #458 grain module-level — contrôle d'écriture chinoise sur les trois gardes zh-CN (60 tests)

**Date :** 2026-10-31 · **Lane :** po-2023 · **Culture :** `zh-CN` seule
**Périmètre :** `DnnModuleLevelControlsZhResxGuardTests`, `DnnModuleLevelAdminZhResxGuardTests`, `DnnModuleLevelDtmAdminZhResxGuardTests` — **3 fichiers de test, 0 `.resx` touché**
**Origine :** pool po-2023, item 7 (« contrôle d'écriture chinoise »), après constat que l'item 6 (instrument d'extraction) était déjà committé (`git ls-files`).

---

## 1. Le trou que ce lot ferme

Les trois gardes zh héritaient du contrôle « valeur ≠ anglais » de leur socle. Ce contrôle n'attrape que les copies **intégrales** : une valeur dont la moitié est chinoise et l'autre moitié anglaise lui est **identique-ment invisible** — elle diffère de l'anglais, donc elle passe, et la carte affiche du texte à moitié traduit.

Le contrôle ajouté exige la présence d'**au moins un caractère chinois** dans chaque valeur livrée. Motif : `[㐀-䶿一-鿿豈-﫿]` — bloc Ext-A, bloc unifié et formes de compatibilité. Les trois sont nécessaires ; un motif limité à U+4E00–U+9FFF laisserait passer les formes de compatibilité présentes dans le corpus.

Aucun `.resx` n'est modifié : les 1 101 valeurs zh du périmètre portent déjà l'écriture chinoise — la règle est là pour que la prochaine livraison ne puisse pas les dégrader en silence.

## 2. Dénominateur mesuré avant écriture

Census indépendant (`python -I`, `ElementTree`, tous les `*.zh-CN.resx` de `DNNPlatform/`) : **21 valeurs sans écriture chinoise** sur toute la surface zh, toutes attribuées :

| Pack | Valeurs sans CJK | Statut dans ce lot |
|---|---:|---|
| `App_GlobalResources` (`GlobalResources`, `SharedResources`, `WebControls`) | 11 | **hors périmètre** (grain module-level) — `[Custom:0]`, `URL：`, `True`/`False`, `APP ID`, `A-Z`, `B`/`KB`/`MB`/`GB`, `：` |
| `controls` (11 fichiers, 133 valeurs) | 2 | **déjà déclarés** verbatim : `TextEditor.ascx:Html.Text` = `HTML`, `UrlTrackingControl.ascx:URL.Text` = `URL` |
| `admin` (27 fichiers, 328 clés livrées) | **0** | aucune exemption nécessaire |
| `DesktopModules/Admin` (20 fichiers, 640 clés livrées) | 7 | **6 verbatim** déjà déclarés (4 marques, `Skype ID`, filtre `A–Z`) + **1 gabarit de format** (§3) |
| `Portals/_default/Skins/2shineBS5` | 1 | hors périmètre — `Menu.Tab170.Url` = `/fallacies_zh.html` |

⚠️ Une valeur sans écriture chinoise n'est pas un défaut en soi : c'est une **décision à déclarer**. Le lot ne supprime aucune de ces valeurs ; il rend impossible l'apparition silencieuse de la 22ᵉ.

## 3. Une exemption nouvelle, déclarée et épinglée

Seul ajout de liste du lot : `Admin/Security/MemberServices.ascx:TrialFee.Text` = `{0}（{1} {2}）`.

Sa prose vient **entièrement des arguments** `{1}`/`{2}` ; ses parenthèses sont en **pleine chasse** (U+FF08/U+FF09), hors des trois blocs CJK. Exiger un caractère chinois dans ce gabarit reviendrait à exiger de la mise en forme.

`ScriptExempt` = les 6 verbatim **plus** cette entrée = **7**, épinglé par `TheScriptExemptListCannotGrowSilently` (compte **et** identité de la 7ᵉ entrée **et** vivacité : le fichier est dans le lot, la clé existe au neutre).

## 4. La garde

| Classe | Avant | Après | Nouveaux tests |
|---|---:|---:|---:|
| `DnnModuleLevelControlsZhResxGuardTests` | 46 | **58** | 11 théories + 1 fait |
| `DnnModuleLevelAdminZhResxGuardTests` | 111 | **138** | 27 théories |
| `DnnModuleLevelDtmAdminZhResxGuardTests` | 83 | **104** | 20 théories + 1 fait |
| **Total** | 240 | **300** | **60** |

Les deux faits ajoutés ne sont pas décoratifs : ils **épinglent la liste d'exemption** dont dépend la nouvelle règle. Sans eux, ajouter une exemption pour faire passer un rouge serait un geste invisible.

Sur `controls` et `admin`, les règles viennent du **socle abstrait** : la sous-classe ne porte que sa culture et le motif CJK, aucune règle recopiée.

## 5. Contrôles inverses — mesurés avant push

Chaque mutation porte sur **une** valeur réelle, est appliquée avec vérification d'unicité (`count == 1`), restaurée depuis sauvegarde, et suivie d'un `sha256` identique.

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| C1 | `controls/Address.ascx` : `plState.Text` `州：` → `Draft English value ` | nouvelle règle rouge, socle vert | ✅ **1 échec / 57 verts** — *« …Address.ascx.zh-CN.resx:plState.Text — la valeur ne porte aucun caractère chinois »* |
| C2 | `admin/Modules/ModuleSettings.ascx` : `ModuleSettings.Text` `模块设置` → idem | idem | ✅ **1 échec / 137 verts** — même message, clé nommée |
| C3 | `DesktopModules/Admin/Security/EditRoles.ascx` : `BasicSettings.Text` `基本设置` → idem | idem | ✅ **1 échec / 103 verts** — *« …EditRoles.ascx.zh-CN.resx:BasicSettings.Text — traduction partielle ou valeur laissée en anglais »* |
| C4 | `MemberServices.ascx` : `TrialFee.Text` → `Draft English value {0} {1} {2}` (**clé exemptée**) | vert : l'exemption est ce qui autorise | ✅ **0 échec / 104 verts** |
| C5 | **+1 exemption** (`Users.ascx:Filter.Text`) dans `ScriptExempt` | le fait d'épinglage rougit | ✅ **1 échec / 103 verts** — *« Expected ScriptExempt to contain 7 item(s) … but found 8 »* |

**C3 + C4 forment la paire de contrôle** : même famille de garde, latin pur ; rouge sans exemption, vert avec. C1–C3 montrent en plus l'apport réel de la règle — la valeur latine produite par la mutation **diffère** de l'anglais et traversait donc le contrôle du socle : 57/137/103 autres tests restent verts dans le même run.

## 6. Suite complète

**2 931 = 2 926 réussis, 0 échec, 5 ignorés** (2 m 3 s) — master `9f60e1e3` mesuré à 2 871, + les 60 de ce lot : l'arithmétique tombe juste.

⚠️ L'écart d'ignorés (6 sur master, 5 ici) est la **même variance d'environnement local** que celle consignée au lot `admin` fa-IR (#2097) : pas de cause inventée, et le total exact (+60) montre qu'aucun test n'a disparu.

⚠️ **Piège d'instrument payé sur ce lot** : la restauration des mutations se fait par `shutil.copy2`, qui **restaure aussi le mtime**. Le build incrémental a donc gardé l'assembly de C5 et la suite complète a rendu **1 échec fantôme** (`ScriptExempt` à 8) alors que la source était propre. Mesure : `dotnet build -t:Rebuild` puis `dotnet test --no-build` → 0 échec. `--no-incremental` n'est **pas** un commutateur de `dotnet test` (il est rejeté par MSBuild). Règle : après tout contrôle inverse qui mute un `.cs`, recompiler **explicitement** avant de lire un compte.

## 7. Ce que cette PR ne fait pas

- **Aucun `.resx` modifié**, aucune traduction produite : lot de garde uniquement. **Dépense API : 0 $.**
- **`ar-SA` / `fa-IR`** non touchés — leurs lots sont la file de la session sœur (#2096 `controls`, #2097 `admin`).
- **`SharedResources` / `GlobalResources`** (11 valeurs sans CJK) ne sont pas couverts : ils sont hors du grain module-level, et un contrôle d'écriture y demanderait d'abord de trancher `True`/`False`/unités de taille (`B`, `KB`, `MB`, `GB`), qui sont **conventionnellement latins** en chinois.
- **Rien côté préprod** — aucun artefact servi n'est touché.

## 8. Suite

Grains libres du pool po-2023 : `DesktopModules` zh **lot 3c** (9 fichiers / 381 clés, sœur), `DesktopModules` **ar-SA**, puis webroot fast-forward et montée préprod (jours de semaine).
