# #1064 — CSP whitelist derivation (7 pages × all directives)

**Issue:** [#1064 — préprod CSP blocks 3rd-party origins used in prod; complete the whitelist before cutover](https://github.com/ArgumentumGames/Argumentum/issues/1064)
**Author:** Claude Code @ myia-po-2024 (worker)
**Date:** 2026-08-11
**Base:** master `349b578f`
**Status:** **DOSSIER / docs-only, non-gated.** Delivers ai-01 dispatch `js3d06` [primaire]: a **complete, sourced whitelist** derived from the HTML really served across the **7 pages × all directives**, not just the home-page probe. It is a **measure**, not a fix — **zero `web.config` mutation** (out of scope without GO; the préprod web.config is a forensic piece per #1049). Whoever has the GO applies the whitelist.

> **Headline finding (extends ai-01's home probe, method = runtime violations):** ai-01's 4 minimal additions (`sibforms.com`, `cdnjs.cloudflare.com`, `cdn.jsdelivr.net`, `assets.sendinblue.com`) are **confirmed correct** — all are global-skin origins on every page. But the 7-page runtime sweep surfaces **5 additional page-specific origins** the home page does not load (`ajax.aspnetcdn.com`, `use.fontawesome.com`, `fonts.cdnfonts.com`, the HTTP font `db.onlinewebfonts.com`), plus a **critical separation** ai-01's method correction (msg `w35711`) implies but did not fully draw: the préprod cutover has **two distinct gates**, not one. (1) The **CSP whitelist** — origins préprod *tries* to load and the policy blocks (the runtime violations). (2) **Content-migration gaps** — assets/tags present in prod but **entirely absent** from préprod, which no CSP change can fix: the **GTM/GA tags did not migrate** (analytics dies silently post-cutover even with GTM whitelisted) and the **8 `fallacies_*.html` mindmap wrappers are 404** on préprod (owner-flagged). CSP work alone closes gate (1); gate (2) needs file/tag migration by po-2023.

---

## TL;DR — the complete whitelist

Current CSP (préprod, read verbatim from the response header, 2026-08-11):

```
default-src 'self';
script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdn.jsdelivr.net;
style-src  'self' 'unsafe-inline' https://fonts.googleapis.com;
font-src   'self' https://fonts.gstatic.com;
img-src    'self' data: https:;
frame-src  'self';
connect-src 'self' https://api.stripe.com;
```

Sources to **add** (sourced from the 7-page sweep — `✓confirmed` = measured CSP violation on préprod; `decision` = needs owner call; `verify` = interaction-gated, unreachable this pass):

| directive | ADD | origin | pages | status |
|---|---|---|---|---|
| **script-src** | `https://sibforms.com` | Brevo newsletter JS | all 7 | ✓confirmed (home console) |
| | `https://cdnjs.cloudflare.com` | Font Awesome 5.15.4 JS | all 7 | ✓confirmed |
| | `https://ajax.aspnetcdn.com` | ASP.NET AJAX CDN | `/Acheter-le-jeu` | ✓confirmed (page-specific) |
| **style-src** | `https://sibforms.com` | Brevo form CSS | all 7 | ✓confirmed |
| | `https://cdn.jsdelivr.net` | fancybox 4.x CSS | `/`, `/Règles` | ✓confirmed |
| | `https://use.fontawesome.com` | Font Awesome CSS (kit) | `/Acheter-le-jeu` | ✓confirmed (page-specific) |
| **font-src** | `https://assets.sendinblue.com` | Brevo Roboto (6 files) | all 7 | ✓confirmed (×6) |
| | `https://fonts.cdnfonts.com` | custom font | `/Règles` | ✓confirmed (page-specific) |
| | ⚠️ `http://db.onlinewebfonts.com` | custom font **over HTTP** | `/Règles` | ✓confirmed — **see §5 defect** |
| **connect-src** | `https://sibforms.com` | newsletter POST (Brevo `serve` endpoint) | all (newsletter) | verify — interaction-gated (script blocked → form non-functional → can't submit) |
| **img-src** | — (already `'self' data: https:`) | — | — | ⚠️ **do not tighten** (ai-01 warning: already permissive, any HTTPS img passes) |
| **frame-src** | `https://www.googletagmanager.com` | GTM `ns.html` noscript iframe | prod home (today) | conditional — **§3: only if GTM tags migrate**; préprod has no GTM iframe today |
| **conditional** | `script-src https://www.googletagmanager.com` + `connect-src https://region1.google-analytics.com` | GTM/GA4 | prod (9/9) | **§3: add ONLY alongside GTM tag migration** — GA endpoint is runtime-injected, invisible to static grep |
| **verify** | `https://js.stripe.com` (script) + `https://checkout.stripe.com` (frame?) | Stripe Checkout | `/Acheter-le-jeu` checkout | **§4 verify** — unreachable (out of stock) |

**Bottom line for the GO-applicator:** apply the 9 `✓confirmed` additions (script ×3, style ×3, font ×3) — that unblocks everything visibly broken on the 7 pages. Then decide the GTM/GA question (§3) and verify Stripe checkout (§4) before/after cutover. The HTTP font (§5) should be re-sourced to HTTPS rather than allowed as `http://`.

---

## 1. Method (triple grounding)

- **Technical (code = truth):** Playwright (Chromium) loaded each of the 7 pages on **prod** (`www.argumentum.games`, no CSP = ground truth of what loads) and **préprod** (`dnn.argumentum.myia.io`, CSP active). Cache-buster `?v=probe1064` on every URL (cache prod = 1 year, per dispatch). The authoritative instruments: (a) the **network log** (`performance.getEntriesByType('resource')` + Playwright network capture) = what actually loaded on prod; (b) the **console CSP violations** on préprod = what each page *tried* to load and the current policy blocked, with the exact directive string quoted in each error. The **full CSP header** was read verbatim via `fetch(url).headers.get('content-security-policy')`.
- **Conversational:** ai-01 dispatch `js3d06` [primaire] + the issue body (provenance `092bb8f3` / PR #442 / Epic #131, "restrictive default; relax per-site as needed").
- **Semantic:** the 7-page list itself comes from ai-01's #984 re-measurement (the menu is client-rendered, so she derived the real page set from her #1049 routing measure — `/`, `/actus`, `/Règles`, `/Argumentation`, `/Téléchargements`, `/Amis`, `/Acheter-le-jeu`).

**Two method pitfalls respected (from the dispatch):**
1. `img-src 'self' data: https:` is **already permissive** — not tightened, only flagged.
2. **Silence ≠ proof** for `connect-src`/`frame-src`: the Brevo newsletter POST and the Stripe checkout are interaction-gated. The Brevo form is non-functional on préprod (its script is CSP-blocked), so I could not submit it to confirm the POST target — `sibforms.com` connect-src is inferred from Brevo's known `serve` endpoint and flagged `verify`. Stripe checkout was unreachable (out of stock) — flagged `verify`.

---

## 2. Per-page origin inventory (préprod, the post-cutover site)

Préprod = DNN **10.3.2** + 2sxc **21.07** (a version *upgrade* over prod's 2sxc 15.02 — see §3). Origins cited are those each page **tried** to load (CSP violation on préprod = authoritative "the page needs this").

| page | script-src (blocked) | style-src (blocked) | font-src (blocked) |
|---|---|---|---|
| `/` (home) | sibforms.com, cdnjs.cloudflare.com | sibforms.com, cdn.jsdelivr.net | assets.sendinblue.com ×6 |
| `/actus` | sibforms.com, cdnjs.cloudflare.com | sibforms.com | assets.sendinblue.com ×6 |
| `/Règles` | sibforms.com, cdnjs.cloudflare.com | sibforms.com, cdn.jsdelivr.net | assets.sendinblue.com ×6, **fonts.cdnfonts.com**, **db.onlinewebfonts.com ⚠️HTTP** |
| `/Argumentation` | sibforms.com, cdnjs.cloudflare.com | sibforms.com | assets.sendinblue.com ×6 |
| `/Téléchargements` | sibforms.com, cdnjs.cloudflare.com | sibforms.com | assets.sendinblue.com ×6 |
| `/Amis` | sibforms.com, cdnjs.cloudflare.com | sibforms.com | assets.sendinblue.com ×6 |
| `/Acheter-le-jeu` | sibforms.com, cdnjs.cloudflare.com, **ajax.aspnetcdn.com** | sibforms.com, **use.fontawesome.com** | assets.sendinblue.com ×6 |

**Observations:**
- `sibforms.com` (script+style), `cdnjs.cloudflare.com` (script), `assets.sendinblue.com` (font) are **global** — in the 2sxc skin, present on all 7 pages. These are ai-01's home-probe finds, confirmed everywhere.
- `cdn.jsdelivr.net` (fancybox) appears on pages **with fancybox content** (`/`, `/Règles`) — not global.
- **Page-specific** (missed by a home-only probe): `/Acheter-le-jeu` adds `ajax.aspnetcdn.com` (script) + `use.fontawesome.com` (style); `/Règles` adds `fonts.cdnfonts.com` + `db.onlinewebfonts.com` (fonts). A whitelist built from the home page alone would leave the purchase page and the rules page partially broken.

---

## 3. Google Tag Manager / Analytics — a CONTENT-MIGRATION gap, not a CSP question (definitive, measured)

ai-01's method correction (`w35711`) flagged GTM as 9/9 must-add on prod. Re-measuring on **préprod** (the post-cutover site) sharpens this: **préprod does not reference GTM at all** — neither script nor iframe. So GTM raises two questions, and conflating them is the trap.

**Measured, HTML-level + network-level:**

| site | `gtag/js` (script) | `gtm.js` (script) | `ns.html` (frame, noscript) | `google-analytics.com` (connect) |
|---|---|---|---|---|
| **prod** home | ✅ `G-VHLTL18PEW` | ✅ `GTM-TZBQ57M` | ✅ (noscript iframe) | ✅ POST `g/collect` at **runtime** — *not* in served HTML |
| **préprod** `/Acheter-le-jeu` | ❌ none | ❌ none | ❌ none | ❌ none |

**The two questions, separated:**

1. **CSP whitelist (gate 1):** IF the GTM tags migrate to préprod, the CSP must allow them — `script-src www.googletagmanager.com`, `frame-src www.googletagmanager.com` (noscript iframe `ns.html`), and `connect-src region1.google-analytics.com` (the GA4 `g/collect` endpoint that GTM injects at runtime, invisible to any static grep — exactly the "GTM loads GA at execution" point in `w35711`). **Add these conditionally**, to take effect only when the GTM tags are present.

2. **Content migration (gate 2):** the GTM tags (`GTM-TZBQ57M`, `G-VHLTL18PEW`) are in prod's skin/content but **absent from préprod**. No CSP entry can fix a missing tag. **Without migrating the GTM container, analytics dies silently post-cutover — even with GTM whitelisted.** This is a po-2023 lane item (content migration), parallel to the CSP change.

⚠️ This is the "silence is not proof" trap in its deepest form: préprod is silent on analytics *not* because the CSP blocks it, but because **the tag isn't there**. A whitelist-only change would silently leave analytics broken; a migration-only change would silently leave it CSP-blocked. **Both gates must close.** Decision (keep vs drop analytics) = jsboige/owner — recorded via ai-01's GO `w35711` as option (a) = keep.

> **Note:** `fonts.gstatic.com` (already in font-src) and `fonts.googleapis.com` (already in style-src) are Google **Fonts** CDNs — unrelated to GTM/GA and already allowed.

---

## 4. Verify — Stripe checkout (unreachable this pass)

`connect-src 'self' https://api.stripe.com` is already in the CSP — so the Stripe **API** call is allowed. But standard **Stripe Checkout** also loads:
- `https://js.stripe.com` (script-src) — Stripe.js loader,
- and opens `https://checkout.stripe.com` or `js.stripe.com/v3/` (frame-src) — the hosted checkout iframe.

I could **not** confirm these because `/Acheter-le-jeu` shows **"Pas de stock disponible"** (out of stock) and the cart is empty — no checkout flow is reachable. The page's own console added no Stripe violation (it never got that far).

**Action for the GO-applicator:** before relying on the whitelist, exercise a real checkout on a stocked product and confirm whether `js.stripe.com` (script) and a Stripe `frame-src` are needed. If the site uses Stripe Checkout (hosted), add them; if it uses only the API (server-side), `api.stripe.com` in connect-src already suffices.

---

## 5. Security defect — HTTP font on `/Règles` (`db.onlinewebfonts.com`)

The `/Règles` page attempts to load a font from **`http://db.onlinewebfonts.com`** (plain HTTP, not HTTPS). Measured as a CSP `font-src` violation. Two problems:

1. **CSP cannot allow it cleanly.** The policy is HTTPS-oriented; allowing `http://db.onlinewebfonts.com` in `font-src` introduces a mixed-content exception and weakens the font directive.
2. **It is a defect independent of CSP.** A page served over HTTPS pulling a font over HTTP is mixed content; browsers may block it regardless of CSP. `db.onlinewebfonts.com` is a public font-CDN that does serve over HTTPS.

**Recommendation (distinct from the whitelist):** do **not** add `http://db.onlinewebfonts.com` to the CSP. Instead, switch the `/Règles` font reference to its HTTPS equivalent (or re-host the font). Same for `fonts.cdnfonts.com` (verify it is HTTPS-served — the violation showed `https://fonts.cdnfonts.com`, so it is fine to allow). A separate issue should track the HTTP-font fix on `/Règles`; it is not blocking the cutover CSP decision but is a latent mixed-content defect.

Also noted (minor): the home page carries an **`http://argumentum.games/argumentum_fallacies.owl`** link (HTTP, not HTTPS) — a mixed-content link to the OWL ontology on the apex domain. Should be HTTPS.

---

## 6. NOT-CSP — mindmap wrapper static assets 404 on préprod (owner-flagged, content-migration gap)

Owner flagged `https://dnn.argumentum.myia.io/fallacies_fr.html` → **404**. Measured (cache-buster): this is **not a CSP issue** (CSP allows `'self'`; the file is simply absent, not blocked) — it is a **static-asset migration gap**, parallel to the GTM tag gap (§3, gate 2). The 7424-file préprod restore from `78cd1aab` captured the DNN platform tree but **not** these hand-deployed static HTML files at the site root.

**Blast radius (measured — the gap is 8× the one file flagged):**

| resource | prod | préprod |
|---|---|---|
| `fallacies_fr.html` | **200** (2.36 MB) | **404** |
| `fallacies_en.html` | **200** (2.21 MB) | **404** |
| `fallacies_ru / pt / es / ar / fa / zh .html` | **200** (all) | **404** (all) |
| `argumentum_virtues_{fr,en}.html` | 404 (both) | 404 (both) — **out of scope**, different path/deployment |

These are the **Fallacies mindmap HTML wrappers** (`<title>Taxonomy Mind Map</title>`, inline SVG + svg-pan-zoom). They live in the repo at `Cards/Fallacies/Mindmaps/{lang}/Fallacies_{lang}.html` (8 langs × 2 variants `_ext`). **No `.cs/.json/.config` file references them by name** — there is no automated deployment path; they are copied to the prod site root by hand, which is why the migration missed them.

**Cutover impact:** `/Acheter-le-jeu` links "taxonomie"→`fallacies_fr.html` and "plusieurs langues"→`fallacies_en.html`. Post-cutover, both break — plus the 6 other-language wrappers die (dead links).

**Fix (NOT a CSP change — po-2023 lane + GO):** copy the 8 wrappers (×2 if `_ext` is also linked) from the repo `Cards/Fallacies/Mindmaps/{lang}/` to the préprod site root. Préprod-webroot mutation = po-2023 procedure (same #1049 safeguards: copy-only, no `/MIR`, horodated backup). Recorded here as a **distinct cutover-prep item**, surfaced during the CSP sweep; it is **not** part of the whitelist table.

---

## 7. Scope / gel

- **0 `web.config` mutation · 0 CSV mutation · 0 code change · 0 artefact regen · 0 CardPen · 0 webroot write.**
- 1 net-new doc, this file. Branch + PR, no direct push to master.
- ⛔ I did **not** read `DNNPlatform/web.config` in git history (machineKeys risk, per ai-01's operational warning). The CSP string was read from the **live response header** of the served page, not from the repo.
- The whitelist is the **matter for the GO-applicator's** decision; applying it is ai-01's/jsboige's lane (verdict + GO), not a worker's. The mindmap-wrapper gap (§6) and GTM-migration gap (§3) are surfaced here but executed in po-2023's lane.

## Sources

- Live measure (2026-08-11, Playwright/Chromium, cache-buster `?v=probe1064`):
  - prod network: `www.argumentum.games` home → 5 external origins (`googletagmanager.com`, `sibforms.com`, `cdn.jsdelivr.net`, `cdnjs.cloudflare.com`, `region1.google-analytics.com`); GTM tags `GTM-TZBQ57M` + `G-VHLTL18PEW` confirmed in HTML.
  - préprod network: `dnn.argumentum.myia.io` home → 5 external origins (`assets.sendinblue.com`, `cdn.jsdelivr.net`, `cdnjs.cloudflare.com`, `fonts.gstatic.com`, `sibforms.com`) — **no Google**; GTM tags **absent from HTML**.
  - préprod CSP violations across 7 pages → the per-page origin inventory (§2); GTM reconciliation HTML+network (§3).
  - CSP header read verbatim via `fetch(url).headers.get('content-security-policy')`.
  - 404 sweep: `fallacies_{fr,en,ru,pt,es,ar,fa,zh}.html` prod=200 / préprod=404 (§6).
- [`docs/release-dossier/README.md`](README.md) §3-§4 (release gate inventory + checklist convention).
- ai-01 #1064 body: provenance `092bb8f3` / PR #442 / Epic #131, "restrictive default; relax per-site as needed". Method correction msg `w35711` (runtime violations, not static HTML); GO option (a) `issuecomment-5255688887`.

🤖 po-2024 — ai-01 dispatch `js3d06` [primaire], base `349b578f`, read-only (0 `web.config` mutation)
