# -*- coding: utf-8 -*-
"""T2 volume worklist derivation — app33 export pivots -> 3 filtered volume CSVs.

Filters (each row of the audit explains every exclusion):
  1. demo entities (manifest rendering.demoMarkers, matched on entity Title,
     accent/case-insensitive) — excluded ENTIRELY;
  2. trial entity 2538A3F0-… (already translated + reviewed, content-trial-11392-filled.csv)
     — excluded, grafted at assembly;
  3. non-prose fr values: file refs (file:NNN), FA icons (fa-*), URLs/emails
     (http://, https://, mailto:, www.), pure numbers — structurally untranslatable;
  4. empty fr (prompt instruction 4 would skip them anyway — they never enter a worklist).

Output: pivots/{content,link,video}-volume.csv, byte-convention of the source pivots
(BOM + CRLF, csv module with lineterminator='\r\n').
Self-test: re-reads outputs, asserts row counts and that no excluded key leaked in.
"""
import csv, io, json, os, re, sys, unicodedata

BASE = 'docs/dnn-localization/release-validation/exports/DNN-Argumentum-export-app33-2026-10-09'
PIV = os.path.join(BASE, 'pivots')
MANIFEST = json.load(open(os.path.join(BASE, 'manifest.json'), encoding='utf-8-sig'))

# Manifest rendering.demoMarkers: 'Demo content' x2 (actual titles: 'Demo content' +
# 'Demo content 2'), 'Demo content without Caption', 'Demo link', 'New video',
# 'Demo VIdeo 2', 'Mentions legales' (actual title: 'Mentions légales', accent-tolerant).
DEMO_MARKERS = ['Demo content', 'Demo content 2', 'Demo content without Caption',
                'Demo link', 'New video', 'Demo VIdeo 2', 'Mentions legales']
TRIAL_GUID = '2538A3F0-9785-458E-9F8A-EDF4001B1BBE'
# Attributes that are paths/enums by construction (never prose, whatever the value):
# Link.Link = href/path ('/Portals/...', 'http://…', '/'), Link.Window = enum ('auto').
NON_PROSE_ATTRIBUTES = {'Link': {'Link', 'Window'}, 'Video': {'VideoLink'}}

URL_RE = re.compile(r'^(https?://|www\.|mailto:)', re.I)
NUM_RE = re.compile(r'^[\d\s.,:%+-]+$')

def norm(s):
    """lowercase + strip diacritics for marker matching (manifest writes 'legales' sans accent)."""
    s = unicodedata.normalize('NFD', s or '')
    return ''.join(c for c in s if unicodedata.category(c) != 'Mn').strip().lower()

def non_prose_reason(v):
    v = (v or '').strip()
    if not v:
        return 'empty-fr'
    if v.lower().startswith('file:'):
        return 'file-ref'
    if v.lower().startswith('fa-') and ' ' not in v:
        return 'fa-icon'
    if URL_RE.match(v):
        return 'url'
    if NUM_RE.match(v):
        return 'numeric'
    return None

def entity_title(rows_of_entity):
    for r in rows_of_entity:
        if r['attribute'] == 'Title' and (r['fr'] or '').strip():
            return r['fr']
    return None

def derive(name, out_name, content_type):
    rows = list(csv.DictReader(open(os.path.join(PIV, name), encoding='utf-8-sig')))
    ents = {}
    for r in rows:
        ents.setdefault(r['guid'], []).append(r)
    audit = {'file': name, 'entities_total': len(ents), 'rows_total': len(rows),
             'demo_entities': [], 'trial_rows': 0, 'non_prose': {}, 'kept': 0}
    demo_guids = set()
    for g, rs in ents.items():
        t = entity_title(rs) or ''
        hit = next((m for m in DEMO_MARKERS if norm(m) == norm(t)), None)
        if hit:
            demo_guids.add(g)
            audit['demo_entities'].append({'guid': g, 'title': t, 'marker': hit})
    kept, writer_fields = [], None
    for r in rows:
        writer_fields = list(r.keys())
        if r['guid'] in demo_guids:
            continue
        if r['guid'] == TRIAL_GUID:
            audit['trial_rows'] += 1
            continue
        if r['attribute'] in NON_PROSE_ATTRIBUTES.get(content_type, set()):
            audit['non_prose'].setdefault('attr-' + r['attribute'], []).append(r['key'])
            continue
        reason = non_prose_reason(r['fr'])
        if reason == 'empty-fr':
            continue  # never part of a worklist
        if reason:
            audit['non_prose'].setdefault(reason, []).append(r['key'])
            continue
        kept.append(r)
    # byte-convention write: BOM + CRLF
    buf = io.StringIO()
    w = csv.DictWriter(buf, fieldnames=writer_fields, lineterminator='\r\n')
    w.writeheader()
    for r in kept:
        w.writerow(r)
    with open(os.path.join(PIV, out_name), 'wb') as f:
        f.write(b'\xef\xbb\xbf' + buf.getvalue().encode('utf-8'))
    audit['kept'] = len(kept)
    audit['kept_by_attribute'] = {}
    for r in kept:
        audit['kept_by_attribute'][r['attribute']] = audit['kept_by_attribute'].get(r['attribute'], 0) + 1
    return audit

audits = []
for name, out, ct in [('content-pivot.csv', 'content-volume.csv', 'Content'),
                      ('link-pivot.csv', 'link-volume.csv', 'Link'),
                      ('video-pivot.csv', 'video-volume.csv', 'Video')]:
    audits.append(derive(name, out, ct))

# ---- self-test ----
ok = True
total_kept = 0
for a in audits:
    out = a['file'].replace('-pivot.csv', '-volume.csv')
    back = list(csv.DictReader(open(os.path.join(PIV, out), encoding='utf-8-sig')))
    assert len(back) == a['kept'], (out, len(back), a['kept'])
    keys = {r['key'] for r in back}
    for k in sum(a['non_prose'].values(), []):
        assert k not in keys, ('non-prose leaked', k)
    total_kept += a['kept']
    print(json.dumps(a, ensure_ascii=False, indent=1))
demo_total = sum(len(a['demo_entities']) for a in audits)
print(f"SELF-TEST OK — demo entities excluded: {demo_total}/7 (manifest) | volume rows kept: {total_kept}")
if demo_total != 7:
    print('!! demo count != 7 — marker matching incomplete, DO NOT USE'); ok = False
sys.exit(0 if ok else 2)
