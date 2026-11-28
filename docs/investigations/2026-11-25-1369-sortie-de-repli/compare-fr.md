## Contrôles de l'instrument

| contrôle | mesuré | verdict |
|---|---|---|
| Rules_01 (avant) contre elle-même | IDENTIQUE | OK |
| Rules_07 contre Rules_08 (avant) | DIFFÉRENT | OK |
| Rules_08 contre une copie à ±1 niveau sur 1 pixel | DIFFÉRENT (1 px, écart max 1) | OK |

## 1. Récolte — ce que CardPen a rendu (`fr/Harvest`)

| récolte:côté:image | taille | pixels | IDAT égaux | octets égaux | écart |
|---|---|---|---|---|---|
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_01 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_02 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_03 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_04 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_05 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Faces:RulesPP_06 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules-Print&Play_harvest_fr.json:Backs:card_001 | 894×1495 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_01 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_02 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_03 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_04 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_05 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_06 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_07 | 897×1497 | DIFFÉRENT 86026 px (6.406 %) | non | non | bbox=(71, 390, 825, 1425) max=255 |
| Rules_harvest_fr.json:Faces:Rules_08 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_09 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_10 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_11 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_12 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_13 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_14 | 897×1497 | IDENTIQUE | oui | oui | - |
| Rules_harvest_fr.json:Faces:Rules_15 | 897×1497 | IDENTIQUE | oui | oui | - |

## 2. Disque — PNG de `Images/`

| fichier | taille | pixels | IDAT égaux | octets égaux |
|---|---|---|---|---|
| Images/density-0/Rules/rules_01_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_02_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_03_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_04_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_05_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_06_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_07_face.png | 826×1417 | DIFFÉRENT 78529 px (6.709 %) | non | non |
| Images/density-0/Rules/rules_08_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_09_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_10_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_11_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_12_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_13_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_14_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules/rules_15_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/card_001.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_01_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_02_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_03_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_04_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_05_face.png | 826×1417 | IDENTIQUE | oui | non |
| Images/density-0/Rules-Print&Play/rulespp_06_face.png | 826×1417 | IDENTIQUE | oui | non |

## 3. Où est l'écart (faces, récolte)

| image | identique | pixels différents | écart max | faible (< 32 niveaux) | fort (>= 32 niveaux) | lignes changées |
|---|---|---|---|---|---|---|
| RulesPP_01 | oui | 0 | - | - | - | - |
| RulesPP_02 | oui | 0 | - | - | - | - |
| RulesPP_03 | oui | 0 | - | - | - | - |
| RulesPP_04 | oui | 0 | - | - | - | - |
| RulesPP_05 | oui | 0 | - | - | - | - |
| RulesPP_06 | oui | 0 | - | - | - | - |
| Rules_01 | oui | 0 | - | - | - | - |
| Rules_02 | oui | 0 | - | - | - | - |
| Rules_03 | oui | 0 | - | - | - | - |
| Rules_04 | oui | 0 | - | - | - | - |
| Rules_05 | oui | 0 | - | - | - | - |
| Rules_06 | oui | 0 | - | - | - | - |
| Rules_07 | non | 86026 px (6.41 %) | 255 | 1079 px, écart 31 | 84947 px, lignes 1306–1424 | lignes 390–1424 sur 1497 |
| Rules_08 | oui | 0 | - | - | - | - |
| Rules_09 | oui | 0 | - | - | - | - |
| Rules_10 | oui | 0 | - | - | - | - |
| Rules_11 | oui | 0 | - | - | - | - |
| Rules_12 | oui | 0 | - | - | - | - |
| Rules_13 | oui | 0 | - | - | - | - |
| Rules_14 | oui | 0 | - | - | - | - |
| Rules_15 | oui | 0 | - | - | - | - |

## 4. Profil de Rules_07 par tranches de 100 lignes (image 897×1497)

| lignes | pixels différents | écart max |
|---|---|---|
| 0–99 | 0 | 0 |
| 100–199 | 0 | 0 |
| 200–299 | 0 | 0 |
| 300–399 | 10 | 4 |
| 400–499 | 100 | 4 |
| 500–599 | 100 | 4 |
| 600–699 | 100 | 4 |
| 700–799 | 100 | 4 |
| 800–899 | 100 | 4 |
| 900–999 | 100 | 4 |
| 1000–1099 | 100 | 4 |
| 1100–1199 | 100 | 4 |
| 1200–1299 | 100 | 4 |
| 1300–1399 | 69351 | 255 |
| 1400–1496 | 15765 | 255 |

Dpi déclaré par la récolte : 300 => 38 px CSS = 118.8 px de rendu

**Instrument SAIN | Bilan récolte : 22 images — 21 identiques, 1 différentes.**
- différente : Rules_harvest_fr.json:Faces:Rules_07 — 86026 px, bbox (71, 390, 825, 1425), écart max 255
