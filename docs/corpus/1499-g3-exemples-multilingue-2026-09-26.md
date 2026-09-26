# #1499 grain ③ — surface **multilingue** des `example_*` : 0 défaut résiduel, et pourquoi un compte par langue ne se lit pas comme un compte de défauts

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **PR :** [#1572](https://github.com/ArgumentumGames/Argumentum/pull/1572) · **Dispatch :** pool v16 grain 6 ([#458 c.5842223755](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5842223755)) — grain ③ de l'epic ([#1499](https://github.com/ArgumentumGames/Argumentum/issues/1499) §5), resté non pris
**Instrument :** `tools/1499-g3-exemples-multilingue.py` (`--self-test` 17/17, `--mutation-test` OK)

## 0. Verdict

**Le grain ③ n'avait jamais été livré.** Le tableau §5 de l'epic le donne 🟢 READY « après ② », et ② a été absorbé par **G2** (`docs/corpus/fallacies-exemples-diff.py`, `FIELD = 'example_fr'`) ; **G3** l'a été par `fallacies-desc-diff.py` (`desc_fr`). Les deux sont **français seul** : la surface des sept autres langues n'était couverte par aucun dossier.

Ce que la mesure rend, en trois résultats :

| # | Résultat | Nature |
|---|---|---|
| 1 | L'instrument **reproduit les 8 chiffres de l'epic, à l'unité**, sur l'arbre immédiatement antérieur à #1546 : `fr 14 · en 16 · ru 21 · pt 12 · es 11 · ar 12 · fa 12 · zh 13`, **union 25 PK** — le « 25 PK distincts » du §2 | mesure |
| 2 | Depuis, **#1546 a balayé les gloses dans les 8 langues** : les huit comptes baissent, l'union tombe à **18 PK**, et la surface résiduelle **ne contient aucun défaut** (lue PK par PK, §4) | mesure + lecture |
| 3 | ⭐ Le détecteur B **n'est pas neutre entre les langues** : le tiret cadratin russe est un **copule/apposition** (« Молочные продукты — наши друзья »), pas une signature de glose. Un compte multilingue de B **mélange une norme typographique et un défaut** | mesure |

⛔ **Zéro écriture.** Le livrable de ce grain est un dossier, jamais un patch (règle de l'epic, §5).

## 1. Fidélité de l'instrument — les chiffres de l'epic, reproduits

L'epic §2 publie, « détecteur B, 8 langues, 25 PK distincts » : `fr 14 · en 16 · ru 21 · pt 12 · es 11 · ar 12 · fa 12 · zh 13`. Re-mesurés avec cet instrument **sur l'arbre immédiatement antérieur à #1546** (`9f606c98^1`) — l'arbre exact de la mesure du 22/09 n'est pas reconstituable, la reproduction vaut donc pour ce qu'elle prouve : **les colonnes `example_*` n'ont pas bougé entre la mesure de l'epic et #1546** — sur les 175 cartes du deck :

| | fr | en | ru | pt | es | ar | fa | zh | union 8 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| **epic §2 (22/09)** | 14 | 16 | 21 | 12 | 11 | 12 | 12 | 13 | **25** |
| **re-mesuré @ `9f606c98^1`** | **14** | **16** | **21** | **12** | **11** | **12** | **12** | **13** | **25** |

**8/8 colonnes et l'union, à l'unité.** ⭐ C'est ce qui autorise à lire la suite comme un **delta** : l'instrument voit bien ce que l'epic voyait, donc les baisses du §3 sont des changements du corpus, pas un instrument qui regarde ailleurs.

## 2. ⛔ La barrière de périmètre — la mesure n'est pas homogène entre les langues

Mesuré **avant** tout compte, parce qu'un total sur 8 langues s'y serait trompé :

| colonne | à la baseline `62b561e75` (22/04/2024, 74 colonnes) | à HEAD (104 colonnes) |
|---|---|---|
| `example_{fr,en,ru,pt}` | **existe** (avec des valeurs) | existe |
| `example_{es,ar,fa,zh}` | ⛔ **la colonne n'existe pas** | existe |

⇒ Pour `es`, `ar`, `fa` et `zh`, **il n'y a ni référence ni contrôle inverse** : le détecteur A (croissance) est **inapplicable**, et « la signature était-elle déjà là ? » n'a pas de réponse. Un compte y est un **niveau**, jamais un **delta**.

⭐ *Une colonne entièrement nouvelle n'est pas une réécriture.* Sommer les 8 langues rendrait un chiffre qui se lit comme une surface de régression alors qu'il additionne **4 deltas et 4 niveaux**.

## 3. Le balayage de #1546 — les huit langues baissent

| | fr | en | ru | pt | es | ar | fa | zh | union 8 |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| avant #1546 (`9f606c98^1`) | 14 | 16 | 21 | 12 | 11 | 12 | 12 | 13 | 25 |
| **après #1546 (HEAD)** | 4 | 7 | 13 | 3 | 2 | 3 | 2 | 5 | **18** |
| Δ | −10 | −9 | −8 | −9 | −9 | −9 | −10 | −8 | **−7** |

⚠️ **La baisse porte sur les 8 langues, pas sur le seul `fr`.** #1546 a écrit **88 cellules** (« gloses retirées »), multilingues. Le lire comme un geste français serait faux — et c'est précisément ce que la mesure empêche.

## 4. La surface résiduelle — les 18 PK, lus un par un

⛔ **Le détecteur B est un pré-filtre, pas un verdict** (epic §3 : « les détecteurs sont partiels par construction »). Les 18 PK restants ont donc été **lus**, segment par segment (`§F` de l'instrument), et se ventilent en **quatre familles, toutes légitimes** :

| famille | PK | lecture |
|---|---|---|
| **tours de parole** (tiret de dialogue) | 658 (×8 langues) · 813 (fr,en,pt,ar) · 974 (fr,en) · 943 (fr) · 1388 (en,fa,zh) | le tiret sépare deux locuteurs, il n'explique rien |
| **glose délibérée** — décision owner Q-16 (c), rétablie par **#1549** | 796 (en,ru,pt,es,ar,zh) · 848 (zh) | ⛔ **voulue** : c'est la glose que l'owner a fait restaurer à l'octet |
| **l'ambiguïté montrée par le tiret** — la carte a pour sujet l'ambiguïté | 847 « Amphibologie » · 855 « Équivoque » | « I prefer chicken with olives—or chicken over olives. » : le tiret **est** le procédé de l'exemple |
| **typographie russe** — le tiret cadratin comme copule ou apposition | 595 · 596 · 698 · 804 · 833 · 834 · 942 · 1301 · 1355 | « Молочные продукты **—** наши друзья » · « Школа **—** как тюрьма » · « Этот йогурт **—** лучший » |

**⇒ 0 défaut résiduel.** La surface de 18 PK est **entièrement expliquée** par ces quatre familles.

### Le contrôle inverse du `0`

Un `0` n'est une absence que si l'instrument pouvait rendre un `1` — ici il a rendu **25** un jour plus tôt (§1), et la `--mutation-test` injecte la signature dans une copie littérale d'une cellule : le détecteur la voit, et la retire. Le `0` du §4 est donc une **lecture**, pas un instrument aveugle.

## 5. ⭐ Le résultat qui compte : B n'est pas neutre entre les langues

**9 des 13 cellules russes** résiduelles sont le tiret cadratin **standard** du russe (copule entre sujet et attribut, apposition), sans aucun rapport avec la signature de glose que l'epic cherchait. Le russe est d'ailleurs la langue la plus haute du classement de l'epic (**21**, contre **12** en `pt`) — et à la lecture, cet écart mesure en bonne partie **une norme typographique**, pas une densité de défauts.

⇒ ⛔ **Un compte multilingue de B ne se publie pas comme une surface de défauts.** C'est la raison pour laquelle le §2 de l'epic — « la répétition du motif dans les traductions indique une propagation par re-génération » — **n'est pas confirmée** par cette lecture : le motif se répète aussi là où il n'est pas un défaut.

## 6. Ce qui n'est PAS conclu

- Que les 4 familles du §4 soient closes **en droit** : elles le sont **à la lecture de ces 18 PK**, sur l'arbre du 26/09. Un ajout ultérieur rouvre la mesure, pas ce dossier.
- Que le détecteur B doive être retiré : il a servi à trouver la surface, et #1546 l'a traitée. Ce qui est dit, c'est qu'il **ne se totalise pas entre les langues**.
- Que `es`/`ar`/`fa`/`zh` soient sans défaut : ils sont **sans référence** (§2). Leur cohérence interne est une autre question, hors de ce grain.
- **Aucune écriture** n'est proposée, ni sur les 18 PK, ni ailleurs.

### Une 9ᵉ colonne, vérifiée puis écartée

`example_en_bis` **est rendue** — quatre gabarits Fallacies la lisent (`Argumentum_Fallacies_Face{,_2,_3,_Web}_fr.json`). Elle est **vide** : `0` cellule non vide sur les **1408 lignes**, à HEAD comme aux deux références. La colonne existe, le contenu n'existe pas : le périmètre de 8 langues est donc complet pour ce que les cartes portent. ⭐ Le motif vaut d'être noté — *une colonne référencée par un gabarit n'est pas une colonne peuplée.*

## 7. Exécutables

```bash
python tools/1499-g3-exemples-multilingue.py                 # HEAD : 8 langues, révélation des segments
python tools/1499-g3-exemples-multilingue.py --ref 9f606c98^1  # avant #1546 : reproduit les 8 chiffres de l'epic
python tools/1499-g3-exemples-multilingue.py --self-test     # 17 cas littéraux
python tools/1499-g3-exemples-multilingue.py --mutation-test # la signature injectée doit être vue
```

⚠️ Le `--self-test` a **attrapé un vrai bug de l'instrument** avant toute mesure : `norm_ws` ramène `—` et `–` à `-` (il normalise pour **comparer**), si bien que le détecteur B lisait le texte normalisé et devenait **muet sur le motif même qu'il cherchait** — un zéro par construction, indistinguable d'une absence réelle. Le détecteur est désormais appliqué au texte NFC **brut**. ⭐ *La garde qui a servi est celle qui a été vue rouge.*

## 8. Références

- Epic : [#1499](https://github.com/ArgumentumGames/Argumentum/issues/1499) §2 (les 3 détecteurs) et §5 (grain ③)
- Balayage des gloses : [#1546](https://github.com/ArgumentumGames/Argumentum/pull/1546) (88 cellules) · restauration voulue : [#1549](https://github.com/ArgumentumGames/Argumentum/pull/1549) (Q-16 (c))
- Instruments voisins : `docs/corpus/fallacies-exemples-diff.py` (G2, `example_fr`) · `docs/corpus/fallacies-desc-diff.py` (G3, `desc_fr`)
