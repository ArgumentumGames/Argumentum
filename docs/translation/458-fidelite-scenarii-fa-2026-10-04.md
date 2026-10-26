# Campagne de fidélité Scénarios — **fa** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture à trois voies intégrale. Le présent dossier **remplace
la moitié fa de la PR #1747** (l'écran mécanique est restitué en §2).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus trois voies généré depuis le CSV (`dump3way_fa.py`, scratchpad de session, régénérable
en une passe) : chaque carte × 6 champs rendus × **FR | EN | fa** (3340 lignes), **lu
intégralement en 4 passes**. Chaque écart est relu contre **les deux sources** — les passes
zh (#1746) et ar (#1749) ont établi que la source effective d'un écart peut être l'EN (§4) et
qu'un noyau de défauts se **partage entre langues** (§5) : chaque candidat fa a été confronté
aux listes zh et ar avant classification.

---

## 2. Écran (mécanique — restitué de la première passe)

Écran multi-langue comparant chaque cellule à FR **et** EN (structure : #1747). Les trois
pièges d'instrument corrigés en route y sont consignés — ils valent pour fa :

| Piège | Faux produit (ar) | Correction |
|---|---|---|
| Jeu de ponctuation **latin** testé sur une écriture RTL | **74** « PONCT » | jeu **par écriture** (`؟` fa = `?`) |
| « latin résiduel » cherché dans une langue à écriture latine | **1169/1169** sur es/pt/en | signal désactivé pour ces langues |
| Comparaison « cellule = FR » sur le bloc EN | **+167** faux | colonnes EN **nues** ; coupure du suffixe piégée |

Mesures fa de cette passe (sur le corpus même) :

- **Couverture : 1169 / 1169 cellules, 0 vide** (167 cartes × 7 champs rendus).
- **Ponctuation finale** — fa fait partie du lot **1.2.3** (contexte et enjeu sans point,
  lot es·ar·fa·zh) mais **pas** du lot 2.1.8 (es·ar·zh : la suggestion fa porte son point).
  Sur **2.2.1**, fa **répare** : point final présent bien que l'EN l'omette (comme 6.3.2).
- **CHIFFRE** — le fa **mélange** chiffres persans (۲۵ یورو, ۲۰۱۹, 5G conservé) et nombres
  en mots (پنج‌ستاره « cinq étoiles », ساعت دو « deux heures ») : un écran qui sonde les
  chiffres produira des faux **seulement** sur 1.3.2 (piocheur) et 7.2.9 (suggestion) — pas
  sur 5.3.4, contrairement à l'ar (#1749 §2).

⚠️ Ce que l'écran ne dit pas : il rendait « 0 défaut dur » (dossier #1747 rejeté). La lecture
à trois voies établit ci-dessous **7 défauts de fidélité et 6 héritages EN** invisibles pour
lui.

---

## 3. Défauts fa établis (à trois voies)

| # | Carte | Champ | FR | EN | fa | Nature |
|---|---|---|---|---|---|---|
| 1 | **1.1.3** | suggestion | « …pour **mes lions**. » | "…for **my lions**." | هوم، چه لقمه دل‌چسبی برای **بچه‌گربه‌های من**. | félidé domestique substitué au lion **+ interjection** (هوم) ; contredit `issue_fa` de la même carte (شیرها, les lions) — **zh · ar · fa**, même carte, même champ |
| 2 | **3.3.2** | enjeu | « …remplacer le mariage par l'adoption d'un caillou… » | idem | …**تعهد متقابلشان را** با به سرپرستی گرفتن یک قلوه‌سنگ **نشان بدهند**… | ajout « montrer leur engagement mutuel », absent des deux sources — **zh · ar · fa** (es · ru signalés par ai-01) |
| 3 | **3.2.8** | titre + ctxt + sugg | « **anniversaire** » / "birthday" | idem | **سالگرد** partout ; sugg : **سالگردمان** (« NOTRE anniversaire ») | « anniversaire de naissance » rendu par « anniversaire d'événement » (un anniversaire de naissance se dit تولد, jamais سالگرد — qui désigne un wedding/death anniversary) : **contresens de concept uniforme** à travers la carte, et la punchline passe de « MON anniversaire » à « NOTRE anniversaire » — la faute reprochée change de nature. Distinct du défaut zh n° 2 (incohérence **interne** 生日/纪念日) : fa est cohérent mais décalé |
| 4 | **4.3.4** | contexte | « …une IA éthique, **sans aucun garde-fou.** » | idem | …**و باید کمیته‌ای را قانع کند از پروژه‌اش حمایت کند**. | l'**enjeu est recopié dans le contexte** : la carte dit deux fois la même chose — défaut **zh · fa** (l'ar est fidèle) |
| 5 | **5.2.5** | enjeu | « Il doit convaincre Rachel… » | "try to prove her wrong" | **بزک‌کننده** باید او را قانع کند… | le rôle s'appelle **چرب‌زبان** sur les 176 autres cellules ; cette seule occurrence dit **بزک‌کننده** (« le flagorneur ») — incohérence terminologique mesurée (چرب‌زبان ×177, بزک‌کننده ×1) ; s'ajoute la dilution héritée de l'EN (§4) |
| 6 | **5.3.5** | suggestion | « Et si nous appelions "gravité" un phénomène que nous avons toujours mal interprété ? » | "What if …misinterpreted?" | **زمین ما را از خودش دور می‌راند؟ چه فکر کاملاً عجیب‌وغریبی!** | la réplique du physicien est **remplacée** : l'inversion « et si la gravité était un phénomène toujours mal interprété » devient une exclamation générique (« La Terre nous repousse ? Quelle idée bizarre ! ») — la substance de la riposte disparaît |
| 7 | **7.3.5** | suggestion | « **Le thème de la soirée, c'est Venise.** Pourquoi êtes-vous déguisé en fromage ? » | idem | **تم مهمانی چی بود؟** جدی جدی شبیه پنیر شده‌ای؟ | punchline **réécrite en question** (« quel était le thème ? … vraiment déguisé en fromage ? ») : l'ancrage **Venise** disparaît de la réplique — **ar · fa**, réécriture à deux clauses identique dans les deux langues |

Les défauts 1 et 4 sont prouvables sans source externe (contradiction interne à la carte) ;
le 5 par comptage ; les 2, 3, 6, 7 par triple lecture.

---

## 4. Écarts **hérités de l'EN** — et non défauts fa

Les trois écarts connus depuis zh sont **confirmés en fa** (3e langue), plus trois nouveaux :

| Carte | Champ | FR | EN | fa |
|---|---|---|---|---|
| **3.1.1** | suggestion | « Laissez-moi tenter ma chance ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | هر دو امتحان کنیم، ببینیم آخرش کی انتخاب می‌شود. |
| **3.2.2** | suggestion | « …mes **affaires** ? » | "…my **clothes**?" | لباس‌هایم را کجا بگذارم؟ |
| **6.1.1** | suggestion | « Pour restaurer la confiance, il faut des règles claires… » (121 car.) | "It is absolutely necessary to rebuild the people's confidence…" | باید هر طور شده اعتماد مردم به طبقهٔ سیاسی را بازسازی کنیم. |
| **3.2.8** | contexte | « l'anniversaire de son partenaire » | "their partner's **all-important** birthday" | سالگرد **بسیار مهم** شریک زندگی‌اش — l'ajout EN est suivi (le contresens سالگرد, lui, est un défaut fa, §3 n° 3) |
| **4.3.1** | suggestion | « Diantre, voilà que j'ai la **berlue** ! » | "My God, I'm so **dizzy**!" | نکند **چشم‌هایم سیاهی می‌رود**! (« mes yeux se noircissent » = évanouissement) — suit l'EN, qui perd déjà le « je vois des choses » du FR |
| **5.2.5** | enjeu | « …qu'il **ne l'a pas trompée**. » | "try to prove her wrong" | ماجرا این‌طور نیست (« la chose n'est pas ainsi ») — dilution EN suivie |
| **5.3.2** | enjeu | « …de **refuser le vaccin**. » | "to refuse **to benefit from it**" | از این امکان استفاده نکند (« ne pas user de cette faculté ») |

7 écarts suivent l'EN, **0 cas inverse** — après trois langues, le motif est stable : ces
écarts relèvent d'une passe sur le **bloc EN**.

**Écrit le 07/10 (file profonde c.5993735448, grain 3b)** — quatre des écarts du tableau
ci-dessus sont **portés à leur référent FR** dans la même PR que les cellules EN jamais
imprimées (règle du GO : même défaut, même rangée) : **6.1.1** (suggestion : le mécanisme de
la prescription), **5.2.5** (enjeu : Rachel nommée, forme du contexte fa), **5.3.2** (enjeu : le
vaccin), **3.2.8** (contexte : le superlatif retiré). Détail au §12 du dossier d'axe imprimé.

⚠️ **Un écart du tableau reste ouvert et est désormais ÉPINGLÉ** : **4.3.1** (suggestion
« mes yeux se noircissent » = évanouissement) suit l'EN *d'avant* sa correction — l'EN a été
corrigé au grain 3a (« I'm seeing things! ») et son sibling fa n'a pas suivi, le grain 3a ne
portant que ses siblings ru. Épinglé dans `ScenariiLanguageSpecificCorrectionsGuardTests.Deferred`
à sa valeur courante : le jour où la cellule est écrite, la garde rougit et demande de retirer
l'entrée. **3.1.1**, **3.2.2** et **6.1.1**-hors-3b restent hors du présent grain.

---

## 5. Matrice inter-langues (état après la passe fa)

Cumulée zh + ar + fa ; les colonnes es · ru · pt · en se remplissent à mesure des passes.

| Carte | Nature | zh | ar | fa | autres |
|---|---|---|---|---|---|
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | es · ru (ai-01) — à confirmer |
| **1.1.3** sugg | lions → félidé domestique | ✓ (小猫咪) | ✓ (قطتي الصغيرة) | ✓ (بچه‌گربه‌های من) | — |
| **2.2.9** ctxt | ajout « personnage mythologique » | ✓ | ✓ | ✓ (چهره‌ای اسطوره‌ای) | — |
| **3.2.16** enjeu | modalité affaiblie | ✓ (试图) | ✓ (يحاول) | ✓ (سعی می‌کند) | — |
| **4.2.8** titre | « inattendu » ajouté, jeu « compromis » perdu | ✓ | ✓ | ✓ (غیرمنتظره) | — |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ (بکوشد) | — |
| **4.1.1** enjeu | modalité affaiblie (« doit vendre » → « essaie de vendre ») | — | — | ✓ (تلاش کند) | — |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — (fidèle) | ✓ | — |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | — |
| **3.2.8** | anniversaire → anniversaire d'événement | ✓ (incohérent) | — (fidèle) | ✓ (uniforme) | — |
| **6.2.1** titre | titre inventé | ✓ | — | — (fidèle : کناره‌گیری توافقی) | — |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — (fidèle) | — |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | es |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — (fa ponctue) | es |
| **3.1.1 · 3.2.2 · 6.1.1** | suit l'EN contre le FR | ✓ | ✓ | ✓ | à mesurer |
| **1.3.1** sugg | ajout ornemental « et sombres » | — | ✓ (ومعتمة) | ✓ (و تیره) | — |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ (حبيبته) | ✓ (دلبر دیشبی) | — |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ (ومعه) | ✓ (و خودش) | — |
| **4.2.2** ctxt | « un peu ivre » (adouci) | — | ✓ (بعض الشيء) | ✓ (کمی مست) | — |
| **7.2.5** enjeu | « pousse à » (vs « tente de convaincre ») | — | ✓ (يدفع) | ✓ (هل می‌دهد) | — |
| **4.1.11** enjeu | « destination de rêve » → superlatif idiomatique ambigu | — | ✓ | ✓ (از این خواستنی‌تر نمی‌شود) | — |

Le motif se précise : un **noyau zh·ar·fa** (3.3.2, 1.1.3, 2.2.9, 3.2.16, 4.2.8, 1.2.3) et un
**sous-ensemble ar·fa** étendu (4.1.12, 7.3.5, 1.3.1, 3.1.5, 3.3.5, 4.2.2, 7.2.5, 4.1.11) —
des choix de rendu **jumeaux** ar/fa sur des cartes où zh diverge autrement. Les passes
es · ru diront si ces familles traversent aussi les écritures latines (3.3.2 y est déjà
signalé).

À verser au passif **EN** (constaté pendant la passe fa) : 7.2.8 sugg *"going to work a bit"*
(contresens — le fa, comme l'ar, suit le FR sainement) · 5.3.2 bara *"An anti -vaccin"* ·
5.1.2 bara *"the smurf"* perd le titre · 4.3.1 sugg *"dizzy"* perd la berlue · 3.2.8 ctxt
"all-important" ajouté.

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **6.1.3** — Johnny Hallyday reste en latin dans l'`enjeu` (Johnny Hallyday) mais est
  translittéré dans le titre et la suggestion (جانی هالیدی) — même hésitation que zh (§5 du
  dossier #1746) ; les rôles sont rendus sur la face.
- **3.1.3** piocheur — شریک جنسی‌اش (« son partenaire sexuel ») : explicitation du
  « partenaire » neutre des deux sources.
- **3.1.9** enjeu — دعوت کند باور کند (« invite à croire ») pour « faire croire » :
  affaiblissement léger, presque comique.
- **3.3.9** sugg — فراواقعی (« surréel ») pour "completely insane" / « délirant » : registre
  adouci.
- **2.2.8** sugg — دام تابلوئی (« piège "tableau" », argot pour « flagrant ») : registre
  familier assumé, à valider.
- **4.3.5** titre — هیچی نفهمید (« rien compris », familier) rend bien le « que dalle ».
- **7.1.6** ctxt — ajout اما مهم‌تر از آن (« mais surtout ») : editorialisation légère.
- **5.1.2** ctxt — اسمورف بزرگ pour « Grand Schtroumpf » / Papa Smurf : le nom officiel
  persan du dessin animé. Voir §7.

---

## 7. Ce que la lecture n'établit pas

- **La chaîne de production** (fa traduit depuis l'EN ou le FR) : 7 écarts suivent l'EN,
  0 l'inverse — rapporté, non vérifié.
- **Que les défauts zh restent zh-seuls** : 4.3.4 est zh·fa, 3.2.8 dévie en fa sous une
  autre forme — mais **6.2.1** (titre inventé) et **4.3.3** (rôle ajouté) sont confirmés
  **zh-seuls** après deux langues de contrôle (ar fidèle, fa fidèle).
- **Le partage ar·fa comme preuve de source commune** : huit cartes portent des rendus
  jumeaux ar/fa (§5), ce qui **suggère** une même vague de production, mais la passe des
  langues latines manque encore pour trancher.
- **La fluidité et le registre globaux** : jugement natif. Ce dossier ouvre la matière.

---

## 8. Fidélité élevée — à consigner aussi

- **5.1.2** — le jeu de mots est **recréé** : اسمورفانه (« smurfement »), et le titre
  **shtroumphissime** devient اسمورفِ اعظم‌نما (« le smurf à la grandeur affectée » — le
  suffixe نما rend précisément la forfanterie du titre). **Troisième langue non latine à
  recréer le jeu (zh · ar · fa) — l'EN, lui, le perd** ("the smurf").
- **5.2.1** — la citation d'Ézéchiel (Pulp Fiction) en **registre biblique** (تو خواهی دانست
  چرا نام من ابدی است…).
- **7.1.7** — « minou-minou » recréé : پیشی‌پیشی نازه.
- **7.2.8** — l'EN est cassé (*"going to work a bit"*) : le fa suit le FR (داری یک کم زیادی
  تند می‌روی = « tu vas un peu vite »), comme l'ar — réparation là où la source EN faute.
- **2.2.1 · 6.3.2** — l'EN omet le point final : le fa **l'ajoute** deux fois.
- **2.1.3** — شهرزاد، شاه ایران : l'histoire revient à son aire culturelle, l'ancrage est
  natif et exact.
- **1.2.4** — la citation d'Olympe de Gouges (« l'échafaud / la tribune ») rendue avec son
  parallélisme.
- **4.2.1** ctxt — fidèle au FR « après avoir découvert » (چون فهمیده‌اند), là où l'ar
  décalait au futur (#1749 §6).

---

## 9. Verdict fa

- **Couverture : complète** (1169/1169, 0 vide), aucune contamination FR.
- **7 défauts établis** (1.1.3, 3.3.2, 3.2.8, 4.3.4, 5.2.5, 5.3.5, 7.3.5), nommés avec
  preuve ; deux d'entre eux sont prouvables sans source externe.
- **6 écarts imputés au bloc EN** (les 3 classiques + 3 nouveaux), retirés du passif fa ;
  0 cas inverse en trois langues.
- Le noyau **zh·ar·fa** (6 cartes) et le faisceau **ar·fa** (8 rendus jumeaux) sont versés à
  la matrice — les passes es · ru diront si le lot traverse les écritures latines.
- **0 écriture** : la relecture native décidera des corrections.


## Écrit — grain 5 (07/10, file profonde c.5993735448)

Les compositions différées de ce dossier sont **écrites, sur délégation** (langues jamais imprimées) :

- **4.2.8 `title_fa`** → **بیداری خفت‌آور** — خفت‌آور (compromettant/gênant), calque du modèle pt/es ;
  « غیرمنتظره » (inattendu) retiré — ajouté sans source.
- **3.1.5 `drawer_fa` + `context_fa`** → **شکار دیشبی‌اش** / **شکارش وارد** — شکار (la prise) était le candidat
  nommé par ce dossier ; دلبر (la dulcinée) adoucissait. Le contexte suit (règle du GO, même rangée).
- **5.3.5 `suggestion_fa`** → la question « et si » du physicien **restaurée** avec **گرانش** = le terme du
  TITRE fa de la carte (گرانش وارونه) — l'exclamation générique avait remplacé la riposte.
  *(6.2.1 `title_fa` کناره‌گیری توافقی était déjà fidèle — inchangé.)*

Garde : `ScenariiComposedCorrectionsGuardTests`. Résidu 4.3.1 `suggestion_fa` : toujours différé (burn-down).

*po-2024*
