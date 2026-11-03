# #458 — Lot 4a **fa-IR** : l'arithmétique de la base, close avant toute garde

**Date :** 2026-11-03 · **Lane :** po-2023 · **Master au moment de la mesure :** `712bee4a`
**Périmètre mesuré :** 37 fichiers neutres `DNNPlatform/DesktopModules/**/*.resx` qui ont un
frère `ar-SA` et **pas** de frère `fa-IR`.

---

## 1. Pourquoi cette note existe

Le pool #458 porte l'item sous la forme : *« lot 4a fa-IR (37 resx / 689 clés — base
mesurée : 732 cellules texte / 648 noms distincts, "689" = 679 + 10 verbatim à reconfirmer
par arithmétique avant assertion de garde ; aucun `.fa-IR.resx` sur les 37) »*.

Trois chiffres, deux bases. Une garde ne s'écrit pas sur une base incertaine : elle
rougirait sur du contenu correct, ou resterait verte sur un lot incomplet. La consigne
demandait la reconfirmation **avant** la garde. C'est fait ici.

## 2. Dérivation du périmètre — 37, et pas un de plus

Aucune liste n'est recopiée : le lot se **dérive** de l'arbre.

```
candidat  ⇔  <stem>.resx existe  ET  <stem>.ar-SA.resx existe  ET  <stem>.fa-IR.resx n'existe pas
```

Résultat : **37 fichiers**, exactement. Le frère `ar-SA` du même lot a été livré par
#2115 (`623f8fd8`, 37 `*.ar-SA.resx` / 689 clés) — les deux lots portent donc sur le
**même** ensemble, ce qui rend la parité des deux traductions mesurable.

## 3. Mesure, sur les **blobs** et non sur le disque

Lecture par `git cat-file -p HEAD:<chemin>` (même instrument que `DnnResxGuardSocle`) :
sur Windows `core.autocrlf` réécrit les fins de ligne au checkout, et une mesure faite sur
le fichier de l'arbre de travail décrit alors le poste, pas le dépôt.

| Grandeur | Valeur mesurée |
|---|---:|
| Fichiers du lot | **37** |
| Éléments `<data>` (cellules) | **689** |
| Valeurs vides au neutre | **0** |
| Noms de clé **distincts** | **642** |
| Occurrences en trop (33 noms présents dans >1 fichier) | **47** |
| Cellules **verbatim** (`ar-SA` = neutre) | **10** |
| Clés du neutre absentes du frère `ar-SA` | **0** |
| Clés `ar-SA` hors neutre | **0** |
| Écarts de variables (`[...]` / `{...}`) neutre vs `ar-SA` | **0** |
| Éléments `<resheader>` | 148 |

Contrôle de cohérence du comptage : `642 + 47 = 689` ✓ — les deux chiffres décrivent le
même objet par deux chemins.

## 4. L'arithmétique du pool — **confirmée**, et sur quelle base

```
689 cellules  −  10 verbatim  =  679 cellules à traduire
```

