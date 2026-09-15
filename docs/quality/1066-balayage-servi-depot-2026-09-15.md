# #1066 — Balayage servi↔dépôt (gate 2c consolidée) + localisation du porteur GTM/GA (2a)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-15 · **Base de lecture** : `origin/master` `6fbde739`
**Nature** : **lecture seule intégrale** — HTTP GET/HEAD (cache-buster), greps disque, SELECT SQL. Aucun geste
webroot, aucune écriture DB, aucune mutation DNN, aucun git dans le webroot.
**Portée** : grain ① du pool po-2023 (#458 c.`5666259809`, pioche 15/09 15:35). Ce dossier consolide 2c
après les passes du 18/08 (po-2023), 12/09 (po-2024, inventaire racine prod) et 14/09 (po-2023, OWL), et
**ferme le point 2a resté ouvert** : où vivent les balises GTM/GA.

---

## §0 — Ce qui était ouvert en entrant

- **2a** : « localiser le mécanisme porteur des balises GTM/GA » — les 4 tables de réglages avaient été
  sondées le 18/08 (négatif), le stockage restait « non documenté ».
- **2c** : balayage d'écart systématique — trois passes partielles existaient, aucune consolidée servi↔dépôt
  avec l'endpoint Pages désormais publié (#1391).

## §1 — Contrôles d'instrument (joués AVANT de croire un résultat)

| contrôle | résultat |
|---|---|
| **Témoin vivant** `/fallacies_fr.html` | **200** · 2 361 730 o ✓ |
| **Témoin mort** `/__witness-dead__.html` | **404** · 4 901 o (page 404 DNN) ✓ |
| **Contrôle positif GTM** | l'instrument retrouve seul `GTM-TZBQ57M`+`G-VHLTL18PEW` (HTTP 7 pages + localisation DB §2) ✓ |
| **Contrôle positif wrappers** | 16/16 présents racine webroot + 200 HTTP ✓ |
| Cache-buster | `?cb=20260915x` sur chaque sonde ✓ |
| Redirections | `-L` partout ; les tabs 170/171 redirigent réellement vers les fichiers statiques (§2.3) ✓ |
| NAT hairpin préprod | `--resolve dnn.argumentum.myia.io:443:127.0.0.1` ✓ |

**Deux faux négatifs de ce tick, corrigés en vol** (consignés parce que la classe sert le cutover) :
1. **URL de zip tronquée** → 404. Le vrai nom est `Argumentum_Arguments_Fallacieux_Cartes_numériques.zip`
   (préfixe complet) ; avec le nom entier : **200, Content-Length exact** (§5). Un 404 sur un nom abrégé
   n'est pas un artefact disparu.
2. **Comptes d'éléments XML non comparables** : `<NamedIndividual` compte des **usages** (10 976 servi),
   pas des termes ; et les deux générations d'OWL ne partagent **ni sérialisation ni modélisation** (§4).
   Toute comparaison doit nommer son unité.

## §2 — 2a FERMÉ : le porteur est `Tabs.PageHeadText` (champ DNN par page)

### 2.1 La mesure qui décide

Chasse exhaustive en base (`ArgumentumGames` @ `localhost\SQLEXPRESS`, login du web.config, SELECT lecture
seule) : **420 colonnes texte** des 164 tables sondées contre `%GTM-TZBQ57M%` :

> **un seul porteur : `[Tabs].[PageHeadText]` — 10 lignes.**

Nulle part ailleurs : ni `TabSettings` (4 clés seulement : AllowIndex, CustomStylesheet, DoNotRedirect,
LinkNewWindow), ni `PortalSettings`/`HostSettings`/`ModuleSettings`/`TabModuleSettings`/`HtmlText`, ni les
tables 2sxc. **Sur disque, le grep global du webroot (validé par contrôles positifs) rend 0** — ni le skin
`2shineBS5`, ni les apps `Portals/1/2sxc/` (1 045 fichiers), ni les wrappers.

### 2.2 Les 10 tabs porteurs (portal 1, aucun supprimé)

| TabId | TabName | rend la balise ? |
|---|---|---|
| 138 | Argumentum (`/`) | **oui** |
| 148 | Actus | **oui** |
| 149 | Acheter le jeu | **oui** |
| 150 | Téléchargements | **oui** |
| 151 | Amis | **oui** |
| 152 | Règles | **oui** |
| 169 | Argumentation | **oui** |
| 157 | Detail | page invisible au menu |
| 170 | Carte mentale fallacieuse | **non** — redirige vers `fallacies_fr.html` (tabid/170 → 200, 2 361 730 o, titre « Taxonomy Mind Map ») |
| 171 | Ontologie fallacieuse | **non** — redirige vers `argumentum_fallacies.owl` (tabid/171 → le fichier, 4 795 192 o) |

Le bloc `PageHeadText` contient **le snippet GTM + le snippet gtag complets** (blocs commentaires Google
canoniques), injectés dans `<head>` juste après les scripts 2sxc — mesuré sur la home servie.

### 2.3 Pourquoi trois sessions ne l'avaient pas trouvé

- Ce n'est **ni un fichier ni un réglage** : c'est une **colonne de `Tabs`** — hors de tout grep disque et
  de toute énumération de settings. La passe du 18/08 avait sondé `TabSettings` (table de paires clé-valeur)
  mais pas la colonne `Tabs.PageHeadText`.
- Les tabs 170/171 **redirigent** vers les artefacts statiques racine : `PageHeadText` n'y rend jamais —
  une sonde HTTP sur ces deux « pages » voit 0 balise **à tort comme conclusion** (l'instrument voit une
  redirection, pas un rendu).

### 2.4 `SiteAnalytics.config` : des templates, pas un porteur

Le fichier (racine webroot) définit **3 moteurs d'analytics DNN** (GoogleAnalyticsEngine Head,
GoogleTagManagerEngine Head+Body) mais avec des **placeholders** `[TRACKING_ID]` / `[GTM_ID]` — aucun
identifiant réel (cohérent avec le grep disque global négatif). Aucun ID n'existe dans aucune table de
réglages ⇒ **les moteurs sont dormants** ; le porteur effectif est et reste `PageHeadText`.

