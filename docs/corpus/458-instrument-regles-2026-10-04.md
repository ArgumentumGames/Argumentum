# Instrument Règles — cinq défauts de mesure réparés ou nommés (0 écriture)

**Mandat** : pool #458. Après les sept passes de fidélité des Scénarios (zh #1746, ar #1749,
fa #1750, es #1751, ru #1752, pt #1753, en #1754), le corpus des **Règles** restait sans
instrument : `Cards/Rules/Argumentum Rules - Cards.csv`, **15 rangées** (dont 6 miroirs
Print&Play), **105 cellules** sur 7 langues cibles.
**Livrable** : `docs/corpus/regles-fidelity-instrument.py` (écran + self-test **14 témoins**).
**Statut** : **0 écriture** — aucune cellule CSV modifiée. Ce document mesure un instrument, il
ne juge pas encore les traductions.

---

## 1. Pourquoi ce document existe

Un écran des Règles avait été écrit le 04/10 au matin. Le premier jet portait **quatre défauts
de mesure** ; un cinquième a été trouvé **après**, en demandant pourquoi un nombre *absent*
était classé « écrit en mots ». Le publier tel quel aurait produit un dossier où un drapeau
fantôme figure sur **toutes** les lignes, et où **un défaut réel était tu**.

⭐ Deux règles ont gouverné la reprise : **un faux positif nommé et borné vaut mieux qu'une
garde élargie qui avale aussi les vrais défauts** ; et **un signal qui se lève partout ne
distingue rien**. La seconde a servi deux fois — contre le `=EN` fantôme, puis contre un
drapeau devenu muet.

---

## 2. Les cinq défauts

| # | Défaut | Mesure avant | Mesure après | Traitement |
|---|---|---|---|---|
| **1** | **`=EN` comparé à lui-même.** La boucle parcourt `['en','ru','pt','ar','es','zh','fa']` ; pour `lg == 'en'`, la cellule `Text_en` était comparée à `Text_en`. **Toujours vrai.** | `en[=EN]` sur **15 / 15** rangées — un « 15 » qui ne mesure rien | **0** | **réparé** : garde `lg != 'en'` |
| **2** | **Chiffres natifs non normalisés.** `\d+` ne voit que l'ASCII : un nombre en chiffres arabo-indiens (U+0660-0669) ou persans (U+06F0-06F9) était vu **absent** (faux `CHIFFRE?`), et un nombre natif **en trop** était invisible. | faux `CHIFFRE?` dès qu'un nombre est en chiffres natifs | comparaison sur chiffres normalisés | **réparé** : `norm_digits()` |
| **3** | **`NUMWORD` déclaré et jamais câblé.** Les tables « nombre en mots » (7 langues × 10) existaient et n'étaient lues **nulle part**. Un instrument annoncé mais inerte. | un nombre écrit en mots rendait `CHIFFRE?` plein | `MOT-NOMBRE?n` | **réparé** : table câblée |
| **4** | **Le nombre peut être dans une autre classe que le cardinal.** L'arabe dit `2` par le **duel** (« رُزمتان »), le russe `4` par le **collectif adverbial** (« вчетвером »). | 6 faux `CHIFFRE?2` (ar) + 1 faux `CHIFFRE?4` (ru) | 6 × `DUEL?2` ; le russe **reste** `CHIFFRE?` | **nommé** : `DUEL?` pour l'arabe ; le russe non réparé — voir (6) |
| **5** | **Le mot-nombre se reconnaissait *dans* un autre mot.** `words[n-1] in v.lower()` est une sous-chaîne nue : « иллюс**три**рует » (illustre) contient т-р-и, donc le 3 **absent** de Rules_06 était reclassé « écrit en mots » et **cessait d'être signalé**. | le 3 manquant sortait en `MOT-NOMBRE?3` — **un vrai défaut tu** | `CHIFFRE?3,4` — **bruyant** | **réparé** : `whole_word()` |

### (4) en détail, parce qu'il porte une leçon

