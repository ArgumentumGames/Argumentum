#!/usr/bin/env pwsh
# Translation Consistency Audit — #192 Étape 1
# Read-only analysis: groups source FR terms and flags inconsistent translations across 8 languages.
# Output: Markdown report with worklist of cells to re-translate.

param(
    [string]$OutputDir = "."
)

Set-StrictMode -Version 3.0
$ErrorActionPreference = "Stop"

# ── CSV helpers ──────────────────────────────────────────────────────────────

function Import-CsvSafe {
    param([string]$Path)
    $content = [System.IO.File]::ReadAllText($Path, [System.Text.UTF8Encoding]::new($false))
    # Handle BOM
    if ($content.StartsWith([char]0xFEFF)) { $content = $content.Substring(1) }
    # Use ConvertFrom-Csv which accepts pipeline/InputObject
    $csv = $content | ConvertFrom-Csv
    return $csv
}

# ── Consistency checker ─────────────────────────────────────────────────────

function Find-InconsistentTranslations {
    param(
        [array]$Records,
        [string]$SourceColumn,
        [string[]]$TranslationColumns,
        [string]$Dataset,
        [string]$IdColumn = "PK"
    )

    $inconsistencies = @()
    $totalTerms = 0
    $uniqueTerms = 0

    # Group by source term
    $groups = $Records | Where-Object { $_.$SourceColumn -and $_.$SourceColumn.Trim() -ne "" } |
        Group-Object -Property { $_.$SourceColumn.Trim() }

    $uniqueTerms = $groups.Count

    foreach ($group in $groups) {
        $sourceTerm = $group.Name
        $totalTerms++

        foreach ($transCol in $TranslationColumns) {
            # Collect all unique translations for this source term in this column
            $translations = @($group.Group |
                Where-Object { $_.$transCol -and $_.$transCol.Trim() -ne "" } |
                ForEach-Object { $_.$transCol.Trim() } |
                Select-Object -Unique)

            if ($translations.Count -gt 1) {
                # Extract language from column suffix
                $lang = if ($transCol -match '_([a-z]{2})$') { $Matches[1] }
                        elseif ($transCol -match '^(\w+)$' -and $transCol -notmatch '_') {
                            # Bare EN columns (category, title, etc.)
                            "en"
                        } else { "unknown" }

                # Get IDs of records with each variant
                $variants = @()
                foreach ($trans in $translations) {
                    $ids = ($group.Group |
                        Where-Object { $_.$transCol -and $_.$transCol.Trim() -eq $trans } |
                        ForEach-Object { $_.$IdColumn }) -join ", "
                    $variants += @{
                        Translation = $trans
                        Count = ($group.Group | Where-Object { $_.$transCol -and $_.$transCol.Trim() -eq $trans }).Count
                        IDs = $ids
                    }
                }

                $inconsistencies += [PSCustomObject]@{
                    Dataset = $Dataset
                    SourceTerm = $sourceTerm
                    Column = $transCol
                    Language = $lang
                    VariantCount = $translations.Count
                    Variants = $variants
                }
            }
        }
    }

    return @{
        Inconsistencies = $inconsistencies
        TotalTerms = $totalTerms
        UniqueTerms = $uniqueTerms
    }
}

# ── Main analysis ────────────────────────────────────────────────────────────

Write-Host "=== Translation Consistency Audit (#192 Étape 1) ===" -ForegroundColor Cyan
Write-Host ""

$report = [System.Text.StringBuilder]::new()
[void]$report.AppendLine("# Translation Consistency Audit — #192 Étape 1")
[void]$report.AppendLine("")
[void]$report.AppendLine("> **Date**: $(Get-Date -Format 'yyyy-MM-dd HH:mm')")
[void]$report.AppendLine("> **Scope**: Read-only terminological consistency analysis")
[void]$report.AppendLine("> **Datasets**: Fallacies, Scenarii, Virtues, Rules (8 languages)")
[void]$report.AppendLine("")

