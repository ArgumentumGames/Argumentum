# Essai gpt-6-luna — détection de défauts de traduction sur Fallacies (#1520)

| | |
|---|---|
| **Date** | 2026-11-10 / 11 |
| **Corpus figé** | SHA `25327fef` (extrait par `git show` — aucune écriture dans `Cards/**`) |
| **Périmètre** | Fallacies, 5 grappes — 5.2 (13 nœuds), 4.1 (29), 6.1 (85), 2.1 (123), 6.3 (257) = 507 nœuds, 8 langues |
| **Bras testé** | `gpt-6-luna` (0,10 $/0,50 $ par 1M tok.) — juge et témoin `gpt-5.6-sol` (4 $/20 $), témoin `claude-haiku-5-5` (0,10 $/0,50 $) |
| **Dépense totale** | **1,8205 $ / 25 $** (7,3 % du plafond) — journal complet : `resultats/spend.jsonl` |
| **Livrables annexes** | instruments/ (6 fichiers rejouables), `planted-defects.json` (témoin), `resultats/` (brut + agrégé) |

Protocole complet et contraintes : issue #1520. Point d'étape posté avant la phase lourde : commentaire [c.6783335148](https://github.com/ArgumentumGames/Argumentum/issues/1520#issuecomment-6783335148) (sonde gratuite, étalonnage dans sa porte de 0,05 $ — tenu à 0,0379 $, passe propre). Chaque proposition embarque l'empreinte SHA-256 de la cellule FR + de la cellule cible au SHA figé (`empreinte_fige` dans `resultats/selector-runs.jsonl`).

---

## 1. Résultat global

| Mesure | Valeur |
|---|---|
| Propositions jugées (sélecteur Luna) | **374** sur 210 émises en phases mesurées + 29 en incrémental |
| Verdicts « meilleur » | **361** → précision **96,5 %** |
| Verdicts « pire » (dégradation) | **12** → dégradation **3,2 %** ; « équivalent » : 1 |
| Rappel sur 30 défauts plantés | **27/30 = 90 %**, rang moyen **2,65** ; 51/51 retrouvailles citent la cellule plantée à l'identique |
| Corrections utiles par dollar | **361 corrections / 1,8205 $ = 0,005 $ par correction** (juge et étalonnage compris) |
| Écritures dans `Cards/**` | **0** — tout l'essai travaille sur copies en mémoire |

Le solde de propositions (479 runs sélection au total) couvre les passes incrémentales auto-jugées (§4) et les répétitions a/c du corpus planté (rappel mesuré sur les deux tailles).

## 2. Grappe × taille de lot (corpus propre, juge sol)

Lot `a` = grappe entière · `b` = 50 nœuds (6.3 seulement) · `c` = 12 nœuds.

| Grappe | Lot | Runs | Props | Meilleur | Pire | Précision | Dégradation |
|---|---|---:|---:|---:|---:|---:|---:|
| 5.2 (13) | a | 1 | 4 | 4 | 0 | 100 % | 0 % |
| 4.1 (29) | a | 1 | 4 | 4 | 0 | 100 % | 0 % |
| 6.1 (85) | a | 1 | 6 | 6 | 0 | 100 % | 0 % |
| 2.1 (123) | a | 1 | 7 | 7 | 0 | 100 % | 0 % |
| 2.1 | c (×11) | 11 | 37 | 35 | 2 | 94,6 % | 5,4 % |
| 6.3 (257) | a | 1 | 4 | 4 | 0 | 100 % | 0 % |
| 6.3 | b (×6) | 6 | 30 | 28 | 2 | 93,3 % | 6,7 % |
| 6.3 | c (×22) | 22 | 71 | 69 | 2 | **97,2 %** | 2,8 % |

**Lecture** : les petits paquets (c, 12 nœuds) font émettre **5 à 10 fois plus de propositions par nœud** que la grappe entière, à précision quasi égale (94,6–97,4 % contre 100 % — mais sur des échantillons de 4–7 propositions au lot a, trop petits pour trancher). Le lot b (50) est le moins bon (93,3 %) ; 22 paquets de 12 sur 6.3 donnent la meilleure précision absolue de l'axe. Sévérités déclarées (corpus propre, 167 props) : 10 grossier, 111 moyen, 46 nuance.

