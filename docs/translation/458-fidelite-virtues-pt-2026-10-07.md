# #458 file profonde — grain 8 : dossier de fidélité Vertus **pt**

**Date :** 2026-10-07 · **Lane :** po-2024 · **Nature :** dossier de mesure, **0 écriture**
**Corpus :** `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 rangées, 82 colonnes, BOM, CRLF)
**Deck :** 131 rangées (`card` non vide) — les seules imprimées
**Verdict global :** la colonne `pt` est **substantiellement fidèle**. Sept observations classées, dont
**deux défauts certains** (`fatos`, pk 175) et **un défaut d'encodage isolé** (pk 1 en NFD). Aucune
correction n'est écrite ici : ce grain **établit**, il ne modifie pas.

---

## §0 — Pourquoi ce grain est délégable (et pourquoi ce n'est pas le cas du grain 10)

Le grain 10 (Vertus **en/ru/pt/es/ar/fa**, 28 cellules) portait sur du contenu **jamais imprimé** : les
Vertus n'ont jamais été régénérées, donc le dossier « EN imprimée » qui servait de prémisse était faux
([#1793 c.6036588022](https://github.com/ArgumentumGames/Argumentum/pull/1793#issuecomment-6036588022)).

Le grain 8 est d'une autre nature : **il n'écrit rien du tout.** Les corrections qu'il identifie sont
proposées, chiffrées et localisées, mais appliquées dans un grain ultérieur — comme le grain 6 (dossier
EN) l'a été avant que le grain 10 ne le solde. La contrainte « contenu FR = owner » n'est pas touchée :
**aucune cellule `fr` n'est concernée par ce dossier** (contrôle en §4.5).

---

## §1 — Périmètre mesuré

| Objet | Mesure |
|---|---|
| Rangées du deck lues intégralement, 3 voix (`fr` \| `en` \| `pt`) | **131 / 131** |
| Rangées du corpus balayées par écran machine | 223 / 223 |
| Cellules vides (`title`/`description`/`remark`, toutes langues, deck) | **0** |
| Colonnes `pt` (mesurées) | `family_pt` `subfamily_pt` `subsubfamily_pt` `title_pt` `description_pt` `remark_pt` `link_pt` — **7** |
| Colonnes du fichier / rangées / deck | 82 / 223 / 131 |

Vides au deck, colonne `pt`, et **leur caractère structurel** (mesuré, pas supposé) :

| Colonne | vides (pt) | identique dans les 8 langues ? | pk concernés |
|---|---|---|---|
| `title_pt` · `description_pt` · `remark_pt` · `family_pt` | **0** | oui (0 partout) | — |
| `subfamily_pt` | 8 | **oui** | 0, 1, 34, 59, 79, 134, 152, 179 — têtes de famille |
| `subsubfamily_pt` | 27 | **oui** | idem, même ensemble |
| `link_pt` | 29 | **non** — lacune **inter-langues**, pas pt-spécifique | #1435/#1437, soldé |

La lecture s'est faite en deux lots : les **30 rangées les plus divergentes** (classement par
`min(sim(fr,pt))` sur `description` et `remark`), puis les **101 restantes** — soit la totalité du deck,
sans échantillonnage.

---

## §2 — Défauts

### V-PT-01 — pk 175 : le titre pt (et es) porte le verbe de sa voisine 176 · **certain**

| Langue | pk 169 | pk 172 | **pk 175** | pk 176 |
|---|---|---|---|---|
| fr | Reconnaître | Reconnaître | **Reconnaître** | Tenir compte |
| en | Recognition | Recognition | **Recognition** | Taking … into account |
| ru | Признание | Признание | **Признание** | Учет |
| ar | التعرّف | التعرّف | **التعرّف** | مراعاة |
| fa | بازشناسی | بازشناسی | **بازشناسی** | درنظرگرفتن |
| zh | 承认 | 承认 | **承认** | 考虑 |
| **pt** | Reconhecimento | Reconhecimento | **Consideração** ✗ | Consideração |
| **es** | Reconocimiento | Reconocimiento | **Consideración** ✗ | Consideración |

`pt` et `es` sont les **deux seules langues** où la tête du titre de 175 est identique à celle de 176.

C'est **exactement la famille ⑱** — dont l'arbitrage (garde `VirtuesDeckTitlesG18GuardTests`) porte sur
`ar`, `fa`, `ru` **et rien d'autre** :

```
("ar", "التعرّف إلى التحيزات", new[] { "169", "172", "175" }),
("fa", "بازشناسی سوگیری",     new[] { "169", "172", "175" }),
("ru", "Признание",            new[] { "169", "172", "175" }),
```

`pt` et `es` n'ont **jamais été vérifiés** sur cette famille. Le défaut y est intact depuis toujours, et
n'a pas été créé par le grain 10 — celui-ci a touché `title_pt`/`title_es` de **176** (le qualificatif),
pas la tête verbale de **175**. Correction proposée, alignée sur les sœurs :

- `title_pt` : `Consideração dos próprios enviesamentos ideológicos` → **`Reconhecimento dos próprios enviesamentos ideológicos`**
- `title_es` : `Consideración de los propios sesgos ideológicos` → **`Reconocimiento de los propios sesgos ideológicos`**

### V-PT-02 — `fatos` dans un corpus PT-PT · **certain**

`facto` est la forme portugaise (PT-PT) ; `fato` est la forme **brésilienne**. Le corpus emploie `factos` :

| Forme | Rangées du corpus | dont deck |
|---|---|---|
| `factos` (PT-PT) | **18** | 12 |
| `fatos` (BR) | **7** — pk 11, 12, 33, 53, **167**, 181, 194 | **5** — 12, 33, 53, 167, 194 |

Trois rangées **mélangent les deux formes dans la même rangée** : **11, 167, 181**. Le cas le plus net est
pk 167, où la même carte écrit `fatos` dans sa description et `factos` dans sa remarque.

⚠️ Les rangées 11 et 181 sont **hors deck** (non imprimées) : la famille est de 7 au corpus mais de **5 au
livrable**.

### V-PT-03 — pk 32 : `fenômeno` (circonflexe, BR) · **certain, isolé**

PT-PT écrit `fenómeno` (aigu) ; `fenômeno` est la graphie brésilienne. Mesure : `fenómeno` sur **2** rangées,
`fenômeno` sur **1 seule** — pk 32, `remark_pt`. C'est le seul écart de cette paire dans tout le corpus.

### V-PT-04 — pk 1 `description_pt` : cellule **décomposée** (NFD) · **certain, isolé**

```
« Afirmação bem fundada que tem em conta o contexto e as provas disponíveis para se justificar. »
         ^^^^^^ c + U+0327 (cédille combinante) + a + U+0303 (tilde combinante)
