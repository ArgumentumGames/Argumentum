#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Temoins des sondes de polices (#1485) -- cle de cache, glyphe nul, famille vide.

    python -I tools/fonts/test_font-coverage.py

POURQUOI. Trois defauts de `font-coverage.py` ont ete releves en contre-revue
(ai-01, c.7191872553, deux bloquants ; po-2024, c.7183335177, un residu). Tous
les trois ont la meme forme : l'instrument rend un chiffre la ou il n'a rien
mesure.

  1. `fetch` indexait son cache par l'ETIQUETTE seule (`reference`,
     `barlow Bold`). Deux URL differentes sous une meme etiquette se servaient
     donc les octets l'une de l'autre : la seconde police etait rapportee sous
     les octets de la premiere, avec un seul telechargement. La cle est
     desormais l'URL.
  2. `cmap_codepoints`, format 12 : un groupe dont le premier glyph id vaut 0
     commence sur `.notdef` (glyphe 0, qui ne rend rien). Son premier point de
     code n'est donc pas couvert ; les suivants le sont.
  3. `family=` qui ne se deplie en aucun fichier -- `METADATA.pb` ne listant ni
     statique Light/Medium/Bold/Black ni variable -- ne produisait aucune ligne.
     Une absence muette, que le docstring promet de dire.

CE QUI EST EPINGLE (8 cas)
  - deux URL sous la MEME etiquette : deux telechargements, deux contenus (1)
  - une meme URL reutilisee : un seul telechargement (tenu depuis toujours)
  - format 12, groupe [U+0041,U+0042] de premier glyph id 0 : seul U+0042 couvre
  - format 12, groupe singleton [U+0041,U+0041] de glyph id 0 : rien ne couvre
  - CONTROLE : un groupe de premier glyph id non nul garde son premier point --
    une regle qui ignorerait TOUJOURS le premier point rougirait ici
  - CONTROLE : format 4 et format 12 s'accordent sur un point mappe a `.notdef`
    (le format 4 tenait deja compte du glyphe nul ; le format 12 divergeait)
  - bout en bout : `main()` avec un `urlopen` simule rend une ligne `ERREUR`
    pour la famille vide
  - CONTROLE : la famille saine du MEME appel garde sa rangee -- un outil qui
    refuserait desormais toute famille ne passerait pas le cas precedent

CONDITION D'EXECUTION (cf. en-tete de .github/workflows/python-tools-tests.yml)
  std-lib (importlib, struct, unittest), OFFLINE : `urlopen` est remplace par un
  faux qui REFUSE toute URL non attendue ; les polices sont synthetiques (un
  sfnt nu de trois tables construit ici) ; le corpus est lu dans le checkout
  (`--ref HEAD`, `git` present sur le runner) pour le seul cas de bout en bout.
  Aucun artefact, aucun Target/, aucun octet telecharge, aucune ecriture hors
  d'un repertoire temporaire.
"""
import contextlib
import importlib.util
import io
import struct
import sys
import tempfile
import unittest
from pathlib import Path
from unittest import mock

HERE = Path(__file__).resolve().parent
ROOT = HERE.parent.parent

_spec = importlib.util.spec_from_file_location("font_coverage", HERE / "font-coverage.py")
fc = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(fc)

URL_ONE = "https://exemple.invalid/one.woff"
URL_TWO = "https://exemple.invalid/two.woff"
GOOGLE = "https://raw.githubusercontent.com/google/fonts/main/ofl/"


# ------------------------------------------------------------------ faux reseau
class _Response:
    def __init__(self, body):
        self._body = body

    def read(self):
        return self._body

    def __enter__(self):
        return self

    def __exit__(self, *exc):
        return False


def _serve(pages):
    """Un `urlopen` qui rend `pages[url]` et REFUSE toute URL non attendue : une
    derive des telechargements echoue bruyamment, jamais en rendant un faux corps."""
    seen = []

    def opener(req, timeout=None):
        url = getattr(req, "full_url", req)
        seen.append(url)
        if url not in pages:
            raise AssertionError("URL non attendue : %s" % url)
        return _Response(pages[url])

    return opener, seen


# ------------------------------------------------------------------- cmap / sfnt
def _cmap(subtables):
    """Table cmap : sous-tables (plateforme, encodage, octets), decalees du bon pas."""
    offset = 4 + 8 * len(subtables)
    records, body = b"", b""
    for platform, encoding, data in subtables:
        records += struct.pack(">HHI", platform, encoding, offset)
        body += data
        offset += len(data)
    return struct.pack(">HH", 0, len(subtables)) + records + body


def _format12(groups):
    """Groupes (premier point, dernier point, premier glyph id)."""
    body = b"".join(struct.pack(">III", start, end, gid) for start, end, gid in groups)
    return struct.pack(">HHIII", 12, 0, 16 + len(body), 0, len(groups)) + body


def _format4(segments):
    """Segments (premier point, dernier point, idDelta) ; sentinelle 0xFFFF ajoutee."""
    segments = list(segments) + [(0xFFFF, 0xFFFF, 1)]
    count = len(segments)
    body = b"".join(struct.pack(">H", end) for _s, end, _d in segments)
    body += struct.pack(">H", 0)                                    # reservedPad
    body += b"".join(struct.pack(">H", start) for start, _e, _d in segments)
    body += b"".join(struct.pack(">h", delta) for _s, _e, delta in segments)
    body += b"".join(struct.pack(">H", 0) for _ in segments)        # idRangeOffset
    return struct.pack(">HHHHHHH", 4, 14 + len(body), 0, count * 2, 0, 0, 0) + body


def _font(cmap_bytes):
    """Un sfnt nu minimal : head (unitsPerEm), OS/2 v4 (graisse, x/em, cap/em), cmap.
    De quoi faire passer la police par `sfnt_tables`, `cmap_codepoints`, `axes`,
    `wght_reach` et `os2_metrics` sans un seul octet telecharge."""
    head = bytes(18) + struct.pack(">H", 1000) + bytes(34)
    os2 = struct.pack(">HH", 4, 700) + bytes(82) + struct.pack(">hh", 500, 700)
    tables = [(b"OS/2", os2), (b"cmap", cmap_bytes), (b"head", head)]
    header = b"\x00\x01\x00\x00" + struct.pack(">HHHH", len(tables), 0, 0, 0)
    offset = 12 + 16 * len(tables)
    records, body = b"", b""
    for tag, data in tables:
        records += tag + struct.pack(">III", 0, offset, len(data))
        body += data
        offset += len(data)
    return header + records + body


# --------------------------------------------------------------------- cas
class CleDeCacheParURL(unittest.TestCase):
    """Deux URL sous une meme etiquette ne se servent plus les octets l'une de
    l'autre ; une meme URL n'est telechargee qu'une fois."""

    def test_deux_url_sous_la_meme_etiquette_donnent_deux_telechargements(self):
        pairs = [fc.resolve("reference=" + URL_ONE, Path("."))[0],
                 fc.resolve("reference=" + URL_TWO, Path("."))[0]]
        self.assertEqual([label for label, _src in pairs], ["reference", "reference"])
        opener, seen = _serve({URL_ONE: b"one", URL_TWO: b"two"})
        with tempfile.TemporaryDirectory() as tmp, \
                mock.patch.object(fc.urllib.request, "urlopen", opener):
            cache = Path(tmp)
            got = [fc.fetch(src, cache) for _label, src in pairs]
        self.assertEqual(got, [b"one", b"two"])
        self.assertEqual(len(seen), 2)

    def test_meme_url_reutilisee_une_seule_fois(self):
        opener, seen = _serve({URL_ONE: b"one"})
        with tempfile.TemporaryDirectory() as tmp, \
                mock.patch.object(fc.urllib.request, "urlopen", opener):
            cache = Path(tmp)
            first = fc.fetch(URL_ONE, cache)
            again = fc.fetch(URL_ONE, cache)
        self.assertEqual((first, again), (b"one", b"one"))
        self.assertEqual(len(seen), 1)


