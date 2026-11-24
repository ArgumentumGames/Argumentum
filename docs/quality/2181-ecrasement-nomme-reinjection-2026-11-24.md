# Écrasement nommé dans l'outil de réinjection EAV (2026-11-24)

**Grain** : P24-51 (#2181), né de la mesure de P24-32 (PR #2370), lane po-2024.
**Objet** : rendre atteignable l'écriture des 3 cellules de P24-32 sur la base servie —
W2-09 l'exige (« les 3 cellules de P24-32 servies »), l'outil de `master` la refuse.

## Ce qui est mesuré, et par qui

| Fait | Nature | Source |
|---|---|---|
| Une cellule dont le registre et la base sont **pleins et différents** est classée `CONFLICT`, et `--apply` refuse l'écriture (« REFUS d'appliquer: conflits ou cibles manquantes non arbitres ») | **mesuré** | `0543ab69:tools/458-eav-reinject.py` (`compute_actions`, refus d'apply) ; rejeu de bout en bout ci-dessous |
| Sur cible synthétique, les 3 cellules de P24-32 y tombent : `CONFLICT: 3`, verdict `ECHEC`, code 1 | **mesuré** | mode à blanc de `main()` sur le registre de la PR #2370, cible portant le registre de `master` |
| Les mêmes 3 cellules, **nommées une à une**, passent en `FORCE` : `0 INSERT + 0 UPDATE + 3 ECRASEMENT(S) NOMME(S)`, plan cohérent, code 0, le reste du plan identique | **mesuré** | même mode à blanc |
| Sur la **base servie**, les 3 cellules portent encore l'ancienne valeur du registre | **supposé** | c'est l'hypothèse que W2-11 (web2, mode à blanc) mesure ; cette lane ne lit pas la base |
| W2-09, tel qu'écrit dans #458, n'est pas atteignable avec l'outil de `master` | **dérivé** des lignes 1 et 4 | — |

Chiffres du mode à blanc (registre = tête de la PR #2370, cible = registre de `master`,
673 lignes, carte d'apps identité) : `SKIP_SAME 1796`, `SKIP_PRESERVED_2181 7`,
`FR_SAME 517`, `FR_EXCLUDED 1`, et `CONFLICT 3` → `FORCE 3` après les trois `--force`.
Les trois écrasements : `Installation [es]` 922 → 920 caractères, `Installation [ar]`
757 → 755, `Content [zh]` 1 170 → 1 170 (deux `)` remplacés par `）` : même nombre de
caractères, `+2` octets chacun en UTF-8 — les `+4` octets du registre).

## Le choix

Une cellule en conflit ne s'écrit **que nommée, entière** :

```
--force AE1EDEFA-6F1B-4593-8230-97FA1EDF4F78:Installation:es
```

- **GUID complet** (un préfixe de 8 est refusé : ce n'est pas un identifiant) ;
- **la cellule désignée seule** quitte le conflit — tout autre conflit refuse l'apply ;
- un nom qui ne désigne **aucune** cellule du plan est un **refus** (faute de frappe) ;
  un nom dont la cellule n'est **pas** en conflit est **rapporté**, sans écriture forcée ;
- la valeur d'avant est sauvegardée **avant** la transaction
  (`reinject-force-prior-*.json`), comme les bases fr ;
- l'écriture, **dans la transaction**, épingle la rangée que le dry-run a lue (ValueId
  MAX de la dimension), la tient sous verrou et exige qu'elle porte **encore exactement**
  cette valeur — comparaison **ordinale** en PowerShell, parce que l'égalité SQL suit le
  classement de la colonne et ignore la casse. Une rangée, sinon ROLLBACK ;
- la relecture in-tx relit l'écriture comme toutes les autres.

**Écarté** : comparer à la version précédente du registre (compare-and-swap contre
`HEAD~1`) — plus fort en principe, mais l'outil tourne là où la base est servie, et
l'ancien CSV devrait y être exporté exprès. À reprendre si les écrasements nommés se
multiplient. **Écarté** aussi : écrire les 3 cellules en SQL à la main — plus de
sauvegarde, plus de relecture, plus de garde de préservation.

## Vérifié

- **Gardes** : `python -I tools/test_458-eav-reinject-guards.py` — **38 cas** après le rebase sur
  `master` (`e39df385`, 27/11) : 16 avant le grain, 29 sur sa base ;
  OK. Quatre propriétés de plus (F1 classification, F2 noms, F3 writer, F4 sauvegarde).
- **Batterie de mutations indépendante** : `python -I tools/2181-reinject-mutations.py` —
  **12/12** mutations vues par le cas nommé d'avance, témoin non muté **29/29** (chaque
  mutation s'applique à une copie du trio outil/export/test, suite entière rejouée).
- **Script writer** : diff contre `master` = **2 lignes remplacées + 31 insérées**, rien
  d'autre ; **0 erreur de parse** en Windows PowerShell 5.1 (contrôle local, hors CI).
- **Répétition hors base** : `python -I tools/2181-reinject-rehearsal.py` sur la vraie
  paire de registres (avant = `master`, après = tête de la PR #2370) — registres 673/673
  lignes, cible synthétique 2 324 rangées de valeur, 10 apps ; oracle **3 conflits**,
  **3/3** retrouvés par l'outil, tous nommés passent en `FORCE`, `--apply` nommé leur
  transmet exactement eux, le fichier d'avant dit avant → après aux octets ;
  `VERDICT: coherent avec l'oracle`, code 0. Les dix contrôles des trois passes sont
  dans la sortie de l'instrument.
- **Bout en bout au banc** (avant que l'instrument ne soit committé) : nom inconnu →
  refus ; nom hors conflit → rapporté et les 3 conflits restent ; `--apply` sans nom →
  refus, writer jamais appelé ; `--force` mal formé → refus avant toute lecture.

## Non vérifié

- **Aucune exécution contre une base**, ni servie ni de test : la branche in-tx est lue
  et ses formes sont gardées, pas éprouvées. La concurrence optimiste (valeur changée
  entre le dry-run et l'apply) n'a pas été jouée en vrai.
- L'état de la base servie (cf. ligne « supposé ») : W2-11 le mesure par le mode à blanc,
  et **doit y voir `CONFLICT` sur les 3 cellules** — c'est l'état attendu, pas un écart.

## Reproduire

```bash
python -I tools/test_458-eav-reinject-guards.py          # 38 cas
python -I tools/2181-reinject-mutations.py               # 12 mutations, témoin 29/29
mkdir -p .tmp/rehearsal
git show master:docs/dnn-localization/eav-content.csv > .tmp/rehearsal/avant.csv
git show origin/fix/458-p2432-registre-typo:docs/dnn-localization/eav-content.csv \
  > .tmp/rehearsal/apres.csv
python -I tools/2181-reinject-rehearsal.py --before .tmp/rehearsal/avant.csv \
  --after .tmp/rehearsal/apres.csv --work-dir .tmp/rehearsal/work
```

Après le merge de la PR #2370, le registre `après` se lit dans `master` et la répétition
doit rendre **0 conflit** (les deux registres coïncident) : c'est le signe que la cible
de la répétition a changé, pas une régression de l'outil.

Le runbook porte la règle d'usage : `1066-cutover-runbook-2026-11-04.md`, phase 2,
étape 10 (« Cellule en conflit »).