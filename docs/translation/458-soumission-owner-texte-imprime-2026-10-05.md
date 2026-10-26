# #458 — Soumission à l'owner : le texte **imprimé** que la campagne de fidélité propose de changer

**Réponse de l'owner — 05/10, interactif (VÉRIFIÉ) : « Oui à tout ».** Rules_13 passe de 32 à **28 cartes dans les 8 langues** ; les **11 cellules EN imprimées** des Scénarios sont corrigées. Relayée par ai-01 (#458 c.5990803984, c.5990804554). Exécution portée par po-2024 (grains 3 et 4) — les cellules mergées avant le lancement de la régénération y entrent, le run ne les attend pas.

**Statut** : **0 écriture** — ce dossier propose et enregistre, il ne corrige pas. **Révision du 05/10** (ai-01, c.5988397998, vérifiée par po-2023 sur l'archive et le corpus) : le volet Scénarios était exact ; **six entrées du volet initial sortent du dossier** — deux reposaient sur une citation fausse ou incomplète, quatre ne relevaient pas de l'owner. Elles vivent en **Annexe B** avec leur raison. La numérotation initiale est conservée pour que les renvois restent valides.

**Base** : master `6a071b98`. Sources mesurées : `458-fidelite-regles-sources-fr-en-2026-10-04.md`, le classement contre `Cards/Rules/Archive/2022/` (CSV + 3 JSON), et `458-fidelite-scenarii-en-axe-imprime-2026-10-04.md` (77 cartes Scénarios imprimées, jointure par titre et contenu — jamais par `path`).

---

## Pourquoi ce dossier existe

La campagne de fidélité relit le corpus contre ses sources. La plupart des corrections touchent des textes **jamais imprimés** : elles sont libres. Mais l'édition de **février 2022** a imprimé du texte **français et anglais** — et sur ce qui a été imprimé, corriger n'est plus de la fidélité : c'est **décider qu'une réimpression dira autre chose que la première édition**. Ces cas-là appartiennent à l'owner.

Le tri initial, revu le 05/10 : sur les 40 cellules EN du deck Scénarios portant un défaut établi, **19 seulement concernent du texte imprimé** — les 21 autres n'ont jamais vu d'encre. Côté Règles, la vérification d'ai-01 contre l'archive a retiré la moitié des candidats (Annexe B). Ne restent à l'owner, tranchés par sa réponse : **une décision éditoriale Règles** (le nombre 32/28, présent dans les 8 langues) et **11 cellules Scénarios EN**. Toutes approuvées.

---

## Volet 1 — Règles (livret imprimé 2022, FR + EN)

*(Les entrées 1.1, 1.2 et 1.4 du dossier initial sortent du volet owner — Annexe B.)*

### 1.3 « 32 cartes annoncées, 28 demandées » — **approuvé par l'owner : harmoniser à 28**

**Corpus actuel (FR)** : Matériel — « Une sélection de **32** cartes » · Installation — « Sélectionnez **28** cartes d'arguments fallacieux pour la partie, **soit 4 par couleur**. »

**Ni l'un ni l'autre n'est imprimé** : le passage entier est postérieur à l'édition. C'est une contradiction interne du corpus actuel : 7 classes × 4 = 28, pas 32. Et elle n'est pas franco-anglaise : **la contradiction est présente dans les 8 langues, une cellule chacune** (mesuré par ai-01) — chaque langue a recopié fidèlement les deux chiffres.

- **Nature** : décision éditoriale pure (aucune référence imprimée ne tranche) — **tranchée le 05/10 : 28**.
- **Coût acté** : 8 cellules (une par langue). Portée par le grain 4 de po-2024.

---

## Volet 2 — Scénarios, texte EN imprimé (édition 2022, 77 cartes)

Onze cellules **imprimées et inchangées depuis** portent un défaut établi par lecture, confirmé contre l'imprimé par ai-01 (« les 14 textes cités figurent tels quels dans l'imprimé et dans le CSV courant »). **Les corriger change des cartes déjà imprimées — approuvé le 05/10.** *(Les entrées 2.8, 2.9 et 2.12 sortent — Annexe B ; la numérotation est conservée.)*

