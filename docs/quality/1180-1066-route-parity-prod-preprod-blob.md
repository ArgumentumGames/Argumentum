# #1180/#1066 — Parité des routes wrappers + OWL : prod × préprod × blob

**Auteur** : po-2023 (worker) · **Date** : 2026-09-14 · **Base** : master `21a72385`
**Nature** : **mesure**, lecture seule. Aucun dépôt, aucune régénération, aucun geste git écrivant sur un webroot.
**Portée** : grain ① de la deep-queue po-2023 (`#458` c.`5656689863`).

---

## §0 — Périmètre : ce que « 35 routes » recouvre

Le dispatch parle de **35 routes wrappers+OWL**. Décompte retenu :

| famille | nombre | variante |
|---|---:|---|
| wrappers de base | 16 | 8 langues × {`fallacies`, `virtues`} |
| wrappers `_ext` | 16 | 8 langues × {`fallacies`, `virtues`} |
| cards FR | 1 | `fallacies_cards_fr.html` |
| OWL | 2 | `argumentum_fallacies.owl`, `argumentum_virtues.owl` |
| **total** | **35** | |

J'ai sondé **36 candidats** : la variante `fallacies_cards_fr_ext.html` a été ajoutée pour trancher
l'ambiguïté (le fichier est committé, la route ne l'était pas dans le décompte) — elle est **hors périmètre
déclaré** et traitée comme telle ci-dessous.

**Mapping chemin committé → chemin servi** (§1, mesuré — jamais deviné) :

| servi | committé |
|---|---|
| `/fallacies_<lang>.html` | `Cards/Fallacies/Mindmaps/<lang>/Fallacies_<lang>.html` |
| `/virtues_<lang>.html` | `Cards/Fallacies/Mindmaps/<lang>/Argumentation_Virtues_<lang>.html` |
| `/<stem>_ext.html` | idem, `…_ext.html` |
| `/fallacies_cards_fr.html` | `Cards/Fallacies/Mindmaps/fr/Fallacies_cards_fr.html` |
| `/argumentum_fallacies.owl` | **aucun chemin committé équivalent** — voir §4 |
| `/argumentum_virtues.owl` | `docs/ontology/argumentum_virtues.owl` |

---

## §1 — Instrument, et son contrôle zéro

- **Sonde** : `curl --compressed` + cache-buster `?cb=`, corps écrit sur disque, `sha256sum` sur les **octets
  reçus** — jamais sur une variable shell (une capture en variable retire le newline terminal et fabrique un
  faux écart).
- **Référence** : `git show origin/master:<chemin>` puis `sha256sum`. `core.autocrlf=false` sur cette machine
  et aucun `text`/`eol` sur les `.html`/`.owl` (`.gitattributes` ne couvre que `*.snapshot.svg` et `*.sh`) ⇒
  disque et blob coïncident : aucune fausse divergence de fin de ligne n'est possible ici.
- **Verdicts** : `IDENTICAL_TO_BLOB` · `DIVERGENT` · `HTTP_404`.

**Cap 0 — l'instrument reproduit deux valeurs déjà connues, obtenues par d'autres moyens** :

