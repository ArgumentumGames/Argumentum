# #1244 — Rendre les branches (a) / (b) / (c) décidables

**Auteur** : po-2023 (worker) · **Date** : 2026-09-13 · **Base** : master `35acac04`
**Statut** : **INSTRUCTION** — mesures en lecture seule, **aucune branche tranchée**.
**Reproductibilité** : toutes les mesures de ce document sont des commandes git/HTTP en lecture seule, citées inline.
**Périmètre** : les **195 fichiers TRACÉS** de `DNNPlatform/bin/`. Le stash `5a086dfe` (binaires **non suivis**) est un objet **distinct**, déjà tranché le 13/09 (« GDrive, hors dépôt ») et déjà exécuté — voir grain ①.

---

## §0 — Ce que ce document est, et ce qu'il n'est pas

Il **instruit** : pour chaque branche, le coût concret et la conséquence si elle n'est pas tranchée.
Il **ne tranche pas** : la décision est à l'owner. La recommandation (a) portée par le corps de l'issue reste une recommandation.

⛔ Aucun geste git écrivant n'a été exécuté sur `DNNPlatform/bin/`. ⛔ Aucun dépôt dans un webroot. ⛔ Rien poussé dans le dépôt hors ce document.

---

## §1 — L'état mesuré (première main, `35acac04`)

### 1.1 Le jeu tracé

```
git ls-files DNNPlatform/bin/ | wc -l                    → 195
```

| | nb | détail |
|---|---:|---|
| fichiers tracés | **195** | = **155** à la racine de `bin/` + **40** dans 5 sous-répertoires (`2sxc` 15, `Imageflow` 12, `Providers` 4, `roslyn` 2, `runtimes` 2) |
| dont DLL | **179** | 146 racine + 15 `2sxc` + 12 `Imageflow` + 4 `Providers` + 2 `runtimes` |
| reste | 16 | 9 `.config`, 3 `.targets`, 3 `.json`, 1 `.xml` |
| poids au worktree | **49 356 814 o ≈ 47,1 Mo** | `git ls-files -z … \| xargs -0 du -cb` |

### 1.2 🔴 L'index et le site vivant **désaccordent déjà**

| | blob/état | taille |
|---|---|---|
| `DotNetNuke.dll` **tracé** | `d632e1fc44f842c48b9fcaf7e69d01a7c8f1da9b` | **2 801 152 o** = **9.11.1** |
| `DotNetNuke.dll` **sur disque** | worktree, mtime 02/07 01:21 | **2 949 632 o** = **10.3.2** |

```
git status --porcelain -- DNNPlatform/bin/   → 140 lignes
  décomposition :  78 «  M » (tracés, disque ≠ index)  +  62 « ?? » (non tracés)
git diff --name-only -- DNNPlatform/bin/     → 78   (dont 71 DLL)
```

Les **62 non tracés** (12,8 Mo) sont nommément la pile **2sxc 21.07 / BouncyCastle 2.6.2 / DNN 10.3.2** :
`ToSic.Sxc.*` (22), `ToSic.Eav.*` (10), `ToSic.Sys.*` (7), `BouncyCastle.Cryptography.dll`,
`DotNetNuke.ContentSecurityPolicy.dll`, `DotNetNuke.Web.Client.ResourceManager.dll`,
`AngleSharp*`, `HtmlSanitizer.dll`, `Microsoft.IdentityModel.*`, `Microsoft.Identity.Client.dll`,
`System.Formats.Asn1.dll`, `roslyn/…`.