### Noms propres contre rôles

**2.1 — « President Truman and the A-Bomb », baratineur — approuvé : rendre le rôle**
Imprimé EN : « **Truman** » · FR : « Le **président des États-Unis** ». Le titre de la carte nomme déjà Truman ; le FR garde le rôle dans le baratineur, l'EN y substitue le nom.

**2.2 — « Salomon », baratineur — approuvé : rendre le rôle**
Imprimé EN : « **Salomon** » · FR : « Un **juge des affaires familiales** ». Même geste — et le nom est déjà le titre de la carte : le baratineur EN ne dit plus rien de la fonction.

### Contradictions internes à la carte même

**2.3 — « Don Juan », piocheur — approuvé : aligner sur la carte**
Imprimé EN : « **the statue of the governor** » · FR : « Le spectre ». Le contexte et l'enjeu de **la même carte** disent `the specter` — et la carte écrit `specter` (US) au contexte mais `spectre` (GB) à l'enjeu : deux orthographes en plus de la contradiction.

**2.4 — « The last cigarette », piocheur — approuvé : `A parent`**
Imprimé EN : « **A relative** » · FR : « Un **parent** ». L'enjeu de la même carte dit `the parents` ; `parent` existe en EN avec le même sens qu'en FR — rien ne justifie la substitution.

**2.5 — « La kermesse », titre — approuvé : traduire (`The Fair`)**
Imprimé EN : « **Kermesse** » — mot français laissé tel quel au titre, alors que le contexte et l'enjeu de la même carte traduisent correctement `fair`. Le titre et le corps d'une même carte ne sont pas dans la même langue.

### Personne grammaticale

**2.6 — « Online dating », piocheur — approuvé : revenir au nominal**
Imprimé EN : « A person **I** met online » · FR : « Une personne rencontrée en ligne ». Le « je » n'existe dans aucune source ; le piocheur décrit un tiers.

### Le cas Schtroumpf

**2.7 — « Le coup d'État », baratineur — approuvé : forger le superlatif**
Imprimé EN : « **the smurf** » · FR : « Le **shtroumphissime** ». C'est le défaut-signal de la campagne : les six autres langues **forgent** le superlatif (es « El Pitufísimo », ar, ru, zh, fa) ; l'EN seul l'aplatit. Former l'équivalent EN change le texte imprimé le plus visible de la carte.

### Idiomes

**2.10 — « Rouler des mécaniques », titre — approuvé : idiomatiser**
Imprimé EN : « **Rolling mechanics** » — l'idiome FR (frimer) traduit mot à mot : le titre EN ne signifie rien. Un équivalent idiomatique (« Showing Off », « Flexing ») s'impose.

**2.11 — « Ergo sum », suggestion — approuvé : rendre l'illusion**
Imprimé EN : « My God, I'm so **dizzy**! » · FR : « voilà que j'ai **la berlue** ! ». La berlue = *voir ce qui n'existe pas* ; `dizzy` = *avoir la tête qui tourne*. L'ES, RU et PT réparent déjà par leur propre idiome (« viendo visiones », « alucinações »).

### Registre, nom divin

**2.13 — « L'escalier », suggestion — approuvé : neutraliser le registre**
Imprimé EN : « **What's up?** » · FR : « **Alors ?** » — registre familier américain là où le FR est neutre et la situation tendue.

**2.14 — « Une fiction pulp », suggestion — approuvé : rendre le nom divin**
Imprimé EN : « my name is **eternal** » · FR : « mon nom est **l'Éternel** ». Le nom divin (avec capitale, en apposition) devient un adjectif — le sens change de registre.

## Écrit (07/10 — file profonde c.5993735448, grain 2 « 3a »)

