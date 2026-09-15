# #134 — Dossier drift des assets de la pré-release : 64 servis vs 80 attendus

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base mesure** : master `c089d526`
**Instrument** : API GitHub releases (`gh api repos/…/releases`), MESURÉ le 15/09 — inventaire intégral
conservé côté worker. **Statut** : **MESURE + QUALIFICATION** — ⛔ `0` upload, ⛔ `0` edit de release,
⛔ `0` tag, ⛔ `0` suppression d'actif.
**Dispatch** : pool #458 c.5666260217 (grain ⑤, actualisé 15/09) — « table remplacer/ajouter/conserver ; aucun upload ».
**Amont** : guide d'acceptation = `134-guide-validation-v2.0.0.md` (éd. 3, PR #1380) · bundle de référence = `review-v2.0.0-regen-20260912` (80 PDF, 3 745,6 Mo, base `65dd4742`).

---

## §0 Le résultat en une phrase

**Chacun des 64 actifs servis précède au moins 38 commits de contenu sur `Cards/`** (base bundle du 24/08 `a8628761`
→ master) — dont la pseudonymisation IP #1189, le retrait de carte #1292 et la correction de rendu #1338 —
et **aucun n'est à conserver** ; **16 actifs Light n'ont jamais été uploadés** ; la pré-release n'est fidèle
ni au bundle 12/09 ni à master.

## §1 Ce qui est servi (MESURÉ, 15/09)

Une seule release : **`v0.9.0-review`** (« v0.9.0 — Visual Review (Thomas & Adeline) »), **pre-release**,
publiée 2026-07-14, non-draft. **64 actifs**, ≈ **3 074 Mo** :

- **61 uploadés le 2026-08-24**, 3 re-uploadés le **2026-08-25** (`PokerCards_Print.Play_A4_{en,es,ru}`)
  — cohérent avec la correction du rendu FR des Poker P&P de l'époque.
- Noms servis : `Argumentum_<Type>_<lang>.pdf` — **`Print.Play` avec un point** (16 actifs), alors que
  `package-v2.0.0.ps1` nomme les types **`Print&Play`** (§5).
- 8 types servis × 8 langues (ar en es fa fr pt ru zh) : `TarotCards`, `TarotCards_Virtues`, `PokerCards`,
  `TarotCards_Print.Play_A4`, `PokerCards_Print.Play_A4`, `Fallacies_Web_A0`, `Fallacies_Web_A4`,
  `Fallacies_Web_Thumbnails_A4`.

## §2 Attendu vs servi

Attendu = **80 actifs** : les 10 types de `$types` (`package-v2.0.0.ps1:65-76`) × 8 langues.

| | actifs | note |
|---|---:|---|
| Servis | **64** | 8 types × 8 langues, uploads 24-25/08 |
| **Manquants** | **16** | exactement `TarotCards_Print&Play_Light_A4` et `PokerCards_Print&Play_Light_A4` × 8 langues — **jamais générés ni uploadés** |

## §3 La fenêtre de drift : `a8628761` (24/08 00:54) → `c089d526` (master)

La base du bundle servi est **`a8628761`** (MESURÉ : ancêtre de master ; uploads API 24-25/08 concordants).
Depuis : **38 commits touchent `Cards/`**. Déficiences portées par les 64 actifs, par gravité :

| catégorie | faits (commits dans la fenêtre) | conséquence sur les PDF servis |
|---|---|---|
| **IP — sensible** | #1189 (`e59ddf26`) : pseudonymisation des **5 cartes portant des identités réelles**, 8 langues | les PDF servis **exposent encore les identités d'origine** — critère de remplacement prioritaire, avant tout partage de la release |
| **Compte de deck** | #1292 (`97431d64`) : PK 96 retiré du deck → **175 faces** Fallacies | TarotCards, TarotCards_Print.Play, Fallacies_Web ×3 portent **176 faces** — compte faux pour tout devis d'impression |
| **Noms & labels** | #1331 PK 511 « Communication non verbale » (8 langues) · #1303 PK 598 pt/ar · #1320 PK 992 · #1319 PK 1311 · #1317 PK 328 · #1316 PK 320 · #1314 PK 449 + 13 refs mortes | cartes servant l'ancien nom visible sur table |
| **Voix / contenus Scenarii** | #1375 **71 cellules** EN 3ᵉ personne · #1367 Tournesol EN/RU · #1295/#1297/#1305/#1308/#1310 (lots adjudiqués) · `bab289c0` noms propres | PokerCards et PokerCards_Print.Play EN/RU périmés sur des dizaines de cartes |
| **Rendu / géométrie** | #1338 (`381ee5c8`) strip alpha natif 300 dpi — **fin de l'aplatissement 720 dpi** (le défaut dont le bundle 11/09 est le témoin) · #1267 tarot 70×120 mm + rognage fond perdu · #1202+#1206 #1190 bannières Rules + gouttière Memo · #1228 police de marque ar/fa/zh | **les 64 PDF portent le défaut de rendu pré-#1338** — tous types confondus |
| **Réseaux / structure** | #1211 réseau relationnel des Vertus · #1286 crosslinks tertiaire + OWL · #1215 25 liens cross-tree · #1265 exemples composites | TarotCards_Virtues et mindmaps embarquées périmées |

