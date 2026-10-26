# Pool #458 v23 — grain ㉝ (g34) : registre des exemples pt/es + cohérence par carte (mesure, 0 écriture)

**Date** : 2026-10-03 · **Branche** : `docs/458-g34-registre-exemples` (depuis master, non empilée) · **Machine** : myia-po-2024
**Dispatch** : c.5968567990 item 4 — « registre pt/es exemples (0 écriture) + cohérence par carte (registre def vs exemple, ex. "tú" déf / "usted" exemple) — l'arbitrage espagnol (jamais imprimé) attend cette mesure ; portugais imprimé → garde sauf décision owner ».
**Instrument** : `docs/corpus/register-consistency-instrument.py` — étendu (`--field example`, `--coherence`), puis **réparé sur mesure** (§2). **Écritures CSV : 0** (`git status --porcelain Cards/` vide avant et après).

---

## 1. Extension de l'instrument

- `--field desc|example` (défaut `desc`) : le classifieur s'applique à `example_pt` / `example_es` — mêmes passes descendantes (explicites → morpho 2pl → zéro-sujet initial → GRIS), même colonne EVIDENCE, même sortie (table PK + distribution).
- `--coherence` : jointure **par carte** desc × example (`pair_bucket`) — le signal demandé par le dispatch. Un verdict **DIFFERENT n'est émis que si les deux côtés portent une preuve mécanique** ; toute rangée GRIS d'un côté est comptée unilatérale (DEF_ONLY / EX_ONLY), jamais qualifiée. `base_reg()` normalise VOS_MORPHO→VOS et VOSOTROS_MORPHO→VOSOTROS avant comparaison (la morphologie verbale et le pronom disent la même personne).
- Self-test étendu : 10 paires plantées (dont la paire du dispatch USTED×TU→DIFFERENT, et les normalisations morpho) + 4 pièges de recalibrage (§2).

## 2. Repair mesuré : une stoplist calibrée sur desc n'est pas étalonnée pour example

Le premier passage `--field example` a produit des VOS_MORPHO dont l'evidence était un **nom/adjectif/adverbe** et des ZERO_SUJET déclenchés par des mots initiaux qui ne sont ni gérondif ni infinitif. Cause : les stoplists de #1723 étaient calibrées **sur desc seulement**. Sweep de tous les matchs des colonnes example :

| Faux positif | nature | rangées dont le verdict a bougé |
|---|---|---|
| pt : `animais`, `automóveis`, `excepcionais`, `favoráveis`, `jamais`, `mortais`, `reais`, `responsáveis`, `variáveis` (9 mots) | noms/adj/adv en -ais/-eis/-veis | 8 DIFFERENT fantômes (105 `jamais`, 108/595/698 `animais`, 177 `responsáveis`, 625 `reais`, 977 `mortais`, 1345 `variáveis`) + 1 EX_ONLY (176 `excepcionais`) + 1 SAME (621 `automóveis`) ; `favoráveis` n'a déplacé aucun verdict (couvert par un marqueur explicite) mais est stoplisté — la couverture n'est pas une garantie |
| `Quando`/`Cuando` initial | conjonction en -ndo | pt 357, 800, 869 ; es 800, 869 |
| `Ayer` initial | adverbe en -er | es 2 |

Réparation : `PT_2PL_STOP` += les 9 mots (commentaire daté) ; `INITIAL_STOP = {quando, cuando, ayer}` testé dans `classify()` ; les 4 cas plantés au self-test comme pièges.

Effet net sur la cohérence (avant → après) — les deltas équilibrent à 0 par langue :

| seau | pt | es |
|---|---|---|
| DIFFERENT | **31 → 20** (−11) | **10 → 8** (−2) |
| SAME | 13 → 12 | 10 → 10 |
| EX_ONLY | 5 → 4 | 9 → 8 |
| DEF_ONLY | 110 → 122 | 90 → 92 |
| BOTH_GRIS | 9 → 10 | 49 → 50 |
| VIDE | 7 | 7 |

