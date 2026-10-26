# Pool v23 — série ㉗ : fidélité des **exemples** chinois du deck (mesure 0-écriture)

**Base** : `master` `0f408864`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), `--field example` (capacité portée par la branche #1724, re-basée sur ce master — rejouer depuis sa tête tant qu'elle n'est pas mergée). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Second grain de la série des exemples (dispatch c.5968567990). Verdict court : **colonne saine** — aucun contresens ni contenu manquant en lisant les 168 non vides, et un fait structurel la distingue de toutes les précédentes : **`example_zh` n'existe dans AUCUNE archive** (`v3`, `v3pp`, `2022`, `2022pp` — « REFERENCE EXPLOITABLE: 0/175 »). Il n'y a donc **pas d'arbitre imprimé** : la mesure est une lecture FR↔zh pure, et une éventuelle correction n'aurait pas d'imprimé auquel revenir.

## Synthèse — 0 A / 0 M / 16 rangées observées / 152 ✓ (168 non vides)

> **Erratum de périmètre complété (03/10, ai-01, [c.5971513525](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5971513525) ; complété 04/10, review c.5972921914).** L'erratum de périmètre ci-dessous citait 476 parmi les multi-lignes hors deck : sa cellule `example_fr` portait deux sauts de ligne **accidentels** (milieu de phrase, sans tiret de réplique), issus d'un retour à la ligne de mise en forme de l'EN (« impacts on vulnerable⏎populations ») que le FR avait traduit en deux morceaux. La jointure seule laissait « personnes vulnérables, populations » (mot orphelin en élément de liste) — **la review a imposé le texte de PK 1300**, même exemple, propre (PR grain 1, tête reprise). 65 et 1300 restent hors deck, non mesurés.

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** (aucune candidate) |
| **M — contenu manquant ou ajouté** | **0** (3 gloses d'intraduisibilité consignées avec frontière, précédent ru c.5968567847) |
| **C-note — observation consignée, aucune écriture proposée** | **16** |
| **✓ — fidèle** | **152** |
| État voulu (gloses G2-C-W2 restaurées, gate-épinglées) | 2 (796, 848) |
| Vide structurel (têtes de famille : `example_fr` vide aussi) | 7 (PK 1, 175, 594, 696, 798, 887, 1280 — mêmes que ru ㉖) |

**Les 168 rangées non vides ont été lues, aucun échantillonnage.**

⚠️ Faiblesses structurelles, dites avant le verdict : lecture **unique** par un **lecteur non natif** ; **aucune référence imprimée** (ci-dessus) — contrairement au ru ㉖ dont l'archive servait à trancher les coquilles, ici rien ne peut trancher qu'une relecture.

## Drapeaux — 44, tous lus

- **35 `POLARITY`** : **parité respectée 35/35**. Mécanismes récurrents (négation lexicalisée dans un mot positif : 了不起 « remarquable », 非凡 « extraordinaire », 不同 « différent », 不忠 « infidélité », 不过 « cependant », 而非 « plutôt que », 不变 « inchangé », 不小心 « par inadvertance » ; restrictif `ne…que` vs 只/一定 ; question rhétorique 怎么可能/难道 vs counterfactual FR). Le détecteur compte des **caractères**, pas des **polarités** — mémoire « un détecteur de signature n'est pas neutre entre les langues » : sur zh exemples, son taux de FP est de 100 %.
- **7 `EMPTY-target`** : les vides structurels ci-dessus.
- **2 `LENDEV+CLAUSES`** : PK 796 et 848 — **gloses restaurées G2-C-W2** (Q-16 (c)), état **voulu**, épinglé par `FallaciesGlossRegisterGate` (l'instrument les re-signe à chaque passe : comportement correct, pas une dérive).

## C-notes — 16 rangées, 4 familles, aucune écriture proposée

### 1. Gloses d'intraduisibilité (3) — même nature que les gloses ru « avocat », arbitrées gardées

| PK | Jeu de mots FR | Ce que fait le zh | Précédent |
|---|---|---|---|
| 846 | « le portrait de la dame **en noir** » | rend la phrase **puis** glose les deux lectures (« 可能是指那位女士，也可能指他用黑色画了肖像 ») | ru 847/855 gardées (c.5968567847) |
| 847 | « le poulet **aux** olives » (à/préférence) | idem (« 可以是胜过橄榄，也可以是配橄榄的鸡肉 ») — l'**en imprimé porte la même glose** (« —or chicken over olives ») | ru 847 gardée |
| 855 | « un **avocat** » (fruit/métier) | glose « 可能是一个律师，也可能是在吃鳄梨 » — note : le zh dit « avocat » d'abord **métier**, le FR joue fruit d'abord | ru/ar 855 gardées |

Frontière M douce : sans la glose, la carte 846/847/855 ne démontre plus l'ambiguïté qu'elle illustre. **Proposition : garder, sur le précédent ru** — arbitrage ai-01.

### 2. Écarts de registre ou de portée, sens conservé (9)

- **PK 595** — « 我 **认为**猫都是无害的 » : le hedging « je pense » est **ajouté** (FR : « les chats SONT donc »).
- **PK 707** — « **有些人认为**使用轮椅是危险的 » : attribution à « certains » ajoutée (FR asserte).
- **PK 699** — « 它不会错 » (« elle ne peut pas se tromper ») : **explicitation** de la prémisse implicite circulaire.
- **PK 659** — « Quel **mal** » → « 有什么**区别** » (« quelle différence ») : adoucissement, la rhétorique minimisante survit.
- **PK 977** — « **n'empêche nullement** les accidents » → « **不能防止所有的**交通事故 » (« ne peut pas empêcher TOUS les accidents ») : portée du quantificateur changée (absolu → partiel). La chute argumentative survit, la force logique non. Frontière.
- **PK 1345** — « Je **ne vois pas** comment » → « 我认为**很难** » (« il me semble difficile ») : adoucissement.
- **PK 79** — « quand vous aurez sorti **un album** » → « **白金唱片** » (disque de platine) : renforcement rhétorique.
- **PK 154** — les deux membres de la contradiction sont **miroirs** (FR : route qui descend/qui monte ↔ zh : 上坡/向下) — la chute (ses indications sont fausses → partons à gauche) est intacte.
- **PK 726 / 1023** — « fromage » → « 瑞士奶酪 » (fromage à trous, clarifie le mécanisme) ; « Pour moi » non rendu. Triviaux.

### 3. Spécifications sans perte (2)

- **PK 809** — « substance chimique » → « 化学**粉末** » (poudre chimique).
- **PK 1023** ci-dessus.

### 4. Observation côté FR, hors colonne zh et hors deck (2 rangées, 1 note)

- **PK 476 et 1300** (Répandage / Déluge argumentatif, même exemple — rangées **hors deck**, `carte` vide, rencontrées au contrôle croisé et non dans la population mesurée) : le **FR lui-même** porte un artefact de découpe — « les impacts sur les personnes vulnérables⟨saut de ligne⟩populations » — la ligne « populations » orpheline tombe au milieu de la phrase. Le zh traduit le texte **cohérent** (弱势群体). Défectueux côté FR, consigné ici sans écriture.

## Contrôle inverse (copie scratchpad, dépôt jamais touché — `git status Cards/` vide vérifié)

1. **Inversion de polarité plantée** (PK 43 : « 不应该受到惩罚 » → « 应该受到惩罚 » — « ne devrait pas être puni » → « devrait être puni ») : `POLARITY` **lève** ✓ — l'écran vit sur ce corpus.
2. **Troncature à 50 % plantée** (PK 796, 105→52 car.) : `LENDEV` **ne lève pas** ✗ — la cellule tronquée retombe **dans la bande calibrée** (pente 0,204, σ 9,8 : −2σ ≈ 20 car.), parce que l'originale était flaguée **longue** (glose). L'écran de longueur est structurellement aveugle aux troncatures qui normalisent une cellule anormalement longue. **La lecture intégrale reste le seul filet pour cette classe** — c'est précisément elle qui a fonctionné ci-dessous.
   - NB : la passe sur copie voit sa jointure dégrader à 0/175 (archives copiées) — le contrôle ne porte que sur les écrans, affichés avant la jointure.

## L'instrument de lecture a trahi avant le corpus — l'incident du grain

La première passe de lecture (fichier compact généré du rapport d'instrument) **tronquait les cellules multi-lignes** : le lecteur regex s'ancre sur une ligne, une cellule à sauts de ligne n'en montre que la première. Lu ainsi, PK 813 « vrai Écossais » semblait ne porter qu'une réplique sur trois (dialogue amputé = candidat M sérieux). **Contrôle croisé contre le CSV : les 3 cellules multi-lignes du deck (813, 943, 974) sont complètes** — le défaut était dans l'extracteur, pas dans le corpus. L'alarme est **retractée**, et la leçon consignée : *une cellule CSV lue par extraction de ligne n'est lue que si elle tient sur une ligne* — croiser contre la source avant de conclure « manquant ».
*(Erratum de périmètre, 03/10 : la rédaction initiale citait « les 6 cellules multi-lignes (65, 476, 813, 943, 974, 1300) » — l'ensemble CSV. **65, 476 et 1300 sont hors deck** (`carte` vide), hors de la population mesurée ; la population multi-lignes du deck est **813/943/974**, toutes trois vérifiées complètes.)*

## N'établit pas

- **Que la colonne zh soit sans défaut** — lecture unique, non native, sans arbitre imprimé.
- **Que les gloses 846/847/855 soient gardées** — proposition sur précédent ru, arbitrage ai-01.
- **Que les 35 FP POLARITY soient exhaustifs** des mécanismes — ils couvrent les 35 levés, pas l'espace des levables.
- **Rien sur** les 7 vides structurels (décision éditoriale, ru ㉖ §v), les champs `link_*`, ni les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.**

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang zh --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175) ; corpus du dépôt **jamais écrit**.
- Sortie mesurée : 175 cartes, 44 drapeaux, jointure 168/175 (157 nom + 11 position) — jointure inchangée depuis ⑳ (indépendante du champ mesuré).
- ⚠️ Nécessite la tête de #1724 tant que le paramètre `--field` n'est pas sur master.
