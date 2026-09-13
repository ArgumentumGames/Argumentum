# #415 — La croissance du pack expliquée : la paire « 2,05 → 5,57 GiB » n'est pas commensurable

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
**Statut** : **MESURE / DOSSIER** — lecture seule. `0` écriture, `0` réécriture d'historique, `0` `gc`.
**Dispatch** : ai-01, deep-queue #458 c.5656689863, grain **①** (`[primaire] #415`).
**Doc amont** : [`415-git-weight-audit.md`](415-git-weight-audit.md) (po-2024, 2026-07-01, base `18b4d023`).

---

## §0 — Deux résultats

**A. Le chiffre de croissance annoncé n'existe pas ici.** « 2,05 → 5,57 GiB » compare **deux clones
différents**, et le second terme n'est pas mesurable sur ce dépôt. Sur le clone que l'audit #415 a
mesuré, la même commande donne aujourd'hui **2,22 GiB**.

| terme | valeur | source | clone |
|---|---|---|---|
| « 2,05 GiB » | **2,050 GiB** | `415-git-weight-audit.md` §Current measurements, 01/07, base `18b4d023` | **ce clone** (C:) |
| « 5,57 GiB » | **5,57 GiB** | `docs/dnn/1244-bin-branches-abc-instruction.md` l.74, **auteur po-2023**, 13/09, base `35acac04` | clone **po-2023** |
| aujourd'hui, même commande | **2,223 GiB** | mesuré ce jour, base `21a72385` | **ce clone** (C:) |

**B. La croissance réelle de ce clone — +176,7 MiB en 10 semaines — est intégralement attribuée
(§3–§7)**, et son moteur n'est pas celui qu'on attendait : **des artefacts régénérables re-commités
en série**, dont la famille de mindmaps que la Phase 1 de #415 avait délibérément laissée tracée.
Au passage, **l'invariant « 0 nouveau blob > 2 Mo » que l'audit a ré-affirmé deux fois est
aujourd'hui faux** (§4).

---

## §1 — Ce clone **est** celui de l'audit (preuve par identité, pas par ressemblance)

L'audit du 01/07 annonce « **`size-pack` = 2.05 GiB (3 packs, 25 857 objects)** ». Les trois packs
présents dans ce dépôt et antérieurs à cette date reproduisent **exactement** ces deux nombres :

| pack | octets | objets |
|---|---:|---:|
| `pack-b7666468…` | 2 168 387 273 | 25 255 |
| `pack-7c8bf30c…` | 17 609 443 | 447 |
| `pack-9bd61e93…` | 15 483 091 | 155 |
| **somme** | **2 201 479 807** = **2,050 GiB** | **25 857** |

Les deux nombres tombent juste à l'unité — c'est le **même clone**, la ligne de base est donc
utilisable telle quelle. (`git merge-base --is-ancestor 18b4d023 HEAD` → vrai, la plage est valide.)

## §2 — La trajectoire réelle

```
2026-07-01  size-pack 2,050 GiB   3 packs   25 857 objets          ← audit #415
2026-09-14  size-pack 2,223 GiB  22 packs   44 726 objets (+1 225 loose = 123,65 MiB)
            ────────────────────────────────────────────────────
            croissance   +0,173 GiB = +176,7 MiB   sur 513 commits (18b4d023..origin/master)
```

