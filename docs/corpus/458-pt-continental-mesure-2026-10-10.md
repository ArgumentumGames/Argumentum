# Pool #458 — mesure des marqueurs du portugais continental dans le corpus (grain ②.3, 0-écriture)

**Base** : `master` `fae1a63f`. **Instrument** : [`pt-continental-markers.py`](pt-continental-markers.py) (committé, rejoué depuis son emplacement — mêmes totaux que la passe de mesure). **Nature** : **mesure, aucune écriture** — `git status --porcelain Cards/` vide. **Dispatch** : ai-01 10/10 12:47Z ([#458 c.6096790964](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6096790964)) — « compter les marqueurs continentaux par corpus avec témoins, puis livrer volume et proposition. L'owner avait demandé un diagnostic propre, pas une bascule mécanique. »

## Synthèse

| Corpus | Rangées | Marqueurs PT (v2) | Rangées touchées | Témoins BR | Lecture |
|---|---:|---:|---:|---:|---|
| Fallacies | 1 408 | 27 hits | 23 | 1 445 (você dominant + fato/esporte/celular…) | mélange, fond BR, poches continentales |
| Scenarii | 167 | 15 hits | 15 | 25 (você, caminhão, ônibus, equipe) | idem |
| Virtues | 223 | 37 hits | 23 (35 cellules) | 33 (você) | **la poche la plus structurée : « factos »** |
| **Rules (témoin)** | 15 | **0** | **0** | 7 (equipe ×7) | **propre** — passe #1095 = BR |

**Verdict global** : le corpus penche **pt-BR** (registre `você` harmonisé partout ; Fallacies écrit « **fatos** » pk 947 là où Virtues écrit « factos » — 0 hit `facto` dans Fallacies ; Rules post-#1095 = zéro marqueur), avec une **couche continentale résiduelle : 79 occurrences / 73 cellules / 61 rangées** concentrée dans quatre poches — **3,4 % des 1 798 rangées** des trois corpus mesurés (Rules exclu, témoin). Ce n'est ni uniforme ni généralisé — « le deck penche pt-PT » (relevé ㉚) se raffine en : dominante BR, poches PT. (Comptes re-dérivés de la sortie instrument, pas additionnés de tête : une cellule peut porter deux marqueurs — pks 457 et 860 ci-dessous.)

## Les quatre poches (toute la liste, aucun échantillonnage)

### ① Virtues « facto(s) » — 34 hits / 32 cellules / 20 rangées, dont 6 libellés canoniques de taxonomie (tier C, mais massif)

`factos` traverse la colonne pt : pks 2, 3, 6, 11, 19, **22 (« Factos verificados » = un titre)**, 23, 28, 31, 132 (×3), 152, 167, 177, 181 — et surtout les **libellés de sous-familles** « **Fidelidade aos factos** » (pks 153, 154, 155, 156, 157, 158 — `subfamily_pt`) et « **Verdade dos factos** » (154, `subsubfamily_pt` + `title_pt`). BR écrit « fatos ». ⚠️ pk 181 : « factos **objectivos** » — orthographe **pré-AO** en plus (défaut d'orthographe historique distinct du dialecte, une occurrence).

### ② Le progressif « estar a + infinitif » — 17 cellules / 18 occurrences (tier B, la construction n'existe pas au Brésil : « está a falar » → « está falando »)

- **Fallacies** (6 cellules / 7 occurrences, toutes `example_pt`, voix de dialogue) : pk 390 (« estás a ouvir… estão a discutir » — deux fois), 457 (« Devem estar a confundir »), 458, 648 (« com medo de estar a desenvolver »), 860, 998.
- **Scenarii** (10, `suggestion_pt`/`context_pt`) : 1.2.6, 3.2.10, 3.2.14, 3.3.4, 4.1.9, 4.1.13, 4.1.14, 4.2.1, 7.2.10, 7.3.4.
- **Virtues** (1) : pk 96 `remark_pt` (« Está a chover », l'exemple du syllogisme).

### ③ Registre archaïque vós/vosso — 14 cellules Fallacies (15 occurrences) + 1 Scenarii (tier B, registre plus que dialecte)

- `desc_pt` ×10 (pks 394, 408, 463, 472, 947, 1053, 1056, 1170, 1171, 1298) — la description à la 2ᵉ personne du pluriel rendue par « vós/vossas » là où le BR attend « vocês/seus ».
- `example_pt` ×4 (pks 346, 453, 457, **808** ×2 occurrences — ce dernier + « ousas » pk 804 = les drapeaux du grain ㉚ item « registre pt/es »).
- Scenarii 5.2.1 : « sobre vós, se abater a vingança » — voix prophétique, **archaïsme stylistique vraisemblablement délibéré** (un dieu parle) ; à faire arbitrer, pas à « corriger » d'office.
- ⚠️ Deux cellules portent **deux marqueurs à la fois** : pk 457 `example_pt` (estar-a **et** vós) et pk 860 `example_pt` (estar-a **et** contactos) — d'où 73 cellules ≠ 79 occurrences.

### ④ Lexical isolé non ambigu (tier A) + 3 cellules B — 8 cellules, les plus visibles (titres et exemples imprimés)

| Cellule | Mot continental | Équivalent BR |
|---|---|---|
| Fallacies pk 621 `example_pt` | camião | caminhão |
| Fallacies pk 630 `example_pt` | ecrã | tela |
| Scenarii 3.1.4 `suggestion_pt` | pequeno-almoço | café da manhã |
| **Scenarii 3.2.9 `title_pt`** | **Desporto ou sofá ?** (titre de carte) | Esporte ou sofá? |
| Scenarii 4.3.6 `context_pt` | desporto | esporte |
| Fallacies pk 860 `example_pt` | contactos | contatos |
| Virtues pk 48 `remark_pt` | receção | recepção |
| Fallacies pk 1361 `example_pt` | « vi-o a aproveitar » (clitique de perception) | « o vi aproveitando » |

⚠️ Scenarii 3.1.4 est un **témoin de mélange intra-scénario** : sa `suggestion_pt` dit « pequeno-almoço » pendant que sa `issue_pt` dit « tomar um café » — deux générations dans la même carte.

Non classés défauts : `dossiê` ×2 (pk 848 Fallacies `example_pt`, pk 146 Virtues `remark_pt` — le mot existe dans les deux variantes, retieré C inoffensif) ; `factos` Scenarii 6.1.1 (registre juridique, « à partir des faits » — défendable en BR formel).

## Témoins et contrôles

- **Témoins absolus du dispatch VUS** : pk 621 « camião » ✓ et pk 1361 « vi-o a » ✓ — l'instrument voit les instances déjà connues (relevé ㉚).
- **Témoin négatif de calibration** : Rules, retraduit par la passe #1095 (pt-BR par décision owner 14/08), rend **0 marqueur continental et 7 témoins BR** — le scanner distingue bien un corpus nettoyé de trois corpus non-nettoyés.
- **Auto-test** : 11 fixtures (dont « convencê-lo a continuer » et « Vossa Majestade » qui ne DOIVENT PAS lever, et une rangée 100 % BR attendue vide) + rangée synthétique levant A et B.
- **Contrôle inverse (v1 → v2)** : la lecture intégrale des hits v1 a révélé deux classes de faux positifs — « clitique + a + INF » attrapait la **préposition** (« convencê-lo a continuar » = les deux variantes ; seuls les verbes de **perception** comme « vi-o a » sont continentaux), et « Vossa Majestade » (honorifique figé). v2 restreint/exclut ; Scenarii passe de 26 à 15 hits. La leçon est dans l'en-tête de l'instrument committé.

## Volume et proposition

**Volume** : 61 rangées touchées / 73 cellules / 79 occurrences sur les 1 798 rangées des trois corpus mesurés (Rules exclu, témoin) — **≈ 3,4 %**, en quatre poches de tailles inégales. Aucune régénération d'assets n'est déclenchée par la seule correction CSV (les champs touchés sont imprimés par les gabarits Desc/Example/Scenarii : une régén suivra le geste, pas le précède).

**Proposition** (l'owner arbitre ; rien n'est exécuté dans ce grain) :

1. **Poches ① et ④ d'abord** — déterministes et peu nombreuses : les 5 lexicaux A + contacto/receção/vi-o (8 cellules) et la décision de nommage Virtues « Fidelidade aos factos → Fidelidade aos fatos » (6 libellés de sous-famille + 1 sous-sous-famille + 2 titres + 23 cellules desc/remark = 32 cellules). Le nommage est une décision **owner** (libellé canonique de taxonomie, affiché sur cartes et mindmaps) ; le texte suit le nommage.
2. **Poche ② ensuite** — 17 cellules « estar a » : transformation régulière (estar a + INF → estar + gérondif), mais à confier à une **passe LLM scopée** avec les prompts désormais pt-BR (prérequis #1855) plutôt qu'à un regex — la proclisis peut devoir suivre.
3. **Poche ③ = grain registre, pas dialecte** — router vers l'item « registre pt/es » déjà ouvert (㉚ l'avait pointé : 804, 808). Le vós de Scenarii 5.2.1 (voix divine) se présente comme **choix stylistique à garder**, à trancher au même arbitrage.
4. **Aucune bascule mécanique globale** — le fond du corpus est déjà BR ; toucher 96,6 % de rangées saines pour 3,4 % de poches serait le geste que l'owner a explicitement écarté.

## N'établit pas

- Que les 61 rangées listées soient les **seules** dérives pt-PT : l'instrument couvre un jeu de motifs déclaré (en-tête du script) ; un marqueur hors liste (p. ex. proclisis systématique, « aeroplano », orthographe pré-AO au-delà de pk 181) lui est invisible.
- Que « fato/factos » BR soit *meilleur* : c'est la cible de la décision owner #1095, rien de plus.
- Aucune lecture native : le tiering lexico-grammatical suit des grammaires de référence, pas un locuteur.
- Aucune écriture n'a eu lieu et aucune n'est proposée hors arbitrage.

## Reproductibilité

```bash
python docs/corpus/pt-continental-markers.py --self-test
python docs/corpus/pt-continental-markers.py            # dépôt déduit de __file__
python docs/corpus/pt-continental-markers.py --repo=<checkout>
```

Sortie attendue sur `fae1a63f` : Fallacies 27 / Scenarii 15 / Virtues 37 / Rules 0 — A=5, B=37, C=37.

⛔ Gel `v2.0.0-review` respecté — aucune republication.
