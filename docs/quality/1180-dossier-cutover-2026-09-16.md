# #1180/#1066 — Dossier de cutover (lecture seule) : écarts, ordre des contrôles, rollback

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base de lecture** : `origin/master` `41b8add2`
**Nature** : **lecture seule intégrale** — aucune opération servante : aucun geste webroot, aucune écriture DB,
aucun déploiement, aucune bascule DNS, aucun tag. Le dossier **rend le cutover décidable** ; il ne l'exécute pas.
**Portée** : pool #458 c.`5666259809` grain ④. Sources : dossier #1066 consolidé (#1399, mergé — balayage
servi↔dépôt + porteur GTM), commentaires #1180 (recette É1-É7, incidents 08/09), états #1064 (CLOSED) /
#1049 (OPEN), mesures fraîches du jour (§0).
**Règle de fond** : le worker propose, jamais ne tranche. Chaque écart ci-dessous porte une **proposition**
qualifiée ; la colonne décision reste **owner/ai-01**. ⛔ Aucun PASS n'est émis ici.

---

## §0 — État mesuré du jour (16/09, préprod, cache-buster + NAT hairpin)

| sonde | résultat |
|---|---|
| `/` home | **200** · 83 440 o · skin `2shinebs5` ×6 · **GTM-TZBQ57M ×1** · G-VHLTL18PEW ×2 · `<noscript>` **0** |
| `/fallacies_fr.html` | **200** · 2 361 730 o (octets exacts de la génération `de763aa9`) |
| `/argumentum_fallacies.owl` | **200** · 4 795 192 o (le vestige mars-2024, #1357) |
| jQuery `03_07_01/jquery.js` | **200** · 87 535 o (les 404 du 08/09 sont réparés — pose 09/09) |
| zip `Argumentum_Arguments_Fallacieux_Cartes_numériques.zip` | **200** (HEAD) |

La préprod **sert** ; le seul résidu analytics mesuré est le **`<noscript>` GTM absent** (E1).

## §1 — Liste exhaustive des écarts (acceptés / non acceptés — propositions worker)

Le **plancher de sonde GTM** posé par le pool (×2 par page, observation NanoClaw + contre-review) sert
d'unité : en prod chaque page rend GTM-TZBQ57M **×2** (script head + iframe noscript) et GA ×2 ; la préprod
rend **×1 + GA ×2** tant que E1 n'est pas soldé.

| # | Écart | État mesuré (source) | Proposition worker | Décision |
|---|---|---|---|---|
| E1 | `<noscript>` GTM absent (iframe de repli) — la sonde plafonne à ×1 | préprod ×1/×2 GA, `<noscript>` 0 (mesuré 16/09 §0 ; ai-01 26/08 : prod = ×2) | **SOLDER avant cutover** : le plancher ×2 est la définition même du contrôle de bascule. Voie : le snippet vit dans `Tabs.PageHeadText` (DB, #1399 §2) — l'ajout du noscript = 10 UPDATE DB **gated owner** (fenêtre), OU accepter ×1 temporaire et re-qualifier le plancher à ×1 script + ×2 GA (décision à écrire noir sur blanc si choisie) | ☐ owner/ai-01 |
| E2 | `/terms` et `/privacy` sans `PageHeadText` | 0/2 préprod vs 9/9 prod (RAPPORTÉ ai-01 18/08 ; inchangé #1399 §2.5) | **Accepter** au cutover (pages légales, trafic résiduel) + backlog post-cutover (2 UPDATE gated, même voie que E1) | ☐ |
| E3 | Wrappers racine : génération `de763aa9` servie, dépôt 6 commits en avance | octets exacts (mesuré 16/09 §0 ; #1399 §3) | **Figer `de763aa9` pour le cutover** (déjà re-déposée et vérifiée à l'octet le 12/09) ; déploiement de la génération committée = **post-cutover** via le runbook #1339 (le geste 2b sur la machine cible serait une première — ne pas l'empiler sur le jour J) | ☐ |
| E4 | OWL servi = vestige mars-2024 **non tracé** (dialecte + modélisation ≠ committé ; 2 610 vs 2 816 prefLabels) | 200 · 4 795 192 o servi (§0) ; aucun commit ne porte ce nom (#1399 §4, #1357) | **Non acceptable à terme** (le site annonce une ontologie vivante), non bloquant au cutover. 3 options : (a) re-déployer le committé sous le nom servi + recâbler, **(b) recâbler le menu vers l'endpoint GitHub Pages** (#1391 — publie la génération committée à chaque push, prouvé sha256 ; **zéro geste webroot**), (c) accepter le vestige. Reco : **(b) au cutover, (a) éventuellement après** | ☐ |
| E5 | `argumentum_virtues.owl` committé mais **404 servi** | #1399 §3/§4 | Couvert par E4 : l'option (b) le rend servi par l'endpoint Pages immédiatement ; l'option (a) le déploie au webroot | ☐ (E4) |
| E6 | 2 SVG orphelins **prod-only** (`fallacies_fr.links.svg`, `virtues_fr.svg`), dans aucun commit | 404 préprod ; sha256 publiés (po-2024 12/09) | Décision pendante owner : **migrer (sha256 font foi) ou perdre**. ⚠️ Si « migrer » : copier **avant** le cutover — l'ancien hôte n'est pas conservé indéfiniment après bascule ; « perdre » = décision écrite ici | ☐ |
| E7 | Lien menu « Ontologie fallacieuse » **absolu `http://argumentum.games/…`** (domaine nu, http) | re-mesuré servi 15/09 (#1399 §6.5) ; connu 18/08 | Recâbler au cutover — **couplé à E4** (l'option (b) rend le recâblage obligatoire et suffit) | ☐ (E4) |
| E8 | #1049 : `web.config` versionné = risque de downgrade d'un webroot par geste git | garde **index Git** mergée (#1393, bin/) ; garde **webroot** = grain ⑤ du pool, pas livrée | **Solder avant cutover** (grain ⑤ : mutation témoin rouge/vert sur index + chemin de publication, frère de #1393) | ☐ |
| E9 | Recette É1-É7 d'août = **historique** (versions disparues ; incident 08/09 réparé depuis) | #1180 c.08/09 (ai-01 ×2 : « preuves historiques, pas l'état courant ») | **Re-jouer É1-É7 sur l'état final** (avec É5 requalifiée sans-stock et sa réserve paiement, #1188) avant d'ouvrir les 3 cases | ☐ |
| E10 | Certificats : prod expire **04/11/2026** ; préprod = mutualisé myia.io expire **06/10** | RAPPORTÉ #1180 (mesure 25/08) | Plan win-acme **reconduction passée avant 04/11 sur l'hôte cible** (#1180 DoD 4). Fenêtre DNS prudente : **avant ~05/10** (marge sur le mutualisé préprod) **ou après renouvellement prod** — la reconduction doit être *effectivement passée*, pas planifiée | ☐ |
| E11 | Fraîcheur des mesures elles-mêmes | — | Toute sonde cutover = **cache-buster + preuve par page** (TTL prod 1 an ; buster au comportement non uniforme — #1180 É7) | (discipline) |

## §2 — Ordre des contrôles proposé

### Phase T-1 (pré-cutover — tous verts requis)

1. E8 soldé (garde #1049 livrée, CI verte).
2. Décisions E1-E7 rendues par owner/ai-01 (le tableau §1 fait foi ; chaque case cochée = une ligne datée).
3. Si E1 = solder : fenêtre DB owner (10 UPDATE `PageHeadText` + re-sonde ×2).
4. Si E6 = migrer : copie des 2 SVG vers la cible + vérification sha256.
5. **Re-jeu É1-É7** sur l'état final (parité prod↔préprod, buster par page, contrôle inverse du comparateur).
6. Backup DB **testé restaurable** (au minimum : restore en sandbox + une sonde) — geste owner.
7. TTL DNS **abaissé** (ex. 300 s) ≥ 24-48 h avant la bascule — condition du rollback §3.

### Jour J (séquence)

1. **Gel git du checkout principal** — ⛔ ZÉRO opération git sur le main checkout pendant copie+bascule
   (règle hard consignée ; le webroot EST un checkout, #1049/#1399).
2. Backup DB final (conservé, nommé, horodaté).
3. Copie du webroot vers la cible (PowerShell `Copy-Item` + vérif compte — jamais robocopy depuis Git-Bash).
4. **Sondes sur la cible AVANT bascule DNS** (NAT hairpin `--resolve`, cache-buster) — plancher :
   - home 200 + **GTM-TZBQ57M ×2** (ou ×1 si E1 accepté — le constat doit alors dire « E1 accepté ») + GA ×2 ;
   - **16/16 wrappers 200 aux octets exacts** (génération figée E3) ;
   - OWL selon option E4 : 200 webroot (a) **ou** menu → endpoint Pages vérifié 200 + sha256 (b) ;
   - jQuery + jQuery-UI 200 ×2 (régression 404 du 08/09) ; skin `2shinebs5` présent, 0 `Xcillion` ;
   - 4 zips 200 aux octets exacts (`Argmentum_` coquille comprise) ;
   - CSP : entrées conditional GTM/GA présentes (#1064 fermé sur cette base).
5. **Bascule DNS** (apex + www → cible).
6. Sondes **externes** post-bascule (cache-buster) : les mêmes que (4), + GTM page à page sur les **9 pages
   porteuses** (#1399 §2.2 — tabs 138/148/149/150/151/152/169/157 + redirections 170/171 servent l'artefact,
   pas la balise : ne pas les compter comme échec) ; `/terms` `/privacy` selon E2.
7. Surveillance 24 h (erreurs 5xx, 404 ressources — le précédent jQuery).

### Phase T+1 (post-cutover)

- E4 option (a) si voulue (re-déploiement OWL committé au webroot, runbook dédié).
- E2 backlog (terms/privacy) ; E3 déploiement génération committée des wrappers (#1339).
- Reconduction cert suivie — **échéance ferme 04/11**.
- 3 cases #1180 (jsboige/Adeline/Thomas) sur le parcours re-joué — jamais avant.

## §3 — Rollback

- **Principe** : le cutover est **additif jusqu'à la bascule DNS** — l'ancienne prod n'est modifiée par aucun
  geste de la séquence (copie, pas mutation). Le rollback = **revert DNS seul**, à TTL bas (§2 T-1.7).
- **Déclencheurs** : toute sonde cible rouge sur un plancher §2.4 **avant** la bascule → ne pas basculer ;
  après la bascule → revert DNS dans la fenêtre de décision (< TTL), ancien hôte reprend (certificat prod
  valide jusqu'au 04/11 — il continue de servir).
- **DB** : backup pré-cutover restaurable (T-1.6) ; la cible n'a jamais servi de trafic public avant la
  bascule ⇒ pas de données à réconcilier au revert.
- **Ce que le rollback ne répare pas** : un E6 « perdre » consommé (SVG orphelins non copiés avant) — d'où
  le placement T-1.4 ; les sondes browser-side des visiteurs pendant la fenêtre (analytics gap borné au TTL).

## §4 — Ce que ce dossier n'établit pas

- ⛔ Aucun PASS, aucune décision — les cases §1 sont vides et le restent jusqu'à owner/ai-01.
- ⛔ Aucune mesure DB **prod** (porteur `PageHeadText` prod = inféré par homologie, #1399 §7).
- ⛔ Aucune exécution : pas de fenêtre réservée, pas de DNS, pas d'UPDATE, pas de copie.
- ⛔ La recette elle-même (É1-É7 re-jeu + 3 cases) = #1180 ; l'esthétique = comparateur #1313 / ai-01.
- ⛔ Le contenu du OWL vestige (PK 511 etc.) = périmètre #133/#1357.

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain ④ · lecture seule : HTTP cache-buster + hairpin,
git show, gh — aucun geste servants · verdict visuel : ai-01 · décisions : jsboige.*
