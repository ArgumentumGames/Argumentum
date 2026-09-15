# AIF structural audit — Layer (c) §5 reference-repair worklist (#847)

> **Provenance.** The actionable half of the §5 backlog in the
> [`847-synthesis-verdict.md`](847-synthesis-verdict.md) (ai-01): *“134 dead
> references (5.2 %) — an upper bound (HEAD-probe over-counts; rationalwiki.org
> returns 503 to HEAD but 200 to GET). Re-probe with GET before repairing, or the
> worklist will contain phantoms.”* Computed by **po-2024** (worker, Cards/AIF
> lane). **MEASURE, not verdict. INPUT for ai-01 synthesis update. GATED jsboige
> ratification. 0 prod-CSV write** (read-only GET probes + read-only CSV inputs).
> Tracking: #847 §5 · chantier #498.

---

## 0. Headline — the upper bound does **not** refine downward under GET

The verdict's standing caveat (`847-layerc-reference-reachability.md` §6.1) held
that the 134 HEAD-`dead` count was an upper bound because `rationalwiki.org`
returns 503 to HEAD but was *assumed* 200 to GET. **This assumption is refuted
firsthand by a three-environment GET re-probe:**

| environment | method | rationalwiki result |
|-------------|--------|---------------------|
| this machine | `curl -X GET`, UA `ArgumentumAIF-Audit/1.0` (bot) | **503** |
| this machine | `curl -X GET`, UA Chrome 126 realistic + Accept headers | **503** |
| Anthropic server | `WebFetch` (server-side fetch, different IP/stack) | **503** |

Sanity anchors pass in the same run: `en.wikipedia.org/wiki/Fallacy` → 200,
`example.com` → 200. The 503 is **UA-invariant and environment-invariant** — it
is not a HEAD-only artefact, and it is not local to this machine's egress.

**Consequence for §5:** the GET re-probe does **not** subtract phantoms from the
134. Of the 134 HEAD-`dead` refs, **133 return a hard error on GET too** (120 ×
503, 12 × 404, 1 × 403) and **1 times out** (connection refused). The repair
backlog is therefore **real, ~134 refs, not an over-count**. The line in the
synthesis verdict *“Re-probe with GET before repairing, or the worklist will
contain phantoms”* is **satisfied** (there are no phantoms to remove), and the
line *“rationalwiki.org returns 503 to HEAD but 200 to GET”* is **factually
incorrect and should be struck** when ai-01 next touches the verdict.

> **Honesty caveat (load-bearing, not decorative).** A 503 from a Cloudflare /
> JS-challenged host is **not proof the page is gone** — a human in a real
> browser (JS, cookies, TLS fingerprint) may see the article alive. What the
> three-source 503 **does** prove is that the reference is **mechanically
> unreachable**: any link-following consumer (PDF reader, scraper, audit
> instrument) that hits it programmatically gets a hard error. For the taxonomy's
> reference purpose that is functionally `dead`. Whether a human eye still sees
> content is a separate, human question — flagged §6 — and does **not** change
> the mechanical verdict here.

---

## 1. Method (code=truth, read-only)

- **Dead-pool source:** the `dead`-bucket rows of
  [`847-layerc-reference-reachability-refs.csv`](847-layerc-reference-reachability-refs.csv)
  (134 rows, the HEAD-probe artefact from the original layer-(c) measure).
- **Re-probe:** `curl -X GET -L --max-redirs 5 --max-time 8`, polite UA, final
  HTTP code captured via `-w %{http_code}`. Classification: `2xx` → alive ·
  `3xx` → redirect (followed) · `4xx/5xx` → dead · timeout/error → unknown.
