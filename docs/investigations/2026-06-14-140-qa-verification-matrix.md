# Matrice de vérification QA multilingue — relevé de verdict multi-axes (#140)

> **À qui s'adresse ce document.** À ai-01 (lane verdict QA visuelle) pour **capter le verdict** de la validation release 8 langues. C'est un **formulaire cochable** : ai-01 le remplit carte-par-carte / axe-par-axe lors du sign-off release. Il ne s'apprend pas à lire, il se remplit.
>
> **Statut.** Template opérationnel **vierge** (base `330ce493`, 2026‑06‑14). Aucune case n'est pré-remplie — toute cellule `✅`/`❌` doit être posée par ai-01 sur artefact frais. Sert les issues **#140** (QA multilingue) et le gate release **#134**.
>
> **Auteur.** po-2024 (backlog worker), sur dispatch ai-01 `le5ote` (cycle courant).

---

## 0. Positionnement — ce doc vs les deux autres

#140 est déjà outillé par **deux documents matures**. Ce troisième doc est **complémentaire**, pas un duplicat :

| Document | Rôle | Question à laquelle il répond |
|---|---|---|
| [`validation-guide.fr.md`](../publication/validation-guide.fr.md) | **Manuel** (narratif) | *Quoi / où / comment vérifier ?* Drapeaux rouges universels, contrôles par langue (sans la lire), couverture CSV, protocole. |
| [`qa-scenario-8langues-release.md`](../publication/qa-scenario-8langues-release.md) | **Parcours** (pas‑à‑pas, ~25‑30 min) | *Quelles pages ouvrir, dans quel ordre, quels PK cibler ?* Inclut une grille de résultat §5 — mais **trop grossière** (Langue × 5 decks = 1 verdict global). |
| **Ce document** | **Relevé de verdict** (formulaire multi‑axes) | *Le verdict lui-même, décomposé par axe mesurable.* Comble le vide du §5 du scénario : décompose chaque CardSet × langue en **5 axes** au lieu d'un PASS/FAIL global. |

**Lacune que cette matrice comble.** La grille §5 du scénario agrège en 1 verdict par langue. Or une langue peut **PASS le texte** (script correct) mais **FAIL la taxonomie** (harvest périmé) — deux axes indépendants qu'un verdict global masque. Cette matrice force la décomposition pour qu'aucun défaut ne se dilue dans un PASS global.

> **Comment s'en servir.** Au moment du sign-off : ouvrir le **scénario** pour savoir quelles pages regarder, le **guide** pour interpréter ce qu'on voit, puis **remplir cette matrice** pour capturer le verdict axe par axe. Les trois se consomment ensemble.

---

## 1. Convention de remplissage

Chaque cellule = 1 verdict pour 1 CardSet × 1 langue × 1 axe. Symboles :

| Symbole | Signe | Quand |
|---|---|---|
| *(vide)* | — | **Non vérifié** (défaut). Toute case non vide dans le doc livré = verdict posé par ai-01. |
| ✅ | PASS | Axe conforme sur l'artefact validé. |
| ❌ | FAIL | Défaut constaté. **Note obligatoire** (PK/page + symptôme). |
| ⚠ | Doute | À confirmer (ex : overflow limite, qualité de traduction fond). Note obligatoire. |
| ➖ | N/A | Axe non applicable à ce CardSet (ex : overflow sur Thumbnails). |

**Règle d'agrégation du verdict global** (§9) : pour un CardSet × langue, verdict global = **PASS** ssi **tous** les axes applicables (non `➖`) sont `✅`. Un seul `❌` ou `⚠` non levé → **FAIL / bloquant**.

**Portée de la vérif.** Sauf indication, la vérif se fait sur **échantillon ciblé** (premières + dernières cartes par famille, + PK à risque du scénario §3), pas carte-par-carte exhaustif. L'inventaire exhaustif (présence/non-vide) est déjà couvert par le **harnais mécanique #412** (cf guide §2bis) — la matrice ne le rejoue pas.

---

## 2. 🔴 RED FLAG — Signature « harvest périmé » (transverse)

