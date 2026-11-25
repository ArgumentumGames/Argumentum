# #1369 — sortie de repli : retrait de l'illustration du pilote Rules_07

| | |
|---|---|
| **Lot** | P23-08 (pool #458) |
| **Lane** | `myia-po-2023:Argumentum` |
| **Date** | 2026-11-25 |
| **Base** | master `64a81a5d` |
| **Statut** | PR en **brouillon** — à ne pas merger avant #2132 (porte du pool #458) |

**Ce qui est demandé à ai-01 :** le verdict visuel. Ce siège n'a pas la vision (le proxy retire les images) : **rien n'a été regardé**, tout a été mesuré par programme. Les planches sont ici, réduites ; les PNG de contrôle en pleine résolution sont sur `G:\Mon Drive\MyIA\Argumentum\repli-1369-20261125\`.

---

## 1. En bref

Le style de l'illustration posée sur la carte **Rules_07** (bande photo en bas de carte, pilote `cover-bingo`) a été refusé ; la reprise du style suit son cours dans **#1472**. Cette sortie de repli retire **la règle CSS qui pose l'illustration** et rien d'autre : le mécanisme (colonne CSV `variant_class` + clé `cardClass`), le CSV et l'asset restent en place.

Trois livrables : le retrait (§2), la liste des PDF qui changeront à la prochaine régénération (§3), le rendu de contrôle et sa mesure (§4–§6).

Un **second retrait** se trouve dans le lot, par nécessité (§4) : une déclaration `@font-face` morte qui **fait échouer la récolte des cartes Rules sur master**.

## 2. Le retrait

Fichier unique : `Cards/Rules/Argumentum_Rules_fr.json`. Deux commits distincts, chacun byte-exact.

| État | Octets | sha256 | `css` (car.) | `@font-face` | `url(` |
|---|---:|---|---:|---:|---:|
| master `64a81a5d` | 80 454 | `1f614fa047cbe89891468afdc9b3381b8503205a6ae7633bd4404d46c9d396ab` | 20 401 | 6 | 15 |
| **commit A** — retrait du `@font-face` mort | 79 618 | `f3498b80c3cf1ee2f698d269ff97722545a6bc3f92724924340eced7481ce212` | 19 599 | 5 | 9 |
| **commit B** — retrait de `card.cover-bingo safe` | 78 688 | `2fb8b1a68573b38d128063218f8670a459f96602906acad29af879db19f9c7dd` | 18 683 | 5 | 8 |

Soldes après les deux retraits (fichier entier / clé `css`) :

| Marqueur | Fichier | `css` | Lecture |
|---|---:|---:|---|
| `TrendSlab` | 0 | 0 | déclaration retirée |
| `db.onlinewebfonts.com` | 0 | 0 | hôte retiré |
| `Assets/cover-bingo.png` | 0 | 0 | illustration retirée |
| `card.cover-bingo` | 1 | 1 | la pierre tombale, rien d'autre |
| `variant_class` | 2 | 1 | la clé `cardClass` + la pierre tombale |

Ce qui **ne bouge pas** : le CSV `Argumentum Rules - Cards.csv` (0 octet modifié), la clé `cardClass = "variant_class"`, la colonne `variant_class` et ses 5 valeurs (`cover-argumentum`, `cover-bingo`, `cover-beau-parleur`, `cover-moulin`, `cover-parlote`), l'asset `Cards/Rules/Assets/cover-bingo.png` (1 640 150 o), et `card:not([class~="1"]) .texte { max-height: 390px; overflow: hidden; }` (#1429, conservé). Les 4 autres classes `cover-*` n'ont **aucune** règle CSS sur master : elles n'ont jamais été illustrées.

La règle est remplacée par une pierre tombale qui garde le mécanisme documenté :

```
/* ========================================
   #1369 - COUVERTURES ILLUSTREES DE VARIANTES (mecanisme)
   Mecanisme : la colonne CSV variant_class (lignes covers 1/7/9/11/13) alimente
   cardClass -> un token de classe sur <card>. Aucun bloc = aucun changement visuel.
   Sortie de repli (#1369) : le bloc d'illustration du pilote Rules_07
   (card.cover-bingo) est retire - style refuse ; reprise du style suivie
   dans #1472.
   ======================================== */
```

**Édition byte-exacte.** Le fichier est une chaîne JSON : on ne ré-sérialise rien (80 Ko réécrits pour deux retraits). Le script découpe des octets bruts et exige que chaque zone soit trouvée **exactement une fois** ; après écriture il revérifie : JSON parsable, **toutes** les autres clés identiques, deltas de longueur exacts, solde d'accolades conservé, CRLF conservé, absence de BOM.

**Aucun test ne cite** `cover-bingo`, `variant_class` ni #1472 ; « 1369 » n'y apparaît que comme fragment d'une empreinte SHA-256, sans rapport avec l'issue. Les deux gardes de police restent vertes sans toucher leurs listes gelées : `BrandFontCounterRuleContractTests` (Rules garde une règle `h1` à police de marque, Alfa Slab One) et `ExternalFontHostRatchetContractTests` (Rules reste offenseur par ses DINPro sur `fonts.cdnfonts.com`, donc la liste gelée ne bouge pas).

**Suite de tests** : `4 834` tests — **`4 828` réussis, `0` échec, `6` ignorés**, 2 min 34 s, mesuré sur un assemblage **recompilé** (`-t:Rebuild`) depuis les sources de HEAD plus le gabarit édité, les deux builds à 0 avertissement.

## 3. Périmètre : les PDF qui changeront à la prochaine régénération

**Méthode** : les 12 `CardSetDocumentConfig` de `WebBasedGeneratorConfig.cs` (l. 468 et suivantes) lus en découpant les accolades — 10 activés. **Un seul** document activé déclare `KnownCardSets.Rules` : `Argumentum_TarotCards_fr.pdf`, avec 7 traductions.

| PDF publié (release `v2.0.0-review`) | Taille (o) | `updatedAt` |
|---|---:|---|
| `Argumentum_TarotCards_ar.pdf` | 66 893 913 | 2026-09-30T07:52:30Z |
| `Argumentum_TarotCards_en.pdf` | 65 575 990 | 2026-09-30T07:53:17Z |
| `Argumentum_TarotCards_es.pdf` | 66 403 392 | 2026-09-30T07:54:09Z |
| `Argumentum_TarotCards_fa.pdf` | 68 109 427 | 2026-09-30T07:54:59Z |
| `Argumentum_TarotCards_fr.pdf` | 64 657 881 | 2026-09-30T07:55:46Z |
| `Argumentum_TarotCards_pt.pdf` | 66 935 648 | 2026-09-30T07:56:48Z |
| `Argumentum_TarotCards_ru.pdf` | 62 732 498 | 2026-09-30T07:57:30Z |
| `Argumentum_TarotCards_zh.pdf` | 69 390 488 | 2026-09-30T07:58:27Z |

**Où** : les Rules ouvrent le deck (15 cartes), donc Rules_07 est la **page 7** de chacun. Mesuré sur le PDF `fr` publié (Ghostscript, pages 1 à 15) : la bande du pilote (88–94 % de la hauteur) mesure une luminance de **71,3 en page 7**, sous une zone blanche (255,0) ; les 13 autres pages de règles mesurent **251,2**. La couverture (page 1) a bien un pied sombre (52,2) mais **sans blanc au-dessus** (97,7) : le critère apparié « bande sombre **et** blanc juste au-dessus » la laisse hors jeu. La position en page 7 n'est pas mesurée pour les 7 autres langues (elle découle de la définition du document).

**Hors périmètre, et mesuré** : `RulesPrintAndPlay` (2 documents activés ⇒ 16 PDF `…Print.Play…`) partage le même gabarit, mais son CSV (`Argumentum Rules - Cards Print and Play.csv`) **n'a pas** de colonne `variant_class`. Ses 7 images (6 faces + 1 dos) ont été rendues dans les deux passes : **7 identiques**.

## 4. La déclaration de police morte (commit A)

**Le rendu de contrôle était impossible avec le gabarit de master.** Première passe, gabarit de master tel quel : **0 image**, course abandonnée. Extrait verbatim du journal :

```
[BROWSER error] Failed to load resource: net::ERR_FAILED @
https://db.onlinewebfonts.com/t/8039f2717fa244ff6675db6a39c54d74.woff2:0:0

System.ApplicationException: Mismatch between generated image count (0) and
expected card count (6). Card IDs: [RulesPP_01, RulesPP_02, RulesPP_03,
RulesPP_04, RulesPP_05, RulesPP_06]
```

puis `[HARVEST-FAILURE] Card set 'Rules-Print&Play' / 'fr' failed and was skipped`, idem pour `Rules`, puis `[HARVEST-RETRY] Retrying 2 failed set(s) serially`. **L'échec est bruyant, pas silencieux.**

**Cause.** Le gabarit déclare `@font-face { font-family: "TrendSlabW00-Four"; … }` avec 6 `url()` sur `db.onlinewebfonts.com`. Plus aucune règle ne demande cette famille. `dom-to-image` récupère pourtant chaque `url()` **déclarée** ; l'hôte refuse (sonde du 2026-11-25T17:05Z : `HTTP 403`, `Cf-Mitigated: challenge`, `server: cloudflare`) ; la récupération échoue et **aucune** image n'est produite. Contrôle de l'autre hôte : `fonts.cdnfonts.com` (DINPro) répond **200**, `font/woff`, 41 692 o.

**Recensement** (`face_usage.py`, CSS lu **parsé** — jamais par regex sur les octets bruts, leçon #1485) : master = 6 blocs `@font-face` (4 DINPro, 1 TrendSlab, 1 Alfa Slab One), 7 usages de `font-family` hors `@font-face` ; **TrendSlab demandée 0 fois** ⇒ déclaration morte. Contrôle inverse de l'instrument : une famille inventée injectée est bien vue 1 fois.

**Chronologie** (git, `-S`) — la déclaration n'a pas toujours été morte :

| Commit | Date | `@font-face` | « TrendSlab » dans le fichier | État |
|---|---|---:|---:|---|
| `993e2f13` | 2023-04-16 | 5 | 3 | vivante (le bloc la nomme 2 fois + 1 usage) |
| `0fc5c235` (#1228) | 2026-09-01 | 5 | 4 | vivante (bloc ×2 + 2 usages) |
| `6b8c2f84` (#1707, #1485 passe 3) | 2026-10-02 | 6 | 3 | Alfa Slab One entre ; 1 usage TrendSlab subsiste |
| `0f408864` (#1716, #1485 passe 6) | 2026-10-03 | 6 | 2 | dernier usage retiré ⇒ **déclaration morte** |

⇒ **morte depuis le 2026-10-03**, trois jours après les assets publiés (30/09). Aucun asset publié n'a été produit avec la déclaration morte. Corollaire opérationnel : **toute régénération qui récolte les Rules avec le gabarit de master échoue** tant que cet hôte répond 403.

**Trois passes, même commande** (`fr`, Debug, 300 dpi, `LocalizationConfig.Enabled=false`, CardSets `Rules` + `Rules-Print&Play`, `StopBeforePdfGeneration=true`) :

| Passe | Gabarit | Images | Fin de course |
|---|---|---:|---|
| 1 | master | **0** | exception non interceptée, aucun PNG, aucune récolte |
| 2 | master − `@font-face` mort | **22** | `Generation finished.` |
| 3 | passe 2 − `card.cover-bingo safe` | **22** | `Generation finished.` |

Le commit A est **indépendant** du reste : si une régénération doit avoir lieu avant la sortie de #2132, il peut être repris seul (premier commit de la branche).

## 5. A/B pixel — ce qui change exactement

**Modèle de comparaison** : la passe 2 est le témoin « avant » (nécessaire, cf. §4), la passe 3 l'« après ». L'écart mesuré est donc celui du **seul commit B**.

**Oracle = pixels, jamais md5** : deux sessions CardPen rendent des PNG pixel-identiques aux octets différents (piège déjà mesuré sur cette issue). Trois contrôles de l'instrument sont joués à chaque exécution : un fichier contre lui-même ⇒ IDENTIQUE ; Rules_07 contre Rules_08 ⇒ DIFFÉRENT ; une copie de Rules_08 à ±1 niveau sur 1 pixel ⇒ vue. **Instrument sain.**

**Récolte** (ce que CardPen a rendu, `dataUrl` décodés, 897×1497) : **22 images — 21 identiques** (IDAT *et* octets égaux), **1 différente**, Rules_07 : 86 026 px (6,406 %), bbox (71, 390, 825, 1425), écart max 255.

| Zone | Pixels | Nature |
|---|---:|---|
| fort (≥ 32 niveaux) | 84 947 | lignes **1306–1424** = 119 lignes = 38 px CSS × 300/96 = 118,8 px, colonnes 72–824 |
| faible (1–31 niveaux) | 1 079 | 998 px à exactement 4 niveaux + 8 px à 1–3, sur la colonne **x=71** (251 → 255) : filet de 1 px au bord gauche de l'élément qui portait l'illustration ; + 73 px à 6–31 niveaux dans la bande (détails clairs de la photo, coins arrondis) |

Le profil par tranches de 100 lignes le confirme : **0** px sur les lignes 0–299, puis exactement **100 px par tranche** de 400 à 1299 (1 px par ligne = le filet) avec un écart max de **4**, puis 69 351 px (1300–1399) et 15 765 px (1400–1496) avec un écart max de 255.

**Disque** (PNG de `Images/density-0/`, 826×1417) : 21 identiques (pixels **et** IDAT ; octets différents), `rules_07_face.png` : 78 529 px (6,709 %), dont 76 559 forts.

Luminance moyenne pleine largeur (disque) — c'est le tableau qui dit ce que devient le bas de carte :

| Lignes | 07 avant | 07 après | 06 (avant = après) | 08 (avant = après) |
|---|---:|---:|---:|---:|
| 1220–1259 | 141,4 | **255,0** | 255,0 | 255,0 |
| 1260–1299 | 71,8 | **255,0** | 255,0 | 255,0 |
| 1300–1339 | 70,8 | **239,4** | 239,4 | 239,4 |
| 1340–1379 | 210,1 | 254,6 | 245,6 | 244,7 |
| 1380–1416 | 254,9 | 254,9 | 254,8 | 254,8 |
| **bande 1236–1348** | **68,9** | **249,4** | 249,4 | 249,4 |

Deux lectures, mesurées et non déduites :

- « après » **n'est pas un blanc uniforme** : les lignes 1300–1339 valent **239,4**, exactement la valeur des témoins 06 et 08 — c'est le pied de carte normal, que la photo recouvrait avant.
- sous la bande (lignes 1349–1416) : **0 pixel différent**. Le reste de la carte est intact ; hors bande il ne subsiste que **1 733 px**, tous sur les colonnes **56 et 57** (251/254 → 255, écarts de 1 et 4 niveaux).

## 6. Planches

Aucun tracé par-dessus une carte : réduction LANCZOS ou recadrage brut, rien d'autre. Chaque planche porte un contrôle programmé (rejoué à chaque construction, `VERDICT : OK`) :

| Fichier | Taille | Contrôle (luminance de la bande) |
|---|---|---|
| `regles-07-avant-apres.png` | 880×836, 183 754 o, `d151bc2cc2f5d29a…` | avant **68,6** / après **249,3** |
| `regles-07-bande-100pct.png` | 862×628, 118 560 o, `fe5ae7a722ea7538…` | à 100 % : avant **68,9** / après **249,4** |
| `pdf-publie-p06-a-p08.png` | 816×527, 157 574 o, `2b19641fe3c59f5a…` | pages 6/7/8 : **250,7 / 71,9 / 250,7** |

Une quatrième planche (`regles-06-07-08-apres.png`, trois cartes voisines, 354 109 o) est livrée **hors dépôt**, sur `G:` seulement, pour ne pas alourdir l'arbre.

## 7. Instruments (dans ce dossier)

```
python compare.py <rendu-avant/Target> <rendu-apres/Target> [lang]
python face_usage.py <gabarit.json>
python make_sheets.py <racine>          # racine = dossier des passes + pub/
```

`compare.py` (oracle pixel, trois contrôles, écrit `compare-<lang>.json`/`.md`), `face_usage.py` (croisement déclaré/demandé, contrôle inverse), `make_sheets.py` (planches + leurs contrôles, `VERDICT : OK` ou sortie 2). Les sorties mesurées sont jointes : `compare-fr.md`, `controles-planches.json`.

## 8. Limites — ce qui n'est pas mesuré

1. **Une seule langue rendue** (`fr`). Les 7 autres partagent le même gabarit et les mêmes colonnes CSV : même retrait attendu, **non mesuré**.
2. **Debug uniquement** ; le chemin Release n'a pas été exécuté.
3. **Aucune image n'a été regardée** sur ce siège : le verdict visuel est à ai-01.
4. Les quatre **DINPro restent sur `fonts.cdnfonts.com`** (200 le 25/11). Un 403 de cet hôte reproduirait l'échec du §4 — c'est le sujet de fond de #1485, pas de ce lot.
5. **Aucun PDF n'est régénéré ici** : le lot livre le retrait et son rendu de contrôle, pas les 8 PDF.
6. L'asset **`cover-bingo.png` (1 640 150 o) reste dans le dépôt**, désormais sans aucune référence CSS. Sa suppression est une décision #1472 / owner, pas de ce lot (aucune suppression sans preuve de préservation).
