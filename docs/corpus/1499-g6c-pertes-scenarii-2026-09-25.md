# #1499 G6-C — pertes dans le fr Scenarii substitué : liste courte, 0 écriture

**Auteur** : po-2024 (worker) · **Date** : 2026-09-25 · **Base** : `origin/master` `9e79e88d`
**Grain** : pool #458 v13, ordre ai-01 — *« réduire les 345 MODIFIE fr (G6, #1556) à une liste courte de pertes ; couche pré-agentique Scenarii dérivée de l'historique git via la clé E1+E2 (⛔ jamais `path` seul) ; deltas 2022→pré-agentique en section séparée ; cellules `paire_limite` marquées, non résolues hors périmètre ; ⛔ 0 écriture »*
**Instrument** : [`tools/1499-g6c-pertes-scenarii.py`](../../tools/1499-g6c-pertes-scenarii.py) — importe la clé validée de G5, **0 écriture** · détail nominatif : [`tools/1499-g6c-pertes.json`](../../tools/1499-g6c-pertes.json)

---

## 1. La couche, dérivée et déclarée

- **B_SC = `b08b62fa` (2023-10-27)** — dernier commit du CSV Scenarii avant l'ère agentique, **mesuré** : le fichier ne bouge plus d'octobre 2023 à mai 2026 (`git log`), l'ancre Fallacies `62b561e75` (2025-07-27) voit le même état, et le premier commit postérieur sur ce CSV est `6fc30ab8` (2026-05-23). Tout ce qui suit est la couche agentique.
- **C = `origin/master` `9e79e88d`**.
- Jointure **E1+E2 de G5** (titre normalisé, puis `path` + contrôle de contenu), jamais `path` seul. La clé re-dérive ici sur 91 × 167 : plancher de bruit q99 = **0,146**, aucune paire E1 sous le plancher.

## 2. Les chiffres — la couche agentique

| | cellules |
|---|---:|
| Paires appariées (E1+E2) | **82** / 91 |
| **MODIFIE** (les deux remplis, texte différent) | **378** |
| **PERDU** (B rempli → C vide) | **0** |
| GAGNE | 0 |

Par champ : `contexte` 75 · `suggestion` 73 · `titre` 69 · `enjeu` 68 · `sous-catégorie` 62 · `baratineur` 15 · `piocheur` 15 · `catégorie` 1. Similarité token (casefold + ponctuation, calibration G6) : `[0 ; 0,2[` **72** · `[0,2 ; 0,4[` 27 · `[0,4 ; 0,6[` 35 · `[0,6 ; 0,8[` 66 · `[0,8 ; 1]` **178**.

⚠️ **Relation au « 345 » de G6** : populations distinctes, nommées comme telles. G6 comptait sur **69 paires archive-2022** (8 champs fr) ; ici c'est **82 paires pré-agentiques** (91 rangées en 2023 contre 77 dans l'archive). Le 345 est le sous-ensemble archive-scope ; les 378 sont la surface de la couche. Les deux chiffres sont vrais, aucun ne remplace l'autre.

## 3. La lecture — 124 cellules, aucune perte

Lu : les **99 cellules < 0,4** (72 + 27, plus tout effondrement de longueur ≤ 0,5), l'**échantillon de 25 médianes** (0,4 ≤ sim < 0,8, graine 20260925), et le contrôle des quasi-identiques (sim ≥ 0,8 : **0 effondrement de longueur** — rien ne disparaît en se dégradant légèrement).

Les 378 modifications se rangent **entièrement** dans des gestes documentés — traces **mesurées** (`git log -S`, un `head -1` par libellé) :

