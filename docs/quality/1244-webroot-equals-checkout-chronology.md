# #1244 — Chronologie « webroot = checkout git » : dossier de preuve

**Auteur** : po-2023 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `21a72385`, relevé sur le checkout local `ae84c91c`
**Nature** : **mesure**, lecture seule. Aucun dépôt, aucune régénération, **aucun écart de version décidé**.
**Portée** : grain ② de la deep-queue po-2023 (`#458` c.`5656689863`), **consomme le grain ①** ([#1357](https://github.com/ArgumentumGames/Argumentum/pull/1357)).
**Interdit tenu** : ni (a) dé-tracker, ni (b) committer le bin 10.3.2, ni (c) garde de déploiement — **rien n'est tranché ici**.

---

## §0 — La question, rendue décidable

#1244 pose l'arbitrage (a)/(b)/(c) mais laisse un préalable explicite : *« établir qu'aucun déploiement en
service ne tire ses binaires de `DNNPlatform/bin/` »*. Ce préalable suppose une question plus élémentaire, qui
n'était jusqu'ici établie par aucun relevé :

> **Le webroot de préprod est-il, ou non, un checkout git ?**

Ce dossier **répond à cette question par la mesure** et fournit la chronologie datée qui la sous-tend. Il ne
choisit aucune des trois branches : il établit le fait sur lequel (a), (b) et (c) reçoivent des coûts
différents.

**Réponse mesurée : oui.** Le webroot de préprod **est** le sous-arbre `DNNPlatform/` d'un working tree git —
et la couche servie y vit sous forme de fichiers **non tracés**. Ce n'est pas une ressemblance : c'est une
identité, et c'est précisément le danger nommé par #1244.

---

## §1 — Instrument, et son contrôle zéro

- **Sonde** : `sha256sum` sur le **fichier disque**, comparé à `git show <rev>:<chemin> | sha256sum`. Les
  comparaisons HTTP du grain ① ne sont pas refaites : elles sont **citées** et servent de second instrument.
- **Aucun geste écrivant** : `git ls-files`, `ls-tree`, `cat-file`, `rev-list`, `for-each-ref`, `status`,
  `stash list`, `reflog show`, `stat`. Pas de `checkout`, pas de `stash`, pas de `clean`, pas de `fetch`.
- **Répertoires** : dépôt = `D:/Dev/Argumentum` (`git rev-parse --show-toplevel`) ; webroot présumé =
  `D:/Dev/Argumentum/DNNPlatform`.

**Cap 0 — l'instrument disque reproduit une valeur obtenue par l'instrument HTTP** (grain ①, hôte
`dnn.argumentum.myia.io`) :

| témoin | grain ① (HTTP) | ce harnais (disque) | accord |
|---|---|---|---|
| `fallacies_fr` | 2 361 730 o | **2 361 730 o** | ✅ |
| `fallacies_fr` (empreinte) | = blob `de763aa9` | = blob `de763aa9` | ✅ |
| `argumentum_fallacies.owl` | sha256 `258a9e430e4f…` | sha256 `258a9e430e4f…` | ✅ |

Deux instruments indépendants — un socket HTTP, un système de fichiers — **nomment le même objet par la même
empreinte**. Le harnais n'est pas calibré sur lui-même.

---

## §2 — Fait structurel : le webroot est un sous-arbre tracé du working tree

```
git rev-parse --show-toplevel   →  D:/Dev/Argumentum
git ls-files DNNPlatform/ | wc -l  →  9262 fichiers tracés
```

`DNNPlatform/` n'est pas un répertoire annexe : c'est **9 262 fichiers suivis** du dépôt. Le site IIS sert
donc depuis un répertoire que git considère comme sien.

Ce n'est pas un indice, c'est la définition du piège de #1244 : `CLAUDE.md:11` décrit `DNNPlatform/` comme un
*export* — mais un export **tracé** est, pour git, une arborescence ordinaire. Toute commande git qui réécrit
le working tree réécrit la couche servie.

### Les fichiers servis sont **non tracés**, à la racine du webroot

`git ls-files DNNPlatform/fallacies_fr.html DNNPlatform/virtues_fr.html DNNPlatform/argumentum_fallacies.owl`
→ **vide**. Les 17 fichiers servis sont **invisibles** à l'inventaire tracé.

Sur disque, ils sont pourtant bien là :

| chemin | octets | mtime |
|---|---:|---|
| `DNNPlatform/fallacies_{ar,en,es,fa,fr,pt,ru,zh}.html` | 2 216 473 – 4 968 179 | **2026-08-28 10:00:17** |
| `DNNPlatform/virtues_{ar,en,es,fa,fr,pt,ru,zh}.html` | 498 023 – 1 225 370 | **2026-08-28 10:00:17** |
| `DNNPlatform/argumentum_fallacies.owl` | 4 795 192 | **2026-08-28 10:00:17** |

**17/17 partagent la même seconde.** Aucun `DNNPlatform/*_ext.html` n'existe sur disque.

---

## §3 — Chaîne causale : servi = disque = stash = **hors de toute couche committée**

Trois maillons, chacun mesuré :

| maillon | valeur |
|---|---|
| **servi** (grain ①, 2 hôtes) | `argumentum_fallacies.owl` sha256 `258a9e430e4f…` |
| **disque** (ce dossier) | `DNNPlatform/argumentum_fallacies.owl` sha256 `258a9e430e4f…` |
| **stash** `5a086dfe` | `258a9e430e4f…` — **égal** |
| **committé** `docs/ontology/argumentum.owl` | `0aff63a3bbb4…` — **différent** (et grain ① : aucune des 9 révisions ne porte le sha servi) |

Pour les wrappers, la concordance est encore plus informative :

| objet | empreinte | lecture |
|---|---|---|
| disque `DNNPlatform/fallacies_fr.html` | `3b6b0f14751015d5` | couche **déployée** |
| stash `5a086dfe:DNNPlatform/fallacies_fr.html` | `3b6b0f14751015d5` | identique au disque |
| committé `de763aa9:Cards/Fallacies/Mindmaps/fr/Fallacies_fr.html` | `3b6b0f14751015d5` | **la même couche**, autre chemin |
| committé `HEAD:Cards/Fallacies/Mindmaps/fr/Fallacies_fr.html` | `510a32ce20469af0` | **6 commits plus loin** |

⇒ La couche servie n'est **pas** dans le chemin tracé : elle a été **copiée** depuis
`Cards/Fallacies/Mindmaps/<lang>/` vers la racine du webroot, où elle vit **hors du contrôle de version**.

**Conséquence, et c'est le cœur du dossier** : `DNNPlatform/{fallacies,virtues}_*.html` et
`DNNPlatform/argumentum_fallacies.owl` sont des artefacts **servis, vivants, et non tracés** — donc
détruits sans avertissement par `git stash -u`, `git clean -xfd`, ou un `git checkout` vers une autre
branche. Ce n'est pas une hypothèse : c'est arrivé, et c'est daté au §4.

---

## §4 — Chronologie datée

### 4.1 — L'archive existe, et elle est **référencée** (correction d'une note interne)

Une note de ma mémoire de session indiquait que l'archive du 28/08 était `stash@{0}`. **La mesure la
contredit** :

```
git stash list            → 1 seule entrée : 042edd2f, 2026-07-17 20:58:44, sur dnn/sandbox-runtime-1032
git reflog show stash     → 1 seule entrée (la même)
refs/stash-archive/arg/9  → fcc73d52  « On master: wip-ai-01-pre-merge-1212 »   2026-08-28 10:00:17
```

L'archive du 28/08 est donc `refs/stash-archive/arg/9`, **pas** `stash@{0}`. Elle est **bien préservée**
(une ref la tient : `git for-each-ref` ne la trouve pas par empreinte, mais `rev-list --all` la traverse), et
`5a086dfe` en est le **troisième parent** :

```
git cat-file -p fcc73d52
  parent 1c5231f6…   (tip master de l'époque)
  parent 74815784…   (index)
  parent 5a086dfe…   (untracked)      ← 343 chemins
```

Le correctif porte sur le **nom du détenteur**, pas sur la préservation. Il compte : un inventaire qui
cherche l'archive dans `stash@{0}` ne la trouve pas.

### 4.2 — Ce que l'archive contient : un inventaire d'application DNN entier

`git ls-tree -r --name-only 5a086dfe` → **343 chemins**, dont les 17 artefacts servis
(`DNNPlatform/argumentum_fallacies.owl`, les 16 wrappers) — et, massivement, des fichiers **stock** d'une
installation DNN : `DesktopModules/Admin/Dnn.PersonaBar/…`, `Dnn.EditBar/css/…`,
`Connectors/GoogleAnalytics4/…`, `Components/Portals/portal.template.xsd`, `App_Data/Upgrade/upgrade.json`.

⇒ Le webroot ne contient pas seulement des *pages* non tracées : il contient, **non tracé**, l'inventaire
d'une application déployée. C'est la signature d'un **site vivant posé sur un working tree**, pas d'un
checkout de développeur.

### 4.3 — Quatre millésimes, mesurés sur disque

Les 343 chemins de l'archive ont été confrontés au disque actuel :

| sort | nombre | lecture |
|---|---:|---|
| **absents** aujourd'hui | **258** | retirés, jamais remis |
| présents, mtime `2026-07-02 01:21` | 63 | millésime **distinct** (voir §6) |
| présents, mtime `2026-08-28 10:00` | **17** | **exactement les artefacts servis** |
| présents, `2026-05-05 20:43` / `2026-09-01` / `2026-09-02` | 3 / 1 / 1 | résiduels |

Le bucket **17** est décisif : `git archive <commit> | tar -x` estampille chaque fichier à la **date du
commit**. Le commit `fcc73d52` porte `1787904017` = **2026-08-28 08:00:17Z** = 10:00:17 locale. Les 17
fichiers portent `10:00:17` **à la seconde**. Aucune autre opération ne produit cette valeur : ils ont été
matérialisés **depuis l'archive de stash**.

> Les 63 fichiers à `2026-07-02 01:21` ne s'expliquent **pas** par cette restauration (une restauration
> depuis la même archive les aurait estampillés `10:00:17`). Leur provenance est un **second millésime**,
> non établi par cette mesure — je le signale comme tel plutôt que de le rattacher au stash.

### 4.4 — L'incident, séquencé à la seconde

| instant (local) | événement | preuve |
|---|---|---|
| **28/08 10:00:17** | `git stash -u` exécuté **dans le webroot** ; commit `fcc73d52`, sujet *« wip-ai-01-pre-merge-1212 »* | `git cat-file -p` + reflog |
| **28/08 10:00:17** | les 17 artefacts servis sont **retirés du disque** et capturés dans `5a086dfe` | mtime disque = date du commit |
| **28/08 10:00:23** | 63 DLL plateforme réécrites dans `bin/` (dont `DotNetNuke.dll` 9.11.1) | #1244 (IISManagement + ai-01) |
| **28/08 10:00:17Z** | préprod renvoie `Last-Modified: Fri, 28 Aug 2026 08:00:17 GMT` sur les 16 wrappers **et** l'OWL | grain ① |
| **12/09** | les 17 artefacts sont restaurés **depuis l'archive** | mtime préservé (§4.3) |
| **12/09 23:16Z** | prod redéploie la couche `origin/master` (fenêtre de 17 s) | grain ① |
| **14/09** | préprod sert toujours `de763aa9`, `_ext` en 404 ×16 | grain ① |

L'ordre **stash (10:00:17) → réécriture binaires (10:00:23)** est celui d'un nettoyage pré-merge suivi d'un
déploiement. Le nettoyage a emporté la couche servie ; le déploiement a downgradé la plateforme. **Deux
dommages, six secondes d'écart, un seul répertoire.**

---

## §5 — Le working tree est un site vivant, pas un checkout propre

`git status --porcelain -- DNNPlatform/` → **314 entrées** :

| code | nombre | nature |
|---|---:|---|
| `M` | 212 | fichiers tracés **modifiés** sur disque |
| `??` | 87 | non tracés (dont **17** artefacts servis, **62** sous `bin/`) |
| `D` | 15 | fichiers tracés **absents** du disque |

Les 15 suppressions sont éloquentes : `App_Data/imageflow_hybrid_cache/db/*/*.metastore` (13),
`.well-known/acme-challenge/web.config`, `Portals/1/NBSTemp/wFvqddGx2k1k`. Ce sont des **caches et fichiers
transitoires d'exécution que le dépôt traque**. Un site vivant les supprime et les régénère ; git les voit
comme des régressions.

⇒ **227 fichiers tracés divergent de `HEAD`.** Le webroot est donc un checkout *sale*, et sa saleté est
celle d'une application en service. C'est cette propriété qui rend le (a)/(b)/(c) coûteux : le dépôt ne
décrit pas seulement des sources, il décrit **aussi** l'état d'un site en marche.

---

## §6 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **Cap 0** | le harnais disque reproduit deux valeurs obtenues par HTTP au grain ① | 2 361 730 o ✅ · `de763aa9` ✅ · `258a9e43…` ✅ |
| **négatif** | le harnais doit discriminer deux couches | `fallacies_fr` disque `3b6b0f14…` ≠ blob `HEAD` `510a32ce…` ✅ |
| **positif** | un fichier non divergent doit ressortir identique | disque = blob `de763aa9` sur les 16 wrappers ✅ |
| **prédictif** *(le plus fort)* | **l'inventaire disque doit prédire l'inventaire HTTP** | 16 wrappers présents ⇔ 200 préprod · `_ext` absents ⇔ **404 ×16** ✅ — **32/32 emplacements** |
| **discriminant d'hôte** | le disque doit correspondre à **préprod**, pas à prod | contenu = couche `de763aa9` = préprod ; prod sert `HEAD` (`510a32ce…`) ⇒ **le disque est le webroot préprod, pas prod** ✅ |

Le contrôle prédictif est celui qui porte : le système de fichiers *annonce* le comportement du site, sans
requête. Un arbre qui prédit 32 routes sur 32 est l'arbre que le site sert.

---

## §7 — Ce que je n'établis PAS

- **Le chemin physique IIS.** `Get-Website` exige l'élévation (non prise). L'identité disque↔webroot est
  établie par **octets, mtime et inventaire prédictif** — pas par la configuration.
- **L'auteur et l'intention du `stash`.** Le sujet *« wip-ai-01-pre-merge-1212 »* **nomme** une opération
  ai-01 ; je le cite comme libellé de commit, je n'en déduis pas une intention.
- **La provenance des 63 fichiers à `2026-07-02 01:21`** (§4.3) : mesurée, non rattachée.
- **Le contenu des 212 modifications tracées** : comptées, non diffé.
- **Les 258 chemins disparus** : je constate l'absence, je n'établis pas *quand* ni *par quoi* chacun a
  disparu (le stash les a retirés le 28/08 ; leur non-retour est postérieur).
- **La sûreté du (a).** Le préalable de #1244 — « aucun déploiement en service ne tire ses binaires de
  `DNNPlatform/bin/` » — **n'est pas levé ici**. Ce dossier ne mesure pas les chemins de déploiement ; il
  mesure que le webroot *est* un working tree, ce qui est la condition du risque, pas sa preuve d'occurrence.
- **`fallacies_cards_fr*`** : hors périmètre (item idle du grain ④, non tranché).

---

## §8 — Ce que ce dossier donne à l'arbitrage (sans trancher)

| branche | ce que la mesure change |
|---|---|
| **(a)** dé-tracker `DNNPlatform/bin/` | sans objet sur le risque principal : les artefacts servis ne sont **pas** dans `bin/`, ils sont **non tracés à la racine du webroot**. (a) retire 195 fichiers de l'index ; il ne rend pas le webroot moins « checkout ». Le préalable de #1244 reste entier. |
| **(b)** committer le bin 10.3.2 | ne protège pas non plus les artefacts servis : ils resteraient non tracés. |
| **(c)** garde de déploiement | c'est la seule branche que la chronologie du §4.4 éclaire **directement** : le dommage du 28/08 n'est pas venu d'un déploiement de `bin/` seul, mais d'une opération **git** (`stash -u`) exécutée dans le webroot, six secondes avant la réécriture des DLL. Une garde qui ne surveille que `DotNetNuke.dll` n'aurait pas vu passer le premier dommage. |

**Constat qui vaut pour les trois branches** : le point de commande n'est pas *`DNNPlatform/bin/`* mais
**`D:/Dev/Argumentum`** — le working tree entier, dont le webroot est un sous-arbre. Tant que git opère là,
aucune des trois branches ne couvre la classe d'incident décrite au §4.4.

---

*master `21a72385` (relevé local `ae84c91c`) · lecture seule : `ls-files`, `ls-tree`, `cat-file`, `rev-list`,
`for-each-ref`, `status`, `stat` · aucun stash, aucun checkout, aucun clean, aucune régénération ·
(a)/(b)/(c) **non tranchés** · verdict visuel : ai-01.*
