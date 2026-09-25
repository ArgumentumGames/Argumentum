# #1499 G9-A — Alignement de cascade : 15 cellules fr changées, 7 traductions pas toutes

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **Dispatch :** pool v14, [#458 c.5838808923](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5838808923)
**Base :** `3d3532c1` (dérivée : dernier master avant le 22/09 13:00Z) · **Tête :** `906284ab` · **⛔ 0 écriture.**

## 0. Résultat

> ⚠️ **Erratum (26/09, avant tout merge)** — la première version de ce dossier classait **1361 `example` DÉSALIGNÉ (ar)**. C'était un **faux positif** : le périmètre exécuté de [#1546](https://github.com/ArgumentumGames/Argumentum/pull/1546) (Q-12 3a) dit explicitement **« ar inchangé (l'arabe standard ne distingue pas tu/vous) »**, arbitré par ai-01 ([c.5825475175](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5825475175)) et **épinglé par la garde** `Pk1361_Example_Matches_The_Polite_Register_Decision` (« ar and en deliberately unchanged, pinned »). Vu en exécutant G9-A-W : une écriture à 4 cellules a **rougi** cette garde — le rouge était le signal, pas l'obstacle. L'écriture a été révertée depuis le backup, jamais poussée ; 1361 passe en **VOULU**. Le point de mesure lui-même (ar inchangé pendant que 6 langues changent) reste exact.

| Classe | Cellules |
|---|---|
| **ALIGNÉ** (8) | 134, 176, 632, 658, 666, 713, 799, 888 (`text`) |
| **VOULU** (5) | 2, 598, 973 (`text`) · 848 (`example`) · **1361 (`example`, ar — erratum)** |
| **DÉSALIGNÉ** (2) | **492** (`text`, es/fa/zh) · **796** (`example`, fa) |

La re-dérivation (outil `tools/1499-g9a-alignement-cascade.py`, garde `EXPECTED_N = 15`) rend **exactement les 15 candidats** de la mesure ai-01, même liste, même répartition changés/inchangés. La base est **dérivée par date**, pas recitée.

## 1. Méthode

