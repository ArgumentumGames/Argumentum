# #134 — Dossier de validation — mesures (déroulé du guide éd. 3 sur le bundle 12/09)

**Auteur** : po-2023 (worker lane) · **Déroulé** : 2026-09-16 05:19 → 07:05 (heure locale)
**Bundle** : `G:\Mon Drive\Argumentum\review-v2.0.0-regen-20260912` · **Dépôt de lecture** : `origin/master` `fe567f5a`
**Outils** : `gswin64c` 10.07.1 · Python 3 + PyMuPDF 1.27.1 · `grep -a` (Git-Bash) · `sha256sum`
**Nature** : exécution du protocole C1→C6 du **guide éd. 3** ([`134-guide-validation-v2.0.0.md`](134-guide-validation-v2.0.0.md),
#1380) sur le paquet distribué — la **suite requise n° 2** du verdict 12/09 §8 (c.5649441292) : un dossier qui permet
à un tiers de **refaire** les mesures sans croire l'opérateur sur parole. Chaque ligne = COMMANDE + ATTENDU + CONSTAT,
qualifiée **MESURÉ** (re-joué ici) / **DÉRIVÉ** (calculé depuis un mesuré) / **RAPPORTÉ** (cité, source nommée).
**⛔ Aucun PASS n'est émis** — le worker signale, ai-01/owner déclare. 0 régén, 0 write corpus.

---

## C1 — Inventaire & manifeste (MESURÉ)

| Contrôle | Commande | Attendu | Constat |
|---|---|---|---|
| Compteur | `find "$B" -name "*.pdf" \| wc -l` | 80 | **80** ✓ |
| Répartition | find par langue | 10/langue | 10 ×8 ✓ |
| Tailles par langue | `find … -printf '%s\n' \| awk` (somme Mo) | à émettre | ar 459,9 · en 469,0 · es 472,2 · fa 470,3 · fr 465,5 · pt 481,4 · ru 449,3 · zh 478,0 — **écart max/min 7,1 %** (ru→pt), aucune langue anormale (légère ou lourde) |
| Total | somme des 8 | cohérent manifeste | **3 745,6 Mo** = valeur du manifeste **à 0,1 Mo près** ✓ |
| Manifeste statut | `grep Status` | `FINAL` | `FINAL - attested by operator (-Freshness final)` ✓ |
| Provenance | `grep "Base commit"` | `65dd4742` | `65dd4742` ✓ |
| Empreintes | `grep -c "^- "` | 80 entrées | 86 lignes = **80 entrées** + 6 puces d'en-tête ✓ (format `- {lang}/{type} {sha256}`) |
| Section A0 | `grep A0` | explicite, 8 affiches | table dédiée 8/8 ✓ |
| sha256 témoin | `sha256sum fr/TarotCards` vs manifeste | concordant | `FD0097C2BAC6A50…` = **CONCORDANT** ✓ |
| Contrôle inverse | même hash vs entrée `en/TarotCards` | MISMATCH | `B399D739…` ≠ — comparateur **non aveugle** ✓ |

## C2 — Comptes de cartes (MESURÉ)

Commande : `gswin64c -q -dNODISPLAY -dNOSAFER -c "(<chemin>) (r) file runpdfbegin pdfpagecount = quit"` ×80.
Attendu : contrat uniforme 8 langues. **Constat : 80/80 conformes, 0 écart** —

| Type | Attendu | Mesuré (8/8 langues) |
|---|---:|---|
| TarotCards | 379 | 379 ✓ |
| TarotCards_Virtues | 262 | 262 ✓ |
| PokerCards | 334 | 334 ✓ |
| TarotCards_Print&Play_A4 | 105 | 105 ✓ |
| TarotCards_Print&Play_Light_A4 | 21 | 21 ✓ |
| PokerCards_Print&Play_A4 | 38 | 38 ✓ |
| PokerCards_Print&Play_Light_A4 | 6 | 6 ✓ |
| Fallacies_Web_A0 | 1 | 1 ✓ |
| Fallacies_Web_A4 | 15 | 15 ✓ |
| Fallacies_Web_Thumbnails_A4 | 9 | 9 ✓ |

TSN complet (80 lignes, méthode rejouable) : scratchpad du run — la commande ci-dessus régénère chaque ligne.

## C3 — Conformité imprimeur (MESURÉ, 1 écart consigné sur le guide lui-même)

`grep -a -c` ×5 ×80 fichiers :

| Littéral | Attendu | Constat |
|---|---|---|
| `/DeviceCMYK` | > 0 | **> 0 ×80/80** ✓ |
| `/DeviceRGB` | 0 | **0 ×80/80** ✓ |
| `/GTS_PDFX` | ≥ 1 | **≥ 1 ×80/80** ✓ |
| `CGATS TR 001` | ≥ 1 | **≥ 1 ×80/80** ✓ (OutputConditionIdentifier mesuré : `CGATS TR 001`) |
| `GPL Ghostscript` | ≥ 1 | **≥ 1 ×80/80** ✓ |
| `SWOP` | (anti-critère éd. 3 : « rend 0 ») | **⚠️ ÉCART CONTRE LE GUIDE : 1 ×80/80** — voir §Écarts |

**Log de la passe `--pdf-cmyk`** (`PDF CMYK post-process complete: N/80`) : **non re-mesurable** — le log vivait dans
l'arbre du run 12/09 (recyclé post-livraison) et le bundle ne contient aucun `.log` (constaté). **RAPPORTÉ** : DoD du
run 12/09 (80/80, journal vert — mémoire de lane `project-cmyk-gate-measured`) ; **corroboré** par mes 80/80
`/GTS_PDFX` + `CGATS TR 001` + `/DeviceCMYK` ci-dessus (la passe a réellement traversé les 80 fichiers).

## C4 — Les 8 langues réellement distinctes (MESURÉ — l'item « à émettre au déroulé » est émis)

Instrument guide (rastériseur ≠ producteur) : PyMuPDF `get_pixmap(dpi=50)` + MD5 par page, `Argumentum_TarotCards_{lang}`.

| Paire | Pages | Identiques |
|---|---:|---:|
| fr ↔ ar | 379 | **0** |
| fr ↔ en | 379 | **0** |
| fr ↔ es | 379 | **0** |
| fr ↔ fa | 379 | **0** |
| fr ↔ pt | 379 | **0** |
| fr ↔ ru | 379 | **0** |
| fr ↔ zh | 379 | **0** |

**Constat : identiques 0 ×7 paires** — l'identité inter-langues sur le bundle 12/09 (laissée « à émettre » par le
verdict 12/09) est émise. Limite d'instrument rappelée (guide C4) : détecte la **recopie intégrale**, pas la conformité.