```

Là où tout le reste du fichier écrit `Afirmação` en précomposé (`U+00E7`, `U+00E3`).

**Le fichier n'est pas intégralement NFC**, et c'est le seul endroit latin qui ne l'est pas :

| Langue | cellules non-NFC | verdict |
|---|---|---|
| fr, en, ru, es, fa, zh | **0** | — |
| **pt** | **1** (pk 1 `description`) | **défaut réel** |
| ar | 23 | **structurel** — voir §4.4 |

Le risque n'est pas cosmétique : une chaîne décomposée **rate les recherches de sous-chaîne**, ne matche pas
une épingle de garde écrite en précomposé, et peut ne pas trouver son glyphe dans une fonte à couverture
partielle. C'est la famille « un caractère invisible se cite par point de code, jamais par copie ».

**Second défaut dans la même cellule** : la tête de phrase est « **Afirmação** » (affirmation) là où la carte
(pk 1, `title_fr` « Argument pertinent ») décrit un **argument** :

- `description_fr` : « **Argument** adapté au contexte, appuyé par des preuves utiles à la conclusion. »
- `description_en` : « **Argument** adapted to the context and supported by evidence useful to the conclusion. »
- `description_pt` : « **Afirmação** bem fundada que tem em conta o contexto… » ✗

### V-PT-05 — 11 `description_pt` sans point final · **certain**

Toutes les `description_fr` et `description_en` du deck finissent par un point (**0 / 131** sans). La colonne
`pt` en compte **13 au corpus**, **11 au deck** :

| pk | fin de `description_pt` | fin de `description_fr` |
|---|---|---|
| 2 | `…raciocínios sólidos` | `…ments solides.` |
| 4 | `…aceites como verdadeiras` | `…s pour vraies.` |
| 6 | `…factos científicos verificáveis` | `…s vérifiables.` |
| 7 | `…posições das partes` | `…s des parties.` |
| 8 | `…a tornar mais compreensível` | `…e plus claire.` |
| 12 | `…sólidas e credíveis` | `… et crédibles.` |
| 24 | `…plausíveis e necessárias` | `…t nécessaires.` |
| 33 | `…incompetência ou ignorância` | `…u l'ignorance.` |
| 34 | `…sofismas e manipulações` | `…manipulations.` |
| 144 | `…uma semelhança pertinente` | `…de pertinente.` |
| 209 | `…em vez de visar as pessoas` | `…les personnes.` |

