#!/usr/bin/env python3
"""Contrat offline de tools/2190-served-mindmap-inventory.py.

Std-lib uniquement, dépôt git jetable, serveur HTTP local : aucune préprod, aucun
accès réseau externe. Le témoin doit être IDENTIQUE au blob ; la réponse mutée
doit diverger ; une route faussement nommée doit rendre l'inventaire rouge.
"""
import importlib.util
import json
import subprocess
import sys
import tempfile
import threading
import urllib.request
from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path

REPO = Path(__file__).resolve().parents[1]
spec = importlib.util.spec_from_file_location(
    "inventory2190", REPO / "tools/2190-served-mindmap-inventory.py")
mod = importlib.util.module_from_spec(spec)
spec.loader.exec_module(mod)
failures = []


def case(name, condition, detail=""):
    print(f"  {name}: {'OK' if condition else 'ROUGE — ' + detail}")
    if not condition:
        failures.append(name)


class Handler(BaseHTTPRequestHandler):
    routes = {}

    def do_GET(self):
        body = self.routes.get(self.path)
        if body is None:
            self.send_error(404)
            return
        self.send_response(200)
        self.send_header("Content-Type", "text/html")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def log_message(self, *args):
        pass


with tempfile.TemporaryDirectory(prefix="t2190-inventory-") as td:
    repo = Path(td) / "repo"
    repo.mkdir()
    subprocess.run(["git", "init", "-q", str(repo)], check=True)
    subprocess.run(["git", "-C", str(repo), "config", "user.email", "test@example.invalid"], check=True)
    subprocess.run(["git", "-C", str(repo), "config", "user.name", "test"], check=True)
    Handler.routes = {}
    for family, route_pattern, file_pattern in mod.TARGETS:
        for lang in mod.LANGS:
            route = route_pattern.format(lang=lang)
            path = repo / "Cards/Fallacies/Mindmaps" / lang / file_pattern.format(lang=lang)
            path.parent.mkdir(parents=True, exist_ok=True)
            body = (f'<input id="mmSearch"><script>var MM_UI = {{}};</script>'
                    f'<g data-crosslink-source="7"></g> {family} {lang}').encode()
            path.write_bytes(body)
            Handler.routes["/" + route] = body
    subprocess.run(["git", "-C", str(repo), "add", "."], check=True)
    subprocess.run(["git", "-C", str(repo), "commit", "-qm", "fixture"], check=True)

    server = ThreadingHTTPServer(("127.0.0.1", 0), Handler)
    threading.Thread(target=server.serve_forever, daemon=True).start()
    base = f"http://127.0.0.1:{server.server_port}"
    out = Path(td) / "inventory.json"
    try:
        sys.argv = ["inventory", "--repo", str(repo), "--ref", "HEAD",
                    "--base", base, "--output", str(out)]
        case("1 témoin 16/16 identique", mod.main() == 0)
        data = json.loads(out.read_text(encoding="utf-8"))
        case("1b 16 routes et marqueurs vus",
             len(data["entries"]) == 16
             and all(e["same_bytes_as_blob"] and e["search_input_count"] == 1
                     and e["mm_ui_present"] and e["crosslink_source_count"] == 1
                     for e in data["entries"]))
        case("1c sonde HTML : casse, entité, guillemets et self-closing",
             mod.navigation_markers(
                 b'<INPUT ID=mmSearch /><g DATA-CROSSLINK-SOURCE="a&#x2D;b"></g>'
             ) == {"search_input_count": 1, "mm_ui_present": False,
                   "crosslink_source_count": 1})
        case("1d un commentaire ou du texte ne fabrique pas de marqueur",
             mod.navigation_markers(
                 b'<!-- id="mmSearch" --><script>"id=mmSearch"</script>'
             )["search_input_count"] == 0)

        Handler.routes["/fallacies_fr.html"] += b"mutation"
        case("2 octets mutés -> divergence", mod.main() == 0)
        data = json.loads(out.read_text(encoding="utf-8"))
        changed = [e for e in data["entries"]
                   if e["family"] == "fallacies" and e["lang"] == "fr"]
        case("2b seule la route mutée diverge",
             len(changed) == 1 and not changed[0]["same_bytes_as_blob"]
             and sum(not e["same_bytes_as_blob"] for e in data["entries"]) == 1)

        Handler.routes["/fallacies_fr.html"] = Handler.routes["/fallacies_fr.html"][:-8]
        del Handler.routes["/Virtues_fr.html"]
        case("3 page absente -> exit 1", mod.main() == 1)
    finally:
        server.shutdown()
        server.server_close()

if failures:
    print("CONTRAT ROUGE :", failures)
    sys.exit(1)
print("CONTRAT VERT : identité, divergence et absence sont distinguées.")