Corpus planté (rappel) : lot a 33 props (97,0 % meilleur), lot c 152 props (97,4 %) — détail au §3.

## 3. Rappel — 30 défauts plantés

30 défauts (6 classes : FR laissé, mauvais registre, contresens, faute lexicale, terminologie, calque) plantés dans une copie du corpus, manifeste dans `planted-defects.json`.

- **27/30 retrouvés (90 %)**, rang moyen **2,65** dans la liste des propositions.
- **51 retrouvailles au total** (un défaut peut être trouvé au lot a ET au lot c) — **les 51 citent la cellule plantée à l'identique** (`extrait_fautif` == cellule plantée) ; dans tous les cas la correction proposée diffère du texte planté.
- **3 manqués** :

| pk | Langue | Champ | Classe plantée |
|---|---|---|---|
| 708 | pt | desc | cl. 3 — texte FR laissé dans la cellule cible |
| 274 | zh | desc | cl. 5 — mauvais registre (tutoiement / « You ») |
| 1212 | pt | example | cl. 1 — contresens (négation inversée) |

## 4. Passes incrémentales (6.1 + 2.1, ≤ 3 passes, seules les « meilleur » appliquées)

| Passe | Props | Précision | Dégradation | Appliquées | Sévérités (g/m/n) |
|---|---:|---:|---:|---:|---|
| P1 | 9 | 88,9 % | 0 % | 8 | 4/4/1 |
| P2 | 9 | 78 % | 22 % | 7 | 2/6/1 |
| P3 | 11 | 91 % | 9 % | 10 | 2/7/2 |

**Arrêt sur la limite des 3 passes**, pas sur un critère de qualité : la précision est remontée en P3 et le flux n'est pas devenu « nuance seulement ». Les 25 corrections appliquées ne le sont que dans la copie de travail de l'essai — rien n'atterrit dans le dépôt. Coût incrémental total : 0,136 $.

## 5. Luna vs Sol vs Haiku (grappes 4.1 + 5.2, lot a, juge sol)

| Bras | Props | Précision | Dégradation | Coût sélecteur |
|---|---:|---:|---:|---:|
| **gpt-6-luna** | 8 | **100 %** | 0 % | 0,0132 $ (part des 2 runs) |
| gpt-5.6-sol | 16 | 87,5 % | 12,5 % | **0,1914 $** |
| claude-haiku-5-5 | 10 | 100 % | 0 % | 0,0084 $ |

- Sol émet **2 fois plus de propositions** mais dégrade 12,5 % du temps, pour un coût sélecteur **~40× Luna** sur le même périmètre — et c'est le juge (sol) qui se juge lui-même ici : l'auto-faveur possible joue **contre** Luna, qui gagne quand même.
- Les trois bras **ne voient pas les mêmes défauts** : sur 4.1, union = 14 propositions pour 8+9+5 émises (recouvrement Luna∩sol = 2, Luna∩haiku = 1, sol∩haiku = 1). Un balayage multi-modèle est complémentaire, pas redondant.
- Haiku (0,0084 $) est parfait sur ce micro-échantillon (10 props, 42 nœuds) — échantillon trop petit pour opposer sa 100 % à celle de Luna ; il n'a pas été testé sur l'axe ni le rappel.

## 6. Coûts

| Poste | $ |
|---|---:|
| Juge (374 verdicts, sol) | 1,2524 |
| Témoins sol (16 props) + haiku (10 props) | 0,1914 + 0,0084 |
| Étalonnage (2 passes, cf. annexe A) | 0,0379 |
| Sélecteur Luna — clean + axe (44 runs) | 0,1005 |
| Sélecteur Luna — plantés (51 runs) | 0,0942 |
| Incrémental (sélecteur + juge inline, 3 passes) | 0,1358 |
| **Total** | **1,8205 / 25 $** |

Par bras : luna **0,237 $** · sol **1,575 $** · haiku **0,0084 $** — le juge (sol) représente 69 % de la dépense ; le bras testé coûte 13 %. Une campagne sans témoin sol serait ~0,45 $.

## 7. Exemples (textes entiers)