Le duel arabe se marque par `تان` (nominatif) ou `تين` (oblique) ; le motif retenu est
**restreint à ces deux terminaisons**. Le duel nu en `ان` est **volontairement exclu** : `ان`
termine des mots arabes courants, et l'accepter transformerait la garde en filet qui **masque
les vraies omissions du 2**. D'où le témoin 9 — le contrôle inverse du témoin 8.

### (5) en détail, parce que c'est le seul qui faisait taire

Les quatre premiers défauts étaient **bruyants** : ils ajoutaient du faux. Le cinquième est de
la famille opposée — il **retirait** un signal. Mesure : `'три' in v` → `True`,
frontière de mot → `False`. Le repli sur `whole_word()` borne chaque mot-nombre par un
lookaround **par script d'écriture** ; `zh` reste volontairement en sous-chaîne, le chinois
n'ayant pas de frontière de mot et ses caractères se concaténant.

⚠️ **Coût mesuré, assumé.** La réparation reclasse **6 cellules** de `MOT-NOMBRE?` en
`CHIFFRE?` : plus bruyantes, et **moins précises**. Le total passe de **22 à 21** drapeaux
(les 6 `MOT-NOMBRE?` retirés, 5 rendus en `CHIFFRE?`, le 6ᵉ fusionnant avec le `CHIFFRE?` déjà
présent dans la même cellule). *Un drapeau qui se trompe en parlant vaut mieux qu'un drapeau
juste qui se tait.*

### (6) — le résidu, nommé et **non** réparé

**6 cellules** où le nombre **est bien écrit**, dans une forme que la table ne liste pas :

| Langue | Cellules | Forme attestée | Ce qu'elle est |
|---|---|---|---|
| ar | Rules_02, 11, 13, 15 | « رزمة **واحدة** » | **féminin** de « واحد » (un) |
| ar | Rules_14 | « اللاعبين **الأربعة** » | « quatre » + **article défini** `ال` |
| ru | Rules_06 | « играют **втроем** или **вчетвером** » | **collectif adverbial** |

⛔ **Non réparé, et c'est un choix.** Compléter la table par les formes attestées serait du
**réglage sur le corpus** : une autre forme (« إحدى », « ثلاث ») produirait demain le même
faux positif, et une table élargie sans preuve commence à **avaler de vraies omissions** — le
défaut (5) à l'envers. Le drapeau reste donc **levé et nommé ici** : il coûte à la lectrice un
coup d'œil par cellule, et il ne cache rien.

---

## 3. Contrôle falsifiant — les témoins ne sont pas vacues

Quatorze témoins, chacun **muet s'il ne se lève pas**. Le self-test tourne **toujours** avant
l'écran et sort en code non nul à l'échec : l'écran ne peut pas être cité sans que ses gardes
aient répondu. ⭐ Trois paires sont des **contrôles inverses** — 8/9 (`DUEL?`), 5/10
(`MOT-NOMBRE?`) et 11/12 (forme des rangées) : sans elles, une garde élargie à tout passerait
le témoin positif et resterait verte **en avalant les cas réels**.

Mutation de l'instrument committé, **une garde retirée à la fois** (instrument intact : `rc=0`,
`SELF-TEST: OK (14 temoins)`) :

| Garde retirée | `rc` | Témoin qui tombe |
|---|---|---|
| `lg != 'en'` (défaut 1) | **1** | `(1 =EN/soi): =EN leve pour lg=en -- comparaison de soi a soi` |
| normalisation de la cible (défaut 2) | **1** | `(3 chiffre persan): chiffre persan non normalise` |
| `whole_word()` (défaut 5) | **1** | `(10 mot-nombre en sous-chaine): le 3 manquant est masque par un morceau de mot -> ['MOT-NOMBRE?3']` |
| garde `DUEL?` (défaut 4) | **1** | `(8 duel arabe): duel arabe mal classe -> ['CHIFFRE?2']` |
| table `NUMWORD` débranchée (défaut 3) | **1** | `(5 nombre en mots): nombre en mots non reconnu` |
| `check_rows` débranchée | **1** | `(12 deux champs manquants): un manque de deux champs passe en silence` |
| `check_scripts` débranchée | **1** | `(13 glissement de colonnes): un glissement passe en silence` |
| *(aucune — instrument intact)* | **0** | `SELF-TEST: OK (14 temoins)` |

