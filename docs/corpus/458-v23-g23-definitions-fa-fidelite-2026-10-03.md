# Pool v23 — série ㉓ : fidélité des **définitions** persanes du deck (mesure 0-écriture)

**Base** : branche `docs/458-g22-definitions-ar-fidelite` (série ㉒, chaîne #1713 → #1715 → #1717 → ce grain). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, `--self-test` (6 contrôles). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Quatrième grain de la série ⑳-㉕. Le portage sur `fa` a réparé **un** défaut d'instrument (le jeu de marqueurs, même famille qu'en arabe) — et la lecture a trouvé **la traduction la plus faible de la série à ce jour** : deux cellules dégradées consignées avec frontière, deux typos probables, cinquante-et-une rangées observées sur 175.

## Synthèse — 2 A / 0 M / 49 rangées observées / 124 ✓ (175)

> **Erratum d'arbitrage (03/10, ai-01, [c.5966971153](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5966971153)).** Ce dossier a d'abord été publié **0 A / 0 M / 51 rangées observées**. **PK 361 et PK 696 sont tranchées en A** : « un mot faux sur l'élément clé » (361 : بی‌نظیر « inédit » pour « que tout le monde accepte » ; 696 : پایان‌بخشی « mettre fin » pour « aboutir »). ⚠️ **Correction du dossier lui-même sur PK 361** : la structure temporelle n'est **pas** inversée — « énoncer des affirmations avant d'introduire la thèse contestable » rend bien « d'abord… puis… », et l'absence de verbe principal suit l'anglais (« Stating… »). Le seul défaut de la cellule est le mot بی‌نظیر ; l'analyse de frontière sur-vendait le constat. **Les 2 coquilles probables (PK 323 نایسته, PK 362 بدرو آوردن) sont confirmées et corrigées** par la PR d'arbitrage. Règle actée : une langue jamais imprimée (fa) se corrige sur délégation. Verdict courant : **2 A / 0 M / 49 rangées observées / 124 ✓**.

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **51** (7 catégories) |
| **✓ — fidèle** | **124** |

**Les 175 rangées ont été lues, aucun échantillonnage.**

⚠️ Même faiblesse structurelle qu'aux grains ㉑ ㉒, dite avant le verdict : aucun `desc_fa` dans les archives (**0/175 exploitable**), lecture **unique** par un **non-locuteur natif**, sans contrôle croisé imprimé. Et une seconde faiblesse, propre à ce grain : **la densité d'observations est 2,6× celle du ㉒** (51 contre 19+15 par langue précédente) — dont deux cellules dont la *structure même* est cassée. Ce corpus a été traduit avec moins de soin, et le verdict le porte.

## Ce que ce grain partage avec ㉑/㉒ : la cellule arbitrée du zh y est **saine**

PK 989 « Renverser la charge de la preuve » (A en chinois, #1714) se lit ici : *مسئولیت طرف مقابل است که **نادرستی** موضع شما را ثابت کند* — « **نادرستی** = incorrectitude/fausseté ». Troisième vérification directe (après l'arabe ㉒) : l'inversion n'existe qu'en chinois.

## Réparation d'instrument — le jeu de marqueurs persans, même défaut qu'en arabe

Le jeu `fa` matchait des sous-chaînes : `نه` à l'intérieur de `خانه` (« maison ») — **42 occurrences** de la sous-chaîne dans le corpus. Frontières posées : « négation détectée » **64/175 → 28/175**, `POLARITY` **57 → 29**.

Nuance mesurée avant de poser `\b` : le ZWNJ (U+200C, non-`\w`) **crée** la frontière de `\bنمی\b` — les 8 `نمی` du corpus sont toutes suivies de ZWNJ (0 attachée), donc la réparation ne rate **rien** ici ; une forme attachée sans ZWNJ serait ratée, limite nommée dans le code.

Famille de faux positifs propre au farsi : la négation **préfixée** (`بی‌-` dans بی‌اساس, `غیر-` dans غیرطبیعی) est invisible pour l'écran — **le miroir exact du défaut russe** (недостаточны) et du défaut français (im-précision). Trois langues, même asymétrie lexicalisée/préfixée vs particule.

## C-notes — 51 rangées, 7 catégories, aucune écriture proposée

### 1. Le farsi garde l'imprimé d'époque, le français a été réécrit depuis (6)

Vérifié **mécaniquement** contre le `desc_en` de l'archive v3. **Deux fois plus qu'en arabe (3)** :

| PK | Titre | L'élément d'époque que le farsi porte | Ce que le français courant dit |
|---|---|---|---|
| 182 | Fausse alternative | « **Pretending** to offer… essentially **similar** results » / وانمود کردن… اساساً… مشابهی | « présentez un choix… au **même** résultat » (sans « prétendre ») |
| 343 | Argument du bâton | « to force someone's **agreement with a proposition** » / جلب موافقت کسی با یک پیشنهاد | « remporter un débat au lieu de défendre par la raison » |
| 511 | Communication non verbale | « gesture, tone of voice, body language or other cues **like facial expression** » / علائم دیگر مانند حالات چهره | « langage corporel ou les inflexions de la voix » |
| 670 | Faux équilibre | « as **more balanced than they actually are** » / تعادل بیشتری نسبت به آنچه واقعاً هستند | « le même poids… de manière injustifiée » |
| 699 | Argument circulaire | « propositions **only true if the conclusion is true** » / فقط در صورتی صحیح هستند که نتیجه‌گیری صحیح باشد | « chaque argument repose sur l'acceptation préalable de la conclusion » |
| 837 | Comparaison incohérente | « **Partially** comparing… **to pretend** to draw a general comparison » / مقایسه جزئی… برای تظاهر | « en ne retenant que certains aspects, ce qui fausse… » |

### 2. Deux cellules dont la structure est cassée — consignées AVEC frontière (2)

⛔ **Le motif du zh PK 989** : consigner avec la frontière écrite, laisser l'arbitrage à ai-01.

- **PK 361 « Appât et substitution »** — FR : *« Vous commencez par des affirmations que **tout le monde accepte**, puis vous glissez subtilement vers vos idées contestables »*. FA : *« با بیان مجموعه‌ای از اظهارات **بی‌نظیر** و بدون بحث قبل از معرفی بحث پیشنهادی و قابل انتقاد »* — « un ensemble d'affirmations **inédites** et sans débat, avant d'introduire la discussion proposée et critiquable ». Trois dégradations : « que tout le monde accepte » → « **inédites** » (propriété erronée), « **puis** vous glissez » → « **avant** d'introduire » (structure temporelle inversée), et la phrase n'a **pas de verbe principal**. Le mécanisme (banal puis contestable) reste reconnaissable — mais si un locuteur natif juge que « بی‌نظیر » détruit le mécanisme, cette ligne passe en A ou M.
- **PK 696 « Erreur de raisonnement »** — FR : *« Votre thèse repose sur un raisonnement incohérent »*. FA : *« **پایان‌بخشی** به یک نتیجه‌گیری با استناد به استدلال نادرست »* — « mise en **fin** d'une conclusion en s'appuyant sur un raisonnement incorrect ». Le mot outils est cassé (پایان‌بخشی « achèvement » pour ce qui voulait dire « votre thèse » ou « aboutir à »), et « incohérent » → « incorrect ». Sens récupérable, rendu défaillant. Même frontière : l'arbitrage appartient à un locuteur natif.

### 3. Jumeaux EXACTS des dossiers zh ET ar — trois langues, même divergence (3)

- **PK 729 « Négation de l'antécédent »** : « condition suffisante » → **علل ممکن** (« causes possibles ») + **la même glose ajoutée** (« علت و شرط لازم را با هم اشتباه می‌گیرید » — confondre cause et condition nécessaire). **zh, ar et fa portent le même ajout** : l'original d'époque commun est quasi certain.
- **PK 134 « Sophisme ludique »** : le relatif « modèles **qui** négligent » → alternative **یا** (« ou en ignorant »). Troisième langue.
- **PK 175 « Influence »** : *persuader/convaincre* → **manipulation vs raisonnement logique**. Troisième langue.

### 4. Jumeaux du dossier arabe (8)

PK 300 « Connivence » (« infléchir son jugement » → « gagner son soutien ») · 322 « Repoussoir » (discréditer → **critiquer**) · 644 « Probabilités faussées » (« fausse votre **raisonnement** » → « dévie votre **conclusion** logique ») · 733 « Affirmation d'une disjonction » (réencadré par le « ou » exclusif) · 781 « Logique du chaudron » (le contraste *seules/ensemble* compressé, + l'adverbe éditorial « متأسفانه » *hélas*) · 804 « Acception arbitraire » (compression « définition sur mesure » → « termes définis arbitrairement ») · 1011 « Exigence relâchée » (difficilement défendable → **indéfendable**) · 1281 « Refus du débat » (« échange constructif fondé sur des arguments rationnels » → « discussion logique »).

### 5. Décalages propres au farsi, sens tenu (6)

| PK | Titre | Ce qui bouge |
|---|---|---|
| 51 | Sophisme du psychologue | « **objectivité** universelle » → « **réalité** générale » — l'arabe déplaçait « perspective », le farsi déplace « objectivité » |
| 313 | Flatterie | **ajoute le mécanisme** : « renforcer le sentiment de grandiosité (خودبزرگ‌بینی) de l'auditoire » |
| 750 | Erreur de modalité | l'énumération *possible/nécessaire/certain/obligatoire* → « les **différences de la logique modale** » (même famille que la C-note ru du ⑳ : « модальности ») |
| 834 | Comparaison abusive | **compression réelle attrapée par l'écran** (`LENDEV −2,7`) : « excessive ou inappropriée » → نادرست (« incorrecte ») |
| 953 | Attention sélective | « thèse » → **پایان‌نامه** (« mémoire/dissertation » — le mot académie, pas l'acception argumentative) ; expansion bénigne (`LENDEV +2,6`) |
| 1362 | Tu quoque | « pas **toujours** conformément à ses principes » → « échec à agir selon ce **postulat** » — le « pas toujours » (nuance tu-quoque) est perdu |

### 6. Adoucissements, compressions et ajouts mineurs, sens tenu (24)

PK 3 (apportent rien → sans poids argumentatif) · 34 (événement isolé → **particulier**) · 43 (« un comportement » → « choix, méthodes **ou** actions », expansion) · 78 (« personne réputée » → « personne **crédible et respectée** », dédoublement) · 112 (« principes **moraux** » → « principes **personnels** » ; « démonstration objective » → « indices objectifs ») · 133 (éléments → données) · 247 (« séduisante » → « **trompeuse** ») · 658 (« la notion d'infini » → « **les** concepts d'infini ») · 697 (« pas démontré ou incorrect » → « douteux ») · 708 (ordre des mots chancelant, sens lisible) · 735 (queue « fausse l'argument » perdue) · 768 (« parallèle injustifié… pas comparables » → « faire **sembler équivalents ») · 796 (« quatre termes » → « quatre **ou plus** ») · 809 (« termes distincts » → « **conditions du débat** ») · 833 (+ « et erronée ») · 845 (« une personne **ou une chose** » → « quelque chose ») · 847 (« la structure peut prêter à » → « structures **ambiguës** ») · 956 (contorsion « sans justification de l'exception », sens tenu) · 1023 (« vos biais » → « biais **cognitifs** » ; « juger la situation » → « raisonnement équilibré ») · 1291 (« causes **profondes** » → « causes **principales** » ; « à la surface » → « **conjectures** superficielles ») · 1352 (objet diffusé en triplet « argument, débat **ou** adversaire ») · 1357 (« en réalité fallacieux » replacé en tête) · 1365 (caricaturez → déformez) · 1388 (« son argumentation » → « argument **ou indices** »).

### 7. Typos probables — lisibles, sens intact (2)

- **PK 323 « Appel au mépris »** : « نایسته » — probablement pour **ناشایسته** (« indigne de »). Le sens se lit, l'orthographe est cassée.
- **PK 362 « Sandwich de louanges »** : « **بدرو** آوردن » — forme non standard (l'idiome attendu : « قرار دادن » ou « در آوردن »). Même statut.

## T2 — d'où vient le farsi ? (mesuré)

| Comparaison | corr avec `fa` |
|---|---:|
| vs **français courant** | **0,715** (n=175) |
| vs **anglais imprimé** (168 rattachées) | **0,296** |

Comme l'arabe (0,825/0,278), le farsi suit le français **courant** — avec **6** survivantes d'époque contre 3 en arabe (et 17 en zh). ⚠️ **Le farsi ne dérive pas de l'arabe** : sur les cartes où les deux divergent, le farsi suit le français courant là où l'arabe suivait l'imprimé (PK 594, 673, 798, 848) ou inversement (PK 304 : l'arabe garde « en dépit des preuves », le farsi suit le français courant ; PK 1242, 1330 : le farsi garde « déformé »/« sans rapport » que l'arabe adoucissait). Deux traductions indépendantes, deux profils d'erreurs différents.

## Contrôle inverse

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

1. **Cellule inversée plantée** (PK 814 : « de nombreuses options s'offrent à vous alors qu'il n'en existe que deux ») → **`FLAGS: -` du premier essai** — aucun écran ne voit le contresens, la lecture le voit immédiatement. Pas d'accident cette fois : la réparation des marqueurs était déjà en place (l'accident arabe du ㉒ l'avait motivée).
2. **Cellule tronquée à 50 %** : détectée `LENDEV` (écran abjad étendu au farsi par le ㉓ — pouvoir mesuré **99/175** à 50 %, 162/175 à 35 %) ; témoin sain propre. Tourne dans `--self-test` (contrôle (c) par écriture).

Les deux `LENDEV` du corpus réel ont été **lues** : PK 834 (compression réelle, catégorie 5) et PK 953 (expansion bénigne, catégorie 5).

## N'établit pas

- **Que les définitions persanes soient sans défaut.** Lecture **unique**, par un **non-locuteur natif** — et ce corpus est le plus faible de la série : deux cellules à structure cassée attendent un arbitre natif.
- **Que les deux cellules cassées (PK 361, 696) soient des A ou des M** — la frontière est écrite, l'arbitrage appartient à ai-01, exactement comme PK 989 au ㉑.
- **Que les typos probables (PK 323, 362) en soient** — un locuteur natif confirme ou infirme ; aucune écriture n'est proposée ici.
- **Que l'imprimé vienne au secours du zéro** : `desc_fa` absent des archives (0/175 exploitable).
- **Que les écrans corroborent quoi que ce soit** : `POLARITY` 29 drapeaux post-réparation, 0 défaut réel derrière (lecture des 175) ; le contresens planté sort `FLAGS: -`.
- **Rien sur** les 7 cartes sans archive (PK 105, 362, 492, 1020, 1092, 1120, 1357), la langue ㉔ (`pt`+`es`), ㉕ (`en`), les champs `example_<lang>`/`link_<lang>`, ni les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.** Les 51 observations attendent l'arbitrage d'ai-01.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang fa --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175) ; corpus du dépôt **jamais écrit** (contrôle inverse sur copie).
- **Non-régression prouvée** : `ru` 26, `zh` 48, `ar` 47 drapeaux — identiques avant/après la réparation `fa` (re-passes complètes ; la correction ne touche que le jeu de marqueurs `fa`).
- Comptes de jointure inchangés (157 nom / 11 position seule / 7 aucune).
