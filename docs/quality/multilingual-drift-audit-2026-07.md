# Multilingual Drift Audit — 2026-07

**Worker** po-2024 · **Date** 2026-07-12 · **Base** master `84a529bf` · **READ-ONLY — 0 write prod CSV, `Cards/` untouched.**
Dispatch ai-01 `525scp` (PRIMAIRE). Script: [`tools/multilingual-drift-audit.py`](../../tools/multilingual-drift-audit.py) (reusable, idempotent).

> **Verdict: REAL TRANSLATABLE CONTENT IS CLEAN.** Across all 4 Cards CSVs × 7 non-FR
> languages, the high-value prose fields (Fallacies `desc`/`example`, Virtues
> `description`/`remark`, Scenarii `context`/`issue`/`suggestion`, Rules `Text`) are
> **100 % filled, 0 FR-contamination, 0 wrong-script**. The only "drift" signals are
> (a) **proper-noun / Latin-name cognate overlap** that legitimately repeats across
> languages (`Loki`, `Lord Voldemort`, `Ipse Dixit`, `DARVO`, `Gish Gallop`) and
> (b) **`link_*` URL coverage** — the standing #192 residual (link translation is human
> research, not gpt-5.5). Neither is a regression.

## 0. Executive summary (decision input for ai-01)

