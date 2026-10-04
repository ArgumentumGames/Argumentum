# Campagne de fidélité Scénarios — **ar** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture à trois voies intégrale. Le présent dossier **remplace la
moitié ar de la PR #1747** (l'écran mécanique y est conservé en §2).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus trois voies généré depuis le CSV (`dump3way_ar.py`, scratchpad de session, régénérable
en une passe) : chaque carte × 6 champs rendus × **FR | EN | ar** (3340 lignes), **lu
intégralement en 4 passes**. Chaque écart ar est relu contre **les deux sources** — la passe zh
(dossier #1746) a établi que la source effective d'un écart peut être l'EN : une lecture FR|ar
seule aurait imputé à l'arabe des écarts qui relèvent du bloc EN (§4).

---

## 2. Écran (mécanique — conservé de la première passe)

Écran multi-langue comparant chaque cellule à FR **et** EN. Signaux : cellule manquante ·
identique à une source · chiffre perdu/ajouté · ratio de longueur · ponctuation finale ·
latin résiduel.

Trois pièges d'instrument corrigés en route, consignés parce qu'ils ont produit des faux
massifs avant correction :

| Piège | Faux produit | Correction |
|---|---|---|
| Jeu de ponctuation **latin** testé sur de l'arabe | **74** « PONCT » (les vrais : 4) | jeu de ponctuation **par écriture** (`؟`, `۔`) |
| « latin résiduel » cherché dans une langue **à écriture latine** | **1169/1169** sur es/pt/en | signal désactivé pour ces langues |
| Comparaison « cellule = FR » sur le bloc EN | **+167** faux (un par carte) | le bloc EN porte `suggestion_en` et des colonnes **nues** ailleurs ; la coupure générique du suffixe comparait la colonne FR à elle-même |

**Couverture : 1169 / 1169 cellules** (167 cartes × 7 champs rendus) — **0 manquante.**
Les 7 « chiffres perdus » (1.3.2, 5.3.4 ×4, 7.2.9 ×2) sont une **borne de l'instrument** :
l'arabe écrit ces nombres **en mots** (*الجيل الخامس* = « la cinquième génération » = 5G ;
*الثانية صباحًا* = « deux heures du matin ») — tout écran futur qui sonde les chiffres
produira 7 faux par passe sur cette langue.

