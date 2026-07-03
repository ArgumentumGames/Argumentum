# DNN idle-hang — Option C (SQL connection-string tuning) prep

**Scope**: Prep-only analysis for the **intermittent idle-hang** observed on the prod DNN site
(`https://dnn.argumentum.myia.io`). Documents the **Option C** mitigation values + rationale, for
**jsboige's arbitration** (prod `web.config` change = ops/VPS lane). **Not applied.**

**Context**: ai-01 dispatch `8b3ymj` idle task. Option A (IIS PeriodicRestart ~4h) was GO'd to
the IIS lane in the prod-rules audit ([2026-07-03-dnn-prod-rules-coverage.md](../investigations/2026-07-03-dnn-prod-rules-coverage.md)
§6). Option C is complementary — it hardens the SQL connection **pool** within the recycle
window.

---

## 1. Symptom (observed during audit #662)

First probes against the site after an idle period → **HTTP 000 (connect timeout / no response)**,
then the site **recovers to 200** (~9.5s HTTPS, ~0.5s HTTP) once re-warmed. The hang is
**intermittent and idle-correlated**, not load-correlated. Classic "first request after idle is
slow/frozen" signature.

## 2. Root-cause hypothesis

**SQL connection-pool staleness post-idle** (most likely; needs VPS-side confirmation):

- DNN (ASP.NET / .NET Framework 4.8) uses ADO.NET SqlClient connection pooling. The pool keeps
  connections open across requests.
- After an idle period, the underlying SQL connection(s) can go stale — the SQL Server service,
  a network NAT/firewall idle timeout, or a server-side reclamation drops or invalidates the
  idle socket while the client pool still believes it's alive.
- The next request grabs a **dead connection** from the pool; the `Open`/first-query blocks
  until the TCP/SqlClient timeout, producing the HTTP 000 / multi-second stall. After the pool
  recycles the dead entry, subsequent requests succeed (the "recovers to 200" behavior).

This is consistent with idle-correlation (not load), intermittent occurrence, and post-idle
onset. **Confirm on the VPS** via SQL Server errorlog + Windows event log around an idle window.

## 3. The two complementary options

| Option | What | Cost | Status |
|--------|------|------|--------|
| **A — IIS PeriodicRestart ~4h** | Recycle the app pool on a schedule → tears down + rebuilds the connection pool wholesale, clearing stale entries. | Low, reversible, IIS-only | ✅ **GO'd** to IIS lane (audit #662) |
| **C — connection-string tuning** | Harden the pool itself so stale connections are shed/forged proactively within the recycle window. | Low, reversible, `web.config` one-liner | 🔴 **Deferred to jsboige** (this doc) |

They compose: Option A bounds the problem at 4h; Option C reduces the within-window failures.
Either alone helps; together they cover both the "stale after long idle" and "stale mid-window" cases.

## 4. Option C — recommended values

Apply to the **`SiteSqlServer`** connection string in **prod `web.config`** (on the VPS — the
repo copy is a placeholder, `Data Source=REPLACE`; keys are excluded from the repo):

```
Min Pool Size=5; Connect Timeout=30; Connection Lifetime=300; Load Balance Timeout=30
```

| Parameter | Default | Recommended | Rationale |
|-----------|---------|-------------|-----------|
| `Min Pool Size` | 0 | **5** | Pre-create 5 connections → first request after idle doesn't pay the cold-connect cost. Also issues a gentle keep-alive load that helps NAT/firewall idle timers stay warm. |
| `Connect Timeout` | 15s | **30** | Explicit; gives a slow SQL server more room **and** fails fast with a clear error instead of an indefinite hang. Bounds the worst-case stall. |
| `Connection Lifetime` | 0 (never) | **300** (5 min) | When a connection is returned to the pool, if its age > 300s it is destroyed. Continuously **sheds stale connections**, so a dead socket can't linger long enough to be handed to a request. The single most relevant knob for the observed symptom. |
| `Load Balance Timeout` | 0 | **30** | Idle-connection reclamation — connections unused for 30s are dropped from the pool. Prevents the pool from holding a clutch of idle (potentially dead) connections through a quiet period. |

`Pooling=true` is the SqlClient default — no need to set it. `Max Pool Size` (default 100) is
fine for a single-site DNN install; do not lower it.

> **Note on parameter aliases**: `Connection Lifetime` and `Load Balance Timeout` are the
> canonical SqlClient keys (synonyms exist historically). They are independent knobs: Lifetime
> caps total age; Load Balance Timeout caps idle age. Both are relevant here.

## 5. How to apply (jsboige, VPS — not in this repo)

1. RDP to the prod VPS. Back up `DNNPlatform/web.config`.
2. Edit the `SiteSqlServer` connection string, appending the 4 parameters above (semicolon-
   separated, inside the `connectionString="..."` value, preserving the existing
   `Server`/`Database`/credentials).
3. Save (touches `web.config` → IIS recycles the app domain, so this also clears the pool on
   first hit).
4. Smoke-test: idle the site for the observed idle window, then probe `/Règles`. The first-hit
   stall should be gone (or much shorter).
5. Optionally confirm the parameter take-up via a DNN "Test database connection" if exposed,
   or by watching the SQL Server `sys.dm_exec_connections` for the `Min Pool Size` pre-warm.

## 6. Risks / reversibility

- **Fully reversible** — remove the 4 parameters to restore defaults.
- `Min Pool Size=5` holds 5 persistent SQL connections; trivial at this scale (DNN single site).
- `Connection Lifetime=300` causes a low-rate connection churn (≤ a handful of reconnects/min);
  negligible vs. the hang cost.
- No schema or app-code change; no DNN/2sxc version implication.

## 7. Recommendation

- **Decision = jsboige** (prod `web.config` = ops lane, out of read-only scope).
- Recommended sequencing: apply **Option A** first (it is already GO'd and bounds the problem),
  then layer **Option C** if within-window stalls persist. They are independent and stackable.
- **Confirm the root cause on the VPS** before relying on Option C alone — if the hang turns
  out to be something other than pool staleness (e.g., IIS worker process start latency, app
  domain first-load JIT), the connection-string tuning won't address it and Option A is the
  primary lever.

## 8. Gate boundaries

- ❌ Not applied. No prod write, no VPS access, no `web.config` mutation.
- ✅ Values + rationale + apply-steps documented for jsboige's arbitration.
- ✅ Consistent with audit #662 §6 (Option C deferred to jsboige).

Relates: [2026-07-03-dnn-prod-rules-coverage.md](../investigations/2026-07-03-dnn-prod-rules-coverage.md)
(audit #662, idle-hang §6), ai-01 dispatch `msg-20260703T171820-8b3ymj`.
