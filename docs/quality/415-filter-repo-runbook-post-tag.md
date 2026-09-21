# #415 — Runbook `git filter-repo` (Phase 2, **POST-TAG**) — ⛔ non exécuté

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `6694d702` *(v7 ⑧ : `cea6e699`)*
**Grain** : pool #458 **v8 ⑧** (révision du runbook écrit en v7 ⑧). Ce document est un **runbook** : il décrit le geste, il ne l'exécute pas.
L'exécution est **gated sur tag v2.0.0 publié + GO owner explicite** (réécriture d'historique = non réversible pour les collaborateurs).

**Révision du 21/09 (v8 ⑧)** — trois ajouts, tous mesurés : le **gain est chiffré** (1,50 GiB) au lieu d'estimé · le pathspec `Cartes/`, déclaré « mort », **ne l'était pas** (1 154 objets) — motif corrigé · le **contrôle de non-régression sur les refs** est ajouté et outillé.

## État mesuré avant le geste (21/09, `git count-objects -vH` sur ce clone)

| | Valeur |
|---|---|
| `size-pack` | **2,17 GiB** (stable depuis le `gc` du 20/09, 2 packs) |
| loose | 120 objets |
| Périmètre de la **commande** | **416 objets / 233 chemins** (`Published/` 240, `ExtensionPackages/` 170, `Downloads/*.zip` 6) |
| Famille élargie (repère) | 462 objets — dont **46 marqueurs `*.REMOVED.git-id`** que les globs ne matchent pas (**2 160 o**) |
| **Gain de la Phase 2 (MESURÉ 21/09)** | **1,50 GiB** = **69 % du pack** ⇒ pack résiduel **≤ 0,67 GiB** |

### Le gain est désormais **mesuré**, plus estimé

