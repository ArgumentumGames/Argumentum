# INDEX des arbitrages owner ouverts — une coche par ligne + carte de `docs/`

**Auteur** : po-2024 (worker) · **Date** : 2026-09-18 · **Base** : master `1c456126` (maj 03:41 :
#506 clôturée C5 → **15 lignes ouvertes** ; coches déléguées du 17/09 soir marquées ☑ — #994=A,
#192 C+D, #830a, figeage #802, 5.6 systématique)
**Grain** : pool #458 c.5666260217, grain ③. **Support de décision — 0 write corpus, 0 décision
prise, 0 re-mesure** : chaque ligne renvoie à la feuille mergée qui porte la coche ; les coûts et
recommandations sont **RAPPORTÉS des feuilles** (l'entrée de relecture du dossier v2.0.0 = `134-entree-validation-release-2026-09-16.md`, ~60 min).

> **Anti-confusion** : [`DECISION-v2.0.0-jsboige.md`](DECISION-v2.0.0-jsboige.md) est un brief
> **RÉPONDU** (4 questions tranchées le 08/08, étampe 13/09) — rien de lui n'est ouvert ici.
> [`README.md`](README.md) du répertoire est **superseded/stale** (RAPPORTÉ du brief) : la présente
> page le remplace comme point d'entrée des décisions.

---

## §1 Les arbitrages ouverts — **15 lignes** (une coche par ligne ; #506 clôturée sortie, coches déléguées 17/09 marquées ☑)

| # | Décision | Feuille (chemin depuis `docs/`) | Coches | Coût / effet (RAPPORTÉ de la feuille) | Reco worker |
|---|---|---|---|---|---|
| 1 | **#994 apostrophes** — ☑ **A coché sous délégation 17/09** (dispatch GO 18:46Z) ; exécution **grain ⓪e HOT** en cours | `release-dossier/994-arbitrage-apostrophes-ABCD-2026-09-15.md` | ☑ A (exécution en cours) | 146 cartes · 6 PDF · 1 OWL · régén `fr/` seule ; éditions **byte-exactes, diff borné aux 146 cellules du dossier**, byte-check par langue ; ⚠️ #1421 a muté le CSV Rules (`variant_class`) — comptes à rejouer sur la base courante | **A** ☑ |
| 2 | **#1369 cover variantes** — **1a mécanisme + 2a fiche style SOLDÉS** (17/09 soir, #1421) | `release-dossier/1369-dossier-owner-cover-variantes-2026-09-15.md` + `1369-cout-mecanisme-cardclass-2026-09-15.md` + fiche `design/1369-fiche-style-covers-variantes.md` | restent : images (3a = 5 muettes) + validation visuelle | pilote `cover-bingo` livré : CSV `variant_class` + template + `Cards/Rules/Assets/cover-bingo.png` | **3a · 4e restants** |
| 3 | **#802 rôles RU/ES** — figeage prompts ☑ GO 17/09 (grain ⓪d) ; les 2 coches de rôles restent ouvertes | `release-dossier/802-feuille-decision-ru-es-2026-09-15.md` (contexte : `802-libelles-roles-2026-09-15.md`) | 2 coches (ES lector, RU софист+rôle 2) | (a) = **0 cellule** touchée ; (b) = alignements ponctuels | **(a) ×2** |
| 4 | **#415 allow-list** | `repo/415-feuille-decision-allowlist-2026-09-15.md` | D1 / D2 / D3 | enjeu **758,7 Mo au HEAD** (DNNPlatform 209,8 · Archive 127,4 · Mindmaps 123,9 · Packaging+Sketch 142,2 · CSV ~28) ; D1-D2 statu quo→LFS, D3 geste LFS post-tag | **(a) ×3** |
| 5 | **#830 les 9 capacités** — ☑ **(a) GO 17/09** (grain ⓪d) | `quality/830-etat-9-capacites-2026-09-15.md` | ☑ (a) (exécution ⓪d) | (a) = aligner la CI sur la barre de l'issue (+ registre mis à jour) | **(a)** ☑ |
| ~~6~~ | ~~**#506 social auth — clôture**~~ — **CLÔTURÉE 17/09 (C5)** : sortie de la liste | ~~`dnn/506-social-auth-assessment.md`~~ | — | clôturée sur l'état mesuré 14/09 §9 (Facebook+Google+Live enabled, Twitter non) | clôturée |
| 7 | **ES « romance » Scenarii** | `release-dossier/scenarii-es-romance-feuille-2026-09-15.md` | A / B | A = statu quo (le mot castillan couvre le sens, §1.4) ; B = remplacement (11 cellules, valeur à nommer) | **A** |
| 8 | **#192 link i18n** — ☑ **C + D cochés 17/09** (grain ⓪b : script langlinks API-confirmées + vider FR-institutionnels, diff borné `link_*`, ⛔ pas de passe LLM) | `release-dossier/192-feuille-decision-link-i18n-2026-09-15.md` | ☑ C · ☑ D (exécution ⓪b) | réparation GO « on répare tout, proprement » — gel corpus levé | **C+D** ☑ |
| 9 | **Fenêtre DNN i18n #682→#685** | `dnn-localization/682-685-dryrun-fenetre-owner.md` | GO fenêtre | provisionner **49 attributs** (content-type `Game Rule`, setID 377, app 60), garde `--expect 0` ; `Title` seul `IsTitle` ; ne pas toucher 210/231 | GO = owner |
| 10 | **GO gpt-5.6-sol + smoke #202** — ☑ **GO 17/09** « on migre vers 5.6, systématiquement, pas de dette » (grain ⓪ : config 44× 5.5 + 4× 5.4 + prompts prescriptifs, re-mesure d'abord) | `translation/202-smoke-datasetupdater-protocole-2026-09-15.md` (prérequis : inventaire `translation/gpt55-modele-courant-inventaire-2026-09-15.md`, propre depuis erratum #1418) | ☑ GO (reste **clé P2** pour le smoke) | migration exécution GO ; smoke = clé depuis FICHIER, vérif cell-by-cell (protocole #1405) | GO ☑ |
| 11 | **Fenêtre FreeMind (mindmaps PK 511)** | pas de feuille dédiée — réservation par annonce dashboard | réservation | un seul run, Batik prouvé, ⛔ jamais fallback XSLT silencieux ; garde chemin court #1179 (jonction `D:\A1114` à recréer J-0) | réservation = owner |
| 12 | **Relecture notes v2.0.0** | entrée `release-dossier/134-entree-validation-release-2026-09-16.md` | — | **~60 min** (ordre indicatif §5 de l'entrée ; 8 pièces + 4 contexte) | point d'entrée |
| 13 | **Fenêtre régén v2.0.0** (×2 coches) | `release-dossier/134-fenetre-regen-reservation-2026-09-16.md` | tranche (dépend #994) puis créneau | périmètre f(#994) : **D=6 · A=12 · B=19 · C=≈52 PDF** (+ OWL si ≠D) ; planifier **4 h** (run 73-80 min RAPPORTÉ + C1→C6 1 h 45 MESURÉ + verdict ai-01 — le PASS 12/09 ne se transporte pas) | après coche #994 |
| 14 | **Montée DNN Vague 1** ; ⚠️ **D1 = B Stripe, GO owner 17/09 ~18:50** (« on a dit qu'on passait sous Stripe ») — POC en cours (`docs/131-d1-stripe-poc`, audit mesuré), reprise des **124 commandes / 111 comptes** = décision owner restante | `release-dossier/131-feuille-decision-d1-d6-2026-09-16.md` | D2-D6 restantes (D1 tranchée) | D2 npm dev (A balai ~1 h / B acceptation #1404) · D3 CVE lookup ~30 min **0 fenêtre** (prérequis D5) · D4 Razor14 3-5 h ou reporter · D5 cutover (lecture + fenêtre DNS) · D6 re-jeu recette ~1 h (verdict ai-01) | D2-A · D3 · D4-B · D6 |
| 15 | **Cutover #1180/#1066** — **E4/E5/E6/E7 SOLDÉS** (recette 17/09, #1420 : recâblage Pages tab 171 + SVG orphelins servis aux sha publiés) | `quality/1180-dossier-cutover-2026-09-16.md` + recette `quality/1180-recette-preprod-2026-09-17.md` | **4 restantes** : E1 · E3 · E9 (=D6) · E10 | E1 noscript GTM (10 UPDATE DB gated OU re-qualifier plancher ×1) · E3 figer `de763aa9` · E9 = D6 (recette re-jeu) · E10 **DNS ferme 04/11**, fenêtre ~05/10 ou post-04/11 | E3 figer · E10 fenêtre |
| 16 | **Tag v2.0.0** | (porte de sortie — aucune feuille dédiée) | pose du tag | débloque #654/#666/#965 exécution, upload release, contact façonniers (RAPPORTÉ cycles ai-01) | après 12-15 |

**Couplages à lire avant de cocher** : #994 → fenêtre régén (ligne 13, le périmètre en dépend) ·
D3 → D5 (ligne 14) · D6 = E9 (14↔15) · ~~E4 → E5/E7~~ **soldé** (17/09 recette #1420) · tout ce qui
est « post-tag » (#830-a, #192-C, #415-D3, fix #1123 implémentation) se débloque à la ligne 16.

## §2 Carte de `docs/` (16 répertoires + racine)

| Répertoire | Fichiers | Rôle |
|---|---:|---|
| `release-dossier/` | 31 | **le dossier v2.0.0** (12 pièces #1407→#1417) + feuilles de décision (#994, #802, #1369×2, ES romance, #192, fenêtre régén, entrée relecture, D1-D6) — la présente page en est l'index |
| `taxonomy/` | 166 | audits taxonomie/AIF (#497-#499), renommages de familles — figés (audit trail) |
| `investigations/` | 125 | archéologie pipeline, rapports debug + scripts PowerShell datés |
| `dnn-localization/` | 42 | i18n DNN (#682→#685), fenêtres, glossaires |
| `dnn/` | 20 | état plateforme, #506 social auth, montée DNN |
| `quality/` | 15 | #830 capacités, #1123 race assemblage, cutover #1180 |
| `ontology/` | 15 | OWL #133, IRIs, round-trips |
| `publication/` | 13 | downloads v2.0.0 FR/EN (tailles mesurées), notes de release |
| `repo/` | 8 | #415 allow-list, croissance du pack |
| `translation/` | 5 | inventaire gpt-5.5 (#1403 + erratum #1418), protocole smoke #202, re-mesure i18n es/ar/fa/zh |
| `sddd/` | 5 | méthodologie SDDD |
| `issues/` · `release-review-v0.9.0/` · `licensing/` · `fabrication/` | 3+2+1+1 | divers classés |
| racine `docs/*.md` | 10 | entrées transverses (validation v0.9.0, etc.) |

## §3 Ce que cet index n'établit pas

⛔ Aucune re-mesure : coûts, comptes et recos sont RAPPORTÉS des feuilles citées (base de chacune =
son merge) — en cas d'écart, **la feuille fait foi**, pas cette page · ⛔ aucune décision prise ni
préjugée (les recos citées sont celles des workers, déjà écrites dans les feuilles) · ⛔ fraîcheur non garantie au-delà de la base `1c456126` — une coche posée sur une feuille invalide
sa ligne ici · ⛔ la carte `docs/` compte des fichiers, pas une revue de contenu · ⛔ ne couvre pas
les arbitrages **fermés** (voir `DECISION-v2.0.0-jsboige.md` §3) ni les gestes hors dépôt
(dashboards, purge swap po-2024).

---
*po-2024 — pool #458 c.5666260217, grain ③. Périphérie : les grains ② (#212 plan tests visuels),
④ (CLAUDE.md fraîcheur) et ⑤ (audit prompts bump 5.6) alimentent respectivement la qualité
post-tag, les compteurs de la ligne 12 et le prérequis de la ligne 10.*
