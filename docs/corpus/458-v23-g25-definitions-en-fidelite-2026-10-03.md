# Pool v23 — série ㉕ : fidélité des **définitions** anglaises du deck (mesure 0-écriture)

**Base** : branche `docs/458-g24-definitions-pt-es-fidelite` (série ㉔, chaîne #1713 → #1715 → #1717 → #1718 → #1719 → ce grain). **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py) — rejouable, `--self-test` (6 contrôles). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Sixième et dernier grain de la série ⑳-㉕. La colonne `en` est celle qui a servi de **référence imprimée** à toute la série — la voici mesurée pour elle-même, et elle renverse la perspective : **62 cartes portent l'archive EN verbatim (+4 à la typographie près)**, la colonne a été réécrite **à moitié**. Là où les grains précédents observaient des « jumeaux » entre langues, ce grain en localise **l'origine** : la plupart sont la trace du deck EN d'époque, désynchronisé du français courant sur la carte livrée elle-même.

## Synthèse — 0 A / 0 M / 36 rangées observées / 139 ✓ (175)

> **Erratum d'arbitrage (03/10, ai-01, [c.5966971153](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5966971153)).** Les frontières de ce dossier sont tranchées : **PK 989 et PK 154 sont gardées — C-note définitives** (989 : la forme « présomption faute de preuve contraire » n'est ni inversée ni contradictoire — contrairement au chinois corrigé par #1714 ; 154 : « is false » est la définition classique de l'*argument from fallacy*). **PK 1015 gardée** : la tournure est maladroite mais compréhensible — la corriger serait une réécriture d'une carte imprimée, pas une coquille (formulation de polissage possible consignée par ai-01 si l'owner veut un jour polir l'anglais imprimé). **Les 3 coquilles héritées (PK 622 « Atrributing », 625 « individual part. », 1362 point final manquant) sont confirmées et corrigées** par la PR d'arbitrage. Règle actée : une langue imprimée (en) garde son texte sauf contresens ; les coquilles se corrigent toujours. Aucun compte ne change.

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** (2 candidates consignées avec frontière) |
| **M — contenu manquant ou ajouté** | **0** |
| **C-note — observation consignée, aucune écriture proposée** | **36** (4 catégories) |
| **✓ — fidèle** | **139** |

**Les 175 rangées ont été lues, aucun échantillonnage.**

⚠️ Faiblesses structurelles, dites avant le verdict : lecture **unique**, par un **lecteur non natif** — pour l'anglais, langue de travail de l'auteur de l'instrument, la competence est meilleure que pour zh/ar/fa mais pas native. La référence d'archive est ici **la colonne elle-même** (`desc_en` dans v3, 168/175 rattachées) : l'arbitrage « imprimé » est mécanique et de bonne qualité (l'archive EN est propre là où l'archive pt du ㉔ était fautive — elle porte ses débris MT, mais rares et nommés plus bas).

## La structure du grain : 62 + 4 survivants, 91 réécritures (mesuré)

Extraction mécanique (parsing strict par bloc, normalisation espaces/apostrophes/guillemets pour le second groupe) :

| Population | Rangées |
|---|---:|
| **IDENTIQUE** (deck EN = archive EN, octet près) | **62** |
| **DIFFERE typographique seul** (PK 733, 735, 833, 1388 — guillemets courbes/droits, double espace) | 4 |
| **DIFFERE réel** (réécritures) | 91 |
| Sans référence exploitable (11 position seule + 7 aucune) | 18 |

T2 : corr(deck EN ~ FR courant) = **0,716**, corr(deck EN ~ archive EN) = **0,560** — la colonne est **à mi-chemin** entre son archive et le français courant (contre pt : 0,838/0,320, réécriture totale). La moitié réécrite suit le FR courant ; l'autre moitié vit encore en avril 2024.

## Les deux candidates sérieuses — la désynchronisation FR/EN est SUR la carte livrée

⛔ **Motif du zh PK 989** : frontière écrite, arbitrage ai-01.

- **PK 989 « Renverser la charge de la preuve »** — deck EN (IDENTIQUE de l'archive) : *« Assuming a proposition to be true until proved false or vice-versa »*. Cette phrase définit la **présomption**, pas le renversement de charge ; la colonne **FR de la même carte** dit correctement « c'est à la partie adverse de démontrer que votre position est fausse ». Le grain ㉔ avait trouvé la cellule es verbatim de l'archive EN — **la source du défaut est la colonne EN du deck elle-même, encore en place**. Frontière : si un locuteur juge que définir un autre sophisme rend la carte fausse → **A** (et la cellule es suit, même arbitrage).
- **PK 154 « Sophisme du sophisme »** — deck EN (IDENTIQUE) : *« Arguing that a conclusion **is false** because its proof involves a fallacy »*. *Rejeter* une conclusion ≠ la *déclarer fausse* — c'est la nuance qui définit ce sophisme même (FR : « Vous **rejetez** »). Le ㉔ avait consigné la même frontière en pt en la supposant héritée : **voilà l'héritage, localisé**. Frontière A pour ai-01.

## C-notes — 36 rangées, 4 catégories, aucune écriture proposée

### 1. Sérieuses, consignées AVEC frontière (2)

PK 989 et PK 154 — voir ci-dessus.

### 2. Survivants d'époque divergents du français courant — le cœur du grain (27)

Ces cartes portent un EN d'avril 2024 que le français courant ne dit plus. **Colonne « jumeaux » : les langues des grains ⑳-㉔ qui portent le même écart — l'origine du jumeau est ici.**

| PK | Titre | Ce que l'EN d'époque dit (et que le FR courant ne dit plus) | Jumeaux |
|---|---|---|---|
| 43 | Pratique courante | « **uncontroversial** statements **before** introducing » — le « glissez **subtilement** » perdu | fa (361 voisin pour la structure) |
| 51 | Sophisme du psychologue | « **subjective experience** of reality with the **essence** » | — (ar/fa déplacent autre chose) |
| 108 | Appel à la nature | « **anything** natural is good » (FR : « une chose ») | pt |
| 175 | Influence | « manipulate **rather than using logical reasoning** » (FR : « persuader au lieu de convaincre ») | zh, ar, fa, pt |
| 182 | Fausse alternative | « **Pretending** to offer an alternative » (FR : « présentez ») | zh, fa |
| 313 | Flatterie | « praise or **ego-boosting implications** » | fa (grandiosité) |
| 603 | Picorage de données | dit le positif (« **pointing to** evidence that supports ») là où le FR dit le négatif (« ignorez les faits qui… ») | — |
| 636 | Sophisme de régression | « the **natural return from an extreme to the average** » — la définition statistique complète (régression vers la moyenne), que le FR a simplifiée en « fluctuation normale » | — |
| 670 | Faux équilibre | « as **more balanced than they actually are** » | zh, fa |
| 673 | Juste milieu | « the **truth** must be found as a compromise » (FR : « la solution ») | es |
| 690 | Opération inappropriée | `SHORT 0,40` — « **in this situation** » perdu | — |
| 696 | Erreur de raisonnement | « **Reaching a conclusion** through flawed reasoning » — la structure que le fa a cassée en traduisant (پایان‌بخشی) | fa |
| 777 | Inconsistance | « **mutually exclusive** assertions » (FR : « se contredisent ») | — |
| 781 | Logique du chaudron | « valid arguments that **unfortunately** contradict each other » — l'adverbe éditorial | ar (متأسفانه « hélas ») |
| 796 | Quaternio terminorum | « **at least four** terms » (FR : « quatre ») | fa (« یا بیشتر ») |
| 798 | Abus de langage | « **misleads or misrepresents the truth** » (FR : « subtilités linguistiques pour convaincre ») | — |
| 800 | Acception vague | « vague enough to open **argumentative leeway** » — compression du mécanisme FR (« modifier votre argumentation en cours de route ») | — |
| 809 | Contraste perdu | « the terms **of the debate** » (FR : « termes distincts ») | fa, pt, es |
| 826 | Définition incohérente | « inconsistent, **logically faulty** ways » | pt |
| 834 | Comparaison abusive | « an **incorrect** comparison », `SHORT 0,60` — l'écran tire sur la SOURCE des compressions fa/pt/es | fa, pt, es |
| 869 | Réification | `SHORT 0,45` — la clause de conséquence (« ce qui fausse votre raisonnement ») perdue | — |
| 887 | Tricherie | `SHORT 0,50` — « règles **tacites** » perdu (« Violating the norms ») | pt |
| 953 | Attention sélective | « **aspects of the argument / relevant dimensions** » (FR : « faits… votre thèse ») | — |
| 1314 | Fausse piste | le but (« **pour échapper aux arguments** de votre contradicteur ») perdu | — |
| 1330 | Noyer le poisson | « **mostly irrelevant** information » (FR : « sans rapport ») | pt |
| 1362 | Tu quoque | « failure to act **consistently** » — le « pas **toujours** » absent | fa, pt, es |
| 1365 | Homme de paille | « a **grotesque picture** » (FR : « caricaturez ») | — |

### 3. Réécritures notables parmi les 91 DIFFERE réels (4)

- **PK 121** (Politiquement correct) — réécrit mais garde « **social or institutional** offense » : l'origine du jumeau ar/fa/pt (le FR courant dit « une partie de l'auditoire »).
- **PK 1011** (Exigence relâchée) — réécrit et **durcit** : « an **indefensible** position » là où le FR dit « difficilement défendable » — l'origine du jumeau ar/fa.
- **PK 1023** (Raisonnement biaisé) — quasi-survivant : la réécriture ne corrige que la faute d'archive (« Argumenting » → « Arguing ») ; « **cognitive** bias » est l'origine du décalage pt.
- **PK 719** (Effet cigogne) — « vous croyez **à tort**… **nécessairement** » : les deux marqueurs perdus (« Asserting a causal relation… on grounds of them being correlated »).

### 4. Typos probables du deck EN — héritées verbatim de l'archive (3)

- **PK 622** « **Atrributing** » (triple r) · **PK 625** « each of its individual **part** » (pluriel manquant) · **PK 1015** « Claiming one's approach is **worth at least for the effort** devoted to it » (construction non native). Plus PK 1362 sans point final (trivial). Toutes IDENTIQUE de l'archive — la colonne les porte depuis 2024.

### Observation (hors décompte) — l'archive EN aussi est marquée MT, mais le deck l'a nettoyée

Débris mesurés dans l'archive v3, tous **corrigés** par la moitié réécrite du deck : « inaccuracies **in in** the use » (594), « unpar his supe » (1398, phrase tronquée en plein mot), « a request a request » (432), « **Your based** your argument » (2), « situtations » (134), « subrepticely » (177), « uncommiting » (994). L'archive EN est donc un étalon **meilleur que l'archive pt** du ㉔ mais pas propre — les survivants de la catégorie 2 héritent de son état brut, typos comprises.

## La boucle de la série est bouclée

Le ㉑ mesurait 17 survivants zh et parlait d'« un original d'époque commun quasi certain » ; le ㉔ en localisait en es un **défectueux** ; ce grain montre que **le vecteur est la colonne EN du deck** : les jumeaux zh/ar/fa/pt/es des grains précédents sont, pour la plupart, des traductions d'un EN qui n'a pas suivi la réécriture du FR. Table de résolution : 989→es, 154→pt, 175→zh/ar/fa/pt, 313→fa, 673→es, 781→ar, 796→fa, 809→fa/pt/es, 826→pt, 834→fa/pt/es, 887→pt, 1011→ar/fa, 1023→pt, 1242→pt/es (« strict, exclusive »), 1291→fa/ar (« superficial assertions »), 1330→pt, 1362→fa/pt/es, 121→ar/fa/pt, 594 (famille « numerical »)→ar/fa/pt/es, 361→fa (structure « before introducing », le بی‌نظیر reste une faute de traduction fa). **Restent sans origine identifiée** : le jumeau quintilingue PK 729 (« cause possible » + glose) — le deck EN dit **correctement** « sufficient condition » — et les décalages propres à chaque langue.

## Contrôle inverse

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

1. **Cellule inversée plantée** (PK 814 : « You reason as if **many options** were open to you, when in fact **only two** possibilities exist »), **longueur calibrée sur le FR** — la leçon du ㉔ (un témoin court fait lever SHORT pour une raison qui n'est pas le sens) : **`FLAGS: -` du premier essai**. Aucun écran ne voit le contresens ; la lecture le voit immédiatement.
2. **Cellule tronquée à 50 %** (PK 34) : `SHORT(0,42)` ✓ — plus un `POLARITY` **incidentel attendu** (la troncature coupe aussi le « not » de la cellule) : c'est le comportement d'une troncature, pas une découverte.

## N'établit pas

- **Que la colonne EN soit sans défaut.** Lecture **unique**, non native. L'établi : *aucun écart trouvé en lisant les 175* hors les 36 consignées.
- **Que les deux frontières (PK 989, 154) soient des A** — l'arbitrage appartient à ai-01 ; la cellule es 989 du ㉔ suit le même arbitrage que sa source.
- **Que les typos probables (PK 622, 625, 1015) soient les seules** — un relecteur natif peut en trouver d'autres.
- **Que les écrans corroborent quoi que ce soit** : 24 drapeaux (14 `POLARITY`, 10 `SHORT`) sur le corpus réel, tous lus — 0 défaut réel derrière les `POLARITY` (parité respectée : « not/no/without » vs « ne/sans ») ; les `SHORT` sont les compressions réelles des catégories 2 (690, 622, 735, 834, 869, 887, 888, 977, 989…).
- **Rien sur** les 18 cartes sans référence exploitable (dont PK 361, 658, 847, 855, 1242 — position seule ou aucune), les champs `example_*`/`link_*`, ni les **1233 rangées hors deck**.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.** Les 36 observations attendent l'arbitrage d'ai-01 — et la série ⑳-㉕ est **complète** : 6 grains, 6 dossiers, ~1 050 rangées lues.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --lang en --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175) ; corpus du dépôt **jamais écrit** (contrôle inverse sur copie).
- **Aucune réparation d'instrument ce grain** — comptes des langues précédentes inchangés par construction (aucun diff sur l'instrument depuis le ㉓).
- Comptes de jointure inchangés (157 nom / 11 position seule / 7 aucune).
- ⚠️ Le comptage « 62 IDENTIQUE / 4 typo » est **reproductible par parsing strict par bloc** (fichier `grain25-survivors.py` du scratchpad) — une regex multiligne glisse sur les blocs sans référence et attribue des lignes IMPRIME à la carte précédente (mesuré : 5 appariements faux sur la première version).
