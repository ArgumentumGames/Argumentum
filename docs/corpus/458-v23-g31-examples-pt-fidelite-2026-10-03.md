# Pool v23 — série ㉚ : fidélité des **exemples** portugais du deck (mesure 0-écriture)

**Base** : `master` `0f408864`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), `--field example` (capacité portée par la branche #1724 — rejouer depuis sa tête tant qu'elle n'est pas mergée). **Nature** : mesure, **aucune écriture** — `git status --porcelain Cards/` vide après la passe.

Cinquième grain de la série des exemples (dispatch c.5968567990). **Premier grain avec un arbitre imprimé** : `example_pt` est présente dans toutes les archives (REFERENCE EXPLOITABLE 157/175) — et c'est précisément ce qui change la nature du grain : **155 des 157 cellules rattachées DIFFÈRENT de l'imprimé** (2 IDENTIQUE). La lecture intégrale montre pourquoi : ce ne sont pas deux états d'une même colonne, ce sont **deux générations**. Verdict deck : **colonne saine** (0 A, 0 M).

## Synthèse — 0 A / 0 M / 11 rangées observées / 157 ✓ (168 non vides)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **11** (dont 796, état restauré gate-épinglé, cf. §4) |
| **✓ — fidèle** | **157** |
| Vide structurel (têtes de famille, `example_fr` vide aussi) | 7 (PK 1, 175, 594, 696, 798, 887, 1280 — mêmes que ru/zh/ar/fa) |

**Les 168 rangées non vides ont été lues, aucun échantillonnage** (cellules multi-lignes 813/943/974 lues entières — leçon ㉗).

## L'imprimé pt — deux générations, pas deux états (le fait du grain)

| Mesure deck ↔ imprimé | Cellules |
|---|---:|
| `IDENTIQUE` | **2** (PK 79 et 313 — courtes phrases où deux générations peuvent coïncider) |
| `[DIFFERE]` | **155** |

Contraste avec le ㉖-ru (même instrument, même jointure) : **51 IDENTIQUE / 98 DIFFERE**. Deux colonnes imprimées, deux comportements : la ru est la même génération (écarts de mot), la pt est une **génération antérieure**.

**Ce que porte la colonne imprimée** (lecture des 155) :

- **7 × `#VALUE!`** — les 7 rangées vides : l'archive a gardé l'erreur de tableur telle quelle ;
- machine gâtée : « entrei em um cães de cães » (PK 2, là où le deck dit « pisei em cocô de cachorro ») ; « Sócrates… os gatos são fatais » (796) ; « Bateria que eu ganho. Rosto você perde » (182, pour pile/face) ; « Paris! Paris indignado!… » (247) ; « Nicolas Cage… piscinas » (632) ; casino/suco de tomate (636) ; « não há chance de ele espalhar manteiga suave » (845) ; « seu radar eletromagnético » (1345)…
- **38 cellules imprimées dont le contenu ne correspond à aucun exemple du deck actuel** (l'exemple a manifestement été remplacé depuis — lecture) : PK **33, 112, 134, 176, 185, 247, 300, 304, 322, 340, 356, 357, 432, 598, 632, 636, 653, 698, 713, 733, 787, 796, 800, 802, 834, 839, 844, 845, 908, 973, 1015, 1287, 1297, 1360, 1365, 1373, 1388, 1398** ;
- variante locale mesurée : le deck penche **pt-PT** là où l'imprimé porte du pt-BR (PK 621 : deck « camião… automóvel » / imprimé « caminhão… carro » ; PK 1361 deck « vi-o a aproveitar ») — sans être uniforme (« bolinhas de gude », forme BR, reste en 1330, des deux côtés).

**Datation (pickaxe, première apparition dans l'historique du CSV)** : « pisei em cocô… » → `e8482fe5` (2025-07-02) ; « camião consome mais combustível » → `5e2477b5` (2026-05-31). La colonne deck est une écriture/retouche **2025-2026**.

⇒ Conséquence pour la série : **« exploitable » (colonne présente) n'est pas « arbitre »**. La doctrine du ㉖-ru (« pour une carte imprimée, le défaut est de revenir à l'imprimé », arb. c.5968567847) ne se transpose pas aux exemples pt : aucun arbitrage cellule-à-cellule n'est proposé depuis cette archive. Les 155 écarts sont des écarts de **génération**, pas des dérives du deck.

## Jumeaux inter-langues — le fait structurel du grain

pt rejoint **trois** familles de déviations déjà cartographiées, et ce sont exactement celles où un « nous » collectif ou une reformulation de voix apparaît :

| PK | Famille | zh | ar | fa | pt |
|---|---|---|---|---|---|
| 154 | membres **miroirs** (« monte/descend » ↔ « descend/monte ») + « **nous** a dit » (le FR dit « m'a dit ») | miroir + 我们 | miroir + لنا | fidèle (من, descend→monte) | **miroir + nos** |
| 595 | hedging « **je pense** » ajouté | 我认为 | أعتقد أنّ | fidèle | **Acho que** |
| 699 | restructuration (« Dieu existe parce que la Bible le dit, et elle ne peut se tromper ») | ✓ | fidèle | ✓ | **✓** |
| 638 | clarifie « une même entité » | 外星人 | كائن غريب | موجود فرازمینی | **« civilização antiga »** + « indevidamente » ajouté |
| 977 | « n'empêche **nullement** » affaibli | 不能防止所有的 | لا تمنع جميع | تمامی...نمی‌کند | **« não eliminou os acidentes mortais »** (passé + restriction) |
| 726 | « fromage » spécifié | 瑞士奶酪 | الجبن السويسري | پنیر سوئیسی | **gruyère** — et l'imprimé pt dit déjà « queijo Gruyère » (spécification antérieure au deck, pas une déviation nouvelle) |

Et pt reste **fidèle là où les six triples zh+ar+fa dévient ensemble** : 659, 707, 814 (et 994). Détail decisif sur 154 : **l'imprimé pt est fidèle au FR** (« para a direita… esta estrada é uma escalada ») — le miroir du deck n'est donc pas hérité de l'imprimé pt ; il vient de la même source que zh et ar. Toute arbitration sur 154, 595, 699, 638, 977 doit trancher **toutes** les langues listées ensemble.

## Drapeaux — 23, tous lus

- **7 `EMPTY-target`** : vides structurels (et `#VALUE!` côté imprimé).
- **14 `POLARITY`** : **parité respectée** (não/sem/nunca vs négations FR, dont restrictifs « ne… que » — le détecteur compte des caractères ; FP par construction, même famille que zh/ar/fa).
- **2 `NUM-only-FR`** : 614 et 644 — le pt écrit les nombres **en lettres** (« cento e trinta », « dez por cento ») ; convention, raisonnement intact. FP.
- **1 paire `LONG(2.68) + CLAUSES(6>3)`** : 796 — **état restauré gate-épinglé** (G2-C-W2, glose « avocat » ; `RestoredExceptions` inclut `796:pt`).
- **1 paire `SHORT(0.60) + POLARITY`** : 1345 — condensation lue, le mécanisme (« trop de variables ») est conservé ; FP documenté.
- ⚠️ **Vocabulaire d'écran** : sur les colonnes **latines** les écrans de longueur s'appellent `LONG/SHORT` (ratio contre la bande classique) ; l'écran `LENDEV` (σ contre la bande **calibrée par écriture**, apport #1722) ne s'applique qu'aux colonnes CJK/arabe — c'est pourquoi ㉗-㉙ et ce grain n'affichent pas les mêmes noms. Pas de ligne `CALIBRATION` en pt : normal, colonne latine.
- Reproductibilité prouvée : re-passe saine **byte-identique** aux lignes `FLAGS` de la mesure initiale (diff vide).

## Contrôle inverse (copie mutée via `--csv`, dépôt jamais écrit — `git status --porcelain` vide)

1. **Polarité inversée plantée** (PK 43 : « isso **não** deveria ser penalizado » → « isso deveria ser penalizado ») : `POLARITY` **lève** ✓.
2. **Troncature 50 % plantée** (PK 796, 324 → 162 car., cellule à glose) : la ligne de drapeaux **disparaît entièrement** (`LONG` **et** `CLAUSES`) ✗ — les deux écrans sont **structurellement aveugles** à une troncature qui « normalise » une cellule anormalement longue. Même famille qu'au ㉗ (`LENDEV`), étendue au vocabulaire latin et à `CLAUSES`.
3. **Témoin sain** (PK 2, 139 car., sans drapeau) : reste `FLAGS: -` ✓.
4. ⭐ **Le compte global est identique dans les deux passes (23)** : +1 (PK 43) et −1 ligne (PK 796, qui portait 2 drapeaux sur une seule ligne) s'annulent. **Un contrôle par totaux n'aurait rien vu** — c'est le diff par PK qui nomme les deux échanges.

## C-notes — 11 rangées, aucune écriture proposée

### 1. Jumeaux (6) — voir le tableau ci-dessus

154, 595, 699, 638, 977, 726.

### 2. Jeu de mots « avocat » : deux stratégies dans la même colonne (1)

- **PK 855** — « O urso comeu uma **manga** » : le deck pt choisit un **autre fruit** (mangue) — l'avocat disparaît, sans glose — alors que **PK 796 pt** a choisi la translittération + glose (« avocat » expliqué). Même jeu de mots, deux stratégies dans la même colonne ; l'imprimé ne peut pas arbitrer (position seule, `PAS un arbitre imprime`). Consigné pour arbitration future, aucun geste.

### 3. Registre hétérogène (2)

- **PK 804** — « Como então **ousas** me censurar » (forme *tu*, littéraire) et **PK 808** — « Eu **adoro-vos**, portanto **não vos amo** » (forme *vós*, archaïque), au milieu d'une colonne dominée par *você/vocês*. À porter au grain **registre pt/es** (dispatch, item 4) — c'est exactement la mesure « une carte, deux registres » qui y est demandée.

### 4. État restauré gate-épinglé (1)

- **PK 796** — glose restaurée G2-C-W2 (`796:pt` dans `RestoredExceptions`) ; `LONG+CLAUSES` = conséquence mécanique de la glose, pas un défaut. Rien à proposer.

### 5. Dérive de détail (1)

- **PK 596** — le deck dit « 80 % dos nossos clientes declaram que este iogurte tem a melhor **relação preço-qualidade** » là où le FR dit « plébiscité par 80 % des consommateurs interrogés dans notre magasin » : la **dérive** est « relação preço-qualidade » (notion ajoutée) ; le **mécanisme** (échantillon biaisé : « nossos clientes ») survit. L'imprimé, lui, est plus proche sur « o melhor » mais perd aussi « dans notre magasin » (« dos consumidores »). Consigné, aucun geste.

## N'établit pas

- **Que la colonne pt du deck soit sans défaut** — lecture unique, non native.
- **Que la génération deck soit « meilleure partout »** — elle est **fidèle au FR courant**, c'est ce qui est mesuré ; le contenu des 38 cellules imprimées sans correspondance est décrit, pas jugé.
- **La généalogie des jumeaux** — l'ancêtre commun (deck EN d'époque, conclusion ㉕) n'a pas été relu pour ce grain ; la source du miroir de 154 en pt n'est pas établie (seulement : il ne vient **pas** de l'imprimé pt, qui est fidèle).
- **Que les 155 écarts soient des défauts** — ce sont des écarts de génération (7 `#VALUE!` et ~38 contenus remplacés sont documentés comme tels).
- **Rien sur** les 7 vides structurels, les `link_*`, les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.**

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang pt --out <sortie>
python docs/corpus/definitions-fidelity-instrument.py --field example --lang pt --csv <copie-mutée> --out <sortie-contrôle>
```

- 175 cartes lues, 23 drapeaux, jointure 168/175 (157 nom + 11 position) ; REFERENCE EXPLOITABLE 157/175 (colonne présente) ; pas de ligne `CALIBRATION` (colonne latine).
- ⚠️ Nécessite la tête de #1724 (`--field`, `--csv`) tant qu'elle n'est pas sur master.