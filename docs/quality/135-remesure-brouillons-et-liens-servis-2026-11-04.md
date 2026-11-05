# #135 — Brouillons re-mesurés + relevé des liens de téléchargement servis (2026-11-04)

> **Nature** : mesure seule (lecture seule, cache-buster `?cb=` sur chaque sonde). ⛔ Zéro écriture
> prod (gel #972), aucun upload, aucune publication DNN. Grains ③④ du dispatch
> `ai01-dispatch-po2023-20261104d` (pool #458 c.6638047321).
> **Base** : master `8c74ee13`. Précédent : [`135-draft-links-vs-published-assets-2026-09-22.md`](135-draft-links-vs-published-assets-2026-09-22.md)
> (grain ⑤ du 22/09 — tous les liens des brouillons résolus contre les assets **v2.0.0-review**).

## 1. Grain ③ — les brouillons : toujours v2.0.0, la cible a bougé (MESURÉ)

Les 4 brouillons de la famille PR #1386 sont en place et inchangés depuis le 22/09 :
[`news-article-v2.0.0.fr.md`](../publication/news-article-v2.0.0.fr.md) + miroir EN +
[`downloads-v2.0.0.fr.md`](../publication/downloads-v2.0.0.fr.md) + miroir EN.

Ce qui a changé depuis leur dernière re-mesure :

| Fait des brouillons (22/09) | État mesuré au 04/11 | Écart |
|---|---|---|
| Cible = tag `v2.0.0` + 80 assets frais | la release candidate est **`v2.1.0-review`** (pre-release, 27/10, 80 assets, arbre figé `341ef2a3`) ; `v2.0.0-review` (18/09) devient l'ancienne génération | **non bloquant mais à trancher au tag** : les brouillons citent « v2.0.0 » dans titres SEO, méta, checklist #134 et avertissement pré-release |
| « 80 assets » | **80** = 10 familles × 8 langues, **tous PDF** (mesuré) | chiffre toujours exact |
| Schéma placeholder `argumentum-{lang}-v2.0.0.zip` (page Téléchargements, #999 DoD 4) | **aucun `.zip` n'existe** parmi les 80 assets v2.1.0-review | les zips par langue restent **à construire** ou le schéma de la page doit passer aux PDF par famille |
| Tailles par PDF (80) : mesurées sur le bundle `review-v2.0.0-regen-20260912` | v2.1.0-review = régén du 27/10 (post-#1288 et passes mindmaps) — **tailles à re-dériver du manifeste v2.1.0 au jour du tag** | les tableaux de tailles des brouillons sont périmés par construction |
| `[PLACEHOLDER]` : date, og:image, URL release, lien communauté | toujours à blanc | normal — se remplissent au jour J |

**Verdict grain ③** : les brouillons restent le bon support (structure, FAQ, checklist vérifiées
22/09) ; leur **habillage de version** (v2.0.0 → v2.1.0) et leurs **tableaux de tailles** sont les
deux reprises à faire au jour du tag — rien d'autre ne les date.

## 2. Grain ④ — liens de téléchargement SERVIS (MESURÉ, 04/11)

Sondées : page Téléchargements **prod** (`www.argumentum.games`) et **préprod**
(`dnn.argumentum.myia.io`), HTML décodé, cache-buster sur chaque sonde.

| # | Libellé servi (tronqué à la mesure) | Lien servi | Nature |
|---|---|---|---|
| 1 | Cartes d'arguments fallacieux — version « Print and Play » allégée | `github.com/…/releases/download/v2.1.0-review/Argumentum_TarotCards_Print.Play_Light_A4_fr.pdf` | **pilote v2.1.0 — PRÉPROD SEULEMENT** (absent de la prod) |
| 2 | Cartes numériques — Déclinées en format carré… | `/Portals/1/Downloads/Argumentum_Arguments_Fallacieux_Cartes_numériques.zip?ver=…` (50 354 291 o, 18/08/2026) | zip local, ancienne génération, FR seul |
| 3 | Cartes numériques web légères | `/Portals/1/Downloads/…_légères.zip?ver=…` (14 933 310 o, 18/08/2026) | zip local, idem |
| 4 | Cartes numériques — Affiche A0 | `/Portals/1/Downloads/…_Affiche_A0.zip?ver=…` (39 423 798 o, 18/08/2026) | zip local, idem |
| 5 | MindMap de la Taxonomie des arguments fallacieux | `/Portals/1/Downloads/Argmentum_Taxonomie_Arguments_Fallacieux_MindMap.zip?ver=…` (6 417 247 o, 18/08/2026 — ⚠️ coquille « Argmentum » dans le nom de fichier) | zip local, idem |

Les 4 zips locaux sont **identiques prod ↔ préprod** (mêmes chemins, mêmes jetons `?ver=`, dates
18/08/2026) : c'est le calque de l'ancienne génération. La préprod porte en plus **un seul** lien
pilote pointant déjà sur un asset v2.1.0-review — la page n'a donc **pas** été repensée, un lien a
été substitué.

## 3. Catalogue de remplacement disponible (MESURÉ)

Assets de `v2.1.0-review` — **10 familles × 8 langues = 80, tous PDF** (noms **publiés** ; GitHub
réécrit `Print&Play` → `Print.Play` — `--clobber` cible le nom publié) :

```
Argumentum_TarotCards_<lang>.pdf                      ×8      deck Tarot complet (CMYK)
Argumentum_TarotCards_Virtues_<lang>.pdf              ×8      deck Vertus
Argumentum_TarotCards_Print.Play_A4_<lang>.pdf        ×8      P&P pleine qualité
Argumentum_TarotCards_Print.Play_Light_A4_<lang>.pdf  ×8      P&P allégé  ← le lien pilote #1
Argumentum_PokerCards_<lang>.pdf                      ×8      deck Scenarii
Argumentum_PokerCards_Print.Play_A4_<lang>.pdf        ×8
Argumentum_PokerCards_Print.Play_Light_A4_<lang>.pdf  ×8
Argumentum_Fallacies_Web_A0_<lang>.pdf                ×8      affiche A0
Argumentum_Fallacies_Web_A4_<lang>.pdf                ×8
Argumentum_Fallacies_Web_Thumbnails_A4_<lang>.pdf     ×8
```

(`_lang_` ∈ ar, en, es, fa, fr, pt, ru, zh.)

## 4. Table de remplacement — À BLANC (à remplir au go-live)

| Lien servi actuel | Remplacement proposé (famille) | Étendue langues | Décision à prendre |
|---|---|---|---|
| #1 pilote `TarotCards_Print.Play_Light_A4_fr.pdf` | conserver tel quel (déjà v2.1.0) | fr seul servi | étendre aux 8 langues ou garder fr + sélecteur ? |
| #2 zip « Cartes numériques » (50 Mo) | `Argumentum_TarotCards_<lang>.pdf` **ou** zip par langue à construire (schéma #999 DoD 4, inexistant aujourd'hui) | ⬜ | PDF direct vs zip : poids (deck CMYK ≫ 50 Mo) et périmètre |
| #3 zip « web légères » (15 Mo) | `Argumentum_Fallacies_Web_A4_<lang>.pdf` / `Thumbnails` (à qualifier : « légères » ≈ basse densité) | ⬜ | quelle famille correspond à « légères » ? |
| #4 zip « Affiche A0 » (39 Mo) | `Argumentum_Fallacies_Web_A0_<lang>.pdf` | ⬜ | mapping direct attendu — vérifier le contenu du zip au moment du geste |
| #5 zip MindMap (6 Mo) | **aucun asset équivalent** — les mindmaps vivent désormais comme wrappers HTML sur le site (48/48) + #133 OWL | ⬜ | remplacer le lien par un lien interne vers la page Argumentation, ou reconstruire un zip |

Règles de remplissage (jour J) : chaque cellule ⬜ est tranchée par ai-01/owner au moment du tag ;
la colonne de droite du §2 (tailles `?ver=`) devient le **témoin de remplacement** — après geste,
un balayage de la page ne doit plus trouver AUCUN `/Portals/1/Downloads/*.zip` (les 4 anciens), et
chaque nouveau lien doit résoudre HTTP 200 vers un asset publié de la release définitive.

*po-2023*