| Geste | Exemples | Trace mesurée |
|---|---|---|
| **Taxonomie** : sous-catégorie ramenée aux 7 catégories | `relation avec les collègues /employeurs` → `relations au travail` (×5) · `du livre` → `religions` (×6) · `contes et littérature` → `contes` (×7) · `enfant` → `famille et enfance` (×5) · `la fabrique de la loi` → `gouvernance` (×4) | `74557ea6` (2026-03-14, *restore golden master*) |
| **Passe de clarté FR** — le gros du volume | 162 enregistrements sur 167 réécrits : `Johnny Johnny` → `Johnny au Panthéon`, `tu t'es vu quand t'as bu` → `Lendemain difficile`, `cascade primaire` → `Retrait négocié`, `wing man` → `Ailier`, reformulations de `contexte`/`enjeu`/`suggestion` | `31092887` (#368, 2026-05-28 — 437 `UpdateRecord`, 162 insertions = 162 suppressions) |
| **Pseudonymisation** des cartes porteuses d'IP | `Star Wars`/`Dark Vador`/`Luke` → `Le Fils du tyran`/`Le Seigneur de l'Empire`/`Le fils rebelle` | `e59ddf26` (#1189/#1210, 5 cartes × 8 langues) |
| **Typos et noms propres** | `Juda` → `Judas`, `Capitaine Hadock` → `Capitaine Haddock`, `désarmer` → `désamorcer` | `7c40caa6` (#1305), `bab289c0` |
| **Apostrophes et clarté** | `le coup d'Etat` → `Le coup d'État`, `Il doit le convaincre` → `Il doit la convaincre` (genre rectifié, 7.2.2) | `ab4af0e1` (#1423), #1375 |
| **Enrichissements** | `Le baratineur est végétarien.` → `…végétarien convaincu qui ne supporte pas la production de foie gras.` (×3,07) · `…pour éviter la mutinerie` (×1,51) | campagnes #994 |

⚠️ La **passe de clarté FR** (`31092887`, gpt-5.5) est de loin la plus grosse contributrice : c'est là que des pertes auraient pu se loger — la lecture n'en a trouvé **aucune** (reformulations, jamais d'amputation : 0 effondrement de longueur).

## 4. La liste courte — **0 « revenir »**

**Aucune cellule ne justifie un retour.** Pas une perte : **0 PERDU**, et les 124 cellules lues sont toutes une normalisation, une correction ou un enrichissement — règle C : garder.

## 5. Périmètres qualifiés (exigés par le dispatch)

- **2 paires E2 LIMITE** — path **3.1.3** (`stealthing` → `Retrait non consenti`) et **5.1.3** (`obélisque` → `La potion de trop`) : leurs cellules sont marquées `paire_limite` dans le JSON (8 dans la bande lue), appariement **récusable à la lecture** — même qualification que G6, jamais fondue dans un total.
- **9 cartes non appariées**, nommées : les **8 de G5/G6** (Louis XVI, Truman, Don Juan, mariage ou pas, 2+1, rétrogradation canapé, 5G, miaw) — dont 5 ont leur `path` réoccupé par une carte sans rapport (sim 0,019–0,070, le contrôle les récuse) — **plus** `t'as pas mille balles?` (7.5.2), présente en B_SC seule. Périmètre **séparé, décision humaine** : ce grain ne tranche pas leur sort.

## 6. Section séparée — deltas 2022 → pré-agentique (gestes d'époque)

Jointure archive-2022 (77) → B_SC (91) : plancher q99 = **0,952**, **dégénéré** — les deux corpus sont quasi identiques, si bien que le q99 tombe *sur* les vraies paires et révoque 6 appariements par titre dont la similarité va de 0,833 à 0,926 (même carte, retouchée). Conséquence **conservatrice**, consignée : le chiffre principal exclut ces 6 paires.

| | cellules |
|---|---:|
| Paires validées | 71 → **30 MODIFIE** |
| Paires révoquées (plancher dégénéré), comptées à part | 6 → **14 MODIFIE**, nommées (`le loup et l'agneau`, `l'arrêt de complaisance`, `obélisque`, `star wars`, `l'emploi fictif`, `un ami qui vous veut du bien`) |

Les gestes d'époque sont **petits** (typographie, retouches) — rien qui ressemble au volume de la couche agentique. Aucune perte non plus de ce côté.

## 7. Ce que ce dossier n'établit pas

- **Lecture partielle** : 124 cellules sur 378 (32,8 %), concentrées sur la pire bande ; les 178 quasi-identiques sont couverts par le contrôle d'effondrement (0), les 101 médianes par échantillon (25, 0 perte) — extrapolation, pas exhaustivité.
- Le sort des 9 cartes non appariées n'est **pas** mesuré ici (périmètre séparé).
- Les cellules des 2 paires LIMITE sont marquées, pas arbitrées.
- ⛔ **0 écriture corpus** — le grain mesure, nomme et qualifie.

## 8. Exécutable

```
python tools/1499-g6c-pertes-scenarii.py --self-test   # rc=0 — natures, trim, similarite
python tools/1499-g6c-pertes-scenarii.py --json-out tools/1499-g6c-pertes.json
```

Référentiel des verdicts : `modifie_classees_bc` dans le JSON (sim/ratio + ancien/nouveau par cellule) ; section 2022 dans `section_2022_preagentic`.

Refs #1499