# ── 1. FALLACIES ─────────────────────────────────────────────────────────────

Write-Host "Analyzing Fallacies..." -ForegroundColor Yellow
$fallaciesPath = "Cards\Fallacies\Argumentum Fallacies - Taxonomy.csv"
$fallacies = Import-CsvSafe $fallaciesPath
[void]$report.AppendLine("## 1. Fallacies — Taxonomy")
[void]$report.AppendLine("")
[void]$report.AppendLine("Records: $($fallacies.Count) | Columns: Fallacies")
[void]$report.AppendLine("")

# Family columns
$fallaciesResult = Find-InconsistentTranslations -Records $fallacies `
    -SourceColumn "Famille" `
    -TranslationColumns @("Family", "Family_ru", "Family_pt", "Family_ar", "Family_es", "Family_zh", "Family_fa") `
    -Dataset "Fallacies" -IdColumn "PK"

# Sub-family columns
$fallaciesSubResult = Find-InconsistentTranslations -Records $fallacies `
    -SourceColumn "Sous-Famille" `
    -TranslationColumns @("Subfamily", "Subfamily_ru", "Subfamily_pt", "Subfamily_ar", "Subfamily_es", "Subfamily_zh", "Subfamily_fa") `
    -Dataset "Fallacies" -IdColumn "PK"

# Sub-sub-family columns
$fallaciesSubSubResult = Find-InconsistentTranslations -Records $fallacies `
    -SourceColumn "Soussousfamille" `
    -TranslationColumns @("Subsubfamily", "Subsubfamily_ru", "Subsubfamily_pt", "Subsubfamily_ar", "Subsubfamily_es", "Subsubfamily_zh", "Subsubfamily_fa") `
    -Dataset "Fallacies" -IdColumn "PK"

$allFallacies = @($fallaciesResult.Inconsistencies) + @($fallaciesSubResult.Inconsistencies) + @($fallaciesSubSubResult.Inconsistencies)

[void]$report.AppendLine("### Family taxonomy (Famille / Sous-Famille / Soussousfamille)")
[void]$report.AppendLine("")
[void]$report.AppendLine("Unique FR families: $($fallaciesResult.UniqueTerms) | Inconsistencies: $($fallaciesResult.Inconsistencies.Count)")
[void]$report.AppendLine("Unique FR sub-families: $($fallaciesSubResult.UniqueTerms) | Inconsistencies: $($fallaciesSubResult.Inconsistencies.Count)")
[void]$report.AppendLine("Unique FR sub-sub-families: $($fallaciesSubSubResult.UniqueTerms) | Inconsistencies: $($fallaciesSubSubResult.Inconsistencies.Count)")
[void]$report.AppendLine("")

if ($allFallacies.Count -gt 0) {
    [void]$report.AppendLine("| # | Source (FR) | Column | Lang | Variants |")
    [void]$report.AppendLine("|---|-------------|--------|------|----------|")
    $i = 0
    foreach ($inc in ($allFallacies | Sort-Object SourceTerm, Column)) {
        $i++
        $variantStr = ($inc.Variants | ForEach-Object {
            "$($_.Translation) ($($_.Count)x)"
        }) -join " / "
        # Truncate long variant strings
        if ($variantStr.Length -gt 120) {
            $variantStr = $variantStr.Substring(0, 117) + "..."
        }
        [void]$report.AppendLine("| $i | $($inc.SourceTerm) | $($inc.Column) | $($inc.Language) | $variantStr |")
    }
    [void]$report.AppendLine("")
}

Write-Host "  Fallacies: $($allFallacies.Count) inconsistencies found" -ForegroundColor $(if ($allFallacies.Count -gt 0) {"Yellow"} else {"Green"})

# ── 2. SCENARII ──────────────────────────────────────────────────────────────

Write-Host "Analyzing Scenarii..." -ForegroundColor Yellow
$scenariiPath = "Cards\Scenarii\Argumentum Scenarii - Cards.csv"
$scenarii = Import-CsvSafe $scenariiPath
[void]$report.AppendLine("## 2. Scenarii — Cards")
[void]$report.AppendLine("")
[void]$report.AppendLine("Records: $($scenarii.Count)")
[void]$report.AppendLine("")

# Category consistency
$scenariiCatResult = Find-InconsistentTranslations -Records $scenarii `
    -SourceColumn "catégorie" `
    -TranslationColumns @("category", "category_ru", "category_pt", "category_ar", "category_es", "category_zh", "category_fa") `
    -Dataset "Scenarii" -IdColumn "path"