### 2.5 Résidus 2a (à porter à la checklist cutover, aucun geste ici)

- **`/terms` et `/privacy` n'ont pas le champ** (mesuré : 0/2) alors que la prod porte 9/9 (RAPPORTÉ 18/08
  ai-01). Écart préprod↔prod inchangé depuis le 18/08.
- **Le `<noscript>` GTM (iframe) reste absent** — le template Body existe dans `SiteAnalytics.config`
  mais le moteur est dormant ; le snippet dans `PageHeadText` ne couvre que le `<head>`.

**Implication migration** : le porteur est **du contenu DB** — il migre avec la base (backup/restore), à
l'inverse des dépôts fichiers racine qui ont déjà été perdus une fois (12/09). Vérification cutover par
sonde HTTP page à page (`GTM-TZBQ57M` ×1 + `G-VHLTL18PEW` ×2 par page rendue).

## §3 — Inventaire servi↔dépôt : racine du webroot préprod (31 fichiers)

| artefact servi | statut HTTP (aujourd'hui) | source dépôt | verdict |
|---|---|---|---|
| 16 wrappers `fallacies_{lang}.html` / `virtues_{lang}.html` | 200 ×16 (0,50→5,32 Mo) | `Cards/Fallacies/Mindmaps/{lang}/Fallacies_{lang}.html` + `Argumentation_Virtues_{lang}.html` (**famille `included`**) | servis = génération **`de763aa9` à l'octet** (preuve : `fallacies_fr.html` sha256 `3b6b0f14…` = blob `de763aa9`) ; le dépôt porte 6 commits de wrappers **en avance** (`56a9975d…`, 2 717 114 o) |
| `argumentum_fallacies.owl` | **200** · 4 795 192 o | ⛔ **le nom n'existe dans AUCUNE ref ni aucun commit** (`git log --all` vide) | artefact **non tracé** de mars-2024 (#1357) — voir §4 |
| `argumentum_virtues.owl` | **404** | `docs/ontology/argumentum_virtues.owl` = 1 103 945 o, **committé** | **défaut de déploiement** : dans le dépôt, servi nulle part (prod ET préprod) |
| 4 zips `Portals/1/Downloads/` | **200** ×4 (50 354 291 · 39 423 798 · 14 933 310 · 6 417 247 o, coquille `Argmentum_` comprise) | hors dépôt (downloads) | intacts — le 404 de première sonde était **mon URL tronquée** (§1) |
| `fallacies_fr.links.svg` (1 552 866 o) / `virtues_fr.svg` (555 208 o) | **404** préprod | ⛔ **orphelins prod-only, dans aucun commit** (po-2024 12/09) | servies en prod uniquement — **décision owner** (migrer telles quelles vs perdre) toujours pendante |
| fichiers plateforme (Default.aspx, configs DNN, favicon…) | — | plateforme DNN, périmètre #131 | hors périmètre 2c |

**Familles wrappers** (l'erreur déjà payée en #1047, revérifiée) : la racine sert bien la famille
**`included`** (0,50→5,32 Mo, `<svg>` inline, autoportante) ; la famille `_ext` (~89 Ko + compagnons
`.content.svg`/`.links.svg`) **n'est pas** déployée à la racine — conforme au geste 2026-08-10.

## §4 — OWL : trois états, une unité par compte

| artefact | taille | dialecte RDF | modélisation | comptes (unité nommée) |
|---|---:|---|---|---|
| **servi** `argumentum_fallacies.owl` | 4 795 192 o | OWL/XML **abbreviatedIRI** | termes = **NamedIndividual** (1 363 IRIs distincts ; 155 Class) | 2 610 assertions `skos:prefLabel` |
| **committé** `docs/ontology/argumentum.owl` (`6fbde739`) | 5 985 727 o | OWL/XML **IRI pleine** | termes = **Class** (1 489 IRIs distincts ; 715 NamedIndividual) | 2 816 assertions prefLabel |
| **committé** `argumentum_virtues.owl` | 1 103 945 o | OWL/XML IRI pleine | — | 446 prefLabels · **404 servi** |
| `argumentum_scenarii.owl` | — | — | — | **inexistant partout** (404 normal, pas un incident) |

- Les deux générations ne partagent **ni nom de fichier, ni sérialisation, ni modélisation** (individus →
  classes) : c'est la preuve matérielle qu'elles viennent de **deux chaînes différentes** — l'artefact servi
  est un vestige mars-2024 jamais régénéré ni tracé (#1357, corroboré 14/09). Comptes RAPPORTÉS au dossier
  #1357 : 1 405 publiés vs 1 440 committés (méthode IRIs distincts de l'époque) ; mes comptes du jour ci-dessus.
- **L'écart OWL pertinent s'est déplacé** : l'endpoint GitHub Pages publie désormais la génération committée
  à chaque push master (#1391, prouvé sha256 servi = blob) — le seul OWL divergent restant est **celui du
  webroot DNN**, jusqu'à décision de re-déploiement (ops gated).

## §5 — Zips `/Portals/1/Downloads/` : intacts

4/4 en **200** aux octets historiques exacts (HEAD, cache-buster). Le « 404 » de ma première sonde portait
un nom tronqué — l'erreur d'instrument du §1, pas un état du site.

## §6 — Implications cutover (checklist, aucun geste exécuté)

1. **Balises GTM/GA** : voyagent avec la **DB** (`Tabs.PageHeadText`) — rien à re-déposer côté fichiers ;
   vérifier par sonde HTTP page à page sur la machine cible. Écarts connus : `/terms` `/privacy` (champ
   absent préprod), `<noscript>` absent partout.
2. **Wrappers** : la préprod sert la génération `de763aa9` alors que le dépôt est en avance — au cutover,
   décider si on déploie la génération committée (re-geste 2b sur la famille `included`) ou on fige
   `de763aa9`.
3. **OWL** : re-déploiement gated (quel nom ? `argumentum_fallacies.owl` n'existe nulle part côté dépôt) ;
   `virtues.owl` committé jamais déployé — même décision.
4. **2 orphelins SVG** prod-only : décision owner pendante (migrer vs perdre) — les sha256 font foi
   (po-2024 12/09).
5. **Lien menu « Ontologie fallacieuse »** : toujours **absolu `http://argumentum.games/argumentum_fallacies.owl`**
   (domaine nu, http) dans le HTML servi — re-mesuré ce jour ; à recâbler en relatif au cutover (connu
   depuis le 18/08, inchangé).

## §7 — Ce que ce balayage n'établit PAS

- **Aucun verdict prod** au-delà des lectures HTTP déjà publiques : la DB prod n'est pas accessible depuis
  cette lane (le porteur `PageHeadText` y est **inféré par homologie**, pas mesuré).
- **Aucune cause** pour la non-traçabilité de l'OWL servi (qui l'a déposé, quand) — périmètre #1357.
- **Aucune décision** : déploiements, migrations d'orphelins, recâblage = gestes gated (owner/ai-01).
- Le contenu du OWL servi (PK 511 présent ou non) : non ouvert ce jour, périmètre OWL #133.

---

*po-2023 (worker lane) · base `origin/master` `6fbde739` · lecture seule : HTTP GET/HEAD + greps disque +
SELECT SQL (login du site, 0 écriture) · ⛔ aucun geste webroot, aucune mutation DB, aucun déploiement ·
verdict visuel : ai-01 · décisions prod : jsboige.*
