# Pipeline Recovery Checkpoint

**Dernière mise à jour**: 2026-08-27 ~05:00 — **#1181 exécuté jusqu'au commit (worktree `.fix-1181-worktree`, branche `fix/1181-mindmap-crosslinks`)**
**Machine**: `myia-po-2023` (hostname vérifié — **WORKER**, pas ai-01). Règle `[[never-coordinate-from-worker-machine]]`.
**Master**: `1c5231f6` (= origin, sync OK au lancement du worktree).

---

## #1181 — liens transverses visibles sur les mindmaps (état final)

**Dispatch ai-01** : aligner l'enum `CrossLink` sur les 8 verbes CSV, résoudre les cibles (chemins taxonomiques, `;`-séparés), assigner `CrossLinks`, régénérer les 41 SVG, organe RED→GREEN même commit.

### Réalisé (branche `fix/1181-mindmap-crosslinks`, worktree `.fix-1181-worktree`)

- Moteur : `CrossLink.cs` (enum 8 verbes + All, fail-loud préservé), `CrossLinkResolver.cs` (nouveau, sémantique = émetteur OWL #1182 : split `;`, trim, drop dangling, drop self, unidirectionnel), `FallacyMindMapDocumentConfig.cs` (branche morte retirée, palette 8 couleurs — **Denounces ≠ noir** : `stroke="black"` par défaut Batik = incommable, mesuré), `FallacyMindMapCreatorConfig.cs` (`CrossLinks = CrossLink.All` × 2 configs), `VirtueMindMapDocumentConfig.cs` (cleanup branche morte).
- **Régén validée (run4)** : 25/25 cartes Fallacies = **exactement 1230 flèches** (13 predatesOn, 2 denounces, 402 leverages, 66 allows, 25 opposes, 41 inverts, 360 mirrors, 321 isRelatedTo — compte dérivé du CSV, croisé avec #1182). 16 SVG Virtues NON recopiés (zéro churn). Signature de comptage : `stroke="rgb(...)"` par couleur de verbe — **Batik n'émet jamais `marker-end`** (tout est aplatí en paths génériques) ; la palette est la seule signature stable.
- Organe : `CrossLinkArrowCountTests` — witness RED capturé (`Fallacies_fr.svg: expected 1230, found 0`) puis GREEN 25/25 après recopie. `CrossLinkResolverTests` 5/5.
- Suite complète : voir rapport final (attendu ~889 pass / 0 fail / 5 skip).

### Pièges rencontrés et solutions (pour le prochain run mindmap)

1. **RDP input-idle** (foreground NULL session-wide) : tous les exports SendKeys échouent silencieusement. Diagnostic : probe `GetForegroundWindow()` direct ; WinSta0\Desktop corrects ; `tscon 2 /dest:console` refusé sans élévation. **Fix : reconnexion RDP interactive** (le watcher auto-lanceur a déclenché le run à la restauration).
2. **OOM FreeMind sur cards_fr** : `freemind.socket` + `~/.freemind/log.0` → `java.lang.OutOfMemoryError: Java heap space` au décodage des thumbnails. **Cause racine : le cycle du 24/08 a régénéré `FallaciesWebThumbnails` en 590×590 (22,4 MB/176)** ; le mindmap cards les référence par chemin (affichage 60×60) et FreeMind décode les entiers (~250 MB heap, JVM 32-bit). Le 20/08 les thumbs étaient petits → #1120 avait réussi. **Contournement : downscale 120×120 des 176 PNG du Target Debug avant le run** (script scratchpad). **La mine est toujours là** — le CardSet produit encore du print-scale → issue à ouvrir.
3. FreeMind.exe = launcher → la fenêtre appartient au **javaw enfant** (poll par titre, pas par le Process du launcher).
4. Override `Mode = Mindmapper` : build puis **revert byte-exact immédiat** (git diff vide vérifié).

### Session state

- PR #1186 (#1179 fail-loud) : poussé, CI 3/3 vert, **review/merge = ai-01** (ACK reçu).
- #1180 : É5 requalifiée publiée (comment `5427550728`) avec réserve paiement + lien #1188. Triage GitHub classe M/F = worker sous bannière po-2023 (politique #1191).
- #133 : porte #1189 (PI) notée — préparer SANS publier.
- ⛔ Rappels : jamais de push direct master ; verdicts visuels = ai-01 ; commit+PR AVANT post DONE.

---

## Identité — confirmation (inchangée)

Cette machine est **po-2023** (worker). Depuis un worker : jamais armer de cron coordinateur, jamais signer « ai-01 », jamais `/coordinate`.
