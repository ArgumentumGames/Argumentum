# Campagne de fidélité Scénarios — **es** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, renvoi c.5977925408 (sur la PR #1747) + dispatch c.5977929362 — les
six dossiers de la première passe étaient des **écrans mécaniques, pas des lectures** ; reprise
en **une PR par langue** avec lecture à trois voies intégrale. Le présent dossier **remplace
la moitié es de la PR #1747** (l'écran mécanique est restitué en §2).
**Objet** : les 167 cartes du deck Scénarios, champs rendus par le gabarit de carte.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

---

## 1. Méthode

Corpus trois voies généré depuis le CSV (`dump3way_es.py`, scratchpad de session, régénérable
en une passe) : chaque carte × 6 champs rendus × **FR | EN | es** (3340 lignes), **lu
intégralement en 4 passes**. Chaque écart est relu contre **les deux sources** et confronté aux
listes établies par les passes zh (#1746), ar (#1749) et fa (#1750).

La passe es est la **première en écriture latine** : elle tranche la question laissée ouverte
par le dossier fa (§5) — le faisceau ar·fa de rendus jumeaux **traverse-t-il les écritures
latines ?** Réponse mesurée ci-dessous §5 : **une partie oui** (7 cartes), deux non.

---

## 2. Écran (mécanique — restitué de la première passe)

Écran multi-langue comparant chaque cellule à FR **et** EN (structure : #1747). Les pièges
d'instrument corrigés en route y sont consignés — pour es, écriture latine, deux des trois
pièges ne s'appliquent pas :

| Piège | Applicable à es | Correction héritée |
|---|---|---|
| Jeu de ponctuation latin testé sur une écriture RTL | non (es est latin) | jeu **par écriture** |
| « latin résiduel » cherché dans une langue à écriture latine | **signal désactivé** (1169/1169 de faux) | — |
| Comparaison « cellule = FR » sur le bloc EN | oui | colonnes EN **nues** |

Mesures es de cette passe (sur le corpus même) :

- **Couverture : 1169 / 1169 cellules, 0 vide** (167 cartes × 7 champs rendus, vérifié au grep).
- **Ponctuation finale** — es appartient aux **deux** lots : **1.2.3** (ctxt «…invadir Rusia»
  et enjeu «Intentar disuadirlo» sans point, lot es·ar·fa·zh) **et** **2.1.8** (sugg «…no se
  enfade» sans point, lot es·ar·zh — fa ponctue). C'est la seule langue dans les deux. Sur
  **2.2.1**, es suit l'omission de l'EN («Vade retro, Satanás» sans point) ; sur **6.3.2**, es
  **répare** (point ajouté, comme ar et fa). **Nouveau, isolé es : 7.2.3 ctxt sans point
  final** (§3 n° 7).
- **CHIFFRE** — es garde les chiffres («1000 historias durante 1001 noches», 25 €, 2019, 5G) :
  un écran qui sonde les chiffres ne produira **aucun faux** sur es, contrairement à ar (mots)
  et fa (chiffres persans mêlés).

⚠️ Ce que l'écran ne dit pas : il rendait « 0 défaut dur » (dossier #1747 rejeté). La lecture
à trois voies établit ci-dessous **7 défauts de fidélité et 9 héritages EN** invisibles pour
lui.

---

## 3. Défauts es établis (à trois voies)

| # | Carte | Champ | FR | EN | es | Nature |
|---|---|---|---|---|---|---|
| 1 | **1.1.3** | suggestion | « Voilà un mets de choix pour **mes lions**. » | "Now that is a choice dish for **my lions**." | «Mmm, un bocado exquisito para mis **gatitos**.» | félidé domestique substitué au lion **+ interjection** («Mmm») ; contredit l'enjeu de la même carte («no echarlo a **los leones**») — **4e langue** (zh · ar · fa · es), même carte, même champ |
| 2 | **3.3.2** | enjeu | « …remplacer le mariage par l'adoption d'un caillou… » | idem | «…en vez de casarse, podrían **demostrar su compromiso mutuo** adoptando una piedra como mascota.» | ajout « compromiso mutuel », absent des deux sources — **4e langue** (zh · ar · fa · es) ; le signal ai-01 (es · ru) est **confirmé à la lecture pour es** |
| 3 | **3.2.8** | titre + ctxt + sugg | « **anniversaire** » / "birthday" | idem | **aniversario** partout (« mi aniversario » dans la punchline) | l'anniversaire de naissance se dit **cumpleaños** ; aniversario désigne l'anniversaire d'**événement** — glisse de concept uniforme à travers la carte, **jumeau exact du défaut fa n° 3** (سالگرد) : famille **fa · es**, même nature ; l'ar est fidèle (عيد ميلاد) ; zh reste classé à part (incohérence interne) |
| 4 | **4.2.8** | titre | « Réveil compromis » | "Compromising Wake-Up" | «Despertar **imprevisto** y compromiso delicado» | « inattendu » ajouté (**4e langue** zh · ar · fa · es) et le double sens du titre (compromettant / compromis) **dédoublé** en deux mots — le jeu est explicité, pas rendu |
| 5 | **4.3.4** | contexte | « …une IA éthique et responsable, **sans aucun garde-fou.** » | idem | «…sin ninguna salvaguarda, **y debe convencer a un comité de apoyar su proyecto**.» | l'**enjeu est recopié dans le contexte** (la carte dit deux fois la même chose) — **3e langue** (zh · fa · es ; l'ar est fidèle) |
| 6 | **7.3.5** | suggestion | « **Le thème de la soirée, c'est Venise.** Pourquoi êtes-vous déguisé en fromage ? » | idem | «**¿Cuál era el tema de la fiesta?** Pareces un queso, ¿en serio?» | punchline **réécrite en question** (« quel était le thème ? » — l'hôte demande ce qu'il sait) : l'ancrage **Venise** disparaît — **3e langue** (ar · fa · es), réécriture de même structure |
| 7 | **7.2.3** | contexte | « …a ravagé le jardin du voisin**.** » | "…wrecked the neighbor's garden**.**" | «…ha destrozado el jardín de su vecino» (sans point) | point final absent — **isolé es** (FR et EN ponctuent tous deux) ; non détecté par la première passe |

Les défauts 1 et 5 sont prouvables sans source externe (contradiction/duplication interne à la
carte), le 7 par simple inspection ; les 2, 3, 4, 6 par triple lecture (le 3 par dictionnaire :
cumpleaños ≠ aniversario).

---

## 4. Écarts **hérités de l'EN** — et non défauts es

Les cinq écarts connus depuis les passes zh/ar/fa sont **confirmés en es** (4e langue), plus
**quatre nouveaux**, trouvés par la présente passe puis vérifiés rétroactivement sur les
corpora ar et fa :

| Carte | Champ | FR | EN | es |
|---|---|---|---|---|
| **3.1.1** | suggestion | « Laissez-moi tenter ma chance ; si j'échoue, je vous laisse le champ libre. » | "We can both try, we'll see who gets picked." | «Podemos intentarlo los dos; ya se verá a quién eligen.» |
| **3.2.2** | suggestion | « …mes **affaires** ? » | "…my **clothes**?" | «…mi **ropa**?» |
| **6.1.1** | suggestion | « Pour restaurer la confiance, il faut des règles claires : le délai doit courir à partir des faits, comme en droit commun. » | "It is absolutely necessary to rebuild the people's confidence…" | «Es absolutamente necesario reconstruir la confianza del pueblo en la clase política.» — note : l'enjeu es récupère «como en el derecho común» (§6) |
| **5.2.5** | enjeu | « Il doit convaincre Rachel qu'il ne l'a pas trompée. » | "try to prove her wrong" | «Convencerla de lo contrario.» — dilution suivie **et aggravée** (Rachel disparaît) |
| **5.3.2** | enjeu | « …de **refuser le vaccin**. » | "to refuse **to benefit from it**" | «…de rechazar **beneficiarse de ella**.» |
| **3.3.9** | suggestion | « Vous avez vu la taille de ce truc ? C'est complètement délirant ! » (question puis exclamation) | "This seems completely insane to me. I mean, have you seen the size of that thing?" (affirmation cadrée puis question) | «Esto me parece completamente surrealista; a ver, ¿has visto el tamaño de esa cosa?» — **ordre et cadrage EN suivis** ; l'ar (أعني) et le fa (یعنی) les suivent aussi : **nouveau membre, ar · fa · es** |
| **3.2.15** | titre | « Le t-shirt **taché** » | "The **Spaghetti** T-Shirt" | «La camiseta de **espaguetis**» — suit l'EN ; l'ar (قميص السباغيتي) aussi ; le fa est hybride (اسپاگتی‌خوره, « rongé aux spaghettis ») |
| **4.1.11** | titre | « Destination Mars » | "Mars, the Next Must-See Destination" | «Marte, el próximo destino imprescindible» — le titre **étendu EN** est suivi ; l'ar et le fa aussi |
| **6.2.3** | suggestion | « tout le monde réclame votre **démission** » | "everyone claims your **head**" | «todo el mundo reclama su **cabeza**» — suit l'EN ; l'ar (برأسك) aussi ; **le fa suit le FR** (استعفای « démission ») — 2e réparation fa |

**9 écarts suivent l'EN, 0 cas inverse — après quatre langues, le motif est stable** : ces
écarts relèvent d'une passe sur le **bloc EN**.

---

## 5. Matrice inter-langues (état après la passe es)

Cumulée zh + ar + fa + es ; les colonnes ru · pt · en se remplissent à mesure des passes.

| Carte | Nature | zh | ar | fa | es | autres |
|---|---|---|---|---|---|---|
| **3.3.2** enjeu | ajout « engagement mutuel » | ✓ | ✓ | ✓ | ✓ | ru (ai-01) — à confirmer |
| **1.1.3** sugg | lions → félidé domestique | ✓ (小猫咪) | ✓ (قطتي الصغيرة) | ✓ (بچه‌گربه‌های من) | ✓ (gatitos) | — |
| **2.2.9** ctxt | ajout « mythologique » | ✓ | ✓ | ✓ | ✓ (personaje mitológico) | — |
| **3.2.16** enjeu | modalité affaiblie | ✓ (试图) | ✓ (يحاول) | ✓ (سعی می‌کند) | ✓ (Intentar) | — |
| **4.2.8** titre | « inattendu » ajouté, jeu « compromis » perdu | ✓ | ✓ | ✓ | ✓ (imprevisto) | — |
| **1.2.3** ctxt+enjeu | ponctuation finale absente | ✓ | ✓ | ✓ | ✓ | — |
| **7.3.5** sugg | punchline réécrite, « Venise » perdu | — | ✓ | ✓ | ✓ | — |
| **4.3.4** ctxt | enjeu recopié dans le contexte | ✓ | — (fidèle) | ✓ | ✓ | — |
| **3.2.8** | anniversaire d'événement pour anniversaire de naissance | ✓ (incohérent) | — (fidèle) | ✓ (uniforme سالگرد) | ✓ (uniforme aniversario) | — |
| **1.3.1** sugg | ajout ornemental « et sombres » | — | ✓ (ومعتمة) | ✓ (و تیره) | ✓ (oscuras) | — |
| **3.3.5** enjeu | explicitation « et lui-même » | — | ✓ (ومعه) | ✓ (و خودش) | ✓ (y él) | — |
| **4.2.2** ctxt | « un peu ivre » | — | ✓ (بعض الشيء) | ✓ (کمی مست) | ✓ (algo achispado) | — |
| **4.1.11** enjeu | superlatif idiomatique | — | ✓ | ✓ | ✓ (no puede ser más deseable) | — |
| **7.2.5** enjeu | « pousse à » | — | ✓ (يدفع) | ✓ (هل می‌دهد) | ✓ (Empujar) | — |
| **3.1.5** pioch | « conquête » adoucie | — | ✓ | ✓ | — (fidèle : Su conquista) | — |
| **4.1.12** enjeu | modalité affaiblie | — | ✓ | ✓ | — (fidèle : debe) | — |
| **4.1.1** enjeu | modalité affaiblie | — | — | ✓ | — (fidèle : debe) | — |
| **6.2.1** titre | titre inventé | ✓ | — | — | — (fidèle : Retirada negociada) | — |
| **4.3.3** enjeu | rôle ajouté | ✓ | — | — | — (explicitation « el revendedor », autre nature — §6) | — |
| **2.1.8** sugg | ponctuation finale absente | ✓ | ✓ | — (fa ponctue) | ✓ | — |
| **3.1.1 · 3.2.2 · 6.1.1** | suit l'EN contre le FR | ✓ | ✓ | ✓ | ✓ | à mesurer (ru · pt · en) |

**Constat structurant de la passe es** :

- Le **noyau zh·ar·fa passe à zh·ar·fa·es sur ses 6 cartes** (3.3.2, 1.1.3, 2.2.9, 3.2.16,
  4.2.8, 1.2.3) — ce n'est pas une signature d'écriture, le lot traverse.
- Le **faisceau ar·fa se scinde** : 5 de ses 8 cartes traversent (1.3.1, 3.3.5, 4.2.2, 4.1.11,
  7.2.5) plus 7.3.5 (déjà ar·fa → ar·fa·es) ; **2 restent ar·fa** (3.1.5, 4.1.12 — es y est
  fidèle au FR). L'hypothèse « même vague de production ar/fa » du dossier fa s'affaiblit :
  le lot majoritaire n'est pas corrélé à l'écriture.
- **6.2.1** (titre inventé) et **4.3.3** (rôle ajouté) restent **zh-seuls après trois langues
  de contrôle**.
- **3.2.8** cristallise une famille **fa·es** (glisse de concept uniforme) distincte du zh
  (incohérence interne) — deux objets différents sous le même symptôme apparent.

À verser au passif **EN** (constaté pendant la passe es) : 7.2.8 sugg *"going to work a bit"*
(contresens — es suit le FR sainement, 3e contrôle après ar et fa) · 5.3.2 bara *"An anti
-vaccin"* · 5.1.2 bara *"the smurf"* perd le titre · 4.3.1 sugg *"dizzy"* perd la berlue
(l'es suit le FR «estoy viendo visiones») · 3.2.8 ctxt "all-important" ajouté (l'es ne le suit
pas — il a son propre défaut, §3 n° 3) · 2.2.5 bara "Salomon" (l'es suit le FR «juez») ·
3.1.2 titre "Did you see yourself when you drank" (l'es suit le FR «Día siguiente difícil»).

---

## 6. Observations à trancher en relecture native (pas des défauts établis)

- **« parent » rendu trois fois différemment** : «padre» (père — 4.1.2 ctxt, 4.1.12 pioch,
  dé-neutralisé), «progenitor» (7.1.1, 7.1.5, 7.2.1, 7.3.2), «padres» (7.2.1 sugg). Les deux
  premières cartes masculinisent un rôle neutre dans les deux sources.
- **« électeur » rendu deux fois** : «votante» (5.1.2, 6.2.6) vs «elector» (6.3.4 — mot rare).
- **Registres d'allocution mêlés** entre cartes : tú (3.1.2 sugg, 7.2.2), usted (majorité),
  vosotros (5.2.7 «Confiadnos»). Le FR vouvoie partout.
- **3.1.3** pioch — «Su **pareja sexual**» : explicitation du « partenaire » neutre des deux
  sources — même geste que fa (شریک جنسی) : famille es · fa.
- **1.2.5** titre — «intenta» suit l'EN "tries" (FR « veut ») : micro-héritage EN, laissé en
  observation (le sens joue).
- **4.1.6** enjeu — «está bien» pour « en parfaite santé » : dé-intensifié.
- **3.2.14** enjeu — «esta situación» pour « cette jungle d'intérieur » : dé-spécifié (la
  jungle survit dans le contexte).
- **4.3.3** enjeu — «el revendedor» explicite le sujet (« il ») et « quiere comprar » remplace
  « lui vendre » : reformulation, à distinguer du défaut zh (rôle ajouté d'une tierce partie).
- **7.1.6** ctxt — ajout «pero sobre todo» (« mais surtout ») : editorialisation légère, même
  geste que fa (اما مهم‌تر از آن) — es · fa.
- **6.1.1** — la suggestion suit l'EN (perte de « comme en droit commun ») mais l'**enjeu
  récupère** «como en el derecho común» : le contenu survit par migration de champ.
- **7.2.9** sugg — «¿Por qué no avisaron…?» (impersonnel « on ») pour « vous n'avez prévenu ».
- **Style des enjeux** — infinitifs nus télégraphiques («Intentar disuadirlo», «Convencerlo
  de…») sur une partie des cartes, phrases complètes («El embaucador debe…») sur d'autres :
  mixage, sans perte de contenu mesurée.

---

## 7. Ce que la lecture n'établit pas

- **La chaîne de production** (es traduit depuis l'EN ou le FR) : 9 écarts suivent l'EN,
  0 l'inverse — rapporté, non vérifié.
- **Que ru · pt · en partageront le lot** : la matrice attend leurs passes (3.3.2 y est déjà
  signalé pour ru par ai-01).
- **La fluidité et le registre globaux** : jugement natif. Ce dossier ouvre la matière.
- **Le statut des micro-héritages** (1.2.5, 6.3.1 «del país» suivant EN "national") : laissés
  en observation, ils n'altèrent pas la mécanique des cartes.

---

## 8. Fidélité élevée — à consigner aussi

- **5.1.2** — le jeu de mots est **recréé avec le lexique officiel espagnol des Schtroumpfs** :
  bara «El **Pitufísimo**» (superlatif -ísimo sur Pitufo, pour *shtroumphissime*), suggestion
  «no estaría **pitufamente** de acuerdo» (pour « ne serait schtroumpfement pas d'accord »),
  contexte «Papá Pitufo», enjeu «Pitufo **Fortachón**» — le nom officiel espagnol du Schtroumpf
  costaud. **Quatrième langue à recréer le jeu (zh · ar · fa · es) — l'EN, lui, le perd**
  ("the smurf").
- **7.1.7** — « minou-minou » recréé : «Para el **minino-minino**...».
- **7.3.4** — « Buuut ! » rendu «**¡Gooool!**» — le cri natif du commentateur de football.
- **7.1.1** — « petit job alimentaire » : «un trabajito para ir tirando».
- **6.2.2** — le cassoulet rendu **fabada** — le plat-frère espagnol, ancrage natif.
- **4.3.5** — « que dalle » : «un cuerno» (familier natif).
- **4.1.13** — « pleuvoir des cordes » : «llueve a cántaros» (idiome natif).
- **3.2.3** — « Le ménage à trois » : «**Tres son multitud**» — l'idiome espagnol équivalent.
- **3.1.8** — « Tous les goûts... » : «Sobre gustos...» — troncation du proverbe espagnol.
- **7.2.12** — « trompe-l'œil » : «trampantojo» — le terme espagnol.
- **4.3.2** — la « carte Vitale » (franco-française) rendue **générique** «tarjeta sanitaria»
  là où l'EN reste piégé ("Vital card").
- **4.3.1** — es suit le FR («estoy **viendo visiones**») là où l'EN dit "dizzy" et où fa
  suivait l'EN : réparation.
- **7.2.8** — es suit le FR («vas un poco demasiado rápido») : **3e langue à réparer** le
  contresens EN (*"going to work a bit"*).
- **6.3.2** — es ajoute le point final que l'EN omet.
- **5.2.1** — Ezéchiel en registre biblique («descargaré el brazo de una terrible cólera…»).
- **2.1.3** — Shéhérazade : «1000 historias durante 1001 noches» — ancrage natif.

---

## 9. Verdict es

- **Couverture : complète** (1169/1169, 0 vide), aucune contamination FR.
- **7 défauts établis** (1.1.3, 3.3.2, 3.2.8, 4.2.8, 4.3.4, 7.3.5, 7.2.3), nommés avec
  preuve ; trois d'entre eux sont prouvables sans source externe.
- **9 écarts imputés au bloc EN** (les 5 connus confirmés en 4e langue + 4 nouveaux découverts
  par cette passe et vérifiés rétroactivement sur ar/fa), retirés du passif es ; **0 cas
  inverse sur quatre langues**.
- Le noyau **zh·ar·fa devient zh·ar·fa·es (6/6)** ; le faisceau ar·fa se scinde 5+1 traversent /
  2 restent — le lot n'est pas lié à l'écriture. **6.2.1 confirmé zh-seul par un 3e contrôle.**
- **0 écriture** : la relecture native décidera des corrections.

*po-2024*
