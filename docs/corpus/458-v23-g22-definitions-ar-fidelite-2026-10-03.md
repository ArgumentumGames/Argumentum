# Pool v23 — série ㉒ : fidélité des **définitions** arabes du deck (mesure 0-écriture)

**Base** : branche `docs/458-g3-jointure-par-nom` (grain 3, instrument réparé par la jointure par nom). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, `--self-test` (6 contrôles). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Troisième grain de la série ⑳-㉕. Comme au ㉑ (`zh`), le portage sur `ar` a **révélé deux défauts d'instrument**, réparés et mesurés ici — l'abjad n'est pas le cyrillique, et le jeu de marqueurs arabes matchait des sous-chaînes.

## Synthèse — 0 A / 0 M / 37 rangées observées / 138 ✓ (175)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **37** (6 catégories) |
| **✓ — fidèle** | **138** |

**Les 175 rangées ont été lues, aucun échantillonnage.**

⚠️ **Même faiblesse structurelle qu'au ㉑, dite avant le verdict** : les archives du dépôt ne portent aucun `desc_ar` — la référence imprimée est **inexploitable sur 0/175** (l'instrument l'imprime : `REFERENCE EXPLOITABLE: 0/175`). Le verdict repose sur une lecture simple sans contrôle croisé imprimé, par un **lecteur non natif**. Ce grain n'est pas plus faible que le ㉑ sur ce plan, il n'est pas plus fort non plus.

## Ce que ce grain partage avec le ㉑ : la cellule arbitrée du zh y est **saine**

