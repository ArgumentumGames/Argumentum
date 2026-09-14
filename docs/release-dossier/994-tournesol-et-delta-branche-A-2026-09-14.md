# #994 — Grain A+B : les 2 cellules Tournesol sont écrites, la branche A est **arrêtée sur delta** (47 → 58)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `874a5d98`
**Statut** : **1 mutation CSV (B, 2 cellules)** + **1 delta déclaré AVANT écriture (A)**.
⛔ `0` cellule du volet voix écrite · ⛔ aucune régénération · ⛔ aucune branche choisie ici.
**Dispatch** : ai-01, deep-queue #458 c.5657210653, grain **④** (« #994 A+B — une PR CSV Scenarii »).
**GO amont** : commentaire owner **#994 `5657115517`** (2026-09-14, publié par ai-01 sous le token
partagé, décisions reçues en interactif).
**Doc amont** : `994-arbitrages-AB-reestampe-2026-09-14.md` (PR #1362, révision 2).

---

## §0 Le résultat en une phrase

**B est écrit** — les 2 cellules Tournesol certaines, exactement celles du GO, byte à byte.
**A ne l'est pas**, et c'est volontaire : le chiffre du GO (**47**) est re-dérivé **exact**, mais
**47 cellules ne suffisent pas** à atteindre l'objectif énoncé (« normaliser l'anglais à la
troisième personne ») — il en faut **58**. Écrire les 47 aurait **dégradé 9 cartes**.

---

## §1 Ce qui a été écrit — branche B, 2 cellules

Fichier : `Cards/Scenarii/Argumentum Scenarii - Cards.csv`, rangée `path = 5.1.1`
(`coordonnées = 5,0101`), **une seule ligne, deux champs** :

| colonne | idx | avant | après |
|---|---:|---|---|
| `drawer` (EN) | 15 | `Sunflower` | **`Cuthbert Calculus`** |
| `drawer_ru` | 23 | `Подсолнух` | **`профессор Лакмус`** |

**Méthode — splice par offsets d'octets, pas de round-trip CSV.** Le fichier est lu en binaire,
la ligne cible découpée par un splitter **quote-aware** qui rend les bornes d'octets de chaque
champ ; seules les deux plages visées sont remplacées. Aucun ré-encodage, aucun re-quoting.

