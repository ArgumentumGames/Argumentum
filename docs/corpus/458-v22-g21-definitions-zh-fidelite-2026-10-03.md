# Pool v22 — série ㉑ : fidélité des **définitions** chinoises du deck (mesure 0-écriture)

**Base** : master `3e830884` (03/10). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, `--self-test` (4 contrôles). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Deuxième grain de la série ⑳-㉕. Le grain ⑳ (russe) a établi la méthode ; ce grain la porte sur `zh` — et **le portage a cassé deux choses**, toutes deux réparées ici et mesurées.

## Synthèse — 1 A / 0 M / 19 C-note / 155 ✓ (175)

> **Erratum d'arbitrage (03/10, ai-01, c.5964429559).** Ce dossier a d'abord été publié **0 A / 0 M / 20 C-note**. PK 989 — classée C-note 2 avec sa frontière écrite — est **tranchée en A** : le raisonnement de frontière était juste, c'est la décision qui change. La correction (un caractère, 为真 → 为假, CSV seul) vit dans la PR dédiée **#1714**. Verdict courant : **1 A / 0 M / 19 C-note / 155 ✓**.

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **1** (PK 989, arbitrage ai-01) |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | 19 |
| **✓ — fidèle** | 155 |

**Les 175 rangées ont été lues, aucun échantillonnage.**

⚠️ **Ce verdict est plus faible que celui du grain ⑳, et il faut le dire avant de le lire.** Le russe disposait de 168 références imprimées *exploitables* ; le chinois en a **zéro** (section suivante). Le verdict repose donc sur une lecture simple, sans contrôle croisé imprimé — c'est un cran de moins dans l'échelle de preuve, et aucun chiffre ci-dessus ne le rattrape. C'est précisément cette lecture sans filet qui a produit le **seul A des grains ⑳-㉑** — et il a fallu l'arbitrage d'ai-01 pour le trancher.

## L'arbitre imprimé est structurellement absent — et l'instrument le cachait

La série repose sur une règle du pool : *une traduction qui retrouve l'imprimé n'est pas une dérive*. Elle suppose qu'il existe un imprimé dans la langue mesurée.

**Il n'en existe pas pour le chinois.** En-têtes des quatre archives du dépôt :

| Archive | Colonnes `desc_*` |
|---|---|
| `v3` | `desc_fr`, `desc_en`, `desc_ru`, `desc_pt` |
| `v3pp` | `desc_fr`, `desc_en`, `desc_ru`, `desc_pt` |
| `2022` | `desc_fr`, `desc_en` |
| `2022pp` | `desc_fr`, `desc_en` |

⇒ **`desc_zh`, `desc_es`, `desc_ar` et `desc_fa` n'existent nulle part.** Pour les grains ㉑㉒㉓ — et pour la moitié `es` du ㉔ — la règle de l'imprimé est **inapplicable**, non pas parce qu'on n'a pas trouvé la référence, mais parce que la référence n'a jamais été imprimée dans cette langue.

⛔ **L'instrument, tel qu'il était livré au grain ㉑, rendait ce fait illisible.** Sur 168 rangées appariées il écrivait `<pas de desc_zh>` — la même chaîne pour « l'archive n'a pas la colonne » et pour « la cellule est vide ». Les deux situations n'ont pas la même conséquence : la première interdit tout arbitrage, la seconde en autorise un. C'est le piège déjà consigné au dépôt (`r.get(col)` → `None` se lit comme une absence de contenu). **Corrigé** : l'instrument distingue désormais `<archive SANS colonne desc_zh>` de `<cellule desc_zh vide>` et imprime un pied de page `REFERENCE EXPLOITABLE: n/175`.

| | `ru` (grain ⑳) | `zh` (ce grain) |
|---|---:|---:|
| Référence imprimée **attachée** | 168/175 | 168/175 |
| Référence imprimée **exploitable** | **168/175** | **0/175** |

⚠️ Le chiffre `168/175` est identique dans les deux colonnes et **ne veut pas dire la même chose**. C'est exactement le genre de chiffre qu'un tableau récapitulatif de série aplatirait à tort.

