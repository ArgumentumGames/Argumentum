# #1460 phase B — organe 3 : proportions d'écritures cyrillique, CJK et arabe, seuils nommés

Organe : `Generation/Converters/Argumentum.AssetConverter.Tests/Localization/ScriptProportionGuardTests.cs`
(7 tests, mesurés verts le 08/11/2026 sur `db037c78`).
Contrat de phase (corps de #1460) : « proportion Cyrillic/CJK/Arabic (elle attrape les cartes
vides — cf. le bug Scenarii pré-`37600e4a`) — phase statistique, donc seuil nommé et justifié ».

## 1. Ce que l'organe assert

Les langues à écriture propre (ru → cyrillique, zh → CJK, ar et fa → écriture arabe) gardent leur
écriture : une cellule est **dominante** dans son écriture attendue quand la majorité de ses
lettres y appartiennent. Deux seuils, chacun nommé et justifié par la mesure (§3).

C'est l'organe **statistique** de la phase — le mode qu'aucune sonde de mots ne peut attraper :
une passe de traduction qui échoue en bloc laisse la colonne dans la mauvaise écriture, dans
n'importe quelle langue de remplissage (le bug Scenarii pré-`37600e4a` : colonnes silencieusement
non traduites).

## 2. Source et métrique

Même source que les organes 1 et 2 (gabarit × CSV) : couche corpus (DataSets des CardSets activés,
colonnes localisées, `link_*` exclues — une URL est latine par construction) + couche embarquée
(CardSets sans DataSet — les dos —, colonnes restreintes aux radicaux référencés par le mustache :
`tagline_zh` du dos Fallacies est une cellule à écriture ; un dos français sous suffixe `_zh` y
lit comme un effondrement de fichier à une cellule — famille #1141).

Métrique : par cellule, majorité des lettres (`\p{L}` ; chiffres et ponctuation ne comptent pas)
dans l'écriture attendue ; par langue, proportion de cellules dominantes. Plages Unicode :
cyrillique 0400–04FF ; CJK 4E00–9FFF + 3400–4DBF + F900–FAFF ; arabe 0600–06FF + 0750–077F +
FB50–FDFF + FE70–FEFF (le farsi partage les plages).

## 3. Calibration — les seuils et leur justification

Mesuré le 08/11 sur `db037c78`, instrument C# (couche corpus + embarquée) :

| Langue | Cellules | Dominantes | Proportion | Porteurs latins légitimes |
|---|---:|---:|---:|---:|
| ru | 11 063 | 11 047 | **99,86 %** | 16 |
| zh | 11 063 | 11 040 | **99,79 %** | 23 |
| ar | 11 063 | 11 062 | **99,99 %** | 1 |
| fa | 11 063 | 11 054 | **99,92 %** | 9 |

Par fichier, le minimum mesuré est **83,3 %** (Rules P&P zh : UN porteur latin légitime sur
6 cellules — effet petit échantillon, d'où un plancher par fichier qui n'est pas à 90 %).

### Seuil 1 — corpus ≥ 95 % par langue

- Sain mesuré : ≥ 99,79 %. Effondrement historique (un fichier entier non traduit) : Scenarii
  seul (1 336 cellules ru sur 11 063) qui cède descend la proportion à **87,9 %** — sous le seuil.
- Marge sur les porteurs légitimes : zh en porte 23 ; le seuil en tolère 553 (×25).

### Seuil 2 — par fichier ≥ 80 % (fichiers ≥ 6 cellules)

- Minimum sain mesuré : 83,3 %. Un fichier effondré lit ≈ 0 %.
- Rôle complémentaire : le seuil corpus attrape la dérive partielle des gros fichiers qu'un 80 %
  par fichier laisserait passer (un dixième de Scenarii non traduit = 89,8 % par fichier — passe
  le 80 %, casse le 95 % corpus).

Les porteurs latins légitimes, nommés (mesurés) : noms de sophismes latins gardés verbatim en
russe (« Credo quia absurdum », « PIDOOMA »), maximes juridiques latines dans la prose zh/ar
(« Nemo auditur… »), mnémoniques de syllogismes (« Barbara 三段论 »), sigles (« DARVO »), noms
propres (« Aurelia Cotta », « Ergo sum »). Ils sont la raison pour laquelle le seuil est une
proportion et non 100 %.

## 4. Ce que les contrôles prouvent

- **Contrôle inverse sur données réelles** : les colonnes ru de Scenarii remplacées en bloc par du
  remplissage non traduit (en mémoire) → la proportion corpus tombe sous 95 % ET le fichier sous
  80 %. Les seuils détectent, ils ne constatent pas.
- **Companion de calibrage** : la proportion courante doit rester ≥ 99 % — une dérive silencieuse
  sous 99 % est du contenu qui change d'écriture et doit être arbitré, pas absorbé par le plancher
  de 95 %.
- **Anti-vacuité** : ≥ 4 langues mesurées, ≥ 4 fichiers par langue, ≥ 8 000 cellules par langue
  (mesuré : 11 063).

## 5. Ce que cet organe n'établit pas

- Les langues à écriture latine (en/es/pt) : leur échec de traduction est du français ou de
  l'anglais DANS de l'écriture latine — invisible par écriture, attrapé par l'organe 2 (mots-outils
  français). Frontière citée.
- Les gabarits eux-mêmes : sosies cyrilliques → garde dédié existant.
- La fraîcheur des csv embarqués des ensembles à DataSet (snapshots périmés, jamais rendus — §2 de
  l'organe 2).

## 6. État de la phase B après cet organe

Organe 1 (placeholders, #2186) · organe 2 (français dans le non-français, PR #2206) · organe 3
(cette PR). La réécriture de la phase B est faite (#2185/#2186) et ne se rejoue pas.
