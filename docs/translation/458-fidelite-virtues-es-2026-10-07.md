# #458 file profonde — grain 9 : dossier de fidélité Vertus **es**

**Date :** 2026-10-07 · **Lane :** po-2024 · **Nature :** dossier de mesure, **0 écriture**
**Corpus :** `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` (223 rangées, 82 colonnes, BOM, CRLF)
**Deck :** 131 rangées (`card` non vide)
**Verdict global :** la colonne `es` est **fidèle et propre** — 0 contamination FR, 0 cellule non traduite,
0 mojibake, 0 cellule non-NFC. Cinq familles relevées, dont **deux partagées avec le `pt`** (une même passe
les a produites) et **une propre à l'espagnol**.

> ⚠️ **Trois défauts visibles dans le corpus sont DÉJÀ portés par la PR #1795 (grain 10, en attente de
> merge)** et ne sont donc **pas** re-signalés ici : pk 54 `description_es` (la vertu décrite comme le vice),
> pk 128 `title_es` (« solide » affaibli en « acceptable »), pk 176 `title_es` (rétréci à « adversario »).
> Ce dossier est mesuré sur master `f15c9360`, où le grain 10 n'est pas encore fusionné.

---

## §1 — Périmètre mesuré

| Objet | Mesure |
|---|---|
| Rangées du deck lues intégralement, 3 voix (`fr` \| `en` \| `es`) | **131 / 131** |
| Rangées balayées par écran machine | 223 / 223 |
| Cellules vides (`title`/`description`/`remark`/`family`, deck) | **0** |
| Colonnes `es` mesurées | `family_es` `subfamily_es` `subsubfamily_es` `title_es` `description_es` `remark_es` `link_es` — **7** |
| Écrans négatifs | contamination FR **0** (calibré, cf. §4.1) · `es == fr` **0** · mojibake **0** · non-NFC **0** |
| Vides structurels | `subfamily_es` 8 · `subsubfamily_es` 27 — **ensembles identiques aux 8 langues** (têtes de famille) |

---

## §2 — Défauts

### V-ES-01 — pk 175 : le titre es dit « Consideración » là où sa propre description dit « Reconocimiento » · **certain**

| Voix | Texte |
|---|---|
| `title_fr` | Reconnaître ses biais idéologiques |
| `title_en` | Recognition of personal ideological biases |
| `title_ru` / `title_ar` / `title_fa` / `title_zh` | Признание … / التعرّف … / بازشناسی … / 承认 … |
| `title_es` | **Consideración** de los propios sesgos ideológicos ✗ |
| **sa propre `description_es`** | « **Reconocimiento** de los propios sesgos ideológicos personales vinculados a convicciones… » |

Deux arguments, et le second est interne à la rangée :

1. **Six langues sur sept** disent *reconnaître* ; `es` et `pt` sont les deux seules à dire *tenir compte* —
   le verbe de la **voisine 176**. C'est la famille ⑱, dont la garde `VirtuesDeckTitlesG18GuardTests`
   ne couvre que `ar`/`fa`/`ru` : **`es` et `pt` n'ont jamais été vérifiés** (le `pt` est traité au grain 8,
   PR #1796).
2. **La rangée se contredit elle-même** : son titre annonce une *consideración*, sa description une
   *reconocimiento*.

Correction proposée : `Reconocimiento de los propios sesgos ideológicos`, aligné sur les sœurs 169/172 et
sur la description de la rangée.

### V-ES-02 — pk 208 : l'antithèse de la carte est perdue — **et le `pt` porte le même texte** · **certain**

| Langue | Texte de `remark` |
|---|---|
| fr | « Dans un débat constructif, vous **critiquez ce qui est dit, non la personne qui le dit**, afin de faire avancer la discussion. » |
| en | « In a constructive debate, **criticize what is said, not the person saying it**… » |
| **es** | « En un debate constructivo, **las críticas son necesarias** para hacer avanzar la discusión. Esto implica formular críticas de manera respetuosa, reconociendo la posición del otro sin juicio personal y refutando sus argumentos de manera constructiva. » |
| **pt** | « Num debate construtivo, **as críticas são necessárias** para fazer avançar a discussão. Isso implica formular críticas de forma respeitosa, reconhecendo a posição do outro sem julgamento pessoal e refutando os seus argumentos de maneira construtiva. » |

Les deux sont **parallèles mot pour mot** : même attaque (« las críticas son necesarias » / « as críticas são
necessárias »), même troisième proposition, même clausule. Ce n'est donc **pas un accident espagnol** : une
**même passe** a produit la perte dans les deux langues.

