# Grain 6 (#458) — titres **ru** : 2 restaurations écrites, 9 cas rendus à l'arbitrage

**Mandat** : pool #458 (dispatch c.5979291622, po-2024), item « douze titres ru ».
**Source du lot** : relevé §3 n° 4 du dossier de lecture [ru](458-fidelite-scenarii-ru-2026-10-04.md)
(12 titres nivelés).
**Statut** : **2 cellules écrites** (`1.3.2`, `7.2.6`) ; **9 titres recensés, non écrits**, avec leur
matériel de décision ci-dessous.
**Mise à jour 05/10 (grain 3 du pool c.5985353780)** : l'arbitrage ai-01 (c.5980960236) — *« les 7
titres marqués candidats s'appliquent ; 6.3.2 et 3.1.1 sont gardés, avec leur raison »* — est exécuté :
**5 écrits** (§5), **2 bloqués** par la PR #1769 (même rangée / ligne adjacente), **2 gardés** (§3.1, §3.2).

---

## 1. Le partage, et pourquoi il n'est pas un demi-travail

Le lot de 12 se scinde en **deux natures** qu'un même geste ne peut pas traiter :

| Nature | Nombre | Ce qu'il faut pour écrire |
|---|---:|---|
| **Déspécifié** — une information que le FR, l'EN **et** le pt portent tous trois a disparu du ru | 3 | un terme russe **standard** qui la ramène ; aucune invention |
| **Réécriture** — le ru décrit une autre scène, ou un autre angle, que la figure des trois sources | 8 | **recréer une figure** ; jugement littéraire russe |
| **Déspécifié sans rendu neutre** — la durée perdue n'a pas de rendu russe qui ne déplace pas le sens | 1 (`6.3.2`) | arbitrage natif |

J'ai écrit les **deux seuls** titres de la première colonne dont le rendu russe est **non ambigu**
(§2). Les neuf autres sont livrés au cas par cas (§3) : les écrire demanderait de trancher à la place
d'un locuteur natif, et le pool prévoit explicitement l'inverse — *« Garder, avec sa raison, un
équivalent idiomatique qui tient (règle C) »*. ⛔ **Aucune des propositions §3 n'est appliquée.**

---

## 2. Écrit — les deux restaurations informationnelles

Colonnes lues : `titre` (FR) · `title` (EN) · `title_pt` · `title_ru`.

| path | FR | EN | pt | ru **avant** | ru **après** |
|---|---|---|---|---|---|
| 1.3.2 | Truman et la bombe **A** | President Truman and the **A-Bomb** | Presidente Truman e a **bomba A** | Президент Трумэн и бомба | Президент Трумэн и **атомная** бомба |
| 7.2.6 | **Millésime** de foie gras | Foie gras **vintage** | **Safra** de foie gras | **Изысканное** фуа-гра | **Марочное** фуа-гра |

