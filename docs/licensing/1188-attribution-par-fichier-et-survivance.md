# #1188 — Attribution des co-auteurs, **par fichier** et **par survivance au HEAD**

**Auteur** : po-2024 (worker lane) · **Grain** : ③ du pool v8 ([#458 c.5761691562](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5761691562)) · **Base** : master `6694d702` · **Date** : 2026-09-21

**Objet** : #1188 §1.a porte « l'accord des **co-auteurs** » comme préalable à toute cession. Le [dossier d'arbitrage](1188-dossier-arbitrage-canal.md) nomme « Thomas, Adeline, contributeurs de traduction ». Ce document mesure **ce que git peut dire** de cette liste — et surtout **ce qu'il ne peut pas dire**.

⛔ **Aucun contact pris avec quiconque. Aucun avis juridique rendu.** Mesure et instruments seulement.

---

## 1. Le résultat en une table

Périmètre = le **corpus livrable**, `Cards/**` à HEAD.

| Surface | Fichiers à HEAD | Instrument | Résultat |
|---|---|---|---|
| tout `Cards/**` | **803** | — | dont **341 hors `Archive/`**, 462 sous `Archive/` |
| texte (json/csv/html/svg/css/md/mm/…) | **150** | `git blame --line-porcelain` → lignes **survivantes** | **2 auteurs** : jsboige **1 049 556** lignes / 147 fichiers · Thomas **13 986** lignes / **6 fichiers** |
| binaire (png/jpg/otf/pdf/ai) | **653** (617 png) | `git log` → **dernier écrivain** du blob à HEAD | **2 auteurs** : jsboige **640** · Thomas **13** |

Sur les **341 fichiers livrés** (hors `Archive/`), dernier écrivain : **jsboige 328 · Thomas 13**.

⇒ **Sur le corpus livrable, l'expression créative qui survit appartient à 2 personnes.**

### Les fichiers de Thomas, nommés

**13 PNG** — dernier écrivain, tous sous `Cards/Fallacies/Assets/Fallacy-front/` :
`1.1.3` · `1.2.1.2.alt` · `1` · `2.2.1.2.1` · `2.2.2.4` · `2.3.1.1.4` · `3.1.1.2.1` · `3.1.2` · `4.3.2.2.1` · `6.1.3.1.1.2` · `6.2.3.4` · `6.3.1.3.1` · `7.3.2.1`

**6 fichiers texte** — lignes survivantes (⚠️ aucun n'est son « dernier écrivain » : il a été réécrit **par-dessus**, ce qu'un instrument unique ne verrait pas) :
`Cards/Packaging/FCPM_065 - FOND - 117x122x28mm.svg` (**12 051** lignes) · `Cards/Packaging/FCPM_065 - CLOCHE - 121x126x26mm.svg` (**1 760**) · `Cards/Rules/Archive/2022/…Smooth Talker….csv` (125) · `Cards/Rules/regles.md` (40) · `Cards/Rules/rules.md` (7) · `Cards/Fallacies/Archive/v2/…Face_v2_Francais.json` (3)

⚠️ **Deux instruments, deux ensembles qui ne se recouvrent pas** (13 PNG vs 6 texte). Un rapport qui n'en utiliserait qu'un **manquerait la moitié de la contribution de Thomas** — et les deux SVG de packaging (gabarits de boîte, dimensions au nom de fichier) sont précisément du contenu **livré**.

---

## 2. ⭐ La trouvaille : les deux ensembles sont **disjoints dans les deux sens**

| | Visible dans **git** | Nommé par les **documents** |
|---|---|---|
| **jsboige** | ✅ (4 identités) | ✅ |
| **Thomas Watanabe-Vermorel** | ✅ (19 fichiers, expression survivante) | ✅ |
| **Adeline** | ⛔ **introuvable** | ✅ (relectrice, recette 3 humains #1180, gate (b) #134/#802) |
| **Ludovic Pelletier** | ✅ (6 commits sur `Cards`+`Generation`) | ⛔ jamais nommé |
| **Nathaniel Richand** | ✅ (178 fichiers touchés) | ⛔ jamais nommé |

### 2.a — Adeline : git rend **0**, et ce zéro est un faux

Mesuré : `git log --all --author=Adeline` → **vide** · `--grep=Adeline` → uniquement des commits **qui parlent d'elle** (revue, recette) · aucun trailer `Co-authored-by` à son nom.

Grep documentaire : `.claude/skills/coordinate/triage-github.md:19` — *« Adeline n'a aucun compte GitHub parmi les 5 collaborateurs (`lpelleti`, `ynnk`, `jsboige`, `ThomasWatanabeVermorel`, `clusterManager-Myia`) — ses demandes arrivent **par le clavier de jsboige** »*. Elle est **relectrice et décideuse** (« trois humains » de la recette #1180 ; BAT physique #1187 ; véhicule de relecture #802).

⇒ **Une attribution par métadonnées git conclurait « Adeline = 0 contribution ».** C'est exactement la famille de faux-zéros déjà consignée : *un instrument qui ne peut pas voir X rend 0 sur X, et ce 0 se lit comme « X n'existe pas »*. **Elle doit être traitée comme co-auteure sur la base des documents, pas de git** — et git ne pourra **jamais** le prouver.

### 2.b — Ludovic Pelletier et Nathaniel Richand : visibles dans git, **0 expression survivante dans le corpus**

| Identité | Fichiers touchés | Survivent à HEAD | Expression survivante dans `Cards/**` |
|---|---|---|---|
| **Ludovic Pelletier** | 5 (dont des chemins `Cartes/**`, pré-renommage) | 2 | **0** — seul reste **2 lignes de `.gitignore`**, dépôt entier |
| **Nathaniel Richand** | **178** | **0** | **0** — les 178 fichiers ont **tous** disparu de HEAD |

⇒ Pour le **corpus**, ni l'un ni l'autre n'a d'expression à licencier. ⚠️ Mais « 0 expression survivante » n'est **pas** « 0 contribution » : une contribution peut être une **direction**, une relecture ou un choix éditorial, qu'aucun metadata ne voit (même angle mort qu'Adeline, en miroir).

---

## 3. Le placeholder `Your <your.email@example.com>` — 95 commits non attribuables

`git` mal configuré : **95 commits** sur `Cards`+`Generation` (561 fichiers touchés, 334 encore à HEAD) portent l'identité **par défaut** de git. Mesure : **0 ligne survivante** dans les 150 fichiers texte de `Cards/**` — leur contenu a été réécrit depuis. Le risque est donc **contenu**, mais il est **déclaré** plutôt que passé sous silence : ces 95 commits ne sont attribuables à personne **par leur métadonnée**.

*(Signe corroborant une ré-application d'historique : plusieurs commits apparaissent en **deux SHA distincts, sujet et liste de fichiers identiques** — ex. `5838f0f7`/`1cde4adf`, `9ba6cca3`/`ede3766b`.)*

---

## 4. Instruments et leurs limites — déclarées, pas cachées

| Mesure | Commande | Limite |
|---|---|---|
| Inventaire | `git ls-files 'Cards/**'` | HEAD seulement (cf. la leçon #415 : un pathspec vérifié à HEAD rend 0 pendant que l'historique porte 462 objets) |
| Auteurs | `git log --all --format='%an\|%ae' -- Cards Generation` | `--all` = toutes branches ; **pas** les contributeurs sans commit |
| Survivance texte | `git blame --line-porcelain HEAD -- <f>` | ⛔ **sans valeur sur un binaire** — un premier passage l'a lancé sur des PNG et a produit des « centaines de lignes » attribuées à une image : **instrument invalide, corrigé** |
| Survivance binaire | `git log --format='C\|%ae' --name-only HEAD -- Cards`, 1ʳᵉ occurrence = dernier écrivain | dit qui a **écrit en dernier**, pas qui a **créé** |
| Adeline | `--author` + `--grep` + trailers | ne peut **structurellement pas** la trouver (aucun commit) |

⛔ **Aucune de ces mesures ne voit une contribution hors dépôt** (relecture, direction artistique, arbitrage). C'est la limite qui compte le plus ici, et elle joue **contre** la conclusion rassurante.

---

## 5. Ce que cela change pour l'arbitrage owner

1. **La demande se rétrécit d'un côté, mesurément** : sur le **corpus livrable**, il n'y a **qu'un** co-auteur externe dont l'expression survit — **Thomas**. Ni Ludovic ni Nathaniel n'ont de contenu à licencier.
2. **Et elle ne se rétrécit pas de l'autre** : **Adeline** est nommée par tous les documents de recette et **introuvable dans git**. Un dossier qui s'appuierait sur `git log` pour lister les co-auteurs la **manquerait** — et publierait « 2 co-auteurs » alors que le chiffre dépend de la source.
3. **Le chiffre « 2 personnes externes » (ai-01, 21/09) est donc juste par coïncidence de méthode, pas par identité de liste** : il compte Ludovic + Thomas (ce que git voit sur `Cards`+`Generation`), tandis que la liste documentaire est Thomas + Adeline. **Les deux valent 2, et ce ne sont pas les mêmes deux.**
4. ⛔ **Toute demande d'accord doit être adressée par nom, jamais dérivée d'un `git log`** — le seul instrument qui liste les co-auteurs réels est documentaire.

**Suite possible (non faite, hors mandat)** : le corpus `Cards/**` hors `Archive/` (341 fichiers) comme **périmètre candidat à relicencier** — 328 fichiers à dernier écrivain jsboige, 13 à Thomas, 0 à quiconque d'autre.

---

## 6. Notes d'exécution

- ⛔ **Zéro écriture** sous `Cards/**` : ce document est la seule écriture du grain.
- Deux identités de jsboige sont fusionnées dans les comptes ci-dessus (`Jean-Sylvain Boige|jsboige@gmail.com`, `jsboige|jsboige@gmail.com`, plus `jsboigeEpita` et `po-2024`).
- Coïncidence à ne pas confondre : `Cards/**` compte **462** fichiers sous `Archive/` ; les **462 objets** de #415 sont des objets de l'**historique**, pas des fichiers à HEAD.
- Le SHA `6694d702` est la base ; toute re-mesure doit repartir d'un arbre vérifié, `git ls-files` ne voyant que HEAD.

🤖 Generated with [Claude Code](https://claude.com/claude-code)
