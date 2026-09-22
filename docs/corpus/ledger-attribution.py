# -*- coding: utf-8 -*-
"""Attribue chaque cellule changee au commit qui a introduit sa valeur COURANTE.

Un seul balayage : on charge le CSV a chacun des commits qui l'ont touche, du plus
ancien au plus recent, et on note pour chaque (path, champ) le premier commit ou la
valeur devient celle d'aujourd'hui. Cout = N chargements, pas N x M appels git.
"""
import csv, io, json, os, re, subprocess, unicodedata

OUT = os.path.dirname(os.path.abspath(__file__))
F = 'Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv'
LED = json.load(io.open(os.path.join(OUT, 'ledger.json'), encoding='utf-8'))

def nw(s):
    s = unicodedata.normalize('NFC', s or '').replace(u'\u00a0', ' ').replace(u'\u202f', ' ')
    s = s.replace(u'\u2019', "'").replace(u'\u2018', "'")
    return re.sub(r'\s+', ' ', s).strip()

# commits qui touchent le CSV, du plus ANCIEN au plus recent
log = subprocess.run(['git', 'log', '--follow', '--reverse', '--format=%H\x1f%ad\x1f%s',
                      '--date=short', '--', F], capture_output=True, cwd=r'D:\Argumentum')
commits = [l.split('\x1f') for l in log.stdout.decode('utf-8', 'replace').splitlines() if l.strip()]
print("commits balayes : %d" % len(commits))

target = {}                      # (path, field) -> valeur courante attendue
for r in LED:
    target[(r['path'], r['field'])] = r['now']

found = {}                       # (path, field) -> (sha, date, subject)
for sha, date, subj in commits:
    blob = subprocess.run(['git', 'show', sha + ':' + F], capture_output=True, cwd=r'D:\Argumentum').stdout
    if not blob:
        continue
    try:
        rows = list(csv.DictReader(io.StringIO(blob.decode('utf-8-sig', 'replace'))))
    except Exception:
        continue
    byp = {(r.get('path') or '').strip(): r for r in rows if (r.get('carte') or '').strip()}
    for (p, f), want in list(target.items()):
        if (p, f) in found:
            continue
        row = byp.get(p)
        if row is not None and nw(row.get(f)) == nw(want):
            found[(p, f)] = (sha[:8], date, subj)

# PR number depuis le sujet de commit (squash merge : "... (#NNN)")
PRRE = re.compile(r'\(#(\d+)\)\s*$')
for r in LED:
    k = (r['path'], r['field'])
    if k in found:
        sha, date, subj = found[k]
        r['commit'], r['date'], r['subject'] = sha, date, subj
        m = PRRE.search(subj)
        r['pr'] = m.group(1) if m else ''
    else:
        r['commit'] = r['date'] = r['subject'] = r['pr'] = ''

json.dump(LED, io.open(os.path.join(OUT, 'ledger.json'), 'w', encoding='utf-8'),
          ensure_ascii=False, indent=1)

from collections import Counter
att = sum(1 for r in LED if r['commit'])
print("cellules attribuees : %d / %d" % (att, len(LED)))
print("\n=== Concentration : quels commits portent les reecritures ? ===")
c = Counter((r['date'], r['commit'], r['pr'], r['subject'][:58]) for r in LED if r['commit'])
print("  %5s  %-10s %-8s %-6s %s" % ('cells', 'date', 'commit', 'PR', 'sujet'))
for (d, sha, pr, s), n in c.most_common(12):
    print("  %5d  %-10s %-8s %-6s %s" % (n, d, sha, ('#' + pr) if pr else '-', s))
print("\n  commits distincts : %d" % len(c))
