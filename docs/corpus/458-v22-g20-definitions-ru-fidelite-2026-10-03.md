# Pool v22 — série ⑳ : fidélité des **définitions** russes du deck (mesure 0-écriture)

**Base** : `3e830884` (03/10). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, contrôle inverse inclus. **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Les **titres** ont eu leurs passes de fidélité (grains ⑧ à ⑰, 7 langues). Les **définitions imprimées** (`desc_<lang>`) n'en avaient jamais eu. Cette passe mesure la fidélité sémantique de `desc_ru` contre `desc_fr` sur les **175 cartes du deck**, rangée par rangée — **aucun échantillonnage**.

## Synthèse — 0 A / 0 M / 16 C-note / 159 ✓ (175)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | 16 |
| **✓ — fidèle** | 159 |

**Le résultat est un zéro, et un zéro ne vaut que ce que l'instrument peut voir.** Trois éléments le bornent, tous mesurés :

1. **La référence imprimée est attachée sur 168/175**, pas 153 (voir « La jointure » plus bas) — la quasi-totalité des verdicts est adossée à l'imprimé, pas au seul français courant.
2. **Le contrôle inverse a été fait** : une définition **délibérément inversée**, plantée dans une copie, sort en **A** à la lecture (section dédiée).
3. **Les écrans mécaniques, eux, ne voient RIEN** : 26 drapeaux levés, **0 défaut réel derrière**. C'est consigné comme le principal enseignement de la passe, pas comme une confirmation.

## A — écarts réels

**Aucun.** Sur les 175 définitions du deck, aucune ne dit le contraire de sa source ni ne porte un sens faux.

## M — contenu manquant ou ajouté

**Aucun.** Un seul candidat s'est présenté — `5.3.1.1` « Ponctuation ambiguë », dont le russe (« Использование пунктуации создает двусмысленность. ») ne reprend pas le qualificatif « imprécise » du français. **Il est levé par la référence imprimée** : la cellule est **identique à l'imprimé v3**, au caractère près. Règle du pool — *« une traduction qui retrouve l'imprimé n'est pas une dérive »* → C-note, pas M.

⛔ Sans la jointure par le pont, cette carte serait sortie « aucune référence » et le candidat M aurait été publié à tort. C'est le seul endroit de la passe où l'instrument a failli produire un faux positif de verdict.

## C-notes — 16 observations, aucune écriture proposée

### 1. Le russe suit l'imprimé, le français a été enrichi depuis (8)

Cellules `IDENTIQUE` à l'archive v3 : le russe reproduit la définition **telle qu'imprimée**, alors que le `desc_fr` a été réécrit plus long depuis. Ce n'est pas une dérive de traduction — c'est un **retard de l'imprimé sur la source**, et il se résorbera à la prochaine régénération des définitions.

| PK | path | fr | Ce que le français ajoute depuis l'imprimé |
|---|---|---|---|
| 121 | `1.2.3.3.2` | Politiquement correct | « ou détournez », « la sensibilité d'une partie de l'auditoire » |
| 323 | `2.2.2.1` | Appel au mépris | « indigne d'attention » |
| 361 | `2.3.1.1.1.2` | Appât et substitution | la glissade progressive est résumée en une phrase |
| 707 | `4.1.2` | Inversion de causalité | la proposition finale redit la précédente (redite, pas perte) |
| 750 | `4.2.3` | Erreur de modalité | l'énumération (possible / nécessaire / certain / obligatoire) → « модальности » |
| 847 | `5.3.1` | Amphibologie | « structure … différentes interprétations » → « двойственный синтаксис » |
| 848 | `5.3.1.1` | Ponctuation ambiguë | le qualificatif « imprécise » (cf. section M ci-dessus) |
| 869 | `5.3.2.3.2.1` | Réification | « ce qui fausse votre raisonnement » |

### 2. Le russe suit l'imprimé, avec un écart lexical ou un ajout (6)