Les deux sont du même type : **les trois sources nomment la même qualité**, et le ru la remplaçait
par un mot d'une autre qualité — `бомба` sans le « A », `изысканное` (« exquis ») au lieu du
millésime. Les remplacements sont des **termes russes standard** (`атомная бомба` est le terme
courant pour A-bomb ; `марочное` est l'adjectif du millésime, cf. `марочное вино`) : rien n'est
inventé, la case retrouve ce que les trois autres langues portaient déjà.

Écrit par chirurgie de **span de champ** (jamais un remplacement fichier-entier) : 2 cellules sur
2 rangées, `git diff --numstat` = `2 2`, **delta octets fichier = +11 = somme des deltas de
cellules** — l'égalité est ce qui prouve que rien d'autre n'a bougé.

Garde : `ScenariiRuTitleSpecificityGuardTests` — 2 épingles pleine cellule, anti-vacuité, contrôle
inverse. **Mutation mesurée** : `1.3.2` remis à sa valeur pré-correction → la garde rougit **en
nommant la rangée** (`1.3.2.title_ru : attendu «…атомная бомба», lu «…бомба»`), 3 autres tests
restent verts ; restauration depuis un `cp` et `sha256` identique (`1a2711c16e3f55cc`).

---

## 3. Non écrit — les 9 cas, au cas par cas

Chaque ligne porte le **quadruple mesuré** des sources et ce que le ru fait à leur place. La
colonne « lecture » est ma proposition ; ⛔ **elle n'est pas appliquée**, et `6.3.2` comme `3.1.1`
sont marqués **à garder**.

### 3.1 `6.3.2` — déspécifié sans rendu neutre

| FR | EN | pt | ru actuel |
|---|---|---|---|
| Ami de **vingt ans** | Friend of **Twenty Years** | Amigo de **vinte anos** | **Старый друг** |

La durée est dans les trois sources ; le ru dit « vieil ami ». C'est le seul des trois déspécifiés
que je n'ai **pas** écrit, parce qu'aucun rendu russe ne reste neutre : `Друг двадцати лет` se lit
aussi « ami *âgé de* vingt ans », et `Друг юности` (« ami de jeunesse ») substitue une autre durée.
Chaque candidat **déplace** le sens au lieu de le restaurer — l'arbitrage revient à un natif.
**Recommandation : garder en l'état** tant qu'un rendu qui tient n'est pas proposé.

### 3.2 `3.1.1` — jeu de mots sans équivalent

| FR | EN | pt | ru actuel |
|---|---|---|---|
| **Ailier** | **Wing man** | **Ala** | Второй парень на свидании |

Les trois sources jouent sur un **seul** mot à double sens (poste au sport / homme d'appui en
séduction). Le russe ne l'a pas : il a décrit la scène (« le deuxième type au rendez-vous »). Une
recréation exigerait de fabriquer la figure — c'est exactement le cas que la **règle C** couvre.
**Recommandation : garder**, la raison étant l'absence d'équivalent, pas l'acceptation du nivellement.

### 3.3 Les sept autres réécritures

| path | FR | EN | pt | ru actuel | ce que le ru fait à leur place | lecture |
|---|---|---|---|---|---|---|
| 2.2.7 | Le **prétendant** de Pénélope | Penelope's **suitor** | O **pretendente** de Pénélope | Завоевать Пенелопу | l'**action** (« conquérir ») au lieu de la **personne** (le prétendant) | `Претендент на Пенелопу` restaure la personne ; terme standard — **candidat** |
| 3.3.6 | **Ciel, mon mari !** | Heavens my husband | Por Deus, meu marido | Нам крышка | un **verdict** (« on est fichus ») au lieu de l'exclamation de l'épouse | l'exclamation se dit en russe (`Батюшки, мой муж!`) — **candidat**, mais la référence culturelle FR est déjà perdue en EN et pt |
| 3.3.10 | Mariage ? **Non merci** | Marriage? **No Thanks** | Casamento? **Não, obrigado** | Свадьба или нет? | une **question neutre** ; le refus a disparu | `Свадьба? Нет, спасибо` restaure le refus tel quel — **candidat** |
| 4.1.1 | **Rouler des mécaniques** | Rolling mechanics | Fazer-se de importante | **Уловки продавца** | change de **sujet** : le vendeur, là où les trois sources parlent de celui qui **frime** | la règle C ne couvre pas un changement de sujet → **défaut plutôt que choix** ; `Понты` / `Пыль в глаза` disent la frime — **candidat** |
| 4.1.2 | **Le martinet** | **The Cane** | **O chicote** | Телесные наказания | l'**objet** concret devient une **abstraction** | `Кнут` / `Розги` restaurent l'objet (le calembour oiseau est déjà perdu en EN et pt) — **candidat** |
| 5.3.1 | **Débat avec un terraplaniste** | **Flat Earth Society** | **Debate com um terraplanista** | Теория плоской земли | la **théorie** au lieu du **débat** | `Дебаты с плоскоземельцем` — terme russe établi — **candidat** |
| 7.1.5 | **La kermesse** | **Kermesse** | A **quermesse** | Праздник | générique (« fête ») ; le mot des trois sources a disparu | `Ярмарка` (ou le calque `Кермесса`) — **candidat** |

« Candidat » veut dire : **le référent est restaurable avec un terme russe standard**, sans
fabriquer de figure. Cela ne veut pas dire que la proposition est la bonne — le **fit éditorial**
(un titre de carte n'est pas un mot de dictionnaire) reste à arbitrer par un locuteur natif.
⭐ Les sept propositions ci-dessus sont **de moi, non validées par un natif** : elles sont le
matériau de la décision, pas la décision.

---

## 4. Instrument — le référent FR n'est pas la colonne qu'on croit

⚠️ **Piège mesuré pendant ce grain.** Dans `Argumentum Scenarii - Cards.csv`, le bloc anglais est
écrit en **colonnes nues de suffixe** : `title`, `context`, `issue`, `category`, `subcategory`,
`smoothTalker`, `drawer` — quand le français porte les **noms français** `titre`, `contexte`,
`enjeu`, `catégorie`, `sous-catégorie`. Une lecture qui prend `title` pour la colonne de base rend
donc de l'**anglais étiqueté fr** : ma première mesure de ce grain a produit
`fr='President Truman and the A-Bomb'` — faux, et faux sans erreur.

Le bon référent est `titre` (colonne 4). ⛔ Ne jamais dériver « la source » d'un nom de colonne
qui *ressemble* à la langue par défaut : **vérifier que la colonne existe sous ce nom**, sinon lire
la valeur et constater la langue. Le contrôle qui l'attrape ici est trivial et je ne l'avais pas
fait d'abord : lire une cellule et regarder ce qu'elle dit.

---

## 5. Suite — arbitrage (04/10) et écriture (05/10, grain 3 du pool c.5985353780)

L'arbitrage ai-01 (c.5980960236, repris tel quel par le pool c.5985353780 grain 3) statue : *« les 7
titres ru marqués "candidat" s'appliquent ; 6.3.2 et 3.1.1 sont gardés, avec leur raison »*. La règle
d'écriture : **entre deux termes proposés, prendre celui qui tient ensemble le FR et l'EN**.

### 5.1 Écrits — 6 des 7 candidats (7.1.5 retiré en review, §5.4 ; lot post-#1769 livré 06/10)

| path | ru avant | ru écrit | pourquoi ce terme |
|---|---|---|---|
| 3.3.6 | Нам крышка | **Батюшки, мой муж!** | les trois sources portent l'exclamation de l'épouse (« Ciel, mon mari ! » / « Heavens my husband » / « Por Deus, meu marido ») ; ru disait un verdict (« on est fichus ») |
| 3.3.10 | Свадьба или нет? | **Свадьба? Нет, спасибо** | « Non merci » / « No Thanks » / « Não, obrigado » — le refus, que la question neutre ru effaçait |
| 4.1.1 | Уловки продавца | **Понты** | FR, EN et pt disent la **frime du client** ; ru décrivait le vendeur. Entre les deux proposés : **понты** = frime ostensible ; **пыль в глаза** déplace vers la duperie — les sources ne disent pas « tromper » |
| 4.1.2 | Телесные наказания | **Розги** | la carte parle de punition scolaire ; entre les deux proposés : **розги** = l'instrument russe de cette punition et le répondant exact du « cane » EN ; **кнут** déplace vers la torture historique |
| 2.2.7 | Завоевать Пенелопу | **Претендент на Пенелопу** | les trois sources portent la **personne** (« Le prétendant » / « suitor » / « O pretendente ») ; ru disait l'action (« conquérir »). Écrit le 06/10, lot post-#1769 (§5.2) |
| 5.3.1 | Теория плоской земли | **Дебаты с плоскоземельцем** | les sources portent le **débat** (« Débat avec un terraplaniste » / « Debate com um terraplanista » / « Flat Earth Society ») ; ru disait la théorie — le `context_ru` dit « на дебаты ». Écrit le 06/10, lot post-#1769 (§5.2) |

Chirurgie par span de champ, **delta octets −20 = somme exacte des 4 cellules** de #1774 (la
cellule 7.1.5 retirée en review était à delta nul : 8 caractères cyrilliques → 8), `--numstat 4 4`,
167 enregistrements intacts, ni BOM ni LF brut introduits (le CSV Scenarii n'en porte aucun).
Le lot post-#1769 (06/10) : **delta +15**, `--numstat 2 2`, mêmes invariants, sha256 vérifié
après restauration de la mutation de contrôle.

### 5.2 Ex-bloqués par la PR #1769 — lot livré le 06/10 (file profonde c.5993735448, grain 1)

- **2.2.7** → `Претендент на Пенелопу` — **même rangée** que le `title_pt` « Penélope » de #1769
  (ligne 35) ;
- **5.3.1** → `Дебаты с плоскоземельцем` — **ligne adjacente** à 5.3.2 (lignes 125/126, tenue par
  #1769) : des rangées disjointes ne suffisent pas, git conflit sur des lignes voisines
  (découverte du cycle XI).

### 5.3 Gardés (arbitrage, définitifs)

- **3.1.1** — jeu de mots à double sens sans équivalent russe (règle C) ;
- **6.3.2** — aucun rendu russe de « vingt ans » ne reste neutre ; relecture native si un rendu est
  proposé.

### 5.4 Retiré en review (05/10, c.5990865284) — 7.1.5

« Кермесса » a été écrite le matin même puis **retirée le jour même** : sa raison écrite — *« seul
le calque tient FR+EN+pt ensemble ; « ярмарка » perd le mot que **même l'EN a gardé** »* — est tombée
avec la réponse de l'owner (« oui à tout » au dossier du texte imprimé) : le titre EN « Kermesse »
passe à « **The Fair** ». Le motif de l'owner s'applique tel quel au russe : *le titre doit dire ce
que dit le corps de la carte* — or le `context_ru` dit « **праздник** своего ребёнка ». « Кермесса »
créerait en russe exactement le défaut qu'on retire en anglais, avec en prime un risque de lecture
(la fête flamande des tableaux, pas la fête de l'école). Le CSV est **revenu à « Праздник »** et la
cellule est ré-épinglée au burn-down ; elle part dans la PR du **grain 3** (même rangée que
« Kermesse → The Fair »), forme recommandée **« Школьный праздник »** — le mot du corps, le sens
scolaire que l'es porte déjà (« Fiesta escolar »), et la généricité levée.

**Écrite le 07/10** (file profonde c.5993735448, grain 2 « 3a ») : `Праздник` → **`Школьный праздник`**, dans la même PR que les 11 cellules EN du GO owner. L'épingle quitte le
burn-down avec sa correction — `Restored` 8→9, `HandedOff` 3→2.

## 6. Reste à faire

- ~~**2.2.7 et 5.3.1** (§5.2) : un lot unique après le merge de #1769~~ **ÉCRITS le 06/10**
  (file profonde c.5993735448, grain 1) : la garde n'est plus tenue par une PR ouverte depuis le
  merge de #1774.
- ~~**7.1.5 `title_ru`** (§5.4) : dans la PR du grain 3a/3b, même rangée que l'EN « Kermesse → The Fair » ;
  forme recommandée « Школьный праздник ».~~ **ÉCRIT le 07/10** (grain 3a, file c.5993735448) :
  même PR que les 11 cellules EN du GO owner.
- La garde `ScenariiRuTitleSpecificityGuardTests` épingle les **9 écrits** (valeur pleine) et le
  **burn-down des 2 restants** (les 2 gardés par arbitrage), chacun avec son
  motif : une exclusion meurt avec sa raison.
- Les 8 cellules écrites changent les **PokerCards ru** à la prochaine régénération ; rien n'est
  republié avant le verdict des associés (gel `v2.0.0-review`).

*po-2024*
