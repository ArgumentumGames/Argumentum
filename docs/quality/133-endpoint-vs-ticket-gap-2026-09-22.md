# #133 — Table d'écart : endpoint servi vs périmètre du ticket (mesuré au `curl`)

**Date** : 2026-09-22 ~07:45 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `762848a6`
**Nature** : **mesure uniquement** — « mesurer l'écart ≠ le combler ». ⛔ **Aucune modification d'IRI** : #133 fige
les IRI, c'est une décision owner (pool po-2023, #458 c.5770154132 grain ④).
**Endpoint** : `https://argumentumgames.github.io/Argumentum/docs/ontology/` (posé par PR #1391, smoke `ab12e897`).

---

## §1 — Le périmètre du ticket, item par item, confronté au servi (MESURÉ 22/09)

Chaque ligne porte le **code HTTP et le `Content-Type` constatés**. Instrument : `tools/133-gap-probes.sh`
(batterie `curl` + parsing, cache-buster `?nc=<ts>` sur chaque sonde).

| Item du ticket (#133 Scope) | Demandé | Mesuré aujourd'hui | Verdict d'écart |
|---|---|---|---|
| *Choose hosting strategy* | GitHub Pages / URL dédiée / URI persistante | GitHub Pages ; `argumentum.owl` **200 `application/rdf+xml`** 5 985 727 o ; `argumentum_virtues.owl` **200 `application/rdf+xml`** 1 104 223 o ; **sha256 servis == blobs `origin/master` (`762848a6`), identiques** | **COMBLÉ** (re-mesuré 22/09 : l'auto-republication a suivi les merges du 21/09 — le sha virtues `3487b0c8…` ≠ celui du smoke 15/09 `032d58e9…` correspond au corpus d'aujourd'hui) |
| *Set up content negotiation* (HTML vs RDF/XML vs Turtle selon `Accept:`) | représentations multiples | `Accept` défaut / `application/rdf+xml` / `text/turtle` / `text/html` / `application/ld+json` : **même 200, même `application/rdf+xml`, mêmes octets** (sha256 `7435ae1c…` / `3487b0c8…` invariants) | **NON COUVERT** — hôte statique, `Accept` ignoré ; une négociation exigerait une couche dédiée (hors Pages) |
| *dcterms* (metadata) | annotations Dublin Core Terms | `http://purl.org/dc/terms/` : **0 occurrence** dans le corps servi ; seule annotation DC = `dc:creator = "Argumentum"` (`elements/1.1`, ×1) | **NON COUVERT** — DC Elements seulement, zéro dcterms |
| *owl:versionInfo* (metadata) | présent | **présent**, Literal **`1.0.0`** (×1) ; bump prévu au tag de release (#999) | **COMBLÉ** (valeur à bumper au tag — déjà staged) |
| *rdfs:seeAlso* (metadata) | pont namespace → endpoint | **×1 970 assertions** (littéraux) ; **1 451** vers Wikipedia (ex. `https://en.wikipedia.org/wiki/Fallacy`), 519 références externes autres ; **0** vers l'URL servie | **PARTIEL** — `seeAlso` documente les concepts, ne ponte pas l'endpoint (changement générateur, hors périmètre, déjà déclaré README §6) |
| *Register namespace URI* | IRI de namespace enregistrée | `ontologyIRI="https://www.argumentum.games/argumentum_fallacies.owl#"` déclarée dans le corps ; l'enregistrement et **tout** changement d'IRI = décision owner | **BLOQUÉ owner** — IRIs figées ; le pont IRI → URL servie n'existe pas (cf. ci-dessus) |
| *Human-readable documentation page (WebVOWL)* | page lisible | `/docs/ontology/` **404 `text/html; charset=utf-8`** (9 379 o) · `index.html` **404** · `webvowl/` **404** · `webvowl.html` **404** ; `README.md` **200 `text/markdown; charset=utf-8`** 12 746 o (source brut, non rendu) | **NON COUVERT** — aucune page HTML rendue ; les 404 portent tous le corps d'erreur générique de l'hôte (9 379 o ≡ témoin mort du smoke #1391) |
| *Version the ontology with semantic versioning* | semver | seul marqueur = `owl:versionInfo 1.0.0` ; copies versionnées `v1.0.0/argumentum.owl` **404** · `v1/argumentum.owl` **404** | **NON COUVERT** (staged : bump au tag + copies `v{semver}/`) |

### Deliverables du ticket, mêmes instruments

| Deliverable (#133) | Mesuré | Verdict |
|---|---|---|
| Public URL serving the OWL file | 2 × **200 `application/rdf+xml`**, sha == master | **COMBLÉ** |
| Human-readable HTML documentation | que du `text/markdown` brut (README) ; tout HTML = 404 | **NON COUVERT** |
| Content negotiation configured | octets invariants quel que soit `Accept` | **NON COUVERT** |
| README section documenting the endpoint | `README.md` **200** — §6 documente l'endpoint **et ses limites** (annonce déjà corrigée : `rdf+xml` constaté, l.160-164) | **COMBLÉ** |

### Écart supplémentaire découvert par la mesure (absent du ticket)

| Constat | Mesure | Conséquence |
|---|---|---|
| **`Content-Type` ≠ sérialisation du corps** | servi `application/rdf+xml`, mais corps = **OWL/XML** : racine `<Ontology ontologyIRI=…>`, **0** `rdf:Description`, **0** `rdf:RDF` ; recensement : `AnnotationAssertion` ×17 192 · `IRI` ×23 821 · `Literal` ×10 566 | un consommateur strictement négocié **RDF/XML** (parseur RDF/XML vanille) échoue sur ce corps ; le consommateur correct lit de l'OWL/XML. Le README le déclare déjà (« Format: OWL/XML », l.10) ; le `Content-Type` est celui de l'hôte. ⛔ Rien changé ici — constat à porter à l'arbitrage |

---

## §2 — Instrument et contrôles

- **Batterie** : `tools/133-gap-probes.sh` — `curl` une sonde par (URL × `Accept`), relevés code + `Content-Type` + taille, sha256 des corps, grep de métadonnées, sondes pages/version, comparaison fraîcheur servi ↔ `git cat-file origin/master:…`.
- **Cache-buster** : `?nc=<timestamp>` sur chaque sonde (cache Fastly/Pages) — résultats identiques avec/sans.
- **Contrôle « négociation absente »** : si une couche de négociation existait, les octets **varieraient** selon `Accept` ; mesuré **invariants** — c'est la preuve négative, pas une absence de sonde.
- **Témoin mort** : tout 404 rend 9 379 o — corps d'erreur générique de l'hôte, ≡ témoin `b6205073…` du smoke #1391 : un 404 ici est indiscernable d'un chemin absent, seule la taille du 200 fait foi.
- **Fraîcheur** : sha256 des deux corps servis comparés aux blobs `origin/master` (`762848a6`, fetch 22/09 ~07:40) — **identiques**, mesures donc ancrées au master du jour.

## §3 — Ce que cette table n'établit pas

- Aucune mesure de la **prod DNN** (gel #972) : l'écart webroot mars-2024 ↔ endpoint est déjà documenté (`133-iri-impact-before-owl-publication.md`, README §6).
- Aucune **parsabilité** vérifiée par un parseur OWL réel (Jena/OWL API) : la lecture du corps est syntaxique (grep/regex ancrés aux structures OWL/XML), pas une désérialisation complète.
- Aucune **décision** : comblement des écarts (couche de négociation, page WebVOWL, dcterms, semver, pont IRI) = arbitrages owner/ai-01 ; les IRI restent figées.
- Le tableau reflète **l'instant 22/09 ~07:45** : l'endpoint étant republié à chaque push master, les tailles/sha sont périssables par construction (la propriété « servi == master » est la seule ligne durable).

---
*po-2023 (worker lane) · instrument : `tools/133-gap-probes.sh` · verdict/arbitrage : ai-01 & jsboige*
