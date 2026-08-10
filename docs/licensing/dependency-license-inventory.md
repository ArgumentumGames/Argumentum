# Dependency license inventory — pipeline (Argumentum.AssetConverter)

> **Purpose.** Release-gate audit: confirm every dependency of the shipping pipeline is under a
> permissive license, surfaced by the accidental discovery (#887) that AutoMapper 15.x flips MIT →
> RPL-1.5/commercial. No full-set audit had been done. Prepared ahead of public release v0.9.0
> (ai-01 dispatch `msg-20260725T153355-0aqodp`).
>
> **Base:** master `a9400a6e`. **Date:** 2026-07-25. **Author:** po-2024 (read-only audit).
> **Method:** `dotnet list package --include-transitive` + NuGet Registration API
> (`registration5-gz-semver2`) for `licenseExpression` / `licenseUrl` / `requireLicenseAcceptance`.
> **Rule:** the nuspec is truth — where doc and nuspec diverge, nuspec wins.

---

## Verdict (TL;DR)

**The shipping pipeline (`Argumentum.AssetConverter`) is 100 % permissive** — all 24 direct deps
and the full transitive closure are MIT / Apache-2.0 / MS-PL / BSD-3. No GPL, AGPL, RPL, SSPL, or
commercial dependency ships in the release binary. The AutoMapper RPL-1.5 exposure is already
mitigated (pinned to 14.0.0, last MIT, #588/#902).

> **Update (post-audit, 2026-07-25):** the one direct dep that originally declared no license —
> `UglyToad.PdfPig 1.7.0-custom-5` — has been **swapped to the official `PdfPig` 0.1.14
> (Apache-2.0)** via #908 (issue #906), so **24/24 direct deps are now permissive**. The original
> finding is retained in §7.2 as the audit record; the §1/§3 tables reflect the post-swap state.

> **Update (2026-07-27): the last flagged item is closed too.** The one non-release flag —
> `FluentAssertions 8.5.0`, commercial Xceed — was arbitrated by jsboige (**downgrade to the last
> Apache-2.0 release**) and **implemented on master** in #955 (`6d0bfda9`): 7.2.2 + a semver-major
> `ignore` in the same PR. §7.1 is retained as the audit record. **Zero flagged items remain**,
> shipping or otherwise.

The license gate for the **shipping binary is PASS**.

---

## 1. Direct dependencies — `Argumentum.AssetConverter` (the shipping pipeline)

24 top-level packages. All permissive.

> ⚠️ **Colonne `Version` périmée sur 16 des 24 lignes** (mesuré sur master `ece9af30`) — voir **#1051**. #970 a monté 23 paquets sans mettre ce tableau à jour. Les conclusions de licence restent valides et **les deux épinglages porteurs tiennent** (`AutoMapper 14.0.0` dernier MIT, `QuestPDF 2022.12.12` dernier MIT, vérifiés sur le `.csproj`) : c'est un défaut d'exactitude du registre, pas de conformité. Ne pas recopier les versions sans revérifier la licence **de la version citée**.

| Package | Version | License (nuspec) | reqAccept |
|---------|---------|------------------|-----------|
| AutoMapper | 14.0.0 | **MIT** ✅ (last MIT; 15.x = RPL-1.5 — pinned, #588) | False |
| OpenAI | 2.10.0 | MIT | True |
| CsvHelper | 31.0.4 | MS-PL OR Apache-2.0 | True |
| dotNetRdf | 3.3.2 | MIT | False |
| Google.Apis.Sheets.v4 | 1.68.0.3525 | Apache-2.0 | False |
| ExtendedXmlSerializer | 3.7.18 | file-url → **MIT** (see §6) | False |
| Humanizer | 2.14.1 | MIT | False |
| Magick.NET-Q16-AnyCPU | 14.15.0 | **Apache-2.0** ✅ (#902) | False |
| Microsoft.Playwright | 1.43.0 | MIT | False |
| Newtonsoft.Json | 13.0.3 | MIT | False |
| OWLSharp | 4.23.0 | Apache-2.0 | False |
| OWLSharp.Extensions | 4.22.0 | Apache-2.0 | False |
| QuestPDF | 2022.12.12 | **MIT** ✅ (>2022.12.12 = commercial — pinned) | False |
| SkiaSharp.NativeAssets.Win32 | 2.88.6 | file-url → **MIT** (MS .NET license, §6) | True |
| SharpToken | 2.0.2 | file-url → **MIT** (see §6) | False |
| Spectre.Console | 0.50.0 | MIT | True |
| Spectre.Console.Json | 0.50.0 | MIT | True |
| System.ComponentModel.TypeConverter | 4.3.0 | file-url → **MIT** (MS .NET license) | True |
| System.Drawing.Primitives | 4.3.0 | file-url → **MIT** (MS .NET license) | True |
| System.Linq.Dynamic.Core | 1.7.2 | Apache-2.0 | False |
| System.Management | 8.0.0 | MIT | False |
| PdfPig | 0.1.14 | **Apache-2.0** ✅ (official; swapped from `UglyToad.PdfPig 1.7.0-custom-5` via #908, §7.2) | False |
| Utf8Json | 1.3.7 | (none in catalog) → **MIT** (known, §6) | False |
| xunit.extensibility.core | 2.8.1 | Apache-2.0 | False |

## 2. Direct dependencies — other solution projects

| Project | Package | Version | License | Notes |
|---------|---------|---------|---------|-------|
| Tests | coverlet.collector | 6.0.2 | MIT | |
| Tests | FluentAssertions | 7.2.2 | Apache-2.0 | downgraded from 8.5.0 commercial via #955 — see §7.1 |
| Tests | Microsoft.NET.Test.Sdk | 17.12.0 | MIT (MS) | |
| Tests | Microsoft.Playwright | 1.43.0 | MIT | |
| Tests | Scriban | 7.2.2 | MIT | |
| Tests | xunit | 2.9.2 | MIT/Apache-2.0 | |
| Tests | xunit.runner.visualstudio | 2.8.2 | MIT (MS) | |
| VisualTests | PdfPig | 0.1.14 | **Apache-2.0** (real PdfPig) | now matches AssetConverter post-swap #908 |
| VisualTests | Verify.ImageSharp | 4.4.1 | MIT | |
| VisualTests | Verify.Xunit | 30.7.3 | MIT | |

## 3. Transitive dependencies (notable, non-trivial)

Full closure enumerated via `--include-transitive`. All permissive. The non-Microsoft transitives:

| Package | Version | License | Pulled by |
|---------|---------|---------|-----------|
| Lucene.Net (+ Analysis.Common, Queries, QueryParser, Sandbox) | 4.8.0-beta00017 | Apache-2.0 (catalog gap, known) | dotNetRdf full-text |
| LightInject | 6.6.1 | MIT | OWLSharp |
| HarfBuzzSharp (+ NativeAssets) | 7.3.0 | MIT | SkiaSharp |
| J2N | 2.1.0 | MIT | Lucene.Net |
| AngleSharp | 1.1.2 | MIT | |
| HtmlAgilityPack | 1.11.67 | MIT | |
| NReco.LambdaParser | 1.0.12 | MIT | System.Linq.Dynamic.Core |
| NetTopologySuite | 2.6.0 | BSD-3-Clause (permissive) | |
| ProjNET | 2.1.0 | MIT | |
| RDFSharp | 3.23.0 | Apache-2.0 | |
| Resta.UriTemplates | 1.4.0 | MIT | |
| **SharpZipLib** | 1.4.2 | **MIT** ✅ (confirmed — 1.x relicensed from old GPL) | |
| SkiaSharp / SkiaSharp.HarfBuzz | 2.88.6 | MIT | |
| Sprache | 2.3.1 | MIT | |
| VDS.Common | 2.0.0 | MIT | dotNetRdf |
| Magick.NET.Core | 14.15.0 | Apache-2.0 | Magick.NET |
| PdfPig.{Core,Fonts,Tokenization,Tokens} | 0.1.14 | Apache-2.0 | PdfPig (post-swap #908; pre-swap `UglyToad.PdfPig.*` 1.7.0-custom-5 carried none, §7.2) |
| xunit.abstractions | 2.0.3 | Apache-2.0 | |

The Microsoft / `System.*` / `runtime.*` / `Humanizer.Core.*` (×50 locale satellites) transitives
are all MIT (MS .NET license) — not enumerated individually, ~120 packages, all permissive.

## 4. The three license-pinned dependencies (cross-check, #902 doc)

| Package | Pinned at | License | Next-version risk | nuspec vs doc |
|---------|-----------|---------|-------------------|---------------|
| **AutoMapper** | 14.0.0 | MIT | 15.0.0+ → **RPL-1.5 / commercial** (Lucky Penny, `requireLicenseAcceptance`) | nuspec = MIT ✅ (matches #902) |
| **Magick.NET-Q16-AnyCPU** | 14.15.0 | Apache-2.0 | none (Apache stable) | nuspec = Apache-2.0 ✅ |
| **QuestPDF** | 2022.12.12 | MIT | >2022.12.12 → **commercial** (community-license) | nuspec = MIT ✅ |

All three nuspec values match the #902 documentation. No divergence.

## 5. `requireLicenseAcceptance = true` (informational — not a risk)

`requireLicenseAcceptance` means the nuspec requires an accept-click on install; it does **not**
imply a non-permissive license. All of these are permissive:
OpenAI, CsvHelper, SkiaSharp.NativeAssets.Win32, Spectre.Console, Spectre.Console.Json,
System.ComponentModel.TypeConverter, System.Drawing.Primitives, NetTopologySuite, Google n/a.
(**FluentAssertions** used to be listed here as the one commercial exception; since #955 master
carries 7.2.2, whose nuspec carries **no `requireLicenseAcceptance` element at all** — NuGet's
default is `false` — against `true` on 8.5.0. Measured 2026-07-27; see §7.1.)

## 6. `type="file"` licenses (the SPDX angle-matter)

These ship a license *file* rather than an SPDX expression — invisible to license scanners that
read only expressions. Resolved manually:

| Package | file-url resolves to | Verdict |
|---------|---------------------|---------|
| ExtendedXmlSerializer 3.7.18 | MIT ( ExtendedXmlSerializer) | permissive |
| SharpToken 2.0.2 | MIT (MattW) | permissive |
| SkiaSharp.NativeAssets.Win32 2.88.6 | MS .NET license (linkid=868514) = MIT | permissive |
| System.ComponentModel.TypeConverter 4.3.0 | MS .NET license (linkid=329770) = MIT | permissive |
| System.Drawing.Primitives 4.3.0 | MS .NET license (linkid=329770) = MIT | permissive |

Catalog-gap (no expression, resolved by known license): `Utf8Json 1.3.7` (MIT, neuecc),
`Lucene.Net 4.8.0-beta00017` (Apache-2.0).

## 7. To arbitrate (flagged)

### 7.1 FluentAssertions 8.5.0 — RESOLVED (downgraded to 7.2.2 Apache-2.0 via #955)

> **Update (2026-07-27): RESOLVED.** jsboige arbitrated **downgrade** (of the three options below).
> Implemented on master in #955 (`6d0bfda9`): `8.5.0 → 7.2.2` in `Tests.csproj` **plus** a
> semver-major `ignore` entry in `dependabot.yml` **in the same PR** — pinning before downgrading
> would have frozen the commercial version. 7.2.x Apache patches keep flowing; only the 7→8 jump is
> blocked. Baseline held at 638/0/5 in **both** matrix legs, 0 warnings, no assertion weakened.
>
> **Correction of this section's own figure:** it named **7.2.0** as "last Apache-2.0". Measured on
> the restored nuspecs 2026-07-27, the last Apache-2.0 release is **7.2.2** — that is what shipped.
>
> | version | `<license>` | `<licenseUrl>` | `<authors>` |
> |---|---|---|---|
> | **7.2.2** | `type="expression"` → `Apache-2.0` | `licenses.nuget.org/Apache-2.0` | Dennis Doomen, Jonas Nyrup |
> | 8.5.0 | `type="file"` → `LICENSE` | `aka.ms/deprecateLicenseUrl` | Dennis Doomen, Jonas Nyrup, **Xceed** |
>
> The commercial takeover is legible in the **authorship line**, not only in the licence: `Xceed`
> joins `<authors>` at 8.x. A scanner reading only SPDX expressions sees *nothing* on 8.5.0 — the
> field is simply absent — which is the same blind spot §6 documents for QuestPDF.

**Original finding (pre-downgrade, audit base `a9400a6e`).**
FluentAssertions moved to a **commercial / proprietary license under Xceed** as of v8.0.0
(2025-02). The nuspec confirms the pattern: `licenseExpression` absent, `type="file"` license URL,
`requireLicenseAcceptance=true` — the exact `type="file"` angle-matter that hides from SPDX-only
scanners. **It is referenced only by `Argumentum.AssetConverter.Tests`** — it does not ship in the
release binary or the pipeline output.

Options for jsboige (**arbitrated: downgrade** — retained as the record of what was weighed):
- **Accept** — it is test-only, not distributed; the commercial term binds redistribution/use of
  FluentAssertions itself, not our test results. Many teams accept this for internal test runners.
- **Downgrade** to the last Apache-2.0 release — restores a permissive license. ✅ **CHOSEN**
  (measured target: 7.2.2, not the 7.2.0 originally written here).
- **Migrate** to an alternative (Shouldly, plain xunit asserts).

Recommendation (for ai-01 synthesis, not a verdict): **downgrade or migrate** if the project wants
an unambiguous permissive posture; **accept** if test-only usage is judged fine. Either way it does
not block the v0.9.0 release of the pipeline binary.

### 7.2 UglyToad.PdfPig 1.7.0-custom-5 — RESOLVED (swapped to official PdfPig via #908)

> **Update (2026-07-25): RESOLVED.** ai-01 hardened the provenance check (owner `grinay` — single
> account unrelated to upstream; invented `1.7.0` version above every real release; `dotnet pack`
> placeholder `<description>Package Description</description>`; no `projectUrl`; the official *package
> ID* is `PdfPig` while only the *namespace* is `UglyToad.PdfPig` — the confusable-IDs trap). The swap
> to official `PdfPig 0.1.14` (Apache-2.0, declared as an SPDX expression in the nuspec) shipped in
> #908 (issue #906). The §1/§3 tables now reflect the post-swap state. This section is retained as the
> audit record of the finding.

**Original finding (pre-swap, audit base `a9400a6e`).** The AssetConverter referenced
`UglyToad.PdfPig 1.7.0-custom-5` — a **custom version not on nuget.org** (the `-custom-5` suffix).
The catalog had no license expression/URL. The real PdfPig package (`PdfPig`, used in VisualTests at
0.1.14) is **Apache-2.0**, but a custom build's license depends on who built it and whether they
changed terms. This was the one item that prevented the headline from reading as a clean PASS —
fixed by swap rather than caveat, as the remediation was small (one file, two `using` lines).

## 8. Conclusion — gate status

- **Shipping binary (`Argumentum.AssetConverter`):** **24/24 direct deps permissive** (MIT /
  Apache-2.0 / MS-PL / BSD-3), full transitive closure permissive.
  AutoMapper/Magick/QuestPDF license-pins verified against nuspec. PdfPig-custom gap closed via #908.
  **License gate: PASS.**
- **Test tooling:** **also clean since 2026-07-27.** FluentAssertions was the single commercial
  item; jsboige arbitrated downgrade and #955 (`6d0bfda9`) landed 7.2.2 (Apache-2.0) with a
  semver-major pin. Across the surface this audit covers — the 24 direct deps of the shipping
  binary plus their transitive closure (§1/§2), and the direct deps of the test project (§3) —
  **no commercial or copyleft dependency remains**. The gate no longer carries an "awareness
  item" caveat (§7.1); §7 now holds two RESOLVED records and nothing open.
- **No GPL / AGPL / RPL / SSPL / proprietary** in the shipping dependency graph.
- **Vendored CardPen libraries** (§9, added 2026-07-29): a **separate, non-binary surface** — the
  card-rendering toolchain CardPen loads at harvest time. **Does not affect the shipping-binary
  gate** (CardPen is generation tooling, not the shipped `.dll`). Catalogued to lift the
  dependabot/scanner blind spot (#942): 12/13 permissive; one copyleft-adjacent item
  (`unidecode.js` data tables, Perl license) and one aged engine (`marked` ~0.3.x) flagged for
  visibility. Neither ships in the binary.

This is a result, not a gap: the audit proves the gate is met, which is what we need to show at
public release.

## 9. Vendored CardPen libraries — the non-npm blind spot (#942)

> **Scope.** CardPen (`Generation/CardPen/`) loads **vendored copies** of JS libraries from
> `lib/*.js` at runtime — these render the cards during harvesting. They are **not** npm packages:
> they are absent from `package.json` / the lockfile, so **dependabot, the license scanner, and no
> automated tool sees them**. This section makes them visible. It is a **read-only inventory**;
> CardPen itself was not modified (0 CardPen change, per #942 DoD).
>
> **Does this affect the shipping-binary gate (§1–§8)? No.** CardPen is generation tooling, not
> the shipped `Argumentum.AssetConverter` binary. By the #905 method — *distinguish what ships
> from what is merely present* — these libs are present-in-toolchain, not shipped-in-binary. They
> are recorded here for **visibility**, not to reopen the binary gate.

### 9.1 The 13 vendored `lib/*.js` files

Header provenance extracted directly from each file (code = truth). "License in file" means the
license text/URL is present in the file itself; "known upstream" means the file carries no
license header and the license is the library's well-known upstream.

| File | Bytes | Library & provenance | Version | License (source) |
|------|------|----------------------|---------|------------------|
| `codemirror.js` | 355 726 | CodeMirror — Marijn Haverbeke, `codemirror.net` | **5.25.2** (in-file) | **MIT** (in file, `codemirror.net/LICENSE`) |
| `handlebars.min.js` | 75 312 | Handlebars — Yehuda Katz, `handlebarsjs.com` | **4.0.10** (in-file) | **MIT** (full text in file) |
| `jszip.min.js` | 101 939 | JSZip — Stuart Knightley, `stuartk.com/jszip` | **3.1.3** (in-file) | **MIT OR GPL-3.0** (dual, in file) — MIT elected |
| `papaparse.min.js` | 14 568 | PapaParse — Matt Holt, `github.com/mholt/PapaParse` | **4.1.2** (in-file) | MIT (known upstream; no license text in file) |
| `underscore-min.js` | 16 410 | Underscore.js — Jeremy Ashkenas, `underscorejs.org` | **1.8.3** (in-file) | **MIT** (in file) |
| `mustache.min.js` | 9 528 | mustache.js — `janl/mustache.js` | **2.3.0** (in-file) | MIT (known upstream; no license text in file) |
| `marked.js` | 28 574 | marked — Christopher Jeffrey, `chjj/marked` | ~0.3.x era (2011–2014 copyright; no embedded version) | **MIT** (in file, "MIT Licensed") |
| `dom-to-image.min.js` | 8 864 | dom-to-image — `tsayen/dom-to-image` | build **04-04-2017** (in-file) | MIT (known upstream; no license text in file) |
| `dom-to-image-more.js` | 47 320 | dom-to-image-**more** fork — `1904labs/dom-to-image-more` | not embedded | MIT (fork of MIT original; no license text in file) |
| `FileSaver.min.js` | 2 446 | FileSaver.js — Eli Grey, `eligrey.com/FileSaver.js` | not embedded | MIT (known upstream; `@source` points to eligrey repo) |
| `bind.min.js` | 5 333 | bind.js — Remy Sharp, `github.com/remy/bind` | not embedded | **MIT** (declared in non-min source `bind.js`: `rem.mit-license.org`) |
| `bind.js` | 19 058 | **non-minified source** of `bind.min.js` — **NOT loaded** (dead file) | — | MIT (declared) |
| `unidecode.js` | 908 687 | node-unidecode — F-G Ribreau; **data tables converted from Perl `Text::Unidecode`** | data **1.000.000** (in-file) | ⚠️ **data tables: Perl license** (in file) — see §9.2 |

**Load map** (which file actually pulls each lib):

- `index.html` loads: `bind.min.js`, `codemirror.js`, `FileSaver.min.js`, `handlebars.min.js`,
  `jszip.min.js`, `marked.js`, `mustache.min.js`, `papaparse.min.js`, `underscore-min.js`,
  `unidecode.js`.
- `js/main.js:1218-1219` **emits** both `dom-to-image.min.js` **and** `dom-to-image-more.js` as
  `<script>` tags into the generated card HTML (both variants shipped into output).
- `bind.js` (non-min) is present but **never referenced** — vendored source duplicate of
  `bind.min.js`.

### 9.2 Flagged items (visibility, not binary-gate)

1. **`unidecode.js` — copyleft-adjacent.** The file declares (lines 546–549): *"the tables used
   (in data) are converted from the tables provided in the perl library Text::Unidecode … and are
   distributed under the perl license"* (`@author Francois-Guillaume Ribreau` = node-unidecode).
   The **Perl license** = Artistic-1.0-Perl OR GPL-1.0+ (the "same terms as Perl" dual). This is
   the **only non-permissive-MIT-family item** in the vendored set. It does **not** ship in the
   binary (CardPen tooling). The JS-wrapper license is not stated in the file; if binary
   shippability is ever in question (it is not), one `npm view unidecode license` resolves the
   wrapper. **Flagged for ai-01/jsboige awareness; no action required for v0.9.0.**

2. **`marked.js` — aged, the real card-rendering engine.** The vendored `marked` is a **~0.3.x
   release (2011–2014 copyright window, pre-fork `chjj/marked`)** — the version that actually
   renders card markdown across all 8 languages. This is **disjoint from** the npm `marked ^16.2.1`
   devDependency (which serves only `npm run build` doc generation, per #915/#942). Dependabot
   cannot see the vendored copy. The 0.3.x line predates many security fixes; **no known CVE is
   asserted here** (out of scope for an inventory), but the version-age is the security-visibility
   angle-mot #942 was opened to surface. Follow-up candidate, not v0.9.0-blocking.

3. **`jszip.min.js` — dual MIT/GPL-3.0.** Elects MIT (the dual license permits either). No issue;
   recorded for completeness.

### 9.3 DoD — #942 (vendored-surface part)

- [x] Inventory of CardPen vendored `lib/*.js` (version + licence + provenance) — §9.1.
- [x] `dependency-license-inventory.md` updated with the non-npm surface — this section.
- [x] Disposition of the dependabot lot — post-tag, one line per lot not per PR. Dispatched to
  po-2024 by ai-01 cycle 63 (superseding the earlier "out of lane" note): classification posted on
  [#942](https://github.com/ArgumentumGames/Argumentum/issues/942#issuecomment-5237884422) — Lot A
  (npm CardPen, merge groupé post-tag), Lot B (DNN `Portals/**`, fermeture motivée + `ignore`
  dependabot.yml *or* délégation chantier 2sxc po-2023 — jsboige tranche), Lot C (nuget pipeline,
  re-merge du reste sain post-#943, OWLSharp 5 sorti du groupe pour triage #133). Count dérivé :
  40 PRs ouvertes aujourd'hui (vs 22 à l'ouverture). 0 PR touchée (gel respecté).

> **Note on CardPen's own `LICENSE.txt`:** CardPen carries a **GPL-3.0** license file (upstream
> CardPen). That governs the CardPen source itself, not the bundled `lib/*.js` (each retains its
> own license). CardPen is generation tooling, not the shipping binary; this is informational for
> the §9 surface record and does not affect §8.

---

*Relates: #887 (AutoMapper 15 license flip), #902 (dependency table), #588 (AutoMapper pin), #134
(release), #458 TRACK 5 (dependabot governance). Audit script:
`dotnet list package --include-transitive` + NuGet Registration API, no third-party tool.*
