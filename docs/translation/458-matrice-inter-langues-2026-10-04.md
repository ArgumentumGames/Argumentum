# #458 — Matrice inter-langues consolidée

**Grain 2 du pool** (`c.5980960236`). Sept passes de lecture native ont produit sept dossiers, dont
six portaient une **matrice inter-langues cumulée**. Ce document les **consolide en une seule**, et
donne à chaque famille le statut que le pool demande : **défaut** ou **observation**.

⚠️ **Documentaire : 0 écriture.** Aucune cellule CSV n'est touchée, aucun erratum n'est posté.

| Pièce | Passe | Lignes | Matrice |
|---|---|---:|---|
| `458-fidelite-scenarii-zh-2026-10-04.md` | zh (#1746) | 165 | — (§6 « écarts hérités » à la place) |
| `458-fidelite-scenarii-ar-2026-10-04.md` | ar (#1749) | 232 | §5 — 11 lignes |
| `458-fidelite-scenarii-fa-2026-10-04.md` | fa (#1750) | 194 | §5 — 21 lignes |
| `458-fidelite-scenarii-es-2026-10-04.md` | es (#1751) | 228 | §5 — 21 lignes |
| `458-fidelite-scenarii-ru-2026-10-04.md` | ru (#1752) | 254 | §5 — 20 lignes |
| `458-fidelite-scenarii-pt-2026-10-04.md` | pt (#1753) | 243 | §5 — 27 lignes |
| `458-fidelite-scenarii-en-2026-10-04.md` | en (#1754) | 380 | §5 — **27 lignes, la complète** |
| | | **1 696** | |

Le `zh` n'a pas de matrice : c'était la première passe, la matrice n'existait pas encore. Sa
contribution est portée par le §5 du dossier `en`, qui relève **15** lignes marquées `✓` en colonne
`zh` — et par son propre §6 « écarts hérités de l'EN », qui fournit trois des lignes.

## 1. Méthode — la règle de classement

Le pool demande, pour chaque famille : **défaut** ou **observation**. La distinction n'est pas un
jugement de goût : **elle est déjà écrite dans les dossiers**, et la règle consiste à la lire.

| Statut | Définition | Où elle se lit |
|---|---|---|
| **défaut** | la famille est **établie à trois voies** (FR \| EN \| cible), ou par contradiction **interne à la carte**, dans le **§3 « Défauts … établis »** d'au moins une passe | §3 d'un dossier |
| **observation** | la famille n'apparaît que dans une **matrice** ou un **§6 « Observations à trancher »** — aucune passe ne l'a établie | §6 / §5 seuls |

Le §3 s'appelle « Défauts … établis » et le §6 « … **pas des défauts établis** » : les dossiers
portent eux-mêmes la partition. Consolider, ici, c'est **appliquer leur propre définition** à
l'ensemble des familles — et non re-juger chaque cellule.

**Portée mécanique.** Aucun chiffre de ce document n'est recopié d'un rapport : lignes, comptes de
familles, marques par langue et colonne `en` sont **re-dérivés par script** depuis les sept
dossiers. Les deux écarts que cette re-dérivation a produits sont au §5.

## 2. La matrice consolidée — 37 familles

**27 familles inter-langues** (les lignes de la matrice `en`) + **10 familles propres à l'EN**
qu'aucune autre langue ne porte. Les marques `✓` sont celles publiées par la matrice `en` §5.

### 2.1 Familles où l'**EN est l'origine** — 9

| Famille | Nature | zh | ar | fa | es | ru | pt | `en` | Statut | Établi par |
|---|---|:-:|:-:|:-:|:-:|:-:|:-:|:-:|---|---|
| **4.1.11** titre | titre étendu | — | ✓ | ✓ | ✓ | ✓ | ✓ | *SOURCE* | **défaut** | passe `en` §4 — **hors des 39** |
| **3.3.9** sugg | structure EN suivie | — | ✓ | ✓ | ✓ | ✓ | ✓ | *SOURCE* | **défaut** | passe `en` §4 — **hors des 39** |
| **3.2.15** titre | « spaghetti » suivi | — | ✓ | ✓ | ✓ | ✓ | ✓ | *SOURCE* | **défaut** | `en` §3 A |
| **3.2.8** ctxt | ajout `all-important` | — | — | ✓ | — | ✓ | ✓ | *SOURCE* | **défaut** | `en` §3 F |
| **3.1.1 · 5.2.5 · 5.3.2 · 6.1.1 · 6.2.3** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | — | — | *SOURCE* | **défaut** | `en` §3 B · §3 E · §4 |
| **3.2.2** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | — | ✓ | *SOURCE* | **défaut** | `en` §3 E |
| **7.2.8** sugg | contresens | ✓ | — | — | — | — | ✓ | *SOURCE* | **défaut** | `en` §3 D |
| **6.3.2** sugg | point final omis | ✓ | — | — | — | — | ✓ | *SOURCE* | **défaut** | `en` §2 (2/501) — **hors des 39** |
| **7.2.5** enjeu | modalité durcie | — | ✓ | ✓ | ✓ | — | — | *durcit déjà* | **défaut** | `en` §3 I |

⚠️ `7.2.5` porte l'étiquette *durcit déjà* et non `SOURCE` : par la définition du dossier `en`
(« `SOURCE` = l'écart existe d'abord dans l'EN »), **elle appartient bien à cette table** — l'EN
écrit `He must convince` là où le FR dit « Il **tente** de convaincre ». Voir §5.

### 2.2 Familles où l'**EN est fidèle** — 18

| Famille | Nature | zh | ar | fa | es | ru | pt | `en` | Statut | Établi par |
|---|---|:-:|:-:|:-:|:-:|:-:|:-:|:-:|---|---|
| **2.2.9** ctxt | ajout « mythologique » | ✓ | ✓ | ✓ | ✓ | ✓ | ✓ | *fidèle* | **défaut** | `ru` §3 n°6 — 5ᵉ langue |
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | ✓ | ✓ | — | *fidèle* | **défaut** | `ar` §3 n°2 · `fa` n°2 · `es` n°2 · `ru` n°5 |
| **3.2.16** enjeu | modalité affaiblie | ✓ | ✓ | ✓ | ✓ | ✓ | — | *fidèle* | **défaut** | `ru` §3 n°7 — 5ᵉ langue |
| **1.1.3** sugg | lions → félidé domestique | ✓ | ✓ | ✓ | ✓ | — | — | *fidèle* | **défaut** | `zh` §3 n°1 · `ar` n°1 · `fa` n°1 · `es` n°1 |
| **4.2.8** titre | « inattendu » ajouté, jeu perdu | ✓ | ✓ | ✓ | ✓ | — | — | *fidèle* | **défaut** | `es` §3 n°4 — 4ᵉ langue |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | ✓ | — | — | *ponctue* | **défaut** | `zh` §3 n°6 — lot inter-langues |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | ✓ | ✓ | — | *garde Venise* | **défaut** | `ar` §3 n°3 · `fa` n°7 · `es` n°6 · `ru` n°9 |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — | ✓ | ✓ | ✓ | — | *fidèle* | **défaut** | `zh` §3 n°4 · `fa` n°4 · `es` n°5 · `ru` n°8 |
| **4.1.11** enjeu | superlatif idiomatique | — | ✓ | ✓ | ✓ | ✓ | — | *fidèle* | **défaut** | `ru` §3 n°12 — 4ᵉ langue |
| **3.2.8** | anniversaire d'événement | ✓ (incoh.) | — | ✓ | ✓ | — | — | *fidèle* | **défaut** | `zh` §3 n°2 · `fa` n°3 · `es` n°3 |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ | ✓ | ✓ | ✓ | — | *fidèle* | **défaut** | `ru` §3 n°11 — 3ᵉ langue |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ | ✓ | ✓ | ✓ | *fidèle* | **défaut** | `ru` §3 n°10 — 3ᵉ langue |
| **6.2.1** titre | titre inventé | ✓ | — | — | — | ✓ | — | *fidèle* | **défaut** | `zh` §3 n°3 · `ru` §3 n°3 |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — | — | — | — | *fidèle* | **défaut** | `zh` §3 n°5 |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — | ✓ | — | — | *ponctue* | **défaut** | `zh` §3 n°6 — lot inter-langues |
| **1.3.1** sugg | modificateur « sombres » | — | ✓ | ✓ | ✓ | — | — | *fidèle* | **observation** | aucun §3 |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ | ✓ | ✓ | — | — | *fidèle* | **observation** | aucun §3 |
| **4.2.2** ctxt | diminutif « un peu » | — | ✓ | ✓ | ✓ | — | ✓ | *fidèle* | **observation** | aucun §3 |

⚠️ `4.2.2` est en **observation** pour cette famille — la passe `en` §3 H établit bien un défaut
sur `4.2.2 ctxt`, mais il porte sur **un autre objet** (le calque de construction absolue
`although already drunk, the sleeper decides`), que le grain 1 a reclassé **E · dérivé** (phrase
**imprimée** conservée, FR avancé). Les deux objets ne se recouvrent pas.

### 2.3 Familles propres à l'EN — 10, aucune autre langue ne les porte

Ces familles n'apparaissaient dans **aucune** matrice antérieure : personne ne les avait vues
parce que personne ne lisait l'EN. Toutes sont des **défauts EN établis** (`en` §3 / grain 1).

| Carte | Nature | Statut |
|---|---|---|
| **1.1.1** bara | nom propre perdu (`Caesar's mother` pour « Aurelia Cotta ») | **défaut** |
| **2.2.5** bara | rôle → nom, déjà le titre (`Salomon`) | **défaut** |
| **2.3.5** pioch | personnage substitué **+ deux orthographes dans la même carte** (`specter` / `spectre`) | **défaut** |
| **2.1.3** sugg | question → affirmation | **défaut** |
| **4.2.1** sugg | dernière phrase coupée (le rationnement) | **défaut** |
| **7.3.2** pioch | rôle substitué, contredit son propre enjeu | **défaut** |
| **7.1.5** titre + enjeu | mot FR non traduit au titre (`Kermesse`), contenu de l'enjeu changé | **défaut** |
| **7.1.6** titre | faute d'orthographe (`The inheritence`) | **défaut** |
| **7.3.4** titre | faute d'orthographe (`Gooal!`) | **défaut** |
| **6.2.2** ctxt | `occult payment` pour « un pot-de-vin » | **défaut** |

### 2.4 Le compte

| Statut | Familles |
|---|---:|
| **défaut** | **34** |
| **observation** | **3** |
| | **37** |

Les trois observations (`1.3.1`, `3.3.5`, `4.2.2`) ont une forme voisine de familles classées
défauts : un ajout ou un adoucissement **absent des deux sources**, porté par 3 à 4 langues. Ce qui
les sépare n'est pas la preuve — **c'est l'écriture** : aucune passe ne les a portées à son §3.
Elles restent donc **à trancher en relecture native**, ce que dit leur statut.

## 3. Les six familles à statut incertain — verdict

Le pool bloque **Corrections ③** sur ce document : *« familles à statut incertain (2.2.9, 3.2.16,
4.1.12, 3.1.5, 4.1.11, 4.2.8) : jusqu'au merge du grain 2 »*. Verdict, famille par famille :

| Famille | Statut | Établie par | Mesure |
|---|---|---|---|
| **2.2.9** ctxt | **défaut** | `ru` §3 n°6 | ajout « personnage mythologique » — **5ᵉ langue** ; absente du FR **et** de l'EN |
| **3.2.16** enjeu | **défaut** | `ru` §3 n°7 | modalité affaiblie (« il doit convaincre » → « il tente ») — **5ᵉ langue** |
| **4.1.12** enjeu | **défaut** | `ru` §3 n°10 | modalité affaiblie — **3ᵉ langue** (ar · fa · ru ; l'es est fidèle, « debe ») |
| **3.1.5** pioch | **défaut** | `ru` §3 n°11 | « conquête » → « élue » — **3ᵉ langue** (ar · fa · ru ; l'es est fidèle) |
| **4.1.11** enjeu | **défaut** | `ru` §3 n°12 | superlatif idiomatique substitué — **4ᵉ langue** |
| **4.2.8** titre | **défaut** | `es` §3 n°4 | « inattendu » ajouté, le jeu compromettant/compromis dédoublé — **4ᵉ langue** |

⭐ **Les six sont des défauts.** Le statut « incertain » venait de ce qu'elles n'existaient que dans
des **matrices** — listes de candidats — et qu'aucune passe ne les avait encore portées à son §3.
C'est la **passe `ru`** qui en a établi **cinq** (n°5 à n°12 : elle liste, outre ses propres
défauts, les confirmations inter-langues devenues majoritaires), et la **passe `es`** la sixième
(n°4). Après elles, aucune n'est restée candidate.

⚠️ **Une septième famille du même dossier n'a pas été portée à cette liste** — `4.1.1 enjeu`
(« doit vendre » → « essaie de vendre », fa **seule**). Voir §5.

## 4. Les lignes « l'EN est la source » — renvoi au grain 1

⏳ **Statut : en attente de l'axe imprimé.** Le grain 1 (PR **#1765**) est **ouvert et CLEAN, non
mergé**. Les verdicts ci-dessous sont **ceux qu'il a mesurés** ; ils sont cités comme provenant
d'une PR non mergée, et devront être relus au merge.

| Famille `en`-origine | Verdict du grain 1 (#1765) | Ce qu'il implique |
|---|---|---|
| **3.1.1** sugg | **C** · EN fidèle à un **FR périmé** | ⭐ l'EN traduit **mot pour mot le FR imprimé** — ce n'est pas un défaut EN. C'est **cette** cellule qui est `SOURCE` de la famille suivie par zh · ar · fa · es : les quatre suivent **l'ancien FR**, par l'EN |
| **5.2.5** enjeu | **G** · carte **hors édition** | rien n'a été imprimé ⇒ correction **libre** |
| **5.3.2** enjeu | **G** · carte hors édition | correction libre |
| **6.1.1** sugg | **G** · carte hors édition | correction libre |
| **6.2.3** sugg | *hors des 39* | **aucun verdict imprimé** — voir §5 |
| **3.2.2** sugg | **C** · EN fidèle à un FR périmé | le FR imprimait « habits » ; `clothes` le traduit ; « affaires » est le mot **courant** |
| **3.2.15** titre | **G** · carte hors édition | correction libre |
| **3.3.9** sugg | *hors des 39* | **aucun verdict imprimé** — voir §5 |
| **4.1.11** titre | *hors des 39* | **aucun verdict imprimé** — voir §5 |
| **3.2.8** ctxt | **G** · carte hors édition | correction libre |
| **7.2.8** sugg | **G** · carte hors édition | correction libre |
| **6.3.2** sugg | *hors des 39* | **aucun verdict imprimé** — voir §5 |

⭐ **Le cas lourd est `3.1.1`.** Le grain 1 n'a pas seulement reclassé la cellule : il a montré que
**quatre langues suivent une source qui n'existe plus**. La correction n'est donc pas « réparer
l'EN » — c'est décider si le FR réécrit fait foi, puis aligner quatre langues sur lui.

## 5. Trois écarts trouvés en consolidant

Ce sont des **produits de la consolidation**, pas des redites : chacun est re-dérivé par script et
n'apparaît dans aucun dossier pris seul.

### 5.1 Quatre cellules EN déviantes **hors des 39** — la matrice voit ce que FR | EN ne voit pas

Les `39 défauts EN établis` du §3 et la table §4 de la matrice ne coïncident pas. Quatre cellules
de la table §4 **ne figurent dans aucune des neuf sous-familles A–I** :

| Cellule | Cellule EN | Suivie par | Réparée par |
|---|---|---|---|
| **4.1.11** titre | `Mars, the Next Must-See Destination` | ar · fa · es · ru · pt | zh |
| **3.3.9** sugg | ordre inversé (affirmation puis question) | ar · fa · es · ru · pt | zh |
| **6.2.3** sugg | `claims your head` | ar · es | fa · ru · pt |
| **6.3.2** sugg | point final omis | zh · pt | ar · fa · es · ru |

Ce que ces quatre ont en commun : **aucune n'est lisible en FR | EN**. `3.2.15` (le titre
« spaghetti ») avait été trouvé parce que le titre **déplace le sujet** ; `3.3.9` et `6.2.3` ne
déplacent rien de visible — ce sont **cinq langues qui suivent le même texte** qui les désignent.
⭐ **La matrice n'est pas un tableau de bord : c'est un instrument de détection**, et il a produit
quatre défauts EN que la lecture à deux voies n'avait pas listés.

⚠️ Conséquence : les quatre n'ont **aucun verdict d'axe imprimé** — le grain 1 ne les a pas
classées, puisqu'il partait des 39. Les trois premières sont donc à porter à son périmètre, la
quatrième (`6.3.2`) était **mesurée** au §2 du dossier `en` (« 2 omissions sur 501 cellules ») sans
avoir été portée aux 39.

### 5.2 Trois lignes comptées `SOURCE` que la définition du dossier range en *fidèle*

Le §5 du dossier `en` conclut : *« La colonne `en` est SOURCE sur **12** lignes et *fidèle* sur
**15** »*. La re-dérivation donne, à la cellule :

| Étiquette portée | Lignes |
|---|---:|
| `SOURCE` (en gras) | **8** |
| *fidèle* | **15** |
| *ponctue* | **2** — `1.2.3`, `2.1.8` |
| *garde Venise* | **1** — `7.3.5` |
| *durcit déjà* | **1** — `7.2.5` |

Le total fait bien 27, mais le regroupement annoncé (12 + 15) range dans `SOURCE` **trois lignes
que la définition du dossier lui-même exclut** :

> *« `SOURCE` = l'écart existe d'abord dans l'EN ; `fidèle` = **l'EN suit le FR**, donc l'écart est
> un ajout propre à la langue cible. »* — §5 du dossier `en`

- **`1.2.3`** et **`2.1.8`** (*ponctue*) : l'EN **ponctue**. Le §2 du même dossier le mesure —
  seules **2** cellules EN sur 501 n'ont pas de ponctuation finale (`2.2.1 sugg`, `6.3.2 sugg`), et
  ni `1.2.3` ni `2.1.8` n'en sont. ⇒ l'EN suit le FR ⇒ **fidèle**.
- **`7.3.5`** (*garde Venise*) : l'EN **garde** « Venise » ; ce sont **quatre langues cibles** qui
  ont réécrit la punchline. L'EN est le **témoin**, pas l'origine. ⇒ **fidèle**.

⇒ Par la définition citée : **9 lignes ont l'EN pour origine, 18 ont l'EN fidèle** — et non 12/15.
Cet écart n'est pas cosmétique : il **retire trois familles du passif EN**, donc de la file
« écarts hérités de l'EN », et les rend **corrigeables côté langues cibles**.

⭐ **Corroboration indépendante** : c'est déjà ainsi que le pool les a traitées. Le **grain 3
(#1763)** corrige `7.3.5` dans les **cinq langues cibles** — il ne touche pas l'EN. Et la classe
« ponctuation finale absente » n'est **pas une famille** mais une **fuite par langue** : les
ensembles ne se recouvrent presque pas.

| Langue | Cellules sans ponctuation finale |
|---|---|
| zh | `1.2.3` (ctxt **et** enjeu), `2.1.8` |
| ar | `1.2.3`, `2.1.8` |
| fa | `1.2.3` |
| es | `1.2.3`, `2.1.8`, **`7.2.3`** (isolé es) |
| ru | **aucune** |
| pt | **`1.1.1`** (isolé pt — 1/167) |
| en | **`2.2.1`**, **`6.3.2`** |

Sept langues, sept ensembles qui ne coïncident qu'en partie : ce n'est pas un lot hérité, c'est une
**scorie répartie**.

### 5.3 `4.1.1 enjeu` — une ligne de matrice perdue en route

La matrice **fa** porte une ligne que les matrices **pt** et **en** n'ont plus :

| Source | Ligne |
|---|---|
| `fa` §5 | `**4.1.1** enjeu │ modalité affaiblie (« doit vendre » → « essaie de vendre ») │ — │ — │ ✓ (تلاش کند) │ — │ —` |
| `es` §5 | `**4.1.1** enjeu │ modalité affaiblie │ — │ — │ ✓ │ — (fidèle : debe) │ —` |
| `pt` §5 | **absente** (`4.1.11`, `4.1.12`, `4.1.13` présentes) |
| `en` §5 | **absente** |

Témoin unique (**fa**), contrôle négatif (**es** : « debe »), jamais re-mesurée par les deux
dernières passes, et **absente de la liste des familles à statut incertain**. C'est exactement la
forme de famille que le pool veut voir instruite, et elle est **tombée du tableau sans clôture**.

⭐ Le patron se généralise : **une ligne de matrice qui n'est reprise par aucune passe ultérieure
n'est pas close, elle est silencieuse.** Les trois lignes retirées en `pt`/`en` faute d'être
confirmées ne portent aucune mention de retrait — le lecteur suivant ne peut pas distinguer
« famille éteinte » de « famille oubliée ». Statut proposé : **à mesurer** (fa contre FR, puis
contrôle sur zh · ru).

## 6. Ce que cette consolidation n'établit pas

- **Elle ne juge aucune cellule.** Le statut est **hérité** des §3 : ce document applique la
  partition des dossiers, il ne la refait pas.
- **Elle ne corrige rien.** Les 34 défauts et les 3 observations sont des **statuts**, pas des
  gestes. Le devenir de chaque famille appartient aux grains de correction.
- **Elle ne remplace pas la relecture native.** Les trois observations restent des questions
  posées à des locuteurs, pas des constats.
- **Elle ne re-mesure pas les colonnes `suivent` / `réparent`** de la table §4 — elles sont
  recopiées des passes, comme le dossier `en` le déclare lui-même.
- **Elle ne poste aucun erratum** sur #1749 → #1754 : le §5 signale trois écarts de **synthèse**
  (compte `SOURCE`, ligne perdue, cellules hors 39) ; il ne prétend pas que les dossiers ont
  mesuré faux. Même régime que le §10 du dossier `en` et le §10 du grain 1.

## 7. Verdict

- **37 familles consolidées** : **34 défauts**, **3 observations**. Les observations (`1.3.1`,
  `3.3.5`, `4.2.2`) le sont **par écriture, non par preuve** — même forme que des familles
  classées défauts, mais jamais portées à un §3.
- ⭐ **Les six familles à statut incertain sont six défauts.** **Corrections ③ est débloqué** : les
  cinq établies par la passe `ru` (n°6, 7, 10, 11, 12) et la sixième par la passe `es` (n°4).
- ⭐ **Neuf lignes ont l'EN pour origine, dix-huit ont l'EN fidèle** — et non 12/15 comme le §5 du
  dossier `en` l'annonce. Les trois lignes déplacées (`1.2.3`, `2.1.8`, `7.3.5`) sortent du
  passif EN et deviennent **côté langues cibles** — ce que le grain 3 avait déjà fait pour `7.3.5`.
- ⭐ **Quatre cellules EN déviantes sont hors des 39** (`4.1.11` titre, `3.3.9` sugg, `6.2.3` sugg,
  `6.3.2` sugg), toutes désignées par la comparaison inter-langues et **invisibles en FR | EN**.
  Aucune n'a de verdict d'axe imprimé.
- ⭐ **`2.2.9` reste la seule carte à six langues** et **la seule famille que l'EN n'explique pas** ;
  **`4.3.3` reste zh-seul après cinq contrôles** ; **`6.2.1` = zh · ru, deux inventions distinctes**.
- **0 écriture**, **0 republication** — `v2.0.0-review` reste **gelé** jusqu'au verdict des associés
  (05/10).
