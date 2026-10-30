# #458 grain module-level, lot 1 — `controls` zh-CN (11 fichiers, 133 clés)

**Date :** 2026-10-30 · **Lane :** po-2023 · **Culture :** `zh-CN` seule (ar-SA et fa-IR non touchés)
**Périmètre :** `DNNPlatform/controls/App_LocalResources/` — les **11 familles CORE** du dossier
**Dénominateur :** mesuré par #2072 (CORE sans `Install/` : 94 fichiers / 2 168 clés par langue)
**Origine :** ordre d'ai-01 du 30/10 (rang ③, *« #2072 modules ar/fa/zh, en parallèle de la montée »*)

---

## 1. Ce qui est livré

**11 `.resx` créés**, 133 clés au total — le dossier `controls` est le plus petit des quatre et
le premier lot du grain module-level :

| Fichier | Clés | | Fichier | Clés |
|---|---:|---|---|---:|
| `Address.ascx` | 34 | | `SkinControl.ascx` | 7 |
| `URLControl.ascx` | 27 | | `DualListControl.ascx` | 6 |
| `User.ascx` | 24 | | `filepickeruploader.ascx` | 4 |
| `TextEditor.ascx` | 13 | | `Help.ascx` | 4 |
| `UrlTrackingControl.ascx` | 9 | | `ModuleAuditControl.ascx` | 3 |
| | | | `LocaleSelectorControl.ascx` | 2 |

Chaque fichier est **nouveau** : aucune des 11 familles n'avait de `zh-CN`, ni `ar-SA`, ni `fa-IR`
(mesuré : `manque=ar-SA,fa-IR,zh-CN` sur les 11).

## 2. Le constructeur n'a PAS été doublé — il a été étendu d'un drapeau

⚠️ Le réflexe aurait été d'écrire un second script « module-level ». Il aurait recopié les contrôles
bloquants du constructeur AGR (clé hors neutre, valeur vide, neutre vide hors coquille, multiset des
placeholders, identité des jetons, reparse XML, égalité du jeu de clés, préservation des clés
existantes du fichier culturel) — soit **neuf règles recopiées**, donc neuf occasions de dériver.

Les 11 fichiers vivant dans **un seul** dossier, le constructeur existant
(`458-item9bis-langpack-build.py`) fonctionne tel quel avec `--root <ce dossier>` : ses entrées sont
`{base: {clé: valeur}}`, sans rien qui suppose `App_GlobalResources`. **La seule chose qui ne
marchait pas était le BOM**, écrit en `utf-8-sig` en dur.

D'où un ajout minimal — `--bom {sig,plain,mirror}`, **défaut `sig`, donc comportement AGR inchangé** :

| Dossier | Convention observée | Drapeau |
|---|---|---|
| `App_GlobalResources` | BOM partout (neutre *et* cultures) | `sig` (défaut) |
| `controls/App_LocalResources` | **hétérogène** : `Address.ascx.resx` sans BOM, `URLControl.ascx.resx` avec ; `Address.pt-BR.resx` avec BOM quand `Address.ru-RU.resx` n'en a pas | `mirror` |

`mirror` fait suivre au fichier produit **l'état de BOM de son propre neutre**. C'est la seule règle
qui tienne sur un dossier hétérogène : une constante par dossier serait fausse dès le deuxième
fichier, et une constante globale le serait pour les deux tiers.

## 3. Contrôles — instruments indépendants du constructeur

Le constructeur valide ce qu'il écrit ; ces contrôles le **rejouent autrement** (parse XML +
lecture d'octets) :

| Contrôle | Résultat |
|---|---|
| Fichiers produits / clés | **11 / 133** |

> ℹ️ Le contrôle BOM/fins de ligne de la **garde C#** mesure le **blob committé** (`git cat-file`), pas
> l'arbre de travail — voir §4. Le vérificateur Python ci-dessus lit l'arbre : sur cette machine de
> travail (LF au checkout), les deux instruments rendent le même verdict.
| Jeu de clés == neutre (par fichier) | **11/11** — 0 manquante, 0 inventée |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** (5 clés en portent : `ModuleAuditControl` ×2, `TextEditor` ×2, `URLControl` ×1) |
| Identité des jetons DNN | **0 écart** — `lblInfo.Text` porte 8 jetons `[ORGANIZATION]`…`[SRC]` et 9 balises HTML, tous préservés |
| BOM suit le neutre, par fichier | **11/11** |
| Fins de ligne | **LF seul**, 0 CR |
| Copies de l'anglais non déclarées | **0** (2 déclarées, voir ci-dessous) |

### Deux valeurs identiques à l'anglais, déclarées

`TextEditor.ascx:Html.Text` = « HTML » (nom du format) et `UrlTrackingControl.ascx:URL.Text` = « URL »
(en-tête de colonne, employé tel quel en chinois). La garde les inscrit nominativement et **épingle
leur cardinal (`= 2`)** : une liste blanche qui grandit en silence transformerait « non traduit » en
« traduit ».

