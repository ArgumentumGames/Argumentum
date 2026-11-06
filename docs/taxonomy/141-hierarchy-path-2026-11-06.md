# #498 grain 1b — les jumeaux `141-aif-*.py` lisent la hiérarchie dans `path`

**Date** : 2026-11-06 · **Lane** : po-2024 · **Grain** : ai-01 [#458 c.6678039968](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6678039968) (grain 1b accepté : « les jumeaux `141-aif-*.py` passent sur `path`, puis l'export des arêtes (aujourd'hui une étoile), **deux PR** »).
**Cette PR = la première des deux** (jumeaux). L'export des arêtes (`tools/aif-attack-graph-export.py`) suit séparément.
⛔ **0 écriture corpus**, 0 cellule CSV, 0 appel API : scripts d'instrument uniquement, comme le grain 1 (#2167).

## 1. Le défaut, mesuré (rappel du grain 1)

Les jumeaux reconstruisaient parents, frères et profondeurs depuis `decimal_path` — une seule virgule, niveaux collés **chiffre par chiffre**. Mesure du 06/11 (sondes rejouées avant cette édition) :

- `comma2dot(decimal_path) == path` sur **1 407 / 1 408** rangées ;
- l'unique écart : **rangée 824** (`5,123224`, vrai `path` `5.1.2.3.2.24`, « Confusion sans retour ») — un rang **24** éclaté en `5.1.2.3.2.2.4`, une cible **inexistante** dans l'arbre (mesuré : `not in bypath`) ;
- la colonne `depth` est **fausse sur 2 rangées** : `0` vaut 0 (racine document), la rangée 824 vaut 7 alors que `path` a **8 segments** — un filtre de profondeur posé sur cette colonne lit la mauvaise population.

Subtilité consignée par le self-test : le parent **coma** fabriqué par l'ancien code (`5,12322`) **existe bel et bien** — c'est `5.1.2.3.2.2`, un **frère** de la rangée 824. Le parent coma tombait par accident sur un voisin réel ; c'est la cible **pointée** (`5.1.2.3.2.2.4`), servie aux prompts via `bydot`, qui n'existe pas. Un contrôle qui n'aurait testé que le parent coma aurait lu un faux « sain ».

## 2. Le changement

**Module partagé [`141-hierarchy.py`](../../docs/taxonomy/141-hierarchy.py)** (unique copie des primitives) :

| Primitive | Rôle |
|---|---|
| `path_parent(p)` | parent = retrait du dernier segment pointé |
| `path_depth(p)` | profondeur = nombre de segments (⚠️ jamais la colonne `depth`) |
| `dot2comma(p)` | rendu coma depuis `path` (famille + virgule + niveaux collés ; rangs ≥ 10 entiers, non ambigus) |
| `load_taxonomy(path)` | `(rows, bypath, bycomma, bydot)` — `bycomma` re-dérivé de `path`, `bydot == bypath` (contrat historique documenté) |
| `build_pool(node, rows, bypath)` | frères + parent + enfants + racines depth-1 — le contrat des jumeaux, inchangé |
| `comma2dot` / `comma_parent_DEFECTUEUX` | **direction défectueuse conservée pour le contrôle inverse** |

**Jumeaux recâblés** : `141-aif-explore.py` (toutes les vues : distribution de profondeur, familles, frères, échantillon, JSON), `141-aif-stage1.py` (pool + cibles pointées + `source_dotted`), `141-aif-fullscale.py` (pool, ordre, clés coma, prompts, checkpoint).

**Forme des clés inchangée** : `source_dp` (coma), `decimal_path` du JSON d'échantillon, cibles `dotted` des prompts — seules la **source de vérité** et les cibles rang-≥-10 changent.

## 3. ⚠️ Provenance des clés — la reproductibilité historique

Le CSV vivant a bougé depuis le pilote #141 (suppression de PK 96 via #1288, retraits, P2-A…) : l'ancien code (lu depuis la colonne `decimal_path` d'origine) et le nouveau (clés re-dérivées de `path`) **ne produisent pas nécessairement les mêmes clés pour les noeuds historiques**. Mesuré sur le vivant : le compte de non-cartes passe **1 232 → 1 233** et la distribution de profondeur diffère (part structure, part corrections).

⇒ **Un échantillon régénéré n'est PAS comparable au sidecar committé sans jointure sur les clés.** C'est la même famille que la fiche « artefact engendré : épingler l'identité au générateur » : les artefacts committés (`141-aif-candidates-*.csv`, `141-aif-stage2-diff.csv`, `141-aif-warn-triage.csv`) sont des **instantanés** ; la régénération est un instrument neuf, pas une reproduction. La note de provenance est dans les docstrings de `stage1`/`fullscale`.

## 4. Contrôles exécutés (preuves, pas récit)

1. **Self-test [`141-hierarchy-self-test.py`](../../docs/taxonomy/141-hierarchy-self-test.py) : 25/25 verts** — corpus vivant (1 408 uniques, 0 orphelin, parent de 824 réel, pool sans cible fabriquée, 2 écarts depth nommés) + synthétiques (**défaut reproduit** : `comma2dot('5,123224') == '5.1.2.3.2.2.4'`).
2. **`py_compile` OK** sur les 7 fichiers (module + self-test + 5 jumeaux, dont les 2 read-only non modifiés).
3. **`141-aif-explore.py` exécuté réellement** (read-only + `tmp/` gitignoré) : sortie complète cohérente, échantillon 28 nœuds.
4. `stage1`/`fullscale` **non exécutés** : ils requièrent une clé API et écrivent leurs sidecars de sortie — hors périmètre du grain (0 appel, 0 réécriture d'artefact). Leur chemin de hiérarchie est couvert par le module testé (pool = même primitive, mesuré sur la rangée 824).
5. `stage2-diff`/`warn-triage` **inchangés** (read-only, sans hiérarchie) : ils consomment les clés coma des sidecars, dont la forme est préservée — jointure directe.

## 5. Suite

**PR suivante (même grain 1b)** : `export_taxonomy_tree` (`tools/aif-attack-graph-export.py`) lit `path`, re-dérivation de `docs/ontology/aif-export/taxonomy-tree-edges.csv` (aujourd'hui une étoile : 1 408 lignes, depth ∈ {1, 2}, 8 parents — mesuré au grain 1).
