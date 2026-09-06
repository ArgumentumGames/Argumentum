# Runbook — Régénération Release (8 langues) + passe CMYK

Recette opérationnelle consolidée à partir des runs validés : 20/08 (`d03fa9f3`), 02-03/09 (`ae84c91c`), 06/09 run E (`2a2e7b32`, deck 175). Chaque piège ci-dessous a coûté au moins un incident réel — l'index en fin de document fait foi.

## Périmètre et invariant

- Produit : 80 PDFs (8 langues × 10 documents) dans `Generation/Converters/Argumentum.AssetConverter/bin/Release/net9.0-windows/Target/{lang}/Documents/density-0/`, puis convertis CMYK **en place**.
- Sortie = `Target/` **dans l'arbre de build**, jamais `Cards/` au racine du dépôt.
- **Un seul converter à la fois** (lock global QuestPDF + contention CPU/IO).
- Le verdict visuel appartient à ai-01 (le worker signale comptes/hashes, jamais PASS).

## Phase 0 — Worktree, jonction, build

1. Worktree dédié par campagne (ex. `.regen-XXXX-worktree`), HEAD sur le commit de base du bundle.
2. **Jonction courte** vers le worktree (ex. `D:\A1292`) : `New-Item -ItemType Junction`.
3. Toutes les commandes pipeline depuis **PowerShell ou cmd, depuis la jonction** (piège 1).
4. Build préalable : `dotnet build -c Release` (évite le deadlock `dotnet run` inter-worktrees, piège 2 ; si hang : `dotnet build-server shutdown` puis `build` puis `run --no-build`).

## Phase A — Dépendances distantes

