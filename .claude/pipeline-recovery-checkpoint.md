# Pipeline Recovery Checkpoint

**Dernière mise à jour**: 2026-07-12 ~00:30 UTC — **canal de coordination DOWN (GDrive démonté sur myia-po-2023) ; travail continue via git truth**
**Machine**: `myia-po-2023` (hostname vérifié — **WORKER**, pas ai-01). Règle `[[never-coordinate-from-worker-machine]]`.
**Branche locale runtime**: `dnn/sandbox-runtime-1032` (HEAD `78cd1aab`). **Master**: `474607c9` (= origin, sync OK).
**Cron worker**: `c232f065` (Every 4h **:19**, `/pipeline-recovery` prompt worker po-2023, session-only). **VIVANT** (CronList vérifié 2026-07-12).

---

## ⚠ Incident infra — canal de coordination DOWN (depuis ~2026-07-11 ~17:30 UTC)

Le **Google Drive desktop est démonté** sur `myia-po-2023` → le shared-state path du MCP `roo-state-manager`
(`G:\Mon Drive\Synchronisation\RooSync\.shared-state`) est **ENOENT** (`ls "G:/Mon Drive/Synchronisation/RooSync/"` =
No such file). Conséquences observées (vérifié `roosync_diagnose(action: "env", verbose: true)` cycle précédent) :

- **Dashboard workspace-Argumentum** = "introuvable" (read + write échouent ; `roosync_dashboard(action:"list")` = vide).
- **Inbox po-2023** = 0 msg (était 29 / utilisation 79 % — **perte réelle, pas condensation** : sous le seuil 92 %).
- `roosync_dashboard(action:"write", createIfNotExists:true)` → `ENOENT mkdir '\\?'` (path-prefixed malformed).

