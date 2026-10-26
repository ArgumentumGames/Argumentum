# Campagne de fidélité Scénarios — **pt** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture à trois voies intégrale. Le présent dossier **remplace
la moitié pt de la PR #1747** (l'écran mécanique est restitué en §2).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus trois voies généré depuis le CSV (`dump3way_pt.py`, dérivé par substitution de colonnes
du générateur ar, scratchpad de session, régénérable en une passe) : chaque carte × 6 champs
rendus × **FR | EN | pt** (3340 lignes), **lu intégralement en 4 passes**. Chaque écart est
relu contre **les deux sources** et confronté aux listes établies par les passes zh (#1746),
ar (#1749), fa (#1750), es (#1751) et ru (#1752).

La passe pt est la **sixième**. Elle **confirme le renversement opéré par ru** (§4 : pt suit
lui aussi le FR sur les cartes-témoins) et apporte deux pièces que nulle autre langue ne
portait : **le résidu français du jeu de mots Schtroumpf, confirmé sur le champ signalé par
ai-01** (§3 n° 1), et **un contresens EN reproduit** là où quatre langues réparaient
(§4, 7.2.8).

---

## 2. Écran (mécanique — restitué de la première passe)

Écran multi-langue comparant chaque cellule à FR **et** EN (structure : #1747). Pour pt,
écriture latine : le signal « latin résiduel » est sans objet, la comparaison « cellule = FR »
sur le bloc EN s'applique.

Mesures pt de cette passe (sur le corpus même) :

- **Couverture : 1169 / 1169 cellules, 0 vide** (167 cartes × 7 champs rendus, vérifié au grep).
- **Ponctuation finale : 2 cellules sur 501 sans point** (ctxt 0, enjeu 1, sugg 1 — mesuré par
  script sur le corpus) : **1.1.1 enjeu** (isolé — FR et EN ponctuent tous deux, §3 n° 6) et
  **6.3.2 sugg** (suit l'omission de l'EN, §4). pt **ponctue** donc les cartes des lots
  connus (1.2.3, 2.1.8, 2.2.1) — aucune n'apparaît dans les lots de ponctuation.
- **Famille « espace-tiret »** — 4 cellules pt (plus 1 EN) portent « - » au lieu de «- »
  (§3 n° 5), mesuré au grep : 1.3.2 pioch, 5.3.2 bara, 5.3.4 pioch, 6.1.2 bara.

⚠️ Ce que l'écran ne dit pas : il rendrait pt quasi propre (2 points manquants, une poignée de
« - »). La lecture à trois voies établit **6 défauts**, dont un résidu français d'un mot
entier dans une cellule imprimée, et **un contresens EN reproduit**.

---

## 3. Défauts pt établis (à trois voies)

| # | Carte | Champ | FR | EN | pt | Nature |
|---|---|---|---|---|---|---|
| 1 | **5.1.2** | baratineur | « Le **shtroumphissime** » | "the smurf" | «**Shtroumphissime**» | **mot français laissé tel quel** dans la cellule imprimée — la seule des six langues à ne pas forger son nom : zh·ar·fa·es·ru créent tous leur superlatif smurf. C'est **le champ signalé par ai-01 (pt)** — **confirmé à la lecture**. Le jeu est pourtant recréé ailleurs dans la carte («**smurfamente**», 6e langue à le recréer, l'EN le perd) avec le mot **Smurf** (et non le mot pt canonique) et «Grande Smurf» pour « Grand Schtroumpf » — cf. §6 |
| 2 | **1.2.2** | baratineur | « Un **esclavagiste** » | "A **slave driver**" | «Uma **escravidão**» | **le rôle est remplacé par le concept** : la cellule annonce « l'esclavage », pas l'esclavagiste. Les deux sources nomment l'agent, pt nomme l'institution — aucune autre langue ne commet cette substitution |
| 3 | **2.2.7** | titre | « …de **Pénélope** » | "…of **Penelope**" | «O pretendente de **Pénélope**» | **accents français** dans le titre — et **incohérence interne** : le pioch de la **même carte** écrit la forme pt correcte «**Penélope**». Résidu FR invisible pour l'écran (comparaison de cellules entières) |
| 4 | **3.2.7** | titre | « L'amour à la plage » | "Love on the Beach" | «**É** amor na praia» | **copule ajoutée** : « L'amour à la plage » devient « **C'est** l'amour à la plage » — le titre nominal est converti en phrase, sans source |
| 5 | **Famille « - »** | — | — | — | 1.3.2 pioch «Um general de 5 **-estrela**» (espace-tiret **et** singulier), 5.3.2 bara «Um anti **-vaccina**», 5.3.4 pioch «Um turista sul **-coreano**», 6.1.2 bara «Porta **-voz** do governo» | **4 cellules** portant « - » (espace avant le trait d'union) ; l'EN porte la même malformation sur 5.3.2 ("An anti -vaccin") — artefact partagé, à corriger par une passe mécanique. Le 1.3.2 y ajoute une faute de nombre («estrela» → «estrelas») |
| 6 | **1.1.1** | enjeu | « …la reine d'Égypte**.** » | "…queen of Egypt**.**" | «…a rainha do Egito» (sans point) | point final absent — **isolé pt** : les deux sources ponctuent, et c'est la **seule** cellule d'enjeu du corpus pt dans ce cas (mesuré : 1/167) |

Les défauts 1, 2, 3 sont prouvables par inspection de la cellule (mot étranger, substitution de
rôle, incohérence titre/pioch de la même carte) ; 4 et 5 par comparaison aux sources.

---

## 4. Écarts hérités de l'EN — pt suit le FR sur six cartes-témoins, et reproduit un contresens

Comme ru, **pt rompt** la régularité mesurée sur zh·ar·fa·es (« les écarts suivent l'EN,
0 inverse ») :

**pt suit le FR là où les quatre premières langues suivaient l'EN (6 cartes)** :

| Carte | Champ | FR | EN | zh·ar·fa·es | pt |
|---|---|---|---|---|---|
| **3.1.1** | sugg | « Laissez-moi tenter ma chance ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | suivent l'EN | «Deixe-me tentar a minha sorte esta noite; se eu falhar, deixo-lhe o caminho livre.» — **suit le FR** |
| **5.2.5** | enjeu | « Il doit convaincre **Rachel** qu'il ne l'a **pas trompée**. » | "prove her wrong" | suivent la dilution EN | «Ele deve convencer **Rachel** de que **não a traiu**.» — **suit le FR, intégralement** |
| **5.3.2** | enjeu | « …de **refuser le vaccin**. » | "refuse to **benefit from it**" | suivent l'EN | «…a **recusar a vacina**.» — **suit le FR** |
| **6.1.1** | sugg | « …le délai doit courir à partir des faits, **comme en droit commun**. » | "rebuild the people's confidence…" (phrase différente) | suivent l'EN | «…o prazo deve correr a partir dos factos, **como no direito comum**.» — **suit le FR** |
| **6.2.3** | sugg | « tout le monde réclame votre **démission** » | "everyone claims your **head**" | ar · es suivent l'EN | «todos exigem sua **demissão**» — **suit le FR** (3e réparation après fa et ru) |
| **5.3.1** | enjeu | « …convaincre l'auditoire que **le cosmonaute** est un imposteur. » | "…that **the latter** is an impostor" | — (non mesuré) | «convencer o público de que **o cosmonauta** é um impostor» — suit le FR là où l'EN se dérobe |

**pt suit l'EN (et non le FR) sur 3 cartes** : **3.2.2** sugg («minhas **roupas**» = "clothes" contre
FR «affaires») · **3.3.9** sugg (structure «affirmation puis question» suivie : «Isto parece-me
completamente surrealista; quer dizer, viste o tamanho desta coisa?» — **5e langue** avec
ar·fa·es·ru) · **3.2.15** titre («A T-shirt com **esparguete**» — **5e langue**).

**Deux écarts EN reproduits — pt ne répare pas** :

| Carte | Champ | EN | pt | Les autres langues |
|---|---|---|---|---|
| **7.2.8** | sugg | *"don't you think you're going to work a bit?"* (**contresens**) | «você não acha que **vai trabalhar um pouco**?» — **le contresens est reproduit** | ar · fa · es · **ru** suivent le FR («toropite/торопите события»…) : **4 langues réparaient, pt non** |
| **6.3.2** | sugg | "…could elect a traitor" (**point final omis**) | «…poderiam eleger um traidor» (point omis) | ar · fa · es · **ru** ajoutent le point : **4 réparations, pt suit l'omission** |

**Bilan** : 6 suivis du FR contre 3 suivis de l'EN + 2 écarts EN recopiés. Comme ru, **pt est une
langue de la chaîne FR**, non du bloc EN — mais il **recopie** deux défauts EN que ru, lui,
réparait. Sur les six langues lues, deux seulement (ru, pt) suivent le FR ; elles divergent
entre elles sur 7.2.8 et 6.3.2.

---

## 5. Matrice inter-langues (état après la passe pt)

Cumulée zh + ar + fa + es + ru + pt ; colonne **en** à la dernière passe.

| Carte | Nature | zh | ar | fa | es | ru | pt |
|---|---|---|---|---|---|---|---|
| **2.2.9** ctxt | ajout « mythologique » | ✓ | ✓ | ✓ | ✓ | ✓ | **✓ (6 langues — seule carte à six)** |
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | ✓ | ✓ | — (fidèle) |
| **3.2.16** enjeu | modalité affaiblie | ✓ | ✓ | ✓ | ✓ | ✓ | — (fidèle : «Tem de convencer») |
| **1.1.3** sugg | lions → félidé domestique | ✓ | ✓ | ✓ | ✓ | — | — (fidèle : «os meus leões») |
| **4.2.8** titre | « inattendu » ajouté, jeu perdu | ✓ | ✓ | ✓ | ✓ | — | — (fidèle : «Despertar comprometido») |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | ✓ | — | — (pt ponctue) |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | ✓ | ✓ | — (fidèle : **«Veneza» conservé**) |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — | ✓ | ✓ | ✓ | — (fidèle) |
| **4.1.11** enjeu | superlatif idiomatique | — | ✓ | ✓ | ✓ | ✓ | — (fidèle : «destino de sonho») |
| **4.1.11** titre | titre étendu EN suivi | — | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **3.3.9** sugg | structure EN suivie | — | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **3.2.15** titre | « spaghetti » suivi | — | ✓ | ✓ | ✓ | ✓ | **✓ (5 langues)** |
| **3.2.8** | anniversaire d'événement | ✓ (incohérent) | — | ✓ | ✓ | — | — (concept juste : aniversário = anniversaire de naissance) |
| **3.2.8** ctxt | ajout EN « all-important » | — | — | ✓ | — | ✓ | **✓ («importantíssimo»)** |
| **1.3.1** sugg | modificateur « sombres » | — | ✓ | ✓ | ✓ | — | — (fidèle : «vidas difíceis» = FR) |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ | ✓ | ✓ | — | — (reformule : «em vez de outra mulher») |
| **4.2.2** ctxt | diminutif « un peu » | — | ✓ | ✓ | ✓ | — | **✓ («meio embriagado» — 4 langues)** |
| **7.2.5** enjeu | « pousse à » | — | ✓ | ✓ | ✓ | — | — (fidèle : «tenta convencer» = FR «tente») |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ | ✓ | — | ✓ | — (fidèle : «Sua conquista») |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ | — | ✓ | **✓ («tentar convencer» — 4 langues)** |
| **6.2.1** titre | titre inventé | ✓ | — | — | — | ✓ | — (fidèle : «Retirada negociada») |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — | — | — | — (**zh seul après 5 contrôles**) |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — | ✓ | — | — (pt ponctue) |
| **3.1.1 · 5.2.5 · 5.3.2 · 6.1.1 · 6.2.3** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | **—** | **—** (ru et pt suivent le FR) |
| **3.2.2** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | — | **✓ (seul témoin où pt suit l'EN)** |
| **7.2.8** sugg | contresens EN | ✓ | — | — | — | — | **✓ (pt recopie ; ar · fa · es · ru réparaient)** |
| **6.3.2** sugg | point final omis | ✓ | — | — | — | — | **✓ (pt recopie ; ar · fa · es · ru réparaient)** |

**Constat structurant de la passe pt** :

- **2.2.9 est la seule carte à six langues** : c'est le seul ajout qu'aucune lecture n'a
  démenti. Le noyau des cinq autres cartes s'est scindé langue par langue — il n'y a pas de
  « vague » unique, il y a des faisceaux qui se croisent.
- **4.3.3 reste zh-seul après cinq contrôles** (ar, fa, es, ru, pt tous fidèles) : c'est le
  seul candidat « signature zh » survivant de la matrice.
- **6.2.1 = zh · ru** (confirmé : ar, fa, es, pt fidèles).
- **ru et pt forment le couple fidèle-au-FR** : mêmes 5 réparations sur les cartes-témoins
  (3.1.1, 5.2.5, 5.3.2, 6.1.1, 6.2.3) et **même carte divergente en sens inverse** — mais pt
  se sépare de ru sur 3.2.2 (suit l'EN), 7.2.8 et 6.3.2 (recopie au lieu de réparer).
- **Le passif EN se précise** : 7.2.8 (4 réparations sur 5 langues mesurées — pt recopie),
  6.3.2 (idem), 3.2.2 bara/3.2.8 ctxt « all-important » (suivis par pt).

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **PT-BR et PT-PT mêlés** — la variante change d'une carte à l'autre : «celular» (5.3.4,
  BR) vs «desporto» (3.2.9, PT), «Polônia» (4.2.3, BR) vs «Polónia», «ônibus» (7.3.1, BR) vs
  «autocarro», «prefeito» (6.2.2, BR) vs «Presidente da Câmara» (2.1.9) **pour le même rôle
  « le maire »**, «gêmeo»/«gémeo». C'est le pendant pt du mélange tú/usted d'es.
- **Registres d'allocution mêlés** — tu («Achas», 2.2.8 ; «não te quero», 3.1.7 ; «Deves
  odiar-me», 3.2.15), você («Você tem seu cartão Vitale?», 4.3.2), o senhor («o senhor deveria»,
  5.3.2 ; «Senhor prefeito», 6.2.2). Le FR vouvoie partout.
- **5.1.1** — pioch : « Tournesol » (EN "Cuthbert Calculus") rendu «**Girassol**» — traduction
  littérale du nom (là où ru substitue «Лакмус» et EN adopte le nom officiel) ; à confronter
  au nom pt canonique du personnage.
- **5.1.2** — ctxt : « le Grand Schtroumpf » rendu «**Grande Smurf**» avec le mot **Smurf**
  (emprunt EN/FR), là où le pt canonique dit «Estrunfe» ; même mot dans enjeu («o forte smurf»)
  et sugg. Le jeu est recréé («smurfamente») mais sur un mot importé.
- **4.3.2** — la « carte Vitale » (franco-française) **conservée** «cartão Vitale» (l'es la
  généricise en «tarjeta sanitaria», l'EN reste piégé "Vital card").
- **7.2.12** — « trompe-l'œil » conservé tel quel dans le titre et le ctxt (l'es naturalise
  «trampantojo» ; ru translittère).
- **4.1.9** vs **4.1.7** — le même rôle (livreur de pizza) rendu «Estafeta de pizzas» (PT-PT)
  et «O entregador de pizzas» (BR) dans deux cartes voisines.
- **7.3.1** — pioch «Uma pessoa **velha**» (BR/peu flatteur) vs ctxt «Uma pessoa **idosa**» :
  incohérence lexicale interne à la carte.
- **7.1.7** — « minou-minou » rendu «**o gatinho**» (diminutif — le redoublement des quatre
  autres langues est perdu) ; le reste de la phrase suit le FR (pas d'ajout).
- **2.3.2** — « Côté obscur » rendu «**Lado negro**» (le canon pt de Star Wars est «Lado
  Sombrio»).
- **3.2.3** — « mon fiston » rendu «o meu **filhote**» (terme animal, familier) : registre.

---

## 7. Ce que la lecture n'établit pas

- **La chaîne de production pt** : 6 cartes suivent le FR, 3 l'EN, 2 défauts EN recopiés —
  mixte ; le présent dossier mesure, il ne date pas les vagues.
- **Que en partagera les lots** : la matrice attend la dernière passe.
- **Le canon pt des noms propres** (Girassol, Smurf/Estrunfe, Vitale) : jugement natif.
- **La cohérence PT-BR/PT-PT voulue** : si le projet vise une variante unique, le mélange est
  un défaut d'édition ; s'il accepte les deux, c'est un fait de style — à trancher par l'auteur.
- **La fluidité et le registre globaux** : jugement natif. Ce dossier ouvre la matière.

---

## 8. Fidélité élevée — à consigner aussi

- **4.3.5** — « que dalle » rendu «**patavina**» («Vão perceber... patavina») — l'idiome pt
  exact pour « rien du tout ».
- **4.1.13** — « pleuvoir des cordes » rendu «está a chover **a cântaros**» — idiome natif
  exact (es : a cántaros ; ru : льёт как из ведра).
- **5.2.1** — Ézéchiel en registre biblique pt («abaterei o braço de uma terrível cólera, de
  uma vingança furiosa e assustadora… o meu nome é o Eterno»).
- **1.3.4** — texte canonique de l'Article 1 de la DUDH en pt («Todos os seres humanos nascem
  livres e iguais em direitos e dignidade»).
- **2.2.1** — «Vade retro, Satanas» **conservé en latin** et **ponctué comme le FR** (l'EN
  omet le point) : réparation.
- **4.3.1** — « j'ai la berlue » rendu «estou tendo **alucinações**» — pt **répare** le
  contresens EN ("dizzy") par son propre idiome (comme es «viendo visiones», ru «затмение»).
- **5.3.1** — « l'auditoire que le cosmonaute est un imposteur » : pt nomme le cosmonaute là
  où l'EN se dérobe ("the latter").
- **7.2.6** — « Millésime de foie gras » rendu «**Safra** de foie gras» — le mot juste (là où
  ru perd le millésime).
- **7.1.5** — « La kermesse » rendue «**A quermesse**» — le mot pt de même racine (là où ru
  généricise «Праздник»).
- **7.1.1** — « bac avec mention Très bien » naturalisé «concluir o **ensino médio** com
  excelentes notas».
- **5.1.2** — le jeu de mots **recréé** («nada **smurfamente** de acordo») — **6e langue**
  (zh · ar · fa · es · ru · pt), l'EN le perd toujours — malgré le résidu du bara (§3 n° 1).

---

## 9. Verdict pt

- **Couverture : complète** (1169/1169, 0 vide), aucune contamination FR détectée en masse.
- **6 défauts établis** : le **résidu français «Shtroumphissime»** (le signal ai-01 sur pt —
  **confirmé**), la substitution de rôle (1.2.2 «escravidão»), les accents FR du titre 2.2.7
  (avec incohérence interne), la copule ajoutée (3.2.7), la famille « - » (4 cellules), le
  point isolé de 1.1.1 — plus **deux défauts EN recopiés** (7.2.8, 6.3.2).
- **La matrice se stabilise** : **2.2.9 seule carte à six langues** ; **4.3.3 zh-seul confirmé
  après 5 contrôles** ; **6.2.1 = zh · ru** ; **ru et pt = couple fidèle au FR**, divergents
  entre eux sur 3 cartes.
- **0 écriture** : la relecture native décidera des corrections.

---

## 10. Aucun erratum déclenché

La passe pt **ne périme aucune ligne** des dossiers déjà publiés : elle confirme les
classifications issues de ru (#1752) — 4.3.3 zh-seul, 6.2.1 zh·ru, lot EN scindé — et
n'ajoute pas de membre aux familles saturées. Elle **enrichit** en revanche la lecture de
5.1.2 : le jeu de mots est recréé dans la suggestion (6e langue) **et** le baratineur reste
en français — les deux constats coexistent dans la même carte.

*po-2024*
