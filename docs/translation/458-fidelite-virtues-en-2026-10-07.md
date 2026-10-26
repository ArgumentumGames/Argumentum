# Campagne de fidélité Vertus — **en** (0 écriture, lecture à deux voies)

**Mandat** : pool #458, file profonde c.5993735448 — grains 6-9, dossiers de fidélité Vertus
**en / ru / pt / es**, aucune écriture, **EN d'abord** (l'EN vérifié devient la troisième colonne
de référence des passes ru/pt/es, comme dans la campagne Scénarios).
**Objet** : `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` — 223 rangées, 82 colonnes,
BOM (la première clé d'en-tête est `﻿pk`) ; le **deck = 131 rangées** à `card` non vide.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

Reprise de l'existant : les passes ⑯/⑰ (titres ar/fa/ru/zh, c.5944233865) et ⑱ (7 cellules
arbitrées owner) sont **vérifiées mergées** sur master — pks 175 (ar/fa/ru), 167 (ar
السعي إلى الموضوعية), 168 (fa جهان‌شمولی), 162/164 (fa sans kasra) ; comptage kasra
corpus : **0**. **L'EN n'avait jamais été lu en entier** : les passes précédentes couvraient
les autres langues et les écrans mécaniques.

---

## 1. Méthode

Écrans mécaniques sur le **corpus entier (223 rangées)** : cellules vides, EN == FR
(non-traduit), mojibake/caractères de contrôle, point final absent (exclusion étendue à
U+201D `”` — voir §2). Puis **lecture intégrale du deck (131 rangées)** en 6 lots,
`title` + `description` systématiquement, `remark` intégral dès que l'écart de longueur
dépassait le seuil (±55 ou +30 caractères) — chaque écart relu contre le FR complet.

Portée déclarée : la lecture couvre le **deck (131/131)** ; les 92 rangées hors deck n'ont
passé que les écrans mécaniques. Un écran borné se lit comme une énumération de sa borne.

## 2. Écran mécanique (corpus 223)

- **0 cellule vide**, **0 EN == FR**, **0 mojibake / caractère de contrôle** sur les champs
  title/description/remark × fr/en.
- **Famille « point final absent » : 13 cellules, toutes `description_en`** — pks
  2, 3, 4, 5, 6, 7, 8, 11, 12, 89, 90, 220, 221. Extrémités mesurées (extrait) :
  `…based on facts or solid reasoning` (2), `…rather than on opinions or assumptions` (3),
  `…reach a rigorous conclusion` (89), `…attentive listening` (220),
  `…to express one's point of view` (221).
- **Aucun FR jumeau de ces 13 pks n'est sans point** — sur la paire FR↔EN, le FR ponctue
  systématiquement et l'EN oublie le point sur 13 cartes.
  ⚠️ **Erratum (07/10, mesure inter-langues du dossier ru #458-g7)** : la formule « l'asymétrie
  est EN-only » publiée ici était bornée à la paire FR↔EN et se lit à tort comme « seule l'EN ».
  La mesure sur les 8 langues (jeu de ponctuation par langue) donne : fr 0 · ru 4 · zh 8 · ar 8 ·
  en 13 · pt 13 · es 13 · fa 18 — **chaque langue a sa propre famille ; le FR est le seul zéro**.
  Les remarks, eux, sont ponctués dans les 8 langues (0 manquant) et les titres n'en portent
  jamais (223/223 dans les 8) : le titre est structurellement exempt.
- Les remarks finissant par `.”` (guillemet bouclé U+201D) étaient des **faux positifs** de
  la première exclusion (ASCII `"` seul) — corrigée avant classification ; pks 99/100/138
  sont sains.
- « Médor » : faux positif résolu (vocabulaire légitime du corpus, pas une contamination).

## 3. Défaut établi (lecture) — 1 entrée

### V-EN-01 · pk 54 `description_en` — inversion de stance (la vertu décrite comme le vice)

| | |
|---|---|
| Titre FR / EN | « Sans chantage aux conséquences » / "No blackmail through consequences" (miroir ✓) |
| `description_fr` | « **Refuser de manipuler** l'interlocuteur par des menaces ou des promesses **liées aux conséquences**. » |
| `description_en` | "**Manipulating** one's interlocutor by resorting to threats or promises constitutes an **affront** to honest and ethical exchange." |

Le FR nomme la **vertu** (le refus) ; l'EN décrit le **vice** (un constat sur ce que fait le
chantage). Ce n'est ni un miroir ni une traduction défendable : la carte EN dit autre chose.
La perte secondaire — « liées aux conséquences » absent de l'EN (le titre porte
"consequences", la description ne le dit plus) — suit le même défaut. Le `remark` EN est,
lui, fidèle (« If you do not accept, everything will be your fault »…) : la description est
bien l'anomalie de la rangée.