Hors deck : pk 3, pk 11 (même famille, non imprimées). C'est **la même famille typographique** que les 13
`description_en` soldés par le grain 10 — transposée à une autre langue.

### V-PT-06 — pk 208 `remark_pt` : l'antithèse de la carte est perdue · **certain**

| Voix | Texte |
|---|---|
| fr | « Dans un débat constructif, vous **critiquez ce qui est dit, non la personne qui le dit**, afin de faire avancer la discussion. » |
| en | « In a constructive debate, **criticize what is said, not the person saying it**, in order to move the discussion forward. » |
| pt | « Num debate construtivo, **as críticas são necessárias** para fazer avançar a discussão. Isso implica formular críticas de forma respeitosa, reconhecendo a posição do outro sem julgamento pessoal e refutando os seus argumentos de maneira construtiva. » |

Le pt remplace l'antithèse (le trait distinctif de la carte) par une généralité, et **ouvre sur une phrase
absente des deux autres voix** (« as críticas são necessárias »).

Ce n'est pas une imprécision de traduction mais une **perte d'information** : la colonne pt *possède*
l'antithèse ailleurs — pk 217 `remark_pt` écrit « Atacar uma ideia **sem visar a pessoa que a defende** ».
L'argument « le pt ne sait pas dire cela » est donc réfuté par le corpus lui-même.

### V-PT-07 — homogénéité interne : `enviesamento`/`viés` et `correcto`/`correto` · **cohérence, pas faute**

À la différence de V-PT-02/03, **les deux formes sont défendables en PT-PT** : `viés` est attesté en
portugais européen, et `correcto`/`correto` relèvent de l'orthographe d'avant / d'après l'Accord de 1990.

| Paire | Rangées du deck | Mélangent **dans la même rangée** |
|---|---|---|
| `enviesamento(s)` (13) / `viés`-`vieses` (3) | 25, 27, 28, 30, 60, 61, 154, 167, 169, 170, 172, 173, 175, 176, 189 / 167, 169, 170 | **167, 169, 170** |
| `correcto`-`correctamente` (6) / `correto`-`corretamente` (5) | — | **82** |

Détail des mélanges, cellule par cellule :

- **pk 167** — `description` = `fatos` + `vieses` (BR) · `remark` = `factos` + `enviesamentos` (PT-PT)
- **pk 169** — `title` + `description` = `enviesamentos` · `remark` = `viés`
- **pk 170** — `remark` porte **les deux formes à l'intérieur de la même cellule**
- **pk 82** — `title` = `corretamente` · `description` = `correcto` · `remark` = `correctamente`

C'est l'incohérence qui est signalée, **pas la graphie** : ces quatre rangées doivent choisir une norme,
quelle qu'elle soit.

---

## §3 — Observations (documentées, non actionnables)

### §3.1 — L'amplification des remarques est réelle mais **concentrée**, pas systématique

