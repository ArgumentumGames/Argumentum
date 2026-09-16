# #131 — Feuille de décision owner : montée DNN, Vague 1 (6 coches)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base** : `origin/master` `eba02e91`
**Source** : extraction du dossier mergé **#1413** (`dedb81b6`) —
[`131-montee-dnn-2026-09-16.md`](../quality/131-montee-dnn-2026-09-16.md) §4-Vague 1 + §6 ; comptes
**RAPPORTÉS** de ce dossier (mesurés 16/09 par cette lane, relus par ai-01 avant merge). D5 renvoie au
dossier cutover **#1411** (`503e64c8`).
**Statut** : **support de décision uniquement** — ⛔ 0 geste serveur, ⛔ 0 write DB/webroot, ⛔ 0 fenêtre.

---

## Contexte en trois lignes

La préprod **sert déjà le socle cible** (DNN 10.3.2 + 2sxc 21.07, 7 pages porteuses 200 le 16/09 — dossier
§1). La **Vague 1** = les 6 prérequis avant toute bascule prod ; chaque coche porte son coût propre et
**aucune n'exécute rien**. L'exécution reste gated : fenêtre IIS pour D1/D4, fenêtre DNS pour D5, aucune
pour D2/D3/D6.

## D1 — Boutique : OpenStore 4.2.4 / Stripe managed / statu quo

**Objet (RAPPORTÉ dossier §2)** : NBrightBuy **4.1.11.0** vif (`/Acheter-le-jeu` 200, catid 140 / eid 130)
+ OS_Stripe 1.0.0.0 (Checkout câblé, webhook IPN) + OS_Reports + OS_Chronopost2 ; RazorEngine 3.10.0
légataire (ref NBrightBuy — aucun CVE ouvert nous visant, dossier §3).