Les 19 packs créés depuis l'audit totalisent **185 333 133 B = 176,7 MiB** ; l'écart avec la
croissance de `size-pack` est de 608 B (19 × 32 B d'en-tête/trailer) — l'instrument ferme.
`.git` complet = 2,4 G. **Aucun `alternates`** (`.git/objects/info/` ne contient que
`commit-graphs/` et `packs`) : l'écart inter-machines du §8 n'est pas un montage d'objets.

## §3 — Familles responsables, par `verify-pack`

Méthode : `git verify-pack -v` sur les 22 index ; sont « neufs » les objets absents des 3 packs de
§1 ; chacun est rattaché à son chemin par `git rev-list --objects --all`. Contribution mesurée =
**colonne `size-in-packfile`** (l'octet réellement stocké), jamais la taille décompressée.

| famille | objets | MiB in-pack |
|---|---:|---:|
| `Cards/Fallacies` | 874 | **37,74** |
| *(inatteignables — sans chemin)* | 7 025 | **32,97** |
| `DNNPlatform` | 1 169 | **31,06** |
| `tmp/` | 88 | **17,28** |
| `docs/ontology` | 55 | 1,84 |
| *(arbres, sans blob)* | 1 822 | 1,49 |
| `docs/investigations` | 145 | 1,39 |
| `docs/taxonomy` | 330 | 0,98 |
| `Generation/Converters` | 1 156 | 0,97 |
| `qa` | 5 | 0,82 |
| `Cards/Scenarii` | 38 | 0,63 |
| `docs/dnn-localization` | 119 | 0,31 |
| `tools` | 119 | 0,23 |
| `Cards/Rules` | 56 | 0,18 |
| **total** | **14 070** | **128,9** |

⚠️ **Deux pièges de lecture — les deux sont le sujet.**

1. **Deux totaux, et ils ne mesurent pas la même chose.** Les 19 packs pèsent **176,7 MiB** sur
   disque mais ne portent que **128,9 MiB d'objets distincts** : **47,9 MiB sont le même objet
   packagé plusieurs fois** (22 packs jamais consolidés). 176,7 = coût de *stockage* ;
   128,9 = contenu *acquis*.
2. **`Cards/Fallacies` n'est pas un binaire.** C'est du texte — CSV de taxonomie, SVG et wrappers
   HTML de mindmaps — et il coûte plus cher que toutes les DLL réunies (§6).

## §4 — L'invariant de l'audit est **rompu**

L'audit #415 conclut, et sa note de suivi du 06/07 puis du 12/07 le ré-affirme : *« 0 nouveau blob
> 2 Mo commité »*. **Mesure ce jour, sur la même plage :**

| | valeur |
|---|---:|
| blobs **> 2 MiB** rendus atteignables depuis `master` depuis l'audit | **341** |
| contenu cumulé de ces 341 blobs (non compressé) | **1 082,6 MiB** |

Le détail par chemin (§6) montre que ce ne sont pas des binaires oubliés mais **des artefacts
régénérables re-commités en série**. La ligne de l'audit qui classait les SVG de mindmaps
« text XML (**< 2 MB**) » ne décrit plus la réalité : au HEAD, `docs/ontology/argumentum.owl` pèse
**5,71 MiB**, `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.html` **5,0 MiB**, `…/ru/Fallacies_ru.svg`
**2,9 MiB**.

⇒ Conséquence pratique : l'invariant s'était dégradé **sans qu'aucune CI ne le voie** — la garde
`large-blob-guard.yml` existe et tourne, mais elle ne s'applique qu'aux **ajouts** de la PR courante,
pas à la ré-accumulation par re-commit de fichiers déjà tracés. C'est ce trou qui laisse passer
la famille ci-dessous.

## §5 — Fenêtre : 4 événements, pas une dérive continue

Datation par le mtime des fichiers `.pack` — l'événement de *packaging*, distinct de la date
d'*auteur* des commits (cf. §7) :

| date | pack | MiB |
|---|---|---:|
| 2026-07-02 | `pack-7ec2a455…` | **36,4** |
| 25/07 → 24/08 | 11 packs | 3,1 *(cumul)* |
| 2026-08-30 | `pack-4d5e7da0…` | **67,7** |
| 2026-09-01 | `pack-f167c6b9…` + `eec09f42…` | 16,7 |
| 2026-09-03 | `pack-6e0710d8…` + `9c17ebf0…` | 12,8 |
| 2026-09-12 | `loose-fb3ab40a…` | **39,6** |
| **total** | **19 packs** | **176,7** |

Fenêtre de commits correspondante sur `master` : **513 commits**, du **2026-07-01** au **2026-09-14**.
Par famille : `Cards/Fallacies` 82 commits (02/07→11/09) · `DNNPlatform` 47 (01/07→30/08) ·
`docs/ontology` 18 (02/07→13/09).

## §6 — Le moteur réel : des artefacts régénérables re-commités

Top des chemins par contenu introduit sur `master` dans la fenêtre (versions > 2 MiB) :

| chemin | versions | MiB contenu |
|---|---:|---:|
| `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | **37** | 145,0 |
| `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.html` | 15 | 73,2 |
| `docs/ontology/argumentum.owl` | 8 | 47,5 |
| `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.content.svg` | 9 | 45,3 |
| `Cards/Fallacies/Mindmaps/fr/Argumentum_Fallacies_MindMap_cards_fr.svg` | 9 | 41,8 |
| `Cards/Fallacies/Mindmaps/ru/Fallacies_ru.html` | 15 | 39,6 |
| `Cards/Fallacies/Mindmaps/pt/Fallacies_pt.html` | 16 | 38,3 |
| `Cards/Fallacies/Mindmaps/ar/Fallacies_ar.html` | 16 | 38,3 |
| `Cards/Fallacies/Mindmaps/{fa,fr,es,en}/Fallacies_*.html` | 15 chacun | 33–37 chacun |
| `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.svg` / `.links.svg` | 6 / 7 | 33,6 / 31,2 |
| `Cards/Fallacies/Mindmaps/*/Fallacies_*.content.svg` (ru/pt/ar/en) | 10–11 | 25–29 chacun |

Deux lectures, toutes deux importantes :

- **Le coût en pack est très inférieur au contenu.** Ces ~1 083 MiB de contenu ne coûtent que
  **39,6 MiB** de pack (`Cards/Fallacies` 37,74 + `docs/ontology` 1,84) : des versions successives
  quasi identiques sont des deltas quasi parfaits. **Un dossier qui chiffrerait la croissance par
  la somme des tailles de fichiers se tromperait d'un facteur ~27.**
- **La famille est exactement celle que la Phase 1 avait laissée tracée.** L'audit note que les
  SVG de mindmaps « were not covered by the Phase-1 untrack » et les laisse « the conservative
  choice », en les supposant < 2 MB. Ils ont depuis dépassé 2 MB et sont re-commités 6 à 16 fois
  par langue sur la fenêtre.

## §7 — Qualification : nouvelle, ou préexistante re-fetchée ?

**Les deux — et la date de commit ne date pas la croissance.**

- **Nouvelle** : l'activité propre de `master` (513 commits) — §6.
- **Préexistante, apportée après coup** : `tmp/` (**17,28 MiB**) vient de
  `origin/validation/regen-dc01445f`, dont les commits sont datés du **2026-06-08** — *avant*
  l'audit. Ces objets n'étaient pas dans ce clone au 01/07 ; ils sont entrés par un **fetch
  ultérieur**. Même mécanisme pour `Chatgpt-plugin` (`origin/semantic-kernel`), dont le jar Tweety
  de 19,92 MiB vit dans le pack de §1, donc antérieur.
- **Inatteignable** : 33,0 MiB (7 025 objets) sans chemin — résidus de refs supprimées ou
  réécrites. Ni nouveau utile, ni préexistant utile : **du déchet**.

Répartition par classe de ref sur les mêmes 128,9 MiB :

| classe | objets | MiB |
|---|---:|---:|
| atteignables depuis `origin/master` | 5 629 | **65,63** |
| atteignables depuis d'**autres refs** seulement | 1 416 | **30,29** |
| **inatteignables** (dangling) | 7 025 | **32,97** |

⇒ **63,26 MiB sur 128,9 (49 %)** ne viennent pas de `master`.

## §8 — Contrôle inverse

Le contrôle demandé — « attribuer **au moins** le delta observé » — ferme exactement :

```
objets distincts des 19 packs neufs ................. 128,9 MiB
duplication intra-packs neufs (même objet
  packagé plusieurs fois) ........................... +47,9 MiB
                                                      ─────────
                                                      176,8 MiB   vs   176,7 MiB observés   ✅
```

L'attribution **sature** la croissance. Le §7 porte sur le même ensemble de 14 070 objets.

## §9 — Le « 5,57 GiB » : ce qui est établi, ce qui ne l'est pas

**Établi.** Source unique et citée : `docs/dnn/1244-bin-branches-abc-instruction.md` l.74
(« `git count-objects -vH` → `size-pack` = **5,57 GiB** aujourd'hui, contre **2,05 GiB** mesuré par
`docs/repo/415-git-weight-audit.md` le 01/07 »). Ce document est **de po-2023** (en-tête : « Auteur :
po-2023 (worker) · 2026-09-13 · base `35acac04` »), et marque lui-même l'écart « hors périmètre de
ce document ; **non instruit** » (l.75, l.171).

**Établi.** Sur ce clone, la commande citée donne **2,223 GiB** — pas 5,57.

**Non établi, et non établissable d'ici :** *pourquoi* le clone de po-2023 mesurerait ~3,3 GiB de
plus. Deux hypothèses restent ouvertes ; ce dossier ne tranche pas.

1. **Accumulation locale** : le clone de po-2023 porte des refs et objets **jamais poussés** — le
   rapport #1244 du même auteur documente `refs/stash-archive/arg/9` (`fcc73d52`), le commit
   *untracked* `5a086dfe` (343 chemins d'une installation DNN) et un OWL servi (`af9e8f38`)
   « atteignable uniquement depuis `5a086dfe` ». Ici, `refs/stash-archive` ne porte que `arg/00`
   et `arg/01`. **Un pack local n'est pas un pack de dépôt.**
2. **Effet d'instrument** : `size-pack` **somme le poids disque de chaque pack**, redondants
   compris — il ne mesure pas l'ensemble d'objets distincts. Ce clone le démontre chiffré au §3
   (176,7 MiB de packs pour 128,9 MiB de contenu).

**Action demandée à po-2023** (une commande, non destructive) : re-lancer sur **son** clone et
publier les quatre lignes de `git count-objects -vH` — `count`, `in-pack`, **`packs`**,
`size-pack` — **plus** `git for-each-ref | wc -l`. `packs` et `in-pack` discriminent : beaucoup de
packs pour peu d'objets distincts ⇒ (2) ; peu de packs et beaucoup d'objets absents d'ici ⇒ (1).

## §10 — Résiduels et pistes non destructives (non exécutées)

1. **33,0 MiB d'objets inatteignables** sont éligibles à `git gc --prune=now` — non destructif pour
   l'historique. Non exécuté : décision owner.
2. **47,9 MiB de duplication** entre les 22 packs se résorberaient par une consolidation
   (`git repack -a`), sans toucher à l'historique. Non exécuté, même raison.
3. **Le fetch des branches mortes** est le premier poste non-`master` (30,3 MiB).
   `git remote prune origin` est non destructif mais ne récupère pas les objets déjà téléchargés,
   d'où l'item 1.
4. **Le vrai levier n'est pas la réécriture d'historique mais le re-commit.** §6 : trois familles
   régénérables (`Mindmaps/**` SVG+HTML, `argumentum.owl`, la taxonomie CSV) ont produit 341 blobs
   > 2 MiB en 10 semaines. Une garde qui compare le blob entrant à **son prédécesseur dans
   l'historique** — et non aux seuls ajouts de la PR — fermerait ce trou sans toucher à l'existant.
   C'est une proposition, pas un geste : la règle « ne pas re-committer ce qu'on peut régénérer »
   relève de l'owner.
5. **Phase 2 (`filter-repo`)** : inchangée, toujours gatée sur GO jsboige ; **ce dossier ne la
   déclenche pas**. Le poids historique reste ~2,05 GiB (§1) : la croissance de 176,7 MiB **ne
   change pas la conclusion de l'audit** — le clone ne s'allège que par réécriture.
6. **Le chiffre du dossier d'arbitrage** (2,05 → 5,57) devrait être corrigé ou annoté dans
   `docs/dnn/1244-bin-branches-abc-instruction.md` — **je ne l'ai pas fait** : ce document est de
   po-2023, sa correction lui revient, et la ligne est déjà marquée « non instruite ». Signalé.

## §11 — Gardes tenues

⛔ Aucune réécriture d'historique · ⛔ aucun `gc` · ⛔ aucun `repack` · ⛔ aucun `remote prune` ·
⛔ aucun CSV touché. Instruments : `git count-objects`, `git verify-pack -v`,
`git rev-list --objects`, `git cat-file`, `ls`. Tout est rejouable par les commandes citées inline.

---

*po-2024 — grain ① du dispatch #458 c.5656689863. Le worker mesure et signale ; le verdict et la
décision (Phase 2, `gc`, prune, garde anti-re-commit) restent à l'owner.*
