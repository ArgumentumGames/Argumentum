# #134 — Dossier de validation release v2.0.0 : page d'entrée pour la relecture

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base** : `origin/master` `e4a54638`
**Pour** : jsboige (relecture WE). **Ce que c'est** : la table des matières décisionnaire du dossier
de validation — chaque pièce mergée, ce qu'elle établit, ce qu'elle **attend de vous**, et l'ordre de
relecture suggéré. Ce n'est ni un PASS, ni une décision, ni un verdict visuel : les pièces font foi.

---

## §1 — Le paquet en une table

| # | pièce (chemin sur master) | PR → merge | ce qu'elle établit | ce qu'elle attend de jsboige |
|---|---|---|---|---|
| 1 | [`134-dossier-validation-mesures.md`](134-dossier-validation-mesures.md) | #1409 → `f1d3f1a0` | Déroulé **C1→C6 intégral** du guide éd. 3 sur le bundle 12/09 : 80/80 PDF, 3 745,6 Mo, manifeste FINAL + contrôle inverse hash, 80/80 pagecounts au contrat, 5 greps CMYK verts, **identité inter-langues 0 ×7 paires ÉMISE**, 7 dos, dimensions/0×1982×3401 | relecture (±20 min) — rien à cocher |
| 2 | [`134-fraîcheur-post-65dd4742-2026-09-16.md`](134-fraicheur-post-65dd4742-2026-09-16.md) | #1407 → `6f315f19` | **6/80 PDF concernés** par les commits corpus post-`65dd4742` (3 documents Poker × {en, ru}), re-dérivés sans recopier le pool ; 74 autres = fraîcheur maintenue | **coche « ciblé 6 vs 80 »** (§3-D2, couplée aux apostrophes) |
| 3 | [`downloads-v2.0.0.fr.md`](../publication/downloads-v2.0.0.fr.md) / [`.en.md`](../publication/downloads-v2.0.0.en.md) | #1410 → `47f0939d` | Page Téléchargements FR/EN : tailles **MESURÉES** (par langue 449,3..481,4 Mo, total 3 745,6, P&P 1 687,1), placeholders nommés (`argumentum-{lang}-{type}-v2.0.0.zip`), bandeau pré-release, checklist gated | relecture (±15 min) — zips à mesurer au packaging |
| 4 | [`1180-dossier-cutover-2026-09-16.md`](../quality/1180-dossier-cutover-2026-09-16.md) | #1411 → `503e64c8` | Cutover DNN : **11 écarts E1-E11** avec propositions qualifiées, ordre 3 phases avec planchers par sonde, **rollback = revert DNS seul** (additif jusqu'à la bascule) | **11 coches E1-E11** (E8 préreçu ✓ — garde livrée) + choix fenêtre DNS |
| 5 | [`131-montee-dnn-2026-09-16.md`](../quality/131-montee-dnn-2026-09-16.md) | #1413 → `dedb81b6` | Montée DNN : état mesuré du jour (7 pages 200, socle 10.3.2/2sxc 21.07, 50 alertes Dependabot classées, CVE 9.11.1 fermées par le palier), 3 vagues, rollback par geste | **6 coches D1-D6** (eshop, npm, CVE, Razor14, E1-E11, Gate 2c) |
| 6 | [`1123-race-assemblage-2026-09-16.md`](../quality/1123-race-assemblage-2026-09-16.md) | #1414 → `e4a54638` | Race d'assemblage #1121 : **collision structurelle** (`GetImageFileName` sans le document — 3 docs partagent le PNG Fallacies), état ARMÉ, séquence TOCTOU, borne honnête, garde proposée | choix de l'option de fix (**post-tag**) — feuille chiffrée à venir (pool grain ④) |
| 7 | [`1049-webroot-guard.md`](../quality/1049-webroot-guard.md) + `DnnWebrootGuardTests` | #1412 → `5b45e19a` (frère #1393 → `517bbecf`) | **Deux déclencheurs, deux gardes** : `bin/` et `web.config` racine hors de l'index Git + boucliers `.gitignore` — le toxique des incidents 10/08 et 16/08 redevient rouge-immédiat | rien (E8 du cutover préreçu ✓) ; Phase 2 appcmd/ACL reste owner |
| 8 | [`RELEASE-NOTES-v2.0.0.md`](../RELEASE-NOTES-v2.0.0.md) (annexe A) | #1385/#1394 | **Erratum volumes ×7 : 197 / 364 / 495** (191/358/489 superseded) — propagé aux articles FR/EN | relecture (±10 min) |

