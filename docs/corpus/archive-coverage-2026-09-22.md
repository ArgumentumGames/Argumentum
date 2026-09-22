# Couverture archive du deck imprimé — correction d'une borne publiée

**Date :** 2026-09-22 · **Instrument :** [`archive-bridge-instrument.py`](archive-bridge-instrument.py) · **Contexte :** #1499 (périmètre de revue des cartes existantes), #1503

---

## Ce que ce document corrige

J'ai publié le 22/09 que **« 22 des 175 cartes Fallacies ne sont couvertes par aucune archive du dépôt »**, et j'en ai fait une borne du périmètre de relecture : pour ces 22, seul un exemplaire physique arbitrerait.

⛔ **Le chiffre est faux d'un facteur 3.** La couverture réelle est **168/175** ; les cartes sans aucune référence dans le dépôt sont **7**.

| | Publié le 22/09 | Mesuré | Écart |
|---|---:|---:|---|
| Cartes couvertes par une archive | 153 | **168** | +15 |
| Cartes sans aucune référence | 22 | **7** | −15 |

**Conséquence pratique :** 15 cartes que j'avais renvoyées à « exemplaire physique seul » sont en fait arbitrables sur pièces, dans le dépôt, tout de suite.

---

## Pourquoi l'instrument s'est trompé

L'instrument précédent joint le deck courant aux archives sur la colonne **`path`**. Ce choix était motivé et mesuré — `path` apparie 153/169 là où `PK` n'apparie que 29/169, les PK ayant été ré-attribués entre éditions.

Mais `path` **encode la position dans la taxonomie**, et la taxonomie a été **restructurée** entre l'archive v3 et la baseline 2024. Mesure : **34 lignes d'archive changent de `path` à nom constant**. Une carte déplacée d'un niveau devient alors « non appariée », et l'instrument l'étiquetait *« carte nouvelle »* — alors que c'est une carte ancienne, au même nom, simplement remontée dans l'arbre.

Exemple, la carte au cœur de la collision signalée par l'owner :

| État | `path` | Nom |
|---|---|---|
| Archive v3 | `3.1.1.1.1.2` | Généralisation hâtive |
| Baseline 2024 | `3.1.1.1.1` | Généralisation hâtive |
| HEAD | `3.1.1.1.1` | **Induction hâtive** |

Entre v3 et 2024 le **chemin** bouge ; entre 2024 et HEAD le **nom** bouge. Une jointure sur `path` rate le premier saut, une jointure sur le nom rate le second. La carte tombe dans les deux trous à la fois.

> ⭐ **Un invariant de jointure doit survivre au changement légitime.** Choisi sur ce que le changement recherché a le droit de toucher, il accuse la **clé** au lieu du **contenu** — et fait sortir du périmètre du vrai matériau. C'est le même motif que l'invariant `family_fr` sur Virtues (58,3 %, qui n'était que les trois renommages documentés d'août 2026) et que la sonde d'attribution tournée contre un état où le défaut avait déjà eu lieu.

⚠️ Et le défaut d'instrument est **maximal là où le défaut cherché est le pire** : une carte à la fois déplacée *et* renommée échappe aux deux clés. Ce ne sont pas des cas marginaux, ce sont les cas qui motivent la revue.

---

## Le pont

```
archive --(nom)--> baseline 2024 --(PK)--> HEAD
```

- **Maillon 1** sain parce que la dérive de noms est **postérieure** à 2024 : en 2024 les noms sont encore ceux des archives. Mesure : 233/295 lignes d'archive apparient un nom unique de la baseline.
- **Maillon 2** déjà validé : jointure `PK`, invariant `Famille` **175/175**.

### Contrôles inverses

1. **Le danger de la jointure par nom seule est exhibé, pas supposé.** « Généralisation hâtive » existe à HEAD sous *Insuffisance › Argument bâclé* (`1.1`) et en archive sous *Erreur mathématique* (`3.1.1.1.1.2`) : **0 niveau de famille commun sur 3**. Un nom-join les apparierait à tort. L'instrument imprime ce témoin à chaque exécution, et signale si le témoin disparaît du corpus.
2. **Le pont doit rater.** Il laisse **7 irrésolues sur 22**. Un pont qui résoudrait 22/22 serait trop permissif — l'instrument sort en erreur dans ce cas plutôt que de publier ses chiffres.

---

## Trois renommages que seul le pont a fait apparaître

Ils étaient invisibles aux deux instruments précédents : la carte n'était appariée ni par `path` (déplacée) ni par nom (renommée).

| PK | `path` | Nom 2024 | Nom HEAD |
|---|---|---|---|
| 598 | `3.1.1.1.1` | Généralisation hâtive | **Induction hâtive** |
| 603 | `3.1.1.2.1` | Cueillette de cerises | **Picorage de données** |
| 680 | `3.3.1.3.4` | Hypothèse non plausible | **Hypothèse peu plausible** |

⇒ **À verser au dossier d'arbitrage des renommages (grain G1)**, qui en comptait 32. ⛔ Ce document ne juge aucun de ces renommages : il constate qu'ils existent et qu'ils sont désormais arbitrables contre une référence.

---

## Les 7 cartes sans aucune référence dans le dépôt

Ni par `path`, ni par leur nom actuel, ni par leur nom de 2024. Pour celles-ci, **seul un exemplaire physique arbitre** — à nommer comme telles dans les livrables G2/G3 plutôt qu'à traiter comme les autres.

| PK | `path` | Nom |
|---|---|---|
| 105 | `1.2.2.3.1` | Appel au précédent |
| 362 | `2.3.1.1.1.2.1` | Sandwich de louanges |
| 492 | `2.3.2.3.4` | Auto-victimisation |
| 1020 | `6.2.3.4` | Sophisme des coûts irrécupérables |
| 1092 | `6.3.1.2.1.1` | Biais de négativité |
| 1120 | `6.3.1.2.2.1` | Pensée dichotomique |
| 1357 | `7.2.3.4` | Arguments factices |

---

## Ce que ce document n'établit pas

- **Aucun jugement éditorial.** Qu'une carte soit retrouvée en archive ne dit rien sur la qualité de son texte actuel. Le pont apparie, il n'arbitre pas.
- **Un appariement dont le nom a changé reste un candidat**, pas une preuve : il se confirme à l'œil sur les deux textes.
- **Les 7 ne sont pas « absentes de l'édition imprimée »** — elles sont absentes des *fichiers d'archive du dépôt*. L'objet physique peut les porter.
- **Rien n'est corrigé ici.** Changer une carte déjà imprimée reste une décision owner, carte par carte.