24 exemples ci-dessous, générés mécaniquement depuis `resultats/aggregate.json` (aucun texte retapé à la main) : 12 « meilleur » (correction retenue) et 12 « pire » (dégradation évitée). Chacun porte : grappe/lot, cellule (pk, langue, champ), la référence FR, le texte cible avant, la proposition, et la raison du juge.

### 7.1 « Meilleur » — correction retenue (12 exemples)

#### Exemple 1 — meilleur · axis-2.1-c/2.1__c01 · pk 177 · en/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction remplace l’idée d’imposer implicitement le cadre et les termes du débat par une technique particulière — le langage chargé d’émotion ou de connotations — qui n’est pas mentionnée dans le texte français.
- **Raison du juge** : B restitue fidèlement l’adresse à « vous », ainsi que l’idée d’imposer implicitement le cadre et les termes du débat, tandis que A ajoute à tort la notion de langage émotionnel ou connoté et omet les « termes ».

**Référence FR** :

> Vous vous exprimez de manière à imposer implicitement le cadre et les termes du débat.

**Texte cible avant (en)** :

> Using emotionally charged or connotative language to implicitly shape the debate's framework.

**Proposition (appliquée comme correction)** :

> Expressing yourself in a way that implicitly imposes the framework and terms of the debate.

#### Exemple 2 — meilleur · axis-2.1-c/2.1__c01 · pk 180 · zh/example

- **Sévérité déclarée** : nuance · **Problème déclaré** : La traduction omet l’idée de « combien » la situation serait meilleure et celle que cela concerne tout le monde.
- **Raison du juge** : B restitue fidèlement la question rhétorique, le « vous », l’idée de « nous tous » et « bien mieux », tandis que A est maladroite et omet la dimension collective.

**Référence FR** :

> Dois-je vous rappeler combien nous serions tous bien mieux sans ces taxes supplémentaires ?

**Texte cible avant (zh)** :

> 我需要提醒您我们没有这些额外税款会更好吗？

**Proposition (appliquée comme correction)** :

> 难道还需要我提醒您，没有这些额外税收，我们大家会过得好得多吗？

#### Exemple 3 — meilleur · axis-2.1-c/2.1__c02 · pk 193 · es/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction dit que les termes servent à impressionner, alors que le français précise qu’ils servent à intimider, ce qui brouille la distinction avec le jargon prestigieux.
- **Raison du juge** : B restitue fidèlement « volontairement » et surtout « pour intimider », tandis que A remplace ce sens par « impresionar ».

**Référence FR** :

> Vous employez volontairement des termes complexes ou prestigieux pour intimider votre auditoire.

**Texte cible avant (es)** :

> Utilizas términos complejos a propósito para impresionar a tu audiencia.

**Proposition (appliquée comme correction)** :

> Utilizas deliberadamente términos complejos para intimidar a tu audiencia.

#### Exemple 4 — meilleur · axis-2.1-c/2.1__c02 · pk 194 · ru/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction remplace l’intimidation par le fait d’impressionner l’auditoire, ce qui atténue l’intention décrite en français.
- **Raison du juge** : A restitue fidèlement le but d’intimidation (« чтобы запугать »), tandis que B remplace ce sens par celui d’impressionner l’auditoire.

**Référence FR** :

> Vous utilisez des expressions latines pour intimider votre public.

**Texte cible avant (ru)** :

> Вы впечатляете аудиторию, используя латинские выражения.

**Proposition (appliquée comme correction)** :

> Вы используете латинские выражения, чтобы запугать свою аудиторию.

#### Exemple 5 — meilleur · axis-2.1-c/2.1__c02 · pk 194 · es/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction parle d’impressionner l’auditoire au lieu de l’intimider, ce qui atténue l’intention décrite en français.
- **Raison du juge** : A restitue fidèlement le « vous » impersonnel et le sens d’intimider, tandis que B est une formulation infinitive amputée et remplace « intimider » par « impressionner ».

**Référence FR** :

> Vous utilisez des expressions latines pour intimider votre public.

**Texte cible avant (es)** :

> Impresionar a la audiencia con el uso de frases en latín.

**Proposition (appliquée comme correction)** :

> Utilizas expresiones latinas para intimidar a tu audiencia.

