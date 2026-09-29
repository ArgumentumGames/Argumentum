# Pool v22 — grain ⑲ : tri des bandeaux Virtues ≠ titre du rang (mesure 0-écriture)

**Base** : `7d4ac363` (29/09) — CSV Virtues byte-identique sur `6374fee4` (diff vide, vérifié). **Méthode** : l'invariant établi par ⑦ (`458-v20-g7-bandeaux-virtues`, deck), étendu aux **223 rangées × 8 langues** : la colonne *k* (`family_<lang>` / `subfamily_<lang>` / `subsubfamily_<lang>`) porte le titre du rang dont le chemin est le **préfixe de longueur k** du nœud (k = 1 à 3), **rang propre compris** — une rangée de profondeur 2 porte son propre titre en `subfamily`, une rangée de profondeur 3 en `subsubfamily`. **Nature** : mesure, aucune écriture.

## ⚠️ Correction du 29/09 (review ai-01, c.5888521481) — ce que la première version avait de faux

La première mouture de ce document publiait **14 divergences** sur la foi de trois erreurs d'instrument, toutes signalées par la review :

1. **Comparateur « parent direct »** : il fabriquait 448 « divergences » structurelles aux profondeurs 5-7, que le doc écartait en inventant une convention « le bandeau est plafonné à subsubfamily ». Avec le comparateur par rang de ⑦, **les profondeurs 5-7 ne produisent aucun faux positif** : `subsubfamily` porte simplement le titre du rang de longueur 3. La section « plafonné » est retirée — le plafonnement n'existe pas.
2. **fr exclu** sur l'affirmation non mesurée « le bandeau fr suit toujours » : le fr diverge à la mesure — **21 cellules** (18 hors deck + 3 dans le deck).
3. **Colonnes du rang propre non comparées** : une rangée d2/d3 porte aussi son propre bandeau, qui peut diverger de son propre titre (cas mesuré : pks 64, 80, 84, 135, 159…).

## Résultat — 136 divergences / 14 rangs (8 langues, fr compris)

| | Cellules |
|---|---:|
| **Total** | **136** |
| hors deck (12 rangs) | 117 |
| deck non imprimées (4 rangs, porte de cascade fermée — annexe de ⑦) | 19 |

Par langue : fr 21 · en 4 · ru 48 · pt 7 · es 4 · ar 16 · fa 17 · zh 19.

| Rang (titre fr) | Où | Cellules | Langues |
|---|---|---:|---|
| `4.3.3` « Raisonnement concluant » | hors deck | 35 | ru 35 (« Убедительное рассуждение » contre le titre « Доказательное рассуждение ») |
| `5.1` « Définitions claires » | hors deck 16 + deck 8 | 24 | 8 langues (« Acceptable/admissible definitions » vs « Clear definitions ») |
| `3.2` « Interprétation rigoureuse des données » | hors deck | 20 | fr ru ar fa zh ×4 |
| `7.1.3` « Ouverture au dialogue » | hors deck | 16 | fr ar zh fa ×4 |
| `6.2` « Clarté des enjeux » | hors deck 6 + deck 3 | 9 | ru pt zh |
| `3.2.3` « Support fini » | hors deck | 7 | 7 langues (sauf pt) |
| `1.1.3` « Raisonner sans biais » | hors deck | 5 | fr ru pt zh fa |
| `4.1` « Causalités bien identifiées » · `4.2` « Énoncés rigoureux » | deck | 4 + 4 | fr ar zh fa |
| `6.3.3` · `7.1.1` | hors deck | 4 + 4 | fr + 3 |
| `7.3.3` · `5.2.1` · `7.2.1` | hors deck | 2 + 1 + 1 | fr |

Le groupe `4.3.3` ru était **invisible au comparateur parent-direct** — c'est le contre-exemple qui a cassé la première version. Exemples fr (la langue que la première version excluait) :

- `3.2` : bandeau « Interprétation **adéquate** des données » vs titre « Interprétation **rigoureuse** des données » (pks 64, 68, 69, 70) ;
- `5.1` : bandeau « Définitions **recevables** » vs titre « Définitions **claires** » (pks 135, 139, 140) ;
- `5.2.1` : « Comparaison adequate » vs « Comparaison **adéquate** » (pk 143) — **accent seul** ;
- `7.3.3` : « Courtoisie dans la **divergence** » vs titre « Courtoisie dans le **désaccord** » (pks 218, 222).

Les 19 cellules du deck non imprimées : rangs propres `4.1` (pk 80, 4 langues), `4.2` (pk 84, 4), `5.1` (pk 135, 8), `6.2` (pk 159, ru/pt/zh) — leur porte de cascade est fermée (pas de `subsubfamily` rempli ⇒ bandeau non rendu), mais un changement de gabarit les rendrait visibles.

## Nature des écarts — les 3 familles de ⑦, aucune nouveauté

1. **Alias périmé** (majoritaire) : le renommage d'un rang n'a pas été propagé dans les bandeaux — `5.1` « recevables→claires » dans les 8 langues, `4.3.3` ru « Убедительное→Доказательное ».
2. **Paraphrase descriptive vs libellé court** : le bandeau décrit (« Identification précise des causalités », « Construction rigoureuse des énoncés »), le titre nomme (« Causalités bien identifiées », « Énoncés rigoureux »).
3. **Mot distinct / accent** : « Disponibilité au dialogue » vs « Ouverture au dialogue », « adequate » sans accent.

**0 vide** : aucune cellule attendue-pleine n'est vide (mesuré — le périmètre « remplie et différente » n'a pas d'angle mort).

## Comparaison avec ⑱ (Fallacies)

| | ⑱ Fallacies | ⑲ Virtues |
|---|---:|---:|
| Cellules divergentes | 422 | **136** |
| Rangs/clusters | 29 | **14** |
| Vides | 94 (dont 2.3.2 = 64) | 0 |
| Exceptions à arbitrer | 2 (ru GoT, en Causalation) | **0** |

## Correction — arbitrée par ai-01 (c.5888518774)

La règle ⑱ s'applique **telle quelle, sans exception** : chaque bandeau est aligné sur le titre de son rang dans sa langue, **y compris en fr** et **y compris les 19 cellules non imprimées du deck**. Le titre fait foi ; en particulier `4.3.3` ru converge vers « Доказательное рассуждение », le titre que le deck imprime déjà.

⑲w s'écrit **après ⑮w** dans la série (les deux touchent le fichier Virtues sans cellule commune ; ⑮w passe en premier, ⑲w se re-mesure sur sa tête).

## Garde (au grain d'écriture)

L'invariant ⑦ sur **les 223 rangées et les 8 langues** : toute cellule remplie de `family/subfamily/subsubfamily_<lang>` égale le `title_<lang>` du rang préfixe — avec **témoin inverse** qui injecte un alias dans une cellule du fr (sans témoin, la garde peut être vacue, cf. #994). Miroir de `FallaciesDeckBandInvariantGuardTests`, étendue hors deck.

## Instrument

- Script `g19_measure.py` (scratchpad) : lecteur CSV dict, comparaison rang-préfixe (k = 1-3) sur les 3 colonnes × 8 langues × 223 rangées ; divergence = cellule **remplie** ≠ titre attendu du rang.
- **Validation croisée** : le total (136), la partition deck/hors deck (19/117) et le tableau par rang reproduisent **exactement** la mesure ai-01 refaite sur `a6e29f0d` (c.5888518774) — 14/14 rangs, y compris les 8 langues de `5.1` et l'absence de pt sur `3.2.3`.
