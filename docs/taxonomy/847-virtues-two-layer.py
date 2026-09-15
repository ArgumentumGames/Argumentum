# -*- coding: utf-8 -*-
"""#847 two-layer structural homogeneity — release re-run (2026-08-28).

Fallacies drift check vs the 2026-07 acompte + FIRST Virtues pass (post-#989B).
MEASURE, not verdict. Re-runnable: reads the prod CSVs at repo root.
Companions: 847-virtues-two-layer.md, 847-fallacies-rerun.csv, 847-virtues-pass.csv.
"""
import csv
import io
import math
import os
from collections import Counter

HERE = os.path.dirname(os.path.abspath(__file__))
ROOT = os.path.abspath(os.path.join(HERE, '..', '..'))
FALL = os.path.join(ROOT, 'Cards', 'Fallacies', 'Argumentum Fallacies - Taxonomy.csv')
VIRT = os.path.join(ROOT, 'Cards', 'Fallacies', 'Argumentum Virtues - Taxonomy.csv')


def load(path):
    with open(path, 'rb') as f:
        return list(csv.DictReader(io.StringIO(f.read().decode('utf-8-sig'))))


def entropy_norm(dist):
    """Shannon entropy normalised by log2(#distinct present types) — July acompte definition."""
    n = sum(dist.values())
    if n == 0:
        return 0.0, 0.0
    present = {t: c for t, c in dist.items() if c > 0}
    if len(present) <= 1:
        return 0.0, max(dist.values()) / n
    h = -sum((c / n) * math.log2(c / n) for c in present.values())
    return h / math.log2(len(present)), max(dist.values()) / n


def scheme_inference(r):
    return '_Inference' in (r.get('AIF_skosDirectRef', '') + r.get('AIF_skosExceptionRef', ''))


def scheme_walton(r):
    """Transposed (b) reading for Virtues: a Walton scheme name in DirectRef (the goodTenorOf layer)."""
    return bool(r.get('AIF_skosDirectRef', '').strip())


def measure(rows, levels, scheme_fn):
    out = []
    typed = [r for r in rows if r.get('AIF_attackType', '').strip()]
    for lvl_name, key in levels:
        groups = {}
        for r in typed:
            g = (r.get(key) or '').strip()
            if g:
                groups.setdefault(g, []).append(r)
        for g, leaves in sorted(groups.items()):
            dist = Counter(r['AIF_attackType'].strip() for r in leaves)
            n = len(leaves)
            a, homo = entropy_norm(dict(dist))
            native = sum(1 for r in leaves if scheme_fn(r))
            b = 1.0 - native / n
            a_high, b_high = a >= 0.50, b >= 0.50
            tag = ('coherent' if not a_high and not b_high else
                   'A-candidate' if a_high and not b_high else
                   'B+A' if a_high and b_high else 'B')
            gate = 'supported' if n >= 8 else ('indicative' if n >= 5 else 'thin')
            out.append({'level': lvl_name, 'node': g, 'n_leaves': n,
                        'undercut': dist.get('undercut', 0), 'undermine': dist.get('undermine', 0),
                        'rebut': dist.get('rebut', 0), 'entropy_norm_a': round(a, 3),
                        'homogeneity_ratio': round(homo, 3), 'native_scheme_n': native,
                        'fail_loud_rate_b': round(b, 3), 'a_high': a_high, 'b_high': b_high,
                        'decision_rule_tag': tag, 'n_gate': gate})
    return out


def write_csv(rows, path):
    cols = ['level', 'node', 'n_leaves', 'undercut', 'undermine', 'rebut', 'entropy_norm_a',
            'homogeneity_ratio', 'native_scheme_n', 'fail_loud_rate_b', 'a_high', 'b_high',
            'decision_rule_tag', 'n_gate']
    with open(path, 'w', encoding='utf-8', newline='') as f:
        f.write("# 847 release re-run 2026-08-28 — see 847-virtues-two-layer.md. MEASURE not verdict.\n")
        f.write(",".join(cols) + "\n")
        for r in rows:
            f.write(",".join(str(r[c]) for c in cols) + "\n")


if __name__ == '__main__':
    fall = load(FALL)
    fall_typed = [r for r in fall if r.get('AIF_attackType', '').strip()]
    fd = Counter(r['AIF_attackType'].strip() for r in fall_typed)
    print(f"FALLACIES typed={len(fall_typed)} dist={dict(fd)}")
    fall_rows = measure(fall, [('d1', 'Famille'), ('d2', 'Sous-Famille'), ('d3', 'Soussousfamille')],
                        scheme_inference)
    write_csv(fall_rows, os.path.join(HERE, '847-fallacies-rerun.csv'))
    print(f"  nodes={len(fall_rows)} A-candidate|B+A={sum(1 for r in fall_rows if r['decision_rule_tag'] in ('A-candidate', 'B+A'))}")

    virt = load(VIRT)
    virt_typed = [r for r in virt if r.get('AIF_attackType', '').strip()]
    vd = Counter(r['AIF_attackType'].strip() for r in virt_typed)
    a_all, h_all = entropy_norm(dict(vd))
    print(f"VIRTUES typed={len(virt_typed)} dist={dict(vd)} entropy={a_all:.3f} homogeneity={h_all:.3f}")
    virt_rows = measure(virt, [('d1', 'family_fr'), ('d2', 'subfamily_fr'), ('d3', 'subsubfamily_fr')],
                        scheme_walton)
    write_csv(virt_rows, os.path.join(HERE, '847-virtues-pass.csv'))
    print(f"  nodes={len(virt_rows)} non-coherent={sum(1 for r in virt_rows if r['decision_rule_tag'] != 'coherent')}")
    for r in virt_rows:
        if r['decision_rule_tag'] != 'coherent':
            print(f"  {r['level']} {r['node'][:40]:40} n={r['n_leaves']:>2} "
                  f"{r['undercut']}/{r['undermine']}/{r['rebut']} a={r['entropy_norm_a']} tag={r['decision_rule_tag']} {r['n_gate']}")
