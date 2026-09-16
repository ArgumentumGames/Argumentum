# #1049 — Organe de garde webroot : le `web.config` racine ne doit plus jamais atteindre un webroot par le dépôt

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base** : `origin/master` `503e64c8`
**Nature** : organe anti-retour (tests) + ce document. **Aucun geste webroot** — aucune écriture dans
`DNNPlatform/` du checkout principal (le webroot vivant), aucun fichier supprimé du disque, aucune
régénération, aucun déploiement. Les témoins de mutation sont joués dans un worktree jetable.

---

## §1 — Le défaut, mesuré deux fois

Le dépôt suivait `DNNPlatform/web.config` — un instantané **DNN 9.11.1 de 2023** (`16e8c6de`),
retouché juin 2026 pour le durcissement CSP/cookies (`092bb8f3`, #131) : **79 286 o**, blob
`b1f7a7a3…2ab38a6f`. L'état de croisière du webroot est un `web.config` de **96 984 o** en 10.3.2.
Le fichier versionné n'a **jamais** pu faire tourner ce site, dans aucun état du dépôt.

| incident | séquence | effet |
|---|---|---|
| **2026-08-10 01:13** | opération git de routine écrit le blob suivi par-dessus le vivant | 500 déterministe sur tout `dnn.argumentum.myia.io` |
| **2026-08-16 12:12 → 14:47** | master local **étale** qui trackait encore le fichier : `git checkout master` ré-écrit le blob par-dessus le vivant, puis `git pull --ff-only` avance jusqu'à `167fbd33` (#1055, le commit qui dé-tracke) ⇒ git **supprime** le fichier du working tree ⇒ IIS sans `web.config` | 500 total, **2 h 35** |

Leçon de l'incident 2 : **dé-tracker un fichier vivant est actif au checkout, pas seulement au
commit** — le correctif #1055 a lui-même été l'instrument de la suppression dans cette séquence.

`#1055` (10/08) : dé-track + renommage en `web.config.example` (assaini, secrets rédacts) + règle
d'ignore `/DNNPlatform/web.config`. La prod (`www.argumentum.games`, web1) est **mesurée hors cause**
(10/08) : son webroot n'est pas un checkout git.

## §2 — Deux déclencheurs, deux gardes

La brèche #1049 a **deux faces**, gardées par **deux organes jumeaux** :

| | #1244 (bin/) | #1049 (web.config racine) |
|---|---|---|
| toxique suivi | 195 fichiers sous `DNNPlatform/bin/` (179 DLL, instantané 9.11.1) | `DNNPlatform/web.config` (79 286 o) |
| effet sur un webroot | downgrade silencieux 10.3.2 → 9.11.1 (panne 28/08) | 500 total, 2 occurrences (10/08, 16/08) |
| résolution | dé-track (a), commit `5c616ec4` + provenance `1244a-…-provenance.md` | dé-track + template `web.config.example`, #1055 `167fbd33` |
| organe | `DnnBinUntrackGuardTests` (PR **#1393**, `517bbecf`) | `DnnWebrootGuardTests` (cette PR) |

Le lien est documenté des deux côtés : `docs/quality/1244a-dnn-bin-untrack-provenance.md` §7
annonçait ce frère ; le présent document le réalise. Les deux organes partagent la même structure
(sonde d'index + sonde de bouclier `.gitignore`, attribut de skip visible hors checkout git) et les
mêmes helpers (`RequiresGitIndexFactAttribute`, `DnnBinUntrackGuard.RunGit`, `TestRepoRoot`).

## §3 — L'organe : un sondage par sens de la brèche

`Generation/Converters/Argumentum.AssetConverter.Tests/Dnn/DnnWebrootGuardTests.cs`, deux faits :

1. **Sens dépôt → webroot (l'index Git).** `git ls-files -- DNNPlatform/web.config` doit être
   **vide**, corroboré par la liste complète filtrée en mémoire (le pathspec borné a déjà produit
   un vide faux, artefact intermittent consigné). Tant que le chemin n'est pas suivi, aucune
   opération git — checkout, pull, restore, stash, reset, merge — ne peut matérialiser le toxique
   dans un webroot. Égalité **exacte** (insensible à la casse, IIS l'est aussi ; séparateurs
   normalisés) : `web.config.example`, les `Web.config` de sous-répertoires (contenu plateforme
   suivi, jamais mesuré toxique) et `Generation/CardPen/web.config` (autre hôte) ne sont **pas**
   des violations.
2. **Sens webroot → dépôt (le bouclier `.gitignore`).** La règle EXPLICITE
   `/DNNPlatform/web.config` doit survivre. Le webroot vivant **est** un checkout git (#1358) et
   le `web.config` vivant porte des secrets (machineKey — exposition déjà payée, rotation 25/07) :
   sans la règle, un `git add .` de routine dans le webroot publie le vivant dans le dépôt
   **public**. Un glob incident (`web.config` nu, `DNNPlatform/`) ou une ré-inclusion `!` ne
   compte pas — même exigence de nommage explicite que la garde sœur #1393.

Le bouclier est la première ligne de défense ; l'organe d'index la seconde. Hors d'un checkout
git (p.ex. un zip du dépôt), les deux faits **sautent visiblement** — jamais un vert par vacuité.

## §4 — Témoins de mutation (joués avant livraison)

Worktree jetable, fichier **texte** uniquement, aucun binaire, aucun webroot :

- `git add -f` d'un témoin texte à `DNNPlatform/web.config` → l'organe passe **rouge** nommant le
  chemin ; `git rm --cached` + retrait du témoin → **vert**.
- Neutralisation de la règle `.gitignore` (réécriture sans `/DNNPlatform/web.config`) → **rouge**
  sur le fait du bouclier ; restauration depuis copie de sauvegarde (diff vide vérifié) → **vert**.
  Nota : la neutralisation a été jouée par réécriture du fichier — l'effet retenu est « la règle
  n'est plus dans les lignes parsées », qui est exactement la condition que le fait teste.
- Six témoins purs supplémentaires (vecteur historique exact, variantes casse/séparateurs,
  proches-non-violations, ré-inclusion `!`, glob incident, règle explicite) rougissent/passent par
  construction.

## §5 — Ce que cet organe n'établit pas

- ⛔ **Il ne ferme pas la topologie** : le correctif structurel (option a — webroot copié hors de
  l'arbre git, Phase 1 robocopy 18/08 vers `D:\DNN\live\…`) reste à compléter par la Phase 2 owner
  (repoint `appcmd` + ACL). L'organe protège le côté dépôt entre-temps.
- ⛔ Il ne couvre pas `DNNPlatform/bin/` (garde sœur #1393) ni les `Web.config` de sous-répertoires.
- ⛔ Il ne dit rien des 9 261 autres fichiers suivis qui restent dans le périmètre du webroot —
  c'est l'objet de la décision topologique, pas d'un test.
- ⛔ Aucun verdict visuel (ai-01), aucune décision (jsboige), aucune mesure du webroot vivant.

---

*Base `503e64c8` · lecture seule sur le dépôt (`ls-tree`, `ls-files`, `.gitignore`) · témoins en
worktree jetable · ⛔ aucune écriture webroot, aucune suppression disque · verdict visuel : ai-01.*
