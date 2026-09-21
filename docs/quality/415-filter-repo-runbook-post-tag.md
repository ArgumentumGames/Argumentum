# #415 — Runbook `git filter-repo` (Phase 2, **POST-TAG**) — ⛔ non exécuté

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `cea6e699`
**Grain** : pool #458 v7 ⑧. Ce document est un **runbook** : il décrit le geste, il ne l'exécute pas.
L'exécution est **gated sur tag v2.0.0 publié + GO owner explicite** (réécriture d'historique = non réversible pour les collaborateurs).

## État mesuré avant le geste (21/09, `git count-objects -vH` sur ce clone)

| | Valeur |
|---|---|
| `size-pack` | **2,17 GiB** (stable depuis le `gc` du 20/09, 2 packs) |
| loose | 120 objets |
| Gain attendu de la Phase 2 | **~2 GB → < 200 MB** (estimation #415, non re-mesurée ici) |

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
  ⛔ Le pathspec `Cartes/…` du plan d'origine (#415, juin) est **mort** (0 objet) : retiré de la commande — un pathspec qui ne matche rien est un no-op silencieux.
- ⚠️ `filter-repo` exige un clone frais (il refuse un dépôt avec remote configuré) : c'est pourquoi le geste part du miroir.
- Les **sources design** (`Generation/Sketch/*.sketch`, `Cards/Packaging/*.ai`) ne sont **pas** dans la liste : sources non régénérables, à trancher séparément (LFS, Phase 3).

## Vérifications AVANT tout push

```bash
git count-objects -vH                    # gain réel — chiffrer, ne pas supposer
git fsck --full                          # intégrité
git log --oneline -3                     # histoire lisible
git ls-files | wc -l                     # ordre de grandeur du HEAD préservé
```
Le gain doit être **chiffré et rapporté** ; un gain nul = un pathspec faux (relire l'étape « vérifier la liste »).

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

⛔ **Rien n'a été exécuté** — aucune mesure de gain réelle (l'estimation `< 200 MB` vient de #415, non re-mesurée) · ⛔ le sort des **sources design** (`Sketch/`, `Packaging/`) reste une décision séparée (Phase 3, LFS) · ⛔ les PRs ouvertes au moment du geste devront être recréées (à séquencer) · ⛔ les références de commits dans les issues existantes resteront affichées mais orphelines — c'est accepté, à dire dans l'annonce.

---

*Ce runbook est un artefact d'exécution : le lire en entier avant d'ouvrir un shell, et chiffrer chaque vérification plutôt que de la supposer.*
