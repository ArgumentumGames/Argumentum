# #1499 G6 — chiffrage des divergences : archive 2022 vs corpus courant, 69 cartes appareillées

**Auteur** : po-2024 (worker) · **Date** : 2026-09-25 · **Base** : `measure/1497-tiering-B` (= master `1833de59`)
**Grain** : pool #458, ordre ai-01 c.5830348025 — *« G6 READY : joindre sur E1 + E2, ⛔ jamais `path` seul. Les 8 non résolues forment un périmètre séparé (décision humaine), les 2 E2 LIMITES sont qualifiées comme telles. »*
**Instrument** : [`tools/1499-g6-chiffrage-divergences.py`](../../tools/1499-g6-chiffrage-divergences.py) — importe le module G5 (clé validée), **0 écriture** · détail nominatif : [`tools/1499-g6-chiffrage.json`](../../tools/1499-g6-chiffrage.json)

---

## 1. Le chiffre que G5 s'était interdit de citer

G5 avait validé la clé de jointure et posé la borne : *« le décompte des cellules changées sur les 69 cartes appariées n'a pas été fait et ne doit pas être cité »*. Le voici, mesuré :

| | cellules |
|---|---:|
| Surface comparable (16 champs × 69 paires) | **1 104** |
| **Cellules divergentes** | **715 (64,8 %)** |
| — `PERDU` (archive remplie → courant vide) | **0** |
| — `GAGNE` (archive vide → courant remplie) | **67** |
| — `MODIFIE` (les deux remplies, textes différents) | **648** |

Comparaison **exacte de chaînes** (trim seul) : un écart d'espace, de casse ou de ponctuation est une divergence — puis sous-qualifié au § 2. Les 16 champs : fr `titre·baratineur·piocheur·contexte·enjeu·suggestion·catégorie·sous-catégorie` + en `title·smoothTalker·drawer·context·issue·suggestion_en·category·subcategory` — **l'archive 2022 n'a que 20 colonnes (fr + en)** : les 6 autres langues n'y existent pas, leur divergence n'est pas mesurable contre elle.

## 2. Les 648 MODIFIE, sous-qualifiées — sinon le chiffre mentirait

Un « 648 MODIFIE » brut se lit comme 648 réécritures. La similarité token (casefoldée, ponctuation retirée — calibration mesurée sur path 1.1.1, où la v1 case-sensible comptait « la mère » → « La mère » comme retouche) sépare trois natures :

| Sous-nature | Seuil (aide de lecture, nommé) | Cellules |
|---|---|---:|
| **quasi-identiques** | sim ≥ 0,9 — casse, ponctuation, guillemets | **198** |
| **retouches** | 0,5 ≤ sim < 0,9 — enrichissement lexical (« César » → « Jules César ») | **179** |
| **réécritures** | sim < 0,5 — autre texte | **271** |

Médiane sim : 0,571. ⚠️ Ces seuils sont des **aides de lecture nommées**, pas des vérités — le JSON porte la similarité par cellule, l'arbitrage peut recalibrer.

## 3. Trois lectures pour l'arbitrage

1. **PERDU = 0** : le corpus courant n'a **rien perdu** de ce que l'archive 2022 portait sur les cartes appareillées. La divergence est faite de **réécritures et d'ajouts**, pas de pertes — le motif de restauration massif (celui de G2-C-W) n'existe pas sur ces 69 cartes.
2. **GAGNE = 67, tout en anglais** : champs `en` vides en 2022, remplis depuis. Ce sont des **complétions**, pas des divergences à annuler.
3. **MODIFIE fr = 345** : la question d'arbitrage n'est pas « restaurer » (rien n'est perdu) mais **laquelle des deux versions fait foi** — l'archive 2022 ou le courant retravaillé (sweeps #1025, campagnes G2-C, enrichissements). Pour 198 d'entre elles la différence est typographique.

Par champ, les plus divergents : `suggestion` 64 · `contexte` 63 · `category` 61 · `titre` 60 · `context`/`issue` 59 · `enjeu` 56.

## 4. Périmètres qualifiés (exigés par le dispatch)

- **Les 2 paires E2 LIMITES** — path **3.1.3** (sim contenu 0,233) et path **5.1.3** (0,163) — portent **25 cellules divergentes**, marquées `paire_limite` dans le JSON : leur appariement reste **récusable à la lecture** ; les compter sans ce marquage ferait porter au chiffrage un appariement faible.
- **Les 8 non résolues** (G5 : Louis XVI, Truman, Don Juan, mariage ou pas, 2+1, rétrogradation canapé, 5G, miaw) restent un **périmètre séparé, décision humaine** — hors chiffrage.
- **es/ar/fa/zh/ru/pt** : pas dans l'archive — seule leur cohérence avec le fr courant est mesurable (autre instrument, si arbitrée).

## 5. Ce que ce chiffrage n'établit pas

- ⛔ **Aucune direction de correction** — restaurer l'archive ou garder le courant est un arbitrage owner ; ce grain mesure, il ne tranche pas.
- La similarité token ignore l'ordre des mots et le sens (deux textes reshufflés peuvent être « quasi-identiques ») — aide de lecture, cf. § 2.
- La jointure hérite des 13 paires E2 : 11 CONFORTABLES (sim ≥ 2× plancher), 2 LIMITES marquées.
- **69 cartes sur 77** : la couverture est celle de G5 (89,6 %), ni plus ni moins.
- Snapshot du 25/09 sur `1833de59` + branche du grain (0 écriture corpus).

## 6. Exécutable

```
python tools/1499-g6-chiffrage-divergences.py --self-test   # rc=0 — natures comptées une à une
python tools/1499-g6-chiffrage-divergences.py               # chiffrage + sous-qualification
python tools/1499-g6-chiffrage-divergences.py --json-out tools/1499-g6-chiffrage.json
```

---

*« 308 cellules » était un chiffre sans clé ; « 648 MODIFIE » aurait été un chiffre sans natures. Le dénominateur juste : **0 perdues, 67 gagnées, 198 normalisées, 179 retouchées, 271 réécrites** — et 25 d'entre elles portées par des paires qu'on peut récuser.*
