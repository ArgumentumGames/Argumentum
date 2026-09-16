# Page Téléchargements — Argumentum v2.0.0 (brouillon FR canonique)

> **Statut : BROUILLON (prep #135 §Téléchargements, pool #458 grain po-2023).** Préparation du contenu de la
> page **Téléchargements** du site DNN pour la release **v2.0.0** (re-scope v0.9.0 → v2.0.0, jsboige 2026-08-06,
> #999). Version FR canonique (le portail sert le FR en primaire). **La publication est GATED** sur #134
> (tag GitHub Release), #132 (déploiement prod) et #131 (DNN 10.3.2 live) — voir « Checklist de publication ».
> **Relecture jsboige requise avant tout publish.** Ce fichier prépare le texte ; il ne publie rien.
>
> **Source de vérité :** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (volet lecteur, chiffres
> re-mesurés + annexe de qualification) · [cards-catalog.fr.md](cards-catalog.fr.md) (formats, volumes) ·
> [news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md) (l'article d'annonce pointe vers cette page) ·
> `manifest-v2.0.0.md` du bundle `review-v2.0.0-regen-20260912` (80 empreintes sha256, `Status: FINAL`,
> base `65dd4742`). Les champs `[PLACEHOLDER]` sont à remplir au moment du tag.
>
> **Tailles** : toutes les tailles PDF ci-dessous sont **MESURÉES le 16/09** sur le bundle final 12/09
> (qualification en annexe A). Les tailles d'**archives zip** seront re-mesurées au packaging (un PDF est
> déjà compressé en Flate ; le zip ne changera l'ordre de grandeur, pas les valeurs).

---

## Avertissement pré-release (à retirer au tag)

⚠️ **Jusqu'au tag `v2.0.0`**, ne pas distribuer les actifs de la pré-release `v0.9.0-review` : ils datent du
24-25/08 et précèdent les corrections de septembre (état détaillé dans
[`134-release-assets-drift-2026-09-15.md`](../release-dossier/134-release-assets-drift-2026-09-15.md), PR #1383).
La page ne doit référencer **que** les assets frais re-uploadés au tag (checklist).

## Organisation par langue — le matériel complet, 8 fois

Chaque langue dispose du matériel **complet** (10 documents). Lien de paquet par langue :
`[PLACEHOLDER — URL asset argumentum-{lang}-v2.0.0.zip, schéma #999 DoD 4 ; nommage
`Print.Play` vs `Print&Play` à trancher au packaging]`.

| Langue | Matériel complet | dont Print & Play seul | Documents |
|---|---:|---:|---:|
| العربية (ar) | **459,9 Mo** | 205,6 Mo | 10 |
| English (en) | **469,0 Mo** | 211,6 Mo | 10 |
| Español (es) | **472,2 Mo** | 212,9 Mo | 10 |
| فارسی (fa) | **470,3 Mo** | 211,4 Mo | 10 |
| Français (fr) — langue source | **465,5 Mo** | 210,2 Mo | 10 |
| Português (pt) | **481,4 Mo** | 217,3 Mo | 10 |
| Русский (ru) | **449,3 Mo** | 204,7 Mo | 10 |
| 中文 (zh) | **478,0 Mo** | 213,4 Mo | 10 |
| **Total** | **3 745,6 Mo** | 1 687,1 Mo | **80** |

« Print & Play seul » = les 4 PDF imprimables maison (tarot standard + Light, poker standard + Light).

## Organisation par format — les 10 documents

| Document | Contenu | Pages | Taille (min..max sur 8 langues) | Usage |
|---|---|---:|---|---|
| `TarotCards` | Deck principal : 15 règles + memo ×7 + 175 sophismes, recto-verso **CMYK** | 379 | 57,9..66,9 Mo | imprimeur |
| `TarotCards_Virtues` | Extension Vertus : 131 cartes, recto-verso CMYK | 262 | 83,1..88,0 Mo | imprimeur |
| `PokerCards` | Deck scénarios : 167 cartes ×2, 7 dos, CMYK | 334 | 25,9..32,7 Mo | imprimeur |
| `TarotCards_Print&Play_A4` | Print & Play tarot (A4, recto-verso) | 105 | 136,1..147,1 Mo | maison |
| `TarotCards_Print&Play_Light_A4` | P&P tarot **Light** (économe encre) | 21 | 29,7..31,4 Mo | maison |
| `PokerCards_Print&Play_A4` | P&P scénarios | 38 | 25,7..32,5 Mo | maison |
| `PokerCards_Print&Play_Light_A4` | P&P scénarios Light | 6 | 6,3..7,4 Mo | maison |
| `Fallacies_Web_A0` | Poster A0 des 175 sophismes | 1 | 22,6..28,1 Mo | affiche |
| `Fallacies_Web_A4` | Poster A4 | 15 | 22,0..27,5 Mo | affiche |
| `Fallacies_Web_Thumbnails_A4` | Planche de vignettes | 9 | 27,0..29,0 Mo | repères |

Volumes de la boîte : **197 / 364 / 495** cartes (fabrication décidée #1187 — détail et qualification dans
les notes de version). Pagecounts = contrat mesuré 80/80 (dossier-validation-mesures, PR #1409 §C2) —
sans confondre les deux decks : tarot **379** pages, poker **334**.

## Organisation par type de paquet

| Paquet | Contenu | Taille | Disponibilité |
|---|---|---|---|
| **Complet** | 80 PDF, les 8 langues | **3 745,6 Mo** MESURÉ | `[PLACEHOLDER — URL]` |
| **Par langue** | 10 PDF d'une langue | 449,3..481,4 Mo (table ci-dessus) | `[PLACEHOLDER — URL ×8]` |
| **Print & Play** | 4 PDF P&P par langue (standard + Light) | 204,7..217,3 Mo | `[PLACEHOLDER — URL]` |
| **Cartes mentales** | SVG Fallacies + Virtues, 8 langues | `[PLACEHOLDER — à mesurer au packaging (SVG du dépôt, hors bundle PDF)]` | `[PLACEHOLDER — URL]` |
| **Ontologie** | `argumentum.owl` + `argumentum_virtues.owl` (SKOS + AIF) | `[PLACEHOLDER — à mesurer au packaging]` | `[PLACEHOLDER — URL]` |

Les PDF d'impression professionnelle sont convertis en **DeviceCMYK avec OutputIntent** (critères
`/GTS_PDFX` + `CGATS TR 001` du guide de validation éd. 3, #1380) ; les cartes en 300 dpi natifs.

## Suivi & SEO (à régler au publish)

- **Analytics téléchargement** : la page est servie par le portail DNN — le porteur GTM/GA est la
  **colonne DB `Tabs.PageHeadText`** (#1399), qui migre avec la base ; la sonde rend **×2 par page**
  (observation NanoClaw + contre-review, propagated pools 16/09). Le clic de téléchargement doit être
  instrumenté `[PLACEHOLDER — événement GTM download_click ou équivalent DNN]`.
- **Meta description** : `[PLACEHOLDER — ~150 car., reprenant « 8 langues, 80 PDF, Print & Play gratuit »]`.
- **Sitemap** : ajouter l'URL de la page au sitemap DNN au publish.

## Checklist de publication (gates #134 / #132 / #131)

- [ ] **#134** — Tag `v2.0.0` + GitHub Release avec **80 assets frais** (pas la pré-release 24/08 ;
      nommage `Print.Play` vs `Print&Play` tranché).
- [ ] Remplacer tous les `[PLACEHOLDER]` : URLs des paquets, événement analytics, meta description,
      tailles des paquets mind maps + ontologie (mesurées au packaging), re-mesure des tailles zip.
- [ ] Retirer l'avertissement pré-release (section ci-dessus) une fois les liens v2.0.0 vivants.
- [ ] Mettre à jour la page **Téléchargements** du portail avec ces tableaux (module DNN concerné :
      `[PLACEHOLDER — module/tabs de la page Téléchargements]`).
- [ ] **Verdict visuel final** = jsboige / ai-01 (le worker signale, ne déclare pas PASS).
- [ ] Relecture jsboige de ce brouillon (condition #999 DoD 3 étendue à la page Téléchargements).

## Traductions (miroirs)

Miroir anglais : [downloads-v2.0.0.en.md](downloads-v2.0.0.en.md). Les 6 autres langues suivent au publish
via le pipeline `DatasetUpdater` (même discipline que #192 — traduction puis validation humaine RTL/CJK).

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — volet lecteur + annexe de qualification des chiffres.
- [cards-catalog.fr.md](cards-catalog.fr.md) — formats, dimensions physiques, volumes (erratum 15/09).
- [news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md) — article d'annonce (checklist alignée).
- `manifest-v2.0.0.md` (bundle `review-v2.0.0-regen-20260912`) — 80 empreintes, `Status: FINAL`, base `65dd4742`.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — cahier des charges §Downloads
  (corps d'origine stale « 4 langues » ; scope réel = 8, cf. article d'annonce).
- [`134-dossier-validation-mesures.md`](../release-dossier/134-dossier-validation-mesures.md) (PR #1409) —
  pagecounts contrat C2 + totaux C1 (corroboration indépendante des tailles).

---

## Annexe A — Qualification des tailles (re-mesure du 16/09)

| valeur | statut | source |
|---|---|---|
| Tailles par PDF (80) et totaux par langue | **MESURÉ** | `find -printf '%s'` sur le bundle `review-v2.0.0-regen-20260912` (16/09, machine po-2023, montage `G:`) ; totaux corroborés par le dossier-validation-mesures PR #1409 §C1 (3 745,6 Mo = manifeste à 0,1 Mo) |
| Pages par document (contrat) | **MESURÉ** | dossier-validation-mesures PR #1409 §C2 (80/80, `gswin64c pdfpagecount`) |
| Tailles des **zips** | **À MESURER** | le packaging n'a pas eu lieu ; les tailles ci-dessus sont des **PDF nus** (déjà Flate-compressés — le zip attendu est du même ordre) |
| Tailles mind maps / ontologie | **À MESURER** | artefacts du dépôt (hors bundle PDF), mesurés au packaging |
| 197 / 364 / 495 | **DÉCIDÉ** + arithmétique vérifiée | #1187 c.`5665864605` (reprise des notes de version, non re-mesuré ici) |

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain #999/#135 · preparation only — aucun tag, aucun upload,
aucune publication · verdict visuel : ai-01 · relecture : jsboige.*
