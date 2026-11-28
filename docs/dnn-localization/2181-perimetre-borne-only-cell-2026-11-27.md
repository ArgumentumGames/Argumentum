# `--only-cell` — le mode à blanc borné de la ré-injection EAV (#2181, W2-09/W2-14)

**Date :** 2026-11-27 · **Lane :** po-2024 · **Pool :** #458, initiative après W2-14
**Base :** `origin/master` `3162ae5f` (rebased après la fusion de #2373, `--force`) · **Registre :**
`docs/dnn-localization/eav-content.csv` (pivot, pas la base)

> **Mise à jour 2026-11-28 (P24-54).** Ce constat écrit, en §2 et en §4, qu'« aucune option ne lève
> l'exclusion » de la base fr du bloc. Ce n'est plus vrai depuis le levier nommé
> `--fr-force GUID:CHAMP`, qui la lève **pour le seul nom de la base fr du bloc** :
> [`2181-levier-nomme-base-fr-bloc-2026-11-28.md`](2181-levier-nomme-base-fr-bloc-2026-11-28.md).
> Par défaut (sans le levier), tout ce qui suit tient. Les chiffres du §5 sont ceux de la date du
> constat : la suite compte 93 cas et la batterie 59 mutations depuis P24-54.

---

## 1. Ce qui bloquait

W2-14 (première passe de W2-09) demande une sortie à blanc du lot « les 7 langues absentes du bloc
« L'argumentation » » — 7 ajouts, aucun conflit **dans le périmètre**. Or `tools/458-eav-reinject.py`
lisait son plan d'écriture **entier** : `blocked = conflicts or missing or fr_missing` (l. 747 avant ce
grain). Sur la base servie, trois cellules d'une **autre** entité sont en `CONFLICT` (voir §3) ; elles
suffisaient à faire rendre `VERDICT: ECHEC` et à refuser `--apply`, quel que soit le lot demandé.
Aucun état du dépôt à la mesure du matin (ni `master` `e39df385`, ni la tête de #2373) n'offrait de
mode borné.

## 2. Ce que le mode borne, et ce qu'il ne borne pas

`--only-cell GUID:CHAMP:LANGUE`, cumulable ; `fr` désigne la cellule de base (dim-0). Jamais un
raccourci de type « registre tronqué » : la carte d'apps, la lecture de la cible (gardes G1) et la
sonde de préservation restent mesurées sur le **registre complet**, et seule la consommation des deux
plans par `compute_actions` / `compute_fr_actions` est bornée.

| Mesure | Registre utilisé |
| --- | --- |
| carte d'apps (G2 : marge du vainqueur, G3 : injectivité) | complet |
| lecture de la cible, paires multi-entités (G1) | complet |
| préservation `#2181` / `#2182` | complet |
| plan d'écriture (`INSERT` / `UPDATE` / `CONFLICT` / `SKIP`) | **borné** |
| comparaison des bases fr (`FR_DRIFT` / `FR_EXCLUDED` / `FR_DB_LOCAL`) | **borné** |

Un registre tronqué aurait fait perdre sa marge à la carte (G2 refuse une égalité) et masqué les
paires G1 hors lot : d'où le choix de borner la seule écriture.

**Trois refus — dont deux avant toute connexion** — un nom mal formé et un nom qui ne désigne
aucune cellule du plan (« périmètre borné à rien » se lirait « plan cohérent ») sont refusés
**avant** `read_connection_string` : la suite épingle qu'aucune requête n'est partie. Le troisième
— une cellule nommée qu'un réglage écarte de l'écriture — se calcule sur les actions, donc
**après** la lecture de la cible (7 requêtes envoyées ; lecture seule, aucune écriture). La
portée de l'affirmation est donc « deux refus avant la connexion », pas « trois ».

- `SKIP_PRESERVED_2181` (les 7 traductions du bloc sans `--include-preserved-2181`) et
  `FR_DB_LOCAL` (référence `file:NNN`, jamais écrite) **bloquent le dry-run** : le levier existe
  (le drapeau), ou la cellule n'est de toute façon pas inscriptible ;
- `FR_EXCLUDED` (la base fr du bloc `#2181`, préservée par l'arbitrage du 20/11, ou un `--fr-exclude`)
  est **montrée à blanc et refusée à l'apply**. W2-09 demande un mode à blanc « borné aux 11 cellules
  (dont le français) » : refuser le dry-run rendrait cette passe impossible, alors qu'aucune option ne
  lève cette exclusion. Le verdict ne dit jamais « prêt pour `--apply` » quand une cellule nommée
  reste exclue.

Ces deux listes ne valent que pour un **périmètre nommé** : hors `--only-cell`, elles décrivent des
cellules que personne n'a demandées et ne bloquent ni le dry-run ni l'apply (contre-revue po-2023
c.6093174887, §3.2).

Quand le seul blocage est une cellule nommée qu'un réglage écarte, le verdict et le refus d'apply
**nomment le réglage** — ils ne parlent pas de « conflits ou cibles manquantes », qu'il n'y en a pas
(contre-revue po-2023 c.6093174887, §4.4 ; corrigé dans cette révision).

## 3. Les trois `CONFLICT` hors lot, re-mesurées

Rapporté par web2 (c.7215188925) : 3 `CONFLICT` sur app 60 `Game Rule` `AE1EDEFA…`. **Mesuré** ici
(hors ligne, `csv.DictReader`, délimiteur virgule, `utf-8-sig`, sur le registre du dépôt) — ce sont
exactement les trois cellules de P24-32 :

| Champ | Langue | Longueur côté registre |
| --- | --- | ---: |
| `Installation` | `es` | 920 |
| `Installation` | `ar` | 755 |
| `Content` | `zh` | 1 170 |

Piège d'instrument, payé une fois pendant ce grain : `Installation` porte **aussi** un `zh`
(longueur 266), qui n'est **pas** la troisième cellule ; la troisième est `Content` `zh` (1 170). Les
deux longueurs cohabitent dans le même champ voisin, et une sonde qui cherche « 1170 dans
`Installation` » conclut à tort qu'il manque une cellule.

**Mesuré** (même lecture, fonctions de l'outil chargées hors ligne) : registre 673 lignes → plan
1 806 cellules traduisibles, 518 bases fr ; les 11 noms de W2-09 résolvent tous (`scope_plan` rend
10 traduisibles + 1 base fr, 0 inconnue). La première passe (7 langues) ne porte donc aucun conflit ;
la seconde (le français + les 3 cellules) en porte trois, et c'est bien le dry-run qui doit le dire.

## 4. Ce qui reste une décision

Aucune option n'écrit la base fr du bloc `#2181` : l'exclusion est un défaut de `compute_fr_actions`,
pas une consigne. Une passe bornée qui **nomme** le français est donc mesurable mais non applicable
tant que l'arbitrage du 20/11 (option c : la base fr du bloc reste celle de la prod) n'est pas rouvert.
Le mode borné ne tranche pas cette question : il la rend visible dans le dry-run et refuse l'apply.

## 5. Reproduire, hors ligne

```bash
python tools/test_458-eav-reinject-guards.py     # 57 cas, aucune connexion
python tools/2181-reinject-mutations.py          # témoin 57/57, puis 34 mutations, chacune vue par son cas
```

Le cas `test_la_passe_de_onze_cellules_de_w2_09_se_joue_a_blanc` rejoue la passe de W2-09 sur une
cible synthétique : 10 `INSERT` cohérents, 1 cellule fr montrée exclue, `--apply` **refusé** pour les
11, et **lancé** pour les 10 — jusqu'au refus de sauvegarde (« exige `--backup-dir` », rc 1), pas
jusqu'à une écriture.

**Ce que l'apply borne transmet au writer** (contre-revue po-2023 c.6093174887, §3.1 à §3.3) — quatre
cas et cinq mutations ajoutés dans cette révision :

- `test_l_apply_borne_ne_transmet_au_writer_que_les_cellules_nommees` — `run_writer` est remplacé par
  un **capteur** (il n'écrit rien) : 7 traductions nommées → 7 `INSERT` et **aucune** base fr, même
  quand une base fr hors lot dérive de la cible. Contrôle inverse
  (`test_controle_inverse_nommer_la_base_et_la_traduction_transmet_huit_ecritures`) : nommer en plus
  B Summary `en` et sa base fr rend **8** écritures dimensionnées, dont 1 `FR_UPDATE`. Avant ces cas,
  aucun `--apply` de la suite n'atteignait `run_writer` — tous s'arrêtaient à un refus, et rien
  n'affirmait ce qui part ;
- `test_sans_only_cell_les_reglages_hors_perimetre_ne_bloquent_pas` — sans `--only-cell`, les 7
  `SKIP_PRESERVED_2181` et le 1 `FR_EXCLUDED` ne bloquent ni le dry-run ni l'apply ;
- `test_force_hors_perimetre_est_refuse` — `--force` sur une cellule que `--only-cell` a exclue est un
  refus de périmètre (« demander l'écrasement d'une cellule qu'on vient de retirer »).

Les mutations N14-N34 de la seconde commande retirent chacune **une** garde du mode borné sur une
copie de l'outil ; le cas qui doit les voir est nommé d'avance. Non vérifié ici : aucune
exécution contre la base servie — le dry-run réel reste à web2, sur le lot de son choix.

## 6. Rebase sur la fusion de `--force` (#2373)

La branche a été rebasée sur `3162ae5f` après la fusion de #2373 (`--force GUID:CHAMP:LANGUE`,
l'écrasement nommé). Les deux options touchaient les mêmes lignes de l'outil, de la suite de gardes et du
workflow ; elles sont gardées **toutes les deux**, et trois points ont changé pour `--only-cell` :

- **Un seul validateur de nom.** `parse_only_cell` utilisait `uuid.UUID` ; il lit maintenant le même
  `FORCE_GUID_RE` que `parse_force` (GUID complet en majuscules ou minuscules). Conséquence mesurée :
  un GUID de 32 caractères **sans tirets**, accepté par `uuid.UUID` et donc par la première version de
  `--only-cell`, est refusé par les deux options. Le mutant N14 retire ce validateur de `--only-cell`
  seul : `test_parse_only_cell_formes` rougit.
- **Un cas que la fusion de #2373 a laissé passer (M1, relevé par ai-01).** Le writer `--force` refuse
  une lecture sans rangée (`aucune rangee a cette dimension`) ; aucun cas ne fixait ce refus, et le
  retirer laissait la suite verte. Le refus est maintenant épinglé par `test_une_seule_branche_et_conforme`
  et rejoué par la batterie (N13) ; `test_mutation_sans_refus_de_rangee_rougit` garde le lien.
- **Une batterie rejouable.** Les quinze mutations de la première version étaient jouées à la session,
  non commises. Elles sont rabattues dans `tools/2181-reinject-mutations.py` (N14-N34), aux côtés des
  treize de `--force` (N1-N13) : 34 mutations, même témoin, même sortie. Les cinq dernières
  (N30-N34) sont celles que la contre-revue po-2023 c.6093174887 nommait « non vues » : le plan
  d'écriture et les bases fr réécrites reconstruits depuis le plan complet, les deux listes de
  cellules écartées rendues bloquantes hors périmètre, et le refus du `--force` hors périmètre.