L'estimation d'origine (#415, juin) — « ~2 GB → **< 200 MB** » — était fausse **aux deux bouts**, et elle était présentée comme un chiffre alors qu'elle n'avait jamais été mesurée. Re-chiffré le 21/09 par [`tools/415-filter-repo-phase2-preflight.py`](../../tools/415-filter-repo-phase2-preflight.py) :

| Périmètre | Objets | Packé |
|---|---:|---:|
| **commande** — `Published/` | 240 | 1 216,5 MiB |
| **commande** — `Downloads/*.zip` | 6 | 205,6 MiB |
| **commande** — `ExtensionPackages/*.resources` | 170 | 112,3 MiB |
| **COMMANDE — total** | **416** | **1 534,4 MiB = 1,50 GiB** |
| *famille élargie* — les mêmes + 46 marqueurs `.git-id` | *462* | *+2 160 o* |

⚠️ **Le compte publié doit dire LEQUEL.** La famille fait **462** objets, la **commande en retire 416** : l'écart est de **46 marqueurs `*.REMOVED.git-id`**, soit **2 160 octets exactement**. Mesuré par `--check-glob` : 277 chemins pour la famille, **233** pour les globs, **0** chemin en sens inverse — les globs ne débordent pas. Le verdict est **gradué par la conséquence** : à 2 160 o il sort **0** en nommant l'écart ; au-delà d'un seuil de **1 MiB** il sort 1.

> ⚠️ **Le seuil s'affiche en octets, et ce n'est pas cosmétique.** Le premier jet testait `gap == 0` : 46 marqueurs de 40 o ne pèsent pas zéro, ils pèsent 2 160 o — mais affichés en MiB ils rendent « 0,0 », et la porte sortait **rouge à tort**. Un écart arrondi devient invisible à celui qui doit décider.

**Pourquoi exactement 46, et pas 123.** La famille contient **123** objets marqueurs `*.REMOVED.git-id` : 77 sous `Published/`, 40 sous `ExtensionPackages/`, 6 sous `Downloads/`. Mais les trois globs n'ont pas la même forme :

| Glob | Forme | Les marqueurs ? |
|---|---|---|
| `**/Published/**` | **large** — tout ce qui est **sous** un dossier `Published/` | ✅ **les 77 sont emportés** |
| `DNNPlatform/App_Data/ExtensionPackages/*.resources` | **épinglé à une extension** | ❌ 40 ratés |
| `DNNPlatform/Portals/*/Downloads/*.zip` | **épinglé à une extension** | ❌ 6 ratés |

⇒ 123 − 77 = **46**, ce qui réconcilie exactement le verdict de `--check-glob`. **L'asymétrie de forme des globs est la cause de l'écart**, et elle est invisible à la lecture : deux des trois sont épinglés, le troisième est libre.

⛔ **La commande n'est PAS élargie pour autant** : le gain est **identique à 2 160 octets près** et élargir la portée d'une réécriture d'historique pour 2 Ko est un mauvais échange. Les 46 marqueurs sont **laissés en place, nommément**. Ce qui change n'est pas le geste, c'est le **chiffre** : « 416 objets retirés », pas « 462 ».

⛔ **La borne est la somme des tailles PACKEES**, jamais des tailles décompressées : ces objets pèsent **4,61 GiB décompressés** mais 1,50 GiB dans le pack — la compression par delta fait le reste. Lire la taille décompressée aurait annoncé un gain de 4,61 GiB sur un pack de 2,17 GiB.

⚠️ **C'est une borne haute, et elle tient ici de près** : un objet cible qui servirait de **base delta** à un objet survivant devrait être re-développé, rognant le gain. Mesuré : **99 des 462** objets de la famille sont déltifiés, mais **0 survivant** ne se base sur une cible — les binaires de build se déltent **entre versions de la même famille**, donc leurs bases sont elles aussi dans le périmètre. Contrôle exécuté : le parse des deltas voit bien 18 056 objets déltifiés (profondeur max 47), donc le zéro est **réel** et non un instrument aveugle.

⚠️ Le résiduel **≤ 0,67 GiB** est un **majorant** : après `filter-repo`, le `gc` re-paquette les survivants **de zéro** et re-optimise leurs deltas — le résultat peut être plus petit.

La **piste non destructive est épuisée** : le verrou `.gitignore` existe (mesuré 20/09, `Published/` l.260, `win-x64.zip` l.98, `Downloads/*.zip` l.263), le `gc` a rendu son gain (−50 MiB, 22→2 packs), les orphelins sont à 0. Rien de plus ne se gagne sans réécrire l'historique.

### ⚠️ Le constat de #415 (juin) est périmé sur un point — et ça change la vérification

#415 décrivait « **~1,4 GB encore trackés au HEAD** ». Mesuré le 21/09 : **0 fichier tracké** sur les 3 familles concernées (`Published/`, `Downloads/*.zip`, `ExtensionPackages/*.resources`) — la Phase 1 `rm --cached` a été faite entre-temps. Les binaires ne vivent plus que **dans l'historique** (462 objets comptés ci-dessous), ce qui est précisément l'objet de la Phase 2. Conséquence opérationnelle : la vérification des pathspecs se fait **sur l'historique**, ⛔ pas au HEAD (voir la commande corrigée ci-dessous).

## Pourquoi POST-TAG, sans exception

`filter-repo` **réécrit tous les SHAs**. Fait pendant une fenêtre de livraison : les PRs ouvertes sont invalidées, les références de commits dans les issues deviennent orphelines, et toute machine qui n'a pas re-cloné se retrouve sur une histoire divergente. Le tag v2.0.0 doit être **publié et validé** avant.

## Prérequis (tous, dans l'ordre)

1. **Tag v2.0.0 publié** et bundle validé (Adeline/Thomas) — cf. #134/#802.
2. **Fenêtre annoncée** : aucune PR ouverte hors dependabot, toutes les lanes prévenues (dashboard + DM), aucun worktree en cours de régénération.
3. **Disque libre ≥ 3× 2,17 GiB** sur la machine qui exécute.
4. **`git-filter-repo` disponible** (`pip install git-filter-repo` ; ⛔ jamais `git filter-branch`, déprécié et lent).
5. **Sauvegarde triple** (obligatoire, dans cet ordre) :
   ```bash
   git clone --mirror https://github.com/ArgumentumGames/Argumentum.git argumentum-mirror-pre
   cd argumentum-mirror-pre && git bundle create ../argumentum-pre-filter-<date>.bundle --all
   # + copie du bundle hors de la machine (GDrive .shared-state ou équivalent)
   ```

## Geste (sur le **miroir**, jamais sur un clone de travail)

```bash
cd argumentum-mirror-pre
git filter-repo --force \
  --path-glob '**/Published/**' \
  --path-glob 'DNNPlatform/Portals/*/Downloads/*.zip' \
  --path-glob 'DNNPlatform/App_Data/ExtensionPackages/*.resources' \
  --invert-paths
```

- ⚠️ **Vérifier la liste des chemins contre l'HISTORIQUE, jamais contre le HEAD** — mesuré le 21/09 :
  ```bash
  git rev-list --objects --all | grep -c 'Published/'          # -> 240
  git rev-list --objects --all | grep -cE 'Downloads/.*zip'    # -> 12
  git rev-list --objects --all | grep -c 'ExtensionPackages/'  # -> 210
  ```
  ⛔ **`git ls-files` est l'instrument faux ici** : il ne voit que le HEAD, or **les binaires n'y sont plus** (Phase 1 `rm --cached` déjà faite — 0 fichier tracké sur les 3 familles). Un contrôle au HEAD rendrait 0 partout et ferait conclure « rien à purger », alors que 462 objets attendent dans l'historique. `filter-repo` opère sur l'historique : c'est là qu'on compte.
  ⛔ **Le pathspec `Cartes/…` du plan d'origine (#415, juin) a été retiré — à raison, mais pour un motif FAUX** (remplacé le 21/09). Ce runbook le déclarait « **mort (0 objet)** ». **Mesuré dans l'historique : 1 154 objets.** Le jugement avait été porté au HEAD — exactement l'erreur contre laquelle la ligne ci-dessus met en garde, commise ici **dans l'autre sens**.

  | `Cartes/` | Objets | Packé |
  |---|---:|---:|
  | total historique | 1 154 | — |
  | dont déjà couverts par `**/Published/**` | 68 | — |
  | **dont hors périmètre** | **1 086** | **14,2 MiB** |

  `Cartes/` est le **nom ancestral** de `Cards/` : ces 1 086 objets sont de l'**historique de source** — `.cs`, `.csproj`, `.json`, `.md`, du corpus — **pas des binaires de build**. Le retirer était donc **juste** ; c'est le **motif** qui était faux.

  ⚠️ **Le danger n'est pas théorique** : un lecteur qui « répare » la commande en remettant `Cartes/`, sur la foi du motif erroné (« il ne matche rien, donc il est inoffensif »), **détruit 1 086 objets d'historique de source pour 14,2 MiB** — 0,9 % du gain, contre un historique de corpus amputé. **Le motif d'un retrait fait partie de la commande : un motif faux se re-exécute à l'envers.**
- ⚠️ `filter-repo` exige un clone frais (il refuse un dépôt avec remote configuré) : c'est pourquoi le geste part du miroir.
- Les **sources design** (`Generation/Sketch/*.sketch`, `Cards/Packaging/*.ai`) ne sont **pas** dans la liste : sources non régénérables, à trancher séparément (LFS, Phase 3).

## Vérifications AVANT tout push

```bash
git count-objects -vH                    # gain réel — chiffrer, ne pas supposer
git fsck --full                          # intégrité
git log --oneline -3                     # histoire lisible
git ls-files | wc -l                     # ordre de grandeur du HEAD préservé
```
Le gain doit être **chiffré et rapporté**. Attendu (mesuré le 21/09, borne haute) : **1,50 GiB de moins**, soit un pack résiduel **≤ 0,67 GiB**. Un gain nul = un pathspec faux (relire l'étape « vérifier la liste ») ; un gain très inférieur à 1,50 GiB = des objets cibles ont survécu.

### Contrôle de non-régression sur les refs

`filter-repo` **réécrit tous les SHAs**, donc aucune comparaison de valeur n'a de sens. Le contrôle porte sur ce qui doit **survivre** — le **nom** des refs — et sur un invariant fort qui, lui, est falsifiable :

| Contrôle | Attendu | Pourquoi |
|---|---|---|
| Nombre de refs PRE → POST | **431 → 431** | 375 branches, 2 tags, 52 remotes, 2 `stash-archive` (relevé du 21/09) |
| Refs **disparues** | **0** | une ref perdue = un travail perdu, et le rollback ne le dit pas |
| Refs **apparues** | **0** | un filtre qui *crée* une ref a mal tourné |
| SHAs de déplacés | **toutes** | c'est le but — **jamais** un critère d'échec |
| **Les 3 familles au HEAD** | **0 fichier** | ⭐ l'invariant : ces chemins ne sont pas au HEAD, donc l'**arbre du HEAD doit être identique** après filtrage. Mesuré le 21/09 : `Published/`=0, `Downloads/*.zip`=0, `ExtensionPackages/`=0. |

L'instrument est **committé**, avec l'état PRE figé :

```bash
python tools/415-filter-repo-phase2-preflight.py                                  # rapport + borne de gain
python tools/415-filter-repo-phase2-preflight.py --emit-list  <fichier>           # les 462 objets visés
python tools/415-filter-repo-phase2-preflight.py --emit-refs  <fichier>           # inventaire PRE des refs
python tools/415-filter-repo-phase2-preflight.py --check-refs <fichier>           # À LANCER APRÈS le filtre
python tools/415-filter-repo-phase2-preflight.py --emit-command                   # la commande littérale
```

Évidence figée au 2026-09-21 : la liste des **416 objets** que la commande retire (sha, type, taille packée, famille, chemin) dans [`415-objets-vises-phase2-2026-09-21.txt`](415-objets-vises-phase2-2026-09-21.txt) — `--family` écrit la famille élargie (462) — et l'inventaire des **431 refs** PRE dans [`415-refs-pre-phase2-2026-09-21.txt`](415-refs-pre-phase2-2026-09-21.txt). ⛔ **Le script est en lecture seule : il n'exécute jamais `filter-repo`.**

⭐ **Le contrôle ne se contente pas de « ne pas régresser »** : il **prouve l'invariant**. Puisque 0 fichier des 3 familles est tracké au HEAD, retirer ces chemins de l'historique **ne peut pas** changer l'arbre du HEAD. Si `git ls-files` change de contenu après le filtre, c'est que le pathspec a mordu **hors** périmètre — le seul mode d'échec qui compte vraiment ici.

## Publication

```bash
git push --force --all
git push --force --tags
```
- **Force-push coordonné et annoncé** (dashboards + DM à toutes les lanes) : c'est le seul cas où il est légitime ici.
- **Toutes les machines re-clonent** (`git clone`, jamais `git pull` — les SHAs ont changé).
- Message d'annonce : ce qui a changé (SHAs), ce qu'il faut faire (re-clone), ce qui reste vrai (contenu du HEAD identique, tag v2.0.0 re-poussé).

## Rollback

Le miroir + bundle pré-filtrés restaurés sur GitHub = retour à l'état antérieur (force-push depuis le miroir). GitHub conserve par ailleurs les objets orphelins ~90 jours (support). ⇒ **La sauvegarde n'est pas optionnelle : c'est le rollback.**

## Ce que ce runbook n'établit pas

⛔ **Rien n'a été exécuté** — le gain **1,50 GiB est une borne haute calculée sur le pack actuel**, pas un résultat de `filter-repo` : le filtre n'a jamais tourné, donc le pack résiduel réel (≤ 0,67 GiB) reste **calculé, non constaté** · ⛔ **la liste des 416 objets est un périmètre de CHEMINS** : elle établit quels objets matchent, ⛔ pas qu'aucun objet légitime ne sera emporté par ricochet (un blob partagé entre un chemin visé et un chemin conservé disparaît pour les deux — le contrôle au HEAD l'attrape, pas l'énumération) · ⛔ **les 46 marqueurs laissés en place** sont nommés mais pas mesurés au-delà de leur taille : on n'établit pas qu'ils soient sans usage (un `.git-id` peut être lu par un outil DNN — non vérifié) · ⛔ le sort des **sources design** (`Sketch/`, `Packaging/`) reste une décision séparée (Phase 3, LFS) · ⛔ les PRs ouvertes au moment du geste devront être recréées (à séquencer) · ⛔ les références de commits dans les issues existantes resteront affichées mais orphelines — c'est accepté, à dire dans l'annonce · ⛔ **le gain n'est pas une mesure de temps** : rien ici ne chiffre la durée du geste ni celle du re-clone des 6 lanes.

*MaJ du 2026-09-21 (pool #458 v8, grain ⑧) : gain chiffré au lieu d'estimé · pathspec `Cartes/` corrigé · périmètre **commande** (416) distingué de la **famille** (462), écart nommé · contrôle de refs ajouté · instrument et évidence committés.*

---

*Ce runbook est un artefact d'exécution : le lire en entier avant d'ouvrir un shell, et chiffrer chaque vérification plutôt que de la supposer.*
