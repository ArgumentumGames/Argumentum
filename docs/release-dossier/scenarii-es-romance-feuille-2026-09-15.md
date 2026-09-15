# Scenarii `subcategory_es` « romance » — feuille de décision (1 coche)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `6fbde739`
**Grain** : pool #458 renouvelé (c.5666260217), grain ⑤. Question éditoriale ouverte documentée par
la re-mesure i18n ([`i18n-remesure-es-ar-fa-zh.md`](../translation/i18n-remesure-es-ar-fa-zh.md)) :
« romance » en castillan est-il le bon libellé là où l'anglais et le portugais portent « Romance » ?
**0 write sans GO** — cette feuille est un support de décision, rien n'est modifié.

---

## §0 L'objet, re-mesuré

La spec du grain comptait « 1 cellule » — **re-mesuré firsthand** (CSV vivant
`Cards/Scenarii/Argumentum Scenarii - Cards.csv`, `6fbde739`) : c'est **1 question × 11 cellules** —
toutes les rangées de la sous-catégorie portent la même valeur :

| Langue | Valeur (verbatim, casse mesurée) | Rangées |
|---|---|---:|
| `subcategory` (EN canonique) | `Romance` | 11 |
| `subcategory_pt` | `Romance` | 11 |
| `subcategory_es` | **`romance`** | **11** |
| `subcategory_ru` | `Романтика` | 11 |
| `subcategory_ar` / `_zh` / `_fa` | localisent (رومانسية / 浪漫爱情 / عاشقانه) | 11 |

Catégorie parente : `Intimate relations` (36 rangées). Les sous-catégories ES voisines dans la même
catégorie : `vida en pareja` (16) et `ligoteo y seducción` (9).

## §1 Les faits qui cadrent la décision (MESURÉS sauf mention)

1. **La casse n'est pas un argument** — écarté d'emblée : `subcategory_es` est minuscule-dominante
   (19/21 valeurs : « vida en pareja », « religiones », « cuentos »… ; seules « Edad Media y era
   moderna » et « Cómics » portent une majuscule, mots propres). « romance » minuscule = convention
   dominante de la colonne, pas une négligence.
2. **La valeur n'est rendue nulle part dans les livrables** : le `mustache` des faces et dos
   Scenarii ne référence pas la sous-catégorie (les occurrences `subcategory` des templates vivent
   dans la clé `csv` embarquée STALE — header de l'instantané) ; les 7 dos sont indexés sur la
   **catégorie**, pas la sous-catégorie. Un changement de valeur = **0 impact visuel PDF**.
3. **Les consommateurs code** : `Scenario.cs` mappe `subcategory_es` **par nom** (CsvHelper) — la
   valeur traverse sans validation de fond ; la sync GSheet contractualise les **colonnes**, pas les
   valeurs. Un remplacement ne casse aucun instrument.
4. **Appréciation lexicale (SUPPOSÉ — jugement worker, pas mesure)** : « romance » existe en
   castillan au sens « relación amorosa », avec une nuance qui penche **passagère/clandestine**
   (« amorío ») ; l'anglais « Romance » (catégorie) est plus large. Le triptyque ES se tient
   néanmoins : `vida en pareja` (couple installé) / `romance` (relation amoureuse) / `ligoteo y
   seducción` (drague) — la distinction que l'anglais fait entre Couple life / Romance / Flirting.

## §2 Les branches

| | **A — Statu quo** *(reco)* | **B — Remplacement ES** |
|---|---|---|
| **Geste** | aucun | 11 cellules `subcategory_es` (même valeur partout, insertion byte-exacte) |
| **Candidats B** | — | « relaciones románticas » (littéral, long) · « amoríos » (connotation affaiblie) · « romántico » (adjectif seul) — aucun candidat évident |
| **Impact livrables** | 0 | **0 aussi** (non rendu, §1.2) — la seule retombée = CSV source + sync GSheet |
| **Risque** | un locuteur natif peut lire « romance » comme plus passager que l'anglais | perdre la symétrie EN/PT « Romance » ; ouvrir la même question pour PT (« romance » = *roman* en portugais, faux-ami symétrique jamais posé) |

**Pourquoi la reco A** : le mot castillan couvre le sens voulu (§1.4), la triade sémantique ES est
plus fine que l'anglais (atout pédagogique pour un jeu de scénarios), le changement n'achète **rien
de visible** et ouvrirait la question symétrique PT. La question avait été soulevée comme
« faux-ami » possible — la re-mesure la ramène à une **nuance de registre**, pas à une erreur de
traduction.

## §3 La coche (owner)

- [ ] **ES « romance »** : ☐ **A — statu quo** (reco) · ☐ **B — remplacement** (valeur à nommer : ______)

Si B : le geste est **gated** (GO nommé requis — gel corpus), exécution worker en insertion
byte-exacte + synchronisation GSheet à programmer ; aucune régén PDF nécessaire (§1.2).

## §4 Ce que cette feuille n'établit pas

⛔ Pas de validation par locuteur natif (l'appréciation lexicale §1.4 est SUPPOSÉE, jugement
worker) · ⛔ pas de consultation RAE/dictionnaire en ligne (hors ligne ce tick) · ⛔ 0 write — le
gel corpus tient · ⛔ la question PT symétrique (« romance » = *roman*) est **nommée, pas traitée** ·
⛔ aucune modification de template/CSV/PDF.

---
*po-2024 — pool #458 renouvelé (c.5666260217), grain ⑤. Feuille autonome ; la question vit dans le
dossier i18n (re-mesure 13/09) et se coche ici en 10 secondes.*