# Sub-category consistency
$scenariiSubCatResult = Find-InconsistentTranslations -Records $scenarii `
    -SourceColumn "sous-catégorie" `
    -TranslationColumns @("subcategory", "subcategory_ru", "subcategory_pt", "subcategory_ar", "subcategory_es", "subcategory_zh", "subcategory_fa") `
    -Dataset "Scenarii" -IdColumn "path"

$allScenarii = @($scenariiCatResult.Inconsistencies) + @($scenariiSubCatResult.Inconsistencies)

[void]$report.AppendLine("### Category taxonomy (catégorie / sous-catégorie)")
[void]$report.AppendLine("")
[void]$report.AppendLine("Unique FR categories: $($scenariiCatResult.UniqueTerms) | Inconsistencies: $($scenariiCatResult.Inconsistencies.Count)")
[void]$report.AppendLine("Unique FR sub-categories: $($scenariiSubCatResult.UniqueTerms) | Inconsistencies: $($scenariiSubCatResult.Inconsistencies.Count)")
[void]$report.AppendLine("")

if ($allScenarii.Count -gt 0) {
    [void]$report.AppendLine("| # | Source (FR) | Column | Lang | Variants |")
    [void]$report.AppendLine("|---|-------------|--------|------|----------|")
    $i = 0
    foreach ($inc in ($allScenarii | Sort-Object SourceTerm, Column)) {
        $i++
        $variantStr = ($inc.Variants | ForEach-Object {
            "$($_.Translation) ($($_.Count)x)"
        }) -join " / "
        if ($variantStr.Length -gt 120) {
            $variantStr = $variantStr.Substring(0, 117) + "..."
        }
        [void]$report.AppendLine("| $i | $($inc.SourceTerm) | $($inc.Column) | $($inc.Language) | $variantStr |")
    }
    [void]$report.AppendLine("")
}

Write-Host "  Scenarii: $($allScenarii.Count) inconsistencies found" -ForegroundColor $(if ($allScenarii.Count -gt 0) {"Yellow"} else {"Green"})

# ── 3. VIRTUES ───────────────────────────────────────────────────────────────

Write-Host "Analyzing Virtues..." -ForegroundColor Yellow
$virtuesPath = "Cards\Fallacies\Argumentum Virtues - Taxonomy.csv"
$virtues = Import-CsvSafe $virtuesPath
[void]$report.AppendLine("## 3. Virtues — Taxonomy")
[void]$report.AppendLine("")
[void]$report.AppendLine("Records: $($virtues.Count)")
[void]$report.AppendLine("")

$virtuesFamResult = Find-InconsistentTranslations -Records $virtues `
    -SourceColumn "family_fr" `
    -TranslationColumns @("family_en", "family_ru", "family_pt", "family_ar", "family_es", "family_zh", "family_fa") `
    -Dataset "Virtues" -IdColumn "pk"

$virtuesSubResult = Find-InconsistentTranslations -Records $virtues `
    -SourceColumn "subfamily_fr" `
    -TranslationColumns @("subfamily_en", "subfamily_ru", "subfamily_pt", "subfamily_ar", "subfamily_es", "subfamily_zh", "subfamily_fa") `
    -Dataset "Virtues" -IdColumn "pk"

$virtuesSubSubResult = Find-InconsistentTranslations -Records $virtues `
    -SourceColumn "subsubfamily_fr" `
    -TranslationColumns @("subsubfamily_en", "subsubfamily_ru", "subsubfamily_pt", "subsubfamily_ar", "subsubfamily_es", "subsubfamily_zh", "subsubfamily_fa") `
    -Dataset "Virtues" -IdColumn "pk"