## A — écarts réels

**PK 989 — « Renverser la charge de la preuve » : la subordonnée inverse la direction de la charge.** Publiée C-note 2, **tranchée A par ai-01** (c.5964429559).

| | Texte |
|---|---|
| FR (courant) | *Vous estimez que c'est à la partie adverse de démontrer que votre position est **fausse**, au lieu de la justifier vous-même.* |
| EN (imprimé) | *Assuming a proposition to be true until proved **false** or vice-versa.* |
| ZH (fautif) | *认为证明自己的主张**为真**是对方的责任，而不是自己证明其正确性。* — c'est à l'adversaire de prouver votre position **vraie** |
| ZH (corrigé, #1714) | *认为证明自己的主张**为假**是对方的责任，而不是自己证明其正确性。* |

**Pourquoi A — et pourquoi la frontière avait d'abord arrêté le classement ici.** La passe de mesure avait lu la contradiction (les deux membres de l'opposition disaient tous deux « prouver vrai ») et l'avait consignée en C-note au motif que *le sujet de la carte survit* : la charge est bien renversée sur l'autre partie. L'arbitrage tranche l'inverse : **la proposition qui porte la définition est inversée**, les cinq autres langues traduites disent toutes « fausse » (ru « опровергать », pt « demonstrar a invalidez », ar « إثبات عدم صحة », es « hasta que se demuestre lo contrario », en « until proved false »), et une définition imprimée qui se contredit se voit en moins de 30 secondes par un lecteur chinois — que le sujet survive ne suffit pas. **Le raisonnement de frontière est conservé comme enseignement** : c'est exactement le genre de ligne qu'un non-locuteur ne doit pas trancher seul ; la consigner avec sa frontière écrite était la bonne action, le verdict appartenait à l'arbitre.

**Propagation.** La phrase fautive est aussi portée par `Cards/Fallacies/Mindmaps/zh/Fallacies_zh.content.svg` et `Fallacies_zh.html` (mesuré par `git grep` au dispatch). **#1714 corrige le CSV seul** — la re-dérivation n° 3 propage, ces fichiers ne se corrigent pas à la main.

## M — contenu manquant ou ajouté

**Aucun.** Aucune définition chinoise ne perd ni n'ajoute de contenu substantiel par rapport à sa source.

## C-notes — 19 observations, aucune écriture proposée

### 1. Le chinois reproduit l'imprimé, le français a été réécrit depuis (17)

Le cas majoritaire, et de loin. Sur ces 17 cartes, le chinois correspond **au mot près** à la définition **imprimée en anglais** (`desc_en` des archives), alors que le `desc_fr` courant dit autre chose. Le français a été enrichi ou reformulé **après** l'impression ; le chinois, lui, est resté fidèle au texte de l'époque.

