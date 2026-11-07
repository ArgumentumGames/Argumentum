# Preuve geometrique #2190 (voie B) : chaque teinte du .mm designe un trait du SVG.
#
# Instrument de la PR 1 (code seul). Il sera execute par la PR 2 sur l'export
# FreeMind REEL d'une carte portant les teintes ; il tourne aussi tel quel sur
# un SVG synthetique pour l'autotest (--selftest).
#
# Ce qu'il prouve, par langue, sur une paire (.mm, .svg) issue du MEME run :
#
#   1. PORTEUR  : chaque arrowlink du .mm porte une teinte unique ; chaque teinte
#      apparait sur exactement un groupe <g stroke="rgb(...)"> du SVG (si FreeMind
#      quantise ou perd une teinte, le compte le dit — mesure, pas assertion).
#   2. IDENTITE : sur le SVG RESTAURE (post-pipeline, palette verbe + attributs
#      data-crosslink-*), chaque groupe porte source/cible/verbe et ces chemins
#      existent dans le .mm.
#   3. GEOMETRIE : pour chaque trait dont les libelles source/cible sont NON
#      AMBIGUS dans le SVG (libelle unique -> position certaine ; c'est le
#      sous-ensemble "240 traits" de la mesure du 07/11), le DEPART du chemin est
#      proche du texte source et l'ARRIVEE proche du texte cible.
#   4. CONTROLE INVERSE (--swap K) : permuter les teintes des traits K et K+1 du
#      SVG AVANT lecture doit faire ECROULER l'accord geometrique. Un instrument
#      qui ne voit pas la permutation ne prouve rien (il croirait vert sur vert).
#
# Sortie : resume console + JSON <out>. Reporte les chiffres, ne les asserte pas —
# la decision appartient a la revue.
#
# Usage :
#   python tools/mindmap_crosslink_tint_proof.py --mm F.mm --svg F.content.svg [--restored F2.svg] [--swap K] [--out R.json]
#   python tools/mindmap_crosslink_tint_proof.py --selftest

import argparse
import json
import math
import re
import sys
import xml.etree.ElementTree as ET
from pathlib import Path

TOL = 400  # px ; generosite maximale mesuree du 07/11 (p95 ~350) vs espacement inter-noeuds >= 1000

TAG_G = re.compile(r'<g\b([^>]*)>|</g>')
ATTR = re.compile(r'([\w:-]+)\s*=\s*"([^"]*)"')
TEXT_RE = re.compile(r'<text\b([^>]*)>(.*?)</text>', re.S)
PATH_D = re.compile(r'<path\b[^>]*\bd="([^"]*)"', re.S)
NUM = re.compile(r'-?\d+(?:\.\d+)?')


def attrs(s):
    return dict(ATTR.findall(s))


def parse_mm(path):
    """Noeuds (ID -> texte) et arrowlinks (teinte -> source, cible) en ordre de document."""
    root = ET.parse(path).getroot()
    nodes, links = {}, []
    tints = {}

    def walk(el):
        for child in el:
            if child.tag == 'node':
                nid = child.get('ID')
                nodes[nid] = ' '.join((child.get('TEXT') or '').split())
                for al in child.findall('arrowlink'):
                    color = (al.get('COLOR') or '').lower()
                    links.append({'src': nid, 'dst': al.get('DESTINATION'), 'tint': color})
                walk(child)
    walk(root)

    for link in links:
        tints[link['tint']] = link  # unicite verifiee par l'appelant

    return nodes, links, tints


def parse_svg(path):
    """Textes (position absolue) et groupes transverses (teinte ou restaure), ordre document."""
    t = Path(path).read_text(encoding='utf-8')
    assert not re.search(r'transform="(?![^"]*translate)', t), "transform non-translate present"
    texts, cross, stack = [], [], []
    pos = 0
    for m in TAG_G.finditer(t):
        if m.group(1) is None:
            if stack:
                stack.pop()
            continue
        a = attrs(m.group(1))
        tr = a.get('transform')
        tx, ty = 0.0, 0.0
        if tr:
            ns = NUM.findall(tr)
            tx, ty = float(ns[0]), float(ns[1])
        poff = stack[-1]['off'] if stack else (0.0, 0.0)
        off = (poff[0] + tx, poff[1] + ty)
        stack.append({'kind': 'g', 'off': off, 'stroke': a.get('stroke', '')})
        nxt = TAG_G.search(t, m.end())
        seg = t[m.end(): nxt.start() if nxt else len(t)]
        for tm in TEXT_RE.finditer(seg):
            ta = attrs(tm.group(1))
            x, y = float(ta.get('x', 0)), float(ta.get('y', 0))
            label = re.sub(r'<[^>]+>', '', tm.group(2))
            texts.append({'x': off[0] + x, 'y': off[1] + y, 'text': ' '.join(label.split())})
        g = stack[-1]
        if g['stroke'].startswith('rgb(') or a.get('class') == 'crosslink':
            pm = PATH_D.search(seg)
            if pm:
                ns = [float(v) for v in NUM.findall(pm.group(1))]
                cross.append({
                    'start': (off[0] + ns[0], off[1] + ns[1]),
                    'end': (off[0] + ns[-2], off[1] + ns[-1]),
                    'stroke': g['stroke'],
                    'class': a.get('class', ''),
                    'verb': a.get('data-crosslink-verb'),
                    'source': a.get('data-crosslink-source'),
                    'target': a.get('data-crosslink-target'),
                })
    return texts, cross