| branche | geste | coût | débloque |
|---|---|---|---|
| **A — monter OpenStore 4.2.4** | sourcer le module, déployer, QA commande | fenêtre IIS idle + ~½ journée avec QA (**SUPPOSÉ** — jamais exécuté ici) | boutique maintenue sur socle suivi |
| **B — Stripe managed products** (Option 2 du #131) | migration catalogue → produits Stripe, retrait du périmètre boutique | effort **à estimer à GO** (jamais chiffré ici) + campagne de tests paiement | supprime NBrightBuy **et** RazorEngine légataire |
| **C — statu quo 4.1.11** | aucun | 0 | compatible DNN 10 (mesuré 22/07) ; dette conservée |

**Reco worker (SUPPOSÉ — jugement, pas mesure)** : A si la boutique reste un canal de vente réel ; B si
les paiements passent déjà effectivement par Stripe (B ferme la dette) ; C = report explicite. L'arbitrage
usage/commerciel appartient à l'owner.

## D2 — npm dev (49 alertes) : balai ou acceptation documentée

**Objet (RAPPORTÉ dossier §3)** : svgo **46** + baseline-browser-mapping **3**, scope **development**,
skins/apps vendored (« Bootstrap 4 Instant », ImageCompare2, QrCode2) — non servies comme code actif ;
**0 alerte runtime site ouverte** (AutoMapper = pipeline .NET, épinglé licences, hors site).

| branche | geste | coût | effet |
|---|---|---|---|
| **A — coup de balai** | merger les PR dependabot par app (#1328, #1322, #1323 + reliquats svgo) | ~1 h review, CI seule, **0 fenêtre** (dev-scope) | Dependabot silencieux, surface d'audit réduite |
| **B — acceptation documentée** | statu quo (tri post-tag #1404 déjà mergé et acté) | 0 | alertes restent visibles jusqu'au balai |

**Reco** : A à loisir — les PR existent déjà, aucun risque runtime ; B est l'état par défaut d'aujourd'hui.

## D3 — CVE 10.3.2 post-audit : lookup avant bascule (GO simple)

**Objet (RAPPORTÉ dossier §3)** : les 4 CVE socle 9.11.1 (dont 52488 9.1, 64095 9.8) sont **fermées par le
palier 10.3.2** ; l'inconnu = CVE publiées **après l'audit juin contre 10.3.2 elle-même** — le dossier ne
l'a pas recherché (pas d'instrument public fiable au jour de sa rédaction).

| geste | coût | effet |
|---|---|---|
| GO lookup (instrument public à nommer : GitHub advisories `DotNetNuke` + NVD feed) | ~30 min worker, **0 fenêtre** | verdict écrit : vert = rien à faire · rouge = case nouvelle à traiter avant D5 |

⚠️ **Prérequis de D5** : à jouer avant la bascule.

## D4 — 12 templates RazorComponent → Razor14 : migrer ou reporter

**Objet (RAPPORTÉ dossier §4-V1.4)** : effort **3-5 h mesuré**, dépendance `shared/_Parts.cshtml` migré
**en premier** ; 24 apps 2sxc installées — les bs3 seront réécrites quoi qu'il arrive.

| branche | geste | coût | effet |
|---|---|---|---|
| **A — migrer avant bascule** | 12 templates, `_Parts.cshtml` d'abord, QA apps | 3-5 h + QA + fenêtre déploiement IIS idle | socle moderne, dette fermée |
| **B — reporter post-tag** *(reco dossier)* | aucun | 0 | dette maintenue — **aucune CVE ouverte** ne la vise (dossier §3) |

**Reco** : B — aucune pression sécurité ; A devient prérequis seulement si la réécriture des apps est
planifiée avant le tag.

## D5 — Cutover E1-E11 : 9 coches décisionnelles restantes (organe = dossier #1411)

Cette feuille **ne duplique pas** : les 11 écarts avec propositions qualifiées et cases vides vivent dans
[`1180-dossier-cutover-2026-09-16.md`](../quality/1180-dossier-cutover-2026-09-16.md) §1. Décompte pour
la coche :

- **E8 préreçu ✓** — garde webroot #1049 livrée (`5b45e19a`) ;
- **E11 = discipline**, pas une coche (cache-buster + preuve par page à chaque sonde) ;
- **9 cases restantes** : E1 `<noscript>` GTM · E2 terms/privacy · E3 figer `de763aa9` · E4 OWL vestige
  (reco worker : option (b) endpoint Pages — **solde E5 + E7 en même temps**) · E6 2 SVG orphelins
  (**à migrer avant bascule si retenus** — l'ancien hôte ne survit pas au cutover) · E9 re-jeu É1-É7 ·
  E10 fenêtre DNS (**ferme 04/11**).

**Coût** : lecture du dossier ~10 min + fenêtre DNS (avant ~05/10 ou après 04/11) + le jour J selon les
phases T-1 / J / T+1 du dossier.

## D6 — Gate 2c / re-jeu E9 : recette #1180 sous son instrument (GO + verdict ai-01)

**Objet (RAPPORTÉ dossier §1)** : les 7 pages 200 du 16/09 = un **premier élément, pas une certification**
— la Gate 2c se re-teste sous l'instrument de la recette #1180. C'est le même geste que E9 (D5), listé
ici parce qu'il ferme la Vague 1.

| geste | coût | effet |
|---|---|---|
| GO re-jeu (sondes : GTM ×2, wrappers octets, jQuery ×2, zips, CSP, skin, pages porteuses) | ~1 h (sondes existantes), 0 fenêtre | état certifié pré-bascule + **verdict ai-01** |

## Les 6 coches (owner)

- [ ] **D1 boutique** : ☐ A OpenStore 4.2.4 · ☐ B Stripe managed · ☐ C statu quo 4.1.11
- [ ] **D2 npm dev** : ☐ A balai (merges dependabot) · ☐ B acceptation documentée (statu quo)
- [ ] **D3 CVE post-audit** : ☐ GO lookup (instrument : ______) — à jouer avant D5
- [ ] **D4 Razor14** : ☐ A migrer (3-5 h, `_Parts.cshtml` d'abord) · ☐ B reporter post-tag (reco)
- [ ] **D5 cutover E1-E11** : 9 cases restantes → dossier #1411 §1 (E8 ✓, E11 discipline) + fenêtre DNS
- [ ] **D6 Gate 2c / E9** : ☐ GO re-jeu recette #1180 — verdict ai-01

## Ordre implicite

**D3 avant D5** (le lookup CVE conditionne la bascule) · **D6 = le re-jeu E9 de D5** (même geste, il
certifie l'état final) · D1 et D4 = fenêtres IIS **indépendantes** de la bascule · D2 reportable sans
terme.

## Ce que cette feuille n'établit pas

⛔ Aucune décision posée · ⛔ aucun geste serveur/webroot/DB, aucune fenêtre planifiée · comptes
**RAPPORTÉS** du dossier #1413 (mesurés 16/09 par cette lane, relus par ai-01 avant merge) — rien de
re-mesuré ici · ne duplique pas les propositions E1-E11 (le dossier #1411 fait foi) · ⛔ aucun verdict
visuel (ai-01) · la reco D1 est un jugement worker (**SUPPOSÉ**), l'arbitrage boutique appartient à
l'owner · les coûts D1-A (½ journée) et D1-B sont **estimés, jamais exécutés**.

---

*po-2023 (worker lane) · pool #458 c.`5666259809` grain ③ · base `origin/master` `eba02e91` · support de
décision uniquement · verdict visuel : ai-01 · décisions : jsboige.*