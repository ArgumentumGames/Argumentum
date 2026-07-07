# AIF #499 — Virtues Cluster PR-3 : Échange enrichissant (7) / Respect de la personne (7.3)

> **Schéma I/RA/CA ratifié #707§4(a)** — 2 colonnes additives `AIF_attackType` (undermine/undercut/rebut) +
> `AIF_attackedNode` (I-node/RA-node/CA-node). Ce doc étend le travail relationnel 12-col (phase-2,
> `crossLink_Opposes` + `AIF_skos*`, 222/222 nœuds CLEAN #518) avec la **décomposition I/RA/CA** (attack-type +
> attacked-node), couche additive absente du phase-2. Réutilise le grounding validé, ne le redérive pas.
>
> **Paradigme inverse (cadrage jsboige)** : Fallacy = exception à un scheme Walton légitime (scheme défait +
> CQ violée). Virtue = bonne tenue du même scheme / réponse correcte à la même CQ.
>
> **PR-1** (`499-aif-raisonnement-causalite-cluster.md`) : subfamily 4.1 Causalités, 4/4 `undercut`/`RA-node`.
> **PR-2** (`499-aif-honnetete-fidelite-aux-faits-cluster.md`) : subfamily 6.1 Fidélité aux faits, mixte 1 `undermine` + 2 `undercut`.
> **PR-3 (ce doc)** : subfamily 7.3 Respect de la personne, **6/6 `undercut`/`RA-node`** + **finding cumulatif rebut-rarity**.

---

## 1. Cluster — Respect de la personne (path 7.3)

Family 7 *Échange enrichissant* / subfamily 7.3 *Respect de la personne* — anchor pk 208 + leaves 212 / 216
(3 feuilles depth-3, scheme commun **Argument from Commitment**).

| Virtue leaf (path) | pk | Titre FR | Scheme Walton | CQ restaurée (AIF_skosMappingType) | Fallacies opposées (crossLink_Opposes) |
|---|---|---|---|---|---|
| 7.3.1 | 208 | Évaluation loyale de la position adverse | Argument from Commitment | La critique vise-t-elle la position **réellement défendue** par l'interlocuteur ? | 322 Repoussoir · 1398 Attaque personnelle |
| 7.3.2 | 212 | Respect de l'origine des idées | Argument from Commitment | L'origine d'une idée est-elle reconnue **sans remplacer l'évaluation de son contenu** ? | 1371 Sophisme génétique · 942 Fausse attribution |
| 7.3.3 | 216 | Courtoisie dans le désaccord | Argument from Commitment | Le désaccord est-il exprimé de manière **compatible avec la poursuite du dialogue** ? | 1398 Attaque personnelle · 1352 Empoisonnement du puits |

### 1.1 Miroir Fallacies — path-prefix **partiel** (scheme-divergence honnête, #677)

Contrairement à PR-1 (4.1↔4.1 Causalité douteuse) et PR-2 (6.1↔6.1 Arranger les faits) où le path-prefix
Virtues↔Fallaxies était exact, **7.3 est un miroir partiel** : la vertu dialogique *Respect de la personne*
résiste à des fallacies issues de **3 familles** différentes, pas seulement Obstruction :

| Fallacy opposée | path | Famille | Rôle dans le miroir 7.3 |
|---|---|---|---|
| 1398 Attaque personnelle | 7.3.3 | **Obstruction** | anchor-path (7.3↔7.3) — opposée par 208 ET 216 |
| 1371 Sophisme génétique | 7.3.2 | **Obstruction** | anchor-path (7.3↔7.3) |
| 1352 Empoisonnement du puits | 7.2.3 | **Obstruction** | same-family (7.x) |
| 322 Repoussoir | 2.2.2 | **Influence** | cross-family (émotion vs évaluation) |
| 942 Fausse attribution | 6.1.2 | **Tricherie** | cross-family (fausse source — déjà opposée en PR-2 par 155) |