PK 989 « Renverser la charge de la preuve » — tranchée **A** en chinois (c.5964429559, corrigée par #1714) — se lit ici : *أنت تفترض أن الطرف الآخر هو الذي يجب عليه إثبات **عدم صحة** موقفك بدلاً من تبريره بنفسك*. **« عدم صحة » = « fausseté »** : la cellule arabe dit ce que disent le français et les quatre autres langues. L'inversion n'existe qu'en chinois — lue ici directement, pas déduite.

## L'instrument a été réparé deux fois — chaque défaut mesuré avant et après

### 1. L'écran de longueur était calibré pour le cyrillique, pas pour l'abjad

L'arabe ne note pas les voyelles brèves et agglutine ses clitiques : ratio mesuré `desc_ar`/`desc_fr` **médiane 0,618** (p10 0,505, min 0,389) — la moitié du corpus **sain** tombe sous la borne SHORT(0,60) du russe : **76/175 drapeaux sur des cellules saines**, un écran qui hurle ne trie rien.

Même remède qu'au ㉑ : étalonnage **sur la langue elle-même** (`len ~ a·fr_len + b`, généralisé à toute écriture à compression stable — la porte `is_arabic_dominant` couvre le farsi ㉓ par avance) :

| | Valeur |
|---|---|
| Ajustement | pente 0,632 · R² **0,681** (plus fort que le zh : 0,498) · écart-type du résidu 9,7 car. |
| SHORT avant → après | **76 → 0** |
| `LENDEV` sur le corpus réel | **2** / 175 — lues toutes deux : expansions bénignes (PK 680 rend les **deux** côtés de l'opposition là où le français n'en nomme qu'un ; PK 1291 ajoute la conséquence « ainsi vous en restez au superficiel ») |
| Contrôle inverse — troncature 50 % | **137** / 175 détectées (zh : 104) |
| Contrôle inverse — troncature 35 % | **168** / 175 détectées |

### 2. Le jeu de marqueurs arabes matchait des sous-chaînes — démontré **par le contrôle inverse lui-même**

Le premier plantage d'une cellule inversée (PK 814) a levé `POLARITY` **par accident** : le marqueur `ما` a matché **à l'intérieur d'أمامك** (« devant vous »). Le jeu arabe était le seul des jeux à écriture espacée **sans frontières de mot**. Réparé : `\b` posés + `لن` (négation du futur, présente sur 12 cellules) ajouté.

| | Avant | Après |
|---|---:|---:|
| « Négation détectée » sur `desc_ar` | 130/175 | **78/175** |
| Drapeaux `POLARITY` | 95 | **45** |

Limite qui **reste** (nommée, pas cachée) : `ما` est un homophone — négation ou pronom indéfini (« سلوكًا ما », un comportement **quelconque**) — indécidable par frontières. L'écran reste une priorité de lecture.

⭐ C'est le **troisième grain** où l'écran de polarité produit des faux positifs par construction (⑳ : négation préfixée russe ; ㉑ : négation lexicalisée française vs morphème chinois ; ㉒ : sous-chaîne puis homophone). Sur les trois corpus : **0 défaut réel détecté par lui**.

## C-notes — 37 rangées, 6 catégories, aucune écriture proposée

### 1. L'arabe garde l'imprimé d'époque, le français a été réécrit depuis (3)

Vérifié **mécaniquement** contre le `desc_en` de l'archive v3 (pas de mémoire) :

| PK | Titre | Ce que l'imprimé EN dit — et l'arabe avec lui | Ce que le français courant dit |
|---|---|---|---|
| 304 | Vœu pieux | « **notwithstanding available evidence** » / بغض النظر عن الأدلة | s'arrête à « qu'elle le soit » |
| 343 | Argument du bâton | « **to force someone's agreement** with a proposition » / لإجبار شخص ما على الموافقة | « remporter un débat au lieu de défendre votre position par la raison » |
| 837 | Comparaison incohérente | « **Partially comparing** … **to pretend** to draw a general comparison » / جزئيًا… لتظاهر بإجراء مقارنة عامة | « en ne retenant que certains aspects, ce qui fausse la comparaison globale » |

⛔ **Ce n'est pas un défaut de l'arabe** — c'est un retard du français courant (famille de la catégorie 1 des grains ⑳ et ㉑). **3 cartes ici, contre 17 au zh** : la mesure T2 ci-dessous dit pourquoi.

### 2-4. Trois jumeaux exacts du dossier chinois (3)

- **PK 729 « Négation de l'antécédent »** — **la même divergence que le zh** (C-note 2 du ㉑) : « condition suffisante » devient **شرطًا ممكنًا** (« condition *possible* »), et la cellule **ajoute la même glose** (« confondant cause et condition nécessaire »). Deux langues indépendantes qui portent le même ajout pointent un original d'époque commun — piste, pas conclusion.
- **PK 134 « Sophisme ludique »** — le connecteur relatif (« modèles **qui** négligent ») devient une alternative (**أو**, « ou en ignorant »). Même C-note que le zh.
- **PK 175 « Influence »** — l'opposition *persuader/convaincre* du français est rendue par **manipulation vs logique**. Même C-note que le zh (« rather than using logical reasoning »).

### 5. Décalages de notion, sens tenu (9)

| PK | Titre | Ce qui bouge |
|---|---|---|
| 51 | Sophisme du psychologue | « perspective individuelle » → « **expérience** personnelle » |
| 79 | Argument d'accomplissement | « validité » → « **valeur** » ; « domaine concerné » → « champ **en débat** » |
| 121 | Politiquement correct | « la sensibilité d'**une partie de l'auditoire** » → « mécontentement **social ou institutionnel** » |
| 179 | Question piège | la glose « **circularité** » (مصادرة) s'ajoute à l'énumération fausse/controversée |
| 300 | Connivence | « infléchir son **jugement** » → « gagner son **soutien** » |
| 594 | Erreur mathématique | la spécification « informations **numériques** » s'ajoute au « quantitatif » |
| 733 | Affirmation d'une disjonction | réencadré par le mécanisme : « un seul terme d'un "ou" ne peut être vrai à la fois » |
| 781 | Logique du chaudron | le contraste « valables **seules** / contradictoires **ensemble** » est compressé en « valables mais contradictoires » |
| 1362 | Tu quoque | « ses propres **principes** » → « ses propres **déclarations** » — plus proche du mécanisme (dire vs faire) |

### 6. Adoucissements et compressions mineurs, sens tenu (22)

PK 105 (affirmez → argumentez) · 112 (démonstration → présentation) · 133 (éléments → données) · 322 (discréditez → critiquez) · 361 (« vos idées contestables » → « un argument discutable », possessif perdu) · 644 (fausse le **raisonnement** → fausse la **conclusion**) · 658 (compréhension → évaluation de l'infini) · 666 (raisonnement mathématique → **calculs**) · 697 (non démontré → non clair) · 707 (« ou inversement » → « confondre ») · 768 (pas comparables → différentes) · 798 (subtilités → **ruses** — convergent avec l'intention de l'imprimé) · 804 (définition sur mesure → termes définis arbitrairement, compression) · 834 (excessive **ou** inappropriée → inacceptable) · 847 (« structure » → « structure **grammaticale** » — même C-note que le ru ⑳) · 1011 (difficilement défendable → indéfendable) · 1015 (« quelqu'un » → « tu », normalisation à la voix du deck) · 1023 (empêche → trouble le jugement) · 1242 (cadre déformé → **biaisé** ; « trop rigide » → « rigide ») · 1281 (compression « fondé sur des arguments rationnels » → « logique ») · 1330 (faiblesse → défaut ; « sans rapport » → « sans rapport **en général** ») · 1365 (argument → hypothèse).

## T2 — d'où vient l'arabe ? (**mesuré, sans l'ambiguïté du zh**)

Au ㉑, les deux corrélations de longueur se contredisaient (zh~fr 0,667 vs zh~en 0,425) et demandaient une reformulation prudente. **Ici les deux mesures disent la même chose** :

| Comparaison | corr avec `ar` |
|---|---:|
| vs **français courant** | **0,825** (n=175) |
| vs **anglais imprimé** (168 rattachées) | **0,278** |

**L'arabe suit le français courant**, pas l'imprimé — d'où 3 cartes d'époque (catégorie 1) contre 17 au zh. Aucun jeton latin isolé dans l'imprimé absent du français n'a de témoin arabe (la sonde du PK 844 zh rend 0 ici — et PK 844 se lit fidèle au français courant). Hypothèse parcimonieuse consignée : l'arabe a été traduit du français **à une époque proche du texte courant** — pas besoin d'invoquer un texte d'époque disparu.

## Contrôle inverse — l'accident, puis l'aveuglement propre

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

1. **Cellule inversée plantée** (PK 814, le faux dilemme dit à l'envers : « de nombreuses options s'offrent à vous alors qu'il n'en existe que deux »). **Premier essai : `POLARITY` a levé — par accident** (`ما` à l'intérieur d'أمامك), exactement l'incident du « не » au grain ⑳. Cet accident **est** la découverte du défaut n° 2 ci-dessus. **Après réparation : `FLAGS: -`** — aucun écran ne voit le contresens ; **la lecture le voit immédiatement** → A si elle était réelle.
2. **Cellule tronquée à 50 %** : détectée `LENDEV` par l'écran étalonné abjad (137/175 de pouvoir sur le corpus) ; contre-témoin sain → aucun `LENDEV`. Ce couple tourne dans `--self-test` pour **chaque écriture** (zh, ar, fa).

## N'établit pas

- **Que les définitions arabes soient sans défaut.** Lecture **unique**, par un **non-locuteur natif**. L'établi est plus étroit : *aucun écart n'a été trouvé en lisant les 175 rangées*.
- **Que l'imprimé vienne au secours de ce zéro — il ne le peut pas** : `desc_ar` n'existe dans aucune archive (0/175 exploitable, imprimé par l'instrument).
- **Que les écrans corroborent le zéro.** `POLARITY` : 45 drapeaux post-réparation, **0 défaut réel** derrière (lecture des 175). `LENDEV` : 2 levées, 2 expansions bénignes lues. Le contresens planté sort `FLAGS: -`.
- **Que l'écran de polarité soit maintenant juste** : l'homophone `ما` reste indécidable par frontières ; le jeu `fa` (㉓) n'a pas été réparé ici — il sera mesuré avec sa langue.
- **Rien sur** les 7 cartes sans archive (PK 105, 362, 492, 1020, 1092, 1120, 1357), les langues restantes (㉓ `fa`, ㉔ `pt`+`es`, ㉕ `en`), les champs `example_<lang>` / `link_<lang>`, ni les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.** Les 37 observations attendent l'arbitrage d'ai-01.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang ar --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175) ; corpus du dépôt **jamais écrit** (contrôle inverse sur copie).
- **Non-régression prouvée sur les langues déjà mesurées** : `ru` 26 drapeaux, `zh` 48 — identiques avant/après les deux réparations (les correctifs ne touchent que la porte abjad et le jeu de marqueurs `ar`).
- Comptes de jointure inchangés par le ㉒ (157 nom / 11 position seule / 7 aucune — mesurés au grain 3).
