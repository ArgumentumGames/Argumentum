# Épinglage — montée DNN 10.4.1 + 2sxc 22.01 (préprod, 2026-11-02)

État **atteint et mesuré** sur `dnn.argumentum.myia.io` le 2 novembre 2026, entre 11:23 et 12:20.
Ce document épingle la montée : versions cibles, empreintes, défauts rencontrés et leur correctif.
Il ne contient **aucun secret** — ni chaîne de connexion, ni `machineKey`, ni jeton.

## 1. Versions cibles atteintes

| Composant | Avant | Après | Mesure |
|---|---|---|---|
| DNN Platform | **10.3.2.0** † | **10.4.1.0** | Avant : `DotNetNuke.dll` de la copie webroot d'avant montée, FileVersion **10.3.2.0** (Product `10.3.2+Branch.master.Sha.4b0214b8…`, 2 949 632 o) ; après : `DotNetNuke.dll` = 10.4.1.0 ; table `Version` = **10.4.1** (VersionId 1048) |
| 2sxc (`ToSic.Sxc.Dnn`) | **21.07.00** † | **22.1.0.0** | Avant : `ToSic.Sxc.Dnn.dll` de la même copie, **21.07.00** (39 936 o) ; après : 22.1.0.0 ; `Install() = True`, `IsValid = True`, `HasWarnings = False` |