#### Exemple 6 — meilleur · axis-2.1-c/2.1__c03 · pk 211 · pt/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : A tradução muda a ideia de invocar muitas fontes para dar autoridade a um argumento fraco e usa «fontes autoritárias», que significa fontes autoritárias no sentido de exercer autoridade, não fontes que conferem autoridade.
- **Raison du juge** : La version B traduit fidèlement et complètement le français, tandis que A modifie le sens en évoquant des sources autoritaires et ajoute l’idée d’indépendance par rapport à leur pertinence ou validité.

**Référence FR** :

> Vous invoquez un grand nombre de sources pour donner de l’autorité à un argument faible.

**Texte cible avant (pt)** :

> Alega que um argumento é apoiado por muitas fontes autoritárias, independentemente de sua relevância ou validade reais.

**Proposition (appliquée comme correction)** :

> Você invoca um grande número de fontes para dar autoridade a um argumento fraco.

#### Exemple 7 — meilleur · axis-2.1-c/2.1__c03 · pk 204 · ar/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : A tradução diz que os números reforçam um argumento fraco, em vez de impressionar para ocultar a falta de solidez do argumento.
- **Raison du juge** : A restitue fidèlement l’idée d’impressionner par de gros chiffres afin de masquer la faiblesse de l’argument, tandis que B transforme ce but en renforcement de l’argument.

**Référence FR** :

> Vous impressionnez par des chiffres énormes pour masquer le manque de solidité de votre argument.

**Texte cible avant (ar)** :

> أنت تستخدم أرقاماً كبيرة للغاية لتعزيز حجتك الضعيفة.

**Proposition (appliquée comme correction)** :

> أنت تبهر بأرقام ضخمة لإخفاء ضعف حجتك.

#### Exemple 8 — meilleur · axis-2.1-c/2.1__c04 · pk 212 · fa/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : عبارت «به تعریف قوی اخلاقی می‌پردازند» لا يعني أن الكلمات تحمل إيحاءً أخلاقيًا قويًا، كما أن مرجع «نتیجه‌گیری آن‌ها» غير واضح.
- **Raison du juge** : نسخهٔ A معنای متن فرانسه را دقیق و طبیعی منتقل می‌کند؛ نسخهٔ B «forte connotation morale» را نادرست ترجمه کرده و با افزودن مفهوم «دیکته کردن» و ضمیر «آن‌ها» معنا را منحرف می‌کند.

**Référence FR** :

> Votre discours repose sur des termes à forte connotation morale, qui orientent déjà la conclusion.

**Texte cible avant (fa)** :

> گفتار شما بر اساس کلماتی است که به تعریف قوی اخلاقی می‌پردازند و نتیجه‌گیری آن‌ها را دیکته می‌کنند.

**Proposition (appliquée comme correction)** :

> گفتار شما بر واژه‌هایی با بار معنایی اخلاقیِ قوی تکیه دارد که از پیش نتیجه‌گیری را جهت می‌دهند.

#### Exemple 9 — meilleur · axis-2.1-c/2.1__c05 · pk 235 · ar/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : A tradução transforma «une idée sérieuse» em «فكرة ساخرة» («uma ideia satírica»), invertendo a característica da ideia imitada.
- **Raison du juge** : B restitue correctement la manière satirique appliquée à l’imitation, tandis que A qualifie à tort l’idée elle-même de « sarcastique ».

**Référence FR** :

> Vous imitez de façon satirique un discours ou une idée sérieuse pour la tourner en dérision.

**Texte cible avant (ar)** :

> تقليد خطاب جاد أو فكرة ساخرة للسخرية منها.

**Proposition (appliquée comme correction)** :

> تقليد خطاب أو فكرة جادة بطريقة ساخرة للسخرية منها.

#### Exemple 10 — meilleur · axis-2.1-c/2.1__c05 · pk 234 · pt/desc

- **Sévérité déclarée** : nuance · **Problème déclaré** : «Os senhores» significa «you gentlemen» e muda o tratamento singular genérico do original para um plural masculino específico.
- **Raison du juge** : A restitue mieux le « vous » générique et emploie correctement « zombar de », tandis que « Os senhores » en B est inutilement formel et pluriel.

**Référence FR** :