| PK | path | fr | Écart |
|---|---|---|---|
| 104 | `1.2.2.3` | Appel à la tradition | « validité d'un énoncé » là où le fr dit « pratique correcte » |
| 304 | `2.2.1.2.1` | Vœu pieux | **ajoute** « невероятна » (l'idée est jugée improbable) — absent du fr |
| 644 | `3.2.2` | Probabilités faussées | « lois mathématiques du calcul des hasards » pour « probabilités » |
| 777 | `4.3.2` | Inconsistance | « противоречивые гипотезы » là où le fr dit « affirmations » |
| 826 | `5.1.3` | Définition incohérente | « противоположным или абсурдным » pour « contradictoires ou incohérentes » |
| 837 | `5.2.1.3` | Comparaison incohérente | décrit un mécanisme voisin (« conclure sur l'ensemble ») plutôt que « fausse la comparaison globale » |

Aucun de ces six ne franchit le seuil : le sens reste celui de la carte, et **chacun est identique à l'imprimé**. Ils sont listés pour être arbitrables, pas corrigés.

### 3. Idiotismes localisés — à conserver (2)

Le russe ne calque pas, il trouve un équivalent. Ce sont des réussites de la passe de traduction, pas des écarts — consignées pour ne pas être « corrigées » par une passe future.

| PK | path | fr | Rendement russe |
|---|---|---|---|
| 992 | `6.2.2` | Vouloir le beurre et l'argent du beurre | **« И на елку влезть... »** (idiotisme symétrique) |
| 1330 | `7.2.1.2.2` | Noyer le poisson | **« Защита Чубакки »** (*Chewbacca defense*) |

### 4. Observations transverses

**T1 — Les écrans mécaniques n'ont trouvé aucun défaut, et c'est un résultat sur l'instrument.** 26 drapeaux sur 175 :

| Drapeau | Levés | Réels | Pourquoi ils se trompent |
|---|---:|---:|---|
| `POLARITY` | **21** | **0** | `\bне\b` ne voit pas la négation **préfixée** russe : « недостаточны », « необоснованными », « неприменим », « невозможность », « несоответствия ». L'écran mesure l'absence d'une **particule**, pas l'absence d'une **négation**. 21/21 faux positifs. |
| `SHORT` | 5 | **0** | Les 5 sont des compressions **identiques à l'imprimé** — toutes les cinq figurent au tableau 1 ci-dessus (PK 121, 707, 750, 847, 869). |

⭐ Le seul contresens planté (contrôle inverse) sort avec `FLAGS: -`. **Sur ce corpus, aucun écran ne sait voir un contresens** : leur valeur est de forcer une relecture, et la lecture porte le verdict. Un `0` produit par ces écrans seuls n'établirait rien.

**T2 — Titres et définitions ne se comportent pas pareil (SUPPOSÉ).** Le grain ⑪ a trouvé **13 A / 3 M** sur les **titres** russes des mêmes 175 rangées ; cette passe trouve **0 / 0** sur les **définitions**. Même corpus, même langue, même chaîne. Hypothèse : un titre est traduit comme une **chaîne courte isolée** (le moteur choisit un sens de dictionnaire, d'où « Софизм аварии », « Логика котла »), une définition comme une **phrase complète**, où le contexte contraint le sens. Non testé ici — consigné comme piste, pas comme conclusion.

## Contrôle inverse — une fausse définition sort-elle en A ?

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**. Définition **inversée** plantée sur `5.1.2.2.4` « Faux dilemme » (PK 814) :

| | Texte |
|---|---|
| FR (source) | *Vous raisonnez à partir d'un choix limité à deux options, alors qu'il existe d'autres possibilités.* |
| RU réel | *Вы рассуждаете на основании бинарной альтернативы, хотя существуют и другие возможности.* |
| **RU planté** | *Вы рассматриваете весь спектр имеющихся возможностей и сводите выбор к их полному набору.* — **dit l'inverse** |

**Résultat, en deux temps :**

- **L'instrument mécanique est aveugle** : longueur 89 car. contre 99 au français (ratio 0,90), aucun chiffre, aucune particule de négation → `FLAGS: -`, compteur inchangé (26). La cellule fausse **ne se distingue pas** d'une cellule saine.
- **La lecture la voit immédiatement** : la carte s'appelle « Faux dilemme » et le texte planté décrit exactement ce que le sophisme dénonce. Verdict **A**, proposition = restaurer la cellule d'origine.

⇒ Le contrôle établit ce qu'il doit établir : **un contresens est détectable par la lecture**, et il établit en plus ce que la passe ne doit pas cacher : **il ne l'est par aucun écran**. Un premier essai de plantage contenant un « не » incident avait levé `POLARITY` — **par accident**, pas par détection ; c'est pourquoi le plantage publié ici n'en contient aucun.

## La jointure imprimée — correction d'un instrument faux

Cette passe a d'abord utilisé la jointure **`path` seul**, qui rend **153/175** et déclare **22** cartes « sans référence ». ⛔ **C'est l'erreur exacte que le dépôt a publiée le 22/09 puis corrigée** ([`archive-coverage-2026-09-22.md`](archive-coverage-2026-09-22.md)) : la taxonomie a été restructurée entre l'archive v3 et la baseline 2024 — **34 lignes d'archive changent de `path` à nom constant** — et la couverture réelle est **168/175**.

L'instrument réutilise donc le **pont** du dépôt (`archive-bridge-instrument.py`) : *archive --(nom)--> baseline 2024 --(PK)--> HEAD*.

| Étage | Cartes |
|---|---:|
| 1 — jointure `path` | 153 |
| 2 — pont (nom puis PK) | **15** |
| **Référence imprimée attachée** | **168/175** |
| Aucune archive, sous aucun nom | 7 |

Les 7 sans aucune référence — **PK 105, 362, 492, 1020, 1092, 1120, 1357** — sont jugées sur le seul français.

> **Erratum jointure (03/10, grain 3 — instrument réparé sur ordre du pool, c.5964435596).** Le rattachement par `path` attache une **position**, pas une carte. Réparé : un `path` ne vaut plus rattachement **confirmé** que si l'archive porte le même nom — directement ou via le nom de la même PK dans la baseline 2024 ; sinon le pont, et à défaut le rattachement est gardé mais marqué **POSITION SEULE** (compté à part, contenu d'archive non affiché). Re-passe `ru` : **157 confirmées par le nom** (139 `etage1-nom` + 18 pont) **+ 11 position seule** — le total 168/175 ne change pas, il se précise.
>
> - **Triplet path `1.1.1`–`1.1.3` (PK 3/33/55)** : les trois sœurs avaient permuté leurs positions depuis l'archive v3 — chacune était lue contre la **mauvaise** carte. **PK 55 passe DIFFERE → IDENTIQUE** (mesurée par ai-01, c'est la carte qui a démontré le défaut) ; PK 3 et 33 restent DIFFERE, désormais contre leur propre texte d'archive. **Aucune des trois n'était citée dans ce dossier — aucun verdict ne change.**
> - **11 renommées** (PK 153, 361, 658, 799, 847, 855, 1024, 1174, 1242, 1313, 1361 — l'archive v3 porte l'ancien nom, la baseline déjà le nouveau) : rattachement plausible mais **non prouvé par le nom**. **Deux lignes de ce dossier sont concernées** : **PK 361** et **PK 847** (tableau 1, et la ligne `SHORT` « compressions identiques à l'imprimé » — sur les 5, il en reste 4 prouvées). Les observations subsistent (elles comparent `ru` et `fr`), mais leur corroboration par l'imprimé est rétrogradée de **prouvée** à **plausible**.
> - Le candidat M levé par l'imprimé (**PK 848**) n'est **pas** concerné : son rattachement passe par le pont, confirmé par le nom, `IDENTIQUE` — la levée reste prouvée.
> - **L'arithmétique du verdict ne change pas : 0 A / 0 M / 16 C-note / 159 ✓.**

## N'établit pas

- **Que les définitions russes soient sans défaut.** Le verdict est une **lecture sémantique unique**, faite par un non-locuteur natif, sur des paires de phrases. Un relecteur natif peut voir ce que cette passe n'a pas vu — c'est le sens même d'un `0 A`. Ce qui est établi est plus étroit : *aucun écart n'a été trouvé en lisant les 175 rangées*.
- **Que les écrans mécaniques corroborent ce zéro.** Ils sont **structurellement aveugles** au contresens (contrôle inverse à l'appui) ; leurs 26 drapeaux ne portent aucun poids de verdict. Un `0` assis sur eux serait un zéro par construction.
- **Que l'imprimé vaille preuve de justesse.** `IDENTIQUE` signifie « pas de dérive **depuis l'imprimé** » ; `DIFFERE` signifie « révisé **après** l'imprimé ». Aucun des deux ne dit que la cellule est *juste*.
- **Que les 7 cartes sans archive soient propres.** Elles n'ont pas de référence : leur verdict repose sur le seul français, sans contrôle croisé possible.
- **Rien sur les autres langues ni sur le reste du corpus.** Cette passe couvre `desc_ru` des **175 cartes du deck** uniquement. Non mesurés : `desc_en` (㉕), `zh` (㉑), `ar` (㉒), `fa` (㉓), `pt`+`es` (㉔), les champs `example_<lang>` et `link_<lang>`, et les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu**, et aucune n'est proposée : les 16 C-notes sont des observations, l'arbitrage revient à ai-01.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang ru --out <sortie>
```

Le compte est vérifié par l'instrument lui-même : dénominateur **175** affiché par exécution, et arrêt bruyant si le deck n'en compte pas 175. La sortie de cet instrument a été comparée à celle de l'instrument de travail : **identiques**, octet pour octet.