† **Erratum (02/11, après relecture ai-01 [c.6576531353](https://github.com/ArgumentumGames/Argumentum/pull/2123#issuecomment-6576531353))** : la première version de ce tableau portait
« 9.x / 15.2.0 » — les versions de la **production** (9.11.1.19 / 15.02, mesurées par web2 le même jour),
pas celles de la préprod. Les valeurs ci-dessus sont **mesurées sur la copie webroot d'avant montée**
(`.tmp\upgrade-dnn1041\webroot-pre-montee\bin\`, copie du 14/10 15:08), qui est le seul objet dont ce
document prétend décrire la transition. Le repli accepté d'avance « 10.3.3 + 21.07 » du plan commité
confirme l'ordre de grandeur : la préprod était déjà sur la génération 10.3 / 21.07.
| Assistant de montée | — | **`Upgrade Complete`** | 11:32:18 → 11:32:44 (26 s) |

Paquet 2sxc utilisé : `ToSic.Sxc.Dnn.22.01.00_Install.zip`, 11 513 076 o,
sha256 `611D49F9F243503638869242D081485F6C8748F30E93A065AF9DAB042BC05C9C`.

Schéma 2sxc constaté après installation : **371 types de contenu**, **11 541 entités**, app `Default` sur la zone 1.

## 2. Sauvegardes (point de retour)

| Objet | Emplacement | Empreinte |
|---|---|---|
| Base `ArgumentumGames` | `ArgumentumGames-pre-montee-20261102-112355.bak` — 101,8 Mo, vérifiée dans `msdb.dbo.backupset` à 11:23:56 | — |
| Webroot | `.tmp\upgrade-dnn1041\webroot-pre-montee\` — 11 094 fichiers / 864,25 Mo | — |
| `web.config` pré-montée | `.tmp\upgrade-dnn1041\web-config-pre-montee.config` | `874C1978…` |

Ces emplacements sont **hors dépôt** (`.tmp\` est ignoré par git) : ils ne sont pas versionnés ici,
seulement nommés pour que le point de retour reste retrouvable.

## 3. Déroulé mesuré

- **Git** : `master` fast-forward `3554741b` → **`eb6f1452`** ; `/` 200 `fr-FR`.
- **Binaires** : 2 799 fichiers DNN 10.4.1 déposés, `web.config` **exclu** du dépôt de binaires.
- **Rétrogradations** : **0** ; 65 assemblies identiques communes, 152 présentes sur la préprod seule — même forme que la copie de validation.
- **Tensions** : 41 (38 ascendantes, **3 descendantes** : `Microsoft.Identity.Client`, `System.Formats.Asn1`, `System.ValueTuple`) — **signalées, non touchées**.
- **Routes** : 8/8 → 200 avec le `lang` attendu ; **37 scopes 2sxc (`sc-`)** par page ; 13 tables `TsDynData*` intactes.
- **Session authentifiée** : oracle `ctl=Profile` 200 sans redirection, `personaBarSettings` et `PersonaBarContainer` présents.

## 4. Défaut A — l'assistant ne réécrit pas les `bindingRedirect`

Déposer les binaires **avant** la montée laisse ASP.NET démarrer avec l'ancienne configuration : les
`bindingRedirect` de `web.config` nomment encore les versions de la génération précédente (10.3.2 / 21.07), d'où
`FileLoadException 0x80131040` (`AngleSharp.Css` 1.0.0.0 demandé / 1.1.2.0 livré, `Serilog` 4.2.0.0
demandé sans aucun redirect).

Correctif : **22** `newVersion` réalignés sur la version réelle de la DLL dans `bin\`, **17**
`<dependentAssembly>` créés. Les versions et `publicKeyToken` sont relus dans le `bin\` de la préprod ;
la **sélection** des noms est empruntée à la copie de validation du 31/10. Contrôle croisé : **17/17, 0 écart**.

## 5. Défaut B — `bindingRedirect` superposés dans un même bloc

L'installation 2sxc laisse, sur trois noms, **deux `<bindingRedirect>` dans le même `<dependentAssembly>`** :

```xml
<bindingRedirect oldVersion="0.0.0.0-10.0.0.12" newVersion="10.0.0.12" />  <!-- DNN 10.4.1 -->
<bindingRedirect oldVersion="0.0.0.0-2.1.1.0"  newVersion="2.1.1.0"  />     <!-- 2sxc / EF Core 2.1.1 -->
```

Noms concernés : `Microsoft.Extensions.Logging.Abstractions`, `Microsoft.Extensions.Primitives`,
`Microsoft.Extensions.Configuration.Abstractions`. Une requête en `2.1.1.0` tombe dans la seconde
plage et résout vers **une version absente de `bin\`** (seule 10.0.0.12 est livrée).

**Symptôme, et pourquoi il est piégeux** : 32 `FileLoadException … Version=2.1.1.0` et 32
« can't find 2sxc instance configuration » — **invisibles côté HTTP**, les pages répondaient 200.
Seul le journal d'événements DNN les portait (jointure `Exceptions` sur `ExceptionHash` ; la table
`EventLog` n'a **pas** de colonne `Severity`).

Correctif : retrait des 3 redirects `2.1.1.0`, ce qui ramène les blocs à la forme de la copie validée
(`0.0.0.0-10.0.0.12 → 10.0.0.12`). Delta mesuré : **exactement 3 lignes remplacées par une ligne vide**,
1 206 lignes inchangées (110 626 → 110 419 o).

| Mesure | Avant | Après |
|---|---|---|
| Exceptions `2.1.1.0` | 32 (dernière à **12:01:40**) | **0** depuis 12:07 |
| Entrées `EventLog` sur `/`, `/en`, `/es` | 32 + 32 | **0** |
| Scopes `sc-` par page | 37 | 37 |

## 6. Definition of Done — explorateur de données 2sxc

- `GET /api/2sxc/app/Default/data/ContentType` (session authentifiée) → **200**, 7 319 o de JSON.
- Shell de l'UI : `DesktopModules/ToSic.Sxc/dist/ng-edit/index-raw.html` → **200**, `<title>Content Management</title>`
  (le fichier est `index-raw.html`, **il n'existe pas d'`index.html`** : une sonde sur `index.html` rend un 404 trompeur).
- **Écran rendu** dans un navigateur authentifié : l'entrée « Contenu » de la PersonaBar ouvre l'explorateur
  (« News Data », 5 items réels, filtres Items/Columns, barre Recycle Bin / Data / Item / Create Metadata).
  Captures transmises à ai-01 par message privé, **jamais publiées** ici.

## 7. Manifeste d'empreintes

`bin-manifest.tsv` — **318 fichiers** de `DNNPlatform\bin` (217 DLL, 46,4 Mo), colonnes
`fichier`, `octets`, `sha256`, `version d'assembly`.
Le fichier lui-même : sha256 `A9D2CCD634D65B4A7821A2DF765684DE8E8C0328D547E14683FEF102380CD3C1`, 33 743 o.

## 8. Exclusions volontaires

| Objet | Pourquoi il n'est pas épinglé ici |
|---|---|
| `web.config` | Porte la chaîne de connexion et la `machineKey`. Seule son empreinte est citée : `B59E9AFB789D5B8C0DC9A8D4483C55CAC482430B6396FF7697310D759B885E52` (110 419 o, XML valide, 98 blocs `dependentAssembly`). |
| Fichiers porteurs de clés | `App_Data\`, `Config\` : configuration d'instance, susceptible de contenir des secrets. |
| Contenus (`Portals\`, médias) | Données d'exploitation, sans valeur d'épinglage. |

## 9. État du dépôt

L'arbre `DNNPlatform/` est **intentionnellement sale** après la montée (fichiers 10.4.1 non suivis ou
modifiés) : il n'a **pas** été nettoyé et ces fichiers ne sont **pas** commités. Cette PR n'ajoute que
le présent dossier. Le sort de l'arbre (nettoyage, `.gitignore`, commit) est une décision ai-01, pas une
conséquence de cette PR.

## 10. Retour arrière

Restauration de la base depuis le `.bak` de la phase 0 **et** de la copie de webroot — c'est l'unique
retour arrière. Une restauration de base requiert une fenêtre UAC ; le reste de la montée n'en demande aucune.

## 11. Suite

Parité #1180, puis reprise du pool #458 (lot 4a fa-IR, 12 clés ModuleCreator).
