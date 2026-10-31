# #458 grain module-level, lot 3 — `controls` ar-SA (11 fichiers, 133 clés)

**Date :** 2026-10-31 · **Lane :** po-2023 · **Culture :** `ar-SA` seule (fa-IR non touchée)
**Périmètre :** `DNNPlatform/controls/App_LocalResources/` — les **11 familles CORE** du dossier
**Dénominateur :** mesuré par #2072 (CORE sans `Install/` : 94 fichiers / 2 168 clés par langue)
**Origine :** suite du lot 1 (`controls` zh-CN, #2087), comme annoncé en §7 de son document

---

## 1. Ce qui est livré

**11 `.resx` créés**, 133 clés — mêmes fichiers, mêmes clés que le lot zh-CN, autre culture :

| Fichier | Clés | | Fichier | Clés |
|---|---:|---|---|---:|
| `Address.ascx` | 34 | | `SkinControl.ascx` | 7 |
| `URLControl.ascx` | 27 | | `DualListControl.ascx` | 6 |
| `User.ascx` | 24 | | `filepickeruploader.ascx` | 4 |
| `TextEditor.ascx` | 13 | | `Help.ascx` | 4 |
| `UrlTrackingControl.ascx` | 9 | | `ModuleAuditControl.ascx` | 3 |
| | | | `LocaleSelectorControl.ascx` | 2 |

Aucune des 11 familles n'avait de `ar-SA` avant ce lot (mesuré sur `origin/master` = `991e7816`).

**Contrat identique au lot zh-CN**, contrôlé indépendamment du constructeur (parse XML +
lecture d'octets) :

| Contrôle | Résultat |
|---|---|
| Fichiers produits / clés | **11 / 133** |
| Jeu de clés == neutre (par fichier) | **11/11** — 0 manquante, 0 inventée |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** (5 clés en portent : `ModuleAuditControl` ×2, `TextEditor` ×2, `URLControl` ×1) |
| Jetons `[ORGANIZATION]`…`[SRC]` | **0 écart** — les 8 de `Help.ascx:lblInfo.Text` préservés avec ses 9 balises HTML |
| Balises HTML (`<h1>`, `<b>`, `<br />`, `<br>`, `<br/>`) | **0 écart**, par clé |
| BOM suit le neutre, par fichier | **11/11** (`Address.ascx` sans BOM, les 10 autres avec — même asymétrie qu'au lot zh) |
| Fins de ligne | **LF seul**, 0 CR |
| Copies de l'anglais non déclarées | **0** (2 déclarées, voir ci-dessous) |

### Deux valeurs latines par nature, déclarées

`TextEditor.ascx:Html.Text` = « HTML » (nom du format) et `UrlTrackingControl.ascx:URL.Text` = « URL »
(en-tête de colonne). Ce sont **exactement les deux mêmes** qu'au lot zh-CN — et le pack arabe les
emploie aussi tels quels, comme le font les interfaces arabes courantes. La garde les inscrit
nominativement et **épingle leur cardinal (`= 2`)**.

## 2. Le constructeur n'a pas été touché

`458-item9bis-langpack-build.py` a produit ce lot **sans modification**, avec
`--lang ar-SA --root DNNPlatform/controls/App_LocalResources --bom mirror` : ses neuf contrôles
bloquants (clé hors neutre, valeur vide, neutre vide hors coquille, multiset des placeholders,
identité des jetons, reparse XML, égalité du jeu de clés, préservation des clés existantes) ont
tous passé. Entrée du lot : `458-item9bis-controls-ar-SA.translations.json` (133 valeurs).

## 3. La garde a été **factorisée**, pas recopiée

⚠️ Le réflexe était d'écrire `DnnModuleLevelControlsArResxGuardTests` en clonant les 46 tests du lot
zh-CN. Cela aurait dupliqué ~200 lignes de règles, et la même copie aurait été refaite pour
**fa-IR**, puis pour les lots `admin` ar/fa — quatre organes à maintenir au lieu d'un.

Le contrat (11 fichiers, 133 clés, mêmes 2 valeurs verbatim) **ne dépend pas de la culture** : seule
la culture change. D'où :

| Fichier | Rôle |
|---|---|
| `DnnModuleLevelControlsResxGuardBase.cs` | socle **abstrait** : les 6 règles + `Culture` abstraite |
| `DnnModuleLevelControlsZhResxGuardTests.cs` | 4 lignes — `Culture => "zh-CN"` |
| `DnnModuleLevelControlsArResxGuardTests.cs` | `Culture => "ar-SA"` + 2 contrôles propres à l'arabe |

xUnit ignore une classe abstraite et rejoue ses `[Fact]`/`[Theory]` dans chaque sous-classe
concrète : les 46 tests du lot zh sont **les mêmes** que ceux du lot ar, exécutés deux fois. Aucun
test n'a été affaibli : les 46 assertions du lot zh sont celles qui tournaient déjà.

### Deux contrôles que le socle ne peut pas porter

Le socle est neutre par construction ; deux vérifications n'ont de sens qu'en arabe :

1. **`EveryNonVerbatimValueCarriesArabicScript`** (théorie sur les 11 fichiers). La règle
   « valeur ≠ anglais » n'attrape qu'une copie **entière** : une valeur moitié traduite
   (`Website:` → `Website: الموقع`) passerait. Ici toute valeur non déclarée verbatim doit porter
   au moins un caractère du bloc Arabic (`؀-ۿ`). C'est la règle qui distingue ce pack d'un
   pack partiellement repris de l'anglais.
2. **`TheArabicPackDeclaresTheSameTwoVerbatimValuesAsTheZhLot`** — les valeurs latines légitimes ne
   peuvent pas diverger d'une culture à l'autre sans décision explicite.

## 4. Contrôles inverses

Le vert seul ne prouve rien : chaque règle a été mise en défaut **exprès**, sur une copie de
sauvegarde, avec `sha256` revérifié après restauration (jamais `git checkout --`).

| Mutation | Attendu | Mesuré |
|---|---|---|
| `{0}` → `{9}` dans `ModuleAuditControl:CreatedBy.Text` | multiset des placeholders rouge | ✅ 1 échec / 103 verts, nommant la clé |
| `الموقع الإلكتروني:` → `Website:` dans `User:plWebsite.Text` | copie de l'anglais rouge | ✅ 2 échecs — voir note ci-dessous |
| `معاينة` → `Aperçu` dans `SkinControl:Preview.Text` | écriture arabe rouge, **seule** | ✅ 1 échec / 103 verts, la seule règle d'écriture |
| BOM **ajouté** à `Address.ascx.ar-SA.resx` (neutre sans BOM), **arbre de travail seul** | vert — la garde ne lit pas l'arbre | ✅ 104/104 |
| Les mêmes octets **committés** (commit temporaire local, jamais poussé, retiré ensuite) | rouge sur ce seul fichier | ✅ 1 échec / 103 verts, message nommant le fichier |

**Note sur la mutation 2** — elle rend **deux** rouges, et c'est correct : une valeur anglaise dans un
pack arabe viole *à la fois* la règle de copie et la règle d'écriture. L'isolement de chaque règle
est apporté par les mutations 1 et 3, qui n'en allument qu'une.

**Les deux moitiés du contrôle de transport** mesurent deux choses différentes : la moitié « arbre »
prouve que la garde **ne lit plus l'arbre de travail** (le runner CI a `core.autocrlf=true`, qui
convertit LF → CRLF au checkout) ; la moitié « committée » prouve qu'elle **lit bien quelque chose**
(un test qui ne lit rien serait vert dans les deux cas). Restauration : `git reset --soft HEAD~1`
puis copie de sauvegarde — aucune commande destructive, `git status` vide, `sha256` identique.

## 5. Tests

| Périmètre | Résultat |
|---|---|
| `DnnModuleLevelControls*` (socle × 2 cultures + contrôles ar) | **104 / 104** |
| Suite complète (après factorisation) | **2 631** — **2 625 réussis, 0 échec**, 6 ignorés (local, `dotnet test`, worktree du lot sur `bc6009fc`) |

Les 46 tests du lot zh-CN sont rejoués **sous les deux cultures** (même code, même assertion) : la
factorisation n'a retiré aucun test, elle a supprimé la copie.

## 6. Ce que cette PR ne fait pas

- **`fa-IR` non touchée** : une culture par PR, jamais empilées.
- **Aucun autre dossier** : ni `admin` (ar/fa restent à faire), ni `DesktopModules`, ni `Install/`.
- **Rien côté préprod** : un `.resx` est un fichier du dépôt ; sa prise en compte par IIS demande un
  recyclage du pool, geste **distinct** (et `[INFO]` dashboard avant, comme toujours).
- **Pas de relecture native** : la traduction a été produite par la lane en arabe standard (MSA).
  Une relecture éditoriale `ar-SA` reste ouverte, comme pour zh et les packs AGR.

## 7. Suite

`controls` **fa-IR** (mêmes 133 clés, mêmes règles — une sous-classe de 4 lignes suffit désormais),
puis `admin` ar/fa (27 fichiers / 328 clés, garde `DnnModuleLevelAdmin*` déjà en place pour zh).
Le constructeur est inchangé ; le socle de garde est prêt pour ces lots sans modification.
