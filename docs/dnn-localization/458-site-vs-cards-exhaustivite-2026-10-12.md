# 458 — Exhaustivité site → cartes : les 8 sections site-seulement classées (0 règle absente)

**Date :** 2026-10-12 · **Lane :** po-2024 · **Pool :** #458 c.6114818851 item 2 (domaine repris du siège -26)
**Nature :** classification 0-écriture · **Instrument :** `docs/corpus/rules-site-vs-cards.py` (inchangé, rejoué depuis son emplacement committé)

## Question

ai-01 : « L'instrument relève 8 sections présentes sur le site seulement. Classe chacune : règle
absente des cartes, ou simple présentation web. Pour chaque règle absente, fais un lot CSV
différentiel en 8 langues, sur le modèle de #1862. Donne la longueur ajoutée par cellule et par
langue. »

## Réponse courte

**0 règle absente.** Les 8 sections sont toutes de la présentation web : 5 vides (artefacts
structurels), 1 encart de téléchargement, 2 re-rendus tableaux de contenu déjà porté par les
cartes (pictogrammes verbatim). Aucun lot CSV requis — la condition owner du 10/10 (« s'il y a
des règles pas reprises depuis le site, il faut MAJ le csv ») ne se déclenche pas. **Longueur
ajoutée par cellule et par langue : 0 partout** — dans le contexte où Rules_06 porte désormais la
cellule la plus longue du fichier (1 584 caractères, relevé ai-01 12/10), ce grain n'allonge rien.

## Le « 8 » est instrument-dépendant (préprod vs prod)

Rejeu mesuré le 12/10, comptage des lignes `<-> (aucune carte)` :

| Base | Sections site-seulement | Détail |
|---|---|---|
| **préprod** (`dnn.argumentum.myia.io`, post-synchro R-site #1835 + alignage #1862) | **8** | « Contenu » ×5 (une par variante) + « Carte Mémo », « Conditions de Victoire », « Nombre de pioches » (École) |
| **prod** (`www.argumentum.games`, défaut de l'instrument) | **3** | « Carte Mémo », « Conditions de Victoire », « Nombre de pioches » (École) |

La différence = les 5 headings « Contenu » vides qui n'existent que sur les pages préprod (pages
détail restructurées lors de la synchronisation). Même famille que le « 8 » du dossier #1852 : un
compte de sections dépend de la base sondée — publier la base avec le chiffre.

## Classification (8/8)

| # | Variante | Section | Contenu site (préprod) | Classe | Preuve |
|---|---|---|---|---|---|
| 1-5 | ecole, bingo, beau, moulin, parlote | « Contenu » | **corps HTML brut vide** (1 caractère après extraction ; 0 octet de balisage entre le heading et le suivant) | présentation web — artefact structurel | dump HTML brut : `(vide)` |
| 6 | ecole | « Carte Mémo » | « Vous pouvez télécharger la carte de mémo suivante… » + libellés du lien | présentation web — téléchargement | la carte mémo est un artefact du pipeline (CardSet Memo, PDF TarotCards) |
| 7 | ecole | « Conditions de Victoire » | 6 lignes pictogrammes (🥇👇👇…➜🏆 · 🥈👇👇≟🥈👇👇 · ✅🎭➜🎭🏆 · ❌🎭 · ✅👇🎭➜👇🏆 · ❌👇🎭➜🎭🏆) | présentation web — **re-rendu** de Rules_05 | **6/6 lignes présentes verbatim** dans « 5. Le décompte » (Rules_05), en prose avec les pictogrammes inline |
| 8 | ecole | « Nombre de pioches » | 4 lignes pictogrammes (✅🏆➜❌0🎴 · ❌🏆➜✅1🎴 · ✅🧲👇➜✅+1🎴 · ✅👇🎭➜✅+1🎴) + lien mémo | présentation web — **re-rendu** de Rules_06 | **4/4 lignes présentes verbatim** dans Rules_06 (dont les deux « + » du lot #1862) |

Les deux sections École se répartissent sur deux cartes : victoire → **Rules_05** (§ « 5. Le
décompte »), pioches → **Rules_06** (variantes).

## Preuves de couverture (lignes site → cellule carte)

- **Conditions de Victoire → Rules_05** : « Le vainqueur de la manche est celui dont la carte a
  obtenu le plus grand nombre de votes… (🥇👇👇…➜🏆) » · « En cas d'égalité (🥈👇👇≟🥈👇👇), le
  baratineur l'emporte (✅🎭➜🎭🏆) » · « À défaut (❌🎭)… » · « …sont tous déclarés vainqueurs
  (✅👇🎭➜👇🏆) » · « …le baratineur l'emporte (❌👇🎭➜🎭🏆) » — les 6 lignes du site sont ces 6
  parenthèses.
- **Nombre de pioches → Rules_06** : les 4 séquences `✅🏆➜❌0🎴`, `❌🏆➜✅1🎴`,
  `✅🧲👇➜✅+1🎴`, `✅👇🎭➜✅+1🎴` sont présentes telles quelles (sonde par sous-chaîne exacte,
  4/4 PRÉSENT).

## Effet de bord observé (information, pas un défaut nouveau)

La prod est périmée sur les compteurs joueurs — connu et GO-gated (déploiement prod), mais les
deux écarts ne sont pas de même nature :

- école **4-10 vs carte 3-10** : écart **attendu et documenté** (décision owner 23/09, le site
  attend sa re-synchronisation — constante `EXPECTED_CARD_COUNTERS` de l'instrument) ;
- bingo **1-10 vs carte 1-20** : écart **réel** de contenu prod (la préprod est à 1-20, conforme).
Une resynchronisation prod (au GO) emportera les deux.

## Verdict

**Grain soldé par la mesure, 0 écriture.** Le corpus cartes couvre l'intégralité du contenu
règles publié sur le site ; les 8 sections site-seulement sont de la présentation (structure,
téléchargement, re-rendus). Aucun lot CSV, aucune longueur ajoutée.
