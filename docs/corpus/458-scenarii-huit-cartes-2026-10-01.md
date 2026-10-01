# #458 — Scenarii : les 8 cartes « non tranchées » de G5 sont 8 survies déplacées (dossier 0-écriture)

**Date :** 2026-10-01 · **Auteur :** po-2024 · **Base :** `4c288ac2` · **Nature :** dossier 0-écriture
(textes cités + règle C). Aucune cellule, aucun CSV modifié.

## Constat de départ

La clé G5 (#1553, [`g5-cle-scenarii-2026-09-25.md`](g5-cle-scenarii-2026-09-25.md)) couvrait 69/77
cartes de l'archive 2022 et nommait **8 non résolues** — soit retirées, soit renommées et réécrites
au-delà de la reconnaissance. Le départage exigeait une décision humaine, carte par carte.

Ce dossier fait cette lecture. **Verdict : les 8 ont toutes survécu, déplacées vers un nouveau
`path` et souvent réécrites.** Aucune carte 2022 n'est perdue ; l'instrument ne les voyait pas
parce que E1 exige le titre normalisé **exact** (elles étaient renommées) et E2 est **ancré sur le
`path`** (elles avaient bougé). La couverture réelle de la clé passe de 69/77 à **77/77** — les 8
dernières par lecture humaine, pas par l'instrument.

## Méthode

Corpus 2022 : `Cards/Scenarii/Archive/2022/Argumentum Scenarii - Cards fevrier 2022.csv` (77 cartes).
Corpus courant : `Cards/Scenarii/Argumentum Scenarii - Cards.csv` (167). Pour chaque carte 2022 :
lecture du titre/contexte/enjeu, puis recherche par thème dans le corpus courant (mots-caractères
des textes 2022). Appariement jugé sur le contenu cité, pas sur le titre.

## Les 8, carte par carte

### 1. Louis XVI — survie à `1,0205`

| | 2022 (`1.3.3`) | Courant (`1,0205`) |
|---|---|---|
| Titre | Louis XVI **tâche de** garder sa tête | Louis XVI **veut** garder sa tête |
| Contexte | « Pendant la révolution Française, le baratineur est jugé par le tribunal révolutionnaire. » | « Pendant la Révolution française, Louis XVI est jugé par le tribunal révolutionnaire. » |
| Enjeu | « Il doit le convaincre de ne pas le condamner à mort. » | (même enjeu, personne nommée) |

L'occupant actuel de `1.3.3` (« Maréchal, nous voilà », la Collaboration) est une **autre carte** —
le path a été réutilisé, pas la carte.

### 2. Truman — survie à `1,0302`

| | 2022 (`1.4.2`) | Courant (`1,0302`) |
|---|---|---|
| Titre | président Truman et la Bombe A | Truman et la bombe A |
| Contexte | « les conseillers du président affirment que l'ennemi est prêt à se rendre » | « les conseillers du président Truman affirment que l'ennemi est prêt à [se rendre] » |
| Enjeu | justifier la bombe atomique malgré la reddition imminente | idem |

`1.4.2` est **vide** dans le courant (l'un des 3 paths vides) — la carte a émigré.

### 3. Don Juan — survie à `2,0305` (réécriture)

2022 : « vie de **stupre et de corruption**… devant le spectre qui doit le juger… convaincre que son
attitude était honnête ». Courant : « vie de **débauche**, Dom Juan se tient devant le spectre
**chargé de le juger pour ses péchés** ». Même scène, même spectre, même enjeu — orthographe
« Dom Juan » (Molière) retenue. L'occupant de `2.1.2` (« Pain d'épices », Hansel et Gretel) est
une autre carte.

### 4. « mariage ou pas » — survie à `3,0310` (réécriture)

2022 : « Cela fait cinq ans… relation harmonieuse avec son conjoint. Il ou elle le demande en
mariage. Le baratineur doit refuser sans rompre. » Courant (« Mariage ? Non merci ») : « vit depuis
cinq ans une relation harmonieuse avec son conjoint, qui lui propose [le mariage] » — même durée,
même situation, même enjeu du refus qui ne rompe pas. ⚠️ Ne pas confondre avec « Mariage de
pierre » (`3,0302`, désaccord sur l'idée du mariage) : c'est bien `3,0310` la descendante.
L'occupant de `3.2.3` (« Le ménage à trois ») est une autre carte.

### 5. « 2+1 » — survie à `3.3.5` (renommée « Plan à trois »)

2022 : « Le baratineur est la femme d'un couple. Son époux suggère de corser votre vie sexuelle
avec un plan à trois avec une autre femme. Le baratineur doit le convaincre que ce soit plutôt
avec un autre homme et lui. » Courant : « Son époux propose de pimenter leur vie sexuelle avec un
plan à trois a[vec…] / Le baratineur doit le convaincre d'inviter plutôt un autre homme. » —
**enjeu quasi verbatim**. `3.4.1` est vide dans le courant.

### 6. « rétrogradation canapé » — survie à `4.2.8` (renommée « Réveil compromis »)

Contexte et enjeu **verbatim** : « Le baratineur se réveille après une soirée bien arrosée qui lui
a laissé peu de souvenirs. Dans le lit à ses côtés, il découvre son patron manifestement enchanté
de la situation. / Il doit l'éconduire sans perdre son emploi. » L'occupant de `4.1.4`
(« Le professeur », recherche médicale) est une autre carte.

### 7. « 5G » — survie à `5,0304` (réécriture légère)

2022 : « Le baratineur pense la 5G est responsable de la pandémie du Coronavirus. Un touriste
Sud-Coréen lui explique qu'il l'utilise dans son pays depuis le début de l'année 2019. » Courant
(« La conspiration de la 5G ») : « pense que la 5G est responsable de la pandémie de coronavirus.
Un touriste s[ud-coréen…] » — même carte, corrigée (le « pense la 5G est » fautif est devenu « pense
que la 5G est »). L'occupant de `5.3.3` (« Remplaçant cryogénique ») est une autre carte.

### 8. « miaw » — survie à `7,0107` (resPELLée « Miaou »)

2022 et courant : « Le baratineur est un chat au régime. / Il essaie de convaincre son maître de
lui ouvrir une boîte de sardines à l'huile. » — textes identiques à l'orthographe près
(« miaw » → « Miaou », « boite » → « boîte »). `7.4.2` est vide dans le courant.

## Règle C appliquée

Pour chacune : **garder le texte courant**. Les réécritures existantes sont des gestes éditoriaux
documentés par G6/G6-C (#1556/#1559 — « 0 revenir » mesuré sur les pertes FR), et aucune des 8
n'est une perte. Ce dossier ne demande **aucune écriture** : il referme la dernière boîte noire de
la couverture G5.

## Ce que ce dossier n'établit pas

- Aucun jugement sur la qualité des réécritures — les arbitrages de contenu relèvent de l'owner.
- La couverture 77/77 est **un constat de descendance** (contenu cité), pas une égalité
  titre-à-titre : E1/E2 restent à 69 pour l'instrument, qui n'apprend rien de cette lecture.
- Les paths 2022 réutilisés par d'autres cartes (`1.3.3`, `2.1.2`, `3.2.3`, `4.1.4`, `5.3.3`)
  restent ce qu'ils sont — ce dossier ne re-clé rien.

Refs #458 · Refs #1499

🤖 Generated with [Claude Code](https://claude.com/claude-code)

*po-2024*