> Vous amplifiez ou tordiez certaines caractéristiques pour moquer ou critiquer quelque chose ou quelqu’un.

**Texte cible avant (pt)** :

> Os senhores amplificam ou distorcem certas características para zombar ou criticar algo ou alguém.

**Proposition (appliquée comme correction)** :

> Você amplifica ou distorce certas características para zombar de algo ou alguém, ou para criticá-lo.

#### Exemple 11 — meilleur · axis-2.1-c/2.1__c06 · pk 243 · fa/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : «به‌طور غیر مستقیم» signifie «indirectement», alors que le français indique que les propos ne doivent pas être interprétés au sens littéral.
- **Raison du juge** : A restitue fidèlement l’idée que les propos ne doivent pas être pris au pied de la lettre, tandis que B remplace « non littérale » par « indirectement », ce qui modifie le sens.

**Référence FR** :

> Vous faites comprendre subtilement que vos propos sont à prendre de façon non littérale.

**Texte cible avant (fa)** :

> شما به‌طور لطیف نشان می‌دهید که گفته‌هایتان باید بطور غیر مستقیم تفسیر شود.

**Proposition (appliquée comme correction)** :

> شما به‌طور ظریف نشان می‌دهید که گفته‌هایتان نباید به‌صورت تحت‌اللفظی برداشت شوند.

#### Exemple 12 — meilleur · axis-2.1-c/2.1__c06 · pk 241 · ru/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : «В яростной манере» signifie «avec fureur» et ne rend pas l’ironie mordante du texte français.
- **Raison du juge** : «Язвительная ирония» передаёт смысл «ironie mordante» точнее и естественнее, чем неидиоматичное «ирония в яростной манере».

**Référence FR** :

> Vous utilisez une ironie mordante pour ridiculiser ou critiquer.

**Texte cible avant (ru)** :

> Вы используете иронию в яростной манере, чтобы высмеять или критиковать.

**Proposition (appliquée comme correction)** :

> Вы используете язвительную иронию, чтобы высмеивать или критиковать.

### 7.2 « Pire » — dégradation évitée (12 exemples)

#### Exemple 1 — pire · axis-2.1-c/2.1__c05 · pk 232 · pt/example

- **Sévérité déclarée** : moyen · **Problème déclaré** : A tradução elimina o jogo com «sujet»: «sobre você» não prepara o sentido de «súdito», e a resposta deixa de funcionar como trocadilho.
- **Raison du juge** : A restitue fidèlement et naturellement le sens de la réplique, tandis que B altère « à votre sujet » en « sobre o seu assunto » et ajoute une explication métalinguistique absente du français.

**Référence FR** :

> Vous me demandez de faire un mot d’esprit à votre sujet, mais, Sire, le roi n’est pas un sujet.

**Texte cible avant (pt)** :

> Você me pede para fazer um comentário perspicaz sobre você, mas Senhor, o rei não é um súdito.

**Proposition (rejetée comme dégradation)** :

> Você me pede uma frase espirituosa sobre o seu «assunto», mas, senhor, o rei não é um «súdito» — em francês, «sujet» pode significar tanto «assunto» quanto «súdito».

#### Exemple 2 — pire · axis-2.1-c/2.1__c07 · pk 253 · zh/example

- **Sévérité déclarée** : moyen · **Problème déclaré** : La phrase transmet l’idée d’une condamnation à perpétuité, mais ne montre plus la modification ludique du mot qui illustre le métaplasme.
- **Raison du juge** : A准确自然地用“无期”表达“perpète”（无期徒刑）；B的“永无期”并非规范说法，且感叹语气偏离原文。

**Référence FR** :

> Las, il en a pris pour perpète.

**Texte cible avant (zh)** :

> 唉，他这下被判了个“无期”。

**Proposition (rejetée comme dégradation)** :

> 唉，他这下可算是判了个“永无期”！

#### Exemple 3 — pire · axis-6.3-b/6.3__b01 · pk 1053 · zh/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La phrase chinoise est mal construite et rend difficile à comprendre l’idée que vous pensez mieux comprendre les autres qu’ils ne vous comprennent.
- **Raison du juge** : B restitue directement la comparaison réciproque (« comprendre les autres mieux qu’ils ne vous comprennent »), tandis que A introduit l’idée que vous les comprenez mieux que d’autres personnes.

