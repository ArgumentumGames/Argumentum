# Validation visuelle — refonte éditoriale des cartes Rules (#250) — 2026-06-04

Rendu **CardPen** (lane ai-01, Playwright sur IIS local) du template `Argumentum_Rules_fr.json`
+ CSV restructuré, branche `fix/250-rules-editorial` (PR #438). Layout **15 cartes**.

Mesure pixel sur les 15 cartes : **0 px d'overflow partout** (`scrollHeight == clientHeight`).
Le CSS est **indexé par numéro de carte** → indépendant de la langue → ce rendu FR valide
la **mise en page / les couleurs / le tenir-sur-la-carte** pour les 8 langues. Le **contenu**
par langue (déplacements de paragraphes miroir) est la tâche de po-2023.

| Capture | Carte | Problème #250 prouvé corrigé |
|---------|-------|------------------------------|
| `card05_ecole_decompte.png` | 5 — École §4 jury + §5 décompte | **1** : §5 décompte tient **entièrement** sur une carte (avec « En cas d'égalité… ») |
| `card07_bingo_setup.png` | 7 — Bingo setup fusionné | **2** (plus de page-titre vide) + **3** (orange cohérent) |
| `card09_dbp_setup.png` | 9 — Dernier beau parleur setup | **2** + **3** (bleu) + **fit** : densité max, dernière ligne Installation **non coupée** (overflow 14 px corrigé via H1 resserré) |
| `card11_moulin_setup.png` | 11 — Moulin setup fusionné | **2** + **3** (vert) |
| `card13_parlote_setup.png` | 13 — Parlote setup fusionné | **2** + **3** (violet — Parlote **n'est plus en blanc**) |
| `card15_parlote_tours.png` | 15 — Parlote §3 tours + §4 décompte | **6** : « 3. Les tours de jeu » **bien placé** en fin de variante |

Problèmes **4** (orphelin « Fin de partie » DBP) et **5** (orphelin « Fin + Variantes » Moulin)
sont corrigés par fusion dans les cartes gameplay 10 / 12 (vérifié structurellement,
invariant de contenu prouvé byte-identique sur les 9 colonnes).

🔒 Pas de merge avant le sign-off visuel #140 (séquencement release).