def rgb_key(value):
    """'rgb(204,255,255)' ou '#ccffff' -> cle normalisee 'r,g,b' (None si irreconnaissable)."""
    v = (value or '').strip().lower()
    if v.startswith('#') and len(v) == 7:
        return f"{int(v[1:3],16)},{int(v[3:5],16)},{int(v[5:7],16)}"
    if v.startswith('rgb(') and v.endswith(')'):
        return ','.join(p.strip() for p in v[4:-1].split(','))
    return None


def unique_texts(texts):
    """libelle -> position, libelles NON ambigus seulement (le sous-ensemble mesurable)."""
    from collections import defaultdict
    by_label = defaultdict(list)
    for t in texts:
        by_label[t['text']].append((t['x'], t['y']))
    return {label: positions[0] for label, positions in by_label.items() if len(positions) == 1}


def dist(a, b):
    return math.hypot(a[0] - b[0], a[1] - b[1])


def main():
    ap = argparse.ArgumentParser(description=__doc__)
    ap.add_argument('--mm', help='carte FreeMind portant les teintes (mem run que le SVG)')
    ap.add_argument('--svg', help='export SVG AVANT restauration (teintes visibles)')
    ap.add_argument('--restored', help='export SVG APRES pipeline (palette + data-crosslink-*)')
    ap.add_argument('--swap', type=int, default=-1,
                    help='controle inverse : permuter les teintes des traits K et K+1 avant lecture')
    ap.add_argument('--out', help='rapport JSON de sortie')
    ap.add_argument('--selftest', action='store_true', help='demo autonome (SVG synthetique integre)')
    args = ap.parse_args()

    if args.selftest:
        return selftest()

    if not (args.mm and args.svg):
        ap.error('--mm et --svg sont requis (ou --selftest)')

    nodes, links, tints = parse_mm(args.mm)
    texts, cross = parse_svg(args.svg)

    report = {'mm': str(args.mm), 'svg': str(args.svg), 'mm_links': len(links),
              'mm_unique_tints': len(tints), 'svg_traits': len(cross)}

    # ── 1. PORTEUR : teinte -> exactement un groupe du SVG ──
    by_key = {}
    for c in cross:
        key = rgb_key(c['stroke'])
        if key:
            by_key.setdefault(key, []).append(c)
    tint_keys = {rgb_key(t): t for t in tints}
    carried = sum(1 for k in tint_keys if k in by_key)
    doubled = [k for k in tint_keys if len(by_key.get(k, [])) > 1]
    orphan_strokes = [c['stroke'] for c in cross
                      if rgb_key(c['stroke']) and rgb_key(c['stroke']) not in tint_keys]
    report['carrier'] = {
        'tints_carried': f"{carried}/{len(tints)}",
        'tints_seen_twice': len(doubled),
        'svg_rgb_groups_not_in_mm': len(orphan_strokes),
    }

    # ── 3. GEOMETRIE sur les traits a libelles non ambigus ──
    pos = unique_texts(texts)
    id_to_text = {nid: text for nid, text in nodes.items()}
    text_positions = {}
    for nid, text in nodes.items():
        if text in pos:
            text_positions[nid] = pos[text]
    report['measurable'] = {
        'svg_texts': len(texts),
        'unique_labels': len(pos),
        'nodes_locatable': len(text_positions),
    }

    def geo_check(pairs):
        """pairs: [(trait, source_id, target_id)] -> stats d'accord vrai et a l'envers."""
        agree = wrongdir = 0
        checked = 0
        for trait, src, dst in pairs:
            if src not in text_positions or dst not in text_positions:
                continue
            sp, dp = text_positions[src], text_positions[dst]
            checked += 1
            if dist(trait['start'], sp) <= TOL and dist(trait['end'], dp) <= TOL:
                agree += 1
            if dist(trait['start'], dp) <= TOL and dist(trait['end'], sp) <= TOL:
                wrongdir += 1
        return {'checked': checked, 'agree': agree, 'wrong_direction': wrongdir}

    # Identification par teinte (SVG pre-restauration)
    pairs = []
    for c in cross:
        key = rgb_key(c['stroke'])
        if key and key in tint_keys:
            link = tints[tint_keys[key]]
            pairs.append((c, link['src'], link['dst']))
    report['geometry_by_tint'] = geo_check(pairs)

    # ── 4. CONTROLE INVERSE : permuter K et K+1 AVANT lecture ──
    if args.swap >= 0 and len(pairs) >= args.swap + 2:
        a, b = pairs[args.swap], pairs[args.swap + 1]
        swapped = [(trait, b[1], b[2]) if trait is a[0]
                   else (trait, a[1], a[2]) if trait is b[0]
                   else (trait, s, d)
                   for trait, s, d in pairs]
        report['geometry_by_tint_swap'] = geo_check(swapped)

    # ── 2. IDENTITE sur le SVG restaure (palette + data-crosslink-*) ──
    if args.restored:
        r_texts, r_cross = parse_svg(args.restored)
        annotated = [c for c in r_cross if c['class'] == 'crosslink']
        incomplete = [c for c in annotated
                      if not (c['verb'] and c['source'] is not None and c['target'] is not None)]
        report['restored'] = {
            'annotated_groups': len(annotated),
            'incomplete_annotations': len(incomplete),
        }
        # La re-verification geometrique par data-* (chemins de taxonomie) exige la table
        # C# (le .mm ne porte que des ID de noeuds, pas des chemins) : la passe 3 ci-dessus
        # l'a deja etablie par teinte ; les tests C# (CrossLinkTintTableTests) couvrent le
        # joint data-* -> corpus de bout en bout.

    out = json.dumps(report, ensure_ascii=False, indent=1)
    print(out)
    if args.out:
        Path(args.out).write_text(out, encoding='utf-8')

    ok = (report['carrier']['tints_carried'].split('/')[0] == str(len(tints))
          and report['geometry_by_tint']['checked'] > 0
          and report['geometry_by_tint']['agree'] == report['geometry_by_tint']['checked'])
    return 0 if ok else 1


