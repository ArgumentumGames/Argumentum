# #458 · P24-07 — balayage de dérive de cascade : mesure, angle mort, reste à décider

**Auteur** : po-2024 (worker lane) · **Date** : 2026-11-23 · **Base** : `origin/master` `93a218e5`
**Nature** : mesure, **lecture seule**. Aucune cellule CSV écrite, aucune traduction produite,
aucun appel API. L'instrument `tools/458-cascade-drift-scan.py` est already committed (24/10) ;
ce rapport livre son **exécution mesurée** et ce qu'elle établit — rien de plus.

---

## §0 — Ce que ce grain demandait, et ce qu'il n'autorise pas

Le pool #458 (l.79) : « Reste `458-cascade-drift-scan.py` seul. » — DoD : « compte avant/après et
chiffres gardés justifiés ; 0 cellule périmée non retraduite ; garde #2235 verte ». La colonne
décision du même tableau porte : « retraduction : clé nommée, crédit accordé par l'owner ».

⇒ **Une retraduction n'est pas un geste de worker.** Ce rapport mesure la liste des candidats,
établit ce que l'instrument peut et ne peut pas conclure, et laisse la décision de retraduction
là où le pool l'a mise : chez l'owner.

---

## §1 — Instrument et périmètre (re-mesurable)

```bash
python -I tools/458-cascade-drift-scan.py --csv "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv" \
    --cards-only --out .tmp/p2407-drift-fallacies.csv
python -I tools/458-cascade-drift-scan.py --csv "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv" \
    --cards-only --out .tmp/p2407-drift-virtues.csv
python -I tools/458-cascade-drift-scan.py --csv "Cards/Scenarii/Argumentum Scenarii - Cards.csv" \
    --out .tmp/p2407-drift-scenarii.csv      # ⇒ erreur de schéma, voir §2
```

**Ce que l'instrument teste** (docstring l.1-16, code l.109-127) : à chaque commit touchant le CSV,
un champ `_fr` a **rétréci** d'au moins `--min-shrink` (12) caractères et la cellule traduite
correspondante n'a **pas** bougé à ce commit — puis un second filtre ne garde que les cellules
encore identiques à `HEAD` (« dropped as already repaired since »). Il distingue explicitement la
dérive de cascade de la localisation légitime ; il **ne juge pas** l'alignement sémantique.

---

## §2 — Résultats mesurés (2026-11-23, `93a218e5`)

| Corpus | commits scannés | candidats au commit | déjà réparés | **actionnables à HEAD** |
|---|---:|---:|---:|---:|
| Fallacies | 164 | 229 | 184 | **45** |
| Virtues | 92 | 0 | 0 | **0** |
| Scenarii | — | — | — | **hors périmètre** |

**Fallacies — 45 lignes sur 13 PK distincts** : `desc` 22 · `text` 16 · `example` 7 ;
`es` 10 · `zh` 8 · `ru` 7 · `fa` 7 · `ar` 6 · `pt` 5 · `en` 2.

**Virtues : 0.** Rien à faire ; c'est un résultat, pas une absence d'instrument (l'instrument a
scanné 92 commits).

**Scenarii : hors périmètre, par construction.** L'outil dérive les colonnes traduites en
`"%s_fr" % champ` et `"%s_%s" % (champ, langue)` ; le CSV Scenarii n'a **ni** `<champ>_fr`
**ni** `<champ>_en` (colonnes EN nues : `title`, `context`, `issue` — asymétrie déjà consignée),
et sa clé de rangée est `path`, pas `PK`. L'outil s'arrête proprement :
`no row key in the header: pass --key-column (looked for PK/pk/Id/id)`. Ce n'est pas un défaut de
l'outil : son schéma de référence est celui de Fallacies/Virtues. Étendre le balayage à Scenarii
demanderait un **prétraitement d'en-tête** (mapper `titre→titre_fr`, `title→titre_en`…), donc un
autre instrument — à décider, pas à improviser.

---

