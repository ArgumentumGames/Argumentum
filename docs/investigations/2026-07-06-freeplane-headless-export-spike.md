# Freeplane headless `c.export()` SVG export — feasibility spike (read-only synthesis)

**Date:** 2026-07-06 · **Author:** po-2024 · **Dispatch:** `5czj9v` [1] PRIMAIRE / issue #568
**Scope:** read-only feasibility analysis. **0 write `Cards/`, 0 régén, 0 re-probe of the stalled path.**
**DoD form delivered:** "SI bloqué (API `c.export()` insuffisante) → blocker documenté + design proposal."

---

## TL;DR

The headless Freeplane `c.export()` SVG path is **blocked at production scale** and carries an
**endorsed NÉGATIF adoption verdict** (ai-01 + jsboige WE 27/06, PR #599 closed). The goal of #568
— *retirer la dépendance RDP-foreground* — is **already achieved** by the OS-level fix **#569**
(`tscon` + persistent interactive session, MERGED 2026-06-21), which keeps the proven FreeMind+Batik
rendering and removes the foreground requirement without changing the engine.

**Recommendation: close #568.** The `c.export()` path is not the load-bearing route to the goal,
is refuted at scale, and no re-probe is justified (the stall is documented on real cards; a
toy-sample probe is not representative). This honors the worker's standing lesson
`never-coordinate-from-worker-machine` ("NEVER re-try #568 — réfuté, #599 closed").

This document was prepared in mode **(a)** of the worker's FLAG to the coordinator (msg `gqvma1`):
consolidate the known blocker + design proposal, **no re-probe**. It does not re-open a closed
dossier; it closes the dispatch's [1] ask by recording that the blocker is *already* documented and
the goal *already* met by another path.

---

## 1. What #568 asks, and what the dispatch requested

**#568 goal:** replace the keyboard-driven FreeMind SVG export (`SendKeys`, requires a foreground
desktop) with a programmatic Freeplane `c.export()` Groovy call that needs no foreground window —
removing the RDP-foreground dependency that breaks autonomous regen.

**Dispatch `5czj9v` [1] DoD:** `docs/investigations/2026-07-06-freeplane-headless-export-spike.md`
= feasibility analysis. *SI faisable → gated prototype + native-script sample. SI bloqué →
blocker documenté + design proposal. Un "bloqué documenté" est un livrable valide.*
**HARD:** path-swap verdict = ai-01; worker ships gated only; 0 write `Cards/`.

This document delivers the **"bloqué documenté"** form: the path is blocked, the blocker is already
known and endorsed-negative, and the goal is already met elsewhere.

---

## 2. History (what was tried, code=truth)

| Date | Artifact | Finding |
|------|----------|---------|
| 2026-06-21 | **#569 MERGED** | **OS-level fix shipped:** `tscon` + persistent interactive session keeps a real foreground desktop even with RDP disconnected. FreeMind+Batik rendering unchanged. **This removes the RDP-foreground dependency that motivated #568.** |
| 2026-06-21 | doc §3.2–3.3 | Freeplane `c.export()` de-risk: `freeplaneConsole.exe -N` (headless) → `Method not implemented` (no SVG render); FreeMind-body `.mm` **rejected** by Freeplane ("format inconnu"). 2 initial blockers. |
| 2026-06-22 | doc §3.6 (po-2024, Freeplane 1.13.2) | Re-probe **inconclusive** on 1.13.2 (session "Déco": no `.export_done` after 90 s; console logs empty). Raised an environmental factor: c.export() may need a **connected** session, not merely an existing one. |
| 2026-06-25 | **PR #599 §3.7** | **Breakthrough (toy-sample):** Freeplane 1.12.11 accepts the FreeMind body with `<map version="freeplane 1.12.1">`. The §3.3 blocker (format rejection) appeared refuted on a 4-node, 302-byte sample. |
| 2026-06-25 | **PR #599 §3.8 (po-2023)** | **Refutation at production scale:** on REAL pipeline cards (Virtues 161 KB, Fallacies 1.15 MB), the `-R` Groovy script **never executes** — Freeplane stalls at `requesting mode: MindMap` → silence → kill at 240 s. The 4-node toy-sample was **not representative**. |
| 2026-06-27 | **jsboige decision (WE 27/06)** | **Adoption verdict NÉGATIF.** "FreeMind + SendKeys (validé #569, OS reliability fix `tscon`) reste le seul chemin fiable." PR #599 **closed** (opt-in code not worth maintaining). |
| 2026-06-27 | ai-01 endorsement | "**Adoption production = NON.** §3.8 démontre un stall Freeplane sur les vrais corps FreeMind… la sérialisation native (§3.4 step 1) reste requise ; la version-seule est insuffisante. FreeMind + #569 (`tscon`) reste le SEUL chemin fiable." |