## C5 — Les 7 dos Scenarii (MESURÉ)

Pages **impaires 1-based** du `PokerCards_fr` (⚠️ indices 0,2,4… en 0-based — voir §Pièges), raster 25 dpi, MD5 :

> **167 dos rasters → 7 empreintes distinctes, répartition 36 · 30 · 27 · 25 · 18 · 17 · 14** — conforme à l'attendu
> « exactement 7 » ; même multi-ensemble que la référence bundle 06/09 (RAPPORTÉ : 17/27/36/30/25/18/14).

Constat visuel de teinte/contraste des dos = **ai-01** (jamais ce script).

## C6 — Sentinelles transverses (MESURÉ sauf mention)

| Sentinelle | Constat |
|---|---|
| **Dimensions** (`get_image_info(xrefs=True)`, jamais `get_images`) | TarotCards fr : **379 objets, 379 × 826×1417** ✓ · TarotCards zh (témoin non-latin) : **379 × 826×1417** ✓ · Virtues fr : **262 × 826×1417** ✓ · A0 fr : **176 objets = 175 × 779×779 + 1 × 1316×475** ✓ · **`1982×3401` : 0 occurrence** sur les 4 documents sondés ✓ |
| `/SMask` | **Retiré comme critère sur le livré** (éd. 3) — non mesuré, non coché. |
| **Fraîcheur** (`git diff --name-only 65dd4742..origin/master -- Cards Generation`) | **9 fichiers** à `fe567f5a` : `Cards/Scenarii/Argumentum Scenarii - Cards.csv` (**le seul corpus** — les 73 cellules des commits #1367+#1375, dossier dédié PR #1407) + 7 fichiers de tests + 1 runbook — **inertes pour le rendu**. Le bundle reste le rendu exact de sa base pour tout sauf ces cellules Scenarii. |
| 8 A0 | ✓ (C1) |
| **Versioning OWL** | `owl:versionInfo = 1.0.0` **présent** sur le blob master de `docs/ontology/argumentum.owl` : l'annotation vit en **couple sur 2 lignes** (`<AnnotationProperty IRI="…owl#versionInfo" />` l.13 + `<Literal>1.0.0</Literal>` l.14) ✓ |
| Déterminisme | **NON MESURÉ** — exige une 2ᵉ régén à base identique : **gated** (aucune régén sans fenêtre). |

## Écarts consignés (2, sans correction posée)

1. **C3/SWOP — la phrase « `grep -a -c "SWOP"` rend 0 sur ce bundle — MESURÉ » du guide éd. 3 est un artefact
   d'instrument, pas une mesure.** Fait : les 80 fichiers portent **exactement 1 occurrence** de `SWOP`, dans
   l'OutputIntent Info — bytes réels : `/Info(U.S. Web Coated \(SWOP\) v2)` (parenthèses **échappées**, convention
   des littéraux PDF). Le « 0 » du verdict 12/09 §4 se reproduit en grepant la **forme non échappée**
   `U.S. Web Coated (SWOP) v2` (mesuré : 0) — l'instrument cherchait des bytes qui n'existent pas sous cette forme.
   Conséquence : l'attendu éd. 2 « Info = U.S. Web Coated (SWOP) v2 » était **vrai aussi sur ce bundle** ; la
   falsification éd. 3 était elle-même prise sur l'instrument mort. **La reco opérationnelle éd. 3 reste saine**
   (critères stables = `/GTS_PDFX` + `CGATS TR 001`, tous deux 80/80 ici) — mais pour la bonne raison : SWOP v2
   est **présent partout**, donc un contrôle SWOP vert/rouge ne discrimine rien **sur ce périmètre**, il n'est pas absent.
