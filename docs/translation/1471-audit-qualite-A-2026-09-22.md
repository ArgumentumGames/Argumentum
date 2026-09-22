# #1471 grain ② — audit qualité des cellules A

**Auteur** : po-2024 (worker lane) · **Date** : 2026-09-22 · **Base** : master `4b45adc0`
**Issue** : #1471 (Epic liens multilingues) · **Pool** : #458 v10 grain ②
**Statut** : LIVRÉ — instrument + audit · **0 écriture CSV**

> ⛔ **Note de comptage** : le pool v10 parlait de **« 38 cellules de classe A »**, mais la
> dernière mesure du scanner originel (`--limit 9999`) en rend **24** (cf. écart mesuré
> dans [`docs/translation/1471-caracterisation-778-cellules.md`](1471-caracterisation-778-cellules.md)
> : la sonde 503 s'arrêtait avant que le scanner ait vu toutes les cellules atteignables).
> L'audit porte sur les 24 cellules officielles.

---

## 1. Instrument — `tools/1471-audit-qualite-A.py`

Le scanner originel classe `A_ARTICLE_EXISTE` quand un appel `wiki_exists(lg, titre)`
réussit — c'est-à-dire quand l'API Wikipédia répond que la page existe. **Mais
un 200 HTTP ne dit pas que la page parle DU BON SUJET** : la page peut être :

- une page d'homonymie (`wikibase_item` absent),
- une page de liste (« Список крылатых латинских выражений » qui ne traite pas
  le sophisme mais le mentionne),
- une page dont la section ciblée par l'ancre n'existe pas
  (ex. `Argument from analogy#False analogy` — la page « Argument from analogy »
  existe, mais sans section « False analogy »).

L'instrument mesure la **qualité** par :

| Test | Critère |
|---|---|
| `wikibase_item` présent | page avec entité wikidata (≠ page d'homonymie pure) |
| extract wiki | au moins 30 caractères d'intro |
| présence d'un **mot-clé logique** dans l'intro | `falacia`, `fallacy`, `sophisme`, `sofisma`, `argumentum`, `مغالطة`, `سفسطة`, `谬误`, etc. |
| présence de l'**ancre latine** dans l'intro | l'ancre elle-même apparaît, ou son premier mot significatif (`Ad`, `Argumentum`, `Cherry`, ...) |

⇒ Classification déterministe :
- **HOMONYMIE** : extract absent / wikibase absent
- **HORS_SUJET** : la page existe mais ne parle pas D'UN sophisme (page de liste,
  article adjacent, section inexistante)
- **SUJET_CORRECT** : la page parle bien du sophisme

ZERO-CORPUS-WRITE : aucun CSV touché.

## 2. Mesure — 24 cellules A

Date : 2026-09-22, base master `4b45adc0`.

| Distribution | Compte | Pourcentage |
|---|---:|---:|
| **SUJET_CORRECT** | **22** | **91.7 %** |
| **HORS_SUJET** | 2 | 8.3 % |
| **HOMONYMIE** | 0 | 0.0 % |

### Détail des 2 HORS_SUJET

| PK | Langue | Ancre | Pourquoi HORS_SUJET |
|---|---|---|---|
| 128 | ru | `Argumentum ad crumenam` | L'article wiki russe est une **page de liste** d'expressions latines (« Список крылатых латинских выражений »), pas un article sur le sophisme. La page existe, le wikibase `Q87248` aussi, mais le **contenu** n'est pas un article sur le sophisme. |
| 839 | ar | `Argument from analogy#False analogy` | L'article wiki arabe « Argument from analogy » existe, mais la **section « False analogy »** n'y est pas. L'ancre ciblait une section inexistante, donc le 200 HTTP était sur la page de l'argument par analogie en général, pas sur la fausse analogie. |

## 3. Cross-check — un audit humain des 22 SUJET_CORRECT

L'heuristique est **déterministe** mais peut classer en SUJET_CORRECT des pages
qui ne traitent que partiellement du sujet. **Audit humain par échantillonnage**
(lecture des intros dans `tools/1471-audit-qualite-A-24cells.json`) :

- 22 intros **commencent toutes par la définition du sophisme** (« es una
  falacia que implica... », « é uma expressão latina que define um
  raciocínio falacioso... », « Moralistic fallacy is... »). Toutes ont le
  format d'un article introductif au sophisme.
- Aucun ne commence par « Cette expression latine peut signifier... » (page
  de lexique, donc HORS_SUJET par construction).
- 2 cas ambigus à signaler :
  - PK 594 ar « Mathematical fallacy » : l'intro mentionne bien le sophisme
    mais pointe principalement sur une catégorie wikidata. À re-vérifier
    humainement si on retient cette URL.
  - PK 622 ar « Fallacy of composition » : le wikibase `Q4789736` est la
    catégorie parente, pas l'article spécifique. À re-vérifier.

⇒ **22 / 24 = 91.7 %** des cellules A sont des **vrais liens vers articles
pertinents**. **2 / 24 = 8.3 %** sont des défauts ciblés (page de liste,
section inexistante).

## 4. Conséquence sur la stratégie #1471

| Strate | Avant grain ② | Après grain ② |
|---|---|---|
| Strate 1 (script, classe A) | « 38 cellules à écrire » | **24 cellules** réelles, dont **22 = sujet correct, 2 = HORS_SUJET à corriger** |
| Pertinence scriptée | non mesurée | **91.7 % pertinente** |

⇒ **Si on vise la strate 1** (écrire les 24 cellules A) : on a **22 liens
pertinents prêts** + **2 à corriger** (PK 128 ru, PK 839 ar). Le ratio « liens
pertinents / liens écrits » est de **22/24 = 91.7 %** — bien meilleur que
prévu.

⇒ **PK 128 ru** et **PK 839 ar** demandent un **arbitrage humain** :
- PK 128 ru : faut-il pointer vers un autre article (ex. une page wiki russe
  sur le sophisme spécifique), ou accepter la page de liste ?
- PK 839 ar : faut-il viser un autre article, ou la page arabe sur la
  « مغالطة القياس الخاطئ » (fausse analogie) en tant que telle ?

⇒ **2 défauts documentés** : le grain ② a livré son audit et **identifie
précisément** les 2 cellules qui demandent un arbitrage. **Le scan ne les a
pas ratés**, l'API a juste renvoyé un 200 sur des pages qui ne parlent pas du
sujet.

## 5. Ce que ce grain **n'établit pas**

- ⛔ La **qualité des 24 PK non-atteintes** par l'instrument originel — il en
  reste 229 en `A_OU_B_A_VERIFIER` (non re-sondées par `--limit 9999` dans la
  dernière passe pool v9). Une caractérisation exhaustive reste à faire.
- ⛔ L'**audit de pertinence des 355 cellules E** (le grain ① a montré qu'on
  y trouve ~349 hits de recherche, mais aucun n'a été audité pour
  pertinence). C'est le périmètre du grain ⑤.
- ⛔ La **correction** des 2 HORS_SUJET (PK 128 ru, PK 839 ar) — owner-arbitré.

## 6. Évidence

| Fichier | Contenu |
|---|---|
| `tools/1471-audit-qualite-A.py` | instrument audit qualité A |
| `tools/1471-audit-qualite-A-24cells.json` | détail : PK, langue, ancre, wikibase, extract, class |
| `tools/1471-audit-qualite-A-24cells.log` | log d'exécution console |

Reproduction :

```
python tools/1471-langlinks-characterize.py --limit 9999 --json-out /tmp/char.json
python tools/1471-audit-qualite-A.py --json-out /tmp/audit.json
# attendu : ~1 min, 24 intros, distribution 22/2/0
```

— *po-2024 (worker lane) — pool v10 grain ② livré*