## §3 — Angle mort mesuré : le FR peut faire un **aller-retour**

L'instrument raisonne sur « le FR a rétréci à ce commit ». Si le FR est revenu à une valeur
**antérieure**, la traduction faite sur cette valeur est alignée — et pourtant signalée.

**Preuve (PK 1282, `desc_fr`, historique complet sur 164 commits)** :

| commit | `desc_fr` |
|---|---|
| `e8482fe5` | Vous affirmez que la vérité est subjective et propre à **chaque individu**. |
| `9d45b4f9` | Vous affirmez que la vérité est subjective et propre à **chacun, afin d'éviter de discuter des arguments ou des faits**. |
| `63eacf46` | Vous affirmez que la vérité est subjective et propre à **chaque individu**. ← signalé (loss 45) |

La cellule `desc_es` vaut « Afirmas que la verdad es subjetiva y propia de cada individuo. » —
**mot pour mot le FR revenu**, jamais retouchée. Le détecteur la compte comme candidate.

**Mesure du phénomène sur les 45** : en rejouant l'historique de chaque couple (PK, champ), on
classe un candidat en aller-retour si la valeur `fr_new` apparaît dans l'historique **avant** la
valeur `fr_old` :

| classe | lignes | groupes (PK, champ) |
|---|---:|---|
| **Aller-retour FR** — traduction alignée sur une valeur revenue | **19** | 1282, 713, 808 (chacun 1) · 888 (4) · 632 (6) · 666 `text` (6) |
| **À examiner** — perte réelle possible | **26** | 758, 644, 666 `desc` (6 chacun) · 799 (3) · 361 (2) · 357, 833, 1291 (1 chacun) |

Commande (re-jouable) : historique par `git log --no-merges --format=%H -- <csv>`, un
`git show <sha>:<csv>` par commit, puis comparaison d'index des deux valeurs. **Mesuré**, pas dérivé.

---

## §4 — Les 8 groupes « à examiner », lus un par un

Lecture des trois champs (FR avant → FR après, traduction). Critère : le changement FR est-il un
**resserrage de style / synonyme**, ou un **contenu retiré** que la traduction porte encore ?

| PK | champ | langues | Ce que le FR a changé | Verdict de lecture |
|---|---|---|---|---|
| 758 | `desc` | 6 | « affirmations de départ » → « prémisses » | synonyme — la traduction dit « hypothèses initiales » : même contenu |
| 644 | `example` | 6 | « particularité présente chez l'accusé que l'on ne trouve que chez dix… » → « … et chez seulement 10 % » | reformulation ; les traductions portent **10 %** — alignées sur l'après |
| 666 | `desc` | 6 | « conclusions… raisonnements incorrects » (pluriel) → « une conclusion… un raisonnement incorrect » (singulier) | nombre grammatical — pas une perte de contenu |
| 799 | `desc` | 3 | « de manière à » → « de façon à » ; « les significations établies » → « leur sens établi » | synonyme |
| 361 | `desc` | 2 | « affirmations sur lesquelles tout le monde est d'accord » → « affirmations que tout le monde accepte » | synonyme |
| 357 | `example` | 1 | « instantanément » → « aussitôt » | synonyme (ru « мгновенно » = instantanément) |
| 833 | `desc` | 1 | « comparaison qui est trompeuse et induit en erreur » → « comparaison trompeuse » | resserrage ; zh porte « 误导性的比较 » = comparaison trompeuse |
| 1291 | `desc` | 1 | « vous n'explorez pas suffisamment … en restant à la surface » → « vous restez à la surface … au lieu d'examiner » | reformulation d'une même idée |

⇒ **Aucune perte de contenu démontrée** dans les 8 groupes : les changements FR sont des
resserrements de style, et les traductions restent fidèles au sens de l'après.

