# #830 — Instrument du verdict mindmap (barème comportemental)

**Date** : 2026-08-23 · **Auteur** : po-2024 (worker) · **Statut** : livré, verdict visuel final = ai-01.

## Objet

#830 demande un **golden-master comportemental** pour les wrappers HTML des mindmaps : un ensemble
de capacités falsifiables, à re-cocher à **chaque** régén, qui prouve que le wrapper généré se
comporte réellement comme l'utilisateur le verra. Contrairement à un diff de pixels, il s'agit
d'un *barème* : chaque capacité a un critère passant/échouant.

Le dispatche ai-01 (2026-08-23 04:03) précisait :
> mesurer d'abord ce que couvre déjà `MindmapWrapperGoldenMasterTests` ; ⚠️ le critère
> « zoom initial lisible » a un **délai**, l'omettre rend un faux positif.

Ce document (1) mesure la couverture existante, (2) livre l'instrument qui comble les manques,
(3) matérialise le correctif anti-faux-positif du **délai de zoom initial**.

## 1. Ce que couvre déjà l'existant

### `MindmapWrapperGoldenMasterTests` (headless, Tests/) — SUBSTRING présence
Couvre **#10** (resize/orientationchange wiring) et **#11** (race object-load `initSvgViewer`)
par **présence de sous-chaînes** dans les 34 fichiers committés. Un test de présence : il prouve
la *présence textuelle* du code, pas son comportement. Il reste la **première ligne** de défense
("le code est là"), mais un handler en erreur JS au runtime passerait ce test.

### `MindmapWrapperTests` (Playwright, VisualTests/) — comportement partiel
Couvre partiellement **#5** (3 icônes de contrôle présentes + reset), **#7** (clic → overlay, sur
un SVG *synthétique* 1-nœud), **#9** (clamp min/max → scale > 0, borné). Pas de #1 (zoom initial),
#2 (centrage), #3 (pan), #4 (wheel souris réel — le test existant cliquait les icônes zoom-in/out,
pas `page.Mouse.Wheel`), #6 (dblclick), #8 (couleurs familles).

### Gaps comblés par l'instrument livré
| # | Capacité | Avant | Instrument livré |
|---|----------|-------|------------------|
| 0 | Intégrité placeholders (`[SVGCONTENT]` absent) | partiel | garde dur dans `ComposeIncludedAsync` |
| 1 | Zoom initial lisible + recentrage #829/#831 | **∅** | ✅ `Cap1` (ratio ≥1.5 + settle-wait) |
| 2 | Centrage racine (id=0) au centre fenêtre | **∅** | ✅ `Cap2` |
| 3 | Drag → pan | **∅** | ✅ `Cap3` |
| 4 | Wheel souris → zoom | **∅** (icônes seules) | ✅ `Cap4` |
| 5 | 3 icônes de contrôle présentes | couvert | inchangé (déjà couvert) |
| 6 | Double-clic → zoom | **∅** | ✅ `Cap6` |
| 7 | Clic `.node` réel → overlay | synthétique | ✅ `Cap7` (sur vrai SVG 1400 nœuds) |
| 8 | Couleurs familles (via `familyclass`) | **∅** | **gap documenté** — pas de palette spec |
| 9 | Clamp min/max zoom | partiel | inchangé (déjà couvert) |
| 10 | resize/orientationchange | couvert (substring) | inchangé |
| 11 | race object-load _ext | couvert (substring) | inchangé |

## 2. Le correctif anti-faux-positif : le délai du zoom initial

Dans `included.html`, l'init `svgPanZoom(svg, { fit: 0, minZoom: 0.15, maxZoom: 15, ... })` produit un
zoom initial = fit-to-viewport (≈0.041 pour Fallacies), puis un `requestAnimationFrame` (l.703)
zoome à `height/2600` et **recentre sur `g.node[id="0"]`** via `getBBox` (≈0.32, soit ×7).

Pour un SVG inline de 2.4–5 MB (1400+ nœuds), le parse peut retarder ce rAF de plus d'une seconde.
**Un instrument qui échantillonne le `CTM.a` une seule fois au premier tick peut donc mesurer le
fit transitoire (0.041) alors que le récentrage va se faire — faux positif de régression #831.**
La régression #831 (zoom initial trop lointain pour lire) resterait invisible si on mesurait trop tôt.

**Règle encode dans l'instrument** : ne JAMAIS mesurer un premier échantillon unique. Attendre la
stabilisation — 2 échantillons consécutifs identiques du facteur d'échelle (tolérance 1e-6, max 15 s),
via `WaitForViewportSettledAsync`. Toute future itération du harnais cap #1 doit copier ce settle-wait.

Mesuré : le settle s'observe entre **218 et 621 ms** en headless (les SVGs composés en temp dir se
parse plus vite que les wrappers 2.4–5 MB servis en HTTP — c'est pourquoi le transitoire n'est pas
toujours visible à l'échantillonnage, mais le *principe* reste requis).

## 3. L'instrument livré

