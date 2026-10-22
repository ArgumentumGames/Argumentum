# G-CC — retrait du pictogramme `$` du pied des cartes Scenarii

Grain #458 G-CC ([c.6327408025](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-6327408025)).
Objet : `Cards/Scenarii/Argumentum_Scenarii_Face_fr.json`, clé `mustache`, bloc `.footer`.

## Geste

Suppression de la seule ligne `<img …dollar-icon.png">` et de son échappement de fin de ligne.
Le pied conserve `cc` + `img` (BY) + `undo` (SA) + `<div>{{CCby}}</div>`, soit la marque BY-SA.
`Cards/Scenarii/Assets/Scenario-front/dollar-icon.png` **reste** : les 3 gabarits `Archive/2022` l'utilisent encore.

## Instrument

Rendu **CardPen local** (`https://argumentum.myia.io/index.html`), import du document complet via
`#import` — c'est le chemin Debug, donc le gabarit du worktree (le chemin Release lit une URL
épinglée sur `master`, #1225/#1228 : la preuve n'est possible qu'en Debug).

L'état ar/RTL est reproduit fidèlement à ce que fait le pipeline : `dir="rtl"` sur `<html>` (comme DNN
pour ar-SA/fa-IR) et réécriture `argu-lang-fr` → `argu-lang-ar` (`StaticConversions`). Les textes de
la carte restent français : ce rendu prouve la **géométrie** du pied en RTL, pas la traduction.

## Mesures

| Contrôle | Avant | Après |
|---|---|---|
| Images du pied | `cc`, `img`, `dollar`, `undo` | `cc`, `img`, `undo` |
| Largeur du pied (fr / LTR) | 128,9 px | 99,4 px |
| Position verticale / hauteur | y 339,2 · h 27,2 | y 339,2 · h 27,2 |
| Pied en ar / RTL | miroir, 4 images | miroir, 3 images |
| `dollar-icon` dans `mustache`/`css` (46 JSON parsés) | 1 gabarit actif + 3 Archives | 0 gabarit actif + 3 Archives |
| Diff pixel avant/après (fr et ar) | — | confiné à `y ∈ [349,358)` sur 279×375 ; **0 pixel** au-dessus de `y=330` |
| Tests ciblés (`Scenarii`, `FieldMappingContract`, `LanguageSpecificCss`) | — | 279 réussis / 0 échec |

Aucune régénération de PDF : un changement de gabarit n'atteint le bundle imprimé qu'à la prochaine
régénération.

## Images

- `gcc-before-fr.png` / `gcc-after-fr.png`
- `gcc-before-ar.png` / `gcc-after-ar.png` (RTL)