⛔ **Ce n'est pas un défaut du chinois.** C'est un retard de la source française, et la même famille que la catégorie 1 du dossier ⑳ (« le russe suit l'imprimé, le français a été enrichi depuis »). Ces lignes sont listées pour être **arbitrables**, pas corrigées.

| PK | Titre | Ce que le français courant dit | Ce que l'anglais imprimé — et le chinois — disent |
|---|---|---|---|
| 78 | Appel au respect | « une personne réputée » | « a reputable, respected person **or entity** » / 或实体 |
| 154 | Sophisme du sophisme | ajoute « **uniquement** » | « because its proof involves a fallacy » (sans « uniquement ») |
| 175 | Influence | oppose *persuader* / *convaincre* | « rather than using logical reasoning » / 而不是使用逻辑推理 |
| 182 | Fausse alternative | « qui mènent au **même** résultat » | « lead to essentially **similar** results » / 类似的结果 |
| 304 | Vœu pieux | s'arrête à « qu'elle le soit » | « **notwithstanding available evidence** » / 无视所有证据 |
| 343 | Argument du bâton | ajoute « au lieu de défendre votre position par la raison » | « to force someone's agreement with a proposition » |
| 511 | Communication non verbale | « au-delà du sens des mots » | « or other cues like **facial expression** » / 或其他面部表情等 |
| 625 | Sophisme de division | « certaines caractéristiques … chacun de ses membres » | « properties of a whole set to each of its individual part » |
| 670 | Faux équilibre | « le même poids à deux avis divergents » | « more balanced than they actually are » / 比实际更为平衡 |
| 673 | Juste milieu | « la solution réside dans le compromis » | « the truth must be found as a compromise » / 真相必然存在于…折中 |
| 699 | Argument circulaire | « vous raisonnez en cercle : chaque argument… » | « propositions that are only true if the conclusion is true » |
| 735 | Erreur de quantification | ajoute « ce qui fausse l'argument » | « Misusing quantifiers such as "all", "none" or "some" » |
| 798 | Abus de langage | « user de subtilités linguistiques pour convaincre » | « Using language in a way that misleads or misrepresents the truth » |
| 837 | Comparaison incohérente | « ce qui fausse la comparaison globale » | « Partially comparing several things to pretend to draw a general comparison » |
| 844 | Sophisme d'association | ne mentionne **ni A ni B** | « Using a shared property between **A** and **B**… » / 通过使用**A**和**B**之间的共同特性 |
| 1011 | Exigence relâchée | ajoute « ce qui dénature le débat » | « Lowering the requirements when you're argumentation seems to fail to meet them » |
| 1280 | Obstruction | « la discussion ne se déroule pas comme prévu » | « Preventing the discussion from focusing on the **main issue at hand** » |

⭐ **PK 844 est le témoin mécanique de toute la catégorie** : le français courant ne contient **aucun** jeton `A` ni `B`, l'anglais imprimé et le chinois les portent tous deux. Un sinogramme ne fabrique pas une lettre latine isolée : le chinois n'a pas pu être traduit du français **actuel**. (Voir T2 — cette observation est plus forte que l'hypothèse qu'elle a d'abord semblé servir.)

### 2. PK 729 — « Négation de l'antécédent » : déplacement de notion + ajout explicatif

| | Texte |
|---|---|
| FR | *…simplement parce qu'une de ses **conditions suffisantes** n'est pas remplie.* |
| EN (imprimé) | *Invalidating a conclusion on grounds of the Falsity of a **sufficient condition**.* |
| ZH | *仅仅因为**可能原因**未发生就否定结论，从而混淆了原因和必要条件。* |

« condition suffisante » devient « cause possible » (可能原因), et le chinois **ajoute** une glose (« confondant cause et condition nécessaire »). La carte s'appelle « négation de l'antécédent » : « cause possible » décrit bien l'antécédent, donc le sens tient. Consigné pour l'ajout, pas pour la notion.

### 3. PK 134 — « Sophisme ludique » : un connecteur change de nature

FR : « des modèles simplistes **qui** négligent leur complexité réelle » (relative). ZH : 使用简单化的模型**或**忽略了真正的复杂性 (« **ou** ignorant… »). Le lien logique passe de la caractérisation à l'alternative. Écart mineur, consigné.

### 4. Observations transverses

**T1 — Les écrans mécaniques n'ont, là encore, trouvé aucun défaut.** Compteur : 47 drapeaux sur 175 (puis 48 après réparation, cf. ci-dessous), **0 défaut réel derrière**.

| Drapeau | Levés | Réels | Pourquoi ils se trompent |
|---|---:|---:|---|
| `POLARITY` | 47 | **0** | **Le miroir exact du défaut russe du grain ⑳.** Le français **lexicalise** la négation dans le nom (*impr**é**cision*, *incohérence*, *inexactitude*, *in**a**pproprié*), le chinois la **morphologise** (不精确, 不一致, 不准确, 不正确). L'écran compare une **particule** française à un **morphème** chinois : il mesure l'absence d'une forme, pas l'absence d'une négation. 42 des 47 sont de cette famille ; les 5 restants sont des négations réelles que le chinois rend par un verbe lexical (无视, 忽略, 避免). |
| `LEN-NA(CJK)` | 175 | — | Écran **non applicable**, pas faux : voir ci-dessous. |
| `LENDEV` | **1** | **0** | PK 112 : le chinois est réellement plus court que ne le prédit sa langue (résidu −12,6 car.). Lecture faite : c'est une compression légitime, la phrase est complète. |

⭐ Sur ⑳ comme sur ㉑, l'écran de polarité produit **exactement un faux positif par carte niée**. Il n'a jamais servi. Sa valeur est de forcer une relecture ; il ne porte aucun poids de verdict, et le contrôle inverse le prouve (section suivante).

**T1bis — L'écran de longueur a été réparé, pas contourné.** Le grain ⑳ avait **déclaré** l'écran non applicable en CJK (`LEN-NA(CJK)`), ce qui est honnête mais laissait **0 %** de pouvoir de détection sur 128 des 175 rangées. Remplacé par un écran **étalonné sur le chinois lui-même** (`zh_len ~ a·fr_len + b`, ajusté sur le corpus) : un écart au résidu attendu est un vrai signal, calibre sur la langue et non sur une autre écriture.

| Mesure | Valeur |
|---|---|
| Ajustement | `pente 0,203 · ordonnée 5,2 · R² 0,498 · écart-type du résidu 4,6 car.` |
| Seuil | ±2,5 écarts-types |
| Cartes levées sur le corpus **réel** | **1** / 175 (PK 112) |
| **Contrôle inverse — troncature à 50 %** | **104** / 175 détectées |
| **Contrôle inverse — troncature à 35 %** | **167** / 175 détectées |
| Pouvoir de l'ancien écran (`LEN-NA`) | **0** / 175 — par construction |

⚠️ Limite nommée : l'écran attrape les troncatures des cellules **longues** et laisse passer celles des cellules courtes (à 50 %, 71 cellules échappent). Il n'est pas un filet complet ; il est strictement meilleur que rien.

### 5. T2 — D'où vient le chinois ? (**SUPPOSÉ**, et deux mesures se contredisent)

La catégorie 1 montre que le chinois ne dérive pas du français **courant**. L'hypothèse naturelle est alors : le chinois dérive de l'**anglais imprimé**. **Deux mesures indépendantes la testent, et elles ne disent pas la même chose.**

**Ce qui la soutient** — PK 844 : le français courant n'a ni `A` ni `B`, l'anglais et le chinois les ont. Aucun autre marqueur de ce type n'a été trouvé (sonde sur les jetons latins isolés : **1** carte ; sur les nombres présents dans l'anglais et absents du français : **1** carte, PK 784, où le chinois écrit 三 « trois » là où l'anglais écrit `3` — différence de notation, **pas** un indice de filiation). La base mécanique de l'hypothèse est donc **une seule carte**.

**Ce qui la contredit** — si le chinois descendait de l'anglais, sa **longueur** devrait suivre celle de l'anglais. Mesuré sur les 157 cartes ayant les trois colonnes :

| Comparaison | corr avec `zh` |
|---|---:|
| `zh` vs **français** courant | **0,667** |
| `zh` vs **anglais** imprimé | 0,425 |
| `zh` vs français, sur les **60 cartes où fr et en divergent le plus** | **0,737** |
| `zh` vs anglais, sur ces mêmes 60 cartes | 0,492 |
| `ratio zh/fr` | médiane 0,256 · écart-type **0,053** |
| `ratio zh/en` | médiane 0,318 · écart-type **0,124** |

Régression conjointe : `zh_len = 0,171·fr_len + 0,074·en_len + 2,6` — le coefficient **français domine**.

**Reformulation qui réconcilie les deux mesures** (et qui est la plus parcimonieuse) : le chinois descend du **français de l'époque de l'impression**, et l'anglais imprimé est un **témoin fidèle de ce même texte d'époque**. Sous cette hypothèse, la longueur suit le français (même couple de langues, compression stable) *et* le chinois coïncide avec l'anglais imprimé (les deux descendent du même original), sans qu'il y ait de filiation anglais → chinois.

⛔ **Non testé ici.** Consigné comme piste, pas comme conclusion — exactement comme T2 au grain ⑳. Ce qui est **établi**, et suffit au verdict : le chinois **ne correspond pas** au français courant sur ces 17 cartes, et l'anglais imprimé **corrobore** le texte qu'il porte à la place.

## Contrôle inverse — une fausse définition sort-elle en A ?

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

Deux étages, parce que ce grain-ci a un instrument de contenu en plus :

1. **Cellule délibérément inversée.** Plantée sur la même carte qu'au grain ⑳ (PK 814, « Faux dilemme »), le texte chinois disant l'inverse du français. **L'instrument mécanique est aveugle** (aucune particule, longueur dans la bande) → `FLAGS: -`. **La lecture le voit immédiatement** → **A**. Le contrôle établit donc les deux choses qu'il doit établir : un contresens est détectable **par la lecture**, et il ne l'est **par aucun écran**.
2. **Cellule tronquée à 50 %** (nouvel écran de contenu) → `LENDEV(-2,7)` : **détectée**. Contre-témoin : une cellule chinoise **saine** rend **aucun** `LENDEV`. C'est ce couple (détection / non-détection) qui autorise à dire que l'écran n'est pas une garde vacue — la leçon du grain ⑳, où un écran déclaré « non applicable » ne pouvait rien détecter par construction.

Ces deux contrôles tournent dans `--self-test`, avec les contrôles (a) cellule tronquée et (b) polarité inversée du grain ⑳.

## N'établit pas

- **Que les définitions chinoises soient sans défaut.** Lecture **unique**, par un **non-locuteur natif**. Un relecteur natif verra ce que cette passe n'a pas vu. L'établi est plus étroit : *aucun écart n'a été trouvé en lisant les 175 rangées*.
- **Que l'imprimé vienne au secours de ce zéro — il ne le peut pas.** C'est la différence de nature avec le grain ⑳ : pour le chinois, **aucune** référence imprimée n'existe. Les 168 appariements de la colonne « attachée » sont des appariements sur `path`/`text_fr` ; ils **ne portent aucun contenu chinois**. Ce grain est donc **plus faible** que le ⑳, et aucun compteur ne le masque.
- **Que l'anglais imprimé arbitre le chinois.** Il corrobore qu'un texte d'époque différait du français courant — il ne dit **pas** que le chinois est juste, ni même qu'il descend de lui (T2 : la mesure de longueur pointe dans l'autre sens).
- **Que les écrans corroborent le zéro.** `POLARITY` : 0 défaut réel sur 47 drapeaux. `LENDEV` : 1 levée, 0 défaut. Le contresens planté sort `FLAGS: -`.
- **Que le nouvel écran de contenu soit complet** : il manque les troncatures de cellules courtes (71 non détectées sur 175 à 50 % de troncature).
- **Rien sur** les 7 cartes sans aucune archive (PK 105, 362, 492, 1020, 1092, 1120, 1357), les 4 autres langues de la série (㉒ `ar`, ㉓ `fa`, ㉔ `pt`+`es`, ㉕ `en`), les champs `example_<lang>` / `link_<lang>`, ni les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu lors de la mesure, et la seule décidée l'a été par l'arbitre** : la correction de PK 989 (#1714, un caractère) suit l'arbitrage d'ai-01, elle n'est pas une proposition de la lane. Les 19 C-notes restent des observations ; l'arbitrage revient à ai-01.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang zh --out <sortie>
```

- Dénominateur **auto-vérifié** : l'instrument **s'arrête bruyamment** si le deck ne compte pas 175 cartes.
- **Non-régression du russe** : sur `--lang ru`, les **175 lignes par carte** de la nouvelle sortie sont **identiques** à celles de l'instrument du grain ⑳ ; la **seule** ligne qui change est le nouveau pied de page `REFERENCE EXPLOITABLE` (168/175 pour `ru` — donc sans perte). Comparaison faite contre l'instrument **commité** (`git show dd62f894:…`), pas de mémoire.
- ⛔ Le corpus du dépôt n'est **jamais** écrit : le contrôle inverse travaille sur une copie.
