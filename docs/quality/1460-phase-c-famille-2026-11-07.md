# #1460 phase C — organe 2 : chaque carte porte la couleur de sa famille

**Date :** 2026-11-07 · **Lane :** po-2023 · **Branche :** `test/1460-phase-c-famille`
**Réf. :** [#1460](https://github.com/ArgumentumGames/Argumentum/issues/1460) (charpente de non-régression), [#1046](https://github.com/ArgumentumGames/Argumentum/issues/1046) (une garde jamais vue rouge est un no-op)

---

## 1. Ce que l'organe mesure

Pour chaque langue qui porte un dossier `Images/density-0/Fallacies`, les **175 cartes** du
paquet Fallacies : la **couleur dominante non blanche** du PNG de récolte doit être la couleur
`--color-background` du bloc `card.<famille>` du gabarit — la famille étant la valeur de la
colonne CSV **`Famille_camelCase`**, c'est-à-dire exactement ce que la configuration injecte
comme classe racine (`cardClass`).

**Aucun PDF n'est ouvert** : cet organe lit les PNG de récolte (Magick.NET, sans Ghostscript).
C'est le complément attendu de l'organe 1, qui lit lui le PDF assemblé.

## 2. Trois ancres, aucune re-dérivation

| Ancre | Source | Rôle |
|---|---|---|
| Colonne `Famille_camelCase` | `Argumentum Fallacies - Taxonomy.csv` (lue par le moteur CSV partagé `HarvestCardIdsCsv`, tolérant aux encodages) | la famille **attendue** de chaque carte |
| Variable `--color-background` du bloc `card.<famille>` | clé `css` du gabarit `Argumentum_Fallacies_Face_fr.json`, **parsée en JSON puis** regex sur la valeur (leçon #1485 : un regex sur les octets bruts s'arrête aux backslashes) | la **couleur** attendue de chaque famille |
| Histogramme du PNG (1 point sur 16) | arbre de régénération (`ARGUMENTUM_PDF_BUNDLE_ROOT`) | la couleur **réalisée** |

La jointure PNG ↔ CSV se fait par le préfixe `argumentum_fallacies_{path}..` — le `path` CSV
porte la hiérarchie **en points** (`1.1.2.1`) et entre tel quel dans le nom de fichier (⚠️ la
virgule n'existe que dans `decimal_path`, notation décimale — le `Replace(',', '.')` du code
est un no-op de robustesse, pas une conversion réelle). **Mesuré : 175/175 appariements
uniques sur les 8 langues.** Le préfixe est non ambigu car le point y est le séparateur de
niveaux : `_1..` ne peut pas matcher `_1.1..`. Un préfixe qui matche ≠ 1 fichier est un échec
**nommé** de la langue entière, jamais une carte silencieusement sautée.

## 3. Le calibrage — mesuré, pas espéré

Arbre : `.prep-1114-worktree` Release (régén d'octobre 2026), 07/11. **8 langues × 175 cartes
= 1 400 cartes**, ~27 s les huit langues. Instrument C# (Magick.NET) :

| Langue | cartes | pire distance à SA famille | part dominante min |
|---|---:|---:|---:|
| ar | 175 | **0,00** | 15,2 % |
| en | 175 | **0,00** | 16,7 % |
| es | 175 | **0,00** | 16,7 % |
| fa | 175 | **0,00** | 15,4 % |
| fr | 175 | **0,00** | 16,8 % |
| pt | 175 | **0,00** | 16,7 % |
| ru | 175 | **0,00** | 16,6 % |
| zh | 175 | **0,00** | **14,8 %** |

La couleur dominante est **exactement** (distance 0,0) la couleur de famille attendue sur
**les 1 400 cartes** — PNG sans perte, zone de teinte pleine. Deuxième instrument indépendant
(Python PIL, `getcolors` + échantillonnage) : mêmes 0,0 partout, même minimum de part dominante
(**14,783 %**, `zh`, `解释谬误`), mêmes distances inter-familles. **Concordance complète.**

Les familles du gabarit, deux à deux :

| Paire la plus proche | Distance |
|---|---:|
| `erreurMathématique` ↔ `abusDeLangage` | **92,92** |
| `erreurDeRaisonnement` ↔ `tricherie` | 114,27 |
| `abusDeLangage` ↔ `insuffisance` | 140,24 |

## 4. Trois bornes, chacune sous le mesuré

| Borne | Valeur | Mesuré | Réserve |
|---|---:|---:|---:|
| `MaxColorDistance` | 30 | 0,0 partout ; première famille *étrangère* à 92,9 | 3,1× sous la première mauvaise réponse |
| `MinFamilyColorSeparation` | 60 | 92,9 (paire la plus proche) | 1,55× |
| `MinDominantShare` | 0,05 | 14,8 % minimum | 2,9× |

La borne de séparation est une **garde structurelle de l'instrument** : si un futur gabarit
définit deux familles plus proches que 2× le seuil, le seuil ne pourrait plus distinguer une
famille d'une autre — l'organe **refuse de verdir** sur un gabarit où il serait aveugle, même
si chaque carte tombe exactement sur sa teinte (témoin synthétique dédié).

## 5. Le contrôle inverse — sur l'objet réel et sur le synthétique

**Mutation du gabarit réel** (07/11) : `--color-background: #08af93` (famille
`erreurMathématique`, 25 cartes) remplacé par `#58ffdd`, fichier restauré byte-identique après
(cp backup/restore, jamais `git checkout --`) :

- gate **rouge** : **200 échecs = 8 langues × 25 cartes**, chaque échec nommant la carte, la
  couleur mesurée `#08af93`, l'attendu muté `#58ffdd`, la famille et la distance (**135,19**) ;
- aucune autre famille touchée — la mutation ne « bave » pas ;
- gabarit restauré → gate **vert** (rejoué).

**Témoins synthétiques** (`FamilyColorIntegrityTests.cs`, 11 tests, CI sans artefact) — chaque
prédicat vu rouge sur l'état défectueux, plus le contrôle inverse du contrôle (un jeu sain rend
0 échec) :

- carte peinte dans la mauvaise famille → rouge, les deux couleurs nommées ;
- **classe absente du gabarit** → rouge « symptôme documenté : fond blanc » (le défaut que
  l'organe garde, cf. CLAUDE.md) ;
- zone de famille fondue (part 0,4 %, couleur exacte) → rouge « la zone a fondu » ;
- deux familles du gabarit à ~13 l'une de l'autre → rouge structurel alors que la carte mesurée
  est exacte ;
- carte sans mesure, mesures dupliquées, entrées vides (3 cas), math de la distance,
  parseur (classe accentuée, hex 8 chiffres, blocs sans couleur ignorés, css vide) → chacun
  son témoin.

## 6. Ce que cet organe n'établit pas

- **La justesse esthétique** de la couleur — que la teinte soit la bonne *pour l'œil* reste un
  verdict humain (charpente §2 : « ⛔ non pour la justesse de la couleur »). L'organe confronte
  trois ancres écrites ; il ne juge pas leur choix.
- **Les couleurs secondaires** (`--color-text-1…3`) : non mesurées.
- **La fraîcheur de l'arbre** : il mesure l'arbre qu'on lui donne contre le CSV et le gabarit
  qu'on lui donne. Un arbre régénéré *avant* une édition du corpus validera l'ancien état contre
  le nouvel attendu — et rougira (l'attendu a bougé), ce qui est le bon comportement, mais la
  fraîcheur relève de la garde d'arbre (organe 0 de la charpente), pas d'ici.
- **Les autres paquets** : Virtues a ses propres classes de famille (`family_fr_camelcase`) ;
  cet organe ne couvre que les Fallacies. Transposé le 07/11 par l'organe 3 —
  [`1460-phase-c-virtues-2026-11-07.md`](1460-phase-c-virtues-2026-11-07.md) — qui a exigé deux
  évolutions du moteur commun (listes de sélecteurs du renommage additif, garde de séparation sur
  les couleurs distinctes).
- La **règle de renommage additif** (nouveau nom cohabite avec l'ancien dans le même bloc CSS)
  n'est pas vérifiée *en soi* : elle l'est par conséquence — un renommage CSV sans bloc gabarit
  fait rougir la colonne « classe absente ».

## 7. Livré

- Moteur pur : `Tests/Structure/FamilyColorIntegrity.cs` (une seule source sur disque,
  compilée par link dans VisualTests — #1054, aucun ProjectReference entre les suites).
- Témoins CI : `Tests/Structure/FamilyColorIntegrityTests.cs` — 11 tests, sans artefact.
- Organe d'arbre : `VisualTests/Structure/FamilyColorGateTests.cs` — env
  `ARGUMENTUM_PDF_BUNDLE_ROOT`, NON MESURABLE rendu tel quel quand ni la variable ni l'arbre
  par défaut n'existent, échec nommé sinon.
- Suite au complet sur la tête de branche : voir le corps de la PR (comptes mesurés le 07/11,
  jamais recopiés d'un rapport précédent).

*po-2023*
