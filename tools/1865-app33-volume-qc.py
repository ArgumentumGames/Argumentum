# -*- coding: utf-8 -*-
"""QC sweep of the T2 volume filled outputs (#457, #1865).

Reads the committed review objects (docs/.../pivots/*-volume-filled.csv) against the
committed worklists (*-volume.csv) — replays entirely from the repo, no sandbox needed.
Checks: row/key parity with worklists, 7-lang fill, FR-source contamination, HTML tag
preservation, glossary verbatim forms where a game name occurs.

Typography rules (verdict ai-01 c.6122858964, fix PR #1872) — each with its inverse
control in --self-test (a faulty synthetic cell MUST turn the rule red, a legitimate
witness MUST keep it green; a 0 is only an absence if the instrument could see a 1):
  T1  space (incl. NBSP, any form) before punctuation in the 7 target languages — the
      French spacing is fr-only. zh additionally: halfwidth !?:; in prose is a defect
      (fullwidth ：；！？ required, spaceless).
  T2  Latin digit in fa PROSE — code blocks {...}, Latin-token neighbours (A0 format,
      2sxc product) and cross-line neighbours (list numbers) are exempt by design.
  T3  first letter of an ar/fa cell separated from its word by a tag boundary
      (^<p>X<span…): Arabic/Persian letters join, the split breaks the ligature.
  T4  es: every !/? must carry its inverted opener (¡/¿) in the same sentence.

FR-IDENTICAL cells that are legitimate BY CONSTRUCTION are classified below with their
reason, not silently whitelisted (une exclusion doit mourir avec sa raison): each entry
names the exact key+lang and why identity with the FR source is the CORRECT outcome.
Any FR-IDENTICAL outside this set fails the sweep.

Pre-review self-check for the ai-01 relecture — NOT a substitute for it.
"""
import csv
import html
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


# --- Typography rules (T1-T4) -----------------------------------------------------------

AR_LETTER = re.compile(r'[ء-يٮ-ۓ]')


def prose_text(cell):
    """Decoded (2-pass), tag-stripped, {...} code blocks removed."""
    t = cell or ''
    for _ in range(2):
        t = html.unescape(t)
    return re.sub(r'\{[^}]*\}', '', re.sub(r'<[^>]+>', '', t))


def rule_t1_space_before_punct(cell):
    return [m.group(1) for m in re.finditer(r'\s+([!?:;؛؟！？：；])', prose_text(cell))]


def rule_t1_zh_halfwidth(cell):
    return re.findall(r'[!?:;]', prose_text(cell))


def rule_t2_fa_latin_digit(cell):
    t = prose_text(cell)
    hits = []
    for m in re.finditer(r'[0-9]+', t):
        before = t[:m.start()].split('\n')[-1].rstrip()[-1:]
        after = t[m.end():].split('\n')[0].lstrip()[:1]
        nb, na = before or ' ', after or ' '   # neutral when line edge / empty
        if (AR_LETTER.match(nb) or AR_LETTER.match(na)) and not re.match(r'[A-Za-z]', nb) and not re.match(r'[A-Za-z]', na):
            hits.append(m.group())
    return hits


def rule_t3_split_letter(cell):
    return bool(re.match(r'^<p>.<span', cell or ''))


def rule_t4_es_inverted(cell):
    t = prose_text(cell)
    bad = []
    for closer, opener in [('!', '¡'), ('?', '¿')]:
        for m in re.finditer(re.escape(closer), t):
            seg = t[max(0, m.start() - 400):m.start()]
            last = max(seg.rfind('.'), seg.rfind('!'), seg.rfind('?'))
            if opener not in seg[last + 1:]:
                bad.append(closer)
    return bad


