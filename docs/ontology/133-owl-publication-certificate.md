# #133 — OWL Publication Certificate (publication-safety validation)

**Worker** po-2024 · **Date** 2026-07-12 · **Base** master `b32a4d7b` · **GO** ai-01 dispatch `1f91k0` (secondaire #133 cert). Companion to [`argumentum.owl`](argumentum.owl) + [`README.md`](README.md).

> **Verdict: PUBLICATION-SAFE.** The committed `argumentum.owl` carries all three relational layers (skos Walton, crossLink, AIF attack) resolvable by readers after the OWL2XML round-trip. The known-fail #133 is an **obsolete partial false-negative** — the `skos:inScheme` drop it documents is **resolved** (1408 survive); only `rdf:type` is still dropped at reload, and the survivor-fallback reader already works around it. This does **not** affect the published artefact.

---

## 0. TL;DR

| Question | Answer |
|---|---|
| Does the known-fail #133 affect the **published** artefact? | **No.** The published `.owl` file is consumed as a static artefact (download/IRI-resolve); the OWLSharp round-trip drop only affects **in-process reload** within the .NET validator, which has a survivor-fallback. |
| Do the 3 relational layers survive? | **Yes** — verified by `LoadedOntology_ContainsCrossLinkAndAifAttackAssertions` (test 5, PASS): crossLink 1985 + aifAttackType 145 + aifAttackedNode 146, all > 0 and resolvable after round-trip. |
| Is the OWLSharp bug still present? | **Partially.** `rdf:type` is still dropped at reload (1409 in file → 0 in `AnnotationAxioms`). But `skos:inScheme` now **survives** (1408 in file, 1408 resolvable) — the bug the known-fail asserted is **half-resolved**. |
| Should the known-fail test be updated? | **Yes (out of scope here).** It asserts `inScheme == 0`, which is now false (1408). It is an obsolete false-negative, not a real regression. A follow-up test-refresh PR should split it: keep the `rdf:type` drop assertion, drop the `inScheme == 0` assertion. |

---

## 1. The known-fail #133 — what it documents

[`OwlE2EGenerationValidationTests.LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip`](../../Generation/Converters/Argumentum.AssetConverter.Tests/Ontology/OwlE2EGenerationValidationTests.cs) asserts that OWLSharp's OWL2XML serializer drops `rdf:type` and `skos:inScheme` annotation assertions when the ontology is **reloaded** (parsed back into `OwlAdapter`):

```csharp
rdfTypeCount.Should().Be(0, "OWL2XML round-trip drops rdf:type annotation assertions ...");
inScheme.Should().Be(0,  "skos:inScheme is also absent from the reloaded AnnotationAxioms ...");
```

The read-path fix (`OwlAdapter.GetResourcesByType` / `GetConcepts`) added a **survivor-fallback**: when the `rdf:type` scan is empty, locate concepts by the distinct subjects of `skos:prefLabel` (+ `skos:hasTopConcept` for schemes). This makes the readers functional **despite** the drop.

---

## 2. Current empirical state (committed `argumentum.owl`, master `b32a4d7b`)

```
=== Raw file (docs/ontology/argumentum.owl) ===
skos:inScheme occurrences:  1408   (one per concept)
rdf:type occurrences:       1409   (one per concept + 1 for the ConceptScheme)

=== After OWLSharp round-trip (reloaded AnnotationAxioms) — from test run ===
rdf:type:   0   (STILL DROPPED at reload — OWLSharp limitation persists)
inScheme:   1408 (SURVIVES — the bug the known-fail asserted is RESOLVED for inScheme)
```

**Test run (`dotnet test --filter OwlE2EGenerationValidationTests`, 2026-07-12):**
- **4 PASS / 1 FAIL / 0 SKIP (5 total)**
- FAIL = `LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip` — fails on the `inScheme == 0` assertion (found 1408). The `rdf:type == 0` assertion above it passes (rdf:type still dropped).
- PASS = incl. `LoadedOntology_ContainsCrossLinkAndAifAttackAssertions` (test 5) + `ProdValidator_InspectsConceptsAndGenuinelyPassesOnLoadedOntology` (test 4) + the survivor-fallback reader tests.

---

## 3. Why this does NOT affect the published artefact

1. **The artefact is consumed statically.** Publication = host the `.owl` file at the IRI; consumers (Protege, rdflib, OWL API, LLMs) parse the **raw file**, which has `rdf:type` 1409 + `inScheme` 1408 + all 3 layers. The OWLSharp drop only occurs inside the .NET **validator's reload** — not on external consumption.
2. **The 3 relational layers survive the round-trip** (test 5 PASS): crossLink 1985 + AIF attack 145/146 + skos match 70. These are emitted as annotation/object properties that OWLSharp preserves.
3. **The validator's read-path is fixed** (survivor-fallback): even with `rdf:type == 0` at reload, `GetConcepts` resolves all 1408 concepts via `prefLabel` subjects. Test 4 (`ProdValidator...GenuinelyPasses`) confirms the production validator passes on the loaded ontology.
4. **`skos:inScheme` now survives** (1408) — the most material regression the known-fail flagged is gone. Only `rdf:type` reload-drop remains, and it is purely a .NET-internal concern (worked around, not consumer-facing).

**Conclusion:** the published `argumentum.owl` is self-consistent, carries all 3 layers, and is safe to publish. The known-fail is a **test-hygiene debt**, not a publication blocker.

---

## 4. Three-layer publication inventory (re-confirmed for the cert)

| Layer | Committed file | Survives round-trip? | Reader resolves? |
|---|---:|---|---|
| `rdf:type` (concept typing) | 1409 | partially (0 at reload) | via survivor-fallback ✅ |
| `skos:inScheme` | 1408 | **yes** (1408) ✅ | yes ✅ |
| `skos:prefLabel` / `broader` | 2816 / 1407 | yes | yes ✅ |
| **skos Walton mappings** (broad/close/narrow Match) | 70 | yes (test 3 PASS) | yes ✅ |
| **crossLink** (8 verbs) | 1985 | yes (test 5 PASS) | yes ✅ |
| **AIF attack** (`aifAttackType` + `aifAttackedNode`) | 145 / 146 | yes (test 5 PASS) | yes ✅ |

All three publication-critical layers (skos Walton, crossLink, AIF attack) survive and resolve.

---

## 5. Follow-up recommendation (out of scope — separate PR)

The known-fail test `LoadedOntology_RdfTypeAndInScheme_DroppedByOwl2XmlRoundTrip` should be **refreshed** to match current reality:

- keep the `rdf:type == 0` assertion (the OWLSharp limitation is real for rdf:type);
- **drop or invert** the `inScheme == 0` assertion (inScheme now survives at 1408 — assert `> 0` instead, documenting the partial resolution);
- this would clear the last red on the test suite (moving 595 pass / 1 fail → 596 pass / 0 fail) and remove the misleading "known-fail" tag from the dashboard/test count.

This is a **test-hygiene change**, gated ai-01, separate from the publication cert. Flagged here so the follow-up is scoped, not forgotten.

---

## 6. Gate boundaries

- ✅ Analysis only — **0 write prod CSV**, `Cards/` untouched, **0 test change** (the refresh is flagged out-of-scope in §5).
- ✅ Test run on the committed `argumentum.owl` (empirical, code=truth).
- ✅ Verdict scoped to publication-safety of the **artefact** (not the .NET validator's internal state).
- ❌ No test modified in this PR — the known-fail refresh is a separate gated change.

🤖 Worker po-2024 — #133 OWL publication certificate, publication-safe verdict, known-fail characterized as obsolete partial false-negative.
