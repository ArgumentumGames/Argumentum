# G5 — la clé de jointure Scenarii, construite et validée

**Épic** [#1499](https://github.com/ArgumentumGames/Argumentum/issues/1499) · grain **G5** · po-2024 · 2026-09-25.
**Instrument** : [`tools/1499-g5-cle-scenarii.py`](../../tools/1499-g5-cle-scenarii.py) (lecture seule, `--self-test` rc=0, mutation `--key path` rc=2).

> ⭐ À retenir en une ligne : **la clé Scenarii validée est le titre normalisé (E1), complété par `path` sous contrôle de contenu (E2) — et `path` seul est condamné par la mesure.**

## 1. Le problème (mesuré, pas supposé)

Le 22/09, ai-01 a failli publier un artefact donnant **« 308 cellules changées »** sur Scenarii en joignant l'archive 2022 au corpus courant sur `path`. Le chiffre était faux : **`path` a été réattribué** — Napoléon n'a pas été réécrit, il a **déménagé** de `1.3.1` à `1.2.3`, et une autre carte occupe désormais son ancienne adresse.

La signature de la panne était lisible dans la sortie de l'instrument lui-même : `catégorie` concordait **100 %** pendant que `sous-catégorie` tombait à 25 %. *Un corpus ne réécrit pas 3 sous-catégories sur 4 en gardant toutes ses catégories.*

Le grain G5 était donc : **construire une clé qui prouve qu'elle compare les mêmes cartes avant de rapporter qu'elles diffèrent** — méthode + contrôle inverse, ⛔ aucun chiffrage.

## 2. La clé

| Étage | Règle | Ce qu'il récupère |
|---|---|---|
| **E1** | **titre normalisé** — casefold + repli d'accents NFKD + unification des apostrophes (U+2019/U+2018/U+02BC → U+0027) + **ponctuation retirée** | une carte **où qu'elle ait déménagé**. La ponctuation compte : « Maréchal nous voilà » et « Maréchal, nous voilà » sont la même carte — une virgule n'est pas un renommage (ce point seul vaut +2 cartes) |
| **E2** | `path` **ET** similarité de contenu **strictement au-dessus** du plancher de bruit | une carte **renommée** restée à sa place. Sans le contrôle, E2 est exactement la jointure fausse |

## 3. Le contrôle — ce qui rend la clé falsifiable

**Similarité de contenu** = Jaccard sur les ensembles de mots de `contexte + enjeu + suggestion` (après normalisation, mots > 2 lettres, mots vides retirés).

**Plancher de bruit calibré exhaustivement** : les **12 859** paires archive × courant (77 × 167), **jamais un échantillon**. Le seuil est le **q99** de cette distribution = **0,148**.

Une clé est **condamnée dès qu'une seule** de ses paires tombe sous le plancher : une paire au niveau du bruit est l'appariement de deux cartes différentes — c'est une re-clé, pas une réécriture.

### Le verdict, mesuré le 25/09 sur master

| Clé | Paires | Similarité min | Sous le plancher | Verdict |
|---|---:|---:|---:|---|
| **E1 titre normalisé** | 56 | **0,439** | **0** | ✅ **validée** |
| `path` | 64 | 0,019 | **12** | ⛔ condamnée |

Les 12 paires qui condamnent `path` sont nommées par l'instrument — de « 5G » apparié à « Remplaçant cryogénique » (0,019) à « Maman ! » apparié à « L'amoureux des bêtes » (0,133).

### Pourquoi `catégorie` ne peut pas servir de contrôle

`catégorie` concorde à **100 % sur la jointure `path` que la mesure condamne** : les cartes d'un même bloc partagent la catégorie, donc ce champ ne peut pas voir une re-clé **intra-bloc** — or la réattribution mesurée est massivement intra-bloc (histoire→histoire, mythologie→mythologie). ⭐ *Un contrôle qui rend 100 % sur une jointure connue fausse ne contrôle rien : un `0` n'est une absence que si l'instrument pouvait rendre un `1`.* `sous-catégorie` ne vaut pas mieux dans l'autre sens (19-25 % partout) : il a **légitimement changé**, donc il ne survit pas à une réécriture — il viole l'exigence d'invariant.

`baratineur`/`piocheur` (89 %/93 % sur E1 contre 67 %/72 % sur `path`) confirment la direction sans être un contrôle : les noms de personnages bougent légitimement dans certaines réécritures.

## 4. Couverture de la clé — 69/77 (89,6 %)

| | cartes | |
|---|---:|---|
| E1 — titre normalisé | **56** | |
| E2 — renommées récupérées sous contrôle | **13** | 11 CONFORTABLES (≥ 2× le plancher, 0,30–0,86) · **2 LIMITES** (0,163 « obélisque »→« La potion de trop », 0,233 « stealthing »→« Retrait non consenti ») |
| **Non résolues** | **8** | nommées ci-dessous |
| **Total** | **69/77** | |

### Les 8 non résolues — la clé seule ne décide pas

| path | carte 2022 | path actuel occupant |
|---|---|---|
| 1.3.3 | Louis XVI tâche de garder sa tête | Maréchal, nous voilà |
| 1.4.2 | président Truman et la Bombe A | *(aucun)* |
| 2.1.2 | Don Juan | Pain d'épices |
| 3.2.3 | mariage ou pas | Le ménage à trois |
| 3.4.1 | 2+1 | *(aucun)* |
| 4.1.4 | rétrogradation canapé | Le professeur |
| 5.3.3 | 5G | Remplaçant cryogénique |
| 7.4.2 | miaw | *(aucun)* |

Ces 8 cartes sont soit **retirées du corpus**, soit **renommées au-delà de toute normalisation** *et* réécrites au point que le contenu ne se reconnaît plus. Les départager exige une décision **humaine, carte par carte** — l'instrument s'arrête là et le dit. ⚠️ Pour 3 d'entre elles (`Truman`, `2+1`, `miaw`), l'ancien `path` est **vide** dans le corpus courant : aucun artefact ne peut apparier ces cartes-là sans inventer.

## 5. Le self-test — les coins que le corpus ne fournit pas à la demande

Fixtures jetables (`--self-test`, rc=0) : ré-attribution, renommage, `path` empoisonné, colonne renommée (la garde d'en-tête échoue **fort** — une colonne absente lue comme vide donnerait un faux 0 silencieux).

Deux règles de conception sont nées d'échecs du self-test, pas de réflexion préalable :

1. **Une fixture de 16 paires met q99 SUR une vraie paire** (similarité 1,0), monte le seuil à 1,0 et condamne toutes les clés. Le plancher q99 n'est valide que si les vraies paires restent **sous 1 %** de la matrice — vrai ici (69/12 859 = 0,54 %), **à re-vérifier si le corpus grandit**.
2. **Une paire assise exactement sur le plancher** (la fixture en produit une, à 0,143 pour un seuil à 0,143) passe un `>=` — d'où le **`>` strict** d'E2. Une paire au plancher est indiscernable du bruit qui a calibré le plancher.

## 6. Mutation falsifiante

```
python tools/1499-g5-cle-scenarii.py             # rc=0 — la clé E1 passe
python tools/1499-g5-cle-scenarii.py --key path  # rc=2 — la même exigence condamne `path`
```

L'instrument ne sait pas dire « la clé est bonne » sans prouver **dans le même run** que son contrôle condamne la clé fausse (`le controle discrimine`). Un contrôle qui passerait tout ne serait pas un contrôle.

## 7. Ce que ce dossier n'établit pas

- ⛔ **Aucun chiffrage de divergences** — c'est le grain **G6**, explicitement gaté sur la validation de cette clé. Le décompte des cellules changées sur les 69 cartes appariées n'a **pas** été fait et ne doit pas être cité.
- **Les 8 non résolues** restent non tranchées (§4).
- **La justesse des appariements E2 LIMITES** (2 cartes) : au-dessus du plancher, mais pas au double — une lecture humaine peut les récuser, et l'instrument affiche leurs scores pour ça.
- **es/ar/fa/zh** : l'archive 2022 n'existe qu'en fr/en — pour ces langues, aucune comparaison à l'imprimé n'est possible, seule la cohérence avec le FR l'est.
- Le dossier ne touche **aucun CSV** ; l'instrument est lecture seule par construction (aucun mode d'écriture).

## 8. Conséquence pour G6

L'événement de reprise de G6 (« G5 livrée et sa clé validée ») est **satisfait sur la méthode**. G6 doit :
- joindre sur **E1 + E2** (jamais `path` seul) ;
- traiter les **8 non résolues** comme un périmètre séparé, décision humaine ;
- qualifier chaque divergence MESURÉE sur les 69 cartes appariées — et les 2 E2 LIMITES comme telles.