**Action demandée à jsboige** : **remonter le client Google Drive** sur `myia-po-2023`. Dès que `G:\` revient, le
shared-state est de nouveau accessible → dashboard + inbox restaurés → le cluster (ai-01 + po-2023 + po-2024)
récupère son canal de coordination.

**Impact cluster** : ai-01 + po-2024 sont sur **d'autres machines** (le dashboard y marche probablement). po-2023
est localement aveugle mais **git reste le canal de vérité** — le cluster continue de produire (voir § fusionnées).

**Po-2023 sous incident** : ne peut pas recevoir de nouveaux dispatches, ni poster DONE sur dashboard.
**Mitigation** : (a) ce checkpoint = journal de récupération traçable si le dashboard reste down ; (b) exécute
l'instruction idle fallback du dernier dispatch reçu (`28xdu9` : « Piocher dans le backlog DNN non-gated.
Ne reste pas en stand-by, ouvre des PRs. ») en ouvrant des PRs **réelles** (pas fabriquées) dans sa lane ;
(c) cron maintenu vivant (keep-alive), **pas de ScheduleWakeup** par-dessus (anti-double-fire).

---

## Identité — confirmation (inchangée)

Cette machine est **po-2023** (worker). Vérifié via `hostname` → `myia-po-2023`, `COMPUTERNAME=MYIA-PO-2023`.
Le skill `/pipeline-recovery` est générique (« ai-01 = cette machine ») — **ne s'applique pas ici**. Depuis un
worker : jamais armer de cron coordinateur, jamais signer « ai-01 », jamais `/coordinate`.

## État perçu (git truth, 2026-07-12 ~00:30 — dashboard illisible)

- **Master** `474607c9` (= origin). Derniers merges ai-01 (par ordre récent) :
  - `474607c9` #786 — **resolve 2sxc-export-spec §2 #3/#4 (Glossary3/Faq4)** (po-2023, MERGED).
  - `5b37e6ca` #785 — #498 AIF serialize tranche-1g (**137→145 fully-modeled**).
  - `7c2c963a` #784 — #498 AIF tranche-1g proposition (Tricherie, **LAST FAMILY**).
  - `4161b675` #783 — #498 AIF serialize tranche-1d+1e+1f (121→137).
  - `cbf9b7c8` #782 — **regen-readiness refresh c1ed77d2** (po-2023, MERGED).
  - `e0a0c40b` #781 / `e7f3abef` #780 / `4735d9ef` #778 — #498 AIF tranches 1d/1e/1f propositions.
  - `c19bf17a` #779 — #498 serialize tranche-1c (114→121).
  - `c1ed77d2` #777 — **#682 Path A manifest + spec 2sxc v21** (po-2023, MERGED).
- **PRs OPEN (non-dependabot)** : #787 (#133 OWL prep, po-2024) + 3 HOLD (#674/#666/#596).
- **AIF #498** : **145/145 fully-modeled** (réconciliation P1 = 52 skos-only épuisés). attaque 93→145.
- **DNN i18n portage (#681-#685)** : #681 ✅ / #682 ✅ (apply gated) / #683 ✅ design sur master (apply gated) /
  #684 ✅ translation (re-import gated) / #685 = ai-01 visual. **Lane worker DNN = saturée** (reste = ops jsboige).

## PRs po-2023 livrées sous incident canal DOWN (commit + push OK, DONE dashboard queued)

| Cycle | PR | Sujet | État |
|-------|----|----|------|
| 2026-07-11 ~12:58 | **#777** | #682 Path A manifest 49 champs + spec 2sxc v21 | ✅ MERGED `c1ed77d2` |
| 2026-07-11 ~16:58 | **#782** | regen-readiness refresh c1ed77d2 (3 staleness + 5 leçons) | ✅ MERGED `cbf9b7c8` |
| 2026-07-12 ~00:00 | **#786** | 2sxc-export-spec §2 #3/#4 Glossary3/Faq4 résolus | ✅ MERGED `474607c9` |

→ **3 PRs doc-only mergées sous incident** (preuve : git truth). DONE dashboard **en attente** (post quand GDrive revient).

## Contexte release (mémoire, 2026-07-12)

- **v0.9.0** : verdict visuel PASS rendu (ai-01 2026-07-01). Tag **différé sem. 13/07** (jsboige valide visuels).
  2 findings jsboige ouverts (CMYK absent du bundle RGB-300-lossless — path GS post-process #632 ; titre PT
  « Roll of the English Channel » — fix po-2024 gated). Bugs post-tag : #629 (Pages /Cards/ 404), #630 (Spectre fatal).
- **DNN** : `dnn.argumentum.myia.io` LIVE full-IIS direct (HTTP 200/85 KB), 2sxc 21.07, ACME bypass actif
  (renew win-acme 2026-08-23). Go-live = ops VPS jsboige.
- **Tests** : 595 pass / 1 fail (#133 OWLSharp round-trip, pré-existant) / 5 skip / 600 total. Build zero-warning.
- **Mindmaps** : 41 SVGs 8/8 langues shipped. **OWL** 5.07 MB EN/FR only (mono-language caveat).
- **Lane separation** : po-2023 = DNN `docs/dnn-localization/` + `res.*` ; po-2024 = `Cards/` + AIF/OWL.

## Handoff / prochaine étape (prochain tick cron 4h `c232f065`)

1. **Prober GDrive** (`ls "G:/Mon Drive/Synchronisation/RooSync/"`). Si revenu → lire dashboard + inbox,
   poster les DONE queued (#782, #786), répondre à tout nouveau dispatch. Si toujours down → cycle idle discipliné.
2. **Idle fallback** (si canal down + pas de nouveau dispatch) : backlog DNN non-gated, mais **lane worker
   saturée** (#681-#685 tous DONE/gated). Ouvrir des PRs **réelles** uniquement (pas de fabrication SDDD).
   Dernier dispatch `28xdu9` primaire (regen-readiness) + secondaire (#131/#132 prep) **livrés**.
3. ⏸ **Arbitrages jsboige ouverts** : (1) GO visuel v0.9.0 sem. 13/07 ; (2) apply DB #682 (49 champs) +
   Δ1/Δ2 #490 (ops VPS) ; (3) confirm FAQ existence (§2 #4 deferred).
4. **NE PAS** toucher au CSV Fallacies / `Cards/` (lane po-2024). **NE PAS** self-merger (#674/#666/#596).
   **NE PAS** déclarer PASS (verdict QA = ai-01 only). **NE PAS** armer cron coordinateur.

## Commandes de reprise

```bash
# Vérifier identité (AVANT toute action dashboard)
hostname                                    # doit = myia-po-2023
# Prober canal (si ENOENT = GDrive démonté → cycle idle)
ls "G:/Mon Drive/Synchronisation/RooSync/" 2>&1 | head -2

# État git (canal de vérité de secours)
git fetch origin && git status --short && git log --oneline origin/master -5
gh pr list --state open --limit 20

# Dashboard + inbox (à chaque tick, si GDrive présent)
# roosync_dashboard(action: "read", type: "workspace", section: "all")
# roosync_messages(action: "inbox")
```