**Avant tout axe, repérer ce pattern — il contamine la taxonomie (axe B) ET peut contaminer le texte.**

| Symptôme | Lecture |
|---|---|
| **Sous-titre / caption / texte d'UI traduit** (ex : « L'art de jamais avoir tort » → EN « THE ART OF NEVER BEING WRONG ») **MAIS taxonomie (Famille/Sous-Famille/Sous-sous-famille) en FR résiduel** | Le harvest `.harvest.json` en cache n'a **pas** été régénéré après le fix de traduction. La régénération a lu le cache, pas le CSV à jour. **Ce n'est PAS un défaut de code ni de traduction — c'est un artefact stale.** |

**Cas avéré de référence.** Guide §2ter « Caveat build Release locale » : `Release/…/en/Images/density-0/Memo/memo_back.png` montrait sous-titre EN **« L'ART DE JAMAIS AVOIR TORT »** (traduit) **+ taxonomie FR** (PRÉJUGÉ · SURINTERPRÉTATION…) → stale pré-#443. **La build Release locale (02-06) n'était pas un artefact release valide pour le Mémo.**

**Détection anti-stale (leçon ai-01 [[feedback_stale_harvest_validation]]).** Avant de re-valider une re-livraison : **hash SHA256 de la capture vs livraison précédente**. SHA identique = capture jamais régénérée → pas un fix, redemander régén avec **clobber explicite** des `.harvest.json` runtime sous `Target/*/Harvest/` (`dotnet clean` ne les vide pas).

**Si le pattern apparaît sur un CardSet × langue** : marquer l'axe B `❌` avec note « **STALE HARVEST — taxonomie FR résiduelle, sous-titre localisé** » et **exiger re-régén avec clobber avant tout PASS**. Ne pas traiter comme un défaut produit.

---

## 3. CardSets de verdict (lignes)

Les CardSets sont **logiques** (contenu source), pas physiques (PDF). Un CardSet peut apparaître dans plusieurs PDFs (ex : Rules dans TarotCards + Print&Play Tarot).

| # | CardSet | PDF(s) à ouvrir | Cartes | Géométrie de référence (axe C) |
|---|---|---|---:|---|
| 1 | **Rules** | `TarotCards_xx` (cover + deck), `TarotCards_Print&Play_A4_xx` | 15 | Cover (carte `class="1"`) + 14 gameplay. Post-refonte #438 (24→15). |
| 2 | **Memo Face** | `TarotCards_xx` | 7 | 1 carte par **famille racine** (7 familles, couleurs familiales). Post-#439. |
| 3 | **Memo Back** | `TarotCards_xx` | 7 | 1 carte par famille racine, chaque carte = **grille de taxonomie** (sous-famille × sous-sous-famille). ⚠ *CardSet à risque stale max* (cf §2). Post-#443/#446. |
| 4 | **Fallacies** | `TarotCards_xx`, `Fallacies_Web_A4_xx`, `Fallacies_Web_A0_xx`, `Fallacies_Web_Thumbnails_A4_xx`, `TarotCards_Print&Play_A4_xx` | 176 | 8 familles racines (couleurs : gris/violet/rose/turquoise/vert/bleu/jaune/rouge). **A0 = 12 colonnes** × ~15 lignes. |
| 5 | **Virtues** | `TarotCards_Virtues_xx` | 114 | 8 familles racines (mêmes couleurs que Fallacies). ⚠ *Overflow body #190/#420*. |
| 6 | **Scenarii** | `PokerCards_xx`, `PokerCards_Print&Play_A4_xx` | ~167 | Format poker. ⚠ *Paragraphes longs (contexte/enjeu), cartes denses*. |

**Langues (colonnes)** : `fr` `en` `ru` `pt` `es` `ar` `fa` `zh`. FR = baseline canonique.

---

## 4. Axe A — Texte localisé (forme)