**Preuve de non-régression desc** : re-passe `--field desc` après réparation — les **350 lignes PK sont byte-identiques** à la sortie mergée #1723 (seul l'en-tête gagne « -- champ desc »). Le seul des 9 mots présent en desc (`desfavoráveis`, desc_pt 1092) ne peut pas déplacer sa rangée : son « Você » explicite gagne la passe 1.

⇒ Même leçon que l'écran NA (⑳) : **un instrument étalonné sur un champ ne transporte pas son étalonnage sur un autre champ** — la recalibration se mesure, elle ne se présume pas.

## 3. Registre des exemples — distributions (post-réparation)

| registre | pt | es |
|---|---:|---:|
| TU | — | 11 |
| VOCE | 23 | — |
| VOCES | 10 | — |
| VOS | 1 (808) | — |
| USTED | — | 3 (176, 299, 989) |
| USTEDES | — | 2 (357, 1282) |
| VOSOTROS + morpho | — | 4 + 3 |
| ZERO_SUJET | 2 (134, 834) | 3 (134, 681, 834) |
| GRIS | 132 | 142 |
| VIDE (têtes de section) | 7 | 7 |
| **total** | **175** | **175** |

Lecture structurelle (pas un verdict) : les définitions s'adressent au lecteur (#1723 : ustedes/vosotros dominants en es, você/vocês en pt) ; les **exemples citent** — dialogues, répliques, incidents rapportés — d'où 75 % (pt) à 81 % (es) de GRIS sans marqueur d'interlocuteur, et un glissement vers **tú** (11 rangées es) quasi absent des définitions. Le signal de cohérence vit dans les rangées porteuses des deux côtés.

## 4. Cohérence par carte — PT (imprimé → garde sauf décision owner)

### DIFFERENT — 20 cartes, preuve mécanique des deux côtés

**Personne ↔ personne (12)** — la liste d'arbitrage pt :

| PK | desc | exemple |
|---|---|---|
| 179 | VOCE 'Você' | VOCES 'Vocês' |
| 219 | VOCES 'Vocês' | VOCE 'você' |
| 362 | VOS_MORPHO 'Enquadrais' | VOCE 'você' |
| 420 | VOCE 'Você' | VOCES 'vocês' |
| 673 | VOCES 'vocês' | VOCE 'Você' |
| 768 | VOCE 'Você' | VOCES 'Vocês' |
| 845 | VOCE 'Você' | VOCES 'Vocês' |
| 878 | VOCES 'Vocês' | VOCE 'Você' |
| 974 | VOCES 'Vocês' | VOCE 'Você' |
| 1282 | VOS_MORPHO 'Afirmais' | VOCES 'vocês' |
| 1314 | VOCES 'Vocês' | VOCE 'você' |
| 1365 | VOCES 'Vocês' | VOCE 'você' |

Le désaccord dominant est **singulier ↔ pluriel** (você ↔ vocês, 10 rangées) ; les 2 rangées vós (362, 1282) superposent la distance archaïque ↔ moderne déjà mesurée en #1723 côté defs — l'exemple suit l'usage courant, la définition ne l'a pas suivi.

