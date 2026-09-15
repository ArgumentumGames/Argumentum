# Guide de validation release v2.0.0 — édition 3

> **Document vivant** (édition courante du guide C1→C6). Remplace l'édition 1
> ([c.5599521969](https://github.com/ArgumentumGames/Argumentum/issues/134#issuecomment-5599521969), 09/09, bundle 06/09)
> et l'édition 2 ([c.5643507137](https://github.com/ArgumentumGames/Argumentum/issues/134#issuecomment-5643507137), 12/09, bundle 11/09).
> Complète [`134-bundle-verification-recipes.md`](134-bundle-verification-recipes.md) (po-2023, #1346) sans le dupliquer :
> les **recettes** disent comment re-mesurer et nomment les pièges d'instrument ; ce **guide** dit quoi accepter.
>
> Discipline inchangée : chaque contrôle = **COMMANDE + ATTENDU + CONSTAT**. Le guide émet des mesures et des
> écarts, **jamais un PASS** — le worker signale, **ai-01/owner déclare**.
>
> **Pourquoi une édition 3** — trois points de l'édition 2 ont été falsifiés ou périmés par le verdict du 12/09 23:38
> ([c.5649441292](https://github.com/ArgumentumGames/Argumentum/issues/134#issuecomment-5649441292)) et ses suites :
> 1. **Critère SWOP faux** : la chaîne « SWOP » est **absente** du bundle courant — un `grep SWOP` rend 0, c'est une panne
>    d'instrument, pas une non-conformité (verdict §4, recettes §4.3). Critères stables : `CGATS TR 001` + `/GTS_PDFX` + log de la passe (C3).
> 2. **Sentinelle 720 dpi périmée** : l'« arbitrage en cours » de l'édition 2 a été tranché le 12/09 08:50
>    (option (a), strip d'alpha, [#1338](https://github.com/ArgumentumGames/Argumentum/pull/1338) `381ee5c8`) ; le bundle
>    courant est **300 dpi natif**. Sur l'artefact livré, le critère de non-défectuosité est la **dimension**, pas `/SMask` (C6).
> 3. **Ancre périmée** : l'édition 2 décrivait le bundle `review-v2.0.0-regen-20260911` (7 071,9 Mo, aplati 720 dpi) ;
>    le paquet à publier est `review-v2.0.0-regen-20260912`.
>
> Origine : pool multi-cycle #458, grain ④ (dispatch ai-01 14/09) ; suites 1 et 2 exigées par le verdict c.5649441292 §8.

---

## 0. Ancrage explicite — le bundle courant (à re-vérifier à chaque déroulé)

| Artefact | Valeur |
|---|---|
| **Bundle courant** | `review-v2.0.0-regen-20260912` — 80 PDF (8 langues × 10 types), **3 745,6 Mo**, base `65dd4742` (inclut #1338 strip alpha) |
| Manifeste | `manifest-v2.0.0.md` — `Status: FINAL`, 80 empreintes sha256, provenance déclarée `65dd4742` |
| Prédécesseur **préservé** | `review-v2.0.0-regen-20260911` — 80 PDF, 7 071,9 Mo, base `6108194d`, pages aplaties 720 dpi : c'est le **témoin du défaut** discriminé en C6 |
| Layout | `{lang}/Argumentum_{type}_{lang}.pdf`, `lang ∈ {ar,en,es,fa,fr,pt,ru,zh}`, `type` = les 10 de `package-v2.0.0.ps1` |

⚠️ **Fraîcheur au jour d'édition (MESURÉ 15/09, `origin/master` `d7013df1`)** : `git diff --name-only 65dd4742..origin/master -- Cards Generation`
rend **5 fichiers** : `Cards/Scenarii/Argumentum Scenarii - Cards.csv` (commits `52ff57e4` #1367 + `29ce5d03` #1375 — les **71 cellules EN du GO
owner #994**) et **4 fichiers de tests** (inertes pour le rendu). **Le bundle livré ne contient pas ces 71 cellules Scenarii EN.** Ce constat
n'est ni un bloqueur ni une autorisation de régén — la décision appartient à ai-01/owner. La sentinelle fraîcheur (C6) se re-mesure à chaque déroulé.

**Outillage** (constat po-2023 09/09, inchangé) : `gswin64c` 10.07.1 · Python + PyMuPDF / pypdf · `pdftotext`.
⚠️ `pdfinfo`/`pdfimages`/`pdftoppm` absents sur les machines de la flotte — remplacés ci-dessous et dans les recettes.

---

## C1 — Inventaire & manifeste (périmètre du paquet)

```powershell
Get-ChildItem -Recurse -Filter *.pdf | Group-Object { $_.Directory.Name } | Measure-Object -Property Length -Sum
Select-String -Path manifest-v2.0.0.md -Pattern '^sha256|^- ' | Measure-Object
```
- **Attendu** : **80 PDF** (8 langues × 10 types) · `manifest-v2.0.0.md` **`Status: FINAL`** avec **80 empreintes sha256**
  (⚠️ nom de fichier `manifest-v2.0.0.md`, pas `manifest.md` — l'ancre du bundle 06/09 ne s'applique plus) · section **A0 explicite** (8 affiches).
- **Référence mesurée (RAPPORTÉ, verdict 12/09 §6)** : 80/80 fichiers, 3 745,6 Mo ; 4 sha256 recalculés concordants **et** contrôle
  inverse (hash d'une autre langue → MISMATCH) — le comparateur n'est pas aveugle.
- ⚠️ **`dossier-validation-mesures.md`** : dernier état mesuré = **ABSENT** du répertoire de revue (verdict 12/09 §8 — suite requise n°2,
  toujours ouverte à l'édition de ce guide). Le déroulé doit re-constater sa présence ou documenter l'écart.
- Répartition de taille par langue : **à émettre au déroulé** — les chiffres de l'édition 2 (ar 866 · en 892 · …, total 7 071,9 Mo)
  décrivent le bundle 11/09 ; ne pas les recopier. Heuristique conservée : une langue anormalement légère ou lourde = écart
  (ordre de grandeur indicatif sur le 11/09 : écart max/min ≈ 5,7 %).

## C2 — Comptes de cartes & contrat du deck (fiabilité du façonnage)

```bash
gswin64c -q -dNODISPLAY -dNOSAFER -c "(<chemin.pdf>) (r) file runpdfbegin pdfpagecount = quit"
```

**Attendus — contrat mesuré, uniforme sur les 8 langues (RAPPORTÉ, verdicts 11/09 §1 + 12/09 §2) :**

| Type | Pages | Contrat dérivé |
|---|---|---|
| `TarotCards` | **379** | 15 Rules (sans dos) + Memo face×7 + dos×7 + Fallacies 175×2 — soit 191 cartes : **175 Fallacies + 15 Rules + 1 Memo** *(197 instances imprimées = volume de fabrication décidé 14/09, #1187 c.5665864605)* |
| `TarotCards_Virtues` | **262** | 131 cartes (faces + dos) |
| `PokerCards` | **334** | 167 faces + 167 dos |
| `TarotCards_Print&Play_A4` | 105 | — |
| `TarotCards_Print&Play_Light_A4` | 21 | — |
| `PokerCards_Print&Play_A4` | 38 | 19 + 19 |
| `PokerCards_Print&Play_Light_A4` | **6** | ⚠️ omis par la 1ʳᵉ rédaction du verdict 11/09, comblé — ne pas le perdre en recopiant |
| `Fallacies_Web_A0` | 1 | affiche (8 présentes) |
| `Fallacies_Web_A4` | 15 | — |
| `Fallacies_Web_Thumbnails_A4` | 9 | — |

⚠️ **Correction d'attendu vs le guide 09/09** : « 191 cartes (176 Fallacies + 15 Rules) » avait un total juste et une décomposition
fausse — le deck porte **175 faces Fallacies** (+ 1 face Memo répétée ×7) et la face Memo compte dans les 191. Contrat recto-verso
dérivé par identité d'image (aucun nombre codé dur) : 182 pages paires → **2 images de dos** ; 182 impaires → **176 images distinctes** ;
recouvrement dos∩faces = **0**.

⚠️ **Piège des dos localisés (MESURÉ, c.5643251291)** : **aucun dos n'est commun aux 8 langues** — les 2 familles de dos portent le
**titre du jeu traduit** (8/8 pixels distincts). Un contrôle inter-langues qui attend un dos commun rendra « 0 dos commun » :
**c'est le résultat correct**, pas une anomalie. Un devis d'impression compte **un dos par langue** (jamais « un dos partagé »).

Géométries mesurées (RAPPORTÉ, verdict 11/09 §1) : tarot **69,9 × 120,0 mm** · poker **63,5 × 88,9 mm** · A4 209,9 × 297,0 · A0 841,0 × 1189,2.

## C3 — Conformité imprimeur (CMYK) — **critère corrigé (suite exigée n°1)**

```bash
grep -a -c "/DeviceCMYK"    <fichier.pdf>   # attendu > 0
grep -a -c "/DeviceRGB"     <fichier.pdf>   # attendu 0
grep -a -c "/GTS_PDFX"      <fichier.pdf>   # attendu >= 1
grep -a -c "CGATS TR 001"   <fichier.pdf>   # attendu >= 1
grep -a -c "GPL Ghostscript" <fichier.pdf>  # attendu >= 1  (Producer = GPL Ghostscript 10.07.1)
```

- **Log de la passe** (recettes §3.2) : dans le log de `--pdf-cmyk`, la ligne `PDF CMYK post-process complete: N/80 converted` —
  **N = 80 attendu, 0 warning**. ⛔ Un exit 0 reste vert même si Ghostscript est absent (la passe saute chaque PDF avec un simple
  warning) : **le log est la preuve, pas le code de sortie**.
- ⛔ **ANTI-CRITÈRE retiré de l'édition 2** : `grep -a -c "SWOP"` rend **0** sur ce bundle — MESURÉ (verdict 12/09 §4, recettes §4.3).
  L'attendu d'édition 2 « `Info = U.S. Web Coated (SWOP) v2` » était vrai sur le bundle 11/09 mais **ne l'est plus sur le paquet à
  publier** : un contrôle SWOP vert/rouge sur ce bundle est une panne d'instrument, pas une mesure. Les littéraux stables sont
  `/GTS_PDFX` et `CGATS TR 001` (l'OutputConditionIdentifier **est** la référence SWOP — c'est lui qu'on vérifie).
- **Référence mesurée (RAPPORTÉ, verdict 12/09 §4)** : **17 352/17 352** objets image en DeviceCMYK · **80/80** fichiers portant
  `/GTS_PDFX` + `CGATS TR 001`.
- ⚠️ Toujours vrai : un run `-c Release` **sans** `--pdf-cmyk` livre du RGB sans erreur.

## C4 — Les 8 langues réellement distinctes (papier localisé)

Instrument inchangé (rastérisation PyMuPDF + MD5 par page ; rastériseur ≠ producteur GS = corroboration) :

```python
import fitz, hashlib
def pages_md5(path, dpi=50):
    doc = fitz.open(path); return [hashlib.md5(p.get_pixmap(dpi=(dpi,dpi)).samples).hexdigest() for p in doc]
a, b = pages_md5("Argumentum_TarotCards_en.pdf"), pages_md5("Argumentum_TarotCards_fr.pdf")
print("pages", len(a), "| identiques", sum(1 for x, y in zip(a, b) if x == y))
```
- **Attendu** : `identiques 0` par paire ; `> 0` = CONSTAT, pas un PASS. Limite d'instrument : l'identité de page détecte la
  **recopie intégrale**, pas la conformité.
- **Références** : balayage intégral **3 032 pages** sur le bundle 11/09 — 182/182 pages de face distinctes entre les 8 langues,
  classe #216 écartée par balayage (c.5643251291). Sur le **bundle 12/09** : balayage d'encre **9 360/9 360 pages**, 0 suspecte,
  0 erreur de rendu, détecteur auto-testé (c.5649441292 §1) — l'identité de page inter-langues sur le 12/09 reste **à émettre au déroulé**.
- Posters `Fallacies_Web_*` vectorisés (`pdftotext` muet) : raster uniquement.

## C5 — Les 7 dos Scenarii (contrainte de façonnage)

Méthode inchangée : pages **impaires** du `PokerCards`, MD5 raster 25 dpi (recette dans l'édition 2 / guide 09/09).
**Attendu** : exactement **7 empreintes distinctes** sur les 167 dos — un 8ᵉ dos = écart. Les 7 catégories : `histoire`,
`mythologie`, `politique`, `pop_culture`, `relation_intime`, `vie_personnelle`, `vie_professionnelle` (#1187).
Référence (RAPPORTÉ, bundle 06/09) : 7 dos, répartition 17/27/36/30/25/18/14 — non re-mesuré sur le bundle 12/09 : **à émettre au
déroulé**. Constat visuel final (teinte/contraste) = ai-01, jamais le script.

## C6 — Sentinelles transverses — **réécrites (suite exigée n°1)**

| Sentinelle | Contrôle |
|---|---|
| **Dimension des objets image — LE critère de non-défectuosité sur l'artefact livré** | Recette §3.1 du doc recettes (`get_image_info(xrefs=True)`, jamais `get_images()` qui déduplique — §4.2). **Attendu** : cartes Tarot/Virtues **826×1417** ; A0 = **175 images 779×779 + 1 bandeau 1316×475**. **Valeurs du défaut à discriminer** : `1982×3401` (upsample nearest-neighbor ×991/413 du bug 720 dpi, corrigé par #1338) et A0 monolithique `23 840×33 710`. **Référence mesurée (RAPPORTÉ, verdict 12/09 §2 + recettes §3.1)** : Tarot fr **379/379** @826×1417, témoin non-latin zh 379/379, Virtues fr 262/262, A0 fr 176 objets — et **aucune occurrence de 1982×3401 nulle part dans le bundle** |
| **`/SMask` — retiré comme critère sur le livré** | La sonde `/SMask = 0` est **aveugle** sur le PDF distribué : l'aplatissement PDF/X consomme l'alpha, un bundle connu défectueux (11/09) mesure 0 exactement comme un bundle correct (contrôle positif + négatif, c.5649441292 §3). `/SMask` reste signifiant **uniquement sur l'arbre pré-CMYK `Target/`** — arbre recyclé post-livraison, mesure non re-mesurable (recettes §2). ⛔ Ne jamais cocher `/SMask = 0` sur le paquet distribué comme preuve de quoi que ce soit |
| **Fraîcheur** | `git diff --name-only 65dd4742..origin/master -- Cards Generation`, re-mesuré au déroulé. État au jour d'édition : voir §0 (71 cellules Scenarii EN post-bundle + tests inertes) |
| 8 A0 présentes | C1 (section A0 du manifeste) |
| Versioning | `owl:versionInfo = 1.0.0` dans `docs/ontology/argumentum.owl` (**blob master**, pas l'arbre local — trap pré-#1286 documentée sous #133) |
| Déterminisme | deux régens d'un même type à base identique → pages identiques |
| ~~Risque RIP A0~~ | **Dissous sur l'artefact livré** (MESURÉ, verdict 12/09 §2 : l'A0 = 176 images modestes, pas un objet monstre). Ne revient que si un objet unique géant réapparaît — couvert par la sentinelle dimension |

---

## Protocole d'exécution (inchangé)

1. **Stamper** chaque run : date-heure, chemin bundle, `git rev-parse origin/master`, versions d'outils.
2. Exécuter C1→C6 sur le paquet **distribué** (le bundle réel, pas une copie de travail).
3. Renseigner les constats (mesure + écart) sous #134 ; **aucune conclusion globale** dans le guide.
4. Un déroulé ne devient verdict que par ai-01/owner.

## Ce que ce guide n'établit pas

- ❌ Ni l'esthétique (verdict ai-01/owner, comparateur #1313), ni l'arbitrage éditorial (#994 et successeurs), ni la recette DNN (#1180).
- ❌ Aucun PASS : le guide émet des mesures ; **le worker signale, ai-01 déclare** (rôle QA visuelle réservé).
- ❌ Ne re-génère rien, n'atteint pas l'arbre pré-CMYK (recyclé — recettes §2), ne tag ni n'uploade.
- ❌ La fraîcheur constatée au 15/09 (71 cellules Scenarii EN hors bundle) n'est ni un bloqueur ni une autorisation de régén.

---

*Édition 3 rédigée par **po-2024** (worker lane), pool multi-cycle #458 grain ④, 15/09/2026. Qualificateurs :
MESURÉ = re-mesuré par l'auteur de l'édition ; RAPPORTÉ = cité d'une mesure publiée et datée (source nommée). Aucun chiffre
de l'édition 2 décrivant le bundle 11/09 n'a été recopié comme décrivant le paquet à publier.*