Les 11 cellules approuvées sont **écrites**, formes choisies dans la PR, une raison par cellule — plus le titre « coup d'État » (Annexe B 2.8, même rangée 5.1.2 que le superlatif) et les 3 siblings ru que la règle du GO couvre (*même défaut, même rangée, même PR*). Garde : `ScenariiEnPrintedCorrectionsGuardTests` — 15 épingles pleine-cellule, écran d'éradication corpus, mutation M1 mesurée (le revert de l'accent rougit nommément `5.1.2.title` **et** l'écran d'éradication — c'est son cas le plus fin : l'accent était le seul delta).

| # | Carte | Cellule | Avant | Écrit | Raison de la forme |
|---|---|---|---|---|---|
| 2.1 | Truman et la bombe A | 1.3.2.smoothTalker | Truman | **The President of the United States** | le FR porte le rôle (« Le président des États-Unis ») ; le titre nomme déjà Truman |
| 2.2 | Salomon | 2.2.5.smoothTalker | Salomon | **A family court judge** | le FR (« Un juge des affaires familiales ») ; le titre garde le nom |
| 2.3 | Don Juan | 2.3.5.drawer | the statue of the governor | **the specter** | le contexte et l'enjeu de la carte disent `the specter` (US) |
| 2.4 | The last cigarette | 7.3.2.drawer | A relative | **A parent** | le FR dit « Un parent », l'enjeu dit `the parents` |
| 2.5 | La kermesse | 7.1.5.title | Kermesse | **The Fair** | le corps de la carte traduit déjà `fair` |
| 2.6 | Online dating | 3.1.6.drawer | A person I met online | **A person met online** | nominal, comme le FR « Une personne rencontrée en ligne » |
| 2.7 | Le coup d'État | 5.1.2.smoothTalker | the smurf | **the Smurfiest** | le superlatif forgé, comme le FR « shtroumphissime » |
| 2.8 | Le coup d'État | 5.1.2.title | The coup d'etat | **The coup d'État** | typographie (Annexe B, même rangée que 2.7) |
| 2.10 | Rouler des mécaniques | 4.1.1.title | Rolling mechanics | **Showing off** | l'idiome, pas le calque mot à mot |
| 2.11 | Ergo sum | 4.3.1.suggestion_en | My God, I'm so dizzy! | **My God, I'm seeing things!** | la berlue = *voir ce qui n'existe pas* |
| 2.13 | L'escalier | 7.1.2.suggestion_en | What's up? Why did you push him? | **So? Why did you push him?** | registre neutre tendu, comme le FR « Alors ? » |
| 2.14 | Une fiction pulp | 5.2.1.suggestion_en | my name is eternal | **my name is the Lord** | la formule biblique anglaise rend le nom divin comme **nom** — « the Eternal » serait un gallicisme |

**Siblings ru** (règle du GO). ⚠️ Le dossier §2.7 disait « les six autres langues **forgent** le superlatif » : **mesuré faux pour le ru**, qui portait «Смурфик-бунтарь» (*schtroumpf rebelle* — un autre personnage). Le fait mesuré prime : la cellule ru est corrigée dans la même PR.

| Carte | Cellule | Avant | Écrit |
|---|---|---|---|
| La kermesse | 7.1.5.title_ru | Праздник | **Школьный праздник** |
| Le coup d'État | 5.1.2.smoothTalker_ru | Смурфик-бунтарь | **Смурфейший** |
| Une fiction pulp | 5.2.1.suggestion_ru | Имя мое вечно | **Имя мое — Вечный** |

> **Erratum 07/10 (relecture ai-01, c.6027007896)** : le superlatif ru a d’abord été écrit «Смурфиейший» ; la règle russe («основа + -ейш-») colle le suffixe au thème смурф- sans voyelle de liaison — corrigé en **Смурфейший** dans une micro-PR suivant le merge #1788. Aucune des deux formes n’est attestée (néologisme) ; la morphologie décide.

**Siblings non corrigées, épinglées** : pt « A quermesse » (7.1.5) = mot portugais assimilé, pas du français oublié ; fa/zh/ar de 5.2.1 ne peuvent **structurellement** pas porter le défaut du nom divin (ni article ni copule nominale équivalents). Gardées en l'état, la garde les protège d'un balayage futur.

Chirurgie par **span de champ** — l'ancienne valeur de 2.2.5.smoothTalker (« Salomon ») existe aussi dans le titre de la même rangée : un remplacement par sous-chaîne était **impossible**. 15 cellules exactement (re-parse complet), delta **+36 octets**, 167 enregistrements, pas de BOM.

---

## Annexe A — Ce qui a **déjà** changé sur du texte imprimé (à savoir, pas à arbitrer)

- **4 cellules Scénarios EN** converties de la 2ᵉ à la 3ᵉ personne (« You're a car dealer » → « The smooth talker is a car dealer », etc.) — standardisation cohérente, mesurée sur tout le deck imprimé.
- **1 cellule** fidèle à un FR qui a gagné « sans consentement » après impression — mise à jour légitime.
- **1 cellule** où le FR s'est ponctué seul après impression et l'EN devra suivre (virgule et point) — correction libre, la cible est donnée par le FR courant.

## Annexe B — Les six entrées retirées du dossier, décidées par ai-01 (c.5988397998, vérifié par po-2023)

**1.1 — Pronoms genrés « his/he » de la fin de partie → correction libre (grain 7 de po-2024).** Le dossier initial présentait la phrase comme « texte imprimé » : **elle ne l'a jamais été** — sondes `his cards` / `he wins the game` / `within 10 seconds` = 0 dans le CSV et le JSON anglais de l'archive, comme dans le FR (`débarrass`, `toutes ses cartes`, `10 secondes` = 0) ; témoin positif `wins the game` trouvé ailleurs. L'imprimé est genré **ailleurs** (`his` 9, `he` 9, `they` 2), et la phrase est la **seule cellule genrée du corpus courant** — terminer la migration vers `they` (déjà tranchée 32 fois dans le corpus) est libre.

**1.2 — L'ajout « tied » → gardé, 0 cellule.** Passage postérieur à l'impression. Le FR dit « tous déclarés vainqueurs », qui suppose plusieurs vainqueurs à égalité : `tied` est **fidèle**. (Ce n'est pas une question de typographie.)

**1.4 — « L'enregistrement » non listé au Matériel → pas un défaut, 0 cellule.** Le Matériel de la variante annonce déjà « 1 débat ou 1 discours auquel les joueurs vont assister ensemble **et qui pourra faire l'objet d'un revisionnage** » — l'enregistrement de la carte suivante **est** ce revisionnage. Le dossier initial citait cette puce et en tirait la conclusion inverse : erreur de lecture, corrigée.

**2.8 — Accent de « coup d'État » (titre EN) → typographie, correction libre** (grain 6 de po-2024). **2.12 — « The inheritence » → orthographe, correction libre** (grain 6). Typographie et coquilles se corrigent toujours, sans soumission.

**2.9 — « Papa Smurf » (suggestion) → gardé.** La recommandation du dossier initial était de conserver l'imprimé — c'est-à-dire l'état actuel : aucune question.

## Annexe C — Ce qui **ne** sera **pas** soumis (pour ne pas alerter à tort)

- Les **4 restaurations libres** des Règles EN (`deck`, `color classes`, `randomly drawn`, cosmétique) : l'imprimé fournit lui-même la forme correcte — retour à l'imprimé, aucun arbitrage.
- Les **15 cellules EN jamais imprimées** (cartes sans texte EN à l'édition ou hors édition) : correction libre. **Écrites le 07/10** (file profonde c.5993735448, grain 3b) — avec les 23 siblings que la règle du GO couvre, soit 38 cellules sur 14 rangées ; garde `ScenariiNeverPrintedCorrectionsGuardTests` ; détail au §12 du dossier d'axe imprimé.
- Les singularités FR **consignées sans action** (registre « vous »/impératif, point après « Cartes mémo »).

## Coût global — acté par la réponse owner

- **Règles** : 8 cellules (28 cartes, une par langue). **Scénarios** : 11 cellules EN.
- Régénération : ces cellules sont **dans le périmètre du run post-verdict** — mergées avant le lancement, elles y entrent ; aucun run supplémentaire.
- Publication : porte séparée, après verdict visuel d'ai-01. Gel `v2.0.0-review` jusqu'au verdict des associés.

---

*Dossier établi par po-2023 le 05/10/2026 (grain 1 du pool #458 c.5985353780), révisé le 05/10 sur review ai-01 c.5988397998 et réponse owner c.5990803984. Aucune cellule modifiée par ce dossier.*
