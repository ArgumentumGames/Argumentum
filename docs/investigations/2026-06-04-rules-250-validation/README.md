# Validation visuelle — refonte éditoriale des cartes Rules (#250) — 2026-06-04

Rendu **CardPen** (lane ai-01, Playwright sur IIS local) du template `Argumentum_Rules_fr.json`
+ CSV restructuré, branche `fix/250-rules-editorial` (PR #438). Layout **15 cartes**.

Le CSS est **indexé par numéro de carte** → indépendant de la langue → ce rendu valide
la **mise en page / les couleurs** pour les 8 langues. Le **contenu** par langue
(déplacements de paragraphes miroir) est la tâche de po-2023.

> ⚠️ **Correction (commit `74ef0971`).** La première mesure indiquée ici —
> *« 0 px d'overflow partout (`scrollHeight == clientHeight`) »* — **était fausse** : elle
> mesurait l'auto-fit du conteneur (trivialement `scroll == client` quand la hauteur n'est
> **pas bornée**), **pas** le débordement réel du corps dans la bande de pied de carte absolue
> (`.colorPalette` / `.pageNumber` / labels). La bonne mesure = **bas-du-corps vs haut-du-pied**,
> sur le moteur CardPen live (`#190 autoFitBodyText` + domtoimage), CSV dataset canonique, 8 langues.
>
> **AVANT** (template restructuré, sans hook `.texte`) — collisions réelles : FR #9 +20px, #15 +22px ·
> RU #9 +12px, #12 +23px · **PT #4 +31px, #9 +28px, #10 +39px, #12 +95px** · EN aucune.
> **APRÈS** (`74ef0971`) : **0 collision non-cover** sur fr/en/ru/pt/es/ar/zh/fa.
>
> Correctif (2 lignes, template source) : corps enveloppé en `.texte > .desc_fr` pour activer `#190`,
> + `card:not([class~="1"]) .texte { max-height: 390px; overflow: hidden; }` qui borne la hauteur
> (cover exclue — son art est sur bleed/safe). C'est le check que le harness mécanique #412/#431
> manquait : il ne pilotait jamais le moteur `#190` live.

| Capture | Carte | Problème #250 prouvé corrigé |
|---------|-------|------------------------------|
| `card05_ecole_decompte.png` | 5 — École §4 jury + §5 décompte | **1** : §5 décompte tient **entièrement** sur une carte (avec « En cas d'égalité… ») |
| `card07_bingo_setup.png` | 7 — Bingo setup fusionné | **2** (plus de page-titre vide) + **3** (orange cohérent) |
| `card09_dbp_setup.png` | 9 — Dernier beau parleur setup | **2** + **3** (bleu) + **fit** : densité max, dernière ligne Installation **non coupée** (H1 resserré pour le titre + corps borné `.texte`/`#190` — voir correction ci-dessus, le corps débordait encore de +20px avant `74ef0971`) |
| `card11_moulin_setup.png` | 11 — Moulin setup fusionné | **2** + **3** (vert) |
| `card13_parlote_setup.png` | 13 — Parlote setup fusionné | **2** + **3** (violet — Parlote **n'est plus en blanc**) |
| `card15_parlote_tours.png` | 15 — Parlote §3 tours + §4 décompte | **6** : « 3. Les tours de jeu » **bien placé** en fin de variante |

Problèmes **4** (orphelin « Fin de partie » DBP) et **5** (orphelin « Fin + Variantes » Moulin)
sont corrigés par fusion dans les cartes gameplay 10 / 12 (vérifié structurellement,
invariant de contenu prouvé byte-identique sur les 9 colonnes).

🔒 Pas de merge avant le sign-off visuel #140 (séquencement release).
