# #458 — Grain « pt-PT → pt-BR » : la liste nominative d'abord

**Date :** 2026-11-02 · **Lane :** po-2024 · **Master au moment de la mesure :** `a3ab8720`
**Périmètre mesuré :** `DNNPlatform/**/*.pt-BR.resx` — **140 fichiers**

---

## 1. La question du pool, et pourquoi elle a dû être re-dérivée

Le pool #458 porte item 1 sous la forme : *« pt-PT → pt-BR : la liste nominative d'abord
(tranche 25 contre 24) »*. Cette « tranche 25 contre 24 » n'a **aucun référent** : ni dans
le pool publié, ni dans les commentaires de #458, ni sur le dashboard. Re-mesuré : on ne
l'exécute pas, on la re-dérive — c'est la règle « une ligne de pool périmée se fait
exécuter ».

Ce que la re-mesure donne : **le lexique continental rend 0**, mais **l'orthographe pré-AO
rend 13 cellules**. La liste nominative existe donc, elle n'est simplement pas celle qu'un
compteur 25/24 laissait attendre.

---

## 2. Méthode — deux familles disjointes, et deux pièges d'instrument payés

Deux familles sont bannies du brésilien et **ne se recouvrent pas** :

| Famille | Ce que c'est | Exemples |
|---|---|---|
| **Orthographe pré-AO** | le `c` latin muet que le Brésil a perdu | `acção` → `ação`, `objecto` → `objeto`, `óptimo` → `ótimo`, `selecção` → `seleção`, `colecção` → `coleção`, `facto` → `fato` |
| **Lexique continental** | vocabulaire pt-PT qui n'existe pas au Brésil | `ficheiro`/`arquivo`, `ecrã`/`tela`, `equipa`/`equipe`, `utilizador`/`usuário`, `palavra-passe`/`senha`, `aceder`/`acessar`, `registo`/`registro` |

### Piège 1 — une classe non accentuée invente 136 défauts

Un premier passage avec `s[íi]tio` a rendu **136 « occurrences »** : la classe attrapait le
« sitio » **espagnol**, non accentué. Idem pour `actual`. Ces 136 cellules sont de la
**contamination espagnole** (volets `sites` / `sitesettings` / `servers` à venir), pas du
pt-PT. Correction : formes à **accent exact** (`\bsítios?\b`), uniques au dialecte.

### Piège 2 — un marqueur borgne au pluriel rend 0 par cécité

`\bregisto\b`, `\bsec[çc][ãa]o\b`, `\brece[çc][ãa]o\b`, `\bac[çc][ãa]o\b` **ne voient pas
les pluriels** (`registos`, `secções`, `receções`, `acções`). Avec les motifs corrigés —
`\bregistos?\b`, `\bsec[çc](?:ã|õ)es?\b`, `\brece[çc](?:ã|õ)es?\b`, `\bac[çc](?:ã|õ)es?\b` —
**6 des 13 cellules apparaissent**. Un balayage vert sur les motifs borgnes aurait livré
« 7 cellules » en laissant 6 défauts derrière.

> Les témoins du contrôle inverse sont donc écrits **au pluriel** partout où le pluriel est
> la forme risquée : un témoin au singulier ne prouve rien sur la couverture du pluriel.

### Hors périmètre, délibérément

Les formes **ambiguës** — `activo/activar`, `actual`, `directo`, `efectivo`, `correcto`,
`exacto`, `colectivo`, `detecção` — sont exclues de la garde :

- `activo`/`actual`/`directo`… sont **aussi espagnoles** : elles vivent dans des cellules déjà
  contaminées (`licensing`, `pages`, `AdminLogs`, `extensions`) qui appartiennent aux volets
  de contamination espagnole. Y mordre ici produirait un rouge attribué au mauvais volet.