- **Candidat** : PK × champ (`text|desc|example`) tel que la cellule `fr` diffère base→tête **et** que moins de 8 langues changent.
- **Classes** (DoD du dispatch) : `ALIGNÉ` = la traduction disait déjà le sens retenu · `VOULU` = décision nommée, avec son lien · `DÉSALIGNÉ` = la traduction porte l'ancien sens ou l'ancien registre.
- Le classement des `text` est une **lecture** (les 8 textes de chaque cellule sont produits par `--dump` et cités ci-dessous à l'essentiel) ; le classement de 1361 est **mécanique** (marqueurs de registre par langue, `--self-test`).
- ⚠️ Instrument noté au passage : `git log -S "Généralisation hâtive"` est **aveugle** sur le renommage PK 598 — la chaîne **déménage** dans le même commit (#1522 retire « Généralisation hâtive » de PK 2 et l'ajoute à PK 598), le compte d'occurrences ne change pas, `-S` ne voit rien. `git log -G` ou une lecture PK-par-PK (faite ici) voit le geste. Même famille que « portée de sonde = portée du claim ».

## 2. Les 15 cellules

Le geste dominant est **#1522 (G1-W, 23/09)** : « écrire 40 titres décidés + aligner 482 bandeaux » — la passe de titres fr qui a suivi les décisions G1/Q-11.

| # | PK | champ | changés | classe | lecture |
|---|---|---|---|---|---|
| 1 | 2 | text | fr,en,ru,pt,es,ar | **VOULU** | Q-11 (11a) : PK 2 devient `sloppyArgument`. fr « Généralisation hâtive »→« Argument bâclé », en→*Sloppy argument*, ru/pt/es/ar suivent. **fa/zh disaient déjà « bâclé »** (استدلال شلخته · 草率论点) — inchangés parce qu'ils portaient déjà le sens retenu. Lien : décision Q-11 (dashboard), geste [#1522](https://github.com/ArgumentumGames/Argumentum/pull/1522) |
| 2 | 134 | text | fr | **ALIGNÉ** | fr « Sophisme du jeu »→« Sophisme ludique » : le fr était l'exception, les 7 disaient déjà « ludique » (*Ludic fallacy* · Игровой софизм · Falácia lúdica · Falacia lúdica · مغالطة لوديكية · سفسطه بازی · 游戏谬论) |
| 3 | 176 | text | fr,fa | **ALIGNÉ** | fr « Technique »→« Procédé rhétorique » rejoint *Rhetorical device* / *Dispositivo Retórico* / أسلوب بلاغي ; fa suit (شگرد→دستگاه بلاغی). Les 6 autres disaient déjà « procédé/dispositif » |
| 4 | 492 | text | fr | **DÉSALIGNÉ** | fr « Auto-victimisation »→« Jouer la victime » (rejoint en *Playing the Victim* · ru Игра в жертву · pt Jogar a Vítima · ar لعب دور الضحية). **es « Auto-victimización » · fa خود-قربانی‌گری · zh 自我受害 portent encore l'ancien titre fr** (« auto-victimisation »), non touchés par #1522 |
| 5 | 598 | text | fr,en,ru,pt,es,ar | **VOULU** | Q-11 (11a) : *hastyGeneralization* rattaché à PK 598. fr « Induction hâtive »→« Généralisation hâtive », en/ru/pt/es/ar suivent. **fa تعمیم عجولانه · zh 轻率概括 disaient déjà « généralisation hâtive »**. Lien : Q-11, geste [#1522](https://github.com/ArgumentumGames/Argumentum/pull/1522) |
| 6 | 632 | text | fr | **ALIGNÉ** | fr « Interprétation quantitative erronée »→« Mauvaise interprétation » : le fr était seul à porter « quantitative », les 7 disaient déjà « interprétation » (*Interpretation fallacy* · Неправильная интерпретация · سوء التفسیر…) |
| 7 | 658 | text | fr | **ALIGNÉ** | fr « fallacieux »→« trompeur » : les 7 disaient déjà « trompeur » (*Deceptive infinity* · Обманчивая бесконечность · Infinito Enganoso · اللانهاية الخادعة…) |
| 8 | 666 | text | fr | **ALIGNÉ** | fr « Conclusion mathématique invalide »→« Résultat invalide » : les 7 disaient déjà « résultat invalide » (*Invalid result* · Resultado Inválido · نتیجه نامعتبر · 无效结果) |
| 9 | 713 | text | fr,ru,pt,ar,fa,zh | **ALIGNÉ** | fr « Deux torts font un droit »→« Sophisme de la double faute », ru/pt/ar/fa/zh suivent le renommage descriptif. **en *Two wrongs make a right* et es « Dos errores hacen un acierto » gardent l'idiome consacré** — qui porte le sens retenu (même logique que *Moving the goalposts*, cf. ligne 14) |
| 10 | 796 | example | fr,fa | **DÉSALIGNÉ** | Q-16 (c) : glose rétablie à l'octet sur 796 ×6 (en,ru,pt,es,ar,zh — inchangés ici) et 848:zh ; « **796 fr reste coupée** ». ⚠️ **fa : glose coupée par #1546 et ABSENTE des 7 restaurées de [#1549](https://github.com/ArgumentumGames/Argumentum/pull/1549)** — ni la restauration ni l'exception fr ne le nomment. Les 6 sœurs ont leur glose, fa non : orphelin d'exécution |
| 11 | 799 | text | fr,en,ru | **ALIGNÉ** | fr « Définition biaisée »→« Définition inexacte », en/ru suivent. pt/es/ar/fa/zh disaient déjà « inexacte » (*Definição Inexata* · Definición inexacta · تعريف غير دقيق · تعریف نادرست · 不准确的定义) |
| 12 | 848 | example | fr,en,ru,pt,es,ar,fa | **VOULU** | G2-C/Q-16 (c) : les 11 gloses didactiques restent coupées (garde d'usine `FallaciesGlossRegisterGate`), **zh restauré** (Q-16 c, #1549). Liens : [#1546](https://github.com/ArgumentumGames/Argumentum/pull/1546) (coupe) · [#1549](https://github.com/ArgumentumGames/Argumentum/pull/1549) (restauration) |
| 13 | 888 | text | fr,en,ar | **ALIGNÉ** | fr « Présentation trompeuse des faits »→« Arranger les faits », en→*Spin doctoring*, ar suit (تلفيق الحقائق). ru Подгонка фактов · es « Arreglar los hechos » · fa دستکاری حقایق · zh 操纵事实 portaient déjà la famille « arranger/manipuler les faits » |
| 14 | 973 | text | fr | **VOULU** | fr « Déplacement des critères »→« Changement de cap ». **en garde *Moving the goalposts*** (nom consacré), **ru Изменение направления et fa تغییر جهت disaient déjà « changement de direction »**, es garde l'idiome (Mover los postes de la portería). Lecture nommée par ai-01 dans le dispatch (écart voulu) — [#458 c.5838808923](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5838808923) |
| 15 | 1361 | example | fr,ru,pt,es,fa,zh | **VOULU** *(erratum — d'abord classé DÉSALIGNÉ)* | Q-12 : l'exemple vouvoyé (geste [#1546](https://github.com/ArgumentumGames/Argumentum/pull/1546)) — fr/ru/pt/es/fa/zh passés à l'adresse formelle (vous · вас · ouvi-lo/vi-o · Oyéndole/le vi · ـتان/می‌کردید · 您). **ar inchangé par décision** : « l'arabe standard ne distingue pas tu/vous » (#1546, arbitrage ai-01) — le pluriel كم y est un nombre, pas une politesse. en exempt par construction. Épinglé par la garde `Pk1361_Example_Matches_The_Polite_Register_Decision` |

## 3. Les 2 DÉSALIGNÉ — ce que G9-A-W proposerait

| PK | langue(s) | état porté | geste proposé (à décider en G9-A-W) |
|---|---|---|---|
| 492 | es · fa · zh | la famille « auto-victimisation » (l'ancien titre fr) | reprendre la famille « jouer la victime » des 4 langues alignées — es « Hacerse la víctima » (registre des titres voisins), zh « 扮演受害者 », fa « قربانی جلوه دادن » |
| 796 | fa | glose coupée, absente des 7 restaurées | **restauration à l'octet depuis l'historique** (base `3d3532c1`) — geste mécanique, même forme que #1549 ; c'est l'omission la plus probable de la worklist G2-C-W2 — **à confirmer par ai-01 avant écriture** |

⛔ Aucune écriture dans ce grain. G9-A-W est un grain séparé, merge après le bundle (même porte que #1562/#1563).

## 4. Contrôle (DoD : « une cellule déjà alignée, modifiée dans une copie, doit sortir DÉSALIGNÉ »)

Le classement de 1361 est mécanique : marqueurs formels/informels par langue (`FORMAL`/`INFORMAL` dans l'outil), fr = cible, en et ar exempt (ar : par décision #1546 — le classificateur naïf qui ne l'exemptait pas a produit le faux positif de l'erratum). `--self-test`, sur **littéraux** :

```
[PASS] HEAD (ar : exempt par décision #1546)  -> retardataires aucun : ALIGNÉ (témoin)
[PASS] copie : pt (aligné) reverti            -> retardataires ['pt'] : DÉSALIGNÉ (le contrôle DoD)
[PASS] copie : fr (cible) reverti             -> retardataires ['fr'] : ROUGE instrument
```

Le témoin compte autant que la mutation : sans lui, un classificateur rouge par construction passerait pour un classificateur qui voit.

## 5. Limites, nommées

- Le classement des 13 cellules `text` est une **lecture** appuyée sur les 8 textes (`--dump` les reproduit) ; les marqueurs mécaniques ne couvrent que le registre (1361). Un relecteur peut contester une classe — le tableau cite ce qui l'a décidée.
- **492 : « auto-victimisation » vs « jouer la victime »** désignent le même concept ; la classe DÉSALIGNÉ suit la lettre du DoD (« porte l'ancien sens » — es/fa/zh traduisent l'ancien titre fr). Si le maintien des trois est une préférence de registre, G9-A-W peut le consigner au lieu d'écrire.
- **713/973 : l'idiome consacré gardé** est classé ALIGNÉ/VOULU par la lecture d'ai-01 (goalposts). Une harmonisation inverse (tout descriptif) serait un autre grain, non demandé.
- **796:fa** : la restauration proposée suppose que Q-16 (c) visait « restaurer sauf 796 fr » — le texte de la décision (« ×6 » + « 848 zh ») énumère 7 cellules et ne nomme pas fa ; l'hypothèse « fa oublié » est la plus probable, à confirmer par ai-01 avant l'écriture.
- **Le faux positif 1361, et sa leçon** : la mesure « ar inchangé pendant que 6 langues changent » est exacte ; c'est la **classe** qui était fausse. Un écart peut être une décision déjà documentée — **avant de classer DÉSALIGNÉ une cellule, chercher la garde qui l'épingle** (`grep` du PK dans les tests du dépôt) : ici `Pk1361_Example_Matches_The_Polite_Register_Decision` disait « deliberately unchanged, pinned » noir sur blanc. Le même geste a évité à 796:fa une écriture hors arbitrage.

## 6. Exécutable

```bash
python tools/1499-g9a-alignement-cascade.py              # 15 candidats, garde EXPECTED_N
python tools/1499-g9a-alignement-cascade.py --dump       # les 8 textes par cellule (g9a-dump.md, hors commit)
python tools/1499-g9a-alignement-cascade.py --self-test  # contrôle registre 1361 : 3/3
```
