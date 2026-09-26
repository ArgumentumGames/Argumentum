# #1499 grain 5 — registre `es` des définitions : **mélange stable**, puis 3 incohérences écrites

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **Dispatch :** pool v14 grain 5 ([#458 c.5838808923](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5838808923)) puis pool v16 grain 3 ([c.5842223755](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5842223755)) · **Base :** `4b29966d` (origin/master, mesure) → `46d9b61c` (écriture)

> ⚠️ **Deux couches datées dans ce fichier.** §1-§2 = la **mesure** (grain 5, ⛔ 0 écriture). §3 = les anomalies, **traitées** par G9-es-W (grain 3, **3 cellules écrites**, §3bis). Les deux couches sont vraies à leurs dates : la mesure n'a rien écrit, l'écriture est postérieure à la mesure et porte sur les **incohérences**, pas sur le mélange.

## 0. Verdict

**Mélange stable** — aucune forme d'adresse n'est majoritaire sans raison sur les 175 `desc_es` du deck : 65 % du corpus est impersonnel, et parmi les 61 définitions qui adressent le lecteur, aucune forme ne domine (tú ≈ 56 % des adressantes seulement, les 4 formes cohabitent dans presque toutes les familles). **Aucune harmonisation n'est écrite** — le mélange est laissé tel quel, c'est le verdict.

> ⚠️ **Décision rendue (G9-es-W, [#1569](https://github.com/ArgumentumGames/Argumentum/pull/1569)) — ce n'est plus une question ouverte.** Le dispatch du grain 3 (pool v16, [#458 c.5842223755](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5842223755)) a tranché : les **incohérences internes** à une même phrase ne relèvent pas d'un arbitrage owner mais d'un **choix linguistique ordinaire** (délégation du 09/09 : décider, puis rapporter avec veto ouvert). **3 cellules** ont été écrites (1297, 1301, 492) ; les **3 autres** flavées vosotros sont **gardées** parce qu'elles sont cohérentes. Détail en §3. Le verdict « mélange stable » porte donc sur **tout le reste** du corpus — les 58 autres adressantes et les 114 impersonnelles, ⛔ inchangées.

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
3. **Le coût dépasse le bénéfice d'un grain worker** : choisir « tout usted » contredirait la tendance tú de 34 cartes ; choisir « tout tú » contredirait la direction de politesse déjà retenue pour les exemples. **L'harmonisation globale n'est pas un choix linguistique ordinaire** au sens de la délégation du 09/09 — et elle est désormais **décidé (ai-01, revue [#1566](https://github.com/ArgumentumGames/Argumentum/pull/1566), délégation 09/09) : pas d'harmonisation**, donc **elle n'a pas été écrite** : G9-es-W a écrit les **incohérences internes** (§3), pas l'harmonisation. ⛔ Ne pas relire ce point comme « l'owner doit trancher le mélange » : le mélange est **retenu par décision rendue** (veto owner ouvert) ; seule une décision owner explicite le rouvrirait.

   ⚠️ **Distinguer les deux objets**, c'est ce qui a décidé le périmètre de G9-es-W : une phrase qui se contredit *elle-même* (verbe au pluriel + possessif au singulier) est un **défaut**, pas un registre — elle se corrige par choix linguistique ordinaire ; deux cartes qui adressent différemment sont un **mélange**, et un mélange n'est pas un défaut tant que personne n'a dit quelle forme est la cible.

## 3. Anomalies nommées — **traitées** (G9-es-W, 3 écritures)

- **6 cartes flavées vosotros** : **3 écrites, 3 gardées** — la ligne de partage est la **cohérence interne de la phrase**, pas le simple fait d'être au pluriel :

| PK | état mesuré | geste |
|---|---|---|
| **1297** | « **Repetís** tu punto de vista… » — verbe 2ᵉ pers. **pluriel** + possessif **singulier** « tu » | **écrite** → « **Repites** tu punto de vista… » |
| **1301** | « **Repetís** tu punto de vista… » — même hybride | **écrite** → « **Repites** tu punto de vista… » |
| **492** | « **Os presentáis** como una víctima… » — vosotros complet, alors que 492 est la carte dont `text_*` vient d'être aligné (#1565) et que le deck adresse partout ailleurs un lecteur **singulier** | **écrite** → « **Te presentas** como una víctima… » |
| 887 | « **Os liberáis** de las reglas tácitas… » — verbe **et** réflexif au pluriel | **gardée** : vosotros cohérent, rien à corriger |
| 1092 | « **Os concentráis** en los elementos… » — idem | **gardée** |
| 1120 | « **Razonáis** … si no **sois** perfectos, **os consideráis** … » — idem | **gardée** |

  ⛔ **887 / 1092 / 1120 ne sont pas touchées** (décision du dispatch grain 3) et sont **épinglées par la garde** de l'outil d'écriture, qui échoue si l'une bouge.

- **Éclatement usted (5) vs ustedes (20)** sur des sources fr identiques en forme (« Vous… ») — signature probable de passes de traduction distinctes. **Non écrit** : les deux formes sont correctes et aucune n'est majoritaire sans raison ⇒ relève de l'harmonisation globale — **décidé (ai-01, revue [#1566](https://github.com/ArgumentumGames/Argumentum/pull/1566), délégation 09/09) : pas d'harmonisation** (§2.3), ⛔ pas d'un grain worker.

### 3bis. Ce qui a été écrit, et le contrôle

- **Instrument :** `tools/1499-g9esw-write-byte-exact.py` + `tools/1499-g9esw-worklist.json`. Écriture **byte-exact** (un seul analyseur, re-sérialisation selon le quotage d'origine) : `-4` octets, **3 cellules**, 104 champs sur chacune des 1409 lignes, BOM et CRLF inchangés.
- **⛔ Seul le mot d'adresse change** : chaque valeur nouvelle partage le **suffixe épinglé** de l'ancienne — la garde refuse toute réécriture qui déborderait du mot d'adresse.
- **Contrôle inverse** (un « 0 » n'est une absence que si l'instrument pouvait voir un « 1 ») : `« Repetís tu »` **2 → 0**, `« Os presentáis »` **1 → 0**, et les trois formes **gardées** toujours présentes **1×** chacune.
- **Contrôle différentiel par l'instrument de mesure** (rejoué sur le fichier de sauvegarde, arbre scratch) : `adresse es par forme` passe de `{tú: 32, usted: 5, ustedes: 20, vosotros: 4}` à `{tú: 33, usted: 5, ustedes: 20, vosotros: 3}` — ✓ 492 bascule de vosotros vers tú, ✓ il ne reste que les 3 cohérentes, ✓ aucune autre forme ne bouge.
- **Post-état typographique #994** sur les 5 CSV × 8 langues : **P=0 A=0 C=0**.
- **Mutation sur littéraux + témoin** : valeur laissée à l'ancienne, suffixe épinglé cassé, valeur tronquée, apostrophe droite, attente de protection fausse — **5/5 vues**, témoin vert.

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