$allVirtues = @($virtuesFamResult.Inconsistencies) + @($virtuesSubResult.Inconsistencies) + @($virtuesSubSubResult.Inconsistencies)

[void]$report.AppendLine("### Family taxonomy (family_fr / subfamily_fr / subsubfamily_fr)")
[void]$report.AppendLine("")
[void]$report.AppendLine("Unique FR families: $($virtuesFamResult.UniqueTerms) | Inconsistencies: $($virtuesFamResult.Inconsistencies.Count)")
[void]$report.AppendLine("Unique FR sub-families: $($virtuesSubResult.UniqueTerms) | Inconsistencies: $($virtuesSubResult.Inconsistencies.Count)")
[void]$report.AppendLine("Unique FR sub-sub-families: $($virtuesSubSubResult.UniqueTerms) | Inconsistencies: $($virtuesSubSubResult.Inconsistencies.Count)")
[void]$report.AppendLine("")

if ($allVirtues.Count -gt 0) {
    [void]$report.AppendLine("| # | Source (FR) | Column | Lang | Variants |")
    [void]$report.AppendLine("|---|-------------|--------|------|----------|")
    $i = 0
    foreach ($inc in ($allVirtues | Sort-Object SourceTerm, Column)) {
        $i++
        $variantStr = ($inc.Variants | ForEach-Object {
            "$($_.Translation) ($($_.Count)x)"
        }) -join " / "
        if ($variantStr.Length -gt 120) {
            $variantStr = $variantStr.Substring(0, 117) + "..."
        }
        [void]$report.AppendLine("| $i | $($inc.SourceTerm) | $($inc.Column) | $($inc.Language) | $variantStr |")
    }
    [void]$report.AppendLine("")
}

Write-Host "  Virtues: $($allVirtues.Count) inconsistencies found" -ForegroundColor $(if ($allVirtues.Count -gt 0) {"Yellow"} else {"Green"})

# ── 4. RULES ─────────────────────────────────────────────────────────────────

Write-Host "Analyzing Rules..." -ForegroundColor Yellow
$rulesPath = "Cards\Rules\Argumentum Rules - Cards.csv"
$rules = Import-CsvSafe $rulesPath
[void]$report.AppendLine("## 4. Rules — Cards")
[void]$report.AppendLine("")
[void]$report.AppendLine("Records: $($rules.Count)")
[void]$report.AppendLine("")
[void]$report.AppendLine("> Rules contain Markdown text blocks, not taxonomy labels. Consistency analysis focuses on heading structure and recurring terms across languages.")
[void]$report.AppendLine("")

# For Rules, check heading consistency (extract ## headings from Text columns)
$headingInconsistencies = @()
$headingMap = @{}

foreach ($rule in $rules) {
    $pk = $rule.pk
    if (-not $pk) { continue }

    foreach ($lang in @("", "_en", "_ru", "_pt", "_ar", "_es", "_zh", "_fa")) {
        $col = "Text$lang"
        $text = $rule.$col
        if (-not $text) { continue }

        # Extract ## headings
        $headings = ($text -split "`n" | Where-Object { $_ -match "^#{1,3}\s+" }) -join " | "
        if ($headings) {
            if (-not $headingMap.ContainsKey($pk)) {
                $headingMap[$pk] = @{}
            }
            $langLabel = if ($lang -eq "") { "fr" } else { $lang.Substring(1) }
            $headingMap[$pk][$langLabel] = $headings
        }
    }
}

# Check: same PK should have same number of headings across languages
[void]$report.AppendLine("### Heading structure comparison")
[void]$report.AppendLine("")

