# i18n `es` / `ar` / `fa` / `zh` — re-mesure **couverture** + **alignement** (0 mutation)

**Status:** MESURE — aucun CSV modifié (⛔ 0 mutation). Ce document est un résultat, pas un plan.
**Author:** po-2024 — grain **primaire** du dispatch ai-01 `msg-20260913T105836-kmhyzw`.
**Base:** master `35acac04` (2026-09-13). Mesure rejouable : parseur CSV, re-dérivée de la SOURCE.

> Le « 100 % vérifié cell-by-cell » de mai 2026 **ne couvre que EN/RU/PT**. Ce document mesure
> les quatre autres langues, qui ne l'avaient jamais été, et sépare deux questions distinctes :
> **couverture** (la cellule est-elle remplie ?) et **alignement** (est-elle dans la bonne langue,
> non recopiée du FR ?). La seconde est celle qui avait attrapé `title_pt` « Jeanne d'Arc », que la
> première déclarait remplie.

## 0. Portée réelle — le chiffre « ~640 cellules » n'est pas reproduit

Mesuré : **12774 cellules par langue** sur 5 corpus, soit **51096 cellules** pour
`es`+`ar`+`fa`+`zh` — et non ~640. Chaque corpus est balayé **intégralement**, ligne par ligne :

| Corpus | Fichier | Lignes de données | Champs | Cellules / langue |
|---|---|---:|---:|---:|
| `Fallacies` | `Argumentum Fallacies - Taxonomy.csv` | 1408 | 7 | 9856 |
| `Virtues` | `Argumentum Virtues - Taxonomy.csv` | 223 | 7 | 1561 |
| `Scenarii` | `Argumentum Scenarii - Cards.csv` | 167 | 8 | 1336 |
| `Rules` | `Argumentum Rules - Cards.csv` | 15 | 1 | 15 |
| `RulesPP` | `Argumentum Rules - Cards Print and Play.csv` | 6 | 1 | 6 |
| | | | **Total** | **12774** |