**Lecture** : 4/6 opposites en Obstruction (7.x, miroir primaire path-prefix), 1 Influence (322), 1 Tricherie (942).
La vertu 7.3 est **transversale aux familles de fallacies interpersonnelles** : le *respect de la personne*
est violé par l'obstruction (attaquer la personne), l'influence (rendre l'idée repoussante) et la tricherie
(fausse source pour discréditer). Le path-prefix 7.3↔7.3 tient pour les anchors (1398, 1371) mais les leaves
atteignent légitimement d'autres familles — c'est la structure réelle, pas un moulage forcé.

### 1.2 PK 942 — cross-référence PR-2 (fallacy résistée par 2 vertus)

PK 942 *Fausse attribution* est opposée par **155** (PR-2, *Attribution juste*, Fidélité aux faits 6.1) ET
**212** (PR-3, *Respect de l'origine des idées*, Échange 7.3). Ce n'est pas une redondance : la même fallacy
viole **deux CQ distinctes** de deux schemes différents —
- **155** (Cause to Effect / Witness Testimony) : la source est-elle **fiable** ? → crédibilité épistémique de la preuve.
- **212** (Argument from Commitment) : l'origine est-elle reconnue **sans remplacer l'évaluation du contenu** ? → loyauté dialogique.

La fallacy 942 tombe sous les deux : une fausse source est à la fois épistémiquement non-fiable (155) ET
dialogiquement malhonnête (212). Modéliser les deux résistances préserve l'information au lieu de la forcer
en une seule row. `crossLink_Opposes` porte déjà les deux arcs (validé phase-2).

---

## 2. Décomposition I/RA/CA — attack-type + attacked-node (Option A)

**Rappel Option A (recommandée, PR-1 §3)** : colonnes = attack-type **prévenu** + node **maintenu**.
Une vertu = bonne tenue du scheme → la fallacy tente de défaire un composant AIF que la vertu maintient.
L'attack-type est **dérivé per-case du mécanisme de la fallacy** (cf. §3 PR-2), pas templétée.

### 2.1 Dérivation par leaf

**7.3.1 pk 208 — Évaluation loyale de la position adverse**
Légitime *Argument from Commitment* : l'interlocuteur est engagé envers P, donc P est évaluée sur sa position
réelle (pas une version déformée).
- **322 Repoussoir** (*« discréditez une idée en la rendant repoussante, au lieu de l'examiner rationnellement »*) :
  rend P inacceptable via l'émotion pour court-circuiter l'évaluation. N'attaque ni une prémisse ni une
  conclusion opposée — attaque l'**inférence** (l'engagement rationnel vers l'évaluation). → **undercut / RA-node**.
- **1398 Attaque personnelle** (*« au lieu de réfuter les arguments, vous attaquez directement la personne »*) :
  discrédite l'engagement de l'interlocuteur en l'attaquant lui-même. La taxonomie elle-même le définit comme
  la **négation de la réfutation** → attaque l'inférence engagement→évaluation, pas la conclusion. → **undercut / RA-node**.

**7.3.2 pk 212 — Respect de l'origine des idées**
Légitime *Argument from Commitment* : reconnaît l'origine d'une idée **sans** laisser l'origine remplacer
l'évaluation du contenu.
- **1371 Sophisme génétique** (*« rejetez une idée en critiquant son origine plutôt que son contenu »*) :
  substitue l'origine au contenu comme critère → l'inférence contenu→jugement est court-circuitée par origine→jugement.
  → **undercut / RA-node**.
- **942 Fausse attribution** : voir cross-réf §1.2. Attaque la crédibilité de la source → inférence source→crédibilité.
  → **undercut / RA-node** (cohérent avec classification PR-2 de 942).
  - **Nuance load-bearing** : 212 tient la **ligne de démarcation** entre vérification légitime de l'origine
    (942 disqualifie réellement la source) et dismissal génétique illégitime (1371 dismiss le contenu à tort).
    Même scheme, deux CQ-voisins, deux attack-shapes proches mais distinctes — d'où l'intérêt de modéliser
    les deux arcs séparément plutôt qu'un seul.

**7.3.3 pk 216 — Courtoisie dans le désaccord**
Légitime *Argument from Commitment* : le désaccord est exprimé de manière compatible avec la **poursuite** du dialogue.
- **1398 Attaque personnelle** : voir 208. → **undercut / RA-node**.
- **1352 Empoisonnement du puits** (*« discréditez l'autre partie au moyen d'informations préjudiciables
  afin d'affaiblir sa position »*) : disqualification préventive qui empêche l'engagement dialogique futur.
  Attaque l'inférence engagement→dialogue continu. → **undercut / RA-node**.

### 2.2 Distribution attack-type — cluster 7.3

| Leaf | Fallacy | attackType (Option A) | attackedNode |
|---|---|---|---|
| 208 | 322 Repoussoir | undercut | RA-node |
| 208 | 1398 Attaque personnelle | undercut | RA-node |
| 212 | 1371 Sophisme génétique | undercut | RA-node |
| 212 | 942 Fausse attribution | undercut | RA-node |
| 216 | 1398 Attaque personnelle | undercut | RA-node |
| 216 | 1352 Empoisonnement du puits | undercut | RA-node |

**= 6/6 `undercut` / `RA-node`** — distribution uniforme, comme PR-1. Aucune prémisse factuelle n'est
attaquée (pas d'`undermine`) : les fallacies interpersonnelles ne nient pas les **faits**, elles court-circuitent
l'**inférence engagement→évaluation/dialogue**. Et aucune ne présente de contre-conclusion (pas de `rebut` — §3).

---

## 3. Finding cumulatif — rebut / CA-node est empiriquement rare (load-bearing pour décision Option)

**Signal empirique sur 3 clusters / 3 familles / 3 subfamilies / 9 feuilles / ~14 fallacy-instances :**

| Cluster | Subfamily | Famille | attack-types observés | rebut ? |
|---|---|---|---|---|
| PR-1 | 4.1 Causalités | Raisonnement valide | 6× undercut | **0** |
| PR-2 | 6.1 Fidélité aux faits | Honnêteté intellectuelle | 1× undermine + 2× undercut | **0** |
| PR-3 | 7.3 Respect de la personne | Échange enrichissant | 6× undercut | **0** |
| **Cumul** | **3 subfamilies** | **3 familles** | **1 undermine + 14 undercut** | **0 rebut** |

**`rebut` / `CA-node` n'a été instancié proprement sur AUCUN des 3 clusters.**

### 3.1 Pourquoi structuralement ( Walton AIF )

Un `rebut` AIF exige que l'attaquant présente une **contre-conclusion indépendante** soutenue par son propre
argument (CA-node ↔ conclusion I-node). Or les fallacies sont presque toujours :
- des **erreurs d'inférence** (le scheme ne tient pas) → `undercut` / RA-node ;
- ou des **prémisses fausses / données mauvaises** → `undermine` / I-node ;

rarement des **arguments indépendants pour la conclusion opposée**. PK 1398 *Attaque personnelle* est, dans la
taxonomie elle-même, défini comme *« au lieu de réfuter les arguments »* — soit **explicitement la négation
du rebuttal** : l'ad hominem ÉVITE la réfutation en attaquant la crédibilité (undercut), il ne présente pas
de contre-conclusion.

Les candidats rebut les plus plausibles (assertions nues type *Preuve par assertion* 1297, *Argument vide* 3)
sont en fait des **undercut par absence** : la fallacy ne fournit pas d'inférence (RA-node manquant), elle
n'avance pas de contre-conclusion soutenable.

### 3.2 Implication pour la décision Option (jsboige)

- **Option A** (`AIF_attackType` = attack prévenue + `AIF_attackedNode` = node maintenue) : la colonne sera
  dominée par `undercut` (inférence/crédibilité) + une minorité `undermine` (prémisse/faits), avec `rebut`
  en **tail case sporadique**. Ne pas sur-investir le tooling validateur sur la couverture `rebut` — l'absence
  n'est PAS un gap de modélisation, c'est la structure réelle du domaine.
- Ce constat **renforce** Option A : *« bonne tenue d'un scheme »* se manifeste précisément par la résistance
  à la défaillance d'inférence (undercut, dominant) et à la fausseté de prémisse (undermine) — ce qui est la
  définition opérationnelle d'une vertu argumentative.
- Si jsboige valide Option A, le back-fill 222-row est **largement mécanique** pour undercut/undermine ;
  les (rares) cas rebut seront à dériver per-case.

### 3.3 Limite du finding

3 clusters ne couvrent pas les 8 familles. rebut pourrait apparaître dans les familles **non encore
échantillonnées** — notamment *Argument pertinent* (1.x, schemes Sign/Position-to-Know) ou *Présentation
intègre* (2.x, schemes rhétoriques). Le finding est **directionnel**, pas exhaustif. Recommandation : 1-2
clusters supplémentaires sur des familles à schemes variés (1.x, 2.x) pour confirmer ou infirmer la rareté
du rebut avant de figer la decision Option.

---

## 4. Honnêteté de modélisation (discipline #677)

- **0 fabrication de tokens** : `undercut`, `undermine`, `RA-node`, `I-node`, `Argument from Commitment`,
  `CA-node` (référence, non instancié) = tous natifs AIF / Walton.
- **Scheme-divergence documentée** (§1.1) : le miroir path-prefix 7.3↔7.3 est **partiel** — 2/6 opposites
  viennent d'autres familles (Influence, Tricherie). Pas forcé en miroir 1:1.
- **Cross-référence PR-2** (§1.2) : PK 942 résistée par 2 vertus (155 + 212), modélisée comme 2 arcs
  distincts sur 2 CQ distinctes, pas dé-dupliquée.
- **Rebut-rarity = finding empirique**, pas une absence forcée. Limite échantillonnage explicitée (§3.3).
- **Anti-duplication** : couche I/RA/CA est additive au phase-2 relationnel (déjà CLEAN 222/222 #518).
  Grounding 12-col réutilisé, pas redérivé.

---

## 5. État chantier Virtues I/RA/CA — cumul PR-1 + PR-2 + PR-3

- **3 subfamilies** modélisées (4.1 Causalités, 6.1 Fidélité aux faits, 7.3 Respect de la personne).
- **3 familles** couvertes (Raisonnement valide, Honnêteté intellectuelle, Échange enrichissant).
- **9 feuilles**, **~14 fallacy-instances** opposées.
- **Attack-types observés** : `undercut` (14) + `undermine` (1) + `rebut` (0 — tail case attendu sporadique).
- **Schemes Walton distincts** couverts : Cause to Effect, Witness Testimony (Position to Know), Bias,
  **Argument from Commitment** (nouveau, ce cluster).
- **Restant** : ~214 nœuds / ~12-15 subfamilies. Post-ratification Option, back-fill largement mécanique
  (undercut/undermine dominants).

---

## 6. Option A/B/C (rappel, identique PR-1/PR-2)

| Option | `AIF_attackType` / `AIF_attackedNode` | Statut |
|---|---|---|
| **A « attaque résistée »** [reco] | attack-type **prévenu** + node **maintenu** (surtout undercut/RA-node) | **Ce doc écrit sous A** |
| B « vide structurel » | colonnes vides (perd l'info attack-type) | possible mais perte |
| C « support AIF-native » | fabrique un token → **viole #677** (0 fabrication) | rejeté |

Seules les 2 dernières colonnes changent si jsboige choisit B/C. La décomposition I/RA/CA (§2) et le finding
rebut-rarity (§3) sont **Option-indépendants** — ils guident le choix indépendamment de la décision finale.
