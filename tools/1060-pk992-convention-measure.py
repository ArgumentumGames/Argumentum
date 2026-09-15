#!/usr/bin/env python3
"""PK 992 convention measure — ai-01 dispatch mbs53t [primaire].

Measures the text_fr vs nom_vulgarisé convention across ALL 1408 nodes to
decide whether PK 992's fields are interverted relative to the population.

Read-only: prints stats, touches nothing. Field-segment splitter respects
doubled-quote escapes + bare LF inside quoted cells (the file has 144 bare LFs
in multi-line cells). Reuses the #753/#1058 splitter idiom.
"""
import sys, statistics

CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
PK992 = "992"

# column indices (measured against the header)
PK_I, PATH_I, FAM_I, CARTE_I, NIV_I = 0, 1, 4, 9, 10
NOMV_I, TEXT_I, DESC_I = 11, 13, 15  # DESC_I=15 (col 14 is LTfr length counter, NOT desc_fr)


def split_logical_rows(text):
    """Yield raw row-strings. A LF inside a quoted field is NOT a row break."""
    rows, cur, q = [], [], False
    for ch in text:
        cur.append(ch)
        if ch == '"':
            q = not q
        elif ch == '\n' and not q:
            rows.append("".join(cur)); cur = []
    if cur:
        rows.append("".join(cur))
    return rows


def split_fields(line):
    out, cur, q = [], "", False
    for ch in line:
        if ch == '"':
            q = not q
        elif ch == ',' and not q:
            out.append(cur); cur = ""; continue
        cur += ch
    out.append(cur)
    # strip surrounding quotes + unescape doubled quotes
    def unquote(s):
        s = s.strip()
        if len(s) >= 2 and s[0] == '"' and s[-1] == '"':
            s = s[1:-1].replace('""', '"')
        return s
    return [unquote(f) for f in out]


