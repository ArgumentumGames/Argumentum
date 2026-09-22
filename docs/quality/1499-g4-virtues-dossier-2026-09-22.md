# #1499 grain G4 — Dossier d'arbitrage Virtues : les 312 cellules fr des 131 cartes imprimables

**Date** : 2026-09-22 (tick 22:41) · **Lane** : po-2023 (worker) · **Base** : `origin/master` `ac797822`
**Instrument** : [`docs/corpus/virtues-baseline-diff.py`](../corpus/virtues-baseline-diff.py) —
exécuté **tel que livré** avant commit (rc=0, témoins a–d PASS ; la CI n'exécute pas `docs/`).
**Périmètre dispatché** : #1499 c.`5777618740` + c.`5777638672` — « G4 (Virtues, 312 cellules fr dont
remark_fr sur 131/131 cartes imprimables) ».

**⛔ Garde centrale respectée** : aucune écriture dans le CSV ni un gabarit. Ce document est un
**dossier d'arbitrage** — la décision de changer une carte déjà imprimée revient à l'owner, carte par carte.

---

## Réponse en une ligne

Le périmètre dispatché est **recoupé à la cellule près** (312 = title 53 + description 128 + remark 131
sur card=1), l'origine est **datée et attribuée** (PR #367, 28/05/2026, gpt-5.5, « FR clarity pass »,
516 cellules), et le fait saillant est une **condensation systématique des remarques** : 131/131
raccourcies, médiane **283 → 139 caractères**.

## 1. Ce que la mesure établit (instrument livré, rc=0)

Périmètre `card=1` (131 cartes imprimables) — jointure `pk`, invariant `path` :

| Colonne | Cellules changées | Médiane longueur | Raccourcies | Similarité <0.6 (réécriture forte) |
|---|---:|---|---|---:|
| `title_fr` | 53 | 30 → 23 car. | 37/53 | 15 |
| `description_fr` | 128 | 117 → 101 car. | 106/128 | 13 |
| `remark_fr` | **131** | **283 → 139 car.** | **131/131** | **117** |
| **contenu (dispatch)** | **312** | | | |
| `family_fr`/`sub`/`subsub` | 42+11+5 = 58 | | | (renommages d'août, cf. §3) |
| `link_fr` | 20 | | | (#1442 liens morts, #1470) |
| **total mesuré** | **390** | | | |

Sur l'arbre complet (223 nœuds) : 660 cellules fr (523 contenu + 137 structure/lien), `remark_fr`
**223/223 raccourcies** (médiane 278→140) — la condensation n'est pas un effet du filtre « imprimables ».

**Témoins** : (a) pk uniques 223/223 des deux côtés · (b) invariant `path` **223/223** · (c) contrôle
inverse `family_fr` = **93 divergences = les 3 renommages d'août** (55+20+18 ; les 130 restants =
58,3 % identiques — le piège d'instrument du dispatch est désamorcé) · (d) les renommages apparaissent
dans la sortie (non-zéro).

## 2. Origine datée — ce ne sont PAS les restaurations d'août

⚠️ Piège évité (bodies lus avant citation) : les PR d'août #1106/#1108/#1119 (« remark/description/title
cells restored », gpt-5.6-sol) corrigeaient **7 langues hors fr** — le fr y est le **pivot intouché**.
Leur « 53 title cells » est une **coïncidence numérique** avec mes 53 `title_fr`.

L'introduction réelle des textes HEAD (sonde `git log -S` sur les 8 remarks les plus réécrits) :
**`b76af806`, 2026-05-28, PR #367 « data(virtues): FR clarity pass gpt-5.5 (223/223 records, 516 cells) »**
— 7/8 sondés. #367 revendique : titres « plus courts, plus percutants », remarks « exemples concrets
remplaçant des définitions abstraites », « No meaning regression observed » — **sur un échantillon de 3
enregistrements**, et son test plan (« visual inspection of 5-10 revised FR virtues », « verify remark_fr
examples are concrete ») est **resté à coches vides** dans le body.

Retouches ultérieures documentées sur les mêmes cellules : normalisation apostrophes (#1020 08/08),
renommages de familles (#993/#1000/#1002 — mes 58 cellules structurelles), liens morts (#1442, #1470).

## 3. Ce que ce dossier n'établit pas

- **La qualité** des textes HEAD vs baseline : l'instrument mesure l'**ampleur** (similarité, longueurs),
  jamais la valeur (clarté gagnée vs substance perdue). La claim « no meaning regression » de #367 repose
  sur 3 échantillons, non re-vérifiée ici.
- **Aucune référence imprimée** : Virtues n'a **aucune archive imprimée** (garde du dispatch). La baseline
  2024 (`62b561e75`, 22/04/2024, pré-agentique) est la **seule** référence disponible. Un écart vs
  baseline **n'est pas** un écart vs imprimé — l'arbitrage porte entre **deux états rédigés**, pas entre
  un imprimé et une dégradation.
- Les autres langues (le grain est fr) et l'attribution carte-par-carte des 131 remarks (la sonde -S
  couvre un échantillon de 8 ; #367 touche les 223 lignes, la paternité est établie en vagues, pas cellule
  par cellule).
- Le rendu : les 131 cartes HEAD ont passé la QA visuelle v2.0.0 (ai-01, 80/80) — la question n'est pas
  « est-ce rendu correctement » mais « quel texte l'owner veut ».

## 4. Mise en arbitrage (proposition)

La question owner n'est pas « 312 cellules une à une » mais **un choix de régime pour les remarks**,
les deux autres colonnes suivant :

1. **Échantillon stratifié d'abord** : les 45 remarks de similarité <0.3 (réécriture quasi totale) sont
   le signal le plus fort — un arbitrage œil sur ~10 d'entre eux (baseline vs HEAD côte à côte) décide
   du régime.
2. Régimes possibles : (A) conserver HEAD partout (#367 assumé) · (B) retour baseline partout (annulation
   de #367 sur le fr) · (C) mixte carte par carte — le plus coûteux, à réserver aux cas où l'œil tranche
   différemment selon la carte.
3. `title_fr` (53) et `description_fr` (128) : réécritures majoritairement modérées (0.6+) — même
   arbitrage, vraisemblablement moins litigieux.

## Reproductibilité

```bash
python docs/corpus/virtues-baseline-diff.py   # depuis la racine du dépôt, lecture seule
```

---
*po-2023 (worker lane) — signale, ne déclare pas PASS · verdict/arbitrage : ai-01 & jsboige*