$headingIssues = @()
foreach ($pk in ($headingMap.Keys | Sort-Object)) {
    $langs = $headingMap[$pk]
    $headingCounts = @($langs.GetEnumerator() | ForEach-Object {
        $count = @($_.Value -split "\|" | Where-Object { $_ -match "\S" }).Count
        @{ Lang = $_.Key; Count = $count; Headings = $_.Value }
    })

    $counts = @(($headingCounts | ForEach-Object { $_.Count }) | Select-Object -Unique)
    if ($counts.Count -gt 1) {
        $detail = ($headingCounts | ForEach-Object { "$($_.Lang)=$($_.Count)" }) -join ", "
        $headingIssues += [PSCustomObject]@{
            PK = $pk
            Detail = $detail
        }
    }
}

if ($headingIssues.Count -gt 0) {
    [void]$report.AppendLine("**Heading count mismatches found:** $($headingIssues.Count)")
    [void]$report.AppendLine("")
    [void]$report.AppendLine("| PK | Heading counts per language |")
    [void]$report.AppendLine("|----|----------------------------|")
    foreach ($issue in $headingIssues) {
        [void]$report.AppendLine("| $($issue.PK) | $($issue.Detail) |")
    }
    [void]$report.AppendLine("")
} else {
    [void]$report.AppendLine("All Rules have consistent heading counts across 8 languages. ✅")
    [void]$report.AppendLine("")
}

Write-Host "  Rules: $($headingIssues.Count) heading mismatches found" -ForegroundColor $(if ($headingIssues.Count -gt 0) {"Yellow"} else {"Green"})

# ── 5. CROSS-DATASET ROLE TERMS (Scenarii) ──────────────────────────────────

Write-Host "Analyzing role terms..." -ForegroundColor Yellow
[void]$report.AppendLine("## 5. Cross-dataset Role Terms (Scenarii)")
[void]$report.AppendLine("")
[void]$report.AppendLine("> The Scenarii roles `baratineur` and `piocheur` appear in all 167 records. Each should have exactly ONE canonical translation per language.")
[void]$report.AppendLine("")

# Check baratineur consistency
$baratineurVariants = @{}
foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
    $col = switch ($lang) {
        "en" { "smoothTalker" }
        default { "smoothTalker_$lang" }
    }
    $variants = @($scenarii | Where-Object { $_.$col -and $_.$col.Trim() -ne "" } |
        ForEach-Object { $_.$col.Trim() } | Select-Object -Unique)
    $baratineurVariants[$lang] = $variants
}

# Check piocheur consistency
$piocheurVariants = @{}
foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
    $col = switch ($lang) {
        "en" { "drawer" }
        default { "drawer_$lang" }
    }
    $variants = @($scenarii | Where-Object { $_.$col -and $_.$col.Trim() -ne "" } |
        ForEach-Object { $_.$col.Trim() } | Select-Object -Unique)
    $piocheurVariants[$lang] = $variants
}

[void]$report.AppendLine("### `baratineur` translations")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Language | Unique translations | Consistent? |")
[void]$report.AppendLine("|----------|-------------------:|:-----------:|")
$baratineurIssues = 0
foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
    $variants = $baratineurVariants[$lang]
    $ok = ($variants.Count -le 1)
    if (-not $ok) { $baratineurIssues++ }
    $variantStr = ($variants -join ", ")
    if ($variantStr.Length -gt 100) { $variantStr = $variantStr.Substring(0, 97) + "..." }
    [void]$report.AppendLine("| $lang | $($variants.Count) | $(if ($ok) {'✅'} else {'⚠️'}) |")
}
[void]$report.AppendLine("")

# Show baratineur detail if inconsistent
if ($baratineurIssues -gt 0) {
    [void]$report.AppendLine("<details><summary>Variant details</summary>")
    [void]$report.AppendLine("")
    foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
        $variants = $baratineurVariants[$lang]
        if ($variants.Count -gt 1) {
            [void]$report.AppendLine("**$lang**:")
            foreach ($v in $variants) {
                $count = ($scenarii | Where-Object {
                    $col = switch ($lang) { "en" { "smoothTalker" } default { "smoothTalker_$lang" } }
                    $_.$col -and $_.$col.Trim() -eq $v
                }).Count
                [void]$report.AppendLine("- ``$v`` ($count×)")
            }
            [void]$report.AppendLine("")
        }
    }
    [void]$report.AppendLine("</details>")
    [void]$report.AppendLine("")
}