- `detecção` est la forme **correcte au Brésil** (le `c` s'y prononce) : l'attraper serait un
  faux positif, pas une garde.

Mesure de cette famille ambiguë, pour mémoire : **117 cellules**, dont la quasi-totalité dans
des fichiers déjà espagnols. Elles se traiteront avec leur volet.

---

## 3. Résultat — la liste nominative (13 cellules, 2 formes)

| # | Fichier | Clé | Avant | Après |
|---|---|---|---|---|
| 1 | `App_GlobalResources\SharedResources` | `PermissionActionsHeader.Text` | `Acções` | `Ações` |
| 2 | idem | `SkinObject.Text` | `O objecto incrustado é um objeto de skin` | `O objeto incrustado…` |
| 3 | idem | `SkinObjects.Text` | `[Objectos skin]` | `[Objetos skin]` |
| 4 | idem | `SkinObjectsDescription.Text` | `Os objectos de tema son…` | `Os objetos de tema son…` |
| 5 | idem | `SkinToken.Text` | `O token é um objecto de Tema` | `…um objeto de Tema` |
| 6 | `Dnn.Extensions\extensions` | `AddModuleControl_HelpURL.HelpText` | `…menú de acções do módulo…` | `…menú de ações do módulo…` |
| 7 | idem | `EditJavascriptLibrary_ObjectName.HelpText` | `Nome do objecto JavaScript…` | `Nome do objeto JavaScript…` |
| 8 | idem | `EditJavascriptLibrary_ObjectName.Label` | `Nome de objecto` | `Nome de objeto` |
| 9 | `Dnn.Security\security` | `Actions.Header` | `Acções` | `Ações` |
| 10 | `CoreMessaging\SharedResources` | `Actions.Text` | `Acções` | `Ações` |
| 11 | `CoreMessaging\View.ascx` | `Actions.Text` | `Acções` | `Ações` |
| 12 | `HTML\Settings.ascx` | `plReplaceTokens.Help` | `…para os objectos Tab…` | `…para os objetos Tab…` |
| 13 | `Install\Installwizard.aspx` | `IsAbleToPerformDatabaseActionsCheck.Text` | `…realizar as acções necessárias` | `…as ações necessárias` |

Les deux familles sont donc : **`acções` ×6** et **`objecto(s)` ×7**.

---

## 4. Provenance — héritage de l'espagnol, pas d'un paquet pt-PT

Mesure des mêmes formes dans les autres locales :

| Locale | Fichiers | Formes pré-AO |
|---|---:|---:|
| **neutre** (`*.resx`) | 8 fichiers porteurs | **0** |
| `es-ES` | 192 | **7** |
| `fr-FR` | 190 | 0 |
| `pt-PT` | **0 fichier** (aucun paquet pt-PT dans l'arbre) | — |

⭐ Les 7 cellules `objecto` de l'**es-ES** sont **les mêmes cellules, avec les mêmes
phrases** : `El objecto incrustado es un objeto de skin`, `Los objectos de tema son…`,
`Nombre del objecto JavaScript…`. Le pt-BR ne descend donc pas d'une traduction pt-PT : il
descend de l'**espagnol** — ce que la contamination lexicale relevée sur ces mêmes cellules
(`ayuda`, `acceder`, `libreria`, `Reemplazo`, `serán`) confirme indépendamment.

**Conséquence pour la suite :** `objecto` est **faux en espagnol aussi** (`objeto`). Ces
7 cellules es-ES sont un défaut réel, **hors périmètre de ce grain** — elles sont nommées
ci-dessus pour être traitées par un volet es, jamais corrigées en passant.

**Ce que ce grain ne corrige pas, et qui reste dû :** les 4 cellules #4, #6, #7, #12 portent
encore de l'espagnol (`son`, `proveer`, `sus`, `ayuda`, `acceder`, `libreria`, `Reemplazo`,
`reemplazar`, `serán`, `si se usa`, `adecuados`). Ce grain ne touche **que l'orthographe** —
mélanger les deux aurait rendu la garde rouge pour une raison qui n'est pas la sienne.

---

## 5. Mécanique de la chirurgie

- **Substitution dans le span de valeur BRUT**, pas dans une valeur décodée : on ne retire
  qu'un seul octet ASCII (`c`) par substitution. Tout le reste — entités `&lt;strong&gt;`,
  le littéral `[object:property|format]`, les sauts de ligne internes de la cellule #12,
  l'attribut `xml:space="preserve"` là où il existe — est préservé **à l'octet près**.
- **Delta contrôlé** : `-1` octet par substitution, par fichier et au total. Mesuré :
  `-5, -3, -1, -1, -1, -1, -1` = **-13 octets**, égal au nombre de substitutions. L'assert
  tire **avant** l'écriture.
- **BOM + LF** revérifiés après écriture ; XML revalidé bien formé (`ElementTree`).
- ⚠️ **Piège de coordonnées, payé une fois** : les offsets du dump de mesure étaient en
  **octets** (décalage BOM inclus), le script de chirurgie les a lus sur une **chaîne**
  (indices en caractères). L'assert de précondition a tiré (`span divergent`) **avant toute
  écriture** — fichier intact. Correction : re-dériver les spans dans le référentiel du
  script. Deux référentiels de coordonnées pour la même donnée = une cellule décalée.

---

## 6. La garde — `DnnPtBrContinentalResxGuardTests`

Deux tests, **28 marqueurs** (6 pré-AO + 22 continentaux), balayant les 140 fichiers pt-BR :

| Test | Ce qu'il tient |
|---|---|
| `No_Continental_Or_PreAo_Form_In_Any_PtBr_Resx` | 0 occurrence des deux familles ; plancher anti-effondrement **100 fichiers** (glob cassée = balayage à vide qui se lit vert) |
| `Each_Marker_Fires_On_Its_Witness` | contrôle inverse : chaque marqueur reconnaît sa phrase témoin, **au pluriel** quand le pluriel est la forme risquée |

L'assertion porte sur la **liste jointe** et non sur la collection : FluentAssertions
n'affiche que le premier élément d'une collection fautive, et une liste nominative tronquée
à 1 se lit « une seule cellule » — on perdrait les 12 autres à chaque régression.

### Banc — le rouge est mesuré, pas asserté

La garde a été écrite et **lancée sur l'arbre non corrigé** : `13 offenseurs`, la liste
nominative complète ci-dessus, sortie telle quelle par le test. Après chirurgie : `2/2`
verts. Le rouge-avant n'est donc pas une affirmation de la note, c'est la sortie de
l'instrument sur la même arborescence.

---

## 7. Validation

Suite complète `Argumentum.AssetConverter.Tests` : **4 338 total (4 288 pass / 44 fail / 6 skip)**
= master `a3ab8720` 4 336 + 2 (cette garde). Arithmétique exacte.

**Les 44 échecs sont un rouge de LOT, requalifié** : tous de la famille
`BomMirrorsTheNeutralAndLineEndingsAreLf` → `CommittedBlob` (« git cat-file -p HEAD:… n'a
pas rendu la main en 30 s ») — 4ᵉ témoin de la famille de flake déjà consignée (2 locaux
par contention LFS d'une workspace sœur, 1 en CI sur #2125), ici le plus gros burst
(44 instances). Rejeu **du même filtre sur le même arbre** : `329/329 verts en 33 s`,
aucun `index.lock`. Le ticket de fragilité (pool #458 item 4, « correctif seulement sur
reproduction ») garde ce témoin à son dossier.

**Limites connues.** La garde ne mesure que les fichiers `pt-BR.resx` de `DNNPlatform/` —
même borne que le socle des volets précédents. Elle ne dit rien du rendu runtime (aucun
gabarit ne référence ces deux formes : ce sont des libellés de ressources), ni des locales
`es-ES`/`fr-FR` (hors périmètre, cf. §4).
