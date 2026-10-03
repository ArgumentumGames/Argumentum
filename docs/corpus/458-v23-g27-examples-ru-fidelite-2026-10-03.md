# Pool v23 — série ㉖ : fidélité des **exemples** russes du deck (mesure 0-écriture)

**Base** : `origin/master` `ad5421fd`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), désormais `--field example` (extension additive de ce grain, non-régression desc prouvée ci-dessous). **Nature** : mesure, **aucune écriture** — le corpus n'est pas touché (`git status --porcelain Cards/` vide après la passe).

Premier grain de la série des exemples (dispatch c.5966980587, item 4), même méthode que la série définitions ⑳-㉕. Le champ `example_*` n'avait jamais été mesuré. Verdict court : **la colonne russe des exemples est la plus saine mesurée à ce jour** — aucun contresens trouvé en lisant les 175, trois coquilles probables, et la cellule 989 (le dossier zh ㉑ avait arbitré la définition) est **saine** ici aussi.

## Synthèse — 0 A / 0 M / 15 rangées observées / 154 ✓ (168 non vides)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** (aucune candidate) |
| **M — contenu manquant ou ajouté** | **0** (3 candidates consignées avec frontière : les gloses d'intraduisibilité) |
| **C-note — observation consignée, aucune écriture proposée** | **15** |
| **✓ — fidèle** | **154** (sur 168 non vides ; 7 vides structurels, voir ci-dessous) |
| Vides structurels (toutes langues) | 7 |

**Les 175 rangées ont été lues, aucun échantillonnage.**

⚠️ Même faiblesse structurelle que le ⑳ : lecture **unique** par un **lecteur non natif** ; l'archive `example_ru` existe (colonne présente dans v3/v3pp) mais est **elle-même dégradée** — coquilles mesurées dans l'archive : « значиь » (PK 759), « являетс » (809), « во его время » (511), un « (?) » laissé dans la cellule (729), une phrase squelette « Мы должны к чему-то » (787). L'étalon imprimé est bon pour **comparer**, pas pour **noter**.

## Extension d'instrument — `--field example` (réparation non, capacité oui)

L'instrument gagne un paramètre `--field desc|example` qui paramètre la famille de colonnes (jointure, écrans, auto-contrôles). **Non-régression mesurée** : re-passes desc complètes — ru 26 / zh 48 / pt 14 / es 10 / en 24 drapeaux, identiques aux comptes publiés ⑳㉑㉔㉕ ; jointure 168/175 (157 nom + 11 position seule) inchangée. ⚠️ **Chevauchement déclaré avec #1722** (marqueurs ar/fa, même fichier, sections disjointes) : la PR qui merge en second rebasera.

Le contrôle (d') a été **réparé pendant le grain, avec sa raison écrite** : il exigeait l'IDENTITÉ `desc_ru` de PK 55 — propriété propre à desc, pas à la jointure. L'invariant réel (le pont rattache PK 55 à SA carte « Sauvetage ad hoc ») est désormais asserté pour les deux champs ; l'identité de contenu reste exigée pour desc, affichée pour example (PK 55 : DIFFERE, 140 car. — une mesure, pas une panne).

## La structure du grain : 51 survivants, 98 réécritures, 7 vides structurels

| Population | Rangées |
|---|---:|
| IDENTIQUE (deck RU = archive, octet près) | **51** |
| DIFFERE réel | 98 |
| Archive cellule vide | 8 |
| Position seule / aucune référence | 11 + 7 |
| **Deck vide structurel** (PK 1, 175, 594, 696, 798, 887, 1280) | **7** |

Les 7 vides sont **structurels, pas un manque ru** : `example_fr` ET `example_en` sont vides sur ces 7 rangées — ce sont les têtes de famille (path racine 1 à 7 : Insuffisance, Influence, Erreur mathématique, Erreur de raisonnement, Abus de langage, Tricherie, Obstruction), même topologie que les vides justifiés du dossier link #1435. ⚠️ Observation : l'archive v3 porte **un exemple pour PK 1** (« Судя по количеству посетителей, этот ресторан… ») que le deck n'a dans **aucune** langue — trace d'un exemple d'époque retiré au moment de la restructuration, pas un défaut.

Le profil est celui des définitions pt/es : le deck suit le **français courant** (réécritures fraîches : 176 « flatterie directe » vs archive Krylov ; 247 « vallée fleurie » vs archive de Gaulle ; 632 climat/natalité vs archive Nicolas Cage), avec 51 survivants d'époque verbatim.

## Coquilles probables — 3 (confirmation par un locuteur, aucune écriture proposée)

- **PK 361** : « удо**о**влетворять » — double о, le mot n'existe pas (standard : « удовлетворять »).
- **PK 1352** : « **васкулярная** хирургия » — anglicisme médical ; le russe standard dit « сосудистая хирургия », et c'est ce que **l'archive porte** — la dérive est propre au deck.
- **PK 848** : « Члены комитета которые утвердили… seront вызвés » — virgules normatives manquantes autour du relatif « которые » (et la carte joue précisément sur l'ambiguïté de l'enchâssement) ; moindre certitude, à confirmer.

## Les 3 candidates M — les gloses d'intraduisibilité du jeu de mots « avocat »

Le FR joue sur « avocat » (profession/fruit) trois fois ; le russe ne peut pas, et **ajoute une glose explicative** :

| PK | Le deck RU fait |
|---|---|
| 796 | exemple complet + « — Французское слово «avocat» меняет значение… » (`LONG 2,66`, `CLAUSES 6>3` — la glose explique le drapeau) |
| 847 | « Я предпочитаю курицу оливкам **— или курицу с оливками** » (`LONG 1,66`) |
| 855 | « Этот медведь съел «авока» **— по-французски это слово означает и авокадо, и адвоката** » (`LONG 3,07`) |

Frontière M : la glose restitue un jeu de mots autrement perdu (choix de traduction défendable) ; si l'arbitre juge qu'elle alourdit la carte imprimée au point de la changer → M. ⛔ Consignées avec frontière, arbitrage ai-01 — même forme que PK 989 au ㉑.

## C-notes restantes — 9, sens tenu partout

- **PK 974** : le deck RU porte le **dialogue complet** (3 répliques) que l'archive porte aussi — le **FR courant l'a abrégé à une réplique**. Héritage d'époque, pas une faute ru.
- Décalages lexicaux mineurs : PK 625 « tigres royaux » → « бенгальские » (Bengale) · 653 « je suis en réussite » → « мне везёт » (j'ai de la chance) · 844 « Jeanne » → « Джейн » (Jane) · 1330 « billes » → « игрушки » (jouets) et « goûter » → « обед » (déjeuner) · 1301 compression 4 phrases → 3 (« Ils sont excellents / vraiment bons » fusionnées, sens tenu).
- **PK 989 — la famille arbitrée au ㉑ est SAINE ici** : « Вам стоит доказать, что он вреден ! » restitue exactement le renversement de charge (le contresens n'existe que dans `desc_zh`). Cinquième vérification directe de cette cellule à travers la série.
- **Archive non-étalon** (coquilles d'archive listées en tête) — conséquence : les 98 DIFFERE ne se lisent pas comme dérive du deck mais, pour l'essentiel, comme **remplacement** du texte d'époque (profil ㉔-pt).

## Écrans — 23 drapeaux priorité, tous lus, aucun défaut réel derrière

18 `POLARITY` : parité de négation respectée dans chaque cas (« ne devrait pas être sanctionné »/« нельзя штрафовать » ; « ni télévision ni voiture »/« ни телевизора, ни машины »…) — faux positifs assumés de l'écran. 3 `LONG` : les gloses 796/847/855. 2 `CLAUSES` : 784 (le FR joint par « ; », le RU par des virgules — comptage de ponctuation, FP) et 796 (la glose). 7 `EMPTY-target` : les vides structurels. **Aucune inversion de polarité réelle détectée par lecture** — et le témoin inversé planté au contrôle inverse n'est pas vu par les écrans (constaté à chaque grain de la série : la lecture reste la charge utile).

## Contrôle inverse

Copie du corpus en scratchpad, **jamais le dépôt** — vérifié après la passe : `git status --porcelain Cards/` **vide**.

1. Self-test `--field example` : (a) troncature → `SHORT` ✓ ; (b) polarité inversée → `POLARITY` ✓ ; (c) écran CJK inchangé (contrôle desc) ; (d) jointure triplet 1.1.1-1.1.3 confirmée par le nom ✓ ; (d') **réparé ce grain** — l'invariant de jointure (PK 55 → « Sauvetage ad hoc ») est asserté pour les deux champs, l'identité de contenu n'est plus exigée à tort pour example.
2. Non-régression desc : comptes identiques aux publiés (26/48/14/10/24).

## N'établit pas

- **Que la colonne soit sans défaut.** Lecture **unique**, non native. L'établi : *aucun écart trouvé en lisant les 175* hors les 15 consignées.
- **Que les 3 gloses soient des M** — frontière écrite, arbitrage ai-01.
- **Que les 3 coquilles en soient** — un locuteur natif confirme ou infirme (celle de 848 est la moins sûre).
- **Rien sur** `example_en_bis` (colonne EN variante présente deck ET archives, non instruite), les langues suivantes de la série (zh ㉗, ar, fa, pt+es, en), les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.** Les 15 observations attendent l'arbitrage d'ai-01.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test            # desc
python docs/corpus/definitions-fidelity-instrument.py --field example --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang ru --out <sortie>
```

- Dénominateur **auto-vérifié** (arrêt bruyant si ≠ 175) ; corpus du dépôt **jamais écrit** (contrôle inverse sur copie).
- Comptes de jointure inchangés par construction (168/175, 157 nom / 11 position seule / 7 aucune — la jointure ne dépend pas du champ).
