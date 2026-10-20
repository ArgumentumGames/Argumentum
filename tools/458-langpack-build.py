#!/usr/bin/env python3
"""#458 — build a DNN language pack (zip + sidecar) from a git commit.

Why (#458): the ru-RU production pack was assembled by a scratch script
(c.6281045558); every pack must be rebuildable at the go-live master hash
(pt-BR already moved with #1912 -> c4529918). The scratch assembly also had a
silent-defect class worth pinning down: its per-file comparison used
PowerShell Compare-Object, which is CASE-INSENSITIVE -- it reported "162
common" where the correct count was 161 (c.6281045558). This tool compares
with Python sets (case-sensitive by construction), and every mismatch refuses
delivery instead of shipping.

Semantics:
  payload  = every tracked *.<culture>.resx at <commit>, read from the git
             object store (byte-exact, worktree-independent), laid out at
             webroot-relative paths (repo path minus the DNNPlatform/ prefix).
  manifest = <name>.dnn listing exactly the payload set. Entries are written
             with forward slashes only. With --upstream-manifest, upstream
             order is preserved (common entries), additions are appended and
             upstream entries absent from master are dropped (excluded count
             is printed and recorded). Without it, a fresh manifest skeleton
             is generated in sorted order; DNN installs both identically.
  refusals (nothing is written, exit != 0):
    1. set(manifest) == set(payload) == set(master), case-sensitive; a
       case-variant hint is printed when one set holds the other's spelling
       under a different case (the Compare-Object defect class);
    2. 0 backslash in manifest paths/names;
    3. 100%% of resx parse as XML, and the manifest parses as XML.
  zip      = deterministic: member order (manifest, releaseNotes, license,
             then resx sorted) and member timestamps pinned to the commit
             time, so two machines building the same commit agree on bytes.
  sidecar  = per-file sha256 + counts + zip/manifest hashes + freshness rule
             (the language's resx must not move between <commit> and go-live;
             the tool prints the current freshness answer).

Usage:
    python tools/458-langpack-build.py --culture ru-RU [--commit <hash>]
        Dry-run: resolve the set, run all refusals, print the plan.
    python tools/458-langpack-build.py --culture pt-BR --commit c4529918 --apply
        Write <out>/<culture>_langpack_master-<hash>.zip + .build.json
    python tools/458-langpack-build.py --self-test
        Run the refusals against synthetic fixtures: a clean fixture must
        PASS every check, then a case-altered entry, a removed file, a
        backslash path and a malformed resx must each be REFUSED.

Known limitation: --upstream-manifest is required only to preserve a
historical upstream pack's ordering/description (the ru-RU R7 pack); it is
never used to decide what the payload contains -- the master tree is.
"""
import argparse
import hashlib
import json
import os
import re
import subprocess
import sys
import time
import zipfile
import xml.etree.ElementTree as ET
from datetime import datetime, timezone

PREFIX = 'DNNPlatform/'


class LangpackRefusal(Exception):
    """A delivery-blocking check failed. Nothing is written."""