**Contrôles passés** (assertions, pas d'inspection visuelle) :

| contrôle | résultat |
|---|---|
| BOM | **absent** avant **et** après (`\xef\xbb\xbf` non présent) |
| fins de ligne | **168 CRLF**, **0 LF nu** — inchangé |
| lignes du fichier | 169 avant / 169 après |
| lignes **modifiées** | **1** (index 112) — assertion « plus d'une ligne changée » bloquante |
| champs modifiés sur cette ligne | **exactement [15, 23]** |
| taille | 556 796 → 556 817 o (**+21** = +9 `Cuthbert Calculus`, +12 `профессор Лакмус`) |
| `git diff --numstat` | `1 insertion, 1 suppression` |

**Les 6 autres langues sont byte-identiques** : `Tornasol` (es), `Girassol` (pt), `向日葵教授` (zh),
`تورنسول` (ar), `پروفسور تورنسل` (fa) et `Tournesol` (fr) n'ont pas été touchés.
**Les 3 cellules Schtroumpfs PT** (`smoothTalker_pt`, `suggestion_pt`, `context_pt`) restent
**hors périmètre** — aucune n'a été écrite.

> ⚠️ Note de non-collision : la carte `5,0101` porte aussi un `context` EN en 2ᵉ personne, donc elle
> figure dans la worklist A du §2. Le champ écrit ici (`drawer`) **n'est pas** l'un des 10 `drawer`
> de A (le sien valait `Sunflower`, pas `Your X`) : les deux volets sont **orthogonaux**, écrire B
> n'engage pas A.

---

## §2 Branche A — le delta, déclaré **avant** d'écrire

### A.1 Ce que le GO autorise, et qui est re-dérivé à l'unité

Sur le CSV courant, `0` écriture :

| surface | GO | **re-mesure** |
|---|---:|---:|
| cartes dont `context` est en 2ᵉ personne | 25 | **25** ✅ |
| cartes dont `smoothTalker` est en 2ᵉ personne | 12 | **12** ✅ |
| cartes dont `drawer` est en 2ᵉ personne | 10 | **10** ✅ |
| **total** | **47** | **47** ✅ |

**Le chiffre du GO est exact.** Le problème n'est pas le nombre — c'est sa **suffisance**.

### A.2 Pourquoi 47 cellules ne suffisent pas

La 2ᵉ personne de l'anglais de Scenarii ne vit pas dans trois colonnes mais dans **quatre**. La
quatrième, `issue`, n'est pas dans les 47 — et c'est celle qui **casse** :

| colonne | cellules 2ᵉ pers. | dans les 47 ? |
|---|---:|---|
| `context` | 25 | ✅ oui |
| `smoothTalker` | 12 | ✅ oui |
| `drawer` | 10 | ✅ oui |
| **`issue`** | **11** | ❌ **non** |

Les **11 `issue`** se répartissent en deux familles :

- **9 sont sur les cartes mêmes dont on normalise le `context`** —
  `3,0104`, `4,0301`, `7,0102`, `7,0103`, `7,0104`, `7,0105`, `7,0107`, `7,0201`, `7,0206`.
  Ce sont les cartes que le dossier A classe **« cohérentes »** (les deux phrases en 2ᵉ personne).
  Leur `issue` est un **impératif adressé au joueur** — `Justify the incident to the teacher.`,
  `Avoid being accused of adultery.`, `Explain the situation to the nephew's parents.` —
  grammaticalement 2ᵉ personne **sans pronom**.
  ⇒ **Normaliser leur `context` seul les rend incohérentes** : contexte à la 3ᵉ, consigne toujours
  à la 2ᵉ. Les 47 **créent 9 bascules là où il n'y en avait pas.**
- **2 sont sur des cartes hors des 25** — `7,0205` (« Convince your neighbour… ») et `7,0303`
  (« Argue so that she lets you go first. »). Leur `context` est **déjà** à la 3ᵉ personne : ces
  deux cartes **basculent déjà** aujourd'hui, dans le sens inverse. Le GO ne les compte pas.

**Conséquence sur le décompte des cartes qui basculent** : le GO en annonce **16**. Il y en a
**18** (16 + `7,0205` + `7,0303`) — et les 47 en **ajouteraient 9**, portant le total à **27**.

### A.3 La worklist préparée — **58 cellules**

| colonne | n | cartes (`coordonnées`) |
|---|---:|---|
| `context` | 25 | `3,0104` `3,0305` `3,0306` `3,0311` `4,0101` `4,0103` `4,0202` `4,0203` `4,0301` `4,0302` `5,0101` `5,0102` `5,0201` `6,0101` `6,0201` `6,0301` `7,0102` `7,0103` `7,0104` `7,0105` `7,0107` `7,0201` `7,0206` `7,0301` `7,0302` |
| `smoothTalker` | 12 | `3,0101` `3,0102` `3,0103` `3,0104` `3,0203` `3,0306` `3,0311` `4,0202` `4,0208` `4,0302` `7,0207` `7,0301` |
| `drawer` | 10 | `3,0102` `3,0103` `3,0202` `3,0203` `3,0306` `3,0311` `4,0208` `4,0302` `7,0106` `7,0207` |
| **`issue`** | **11** | les 9 ci-dessus **+** `7,0205` `7,0303` |
| **total** | **58** | |

**La cible n'est pas à inventer : le FR canonique est la référence 3ᵉ personne de chaque carte**, et
elle est déjà en 3ᵉ personne partout. La réécriture est donc **mécanique et corroborable ligne à
ligne contre le FR**, exactement comme le GO le justifie :

| `pk` | FR `contexte` (référence) | EN `context` (à normaliser) |
|---|---|---|
| `5,0101` | *Le baratineur est un marin alcoolique qui doit partir à bord d'une fusée expérimentale…* | `You're an alcoholic sailor who had to go to the moon…` |
| `3,0104` | *Le baratineur se réveille en compagnie d'une personne dont il ne se rappelle plus le prénom.* | `You wake up in the company of someone whose name you can't remember.` |
| `7,0102` | *Le baratineur a poussé un camarade dans l'escalier…* | `You're a kid at school. You pushed your classmate down the stairs…` |
| `7,0104` | *Le baratineur est la mère d'un nouveau-né qui n'a manifestement pas la même origine ethnique que son conjoint.* | `You are the mother of a newborn child who is clearly not of the same ethnic origin as your spouse.` |

---

## §3 Pourquoi je n'ai pas écrit A

**Parce que le GO me l'ordonne, noir sur blanc.** Le commentaire owner porte sa propre clause
d'arrêt :

> « La re-mesure détaillée reste portée par la PR #1362 ; **si son chiffre diffère avant écriture,
> le worker doit publier le delta et arrêter plutôt que forcer l'attendu**. »

Le chiffre **47 est confirmé** — mais ce que la re-mesure met au jour, c'est que **47 ne réalise pas
l'attendu**. Exécuter la lettre du GO produirait trois choses, toutes contre son intention :

1. **9 cartes neuves passeraient à l'incohérence** (contexte 3ᵉ + consigne impérative 2ᵉ) — le GO
   dit vouloir « normaliser », pas déplacer le défaut ;
2. **`7,0205` et `7,0303` resteraient fausses** (incohérentes aujourd'hui, hors périmètre du GO) ;
3. l'EN ne serait **toujours pas** homogène à la 3ᵉ personne — l'objectif ne serait pas atteint
   après une écriture qu'on ne peut pas défaire sans une seconde PR.

Écrire 58 cellules de ma propre initiative serait l'erreur symétrique : **dépasser le périmètre
autorisé**. Le GO autorise 47 ; il n'autorise pas 58.

⇒ **Le geste juste était de s'arrêter et de publier le delta.** C'est ce qui est fait ici.

---

## §4 Ce qu'il faut de l'owner

Une décision, sur un écran :

| | |
|---|---|
| **La question** | Le périmètre de la branche A est-il étendu de **47 à 58 cellules** (ajout des **11 `issue`**) ? |
| **Option 1 — étendre à 58** *(recommandée)* | C'est le **seul** périmètre qui atteint l'objectif du GO : EN entièrement à la 3ᵉ personne, **zéro** bascule en milieu de carte. Ajoute 11 cellules, toutes mécaniques depuis le FR. |
| **Option 2 — garder 47** | Il faut alors **assumer explicitement** que les 9 cartes restent mixtes (contexte 3ᵉ, consigne impérative 2ᵉ) et que `7,0205`/`7,0303` restent fausses. Ce n'est pas « normaliser », c'est **déplacer** le défaut de 16 à 25 cartes. |
| **Option 3 — 47 + les 2 hors-25 (49)** | Corrige les deux oublis du décompte sans toucher aux 9, qui restent mixtes. Compromis, mais laisse le même défaut sur 9 cartes. |
| **Ce qui ne change pas** | Les **PDF** : la branche A reste `fr`-seule pour l'apostrophe, et pour la voix c'est **`en/` seul**. `smoothTalker`/`drawer`/`context`/`issue` sont **rendus** ⇒ le lot invalide les cartes EN. |
| **Ordre — inchangé et impératif** | A **puis** les apostrophes (#1363, §6) : normaliser dans l'ordre inverse réécrirait deux fois les mêmes cellules et pourrait réintroduire des courbes. |

---

## §5 Périmètre tenu

⛔ `0` cellule du volet **voix** écrite · ⛔ `0` cellule Schtroumpfs PT · ⛔ `0` OWL touché ·
⛔ aucune régénération · ⛔ aucune branche choisie par le worker · ⛔ `link_*` non touché (URLs).
**Une seule** mutation dans cette PR : les 2 cellules du §1, explicites dans le GO
(« corriger **immédiatement** les deux cellules Tournesol certaines »).

---

*po-2024 — grain ④ du dispatch #458 c.5657210653. Le worker mesure et signale ; l'extension de
périmètre est une décision owner.*
