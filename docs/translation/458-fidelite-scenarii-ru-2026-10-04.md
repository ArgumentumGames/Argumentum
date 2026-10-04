# Campagne de fidélité Scénarios — **ru** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture à trois voies intégrale. Le présent dossier **remplace
la moitié ru de la PR #1747** (l'écran mécanique est restitué en §2).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus trois voies généré depuis le CSV (`dump3way_ru.py`, dérivé par substitution de colonnes
du générateur ar, scratchpad de session, régénérable en une passe) : chaque carte × 6 champs
rendus × **FR | EN | ru** (3340 lignes), **lu intégralement en 4 passes**. Chaque écart est
relu contre **les deux sources** et confronté aux listes établies par les passes zh (#1746),
ar (#1749), fa (#1750) et es (#1751).

La passe ru est la **cinquième** — et la première qui **renverse deux généralisations**
publiées par les passes précédentes : le motif « les écarts suivent l'EN, jamais l'inverse »
(§4) et le statut zh-seul de 6.2.1 (§3, §5). Les errata correspondants sont listés §10.

---

## 2. Écran (mécanique — restitué de la première passe)

Écran multi-langue comparant chaque cellule à FR **et** EN (structure : #1747). Pour ru,
écriture cyrillique : les pièges RTL et « latin résiduel » ne s'appliquent pas, la
comparaison « cellule = FR » sur le bloc EN s'applique.

Mesures ru de cette passe (sur le corpus même) :

- **Couverture : 1169 / 1169 cellules, 0 vide** (167 cartes × 7 champs rendus, vérifié au grep).
- **Ponctuation finale : ru ponctue** — c'est la mesure la plus contrastée de la campagne :
  là où zh·ar·fa·es omettent le point sur **1.2.3** et **2.1.8**, ru l'écrit ; sur **6.3.2**
  et **2.2.1**, il **répare** l'omission de l'EN. Le lot « ponctuation finale absente » ne
  contient aucune cellule ru.
- **Chiffres** : ru les garde («1001 ночь», 25 €…) — l'écran chiffres ne produit aucun faux.

⚠️ Ce que l'écran ne dit pas : il rendrait ru propre. La lecture à trois voies établit
ci-dessous **13 défauts** dont deux majeurs propres à ru (7.2.7, 3.2.4) et une famille
structurelle invisible pour lui (les titres réécrits).

---

## 3. Défauts ru établis (à trois voies)

| # | Carte | Champ | FR | EN | ru | Nature |
|---|---|---|---|---|---|---|
| 1 | **7.2.7** | suggestion | « Oh, je suis tellement désolé, mais j'ai enfin un **entretien** ! » | "Oh, I'm so sorry, but I finally have a **job interview**!" | «Слушай, в итоге я не смогу с тобой поехать.» | **l'entretien disparaît de la réplique** — or c'est le pivot de la carte (le chômeur est convoqué) ; l'excuse saute aussi. Punchline amputée — **ru seul** |
| 2 | **3.2.4** | contexte | « le baratineur et **son partenaire** décident d'avoir un enfant » | "the smooth talker and **their partner**" | «Софист и его партнёр, **Берущий карту**, решают завести ребёнка» | « celui qui prend la carte » — **terme d'interface de jeu** imprimé dans la cellule, là où les deux sources disent « partenaire » ; le lecteur de la carte imprimée ne « prend » rien. **Fuite d'interface — ru seul** |
| 3 | **6.2.1** | titre | « Retrait négocié » | "Negotiated Withdrawal" | «**Рокировка**» (« la roque », aux échecs — ou par extension la permutation politicardière) | **titre inventé** — 2e langue après zh, invention distincte : le statut « zh-seul » de cette carte **tombe** (errata §10) |
| 4 | **Lot des titres réécrits** | titre | — | — | **8 réécritures** : 2.2.7 «Завоевать Пенелопу» (« conquérir Pénélope » pour *Le prétendant*), 3.1.1 «Второй парень на свидании» (*Ailier*), 3.3.6 «Нам крышка» (« on est fichus » pour *Ciel, mon mari !*), 3.3.10 «Свадьба или нет?» (*Mariage ? Non merci*), 4.1.1 «Уловки продавца» (*Rouler des mécaniques*), 4.1.2 «Телесные наказания» (*Le martinet*), 5.3.1 «Теория плоской земли» (*Débat avec un terraplaniste*), 7.1.5 «Праздник» (*La kermesse*) ; **+ 3 déspécifiés** : 1.3.2 (ajoute «Президент», perd le « A »), 6.3.2 «Старый друг» (perd « de vingt ans »), 7.2.6 «Изысканное фуа-гра» (perd «Millésime») | **famille structurelle ru** : ~12/167 titres remplacés par des descriptions fonctionnelles ou génériques — les jeux de mots et ancrages des titres FR sont nivelés. Signature de la passe ru, inaperçue des écrans |
| 5 | **3.3.2** | enjeu | « …remplacer le mariage par l'adoption d'un caillou… » | idem | «…вместо свадьбы они могут **доказать свою взаимную преданность**, заведя камень…» | ajout « engagement mutuel », absent des deux sources — **5e langue (zh · ar · fa · es · ru)** : le signal ai-01 (es · ru) est confirmé sur les deux |
| 6 | **2.2.9** | contexte | « Sisyphe est condamné à pousser éternellement une pierre… » | idem | «Сизиф — **мифологический персонаж**, обречённый вечно катить камень…» | ajout « personnage mythologique » — **5e langue** |
| 7 | **3.2.16** | enjeu | « Il doit **convaincre** sa moitié… » | "must convince" | «Он **пытается** убедить свою половину…» | modalité affaiblie (« essaie de ») — **5e langue** |
| 8 | **4.3.4** | contexte | « …une IA éthique et responsable, sans aucun garde-fou. » | idem | «…без каких-либо ограничителей, **и должен убедить комитет поддержать его проект**.» | l'**enjeu est recopié dans le contexte** — **4e langue (zh · fa · es · ru)** ; l'ar est fidèle |
| 9 | **7.3.5** | suggestion | « **Le thème de la soirée, c'est Venise.** Pourquoi êtes-vous déguisé en fromage ? » | idem | «**Какая вообще тема вечеринки?** Ты серьезно выглядишь как сыр?» | punchline **réécrite en question**, ancre « Venise » perdue — **4e langue (ar · fa · es · ru)**, même structure de réécriture |
| 10 | **4.1.12** | enjeu | « …inventer une excuse et **convaincre** le parent… » | idem | «…придумать оправдание и **попытаться** убедить родителя…» | modalité affaiblie (« tenter de ») — **3e langue (ar · fa · ru)** ; l'es est fidèle («debe») |
| 11 | **3.1.5** | pioch | « Sa **conquête** de la veille » | "His conquest" | «Новая **избранница**» (« sa nouvelle élue ») | la conquête devient une élue — même adoucissement que ar · fa ; l'es est fidèle («Su conquista») — **3e langue (ar · fa · ru)** |
| 12 | **4.1.11** | enjeu | « …que Mars est une destination de rêve. » | "a dream destination" | «…что **более желанного направления просто не найти**.» | superlatif idiomatique substitué — **4e langue (ar · fa · es · ru)** |
| 13 | **Grammaire ×3** | — | — | — | 1.2.2 enjeu «…убедить императора в том, **индейцев** можно сделать рабами» (**что** manquant) ; 1.3.3 sugg «никогда **не будет согнется**» (double futur) ; 3.2.1 enjeu «…должен **убедить позволить** ему…» (objet de « убедить » manquant) | glisses de grammaire russes, isolées, à confirmer en relecture native (aucune n'est une variante standard connue de nous) |

Les défauts 1, 2, 3, 8, 13 sont prouvables par simple inspection de la cellule ou de la
carte ; 5-7, 9-12 par triple lecture.

---

## 4. Écarts hérités de l'EN — **et la rupture de motif**

La passe ru **rompt** la régularité mesurée sur quatre langues (« N écarts suivent l'EN,
0 l'inverse ») :

**ru suit le FR là où les quatre autres langues suivaient l'EN (6 cartes — les « inverses »)** :

| Carte | Champ | FR | EN | ce que zh·ar·fa·es font | ru |
|---|---|---|---|---|---|
| **3.1.1** | sugg | « Laissez-moi tenter ma chance ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | suivent l'EN | «Дайте мне сегодня попытать счастья; если… не получится, я уступлю вам дорогу.» — **suit le FR** |
| **3.2.2** | sugg | « mes **affaires** ? » | "my **clothes**?" | suivent l'EN (vêtements) | «свои **вещи**?» — **suit le FR** |
| **5.2.5** | enjeu | « Il doit convaincre **Rachel** qu'il ne l'a **pas trompée**. » | "prove her wrong" | suivent la dilution EN | «Он должен убедить **Рэйчел**, что **не изменял ей**.» — **suit le FR, intégralement** |
| **5.3.2** | enjeu | « …de **refuser le vaccin**. » | "refuse to **benefit from it**" | suivent l'EN | «…**отказаться от вакцины**.» — **suit le FR** |
| **6.1.1** | sugg | « …le délai doit courir à partir des faits, **comme en droit commun**. » | "rebuild the people's confidence…" (phrase différente) | suivent l'EN | «…срок должен исчисляться с момента совершения деяния, **как и в общем праве**.» — **suit le FR** |
| **6.2.3** | sugg | « tout le monde réclame votre **démission** » | "everyone claims your **head**" | ar · es suivent l'EN ; fa suit le FR | «все требуют вашей **отставки**» — **suit le FR** (2e réparation après fa) |

**ru suit l'EN contre le FR (4 cartes)** :

| Carte | Champ | FR | EN | ru |
|---|---|---|---|---|
| **3.2.8** | ctxt | « a oublié l'anniversaire » | "forgot the **all-important** birthday" | «напрочь забыл **важнейший** день рождения» — suit l'ajout EN. Note : sur le **concept**, ru est le seul des cinq à être juste (день рождения = anniversaire de naissance partout) — il échappe au défaut fa·es (anniversaire d'événement) |
| **3.3.9** | sugg | question puis exclamation | affirmation cadrée puis question | «Это выглядит совсем нереально. Ты вообще видел размер этой штуки?» — **ordre et cadrage EN suivis** (4e langue : ar · fa · es · ru) |
| **3.2.15** | titre | « Le t-shirt **taché** » | "The **Spaghetti** T-Shirt" | «Футболка со **спагетти**» — 4e langue |
| **4.1.11** | titre | « Destination Mars » | "Mars, the Next Must-See Destination" (titre étendu) | «Марс — новое место, куда обязательно надо съездить» — 4e langue |

**Bilan** : 6 suivis du FR contre 4 suivis de l'EN sur les cartes-tests. Le motif
« traduction sur le bloc EN » ne se généralise pas : **la chaîne ru est distincte** — c'est
la langue la plus proche du FR des cinq lues. La phrase « 0 cas inverse » des dossiers
fa (#1750 §4) et es (#1751 §4) reste vraie **pour leurs langues** mais l'inférence qui la
suivait (« ces écarts relèvent d'une passe sur le bloc EN ») ne survit pas à ru — errata §10.

---

## 5. Matrice inter-langues (état après la passe ru)

Cumulée zh + ar + fa + es + ru ; colonnes pt · en aux passes suivantes.

| Carte | Nature | zh | ar | fa | es | ru |
|---|---|---|---|---|---|---|
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **2.2.9** ctxt | ajout « mythologique » | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **3.2.16** enjeu | modalité affaiblie | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **1.1.3** sugg | lions → félidé domestique | ✓ | ✓ | ✓ | ✓ | — (fidèle : «для моих львов») |
| **4.2.8** titre | « inattendu » ajouté, jeu perdu | ✓ | ✓ | ✓ | ✓ | — (fidèle : «Компрометирующее пробуждение») |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | ✓ | — (ru ponctue) |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | ✓ | **✓ (4 langues)** |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — | ✓ | ✓ | **✓ (4 langues)** |
| **4.1.11** enjeu | superlatif idiomatique | — | ✓ | ✓ | ✓ | **✓ (4 langues)** |
| **3.2.8** | anniversaire d'événement | ✓ (incohérent) | — | ✓ | ✓ | — (concept juste ; ctxt suit l'EN, §4) |
| **1.3.1** sugg | modificateur « sombres/difficiles » | — | ✓ | ✓ | ✓ | — (fidèle : «нелёгкую» = FR «difficiles») |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ | ✓ | ✓ | — (reformule autrement : « si le troisième est un homme ») |
| **4.2.2** ctxt | diminutif « un peu » | — | ✓ | ✓ | ✓ | — (intensifie : «хорошо выпивший») |
| **7.2.5** enjeu | « pousse à » | — | ✓ | ✓ | ✓ | — (fidèle : «пытается» = FR «tente») |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ | ✓ | — | **✓ (ar · fa · ru)** |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ | — | **✓ (ar · fa · ru)** |
| **6.2.1** titre | titre inventé | ✓ | — | — | — | **✓ (zh · ru, inventions distinctes)** |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — | — | — (**zh seul après 4 contrôles**) |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — | ✓ | — (ru ponctue) |
| **3.1.1 · 3.2.2 · 5.2.5 · 5.3.2 · 6.1.1 · 6.2.3** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | **— (ru suit le FR sur les six, §4)** |

**Constat structurant de la passe ru** :

- Le **noyau** zh·ar·fa·es **se scinde** : 3 cartes passent à 5 langues (3.3.2, 2.2.9,
  3.2.16), 3 s'arrêtent à 4 (1.1.3, 4.2.8 — ru fidèle au FR ; 1.2.3 — ru ponctue). Le lot
  n'est donc **pas un destin commun** : c'est un faisceau majoritaire, pas une signature de
  vague unique.
- Le faisceau ar·fa·es **se scinde aussi** : 4.1.11 et 7.3.5 traversent (4 langues), 4.1.12
  et 3.1.5 **y entrent par ru** (ar·fa·ru — l'es y était fidèle), les 4 autres s'arrêtent
  (1.3.1, 3.3.5, 4.2.2, 7.2.5 — ru fidèle ou reformulation propre).
- **6.2.1 tombe de « zh-seul »** : zh et ru inventent chacun leur titre — deux déviations
  distinctes sous le même symptôme. **4.3.3 reste zh-seul après quatre contrôles** — c'est
  désormais l'unique candidat « signature zh » de la matrice.
- **La lecture ru invalide l'homogénéité de la chaîne de production** : une même carte-
  témoin suit l'EN dans quatre langues et le FR dans la cinquième. Les vagues de production
  ne sont pas une.

À verser au passif **EN** (constaté pendant la passe ru) : 7.2.8 sugg *"going to work a
bit"* (contresens — **4e réparation**, ru suit le FR «вы немного торопите события») ·
6.3.2 sugg point final omis (**4e réparation**) · 2.2.1 sugg ponctuation omise (ru répare
en rendant le latin par le russe biblique canonique «Отойди от меня, Сатана») · 3.1.2 titre
"Did you see yourself when you drank" (ru suit le FR «Тяжёлое утро») · 2.2.5 bara "Salomon"
(ru suit le FR «Судья по семейным делам») · 4.3.1 sugg "dizzy" (ru prend son propre chemin
idiomatique «нашло какое-то затмение»).

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **5.1.1** — pioch : « Tournesol » (EN "Cuthbert Calculus") rendu «**профессор Лакмус**» —
  substitution propre (ni translittération du FR ni du EN) ; à confronter au nom russe
  canonique du personnage.
- **5.1.2** — ctxt : « le Grand Schtroumpf » rendu littéralement «**Великий Смурф**» (le
  canon russe dit «Папа Смурф») ; bara : «Смурфик-бунтарь» (« le Schtroumpf rebelle »)
  descriptivise le titre, même geste que l'EN "the smurf" — **mais la suggestion recrée le
  jeu** : «совершенно **не смурфно** согласен» (« ne serait pas smurfement d'accord ») —
  **5e langue à recréer le jeu (zh · ar · fa · es · ru)**, l'EN le perd toujours.
- **7.1.3** — sugg remaniée : «Vous avez vu ? J'ai été sage, moi.» devient «Дедушка Мороз, я
  хорошо себя вел. Ты принес мне подарок?» (perte du reproche, ajout de la question du
  cadeau) ; père Noël → **Дед Мороз** (nativisation défendable).
- **7.1.7** — « minou-minou » rendu «нашей **Мурочки**» (nom affectueux natif de chat — le
  redoublement des quatre autres langues est perdu) ; ajout «**безлактозный**» (sans
  lactose — absent des deux sources) ; perte de « extra-light ».
- **7.1.4** — ctxt : « la mère d'un nouveau-né » rendu «**роженица**» (la parturiente —
  glisse lexicale) ; tiret simple «-» pour la copule au lieu de «—».
- **6.1.1** — le « droit commun » migre : la suggestion le garde (FR suivi, §4) mais
  l'**enjeu ajoute** «как в уголовном праве» (« comme en droit **pénal** » — autre nom de
  droit), absent des deux sources : la carte se cite elle-même avec dérive.
- **6.3.1** — ctxt : ajout «в прессу» (« dans la presse » — la fuite s'y précise, absent des
  deux sources).
- **6.2.2** — le cassoulet **translittéré** «кассуле» (l'es ancre en fabada) : deux
  traitements natifs opposés, tous deux défendables.
- **7.2.12** — « trompe-l'œil » translittéré «в технике тромплёй» — graphie à confirmer.
- **7.2.6** — bara «Фермер» (« l'agriculteur ») pour le canard du titre FR (cf. §3 n° 4,
  même carte : le titre perd « Millésime »).

---

## 7. Ce que la lecture n'établit pas

- **La chaîne de production ru** : 6 cartes suivent le FR, 4 suivent l'EN — mixte ; le
  présent dossier mesure les deux sens, il ne date pas la vague.
- **Que pt · en partageront les lots** : la matrice attend leurs passes.
- **Le canon russe des noms propres** (Лакмус, Великий Смурф, Дед Мороз) : jugement natif.
- **Le statut exact des 3 glisses de grammaire** (§3 n° 13) : à confirmer nativement —
  aucune ne nous apparaît comme variante standard.
- **La fluidité et le registre globaux** : jugement natif. Ce dossier ouvre la matière.

---

## 8. Fidélité élevée — à consigner aussi

- **5.2.1** — titre : «Une fiction pulp» rendu «**Криминальное чтиво**» — **le titre russe
  officiel de Pulp Fiction** ; Ezéchiel passe en registre slavon («И поражу я дланью
  ужасного гнева и яростной мести…») — l'archaïsme sacré que le FR porte par la citation
  d'Ézéchiel.
- **1.3.4** — «Tous les hommes naissent libres et égaux…» rendu par le **texte canonique
  russe de l'Article 1 de la DUDH** («Все люди рождаются свободными и равными в
  достоинстве и правах»).
- **1.1.2** — «Détruire Carthage» rendu «**Карфаген должен быть разрушен**» — la formule
  canonique russe de *Carthago delenda est*.
- **2.2.1** — «Vade retro, Satanas» rendu par le russe biblique canonique («Отойди от меня,
  Сатана», Mc 8.33), et ponctué comme le FR — **réparation** de l'omission EN.
- **3.1.7** — «pas l'oreille musicale» rendu «**медведь на ухо наступил**» — l'idiome
  russe natif exact.
- **4.1.13** — «pleuvoir des cordes» rendu «**льёт как из ведра**» — idiome natif exact.
- **7.1.1** — le « bac avec mention Très bien » naturalisé en «**аттестат зрелости с
  отличием**» — l'équivalent russe précis.
- **5.1.2** — le jeu **recréé** («не смурфно») — 5e langue (§6).
- **7.2.8 · 6.3.2 · 2.2.1** — ru **répare** là où l'EN est cassé (contresens, points omis) ;
  **3.1.2 · 2.2.5** — ru suit le FR là où l'EN dévie (titre, bara).
- **Ponctuation** — ru ponctue systématiquement (1.2.3, 2.1.8, 6.3.2) : la seule langue des
  cinq qui n'a **aucune** omission de point final sur le corpus.

---

## 9. Verdict ru

- **Couverture : complète** (1169/1169, 0 vide), aucune contamination FR.
- **13 défauts établis** : deux majeurs propres à ru (7.2.7 punchline amputée de
  l'entretien ; 3.2.4 fuite du terme d'interface «Берущий карту»), 6.2.1 rejoint zh, la
  famille des titres réécrits (~12), cinq extensions de familles inter-langues (3.3.2,
  2.2.9, 3.2.16 à 5 langues ; 4.3.4, 7.3.5, 4.1.11 à 4 ; 4.1.12, 3.1.5 à 3), trois
  glisses de grammaire.
- **La passe renverse deux généralisations** : « 0 cas inverse » (ru suit le FR sur 6 cartes-
  témoins — c'est la langue la plus proche du FR des cinq lues) et « 6.2.1 zh-seul ».
- **4.3.3 devient l'unique zh-seul** de la matrice, après quatre contrôles.
- **0 écriture** : la relecture native décidera des corrections.

---

## 10. Errata déclenchés par cette passe

Conformément au protocole appliqué à #1749 par la passe fa, les lignes suivantes des
dossiers déjà publiés sont **périmées** et font l'objet d'un commentaire d'erratum sur
leurs PR :

1. **#1751 (es)** — §5 « 6.2.1… restent zh-seuls après trois langues de contrôle » et §9
   « 6.2.1 confirmé zh-seul par un 3e contrôle » : **tombé** — ru invente «Рокировка» ;
   6.2.1 = zh·ru, inventions distinctes. Idem §4 : « 0 cas inverse — le motif est stable » —
   l'inférence ne survit pas à ru (6 inverses, §4 du présent dossier).
2. **#1750 (fa)** — matrice « 6.2.1 et 4.3.3 confirmés zh-seuls après deux langues de
   contrôle » : la moitié **6.2.1** tombe (4.3.3 tient, renforcé à quatre contrôles).
3. **#1749 (ar)** — si sa matrice porte la même ligne 6.2.1 zh-seul, même erratum.

*po-2024*