## 4. La garde — et pourquoi elle n'est pas adossée à `ru-RU`

`DnnModuleLevelControlsZhResxGuardTests` (46 tests) est écrite pour être **étendue** : un lot suivant
ajoute ses fichiers à `Covered`, les règles ne bougent pas.

⚠️ La référence est le **neutre**, jamais `ru-RU` : la mesure #2072 a montré que **15 des 96 fichiers
CORE n'ont pas le même jeu de clés que ru** (jusqu'à `Copyright.ascx`, où ru n'a **aucune** clé). Une
garde adossée au russe accepterait des clés manquantes — c'est exactement le défaut que #2072 a
consigné avant que le pack n'existe.

Le BOM est asserté **par dérivation** (produit == neutre), pas par une liste de 11 booléens : une
liste recopiée serait fausse au premier fichier qui change.

### Transport : mesuré sur le blob committé, pas sur l'arbre de travail

⚠️ La première version de `BomMirrorsTheNeutralAndLineEndingsAreLf` lisait
`File.ReadAllBytes` sur l'arbre de travail — **rouge 7 fois en CI** (`build (Debug)` + `build
(Release)`) alors que la suite locale était 46/46 : le runner CI a `core.autocrlf=true`, qui
convertit LF → CRLF au checkout, donc un fichier sain y apparaît en CRLF. L'instrument mesurait
la **configuration de checkout**, pas le dépôt. La règle était déjà consignée par la lane
(#2078 : « BOM/EOL vérifiés sur blob git, jamais sur arbre de travail ») — la garde la réapplique :
l'assertion lit `git cat-file -p HEAD:<chemin>`, pour le neutre **et** le produit.

**Contrôles inverses, deux moitiés** (le vert seul ne prouve rien) :

| Mutation | Attendu | Mesuré |
|---|---|---|
| 140 CR injectés dans l'**arbre de travail** (`SkinControl.ascx.zh-CN.resx`), blob intact | vert — la garde ne lit plus l'arbre | ✅ 46/46 |
| Les mêmes 140 CR **committés** (commit temporaire local, jamais poussé, retiré ensuite) | rouge sur ce seul fichier | ✅ 1 échec / 45 verts, message nommant le fichier |

Restauration par copie de sauvegarde, `sha256` revérifié identique, `git status` propre.

### Contrôles inverses — trois mutations, trois règles distinctes

| Mutation | Attendu | Mesuré |
|---|---|---|
| `{0}` → `{9}` dans `ModuleAuditControl:CreatedBy.Text` | multiset des placeholders rouge | ✅ 1 échec, message nommant la clé |
| `网站：` → `Website:` dans `User:plWebsite.Text` | copie de l'anglais rouge | ✅ 1 échec, message nommant la clé |
| BOM **ajouté** à `Address.ascx.zh-CN.resx` (neutre sans BOM) | BOM rouge | ✅ 1 échec / 45 verts |

⚠️ **La troisième a d'abord été mal construite** : j'ai commencé par **retirer** le BOM d'`Address`,
qui n'en a pas au neutre — la mutation *respectait* donc la règle et le test est resté vert. Un
contrôle inverse se conçoit **contre l'attente réelle**, sinon il ne teste que sa propre erreur.
Le vert obtenu était le bon comportement de la garde, pas un trou.

Restauration par **copie de sauvegarde** (jamais `git checkout --`), `sha256` revérifiés, contrôle
indépendant rejoué : **OK**.

## 5. Tests

| Périmètre | Résultat |
|---|---|
| `DnnModuleLevelControlsZhResxGuardTests` | **46 / 46** (2 faits + 11 × 4 théories) |
| Suite complète (re-mesuré après le fix transport) | **2450** — **2444 réussis, 0 échec**, 6 ignorés |

## 6. Ce que cette PR ne fait pas

- **`ar-SA` et `fa-IR` non touchés** : une culture par PR, jamais empilées.
- **Aucun autre dossier** : ni `admin` (27 fichiers / 338 clés), ni `DesktopModules` (56 / 1 704), ni
  `Install/` (2 / 187 — recommandé à l'exclusion par #2072, décision ai-01).
- **Rien côté préprod** : un `.resx` est un fichier du dépôt ; sa prise en compte par IIS demande un
  recyclage du pool, geste **distinct**. Le webroot servi n'est pas touché.
- **Pas de relecture native** : la traduction a été produite par la lane. Une relecture éditoriale
  zh reste ouverte, comme pour les packs AGR.

## 7. Suite

`controls` **ar-SA** puis **fa-IR** (mêmes 133 clés × 2), puis `admin` (27 / 338) — un lot par
couple (dossier, culture). Le constructeur et la garde sont prêts pour ces lots sans modification.
