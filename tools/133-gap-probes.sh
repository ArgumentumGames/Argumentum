#!/usr/bin/env bash
# #133 — instrument de la table d'écart endpoint vs périmètre du ticket.
# Mesure ce que l'endpoint Pages rend AUJOURD'HUI : négociation de contenu,
# métadonnées, page lisible, versionnage, fraîcheur servi<->master.
# Lecture seule : aucune écriture distante, aucune modification d'IRI.
#
# Usage : bash tools/133-gap-probes.sh          (depuis un checkout du repo)
# Variante silencieuse/CI :  idem, exit 0 toujours — l'instrument MESURE,
# il ne juge pas (le verdict d'écart appartient à la table docs/quality/133-*.md).
set -u
BASE="${BASE:-https://argumentumgames.github.io/Argumentum/docs/ontology}"
TS=$(date +%s)
TMP="${TMPDIR:-/tmp}/arg133-probes"
mkdir -p "$TMP"

probe() { # $1=URL $2=Accept ("" = curl par défaut */*)
  if [ -n "$2" ]; then
    curl -sS -o /dev/null -w "%{http_code}\t%{content_type}\t%{size_download}" \
      --max-time 30 -H "Accept: $2" "$1?nc=$TS"
  else
    curl -sS -o /dev/null -w "%{http_code}\t%{content_type}\t%{size_download}" \
      --max-time 30 "$1?nc=$TS"
  fi
}

echo "== A. Negotiation de contenu (code / Content-Type / octets) =="
for f in argumentum.owl argumentum_virtues.owl; do
  for acc in "" "application/rdf+xml" "text/turtle" "text/html" "application/ld+json"; do
    printf "%s\tAccept[%s]\t" "$f" "${acc:-defaut}"
    probe "$BASE/$f" "$acc"; echo
  done
done

echo "== A'. Octets invariants quel que soit Accept ? (sha256 servis) =="
# Preuve negative : une couche de negotiation varierait les octets selon Accept.
for f in argumentum.owl argumentum_virtues.owl; do
  for acc in "application/rdf+xml" "text/turtle"; do
    printf "%s\t%s\t" "$f" "$acc"
    curl -sS --max-time 60 -H "Accept: $acc" "$BASE/$f?nc=$TS" | sha256sum | cut -c1-16
  done
done

echo "== B. Page lisible / WebVOWL =="
for u in "$BASE/" "$BASE/index.html" "$BASE/webvowl/" "$BASE/webvowl.html" "$BASE/README.md"; do
  printf "%s\t" "${u#"$BASE"/}"
  probe "$u" ""; echo
done

echo "== C. Versionnage semantique (copies versionnees) =="
for u in "$BASE/v1.0.0/argumentum.owl" "$BASE/v1/argumentum.owl"; do
  printf "%s\t" "${u#"$BASE"/}"
  probe "$u" ""; echo
done

echo "== D. Metadonnees dans le corps servi =="
curl -sS --max-time 90 "$BASE/argumentum.owl?nc=$TS" -o "$TMP/arg133.owl"
python - "$TMP/arg133.owl" <<'PY'
import re, sys
x = open(sys.argv[1], encoding='utf-8').read()
print("octets servis:", len(x.encode('utf-8')))
print("dcterms (purl.org/dc/terms/):", x.count('http://purl.org/dc/terms/'))
print("dc elements/1.1:", x.count('http://purl.org/dc/elements/1.1/'))
m = re.search(r'AnnotationProperty IRI="[^"]*versionInfo" />\s*<Literal[^>]*>([^<]*)</Literal>', x)
print("owl:versionInfo:", repr(m.group(1)) if m else "ABSENT")
print("rdfs:seeAlso assertions:", len(re.findall(r'seeAlso', x, re.I)))
print("rdf:Description:", x.count('rdf:Description'), "| racine rdf:RDF:", x.count('<rdf:RDF'))
m2 = re.search(r'<Ontology[^>]*ontologyIRI="([^"]+)"', x)
print("ontologyIRI:", m2.group(1) if m2 else "ABSENT")
lit = re.findall(r'seeAlso[^>]*/>\s*<IRI>[^<]*</IRI>\s*<Literal[^>]*>([^<]*)</Literal>', x)
ext = [t for t in lit if 'wikipedia.org' in t]
srv = [t for t in lit if 'github.io' in t or 'argumentumgames' in t]
print(f"seeAlso Litteraux: {len(lit)} | wikipedia: {len(ext)} | vers endpoint servi: {len(srv)}")
PY

echo "== E. Fraicheur servi <-> master (git cat-file) =="
for f in argumentum.owl argumentum_virtues.owl; do
  blob=$(git cat-file blob "origin/master:docs/ontology/$f" 2>/dev/null | sha256sum | cut -c1-16)
  srv=$(curl -sS --max-time 90 "$BASE/$f" | sha256sum | cut -c1-16)
  if [ -n "$blob" ]; then
    [ "$blob" = "$srv" ] && v=IDENTIQUE || v=DIFFERENT
  else
    v="blob introuvable (origin/master pas fetch ?)"
  fi
  echo "$f : servi=$srv master=$blob $v"
done