| Ratio longueur pt/fr sur `remark` | min | Q1 | médiane | Q3 | max | rangées > 1,5× |
|---|---|---|---|---|---|---|
| vs `fr` | 0,84 | 0,98 | **1,10** | 1,71 | 3,18 | **43 / 131** |
| vs `en` | 0,53 | 1,00 | **1,08** | 1,76 | 3,36 | 39 / 131 |

La médiane est à **1,10** : la remarque pt est en règle générale **à peine plus longue** que la française.
L'expansion forte existe (un tiers du deck dépasse 1,5×) mais elle est **concentrée sur ces rangées**, et
plusieurs remarques pt sont **plus courtes** que la fr (min 0,84). Écrire « les remarques pt sont
systématiquement amplifiées » serait plus fort que la mesure.

L'amplification observée conserve le contenu : elle ajoute des phrases d'amorce (« É imperativo… »,
« A utilização… é primordial… », « É prática corrente… ») et des gloses. Le seul cas où elle **retire** du
sens est V-PT-06.

### §3.2 — La lignée est **majoritairement française**, avec une minorité anglaise

Mon impression initiale, tirée de la lecture de ~15 rangées, était que la colonne pt suivait la **voix
anglaise**. La mesure sur 131 rangées **la réfute** :

| Champ | Δ = sim(en,pt) − sim(fr,pt), moyenne | médiane | rangées plus proches de l'**en** |
|---|---|---|---|
| `description` | **−0,084** | −0,089 | **27 / 131** |
| `remark` | **−0,068** | −0,046 | **39 / 131** |

Le pt est donc plus proche du **fr** dans la grande majorité des rangées ; un quart à un tiers penche vers
l'en. Les deux sont vrais, mais le second n'est pas la règle.

### §3.3 — Nominalisation des titres (déjà documentée au grain 6, non résignalée)

Ex. pk 174 : fr « Mettre les idéologies à distance » → pt « **Distanciamento** das ideologias ». Famille
neutre, sans effet sur le livrable.

### §3.4 — Choix de tête nominale sur deux titres

- **pk 94** : fr « Validité formelle » → pt « **Lógica formal válida** » (la tête passe de *validité* à *logique*)
- **pk 92** : fr « Cohérence interne » → pt « Ausência de contradições internas » — le pt suit ici **l'en**
  (« Absence of internal contradictions »). Lignée documentée, pas un défaut.

### §3.5 — pk 200 : déjà arbitré, inchangé

`title_pt` « **Adesão ao tema** » pour fr « Respect du sujet » — pivot *respect → adhésion* signalé comme
note C au dossier ⑮/⑮w, **laissé en l'état** (choix éditorial). Aucun signalement nouveau.

### §3.6 — pk 161 : glissement normatif → descriptif

- fr : « l'auteur d'une affirmation **doit** fournir des preuves ou des raisons »
- pt : « **É prática corrente** num debate racional **solicitar ao autor** … que apresente as provas »

Le principe (charge de la preuve sur celui qui affirme) survit comme implication, mais le cadrage passe
d'une règle normative à une pratique observée. Observation, pas défaut — la carte n'est pas fausse.

---

## §4 — Écrans, contrôles et **trois défauts d'instrument que j'ai dû corriger**

### §4.1 — `vies` matchait à l'intérieur de `enviesamento`

Mon premier écran comptait `vies` **par sous-chaîne** : 35 occurrences. Or `en**vies**amento` contient
`vies`. Le chiffre était gonflé par la forme concurrente qu'il prétendait mesurer. Corrigé par bornes de mot
(`\b`) : **9** au corpus, **3** au deck. Un compteur par sous-chaîne sur des formes qui s'emboîtent mesure
les deux à la fois.

### §4.2 — L'écran de ponctuation terminale était aveugle au chinois **et aux guillemets courbes**

| Écran | zh `description` | en `remark` |
|---|---|---|
| naïf (`. ! ? … » " ' ) ]`) | **223 / 223** ✗ | **4 / 223** ✗ |
| corrigé (+ `。！？` + `”` `’`) | **8 / 223** | **0 / 223** |