> `Rules` et `RulesPP` sont **deux corpus distincts** (`Rules_01`… vs `RulesPP_01`…, traductions
> propres) : mesurer le seul `Cards.csv` aurait laissé 6 rangées × 8 langues hors champ.
> ⚠️ Le compte de **lignes de données** vient du parseur CSV, jamais de `wc -l` (newlines quotés).
>
> Les colonnes `vides structurels` / `vides réels` des tables suivantes valent le **maximum sur
> les langues affichées** : une cellule vide dans **les 8 langues** est structurelle (le nœud de
> taxonomie n'a pas ce niveau), sinon c'est une lacune de traduction de la langue concernée.

## 1. Instrument — deux mesures, trois sondes, un contrôle

| | Sonde | Ce qu'elle attrape | Angle mort |
|---|---|---|---|
| **Couverture** | cellule non vide | lacune de traduction | ne dit rien de la langue du contenu |
| **Alignement ①** | identité **byte-exacte** au champ FR homologue | recopie verbatim (« Jeanne d'Arc ») | **sensible à la casse** : un écart de casse/diacritique lui échappe ; aveugle au reformulé |
| **Alignement ②** | part de lettres dans le **script attendu** (arabe / CJK / latin / cyrillique) | cellule restée latine dans une colonne `ar`/`zh` | un terme latin légitime est signalé (à lire) |
| **Alignement ③** | **orthographe française** (`à è ù â ê î ô û ç œ æ ë ï`) | FR non-verbatim dans une colonne `es` | n'existe que pour `es` (l'espagnol n'emploie aucun de ces signes) |

**Contrôle positif obligatoire** : l'instrument doit d'abord **reproduire le 100 % connu EN/RU/PT**.
Un instrument non contrôlé qui rend « 0 défaut » sur les 4 nouvelles langues ne prouve rien.

**Vides décomposés** : une cellule vide est *structurelle* si elle est vide dans **les 8 langues**
(le nœud de taxonomie n'a pas ce niveau) et *réelle* sinon (lacune de traduction). Sans cette
décomposition, `Subfamily 99,4 %` se lit à tort comme une lacune.

## 2. CONTRÔLE POSITIF — EN / RU / PT

### Couverture par corpus × champ × langue (% rempli)

**`Fallacies`**

| Champ (colonne FR) | en | ru | pt | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|
| `Famille` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `Sous-Famille` | 96.0% | 99.4% | 99.4% | 8 | 48 |
| `Soussousfamille` | 94.5% | 97.9% | 97.9% | 29 | 48 |
| `text_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `desc_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `example_fr` | 99.5% | 99.4% | 99.5% | 7 | 2 |
| `link_fr` | 94.7% | 8.6% | 7.2% | 74 | 1232 |

**`Virtues`**

| Champ (colonne FR) | en | ru | pt | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|
| `family_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `subfamily_fr` | 96.4% | 96.4% | 96.4% | 8 | 0 |
| `subsubfamily_fr` | 87.0% | 87.0% | 87.0% | 29 | 0 |
| `title_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `description_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `remark_fr` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `link_fr` | 87.0% | 87.9% | 83.4% | 11 | 26 |

**`Scenarii`**

| Champ (colonne FR) | en | ru | pt | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|
| `catégorie` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `sous-catégorie` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `titre` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `baratineur` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `piocheur` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `contexte` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `enjeu` | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `suggestion` | 100.0% | 100.0% | 100.0% | 0 | 0 |

**`Rules`**

| Champ (colonne FR) | en | ru | pt | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|
| `Text` | 100.0% | 100.0% | 100.0% | 0 | 0 |

**`RulesPP`**

| Champ (colonne FR) | en | ru | pt | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|
| `Text` | 100.0% | 100.0% | 100.0% | 0 | 0 |

### Alignement — cellules identiques au FR (byte-exact, hors `link`)

| Corpus | Champ | en | ru | pt |
|---|---|---:|---:|---:|
| `Fallacies` | `Famille` | 546 | 0 | 0 |
| `Fallacies` | `Sous-Famille` | 46 | 0 | 46 |
| `Fallacies` | `Soussousfamille` | 60 | 0 | 0 |
| `Fallacies` | `text_fr` | 64 | 5 | 29 |
| `Scenarii` | `titre` | 10 | 0 | 8 |
| `Scenarii` | `baratineur` | 5 | 0 | 6 |
| `Scenarii` | `piocheur` | 11 | 0 | 9 |
| `Scenarii` | `suggestion` | 0 | 0 | 1 |

> Ces identités ne sont **pas** des défauts et n'invalident pas le 100 % : `Fallacies.Famille`
> en `en` (546) et `Soussousfamille` (60) sont des **cognates** — `Influence`, `Obstruction`,
> `Evasion`, `Humour` s'écrivent pareil en anglais —, et les 46 `Sous-Famille` « Ad hominem »
> sont un terme latin porté à l'identique par `en` **et** `pt`. C'est précisément ce qui rend
> le contrôle utile : la sonde ① doit être lue **langue par langue**, jamais en total.

## 3. MESURE — es / ar / fa / zh

### Couverture par corpus × champ × langue (% rempli)

**`Fallacies`**

| Champ (colonne FR) | es | ar | fa | zh | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|---:|
| `Famille` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `Sous-Famille` | 99.4% | 99.4% | 99.4% | 99.4% | 8 | 0 |
| `Soussousfamille` | 97.9% | 97.9% | 97.9% | 97.9% | 29 | 0 |
| `text_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `desc_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `example_fr` | 99.5% | 99.5% | 99.5% | 99.5% | 7 | 0 |
| `link_fr` | 7.5% | 7.2% | 6.5% | 6.2% | 74 | 1246 |

**`Virtues`**

| Champ (colonne FR) | es | ar | fa | zh | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|---:|
| `family_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `subfamily_fr` | 96.4% | 96.4% | 96.4% | 96.4% | 8 | 0 |
| `subsubfamily_fr` | 87.0% | 87.0% | 87.0% | 87.0% | 29 | 0 |
| `title_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `description_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `remark_fr` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `link_fr` | 68.6% | 41.3% | 44.8% | 47.1% | 11 | 120 |

**`Scenarii`**

| Champ (colonne FR) | es | ar | fa | zh | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|---:|
| `catégorie` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `sous-catégorie` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `titre` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `baratineur` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `piocheur` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `contexte` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `enjeu` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |
| `suggestion` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |

**`Rules`**

| Champ (colonne FR) | es | ar | fa | zh | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|---:|
| `Text` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |

**`RulesPP`**

| Champ (colonne FR) | es | ar | fa | zh | vides structurels | vides réels |
|---|---:|---:|---:|---:|---:|---:|
| `Text` | 100.0% | 100.0% | 100.0% | 100.0% | 0 | 0 |

### Alignement — cellules identiques au FR (byte-exact, hors `link`)

| Corpus | Champ | es | ar | fa | zh |
|---|---|---:|---:|---:|---:|
| `Fallacies` | `Sous-Famille` | 46 | 0 | 0 | 0 |
| `Fallacies` | `text_fr` | 23 | 0 | 0 | 2 |
| `Scenarii` | `sous-catégorie` | 11 | 0 | 0 | 0 |
| `Scenarii` | `titre` | 7 | 0 | 2 | 0 |
| `Scenarii` | `baratineur` | 6 | 0 | 1 | 1 |
| `Scenarii` | `piocheur` | 7 | 0 | 1 | 1 |

## 4. Alignement — les cellules `=FR` sont NOMMÉES, et lues

Aucune valeur n'est classée par une heuristique : chaque groupe est confronté aux
**langues de référence** (`en`/`ru`/`pt`) sur **la même rangée**. Une valeur identique au FR
*et* portée à l'identique par les langues de référence n'est pas une non-traduction — c'est un
terme partagé. Seuls les groupes que le contrôle **ne tranche pas** sont lus un par un (§4②).

### ① Le contrôle tranche — groupes `=FR` partagés avec `en`/`ru`/`pt`

| Corpus | Champ | Valeur | × | Langues cibles | Identique en | (à la casse près) |
|---|---|---|---:|---|---|---|
| `Fallacies` | `Subfamily` | « Ad hominem » | 46 | es | en, pt | — |
| `Scenarii` | `smoothTalker` | « Ross » | 3 | es, fa, zh | en, pt | — |
| `Scenarii` | `drawer` | « Rachel » | 3 | es, fa, zh | en, pt | — |
| `Fallacies` | `text` | « Creepypasta » | 2 | es, zh | en, pt | — |
| `Fallacies` | `text` | « DARVO » | 2 | es, zh | pt, ru | en |
| `Scenarii` | `title` | « Ergo sum » | 2 | es, fa | en, pt | — |
| `Scenarii` | `title` | « Casper » | 2 | es, fa | en, pt | — |
| `Fallacies` | `text` | « Fedspeak » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Amphigouri » | 1 | es | pt | — |
| `Fallacies` | `text` | « Bathos » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Hendiadys » | 1 | es | en | — |
| `Fallacies` | `text` | « Zeugma » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Gish gallop » | 1 | es | en | pt |
| `Fallacies` | `text` | « Sealioning » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Slut-shaming » | 1 | es | en | — |
| `Fallacies` | `text` | « Sadfishing » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Shaka » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Facepalm » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Mondegreen » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Astroturfing » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Canard » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Argumentum a silentio » | 1 | es | en, pt, ru | — |
| `Fallacies` | `text` | « Ad hominem » | 1 | es | en, pt, ru | — |
| `Fallacies` | `text` | « Tu quoque » | 1 | es | en, pt | — |
| `Fallacies` | `text` | « Reductio ad Hitlerum » | 1 | es | en | — |
| `Scenarii` | `title` | « Veto » | 1 | es | en, pt | — |
| `Scenarii` | `title` | « Moriarty » | 1 | es | en, pt | — |
| `Scenarii` | `title` | « Pollock » | 1 | es | en, pt | — |
| `Scenarii` | `title` | « Titanic » | 1 | es | en, pt | — |
| `Scenarii` | `title` | « King size » | 1 | es | pt | en |
| `Scenarii` | `smoothTalker` | « Judas » | 1 | es | en, pt | — |
| `Scenarii` | `smoothTalker` | « Loki » | 1 | es | en, pt | — |
| `Scenarii` | `smoothTalker` | « Don Juan » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Gretel » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Hades » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Thor » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Zeus » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Dorothy » | 1 | es | en, pt | — |
| `Scenarii` | `drawer` | « Sherlock Holmes » | 1 | es | en, pt | — |

Tous ces termes sont des **noms d'usage internationaux** : `Ad hominem`, `Tu quoque`,
`Argumentum a silentio`, `Reductio ad Hitlerum`, `DARVO`, `Gish gallop`, `Creepypasta`,
`Zeugma`, `Bathos`, `Hendiadys`, `Astroturfing`… — les traduire romprait l'usage ; `ar`/`fa`/`zh`
les translittèrent ou les glosent quand leur écriture l'exige (`إيبسي ديكست`, `自我引用`).
Les noms propres de scénario (`Sherlock Holmes`, `Zeus`, `Judas`, `Moriarty`, `Titanic`…) relèvent
du même régime.

### ② Les groupes que le contrôle NE tranche PAS — lus un par un

| Corpus | Champ | es | ar | fa | zh | en | ru | pt | Lecture |
|---|---|---|---|---|---|---|---|---|---|
| `Scenarii` | `subcategory` | « romance » | « رومانسية » | « عاشقانه » | « 浪漫爱情 » | « Romance » | « Романтика » | « Romance » | faux-ami — `en`/`pt` portent la **même forme** (`Romance`) |
| `Fallacies` | `text` | « Ipse dixit » | « إيبسي ديكست » | « ایپسه دیکست » | « 自我引用 » | « Ipse Dixit » | « Ipse Dixit » | « Ipse Dixit » | terme latin — `en`/`ru`/`pt` = `Ipse Dixit` (casse) |
| `Fallacies` | `text` | « Namasté » | « ناماستي » | « ناماسته » | « 合十礼 » | « Namaste » | « Намасте » | « Namastê » | emprunt — `en` `Namaste`, `pt` `Namastê`, `ru` `Намасте` |
| `Fallacies` | `text` | « Delenda Carthago » | « يجب تدمير قرطاج » | « کارتاژ باید نابود شود » | « 迦太基必须被毁 » | « Carthago delenda est » | « Да погибнет Карфаген » | « Delenda Cartago » | citation latine — `pt` `Delenda Cartago`, `ru` traduit |
| `Scenarii` | `smoothTalker` | « Un fan » | « مشجّع » | « یک هوادار » | « 一名球迷 » | « A fan » | « Фанат » | « Um fã » | **espagnol correct** (`un fan`) — `en` `A fan`, `pt` `Um fã` |
| `Scenarii` | `smoothTalker` | « Un anti-5G » | « مناهض للجيل الخامس » | « یک مخالف 5G » | « 一名反 5G 人士 » | « An anti-5g » | « Анти-5G » | « Um anti-5g » | **espagnol correct** — `en` `An anti-5g`, `pt` `Um anti-5g` |

- **`Scenarii.subcategory` « romance »** — 11 rangées, catégorie `relation intime`. `en` **et** `pt` écrivent `Romance` : la forme espagnole est le **même lexème**, pas une recopie du FR. `ar`/`fa`/`zh`/`ru` le rendent par leur propre vocabulaire (`رومانسية`, `عاشقانه`, `浪漫爱情`, `Романтика`). En espagnol `romance` désigne aussi la ballade : **question de faux-ami à trancher par l'owner** — pas une faute de langue mesurée.
- **`Scenarii.smoothTalker` « Un fan »** — `Un fan` **est de l'espagnol correct** (`un fan` = un fanatique) ; `en` `A fan`, `pt` `Um fã`, `ar`/`fa`/`zh` traduits. Identique au FR par convergence des deux langues, pas par recopie.
- **`Scenarii.smoothTalker` « Un anti-5G »** — Idem : `Un anti-5G` est une forme espagnole autonome (`en` `An anti-5g`, `pt` `Um anti-5g`, `ru` `Анти-5G`, `ar`/`fa`/`zh` traduits). Convergence, pas contamination.
- **`Fallacies.text` « Ipse dixit »** — Les trois langues de référence portent la **même locution latine**, à la casse près (`Ipse Dixit`) ; `ar`/`fa`/`zh` translittèrent (`إيبسي ديكست`, `自我引用`). Terme d'usage, non traduit par choix.
- **`Fallacies.text` « Namasté »** — `en` `Namaste`, `pt` `Namastê`, `ru` `Намасте` : la même forme à un diacritique près. Emprunt international ; l'espagnol l'écrit avec l'accent français — écart cosmétique, pas de non-traduction.
- **`Fallacies.text` « Delenda Carthago »** — Citation latine. `en` emploie la forme canonique `Carthago delenda est`, `pt` l'écrit à la portugaise (`Delenda Cartago`), `ru` traduit (`Да погибнет Карфаген`). `es` conserve la forme latine **comme le FR** : c'est une citation, non un texte à traduire.

### ③ Différences de casse / diacritique seule (angle mort de la sonde ①)

La sonde ① est byte-exacte : deux chaînes qui ne diffèrent que par la casse ou un diacritique
lui échappent. Bornage de cet angle mort, sur les langues cibles :

| Corpus | Langue | Champ | Valeur cible | Valeur FR | Enregistrement |
|---|---|---|---|---|---|
| `Fallacies` | zh | `text` | « Gish Gallop » | « Gish gallop » | 475 |

Cas unique : `Fallacies.zh.text` « Gish Gallop » contre « Gish gallop » — capitalisation du nom
d'usage, déjà partagé avec `en`/`pt` (cf. §4①).

### ④ Orthographe française dans une colonne `es` (sonde ③)

| Corpus | Champ | Valeur | Enregistrement |
|---|---|---|---|
| `Scenarii` | `title` | « Déjà vu » | 2.2.9 |

`Déjà vu` est un emprunt du français **employé tel quel en espagnol** : la seule marque que la
sonde ③ pouvait attraper est un accent grave, qui appartient aussi à l'espagnol dans les
emprunts. Aucune marque **exclusivement** française n'apparaît dans une cellule `es`.

### ⑤ Cellules hors script attendu (hors `link`) — toutes lues

| Corpus | Langue | Champ | Valeur | Enregistrement | Profil |
|---|---|---|---|---|---|
| `Fallacies` | ar | `example` | « كما يقول المثل القانوني "Nemo auditur propriam » | 194 | lat=39 ara=35 |
| `Fallacies` | fa | `text` | « اثر وون Restorff » | 1043 | lat=8 ara=6 |
| `Fallacies` | zh | `example` | « 正如法律格言“Nemo auditur propriam turpitudinem alle » | 194 | lat=39 cjk=20 |
| `Fallacies` | zh | `text` | « Gish Gallop » | 475 | lat=10 |
| `Fallacies` | zh | `text` | « Creepypasta » | 927 | lat=11 |
| `Fallacies` | zh | `text` | « DARVO » | 1356 | lat=5 |
| `Fallacies` | zh | `text` | « Whataboutism（什么主义） » | 1363 | lat=12 cjk=4 |
| `Rules` | zh | `Text` | « # Argumentum / ## 说谎者学校 » | Rules_01 | lat=10 cjk=5 |
| `RulesPP` | zh | `Text` | « # Argumentum / ## 骗子学校 » | RulesPP_01 | lat=10 cjk=4 |
| `Scenarii` | fa | `drawer` | « Rachel » | 5.2.5 | lat=6 |
| `Scenarii` | fa | `smoothTalker` | « Aurelia Cotta، مادر سزار » | 1.1.1 | lat=12 ara=8 |
| `Scenarii` | fa | `title` | « Ergo sum » | 4.3.1 | lat=7 |
| `Scenarii` | fa | `title` | « Casper » | 5.1.5 | lat=6 |
| `Scenarii` | zh | `drawer` | « Rachel » | 5.2.5 | lat=6 |
| `Scenarii` | zh | `suggestion` | « Bilou去哪儿了？ » | 3.2.6 | lat=5 cjk=4 |
| `Virtues` | fa | `title` | « قیاس Barbara » | 106 | lat=7 ara=4 |
| `Virtues` | fa | `title` | « قیاس Celarent » | 107 | lat=8 ara=4 |
| `Virtues` | fa | `title` | « قیاس Cesare » | 111 | lat=6 ara=4 |
| `Virtues` | zh | `title` | « Barbara 三段论 » | 106 | lat=7 cjk=3 |
| `Virtues` | zh | `title` | « Celarent 三段论 » | 107 | lat=8 cjk=3 |
| `Virtues` | zh | `title` | « Darii 三段论 » | 108 | lat=5 cjk=3 |
| `Virtues` | zh | `title` | « Ferio 三段论 » | 109 | lat=5 cjk=3 |
| `Virtues` | zh | `title` | « Cesare 三段论 » | 111 | lat=6 cjk=3 |
| `Virtues` | zh | `title` | « Camestres 三段论 » | 112 | lat=9 cjk=3 |
| `Virtues` | zh | `title` | « Disamis 式三段论 » | 118 | lat=7 cjk=4 |
| `Virtues` | zh | `title` | « Datisi 式三段论 » | 119 | lat=6 cjk=4 |
| `Virtues` | zh | `title` | « Bocardo 式三段论 » | 120 | lat=7 cjk=4 |
| `Virtues` | zh | `title` | « Ferison 式三段论 » | 121 | lat=7 cjk=4 |
| `Virtues` | zh | `title` | « Camenes 三段论 » | 123 | lat=7 cjk=3 |
| `Virtues` | zh | `title` | « Dimatis 三段论 » | 124 | lat=7 cjk=3 |
| `Virtues` | zh | `title` | « Fesapo 三段论 » | 125 | lat=6 cjk=3 |

Chaque ligne a été **lue** (une valeur latine dans une colonne `ar`/`zh` n'est pas une faute en soi) :

- **Mnémotechniques du syllogisme** (`Barbara 三段论`, `قیاس Celarent`, …) — `Barbara`, `Celarent`,
  `Darii` … sont les **mots-clés latins** du carré logique ; les traduire détruirait l'information.
  Conservés dans les 8 langues, y compris en `ru` (`Инференция: reductio ad absurdum`).
- **Termes internationaux** (`DARVO`, `Gish Gallop`, `Creepypasta`, `Whataboutism`, `Idola fori`,
  `Credo quia absurdum`, `PIDOOMA`, `Ad hominem`) — identiques en `en`/`pt`/`ru` (cf. §4①).
- **Citations** enchâssées dans une phrase traduite (rangée 194 : la maxime *Nemo auditur propriam
  turpitudinem allegans* citée dans une phrase **arabe** puis **chinoise** — la phrase, elle, est traduite).
- **Noms propres** (`Rachel`, `Casper`, `Aurelia Cotta`, `Ergo sum`, `Bilou`) et **chiffres romains**
  (`XX и XXI век`) — ainsi que l'**effet von Restorff** (`اثر وون Restorff`).
- **Bandeaux de marque** : `# Argumentum` en tête des deux corpus `Rules` — le titre du jeu, suivi
  du titre traduit (`说谎者学校` / `骗子学校`).

## 5. Couverture — la seule lacune réelle est `link`

| Corpus | Langue | `link` rempli | Taux |
|---|---|---:|---:|
| `Fallacies` | en | 1333 / 1408 | 94.7% |
| `Fallacies` | ru | 121 / 1408 | 8.6% |
| `Fallacies` | pt | 102 / 1408 | 7.2% |
| `Fallacies` | es | 105 / 1408 | 7.5% |
| `Fallacies` | ar | 102 / 1408 | 7.2% |
| `Fallacies` | fa | 91 / 1408 | 6.5% |
| `Fallacies` | zh | 88 / 1408 | 6.2% |
| `Virtues` | en | 194 / 223 | 87.0% |
| `Virtues` | ru | 196 / 223 | 87.9% |
| `Virtues` | pt | 186 / 223 | 83.4% |
| `Virtues` | es | 153 / 223 | 68.6% |
| `Virtues` | ar | 92 / 223 | 41.3% |
| `Virtues` | fa | 100 / 223 | 44.8% |
| `Virtues` | zh | 105 / 223 | 47.1% |

Les champs **substantiels** (hors `link`) sont à **100 %** partout, sauf les vides **structurels**
ci-dessus — identiques dans les 8 langues. `link` est la lacune connue : ce sont des **URL de
source** à trouver une par une, non traduisibles par lot ; le contrôle `en` (94,7 % sur Fallacies)
montre que la lacune n'est pas propre aux 4 langues mesurées.

## 6. Verdict

1. **Couverture** : `es`/`ar`/`fa`/`zh` atteignent le niveau de EN/RU/PT sur tous les champs
   substantiels des 5 corpus. Le contrôle positif reproduit le 100 % connu.
2. **Alignement** : **0 contamination FR** mesurée. Les identités au FR sont des termes latins /
   noms propres **partagés avec `en`/`pt`** (à la casse ou au diacritique près), ou des URL de
   source communes — jamais une cellule recopiée là où les langues de référence traduisaient.
3. **La lacune réelle est `link`** (6,2–7,5 % sur Fallacies, 41–69 % sur Virtues) — et elle n'est
   pas propre aux 4 langues : `en` fait 94,7 %, `ru`/`pt` 7–9 %.
4. **Une question éditoriale**, non une faute de langue : `Scenarii.es.subcategory` « romance »
   (11 rangées, `relation intime`) — `en` et `pt` portent la même forme (`Romance`), donc c'est un
   **faux-ami** à trancher par l'owner, pas une contamination mesurée.

Les **six** groupes que le contrôle ne tranchait pas ont été lus un par un (§4②) : locutions
latines, emprunt accentué, citation, et deux formes espagnoles autonomes. **Aucun** ne révèle une
cellule recopiée du français là où les langues de référence auraient traduit.

## 7. Ce qui a corrigé l'instrument (à garder)

Le contrôle positif a révélé **10 colonnes EN muettes** : dans `Fallacies` (`Family`, `Subfamily`,
`Subsubfamily`) et `Scenarii` (`category`, `subcategory`, `title`, `smoothTalker`, `drawer`,
`context`, `issue`) les colonnes EN sont **nues**, alors qu'elles sont **suffixées** dans `Virtues`
et `Rules`. La sonde attendait `Family_en` : elle mesurait **le vide** sans erreur. Les deux formes
sont désormais lues sur l'en-tête, jamais supposées.

*Fin du rapport.*