**Référence FR** :

> Vous croyez comprendre les autres mieux qu’ils ne vous comprennent.

**Texte cible avant (zh)** :

> 你认为自己比别人理解你更理解他们。

**Proposition (rejetée comme dégradation)** :

> 你认为自己比别人更了解他们，而他们并没有那么了解你。

#### Exemple 4 — pire · axis-6.3-b/6.3__b01 · pk 1025 · es/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction ajoute une perte d’objectivité scientifique qui n’est pas dans le français et omet l’idée de réflexes liés à la perspective humaine.
- **Raison du juge** : La version B restitue fidèlement l’attention sélective selon les intérêts ou l’état émotionnel, tandis que la version A exprime une idée entièrement différente.

**Référence FR** :

> Vous accordez plus d’attention à certains types d’informations en fonction de vos intérêts ou de votre état émotionnel.

**Texte cible avant (es)** :

> Prestáis más atención a ciertos tipos de información según vuestros intereses o estado emocional.

**Proposition (rejetée comme dégradation)** :

> Tus argumentos reflejan las limitaciones y los reflejos propios de la perspectiva humana.

#### Exemple 5 — pire · axis-6.3-c/6.3__c06 · pk 1083 · fa/desc

- **Sévérité déclarée** : nuance · **Problème déclaré** : La traduction omet l’idée de données ou d’événements « sans lien », présente dans le français en plus de celle du caractère aléatoire.
- **Raison du juge** : A est plus fidèle au texte français, tandis que B ajoute « نامرتبط » (« non liés »), notion absente de la référence.

**Référence FR** :

> Vous identifiez des liens ou des motifs dans des données ou des événements aléatoires, en créant du sens là où il n’y en a pas.

**Texte cible avant (fa)** :

> شما در داده‌ها یا رویدادهای تصادفی اتصالات یا الگوهایی را شناسایی می‌کنید، معانی‌ای را ایجاد می‌کنید که وجود ندارند.

**Proposition (rejetée comme dégradation)** :

> شما در داده‌ها یا رویدادهای تصادفی یا نامرتبط، ارتباط‌ها یا الگوهایی را شناسایی می‌کنید و معناهایی را ایجاد می‌کنید که وجود ندارند.

#### Exemple 6 — pire · axis-6.3-c/6.3__c08 · pk 1115 · ar/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : الجملة تكرر معنى التأثير مرتين في نهايتها، ما يجعلها مضطربة مقارنةً بالمعنى الفرنسي.
- **Raison du juge** : La version B restitue « contrôler ou influencer », tandis que la version A omet l’idée de pouvoir influencer les événements.

**Référence FR** :

> Vous pensez pouvoir contrôler ou influencer des événements sur lesquels vous n’avez en réalité que peu ou pas d’influence.

**Texte cible avant (ar)** :

> تعتقدون أنكم تستطيعون التحكم في أحداث لا تملكون في الواقع إلا تأثيرًا ضئيلًا عليها أو لا تملكون أي تأثير عليها، أو التأثير فيها.

**Proposition (rejetée comme dégradation)** :

> تعتقدون أنكم تستطيعون التحكم في أحداث لا تملكون في الواقع إلا تأثيرًا ضئيلًا عليها أو لا تملكون أي تأثير عليها.

#### Exemple 7 — pire · planted-a/6.1__a · pk 910 · zh/desc

- **Sévérité déclarée** : moyen · **Problème déclaré** : Le mot français « interlocuteur » a été laissé sans traduction dans la cellule chinoise.
- **Raison du juge** : A conserve l’idée que l’argument repose sur de fausses informations sciemment présentées comme des actualités, tandis que B ajoute une diffusion et une intention de tromper absentes du français.

**Référence FR** :

> Votre argument s’appuie sur de fausses informations sciemment présentées comme des actualités.

**Texte cible avant (zh)** :

> 你的论点依靠故意作为新闻报道的虚假信息。

**Proposition (rejetée comme dégradation)** :

> 你明知信息不正确，却仍将其传播，以此误导对方。

#### Exemple 8 — pire · planted-c/2.1__c05 · pk 230 · pt/example