Et la colonne `es` *possède* l'antithèse ailleurs — pk 217 `remark_es` : « **Atacar una idea sin arremeter
contra la persona** que la sostiene… ». L'excuse « l'espagnol ne sait pas le dire » est réfutée par le
corpus lui-même.

### V-ES-03 — Le registre d'adresse est hétérogène : 26 rangées tutoient, 7 vouvoint · **certain, propre à l'es**

Mesure par inventaire de **marqueurs non ambigus** (2ᵉ personne du singulier contre `usted`-explicite ou
impératif de 3ᵉ personne), sur les 131 rangées du deck :

| Registre | Rangées |
|---|---|
| **Tutoiement** (`tu`, `tus`, `te`, `comprueba`, `adapta`, `reconoces`, `defiendes`, `sabes`, `puedes`, `debes`, `concéntrate`, `expón`, `dudes`, `evalúa`, `buscas`…) | **26** — 9, 10, 12, 33, 42, 55, 56, 83, 92, 93, 95, 133, 135, 159, 163, 169, 187, 190, 196, 206, 207, 209, 210, 211, 212, 216 |
| **Vouvoiement** (`usted`, `exponga`, `cuente`, `encuesta`, `procure`, `dispone`/`anuncie`/`reserve`, `espere`) | **7** — 5, 34, 61, 151, 167, 204, 220 |
| Les deux **dans la même rangée** | **0** |
| Sans marqueur (impersonnel) | 98 |

La répartition est **par rangée**, jamais mêlée à l'intérieur d'une rangée : ce ne sont pas des accidents de
rédaction mais des cartes traduites dans deux registres différents.

**Le français d'origine est homogène** — et c'est ce qui rend la famille propre à l'es. Contrôle :

| Colonne | adresse | mesure |
|---|---|---|
| `fr` | `vous` / `votre` / `vos` | **72 cellules** |
| `fr` | `tu` / `ton` / `tes` | **0 réelle** — les 6 cellules détectées sont toutes le **nom** « ton » (« un *ton* méprisant », « *Ton* respectueux »), cf. §4.2 |
| `pt` (témoin) | `você` / `te` / `teu` | 23 cellules, **0 mélange intra-cellule** |

Le `pt` a donc choisi un registre unique ; l'`es` en a deux. La norme majoritaire (26 rangées) est le
tutoiement ; l'alignement des 7 rangées restantes est une **décision éditoriale**, pas une correction
évidente — c'est le seul point de ce dossier qui demande un arbitrage.

### V-ES-04 — 10 descriptions sans point final, et la famille est **une seule**, pas six · **certain**

| Langue (deck) | `description` sans point | dont communes avec `en` |
|---|---|---|
| fr | **0 / 131** | — |
| **es** | **10** — 2, 4, 6, 7, 8, 12, 24, 144, 220, 221 | 8 |
| en | 11 | — |
| pt | 11 | 6 |
| fa / ar / zh / ru | 14 / 6 / 6 / 4 | 7 / 5 / 4 / 2 |

13 rangées au corpus pour l'es (les 10 du deck + pk 3, 11, 44 hors deck). Le `fr` est **la seule langue à
toujours porter le point**.

⚠️ **Le fait structurant est le noyau, pas le total** — c'est ce que la comparaison inter-langues établit :

| Rangée | Langues sans point (sur 8) |
|---|---|
| pk **8** | **7** |
| pk 4, 6, 7 | 6 |
| pk 12 | 5 |
| pk 2 | 4 |
| pk 5, 10, 24, 144 | 3 |
| pk 9, 33, 220, 221 | 2 |
| pk 28, 29, 31, 32, 34, 89, 90, 209 | 1 |

**22 rangées** sont touchées au moins une fois. Quatre rangées perdent le point dans 6 ou 7 langues
simultanément : ce n'est pas six accidents indépendants mais **un point perdu à une passe partagée**. La
correction est donc **un seul geste inter-langues**, pas une famille par langue — ce qui change le chiffrage
du grain qui la portera.

### V-ES-05 — `link_es` : 43 vides contre 24 pour le fr · **certain, périmètre restreint**

| Colonne | deck | corpus |
|---|---|---|
| `link_fr` / `link_en` | 24 | 33 |
| `link_ru` / `link_pt` | 26 / 29 | 36 / 37 |
| **`link_es`** | **43** | **73** |
| `link_ar` / `link_fa` / `link_zh` | 74 / 72 / 67 | 131 / 128 / 118 |