- **Node join:** `PK` → `(Famille, Sous-Famille, Soussousfamille)` from the prod
  CSV `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (read-only) to
  localise dead refs per sub-family — `Argument bâclé` is the priority target
  because a grouping arbitration is blocked behind it (verdict §3.3).
- **0 mutation:** GET is read-only; no file, no CSV, no registry is written. The
  worklist artefact is this doc + a companion CSV in the scratchpad (not the
  repo; reproducible from the inputs above).

## 2. Results — 134 HEAD-dead, by GET code

| GET code | n | reading |
|----------|---|---------|
| **503** | **120** | `rationalwiki.org` — mechanically unreachable, UA-invariant (see §0) |
| **404** | **12** | confirmed dead — page genuinely gone |
| **403** | 1 | `fastcompany.com` — forbidden (anti-bot or paywall) → effectively unreachable |
| 0 (timeout/conn-refused) | 1 | `ikipedia.qwika.com` — host dead (qwika.com service defunct) |
| **total** | **134** | (was 134 HEAD-dead; **0 phantoms removed**) |

### 2.1 The 12 confirmed-dead 404s (real, repair-ready)

| host | n | sub-families hit |
|------|---|------------------|
| climatechangecommunication.org | 5 | Préjugé (3) · Arranger les faits · Changement de cap |
| mgtow-france.fr | 2 | Surinterprétation · Changement de cap |
| autopsia.fr | 1 | **Argument bâclé** |
| cosmovisions.com | 1 | Procédé rhétorique |
| cs.oswego.edu | 1 | Arranger les faits |
| eyrolles.com | 1 | Résultat invalide |
| scu.edu | 1 | Manipulation mentale |

These are unambiguous: the server resolved and returned 404 Not Found. The page
is gone. Repair = repoint to a stable anchor (Wikipedia where one exists,
`archive.org` snapshot otherwise). The 39 already-`archive-repointed` refs in
the taxonomy establish the pattern.

### 2.2 The 120 `rationalwiki.org` 503s — the decision point

All 120 return 503 across three independent environments (§0). They are
**mechanically dead for any programmatic link-follow**. Two defensible
dispositions, neither of which a worker can settle:

- **(R1) Trust the mechanical verdict → repair preventively.** Repoint each
  `rationalwiki.org/wiki/<X>` to the equivalent Wikipedia article (most
  rationalwiki entries are fallacy-named and have a direct `en.wikipedia.org`
  equivalent, often already present in another `link_*` column for the same
  leaf). This converts 120 dead→stable in one pass and **unblocks §3.3
  `Argument bâclé`** without waiting on a human eye.
- **(R2) Defer to a human eye → spot-check before bulk repair.** Have a human
  (ai-01 / jsboige) open ~5 rationalwiki URLs in a real browser; if they load,
  rationalwiki is a Cloudflare soft-block (alive for humans, dead for machines)
  and the refs may be kept with an annotation rather than repointed.

This is **jsboige / ai-01's call**, not a worker's. The measure here only
supplies the evidence: 120 refs, 3-source 503, concentrated in 6 sub-families
(see §3).

### 2.3 The 2 edge cases (403 / timeout)

- `fastcompany.com/3032675/…` (pk 317, *Appel à l'émotion*) → 403. Likely
  anti-bot/paywall. Repair via `archive.org` snapshot.
- `wikipedia.qwika.com/en2fr/Ecological_fallacy` (pk 626, *Généralisation
  abusive*) → connection refused. qwika.com is a defunct translation mirror.
  The page exists verbatim at `en.wikipedia.org/wiki/Ecological_fallacy` —
  trivial repair.

## 3. Where the dead refs land — sub-family concentration

| sub-family (d2) | dead refs | of which rationalwiki 503 |
|-----------------|----------:|--------------------------:|
| Argument bâclé | 18 | 17 |
| Préjugé | 17 | 14 |
| Changement de cap | 15 | 15 |
| Ad hominem | 15 | 15 |
| Appel à l'émotion | 12 | 11 |
| Généralisation abusive | 7 | 6 |
| Surinterprétation | 6 | 4 |
| Procédé rhétorique | 5 | 4 |
| Manipulation mentale | 5 | 4 |
| Arranger les faits | 5 | 3 |

The dead pool is **not uniformly distributed** — it concentrates where the
taxonomy leaned on `rationalwiki.org` as a secondary anchor (d2 sub-families).
`Argument bâclé`, `Préjugé`, `Changement de cap`, `Ad hominem`, `Appel à
l'émotion` carry **77 of the 134** dead refs (~57 %). These are exactly the
evidentially-exposed nodes the verdict flagged (§3.3, §4 of the synthesis).

## 4. Impact on §3.3 — `Argument bâclé` is **rationalwiki-blocked**