- `zh` : 215 / 223 descriptions finissent par `。`, absent de mon ensemble terminal. Un `223/223` sur un
  corpus qui ponctue massivement est **le signe que l'instrument ne voit pas**, pas un résultat.
- `en remark` : les 4 « sans point » finissent tous par `.”` — **le point est là**, suivi d'un guillemet
  courbe. Faux positifs.

⚠️ Ce second point **annule une ligne que j'avais écrite** dans le rapport du grain 10 (« il reste des
`remark_en` sans point final ») : mesurés correctement, il en reste **zéro**.

### §4.3 — Un écran de normalisation NFC n'est pas neutre entre les langues

`23` cellules arabes ressortent non-NFC — et c'est **structurel** : l'arabe n'a pas de forme précomposée
pour `ش + َ` (U+0651 shadda, U+064E fatha) ; les diacritiques y sont des marques combinantes **par
construction**. Un écran NFC qui sommerait 24 cellules sur 8 langues mélangerait 23 non-défauts et
1 défaut. Seule la cellule portugaise est réelle.

### §4.4 — Contrôle inverse : les formes brésiliennes ne viennent pas du français

| Forme | occurrences dans la colonne `fr` |
|---|---|
| `fatos` | **0** |
| `viés` | **0** |
| `vieses` | **0** |
| `fenômeno` | **0** |

L'écran pouvait voir un `1` : il voit 0. La famille BR est bien propre à la colonne pt.

### §4.5 — Contrôle : aucune cellule `fr` n'est concernée par ce dossier

Les 7 observations portent sur `pt` (V-PT-01 → 07), plus `es` pour V-PT-01. **0 cellule `fr`.** La règle
« contenu FR Vertus = owner » n'est pas sollicitée, ce qui rend le grain délégable.

### §4.6 — Contrôle structurel : les titres ne prennent jamais de point final

**131 / 131 dans les 8 langues**, écran corrigé compris. C'est une propriété de structure, pas un défaut —
et c'est le témoin qui prouve que l'écran de §4.2 n'est pas simplement débranché.

---

## §5 — Ce qui n'est PAS fait (et ce que le grain 10 devait déclarer, lui)

| Objet | État |
|---|---|
| **Écritures CSV** | **aucune** — ce grain mesure |
| **Décalage mind-map** | **sans objet** : rien n'a bougé. (Contraste avec le grain 10, qui devait déclarer 30 fichiers SVG/HTML en retard de re-dérivation.) |
| **Décalage OWL** | **sans objet**, même raison |
| **Corrections de §2** | proposées, localisées, chiffrées — à appliquer dans un grain d'écriture (V-PT-01 à 06 ; V-PT-07 = choix de norme) |
| **`remark_en` sans point** | **0** — les 4 annoncés étaient un faux positif de mon écran (§4.2) |

---

## §6 — Reproduire

Scripts dans le scratchpad de session (`…/6ffd7674-…/scratchpad/`), aucun n'écrit sur le dépôt :

| Script | Rôle |
|---|---|
| `g8-ecrans.py` | écrans de base : vides, contamination FR, `pt == fr`, mojibake, ponctuation, ratios, chiffres |
| `g8-calibrate.py` | écran FR **recalibré** (9 homographes portugais retirés et nommés) + classement par divergence |
| `g8-mesures.py` | variantes PT-PT/PT-BR, famille 169/172/175/176 toutes langues, dump des 101 rangées restantes |
| `g8-variantes2.py` | variantes **à bornes de mot** + liste nominative des 11 descriptions sans point |
| `g8-unicode.py` | NFD/NFC par langue + variantes élargies + rangées qui mélangent |
| `g8-chiffres.py` | lignée fr/en quantifiée, détail des mélanges, amplifications, liste BR finale, contrôle inverse |
| `g8-ponctuation.py` | écran ponctuation **naïf vs corrigé** (guillemets courbes + CJK) |

---

*po-2024*