**Règle appliquée** (29/09) : un texte qui dit autre chose que le FR est corrigé. **Nature** :
l'EN est une langue de référence imprimée → correction **au grain 10, arbitrage owner**
(forme cible proposée alors : restaurer la stance du FR, ex. "Refusing to manipulate one's
interlocutor through threats or promises linked to consequences."). Ce dossier n'écrit rien.

### V-EN-02 · famille typographique — 13 points finals absents (§2)

« La typographie se corrige toujours » (29/09). Famille mécanique, sans ambiguauté de sens,
**prête pour le grain 10** (une PR, 13 cellules, aucune lecture supplémentaire nécessaire) ;
langue EN imprimée → GO owner requis.

## 4. Observations (neutres — aucune correction proposée ici)

- **Amplification systématique des remarks EN** — mesuré : 130→307 (pk 189), 114→232 (217),
  144→230 (199), 167→237 (180), 171→250 (190), 166→252 (133), 219→269 (156). Ajouts
  typiques : phrase de fermeture ("By respecting these
  practices, a debate can become truly fruitful", 199), "emotional neutrality" (190),
  "misunderstandings or resentment" (217), "synergy of ideas" (180), "better deliberative
  quality" (189), "statistical consensus" (24). **Le contenu FR est préservé partout** —
  c'est un style (l'EN enrichit), pas des inversions. Une règle de correction ne se déduit
  pas d'ici sans arbitrage owner.
- **pk 162 `description` — cadrage dérivé** : FR « Exigence de critères clairs, mesurables et
  équitables pour évaluer la réussite d'une argumentation » → EN "The expected objective for
  **the opponent** must be **attainable** and fair…" — l'adversaire et l'accessibilité sont
  introduits, absents du FR (le sibling 164 porte le cadrage adversaire, côté FR aussi).
  Candidat lecture-owner au grain 10, pas une inversion nette.
- **pk 176 `title` — rétrécissement** : la description dit fidèlement "parties to an
  exchange" (« parties prenantes ») ; le titre EN rétrécit à "**the opponent's** ideological
  biases".
- **pk 128 `title` — affaiblissement lexical** : « Logique informelle **solide** » →
  "**Acceptable** informal logic".
- **pk 79 `remark` — ouverture tautologique** : "Rigorous reasoning uses a rigorous and
  logically valid reasoning methodology" (raisonnement rigoureux = utilise une méthodologie
  rigoureuse) ; le contenu du FR (prémisses solides, données pertinentes, vérification de
  chaque inférence) arrive intégralement ensuite.
- **pk 205 `description`** : ajoute le cadrage "An intellectually honest approach that…",
  absent du FR ; le reste est fidèle.
- **pk 0 `title`** : « Argument **valable** » → "Valid argument" — question de terminologie
  (le corpus réserve "validity" au sens technique à pk 94 « Validité formelle »/formal
  validity). Miroir défendable, noté pour mémoire.
- **Nominalisation systématique des descriptions** (impératif FR « Employer X » → "Use of X")
  — style corpus-wide, fidèle ; documenté pour que les passes ru/pt/es ne le re-signalent pas.
- **Restructurations fidèles des remarks** (pks 24/36/40/41, lots 1-2) : l'EN réordonne mais
  conserve exemples et substance — même famille que l'amplification ci-dessus.

## 5. Ce que ce dossier n'établit pas

- Les 92 rangées hors deck : écrans mécaniques seulement, pas de lecture champ à champ.
- La qualité stylistique de l'EN **en soi** (grammaire, idiomatique) : la lecture compare au
  FR ; elle ne relit pas l'EN comme un texte natif.
- Les autres langues : ru/pt/es font l'objet des grains 7-9, avec l'EN vérifié ci-dessus
  comme colonne de référence.

---

*po-2024*