⇒ **Ces 62 fichiers sont la « combinaison » que la décision du 13/09 désigne comme la valeur** (DNN 10.3.2 + 2sxc 21.07 + BouncyCastle 2.6.2 telle qu'elle tournait).

### 1.3 Corroboration indépendante du « même runtime »

| instrument | valeur |
|---|---|
| poids du `bin/` **sur disque** (runtime 10.3.2 vivant) | **81,3 Mo** |
| poids de `4b0297ee` (instantané canonique, **mesuré par ai-01**) | **81,1 Mo** |

Deux instruments, deux machines : même ordre de grandeur. **Ce n'est pas une identité** — elle se prouverait en comparant les DLL, jamais des totaux. C'est une corroboration que les deux jeux ont la même **composition**.

### 1.4 `DNNPlatform/bin/` est la **dernière** zone binaire tracée du dépôt

```
git ls-files | grep -icE '\.(dll|pdb|exe)$'   → 180
  dont DNNPlatform/bin/ : 179   (99,4 %)
       DNNPlatform/App_Data/FipsCompilanceAssemblies : 1
```

⇒ La Phase 1 de #415 (PR **#416** `94b43712`, PR **#501** `ff031470`) a dé-tracé les autres zones régénérables ; **celle-ci est restée**. (a) la **termine**, elle n'innove pas.

*Contexte de poids, flaggé et non expliqué ici* : `git count-objects -vH` → `size-pack` = **5,57 GiB** aujourd'hui, contre **2,05 GiB** mesuré par `docs/repo/415-git-weight-audit.md` le 01/07. Écart hors périmètre de ce document ; **non instruit**.

---

## §2 — La prémisse de (a) est MESURÉE, et plus forte qu'énoncée

Le corps de l'issue pose : *« (a) demande une vérification préalable : établir qu'aucun déploiement en service ne tire ses binaires de `DNNPlatform/bin/`. »*

**Mesuré — le webroot préprod vivant EST ce working tree.** Preuve appariée (disque vs servi) :

| côté | taille | sha256 |
|---|---:|---|
| disque `DNNPlatform/Resources/libraries/jQuery/03_07_01/jquery.js` | 87 535 o | `7aa6b0e08f48a0f95d8df7ea89e4cbfe1ef3d1e8c0f7373f7f25edfb4e4a325e` |
| servi `GET https://dnn.argumentum.myia.io/Resources/libraries/jQuery/03_07_01/jquery.js?cb=…` | 87 535 o · HTTP **200** · `application/javascript` | `7aa6b0e08f48a0f95d8df7ea89e4cbfe1ef3d1e8c0f7373f7f25edfb4e4a325e` |

Égalité à l'octet. Corroboré par le dépôt lui-même (`.gitignore` l.141 : *« DNNPlatform/ is a LIVE webroot (préprod dnn.argumentum.myia.io, IIS id:52) »*) et par le défaut du seul script de repair (`docs/dnn/repair-bin-net48.ps1` l.38 : `$BinRoot = 'D:\Dev\Argumentum\DNNPlatform\bin'`).

**Conséquence, en deux temps :**

1. **`git rm --cached` est purement index.** Il ne retire aucun octet du disque. La préprod **ne peut pas** perdre son `bin/` par (a) : elle tourne sur du contenu qui **diffère déjà** de l'index sur 140 chemins. ⇒ **La vérification préalable que (a) exigeait est satisfaite**, sur un mode plus direct que demandé (on ne vérifie pas qu'un déploiement *consulte* le dépôt ; on mesure que le webroot **est** le worktree, et que son `bin/` a déjà divorcé de l'index).

2. ⚠️ **Mais la mesure nomme aussi ce que (a) NE corrige PAS.** Le webroot **est un working tree git** — et c'est là le **vecteur** de toute la classe d'incidents 28/08 :

   | incident | ce que le vecteur a fait |
   |---|---|
   | 28/08 10:00 | déploiement → `Default.aspx` régressé en variante 9.11.1 (NRE site-wide, cf. commentaires de cette issue) |
   | 10/08 01:13 | `git reset/checkout/pull` sur le worktree → `web.config` (97 Ko déployé vs 79 Ko tracé) écrasé, site tué |
   | 28/08 | `git stash -u` a emporté 16 wrappers + OWL (3ᵉ incident de la classe) |

   **(a) retire l'*appât* (des blobs 9.11.1 tracés, qu'un `restore-from-HEAD` ressert), pas le *vecteur* (le webroot est un checkout).** Une décision qui ne prendrait que (a) laisserait la classe ouverte pour tout autre fichier tracé divergent.

---

## §3 — Détail par branche : coût, effet sur un déploiement futur, conséquence si non tranchée