> **Critère.** Le texte est dans le **bon script** pour la langue, et **aucun résidu FR** ne fuite dans les cartes non-FR (régression #216 historique).
>
> **Mesure** (forme, sans lire le fond) :
> - `fr`/`en`/`pt`/`es` → latin, pas de résidu d'une autre langue.
> - `ru` → **cyrillique** (pas de latin résiduel).
> - `ar` → arabo-persan **RTL + cursif** (lettres connectées, texte démarre à droite).
> - `fa` → persan RTL + lettres persanes **پ چ ژ گ** rendues.
> - `zh` → **sinogrammes** rendus (pas de tofu `□`), ponctuation pleine largeur.
>
> **Résidu FR = FAIL immédiat** (bug #216). Voir guide §3 par langue pour les contrôles forme spécifiques.

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | | | | | | | | |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | | | | | | | | |

---

## 5. Axe B — Taxonomie localisée

> **Critère.** Les labels de **Famille / Sous-Famille / Sous-sous-Famille** sont traduits dans la langue cible (pas de FR résiduel).
>
> **⚠ Vérifier d'abord le RED FLAG §2.** Si sous-titre/caption traduit MAIS taxonomie FR → `❌` note « STALE HARVEST » (artefact stale, pas défaut produit).
>
> **Points de contrôle spécifiques.**
> - **Fallacies** : PK 457 « Déstabilisation » — labels RU `Влияние → Манипуляция сознанием → Игра престолов` complets (pas de labels vides, #421 rattrapé).
> - **Memo Back** : 7 familles racines localisées 8/8 (validation 2026-06-08 guide §2ter : EN PREJUDICE/OVERINTERPRETATION · RU ПРЕДРАССУДОК/Домыслы · PT PRECONCEITO…).
> - **Virtues** : noms de famille localisés (couleurs = repère visuel indépendant de la langue).

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ | ➖ |

*(Rules n'a pas de taxonomie familiale — que des règles de jeu. Scenarii non plus — que des scénarios.)*

---

## 6. Axe C — Géométrie (compter, pas juste lire)

> **Critère.** Le **compte et l'agencement** des cartes/cellules/colonnes est conforme à la référence du §3. Leçon clé [[feedback_verification_geometry_not_just_text]] : pour une carte structurée (grille), **compter la géométrie** (colonnes × lignes par famille), pas seulement « texte traduit ». Un 6/6 PASS « texte » sans géométrie est un **faux PASS**.
>
> **Géométries attendues** (à compter au verdict) :
> - Rules : **15 cartes** (cover + 14).
> - Memo Face / Memo Back : **7 cartes** (1 par famille racine, groupement 8/8 langues invariant via `ifCond` FR).
> - Memo Back interne : **grille de taxonomie par famille** — compter les cellules sous-famille × sous-sous-famille (exemple dispatch « ~3×3/famille » — **à confirmer au verdict**).
> - Fallacies : **176 cartes**, **8 familles racines** (couleurs distinctes).
> - Fallacies A0 : **12 colonnes** complètes (aucune colonne vide).
> - Virtues : **114 cartes**, **8 familles racines**.
> - Scenarii : **~167 cartes** format poker.

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | | | | | | | | |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | | | | | | | | |

---

## 7. Axe D — Overflow / débordement

> **Critère.** Aucun texte ne sort du cadre de la carte (ni en bas, ni à droite).
>
> **Zones à risque connues** (corriger si FAIL) :
> - **Virtues body** — #190 / auto-shrink body #420 (`minFontSize: 7`). Risque max sur zh/ru/es/pt (textes longs).
> - **RU titres** — #316 / auto-shrink #400. Les 10 titres RU les plus longs (scénario §3a) doivent tenir dans le bandeau.
> - **Rules** — fix #250 (`74ef0971`, `card:not([class~="1"]) .texte { max-height: 390px; overflow: hidden; }`). 0 collision non-cover attendue sur 8 langues.
> - **Scenarii** — paragraphes longs (contexte/enjeu), ~7 cartes denses saturent uniformément 8 langues (intrinsèque, pas traduction — cf guide §2bis).
> - **AR/FA RTL** — un layout pensé LTR peut casser en RTL (débordement ou miroir).

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | | | | | | | | |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | | | | | | | | |

---

## 8. Axe E — Cover / titre

> **Critère.** La **page de couverture** est rendue correctement (art cover présent, titre localisé) et le **titre** tient dans son bandeau (auto-shrink actif pour les langues longues).
>
> **Points de contrôle.**
> - **Rules cover** (carte `class="1"`) : fond `bg-rules.jpg` (tableau craie) + `rules-kids.png` + titre craie jaune `#ffc307` localisé (ex FR « L'École des Menteurs »).
> - **FallaciesWeb A0** : entête **logo + QR code** visible en haut.
> - **RU titres** : auto-shrink #400 visible (police plus petite que FR, titre entier dans le bandeau).

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | | | | | | | | |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | | | | | | | | |

---

## 9. Grille de synthèse finale — verdict global par CardSet × langue

> **Agrégat des 5 axes.** Verdict global = **PASS** ssi **tous** les axes applicables (non `➖`) sont `✅`. Remplir **après** les axes §4-§8. **Un seul `❌`/`⚠` non levé → FAIL bloquant.**
>
> Cette grille remplace/affine la grille §5 du scénario (qui était Langue × 5 decks = 1 verdict) : ici on distingue les 6 CardSets logiques, et chaque verdict est la **trace** des 5 axes au-dessus (pas une opinion globale isolée).

| CardSet | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Rules | | | | | | | | |
| Memo Face | | | | | | | | |
| Memo Back | | | | | | | | |
| Fallacies | | | | | | | | |
| Virtues | | | | | | | | |
| Scenarii | | | | | | | | |

**Verdict release (gate #134)** : **GO** ssi cette grille est **intégralement PASS** (toutes cellules `✅` ou `➖`) **ET** couverture CSV 0 gap (guide §4) **ET** tests verts. Toute cellule `❌`/`⚠` ouverte = **NO-GO**, blocker à traiter avant tag.

---

## 10. Notes de remplissage pour ai-01

- **Source d'artefact.** Ne valider que sur une build **`-c Release` fraîche** post-`330ce493`, avec **clobber des harvests Mémo** (`rm Target/*/Harvest/*Memo*`) — sinon axe B Mémo Back = ❌ stale (cf §2). La build Release locale 02-06 n'est **pas** un artefact valide (pré-#443/#446).
- **FR d'abord.** FR sert de **baseline** : mémoriser position/taille du texte sur 2-3 cartes types (scénario §1) avant de juger les autres langues.
- **Fond AR/FA/ZH.** Les axes ci-dessus vérifient la **forme** (script, RTL, glyphes, géométrie, overflow). Le **fond** (sens des phrases) nécessite un relecteur natif — marquer « forme validée, fond en attente » (ne bloque pas la release *forme*, mais documenter, guide §5 point 6).
- **Pas de PASS sans géométrie.** Pour Memo Back et grilles FallaciesWeb : l'axe C (compter les cellules/colonnes) est **obligatoire** avant un PASS axe A/B — sinon faux PASS.
- **Anti-doublon avec harnais.** L'inventaire (présence/non-vide 8 langues) est déjà prouvé par #412 (guide §2bis). Cette matrice ne rejoue pas l'inventaire — elle capture le **verdict qualité** dessus.

---

## 11. Issues liées

- **#140** — QA multilingue (cette matrice en est l'instrument de verdict, aux côtés du guide et du scénario).
- **#134** — GitHub Release v0.9.0 (gate = cette grille §9 PASS + couverture 0 gap + tests verts).
- **#216** — Fuite de langue FR dans PDF EN/RU/PT/ES/AR/FA/ZH (axe A — vérifier la non-régression).
- **#250** — Layout Rules (axe D — fix `74ef0971`, 0 collision non-cover attendue).
- **#190 / #420** — Overflow + auto-shrink body Virtues (axe D).
- **#316 / #400** — Overflow + auto-shrink titres RU (axes D + E).
- **#403** — Remplissage i18n 100 % (axes A/B — cellules vides = FAIL).
- **#411** — Résidus cosmétiques traduction (transverse, non bloquant pour la *forme*).
- **#421** — PK457 RU labels (axe B Fallacies).
- **#443 / #446** — Mémo Back localisation (axe B Mémo Back + signature stale §2).
- **#412 / #431** — Harnais QA mécanique (inventaire pré-prouvé, ne pas rejouer).