**Zéro-sujet ↔ personne (8)** — comptées, jamais jugées (une définition à l'infinitif est le style normal des fiches ; l'exemple, lui, adresse quelqu'un) : 51, 313, 343, 598, 900, 1388 (desc ZERO_SUJET → exemple VOCE) · 134, 834 (desc VOCE → exemple ZERO_SUJET 'Gerenciar').

Autres seaux : SAME 12 (79, 299, 323, 432, 814, 839, 876, 943, 973, 989, 1015, 1355) · DEF_ONLY 122 · EX_ONLY 4 (182, 658, 681, 808) · BOTH_GRIS 10 · VIDE 7. **Total 175 ✓**

C-note : 808 porte le seul **VOS des exemples** (« vos » dans la réplique) — cohérent avec les defs vós de #1723, mais isolé côté exemples.

## 5. Cohérence par carte — ES (jamais imprimé — la matière de l'arbitrage)

### DIFFERENT — 8 cartes, preuve mécanique des deux côtés

**Personne ↔ personne (6)** :

| PK | desc | exemple |
|---|---|---|
| 70 | USTEDES 'Ustedes' | TU 'tu' |
| 176 | USTEDES 'Ustedes' | USTED 'usted' |
| 219 | VOSOTROS 'vuestra' | TU 'tú' |
| 299 | USTEDES 'Ustedes' | USTED 'usted' |
| 357 | VOSOTROS_MORPHO 'Creáis' | USTEDES 'ustedes' |
| 1388 | VOSOTROS_MORPHO 'Desacreditáis' | TU 'tu' |

**Zéro-sujet ↔ personne (2)** : 134 (desc 'Abordáis' → exemple 'Dirigir') · 681 (desc TU 'Tu' → exemple 'Sabiendo').

Autres seaux : SAME 10 (51, 313, 432, 834, 876, 888, 900, 973, 994, 1282) · DEF_ONLY 92 · EX_ONLY 8 (79, 177, 179, 182, 362, 989, 1365, 1373) · BOTH_GRIS 50 · VIDE 7. **Total 175 ✓**

LeSAME es 834 est instructif en miroir du DIFFERENT pt 834 : même carte, l'exemple ouvre sur l'infinitif (« Gerenciar »/« Gestionar ») des deux côtés, mais **la définition pt adresse le lecteur (« Você ») quand l'es reste impersonnelle (« Razonar »)** — la divergence pt/es de registre naît dans la définition, pas dans l'exemple.

## 6. La paire nommée par le dispatch (« tú » déf / « usted » exemple)

**Mesuré : ce cas exact n'existe pas dans le deck.** Les définitions es en TU (681, 800, 869) ont pour exemples : 681 zéro-sujet ('Sabiendo' → DIFFERENT personne↔zéro), 800 et 869 des répliques sans marqueur (→ DEF_ONLY après stop de « Cuando »). Ce qui existe est la **direction inverse** et ses cousines — la définition adresse un groupe (ustedes/vosotros), l'exemple cite un interlocuteur singulier : 70, 176, 219, 299, 357, 1388 (§5).

L'hétérogénéité **intra**-exemple reste mesurée au ㉛ (432 : « estáis » puis l'impératif 3pl « levanten » dans la même réplique) — elle n'est pas re-mesurée ici.

## 7. Ce que le grain n'établit pas

- **Aucun jugement de langue** : DIFFERENT dit « deux personnes mécaniquement prouvées sur la même carte », pas « fautif » — une définition en ustedes suivie d'un dialogue en tú peut être un choix éditorial.
- **GRIS n'est pas « sans personne »** : la morphologie 2sg n'est pas armée (elle matcherait cosas/personas/veces par centaines) — la lecture tranche, la cellule est rendue dans la sortie à cet effet.
- **Ces tableaux n'autorisent aucune écriture** : l'arbitrage du registre es (un seul registre ? tú réservé aux exemples ?) est owner — l'es n'est pas imprimé, le pt se gèle sauf décision explicite. Les 12 + 6 rangées personne↔personne sont la matière, pas la décision.

## 8. Reproductibilité

```bash
python docs/corpus/register-consistency-instrument.py --self-test          # PASS (18 cas + 8 pièges + 10 paires)
python docs/corpus/register-consistency-instrument.py --field example --out <scratch>/ex.txt
python docs/corpus/register-consistency-instrument.py --coherence       --out <scratch>/coh.txt
python docs/corpus/register-consistency-instrument.py --field desc     --out <scratch>/desc.txt
# desc.txt : 350 lignes PK byte-identiques à la sortie #1723 (non-régression prouvée)
git status --porcelain Cards/                                             # vide
```

Deck = 175 rangées `carte` non vide ; VIDE = les 7 têtes de section structurelles (1, 175, 594, 696, 798, 887, 1280) ; hors-deck (65, 476, 1300) hors périmètre. Sorties complètes archivées dans le scratchpad session (registre-ex-v2, registre-coh-v2, registre-desc-v3).

---

*po-2024*
