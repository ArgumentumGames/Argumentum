# Fiche de style — covers de variantes #1369 (grain ⓪″, étape 2a)

**Statut** : fiche AVANT toute génération (dispatch ai-01 16/09). 0 image générée à ce stade.
**Sources échantillonnées** (4 visuels) : `bg-rules.jpg`, `rules-kids.png`, `rules-board.png`, rendu cover FR actuelle `rules_01_face.png` (arbre Release 12/09).

## 1. Famille visuelle — constat

La famille Rules est **photographique composite** (Photoshop 2020-2021), pas vectorielle :
- scènes mises en scène (écoliers en classe, plateau en éventail), éclairage cinématique
- fonds sombres **vignettés** (bords plus sombres, centre légèrement plus clair)
- texture papier/velours grain fin sur les fonds

La cover actuelle (card1) superpose : fond texturé + **plateau lumineux en haut-centre derrière le titre** + **bandeau photographique en bas** + typographie CSS par-dessus.

## 2. Palette (mesurée, quantification médiane 8 couleurs)

| Rôle | Hex | Source |
|---|---|---|
| Fond dominant (rouge brûlé) | `#8a1414` (gamme `#690000`→`#951d1e`) | bg-rules.jpg 21%+14%+13%… |
| Ombres / vignetage (bords) | `#250302` · `#200202` | rules-board.png 63%, rules-kids 16% |
| Accent chaud ambre/bois (lumières) | `#b79069` · `#8c684a` | rules-board 15%, cover actuelle 11% |
| Brun-rouge intermédiaire | `#7a2417` | rules-board 15% |
| Crème (texte/clartés) | `#eadec8` · `#fef4e0` · `#fff9e7` (h1 CSS) | cover actuelle, rules-kids |
| Typographie CSS (référence) | h1 `#fff9e7` TrendSlab · h2 `#d9c1b4` Bebas | styles.css .card1 |

## 3. Traits / texture

- **Vignettage** : dégradé radial du centre (`~#8a1414`) vers les bords (`~#250302`) — systématique.
- **Grain** : texture papier/velours, pas d'aplat numérique propre.
- **Lumière** : source chaude ponctuelle (ambre `#b79069`), contre-jour/clair-obscur, jamais de lumière neutre blanche.
- **Style composite** : assemblage photographique réaliste (personnages/objets mis en scène), PAS d'illustration vectorielle plate générique.

## 4. Cadrage (ratio carte 750×1050 = 70×120 mm)

Reproduire l'architecture de la cover actuelle :
1. **Zone titre LIBRE en haut** (~40% supérieur, légèrement assombri) — le texte localisé restera en CSS par-dessus : ⛔ AUCUN texte dans l'image générée (pilote muet).
2. **Sujet central-bas** : scène photographique occupant ~60% inférieur, pleine largeur, fondue dans le fond sombre (pas de bord franc — fondu type rules-kids en bas de card1).
3. Fond global rouge brûlé vignetté partout ailleurs.

## 5. Direction par variante (une scène chacune, même recette)

| Ligne CSV | Variante | Direction de scène (muette) |
|---|---|---|
| 1 | Argumentum — L'école des menteurs | classe d'écoliers d'un autre siècle, doigt levé (référence = cover actuelle / rules-kids) |
| 7 | Bingo | table de jeu avec plateaux/fiches et jetons, lueur ambre |
| 9 | Dernier beau parleur | duel oratoire : deux personnages face à face, auditoire en pénombre |
| 11 | Moulin à baratin | roue/moulin stylisé + personnages parlant, mouvement |
| 13 | Parlote coinchée | table de coinche, cartes en main, complicité |

**Pilote = 1 seule cover** (1 image). La scène ci-dessus guide le prompt du modèle ; la palette et le cadrage (§2-4) sont contractuels, la scène est directionnelle.

## 6. Contraintes techniques

- Résolution cible : ≥1500×2100 px (raster 2× du 750×1050), format PNG ou JPG qualité max.
- ⛔ Zéro texte, zéro lettrage, zéro logo dans l'image (le titre reste en CSS localisé — c'est l'objet même du #1369).
- Modèle : **stack locale uniquement** (sk-agent) — si aucun modèle image joignable : rapport BLOCKED nommant le besoin, ⛔ pas d'API payante improvisée.
- Livraison asset : `Cards/Rules/Assets/` du worktree feature ; en Debug le chemin est résolu localement (worktree), la validation Release attend le merge (cf #1225/#1228 — un asset de branche n'est pas serviable par les URLs raw master).

## 7. Ce que cette fiche ne fait pas

Pas de PASS, pas de génération. Verdict visuel = ai-01. La fiche sera livrée avec la PR du grain.