---

## 4. Ce que l'écran lève, une fois réparé

```
Rules_02   ar[CHIFFRE?1]
Rules_03   ar[DUEL?2]
Rules_05   ar[DUEL?2 MOT-NOMBRE?1]
Rules_06   ru[CHIFFRE?3,4]  ar[DUEL?2]
Rules_08   ar[PONCT]  es[PONCT]  zh[PONCT]  fa[PONCT]
Rules_09   ar[DUEL?2]
Rules_11   ar[CHIFFRE?1]
Rules_13   ar[DUEL?2 CHIFFRE?1]
Rules_14   ar[CHIFFRE?4]
Rules_15   ar[DUEL?2 CHIFFRE?1 PONCT]  es[PONCT]  zh[PONCT]  fa[PONCT]

=== totaux par signal === {'CHIFFRE': 6, 'DUEL': 6, 'MOT-NOMBRE': 1, 'PONCT': 8}
rangees : 15 | langues : 7 | cellules : 105 | remplies : 105
```

**Couverture : 105 / 105 cellules, 0 vide. 0 `=FR` : aucune cellule des Règles n'est restée en
français.** C'est le point que l'écran établit le plus solidement — et le `=EN` fantôme
l'occupait avant.

**Décomposition des 21 drapeaux** : **13 faux positifs mesurés** — 6 `DUEL?` (le duel est une
façon correcte d'écrire `2`), 1 `MOT-NOMBRE?` (le nombre est écrit en mots), 6 `CHIFFRE?`
(défaut (6), la forme n'est pas listée) — et **8 cellules réelles**, la famille `PONCT`.

⚠️ Un premier jet de ce paragraphe écrivait « 21 faux positifs + 1 famille réelle » : le total
ne refermait pas (21 + 8 ≠ 22), parce qu'il comptait la ponctuation **des deux côtés**.
*Quand une arithmétique de dépouillement ne referme pas, c'est le dépouillement qu'il faut
refaire — pas la phrase qu'il faut arrondir.*

### La seule famille réelle — `PONCT`, 8 cellules

Le **dernier bloc** de **Rules_08** et **Rules_15** se termine **sans ponctuation finale** en
**ar, es, zh, fa**, alors que le FR et l'EN y terminent par `.`. Dernier caractère **mesuré**
de chaque cellule (`rstrip`) :

| Rangée | FR | EN | ru | pt | ar | es | zh | fa |
|---|---|---|---|---|---|---|---|---|
| **Rules_08** | `.` | `.` | `.` | `.` | `ن` | `s` | `者` | `د` |
| **Rules_15** | `.` | `.` | `.` | `.` | `ة` | `s` | `胜` | `د` |

Ce ne sont ni des cellules vides ni des cellules tronquées : les quatre disent la même chose
que le FR (« …sont déclarés vainqueurs » / « …dépasse 1000 points »), **sans le point**.
Invisible à toute lecture qui ne va pas jusqu'au **dernier caractère** du document — et c'est
le seul geste que cet écran sait faire que la lecture d'un paragraphe ne fait pas.

⚠️ Le caractère zh de Rules_08 avait d'abord été consigné `)` — **faux** : la cellule finit sur
`者`, dernier caractère de « vainqueur ». Corrigé **avant** publication, en re-mesurant
`rstrip()[-1:]` au lieu de relire une note. *Un caractère cité de mémoire n'est pas un
caractère mesuré, même quand le compte autour de lui est juste.*

---

## 5. Ce que cet instrument n'établit pas

- **La fidélité des Règles.** Il ne compare que des **nombres** et des **ponctuations de fin**,
  sur des cellules qui sont des **documents entiers** (plusieurs centaines d'octets de markdown
  avec titres `##`, listes `*` et emoji `❌🏆➜✅1🎴`). Il ne peut rien dire d'un paragraphe et
  **ne sait pas voir un contresens** : la lecture le porte.
- **Que les traductions des Règles soient fidèles.** Les **21 drapeaux** se décomposent en
  **13 faux positifs mesurés** et **8 cellules réelles** (la famille `PONCT`). Un écran à
  105/105 cellules remplies ne dit rien de la justesse — et **8 cellules sur 105 ne font pas
  un verdict sur 129 Ko de texte**.
- **Que l'instrument soit complet.** Cinq défauts ont été trouvés en deux passes ; le cinquième
  ne se voyait **pas** dans les totaux (il *retirait* un drapeau). Un écran qui ne se trompe
  jamais n'a pas été vérifié — il a été écrit le même jour.
- **La lecture intégrale du corpus Règles** — **elle reste due**. 129 Ko sur 8 langues
  (FR 16,5 · EN 15,2 · 7 autres 97,1) ; c'est le grain suivant, pas un acquis de ce document.

---

## 6. Garde de forme — **dix rangées sur quinze sont courtes d'un champ**

Trouvé en préparant la lecture, par une alarme qui a d'abord ressemblé à une corruption :
`AssertionError: rangee de 10 champs pour 11 colonnes`.

**Mesure** : l'en-tête déclare **11** colonnes ; **10 des 15 rangées n'en portent que 10**. Le
champ manquant est **toujours le dernier** (`variant_class`), et les **5** rangées qui le
portent sont exactement les **couvertures** (`cover-argumentum`, `cover-bingo`,
`cover-beau-parleur`, `cover-moulin`, `cover-parlote`).

⇒ **Ce n'est pas une corruption, et l'écran lit juste** : `dict(zip(h, r))` apparie le
**préfixe**, et un champ manquant **en fin** de ligne ne décale rien. Le seul cas dangereux
serait un champ manquant **au milieu** — tout ce qui suit glisse d'une position et l'écran
compare des colonnes étrangères. C'est exactement le défaut qui a fait jeter un relevé le même
jour.

Deux gardes le distinguent, ajoutées après cette mesure :

| Garde | Ce qu'elle fait | Ce qu'elle **ne** fait **pas** |
|---|---|---|
| `check_rows` | refuse une rangée **trop longue**, ou à qui il manque **plus d'un** champ — sans refuser le cas réel (10/15) | ⛔ **ne voit pas** la suppression d'**un seul** champ au milieu : la ligne reste « courte d'un » et le champ manquant reste le dernier du compte |
| `check_scripts` | refuse une cellule dont l'**écriture** n'est pas celle de sa colonne (du chinois dans `Text_ru` = valeurs glissées) | ⛔ ne décide **pas** pour `pt`, `es`, `en` — latins comme le `fr`. Limite **nommée**, pas couverture supposée |

⭐ *Un compteur de champs ne mesure pas une langue* — et une garde de forme qui prétendrait
couvrir le glissement serait une garde qui se tait au moment précis où elle sert.

---

## 7. Reproductibilité

```bash
python docs/corpus/regles-fidelity-instrument.py     # self-test (14 témoins) puis l'écran
```

Le CSV est lu par **nom de colonne** (`Text`, `Text_en`, `Text_<lang>`), jamais par position.
⚠️ Un relevé antérieur du même jour avait été **jeté** pour cette raison exacte — indices de
colonne décalés d'une position, la colonne de catégorie lue comme colonne de suggestion,
« 167/167 suggestions sans ponctuation finale ». *Un total qui devrait être presque nul et vaut
100 % est un symptôme de colonne, pas de corpus.* (Même famille sur un autre corpus :
`docs/translation/458-fidelite-scenarii-en-2026-10-04.md` §1.)

*po-2024*