[void]$report.AppendLine("### `piocheur` translations")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Language | Unique translations | Consistent? |")
[void]$report.AppendLine("|----------|-------------------:|:-----------:|")
$piocheurIssues = 0
foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
    $variants = $piocheurVariants[$lang]
    $ok = ($variants.Count -le 1)
    if (-not $ok) { $piocheurIssues++ }
    $variantStr = ($variants -join ", ")
    if ($variantStr.Length -gt 100) { $variantStr = $variantStr.Substring(0, 97) + "..." }
    [void]$report.AppendLine("| $lang | $($variants.Count) | $(if ($ok) {'✅'} else {'⚠️'}) |")
}
[void]$report.AppendLine("")

if ($piocheurIssues -gt 0) {
    [void]$report.AppendLine("<details><summary>Variant details</summary>")
    [void]$report.AppendLine("")
    foreach ($lang in @("en", "ru", "pt", "ar", "es", "zh", "fa")) {
        $variants = $piocheurVariants[$lang]
        if ($variants.Count -gt 1) {
            [void]$report.AppendLine("**$lang**:")
            foreach ($v in $variants) {
                $count = ($scenarii | Where-Object {
                    $col = switch ($lang) { "en" { "drawer" } default { "drawer_$lang" } }
                    $_.$col -and $_.$col.Trim() -eq $v
                }).Count
                [void]$report.AppendLine("- ``$v`` ($count×)")
            }
            [void]$report.AppendLine("")
        }
    }
    [void]$report.AppendLine("</details>")
    [void]$report.AppendLine("")
}

Write-Host "  baratineur: $baratineurIssues languages with inconsistencies" -ForegroundColor $(if ($baratineurIssues -gt 0) {"Yellow"} else {"Green"})
Write-Host "  piocheur: $piocheurIssues languages with inconsistencies" -ForegroundColor $(if ($piocheurIssues -gt 0) {"Yellow"} else {"Green"})

# ── 6. COVERAGE REPORT ──────────────────────────────────────────────────────

Write-Host "Computing coverage..." -ForegroundColor Yellow
[void]$report.AppendLine("## 6. Translation Coverage Report")
[void]$report.AppendLine("")

$coverageData = @()

# Fallacies coverage
$fallaciesContentCols = @{
    "desc_fr" = @("desc_en", "desc_ru", "desc_pt", "desc_ar", "desc_es", "desc_zh", "desc_fa")
    "example_fr" = @("example_en", "example_ru", "example_pt", "example_ar", "example_es", "example_zh", "example_fa")
    "text_fr" = @("text_en", "text_ru", "text_pt", "text_ar", "text_es", "text_zh", "text_fa")
}

[void]$report.AppendLine("### Fallacies — Content field coverage")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Field | FR filled | EN | RU | PT | AR | ES | ZH | FA |")
[void]$report.AppendLine("|-------|----------:|---:|---:|---:|---:|---:|---:|---:|")

foreach ($frCol in $fallaciesContentCols.Keys) {
    $transCols = $fallaciesContentCols[$frCol]
    $frFilled = ($fallacies | Where-Object { $_.$frCol -and $_.$frCol.Trim() -ne "" }).Count

    $langCounts = @()
    foreach ($transCol in $transCols) {
        $count = ($fallacies | Where-Object {
            $_.$frCol -and $_.$frCol.Trim() -ne "" -and
            $_.$transCol -and $_.$transCol.Trim() -ne ""
        }).Count
        $langCounts += $count
    }
    $langStr = $langCounts -join " | "
    [void]$report.AppendLine("| $frCol | $frFilled | $langStr |")
}
[void]$report.AppendLine("")

