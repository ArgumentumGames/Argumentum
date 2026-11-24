#!/usr/bin/env python3
"""Derive the execution registry for the 22/11 staging apply (ai-01 arbitrage).

Arbitrage ai-01 du 22/11 21:40Z (option iii, ``ai01-arbitrage-missing-pp-20261122``) :
aucune URL de pré-release n'atteint le site ; le registre officiel du dépôt reste intact.
Drops: every row of the 3 P&P Link entities absent from prod (MISSING), and the
Link row of the existing card 6A24AE92 — the 4 cells carrying a pre-release URL.
Official registry is never modified. Usage: python -I derive_run_registry.py <official.csv> <out.csv>

Auto-checks (each one a STOP, exit non nul) :
  - l'empreinte d'entrée est celle du registre officiel (sha256[:16] ``fc23b9a748ce3758``,
    depuis P24-32 le 24/11 ; avant : ``277aac83e0b71e64``) ;
  - pas de BOM ;
  - 673 lignes de corps -> 22 retirées -> 651 conservées ;
  - aucune chaîne ``-review`` ne survit dans ce qui reste ;
  - l'aller-retour du parse est identique.

Empreinte de sortie attendue :
  sha256 = a130a663b9374655c6e6eb5a3c913b540d689c3c8d0ac3c183d44f28e92627d7 (1 166 868 octets).
Calculée par ai-01 sur le blob master pour l'empreinte 92bf1e06… (1 166 864 octets) ;
re-mesurée par po-2024 (P24-32, 24/11) après la correction typographique de 3 cellules
du registre (+4 octets), 673 -> 22 -> 651 inchangé - à revérifier par la contre-revue.
Toute autre valeur : STOP, et poster les deux sorties.
"""
import csv, hashlib, io, sys
src, dst = sys.argv[1], sys.argv[2]
raw = open(src, 'rb').read()
print('input sha256[:16]', hashlib.sha256(raw).hexdigest()[:16], 'bytes', len(raw))
assert hashlib.sha256(raw).hexdigest()[:16] == 'fc23b9a748ce3758', 'STOP: input is not the official registry'
assert not raw.startswith(b'\xef\xbb\xbf')
rows = list(csv.reader(io.StringIO(raw.decode('utf-8'), newline='')))
hdr, body = rows[0], rows[1:]
ki, gi, fi = hdr.index('key'), hdr.index('entity_guid'), hdr.index('field')
MISSING = {'494AB453-EC8B-4372-A6F7-A4A13A142153', '8BF57CB5-4AC3-4E21-902C-32700B3C66BC', 'AA9754AF-86D8-4F5F-BEA9-C0DD6FCDBA4F'}
drop = lambda r: r[gi].upper() in MISSING or r[ki] == '6A24AE92-8177-44EF-841B-6A7C41937597|Link'
kept = [r for r in body if not drop(r)]
gone = [r[ki] for r in body if drop(r)]
assert len(body) == 673 and len(gone) == 22 and len(kept) == 651, (len(body), len(gone), len(kept))
assert not any('-review' in c for r in kept for c in r), 'STOP: a pre-release URL survived'
buf = io.StringIO(newline='')
csv.writer(buf, lineterminator='\r\n').writerows([hdr] + kept)
out = buf.getvalue().encode('utf-8')
back = list(csv.reader(io.StringIO(out.decode('utf-8'), newline='')))
assert back == [hdr] + kept, 'STOP: round-trip mismatch'
open(dst, 'wb').write(out)
print('removed', len(gone)); [print('  -', k) for k in gone]
print('kept rows', len(kept), '| output sha256', hashlib.sha256(out).hexdigest(), 'bytes', len(out))