⚠️ **Portée exacte de ce verdict** : c'est une **lecture** (latine + zh, dont le texte est fourni),
**pas un oracle**. Pour `ar` et `fa` (PK 758, 644, 666, 799), l'équivalence sémantique n'a pas été
établie par un instrument — je la rapporte comme **supposée d'après la traduction lue**, jamais
comme mesurée. Un oracle de traduction (relecture humaine ou modèle dédié) reste nécessaire avant
de déclarer ces cellules saines.

---

## §5 — DoD de P24-07 : état, sans arrondi

| Critère du pool | État mesuré |
|---|---|
| « compte avant/après et chiffres gardés justifiés » | appartient au Grain A **mergé** (#2264, 161 → 84) ; non re-mesuré ici |
| « 0 cellule périmée non retraduite » | **non démontrable par cet instrument** : il produit des *candidats*, pas des verdicts. Mesure : 45 candidats → 19 prouvés alignés (aller-retour), 26 examinés par lecture → 0 perte démontrée, 4 langues non latines restant **supposées**. Aucune écriture. |
| « garde #2235 verte » | ✅ **23/23 OK** — `python -I tools/test_458-eav-export-guards.py` (0,3 s), la garde vit dans `tools/test_458-eav-export-guards.py` et est câblée dans `.github/workflows/python-tools-tests.yml` (PR #2235, mergée 11/11) |

---

## §6 — Ce que je n'établis PAS

- **Que les 45 candidats soient périmés** — ni qu'ils ne le soient pas. L'instrument ne tranche pas.
- **L'équivalence sémantique en `ar` et `fa`** (§4) : lecture, pas oracle.
- **Que Scenarii n'ait pas de dérive** : il est **hors du périmètre de l'instrument**, ce qui n'est
  pas la même chose que « 0 trouvé ».
- **Qu'un balayage plus fin (seuil ≠ 12) ne changerait pas la liste** : `--min-shrink` n'a pas été
  varié ; 12 est le défaut de l'outil, choisi pour écarter les coquilles.

---

## §7 — Suite proposée (à décider, rien n'est engagé ici)

1. **Aller-retour** : 19 lignes sont alignées par construction — le détecteur pourrait les écarter
   en comparant la valeur revenue à l'historique **avant** d'émettre le candidat (petit ajout,
   ~10 lignes, un témoin synthétique). Non demandé dans cette PR.
2. **Scenarii** : un prétraitement d'en-tête est nécessaire pour l'y porter. Autre grain.
3. **Les 26 restants** : la décision est celle du pool — retraduction **clé nommée + crédit owner**,
   ou clôture motivée après relecture humaine des 8 groupes du §4.

---

## Annexe — les 45 candidats (PK · nombre de cellules · cellules et perte en caractères)

| PK | cellules | colonnes (perte) |
|---|---:|---|
| 1282 | 1 | desc_es (45) |
| 713 | 1 | desc_pt (35) |
| 808 | 1 | desc_es (28) |
| 758 | 6 | desc_ru / desc_pt / desc_es / desc_ar / desc_fa / desc_zh (20) |
| 357 | 1 | example_ru (17) |
| 833 | 1 | desc_zh (17) |
| 1291 | 1 | desc_es (15) |
| 644 | 6 | example_ru / pt / es / ar / fa / zh (15) |
| 666 | 12 | text_en / ru / es / ar / fa / zh (15) · desc_ru / pt / es / ar / fa / zh (13) |
| 888 | 4 | text_ru / es / fa / zh (14) |
| 361 | 2 | desc_es (13), desc_zh (13) |
| 799 | 3 | desc_pt / ar / fa (13) |
| 632 | 6 | text_en / ru / es / ar / fa / zh (12) |

Reproductible par la commande du §1 ; la worklist machine vit hors dépôt (`.tmp/`, gitignoré) et
n'est pas commitée : ce tableau la remplace.

---

*master `93a218e5` · instrument `tools/458-cascade-drift-scan.py` (déjà committé) ·
lecture seule : ⛔ 0 cellule CSV écrite, 0 traduction produite, 0 appel API ·
verdict de retraduction : owner (clé nommée + crédit) · garde #2235 : 23/23 verte.*