---

## 3. The blocker (§3.8, production-scale stall) — why c.export() is insufficient

On real pipeline cards, the headless export does not produce output and does not even run the
export script:

| Card | Size | `script_started` marker? | `export_done` / `failed`? |
|------|------|--------------------------|---------------------------|
| Virtues | 161 KB | ❌ NONE (240 s timeout) | ❌ none |
| Fallacies | 1.15 MB | ❌ NONE (identical pattern) | ❌ none |

Freeplane stalls at `requesting mode: MindMap`, then silence until kill. The leading (unconfirmed)
hypothesis is a **modal dialog at map-open** on complex FreeMind-body maps (conversion/upgrade/format
prompt) requiring a user click — which the toy 4-node sample never triggers. The version-only patch
(`freeplane 1.12.1`) is **insufficient**; the originally-planned **native Freeplane serialization**
(doc §3.4 step 1) remains the only thing that could plausibly unblock it — and that is multi-tick
work plus a visual-QA gate (Freeplane ≠ Batik fidelity).

**Conclusion:** the `c.export()` API is **insufficient at production scale** — exactly the dispatch's
"SI bloqué (API c.export() insuffisante)" branch. A blocker documented per DoD.

---

## 4. Design proposal — alternatives to c.export() for "retirer RDP-foreground"

The dispatch's HARD line is "verdict de swap du path = ai-01." The verdict was already given
(27/06: NÉGATIF). The design space for the *goal* (not the stalled mechanism):

| Option | Status / risk | Verdict |
|--------|---------------|---------|
| **(A) #569 OS-level fix — `tscon` + persistent session** | **MERGED 2026-06-21.** Keeps FreeMind+Batik (proven, high-fidelity). Removes the foreground requirement operationally. 0 engine change. | **✅ Recommended. Already meets the #568 goal.** |
| (B) c.export() + native Freeplane serialization (doc §3.4 step 1) | Multi-tick work; engine change → Freeplane ≠ Batik fidelity; mandatory visual QA (ai-01/jsboige). Need already covered by (A). | Not justified while (A) holds. |
| (C) c.export() + timeout/retry wrapper | Does not solve the stall (hypothesized modal at map-open, not a transient). Risks deadlock on large cards. | Rejected — addresses symptom not cause. |
| (D) XSLT fallback (`TryXsltSvgConversion`) | **Explicitly forbidden** by jsboige decision (PR #184 retired it; "Je ne pense pas que tu puisses faire aussi bien en XSLT que ce que FreeMind fait"). | Out of scope. Do not re-wire. |

**The goal of #568 is satisfied by (A).** The c.export() path is a means, not the end; since the
end is met, pursuing the stalled means has no marginal value.

---

## 5. Recommendation

1. **Close #568.** The stated goal — "supprimer la dépendance RDP-foreground" — is achieved by the
   shipped OS-level fix #569 (`tscon`, MERGED). The c.export() path it originally envisioned is
   blocked at production scale (§3.8 stall) and carries an endorsed NÉGATIF verdict (27/06, #599
   closed).
2. **Do not re-probe c.export()** without an explicit new jsboige/ai-01 mandate and a concrete
   hypothesis distinguishing the production stall from the toy-sample success. A bare re-run of
   the known-stalled path is the worker's documented anti-pattern.
3. **If a future need emerges** (e.g. #569's OS-level fix proves insufficient on a new target
   environment), the path forward is (B): native Freeplane serialization + visual QA — not a
   version-only patch (refuted) and not XSLT (forbidden).

---

## 6. Non-goals (respected)

- 0 write under `Cards/`. 0 production / rendering code change. 0 CSV/DB/OWL mutation. 0 régén.
- **0 re-probe of the stalled path** — this is a documentary synthesis of git history + PR #599 +
  the 2026-06-21 investigation, not a new Freeplane run.
- Read-only tools only: `gh pr view`, `gh issue view`, `git log/show`, `Read` of existing docs.

Relates: #568, #569, #599, dispatch `5czj9v` [1], `2026-06-21-mindmap-reliability-without-rdp.md`.