- **Sévérité déclarée** : moyen · **Problème déclaré** : La traduction ne donne que le sens littéral et omet l’explication du contrepèterie obscène, indispensable pour comprendre l’exemple en portugais.
- **Raison du juge** : A traduit fidèlement et intégralement la phrase française, tandis que B ajoute une explication et un jeu de mots obscène absents du texte de référence.

**Référence FR** :

> Le général est arrivé à pied par la Chine.

**Texte cible avant (pt)** :

> O general chegou a pé pela China

**Proposition (rejetée comme dégradation)** :

> O general chegou a pé pela China — em francês, « à pied par la Chine »; trocando os sons, obtém-se a expressão obscena « à chier par la pine ».

#### Exemple 9 — pire · planted-c/4.1__c02 · pk 713 · zh/desc

- **Sévérité déclarée** : nuance · **Problème déclaré** : «正确» signifie «correct» plutôt que «juste» au sens de résultat équitable.
- **Raison du juge** : «正确的结果» traduit fidèlement «un résultat juste» au sens de résultat correct, tandis que «公正的结果» évoque à tort un résultat équitable.

**Référence FR** :

> Vous croyez à tort que deux erreurs combinées peuvent produire un résultat juste.

**Texte cible avant (zh)** :

> 您错误地认为，两个错误结合起来可以产生一个正确的结果。

**Proposition (rejetée comme dégradation)** :

> 您错误地认为，两个错误结合起来可以产生一个公正的结果。

#### Exemple 10 — pire · planted-c/6.3__c08 · pk 1115 · ar/desc

- **Sévérité déclarée** : nuance · **Problème déclaré** : La fin répète inutilement l’idée d’influencer les événements et rend la phrase maladroite.
- **Raison du juge** : A restitue l’alternative « contrôler ou influencer », tandis que B omet la possibilité d’influencer les événements.

**Référence FR** :

> Vous pensez pouvoir contrôler ou influencer des événements sur lesquels vous n’avez en réalité que peu ou pas d’influence.

**Texte cible avant (ar)** :

> تعتقدون أنكم تستطيعون التحكم في أحداث لا تملكون في الواقع إلا تأثيرًا ضئيلًا عليها أو لا تملكون أي تأثير عليها، أو التأثير فيها.

**Proposition (rejetée comme dégradation)** :

> تعتقدون أنكم تستطيعون التحكم في أحداث لا تملكون في الواقع إلا تأثيرًا ضئيلًا عليها أو لا تملكون أي تأثير فيها.

#### Exemple 11 — pire · witness-sol/4.1__a · pk 720 · zh/example

- **Sévérité déclarée** : moyen · **Problème déclaré** : La construction finale rattache syntaxiquement « 冲出路外 » à l’écureuil et rend ambigu le fait que c’est la voiture qui est sortie de la route.
- **Raison du juge** : B restitue fidèlement la causalité due à l’écureuil, tandis que A ajoute l’idée absente du français que le conducteur lui en attribue la faute.

**Référence FR** :

> Ma voiture, que je conduisais ivre et au téléphone, est sortie de route à cause d’un écureuil.

**Texte cible avant (zh)** :

> 我醉酒并打电话时驾驶的汽车因为一只松鼠冲出路外。

**Proposition (rejetée comme dégradation)** :

> 我当时醉酒驾车，还在打电话，可我的车冲出道路却被我归咎于一只松鼠。

#### Exemple 12 — pire · witness-sol/4.1__a · pk 713 · zh/desc

- **Sévérité déclarée** : nuance · **Problème déclaré** : « 正确的结果 » évoque un résultat factuellement correct, tandis que le français parle d’un résultat moralement juste censé compenser deux fautes.
- **Raison du juge** : B traduit fidèlement « deux erreurs » par « 两个错误 » et « résultat juste » par « 正确的结果 », tandis que A évoque deux mauvaises actions et un résultat équitable.

**Référence FR** :

> Vous croyez à tort que deux erreurs combinées peuvent produire un résultat juste.

**Texte cible avant (zh)** :

> 您错误地认为，两个错误结合起来可以产生一个正确的结果。

**Proposition (rejetée comme dégradation)** :

> 您错误地认为，两件错事结合起来可以产生一个公正的结果。