class Format12GlypheNul(unittest.TestCase):
    """Le format 12 ne compte un point de code que si son glyphe n'est pas `.notdef`."""

    def test_premier_point_d_un_groupe_de_glyph_id_zero_n_est_pas_couvert(self):
        tables = {"cmap": _cmap([(3, 10, _format12([(0x41, 0x42, 0)]))])}
        self.assertEqual(fc.cmap_codepoints(tables), {0x42})

    def test_groupe_singleton_de_glyph_id_zero_ne_couvre_rien(self):
        tables = {"cmap": _cmap([(3, 10, _format12([(0x41, 0x41, 0)]))])}
        self.assertEqual(fc.cmap_codepoints(tables), set())

    def test_groupe_de_glyph_id_non_nul_garde_son_premier_point(self):
        tables = {"cmap": _cmap([(3, 10, _format12([(0x41, 0x42, 5)]))])}
        self.assertEqual(fc.cmap_codepoints(tables), {0x41, 0x42})

    def test_format_4_et_format_12_s_accordent_sur_un_point_notdef(self):
        # 0x41 -> glyphe 0, 0x42 -> glyphe 1, dans les deux formats.
        quatre = {"cmap": _cmap([(3, 1, _format4([(0x41, 0x42, -0x41)]))])}
        douze = {"cmap": _cmap([(3, 10, _format12([(0x41, 0x42, 0)]))])}
        self.assertEqual(fc.cmap_codepoints(quatre), {0x42})
        self.assertEqual(fc.cmap_codepoints(douze), {0x42})


class FamilleSansPolice(unittest.TestCase):
    """`family=` qui ne se deplie en rien est DIT ; une famille saine, mesuree dans
    le meme appel, garde sa rangee (controle : l'outil n'est pas devenu refractaire)."""

    def _run(self, specs):
        pages = {
            GOOGLE + "abel/METADATA.pb": b'filename: "Abel-Regular.ttf"\n',
            GOOGLE + "faux/METADATA.pb": b'filename: "Faux-Bold.ttf"\n',
            GOOGLE + "faux/Faux-Bold.ttf": _font(_cmap([(3, 10, _format12([(0x41, 0x42, 0)]))])),
        }
        opener, _seen = _serve(pages)
        out = io.StringIO()
        with tempfile.TemporaryDirectory() as tmp, \
                mock.patch.object(fc.urllib.request, "urlopen", opener), \
                mock.patch.object(sys, "argv", ["font-coverage.py", "--repo", str(ROOT), "--ref",
                                                "HEAD", "--cache", tmp] + specs), \
                contextlib.redirect_stdout(out):
            fc.main()
        return out.getvalue()

    def test_famille_vide_donne_une_ligne_erreur(self):
        text = self._run(["family=abel", "family=faux"])
        self.assertRegex(text, r"family=abel\s+ERREUR LookupError")

    def test_famille_saine_reste_mesuree_dans_le_meme_appel(self):
        text = self._run(["family=abel", "family=faux"])
        self.assertRegex(text, r"faux Bold\s+\d+\s+\d+\s")     # une rangee, avec des chiffres
        self.assertNotRegex(text, r"faux Bold\s+ERREUR")


if __name__ == "__main__":
    unittest.main(verbosity=2)
