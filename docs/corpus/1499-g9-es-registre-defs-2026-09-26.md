# #1499 grain 5 — registre `es` des définitions : **mélange stable** (mesure, ⛔ 0 écriture)

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **Dispatch :** pool v14 grain 5 ([#458 c.5838808923](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5838808923)) · **Base :** `4b29966d` (origin/master)

## 0. Verdict

**Mélange stable** — aucune forme d'adresse n'est majoritaire sans raison sur les 175 `desc_es` du deck : 65 % du corpus est impersonnel, et parmi les 61 définitions qui adressent le lecteur, aucune forme ne domine (tú ≈ 56 % des adressantes seulement, les 4 formes cohabitent dans presque toutes les familles). Aucune harmonisation n'est proposée comme écriture ; deux anomalies nommées ci-dessous restent disponibles **si l'owner veut un grain**, ⛔ rien n'est écrit.

## 1. Mesure (instrument : `tools/1499-g9es-registre-defs.py`, self-test OK)

| Classe `desc_es` | n | % |
|---|---:|---:|
| Impersonnel / 3ᵉ pers. (**I**) | **114** | 65,1 % |
| Adresse lecteur, total | **61** | 34,9 % |

Ventile des 61 adressantes :

| Forme | n | cartes |
|---|---:|---|
| **tú** (singulier) | 32 (dont 2 hybrides, cf. §3) | 34, 165, 175, 177, 247, 304, 313, 319, 337, 340, 376, 644, 658, 681, 696, 750, 777, 787, 800, 802, 833, 846, 847, 869, 876, 943, 1004, 1011, 1297, 1301, 1355, 1357 |
| **ustedes** (pluriel de politesse) | 20 | 70, 176, 184, 185, 299, 356, 358, 420, 511, 633, 636, 713, 799, 889, 908, 1282, 1287, 1312, 1360, 1361 |
| **vosotros** | 4 | 492, 887, 1092, 1120 |
| **usted** (singulier) | 5 | 43, 719, 735, 740, 942 |

**Aucune définition ne mélange tú et usted** (M = 0). Croisement : `desc_fr` est **« vous » sur 175/175** — l'éclatement `es` vient de l'ambiguïté du « vous » français (politesse singulier *vs* pluriel), chaque passe de traduction ayant tranché différemment.

Par famille, les deux formes principales cohabitent dans les 7 : p. ex. Influence tú 9 / ustedes 8 / vosotros 1 ; Obstruction ustedes 5 / tú 4 ; Abus de langage tú 7 / ustedes 1. **Aucune famille n'est homogène.**

## 2. Pourquoi « mélange stable » et pas « harmonisation »

1. **Pas de majorité nette** : tú plafonne à ~56 % des adressantes — soit 35 % du deck réel (61/175 adressent) ; harmoniser exigerait de réécrire 25-57 définitions selon la cible.
2. **Aucune raison d'écarter une forme** : tú et usted(es) sont tous deux corrects ; la décision Q-12 3a (vouvoiement pour `example_*`, #1546) est **example-scoped** — aucune décision documentée ne couvre `desc_*` (grep des gardes : 0 hit).
3. **Le coût dépasse le bénéfice d'un grain worker** : choisir « tout usted » contredirait la tendance tú de 34 cartes ; choisir « tout tú » contredirait la direction de politesse déjà retenue pour les exemples. C'est un arbitrage éditorial owner, pas un choix linguistique ordinaire au sens de la délégation du 09/09.

## 3. Anomalies nommées (candidates SI l'owner veut un grain — non dispatchées)

- **6 cartes flavées vosotros** alors que le deck adresse partout ailleurs un lecteur **singulier** : 492, 887, 1092, 1120 (détection instrumentale : « Os presentáis », « Os liberáis », « Os concentráis », « os consideráis… sois ») **et 1297, 1301** (hybrides comptés en « tú » par l'instrument : verbe 2ᵉ pers. pluriel « Repetís » + possessif singulier « tu punto » dans la même phrase). ⚠️ 492 est la carte dont `text_*` vient d'être alignée (PR #1565) — sa `desc_es` reste en vosotros.
- **Éclatement usted (5) vs ustedes (20)** sur des sources fr identiques en forme (« Vous… ») — signature probable de passes de traduction distinctes.

## 4. Limites

- **I = plafond d'impersonnel, pas une borne exacte** : « le/su » + verbe 3ᵉ pers. est indissociable de l'impersonnel en espagnol ; seuls les « usted(es) » explicites comptent comme V. Une part indéterminée des 114 I peut adresser poliment le lecteur sans le dire.
- Mesure limitée à `desc_es` (périmètre du dispatch) ; `example_es` est couvert par la décision Q-12 3a et sa garde, hors périmètre.
- ⛔ Aucun appel payant, aucune écriture, aucune régénération.

## 5. Exécutable

```bash
python tools/1499-g9es-registre-defs.py               # tables + croisement
python tools/1499-g9es-registre-defs.py --dump        # les 61 adressantes
python tools/1499-g9es-registre-defs.py --self-test   # 6 cas classe + 4 formes
```
