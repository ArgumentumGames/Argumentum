# #986 (C1) — Pré-draft : trois postures pour le principe générateur du deck Vertus

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `cea6e699`
**Grain** : pool #458 v7 ⑥ · **Statut** : **pré-draft — ⛔ aucune posture n'est retenue ici**

---

## Ce que ce document est, et ce qu'il n'est pas

C'est le **matériau** de la note de cadrage C1 : les trois postures défendables, chiffrées, avec ce que chacune coûte et ce que chacune interdit. C'est écrit pour que Thomas puisse **amender ou trancher**, pas pour conclure à sa place.

Ce n'est **pas** la note C1. La DoD de #986 demande une note **co-signée** énonçant le principe retenu ; ce document ne retient rien. Les cinq sous-issues C2–C5 restent non démarrées (`#980`).

**Un chiffre du corps de #980 est corrigé ici**, et il change le coût d'une des postures : voir §2.b. Il ne change pas la prémisse de l'Epic.

---

## 1. L'objection, dans sa forme forte

> « Je ne crois pas du tout que ce soit une bonne idée de fonctionner sur le parallélisme des arguments fallacieux. »
> — Thomas Watanabe Vermorel, 2026-08-04 (cité dans #980)

Reformulée : un sophisme est un défaut dans un **coup** inférentiel. La négation d'un défaut n'est pas une vertu, c'est une **ligne de base** — ne pas commettre d'*ad hominem* n'est pas vertueux, c'est l'absence de faute. Dans la littérature d'argumentation vertueuse (**Aberdein** ; **Cohen** ; **Bowell & Kingsbury**), les vertus argumentatives sont des **dispositions de l'argumentateur** — humilité intellectuelle, charité, courage, disposition à réviser — et non des propriétés de coups isolés.

---

## 2. Ce que la mesure ajoute, et qu'aucune des deux parties n'avait

### 2.a — Les titres des 131 cartes ne sont pas ce que l'objection décrit

Recensement des 131 `title_fr` du deck Vertus (`card=1`), par instrument lexical déclaré (§7) :

| Bucket | Ce que le titre désigne | Cartes | Part |
|---|---|---:|---:|
| **S** — standard | propriété de **l'argument** (« Indépendance des prémisses », « Preuves tangibles ») | **58** | 44,3 % |
| **D** — disposition | état ou acte de **la personne** (« Honnêteté intellectuelle », « Écoute active », « Reconnaître ses erreurs ») | **41** | 31,3 % |
| **?** — indécidable | l'instrument ne tranche pas (« Présentation intègre », « Universalisme », « Rhétorique éloquente ») | **24** | 18,3 % |
| **P** — principe nommé | ni personne ni produit (« Principe de charité », « Rasoir d'Ockham ») | **5** | 3,8 % |
| **N** — négation d'un défaut | forme prohibitive (« Ne pas interrompre », « Éviter les attaques personnelles », « Sans chantage aux conséquences ») | **3** | 2,3 % |

**La négation d'un défaut, prise littéralement, c'est 3 cartes sur 131 — 2,3 %.** Et l'exemple même de Thomas y est : **« Éviter les attaques personnelles »** est l'une des trois. Son exemple n'est pas une hypothèse sur le deck, c'est une de ses cartes.

### 2.b — ⚠️ Le niveau relationnel, lui, EST bâti par le miroir — mais moins exclusivement que #980 ne l'écrit

Le corps de #980 porte un tableau où les vertus sont à **0 %** sur `Leverages`, `Mirrors` et `IsRelatedTo`. Le commentaire du 14/09 signalait déjà que ces pourcentages n'avaient **pas** été re-mesurés. Re-mesurés le 21/09 sur les **223 nœuds** (et sur les 131 cartes) :

| Colonne `crossLink_*` | 223 nœuds | 131 cartes | Corps de #980 |
|---|---:|---:|---|
| `Opposes` | **222 (99,6 %)** | 130 (99,2 %) | 99,6 % ✅ |
| `IsRelatedTo` | **56 (25,1 %)** | 39 (29,8 %) | ~~0 %~~ ❌ |
| `Leverages` | **55 (24,7 %)** | 31 (23,7 %) | ~~0 %~~ ❌ |
| `Allows` | **23 (10,3 %)** | 16 (12,2 %) | ~~0 %~~ ❌ |
| `Mirrors` · `Inverts` · `PredatesOn` · `Denounces` | 0 | 0 | 0 % ✅ |

**Ce qui reste vrai, et qui est le point de Thomas** : `Opposes` est la **seule relation universelle** (222/223), et aucune des quatre colonnes restantes n'atteint 26 %. Le miroir est bien le principe générateur **de la structure** — mais il n'est **pas la seule relation renseignée**, et la sous-issue **C3** (#988), dont l'énoncé est « doter les vertus d'un réseau relationnel propre (`Leverages`/`IsRelatedTo`/`Allows`, **aujourd'hui à 0 %**) », part aujourd'hui d'une prémisse **fausse d'un facteur ≈ 25 %**. Son coût est **plus faible** que prévu : il ne s'agit pas de créer un réseau absent, mais de **réduire la part d'`Opposes`** face à des relations qui existent déjà.

### 2.c — Le miroir, chiffré : il atteint 33 % du deck Sophismes, sans orphelin

Les 130 cartes vertus portant un `Opposes` totalisent **259 arêtes** vers **58 cartes Sophismes distinctes** — toutes dans le paquet imprimé, aucune arête pendante.

| Mesure | Valeur |
|---|---:|
| Cartes Vertus avec au moins une arête `Opposes` | **130 / 131** (99,2 %) |
| Arêtes `Opposes` | **259** |
| Cartes **Sophismes** visées (distinctes) | **58 / 175** — **33,1 %** du deck Sophismes |
| Arêtes par carte Vertus (moyenne) | 1,99 |
| Vertus convergeant sur une même carte Sophisme (max) | **15** (PK 953 « Attention sélective ») |

Répartition par famille de sophismes — **aucune famille n'est orpheline** :

| Famille Sophismes | Cartes visées / total | Part |
|---|---:|---:|
| Abus de langage | 9/23 | 39,1 % |
| Obstruction | 9/24 | 37,5 % |
| Erreur de raisonnement | 9/24 | 37,5 % |
| Tricherie | 9/25 | 36,0 % |
| Erreur mathématique | 8/25 | 32,0 % |
| Influence | 8/30 | 26,7 % |
| Insuffisance | 6/24 | 25,0 % |

⇒ Le bénéfice pédagogique du miroir est **réel et uniforme** — mais son **étendue est 33 %**, pas la moitié, et il est **très inégalement réparti côté vertus** : 15 vertus différentes pointent la même carte Sophisme, si bien que « connaître cette carte » ne prédit **rien d'utile** sur laquelle des 15 vertus s'applique. C'est cette non-injectivité — déjà notée dans #980 comme propriété de forme, **chiffrée ici** — que la posture retenue devra remplacer, pas seulement conserver.

---

## 3. Le déplacement que la mesure impose

L'objection de Thomas est **exacte sur le graphe** et **inexacte sur les cartes**.

- Sur les **cartes** : 58 standards, 41 dispositions, 24 indécidables, 5 principes, **3 négations**. Le deck n'est pas engendré par négation — il est **déjà hétérogène**, et majoritairement composé de standards (44 %) plutôt que de dispositions (31 %).
- Sur le **graphe** : une seule relation universelle, et c'est l'opposition (99,6 %) ; le miroir n'est pas injectif (130 cartes → 58 cibles).

⇒ La question de C1 n'est donc **pas** « dispositions **ou** standards » comme choix global — le deck **est déjà** un mélange non déclaré. La question réelle est : **quel principe unique (ou quel jeu de types déclarés) rend ce mélange légitime et son critère d'inclusion applicable carte par carte ?** C'est plus étroit que #980 ne le formule, et c'est plus tranchable.

**Plancher commun aux trois postures** : les **3 cartes `N`** sont condamnées par les trois — elles sont, littéralement, l'absence de faute que Thomas décrit. Aucune posture ne les sauve. Le débat ne porte donc que sur **128 cartes**, et il est déjà gagné sur 3.

---

## 4. Les trois postures

Chaque posture est présentée avec : son énoncé · son critère d'inclusion · **ce qu'elle coûte** (chiffré sur le corpus) · **ce qu'elle interdit** · ce qu'elle laisse intact · comment elle remplace le bénéfice pédagogique · la question qu'elle laisse ouverte.

### Posture A — Le deck décrit des **dispositions** (traits de l'argumentateur)

**Énoncé.** Une carte Vertus nomme un trait ou un acte de **la personne qui argumente**. Le deck Vertus n'est pas le miroir du deck Sophismes ; il est un portrait de l'argumentateur.

**Critère d'inclusion.** Le titre doit pouvoir se prédire d'**une personne** : « *il/elle est …* », ou se lire comme un acte de l'argumentateur. Un titre qui ne se prédit que de l'argument est hors sujet.

**Ce qu'elle coûte.** Toute carte dont le titre ne se prédit pas d'une personne sort : **58 `S`** + **5 `P`** = **63 titres à réécrire (48,1 % du deck)**, plus les **24 indécidables** à arbitrer. Sur 8 langues, cela touche les colonnes `title_*` **et** `description_*`/`remark_*` des 63 cartes — le contenu rédactionnel (223/223 rempli et traduit, cf. #980) est **en grande partie conservé**, seule la couche de titrage est refaite.

**Ce qu'elle interdit.** Les cartes-faites qui portent l'identité actuelle du deck : « Syllogisme valide », « Indépendance des prémisses », « Rasoir d'Ockham », « Échantillonnage représentatif », « Validité formelle ». Elle interdit aussi de présenter le deck comme un **cours de logique** — c'est précisément le reproche de catégorie que Thomas adresse au miroir.

**Ce qu'elle laisse intact.** Les 41 `D`, la totalité du contenu rédactionnel, le rendu (25 classes CSS, 0 famille manquante).

**Remplacement du bénéfice pédagogique.** Le pont avec les sophismes n'est plus « la négation de X » mais « **le défaut X survient quand la disposition Y manque** » : on garde 7 familles de sophismes comme **diagnostic**, et chaque carte vertu devient la **réponse** au défaut. Le joueur conserve la moitié du parcours, dans l'autre sens.

**Question laissée ouverte.** Que devient « Rasoir d'Ockham »/« Rasoir de Hanlon » — principe méthodologique sans porteur ? Et la famille `Honnêteté intellectuelle` contient 5 cartes sur les biais culturels/idéologiques : disposition, ou contenu de savoir ?

---

### Posture B — Le deck décrit des **standards** (propriétés d'un bon argument)

**Énoncé.** Une carte Vertus nomme une propriété que **l'argument** doit avoir. Assumé et déclaré, ce principe est cohérent : le deck devient un référentiel de qualité argumentative, à côté du référentiel de défauts.

**Critère d'inclusion.** Le titre doit pouvoir se prédire de **l'argument** ou de son produit (texte, inférence, présentation).

**Ce qu'elle coûte.** **41 `D`** + **3 `N`** = **44 titres à réécrire (33,6 % du deck)** — le coût le plus faible des deux postures « pures » — plus les 24 indécidables.

**Ce qu'elle interdit — et c'est le point dur.** Elle **exclut la littérature que Thomas cite pour fonder son objection**. « Honnêteté intellectuelle », « Écoute active », « Empathie », « Ouverture au dialogue », « Reconnaître ses erreurs », « Principe de charité », « Courtoisie dans le désaccord », « Suspension du jugement » — soit **une trentaine de cartes** — sont des dispositions de personne, exactement ce qu'Aberdein / Cohen / Bowell & Kingsbury décrivent. Choisir B, c'est **accepter l'écart de catégorie** que Thomas dénonce, en le déclarant plutôt qu'en le niant.

**Ce qu'elle laisse intact.** Les 58 `S`, le contenu rédactionnel, le rendu.

**Remplacement du bénéfice pédagogique.** Le plus direct : le miroir reste lisible tel quel (chaque défaut a son standard en vis-à-vis), et la non-injectivité cesse d'être un défaut — deux vertus peuvent porter sur le même sophisme si leurs standards diffèrent.

**Question laissée ouverte.** Si le deck ne porte plus aucune disposition, quelle carte porte « l'argumentateur » dans le jeu ? Le deck Vertus devient un **référentiel**, plus un **portrait** — est-ce le produit voulu ?

---

### Posture C — **Deux couches, déclarées**

**Énoncé.** Le deck Vertus porte **deux types de cartes explicitement typés** : des *dispositions* (traits de l'argumentateur) et des *standards* (propriétés du produit). Le principe générateur n'est ni l'un ni l'autre : c'est **le typage**, et le critère d'inclusion est posé **par type**. C'est la lecture littérale de la formule de #986 : *« Les deux sont défendables — mais pas simultanément sans le dire. »*

**Critère d'inclusion.** Une carte est admise si elle satisfait **le test de l'un des deux types**, et le type retenu est **écrit sur la carte** (colonne de typage).

**Ce qu'elle coûte.** **Zéro réécriture de titre.** Une colonne de typage, l'arbitrage des **24 indécidables**, et le passage des 3 `N` (ou leur reformulation). C'est la posture la moins chère en contenu — et la plus chère en **explicitation** : il faut écrire le test des deux types, et l'appliquer 131 fois.

**Ce qu'elle interdit.** De prétendre qu'**un seul** principe engendre le deck — c'est-à-dire d'affirmer ce qui est déjà faux aujourd'hui. Elle interdit aussi la version silencieuse de C : laisser le deck hétérogène **sans** dire le typage, ce qui est l'état actuel.

**Ce qu'elle laisse intact.** L'intégralité des titres, du contenu rédactionnel et du rendu.

**Remplacement du bénéfice pédagogique.** Le plus explicite : le joueur apprend que le deck a **deux moitiés** — « ce que je fais » et « ce que mon argument doit être ». La famille de sophismes reste le fil de navigation ; le type devient un second axe de lecture.

**Question laissée ouverte.** Un deck à deux types reste-t-il **un** deck ? Le risque est la lisibilité : deux cartes voisines classées dans la même famille peuvent désormais appartenir à deux registres différents, sans que le joueur le voie au premier coup d'œil.

---

## 5. Le test d'inclusion — formulation applicable aux trois postures

Le critère que la note C1 doit publier, quel que soit le choix, tient en une opération vérifiable :

> **Un titre est admis s'il satisfait le prédicat du ou des types retenus :**
> - *disposition* → « … est ⟨titre⟩ » se dit d'**une personne** (« elle est d'une honnêteté intellectuelle… »). Sinon → rejeté ou reformulé.
> - *standard* → « … est ⟨titre⟩ » se dit d'**un argument** ou de son produit (« l'argument est fondé »). Sinon → rejeté ou reformulé.
>
> Un titre **négatif** (« ne pas… », « sans… », « éviter… ») est rejeté **dans les trois postures** : il nomme l'absence d'une faute, pas une qualité.

C'est ce test — et non un ratio — que **C2** (#987) appliquera carte par carte. En posture C, C2 produit en plus **le type** de chaque carte, et non seulement un ratio A/B/C.

---

## 6. Ce que ce document n'établit pas

⛔ **Aucune posture n'est retenue** — c'est le rôle de la note co-signée, et il revient à Thomas.
⛔ **Le classement `D`/`S`/`P`/`N` est lexical, pas sémantique** (§7) : les **24 indécidables** sont rendus tels quels, jamais répartis d'office ; la frontière `D`/`S` est attaquable sur au moins la famille `Honnêteté intellectuelle`.
⛔ **Le coût des postures est un compte de titres, pas un devis de traduction** : les 8 colonnes `title_*` sont à refaire, mais je n'ai pas chiffré le coût de re-traduction par langue.
⛔ **Le séquencement avec #951 n'est pas traité** : `GetId()` est alimenté par `family_en`, donc les libellés de **familles** sont gelés par #951 (38 références d'IRI publiées, cf. #980 c.5198984609). Les **titres** de cartes sont hors de ce gel : ils ne nourrissent pas l'identité publiée. ⇒ Une réécriture de titres **ne réveille pas** #951 ; une réécriture de **familles** si.
⛔ **Le sort des 5 `Principe de …` n'est pas tranché** — dont « Principe de non-disqualification », négatif sous une forme affirmative (limite d'instrument déclarée en §7).
⛔ Le chiffre **33,1 %** mesure la couverture **du deck Sophismes imprimé** (175 cartes), pas de la taxonomie (1408 nœuds).

---

## 7. Instrument, contrôle inverse, mutation falsifiante (§2)

Recensement des `title_fr` des 131 cartes `card=1` de `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`, par **lexique explicite** : préfixes `Principe de…`/`Rasoir d'…` → `P` ; marqueurs `sans`/`ne pas`/`éviter les` → `N` ; tête verbale (`illustrer`, `citer`, `reconnaître`, `gérer`…) → `D` ; sinon tête nominale cherchée dans un lexique `personne` (honnêteté, écoute, empathie, respect, courtoisie, équité…) puis `produit` (preuves, indépendance, définitions, langage, énoncés, cohérence…).

**Contrôle inverse** (le classifieur doit placer quatre cas déclarés où ils sont annoncés) :

```
OK  'Honnêteté intellectuelle'  attendu=D obtenu=D
OK  'Indépendance des prémisses' attendu=S obtenu=S
OK  'Principe de charité'        attendu=P obtenu=P
OK  'Ne pas interrompre'         attendu=N obtenu=N
→ PASS
```

**Mutation falsifiante** (renommer « Principe de charité » → « Charité » doit déplacer exactement un cran de `P` vers `D`) :

```
avant : {S: 58, D: 41, ?: 24, P: 5, N: 3}
après : {S: 58, D: 42, ?: 24, P: 4, N: 3}
→ PASS
```

**Clôture** : somme des buckets = **131** (aucune carte perdue). `crossLink_*` mesuré par remplissage non vide des 8 colonnes, sur 223 nœuds / 131 cartes / 92 non-cartes. Le miroir est mesuré par éclatement des valeurs `crossLink_Opposes` sur `;` et recoupement des PK contre `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` (colonne `PK` **majuscule**) — 0 arête pendante.

---

*Ce document est un **pré-draft** : il est écrit pour être barré, amendé ou remplacé par Thomas. Sa seule affirmation propre est que l'objection de #980 est exacte sur le graphe et inexacte sur les cartes — et que cela déplace la question.*
