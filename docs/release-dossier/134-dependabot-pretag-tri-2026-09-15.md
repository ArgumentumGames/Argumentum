# #134 — Tri Dependabot pré-tag : 5 PR ouvertes, 2 à défermer, 0 licence-pinned

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base mesure** : master `c089d526`
**Instrument** : `gh pr view/diff` par PR (body + files + diff lus intégralement) + `dotnet restore --force`.
**Statut** : **TRI (lecture seule)** — ⛔ `0` merge, ⛔ `0` fermeture, ⛔ `0` édition de PR.
**Dispatch** : pool #458 c.5666260217 (grain ⑥, actualisé 15/09) — « hors binaire / touche binaire / licence-pinned ; forced restore ; aucune PR Dependabot mergée ».

---

## §0 Le résultat en une phrase

**5 PR Dependabot ouvertes, aucune mergée depuis la base du bundle 12/09** (dernière merge = #1301,
07/09, antérieure à `65dd4742`) ; le tri donne **2 à défermer post-tag** (#1371 touche le binaire,
#1370 touche l'instrument de QA), **3 mergeables sans effet release**, et **0 licence-pinned en file** ;
le restore forcé de master est **propre (0 advisory)** — rien ne presse.

## §1 L'état mesuré (15/09)

| PR | ouverte | périmètre | classe |
|---|---|---|---|
| #1371 | 14/09 | `System.Management` 10.0.11→**10.0.12** dans `Argumentum.AssetConverter.csproj` + `Microsoft.NET.Test.Sdk` 18.9.0→**18.10.0** (Tests + VisualTests) | **TOUCHE LE BINAIRE** |
| #1370 | 14/09 | `playwright` npm 1.62.1→**1.63.0** dans `docs/investigations/pdf-validation` | hors binaire, **instrument de QA** |
| #1328 | 11/09 | `svgo` 2.8.3→2.8.4 — DNN skin « Bootstrap 4 Instant » | hors binaire, hors livrable |
| #1323 | 10/09 | `baseline-browser-mapping` 2.10.27→2.11.22 — DNN 2sxc `ImageCompare2` | hors binaire, hors livrable |
| #1322 | 10/09 | `baseline-browser-mapping` 2.10.12→2.11.22 — DNN 2sxc `QrCode2` | hors binaire, hors livrable |

**File licence-pinned : vide.** Aucune re-proposition AutoMapper 15.x ouverte (la file périodique
#887 est saine au 15/09) ; les 2 bumps .NET de #1371 sont MIT et ne touchent aucun pin
(AutoMapper 14.0.0 et QuestPDF 2022.12.12 intacts — CLAUDE.md « Stable Dependency Versions »).

## §2 Verdicts pré-tag (recommandations — la décision est à ai-01/owner)

| PR | verdict | rationale |
|---|---|---|
| **#1371** | **DÉFERMER jusqu'au post-tag** | `System.Management` vit dans le csproj du **binaire du pipeline** (MESURÉ : hunk sur `Argumentum.AssetConverter.csproj`) — patch de servicing MIT sans urgence (§3) ; la PR est **atomique** (groupe dotnet), le Test.Sdk ne peut pas merger sans lui. Le binaire est la base des régéns de la fenêtre release : il ne bouge pas pendant le gel. |
| **#1370** | **DÉFERMER jusqu'au post-tag** | Hors binaire, mais c'est le **playwright de `pdf-validation`** = l'instrument qui mesure les PDF livrés. Bumper l'instrument pendant la fenêtre de validation #134, c'est risquer une dérive de mesure entre deux runs de QA. |
| #1328 · #1323 · #1322 | **Mergeable, aucune urgence** | npm dev-deps du périmètre DNN (#131/#132, non déployé) — aucun effet sur le livrable ou la chaîne de mesure. À merger à la convenance d'ai-01, avant ou après le tag. |

## §3 Le restore forcé (MESURÉ sur `c089d526`)

`dotnet restore --force "Argumentum Converters.sln"` : **3/3 projets restaurés** (AssetConverter,
Tests, VisualTests), **0 `NU1901`/`NU1902`/warning**, exit 0. L'invariant zero-warning #587 tient au
niveau restore — les advisories ne remontent que sur restore forcé (jamais sur build incrémental),
c'est donc la seule mesure honnête. **Conséquence : aucun advisory ne presse un bump pré-tag** —
les deux défermages ci-dessus ne coûtent rien en exposition sécurité.

## §4 Ce que ce tri n'établit pas

⛔ Aucun merge, fermeture ou commentaire posté sur les 5 PR · ⛔ ne préjuge pas du moment post-tag
(#654/#965/#1123 restent HOLD post-tag) · ⛔ n'audite pas les locks npm au-delà des 5 PR ouvertes ·
⛔ les verdicts sont des recommandations, pas des actions — la file peut bouger entre ce dossier et
le tag (Dependabot rouvre périodiquement ; re-trier au moment du tag si la fenêtre dure).

---

*po-2024 — pool #458, grain ⑥. Le worker trie et mesure ; merge et fermeture sont à ai-01/owner.*