La formule du pool « 689 = 679 + 10 » est donc **exacte**. Les 10 verbatim sont les valeurs
que la traduction arabe a laissées en anglais ; elles seront déclarées nommément dans la
garde fa-IR (comme le lot ar-SA l'a fait), jamais absorbées par un plancher.

## 5. Les deux chiffres qui **ne se reproduisent pas** — et ce que j'ai écarté

Le pool annonce *« 732 cellules texte / 648 noms distincts »*. Mon instrument ne les rend
pas. Trois hypothèses ont été testées, et **toutes rejetées** :

| Hypothèse | Mesure | Verdict |
|---|---|---|
| 732 = `<data>` + `<resheader>` | 689 + 148 = **837** | rejetée |
| 648 = union des noms neutre ∪ `ar-SA` | **642** (les deux ensembles sont identiques : 0 manquante, 0 hors neutre) | rejetée |
| 648 = noms distincts + doublons d'un autre découpage | 642 + 47 = 689, pas 648 | rejetée |

Ce que je **n'affirme pas** : que 732/648 soient faux. Je dis qu'ils décrivent un objet que
cet instrument ne produit pas sur les 37 fichiers à `712bee4a` — vraisemblablement un
relevé antérieur, sur un découpage ou un jeu de fichiers différent. **La base retenue est
celle qui est mesurée ici, et elle est corroborée** : le lot ar-SA livré (#2115) annonce
lui aussi **689 clés** sur les mêmes 37 fichiers.

⚠️ Corollaire d'instrument : un `642` n'est un « 648 faux » que si l'instrument **pouvait**
rendre 648. Les trois voies par lesquelles 648 aurait pu apparaître ont été essayées
nommément (§5), pas seulement écartées d'intuition.

## 6. Le lot, fichier par fichier

| Cellules | Fichier |
|---:|---|
| 3 | `Admin/Authentication/App_LocalResources/Authentication.ascx` |
| 1 | `Admin/Authentication/App_LocalResources/Logoff.ascx` |
| 38 | `Admin/Console/App_LocalResources/Settings.ascx` |
| 7 | `Admin/Console/App_LocalResources/ViewConsole.ascx` |
| 1 | `Admin/Dnn.EditBar/App_LocalResources/AddExistingModule` |
| 3 | `Admin/Dnn.EditBar/App_LocalResources/AddModule` |
| 3 | `Admin/Dnn.EditBar/App_LocalResources/Common` |
| 1 | `Admin/Dnn.EditBar/App_LocalResources/EditBar` |
| 2 | `Admin/Dnn.EditBar/App_LocalResources/QuickAddModule` |
| 51 | `Admin/Dnn.EditBar/Resources/ContentEditorManager/App_LocalResources/SharedResources` |
| 15 | `Admin/EditExtension/App_LocalResources/AuthenticationEditor.ascx` |
| 44 | `Admin/EditExtension/App_LocalResources/EditExtension.ascx` |
| **142** | `Admin/HtmlEditorManager/App_LocalResources/ProviderConfiguration.ascx` |
| 20 | `Admin/ModuleCreator/App_LocalResources/CreateModule.ascx` |
| 34 | `Admin/ModuleCreator/App_LocalResources/ViewSource.ascx` |
| 29 | `Admin/SearchResults/App_LocalResources/ResultsSettings.ascx` |
| 35 | `Admin/SearchResults/App_LocalResources/SearchResults.ascx` |
| 10 | `Admin/SearchResults/App_LocalResources/SearchableModules` |
| 7 | `Admin/Security/App_LocalResources/DataConsent.ascx` |
| 3 | `AuthenticationServices/Facebook/App_LocalResources/Login.ascx` |
| 1 | `AuthenticationServices/Facebook/App_LocalResources/Settings.ascx` |
| 3 | `AuthenticationServices/Google/App_LocalResources/Login.ascx` |
| 1 | `AuthenticationServices/Google/App_LocalResources/Settings.ascx` |
| 3 | `AuthenticationServices/Live/App_LocalResources/Login.ascx` |
| 1 | `AuthenticationServices/Live/App_LocalResources/Settings.ascx` |
| 3 | `AuthenticationServices/Twitter/App_LocalResources/Login.ascx` |
| 1 | `AuthenticationServices/Twitter/App_LocalResources/Settings.ascx` |
| 14 | `Connectors/Azure/App_LocalResources/SharedResources` |
| 9 | `Connectors/GoogleAnalytics/App_LocalResources/SharedResources` |
| 5 | `Connectors/GoogleTagManager/App_LocalResources/SharedResources` |
| 16 | `ResourceManager/App_LocalResources/EditFolderMapping.ascx` |
| **106** | `ResourceManager/App_LocalResources/ResourceManager` |
| 5 | `ResourceManager/App_LocalResources/Settings.ascx` |
| 3 | `ResourceManager/App_LocalResources/View.ascx` |
| 10 | `SiteExportImport/App_LocalResources/ExportImport` |
| 45 | `TelerikRemoval/App_LocalResources/View.ascx` |
| 14 | `ToSic.Sxc/App_LocalResources/View.ascx` |
| **689** | **37 fichiers** |

Deux fichiers portent à eux seuls **248 cellules** (36 %) : `ProviderConfiguration.ascx`
(142) et `ResourceManager.resx` (106). Ce sont eux qui décideront du découpage du travail.

## 7. Ce que cette note autorise

La garde fa-IR pourra épingler, en nombres **exacts** et non en planchers :

- **37** fichiers, **689** cellules, **642** noms distincts ;
- **0** clé hors neutre, **0** clé manquante, **0** écart de variables ;
- **10** verbatim déclarés nommément (parité avec le lot ar-SA) ;
- écriture persane : lettres propres (پ چ ژ گ ک ی) présentes au niveau du **lot**, et
  **0** lettre propre à l'arabe (ي ك ى) — le critère est un fait de lot, jamais une règle
  par valeur (voir `DnnPersianScriptMarkers`).

**Non fait ici, et volontairement :** aucune traduction. Ce lot porte 689 cellules ; les
livrer à moitié serait pire que ne rien livrer — la garde exige le lot entier.

---

*Mesure rejouable : la dérivation et le comptage sont en §2 et §3, sur blobs `HEAD`.*
*Lane po-2023.*