# Scenarii coverage
$scenariiContentCols = @{
    "contexte" = @("context", "context_ru", "context_pt", "context_ar", "context_es", "context_zh", "context_fa")
    "enjeu" = @("issue", "issue_ru", "issue_pt", "issue_ar", "issue_es", "issue_zh", "issue_fa")
    "baratineur" = @("smoothTalker", "smoothTalker_ru", "smoothTalker_pt", "smoothTalker_ar", "smoothTalker_es", "smoothTalker_zh", "smoothTalker_fa")
    "piocheur" = @("drawer", "drawer_ru", "drawer_pt", "drawer_ar", "drawer_es", "drawer_zh", "drawer_fa")
    "suggestion" = @("suggestion_en", "suggestion_ru", "suggestion_pt", "suggestion_ar", "suggestion_es", "suggestion_zh", "suggestion_fa")
}

[void]$report.AppendLine("### Scenarii — Content field coverage")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Field | FR filled | EN | RU | PT | AR | ES | ZH | FA |")
[void]$report.AppendLine("|-------|----------:|---:|---:|---:|---:|---:|---:|---:|")

foreach ($frCol in $scenariiContentCols.Keys) {
    $transCols = $scenariiContentCols[$frCol]
    $frFilled = ($scenarii | Where-Object { $_.$frCol -and $_.$frCol.Trim() -ne "" }).Count

    $langCounts = @()
    foreach ($transCol in $transCols) {
        $count = ($scenarii | Where-Object {
            $_.$frCol -and $_.$frCol.Trim() -ne "" -and
            $_.$transCol -and $_.$transCol.Trim() -ne ""
        }).Count
        $langCounts += $count
    }
    $langStr = $langCounts -join " | "
    [void]$report.AppendLine("| $frCol | $frFilled | $langStr |")
}
[void]$report.AppendLine("")

# Virtues coverage
$virtuesContentCols = @{
    "title_fr" = @("title_en", "title_ru", "title_pt", "title_ar", "title_es", "title_zh", "title_fa")
    "description_fr" = @("description_en", "description_ru", "description_pt", "description_ar", "description_es", "description_zh", "description_fa")
    "remark_fr" = @("remark_en", "remark_ru", "remark_pt", "remark_ar", "remark_es", "remark_zh", "remark_fa")
}

[void]$report.AppendLine("### Virtues — Content field coverage")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Field | FR filled | EN | RU | PT | AR | ES | ZH | FA |")
[void]$report.AppendLine("|-------|----------:|---:|---:|---:|---:|---:|---:|---:|")

foreach ($frCol in $virtuesContentCols.Keys) {
    $transCols = $virtuesContentCols[$frCol]
    $frFilled = ($virtues | Where-Object { $_.$frCol -and $_.$frCol.Trim() -ne "" }).Count

    $langCounts = @()
    foreach ($transCol in $transCols) {
        $count = ($virtues | Where-Object {
            $_.$frCol -and $_.$frCol.Trim() -ne "" -and
            $_.$transCol -and $_.$transCol.Trim() -ne ""
        }).Count
        $langCounts += $count
    }
    $langStr = $langCounts -join " | "
    [void]$report.AppendLine("| $frCol | $frFilled | $langStr |")
}
[void]$report.AppendLine("")

# Rules coverage
[void]$report.AppendLine("### Rules — Content field coverage")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Field | FR filled | EN | RU | PT | AR | ES | ZH | FA |")
[void]$report.AppendLine("|-------|----------:|---:|---:|---:|---:|---:|---:|---:|")

$rulesContentCols = @{
    "Text" = @("Text_en", "Text_ru", "Text_pt", "Text_ar", "Text_es", "Text_zh", "Text_fa")
}

