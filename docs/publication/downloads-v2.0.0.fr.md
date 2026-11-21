# Page Téléchargements — Argumentum v2.0.0 (brouillon FR canonique)

> **Statut : BROUILLON (prep #135 §Téléchargements, pool #458 grain po-2023 ; aligné sur #2234 et la
> correspondance tranchée [c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268)
> par P24-03, 21/11/2026).** Préparation du contenu de la **page Téléchargements** du site DNN pour la
> release **v2.0.0** (re-scope v0.9.0 → v2.0.0, jsboige 2026-08-06, #999). Version FR canonique (le portail
> sert le FR en primaire). **La publication est GATED** sur #134 (tag GitHub Release), #132 (déploiement
> prod) et #131 (DNN 10.3.2 live) — voir « Checklist de publication ». **Relecture jsboige requise avant
> tout publish.** Ce fichier prépare le texte ; il ne publie rien.
>
> **Source de vérité :** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (volet lecteur, chiffres
> re-mesurés + annexe de qualification) · [cards-catalog.fr.md](cards-catalog.fr.md) (formats, volumes) ·
> [news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md) (l'article d'annonce pointe vers cette page) ·
> **[135-downloads-assets-table.md](135-downloads-assets-table.md) — table des 80 noms publiés, DÉRIVÉE de
> `WebBasedGeneratorConfig.cs` et confrontée à la release (PR #2234)** · `manifest-v2.0.0.md` du bundle
> `review-v2.0.0-regen-20260912` (80 empreintes sha256, `Status: FINAL`, base `65dd4742`). Les champs
> `[PLACEHOLDER]` sont à remplir au moment du tag.
>
> **Nommage TRANCHÉ** : `Print.Play` est la **forme publiée** (GitHub réécrit `&` → `.` ; 32/32 PDF P&P
> publiés mesurés `Print.Play`, zéro `&` — dossier #1491). Les liens pointent vers la **release finale**
> (#134), **jamais** vers une pré-release (`v0.9.0-review`, `v2.1.0-review`).
>
> **Tailles** : toutes les tailles PDF ci-dessous sont **MESURÉES le 16/09** sur le bundle final 12/09
> (qualification en annexe A). Les tailles d'**archives zip** seront re-mesurées au packaging (un PDF est
> déjà compressé en Flate ; le zip ne changera l'ordre de grandeur, pas les valeurs).

---

## Avertissement pré-release (à retirer au tag)

⚠️ **Jusqu'au tag `v2.0.0`**, ne pas distribuer les actifs des pré-releases : ceux de `v0.9.0-review`
datent du 24-25/08 et précèdent les corrections de septembre (état détaillé dans
[`134-release-assets-drift-2026-09-15.md`](../release-dossier/134-release-assets-drift-2026-09-15.md),
PR #1383) ; ceux de `v2.1.0-review` ne sont pas le lot approuvé. La page ne doit référencer **que** les
assets frais re-uploadés au tag (checklist).

## Organisation par langue — le matériel complet, 8 fois

Chaque langue dispose du matériel **complet** : **10 documents PDF** (noms publiés dans la
[table dérivée #2234](135-downloads-assets-table.md) — 10 stems × 8 langues = 80 assets), plus **deux
paquets** construits au tag par [`tools/135-build-download-zips.py`](../../tools/135-build-download-zips.py)
et joints à la release finale :

- **Cartes numériques** — `Argumentum_Cards_PNG_<lang>.zip` : les **175** cartes en PNG, reconstruites
  depuis le même arbre de génération que la release (les PNG actuellement servis n'existent qu'en français
  et comptent 157 cartes — doublement périmés) ;
- **Cartes mentales** — `Argumentum_MindMaps_<lang>.zip` : les SVG Fallacies **et** Vertues de la langue,
  pris dans le dépôt (`Cards/Fallacies/Mindmaps/<lang>/`), plus un lien vers la page du site qui les
  affiche.

| Langue | Matériel complet (10 PDF) | dont Print & Play seul | Documents |
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
Il n'y a **pas de zip « langue complète »** : chaque PDF est servi individuellement depuis la release.

## Organisation par format — les 10 documents

Noms de la table dérivée #2234 (forme **publiée**, `Print.Play`) :

| Document | Contenu | Pages | Taille (min..max sur 8 langues) | Usage |
|---|---|---:|---|---|
| `Argumentum_TarotCards` | Deck principal : 15 règles + memo ×7 + 175 sophismes, recto-verso **CMYK** | 379 | 57,9..66,9 Mo | imprimeur |
| `Argumentum_TarotCards_Virtues` | Extension Vertus : 131 cartes, recto-verso CMYK | 262 | 83,1..88,0 Mo | imprimeur |
| `Argumentum_PokerCards` | Deck scénarios : 167 cartes ×2, 7 dos, CMYK | 334 | 25,9..32,7 Mo | imprimeur |
| `Argumentum_TarotCards_Print.Play_A4` | Print & Play tarot (A4, recto-verso) | 105 | 136,1..147,1 Mo | maison |
| `Argumentum_TarotCards_Print.Play_Light_A4` | P&P tarot **Light** (économe encre) | 21 | 29,7..31,4 Mo | maison |
| `Argumentum_PokerCards_Print.Play_A4` | P&P scénarios | 38 | 25,7..32,5 Mo | maison |
| `Argumentum_PokerCards_Print.Play_Light_A4` | P&P scénarios Light | 6 | 6,3..7,4 Mo | maison |
| `Argumentum_Fallacies_Web_A0` | Poster A0 des 175 sophismes | 1 | 22,6..28,1 Mo | affiche |
| `Argumentum_Fallacies_Web_A4` | Poster A4 | 15 | 22,0..27,5 Mo | affiche |
| `Argumentum_Fallacies_Web_Thumbnails_A4` | Planche de vignettes | 9 | 27,0..29,0 Mo | repères |

Chaque document se décline `<stem>_<lang>.pdf` (ex. `Argumentum_TarotCards_Print.Play_A4_fr.pdf`).

Volumes de la boîte : **197 / 364 / 495** cartes (fabrication décidée #1187 — détail et qualification dans
les notes de version). Pagecounts = contrat mesuré 80/80 (dossier-validation-mesures, PR #1409 §C2) —
sans confondre les deux decks : tarot **379** pages, poker **334**.

## Organisation par type de paquet

Correspondance **tranchée** ([c.6650903268](https://github.com/ArgumentumGames/Argumentum/issues/135#issuecomment-6650903268),
05/11) — ce que la page propose aujourd'hui garde sa nature, mis à jour et décliné dans les 8 langues :

| Paquet | Contenu | Taille | Disponibilité |
|---|---|---|---|
| **Complet** | 80 PDF, les 8 langues — pas de zip global | **3 745,6 Mo** MESURÉ | liens par langue ci-dessus (chaque PDF sur la release finale) |
| **Par langue** | 10 PDF + zip cartes + zip mind maps de la langue | 449,3..481,4 Mo (PDF seuls) | `[PLACEHOLDER — URLs release finale #134]` |
| **Print & Play** | 4 PDF P&P par langue (standard + Light) | 204,7..217,3 Mo | `Argumentum_{Tarot,Poker}Cards_Print.Play[_Light]_A4_<lang>.pdf` |
| **Affiche A0** | poster des 175 sophismes | 22,6..28,1 Mo | `Argumentum_Fallacies_Web_A0_<lang>.pdf` (remplace l'ancien zip FR seul) |
| **Planches A4** | poster A4 + planche de vignettes | 22,0..29,0 Mo | `Argumentum_Fallacies_Web_{A4,Thumbnails_A4}_<lang>.pdf` |
| **Cartes numériques** | 175 cartes PNG par langue | `[PLACEHOLDER — à mesurer au packaging]` | `Argumentum_Cards_PNG_<lang>.zip` (construit au tag, joint à la release) |
| **Cartes légères** | 175 PNG densité légère | `[PLACEHOLDER — à mesurer]` | `Argumentum_Cards_PNG_light_<lang>.zip` **si** l'arbre de la release porte cette densité ; sinon le paquet disparaît, remplacé par la planche `Argumentum_Fallacies_Web_Thumbnails_A4_<lang>.pdf` |
| **Cartes mentales** | SVG Fallacies + Virtues par langue | `[PLACEHOLDER — à mesurer au packaging (SVG du dépôt)]` | `Argumentum_MindMaps_<lang>.zip` + lien vers la page du site qui les affiche |
| **Ontologie — sophismes** | OWL des 175 sophismes (SKOS + AIF), **servie par le site** | `[PLACEHOLDER — taille à mesurer]` | `/argumentum_fallacies.owl` (racine du site) |
| **Ontologie — vertus** | OWL des vertus (SKOS + AIF), **servie par le site** | `[PLACEHOLDER — taille à mesurer]` | `/argumentum_virtues.owl` (racine du site) |

> Lien OWL (#133 option A, répétée en préprod le 05/11 — runbook §6) : le nom servi vient de l'**IRI**
> déclarée par le blob, pas du nom de fichier du dépôt (`argumentum.owl` → servi `argumentum_fallacies.owl`).
> Version et date restent à blanc jusqu'au tag.
>
> Les 6 anciens zips du répertoire de téléchargements (18/08, français) ne sont plus liés ; ⛔ aucun n'est
> supprimé sans décision de retrait accompagnée d'une preuve de préservation (c.6650903268). La coquille
> « Argmentum » disparaît avec les nouveaux noms.

Les PDF d'impression professionnelle sont convertis en **DeviceCMYK avec OutputIntent** (critères
`/GTS_PDFX` + `CGATS TR 001` du guide de validation éd. 3, #1380) ; les cartes en 300 dpi natifs.

## Suivi & SEO (à régler au publish)

- **Analytics téléchargement** : la page est servie par le portail DNN — le porteur GTM/GA est la
  **colonne DB `Tabs.PageHeadText`** (#1399), qui migre avec la base ; la sonde rend **×2 par page**
  (observation NanoClaw + contre-review, propagated pools 16/09). Le clic de téléchargement doit être
  instrumenté `[PLACEHOLDER — événement GTM download_click ou équivalent DNN]`.
- **Meta description** : `[PLACEHOLDER — ~150 car., reprenant « 8 langues, 80 PDF, Print & Play gratuit »]`.
- **Sitemap** : ajouter l'URL de la page au sitemap DNN au publish. Balises de partage (`og:`, `twitter:card`)
  absentes aujourd'hui (baseline mesurée 19/11, #135 P23-04) — geste à arbitrer dans le fil #135.

## Checklist de publication (gates #134 / #132 / #131)

- [ ] **#134** — Tag `v2.0.0` + GitHub Release avec **80 assets frais** (pas une pré-release ; nommage
      `Print.Play`, tranché).
- [ ] **Zips construits au tag** (`tools/135-build-download-zips.py`) : `Argumentum_Cards_PNG_<lang>.zip`
      ×8 (+ `light` ×8 **si** l'arbre la porte) et `Argumentum_MindMaps_<lang>.zip` ×8, joints à la
      release finale (po-2023 construit, ai-01 joint — c.6650903268), puis leurs tailles relevées.
- [ ] Remplacer tous les `[PLACEHOLDER]` : URLs des paquets, événement analytics, meta description,
      tailles des paquets cartes + mind maps + ontologie (mesurées au packaging), re-mesure des tailles zip.
- [ ] Retirer l'avertissement pré-release (section ci-dessus) une fois les liens v2.0.0 vivants.
- [ ] Mettre à jour la page **Téléchargements** du portail avec ces tableaux (module DNN concerné :
      `[PLACEHOLDER — module/tabs de la page Téléchargements]`).
- [ ] **Contrôle** (c.6650903268) : la page ne sert plus aucun zip de l'ancien répertoire ; chaque lien
      résout en 200 vers un asset publié de la release finale ; la taille servie égale celle de l'asset.
- [ ] **Verdict visuel final** = jsboige / ai-01 (le worker signale, ne déclare pas PASS).
- [ ] Relecture jsboige de ce brouillon (condition #999 DoD 3 étendue à la page Téléchargements), y compris
      validation humaine RTL/CJK des 6 miroirs.

## Traductions (miroirs)

Miroir anglais : [downloads-v2.0.0.en.md](downloads-v2.0.0.en.md). Les **6 autres miroirs existent**
(P24-03, 21/11) : [ru](downloads-v2.0.0.ru.md) · [pt](downloads-v2.0.0.pt.md) · [es](downloads-v2.0.0.es.md) ·
[ar](downloads-v2.0.0.ar.md) · [fa](downloads-v2.0.0.fa.md) · [zh](downloads-v2.0.0.zh.md). Noms d'assets
identiques dans les 8 fichiers (proper nouns) ; validation humaine RTL/CJK avant publish (même discipline
que #192).

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — volet lecteur + annexe de qualification des chiffres.
- [cards-catalog.fr.md](cards-catalog.fr.md) — formats, dimensions physiques, volumes (erratum 15/09).
- [news-article-v2.0.0.fr.md](news-article-v2.0.0.fr.md) — article d'annonce (checklist alignée).
- [135-downloads-assets-table.md](135-downloads-assets-table.md) — **table des 80 noms publiés + 24 zips,
  dérivée de la config et confrontée** (PR #2234).
- `manifest-v2.0.0.md` (bundle `review-v2.0.0-regen-20260912`) — 80 empreintes, `Status: FINAL`, base `65dd4742`.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — cahier des charges §Downloads
  (corps d'origine stale « 4 langues » ; scope réel = 8, cf. article d'annonce ; correspondance tranchée
  c.6650903268).
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
| Noms publiés (80 PDF + 24 zips) | **DÉRIVÉS** | PR #2234 (`WebBasedGeneratorConfig.cs` + code de l'outil, forme `Print.Play`) |

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain #999/#135 · alignement P24-03 (po-2024, 21/11) ·
preparation only — aucun tag, aucun upload, aucune publication · verdict visuel : ai-01 · relecture : jsboige.*
