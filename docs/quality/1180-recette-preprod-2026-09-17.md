# Recette préprod 2026-09-17 — écarts E4/E6/E7 soldés + état « préprod nickel »

**Auteur** : po-2023 (worker lane) · **Grain** : ⓪′ PRÉPROD NICKEL (dispatch ai-01 16/09 soir, GO owner première main — « il faut un site de preprod nickel, vérifié par vos soins d'abord », réunion Adeline 17/09 soir)
**Base de comparaison** : dossiers `1180-dossier-cutover-2026-09-16.md` (écarts E1-E11) et `1066-balayage-servi-depot-2026-09-15.md` (référentiel 36 routes, couche wrappers `de763aa9`)
**Environnement** : préprod `https://dnn.argumentum.myia.io` (IIS site 52, pool dédié, hairpin `--resolve …:443:127.0.0.1`), webroot = sous-arbre `DNNPlatform/` du checkout LIVE `ae84c91c` (en retard VOULU, jamais de pull)

## 1. Instrument

`curl --http1.1 --compressed -L` + cache-buster `?cb=` ; **identité = sha256 du corps décompressé** (`%{size_download}` avec `--compressed` = octets réseau, pas l'identité). Témoin mort 404 vérifié. ETag préprod NON dérivé du contenu (piège documenté #1066) — jamais utilisé comme preuve. Sorties brutes : `recette-0p/` (scratchpad, 60 corps).

## 2. Écarts soldés ce jour

### E4 + E7 — recâblage du lien OWL du menu (tab 171 « Ontologie fallacieuse »)

**Justification préal** : l'endpoint Pages sert l'artefact exact du blob `c9290bab` — mesuré avant d'agir :

| Endpoint | HTTP | octets | sha256[:12] |
|---|---|---:|---|
| `argumentumgames.github.io/Argumentum/docs/ontology/argumentum.owl` | 200 | 5 985 727 | `7435ae1c876f` |
| `…/argumentum_virtues.owl` | 200 | 1 103 945 | `032d58e99fcf` |

**Changement DB** (colonne `Tabs.Url`, DNN = liens externes du menu) :

```sql
SET QUOTED_IDENTIFIER ON;  -- requis : vues indexées/colonnes calculées sur Tabs, sinon Msg 1934
UPDATE Tabs SET Url = 'https://argumentumgames.github.io/Argumentum/docs/ontology/argumentum.owl',
  LastModifiedOnDate = GETDATE()
WHERE TabId = 171 AND Url = 'http://argumentum.games/argumentum_fallacies.owl';
```

**Rollback** (valeur pré-changement, LastModified était `2026-08-14 21:46:27.620`) :

```sql
SET QUOTED_IDENTIFIER ON;
UPDATE Tabs SET Url = 'http://argumentum.games/argumentum_fallacies.owl', LastModifiedOnDate = GETDATE() WHERE TabId = 171;
```

+ re-flush du cache DNN (voir méthode §2.3).

**Flush du cache DNN en mémoire** : recycle AppDomain via **mtime-touch de `web.config`** — SHA256 du contenu `B3ADCF76CF4CE8A4` et taille 97 508 o **identiques avant/après** (preuve que seul l'horodatage a bougé). `appcmd` non-élévé = exit 87 (artefact connu), `Restart-WebAppPool` indisponible dans la session — le mtime-touch est la méthode validée ici.

**Preuves post-changement (MESURÉ)** :
- corps home : href menu = `https://argumentumgames.github.io/Argumentum/docs/ontology/argumentum.owl` (l'ancien href `http://argumentum.games/argumentum_fallacies.owl` a disparu du corps) ; taille 83 440 → 83 490 o (+50 o = href menu plus long)
- `/tabid/171/` (forme friendly) → 302 → **200, 5 985 727 o, sha `7435ae1c876f`** = endpoint Pages, bit-exact
- avant changement : `/tabid/171/` servait le vestige `258a9e430e4f` (4 795 192 o) — l'ancienne cible

**E5 couvert** : l'endpoint Pages sert aussi `argumentum_virtues.owl` (`032d58e99fcf`) — le lien virtues du menu pointe déjà dessus via le même mécanisme.

**Vestige toujours servi** : `/argumentum_fallacies.owl` sur préprod répond toujours 200 (4 795 192 o, `258a9e430e4f`) — attendu, E4 ne recâble que le lien menu ; le fichier plat du webroot reste en place (couche E3 gelée).

### E6 — 2 SVG orphelins migrés vers le webroot préprod AVANT bascule

Copiés depuis des octets prod **frais** (re-téléchargés le jour même, pas depuis l'archive), sha = valeurs publiées (`docs/dnn/prod-webroot-orphans/README.md`), puis servis par préprod (**MESURÉ ce jour**) :

| Fichier | préprod | octets | sha256[:12] |
|---|---|---:|---|
| `fallacies_fr.links.svg` | 200 | 1 552 866 | `e47d5c6aa4e9` |
| `virtues_fr.svg` | 200 | 555 208 | `b6df4cab02b3` |

⚠️ Ces 2 fichiers sont désormais des **non-suivis** du checkout principal (webroot=checkout) — à intégrer à la couche servie lors de la prochaine capture/commit de couche, pas avant.

### E7 (volet lien absolu) — soldé par le même recâblage

Le lien absolu `http://argumentum.games/…` (mixed content + domaine apex) n'existe plus dans le menu servi.

## 3. Recette générale (17/09, cache-buster)

### Wrappers base ×16 (couche `de763aa9`) — 16/16 aux sha publiés 16/09

| | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| `fallacies_<l>.html` sha | `3b6b0f14` | `857bbe92` | `07361597` | `995aaa8e` | `9aec30f7` | `bf1a1183` | `0055dbca` | `0c11ccba` |
| octets | 2 361 730 | 2 216 473 | 2 643 818 | 2 361 323 | 2 302 920 | 2 359 846 | 2 458 296 | 4 968 179 |
| `virtues_<l>.html` sha | `86ce620a` | `c0a4a80f` | `f8f1a248` | `aae0cd29` | `bc8d4a6b` | `95901fc0` | `ed89eb49` | `5d747006` |
| octets | 512 022 | 498 023 | 566 278 | 524 796 | 509 833 | 542 081 | 542 694 | 1 225 370 |

Recoupement : `/tabid/170/` (Carte mentale fallacieuse, forme friendly) sert `3b6b0f14` = `fallacies_fr.html` bit-exact ✓.

### Wrappers `_ext` ×16 — 404 caractérisés (#1040)

Les 16 `fallacies_<l>_ext.html` / `virtues_<l>_ext.html` répondent **404** (corps 4 897-4 901 o, distincts par URL — la page DNN 404 embarque l'URL demandée ; même classe de taille que le témoin mort `__witness-dead__.html` 4 901 o). **Vraie absence** : les fichiers `_ext` ne sont pas déployés à la racine plate du webroot et 0 fichier correspondant sur disque. Fix éventuel = copier les 16 fichiers à la racine — **gated par le gel de couche E3**, décision owner.

### Autres routes

| Route | HTTP | octets | sha256[:12] |
|---|---|---:|---|
| `/` (home, post-recâblage) | 200 | 83 490 | `835b47293a24` |
| `/Resources/Libraries/jQuery/03_07_01/jquery.js` | 200 | 87 535 | `7aa6b0e08f48` |
| `/terms` | 200 | 61 257 | `10a9d720605f` |
| `/privacy` | 200 | 55 671 | `86a2975e457c` |
| zip cartes numériques | 200 | 50 354 291 | `d32aacc799ee` |
| zip A0 | 200 | 39 423 798 | `25d52dca77ed` |
| zip légères | 200 | 14 933 310 | `ec55856abf49` |
| zip mindmap | 200 | 6 417 247 | `6bde8c76c5c4` |
| `/argumentum_fallacies.owl` (vestige) | 200 | 4 795 192 | `258a9e430e4f` |

### Skin, CSP, GTM (E1 inchangé — owner-gated)

- CSP : entrées conditionnelles GTM/GA présentes (header home)
- Skin : `2shinebs5` ×6 occurrences, `Xcillion` ×0 — conforme à la peau active attendue
- Porteuses GTM (tabs 138/148/149/150/151/152/157/169/170/171 + home) : **GTM=1, GA=2, noscript=0 partout** — E1 (noscript GTM) reste ouvert, décision owner (dossier cutover 16/09)

## 4. Caractérisations (non-bloquant, documenté)

- **#674 (RulesExplorer i18n)** : MERGED 15/09 mais **pas déployé au runtime** — vues `Portals/1/2sxc/Argumentum/_RulesExplorer_{RuleList,RuleDetail}.cshtml` servies avec `Loc()=0, Culture=0` (pré-#674). Attendu : le checkout LIVE est en retard par conception ; déploiement = geste post-cutover.
- **`?tabid=N` ne redirige pas** : seule la forme friendly `/tabid/N/Default.aspx` déclenche la redirection DNN. Une sonde querystring rend une page DNN vide (83 468 o, titre distinct par tab) — **instrument, pas régression**.
- **ETag préprod non content-derived** : jamais utiliser l'ETag comme preuve d'identité/variation.

## 5. Preuves rasterisées (transport = prérequis, G: monté)

`G:\Mon Drive\Argumentum\recette-preprod-20260917\` — Edge headless (`--headless=new --screenshot --window-size=1280,1800 --virtual-time-budget=15000 --host-resolver-rules="MAP dnn.argumentum.myia.io 127.0.0.1"`) :

- `home.png` (309 796 o) — nav complète + hero + boutique + actus, rendu intégral
- `fallacies_fr.png` (74 664 o) — **mindmap fallacieuse intégralement rendue** (SVG radial complet ; 74 Ko = compression PNG d'un fond clair, pas un rendu partiel)
- `terms.png` (151 612 o) · `privacy.png` (155 612 o) — pages légales

## 6. État de préparation — bascule coordination roo-extensions

La coordination de la bascule est **portée par ai-01** (DM 17:12). De mon côté (po-2023) : préprod vérifié au sens du présent dossier, E4/E6/E7 soldés avec rollback documenté, transports (G:) opérationnels, 2 SVG non-suivis signalés à la prochaine capture de couche. **Rien ne bloque la bascule côté préprod.**

## 7. Ce que ce document n'établit pas

- ⛔ Aucun PASS — po-2023 signale ; **verdict visuel = ai-01**, décisions = jsboige
- E1 (noscript GTM) : inchangé, owner-gated
- E3 (gel de couche servie) : inchangé — les 2 SVG orphelins sont la seule addition, motivee E6
- E9 (recette re-jeu) : reste la coche D6 de la feuille #1417
- E10 (certs) : préprod expire 06/10, prod 04/11 — fenêtre DNS inchangée
- Pas de test de charge, pas de parcours boutique de bout en bout

---

*po-2023 (worker lane) · 17/09 · instruments : curl hairpin + sha256 corps décompressé, sqlcmd (QUOTED_IDENTIFIER ON), mtime-touch web.config (SHA prouvé), Edge headless · verdict : ai-01 · décisions : jsboige*