L'essentiel du trou est **inter-langues** (`link` n'est rendu par aucun gabarit de carte — 0 `{{link}}` dans
`Cards/**/*.json` — donc **aucune régénération n'est requise**), et 10 rangées sont vides dans les 8 langues
(pk 12, 29, 47, 157, 165, 200, 216, 217, 219, 221) : structurel.

**Le seul point actionnable** est l'es-seul : **pk 208 et pk 209 sont vides en `es` alors que les 7 autres
langues portent un lien**. (Pour mémoire, les autres langues ont leurs propres trous isolés : en 61, ru 167,
pt 205/206, ar 92/135, fa 8/53/73/84/156.)

### V-ES-06 — pk 79 : l'es recopie une tautologie **introduite par l'anglais** · **certain, cas citable**

| Langue | `remark` |
|---|---|
| fr | « **Un raisonnement valide avance étape par étape** : il part de prémisses solides, tient compte des données pertinentes et vérifie chaque inférence avant de conclure. » |
| en | « **Rigorous reasoning uses a rigorous and logically valid reasoning methodology** to arrive at sound conclusions… » ← circulaire |
| es | « **Un razonamiento riguroso utiliza una metodología de razonamiento rigurosa** y lógicamente válida para llegar a conclusiones justas… » ← recopie |

Le français est net ; l'anglais introduit la circularité (« un raisonnement rigoureux utilise une
méthodologie de raisonnement rigoureuse ») ; l'espagnol la reproduit **terme à terme**. C'est l'instance
citable de la lignée EN mesurée en §3.1 — utile parce qu'une similarité de chaîne ne prouve pas une
filiation, alors qu'une tautologie recopiée mot pour mot, si.

---

## §3 — Observations (documentées, non actionnables)

### §3.1 — Lignée : majoritairement française, avec une minorité anglaise mesurée

| Champ | Δ = sim(en,es) − sim(fr,es), moyenne | médiane | rangées plus proches de l'**en** |
|---|---|---|---|
| `description` | **−0,092** | −0,108 | **30 / 131** |
| `remark` | **−0,081** | −0,060 | **30 / 131** |

Même structure que pour le `pt` (grain 8) : la lignée est **française en majorité**, l'anglais est une
minorité réelle (~23 %) et **citable** (V-ES-06). L'anglais gagne quand il a été réécrit — l'espagnol suit
alors la version anglaise réécrite : `title_es` de pk 84 (« Construcción rigurosa de los enunciados » ← en
« Rigorous construction of statements »), `title_es` de pk 92, `title_es` de pk 177 (« Aceptación de la
incertidumbre »).

### §3.2 — Amplification modérée

Ratio de longueur es/fr sur `description`+`remark` : min 0,92 · Q1 1,05 · **médiane 1,20** · Q3 1,51 ·
max 2,55. Contre `en` : médiane 1,13. Plus marquée que le `pt` (1,10), sans être systématique.

### §3.3 — Ajouts sans perte de sens (famille d'amplification)

- **pk 177** : l'es ouvre sur « **En ciencia**, la incertidumbre no es un fracaso, sino un estado inherente
  al conocimiento » — phrase **absente** du fr et de l'en, qui ouvrent tous deux sur la médecine. Le propos
  médical est conservé ensuite ; c'est un ajout, pas une dérive.
- **pk 89** : la remarque es **reprend la définition de la description** avant de dérouler ; la métaphore du
  fr/en (« chaque inférence sert de **marche** » / « acts as a **step** ») est remplacée par une formulation
  procédurale. Petite perte d'image, contenu conservé.

### §3.4 — Deux pivots déjà consignés au dossier ⑮/⑮w, inchangés

- **pk 200** : `title_es` « **Adherencia** al tema » pour fr « Respect du sujet » — même pivot que le pt
  (« Adesão ao tema »), déjà noté C.
- **pk 89** : `title_es` « Razonamiento **jalonado** » — gallicisme noté C (es + pt) ; l'`en` dit
  « Stepwise reasoning ».

### §3.5 — Nominalisation et glissement de nombre

`pk 0` : `description_es` et `remark_es` sont au **pluriel** (« Argumentos bien construidos… ») là où le fr
est au singulier (« Argument bien construit… »). Même décalage que dans le `pt` — partagé, documenté, sans
effet sur le livrable.

---