| témoin | valeur connue (source) | ce harnais |
|---|---|---|
| préprod `fallacies_fr.html` | 2 361 730 o (couche `de763aa9`, #830) | **2 361 730 o** ✅ |
| prod `fallacies_fr.html` | 2 717 114 o (couche HEAD, handover ai-01) | **2 717 114 o** ✅ |

Les deux hôtes servent donc bien **deux couches différentes**, et le harnais les distingue.

---

## §2 — Table de parité (36 candidats × 2 hôtes × blob)

« Couche antérieure » = octets **identiques** au blob de `de763aa9` (§6, contrôle inverse).

| route | blob `origin/master` (o · sha256¹²) | prod | préprod | verdict |
|---|---|---|---|---|
| `fallacies_fr` | 2717114 · `56a9975d4201` | 2717114 · `56a9975d4201` | 2361730 · `3b6b0f147510` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_en` | 2568470 · `3ec93e94d4ad` | 2568470 · `3ec93e94d4ad` | 2216473 · `857bbe926e2d` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_ru` | 3003724 · `322ff9d3ee4b` | 3003724 · `322ff9d3ee4b` | 2643818 · `07361597c9db` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_pt` | 2714086 · `c15fdaac498c` | 2714086 · `c15fdaac498c` | 2361323 · `995aaa8e59a9` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_es` | 2656860 · `9cf1a09d18dd` | 2656860 · `9cf1a09d18dd` | 2302920 · `9aec30f7d573` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_ar` | 2708977 · `0f7b3d908d30` | 2708977 · `0f7b3d908d30` | 2359846 · `bf1a118314c5` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_fa` | 2815240 · `5a2fdf40524b` | 2815240 · `5a2fdf40524b` | 2458296 · `0055dbca2e9a` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_zh` | 5322382 · `0e32886b11ca` | 5322382 · `0e32886b11ca` | 4968179 · `0c11ccba877e` | **prod = blob ; préprod = couche antérieure** |
| `virtues_fr` | 521917 · `e7895bc080d7` | 521917 · `e7895bc080d7` | 512022 · `86ce620a6713` | **prod = blob ; préprod = couche antérieure** |
| `virtues_en` | 508579 · `9b1b7f8d8a50` | 508579 · `9b1b7f8d8a50` | 498023 · `c0a4a80fb9d8` | **prod = blob ; préprod = couche antérieure** |
| `virtues_ru` | 577313 · `c1cadbc22db3` | 577313 · `c1cadbc22db3` | 566278 · `f8f1a2482dd0` | **prod = blob ; préprod = couche antérieure** |
| `virtues_pt` | 533212 · `2f770ef3ec3d` | 533212 · `2f770ef3ec3d` | 524796 · `aae0cd296174` | **prod = blob ; préprod = couche antérieure** |
| `virtues_es` | 520740 · `7999ce05c583` | 520740 · `7999ce05c583` | 509833 · `bc8d4a6b0db1` | **prod = blob ; préprod = couche antérieure** |
| `virtues_ar` | 551592 · `4c8f225b8f64` | 551592 · `4c8f225b8f64` | 542081 · `95901fc02d9f` | **prod = blob ; préprod = couche antérieure** |
| `virtues_fa` | 552029 · `8decf6f44372` | 552029 · `8decf6f44372` | 542694 · `ed89eb490fb9` | **prod = blob ; préprod = couche antérieure** |
| `virtues_zh` | 1236196 · `ca246a0fb6d7` | 1236196 · `ca246a0fb6d7` | 1225370 · `5d7470063cd5` | **prod = blob ; préprod = couche antérieure** |
| `fallacies_fr_ext` | 91269 · `0caafb9213dc` | 91269 · `0caafb9213dc` | **404** | **prod = blob ; préprod absente** |
| `fallacies_en_ext` | 91269 · `8f8255dd4efe` | 91269 · `8f8255dd4efe` | **404** | **prod = blob ; préprod absente** |
| `fallacies_ru_ext` | 91269 · `3ca5bee31de2` | 91269 · `3ca5bee31de2` | **404** | **prod = blob ; préprod absente** |
| `fallacies_pt_ext` | 91269 · `b9139e2527db` | 91269 · `b9139e2527db` | **404** | **prod = blob ; préprod absente** |
| `fallacies_es_ext` | 91269 · `69df07765a1b` | 91269 · `69df07765a1b` | **404** | **prod = blob ; préprod absente** |
| `fallacies_ar_ext` | 91269 · `6d197291ed52` | 91269 · `6d197291ed52` | **404** | **prod = blob ; préprod absente** |
| `fallacies_fa_ext` | 91269 · `33296fa750d2` | 91269 · `33296fa750d2` | **404** | **prod = blob ; préprod absente** |
| `fallacies_zh_ext` | 91269 · `d6855dcfba7b` | 91269 · `d6855dcfba7b` | **404** | **prod = blob ; préprod absente** |
| `virtues_fr_ext` | 91286 · `406168fa52be` | 91286 · `406168fa52be` | **404** | **prod = blob ; préprod absente** |
| `virtues_en_ext` | 91286 · `ea837db27a90` | 91286 · `ea837db27a90` | **404** | **prod = blob ; préprod absente** |
| `virtues_ru_ext` | 91286 · `c215dff97641` | 91286 · `c215dff97641` | **404** | **prod = blob ; préprod absente** |
| `virtues_pt_ext` | 91286 · `aa67b9ceeed9` | 91286 · `aa67b9ceeed9` | **404** | **prod = blob ; préprod absente** |
| `virtues_es_ext` | 91286 · `e8df8fd72b8e` | 91286 · `e8df8fd72b8e` | **404** | **prod = blob ; préprod absente** |
| `virtues_ar_ext` | 91286 · `201f9e99ed4e` | 91286 · `201f9e99ed4e` | **404** | **prod = blob ; préprod absente** |
| `virtues_fa_ext` | 91286 · `b94ea9e4a1ce` | 91286 · `b94ea9e4a1ce` | **404** | **prod = blob ; préprod absente** |
| `virtues_zh_ext` | 91286 · `eb071fa30421` | 91286 · `eb071fa30421` | **404** | **prod = blob ; préprod absente** |
| `cards_fr` | 5855877 · `4cdd3d06a92e` | **404** | **404** | **non servie sur les deux hôtes** |
| `cards_fr_ext` | 91294 · `8b56e784abd8` | **404** | **404** | **non servie sur les deux hôtes** |
| `owl_fallacies` | 5985122 · `0aff63a3bbb4` | 4795192 · `258a9e430e4f` | 4795192 · `258a9e430e4f` | **les deux hôtes divergent du blob (identiques entre eux)** |
| `owl_virtues` | 1103945 · `032d58e99fcf` | **404** | **404** | **non servie sur les deux hôtes** |

---

## §3 — Datation et explication de chaque dépôt divergent

### prod — une seule fenêtre de déploiement, le **12/09 à 23:16Z**

`Last-Modified` des 32 wrappers prod : `Sat, 12 Sep 2026 23:16:02` → `23:16:19 GMT`, **une fenêtre de ~17 s**
pour les 36 fichiers. Les octets servis sont **identiques au blob `origin/master`** (`21a72385`) sur les 32.

⇒ **prod est à jour** : ce n'est pas un dépôt divergent, c'est le dépôt courant.

### préprod — la couche du **28/08 à 08:00:17Z** (= 10:00:17 locale)

`Last-Modified` uniforme `Fri, 28 Aug 2026 08:00:17 GMT` sur les 16 wrappers servis et sur l'OWL.
C'est **la même seconde** que l'empreinte de déploiement déjà documentée en #1244 (`Default.aspx`,
63 DLL plateforme, `Resources/libraries`) et rappelée en #830 — **4ᵉ instance indépendante du même horodatage**.

⇒ préprod sert la couche `de763aa9`, **6 commits de wrappers derrière HEAD**, plus petite de **−12,5 % à
−13,8 %** sur les 16 (`fallacies_*` : −13,1 % à −13,8 % ; `virtues_*` : −1,9 % à −2,0 %).

### ⚠️ Piège d'instrument trouvé en route — l'`ETag` IIS de préprod n'est pas dérivé du contenu

préprod renvoie **le même `ETag`** (`"807e8043c336dd1:0"`) pour `fallacies_fr.html` (**2 361 730 o**) et
`virtues_fr.html` (**512 022 o**) — deux fichiers de tailles incomparables. Sur prod, les deux `ETag`
diffèrent. ⇒ **ne pas utiliser l'`ETag` de préprod comme identité de contenu** : il ne discrimine pas, et
s'en servir produirait un « identique » faux. La comparaison retenue ici est octets + sha256.

---

## §4 — Le cas OWL : provenance **établie par identité de blob**, pas seulement « divergent »

`argumentum_fallacies.owl` est servi **à l'octet identique par les deux hôtes** (sha256
`258a9e430e4f…`, 4 795 192 o) mais il **diffère du blob committé** `docs/ontology/argumentum.owl`
(sha256 `0aff63a3bbb4…`).

Trois instruments successifs, jusqu'à épuisement :

1. **Historique du chemin committé** — 9 révisions de `docs/ontology/argumentum.owl` (2026-03-28 → 2026-09-05),
   tailles **5,3 à 6,8 Mo**. Aucune ne porte le sha256 servi ; toutes sont **plus grosses** que l'artefact servi.
2. **Balayage exhaustif du magasin d'objets** (`git cat-file --batch-all-objects`, filtre taille = 4 795 192) —
   **un seul** blob de cette taille existe : `af9e8f385e04e1aff2c9d31fc47b22543c570e50`, sha256 **égal au servi**.
3. **`--find-object` sur toutes les refs** — ce blob n'est atteignable que depuis **`5a086dfe`**, dont le sujet
   est *« untracked files on master: 1c5231f6 … »* : **le commit « untracked files » du stash du 28/08** — soit
   précisément l'archive que le grain ① a extraite vers GDrive.

**Sur disque** : `DNNPlatform/argumentum_fallacies.owl`, 4 795 192 o, `mtime 2026-08-28 10:00`,
**non tracé** (`git ls-files` : aucun ; `git check-ignore` : aucune règle) et absent de l'arbre
`origin/master`. Son `Last-Modified` HTTP est **`Sat, 02 Mar 2024 02:37:29 GMT`**.

⇒ **L'OWL publié est un artefact non tracé du webroot, daté de mars 2024**, antérieur de ~2,5 ans à la
première version committée, et dont la seule trace côté dépôt vit dans une **archive de stash**. Ce n'est pas
un « dépôt divergent » au sens d'un commit en retard : c'est un fichier **hors du contrôle de version**.

### Et le marqueur PK 511 ne dit pas ce que la deep-queue suppose

| | `nonverbalInfluence` | `nonverbalCommunication` |
|---|---:|---:|
| OWL **servi** (les 2 hôtes) | **25** | **0** |
| blob **committé** `docs/ontology/argumentum.owl` | **28** | **0** |

⇒ **aucun des deux** ne porte le libellé cible. La divergence servi ↔ committé n'est donc **pas** le
renommage PK 511 : ce renommage n'existe aujourd'hui dans **aucun** OWL, publié ou committé. Il vit dans la
source (CSV) et n'a pas encore été propagé — matière directe pour le grain ③ (#133), pas pour celui-ci.

---

## §5 — Ce qui n'est **pas servi** (404)

| route | prod | préprod | lecture |
|---|---|---|---|
| `fallacies_cards_fr.html` | **404** | **404** | committé, déployé **nulle part** |
| `fallacies_cards_fr_ext.html` *(hors périmètre)* | 404 | 404 | idem |
| les 16 `_ext` | 200 | **404** | présents en prod, jamais migrés en préprod |
| `argumentum_virtues.owl` | **404** | **404** | committé sous `docs/ontology/`, **absent du webroot** : aucun `DNNPlatform/argumentum_virtues.owl` sur disque |

Les 404 sont de **vraies pages d'erreur IIS** (corps de ~1,2 ko en prod, ~4,9 ko en préprod, intitulé
*« IIS 10.0 Detailed Error - 404 »*, URL reflétée ⇒ sha distinct par route). Un 404 est donc distinguable
d'un fichier présent mais périmé — les deux ne se confondent pas dans ce relevé.

⚠️ Ces absences **datent du 08/09** dans la documentation #1064 (`fallacies_*.html` = 404 préprod). La mesure
du jour montre que **la famille de base a depuis été migrée** (16/16 en 200) et que **seuls les `_ext` et
l'OWL Virtues restent absents**. Un chiffre de #1064 est donc périmé — à dater, pas à supprimer.

---

## §6 — Contrôles (le cœur du DoD)

| contrôle | énoncé | résultat |
|---|---|---|
| **inverse** | une route **volontairement divergente** doit être vue divergente, *et* identifiée à la bonne couche | préprod = blob **`de763aa9` sur 16/16 wrappers de base, à l'octet** ✅ |
| **négatif** | le harnais doit discriminer deux routes différentes | `fallacies_fr` servi (`56a9975d…`) ≠ blob de `Fallacies_es.html` (`9cf1a09d…`) ✅ |
| **positif** | une route non divergente doit ressortir identique | prod `fallacies_fr` = blob HEAD (`56a9975d…`) ✅ |

**Transparence sur un faux départ** : ma première passe du contrôle négatif a rendu « correctement
différent » **sur une commande `git` échouée** (shell lancé hors dépôt ⇒ empreinte de référence vide ⇒
comparaison trivialement inégale). Un `git rev-parse` vide **se lit comme un écart**. Le contrôle a été
relancé depuis le dépôt, avec garde sur la vacuité de la référence. Sans cette garde, les trois contrôles
auraient pu « passer » sans rien prouver.

---

## §7 — Ce que je n'établis PAS

- **Aucune preuve de fraîcheur.** `Last-Modified` et `ETag` sont des **déclarations du serveur** ; ils datent
  un fichier, pas un déploiement. Le fait qu'ils soient uniformes par hôte est un indice fort, pas une preuve.
- **L'intention derrière les absences.** Je ne sais pas si l'absence des 16 `_ext` en préprod est un oubli de
  migration ou un choix — la mesure constate, elle ne juge pas.
- **`fallacies_cards_fr*`** : je n'établis pas si ces routes appartiennent au livrable #830/#802 ; j'établis
  qu'elles ne sont servies **nulle part** (item idle, non tranché ici).
- **Le contenu des 6 commits de wrappers** `de763aa9..HEAD` n'est pas diffé — écart déjà déclaré en #830.
- **Le chemin physique IIS** : `Get-Website` exige l'élévation (non prise). L'identité webroot↔disque est
  établie par les **octets**, pas par la configuration.

---

## §8 — Ce que cette mesure donne aux grains suivants

**Grain ② (#1244, chronologie « webroot = checkout git »)** — le cas OWL est une **pièce de la chronologie** :
un fichier servi, vivant, **jamais passé par un commit**, dont la seule trace repo est une archive de stash.
Il matérialise le vecteur nommé par #1244 : ce n'est pas seulement que le webroot *est* un checkout, c'est
qu'il **contient des artefacts servis qui ne sont dans aucune branche**.

**Grain ③ (#133, impact IRI)** — le renommage PK 511 **n'a atteint aucun OWL** : ni le publié (25 ×
`nonverbalInfluence`), ni le committé (28 ×). Toute régénération déplacerait donc l'IRI **des deux côtés** à la
fois, et le publié porte un artefact de 2024 qu'aucune chaîne de régénération actuelle ne reproduit.

---

*master `21a72385` · 36 candidats × 2 hôtes · prod `www.argumentum.games` · préprod `dnn.argumentum.myia.io` ·
lecture seule, aucun dépôt, aucune régénération. Verdict visuel : ai-01.*