Non-drift, pour éviter la fausse ligne : les **renommages de familles Vertus** (#981/#998/#1002, 6-7/08) sont
**antérieurs à la base** — les PDF servis portent déjà les noms actuels (`Inférence maîtrisée`, etc.).

## §4 Table remplacer / ajouter / conserver

| type (× 8 langues) | verdict | qualification principale (§3) |
|---|---|---|
| `TarotCards` (8) | **REMPLACER** | IP #1189 · 176 faces #1292 · PK 511/598/1293 · géométrie #1267 · rendu #1338 |
| `TarotCards_Print.Play_A4` (8) | **REMPLACER** | idem TarotCards (même assemblage P&P) |
| `TarotCards_Virtues` (8) | **REMPLACER** | réseau #1211 · rendu #1338 · géométrie #1267 |
| `PokerCards` (8) | **REMPLACER** | IP #1189 · voix EN #1375 · Tournesol #1367 · lots Scenarii · rendu #1338 |
| `PokerCards_Print.Play_A4` (8) | **REMPLACER** | idem PokerCards |
| `Fallacies_Web_A0` / `_A4` / `_Thumbnails_A4` (24) | **REMPLACER** | 176 faces #1292 · IP #1189 · PK 511/1293 · rendu #1338 |
| `TarotCards_Print&Play_Light_A4` (8) | **AJOUTER** | jamais générés ni uploadés |
| `PokerCards_Print&Play_Light_A4` (8) | **AJOUTER** | jamais générés ni uploadés |
| **Totaux** | **64 REMPLACER · 16 AJOUTER · 0 CONSERVER** | aucun actif servi n'est postérieur au drift |

## §5 Écart de nommage : `Print.Play` servi vs `Print&Play` du script

Les 16 actifs P&P servis portent **`Print.Play`** (point) ; `package-v2.0.0.ps1` définit
**`Print&Play`** (`&`). Le `&` est vraisemblablement translittéré à l'upload (réservation d'URL).
**À trancher avant tout re-upload** : renommer les actifs existants vers la forme servie, ou aligner le
script sur la forme servie — pour que la série complète (80) soit homogène. **Aucune action ici.**

## §6 Repères de fraîcheur (qui est fidèle à quoi)

- **Pré-release servie** (24-25/08) : fidèle à `a8628761` — **38 commits `Cards/` de retard**, rendu pré-#1338.
- **Bundle 12/09** (`review-v2.0.0-regen-20260912`, 80 PDF, base `65dd4742`) : rendu post-#1338,
  deck 175 — mais **2 commits CSV derrière master** (#1367 Tournesol + #1375 71 cellules, MESURÉ :
  `65dd4742` précède les deux).
- **Master `c089d526`** : la seule source couvrant tout — une publication v2.0.0 exigerait une régén
  depuis master (fenêtre gated, cf. gel prod #972), pas une promotion du bundle 12/09.

## §7 Ce que ce dossier n'établit pas

⛔ Aucun upload, edit, tag ou suppression sur la release · ⛔ aucune régénération lancée (fenêtre gated) ·
⛔ aucun verdict visuel (réservé ai-01) · ⛔ ne tranche ni le nommage (§5) ni le moment v2.0.0 (#134 reste
en attente de validation docs ; #654/#965/#1123 HOLD post-tag) · ⛔ ne mesure pas les tailles type-par-type
du bundle 12/09 (le manifeste FINAL 80 sha256 fait foi).

---

*po-2024 — pool #458, grain ⑤. Le worker mesure et qualifie ; l'upload et le tag sont à l'owner.*
