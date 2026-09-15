using Argumentum.AssetConverter.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using Xunit;

namespace Argumentum.AssetConverter.Tests.Parsing
{
    /// <summary>
    /// Strict-contract pin for the production CSV load path
    /// <see cref="CsvBase{T,TMap}.LoadFromContent"/> (CsvHelper 31.0.4). The
    /// <c>CsvConfiguration</c> built there sets exactly two callbacks:
    /// <list type="bullet">
    /// <item><c>PrepareHeaderForMatch</c> — strips diacritics/underscore/hyphen/space and
    /// lowercases header names (covered for real headers by the Scenario test in
    /// <c>EntityClassMapRegressionTests</c>; not re-tested here).</item>
    /// <item><c>MissingFieldFound</c> — a non-fatal <c>Logger</c> callback: a data row shorter
    /// than the header is logged, not thrown.</item>
    /// </list>
    /// It deliberately does NOT set <c>HeaderValidated</c>, so CsvHelper's default is in effect:
    /// a mapped column that is NOT <c>.Optional()</c> and is absent from the header throws
    /// <see cref="HeaderValidationException"/>.
    ///
    /// The entity ClassMap regression suites (#476 <c>EntityClassMapRegressionTests</c>,
    /// #485 <c>FallacyClassMapRegressionTests</c>) pin only ONE direction of this contract:
    /// "Optional column absent → does not throw". The inverse half — "non-Optional column
    /// absent → throws" — was never asserted. That inverse is the strict guard that would catch
    /// a dropped <c>.Optional()</c> on a currently-required column (the #216/#477 localization
    /// fragility class) at load time instead of silently zeroing the field. This file fixes the
    /// bidirectional contract in one self-contained place, decoupled from any domain ClassMap
    /// that evolves, via a minimal synthetic entity.
    ///
    /// Additive only: no production code or existing test is modified. Dispatch #204 secondaire.
    /// </summary>
    public class CsvBaseStrictContractTests
    {
        /// <summary>
        /// Minimal synthetic entity exercising the REAL <see cref="CsvBase{T,TMap}"/>
        /// configuration. One required column (<see cref="RequiredEntity"/>) + one
        /// <c>.Optional()</c> column is the smallest setup that discriminates the two halves of
        /// the strict contract. Declared against the real base so the actual
        /// <c>CsvConfiguration</c> (Logger <c>MissingFieldFound</c> + default
        /// <c>HeaderValidated</c>) is exercised, not a parallel copy.
        /// </summary>
        public class ContractEntity : CsvBase<ContractEntity, ContractEntityMap>
        {
            public string Required { get; set; } = null!;
            public string Optional { get; set; } = null!;
        }

        public sealed class ContractEntityMap : ClassMap<ContractEntity>
        {
            public ContractEntityMap()
            {
                Map(m => m.Required).Name("required");
                Map(m => m.Optional).Name("optional").Optional();
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (1) THE headline guard — a non-Optional column absent from the header THROWS.
        //     CsvBase does not override HeaderValidated, so CsvHelper's default
        //     HeaderValidationException fires. A dropped .Optional() on a currently-required
        //     column would surface here instead of silently zeroing the field (the #216/#477
        //     regression class). This is the contract half that was never asserted before.
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact]
        public void LoadFromContent_Throws_WhenRequiredColumnAbsentFromHeader()
        {
            // Header omits "required" (non-Optional). Only "optional" is present.
            var csv = "optional\nopt-value\n";

            var act = () => ContractEntity.LoadFromContent(csv);

            act.Should().Throw<HeaderValidationException>(
                "CsvBase does not override HeaderValidated; CsvHelper's default throws when a " +
                "mapped non-Optional member is missing from the header — the strict guard against " +
                "a dropped .Optional() silently zeroing a required field (#216/#477 fragility class)");
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (2) Contrast half — an Optional column absent from the header does NOT throw, and
        //     the Optional property stays null. Together with (1) this fixes the bidirectional
        //     strict contract in one self-contained place.
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact]
        public void LoadFromContent_DoesNotThrow_WhenOnlyOptionalColumnAbsentFromHeader()
        {
            // Header omits "optional" (.Optional). Only "required" is present.
            var csv = "required\nreq-value\n";

            var act = () => ContractEntity.LoadFromContent(csv);

            act.Should().NotThrow(
                ".Optional() members are explicitly allowed to be absent from the header; only " +
                "non-Optional members trigger HeaderValidated");
            var entity = act().Should().Subject.Single();
            entity.Required.Should().Be("req-value");
            entity.Optional.Should().BeNull();
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // (3) MissingFieldFound is non-fatal — a data row shorter than the header is logged,
        //     not thrown, and the record still loads. CsvBase overrides CsvHelper's default
        //     MissingFieldFound (which would throw MissingFieldException) with a Logger callback;
        //     this pins that the load tolerates short rows.
        //
        //     Subtle characterization detail the test pins: the missing field on a short row
        //     resolves to EMPTY STRING ("") — NOT null. This differs from (2), where an
        //     Optional column absent from the header yields null (the property is never bound).
        //     A field that IS bound to a header slot but absent from a given row body is "".
        // ─────────────────────────────────────────────────────────────────────────────
        [Fact]
        public void LoadFromContent_DoesNotThrow_OnShortDataRow_MissingFieldIsLoggedNotThrown()
        {
            // Header has two columns; the data row has only one → the second field is missing.
            var csv = "required,optional\nreq-value\n";

            var act = () => ContractEntity.LoadFromContent(csv);

            act.Should().NotThrow(
                "CsvBase sets MissingFieldFound to a non-fatal Logger callback, so a short data " +
                "row is logged and the record still loads rather than throwing MissingFieldException");
            var entity = act().Should().Subject.Single();
            entity.Required.Should().Be("req-value");
            // A bound-but-absent field on a short row resolves to "" (empty string), NOT null.
            // Distinct from the Optional-absent-from-header case in (2) which yields null.
            entity.Optional.Should().BeEmpty(
                "a field bound to a header slot but missing from a short data row resolves to " +
                "empty string, not null — unlike an Optional column absent from the header");
        }
    }
}