The verdict held `Argument bâclé` as *evidence-blocked* (46 % wiki · 24 % dict ·
22 % long-tail) and routed it to this backlog before arbitration. The re-probe
**localises the block precisely**: of its 18 dead refs, **17 are
`rationalwiki.org`** (503, mechanically unreachable) and **1 is `autopsia.fr`**
(404, genuinely gone).

- The `autopsia.fr` ref is a clean repair (404 → archive.org/Wikipedia), doable
  immediately.
- The 17 `rationalwiki.org` refs are the actual block. Their disposition **is
  the rationalwiki decision (R1 vs R2, §2.2)** — there is no `Argument bâclé`
  repair that does not pass through it.

**Therefore the `Argument bâclé` arbitration remains evidence-blocked until R1
or R2 is chosen.** This is consistent with the verdict (it predicted a block);
the refinement is *why* and *where*: not a diffuse long-tail decay, but a single
concentrated host decision.

## 5. Recommended repair sequencing (proposal, gated jsboige)

1. **Class A (12 × 404 + 1 qwika + 1 fastcompany = 14 refs)** — **uncontroversial,
   execute now** (on a green light). 404/timeout/403 on non-rationalwiki hosts;
   repair = `archive.org` snapshot or Wikipedia equivalent. Independent of the
   rationalwiki decision. Removes 14 dead, no judgement call.
2. **Class B (120 × rationalwiki 503)** — **await R1/R2 (§2.2)**. jsboige / ai-01
   to pick: bulk-repoint to Wikipedia (R1, recommended — converts 120 dead→stable,
   unblocks `Argument bâclé`) or human spot-check first (R2). Either way, **0
   prod write until ratified**.
3. **Re-measure after repair.** Once Class A (and optionally B) land, re-run the
   layer-(c) probe (`847-layerc-reference-reachability` instrument) on the
   repaired taxonomy. The `Argument bâclé` long-tail share should drop; if its
   evidential profile crosses the Wikipedia-anchor threshold, the §3.3
   arbitration unblocks.

## 6. What this measure cannot settle (human lane)

- **Is rationalwiki alive for a human browser?** Three machine environments say
  503. A human opening the URL in Chrome/JS is the only thing that can confirm
  "Cloudflare soft-block, content fine" vs "service-degraded for everyone". This
  is a 2-minute spot-check for ai-01 or jsboige — not a worker task.
- **Does a repaired anchor still say what the citation claimed?** Mechanical
  reachability ≠ evidential validity (the standing caveat in
  `847-layerc-reference-reachability.md` §6.3). A `200` from a repurposed /
  domain-squatted page still reads `alive`. Validating *content* alignment is
  human work, out of scope for any mechanical measure.

## 7. Governance

- **MEASURE, not verdict.** No restructuring, no CSV edit, no node move. This
  doc refines §5 of the synthesis verdict (corrects one factual line about
  rationalwiki + supplies the repair worklist) and is **input for ai-01's next
  verdict touch**, not a self-applied edit to `847-synthesis-verdict.md`.
- **0 prod-CSV write** (post-T&A #802 freeze). The 14 Class-A repairs are
  *proposed*, not applied; application is gated jsboige and post-tag.
- **Read-only inputs only.** GET probes + CSV reads; no mutation anywhere.
- The rationalwiki 503 verdict is **triangulated across 3 environments** before
  being stated — no single-source claim.

## Refs

- Verdict: [`847-synthesis-verdict.md`](847-synthesis-verdict.md) §3.3 (`Argument
  bâclé` evidence-blocked), §5 (this backlog).
- Source measure: [`847-layerc-reference-reachability.md`](847-layerc-reference-reachability.md)
  §6.1 (the HEAD-over-count caveat this re-probe refutes), §3 (headline buckets).
- Artefact: [`847-layerc-reference-reachability-refs.csv`](847-layerc-reference-reachability-refs.csv)
  (the 134-`dead` pool, re-probed here).
- Method: [`aif-structural-audit-method.md`](aif-structural-audit-method.md) §2.2, §4, §6.
- Tracking: #847 · chantier #498 · repair-pattern precedent: 39 existing
  `archive-repointed` refs.