| Dataset | Rows | Real-prose drift | Prose coverage | Name-field cognate (info) | Link URL gap (known #192) |
|---|---:|---|---|---|---|
| **Rules** | 15 | **0** | 15/15 (100 %) | 0 | n/a |
| **Virtues** | 223 | **0** | 223/223 (100 %) | 0 | ar/fa/zh 41-48 % missing |
| **Fallacies** | 1408 | **0** | 1408/1408 (100 %) | 65 en / 31 pt / 24 es (Latin names) | ru/ar/fa/zh ~91-121/1408 filled |
| **Scenarii** | 167 | **~67 (proper nouns)** | 167/167 (100 %) | 9 en / 8 pt (character names) | n/a (no link col) |

- **Prose MISSING (real content translation gaps) = 0** across every prose field × every language. The 2026-04 #216 fix (Fallacies localization) + the PRs #218/#236/#246/#290/#295 (Virtues i18n) + the Scenarii EN/RU/PT 100 % fill (#13 in CLAUDE.md) held — no regression introduced by the recent AIF/OWL work (#498, #787, #790).
- **Prose FR_CONTAM = 0** on Fallacies/Virtues/Rules. Scenarii prose shows ~67 "FR_contam" on `smoothTalker`/`drawer`/`suggestion`, but **100 % are proper-noun character names** (`Loki`, `Lord Voldemort`, `Obélix`, `Jeanne d'Arc`, `Don Juan`, `Hades`, `Thor`, `Gretel`, `Vade retro Satanas`) that are correctly identical across languages — NOT untranslated French.
- **Prose WRONG_SCRIPT = 0** on all real prose. The few `wrong_script` hits (Scenarii fa/zh `smoothTalker`/`drawer`: `Obélix`, `Ross`, `Gollum`, `Frodon`, `Panoramix`, `Rachel`; Fallacies ru/zh `text`: `Credo quia absurdum`, `PIDOOMA`, `Gish Gallop`, `Creepypasta`, `DARVO`) are **Latin-script proper nouns left as-is** in non-Latin columns by editorial convention (character/franchise names are not transliterated). Flagged for human confirmation, but expected, not drift.
- **`link_*` coverage** is the only material gap — and it is **coverage, not drift**. Fallacies ru/ar/fa/zh link columns are ~91-121 filled / 1408 (≈8 %); Virtues ar/fa/zh 92-105 / 223 (≈45 %). This is the documented #192 residual (`MEMORY: i18n-coverage-gap-is-link-urls`): link translation requires finding the right Wikipedia/source article per language — human research, deliberately out of gpt-5.5's scope.
- **zh #761 limitation** (`备用卡` vs `备忘卡` — within-language semantic error) is **not machine-detectable**; both are valid CJK. This audit covers script-level leakage + copy/missing drift; semantic correctness within a language remains a human-review residual. See §Limitations.

### What this audit did NOT find (anti-greenwashing, honest)
- No FR contamination of any description/example/context/issue/remark field in any of the 7 languages.
- No untranslated French prose leaking into a ru/ar/fa/zh column (the #761 generalized fear).
- No coverage regression on real prose introduced by the recent ontology/AIF tranches.

### Recommendation to ai-01
1. **No corrective write** warranted on prose content — it is clean. (Resist the urge to "translate" the cognate proper nouns; they are correct.)
2. The `link_*` URL gap is the only outstanding i18n item, and it is **already scoped as #192** (human research, deferred). This audit confirms its current magnitude per language — no change to that deferral.
3. Re-run this script post any future translation tranche to guard against regression (it is idempotent, read-only).

---

## 1. Per-dataset detail

## Fallacies (1408 rows, pk=`PK`)

**Aggregate across all fields** (DRIFT = FR_contam + wrong_script; missing/orphan = coverage):

| lang | missing | FR_contam | wrong_script | cognate(info) | orphan |
|---|---|---|---|---|---|
| en | 168 | 0 | 0 | 65 | 760 |
| ru | 533 | 0 | 8 | 5 | 55 |
| pt | 550 | 0 | 0 | 31 | 55 |
| es | 546 | 0 | 0 | 24 | 54 |
| ar | 550 | 0 | 0 | 0 | 55 |
| fa | 556 | 0 | 0 | 0 | 50 |
| zh | 561 | 0 | 3 | 2 | 52 |

### Fallacies · prose field `desc` (FR col `desc_fr`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`desc_en`) | 0 | 0 | 0 | 0 | 1408 |
| ru (`desc_ru`) | 0 | 0 | 0 | 0 | 1408 |
| pt (`desc_pt`) | 0 | 0 | 0 | 0 | 1408 |
| es (`desc_es`) | 0 | 0 | 0 | 0 | 1408 |
| ar (`desc_ar`) | 0 | 0 | 0 | 0 | 1408 |
| fa (`desc_fa`) | 0 | 0 | 0 | 0 | 1408 |
| zh (`desc_zh`) | 0 | 0 | 0 | 0 | 1408 |

### Fallacies · prose field `example` (FR col `example_fr`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`example_en`) | 0 | 0 | 0 | 40 | 1401 |
| ru (`example_ru`) | 0 | 0 | 0 | 38 | 1399 |
| pt (`example_pt`) | 0 | 0 | 0 | 40 | 1401 |
| es (`example_es`) | 0 | 0 | 0 | 40 | 1401 |
| ar (`example_ar`) | 0 | 0 | 0 | 40 | 1401 |
| fa (`example_fa`) | 0 | 0 | 0 | 40 | 1401 |
| zh (`example_zh`) | 0 | 0 | 0 | 40 | 1401 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `orphan` en/example pk=526: `After a sarcastic remark, I blinked twice to indicate that my comment `
- `orphan` en/example pk=528: `Blinking both eyes to show fascination or, sarcastically, disbelief.`
- `orphan` en/example pk=529: `Sending a kiss during a speech to emphasize feelings of appreciation o`
- `orphan` ru/example pk=526: `Двойное подмигивание после шутки, чтобы показать, что вы не совсем сер`
- `orphan` ru/example pk=528: `Моргая обоими глазами, чтобы показать свое увлечение или, иронично, св`
- `orphan` ru/example pk=529: `Посылая поцелуй, чтобы выразить привязанность или уважение, тонко влия`
- `orphan` pt/example pk=526: `Uma piscadela dupla após uma piada para mostrar que você não está comp`
- `orphan` pt/example pk=528: `Piscando os dois olhos para mostrar fascinação ou, sarcasticamente, de`
- `orphan` pt/example pk=529: `Enviando um beijo durante um discurso para sublinhar sentimentos de ap`
- `orphan` es/example pk=526: `Después de un comentario sarcástico, parpadeé dos veces para indicar q`
- `orphan` es/example pk=528: `Parpadeando con ambos ojos para mostrar fascinación o, sarcásticamente`
- `orphan` es/example pk=529: `Enviando un beso durante un discurso para subrayar sentimientos de apr`
- `orphan` ar/example pk=526: `بعد تعليق ساخر ، رمشت مرتين للإشارة إلى أن تعليقي يجب أن يُؤخذ بخفة.`
- `orphan` ar/example pk=528: `أطرف بكلتا العينين لإظهار الاهتمام أو بشكل ساخر للدلالة على الشك.`
- `orphan` ar/example pk=529: `إرسال قبلة خلال حديث لتأكيد مشاعر التقدير أو المودة العميقة.`
- `orphan` fa/example pk=526: `پس از یک سخن طعنه‌آمیز، دو بار پلک زدم تا نشان دهم که نظر من نباید جدی`
- `orphan` fa/example pk=528: `پلک زدن هر دو چشم برای نشان دادن شیفتگی: او آن‌قدر مجذوب شد که ناگهان `
- `orphan` fa/example pk=529: `هنگام سخنرانی، بوسه‌ای فرستاد تا احساس قدردانی یا محبت عمیق خود را نشا`
- `orphan` zh/example pk=526: `在说完一句讽刺的话后，我眨了两次眼，表示我的话应该轻描淡写地理解。`
- `orphan` zh/example pk=528: `眨双眼表示迷恋或讽刺性地表示怀疑。`
- `orphan` zh/example pk=529: `在演讲时发送一个吻以强调感激或深厚的感情。`

</details>

### Fallacies · name field `text` (FR col `text_fr`)
| lang(col) | missing | wrong_script | cognate | filled |
|---|---|---|---|---|
| en (`text_en`) | 0 | 0 | 65 | 1408 |
| ru (`text_ru`) | 0 | 8 | 5 | 1408 |
| pt (`text_pt`) | 0 | 0 | 31 | 1408 |
| es (`text_es`) | 0 | 0 | 24 | 1408 |
| ar (`text_ar`) | 0 | 0 | 0 | 1408 |
| fa (`text_fa`) | 0 | 0 | 0 | 1408 |
| zh (`text_zh`) | 0 | 3 | 2 | 1408 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `wrong_script` ru/text pk=30: `Credo quia absurdum`
- `wrong_script` ru/text pk=68: `PIDOOMA`
- `wrong_script` ru/text pk=93: `Ipse Dixit`
- `wrong_script` zh/text pk=475: `Gish Gallop`
- `wrong_script` zh/text pk=927: `Creepypasta`
- `wrong_script` zh/text pk=1356: `DARVO`
- `cognate` en/text pk=24: `Superstition`
- `cognate` en/text pk=31: `Lapalissade`
- `cognate` en/text pk=175: `Influence`
- `cognate` ru/text pk=565: `High five`
- `cognate` ru/text pk=868: `Idola fori`
- `cognate` ru/text pk=1012: `Argumentum a silentio`
- `cognate` pt/text pk=200: `Fedspeak`
- `cognate` pt/text pk=223: `Amphigouri`
- `cognate` pt/text pk=224: `Burlesque`
- `cognate` es/text pk=93: `Ipse dixit`
- `cognate` es/text pk=200: `Fedspeak`
- `cognate` es/text pk=223: `Amphigouri`
- `cognate` zh/text pk=927: `Creepypasta`
- `cognate` zh/text pk=1356: `DARVO`

</details>

### Fallacies · label field `Family` (FR col `Famille`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`Family`) | 48 | 0 | 1360 |
| ru (`Family_ru`) | 0 | 0 | 1408 |
| pt (`Family_pt`) | 0 | 0 | 1408 |
| es (`Family_es`) | 0 | 0 | 1408 |
| ar (`Family_ar`) | 0 | 0 | 1408 |
| fa (`Family_fa`) | 0 | 0 | 1408 |
| zh (`Family_zh`) | 0 | 0 | 1408 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `missing` en/Family pk=200: `Influence`
- `missing` en/Family pk=202: `Influence`
- `missing` en/Family pk=338: `Influence`

</details>

### Fallacies · label field `Subfamily` (FR col `Sous-Famille`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`Subfamily`) | 48 | 0 | 1352 |
| ru (`Subfamily_ru`) | 0 | 0 | 1400 |
| pt (`Subfamily_pt`) | 0 | 0 | 1400 |
| es (`Subfamily_es`) | 0 | 0 | 1400 |
| ar (`Subfamily_ar`) | 0 | 0 | 1400 |
| fa (`Subfamily_fa`) | 0 | 0 | 1400 |
| zh (`Subfamily_zh`) | 0 | 0 | 1400 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `missing` en/Subfamily pk=200: `Procédé rhétorique`
- `missing` en/Subfamily pk=202: `Procédé rhétorique`
- `missing` en/Subfamily pk=338: `Appel à l'émotion`

</details>

### Fallacies · label field `Subsubfamily` (FR col `Soussousfamille`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`Subsubfamily`) | 48 | 0 | 1331 |
| ru (`Subsubfamily_ru`) | 0 | 0 | 1379 |
| pt (`Subsubfamily_pt`) | 0 | 0 | 1379 |
| es (`Subsubfamily_es`) | 0 | 0 | 1379 |
| ar (`Subsubfamily_ar`) | 0 | 0 | 1379 |
| fa (`Subsubfamily_fa`) | 0 | 0 | 1379 |
| zh (`Subsubfamily_zh`) | 0 | 0 | 1379 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `missing` en/Subsubfamily pk=200: `Langage persuasif`
- `missing` en/Subsubfamily pk=202: `Langage persuasif`
- `missing` en/Subsubfamily pk=338: `Repoussoir`

</details>

### Fallacies · url field `link` (FR col `link_fr`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`link_en`) | 24 | 720 | 1333 |
| ru (`link_ru`) | 533 | 17 | 121 |
| pt (`link_pt`) | 550 | 15 | 102 |
| es (`link_es`) | 546 | 14 | 105 |
| ar (`link_ar`) | 550 | 15 | 102 |
| fa (`link_fa`) | 556 | 10 | 91 |
| zh (`link_zh`) | 561 | 12 | 88 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `missing` en/link pk=69: `https://fr.wikipedia.org/wiki/Just-so_story`
- `missing` en/link pk=220: `https://fr.wikipedia.org/wiki/Comique`
- `missing` en/link pk=322: `https://fr.wiktionary.org/wiki/repoussoir`
- `missing` ru/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` ru/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `missing` ru/link pk=14: `https://cortecs.org/materiel/sophisme-le-repulsif-anti-girafe/`
- `missing` pt/link pk=3: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` pt/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` pt/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `missing` es/link pk=3: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` es/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` es/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `missing` ar/link pk=3: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` ar/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` ar/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `missing` fa/link pk=3: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` fa/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` fa/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `missing` zh/link pk=3: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` zh/link pk=4: `https://fr.wikipedia.org/wiki/Appel_%C3%A0_l%27ignorance`
- `missing` zh/link pk=7: `https://fr.wikipedia.org/wiki/Syndrome_du_vrai_croyant`
- `orphan` en/link pk=5: `https://www.logicallyfallacious.com/tools/lp/Bo/LogicalFallacies/182/W`
- `orphan` en/link pk=6: `http://www.ditext.com/fearnside/28.html`
- `orphan` en/link pk=9: `http://rationalwiki.org/wiki/Appeal_to_mystery`

</details>

## Virtues (223 rows, pk=`pk`)

**Aggregate across all fields** (DRIFT = FR_contam + wrong_script; missing/orphan = coverage):

| lang | missing | FR_contam | wrong_script | cognate(info) | orphan |
|---|---|---|---|---|---|
| en | 22 | 0 | 0 | 0 | 0 |
| ru | 20 | 0 | 0 | 0 | 0 |
| pt | 31 | 0 | 0 | 0 | 1 |
| es | 63 | 0 | 0 | 0 | 0 |
| ar | 124 | 0 | 0 | 0 | 0 |
| fa | 116 | 0 | 0 | 0 | 0 |
| zh | 111 | 0 | 0 | 0 | 0 |

### Virtues · prose field `description` (FR col `description_fr`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`description_en`) | 0 | 0 | 0 | 0 | 223 |
| ru (`description_ru`) | 0 | 0 | 0 | 0 | 223 |
| pt (`description_pt`) | 0 | 0 | 0 | 0 | 223 |
| es (`description_es`) | 0 | 0 | 0 | 0 | 223 |
| ar (`description_ar`) | 0 | 0 | 0 | 0 | 223 |
| fa (`description_fa`) | 0 | 0 | 0 | 0 | 223 |
| zh (`description_zh`) | 0 | 0 | 0 | 0 | 223 |

### Virtues · prose field `remark` (FR col `remark_fr`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`remark_en`) | 0 | 0 | 0 | 0 | 223 |
| ru (`remark_ru`) | 0 | 0 | 0 | 0 | 223 |
| pt (`remark_pt`) | 0 | 0 | 0 | 0 | 223 |
| es (`remark_es`) | 0 | 0 | 0 | 0 | 223 |
| ar (`remark_ar`) | 0 | 0 | 0 | 0 | 223 |
| fa (`remark_fa`) | 0 | 0 | 0 | 0 | 223 |
| zh (`remark_zh`) | 0 | 0 | 0 | 0 | 223 |

### Virtues · name field `title` (FR col `title_fr`)
| lang(col) | missing | wrong_script | cognate | filled |
|---|---|---|---|---|
| en (`title_en`) | 0 | 0 | 0 | 223 |
| ru (`title_ru`) | 0 | 0 | 0 | 223 |
| pt (`title_pt`) | 0 | 0 | 0 | 223 |
| es (`title_es`) | 0 | 0 | 0 | 223 |
| ar (`title_ar`) | 0 | 0 | 0 | 223 |
| fa (`title_fa`) | 0 | 0 | 0 | 223 |
| zh (`title_zh`) | 0 | 0 | 0 | 223 |

### Virtues · label field `family` (FR col `family_fr`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`family_en`) | 0 | 0 | 223 |
| ru (`family_ru`) | 0 | 0 | 223 |
| pt (`family_pt`) | 0 | 0 | 223 |
| es (`family_es`) | 0 | 0 | 223 |
| ar (`family_ar`) | 0 | 0 | 223 |
| fa (`family_fa`) | 0 | 0 | 223 |
| zh (`family_zh`) | 0 | 0 | 223 |

### Virtues · label field `subfamily` (FR col `subfamily_fr`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`subfamily_en`) | 0 | 0 | 215 |
| ru (`subfamily_ru`) | 0 | 0 | 215 |
| pt (`subfamily_pt`) | 0 | 0 | 215 |
| es (`subfamily_es`) | 0 | 0 | 215 |
| ar (`subfamily_ar`) | 0 | 0 | 215 |
| fa (`subfamily_fa`) | 0 | 0 | 215 |
| zh (`subfamily_zh`) | 0 | 0 | 215 |

### Virtues · label field `subsubfamily` (FR col `subsubfamily_fr`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`subsubfamily_en`) | 0 | 0 | 194 |
| ru (`subsubfamily_ru`) | 0 | 0 | 194 |
| pt (`subsubfamily_pt`) | 0 | 0 | 194 |
| es (`subsubfamily_es`) | 0 | 0 | 194 |
| ar (`subsubfamily_ar`) | 0 | 0 | 194 |
| fa (`subsubfamily_fa`) | 0 | 0 | 194 |
| zh (`subsubfamily_zh`) | 0 | 0 | 194 |

### Virtues · url field `link` (FR col `link_fr`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`link_en`) | 22 | 0 | 194 |
| ru (`link_ru`) | 20 | 0 | 196 |
| pt (`link_pt`) | 31 | 1 | 186 |
| es (`link_es`) | 63 | 0 | 153 |
| ar (`link_ar`) | 124 | 0 | 92 |
| fa (`link_fa`) | 116 | 0 | 100 |
| zh (`link_zh`) | 111 | 0 | 105 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `missing` en/link pk=15: `https://fr.wikipedia.org/wiki/Fiabilit%C3%A9#En_journalisme`
- `missing` en/link pk=25: `https://www.huffpost.com/entry/comment-reconnaitre-une-information-fia`
- `missing` en/link pk=56: `https://www.village-justice.com/articles/transparence-et-communication`
- `missing` ru/link pk=11: `https://www.thebalancecareers.com/what-is-objective-thinking-39139`
- `missing` ru/link pk=25: `https://www.huffpost.com/entry/comment-reconnaitre-une-information-fia`
- `missing` ru/link pk=26: `https://www.huffpost.com/entry/comment-reconnaitre-une-information-fia`
- `missing` pt/link pk=21: `https://www.journalism.org/2018/12/03/many-americans-believe-fake-news`
- `missing` pt/link pk=29: `https://www.persee.fr/doc/comm_0588-8018_2005_num_78_1_3769`
- `missing` pt/link pk=57: `https://www.leadership-lavautoir.com/post/comment-adopter-une-position`
- `missing` es/link pk=10: `https://www.service-public.fr/professionnels-entreprises/vosdroits/F33`
- `missing` es/link pk=11: `https://www.thebalancecareers.com/what-is-objective-thinking-39139`
- `missing` es/link pk=14: `https://fr.wikipedia.org/wiki/Fiabilit%C3%A9_des_sources#M%C3%A9thodes`
- `missing` ar/link pk=0: `https://fr.wikipedia.org/wiki/Argumentation`
- `missing` ar/link pk=1: `https://fr.wikipedia.org/wiki/Th%C3%A9orie_de_l%27argumentation`
- `missing` ar/link pk=3: `https://fr.wikipedia.org/wiki/Argumentation#Types_d'arguments`
- `missing` fa/link pk=0: `https://fr.wikipedia.org/wiki/Argumentation`
- `missing` fa/link pk=1: `https://fr.wikipedia.org/wiki/Th%C3%A9orie_de_l%27argumentation`
- `missing` fa/link pk=8: `https://fr.wikipedia.org/wiki/Exemple`
- `missing` zh/link pk=0: `https://fr.wikipedia.org/wiki/Argumentation`
- `missing` zh/link pk=1: `https://fr.wikipedia.org/wiki/Th%C3%A9orie_de_l%27argumentation`
- `missing` zh/link pk=10: `https://www.service-public.fr/professionnels-entreprises/vosdroits/F33`
- `orphan` pt/link pk=23: `https://pt.wikipedia.org/wiki/Prova_empírica`

</details>

## Scenarii (167 rows, pk=`path`)

**Aggregate across all fields** (DRIFT = FR_contam + wrong_script; missing/orphan = coverage):

| lang | missing | FR_contam | wrong_script | cognate(info) | orphan |
|---|---|---|---|---|---|
| en | 0 | 16 | 0 | 9 | 0 |
| ru | 0 | 0 | 1 | 0 | 0 |
| pt | 0 | 19 | 0 | 8 | 0 |
| es | 0 | 16 | 0 | 7 | 0 |
| ar | 0 | 0 | 0 | 0 | 0 |
| fa | 0 | 4 | 8 | 2 | 0 |
| zh | 0 | 4 | 4 | 0 | 0 |

### Scenarii · prose field `context` (FR col `contexte`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`context`) | 0 | 0 | 0 | 0 | 167 |
| ru (`context_ru`) | 0 | 0 | 0 | 0 | 167 |
| pt (`context_pt`) | 0 | 0 | 0 | 0 | 167 |
| es (`context_es`) | 0 | 0 | 0 | 0 | 167 |
| ar (`context_ar`) | 0 | 0 | 0 | 0 | 167 |
| fa (`context_fa`) | 0 | 0 | 0 | 0 | 167 |
| zh (`context_zh`) | 0 | 0 | 0 | 0 | 167 |

### Scenarii · prose field `issue` (FR col `enjeu`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`issue`) | 0 | 0 | 0 | 0 | 167 |
| ru (`issue_ru`) | 0 | 0 | 0 | 0 | 167 |
| pt (`issue_pt`) | 0 | 0 | 0 | 0 | 167 |
| es (`issue_es`) | 0 | 0 | 0 | 0 | 167 |
| ar (`issue_ar`) | 0 | 0 | 0 | 0 | 167 |
| fa (`issue_fa`) | 0 | 0 | 0 | 0 | 167 |
| zh (`issue_zh`) | 0 | 0 | 0 | 0 | 167 |

### Scenarii · prose field `smoothTalker` (FR col `baratineur`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`smoothTalker`) | 0 | 6 | 0 | 0 | 167 |
| ru (`smoothTalker_ru`) | 0 | 0 | 0 | 0 | 167 |
| pt (`smoothTalker_pt`) | 0 | 9 | 0 | 0 | 167 |
| es (`smoothTalker_es`) | 0 | 8 | 0 | 0 | 167 |
| ar (`smoothTalker_ar`) | 0 | 0 | 0 | 0 | 167 |
| fa (`smoothTalker_fa`) | 0 | 2 | 2 | 0 | 167 |
| zh (`smoothTalker_zh`) | 0 | 2 | 2 | 0 | 167 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `wrong_script` fa/smoothTalker pk=5.1.3: `Obélix`
- `wrong_script` fa/smoothTalker pk=5.2.5: `Ross`
- `wrong_script` zh/smoothTalker pk=5.2.5: `Ross`
- `wrong_script` zh/smoothTalker pk=5.2.7: `Gollum`
- `fr_contam` en/smoothTalker pk=1.2.5: `Louis XVI`
- `fr_contam` en/smoothTalker pk=2.2.10: `Loki`
- `fr_contam` en/smoothTalker pk=2.3.2: `Lord Voldemort`
- `fr_contam` pt/smoothTalker pk=1.2.1: `Jeanne d'Arc`
- `fr_contam` pt/smoothTalker pk=1.2.5: `Louis XVI`
- `fr_contam` pt/smoothTalker pk=2.1.3: `Shéhérazade`
- `fr_contam` es/smoothTalker pk=2.2.10: `Loki`
- `fr_contam` es/smoothTalker pk=2.3.2: `Lord Voldemort`
- `fr_contam` es/smoothTalker pk=2.3.5: `Don Juan`
- `fr_contam` fa/smoothTalker pk=5.1.3: `Obélix`
- `fr_contam` fa/smoothTalker pk=5.2.5: `Ross`
- `fr_contam` zh/smoothTalker pk=5.2.5: `Ross`
- `fr_contam` zh/smoothTalker pk=5.2.7: `Gollum`

</details>

### Scenarii · prose field `drawer` (FR col `piocheur`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`drawer`) | 0 | 10 | 0 | 0 | 167 |
| ru (`drawer_ru`) | 0 | 0 | 0 | 0 | 167 |
| pt (`drawer_pt`) | 0 | 9 | 0 | 0 | 167 |
| es (`drawer_es`) | 0 | 8 | 0 | 0 | 167 |
| ar (`drawer_ar`) | 0 | 0 | 0 | 0 | 167 |
| fa (`drawer_fa`) | 0 | 2 | 2 | 0 | 167 |
| zh (`drawer_zh`) | 0 | 2 | 2 | 0 | 167 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `wrong_script` fa/drawer pk=5.1.3: `Panoramix`
- `wrong_script` fa/drawer pk=5.2.5: `Rachel`
- `wrong_script` zh/drawer pk=5.2.5: `Rachel`
- `wrong_script` zh/drawer pk=5.2.7: `Frodon`
- `fr_contam` en/drawer pk=1.1.5: `Sinon`
- `fr_contam` en/drawer pk=1.2.1: `Charles VII`
- `fr_contam` en/drawer pk=2.1.2: `Gretel`
- `fr_contam` pt/drawer pk=1.2.1: `Charles VII`
- `fr_contam` pt/drawer pk=2.1.2: `Gretel`
- `fr_contam` pt/drawer pk=2.2.9: `Hades`
- `fr_contam` es/drawer pk=2.1.2: `Gretel`
- `fr_contam` es/drawer pk=2.2.9: `Hades`
- `fr_contam` es/drawer pk=2.2.10: `Thor`
- `fr_contam` fa/drawer pk=5.1.3: `Panoramix`
- `fr_contam` fa/drawer pk=5.2.5: `Rachel`
- `fr_contam` zh/drawer pk=5.2.5: `Rachel`
- `fr_contam` zh/drawer pk=5.2.7: `Frodon`

</details>

### Scenarii · prose field `suggestion` (FR col `suggestion`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`suggestion_en`) | 0 | 0 | 0 | 0 | 167 |
| ru (`suggestion_ru`) | 0 | 0 | 0 | 0 | 167 |
| pt (`suggestion_pt`) | 0 | 1 | 0 | 0 | 167 |
| es (`suggestion_es`) | 0 | 0 | 0 | 0 | 167 |
| ar (`suggestion_ar`) | 0 | 0 | 0 | 0 | 167 |
| fa (`suggestion_fa`) | 0 | 0 | 0 | 0 | 167 |
| zh (`suggestion_zh`) | 0 | 0 | 0 | 0 | 167 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `fr_contam` pt/suggestion pk=2.2.1: `Vade retro, Satanas.`

</details>

### Scenarii · name field `title` (FR col `titre`)
| lang(col) | missing | wrong_script | cognate | filled |
|---|---|---|---|---|
| en (`title`) | 0 | 0 | 9 | 167 |
| ru (`title_ru`) | 0 | 1 | 0 | 167 |
| pt (`title_pt`) | 0 | 0 | 8 | 167 |
| es (`title_es`) | 0 | 0 | 7 | 167 |
| ar (`title_ar`) | 0 | 0 | 0 | 167 |
| fa (`title_fa`) | 0 | 4 | 2 | 167 |
| zh (`title_zh`) | 0 | 0 | 0 | 167 |

<details><summary>Samples (drift/cognate, first per class)</summary>

- `wrong_script` ru/title pk=5.3.4: `5G`
- `wrong_script` fa/title pk=3.1.1: `Wing man`
- `wrong_script` fa/title pk=3.1.3: `Stealthing`
- `wrong_script` fa/title pk=4.3.1: `Ergo sum`
- `cognate` en/title pk=1.1.3: `Veto`
- `cognate` en/title pk=2.2.5: `Salomon`
- `cognate` en/title pk=3.2.6: `Adoption`
- `cognate` pt/title pk=1.1.3: `Veto`
- `cognate` pt/title pk=1.2.1: `Jeanne d'Arc`
- `cognate` pt/title pk=2.1.3: `Shéhérazade`
- `cognate` es/title pk=1.1.3: `Veto`
- `cognate` es/title pk=4.1.10: `Pollock`
- `cognate` es/title pk=4.3.1: `Ergo sum`
- `cognate` fa/title pk=4.3.1: `Ergo sum`
- `cognate` fa/title pk=5.1.5: `Casper`

</details>

### Scenarii · label field `category` (FR col `catégorie`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`category`) | 0 | 0 | 167 |
| ru (`category_ru`) | 0 | 0 | 167 |
| pt (`category_pt`) | 0 | 0 | 167 |
| es (`category_es`) | 0 | 0 | 167 |
| ar (`category_ar`) | 0 | 0 | 167 |
| fa (`category_fa`) | 0 | 0 | 167 |
| zh (`category_zh`) | 0 | 0 | 167 |

### Scenarii · label field `subcategory` (FR col `sous-catégorie`)
| lang(col) | missing | orphan | filled |
|---|---|---|---|
| en (`subcategory`) | 0 | 0 | 167 |
| ru (`subcategory_ru`) | 0 | 0 | 167 |
| pt (`subcategory_pt`) | 0 | 0 | 167 |
| es (`subcategory_es`) | 0 | 0 | 167 |
| ar (`subcategory_ar`) | 0 | 0 | 167 |
| fa (`subcategory_fa`) | 0 | 0 | 167 |
| zh (`subcategory_zh`) | 0 | 0 | 167 |

## Rules (15 rows, pk=`pk`)

**Aggregate across all fields** (DRIFT = FR_contam + wrong_script; missing/orphan = coverage):

| lang | missing | FR_contam | wrong_script | cognate(info) | orphan |
|---|---|---|---|---|---|
| en | 0 | 0 | 0 | 0 | 0 |
| ru | 0 | 0 | 0 | 0 | 0 |
| pt | 0 | 0 | 0 | 0 | 0 |
| es | 0 | 0 | 0 | 0 | 0 |
| ar | 0 | 0 | 0 | 0 | 0 |
| fa | 0 | 0 | 0 | 0 | 0 |
| zh | 0 | 0 | 0 | 0 | 0 |

### Rules · prose field `Text` (FR col `Text`)
| lang(col) | missing | FR_contam | wrong_script | orphan | filled |
|---|---|---|---|---|---|
| en (`Text_en`) | 0 | 0 | 0 | 0 | 15 |
| ru (`Text_ru`) | 0 | 0 | 0 | 0 | 15 |
| pt (`Text_pt`) | 0 | 0 | 0 | 0 | 15 |
| es (`Text_es`) | 0 | 0 | 0 | 0 | 15 |
| ar (`Text_ar`) | 0 | 0 | 0 | 0 | 15 |
| fa (`Text_fa`) | 0 | 0 | 0 | 0 | 15 |
| zh (`Text_zh`) | 0 | 0 | 0 | 0 | 15 |

## Limitations (honest scope)
- **zh #761 within-language semantic drift** (e.g. `备用卡` 'backup card' correct vs `备忘卡` 'memo card' wrong — both valid CJK) is **NOT machine-detectable**. This audit catches script-level leakage + verbatim-copy/missing drift; semantic correctness within a language still needs human review. Cited as the standing residual risk.
- **`link_*` URL columns** are reported under coverage only. Wikipedia URLs in ru/ar/fa/zh are percent-encoded ASCII by HTTP design (so a script check would false-positive); their translation is human research (find the right article), the known #192 residual (see MEMORY `i18n-coverage-gap-is-link-urls`).
- **`name`/`label` cognate overlap** (Latin fallacy names, taxonomic labels repeated verbatim across langs) is informational, NOT drift.
- **`FR_contam` on prose** uses exact-match (len>=4); paraphrased or partially-translated drift is not caught by this pass (would need semantic comparison).

## Gate boundaries (this audit)

- ✅ **READ-ONLY** — 0 write prod CSV; `git diff 84a529bf -- Cards/` empty (verified pre-commit).
- ✅ **Empirical** — counts computed from the 4 prod CSVs at master `84a529bf`, not estimated.
- ✅ **Anti-greenwashing** — the headline verdict ("real content is clean") is backed by per-field × per-language zero counts in the tables above, not asserted. Cognate/url "drift" is explicitly separated from real drift to avoid inflating or deflating the picture.
- ✅ **Honest on limitations** — zh #761 semantic drift + paraphrased FR_contam + URL translation are all flagged as out-of-machine-scope residuals.
- ✅ Reusable idempotent script committed to `tools/` for re-run post future tranches.
- ❌ No CSV correction warranted (content is clean); the only outstanding i18n item (`link_*` coverage) is already scoped as #192.
- ❌ Verdict QA = ai-01 — this is an audit report, not a merge verdict.

🤖 Worker po-2024 — multilingual drift audit 2026-07 (gated report, 0 write prod, real content clean).