def main():
    with open(CSV_PATH, encoding="utf-8-sig") as f:
        text = f.read()
    rows = split_logical_rows(text)
    header = split_fields(rows[0])
    assert header[PK_I].strip() == "PK", header[PK_I]
    assert header[NOMV_I].strip() == "nom_vulgarisé", header[NOMV_I]
    assert header[TEXT_I].strip() == "text_fr", header[TEXT_I]

    data = []  # (pk, path, fam, carte, niv, nomv, textfr, descfr)
    for line in rows[1:]:
        if not line.strip():
            continue
        f = split_fields(line)
        if len(f) < DESC_I + 1:
            continue
        pk = f[PK_I].strip()
        if not pk or not pk.isdigit():
            # keep only numeric PK nodes (the 1408 taxonomy rows)
            continue
        data.append((pk, f[PATH_I], f[FAM_I], f[CARTE_I], f[NIV_I],
                     f[NOMV_I], f[TEXT_I], f[DESC_I]))

    print(f"== population ==  numeric-PK rows: {len(data)}")

    def filled(s): return s.strip() != ""
    nomv_fill = sum(1 for d in data if filled(d[5]))
    text_fill = sum(1 for d in data if filled(d[6]))
    print(f"nom_vulgarisé filled : {nomv_fill}/{len(data)}")
    print(f"text_fr filled       : {text_fill}/{len(data)}")

    # both filled — the comparable population
    both = [d for d in data if filled(d[5]) and filled(d[6])]
    print(f"both filled          : {len(both)}  (comparable set)")

    # 1) equality
    eq_exact = sum(1 for d in both if d[5] == d[6])
    eq_ci = sum(1 for d in both if d[5].lower() == d[6].lower())
    print(f"\n== equality ==")
    print(f"text_fr == nom_vulgarisé (exact)      : {eq_exact}")
    print(f"text_fr == nom_vulgarisé (case-insens): {eq_ci}")

    # 2) lengths
    lnom = [len(d[5]) for d in both]
    ltext = [len(d[6]) for d in both]
    print(f"\n== length (chars, on {len(both)} both-filled) ==")
    print(f"nom_vulgarisé len : mean {statistics.mean(lnom):.1f}  median {statistics.median(lnom)}  min {min(lnom)} max {max(lnom)}")
    print(f"text_fr      len : mean {statistics.mean(ltext):.1f}  median {statistics.median(ltext)}  min {min(ltext)} max {max(ltext)}")

    # 3) which is longer per node
    nom_longer = sum(1 for d in both if len(d[5]) > len(d[6]))
    text_longer = sum(1 for d in both if len(d[6]) > len(d[5]))
    same_len = sum(1 for d in both if len(d[5]) == len(d[6]))
    print(f"\n== per-node which field is longer ==")
    print(f"nom_vulgarisé LONGER than text_fr : {nom_longer}  ({100*nom_longer/len(both):.1f}%)")
    print(f"text_fr LONGER than nom_vulgarisé : {text_longer}  ({100*text_longer/len(both):.1f}%)")
    print(f"equal length                       : {same_len}")

    # 4) PROVERB heuristic — does text_fr read like a sentence (proverb/desc)
    #    or like a nominal term? Cheap proxy: starts with capital + contains a
    #    space + length>25 → "phrase-like"; else "term-like".
    def phrase_like(s):
        s = s.strip()
        return len(s) > 25 and " " in s
    text_phrase = sum(1 for d in both if phrase_like(d[6]))
    nomv_phrase = sum(1 for d in both if phrase_like(d[5]))
    print(f"\n== phrase-like (len>25 + space) proxy ==")
    print(f"text_fr      phrase-like : {text_phrase}  ({100*text_phrase/len(both):.1f}%)")
    print(f"nom_vulgarisé phrase-like: {nomv_phrase}  ({100*nomv_phrase/len(both):.1f}%)")

    # 5) INTERVERSION CANDIDATES — nom_vulgarisé is a long phrase AND text_fr is a short term
    #    (i.e. the POPULATION pattern is nomv=term/text=phrase; a node with the
    #    opposite is the anomaly the editor worried about)
    def term_like(s):
        s = s.strip()
        return 0 < len(s) <= 25
    flipped = [d for d in both if phrase_like(d[5]) and term_like(d[6])]
    print(f"\n== 'flipped' candidates (nom_vulgarisé phrase-like AND text_fr term-like ≤25) ==")
    print(f"count: {len(flipped)}")
    for d in flipped[:20]:
        print(f"  PK {d[0]:>5}  path {d[1]}  fam={d[2]!r}")
        print(f"         nom_vulgarisé ({len(d[5])}): {d[5][:80]!r}")
        print(f"         text_fr       ({len(d[6])}): {d[6][:80]!r}")

    # 6) PK 992 in detail
    print(f"\n== PK 992 (the editor's flag) ==")
    p992 = [d for d in data if d[0] == PK992]
    if p992:
        d = p992[0]
        print(f"  path={d[1]}  Famille={d[2]!r}  carte={d[3]!r}  niveau={d[4]!r}")
        print(f"  nom_vulgarisé ({len(d[5])}): {d[5]!r}")
        print(f"  text_fr       ({len(d[6])}): {d[6]!r}")
        print(f"  desc_fr       ({len(d[7])}): {d[7]!r}")
        print(f"  text_fr longer than nom_vulgarisé ? {len(d[6]) > len(d[5])}")
        print(f"  text_fr phrase-like ? {phrase_like(d[6])}   nom_vulgarisé phrase-like ? {phrase_like(d[5])}")
    else:
        print("  NOT FOUND among numeric-PK rows")

    # 7) carte=2 (printed deck) context
    c2 = [d for d in data if d[3].strip() == "2"]
    print(f"\n== carte=2 (printed new cards) : {len(c2)} nodes ==")
    # is PK 992 among them?
    print(f"  PK 992 in carte=2 ? {any(d[0]==PK992 for d in c2)}")

    # 8) neighbours of PK 992 — same Famille "Tricherie", to see their convention
    print(f"\n== sample: Famille 'Tricherie' (PK 992's family) — first 12 both-filled ==")
    tri = [d for d in both if d[2].strip() == "Tricherie"][:12]
    for d in tri:
        print(f"  PK {d[0]:>5}  nomv({len(d[5])}): {d[5][:40]!r:42}  text_fr({len(d[6])}): {d[6][:45]!r}")


if __name__ == "__main__":
    main()