def sh(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def git(repo, *args, binary=False):
    r = subprocess.run(['git', '-C', repo, *args], capture_output=True, check=True)
    return r.stdout if binary else r.stdout.decode('utf-8', 'replace').strip()


# ---------------------------------------------------------------- checks ----
def case_variant_hints(s1, s2):
    lower2 = {}
    for y in s2:
        lower2.setdefault(y.lower(), []).append(y)
    hints = []
    for x in sorted(s1):
        if x not in s2 and x.lower() in lower2:
            hints.append((x, lower2[x.lower()][0]))
    return hints


def check_sets(manifest_rels, payload_rels, master_rels):
    """Refusal 1: the three sets are equal, case-sensitively, without dups."""
    for label, rels in (('manifest', manifest_rels), ('payload', payload_rels)):
        if len(set(rels)) != len(list(rels)):
            dups = sorted(x for x in set(rels) if list(rels).count(x) > 1)
            raise LangpackRefusal(f'{label}: entrees dupliquees: {dups[:8]}')
        if set(rels) != set(master_rels):
            diff = sorted(set(rels) ^ set(master_rels))
            hints = case_variant_hints(set(rels), set(master_rels)) + \
                case_variant_hints(set(master_rels), set(rels))
            msg = f'{label} != master: {len(diff)} entree(s): {diff[:8]}'
            if hints:
                msg += f' | VARIANTES DE CASSE: {hints[:8]}'
            raise LangpackRefusal(msg)


def check_no_backslash(pairs):
    """Refusal 2: no backslash in any manifest <path>/<name>; no '/' in name."""
    bad_sep = [(p, n) for p, n in pairs if '\\' in p]
    bad_name = [(p, n) for p, n in pairs if '/' in n or '\\' in n]
    if bad_sep:
        raise LangpackRefusal(f'manifest: {len(bad_sep)} chemin(s) a antislash: {bad_sep[:5]}')
    if bad_name:
        raise LangpackRefusal(f'manifest: {len(bad_name)} <name> non-basename: {bad_name[:5]}')


def check_xml(manifest_text, resx_items):
    """Refusal 3: manifest and every resx parse as XML."""
    try:
        ET.fromstring(manifest_text.encode('utf-8'))
    except ET.ParseError as e:
        raise LangpackRefusal(f'manifest XML invalide: {e}')
    offenders = []
    for rel, data in resx_items:
        try:
            ET.fromstring(data)
        except ET.ParseError as e:
            offenders.append(f'{rel}: {e}')
    if offenders:
        raise LangpackRefusal(f'{len(offenders)} resx XML invalides: {offenders[:5]}')


# ------------------------------------------------------------- git access ----
def resolve_commit(repo, commit):
    full = git(repo, 'rev-parse', '--verify', f'{commit}^{{commit}}')
    short = git(repo, 'rev-parse', '--short', full)
    ct = int(git(repo, 'show', '-s', '--format=%ct', full))
    return full, short, ct


def master_files(repo, full, culture):
    all_paths = [p for p in git(repo, 'ls-tree', '-r', '--name-only', full).splitlines() if p]
    suffix = f'.{culture}.resx'
    exact = sorted(p for p in all_paths if p.endswith(suffix))
    variant = [p for p in all_paths if p.lower().endswith(suffix.lower()) and not p.endswith(suffix)]
    if variant:
        raise LangpackRefusal(f'culture {culture}: variantes de casse sur master: {variant[:8]}')
    outside = [p for p in exact if not p.startswith(PREFIX)]
    if outside:
        raise LangpackRefusal(f'{len(outside)} resx hors {PREFIX}: {outside[:5]}')
    return exact


def read_blobs(repo, full, rels_full):
    spec = ''.join(f'{full}:{p}\n' for p in rels_full).encode('utf-8')
    r = subprocess.run(['git', '-C', repo, 'cat-file', '--batch'],
                       input=spec, capture_output=True, check=True)
    out = r.stdout
    i, blobs = 0, []
    for spec_path in rels_full:
        j = out.index(b'\n', i)
        header = out[i:j].decode('utf-8', 'replace')
        i = j + 1
        parts = header.split(' ')
        if parts[-1] == 'missing':
            raise LangpackRefusal(f'blob git absent: {spec_path}')
        oid, typ, size = parts[0], parts[1], int(parts[2])
        data = out[i:i + size]
        i += size + 1  # trailing LF
        blobs.append(data)
    return blobs


# -------------------------------------------------------------- manifest -----
def emit_entries(rels):
    entries = ''
    for rel in rels:
        p, n = rel.rsplit('/', 1)
        entries += ('\t\t\t\t\t\t<languageFile>\n'
                    f'\t\t\t\t\t\t\t<path>{p}</path>\n'
                    f'\t\t\t\t\t\t\t<name>{n}</name>\n'
                    '\t\t\t\t\t\t</languageFile>\n')
    return entries


def parse_pairs(text):
    return [(p, n) for p, n in re.findall(r'<path>(.*?)</path>\s*<name>(.*?)</name>', text)]


def build_manifest(repo, full, short, culture, rels, upstream_path, desc, keep_license=False):
    if upstream_path:
        raw = open(upstream_path, 'rb').read()
        bom = raw[:3] == b'\xef\xbb\xbf'
        text = raw.decode('utf-8-sig')
        up_rel = [f'{p}/{n}' for p, n in parse_pairs(text)]
        # The raw R7 manifest carried 22 backslash paths + 1 case divergence
        # (Dnn.EditBar/resources vs Resources). Classification stays RAW --
        # matching the shipped pack's sidecar (139 common / 27 added / 34
        # excluded, c.6281045558): the variants fall to upstream-excluded, and
        # the master-spelled files surface in `added`. Only master-exact
        # spellings reach the written manifest either way (0 backslash is
        # enforced on the output). Count the variants for the record.
        master_lookup = {p.lower(): p for p in rels}
        master_exact = set(rels)
        variants = [r for r in up_rel if r not in master_exact and r.replace('\\', '/').lower() in master_lookup]
        if variants:
            print(f'upstream spelling variants: {len(variants)} (antislash/casse) -> classees upstream-excluded, reprises dans `added` en graphie de master')
        master_set = set(rels)
        up_set = set(up_rel)
        common = [r for r in up_rel if r in master_set]          # upstream order
        added = sorted(master_set - up_set)
        excluded = [r for r in up_rel if r not in master_set]
        new_order = common + added
        if not set(new_order) == master_set or set(new_order) & set(excluded):
            raise LangpackRefusal('upstream: recomposition incoherente')
        print(f'upstream languageFile: {len(up_rel)} | common {len(common)} | added {len(added)} | excluded {len(excluded)}')
        m = re.search(r'<package name="([^"]+)"', text)
        pkg_name = (m.group(1) if m else f'R7.DNNCE.Core.{culture}') + f'_{short}'
        old_desc = re.search(r'<description>.*?</description>', text, re.S)
        if not old_desc:
            raise LangpackRefusal('upstream: <description> introuvable')
        text = text.replace(old_desc.group(0), f'<description>{desc}</description>')
        text = re.sub(r'<code>.*?</code>', f'<code>{culture}</code>', text, count=1)
        if not keep_license and '<license' in text:
            text = re.sub(r'\s*<license[^>]*/>', '', text)
        i1 = text.index('<languageFile>')
        i2 = text.index('</languageFiles>')
        text = text[:i1] + emit_entries(new_order) + text[i2:]
        if text.count('<languageFile>') != len(new_order):
            raise LangpackRefusal('upstream: compte de languageFile incoherent')
        man_name = pkg_name + '.dnn'
        return text, man_name, len(common), added, excluded, bom
    # scratch skeleton
    pkg_name = f'Argumentum.DNNCE.Core.{culture}'
    man_name = f'{pkg_name}_{short}.dnn'
    text = ('<dotnetnuke type="Package" version="5.0">\n'
            '\t<packages>\n'
            f'\t\t<package name="{pkg_name}" type="CoreLanguagePack" version="00.01.00">\n'
            f'\t\t\t<friendlyName>{culture}</friendlyName>\n'
            f'\t\t\t<description>{desc}</description>\n'
            '\t\t\t<iconFile />\n'
            '\t\t\t<owner>\n'
            '\t\t\t\t<name>Argumentum</name>\n'
            '\t\t\t\t<organization>Argumentum</organization>\n'
            '\t\t\t\t<url>https://www.argumentum.games</url>\n'
            '\t\t\t</owner>\n'
            '\t\t\t<releaseNotes src="releaseNotes.txt" />\n'
            '\t\t\t<dependencies>\n'
            '\t\t\t\t<dependency type="CoreVersion">09.06.00</dependency>\n'
            '\t\t\t</dependencies>\n'
            '\t\t\t<components>\n'
            '\t\t\t\t<component type="CoreLanguage">\n'
            '\t\t\t\t\t<languageFiles>\n'
            f'\t\t\t\t\t\t<code>{culture}</code>\n'
            f'\t\t\t\t\t\t<displayName>{culture}</displayName>\n'
            '\t\t\t\t\t\t<fallback>en-US</fallback>\n'
            + emit_entries(rels) +
            '\t\t\t\t\t</languageFiles>\n'
            '\t\t\t\t</component>\n'
            '\t\t\t</components>\n'
            '\t\t</package>\n'
            '\t</packages>\n'
            '</dotnetnuke>\n')
    return text, man_name, 0, sorted(rels), [], True


# ----------------------------------------------------------------- main ------
def run_build(a):
    repo = os.path.abspath(a.repo or os.path.join(os.path.dirname(os.path.abspath(__file__)), '..'))
    if not re.fullmatch(r'[A-Za-z]{2,3}(-[A-Za-z0-9]{2,8})*', a.culture or ''):
        print(f'REFUS: culture invalide: {a.culture!r}')
        return 1
    try:
        full, short, ct = resolve_commit(repo, a.commit)
        rels_full = master_files(repo, full, a.culture)
        rels = [p[len(PREFIX):] for p in rels_full]
        print(f'commit {short} ({full[:12]}) culture {a.culture} | master resx: {len(rels)}')
        if not rels:
            raise LangpackRefusal('aucun resx pour cette culture')
        blobs = read_blobs(repo, full, rels_full)
        file_hashes = {rel: sh(data) for rel, data in zip(rels, blobs)}
        date_utc = datetime.fromtimestamp(ct, tz=timezone.utc).strftime('%Y-%m-%d')
        desc = (f'Argumentum build from master {short} ({date_utc}); culture {a.culture}; '
                f'{len(rels)} tracked *.{a.culture}.resx at that commit, byte-exact')
        man_text, man_name, common_n, added, excluded, bom = build_manifest(
            repo, full, short, a.culture, rels, a.upstream_manifest, desc, a.keep_license)
        pairs = parse_pairs(man_text)
        man_rels = [f'{p}/{n}' for p, n in pairs]
        # refusals -- nothing written until all three pass
        check_sets(man_rels, rels, rels)
        check_no_backslash(pairs)
        check_xml(man_text, list(zip(rels, blobs)))
        print('checks: sets==manifest==payload==master (casse stricte) | 0 antislash | XML 100%  -> OK')

        ordered_members = None  # member order is fixed below (manifest, notes, license, resx sorted)
        rel_notes = (f'Argumentum {a.culture} language pack\r\n'
                     f'Built from Argumentum master {short} ({date_utc}) by tools/458-langpack-build.py.\r\n'
                     f'Content: {len(rels)} tracked *.{a.culture}.resx at that commit, byte-exact (sha256 in sidecar).\r\n')
        if a.upstream_manifest:
            rel_notes += f'Base: upstream manifest {os.path.basename(a.upstream_manifest)}; common {common_n}, added {len(added)}, upstream-excluded {len(excluded)}.\r\n'

        # freshness (informational)
        fresh = git(repo, 'log', '--oneline', f'{short}..master', '--', f'*.{a.culture}.resx')
        fresh_lines = [l for l in fresh.splitlines() if l.strip()]
        print(f'freshness: {len(fresh_lines)} merge(s) *.{"%s" % a.culture}.resx depuis {short}'
              + (f' -> PAQUET PERIME: {fresh_lines[:3]}' if fresh_lines else ' -> a jour'))

        if a.hashes_out:
            with open(a.hashes_out, 'w', encoding='utf-8') as f:
                json.dump(file_hashes, f, ensure_ascii=False, indent=1, sort_keys=True)
            print(f'hashes: {a.hashes_out}')

        out = os.path.abspath(a.out or os.path.join(repo, '.tmp', 'langpack-out'))
        zip_name = f'{a.culture}_langpack_master-{short}.zip'
        print(f'plan: {zip_name} + {man_name} + {a.culture}_langpack_master-{short}.build.json -> {out}')
        if not a.apply:
            print('DRY-RUN OK — rerun with --apply')
            return 0

        os.makedirs(out, exist_ok=True)
        zpath = os.path.join(out, zip_name)
        dt = time.gmtime(ct)[:6]
        members = [(man_name, man_text.encode('utf-8-sig' if bom else 'utf-8')),
                   ('releaseNotes.txt', rel_notes.encode('utf-8'))]
        if a.license:
            members.append(('license.txt', open(a.license, 'rb').read()))
        members += [(rel, data) for rel, data in sorted(zip(rels, blobs))]
        with zipfile.ZipFile(zpath, 'w', zipfile.ZIP_DEFLATED) as z:
            for name, data in members:
                zi = zipfile.ZipInfo(name, date_time=dt)
                zi.compress_type = zipfile.ZIP_DEFLATED
                zi.external_attr = 0o644 << 16
                z.writestr(zi, data)
        # read-back: zip bytes == disk bytes for every resx, member set == master set
        with zipfile.ZipFile(zpath) as z:
            names = z.namelist()
            zresx = sorted(n for n in names if n.endswith(f'.{a.culture}.resx'))
            if set(zresx) != set(rels):
                raise LangpackRefusal(f'zip != master: {sorted(set(zresx) ^ set(rels))[:8]}')
            for rel in rels:
                if sh(z.read(rel)) != file_hashes[rel]:
                    raise LangpackRefusal(f'zip octets != disque pour {rel}')
        zip_sha = sh(open(zpath, 'rb').read())
        man_bytes = man_text.encode('utf-8-sig' if bom else 'utf-8')
        with open(os.path.join(out, man_name), 'wb') as f:
            f.write(man_bytes)
        man_sha = sh(man_bytes)
        record = {
            'artifact': zip_name,
            'zip_sha256': zip_sha,
            'manifest': man_name,
            'manifest_sha256': man_sha,
            'master': short,
            'master_full': full,
            'built_at_utc': datetime.now(timezone.utc).strftime('%Y-%m-%dT%H:%M:%SZ'),
            'files': file_hashes,
            'counts': {'total': len(rels), 'common_with_upstream': common_n,
                       'added': added, 'excluded_upstream': excluded},
            'freshness_check': f"git -C <repo> log --oneline {short}..master -- '*.{a.culture}.resx' must be empty",
        }
        spath = os.path.join(out, f'{a.culture}_langpack_master-{short}.build.json')
        with open(spath, 'w', encoding='utf-8') as f:
            json.dump(record, f, ensure_ascii=False, indent=2)
        print(f'zip: {zpath} ({os.path.getsize(zpath)} o)')
        print(f'zip sha256: {zip_sha}')
        print(f'manifest sha256: {man_sha}')
        print(f'sidecar: {spath}')
        return 0
    except LangpackRefusal as e:
        print(f'REFUS: {e}')
        return 1


# ------------------------------------------------------------- self-test -----
def run_selftest():
    ok = True
    clean_man = ['App_GlobalResources/A.resx', 'DesktopModules/X/B.resx']
    clean_payload = set(clean_man)
    pairs_ok = [('App_GlobalResources', 'A.resx'), ('DesktopModules/X', 'B.resx')]
    man_ok = ('<dotnetnuke><packages><package><components><component><languageFiles>'
              '<languageFile><path>App_GlobalResources</path><name>A.resx</name></languageFile>'
              '</languageFiles></component></components></package></packages></dotnetnuke>')
    resx_ok = [('App_GlobalResources/A.resx', b'<root><data name="x"><value>1</value></data></root>')]
    # positive control: the cake must bake before "it refuses" proves anything
    try:
        check_sets(clean_man, clean_payload, clean_payload)
        check_no_backslash(pairs_ok)
        check_xml(man_ok, resx_ok)
        print('self-test fixture propre: PASS (les 3 controles le laissent passer)')
    except LangpackRefusal as e:
        print(f'self-test fixture propre: FAIL -> {e}')
        ok = False
    # 1. case-altered manifest entry
    try:
        check_sets(['App_GlobalResources/a.RESX'], clean_payload, clean_payload)
        print('self-test casse alteree: FAIL (non refuse)'); ok = False
    except LangpackRefusal as e:
        hint = 'VARIANTES DE CASSE' in str(e)
        print(f'self-test casse alteree: PASS (refuse, indice casse: {hint})')
        ok = ok and hint
    # 2. removed file from payload
    try:
        check_sets(clean_man, {'App_GlobalResources/A.resx'}, clean_payload)
        print('self-test fichier retire: FAIL (non refuse)'); ok = False
    except LangpackRefusal:
        print('self-test fichier retire: PASS (refuse)')
    # 3. backslash path
    try:
        check_no_backslash([('DesktopModules\\X', 'B.resx'), ('App_GlobalResources', 'A.resx')])
        print('self-test antislash: FAIL (non refuse)'); ok = False
    except LangpackRefusal:
        print('self-test antislash: PASS (refuse)')
    # 4. malformed resx
    try:
        check_xml(man_ok, [('App_GlobalResources/A.resx', b'<root><unclosed>')])
        print('self-test resx XML invalide: FAIL (non refuse)'); ok = False
    except LangpackRefusal:
        print('self-test resx XML invalide: PASS (refuse)')
    # 5. duplicate manifest entry
    try:
        check_sets(clean_man + [clean_man[0]], clean_payload, clean_payload)
        print('self-test doublon manifeste: FAIL (non refuse)'); ok = False
    except LangpackRefusal:
        print('self-test doublon manifeste: PASS (refuse)')
    print('SELF-TEST: ' + ('OK — 1 fixture propre passe, 5 mutations refusees' if ok else 'ECHEC'))
    return 0 if ok else 1


def main():
    p = argparse.ArgumentParser(description='Build a DNN language pack from a git commit (#458).')
    p.add_argument('--culture', help='e.g. ru-RU, pt-BR, zh-CN')
    p.add_argument('--commit', default='HEAD')
    p.add_argument('--repo', default=None)
    p.add_argument('--out', default=None, help='default <repo>/.tmp/langpack-out')
    p.add_argument('--upstream-manifest', default=None)
    p.add_argument('--license', default=None, help='file copied into the zip as license.txt')
    p.add_argument('--keep-license', action='store_true',
                   help='upstream mode: keep the upstream <license src="license.txt"/> reference')
    p.add_argument('--hashes-out', default=None)
    p.add_argument('--apply', action='store_true')
    p.add_argument('--self-test', action='store_true')
    a = p.parse_args()
    if a.self_test:
        return run_selftest()
    if not a.culture:
        p.error('--culture requis (ou --self-test)')
    return run_build(a)


if __name__ == '__main__':
    sys.exit(main())