## 8. Ce que l'essai n'établit pas

1. **Le juge est un modèle (gpt-5.6-sol), pas un relecteur natif.** « Précision 96,5 % » = accord Luna→Sol ; ce n'est pas une vérité humaine. Les 361 corrections « utiles » sont des **candidats**, pas des corrections validées — la relecture native reste à faire (aucune n'est appliquée au dépôt).
2. **Le témoin sol est jugé par lui-même** (auto-faveur possible). Elle jouerait en faveur de sol — qui perd quand même face à Luna ; la conclusion du §5 y gagne en robustesse, elle n'en devient pas une vérité humaine pour autant.
3. **Périmètre = Fallacies seulement** (5 grappes, 507 nœuds, 8 langues). Rien sur Scenarii, Rules, Virtues, ni sur les champs `link_*`.
4. **L'étalonnage couvre 6 classes de défauts plantés** — il valide le juge sur ces classes, pas sur toutes les nuances d'une relecture réelle (style, cohérence inter-cartes, terminologie de famille).
5. **Les tailles de lot ne sont pas tranchées statistiquement** : lot b mesuré sur une seule grappe (6.3), lot a sur 4–7 propositions par grappe. La hiérarchie observée (c ≥ a > b) est une tendance, pas un classement démontré.
6. **Haiku n'est mesuré que sur 10 propositions** — sa perfection locale ne dit rien de son rappel ni de son comportement sur les grappes denses.

## 9. Conclusion (3 lignes)

> **Oui, gpt-6-luna apporte une correction utilisable** : 90 % des défauts plantés retrouvés (rang moyen 2,65), 96,5 % de propositions jugées meilleures, 0,005 $ par correction utile — et les 51 retrouvailles citent la cellule fautive exacte.
> **Réglage recommandé : petits paquets (~12 nœuds)** — 5–10× plus de propositions à précision quasi égale (97,2 % sur la plus grosse grappe), contre la grappe entière qui n'émet presque rien.
> **Le coût n'est pas l'obstacle (1,82 $ / 25 $)** : l'étape à financer est la relecture humaine des 361 candidats, et le juge reste un modèle.

---

## Annexe A — Transparence instrument

La première passe d'étalonnage mesurait en réalité un bug : les clés `pk` du corpus sont des **chaînes**, la recherche `corpus.get(int(pk))` échouait silencieusement et le prompt juge recevait « (nœud introuvable) » comme référence FR. Verdicts de cette passe : 4/6 mesuraient le bug (2 corrects venaient de défauts visibles sans la référence FR). Corrigé (recherche par chaîne + garde `assert` avant chaque appel), la passe re-jouée donne **6/6**. Les deux passes sont conservées dans le journal de dépense (0,0379 $ ≤ porte de 0,05 $) — le bug n'a jamais touché les phases mesurées, toutes postérieures au correctif.

## Annexe B — Contenu du dossier et reproduction

```
instruments/
  extract-clusters.py    # extrait les 5 grappes du SHA figé (git show, lecture seule)
  plant-defects.py       # plante les 30 défauts dans une COPIE (témoin jamais committé avant essai)
  essai.py               # runner payant : sonde/étalonnage/clean/axe/plantés/témoins/juge/incrémental
  aggregate.py           # agrégateur rejouable (produit resultats/aggregate.json)
  prompt-selection.md    # prompt sélecteur (json_schema strict)
  prompt-juge.md         # prompt juge + critères meilleur/pire/équivalent
planted-defects.json     # manifeste témoin des 30 défauts
resultats/
  selector-runs.jsonl    # 479 runs (1 ligne/run, propositions + empreintes)
  verdicts.jsonl         # 374 verdicts du juge
  spend.jsonl            # journal de dépense ligne à ligne (520 appels)
  etalonnage.json  incremental.json  aggregate.json
```

Rejouer : `python instruments/essai.py <stage>` (clés lues dans `<clone>/.keys/`, hors dépôt ; la garde de plafond stoppe à 25 $). Toute la chaîne travaille sur `git show 25327fef:…` — le dépôt n'est jamais écrit.

---

*Essai conduit par la lane po-2024 dans le cadre du pool #458, issue #1520.*
