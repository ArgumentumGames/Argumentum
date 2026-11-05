# #135 — Contenu des zips servis + proposition de correspondance (2026-11-05)

> **Nature** : mesure seule (lecture du répertoire servi et des entrées zip — **aucune
> extraction**). Item 5 du pool ai-01 (c.6645521254). Fait suite à
> [`135-remesure-brouillons-et-liens-servis-2026-11-04.md`](135-remesure-brouillons-et-liens-servis-2026-11-04.md).
> ⚠️ La page Téléchargements est **identique prod ↔ préprod** (mesuré 04/11) : 4 liens zip + 1
> lien pilote PDF. Ce document décrit ce qu'il y a DERRIÈRE ces liens.

## 1. Les 4 zips servis — contenu mesuré (tous FR, génération 18/08)

| Zip servi | Taille | Contenu | Génération |
|---|---|---|---|
| `…_Cartes_numériques.zip` | 50 354 291 o | **157 PNG** `Fallacies-Web/` + 1 `.ini` (une carte par sophisme, densité web) | 18/08, pré-#1288 |
| `…_légères.zip` | 14 933 310 o | **157 PNG** `Fallacies-Web-Light/` + 1 `.ini` (même corpus, basse densité) | 18/08 |
| `…_Affiche_A0.zip` | 39 423 798 o | **1 PDF** `Argumentum-Fallacies-Web-A0.pdf` (41,7 Mo décompressé) | 18/08 |
| `Argmentum_Taxonomie…_MindMap.zip` ⚠️ coquille | 6 417 247 o | **1 SVG** (`…_Fr_2_cartes.svg`, 9,9 Mo) + **1 `.mm** Freemind FR | 18/08 |

Les 157 PNG = les cartes **web** du corpus Fallacies (pas le deck Tarot imprimable). Aucun
des quatre ne dépasse le français ; aucun n'est dans la release `v2.1.0-review` (80 PDF).

## 2. DÉCOUVERTE — 2 zips sur disque sans aucun lien servi

Le répertoire servi contient **6** zips, pas 4 (mesuré 05/11 sur le webroot) :

| Zip NON lié | Taille | Posé le | Contenu |
|---|---|---|---|
| `…_Planches_A4.zip` | 10 779 141 o | **06/10** | 1 PDF `Argumentum-Fallacies-Web-A4.pdf` (11,1 Mo) |
| `Argumentum_Print&Play.zip` | 93 587 869 o | **06/10** | 2 PDF : `…-TarotCards-Print&Play.pdf` (50,5 Mo) + `…-PokerCards-Print&Play.pdf` (44,4 Mo) |

La page ne les sert pas (mesuré : 4 href zip + 1 href pilote, rien d'autre). Deux lectures
possibles — des liens prévus jamais posés, ou retirés ; **l'état « sur disque mais non
indexé » est le leur depuis le 06/10**. Toute table de remplacement #135 doit trancher
leur sort, pas seulement celui des 4 liés.

## 3. Proposition de correspondance (v2.1.0-review, 10 familles × 8 langues)

| Zip servi actuel | Correspondance proposée | Qualité du mapping |
|---|---|---|
| `…_numériques.zip` (157 PNG web) | **aucun équivalent direct** — la release est 100 % PDF. Soit reconstruire un zip PNG par langue (pipeline : `Fallacies-Web/` par langue), soit remplacer le lien par `Argumentum_Fallacies_Web_A4_<lang>.pdf` en assumant le changement de nature (PNG → PDF) | ⚠️ à trancher |
| `…_légères.zip` (157 PNG light) | idem — « légères » n'existe qu'en PNG ; `…_Thumbnails_A4_<lang>.pdf` est la famille la plus proche par l'intention (aperçu compact) | ⚠️ à trancher |
| `…_Affiche_A0.zip` (1 PDF A0 FR) | **mapping direct** : `Argumentum_Fallacies_Web_A0_<lang>.pdf` — même document, 8 langues | ✅ évident |
| MindMap zip (SVG + .mm FR) | **aucun asset équivalent** — les mindmaps vivent comme wrappers HTML sur le site + OWL [#133](../ontology/133-etat-servi-2026-11-05.md) ; remplacer par un lien interne vers la page Argumentation, ou reconstruire un zip par langue | ⚠️ à trancher |
| *(non lié)* `Planches_A4.zip` | `Argumentum_Fallacies_Web_A4_<lang>.pdf` — **déjà son remplaçant exact existe en release** : le plus simple est de lier la release et de laisser le zip orphelin | ✅ évident |
| *(non lié)* `Print&Play.zip` (2 PDF) | `Argumentum_TarotCards_Print.Play_A4_<lang>.pdf` + `Argumentum_PokerCards_Print.Play_A4_<lang>.pdf` — couplage 1 zip ↔ 2 assets ; ou reconstruire un zip par langue | ✅ mapping clair |

**Recommandation po-2023** : les 3 mappings « évidents » sont posables le jour J sans
débat ; les 3 « à trancher » sont des **changements de nature** (PNG → PDF) que seul
ai-01/owner peut décider — le tableau à blanc du 04/11 porte déjà les cases ⬜.

## 4. Témoins de remplacement (jour J)

- Après geste : la page ne doit plus servir AUCUN `/Portals/1/Downloads/*.zip` visé par la
  décision, et chaque nouveau lien résout 200 vers un asset **publié** de la release finale
  (`Print.Play` = nom réécrit par GitHub).
- Les 2 zips orphelins restent sur disque tant qu'une décision de retrait n'est pas prise :
  ⛔ rien ne se supprime sans preuve de préservation.

*po-2023 · instrument : lecture des entrées zip (pas d'extraction) + sondes HTTP cache-buster*
