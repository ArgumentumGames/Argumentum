# #1471 P24-23 — classes de parenté des 219 collisions, et la liste des groupes « autres »

P24-23 du pool #458, suite directe de l'audit P24-22 ([`1471-redirect-audit-2026-11-15.md`](1471-redirect-audit-2026-11-15.md)). **Reprise du 15/11 soir** ([c.6899134834](https://github.com/ArgumentumGames/Argumentum/pull/2282#issuecomment-6899134834)) : la première version classait un groupe dès qu'**une paire** de membres était apparentée — elle acceptait des groupes dont un membre n'est relié à aucun autre. La règle actée ici est **par connexité** : un groupe n'est accepté que si ses membres forment **un seul bloc**. **Zéro écriture CSV** : les listes sont le terrain d'arbitrage, aucune correction n'est proposée ici.

- **Date** : 2026-11-15 (reprise du soir) · **Base** : `b3923220` (master après #2280 ; les colonnes `link_*` sont inchangées sur `26572548`) · **Outil** : `tools/1471-redirect-audit.py --classes` (règle par connexité).
- **Nature des chiffres** : *mesurés* par l'outil sur le corpus de la base ; la reproduction est en fin de dossier.

## 1. La règle par connexité

`--classes` relie deux à deux les membres d'un groupe de collision (l'ensemble des PK dont le lien mène au même article d'arrivée, même fragment) par les trois relations acceptées :

| Relation | Règle |
|---|---|
| ascendance | un chemin est préfixe **strict** de l'autre, à frontière de segment (`5.1` est préfixe de `5.1.2`, pas de `5.10`) |
| fratrie | même parent de chemin |
| même libellé | `text_fr` **ou** `text_en` normalisés égaux (casefold, `_`→espace) |

- Le groupe est **accepté** si ce graphe ne forme **qu'une seule composante** : chaque membre est relié au reste, directement ou en chaîne. Sa classe suit la précédence `carte-ancetre` > `freres` > `meme-sophisme` (sur les relations **présentes** dans le bloc, chaînes mixtes comprises).
- Sinon, c'est `autres` : **au moins un membre est isolé, et c'est précisément lui que les lots doivent juger**.

La classe décrit la relation des **nœuds**. Le nombre de **cartes du deck** du groupe (colonne `carte` non vide) est imprimé à part : il trie les jugements, il ne décide pas de la classe.

**Pourquoi la connexité et pas « une paire suffit » ni « tous liés à tous ».** Les deux règles extrêmes échouent chacune sur un côté mesuré : « une paire suffit » (première version de ce dossier, tête `dc6e4f42`) acceptait **6 groupes contenant un membre isolé** ; « tous liés à tous » (règle du résumé de coordination du 15/11 matin) excluait à tort un bloc relié en chaîne (fr « Corrélation illusoire » : 633 ancêtre de 642, 642 jumeau de 1085 — chaque membre est relié, aucun n'est l'ancêtre de tous). La connexité est la lecture fidèle de la décision du 15/11 : **aucun membre ne reste sans lien**.

## 2. Partition mesurée — 219 groupes

| Classe | Groupes |
|---|---:|
| `carte-ancetre` | **42** |
| `freres` | **16** |
| `meme-sophisme` | **117** |
| `autres` | **44** |
| **Total** | **219** |

Par colonne : fr 54 · en 80 · ru 13 · pt 13 · es 16 · ar 15 · fa 12 · zh 16 — les mêmes 219 que P24-22 (dont le témoin pt à 13). **44** groupes `autres` = **28 ensembles de PK distincts**, dont **25 groupes** à au moins deux cartes du deck.

### Les 6 groupes qui changeaient avec « une paire suffit »

Mesurés par ai-01 dans la revue et reproduits ici par la reprise :

| Colonnes | Article d'arrivée | Ancienne classe | Membre(s) isolé(s) |
|---|---|---|---|
| en, ar, zh (3 groupes) | Faulty generalization et traductions | carte-ancetre | **PK 2** « Argument bâclé », dont le `link_en` est resté `Hasty_generalization`, le titre anglais de son ancien nom d'avant #1522 |
| en | Correlation does not imply causation | carte-ancetre | **634, 707** |
| en | Appeal to motive | carte-ancetre | **1382** |
| fr | Régression à l'infini | meme-sophisme | deux composantes : **662/664** (frères) et **979/1349** (jumeaux) |

Le premier cas porte trois cartes du deck (2, 595, 598) : c'est exactement le défaut que les lots doivent attraper.

## 3. Les 44 groupes `autres` — 28 ensembles distincts

Les mêmes nœuds collisionnent souvent dans plusieurs colonnes : les 44 groupes se ramènent à **28 ensembles de PK distincts** (le jugement se prend par ensemble). `deck=` = nombre de cartes du deck dans le groupe ; `[ROUGE]` = article d'arrivée manquant (mesuré).

| # | Colonnes | PK (deck) | Article d'arrivée | Les nœuds |
|---:|---|---|---|---|
| 1 | fr | 2 (1) · 598 (1) | « Généralisation abusive » | 1.1 « Argument bâclé » · 3.1.1.1.1 « Généralisation hâtive » |
| 2 | fr | 160 (0) · 1382 (0) | « Procès d'intention » | 1.3.2.3.2 « Procès d'intention » · 7.3.2.2.1 « Appel au motif » |
| 3 | fr, ru, pt, es, ar, fa, zh (7) | 182 (1) · 814 (1) | « Faux dilemme » (variantes locales) | 2.1.1.1.3 « Fausse alternative » · 5.1.2.2.4 « Faux dilemme » |
| 4 | fr | 233 (0) · 264 (0) | « Hyperbole (rhétorique) » | 2.1.2.2.2 « Exagération » · 2.1.3.2.4 « Hyperbole » |
| 5 | fr | 358 (1) · 1039 (0) | « Cadrage (décision) » | 2.3.1.1 « Cadrage » · 6.3.1.1.1.2.1 « Effet de cadrage » |
| 6 | fr | 579 (0) · 582 (0) | « Gestuelle des doigts et des mains » **[ROUGE]** | 2.3.3.4.2.6.8 « Pouce baissé » · 2.3.3.4.2.7.2 « Se tourner les pouces » |
| 7 | fr | 604 (0) · 657 (0) | « Erreur de conjonction » | 3.1.1.2.2 « Biais de représentativité » · 3.2.2.4 « Erreur de conjonction » |
| 8 | fr | 662 (0) · 664 (0) · 979 (0) · 1349 (0) | « Régression à l'infini » | deux composantes : 3.2.3.2.1 « Régression infinie vicieuse » + 3.2.3.2.2 « Mauvais argument de la… » · et 979/1349 « Justification infinie » (jumeaux) |
| 9 | fr, en (2) | 702 (0) · 1328 (0) | « Ignoratio elenchi » / « Irrelevant conclusion » | 4.1.1.3 « Conclusion excessive » · 7.2.1.2.1 « Conclusion non pertinente » |
| 10 | fr | 768 (1) · 833 (1) | « Fausse équivalence » | 4.3.1.2 « Fausse équivalence » · 5.2 « Comparaison fallacieuse » |
| 11 | fr | 1020 (1) · 1151 (0) | « Coût irrécupérable » | 6.2.3.4 « Sophisme des coûts irrécupérables » · 6.3.1.2.3.1.1.3 « Biais des coûts irrécupérables » |
| 12 | en | 2 (1) · 595 (1) · 598 (1) · 1123 (0) | « Faulty generalization » | 1.1 « Argument bâclé » · 3.1 « Généralisation abusive » · 3.1.1.1.1 « Généralisation hâtive » · 1123 |
| 13 | en | 43 (1) · 98 (1) · 1326 (0) | « Argumentum ad populum » | 1.1.2.2 · 1.2.2.2 · 7.2.1.1.2.3 « Raison de la majorité » |
| 14 | en | 79 (1) · 1386 (0) | « Appeal to accomplishment » | 1.2.1.2.1 « Argument d'accomplissement » · 7.3.2.3 « Appel à la réussite » |
| 15 | en | 117 (0) · 315 (0) | « Moral high ground » | 1.2.3.3 « Supériorité morale » · 2.2.1.3.2 « Appel à l'élévation morale » |
| 16 | en | 158 (0) · 160 (0) · 1382 (0) | « Appeal to motive » | 1.3.2.3 « Appel à l'intention » · 1.3.2.3.2 « Procès d'intention » · 7.3.2.2.1 « Appel au motif » |
| 17 | en | 187 (0) · 944 (0) | « Sound bite » | 2.1.1.4.2 « Petite phrase » · 6.1.2.1.1 « Accroche » |
| 18 | en | 268 (0) · 279 (0) | « Zeugma and syllepsis » | 2.1.3.3 « Adjonction » · 2.1.3.4.6 « Zeugma » |
| 19 | en, es, ar, zh (4) | 323 (1) · 844 (1) | « Association fallacy » (variantes locales) | 2.2.2.1 « Appel au mépris » · 5.2.3 « Sophisme d'association » |
| 20 | en | 390 (0) · 1040 (0) | « Contrast effect » | 2.3.1.2.3.1 « Jeu de contraste » · 6.3.1.1.1.2.1.1 « Effet de contraste » |
| 21 | en | 559 (0) · 570 (0) | « Raised fist » | 2.3.3.4.2.4.6 « Poing levé » · 2.3.3.4.2.5.7 « Tendre le poing serré vers le ciel » |
| 22 | en | 566 (0) · 579 (0) | « Thumb signal » | 2.3.3.4.2.5.3 « Pouce levé » · 2.3.3.4.2.6.8 « Pouce baissé » |
| 23 | en | 634 (0) · 707 (1) · 719 (0) · 724 (1) | « Correlation does not imply causation » | 3.2.1.1 « Confusion corrélation/causalité » · 4.1.2 « Inversion de causalité » · 4.1.3 « Effet cigogne » · 4.1.3.2 |
| 24 | en | 635 (0) · 725 (0) | « Post hoc ergo propter hoc » | 3.2.1.2 « Confusion antériorité/causalité » · 4.1.3.4 « Appel à la temporalité comme cause » |
| 25 | ru | 0 (0) · 154 (1) | « Софизм » | racine « Argument fallacieux » · 1.3.2.1 « Sophisme du sophisme » |
| 26 | ru, pt, es, ar, fa, zh (6) | 43 (1) · 98 (1) | « Argumentum ad populum » (variantes locales) | 1.1.2.2 « Pratique courante » · 1.2.2.2 « Appel à la majorité » |
| 27 | zh | 809 (1) · 973 (1) | « 歧義謬誤 » | 5.1.2.2.1 « Contraste perdu » · 6.2 « Changement de cap » |
| 28 | ar, zh (2) | 2 (1) · 595 (1) · 598 (1) | « تعميم خاطئ » / « 草率歸納 » | 1.1 « Argument bâclé » · 3.1 « Généralisation abusive » · 3.1.1.1.1 « Généralisation hâtive » |

Lecture : les ensembles 3, 10, 12, 13, 19, 23, 26, 27, 28 portent **au moins deux cartes du deck** — les candidats les plus lourds du jugement par lot. Les groupes 6 et 25 touchent un article rouge ou la racine ; ils restent listés, jamais corrigés ici. L'ensemble 8 est le seul « autres » **à deux composantes internes** (deux paires reliées, sans lien entre elles) ; l'ensemble 12 est le seul où le membre isolé (PK 2) est un **reste d'ancien titre** mesurable (`link_en` = `Hasty_generalization`, d'avant #1522).

## 4. Ce que ce dossier ne fait pas

- **Aucune écriture du CSV** (lecture seule, comme P24-22).
- **Aucun jugement** des groupes : les lots trancheront, groupe par groupe, sur cette liste.
- L'historique de l'écart avec le résumé du 15/11 matin (41/16/117/45) est **documenté, pas lissé** (§1) : la règle « universelle » du résumé et la règle « une paire suffit » de la première version divergeaient toutes deux de la connexité sur les 20 groupes de trois nœuds ou plus.

## 5. Reproduction

```bash
python tools/1471-redirect-audit.py --self-test           # hors réseau (classes + 2 témoins du HOLD)
python tools/1471-redirect-audit.py --classes             # audit complet + classes (~5 min, API)
python tools/1471-redirect-audit.py --classes --lang pt   # une colonne
```

- L'auto-test porte **deux témoins du HOLD** : {`3.1`, `3.1.1`, `1.1`} (une paire apparentée + un membre isolé) et la forme {662/664 frères} + {979/1349 jumeaux} (deux paires sans lien) — tous deux doivent sortir `autres`. Contrôle du témoin exécuté : ces deux formes rendaient respectivement `carte-ancetre` et `meme-sophisme` sous l'ancienne règle « une paire suffit » (tête `dc6e4f42`) — les assertions étaient bien ROUGES avant la reprise.
- La résolution des redirections est mise en cache disque (`.tmp/1471-redirect-audit-cache.json`, hors dépôt) : un second passage rejoue sans re-payer les titres déjà résolus au sens du cache (seuls `normalized`/`redirects`/`missing` y sont persistés — les titres arrivant à l'identité sont re-requêtés).

*po-2024*