foreach ($frCol in $rulesContentCols.Keys) {
    $transCols = $rulesContentCols[$frCol]
    $frFilled = ($rules | Where-Object { $_.$frCol -and $_.$frCol.Trim() -ne "" }).Count

    $langCounts = @()
    foreach ($transCol in $transCols) {
        $count = ($rules | Where-Object {
            $_.$frCol -and $_.$frCol.Trim() -ne "" -and
            $_.$transCol -and $_.$transCol.Trim() -ne ""
        }).Count
        $langCounts += $count
    }
    $langStr = $langCounts -join " | "
    [void]$report.AppendLine("| $frCol | $frFilled | $langStr |")
}
[void]$report.AppendLine("")

# ── 7. SUMMARY ───────────────────────────────────────────────────────────────

$totalInconsistencies = $allFallacies.Count + $allScenarii.Count + $allVirtues.Count + $headingIssues.Count + $baratineurIssues + $piocheurIssues

[void]$report.AppendLine("## 7. Summary & Worklist")
[void]$report.AppendLine("")
[void]$report.AppendLine("| Dataset | Inconsistencies | Type |")
[void]$report.AppendLine("|---------|----------------:|------|")
[void]$report.AppendLine("| Fallacies (families) | $($allFallacies.Count) | Terminology |")
[void]$report.AppendLine("| Scenarii (categories) | $($allScenarii.Count) | Terminology |")
[void]$report.AppendLine("| Virtues (families) | $($allVirtues.Count) | Terminology |")
[void]$report.AppendLine("| Rules (headings) | $($headingIssues.Count) | Structure |")
[void]$report.AppendLine("| Scenarii (baratineur) | $baratineurIssues languages | Role term |")
[void]$report.AppendLine("| Scenarii (piocheur) | $piocheurIssues languages | Role term |")
[void]$report.AppendLine("| **TOTAL** | **$totalInconsistencies** | |")
[void]$report.AppendLine("")

if ($totalInconsistencies -eq 0) {
    [void]$report.AppendLine("### Conclusion")
    [void]$report.AppendLine("")
    [void]$report.AppendLine("✅ **All translations are terminologically consistent.** No worklist needed.")
    [void]$report.AppendLine("")
    [void]$report.AppendLine("The translations across all 4 datasets and 8 languages use consistent terminology for:")
    [void]$report.AppendLine("- Fallacy/Virtue family, sub-family, and sub-sub-family labels")
    [void]$report.AppendLine("- Scenarii categories and sub-categories")
    [void]$report.AppendLine("- Role terms (baratineur/piocheur)")
    [void]$report.AppendLine("- Rules heading structure")
    [void]$report.AppendLine("")
    [void]$report.AppendLine("**Next step (#192 Étape 2)**: Smoke-test the OpenAI key + gpt-5.5 (3-5 cells) to validate the pipeline, then proceed to quality improvement passes if desired.")
} else {
    [void]$report.AppendLine("### Recommended Actions")
    [void]$report.AppendLine("")
    [void]$report.AppendLine("1. Review each inconsistency above and decide: canonical variant or intentional variation?")
    [void]$report.AppendLine("2. For genuine inconsistencies, create a worklist for gpt-5.5 re-translation passes")
    [void]$report.AppendLine("3. Rules heading mismatches may indicate structural differences (not just terminology)")
    [void]$report.AppendLine("4. Role terms (baratineur/piocheur) should be standardized to ONE canonical translation per language")
    [void]$report.AppendLine("")
}

[void]$report.AppendLine("---")
[void]$report.AppendLine("")
[void]$report.AppendLine("*Generated by TranslationConsistencyReport.ps1 — #192 Étape 1*")

# ── Write output ─────────────────────────────────────────────────────────────

$reportPath = Join-Path $OutputDir "translation-consistency-audit-192.md"
[System.IO.File]::WriteAllText($reportPath, $report.ToString(), [System.Text.UTF8Encoding]::new($false))

Write-Host ""
Write-Host "=== Report written to: $reportPath ===" -ForegroundColor Green
Write-Host "Total inconsistencies: $totalInconsistencies" -ForegroundColor $(if ($totalInconsistencies -gt 0) {"Yellow"} else {"Green"})