## §4 — Écrans, contrôles, et **trois pièges d'instrument**

### §4.1 — L'écran de contamination FR était **entièrement du bruit**

Sans calibration : **24** cellules `description_es` et **61** `remark_es` flaguées comme contaminées par le
français. Après retrait des **homographes espagnols présents dans la liste FR** — `car entre la ni par que
si son sur` (9 mots, nommés) — les deux compteurs tombent à **0**.

Un écran qui flaguait 85 cellules et dont le résultat correct est 0 n'était pas un instrument, c'était un
générateur de faux positifs. La calibration **doit** être refaite langue par langue : la liste de
mots-outils française contient mécaniquement du vocabulaire espagnol.

### §4.2 — `ton` est un nom français avant d'être un possessif

L'écran « registre » comptait `tu / ton / tes / toi` dans la colonne `fr` : **6 cellules**. Toutes les six
sont le **substantif** « ton » (« un *ton* méprisant », « garder un *ton* calme », « *Ton* respectueux »).
Le possessif n'apparaît **nulle part**. Le français est donc à **100 % au vouvoiement**, et l'hétérogénéité
de V-ES-03 est **propre à l'espagnol** — sans ce contrôle, j'aurais conclu à une hétérogénéité héritée.

### §4.3 — `se`, `su`, `sus`, `le`, `les` ne disent pas le vouvoiement

Mon premier écran de registre incluait ces cinq formes et trouvait **8 rangées mélangeant tutoiement et
vouvoiement**. C'étaient **8 faux positifs** : en espagnol, `se`/`su`/`sus` sont aussi — et le plus souvent —
de **3ᵉ personne anaphorique**. « **sus** premisas » désigne les prémisses *du raisonnement*, pas celles *du
lecteur*. Après retrait : **0 mélange intra-rangée**.

Puis, sur les 9 rangées restantes du côté formel, **2 de plus** ont été écartées à la lecture :

- **pk 147** — `usted` vit **à l'intérieur d'un exemple linguistique cité** (« Usted acusa a Pedro, que
  estaba ausente »), pas dans une adresse au lecteur ;
- **pk 219** — `tenga` est un **subjonctif de 3ᵉ personne** déclenché par « requiere que », pas un
  vouvoiement.

⚠️ Réserve générale à porter : en espagnol, le vouvoiement et **l'impersonnel de 3ᵉ personne sont
morphologiquement identiques**. Les 7 rangées retenues sont un **plancher**, pas un compte exact : certaines
pourraient être impersonnelles plutôt que vouvoyées.

### §4.4 — Contrôle structurel

Les titres ne prennent **jamais** de point final : **131 / 131 dans les 8 langues**, écran corrigé compris.
Témoin que l'écran de ponctuation n'est pas simplement débranché.

### §4.5 — Contrôle : aucune cellule `fr` n'est concernée

Les observations portent sur `es` (V-ES-01 → 06) et, pour V-ES-02, sur une famille **partagée avec le `pt`**.
**0 cellule `fr`.** La règle « contenu FR Vertus = owner » n'est pas sollicitée par ce dossier.

---

## §5 — Ce qui n'est PAS fait

| Objet | État |
|---|---|
| **Écritures CSV** | **aucune** — ce grain mesure |
| **Décalage mind-map / OWL** | **sans objet** : rien n'a bougé (contraste avec le grain 10) |
| **Corrections de §2** | proposées ; V-ES-01/02/04/06 sont des corrections, **V-ES-03 demande un arbitrage de registre**, V-ES-05 est 2 cellules `link_es` |
| **Déjà porté par #1795** | pk 54 `description_es` · pk 128 `title_es` · pk 176 `title_es` — **non re-signalés** |

---

## §6 — Reproduire

Scripts dans le scratchpad de session (`…/6ffd7674-…/scratchpad/`), aucun n'écrit sur le dépôt :

| Script | Rôle |
|---|---|
| `glang-ecrans.py <lang>` | écran **générique** : vides, contamination FR calibrée, `lang == fr`, mojibake, ponctuation à ensemble terminal complet, ratios, chiffres, NFD, classement + dumps |
| `g9-points.py` | ensembles de rangées sans point final par langue, intersections, noyau, trous de `link` |
| `g9-tuteo.py` / `g9-tuteo2.py` | registre d'adresse — version large (contaminée) puis version **à marqueurs non ambigus** |
| `g9-verif.py` | vérification des rangées « vouvoiement » une à une (citation ? impersonnel ?) |

---

*po-2024*
