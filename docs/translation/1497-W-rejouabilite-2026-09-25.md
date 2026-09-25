# #1497-W — re-jouabilité de l'instrument d'écriture des `link_*`

**Auteur** : po-2024 (worker) · **Date** : 2026-09-25 · **Base** : `9e79e88d`
**Grain** : pool v13 ④ ([#458 c.5836219154](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5836219154))
**Objet** : `tools/1497-write-261W-byte-exact.py` — un instrument « écrire puis prouver » doit lire sa base dans l'**historique**, pas dans l'arbre déjà écrit.

---

## 1. Le défaut, reproduit

ai-01 l'a constaté au merge de #1557 : *« `--mutation-test` ne se rejoue plus après l'écriture : la garde de collision lit l'arbre déjà écrit. »*

Reproduit **avant** tout correctif, sur `9e79e88d` :

```
$ python tools/1497-write-261W-byte-exact.py --mutation-test
worklist : 254 ecritures, 7 retenues nommees, 11 sans section (non ecrites)
AssertionError: COLLISION pk=34 link_ar non vide: 'https://ar.wikipedia.org/wiki/%D8%AF%D9%84%D9%8A%D9%84_...'
rc=1
```

**Mécanisme** : `mutation_test()` appelait `load_raw()` — le fichier de travail — puis `build()`, dont la garde (1) exige des cellules **vides**. Après `--apply`, ces cellules sont remplies : la garde rougit sur l'écriture qu'elle vient de faire. La preuve ne se rejouait donc **qu'une fois**, et `--check` tombait par le même chemin.

## 2. Le correctif

La base devient un **paramètre**, et sa valeur par défaut est **dérivée**, jamais récitée :

- `--apply` lit **l'arbre** — c'est l'opération d'écriture, la garde (1) est son garde-fou ;
- `--check` (défaut), `--replay-check`, `--mutation-test` lisent **la base historique** ;
- la base est **dérivée** par `find_base_ref()` : le commit le plus récent dont le blob porte **toutes les cibles vides** — cibles écrites *et* retenues. Le commit d'écriture lui-même est donc écarté par le **prédicat**, jamais par un SHA recopié ;
- la base retenue est **imprimée** dans chaque sortie (`base : <sha> (derivee)`), et `--base-ref <ref>` la force.

Deux modes sont ajoutés, pour que la re-jouabilité soit **mesurable** et pas seulement affirmée :

| Mode | Ce qu'il prouve |
|---|---|
| `--replay-check` | la re-dérivation depuis la base reproduit les **cellules de l'arbre** (et rapporte l'égalité d'octets en information) |
| `--self-test` | contrôle direct **et** contrôle inverse : mutation vue sur la base dérivée, et garde (1) qui **rougit** quand la base est l'état écrit |

## 3. La base, dérivée — et l'écart au SHA du dispatch, déclaré

Le dispatch nommait `d5ca4175`. La dérivation rend **`1833de59`** (2026-09-25, #1554), soit le commit qui **touche réellement le CSV** avant l'écriture. L'écart est un écart de **nom**, pas de **contenu** — mesuré sur les quatre refs candidates :

| `ref:CSV` | sha256 du blob |
|---|---|
| `1833de59` (dérivée) | `42282278c89c76c3` |
| `d5ca4175` (nommée par le dispatch) | `42282278c89c76c3` |
| `92da16ab` | `42282278c89c76c3` |
| `9e79e88d^` | `42282278c89c76c3` |

`d5ca4175` (#1555) ne touche que `docs/` et `tools/` : son blob CSV est **byte-identique** à celui de son ancêtre `1833de59`. Les deux bases sont donc correctes, et la dérivation nomme celle qui **porte** l'état du fichier plutôt que celle qui l'hérite.

## 4. Preuves (toutes sur `9e79e88d`, instrument patché)

```
$ python tools/1497-write-261W-byte-exact.py                      # rc=0
base : 1833de5941eb13ae8d2d495bc4d679f8ff6eed63 (derivee)
plan : 254 cellules
verification : 254 cellules changees, exactement la worklist ; 1408 lignes ; BOM/CRLF invariants ; 7 retenues vides
octets : 4151129 -> 4172087 (delta +20958)
MODE --check : aucune ecriture.

$ python tools/1497-write-261W-byte-exact.py --replay-check       # rc=0
base      : 1833de5941eb13ae8d2d495bc4d679f8ff6eed63
re-derive : 254 cibles, sha256 5ad17709314d8cb4
arbre     : sha256 5ad17709314d8cb4 (octets egaux)
re-jouabilite : OK — 254/254 cibles de l'arbre reproduites depuis 1833de59

$ python tools/1497-write-261W-byte-exact.py --mutation-test      # rc=0
MUTATION VUE (garde rougit) : RETENUE NON VIDE (614,pt) [...]

$ python tools/1497-write-261W-byte-exact.py --self-test          # rc=0
MUTATION VUE (garde rougit) : RETENUE NON VIDE (614,pt) [...]
controle falsifiant VU (base = ecrit) : COLLISION pk=34 link_ar non vide: 'https://ar.wikipedia.org/wiki/%D8%AF...
self-test OK : base 1833de59 (cibles vides) < ecrit 9e79e88d (cibles remplies)

$ python tools/1497-write-261W-byte-exact.py --mutation-test --base-ref HEAD   # rc=1
AssertionError: COLLISION pk=34 link_ar non vide: 'https://ar.wikipedia.org/wiki/%D8%AF...
```

⭐ Le `--replay-check` est la preuve la plus forte : la re-dérivation depuis la base reproduit **à l'octet** le CSV livré (`sha256 5ad17709314d8cb4` des deux côtés). L'instrument ne se contente pas de re-tourner — il **reproduit l'artefact**.

⭐ Le dernier appel est le **contrôle falsifiant** : forcer la base sur l'état écrit reproduit **exactement** le défaut d'origine. Un instrument dont on n'a pas vu l'échec n'a rien prouvé.

## 5. Limites, nommées

- **Le contrôle falsifiant rend `rc=1`** (assertion non rattrapée), pas un code dédié. C'est un comportement attendu ici, mais ⚠️ **une automatisation ne doit pas lire `1` comme « instrument cassé »** : le même code sort d'un `--base-ref` délibéré et d'une base réellement corrompue. Les seuls codes porteurs de sens sont `--self-test` / `--replay-check` à `0`.
- **La dérivation dépend de l'historique git.** Dans un clone superficiel (`fetch-depth: 1`, le défaut d'`actions/checkout`), `git log -- <csv>` ne rend qu'un commit : `find_base_ref()` lèvera *« aucune base pre-ecriture »*. ⛔ Ne pas câbler cet instrument dans un workflow CI sans `fetch-depth: 0`. Le test `test_*.py` std-lib de #1460 n'a pas cette contrainte ; celui-ci l'a. Câblage non fait ici : c'est une décision de lane.
- **`--apply` post-écrit rougit par construction** (garde 1) : c'est le comportement voulu — il n'y a plus rien à appliquer — et non un défaut.
- Aucune autre assertion n'a été modifiée : gardes (1) à (6), `--head-check`, le splice par offsets et la borne de 261 sont **inchangés**.

## 6. Exécutable

```
python tools/1497-write-261W-byte-exact.py --self-test      # rc=0 : direct + inverse
python tools/1497-write-261W-byte-exact.py --replay-check   # rc=0 : re-derivation == arbre
python tools/1497-write-261W-byte-exact.py                  # rc=0 : plan, 0 ecriture
```

Référentiels : `tools/1497-write-261W-worklist.json`, `tools/1497-audit-261W.json`.