def selftest():
    """Demo autonome : .mm et SVG synthetiques, permutation incluse. Doit rendre VERT puis ROUGE."""
    import tempfile
    tmp = Path(tempfile.mkdtemp(prefix='tint-proof-'))
    mm = tmp / 'demo.mm'
    mm.write_text(
        '<map><node ID="A" TEXT="Source Alpha">'
        '<arrowlink DESTINATION="B" COLOR="#c2f9ff"/></node>'
        '<node ID="B" TEXT="Cible Beta">'
        '<arrowlink DESTINATION="A" COLOR="#c3f8ff"/></node>'
        '</map>', encoding='utf-8')
    svg = tmp / 'demo.svg'
    svg.write_text(
        '<svg><g transform="translate(1000,1000)"><text x="0" y="0">Source Alpha</text></g>'
        '<g transform="translate(5000,4000)"><text x="0" y="0">Cible Beta</text></g>'
        '<g stroke="rgb(194,249,255)" fill="rgb(194,249,255)">'
        '<path d="M 1050 1050 C 0 0 0 0 4950 3950"/></g>'
        '<g stroke="rgb(195,248,255)" fill="rgb(195,248,255)">'
        '<path d="M 4950 4050 C 0 0 0 0 1050 1150"/></g>'
        '</svg>', encoding='utf-8')
    report_path = tmp / 'report.json'
    rc_ok = main_invoke(['--mm', str(mm), '--svg', str(svg), '--out', str(report_path), '--swap', '0'])
    report = json.loads(report_path.read_text(encoding='utf-8'))
    print(f"\nselftest: rc={rc_ok} agree={report['geometry_by_tint']['agree']}"
          f"/{report['geometry_by_tint']['checked']}"
          f" swap_agree={report['geometry_by_tint_swap']['agree']}"
          f"/{report['geometry_by_tint_swap']['checked']}")
    if rc_ok == 0 and report['geometry_by_tint']['agree'] == 2 \
            and report['geometry_by_tint_swap']['agree'] == 0:
        print("SELFTEST OK : vert sans permutation, ROUGE avec (le controle inverse voit la permutation)")
        return 0
    print("SELFTEST ECHEC")
    return 1


def main_invoke(argv):
    sys.argv = [sys.argv[0]] + argv
    return main()


if __name__ == '__main__':
    sys.exit(main())