### (a) Dé-tracker `DNNPlatform/bin/`

**Geste exact, mesuré** — ce **n'est pas** une ligne de `.gitignore`. Le dossier est **ré-inclus par négation** (`.gitignore` l.152-158) :

```
/DNNPlatform/web.config
# Bin DNN (exception pour inclure ce dossier)
!/DNNPlatform/bin/
# … the directory re-include above does NOT override the *.dll/*.pdb/*.exe rules (L19-21)
!/DNNPlatform/bin/*.dll
!/DNNPlatform/bin/*.pdb
!/DNNPlatform/bin/*.exe
```

⇒ (a) = **retirer ce bloc de négation** *puis* `git rm --cached` sur les 195 chemins. Deux gestes, dont un que l'issue ne mentionne pas.

| | mesure |
|---|---|
| **Poids dépôt** | **−47,1 Mo au checkout HEAD** (778 Mo → ≈731 Mo, −6 %) · **0 sur le pack** — c'est établi par `docs/repo/415-git-weight-audit.md` §TL;DR : *« Phase 1 does not reduce the pack (it only stops re-adding at HEAD) »*. **Le clone ne maigrit pas.** |
| **Licences tierces redistribuées** | **aucune ajoutée — (a) en retire.** Les 179 DLL sont **déjà** publiques aujourd'hui ; (a) cesse de les redistribuer. |
| **Effet sur un déploiement futur** | Le dépôt cesse d'être une source de 9.11.1. Un déploiement doit alors sourcer son `bin/` **ailleurs** — la route nommée et déjà approuvée par l'owner le 13/09 est **hors dépôt (GDrive)**, celle-là même du grain ①. |
| **⚠️ Ce qu'il faut mesurer avant** | Que ce « ailleurs » soit **nommé et opérationnel** avant le geste, sinon (a) remplace un appât par un vide. Le grain ① a déposé 128 fichiers (16,5 Mo) ; `4b0297ee` (330 fichiers) est sur ai-01 et **inatteignable d'ici**. |
| **Conséquence si non tranchée** | `master` continue d'offrir des blobs **9.11.1** comme source de déploiement. Tout `restore-from-HEAD` ou clone-frais-déployé **ré-arme exactement le 500 du 28/08**. |

**Précédent fusionné** : le geste est **déjà établi dans ce dépôt**, y compris sur du contenu 2sxc — PR **#416** (`94b43712`, *stop tracking 968 MB of regenerable build artifacts*) et PR **#501** (`ff031470`, *#415 untrack 10.5 MB 2sxc install module + gitignore*). (a) n'est pas une nouveauté risquée ; c'est la **dernière case** d'un nettoyage commencé.

### (b) Committer le `bin/` 10.3.2

