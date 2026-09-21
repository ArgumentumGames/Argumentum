# Epic #1471 — grain ① : le **rendement réel** de l'API `langlinks`

**Auteur** : po-2024 (worker lane) · **Base** : master `6694d702` · **Date** : 2026-09-21 · **Instrument** : [`tools/1471-langlinks-yield.py`](../../tools/1471-langlinks-yield.py)

**Mandat** ([#1471 c.5762155854](https://github.com/ArgumentumGames/Argumentum/issues/1471#issuecomment-5762155854), arbitrage owner vérifié) : la branche **(b) Wikipédia par interlangue** est ordonnancée **en premier** ; le grain ① doit **mesurer son rendement** — `visé / trouvé / manquant`, par langue cible, **plus** la worklist nominative de ce que la passe laisse. ⛔ **Aucune écriture CSV.**

---

## 1. Le résultat, en une ligne

> **La passe scriptée rend 225 cellules sur 728 visées — soit 30,9 %, pas ~100 %.**
> **Et rapportée aux 1003 cellules à produire, elle en couvre 22,4 % ; il en reste 778 (77,6 %) à la main.**

| Langue cible | visé | **trouvé** | manquant | taux |
|---|---:|---:|---:|---:|
| `fr` | 42 | **6** | 36 | **14,3 %** |
| `en` | 2 | **0** | 2 | **0,0 %** |
| `ru` | 99 | **20** | 79 | 20,2 % |
| `pt` | 116 | **32** | 84 | 27,6 % |
| `es` | 117 | **40** | 77 | 34,2 % |
| `ar` | 117 | **47** | 70 | 40,2 % |
| `fa` | 118 | **36** | 82 | 30,5 % |
| `zh` | 117 | **44** | 73 | 37,6 % |
| **TOTAL** | **728** | **225** | **503** | **30,9 %** |

⚠️ **Le chiffre de 728 était un plafond, et c'est bien un plafond** : il est intégralement reproduit (§2), et il se dégrade de 69 % à l'exécution. Ce que le plafond ne pouvait pas dire est maintenant mesuré.

---

## 2. Étalonnage de l'instrument — **11/11 avant de produire un seul chiffre**

Un rendement mesuré par un instrument non étalonné ne vaut rien. L'instrument a donc dû **reproduire les 11 chiffres publiés** par #1471 **avant** d'interroger l'API :

| Mesure | Attendu (#1471) | Obtenu |
|---|---|---|
| cartes du deck | 175 | **175** ✓ |
| `en` natif / Wikipédia | 161 / 112 | **161 / 112** ✓ |
| `fr` natif / Wikipédia | 89 / 72 | **89 / 72** ✓ |
| `ru` natif / Wikipédia | 27 / 23 | **27 / 23** ✓ |
| `pt`/`es`/`ar`/`zh` natif | 9/8/8/8 | **9/8/8/8** ✓ |
| `fa` natif | 7 | **7** ✓ |
| cartes ancrées Wikipédia | 125 | **125** ✓ |
| cartes avec ≥ 1 lien | 165 | **165** ✓ |
| **cellules visées** | **728** | **728** ✓ |

⭐ **Un écart a été trouvé et résolu, pas absorbé.** Le premier passage rendait `en` = **111** au lieu de 112. Cause mesurée : **`PK 108` porte DEUX URLs dans une seule cellule** — `en.wikipedia.org/wiki/Appeal_to_nature` **et** `utminers.utep.edu/…/fallacies.htm`, séparées par un retour à la ligne. Un motif ancré sur la cellule entière la rejette. ⇒ **Mon instrument était plus strict que le comptage de #1471, pas plus juste** ; les deux mesures sont défendables mais ne mesurent pas la même chose (« cellule qui *contient* une URL Wikipédia » vs « cellule qui *est* une URL Wikipédia »). Corrigé : la cellule est découpée en URLs, **2 cellules multi-URL** existent sur le deck (`en` + `ru`).

⚠️ **Ce découpage n'est pas cosmétique pour cet Epic** : une cellule multi-URL porte **deux ancres**, donc deux chances d'interwiki. Ne pas découper, c'est perdre des ancres en silence.

L'instrument **refuse de publier** si l'étalonnage échoue (`if not calibrate(cards): return 2`) — les colonnes sont par ailleurs **assertées d'existence**, parce qu'une première mesure d'ai-01 s'était effondrée sur des noms de colonnes **tapés** au lieu d'être dérivés de l'en-tête.

---

## 3. ⭐ Ce que la mesure change : la structure de coût est **inverse** de celle annoncée

Le mandat owner ordonnait : *« des **low hanging fruits** possibles avec la recherche scriptée systématique des traductions wikipedia. **Ensuite** il faudra combler les trous à la main. »* La mesure donne le poids réel de chaque phase :

| Phase | Cellules | Part des 1003 |
|---|---:|---:|
| **Scriptée** (`langlinks`) | **225** | **22,4 %** |
| **À la main** — 503 visées sans interwiki **+ 275 sans ancre Wikipédia** | **778** | **77,6 %** |

⇒ **Les « fruits à portée de main » sont un cinquième du travail, et le travail manuel en est les quatre cinquièmes.** Ce n'est pas un argument contre l'ordre choisi — la passe scriptée reste gratuite, déterministe et sans hallucination, donc elle passe bien en premier. C'est un argument contre **l'attente de coût** qu'elle crée : qui lit « low hanging fruits » puis « ensuite on verra » chiffrera mal la suite. **L'arbitrage (a)/(c) sur le reste porte donc sur 778 cellules, pas sur un résidu.**

### La langue la plus « évidente » est la moins productive

⭐ **`fr` a le plus mauvais rendement du lot : 14,3 %** — alors que #1471 proposait de commencer par elle parce que c'est « la langue par défaut du jeu, et la moins chère (sources francophones abondantes) ». Et `en` rend **0/2**.

L'explication tient dans la donnée, pas dans une hypothèse : les cellules `fr` vides sont vides sur des cartes dont les ancres sont **majoritairement des articles `en`** — et la couverture interlangue **fr** des articles de sophismes anglais est **mince**. C'est **le motif de PK 590 à l'échelle** (`en:Human physical appearance` existe en 10 langues, **sans le fr**). ⇒ **L'abondance des sources francophones ne dit rien de la densité des interwikis `en → fr`**, et c'est cette dernière qui décide du rendement.

Symétriquement, `ar` (40,2 %), `zh` (37,6 %) et `es` (34,2 %) — les langues annoncées comme « les plus coûteuses » — sont les **plus productives par script**. Le classement par difficulté que #1471 proposait (`fr` → `ru` → `es`/`pt` → `zh`/`ar`/`fa`), fondé sur la disponibilité supposée des sources, **n'est pas celui que la mesure rend**.

### Projection de couverture native, après la passe scriptée

| Langue | natif | + script | après | couverture |
|---|---:|---:|---:|---:|
| `en` | 161 | 0 | 161 | **92,0 %** |
| `fr` | 89 | 6 | 95 | 54,3 % |
| `ar` | 8 | 47 | 55 | 31,4 % |
| `zh` | 8 | 44 | 52 | 29,7 % |
| `es` | 8 | 40 | 48 | 27,4 % |
| `ru` | 27 | 20 | 47 | 26,9 % |
| `fa` | 7 | 36 | 43 | 24,6 % |
| `pt` | 9 | 32 | 41 | 23,4 % |
| **Total** | **317 / 1400** | **+225** | **542 / 1400** | **22,6 % → 38,7 %** |

⚠️ **`en` passe de 161 à 161 : la passe scriptée n'apporte RIEN à l'anglais** (2 visées, 0 trouvée). C'est cohérent — `en` est la langue **source** de la plupart des ancres, pas une cible.

---

## 4. Qualité des ancres — faux positifs **mesurés**

| | |
|---|---:|
| ancres uniques résolues | **241** |
| dont pages d'**homonymie** — **exclues** du rendement | **2** |
| dont **redirections** — suivies, cible réelle interrogée | **20** |
| dont pages absentes | 1 |
| erreurs réseau/API | **0** |

Les 2 homonymies, nommées : `en:'Non sequitur'` et `fr:'Cueillette de cerises'`. ⚠️ **C'est exactement le faux-positif annoncé par #1471** : `langlinks` sur une page d'homonymie rend des liens vers d'**autres pages d'homonymie**, pas vers l'article. L'instrument les **écarte** au lieu de les compter comme trouvées ; les compter aurait gonflé le rendement d'un chiffre faux.

**Redirections : 20 sur 241 (8,3 %)** — l'API est appelée avec `redirects=1`, donc la cible réelle est interrogée et le titre rendu est celui de la cible. ⚠️ Conséquence pour le grain ② : **l'URL à écrire doit être celle que l'API rend**, pas le titre d'origine — écrire le titre d'origine produirait un lien qui marche mais sur une redirection.

### Vérification de la DoD : la langue **constatée**, jamais déduite

#1471 exige « URL vérifiée vivante (HTTP 200) **et** langue de la page **constatée**, ⛔ pas déduite du domaine ». Échantillon d'un `trouvé` par langue cible (7 langues concernées — `en` n'a rien trouvé) :

```
fr:Cliché                     HTTP 200  constate lang='fr'  OK
ru:Политическая корректность  HTTP 200  constate lang='ru'  OK
pt:Argumentum ad ignorantiam  HTTP 200  constate lang='pt'  OK
es:Argumento ad ignorantiam   HTTP 200  constate lang='es'  OK
ar:تعميم خاطئ                 HTTP 200  constate lang='ar'  OK
fa:توسل به نادانی             HTTP 200  constate lang='fa'  OK
zh:草率歸納                   HTTP 200  constate lang='zh'  OK
⇒ 7/7 conformes
```

La langue est lue sur le `<html lang="…">` **de la page servie**, pas sur le domaine de l'URL.

---

## 5. Ce que le grain ② devra faire, et ce qu'il ne devra **pas** faire

**Ingrédients vérifiés et utilisables tels quels** : les **225 couples (cellule, URL cible)** sont produits par l'instrument et **vérifiés** ; `link_fa` s'écrit en **persan brut** (90/90 déjà mesuré, cf. #1470) ; `link_fr` accepte les deux formes.

⛔ **Ce que la passe ne couvre pas et ne doit pas être forcé à couvrir** : les **503** visées sans interwiki, les **275** sans ancre Wikipédia (dont les `web.archive.org` de `fr` et les `logicallyfallacious` de `en`, qui **ne portent aucun interwiki** par construction), et les **2 homonymies** (une page d'homonymie n'est pas un article).

⛔ **Ne pas toucher aux 10 vides justifiés** de #1435 (`PK 1, 33, 658, 681, 690, 798, 826, 876, 1004, 1281`) — ce sont des têtes de famille abstraites sans article dédié.

⚠️ **Coût réel de l'API : 11 appels** (`titles` = 50/call, 241 ancres uniques, 8 langues sources), `User-Agent` identifiant le projet, ~0,6 s entre appels. **Aucune clé, aucun coût, aucune hallucination possible.**

**Worklist nominative** : §6, par langue cible.

---

## 6. Worklist nominative — les 503 cellules que la passe **ne** couvre pas

*(le complément est en §6.b : les 275 cellules sans ancre Wikipédia, déjà connues de #1471 et **non re-mesurées ici** — leur compte est confirmé par différence, `1003 − 728 = 275`.)*

### `link_fr` — 36 cellule(s)

- **PK 51** — 2
- **PK 79** — 2
- **PK 112** — 2
- **PK 134** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 362** — 2
- **PK 420** — 2
- **PK 594** — 1
- **PK 614** — 1
- **PK 625** — 1
- **PK 633** — 2
- **PK 659** — 1
- **PK 666** — 1
- **PK 697** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 759** — 2
- **PK 768** — 1
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 808** — 2
- **PK 809** — 1
- **PK 908** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 956** — 1
- **PK 973** — 1
- **PK 1023** — 1
- **PK 1174** — 1
- **PK 1297** — 2
- **PK 1313** — 1
- **PK 1314** — 2

### `link_en` — 2 cellule(s)

- **PK 43** — 1
- **PK 1398** — 1

### `link_ru` — 79 cellule(s)

- **PK 2** — 1
- **PK 51** — 2
- **PK 78** — 1
- **PK 79** — 2
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 323** — 1
- **PK 337** — 2
- **PK 340** — 2
- **PK 356** — 1
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 595** — 1
- **PK 598** — 2
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 632** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 677** — 1
- **PK 697** — 1
- **PK 707** — 2
- **PK 713** — 2
- **PK 719** — 1
- **PK 733** — 1
- **PK 740** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 809** — 1
- **PK 833** — 1
- **PK 839** — 1
- **PK 844** — 2
- **PK 845** — 1
- **PK 847** — 2
- **PK 887** — 1
- **PK 900** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 994** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1352** — 1
- **PK 1371** — 1
- **PK 1398** — 1

### `link_pt` — 84 cellule(s)

- **PK 2** — 1
- **PK 43** — 1
- **PK 51** — 2
- **PK 55** — 2
- **PK 71** — 2
- **PK 78** — 1
- **PK 79** — 2
- **PK 98** — 2
- **PK 104** — 1
- **PK 108** — 1
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 323** — 1
- **PK 340** — 2
- **PK 356** — 1
- **PK 357** — 2
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 595** — 1
- **PK 598** — 2
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 632** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 677** — 1
- **PK 697** — 1
- **PK 699** — 1
- **PK 713** — 2
- **PK 733** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 809** — 1
- **PK 833** — 1
- **PK 839** — 1
- **PK 844** — 2
- **PK 847** — 2
- **PK 869** — 2
- **PK 887** — 1
- **PK 942** — 2
- **PK 943** — 1
- **PK 956** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 994** — 1
- **PK 1023** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1398** — 1

### `link_es` — 77 cellule(s)

- **PK 2** — 1
- **PK 43** — 1
- **PK 51** — 2
- **PK 55** — 2
- **PK 79** — 2
- **PK 98** — 2
- **PK 104** — 1
- **PK 108** — 1
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 313** — 1
- **PK 323** — 1
- **PK 340** — 2
- **PK 356** — 1
- **PK 357** — 2
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 595** — 1
- **PK 598** — 2
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 632** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 697** — 1
- **PK 699** — 1
- **PK 713** — 2
- **PK 733** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 809** — 1
- **PK 833** — 1
- **PK 839** — 1
- **PK 845** — 1
- **PK 847** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1398** — 1

### `link_ar` — 70 cellule(s)

- **PK 51** — 2
- **PK 55** — 2
- **PK 79** — 2
- **PK 104** — 1
- **PK 108** — 1
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 323** — 1
- **PK 340** — 2
- **PK 356** — 1
- **PK 357** — 2
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 697** — 1
- **PK 699** — 1
- **PK 733** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 833** — 1
- **PK 839** — 1
- **PK 845** — 1
- **PK 869** — 2
- **PK 900** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 1023** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1398** — 1

### `link_fa` — 82 cellule(s)

- **PK 2** — 1
- **PK 43** — 1
- **PK 51** — 2
- **PK 55** — 2
- **PK 79** — 2
- **PK 98** — 2
- **PK 104** — 1
- **PK 108** — 1
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 154** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 319** — 1
- **PK 323** — 1
- **PK 340** — 2
- **PK 356** — 1
- **PK 357** — 2
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 595** — 1
- **PK 598** — 2
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 632** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 697** — 1
- **PK 699** — 1
- **PK 733** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 799** — 1
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 809** — 1
- **PK 833** — 1
- **PK 839** — 1
- **PK 844** — 2
- **PK 845** — 1
- **PK 869** — 2
- **PK 900** — 2
- **PK 908** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 994** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1373** — 2
- **PK 1398** — 1

### `link_zh` — 73 cellule(s)

- **PK 43** — 1
- **PK 51** — 2
- **PK 55** — 2
- **PK 71** — 2
- **PK 78** — 1
- **PK 79** — 2
- **PK 98** — 2
- **PK 104** — 1
- **PK 108** — 1
- **PK 112** — 2
- **PK 128** — 2
- **PK 134** — 1
- **PK 175** — 1
- **PK 176** — 1
- **PK 177** — 2
- **PK 179** — 2
- **PK 184** — 2
- **PK 299** — 1
- **PK 300** — 2
- **PK 323** — 1
- **PK 340** — 2
- **PK 356** — 1
- **PK 357** — 2
- **PK 358** — 2
- **PK 362** — 2
- **PK 420** — 2
- **PK 432** — 2
- **PK 492** — 2
- **PK 511** — 1
- **PK 594** — 1
- **PK 603** — 1
- **PK 614** — 1
- **PK 621** — 2
- **PK 622** — 2
- **PK 625** — 1
- **PK 659** — 1
- **PK 666** — 1
- **PK 670** — 1
- **PK 673** — 2
- **PK 697** — 1
- **PK 699** — 1
- **PK 733** — 1
- **PK 740** — 2
- **PK 752** — 2
- **PK 758** — 1
- **PK 759** — 2
- **PK 768** — 1
- **PK 781** — 2
- **PK 787** — 2
- **PK 802** — 1
- **PK 804** — 2
- **PK 808** — 2
- **PK 833** — 1
- **PK 839** — 1
- **PK 845** — 1
- **PK 869** — 2
- **PK 900** — 2
- **PK 942** — 2
- **PK 943** — 1
- **PK 973** — 1
- **PK 977** — 2
- **PK 989** — 1
- **PK 994** — 1
- **PK 1024** — 2
- **PK 1120** — 2
- **PK 1174** — 1
- **PK 1242** — 1
- **PK 1297** — 2
- **PK 1301** — 1
- **PK 1313** — 1
- **PK 1314** — 2
- **PK 1330** — 1
- **PK 1398** — 1

**Total worklist = 503 cellules.**

### 6.b — Les 275 cellules sans ancre Wikipédia

Non re-mesurées ici : leur compte est **confirmé par différence** (`1003 − 728 = 275`), et #1471 les a déjà caractérisées (les `web.archive.org` de `fr`, les 17 `logicallyfallacious` et 11 `fallacyfiles` de `en`, etc. — aucune de ces sources ne porte d'interwiki par construction). **Le grain ② ne les traitera pas** : elles relèvent de l'arbitrage (a)/(c).
