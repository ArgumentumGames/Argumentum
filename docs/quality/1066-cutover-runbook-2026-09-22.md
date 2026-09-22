# #1066 — Runbook de cutover : chaque item re-mesuré, avec geste, contrôle, rollback

**Date** : 2026-09-22 ~10:00 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `762848a6`
**Nature** : re-mesure préprod (lecture seule, cache-buster) + runbook. ⛔ **Zéro écriture prod** (gel #972) ;
⛔ pas de `/MIR`, pas de synchronisation de répertoire, pas de `git checkout` sur un webroot, ⛔ jamais copier
le `web.config` préprod → prod.
**Instrument** : `curl` — préprod en hairpin (`--resolve dnn.argumentum.myia.io:443:127.0.0.1`), prod avec
`-L` (pages redirigeantes : sans `-L`, corps vide lu comme « absent »). Cache-buster `?nc=<ts>` sur CHAQUE
sonde (cache prod = 1 an). Statiques sondés SANS query string (un `?v=` tombe dans le routage DNN).

---

## État re-mesuré du 22/09 — table pivot

| Item | Préprod (22/09) | Prod (22/09, lecture) | Verdict |
|---|---|---|---|
| E12 `github-mark.png` | **200 · 6 393 o** | 200 · 6 393 o | **tenu** (posé 19/09) |
| E12 `Open-Store.png` statique | **200 · 40 766 o** | 200 · 40 766 o | **tenu** |
| E12 `Open-Store.png` dérivé `?w=800&h=480…` | **200 · 14 763 o** | 200 · 14 708 o | tenu (dérivé ≠ octets par construction : 2 builds ImageFlow) |
| Wrappers ×16 (`{fallacies,virtues}_{8 langues}.html`) | **16/16 × 200** (0,50 → 5,32 Mo) | 200 ×16 | **tenu** (réparé 12/09) |
| OWL `argumentum_fallacies.owl` | **200 · 4 795 192 o** | 200 · 4 795 192 o | écart servi↔dépôt inchangé (artefact mars-2024 non tracé) |
| OWL `argumentum_virtues.owl` | **404** | 404 | **défaut de déploiement** (existe au dépôt, servi nulle part) |
| OWL `argumentum_scenarii.owl` | 404 | 404 | normal (n'existe nulle part) |
| GTM/GA balises | **7/9 pages à parité** (`GTM-TZBQ57M` + `G-VHLTL18PEW`) ; **`/terms` + `/privacy` : GTM sans `G-VHLTL18PEW`** | 9/9 portent les deux IDs | **état CHANGÉ depuis le 11/08** (0×9 → 7×9) ; gap résiduel = 2 pages |
| Balayage 2c (1ᵉʳ passage, ce runbook) | 23 candidats servables sondés | — | **0 nouveau gap de la classe E12** (404/404 ×12 clés ; témoins vivants 200/200) |

⚠️ **Changement d'état à signaler** : la mesure d'origine (11/08) disait balises **0 × 9/9** en préprod. Le
22/09, la préprod porte les DEUX IDs sur 7/9 pages — cohérent avec le chantier #1180 (E1 : seule la colonne
DB `PortalSettings.PageHeadText` rend). **Le volet 2a n'est plus « migrer les balises » mais « compléter
2 pages ».** (Bruit d'instrument écarté : `G-S` matché une fois sur actus prod = regex sur un fragment de
texte, pas un ID ; les IDs réels sont les deux ci-dessus.)

---

## Le runbook — un geste, un contrôle, un rollback par item

### R1 — Les 2 fichiers E12 (causes É2)

- **Geste** : copie des **2 fichiers nommés** (et rien d'autre) depuis prod fraîchement lus (`curl`, cache-buster)
  vers le webroot cible : `Portals/1/Images/github-mark.png` (6 393 o, mtime prod 2023-02-27 10:20:57Z) et
  `Portals/1/adam/News5/4bLeKJXdFUGdPtYu4lCPnA/Image/Open-Store.png` (**source brute 40 766 o**, pas le dérivé
  14 708 o — déposer la source reproduit les DEUX formes d'URL, le handler dérive à la volée).
- **Contrôle** : `sha256` cible == `sha256` prod lu au même jour · HTTP **200 des deux côtés** (statique SANS
  query string ; dérivé AVEC `?w=800&h=480&quality=80&mode=crop`) · re-jeu É2 home/actus : les bandes causales
  de 1 148 px doivent disparaître (**contrôle inverse : si l'écart persiste, la cause était fausse — le dire**).
- **Rollback** : supprimer les 2 entrées (fichier + répertoire GUID pour le second). Aucun autre levier.
- **État** : ✅ déjà posé en préprod (19/09, E12) et re-mesuré tenu le 22/09. À rejouer tel quel au cutover.

### R2 — Les 16 wrappers mindmap

- **Geste** : déposer la famille **`included`** (~2,4 Mo, `<svg>` inline, autoportante) — ⛔ **PAS `_ext`**
  (~89 Ko, `<object>` → `.content.svg`, 3 compagnons par wrapper ; la mauvaise famille = 16 pages vides) —
  depuis `Cards/Fallacies/Mindmaps/{lang}/` + équivalent Vertus, avec liens versionnés (`?v=`).
- **Contrôle** : 16/16 en **200** avec tailles substantielles (0,50 → 5,32 Mo) ; les 2 liens de
  `/Acheter-le-jeu` (« taxonomie » → `fallacies_fr.html`, « plusieurs langues » → `fallacies_en.html`) résolvent.
- **Rollback** : retirer les 16 fichiers.
- **État** : ✅ tenu en préprod (re-mesuré 22/09). ⚠️ résiduel mesuré : `fallacies_fr.html` préprod 2 717 913 o
  vs prod 2 717 114 o (+799 o, 0,03 %) — wrappers déposés séparément, classe pré-existante, pas un gap neuf.
  Au cutover : la préprod EST la source (16/16 servie), pas la copie Phase 1.

### R3 — OWL (gaté)

- **Geste** (⛔ ops gatées #133/#134, pas au runbook de bascule) : déployer `docs/ontology/argumentum_virtues.owl`
  (1 103 945 o au dépôt — servi nulle part aujourd'hui) et republier `argumentum_fallacies.owl` depuis le dépôt
  (le servi = artefact mars-2024 non tracé de 4 795 192 o).
- **Contrôle** : sha256 servi == blob `origin/master` (méthode du smoke #1391).
- **Rollback** : retirer le fichier déployé (l'artefact mars-2024 reste en prod tant que non remplacé).
- **État** : inchangé au 22/09 (200/404/404 mesurés ci-dessus).

### R4 — Balises GTM/GA : compléter `/terms` + `/privacy`

- **Geste** : ajouter l'ID GA4 `G-VHLTL18PEW` sur les 2 pages, **par le même mécanisme que les 7 autres**
  (colonne DB `PortalSettings.PageHeadText` — seule voie qui rend, E1 #1180 ; TabSettings per-page NE rend PAS).
- **Contrôle** : 9/9 pages préprod portent `GTM-TZBQ57M` **et** `G-VHLTL18PEW` (curl -L + grep) ; **0 violation
  CSP runtime** (sonde navigateur : les 3 entrées `conditional` de PR #1065 doivent être actives —
  `region1.google-analytics.com` est injecté par GTM à l'exécution, invisible au curl).
- **Rollback** : retirer la ligne ajoutée dans PageHeadText des 2 pages.
- **État** : 7/9 mesuré ce jour ; les CSP conditional sont actives préprod (2c état 19/08).

### R5 — Balayage 2c : méthode (rejouable J-1 du cutover)

1. Énumérer les fichiers servables du webroot **source de vérité du jour** (au 22/09 : copie Phase 1
   `D:\DNN\live\dnn.argumentum.myia.io\` — datée ; au cutover, la préprod réparée EST la meilleure source).
2. Exclure : `App_Data`, `Logs`, `bin*`, `*.config`, `*.bak*`, `*.broken*`, `.pdb/.dll/.deps.json`.
3. Diff disque → candidats ; sonder chaque candidat en HTTP **sur prod** (lecture seule, `-L`, cache-buster).
4. **Seul prod-200 + cible-404 = gap de cutover** (la classe E12). Tout le reste n'est pas un gap.
5. **Témoins obligatoires à chaque passe** : un chemin connu-vivant des deux côtés (`github-mark.png`,
   `fallacies_fr.html` — 200/200 mesurés ce jour) ; un uniforme N/N est un signal d'instrument avant d'être un résultat.

**Résultat du 1ᵉʳ passage (22/09)** : 9 376 fichiers comparés → 23 candidats servables → 12 clés sondés :
**404/404 ×12** (EditBar css, PersonaBar Styles, ViewProfile, connector GoogleAnalytics4 ×3, librairies jQuery
×6, `default.css` dnndefault…) ⇒ **aucun 3ᵉ exemplaire de la classe E12 dans ce jeu**. Les librairies DNN
(jQuery…) ne sont pas servies en chemin direct (combiner `DependencyHandler.axd`) — leur absence du disque
préprod n'est donc PAS un gap servi. **Bornes** : copie Phase 1 datée (10/08), chemins directs uniquement
(assets servis via combiner ou DB non couverts), bin/config exclus volontairement.

### R6 — Interdictions transverses (toujours vraies)

⛔ Zéro écriture prod (#972) · ⛔ `/MIR` et synchronisation de répertoire (divergence webroot↔dépôt
**bidirectionnelle** : un miroir détruit ce qui ne vit que d'un côté) · ⛔ `git checkout` sur un webroot ·
⛔ copie du `web.config` préprod → prod · cache-buster sur chaque sonde · `-L` systématique · statiques sans
query string · jamais conclure d'une sonde unique (résultat uniforme = signal d'instrument).

## Ce que ce runbook n'établit pas

- **R5 couvre son jeu de candidats, pas l'univers** : une source plus fraîche que la copie Phase 1 (10/08)
  peut révéler d'autres fichiers ; la passe J-1 du cutover devra la rejouer depuis l'état du jour.
- La **date/cause** du retrait de `github-mark.png` du webroot vivant reste non établie (constaté #1049).
- Le **mécanisme porteur historique** des balises en prod (skin ? module ? PageHeadText prod ?) n'est pas
  audité ici — seuls les IDs servis et leur parité le sont.
- Aucun verdict visuel (ai-01) ; aucun geste exécuté sur la prod.

---
*po-2023 (worker lane) · instruments : curl hairpin préprod + `-L` prod, cache-buster ; sweep disque Python
(os.walk + filtre servables) · verdict/arbitrage : ai-01 & jsboige*