| | mesure |
|---|---|
| **Poids** | Référence ai-01 : `4b0297ee` = **330 fichiers / 255 DLL / 81,1 Mo**. Delta **mesuré sur le webroot vivant** vs le jeu tracé actuel : **78 remplacements + 62 ajouts (12,8 Mo)**. Le reste de l'écart (330 vs 195) tient aux fichiers que le repo ne porte pas du tout. |
| **Licences — l'argument est plus faible qu'il n'y paraît** | Les 62 ajouts sont des familles **permissives** : 2sxc/ToSIC (MIT), `BouncyCastle.Cryptography` (MIT), DNN Platform (MIT), AngleSharp (MIT), HtmlSanitizer (MIT), `Microsoft.Identity.*` (MIT). |
| **⚠️ Mais une exposition existe DÉJÀ, et (b) ne la crée pas** | Le dépôt traque **déjà** `ImageResizer.dll` + `ImageResizer.Plugins.DiskCache.dll` + `ImageResizer.Plugins.WebP.dll` (éditeur **Imazen**, AGPL-3.0 / commercial dual selon l'éditeur). **Je n'ai pas lu les textes de licence** — à vérifier avant toute décision. Le point de méthode : l'objection « ne pas mettre de DLL tierces dans un dépôt public » vise un état **déjà** en place (179 DLL tracées), pas ce que (b) ajouterait. |
| **Effet sur un déploiement futur** | Le dépôt devient une source cohérente — **mais une cible mouvante** : il faudrait re-synchroniser le tracé à chaque churn du `bin/` vivant. Or ce churn est **mesuré à 78 fichiers en quelques semaines**. (b) achète la cohérence au prix d'une maintenance récurrente. |
| **Conséquence si non tranchée** | Le vecteur de downgrade reste armé. Et (b) **n'est pas un fix stable** de toute façon : il re-diverge. |

**Argument versé au dossier, pas décision** : la décision owner du 13/09 a **refusé** la réintégration du stash (~60 DLL) au motif « dépôt PUBLIC + DLL tierces ». Ce motif s'applique mot pour mot à (b). **Cohérence à trancher par l'owner, pas par moi.**

### (c) Garder tel quel + garde de déploiement

| | mesure |
|---|---|
| **Coût** | Nul sur le dépôt. La garde vit dans le script de déploiement. |
| **🔴 Finding — la garde n'a pas de domicile** | Les **seuls** scripts du dépôt référençant `DNNPlatform[\\/]bin` sont : `docs/dnn/repair-bin-net48.ps1` (qui se déclare lui-même **SUPERSEDED et INVERTED** pour 2sxc-21, l.60) et `tmp/dnn-restore-bin/restore-bin-surgical.ps1` (artefact `tmp/`). **Il n'existe pas de script de déploiement canonique dans le dépôt où poser la garde.** (c) présuppose un chemin que le dépôt ne nomme pas. |
| **Effet sur un déploiement futur** | Protège **uniquement** les chemins instrumentés. |
| **Conséquence si non tranchée** | Le piège reste armé pour **tout** chemin non instrumenté — c'est-à-dire, en l'état, **tous**. (c) seul ne ferme rien tant que le domicile de la garde n'est pas nommé. |

---

## §4 — L'organe (commun aux trois branches)

L'issue demande : *« si `DNNPlatform/bin/DotNetNuke.dll` est tracé, sa version doit être celle que la base attend »*.

⚠️ **Calibrage** : aujourd'hui il **est** tracé **et** en 9.11.1 alors que la base est en 10.3.2 ⇒ **ce test serait ROUGE dès son introduction**. Il ne peut donc pas être posé comme une garde verte : c'est un **détecteur** dont l'état initial connu est l'échec, et il ne passera au vert que si (a) ou (b) est tranchée. Utile à savoir avant de le proposer en CI.

---

## §5 — Ce que ce document n'établit PAS

- **Les textes de licence** des 179 DLL tracées : non lus. La classification par préfixe de nom est un **indice**, pas une preuve d'identité d'éditeur ni de licence.
- **`4b0297ee`** : inatteignable depuis cette machine (objet local à ai-01). Les 330 fichiers / 255 DLL / 81,1 Mo sont **repris d'ai-01**, non re-mesurés ici.
- **Le chemin physique IIS** : `Get-Website` exige l'élévation, non prise. L'identité webroot↔worktree est prouvée par le **sha256 apparié servi/disque** (§2), pas par la configuration IIS.
- **La croissance du pack** (2,05 → 5,57 GiB) : mesurée, **non expliquée**.
- **Le contenu de `4b0297ee` vs le `bin/` vivant** : non comparé DLL à DLL. Les 81,3 / 81,1 Mo sont une **corroboration de composition**, pas une identité.

---

## §6 — Ce qui reste à l'owner

1. **(a) / (b) / (c)** — la décision. Rien n'est tranché ici.
2. Si **(a)** : nommer la source `bin/` hors dépôt d'un futur déploiement **avant** le geste (le grain ① en est la moitié).
3. Une question que le dossier ouvre et que personne n'a posée : **le webroot doit-il rester un working tree git ?** C'est lui qui a produit les trois incidents 28/08 et 10/08, et **aucune** des trois branches ne le traite. (a) réduit la surface, (b) l'augmente, (c) l'ignore.
4. **Cohérence (b) ↔ décision du 13/09** : le motif du refus du stash s'applique à (b).

---

🤖 po-2023 — mesures en lecture seule, base `35acac04`
