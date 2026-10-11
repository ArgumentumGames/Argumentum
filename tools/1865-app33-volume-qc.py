# -*- coding: utf-8 -*-
"""QC sweep of the T2 volume filled outputs (#457, #1865).

Reads the committed review objects (docs/.../pivots/*-volume-filled.csv) against the
committed worklists (*-volume.csv) — replays entirely from the repo, no sandbox needed.
Checks: row/key parity with worklists, 7-lang fill, FR-source contamination, HTML tag
preservation, glossary verbatim forms where a game name occurs.

FR-IDENTICAL cells that are legitimate BY CONSTRUCTION are classified below with their
reason, not silently whitelisted (une exclusion doit mourir avec sa raison): each entry
names the exact key+lang and why identity with the FR source is the CORRECT outcome.
Any FR-IDENTICAL outside this set fails the sweep.

Pre-review self-check for the ai-01 relecture — NOT a substitute for it.
"""
import csv
import re
import sys

PIV = 'docs/dnn-localization/release-validation/exports/DNN-Argumentum-export-app33-2026-10-09/pivots/'
LANGS = ['en', 'ru', 'pt', 'es', 'ar', 'fa', 'zh']

GLOSSARY = {  # fr name -> expected verbatim form per lang (from the locked prompt glossary)
    "L'école des menteurs": {'en': 'The school of liars', 'ru': 'Школа лжецов', 'pt': 'A escola dos mentirosos',
                             'es': 'La escuela de los mentirosos', 'ar': 'مدرسة الكاذبين',
                             'fa': 'مدرسهٔ دروغ‌پردازان', 'zh': '说谎者学校'},
}

# (file-stem, full key, lang) -> why identity with FR is correct
LEGIT_IDENTICAL = {
    ('content', '33|Content|1D949D90-51BF-48EB-A71D-3B5F6D29B5A9|Text', l):
        'iframe embed: no translatable text, byte-identical by design'
    for l in LANGS
}
LEGIT_IDENTICAL[('content', '33|Content|92F7AAD9-ECF1-4BAF-9BDA-A464605E7D1E|Text', 'en')] = (
    'FR source is itself English placeholder text (Heading 1/Paragraph 1...); the 6 other langs translated it')
LEGIT_IDENTICAL[('video', '33|Video|5C0328AC-65C0-40F5-9975-A966C5C4FCD0|Title', 'en')] = (
    '"Shortlink Test": international test term, identical form is correct')


def main():
    fail = 0
    for stem, n_expect in [('content', 45), ('link', 20), ('video', 6)]:
        work = list(csv.DictReader(open(PIV + f'{stem}-volume.csv', encoding='utf-8-sig')))
        got = list(csv.DictReader(open(PIV + f'{stem}-volume-filled.csv', encoding='utf-8-sig')))
        if len(got) != n_expect:
            print(f'FAIL {stem}: {len(got)} rows, expected {n_expect}')
            fail += 1
            continue
        if [r['key'] for r in work] != [r['key'] for r in got]:
            print(f'FAIL {stem}: key set mismatch')
            fail += 1
        missing, frsame_bad, legit, tagdiff, gloss_miss = [], [], [], [], []
        for r in got:
            empt = [l for l in LANGS if not (r[l] or '').strip()]
            if empt:
                missing.append((r['key'][:40], empt))
                continue
            if r['attribute'] == 'Text':
                src_tags = sorted(re.findall(r'</?[a-zA-Z]+', r['fr']))
                for l in LANGS:
                    if sorted(re.findall(r'</?[a-zA-Z]+', r[l])) != src_tags:
                        tagdiff.append((r['key'][:40], l))
            for l in LANGS:
                if (r[l] or '').strip() == (r['fr'] or '').strip() and len((r['fr'] or '').strip()) > 12:
                    reason = LEGIT_IDENTICAL.get((stem, r['key'], l))
                    if reason:
                        legit.append((r['key'][:40], l, reason))
                    else:
                        frsame_bad.append((r['key'][:40], l))
            for fr_name, forms in GLOSSARY.items():
                if fr_name.lower() in (r['fr'] or '').lower():
                    for l, expect in forms.items():
                        if expect not in (r[l] or ''):
                            gloss_miss.append((r['key'][:40], l, expect[:20]))
        print(f'== {stem}: rows={len(got)} missing_targets={len(missing)} '
              f'fr_identical_unexpected={len(frsame_bad)} fr_identical_legit={len(legit)} '
              f'html_tag_diff={len(tagdiff)} glossary_miss={len(gloss_miss)}')
        for label, lst in [('MISSING', missing), ('FR-IDENTICAL-UNEXPECTED', frsame_bad),
                           ('TAG-DIFF', tagdiff), ('GLOSSARY-MISS', gloss_miss)]:
            for item in lst[:6]:
                print(f'   {label}: {item}')
        for item in legit:
            print(f'   FR-IDENTICAL-LEGIT: {item[0]} {item[1]} — {item[2]}')
        fail += len(missing) + len(frsame_bad) + len(tagdiff) + len(gloss_miss)

    print('QC VERDICT:', 'FAIL' if fail else 'ALL CLEAN')
    return 1 if fail else 0


if __name__ == '__main__':
    sys.exit(main())