⚠️ **Ce que l'écran a dit et ce qu'il ne dit pas** : l'écran rendait « 0 défaut dur, verdict
propre » (c'est le dossier #1747 rejeté). La lecture à trois voies établit ci-dessous
**3 défauts de fidélité et 5 héritages EN** que l'écran ne peut pas voir — un écran de
structure ne mesure pas une traduction.

---

## 3. Défauts ar établis (à trois voies)

| # | Carte | Champ | FR | EN | ar | Nature |
|---|---|---|---|---|---|---|
| 1 | **1.1.3** | suggestion | « Voilà un mets de choix pour **mes lions**. » | "…a choice dish for **my lions**." | هممم، يا لها من وجبة فاخرة **لقطتي الصغيرة**. | félidé domestique substitué au lion **+ interjection ajoutée** (هممم) ; **contredit `issue_ar` de la même carte**, qui dit للأسود (les lions) — jumeau exact du défaut zh n° 1 (même carte, même champ) |
| 2 | **3.3.2** | enjeu | « …remplacer le mariage par l'adoption d'un caillou… » | "…replace marriage with adopting a rock…" | …بدلًا من الزواج، **يستطيعان إظهار التزامهما المتبادل** بتبنّي حصاة… | ajout « démontrer leur engagement mutuel », absent du FR **et** de l'EN — défaut trouvé par ai-01 (c.5977925408), confirmé à la lecture |
| 3 | **7.3.5** | suggestion | « **Le thème de la soirée, c'est Venise.** Pourquoi êtes-vous déguisé en fromage ? » | "**The party theme is Venice.** Why are you dressed as cheese?" | **ما موضوع الحفلة أصلاً؟** أنت تشبه الجبن، بجدية؟ | la punchline est **réécrite en question** (« quel est déjà le thème de la fête ? ») : l'ancrage **Venise** — la mécanique même de la carte (thème vénitien vs costume fromage) — disparaît de la réplique |

Le défaut 1 est le plus solide : incohérence **interne à la carte** (l'enjeu ar dit les lions,
la suggestion ar dit ma petite chatte), prouvable sans source externe — même structure de
preuve que les défauts zh n° 1, 2, 5.

---

## 4. Écarts **hérités de l'EN** — et non défauts ar

La passe zh avait établi 3 écarts où le zh suit l'EN contre le FR. **L'ar suit l'EN sur les
trois mêmes cartes**, plus deux nouveaux :

| Carte | Champ | FR | EN | ar |
|---|---|---|---|---|
| **3.1.1** | suggestion | « Laissez-moi tenter ma chance ce soir ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | يمكننا أن نجرب كلانا، وسنرى من سيقع عليه الاختيار. |
| **3.2.2** | suggestion | « …où vais-je mettre **mes affaires** ? » | "…where am I going to put **my clothes**?" | أين سأضع **ملابسي**؟ |
| **6.1.1** | suggestion | « Pour restaurer la confiance, il faut des règles claires : le délai doit courir à partir des faits, comme en droit commun. » (121 car.) | "It is absolutely necessary to rebuild the people's confidence…" (89 car.) | لا بدّ من إعادة بناء ثقة الشعب بالطبقة السياسية. |
| **3.2.8** | contexte | « …l'anniversaire de son partenaire. » | "…their partner's **all-important** birthday." | …عيد الميلاد **البالغ الأهمية** لشريكه. |
| **5.2.5** | enjeu | « Il doit convaincre Rachel **qu'il ne l'a pas trompée**. » | "…try to prove her wrong." | عليه أن يقنعها **بعكس ذلك**. |

Les deux derniers sont nouveaux côté ar : l'EN **ajoute** (« all-important ») ou **dilue**
(« prove her wrong » sans l'objet de la dispute), et l'ar suit l'EN — pas le FR. Ces écarts
relèvent d'une passe sur le **bloc EN**, pas sur la traduction arabe. 5 cas suivent l'EN,
**0 cas inverse** repéré — la matrice inter-langues tranchera à mesure des passes suivantes.

---

## 5. Matrice inter-langues (état après la passe ar)

Provisoire — les colonnes es · ru · pt · fa · en se remplissent à mesure des passes. Les
candidats sont cumulés depuis le dossier zh + l'erratum #1746 §9.

| Carte | Nature | zh | ar | autres langues |
|---|---|---|---|---|
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | es · ru · fa (trouvés par ai-01 — à confirmer passe par passe) |
| **1.1.3** sugg | lions → félidé domestique | ✓ (小猫咪) | ✓ (قطتي الصغيرة) | — |
| **2.2.9** ctxt | ajout « personnage mythologique » | ✓ | ✓ (شخصية ميثولوجية) | — |
| **3.2.16** enjeu | modalité affaiblie (« doit » → « tente ») | ✓ (试图) | ✓ (يحاول) | — |
| **4.2.8** titre | ajout « inattendu » + jeu « compromis » perdu | ✓ | ✓ (استيقاظ غير متوقع) | — |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ (يحاول) | — |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | es · fa |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | es |
| **6.1.1 · 3.1.1 · 3.2.2** | suit l'EN contre le FR | ✓ | ✓ | à mesurer |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | à mesurer |

À verser au passif **EN** (constaté pendant la passe ar, pour le futur dossier en) :
7.2.8 sugg *"don't you think you're going to work a bit?"* (contresens — l'ar, lui, suit le
FR sainement) · 5.3.2 bara *"An anti -vaccin"* (espace + mot français) · 5.1.2 bara *"the
smurf"* perd le titre (le roi des Schtroumpfs de l'épisode) · 3.2.8 ctxt ajoute
"all-important".

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **Modalité affaiblie** (famille ci-dessus) : 3.2.16 et 4.1.12 disent يحاول (« tente de »)
  là où FR et EN disent « doit convaincre » / "must convince". 3.2.16 est **partagé avec zh** —
  signature de lot possible.
- **1.2.2** ctxt + sugg : الهنود الحمر (« les Indiens rouges ») pour *Amérindiens* — terme
  daté à connotation péjorative ; le rendu courant serait الهنود الحمر → سكان أمريكا الأصليون.
  Question de registre pour un locuteur natif.
- **1.3.1** sugg : ajoute ومعتمة (« et sombres ») aux « vies difficiles » du FR — petit ajout
  ornemental.
- **3.3.3** ctxt : « a prétendu **hériter** » rendu au futur سيرث (« héritera ») — décalage
  temporel léger.
- **3.1.5** piocheur : حبيبته (« sa bien-aimée ») pour « sa conquête de la veille » —
  adoucissement du registre.
- **4.1.11** enjeu : « destination de rêve » rendu par « cette destination ne peut pas être
  plus alléchante qu'elle n'est » — formulation idiomatique mais **ambiguë** (lisible comme un
  plafond : « pas mieux que ça » au lieu d'un superlatif).
- **4.2.2** ctxt : « déjà éméché » rendu par ثمل بعض الشيء (« un peu ivre ») — adoucissement,
  et le « déjà » disparaît.
- **4.3.5** titre : « que dalle » rendu par ولا حاجة — correct **en dialecte** (égyptien /
  levantin : « rien du tout ») ; registre familier assumé, à valider.
- **6.1.3** : Panthéon transcrit **بانتيون** (titre, sugg) et **بانثيون** (enjeu) dans la
  même carte — double translittération.
- **7.2.5** enjeu : « tente de convaincre » rendu par يدفع…إلى (« pousse à ») — modalité
  accomplies.

---

## 7. Ce que la lecture n'établit pas

- **La chaîne de production** (ar traduit depuis l'EN ou depuis le FR) : 5 écarts suivent
  l'EN, 0 l'inverse — rapporté, non vérifié.
- **Que les défauts zh se généralisent** : sur les 5 défauts de contenu du dossier zh
  (3.2.8, 6.2.1, 4.3.4, 4.3.3, 1.1.3), l'ar en reproduit **un seul** (1.1.3). L'anniversaire
  (3.2.8), le titre inventé (6.2.1), l'enjeu recopié dans le contexte (4.3.4) et le rôle
  ajouté (4.3.3) sont **fidèles en ar** — les défauts d'incohérence interne zh sont
  spécifiques zh.
- **La fluidité et le registre globaux** : relèvent du jugement d'un locuteur natif ; ce
  dossier ouvre la matière, il ne la tranche pas.
- **L'absence d'autres substitutions lexicales** du type 1.1.3 : la lecture intégrale n'en a
  pas vu d'autre, mais une relecture native reste la voie de confirmation.

---

## 8. Fidélité élevée — à consigner aussi

- **5.1.2** — le jeu de mots « schtroumpfement » est **recréé** : سنفوريًا (« smurfement ») ;
  et السنفور الأعظم (« le Très-Grand Schtroumpf ») rend le titre **shtroumphissime** là où
  l'EN le perd ("the smurf"). Les noms bابا سنفور / سنفور القوي correspondent aux noms
  arabes officiels du dessin animé. **L'ar est ici supérieur à sa source EN.**
- **5.2.1** — la citation d'Ézéchiel (Pulp Fiction) est rendue dans son **registre biblique**
  (وستعرف لماذا اسمي الأزلي حين تنقضّ عليك نقمة القدير).
- **2.1.3** — Shéhérazade revient à la maison : شهرزاد, ألف ليلة وليلة — l'ancrage culturel
  natif est exact.
- **7.1.7** — « minou-minou » recréé en diminutif arabe : للقطقوط الحلو.
- **7.2.8** — l'EN est cassé (*"going to work a bit"* pour « vous allez un peu vite en
  besogne ») : l'ar suit le FR correctement (تستعجل الأمور قليلًا) — réparation là où la
  source EN est fautive.
- **6.3.2** — l'EN omet le point final de la suggestion, l'ar **l'ajoute**.

---

## 9. Verdict ar

- **Couverture : complète** (1169/1169), aucune cellule manquante, aucune contamination FR.
- **3 défauts établis** (1.1.3, 3.3.2, 7.3.5), nommés avec leur preuve à trois voies.
- **5 écarts imputés au bloc EN** (dont les 3 déjà connus de la passe zh, tous confirmés),
  retirés du passif ar.
- Les partages zh↔ar (1.1.3, 2.2.9, 3.2.16, 4.2.8, ponctuation) sont versés à la matrice
  inter-langues — la passe des 5 langues restantes dira s'ils sont des signatures de lot.
- **0 écriture** : ce dossier n'a modifié aucune cellule ; la relecture native décidera des
  corrections.

*po-2024*