2. **C5/instrument — parité des dos** : mon premier passage comptait les pages **paires** 1-based (les faces — 167/167
   distinctes, signature attendue de faces) au lieu des impaires. Corrigé en vol, les deux parités jouées, résultat
   ci-dessus sur la bonne. La règle opératoire : **dos = pages impaires 1-based = indices 0,2,4 en 0-based**.

## Pièges d'instrument du déroulé (à la charge du prochain exécutant)

- **PyMuPDF 1.27** : `get_pixmap(dpi=…)` exige un **scalaire** — un tuple lève `TypeError` (mordu deux fois sur cette lane).
- **Littéraux PDF** : les parenthèses y sont échappées `\(…\)` — toute grep de chaîne attendue doit prévoir la forme échappée.
- **Manifeste** : l'empreinte est le **3ᵉ champ** awk de `- {lang}/{type} {sha256}` (le 2ᵉ est le nom).
- **OWL/XML** : une annotation = couple `AnnotationProperty` + `Literal` sur lignes séparées — un grep du nom seul
  voit la déclaration (l.13) et peut se lire à tort comme une absence de valeur.
- **C2 en série** : 80 appels `gswin64c` ≈ 12 min — les paralléliser casse l'ordre du TSN, pas les comptes.

## Ce que ce déroulé n'établit pas

- ⛔ Aucun PASS, aucun verdict — la déclaration appartient à ai-01/owner (rôle QA visuelle réservé).
- ⛔ L'esthétique (comparateur #1313), l'arbitrage éditorial (#994+), la recette DNN (#1180) — hors périmètre guide.
- ⛔ Le log `--pdf-cmyk` original (arbre recyclé) — la preuve de la passe est RAPPORTÉE + corroborée, pas re-jouée.
- ⛔ Le déterminisme (sentinelle) — gated sur fenêtre de régén.
- ⛔ La correction du guide éd. 3 (phrase SWOP) appartient à son auteur/ai-01 — consignée ici sans être posée.

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain ② · exécution du guide éd. 3 (#1380) sur le paquet distribué ·
 instruments datés en tête · verdict visuel : ai-01 · décisions : ai-01/jsboige.*
