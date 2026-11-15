# #1471 P24-23 — classes de parenté des 219 collisions, et la liste des groupes « autres »

P24-23 du pool #458, suite directe de l'audit P24-22 ([`1471-redirect-audit-2026-11-15.md`](1471-redirect-audit-2026-11-15.md)) et de la décision de coordination du 15/11 : sur les 219 groupes de collision d'article d'arrivée, un doublon carte↔ancêtre, entre frères ou entre deux exemplaires du même sophisme est **accepté tel quel** ; seuls les groupes de **nœuds sans parenté** se jugent un par un dans les lots. Ce dossier publie la liste de ces groupes. **Zéro écriture CSV** : les listes sont le terrain d'arbitrage, aucune correction n'est proposée ici.

- **Date** : 2026-11-15 · **Base** : `b3923220` (master après #2280) · **Outil** : `tools/1471-redirect-audit.py --classes` (P24-23, ajout de la classification).
- **Nature des chiffres** : *mesurés* par l'outil sur le corpus de la base ; la reproduction est en fin de dossier.

## 1. La règle mécanique

`--classes` classe chaque groupe de collision (l'ensemble des PK dont le lien mène au même article d'arrivée, même fragment) par la parenté **mécanique** de ses nœuds, avec la précédence suivante :

| Classe | Règle |
|---|---|
| `carte-ancetre` | une paire du groupe est en relation d'ascendance : un chemin est préfixe **strict** de l'autre, à frontière de segment (`5.1` est préfixe de `5.1.2`, pas de `5.10`) |
| `freres` | tous les PK partagent le même parent de chemin (même profondeur) |
| `meme-sophisme` | une paire partage un **libellé normalisé** (casefold, `_`→espace) : `text_fr` **ou** `text_en` |
| `autres` | aucune des trois — **à juger un par un** |

La classe décrit la relation des **nœuds**. Le nombre de **cartes du deck** du groupe (colonne `carte` non vide) est imprimé à part : il trie les jugements, il ne décide pas de la classe.

## 2. Partition mesurée — 219 groupes

| Classe | Groupes |
|---|---:|
| `carte-ancetre` | **47** |
| `freres` | **16** |
| `meme-sophisme` | **118** |
| `autres` | **38** |
| **Total** | **219** |

Par colonne : fr 54 · en 80 · ru 13 · pt 13 · es 16 · ar 15 · fa 12 · zh 16 — les mêmes 219 que P24-22 (dont le témoin pt à 13).

### Réconciliation avec le résumé de coordination du 15/11 (41 / 16 / 117 / 45)

Le résumé de la décision annonce 41 carte↔ancêtre, 16 frères, 117 même sophisme, 45 autres. Mesuré ici : **47 / 16 / 118 / 38**. L'écart est de **6 groupes** (ancêtre → autres) et **1 groupe** (sophisme → autres). Les marges candidates sont visibles et nommées, pour que l'arbitrage porte sur les objets et non sur les totaux :

- **Marge « ancêtre » : les 8 groupes sans aucune carte du deck.** Sept groupes « micromanagement » (PK 499/500, un par colonne sauf fr : en, ru, pt, es, ar, fa, zh) et le groupe « appeal to motive » (en, PK 158/160/1382). Ce sont des paires nœud↔descendant où **aucun** des deux n'est une carte du deck ; si la règle veut qu'« une carte et son ancêtre » implique une carte, ils basculent en `autres` — ce qui donne 39/16/118/47, pas 41/16/117/45 : la règle exacte du 41 n'est pas re-dérivable du résumé seul.
- **Marge « sophisme » : les 7 groupes appariés sur `text_en` seul** — {460, 915} « Firehose of falsehood » (fr), {695, 1271} « Pseudoscience » (fr — les `text_fr` diffèrent d'un trait d'union, `Pseudo-science` vs `Pseudoscience`, donc seule la clé en les rapproche), {106, 1187} « Not invented here » (en), {476, 1300} « Spreading » (en), {654, 717} « Gambler's fallacy » (en), {1042, 1239} « Less-is-better effect » (en). Un seul de ces groupes suffit à expliquer l'écart de 1 ; le résumé ne dit pas lequel exclure.

**Le critère « deux cartes du deck ou plus »** : mesuré ici, les groupes `autres` se répartissent en **21 groupes à ≥ 2 cartes du deck** et 17 en dessous (le résumé annonce 25 — même marge de 4, cohérente avec les 7 groupes ci-dessus).

## 3. Les 38 groupes `autres` — 23 ensembles distincts

Les mêmes nœuds collisionnent souvent dans plusieurs colonnes : les 38 groupes se ramènent à **23 ensembles de PK distincts** (le jugement se prend par ensemble). `deck=` = nombre de cartes du deck dans le groupe ; `[ROUGE]` = article d'arrivée manquant (mesuré).

| # | Colonne | PK (deck) | Article d'arrivée | Les nœuds |
|---:|---|---|---|---|
| 1 | fr | 2 (1) · 598 (1) | « Généralisation abusive » | « Argument bâclé » / 3.1.1.1.1 « Généralisation hâtive » |
| 2 | fr | 160 (0) · 1382 (0) | « Procès d'intention » | 1.3.2.3.2 « Procès d'intention » · 7.3.2.2.1 « Appel au motif » |
| 3 | ru, pt, es, ar, fa, zh (6) | 43 (1) · 98 (1) | « Argumentum ad populum » (variantes locales) | 1.1.2.2 « Pratique courante » · 1.2.2.2 « Appel à la majorité » |
| 4 | fr, ru, pt, es, ar, fa, zh (7) | 182 (1) · 814 (1) | « Faux dilemme » (variantes locales) | 2.1.1.1.3 « Fausse alternative » · 5.1.2.2.4 « Faux dilemme » |
| 5 | fr | 233 (0) · 264 (0) | « Hyperbole (rhétorique) » | 2.1.2.2.2 « Exagération » · 2.1.3.2.4 « Hyperbole » |
| 6 | fr | 358 (1) · 1039 (0) | « Cadrage (décision) » | 2.3.1.1 « Cadrage » · 6.3.1.1.1.2.1 « Effet de cadrage » |
| 7 | fr | 579 (0) · 582 (0) | « Gestuelle des doigts et des mains » **[ROUGE]** | 2.3.3.4.2.6.8 « Pouce baissé » · 2.3.3.4.2.7.2 « Se tourner les pouces » |
| 8 | fr | 604 (0) · 657 (0) | « Erreur de conjonction » | 3.1.1.2.2 « Biais de représentativité » · 3.2.2.4 « Erreur de conjonction » |
| 9 | fr, en (2) | 702 (0) · 1328 (0) | « Ignoratio elenchi » / « Irrelevant conclusion » | 4.1.1.3 « Conclusion excessive » · 7.2.1.2.1 « Conclusion non pertinente » |
| 10 | fr | 768 (1) · 833 (1) | « Fausse équivalence » | 4.3.1.2 « Fausse équivalence » · 5.2 « Comparaison fallacieuse » |
| 11 | fr | 1020 (1) · 1151 (0) | « Coût irrécupérable » | 6.2.3.4 « Sophisme des coûts irrécupérables » · 6.3.1.2.3.1.1.3 « Biais des coûts irrécupérables » |
| 12 | en | 43 (1) · 98 (1) · 1326 (0) | « Argumentum ad populum » | 1.1.2.2 · 1.2.2.2 · 7.2.1.1.2.3 « Raison de la majorité » |
| 13 | en | 79 (1) · 1386 (0) | « Appeal to accomplishment » | 1.2.1.2.1 « Argument d'accomplissement » · 7.3.2.3 « Appel à la réussite » |
| 14 | en | 117 (0) · 315 (0) | « Moral high ground » | 1.2.3.3 « Supériorité morale » · 2.2.1.3.2 « Appel à l'élévation morale » |
| 15 | en | 187 (0) · 944 (0) | « Sound bite » | 2.1.1.4.2 « Petite phrase » · 6.1.2.1.1 « Accroche » |
| 16 | en | 268 (0) · 279 (0) | « Zeugma and syllepsis » | 2.1.3.3 « Adjonction » · 2.1.3.4.6 « Zeugma » |
| 17 | en, es, ar, zh (4) | 323 (1) · 844 (1) | « Association fallacy » (variantes locales) | 2.2.2.1 « Appel au mépris » · 5.2.3 « Sophisme d'association » |
| 18 | en | 390 (0) · 1040 (0) | « Contrast effect » | 2.3.1.2.3.1 « Jeu de contraste » · 6.3.1.1.1.2.1.1 « Effet de contraste » |
| 19 | en | 559 (0) · 570 (0) | « Raised fist » | 2.3.3.4.2.4.6 « Poing levé » · 2.3.3.4.2.5.7 « Tendre le poing serré vers le ciel » |
| 20 | en | 566 (0) · 579 (0) | « Thumb signal » | 2.3.3.4.2.5.3 « Pouce levé » · 2.3.3.4.2.6.8 « Pouce baissé » |
| 21 | en | 635 (0) · 725 (0) | « Post hoc ergo propter hoc » | 3.2.1.2 « Confusion entre antériorité et causalité » · 4.1.3.4 « Appel à la temporalité comme cause » |
| 22 | ru | 0 (0) · 154 (1) | « Софизм » | racine « Argument fallacieux » · 1.3.2.1 « Sophisme du sophisme » |
| 23 | zh | 809 (1) · 973 (1) | « 歧義謬誤 » | 5.1.2.2.1 « Contraste perdu » · 6.2 « Changement de cap » |

Lecture : les ensembles 1, 3, 4, 10, 17, 23 portent **deux cartes du deck** — les candidats les plus lourds du jugement par lot. Les groupes 7 et 22 touchent des articles rouges ou la racine ; ils restent listés, jamais corrigés ici.

## 4. Ce que ce dossier ne fait pas

- **Aucune écriture du CSV** (lecture seule, comme P24-22).
- **Aucun jugement** des groupes : les lots trancheront, groupe par groupe, sur cette liste.
- L'écart avec les totaux du résumé du 15/11 est **déclaré, pas lissé** (§2) : la règle qui produit 41/117 n'est pas re-dérivable du résumé seul ; la règle mécanique de l'outil est celle du §1, et les marges sont nommées pour l'arbitrage.

## 5. Reproduction

```bash
python tools/1471-redirect-audit.py --self-test           # hors réseau (classes comprises)
python tools/1471-redirect-audit.py --classes             # audit complet + classes (~5 min, API)
python tools/1471-redirect-audit.py --classes --lang pt   # une colonne
```

La résolution des redirections est mise en cache disque (`.tmp/1471-redirect-audit-cache.json`, hors dépôt) : un second passage rejoue sans re-payer les titres déjà résolus au sens du cache (seuls `normalized`/`redirects`/`missing` y sont persistés — les titres arrivant à l'identique sont re-requêtés). L'auto-test couvre les quatre classes, la précédence et la frontière de segment.

*po-2024*
