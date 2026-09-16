# #134 — Fenêtre de régénération v2.0.0 : dossier de réservation (décision-support)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base** : `origin/master` `e4a54638`
**Portée** : pool #458 c.`5666259809`, grain ② — « périmètre conditionnel (ciblé 6 PDF si D apostrophes ·
80 sinon — dossier #1407), prérequis, durée estimée, DoD de sortie, créneaux types ; rend le choix
décidable, **aucune réservation exécutée** ».
**Nature** : lecture seule intégrale — git + code master + disque. ⛔ Aucun run, aucun clobber, aucune
réservation, aucune écriture corpus. Chaque chiffre porte son qualificatif : **MESURÉ** (rejoué ce jour) /
**DÉRIVÉ** (calculé depuis un mesuré) / **RAPPORTÉ** (cité, source nommée).

---

## §0 — Le résultat en une phrase

La fenêtre de régén v2.0.0 est **décidable en deux choix** : (1) la tranche apostrophes **A/B/C/D** fixe le
périmètre (6 / 12 / 19 / ≈52 PDF une fois l'union avec le rayon fraîcheur faite) ; (2) la durée totale
planifiée est **4 h** (run ≈ 73-80 min RAPPORTÉ + mesures C1→C6 ≈ 1 h 45 MESURÉ + verdict ai-01). Prérequis
**vérifiés ce jour** : pas de fenêtre FreeMind — `AssetConverterConfig.cs:59` exclut le stage Mindmapper du
Mode par défaut — et disque VERT (269,3 Go libres vs ≈ 11,7 Go par run). ⛔ **Aucune réservation posée** :
ce dossier équipe le choix, il ne l'exécute pas.

## §1 — Le périmètre conditionnel : c'est la tranche apostrophes qui décide

### Les deux composantes du delta (l'une déjà mergée, l'autre pendante)

1. **Le rayon fraîcheur déjà mergé** — 2 commits Scenarii `52ff57e4` (#1367) + `29ce5d03` (#1375), 73
   cellules : **6/80 PDF** = `{PokerCards, PokerCards_Print&Play_A4, PokerCards_Print&Play_Light_A4} ×
   {en, ru}` (MESURÉ — dossier [`134-fraicheur-post-65dd4742-2026-09-16.md`](134-fraicheur-post-65dd4742-2026-09-16.md)
   #1407 → `6f315f19` §3). Tout run posé sur une base postérieure à l'ancre `65dd4742` doit les reconstruire.
2. **La tranche apostrophes A/B/C/D** — décision owner pendante, feuille tranchable
   ([`994-arbitrage-apostrophes-ABCD-2026-09-15.md`](994-arbitrage-apostrophes-ABCD-2026-09-15.md)
   #1382 → `fbc98867`) : PDF touchés = D 0 · A 6 · B 16 · C 52 (RAPPORTÉ #994 §2, comptes rejoués sur
   `c089d526` par l'instrument `tools/994-apostrophe-dryrun.py`).

### Périmètre probable de la passe unique post-tranche (DÉRIVÉ — à rejouer à J-0)

| tranche | PDF apostrophes (#994) | ∪ rayon fraîcheur 6 | ≈ périmètre | langues du run |
|---|---|---|---|---|
| **D** statu quo | 0 | 0 ∪ 6 | **6** | en, ru |
| **A** FR seule | 6 (Poker ×3 fr + Tarot ×3 fr) | en/ru ⊆ déjà couverts | **12** | fr + en + ru |
| **B** A + inverse EN | 16 | + Poker ×3 ru | **19** | fr + en + ru |
| **C** 8 langues | 52 | en/ru dans le périmètre | **≈ 52** | 8/8 |

Plus, pour A/B/C : la re-dérivation `argumentum.owl` (102 cellules AIF → courbes en `rdfs:comment`) —
passe pipeline courte, **hors FreeMind** (RAPPORTÉ #994 §3 « mécanique post-choix ») — publication Pages
automatique au push master (prouvée #1391). ⚠️ Si la tranche atteint les corpus des mindmaps (Fallacies /
Virtues — en B : 9 cellules Fallacies + 2 Virtues), la re-dérivation des **SVG reste une fenêtre FreeMind
séparée, jamais fusionnée** dans celle-ci : le présent périmètre ne couvre que les PDF (+ l'OWL).

### Le fait qui pèse (re-pris de #1407 §4)

Toute tranche **≠ D ⇒ une passe unique post-tranche** : elle couvre fraîcheur + apostrophes en un run, un
manifeste, un verdict — la ciblée « 6 fraîcheur » immédiate ne ferait qu'ajouter une fenêtre partiellement
à refaire. **D ⇒ ciblée 6 possible à tout moment**, l'unique cas où « ciblé 6 vs 80 » se pose tel quel.

## §2 — Durée estimée

| composante | valeur | qualificatif |
|---|---|---|
| Run pipeline complet (80 PDF, harvests en cache, clobber ciblé, passe `--pdf-cmyk` incluse) | **≈ 73-80 min** | RAPPORTÉ (run 12/09 DoD — mémoire lane 13/09 ; corroboré par #1407 §4 « Option R : fenêtre ~80 min ») |
| Mesures C1→C6 du guide éd. 3 | **≈ 1 h 45** (05:19 → 07:05, 16/09) | MESURÉ (dossier [`134-dossier-validation-mesures.md`](134-dossier-validation-mesures.md) #1409 → `f1d3f1a0`, en-tête) |
| Verdict visuel ai-01 + reprises éventuelles | 0,5 – 2 h | SUPPOSÉ |
| **Planification totale recommandée** | **4 h** | DÉRIVÉ (13/09, mémoire lane : « planifier 4 h ») |
| Run ciblé (6-19 PDF) | **≤ ~1 h** | ESTIMÉ — jamais chronométré à l'unité : coût dominant = imagerie + assembly, les phases fixes du run sont conservées |

Chronométrage à la volée : horodatage début/fin depuis le `file_logger` (archivé à chaque run, #1179) —
chaque fenêtre alimente l'estimation suivante.

## §3 — Prérequis (état au 16/09, sauf mention J-0)

| # | prérequis | état |
|---|---|---|
| P1 | **Pas de fenêtre FreeMind** : `AssetConverterConfig.cs:59` — Mode défaut `WebBasedImageGeneration \| QuestPdfGeneration`, stage Mindmapper (`:580`) hors défaut ⇒ un run standard ne touche ni `.mm` ni SVG committés | VÉRIFIÉ (code master) |
| P2 | **Worktree dédié + jonction courte** (#1179) : run lancé depuis PowerShell/cmd dans `D:\A1xxx` → worktree `.regen-*-worktree` ; ⛔ jamais Git-Bash sur jonction (résolue au spawn → MAX_PATH) | ⚠️ jonction `D:\A1114` **pendante** ce jour (cible `.regen-final-worktree` recyclée — MESURÉ) — à recréer à J-0 |
| P3 | **IIS local UP** (`UseLocalCardpen=true`, `LocalCardpenUrl=http://argumentum.myia.io` — #629 : un flip casse la régén en 404 silencieux) | sonde à J-0 |
| P4 | **Clobber harvest ciblé par le rayon** : les PNG existants sont sautés (`ImageHelper.cs:119` File.Exists) — le clobber fait partie du run, pas une option (pattern runs 06/09 et 12/09) | liste dérivable du rayon à J-0 |
| P5 | **Disque** : run ≈ **11,7 Go** (RAPPORTÉ) ; `D:` libre = **269,3 Go** | VERT (MESURÉ ce jour) — marge ≈ 23× |
| P6 | Zéro geste webroot, zéro op main checkout, zéro écriture corpus hors worklist approuvée | durable (gardes #1049/#1244 actives) |

## §4 — DoD de sortie (ce que la fenêtre doit prouver avant clôture)

1. **Périmètre annoncé** : N/N au compteur + pagecounts au contrat C2 (379/262/334/105/21/38/6/1/15/9 —
   MESURÉ #1409 §C2).
2. **C3 ×5 verts** : `/DeviceCMYK` >0 · `/DeviceRGB` =0 · `/GTS_PDFX` ≥1 · `CGATS TR 001` ≥1 ·
   `GPL Ghostscript` ≥1 (la forme échappée `\(SWOP\)` ne discrimine rien sur ce périmètre — #1409 §Écarts).
3. **Log de passe** : `PDF CMYK post-process complete: N/N` + 0 erreur ; un couple produisant zéro image =
   échec du run (#1177), jamais un skip silencieux.
4. **Manifeste réédité** : provenance **par PDF** (base mixte assumée en mode ciblé : `65dd4742` + base
   neuve) + contrôle inverse sha256 non aveugle (C1 #1409).
5. **Identité inter-langues** : re-sondée sur les langues du périmètre (paires fr×N, instrument C4 #1409).
6. **Dos Scenarii intacts** : 7 empreintes, répartition 36·30·27·25·18·17·14 (C5 #1409) — la tranche ne
   touche aucun dos.
7. **Verdict visuel ai-01** : ciblé = pages du contrat + cartes changées ; complet = le PASS 12/09 **ne se
   transporte pas** sur une base neuve ⇒ déroulé C1→C6 intégral (≈ 1 h 45) + verdict.
8. **Fraîcheur du rayon** : `git diff --name-only ancre..base -- Cards/` = le rayon annoncé, rien d'autre.

## §5 — Créneaux types (proposition — aucune réservation)

| créneau | enchaînement | condition |
|---|---|---|
| **Soir** (début 18-19 h, fin ≤ 22 h) | run en soirée, mesures soir + matin, verdict ai-01 matin | ai-01 répond ≤ 12 h |
| **Journée** (08-12 h) | run + mesures + verdict dans la même fenêtre | ai-01 disposé dans la fenêtre (cycle cluster) |
| **Nuit** (début 22 h) | run pendant la nuit, mesures au matin | le plus sûr pour la disponibilité CPU/IO |

Exclusions : ⛔ jamais pendant la fenêtre **cutover DNS** (avant ~05/10 ou après renouvellement cert —
**échéance ferme 04/11** — dossier cutover #1411 → `503e64c8`) · ⛔ jamais fusionnée avec une **fenêtre
FreeMind** · po-2024 = autre machine (pas de collision locale) · la réservation se matérialise par une
**annonce dashboard** (créneau + périmètre + base) posée par l'owner/ai-01 — ce dossier n'en pose aucune.

## §6 — Ce que ce dossier n'établit pas

⛔ Aucune réservation exécutée · ⛔ aucun choix de tranche (A/B/C/D = jsboige) · ⛔ aucun run lancé, aucun
harvest clobberé · ⛔ aucun verdict visuel (rôle ai-01, jamais cette lane) · ⛔ les unions de périmètre du
§1 sont DÉRIVÉES de deux pièces MESURÉES — à rejouer à J-0 sur la base du run (worklist `--worklist A|B|C`
#994 + rayon #1407) · ⛔ le « 73-80 min » est RAPPORTÉ d'un run antérieur, pas re-mesuré · ⛔ pas la feuille
D1-D6 (grain ③) ni le fix #1123 (grain ④).

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain ② · base `origin/master` `e4a54638` · lecture seule :
git + code master + disque · verdict visuel : ai-01 · décisions et fenêtres : jsboige.*