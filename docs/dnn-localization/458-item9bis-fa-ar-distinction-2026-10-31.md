# #458 grain module-level — distinguer le persan de l'arabe dans les gardes fa-IR (4 tests)

**Date :** 2026-10-31 · **Lane :** po-2023 · **Cultures :** `fa-IR` contrôlée, `ar-SA` comme comparateur
**Périmètre :** `DnnModuleLevelControlsFaResxGuardTests`, `DnnModuleLevelAdminFaResxGuardTests` + un instrument neuf, `DnnPersianScriptMarkers` — **0 `.resx` touché**
**Origine :** revue NanoClaw de #2097, item 6 du pool po-2023 (c.6543033289). Question posée : « une copie intégrale de l'ar-SA passerait-elle les gardes fa à 100 % ? »

---

## 1. Le trou

Le contrôle d'écriture des lots `fa-IR` borne l'écriture au bloc **U+0600–U+06FF**. Ce bloc est **commun à l'arabe et au persan** : l'alphabet persan y ajoute quatre lettres (پ چ ژ گ) et deux formes (ک ی), mais tout texte arabe y tombe aussi.

Conséquence mesurée : **une copie intégrale de l'ar-SA passe le contrôle d'écriture à 100 %**. La garde dit « cette valeur porte de l'écriture persane » alors que le pack pourrait être entièrement arabe — aucune garde ne distinguait les deux langues.

Pourquoi ce n'est pas une règle par valeur : des valeurs persanes légitimes ne portent aucune lettre proprement persane (nombres, noms propres, libellés d'une seule lettre partagée). Mesuré sur les lots livrés : **17 valeurs sur 133** (`controls`) et **49 sur 330** (`admin`) n'en portent aucune, et ce sont des traductions correctes. Une règle par valeur rougirait sur du contenu sain.

## 2. Mesure du discriminant (31/10, avant écriture)

Instrument : `python -I` + `ElementTree`, sur les fichiers appariés des deux cultures (mêmes 11 et mêmes 28 fichiers).

| Lot | Culture | Valeurs | Portent une lettre persane (پ چ ژ گ ک ی) | Portent ي / ك / ى |
|---|---|---:|---:|---:|
| `controls` | `fa-IR` | 133 | **116 (87 %)** | **0** |
| `controls` | `ar-SA` | 133 | 0 (0 %) | **72** |
| `admin` | `fa-IR` | 330 | **281 (85 %)** | **0** |
| `admin` | `ar-SA` | 330 | 0 (0 %) | **218** |

Les deux critères séparent les langues sans recouvrement. Le plancher retenu, **80 %**, laisse une marge de 5 à 7 points au-dessus du lot livré et **37 points sous** la borne d'une copie arabe.

## 3. Ce qui est gardé

Deux faits par lot, ajoutés aux classes fa existantes :

| Fait | Portée | Critère |
|---|---|---|
| `TheLotIsPersianNotAnArabicCopy` | le lot `fa-IR` | ≥ 80 % des valeurs portent une lettre propre au persan **et** aucune valeur ne porte ي / ك / ى |
| `TheInstrumentRejectsTheArabicLot` | le lot `ar-SA` des mêmes fichiers | le **même** instrument doit échouer aux **deux** critères |

Le second n'est pas décoratif : il s'exécute à chaque passage de la suite et prouve que l'instrument **sait** rougir. Un plancher abaissé par mégarde (ou un motif cassé qui ne matcherait plus rien) resterait sinon vert sur une copie arabe sans que personne ne le voie.

Le profil est calculé une seule fois, dans `DnnPersianScriptMarkers.Profile`, partagé par les deux lots — aucune règle recopiée.

## 4. Contrôle inverse réel (mesuré, pas affirmé)

Les deux faits verts ne prouvent rien seuls : un plancher jamais atteint resterait vert pour de mauvaises raisons. Le contrôle a donc été fait sur les données, pas seulement sur l'instrument.

**Mutation** : les **11 fichiers** `controls/*.fa-IR.resx` reçoivent, clé par clé, le **contenu XML brut** de leur jumeau `ar-SA` (37 + 6 + 4 + 5 + 6 + 10 + 16 + 30 + 9 + 27 + 7 = 157 valeurs remplacées). Les 11 fichiers restent XML valides (contrôle `ElementTree` : 0 fichier malformé).

**Mesuré** :

```
Expected ((double)persian / total) to be greater than or equal to 0.8 because
controls/fa-IR : 0/131 valeurs portent une lettre propre au persan — sous le
plancher de 80 %, le pack n'est plus distinguable d'une traduction arabe,
but found 0.0 (difference of -0.8).
```

**Rouge, et rouge pour la bonne raison** : 0/131, exactement le profil d'une copie arabe. Les 17 valeurs sans lettre persane du lot d'origine sont devenues 131 : le lot entier a basculé.

**Restauration** : `cp` depuis sauvegarde, **11 fichiers, 0 écart sha256**, `git status` propre sur `DNNPlatform/controls`.

⚠️ **Piège payé** : la première mutation a substitué la valeur **parsée** par `ElementTree` au lieu du XML brut. Une valeur arabe contenant un `<br />` a produit `<br>` littéral dans le fichier cible → `XmlException: The 'br' start tag on line 148 … does not match the end tag of 'value'`, et **5 rouges qui ne mesuraient rien**. La substitution doit porter sur le contenu **brut** de l'élément `<value>`, entités comprises.

## 5. Suite

**3 183 = 3 178 réussis, 0 échec, 5 ignorés** (1 m 23 s), arbre restauré.

Base `9038690d` : 3 179. Le delta de **+4** est exactement les quatre faits de ce lot (2 `controls` + 2 `admin`) — aucune autre variation. L'écart d'ignorés (6 en base, 5 ici) est la variance d'environnement local déjà consignée aux lots #2097 et #2098.

## 6. Ce que ce lot ne fait pas

- **Aucun `.resx` n'est modifié** : c'est un organe de contrôle, pas une livraison.
- Il ne **corrige** aucune traduction : les 17 et 49 valeurs sans lettre persane sont correctes et restent telles quelles.
- Il ne ferme **pas** la traduction partielle : une valeur à moitié persane et à moitié anglaise porte une lettre persane, donc elle passe. Le commentaire de classe du lot `admin` fa-IR disait l'inverse — corrigé ici, avec les lots zh du même défaut (PR #2098).
- Il ne couvre pas `App_GlobalResources` ni les skins : hors grain module-level.

---

*lane po-2023 · mesures du 31/10/2026, instrument indépendant des gardes (Python `-I` + `ElementTree`)*
