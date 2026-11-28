# Le levier nommé de la base fr du bloc #2181 (`--fr-force`) — P24-54

**Date :** 2026-11-28 · **Lane :** po-2024 · **Pool :** #458, grain P24-54 (file de travail du lot)
**Base :** `origin/master` `21d64b7d` · **Outil :** `tools/458-eav-reinject.py`
**Registre :** `docs/dnn-localization/eav-content.csv` (pivot, pas la base)

---

## 1. Ce qui bloquait

La prod sert, pour la base fr du bloc « L'argumentation » (app 33, `Content`.`Text`, GUID
`8A8636A4-069A-4692-A139-4773B161931B`), un corps **plus long** que celui du registre : l'arbitrage
du 20/11 (option c) a decide que la base de la prod reste celle de la prod, et `tools/458-eav-reinject.py`
exclut donc cette cellule par defaut (`FR_EXCLUDED`). Aucune option ne levait cette exclusion — ni
l'outil, ni la sortie de l'outil, qui affirmait le contraire : « aucune option ne leve l'exclusion ».

Le grain ajoute **un seul levier, pour un seul nom** : `--fr-force GUID:CHAMP`, qui ne nomme que la
base fr du bloc. Il ne leve pas l'exclusion des sept traductions du bloc (elles restent sous
`--include-preserved-2181`), il ne touche ni aux autres entités, ni au rapport de préservation.

## 2. Ce que le levier fait, et ce qu'il refuse

- `--fr-force GUID:CHAMP` : GUID **complet** (le même validateur `FORCE_GUID_RE` que `--force` et
  `--only-cell` ; 32 hex sans tirets sont refusés), GUID indifferent à la casse, **champ comparé
  à l'identique** (`Text` ; `text` est refusé).
- Quatre refus, dont **trois avant toute lecture** (du registre et de la cible) : nom mal formé ;
  nom qui n'est pas celui du bloc (« n'accepte qu'un nom ») ; la même base nommée aussi par
  `--fr-exclude`. Le quatrième — la base **hors du périmètre** `--only-cell` — se calcule après la
  lecture du registre, avant la connexion. Pour les quatre, la suite épingle qu'aucune requête
  n'est partie.
- Levé sur cette base, l'exclusion devient une réécriture **nommée** : la cellule passe de
  `FR_EXCLUDED` à `FR_FORCE`, et le dry-run l'annonce ligne par ligne (GUID complet, champ, longueurs
  avant/après).
- La valeur d'avant est sauvegardée **avant** l'appel au writer, dans
  `reinject-fr-force-prior-<stamp>.json`.

## 3. Le writer et la garde

**Branche `FR_FORCE` du script writer** — même discipline que `--force` : la rangée est épinglée
(`TOP 1 … ORDER BY v.ValueId DESC`, dimension 0), lue **sous verrou** (`UPDLOCK, HOLDLOCK`) ; la
valeur en base est comparée **ordinalement** à celle du dry-run, sinon refus et `ROLLBACK` ; l'écriture
est `UPDATE … WHERE ValueId=@vid` et doit toucher **exactement une** rangée, sinon refus ; la branche
se termine par `continue` — le chemin `FR_UPDATE` n'est pas pris pour une base nommée.

**Garde « #2181 identique »** — elle n'accepte la réécriture que si **exactement un** `FR_FORCE`
porte le GUID littéral du bloc et le champ `Text` (comparés dans le script, pas dans la charge utile),
que la rangée réécrite est celle du dry-run, et que la base vaut le registre. Sans ce nom, le refus
d'avant (« valeur de base MODIFIEE ») s'applique comme avant.

**Post-contrat après le COMMIT** — la base est sur la rangée du dry-run, elle vaut le registre, elle
diffère de la valeur d'avant, et les sept News #2182 sont intactes. Sinon : verdict
« ⚠️ PRESERVATION A QUALIFIER », code de sortie 1.

## 4. Mesures

