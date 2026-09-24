# -*- coding: utf-8 -*-
"""G2-C (#1499) — Liste courte des pertes : paires BASELINE 2024 → HEAD, textes entiers.

Le dossier G2 (§5) cite des extraits imprimé→HEAD ; G2-C arbitre le geste
AGENTIQUE : « l'ancien » = la baseline `62b561e75` (version d'avant la couche
agentique — consigne #458 c.5801633624), pas l'imprimé 2022.

Entrée : fallacies-exemples-diff.json (régénéré par fallacies-exemples-diff.py,
         témoins PASS exigés). Surface = couche agentique × SUBST (111 mesuré).
Sortie : g2c-paires.txt — une section par PK, textes ENTIERS (baseline vs HEAD),
         similarité difflib, longueurs, titre HEAD, drapeaux tu/vous.
Ce fichier est une AIDE DE LECTURE : la classe mécanique ne tranche rien
(même règle que G2 §8).
"""
import difflib
import io
import json
import os
import re
import subprocess
import sys
import unicodedata

BASE = os.path.abspath(os.path.join(os.path.dirname(os.path.abspath(__file__)), '..', '..'))
CSV_PATH = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
BASELINE_REF = '62b561e75'
FIELD = 'example_fr'
DIFF_JSON = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'fallacies-exemples-diff.json')
OUT = os.path.join(os.path.dirname(os.path.abspath(__file__)), 'g2c-paires.txt')

TU = re.compile(r"\b(tu|ton|ta|tes|tien|tiens)\b|(?<![a-zA-Zé])t'", re.IGNORECASE)
VOUS = re.compile(r"\b(vous|votre|vos)\b", re.IGNORECASE)


def rows_of(commit):
    import csv
    p = subprocess.run(['git', '-C', BASE, 'show', '%s:%s' % (commit, CSV_PATH)],
                       capture_output=True)
    if p.returncode:
        sys.exit('illisible: %s@%s' % (CSV_PATH, commit))
    return list(csv.DictReader(io.StringIO(p.stdout.decode('utf-8-sig', 'replace'))))


def nfc(s):
    return unicodedata.normalize('NFC', s or '')


def main():
    data = json.load(io.open(DIFF_JSON, encoding='utf-8'))
    surface = [r for r in data if r['couche'] == 'agentique' and r['cls'] == 'SUBST']
    print('surface agentique SUBST : %d (attendu 111)' % len(surface))
    if len(surface) != 111:
        sys.exit('FAIL: surface != 111 — re-générer fallacies-exemples-diff.json')

    head = {nfc(r.get('PK', '')).strip(): r for r in rows_of('HEAD')}
    basel = {nfc(r.get('PK', '')).strip(): r for r in rows_of(BASELINE_REF)}

    lines = []
    tu_vous = 0
    for r in sorted(surface, key=lambda r: int(r['pk']) if r['pk'].isdigit() else 0):
        pk = r['pk']
        b = nfc(basel[pk].get(FIELD, '')).strip()
        h = nfc(head[pk].get(FIELD, '')).strip()
        titre = nfc(head[pk].get('text_fr', '')).strip()
        sim = difflib.SequenceMatcher(None, b, h).ratio()
        tu_b, v_h = bool(TU.search(b)), bool(VOUS.search(h))
        if tu_b and v_h:
            tu_vous += 1
        lines.append('=== PK %s · path %s · sim %.2f · %d → %d c. · tu→vous %s'
                     % (pk, r['path'], sim, len(b), len(h), 'OUI' if tu_b and v_h else '-'))
        lines.append('TITRE  : %s' % titre)
        lines.append('ANCIEN : %s' % b)
        lines.append('ACTUEL : %s' % h)
        lines.append('')

    io.open(OUT, 'w', encoding='utf-8').write('\n'.join(lines))
    sims = [difflib.SequenceMatcher(None,
                                    nfc(basel[r['pk']].get(FIELD, '')),
                                    nfc(head[r['pk']].get(FIELD, ''))).ratio()
            for r in surface]
    print('similarités : min %.2f · médiane %.2f · max %.2f'
          % (min(sims), sorted(sims)[len(sims) // 2], max(sims)))
    print('tu→vous (drapeau) : %d/%d' % (tu_vous, len(surface)))
    lens = [(len(nfc(basel[r['pk']].get(FIELD, ''))), len(nfc(head[r['pk']].get(FIELD, ''))))
            for r in surface]
    raccourcis = sum(1 for lb, lh in lens if lh < lb)
    allonges = sum(1 for lb, lh in lens if lh > lb)
    print('longueur : raccourcis %d · allongés %d · égaux %d'
          % (raccourcis, allonges, len(lens) - raccourcis - allonges))
    print('-> %s' % OUT)


if __name__ == '__main__':
    main()
