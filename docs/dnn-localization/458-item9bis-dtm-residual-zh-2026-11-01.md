# #458 module-level — lot 3c : résidu zh-CN de `DesktopModules` (37 resx, 689 clés)

**Date :** 2026-11-01 · **Lane :** po-2023 · **PR :** `feat/458-modulelevel-dtm-residual-zh`
**Base :** master `d60b9471` · **Dépense API :** 0 $ (traduction authorée lane)

---

## 1. Périmètre — mesuré, pas recopié

L'étiquette du pool annonçait **9 fichiers / 381 clés**. Re-mesure sur l'arbre au
moment de la livraison : **37 fichiers / 689 clés**. L'écart est consigné ici et
dans la garde, jamais exécuté à l'aveugle — c'est la règle « étiquette de pool ≠
périmètre » (décision 18 du dashboard, payée sur « 67 » de Registration et « 38 »
d'IP filters).

Le résidu est défini mécaniquement : **tout neutre `*.resx` de
`DNNPlatform/DesktopModules/**/App_LocalResources/` sans jumeau `*.zh-CN.resx`**,
hors `PersonaBar`, `NBright` et `Install/` (exclus par #2072). Après les lots
`Admin` (20 fichiers / 640 clés) et « modules » (#2089, 27 fichiers / 668 clés),
il restait ces 37.

| Répertoire | Fichiers | Clés |
|---|---:|---:|
| `Admin/` (Authentication, Console, Dnn.EditBar, EditExtension, HtmlEditorManager, ModuleCreator, SearchResults, Security) | 19 | 446 |
| `AuthenticationServices/` (Facebook, Google, Live, Twitter) | 8 | 16 |
| `Connectors/` (Azure, GoogleAnalytics, GoogleTagManager) | 3 | 28 |
| `ResourceManager/` | 4 | 130 |
| `SiteExportImport/` | 1 | 10 |
| `TelerikRemoval/` | 1 | 45 |
| `ToSic.Sxc/` | 1 | 14 |
| **Total** | **37** | **689** |

**Sept de ces fichiers n'avaient aucune sœur de culture** — ni `fr-FR`, ni
`ru-RU`, ni `pt-BR`, ni `es-ES` : `ResourceManager` (4 : `ResourceManager`,
`EditFolderMapping`, `Settings`, `View`), `TelerikRemoval/View.ascx`,
`Connectors/GoogleTagManager/SharedResources`, `Admin/Dnn.EditBar/QuickAddModule`.
C'est du **jamais-localisé**, pas un trou zh — la distinction compte : un trou zh
se comble en copiant une sœur, un jamais-localisé s'écrit. Les cinq autres
fichiers `Admin/Dnn.EditBar` n'ont que `fr-FR` + `ru-RU` : même situation, une
sœur existante en moins.

Un fichier neutre est **hors livraison** et déclaré :
`Admin/HtmlEditorManager/Controls/App_LocalResources/InvalidConfiguration.ascx.resx`
porte **0 clé** (`<data>` absent). Un `.zh-CN.resx` vide n'aurait aucun sens ; la
garde vérifie que ce neutre est **toujours vide**, sans quoi la dispense
deviendrait une traduction perdue.

## 2. Livraison — cinq règles de transport

1. **Le fichier produit est son neutre octet pour octet**, sauf le contenu interne
   de chaque `<value>`. Le schéma ResX, les `<comment>` et l'ordre des clés sont
   ceux du neutre : le diff ne montre que de la prose.
2. **XML brut, jamais la valeur parsée.** Substitution par
   `(<data name="K"[^>]*>\s*<value[^>]*>)(.*?)(</value>)` DOTALL sur le fichier
   brut, valeur échappée (`&`/`<`/`>`), puis validation `ET.fromstring` **avant**
   écriture. Le piège payé sur #2101 (casser un `<br />` en substituant la valeur
   parsée) ne peut pas se reproduire.
3. **BOM dérivé du neutre** — le module-level est hétérogène : dans ce lot,
   **30 fichiers portent le BOM, 7 n'en portent pas**, et ces 7 sont exactement
   ceux dont le neutre n'en a pas (`Admin/Authentication/Authentication.ascx`,
   `Admin/Authentication/Logoff.ascx`,
   `Admin/Dnn.EditBar/Resources/ContentEditorManager/SharedResources`,
   `Admin/EditExtension/AuthenticationEditor.ascx`,
   `Admin/Security/DataConsent.ascx`, `ResourceManager/ResourceManager`,
   `TelerikRemoval/View.ascx`). Ce n'est pas une liste à recopier : **la règle est
   la dérivation**, l'attente de la garde se calcule par fichier depuis son neutre
   et passe 37/37. ⚠️ Une version antérieure de cette ligne annonçait « les 36
   autres en ont un » — c'était faux (30/7), trouvé par deux instruments
   indépendants, et corrigé ici : une phrase fausse dans une note de référence
   fait « réparer » un BOM correct au passage suivant.
4. **LF toujours**, y compris quand le neutre est commité en **CRLF**
   (`ToSic.Sxc/App_LocalResources/View.ascx.resx` : 102 CR dans le blob). Les fins
   de ligne du dépôt sont LF ; un CR est du bruit de diff, et le constructeur
   normalise.
5. **Espaces signifiants préservés** : `ResourceManager` porte deux valeurs
   concaténées côté client (`FileSizeErrorMessage.Text` = « 过大。允许的最大文件大小为 »
   et `InvalidExtensionMessage.Text` = « 的类型不允许上载 ») dont les espaces de tête et
   de queue sont structurels.

**Traduction** : 689 valeurs, authorées lane, terminologie alignée sur les lots
précédents (设置 / 保存 / 取消 / 上载 / 标记 / 门户 …). Les exemples de syntaxe de
recherche (`kettle~`, `cat*`, `"dnn is awesome"`) sont **laissés tels quels** :
ce sont des jetons de démonstration, les traduire casserait l'exemple.

**Onze valeurs verbatim, épinglées** :

| Nature | Valeurs |
|---|---|
| Jetons de remplacement DNN | `[PortalRoot]`, `[UserFolder]` |
| Marqueur de tri alphabétique | `A-Z` |
| Noms de marques | `Facebook`, `Google`, `Live`, `Twitter` (×`Login.ascx:Title.Text`) |
| Noms de langages | `C#`, `Visual Basic` (×`CreateModule`, ×`ViewSource`) |

Un jeton traduit cesse d'être un jeton ; une marque n'a pas de traduction. La
liste est **épinglée à 11** : ajouter une dispense pour faire passer un rouge
rougit le fait de comptage.

## 3. Vérificateur indépendant — 0 écart

Instrument **distinct du constructeur** (aucune fonction partagée), périmètre
dérivé de **git** (`git status --porcelain`, fichiers `??` en `*.zh-CN.resx`) et
non de la liste du constructeur : 37 fichiers, 689 clés.

Contrôles : jeu de clés == neutre · multiset des placeholders `{n}` ·
**multiset des balises HTML/XML** (les `.Help` portent des `<a>`, `<b>`, `<ul>`,
`<table>` — une balise perdue rendrait une aide mutilée) · BOM == neutre ·
0 CR · pas de copie de l'anglais hors dispense · écriture chinoise hors dispense.

**Résultat : 0 écart.** Deux valeurs à chevrons **littéraux**
(`<None Specified>`, `<Choose Content Type>`) sont neutralisées de la comparaison
de balises : ce ne sont pas des balises, et les compter en produirait un faux
positif — c'est l'instrument qui doit le savoir, pas la règle qui doit se taire.

## 4. Contrôles inverses — 5 mesures

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| ① | placeholder retiré (`ResultsCount.Text` → « 约 个结果 ») | rouge nommant la clé | **1 rouge**, `multiset des placeholders modifié` — 187 verts |
| ② | copie de l'anglais (`View.ascx:EditModule.Text` = « Edit Module ») | rouge nommant la clé | **1 rouge**, deux assertions : « aucune écriture chinoise » **et** « copie de l'anglais par accident » |
| ③ | latin **non** anglais (`ViewConsole:ControlTitle_.Text` = « Fonctions de base ») | rouge écriture seule | **1 rouge**, `la valeur ne porte aucun caractère chinois` — l'assertion « ≠ anglais » ne tire pas : c'est la règle CJK seule qui voit ce cas |
| ④ | BOM ajouté à `ResourceManager.zh-CN.resx`, **arbre de travail seul** | **vert** (la garde lit le blob `HEAD`) | **188/188 verts** |
| ⑤ | le même BOM **commité** (commit temporaire) | rouge nommant le fichier | **1 rouge**, `le BOM doit suivre celui du neutre (neutre=False), but found True` |

Chaque mutation est faite sur le **XML brut interne**, backup + empreinte, et
restaurée par `cp` (+ sha256 identique vérifié) — **jamais** `git checkout --`
(règle checkout safety). ⑤ défait par `reset --soft HEAD~1` + `restore --staged`
+ `cp` : arbre == HEAD, HEAD revenu sur `6ec61c2f`.

## 5. Garde

`DnnModuleLevelDtmResidualZhResxGuardTests` — **188 tests** :

| Fait / théorie | Portée |
|---|---|
| `TheLotCoversThirtySevenFilesAndSixHundredEightyNineKeys` | 37 couples uniques, somme = 689 dérivée de `Covered`, pas un littéral |
| `PackCoversTheDeclaredKeyCount` (×37) | compte par fichier |
| `TheDeliveredKeySetIsExactlyTheNeutralOne` (×37) | aucun manquant déclaré : livré == neutre |
| `EveryDeliveredValueIsSaneAgainstTheNeutral` (×37) | non vide, placeholders, jetons DNN, ≠ anglais hors dispense |
| `EveryNonExemptValueCarriesChineseScript` (×37) | écriture chinoise, dispenses épinglées |
| `BomMirrorsTheNeutralAndLineEndingsAreLf` (×37) | **blob committé**, BOM dérivé du neutre, 0 CR |
| `TheVerbatimListCannotGrowSilently` | 11 verbatim, 11 dispenses d'écriture |
| `TheDeclaredEmptyNeutralIsStillEmpty` | le neutre à 0 clé l'est toujours |

La garde est **autonome** (miroir du patron des lots précédents), pas un socle :
la factorisation des gardes `DesktopModules` est un grain distinct, désigné
après la fermeture de la séquence zh (décision 34), et porte les deux 🟡 NanoClaw
de #2105.

## 6. Suites

| Mesure | Résultat |
|---|---|
| Garde seule | **188 / 188** |
| Suite complète locale | **3 622 = 3 617 / 0 / 5** mesuré (2 min 28 s) |

Le **total** tombe exactement sur la base annoncée : `d60b9471` **3 434 = 3 428 / 0 / 6**
(rapporté par ai-01) + 188 = 3 622. En revanche la **répartition** mesurée ici
(3 617 / 0 / 5) diffère d'un test : ai-01 compte **6** ignorés, cette machine **5**.

Écart **instruit, et il ne vient pas du lot** — deux constats :

1. La suite porte des `Skip` **conditionnels à la machine**, pas au code :
   `ScenariiVoixEN71ContractTests` s'ignore quand `python` n'est pas exécutable,
   `DnnBinUntrackGuardTests` quand l'index git n'est pas évaluable. Ici
   `python --version` rend **Python 3.13.3**, donc ce test **s'exécute** (et passe) ;
   sur une machine sans interpréteur il s'ignore. Cela déplace exactement un test
   d'« ignoré » vers « réussi », sans qu'une ligne du dépôt change.
2. Les 188 tests du lot sont tous des faits et théories **sans condition** : le lot
   ne peut pas fabriquer ni retirer un ignoré.

⚠️ La conséquence pratique vaut au-delà de ce lot : **un compte d'ignorés se compare
entre mesures de la même machine**, jamais entre deux hôtes. Un `total` identique
avec une répartition différente n'est pas une anomalie, c'est la signature d'un
`Skip` conditionnel — et le lire comme une régression enverrait chercher un défaut
qui n'existe pas.

## 7. Non-objectifs

- Aucune installation préprod (fenêtre = jour de semaine, item 6/7 du pool).
- Aucune factorisation de garde (grain « socle », item 5).
- Aucun `PersonaBar` / `NBright` (exclus #2072).
- Aucune re-traduction des lots antérieurs : leurs gardes restent seules juges.