Contexte déjà mergé utile : guide de validation éd. 3 ([`134-guide-validation-v2.0.0.md`](134-guide-validation-v2.0.0.md)) · recettes ([`134-bundle-verification-recipes.md`](134-bundle-verification-recipes.md)) · tri Dependabot pré-tag ([`134-dependabot-pretag-tri-2026-09-15.md`](134-dependabot-pretag-tri-2026-09-15.md)) · dérive assets ([`134-release-assets-drift-2026-09-15.md`](134-release-assets-drift-2026-09-15.md)).

## §2 — Ce qui est VERT sans décision (acquis mesurés)

- **Bundle 12/09 DoD intégral** : 80/80 PDF sur `65dd4742`, 0 `/SMask`, decks 14/14, **CMYK 80/80**, manifeste FINAL sha256 (3 745,6 Mo), arbre 06/09 préservé.
- **Mesures C1→C6 toutes vertes** sur ce bundle (pièce 1) — y compris l'item « à émettre » du verdict 12/09 (identité inter-langues 0 ×7, **ÉMISE**).
- **Chaîne OWL bouclée** : publication Pages automatique prouvée (404 pré-pose → 200 + sha256 post-pose).
- **Gardes webroot actives** en CI (pièce 7) — le côté dépôt de la brèche #1049 est fermé.
- **Préprod saine au 16/09** : 7 pages porteuses 200, socle DNN 10.3.2 + 2sxc 21.07 servi, boutique OpenStore vivante.

## §3 — Les décisions qui restent à jsboige (les coches, par pièce)

| # | décision | où | couplage |
|---|---|---|---|
| D1 | **Apostrophes A/B/C/D** (#994 — dossier mergé `fbc98867`) | issue #994 | débloque le solde EN 22 ; **toute branche sauf D re-touche le deck Poker** |
| D2 | **Régén ciblée 6 vs 80** | pièce 2 (§ fraîcheur) | couplée à D1 : passe unique post-tranche recommandée si ≠ D ; dossier de réservation de fenêtre à venir (pool grain ②) |
| D3 | **Cutover E1-E11** | pièce 4 | E4/E5/E7 couplées (une décision solde les 3) ; fenêtre DNS **avant ~05/10 ou après 04/11 — échéance ferme 04/11** |
| D4 | **Montée DNN D1-D6** | pièce 5 | D5 = même objet que D3 ; fenêtre prod = ops VPS |
| D5 | **Fix #1123 : option** | pièce 6 | post-tag (l'implémentation suit la feuille chiffrée du pool) |
| D6 | **Fenêtre FreeMind** (mindmaps PK 511) | candidats bloqués pool | réservation explicite — jamais fallback XSLT silencieux |
| — | déjà connues du dashboard : #498 P3 A/B/C · #1369 4 coches · #682→#685 provisionnement (49 champs, dry-run mergé #1392) | issues | indépendantes du dossier |

**La porte de sortie** : le tag v2.0.0 dépend du go-live DNN (décisions D3) — et débloque #654/#666/#965, l'upload release et le contact façonniers.

## §4 — Un finding à connaître avant de relire le guide

L'**anti-critère SWOP de l'éd. 3 du guide est un artefact d'instrument** (pièce 1, §Écarts) : les 80
PDF portent exactement 1 occurrence `SWOP` sous forme **échappée** (`/Info(U.S. Web Coated \(SWOP\)
v2)`) ; le « rend 0 » du verdict 12/09 greppait la forme nue. L'attendu de l'éd. 2 était **vrai** sur
ce bundle. La correction du guide revient à son auteur/ai-01 — consigné sans corriger.

## §5 — Ordre de relecture suggéré (WE, ~60 min au total)

1. **Pièce 1** mesures (±20 min) — le cœur : tout est vert, qualifié VERIFIÉ.
2. **Pièce 2** fraîcheur (±10 min) — puis **cocher D2** si D1 est tranché.
3. **Pièce 3** downloads FR/EN (±15 min) — relecture rédactionnelle.
4. **Pièce 4** cutover (±10 min) — les 11 cases, la fenêtre DNS.
5. **Pièces 5-6** (±10 min) — annexes DNN : coches D1-D6, option fix (post-tag, sans urgence).

## §6 — Ce que cette page n'est pas

⛔ Pas un PASS ni un verdict visuel (ai-01) · pas une décision (§3 = les vôtres) · pas une mesure
nouvelle — chaque chiffre cité renvoie à sa pièce et son instrument · pas l'exécution d'aucune
fenêtre (DNS, régén, FreeMind, cutover).

---

*Base `e4a54638` · liens relatifs vérifiés sur master au 16/09 · po-2023 (worker lane) ·
verdict visuel : ai-01 · décisions : jsboige.*