`Generation/Converters/Argumentum.AssetConverter.VisualTests/MindmapWrapperCapabilitiesTests.cs`

- **Composition via le chemin pipeline réel** : `MindMapHtmlWrapper.FormatWrapper(template, svgRelPath, svgContent)`
  avec le template committé `included.html` + les `.content.svg` committés — identique au fichier que
  la pipeline écrit sous `Cards/Fallacies/Mindmaps/{lang}/`. **Aucune écriture dans `Cards/**`** (lecture seule).
- **Viewport fixe 1400×900** pour que le critère soit déterministe (indépendant de la taille de fenêtre).
- **Toutes les assertions passent par `WaitForViewportSettledAsync`** (le caveat délai).
- Mesure du scale via `el.getCTM().a` sur `.svg-pan-zoom_viewport` (le réel facteur d'échelle).
- 5 wrappers couverts : fr/ar/zh Fallacies (Latin/RTL/CJK) + fr Virtues (famille régénérée par #983).

### Résultats (headless Chromium .NET, 2026-08-23)

| Test | Wrapper | Mesure | Verdict |
|------|---------|--------|---------|
| Cap1 | fr Fallacies | scale 0.3219 / fit 0.0419 / **ratio 7.69** ; médiane 47.8px ; 1423/1423 ≥9px | ✅ |
| Cap1 | ar Fallacies | ratio **7.69** ; médiane 23.9px ; 1408/1408 ≥9px | ✅ |
| Cap1 | zh Fallacies | ratio **7.69** ; médiane 47.8px ; 1408/1408 ≥9px | ✅ |
| Cap1 | fr Virtues | ratio **2.23** ; médiane 34.7px ; 232/232 ≥9px | ✅ |
| Cap2 | fr Fallacies | root-centre offset dx=18.8px / dy=4.7px (canvas 1384×841, floor 84px) | ✅ |
| Cap2 | zh Fallacies | offset dx=21.5px / dy=0.8px | ✅ |
| Cap3 | fr Fallacies | pan (-491.4,-2781.4) → (-371.4,-2691.4) (delta = drag 120,90) | ✅ |
| Cap4 | fr Fallacies | wheel scale 0.3219 → 0.3469 | ✅ |
| Cap6 | fr Fallacies | dblclick scale 0.3219 → 0.6278 | ✅ |
| Cap7 | fr Fallacies | overlay `.famille` = « Argument fallacieux » (= family du root) | ✅ |

`dotnet test --filter FullyQualifiedName~MindmapWrapperCapabilitiesTests` → **10/10 pass, 32 s**.
Build zero-erreur (une seule alerte Verify SolutionDir, préexistante et bénigne).

## 4. Correction factuelle : les SVGs portent bien `.node`

Le docstring `MindmapWrapperTests.cs` (l.66-69) prétendait « the current Batik SVGs are visual-only
and intentionally carry no semantic node class ». **Faux** : mesure 2026-08-23 — **1400** `class="node"`
dans `Fallacies_fr.content.svg` (1397-1408 selon la langue), **223** dans chaque
`Argumentum_Virtues_MindMap_*.content.svg`, et chaque `.node` porte `family`/`subfamily`/`description`/
`example`/`link`/`depth`/`familyclass` (7596 attributs `family=` sur Fallacies_fr). Docstring corrigée.
⇒ les capacités #7/#8 sont testables sur les **vrais** SVGs — d'où Cap7 sur le SVG réel.

## 5. Gaps restants (honnêteté)

- **#8 couleurs familles** : les `.node` portent `familyclass`, mais la palette mindmap n'est pas
  spécifiée dans un référentiel unique (contrairement aux cartes CardPen dans `CLAUDE.md`). Non
  instrumenté pour éviter un test qui *devine* une couleur. À instrumenter si une spec de palette
  est fixée.
- **Verdict « 16 wrappers × 10 caps » exhaustif** : l'instrument couvre 5 wrappers représentatifs
  + les #5/#9/#10/#11 existants. Étendre à tous les 16 embarqués (+ 16 _ext) est une passe
  d'itération, pas un changement d'architecture (`[InlineData]` à allonger).
- **#10/#11** restent en *présence de sous-chaîne* (headless) — un test comportemental de resize
  réel et de race object-load serait un renforcement, non requis par le dispatche.

## 6. Repro

```bash
dotnet test "Generation/Converters/Argumentum.AssetConverter.VisualTests/Argumentum.AssetConverter.VisualTests.csproj" \
  --filter "FullyQualifiedName~MindmapWrapperCapabilitiesTests"
```

Nécessite Chromium Playwright déjà installé (le projet VisualTests le référence).
Certains tests lancent Chromium avec un SVG 1.5–5 MB composé — comptez ~30–60 s.

## 7. Refs

#830 (barème), #829/#831 (recentrage zoom initial / régression), #983 (régén Virtues 8 langues),
#825 (svg-pan-zoom v3.6.2, CTM), #1037 (resize). Instrument : PR à venir (branche
`fix/830-mindmap-verdict-instrument`).
