# Dependencies — pinned versions and why

> **Read on demand — not loaded by default.** Read this before bumping a package or reviewing a Dependabot pull request.
> Entry point and invariants: [`AGENTS.md`](../../AGENTS.md) · silent failures and the map of section titles: [`CLAUDE.md`](../../CLAUDE.md) · about this folder: [`README.md`](README.md).
> The sections below were moved here **verbatim** from `CLAUDE.md` on 2026-11-13 — headings and wording unchanged, so a citation such as *CLAUDE.md § Stable Dependency Versions* names a heading that exists here.

### Stable Dependency Versions

| Package | Version | Notes |
|---------|---------|-------|
| QuestPDF | 2022.12.12 | **Licence-pinned** — MIT free license; also thread-safe issues above this |
| Magick.NET-Q16-AnyCPU | **15.0.0** (csproj) | Image processing (per-image CMYK conversion is a no-op for PNG output; CMYK for print is applied via Ghostscript post-process on the final PDFs, see #632). Bumped 14.14.0 → 14.15.0 via #871 (2026-07-25, the declared first-patched version for 5 advisories accumulated against 14.14.0), then → 14.17.1 via #1301, then → 14.17.2 via #1627, then → **15.0.0** via #1905 (2026-10-22 — major: the `AlphaOption` enum is renamed `AlphaAction`, 2 call sites in `ImageHelper`; web2 measured 24/24 real cards pixel-identical 14.17.2 vs 15.0.0 with an inverse control). Licence unchanged (Apache-2.0, re-read on the 15.0.0 nuspec 2026-10-22) |
| AutoMapper | 14.0.0 | **Licence-pinned — do NOT bump.** 14.0.0 is the last MIT release; 15.0.0+ is RPL-1.5 / commercial dual-licensed (Lucky Penny), `requireLicenseAcceptance: true`. Decision jsboige 2026-06-23, implemented in #588 (`6caf5833`): stay MIT-pure + targeted `NuGetAuditSuppress` for GHSA-rvv3-g6hj-g44x + `MaxDepth(1)` guard in `Entities/MappingProfile.cs` (the only Profile is flat and acyclic, so the vulnerable recursion path is unreachable). No patched 14.x exists. Dependabot re-proposes 15.x periodically — close it (see #887) |
| SkiaSharp.NativeAssets.Win32 | **2.88.9** (csproj) | Required for QuestPDF — minor/patch bumps stay inside the 2.88.x line QuestPDF 2022.12.12 links against (2.88.9 via #970) |
| Microsoft.Playwright | 1.43.0 | Browser automation |

> **NuGet audit warnings surface on `restore`, not on incremental `build`.** An incremental `dotnet build` can report 0 warnings while `dotnet restore --force` reports dozens of `NU1901`/`NU1902` advisories. To check the zero-warning invariant (#587) honestly, force a restore. Advisory sets drift over time against a pinned version, so this can go red without any code change.