| Figure | Nature | Comment la re-mesurer |
|---|---|---|
| Base fr du bloc au registre : **6 687 caractères** | **mesuré** | `read_registry` + `build_fr_plan` de l'outil sur `docs/dnn-localization/eav-content.csv` (hors ligne) |
| Empreinte de cette valeur : `ae3f216e504ec7c923d8dfa9a750971c9a9645669f0b98944e023d234a5d30d4` | **mesuré** | `sha256` des octets **UTF-8** de la valeur (la même convention que le chiffre rapporté) — en UTF-16-LE : `1f44148c8896c50e9567b8c7fa1bd127d58fe6c8f86026cc20950da0ac143081` |
| Base fr du bloc servie : **6 709 caractères**, `9bd05ea5ae5d…`, ValueId 27 942 | **rapporté** (web2, #2181 c.7218258887) | non re-mesuré par cette lane (ni base, ni préprod) |
| Écart : **22 caractères** | **dérivé** (6 709 rapporté − 6 687 mesuré) | idem |
| Suite de gardes : **93 cas, tous verts**, ~2 s | **mesuré** | `python tools/test_458-eav-reinject-guards.py` |
| Batterie de mutations : **témoin 93/93, 59 mutants sur 59 vus** par le cas nommé d'avance | **mesuré** | `python tools/2181-reinject-mutations.py` (hors CI) |
| Script writer : 18 222 caractères / 264 lignes (master : 14 573 / 217) | **mesuré** | `len(WRITER_PS)` — sous la limite de ligne de commande Windows (32 767) ; un cas de la suite l'épingle |

La valeur du registre contient 41 LF nus et aucun CR : ce sont des fins de ligne **de contenu**
(le corps est du HTML `<p>`), pas du transport. La comparaison du writer est donc une égalité de
chaîne, pas une égalité de lignes. L'empreinte de l'outil lui-même (celle de sa sonde de préservation)
est en UTF-16-LE, l'encodage natif d'une colonne `nvarchar` ; celle du tableau est en UTF-8, la
convention du chiffre rapporté — les deux se calculent, aucune ne vaut « la » bonne.

## 5. « Sans le levier, le comportement de master est inchangé » — mesuré

Douze scénarios du même banc synthétique (périmètre vide, drapeau du bloc, les 7 traductions, la base
fr nommée, la passe de 11 cellules de W2-09, une base qui dérive hors bloc, un apply refusé, un apply
avec capteur) rejoués avec l'outil de `master` puis avec celui de la branche, **sans `--fr-force`** :

- **8 scénarios sur 12 sont identiques** — même code de retour, même sortie, même rapport JSON, et
  même contenu transmis au writer (contrôle par un capteur) ;
- les 4 autres ne diffèrent que par **deux phrases** : celles qui affirmaient chez master qu'« aucune
  option ne lève l'exclusion » — devenues fausses avec le levier ;
- le rapport JSON ne porte **aucune** section du levier tant qu'il n'a pas servi (`fr_named` n'existe
  que s'il y a une réécriture nommée), et le fichier de résultat `reinject-result-*.json` ne porte sa
  clé (`base_fr_2181_reecrite_nommee`) que si le plan nomme la base : sans lui, l'un et l'autre sont
  ceux de master, octet pour octet ;
- `run_writer` lui-même, joué sans le levier avec un writer factice (`subprocess.run` remplacé) :
  même code de retour, même sortie, même charge utile, mêmes fichiers dans le dossier de sauvegarde et
  même contenu — le fichier de résultat y compris.

La comparaison rejoue le même banc avec les deux versions de l'outil chargées depuis leur chemin
(`importlib`), et normalise le seul élément qui diffère par construction : le dossier temporaire.
**Le banc de comparaison est un script de mesure local, non commité** : ces 12 scénarios ne se
rejouent pas depuis le dépôt seul. Ce qui est commité et rejouable hors ligne, ce sont la suite
(93 cas) et la batterie (59 mutations).

## 6. Ce qui n'a **pas** été vérifié

- **La branche `FR_FORCE` du writer n'a jamais été exécutée contre une base** (ni base, ni
  transaction, ni `apply` réel) : ses lignes sont lues, et les lignes de *décision* de la garde
  « #2181 identique » sont jouées sous PowerShell sur une charge utile factice. L'exécution réelle
  reste à la lane qui sert la base.
- La présence de `pwsh` sur l'image `ubuntu-latest` n'est pas présumée : le job CI installe un shell
  s'il n'y en a pas, et le cas « garde jouée sous powershell » **rougit** en CI au lieu de se sauter
  (un contrat aveugle ne doit pas être vert).
- Le chiffre servi (6 709 caractères, empreinte, ValueId) est **rapporté**, pas re-mesuré ici.
- Le dry-run sur la base servie n'a pas été rejoué par cette lane.
- La comparaison « sans le levier » du §5 a été jouée **une fois**, sur l'état final des fichiers,
  par un banc local non commité : elle n'est pas gardée par la CI, seulement rapportée ici.

## 7. Reproduire, hors ligne

```bash
python tools/test_458-eav-reinject-guards.py     # 93 cas, aucune connexion
python tools/2181-reinject-mutations.py          # témoin 93/93, puis 59 mutations, chacune vue par son cas
```

Le levier se joue sur le banc synthétique de la suite :

```bash
python tools/458-eav-reinject.py --config <web.config cible> --fr-force 8A8636A4-069A-4692-A139-4773B161931B:Text
python tools/458-eav-reinject.py --config <web.config cible> --fr-force 8A8636A4-069A-4692-A139-4773B161931B:Text --apply --backup-dir <hors-depot>
```

Le premier passage est à blanc (aucune écriture) : il annonce la réécriture nommée, sa ligne, et les
longueurs avant/après. Le second exige `--backup-dir` hors du dépôt, sauvegarde la valeur d'avant, et
n'écrit qu'après les refus du writer.
