# `link-langlinks-resolve.py` — `link_*` candidate-URL resolver (sidecar, 0 write)

Companion tool to [`docs/taxonomy/192-link-coverage-langlinks-probe.py`](../docs/taxonomy/192-link-coverage-langlinks-probe.py). Extends the #600/#606 coverage track from **measurement** (the probe answers *"is this cell resolvable?"*) to **resolution** (this tool answers *"what is the URL?"* and emits the candidate-fill list).

This is **step 1 of the #600 §6 fill methodology**. It never writes under `Cards/`.

## What it produces

A sidecar report (stdout, or `--out <path>`):

```
dataset,key,link_lang,resolved_url
fallacies,52,pt,https://pt.wikipedia.org/wiki/Fal%C3%A1cia_da_proje%C3%A7%C3%A3o_mental
fallacies,1260,ar,https://ar.wikipedia.org/wiki/...
```

For every node that has an `en.wikipedia.org/wiki/<Title>` `link_en` and is **missing** `link_<lang>`, it queries the MediaWiki `langlinks` API, captures the target-language title, and emits the resolved URL. The probe measured **~2919 resolvable cells (57 %)** (#600 §5.1); this tool materializes the candidate URLs behind that ceiling.

## Usage

```bash
# full run, fallacies, stdout
python tools/link-langlinks-resolve.py

# strided sample of 50, virtues
python tools/link-langlinks-resolve.py 50 virtues

# full run, fallacies, sidecar file
python tools/link-langlinks-resolve.py 0 fallacies --out tmp/link-resolve-fallacies.csv
```

Args: `[sample_size] [dataset] [--out path]`. `sample_size=0` = full run. Dataset ∈ {fallacies, virtues}.

## Safety / scope

- **0 write under `Cards/`** — sidecar only (stdout or `--out`). Pre-tag freeze respected.
- Public MediaWiki API, no key, ~0.3 s throttle, descriptive User-Agent (default urllib UA is 403-forbidden).
- **RTL/CJK homonym risk** (#600 §6.4): AR/FA/ZH resolved URLs are **candidates**, not authoritative — human spot-validation is non-optional before any write. The sample run already surfaces this (e.g. an English "Engagement" homonym resolved for a fallacy node).
- Resolves **from** `link_en` (Wikipedia URLs only). Non-Wikipedia `link_en` (rationalwiki, yourlogicalfallacyis, etc. — 433 Fallacies / 9 Virtues) are curated sources, excluded and preserved as-is (#600 §6.2).

## Next step (post-release, gated)

A follow-up PR consumes this sidecar: apply the candidate URLs cell-by-cell (drift-free `QUOTE_MINIMAL` + CRLF, method #595), **skip non-empty cells** (preserve curated links), then human spot-validate the ~5 % residue (AR/FA/ZH priority, ~150 cells).

Relates to #600, #606, memory `i18n-coverage-gap-is-link-urls`.