def self_test():
    """Inverse controls: faulty synthetic cells fire; legitimate witnesses stay green."""
    checks = [
        # T1 — space before punctuation (any script, any space form)
        (lambda: rule_t1_space_before_punct('mot : suite'), True, 'NBSP before : must fire'),
        (lambda: rule_t1_space_before_punct('mot&nbsp;; suite'), True, '&nbsp; entity before ; must fire'),
        (lambda: rule_t1_space_before_punct('mot: suite'), False, 'glued punctuation must stay green'),
        (lambda: rule_t1_space_before_punct('{ id : "1"} code'), False, 'code block exempt by design'),
        (lambda: rule_t1_space_before_punct('كلمة ؟ سؤال'), True, 'Arabic ؟ with space must fire'),
        (lambda: rule_t1_zh_halfwidth('艺术吧 !'), True, 'zh halfwidth ! must fire'),
        (lambda: rule_t1_zh_halfwidth('艺术吧！'), False, 'zh fullwidth must stay green'),
        (lambda: rule_t1_zh_halfwidth('数据！ { id : "1"}'), False, 'zh halfwidth inside code exempt'),
        # T2 — Latin digit in fa prose
        (lambda: rule_t2_fa_latin_digit('از 7 خانواده'), True, 'digit between Persian words must fire'),
        (lambda: rule_t2_fa_latin_digit('قطع A0، بزرگ'), False, 'A0 format exempt'),
        (lambda: rule_t2_fa_latin_digit('متن 2sxc است'), False, '2sxc product token exempt'),
        (lambda: rule_t2_fa_latin_digit('فهرست\n2\n3\n4'), False, 'cross-line list numbers exempt'),
        (lambda: rule_t2_fa_latin_digit('از ۷ خانواده'), False, 'Persian digit ۷ is green'),
        # T3 — first letter split by tag boundary (ar/fa)
        (lambda: rule_t3_split_letter('<p>ه<span style="font-weight: 400;">ل ترغب'), True, 'split letter must fire'),
        (lambda: rule_t3_split_letter('<p><span style="font-weight: 400;">هل ترغب'), False, 'letter inside span is green'),
        (lambda: rule_t3_split_letter('<p>Hello <span>world'), False, 'two-char prefix is not the signature'),
        # T4 — es inverted marks
        (lambda: rule_t4_es_inverted('¡Haz clic para saber más!'), False, 'proper ¡ pair is green'),
        (lambda: rule_t4_es_inverted('Haz clic para saber más!'), True, 'missing ¡ must fire'),
        (lambda: rule_t4_es_inverted('¿A quién va dirigido?'), False, 'proper ¿ pair is green'),
        (lambda: rule_t4_es_inverted('A quién va dirigido?'), True, 'missing ¿ must fire'),
    ]
    failed = 0
    for fn, expect_fire, label in checks:
        fires = bool(fn())
        ok = fires == expect_fire
        if not ok:
            failed += 1
        print(f'  [{"OK" if ok else "FAIL"}] {"fires " if expect_fire else "silent"} — {label}')
    return failed


def main():
    if '--self-test' in sys.argv:
        n = self_test()
        print('SELF-TEST:', 'OK' if n == 0 else f'{n} CHECK(S) FAILED')
        return 1 if n else 0

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
        missing, frsame_bad, legit, tagdiff, gloss_miss, typo = [], [], [], [], [], []
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
            # --- T1-T4 typography ---
            for l in LANGS:
                hits = rule_t1_space_before_punct(r[l])
                if hits:
                    typo.append((r['key'][:40], l, 'T1-space-before-punct', hits[:3]))
                if l == 'zh' and rule_t1_zh_halfwidth(r[l]):
                    typo.append((r['key'][:40], l, 'T1-zh-halfwidth', rule_t1_zh_halfwidth(r[l])[:3]))
                if l == 'es' and rule_t4_es_inverted(r[l]):
                    typo.append((r['key'][:40], l, 'T4-es-inverted-missing', rule_t4_es_inverted(r[l])[:3]))
                if l == 'fa' and rule_t2_fa_latin_digit(r[l]):
                    typo.append((r['key'][:40], l, 'T2-fa-latin-digit', rule_t2_fa_latin_digit(r[l])[:3]))
                if l in ('ar', 'fa') and rule_t3_split_letter(r[l]):
                    typo.append((r['key'][:40], l, 'T3-split-letter', ['first letter outside <span>']))
        print(f'== {stem}: rows={len(got)} missing_targets={len(missing)} '
              f'fr_identical_unexpected={len(frsame_bad)} fr_identical_legit={len(legit)} '
              f'html_tag_diff={len(tagdiff)} glossary_miss={len(gloss_miss)} typo={len(typo)}')
        for label, lst in [('MISSING', missing), ('FR-IDENTICAL-UNEXPECTED', frsame_bad),
                           ('TAG-DIFF', tagdiff), ('GLOSSARY-MISS', gloss_miss), ('TYPO', typo)]:
            for item in lst[:6]:
                print(f'   {label}: {item}')
        for item in legit:
            print(f'   FR-IDENTICAL-LEGIT: {item[0]} {item[1]} — {item[2]}')
        fail += len(missing) + len(frsame_bad) + len(tagdiff) + len(gloss_miss) + len(typo)

    print('QC VERDICT:', 'FAIL' if fail else 'ALL CLEAN')
    return 1 if fail else 0


if __name__ == '__main__':
    sys.exit(main())