- `git fetch` + résolution templates : le WAN GitHub peut flaker (hang phase A du run E) — retry, ne pas relancer le run complet pour un fetch.
- `UseLocalCardpen = true` obligatoire (IIS local) — GitHub Pages ne sert pas `/Cards/` → 404 → 0 images silencieux (#629).

## Phase B — Harvest (Playwright/CardPen)

- Timeout 120 s par lot (90-120 s réels nécessaires).
- Les timeouts réseau (#613) se résorbent par **boucles de reprise** : run E = boucle ×5 pour 112/112 harvests. Un harvest partiel n'est pas un échec définitif.
- Harvest vide (`Images: {}`) = erreur JS CardPen ou `CsvType` manquant — voir la console Playwright.

## Phase C — Clobber (la phase qui tue les runs)

Avant toute régén sur un `Target/` déjà peuplé :

1. **Clobber des `.harvest.json`** concernés (re-harvest forcé).
2. **Clobber des PNG** correspondants dans `Target/{lang}/Images/density-0/<CardSet>/` — le clobber harvest SEUL ne suffit pas : `ImageHelper.cs:119` court-circuite par `File.Exists` et saute les PNG existants (piège 3, run E : 2 662 PNG Scenarii réécrits après interception).
3. Ne JAMAIS `dotnet clean` en croyant vider : `Target/` vit **dans** `bin/` — un clean détruit 25 Go de bundles.

## Phase D — Génération PDF

- Lancer depuis la jonction, PowerShell, `-c Release`.
- Durcissement #1179 : un couple document×langue à zéro images **fait échouer le run** (avant : skip silencieux #1177). Vérifier le log, pas seulement l'exit code.
- Rotation de log #1179 : le run courant écrit un `file_logger.log` frais, le précédent est archivé `file_logger-<ts>.log`. **Toujours vérifier que la bannière du log appartient au run courant.**

## Phase E — Contrôles contractuels (chiffres deck 175)

| Contrat | Valeur attendue |
|---|---|
| TarotCards_{lang} | **379 pages × 8/8 langues** |
| Total PDFs frais | 80 (`Documents/density-0/`, horodatage du run) |
| Fallacies faces | 175 (PK 96 sorti, #1292) |
| Deck tarot | 191 cartes (175+15+1) ; boîte 358 sans Virtues / 489 avec |
| Géométrie tarot | 70×120 mm (#1267) → 413×708 px @150 dpi |
| Géométrie poker | 63,5×88,9 mm → 375×525 px @150 dpi |
| Parité recto-verso | pages **paires = faces, impaires = dos** (vérifié produit) |

## Phase F — Passe CMYK détachée

```powershell
$exe = "<jonction>\Generation\Converters\Argumentum.AssetConverter\bin\Release\net9.0-windows\Argumentum.AssetConverter.exe"
$p = Start-Process -FilePath $exe -ArgumentList "--pdf-cmyk" -WorkingDirectory "<jonction>" `
    -WindowStyle Hidden -RedirectStandardOutput <out.log> -RedirectStandardError <err.log> -PassThru
```

- **`--pdf-cmyk` en args[0]** (les entrées args[0] sont immunisées contre le crash `Console.ReadKey` headless, piège 4).
- **Détaché obligatoire** (`Start-Process` sur l'exe, pas `dotnet run`) : une passe attachée à une session meurt avec elle — exit 1073807364 (0x40010004, kill) / -1073741502 (0xC0000142, DLL init failed) = signature de cascade de mort de session parente (piège 5). Re-convertir des PDF déjà convertis est sûr (idempotent, en place).
- Ghostscript (`gswin64c`) doit être sur `PATH`, sinon **chaque PDF est skippé avec un simple warning** — panne silencieuse RGB.

**Contrôle = le LOG, jamais l'exit code** :

1. Bannière `PDF CMYK post-process: N PDF(s) to convert` (N=80) — confirme que le log est celui du run courant.
2. Compter `[Success]` → **80/80**.
3. `FAILED` = 0, `Skipping` = 0.
4. Poids : attendu **+~50 %** vs RGB (mesuré +51 %). ⚠️ conversion en place = pas de baseline RGB sur disque après coup ; capturer les tailles avant la passe si un diff exact est requis.
5. Durée typique : ~20 s/PDF (40 s+ gros decks) → 80 PDFs ≈ 30-50 min.

## Phase G — Rendus QA + signal

- Rasteriser **AVANT la passe CMYK** (les PDFs sont mutés en place — collision lecture/écriture sinon) :
  `gswin64c -dSAFER --permit-file-read=<pdf> -sDEVICE=png16m -r150 -dFirstPage=N -dLastPage=N -o out.png in.pdf` (page par page, pas de `%d` — piège 6).
- Échantillon minimal : TarotCards fr 1 face (paire) + 1 dos (impaire), PokerCards fr 1 face, Virtues 1 face. PNG ~150 dpi, pas de PDF (4 Go ne passent pas en pièces jointes).
- Signaler provenance complète : commit de base, horodatage PDFs, RGB/CMYK, dpi, géométries.

## Index des pièges

| # | Piège | Symptôme | Réf |
|---|-------|----------|-----|
| 1 | Lancement depuis Git-Bash via jonction | Jonction résolue au spawn → chemin +30 chars → `MagickCoderErrorException: WriteBlob Failed` + garde silencieuse | #1177, #1121 |
| 2 | `dotnet run` cross-worktrees | Hang silencieux build-server | build-server shutdown + `--no-build` |
| 3 | Clobber harvest sans clobber PNG | PNG périmés réutilisés (`File.Exists`, `ImageHelper.cs:119`) | run E 06/09 |
| 4 | Entrée sans args[0] en headless | Crash `Console.ReadKey` | `--pdf-cmyk` / `--generate-owl` en args[0] |
| 5 | Passe CMYK attachée à la session | Mort en cascade (exit 0x40010004 / 0xC0000142) | run 2 06/09 |
| 6 | `-o out%d.png` Ghostscript | `%d` non interpolé | page par page `-dFirstPage/-dLastPage` |
| 7 | GS absent du PATH | Skip silencieux, bundle RGB livré comme CMYK | contrôle logSkipping=0 |
| 8 | `dotnet clean` pour « vider » | Détruit les bundles (Target/ dans bin/) | triage worktrees |
| 9 | GitHub Pages comme source CardPen | 404 → 0 images → 0 PDF | #629, `UseLocalCardpen=true` |
| 10 | JSON de config édité à la main | Source de vérité = C# (`SkipConfigFile=true` délibéré) | CLAUDE.md |

---
*Dernière validation : run E 06/09/2026 @ `2a2e7b32` (deck 175 + #1295) — 379 pages ×8, 80 PDFs, témoins vérifiés.*
