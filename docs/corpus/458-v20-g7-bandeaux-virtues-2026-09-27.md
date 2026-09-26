# Pool v20 — Grain ⑦ : invariant bandeau sur le deck Virtues (mesure 0-écriture)

**Arbre** : master `7cf48616` (2026-09-26) · **Date** : 2026-09-27 · **Lane** : po-2024
**Nature** : mesure, **aucune écriture** — ni CSV, ni garde, ni code. Le réparateur est un grain ultérieur à décider par ai-01.
**Objet** : le même invariant que #1588/#1593 (grain ④ Fallacies), appliqué au deck **Virtues** avec **ses propres conditions d'impression** lues dans son gabarit.

---

## 1. Règle d'impression — lue verbatim dans `Argumentum_Virtues_Face_fr.json`

```handlebars
{{#if subfamily_fr}}
  <div class="famille">{{family_fr}}</div>
  {{#if subsubfamily_fr}}
    <div class="sous_famille">{{subfamily_fr}} | {{subsubfamily_fr}}</div>
  {{/if}}
{{/if}}
```

**Cascade identique au gabarit Fallacies** (`Argumentum_Fallacies_Face_fr.json`, #1588), mais :

| | Fallacies | Virtues |
|---|---|---|
| colonnes bandeau | `Famille`/`Sous-Famille`/`Soussousfamille` (fr), `Family_<lang>` (autres) | `family_<lang>`/`subfamily_<lang>`/`subsubfamily_<lang>` — **minuscules, uniformes** |
| colonne titre | `text_<lang>` | `title_<lang>` |
| colonne deck | `carte` | `card` |

⇒ `family` imprime ssi `subfamily` rempli ; `subfamily` ET `subsubfamily` impriment ssi `subsubfamily` rempli. **Invariant** : toute cellule de bandeau **imprimée** d'une carte deck égale le `title_<lang>` du rang d'ancêtre (préfixe de `path` de longueur 1-3).

## 2. Instrument

Script `g7_measure.py` (scratchpad) : lecteur CSV dict, reconstitution de la cascade, comparaison au titre du rang d'ancêtre. **Témoin vert** : la même mesure rend **fr:0** sur trois refs historiques (§5) — l'instrument voit l'alignement quand il existe, il n'est pas mort par construction.

## 3. Chiffres de contrôle (master `7cf48616`)

- **223 rangées**, **131 cartes deck** (`card` non vide) — conforme au référentiel (CLAUDE.md, #1187).
- **331 cellules imprimées par langue, identiques sur les 8 langues** — les portes de la cascade (`subfamily`/`subsubfamily` remplis) sont un fait **structurel**, pas linguistique : la répartition des niveaux d'impression est la même partout.

### Écarts imprimés par langue

| fr | en | ru | pt | es | ar | fa | zh | **Total** |
|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| 51 | 12 | 26 | 12 | 5 | 29 | 35 | 35 | **205** |

Annexe (divergences **non imprimées**, porte de cascade fermée — invisibles dans les PDF) : fr 4, en 1, ru 2, pt 2, es 1, ar 3, fa 3, zh 4 = **20**.

## 4. Structure : les écarts se groupent par nœud ancêtre — 25 nœuds portent tout

Chaque nœud ci-dessous = un rang dont le titre ne correspond pas au(x) bandeau(x) qui le désignent. « Langues » = langues où au moins une carte deck porte un bandeau divergent vers ce rang.

| pk (rang) | Titre du rang (fr) | Langues | Bandeau porté (exemple fr) |
|---|---|---|---|
| 31 | Représentation parcimonieuse | ru | « Сдержанное представление » |
| 51 | Équilibre émotionnel | fr | « Equilibre émotionnel » — **accent seul** |
| 52 | Neutralité émotionnelle | **8/8** | « Connexion émotionnelle neutre » |
| 53 | Sobriété dramatique | fr pt ar fa | « Réticence dramatique » |
| 54 | Sans chantage aux conséquences | fr pt ar fa | « Pas de chantage par les conséquences » |
| 61 | Échantillonnage représentatif | fr | **accent seul** (É-) |
| 64 | Interprétation rigoureuse des données | fr ru ar fa zh | « Interprétation adéquate des données » |
| 80 | Causalités bien identifiées | fr ar fa zh | « Identification précise des causalités » |
| 81 | Indépendance des prémisses | fr | « Indépendance des prémisses de la conclusion » |
| 82 | Causalité bien orientée | fr | « Causalité correctement orientée » |
| 83 | Exclusion des causes alternatives | fr | « Exclusion de causes alternatives » |
| 84 | Énoncés rigoureux | fr ar fa zh | « Construction rigoureuse des énoncés » |
| 85 | Énoncés propositionnels | fr fa zh | « Enoncés propositionnels bien formés » |
| 86 | Énoncés quantifiés | fr zh | « Enoncés quantifiés bien formés » |
| 87 | Modalités adéquates | fr fa zh | « Utilisation adéquate des modalités » |
| 89 | Raisonnement jalonné | fr | « Raisonnement jaloné » — **accent seul** (un n) |
| 93 | (ru) Доказательное рассуждение | ru | « Убедительное рассуждение » |
| 135 | Définitions claires | **8/8** | « Définitions recevables » |
| 159 | (fr conforme) — ru pt zh divergent | ru pt zh | pt « Clareza dos desafios » |
| 168 | Universalism | en | « Universality » |
| 174 | Mettre les idéologies à distance | fr ru ar fa | « Mise à distance des idéologies » |
| 186 | Ouverture au dialogue | fr ar fa zh | « Disponibilité au dialogue » |
| 200 | Respect du sujet | fr | « Adhérence au sujet » |
| 208 | Évaluation loyale de la position adverse | fr en fa zh | « Evaluation raisonnable de la position adverse » |
| 216 | Courtoisie dans le désaccord | fr | « Courtoisie dans la divergence » |

Contrôle de cohérence : la somme des cellules par nœud reconstitue les comptes par langue (ex. en = 52:1 + 135:4 + 168:3 + 208:4 = 12 ✓).

**Deux nœuds divergent dans les 8 langues** (52 « Neutralité émotionnelle », 135 « Définitions claires ») ; 21 nœuds touchent le fr.

### Trois familles d'écarts

1. **Accent seul** (fr) : pks 51, 61, 89 — le bandeau est le titre sans ses accents (ou « jaloné » à un n près).
2. **Paraphrase descriptive vs libellé court** : pks 80-87, 135, 159, 186 — le bandeau décrit (« Construction rigoureuse des énoncés »), le titre nomme (« Énoncés rigoureux »). Majoritaire.
3. **Libellé différent** : pks 53, 54, 168, 174, 200, 208, 216 — deux mots distincts pour le même nœud (« Sobriété »/« Réticence », « Universality »/« Universalism », « loyale »/« raisonnable »).

## 5. Trajectoire historique — deux fondateurs distincts, un par « côté »

| Ref | Date | deck | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| `3fa04ad0` Introducing Virtues | 2023-05-20 | 159 | **0** | — | — | — | — | — | — | — |
| `e2bc1eba` MAJ manuelle | 2023-12-08 | 113 | **0** | — | — | — | — | — | — | — |
| `25c83aad` MAJ Virtues | 2023-12-15 | 113 | **0** | — | — | — | — | — | — | — |
| `b76af806^` (veille #367) | 2026-05-28 | 113 | **0** | 32 | 5 | 34 | 13 | 101 | 98 | 101 |
| `b76af806` #367 passe clarté FR | 2026-05-28 | 113 | **44** | 32 | 5 | 34 | 13 | 101 | 98 | 101 |
| `7cf48616` HEAD | 2026-09-26 | 131 | 51 | 12 | 26 | 12 | 5 | 29 | 35 | 35 |

Lecture des deux côtés :

- **Côté fr — fondateur unique : #367** (« FR clarity pass gpt-5.5 », 223/223 rangées, 516 cellules). La passe a retravaillé les **titres** de rang ; les **bandeaux** ont gardé le libellé d'origine. Sondage pickaxe : « Définitions claires » (titre pk 135) n'existe qu'à partir de `b76af806`, « Définitions recevables » (bandeau) date de l'import `3fa04ad0`. Fr passe de 0 à 44 d'un seul commit, puis 44→51 avec le deck 113→131.
- **Côté 7 autres langues — fondateur : les passes de traduction** (avril-mai 2026, PRs #218/#236/#246/#290/#295). Dès `b76af806^` les bandeaux traduits divergent massivement (ar 101, zh 101, fa 98) : bandeaux et titres ont été traduits **indépendamment**. Depuis, des passes ultérieures ont réduit (ar 101→29, zh 101→35, pt 34→12, en 32→12) mais **ru a empiré** (5→26).

## 6. Ce que la mesure n'établit pas

- Elle ne dit pas quel côté est « bon » : pour chaque nœud, réparer = choisir le titre de rang ou le bandeau comme libellé de référence (décision éditoriale, ai-01). Les familles 1 (accent) et 2 (paraphrase) ont une réponse mécanique évidente — les bandeaux suivent les titres, comme grain ④ ; la famille 3 (« loyale » vs « raisonnable », « Universality » vs « Universalism ») exige une lecture.
- Elle ne couvre pas les **non-imprimées** au-delà de l'annexe (20 cellules) — porte fermée = jamais rendues, priorité nulle.
- Elle ne mesure pas les cartes hors deck (223−131=92 rangées) : leurs bandeaux ne s'impriment jamais.

## 7. Recommandation (pour arbitrage, pas exécutée)

Même remède que le grain ④ Fallacies (#1593) : **les bandeaux suivent les titres de rang**, exception documentée pour les nœuds où le libellé du bandeau est jugé meilleur. Ampleur : 205 cellules imprimées sur 8 langues, concentrées sur 25 nœuds — un grain unique est faisable, mais la famille 3 (~10 nœuds × langues) demande une décision par nœud. Prérequis identique à ④ : définir si les 92 rangées hors deck sont incluses (recommandé : non — jamais imprimées).
