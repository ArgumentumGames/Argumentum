# #1187 — Dossier de fabrication : volumes mesurés, géométrie, matrice d'appel d'offres

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-14 · **Base** : `origin/master` `769a373d`
**Nature** : mesure, lecture seule. Aucune régénération, aucune publication, aucun contact façonnier,
aucun devis demandé, aucun verdict fabrication.
**Portée** : grain ⑤ de la deep-queue po-2023 (`#458` c.`5657210653`). Complète le corps de #1187 et les
matrices déjà postées en commentaires (`5448477810`, `5464177683`, `5552589768`).

---

## §0 — Ce que ce dossier est, et ce qu'il n'est pas

Le corps de #1187 porte la décision owner (une boîte par langue, contenu = deck Tarot + deck Scenarii,
Virtues conditionnel) et une matrice postée le 28/08. Depuis, **quatre décisions ont déplacé les nombres** :
PK 96 sorti du deck (05/09), format tranché à 70 × 120 mm (#1250), et le défaut `CCby` soldé (#1227).

Ce dossier **re-dérive chaque chiffre à la source et sur les artefacts produits**, puis les confronte à ce
qui a été publié. Il ne tranche aucune décision ouverte : il les **chiffre**.

⚠️ **Aucune valeur de prix, de plafond de budget ou de fourchette d'arbitrage ne figure ici.** La fourchette
`~20–36 €` de #445 est une donnée d'arbitrage **interne** et ne doit pas partir chez un façonnier.

---

## §1 — Volumes : mesurés sur les PDF réellement produits

⚠️ **Ce qui suit n'est pas un report du tableau de référence.** Le nombre de cartes d'un devis se mesure
**à la carte** ; un tableau qui a dérivé produit un devis faux. Chaque ligne a donc été re-mesurée ici.

**Arbre de mesure** : `D:\Dev\Argumentum\.regen-1292-worktree` (Release, régénération du **06/09**).
Le bundle `review-v2.0.0-regen-20260912` **n'est pas présent sur cette machine** : les chiffres ci-dessous
décrivent l'arbre du 06/09, qui porte déjà la sortie de PK 96 (175 faces Fallacies).

**Instrument** : rendu de chaque page à 25 dpi par PyMuPDF, MD5 par page, puis comptage des motifs répétés.
Il mesure trois choses d'un coup — le nombre de pages, le nombre de **contenus distincts**, et la
**multiplicité** de chaque carte. C'est le nombre de contenus distincts qui donne les faces et les dos.

| Deck | PDF | pages | contenus distincts | faces | dos | motifs répétés |
|---|---|---:|---:|---:|---:|---|
| **Tarot** (Fallacies + Rules + Memo) | `Argumentum_TarotCards_fr.pdf` | **379** | 193 | 175 + 15 + **1** | 1 partagé + 1 Memo | `175×` (dos Fallacies), **`7×` + `7×` (Memo face + dos)** |
| **Scenarii** | `Argumentum_PokerCards_fr.pdf` | **334** | 174 | 167 | **7** | `36 / 30 / 27 / 25 / 18 / 17 / 14` |
| **Virtues** *(conditionnel)* | `Argumentum_TarotCards_Virtues_fr.pdf` | **262** | 132 | 131 | 1 partagé | `131×` (dos partagé) |

**Confrontation aux sources** (CSV `origin/master`, re-comptés) :

| Deck | source | règle de sélection | compte source | faces artefact | concorde |
|---|---|---|---:|---:|---|
| Fallacies | `Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv` | `carte` ∈ {1, 2} | **175** | 175 | ✅ |
| Rules | `Cards/Rules/Argumentum Rules - Cards.csv` | toutes les lignes | **15** | 15 | ✅ |
| Memo | `Cards/Memo/Argumentum_Memo_Face_fr.json` | 1 face | **1** | 1 | ✅ |
| Virtues | `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` | `card` = 1 | **131** | 131 | ✅ |
| Scenarii | `Cards/Scenarii/Argumentum Scenarii - Cards.csv` | toutes les lignes | **167** | 167 | ✅ |

**Les 7 dos Scenarii sont confirmés sur l'artefact, pas seulement au CSV.** Les multiplicités mesurées
(`36 / 30 / 27 / 25 / 18 / 17 / 14`) correspondent **exactement** aux 7 catégories du CSV
(`relation intime` 36 · `vie professionnelle` 30 · `mythologie` 27 · `vie personnelle` 25 · `pop culture` 18 ·
`histoire` 17 · `politique` 14 ; somme **167**). Sept dos = **sept calages** ou un appairage recto-verso sur
tout le deck. Un devis établi sur l'hypothèse « dos commun » revient faux, et il revient faux *après* le
délai de consultation.

**Contrôle de dérive :** PK 96 (`carte` = `''` au CSV, `carte` = `1` pour PK 108) est bien sorti — le compte
Fallacies est passé de 176 à **175**. Le chiffre de **381 pages/langue** cité dans le corps de l'issue datait
d'avant cette sortie : 381 − 379 = **2** = une face + son dos. L'écart est entièrement expliqué par la
décision, ce qui confirme que l'instrument discrimine.

---

## §2 — Le point qui change un devis : le Memo est imprimé **7 fois**, pas 1

**La mesure :** dans `Argumentum_TarotCards_fr.pdf`, la face **et** le dos du Memo apparaissent chacun
**7 fois** (pages p16 et p17, hash identique répété 7×). La configuration le dit aussi :
`WebBasedGeneratorConfig.cs` → `CardSetName = KnownCardSets.Memo, NbCopies = 7`.

```
dossier d'images : Target/fr/Images/density-0/Memo/ = 2 fichiers (1 face + 1 dos)
PDF              : la face Memo occupe 7 pages, le dos Memo 7 pages
```

C'est cohérent par construction : le Mémo est une carte d'aide, tirée **une par joueur** (le jeu se joue de 4
à 8 joueurs) — mais **ce n'est pas ce que disent les volumes publiés**.

| | Memo compté pour | Deck Tarot | Boîte sans Virtues | Boîte avec Virtues |
|---|---:|---:|---:|---:|
| **Publié** (corps d'issue + c.`5552589768`, 05/09) | 1 | **191** | **358** | **489** |
| **Mesuré sur l'artefact** | **7** | **197** | **364** | **495** |

⇒ **Six cartes d'écart par boîte.** Sur 8 langues, 48 cartes ; sur une série de 1 000 boîtes, 6 000 cartes.
À l'échelle d'un tirage, ce n'est pas un arrondi — c'est une ligne de devis.

**Les deux lectures ne peuvent pas être vraies en même temps.** Soit le deck doit sortir à **7 Mémos** et les
volumes publiés sont à corriger (364 / 495) ; soit il doit sortir à **1 Mémo** et c'est `NbCopies` qui doit
descendre à 1 — auquel cas **le PDF actuel ne correspond pas au produit voulu**. L'arbitrage est ouvert
depuis le 28/08 (c.`5448250092`, « Arbitrage Memo 1-vs-7 ») et **il n'a pas été refermé** avant que les
volumes 358/489 soient publiés le 05/09.

**Aucune valeur n'est publiée ici en tant que vérité** : la mesure dit ce que l'artefact fait, pas ce que le
produit doit être. Le worker mesure ; la décision est owner.

---

## §3 — Géométrie et fonds perdu

| Deck | format mesuré (artefact) | pixels @ 300 dpi | statut |
|---|---|---|---|
| Tarot (Fallacies, Rules, Memo, Virtues) | **69,9 × 120,0 mm** (nominal **70 × 120**) | 826 × 1417 | ✅ tranché par **#1250** (`3d218449`, ancêtre de `origin/master`) |
| Scenarii | **63,5 × 88,9 mm** — poker US 2,5″ × 3,5″ | 750 × 1050 | ✅ standard, aucun arbitrage requis |

#1250 était le gate qui interdisait de rédiger la case « format » : la branche retenue est **70 × 120 mm**,
et le fonds perdu a été **rogné** par la même passe. Conséquence directe sur le cahier des charges :

⚠️ **Le fonds perdu est à 0 dans les PDF livrés** (`BorderMM = 0` sur les quatre `DocumentCardSet` du deck
Tarot). Un imprimeur cale sur un fonds perdu de **3 mm** usuel : soit le pipeline doit **produire un PDF avec
fonds perdu** pour le façonnier, soit le façonnier l'ajoute — mais **il faut le demander explicitement**,
sinon il sera supposé. Le point relevé le 01/09 (fond perdu 5 mm face / 3 mm dos, non uniforme) est donc
**résolu par soustraction** : il n'y en a plus, ce qui déplace la question du fichier vers le cahier des charges.

---

## §4 — Cahier des charges technique — état par case

Statut : ✅ établi par mesure · 🟡 proposition à valider · 🔴 ouvert (owner/façonnier)

| Case | Valeur | Statut | Source |
|---|---|---|---|
| **Format Tarot** | 70 × 120 mm | ✅ | #1250, mesuré 69,9 × 120,0 |
| **Format Scenarii** | 63,5 × 88,9 mm (poker US) | ✅ | mesuré |
| **Fonds perdu** | **à 0 dans les PDF livrés** — à produire ou à convenir | 🔴 | `BorderMM = 0` |
| **Résolution** | 300 dpi (826 × 1417 / 750 × 1050 px) | ✅ | mesuré |
| **Espace colorimétrique** | DeviceCMYK + OutputIntent **SWOP** | ✅ | chaîne `--pdf-cmyk` (#632/#652) |
| **Nombre de dos Tarot** | 1 dos partagé (Fallacies, répété 175×) + 1 dos Memo + 1 dos Virtues | ✅ | mesuré |
| **Nombre de dos Scenarii** | **7** (un par catégorie) | ✅ | mesuré |
| **Grammage / finition / pelliculage** | 300 g/m² couché (Tarot) · 350 g/m² noyau noir (Scenarii) · lin grain fin · pelliculage mat | 🟡 | proposition c.`5448477810`, non validée |
| **Boîte** | type, grammage, finition | 🔴 | décision 4, non tranchée |
| **EN71** | conformité jouet **obligatoire** si le public inclut des mineurs | 🔴 | à attester par le façonnier |
| **Pelliculage / marquage CE / traçabilité** | — | 🔴 | à cadrer avec le façonnier retenu |

---

## §5 — Matrice à transmettre (2 configurations × paliers × changement de référence)

Le squelette déjà posté (c.`5448477810`) reste valable **une fois la ligne « format » et la ligne « Memo »
corrigées**. Trois dimensions, et elles sont toutes les trois nécessaires :

1. **Deux configurations en colonnes** : *(Tarot + Scenarii)* et *(Tarot + Scenarii + Virtues)* — même boîte,
   même finition, seul le contenu change. Un devis mono-configuration ne peut pas arbitrer un choix binaire :
   il impose un second tour d'attente.
2. **Deux à trois paliers de volume** — c'est le seuil numérique/offset qui déplace le coût unitaire.
3. **Coût du changement de référence** : chaque langue est une référence distincte (8 références monolingues).
   Demander explicitement le **calage** et le **minimum par référence** — c'est ce poste, et non le prix
   unitaire, qui décide si les 8 langues partent ensemble ou par vagues.

**Volumes à porter sur la matrice** — les deux lignes Memo, pour que le façonnier chiffre la question au lieu
de la subir :

| Ligne de devis | Mémo = 1 | Mémo = 7 *(ce que l'artefact produit aujourd'hui)* |
|---|---:|---:|
| Fallacies | 175 | 175 |
| Rules | 15 | 15 |
| Memo | 1 | **7** |
| **Deck Tarot** | **191** | **197** |
| Scenarii | 167 | 167 |
| **Config A** — Tarot + Scenarii | **358** | **364** |
| Virtues | 131 | 131 |
| **Config B** — + Virtues | **489** | **495** |

---

## §6 — Défauts soldés depuis le BAT (à ne pas re-payer)

| Défaut | État | Preuve |
|---|---|---|
| **`CCby` vide sur 74 / 167 Scenarii** (verdict BAT 29/08, « 44 % du deck sort sans sa ligne Creative Commons ») | ✅ **soldé** | `a23e9568` — *« fix(scenarii): #1226 fill 74 empty CCby cells with 'Argumentum' + attribution guard (#1227) »* ; re-mesure sur `origin/master` : **167/167** lignes portent `Argumentum`, **0 vide** |
| **PK 96 « Appel à la nature »** doublon taxonomique | ✅ **sorti du deck** | CSV : PK 96 `carte` = `''`, PK 108 `carte` = `1` ; artefact : 175 faces (était 176) |
| **Format 60 × 113 mm non standard, cartes étirées +4 à +15 %** | ✅ **tranché** | #1250 → 70 × 120 mm, mesuré conforme |
| Police de marque perdue en `ar`/`fa`/`zh` (bloquant BAT du 29/08) | ⚠️ **hors périmètre de cette mesure** | non re-testé ici — à confirmer par ai-01 sur un arbre postérieur au correctif |

Le `CCby` est le cas le plus utile à consigner : il figurait au verdict BAT comme un défaut **du tier boîte**,
et il a été fermé **sans que le dossier de fabrication l'enregistre**. Un dossier qui ne solde pas ses défauts
les fait redemander.

---

## §7 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **Cap 0** | la mesure porte sur les artefacts, pas sur un tableau | les 3 PDF sont ouverts et pages comptées, pas recopiés ✅ |
| **positif** | les comptes source et artefact doivent concorder | 5 decks sur 5 concordent (175 / 15 / 1 / 131 / 167) ✅ |
| **négatif** | l'instrument doit discriminer | PK 96 : 381 → 379 pages (**−2** = 1 face + 1 dos), écart expliqué par la décision ✅ |
| **multiplicité** | un contenu répété doit être vu comme répété | dos Fallacies `175×`, Memo `7×`, dos Scenarii `7 motifs distincts` — sinon le Memo serait passé pour 1 carte ✅ |
| **7 dos** | les 7 dos Scenarii doivent correspondre aux 7 catégories | multiplicités `36/30/27/25/18/17/14` = catégories CSV, somme 167 ✅ |
| **géométrie** | le format doit être conforme à #1250 | 826 × 1417 px = 69,9 × 120,0 mm ✅ |

---

## §8 — Ce que je n'établis PAS

- **Que le Mémo doive sortir à 1 ou à 7.** Je mesure ce que l'artefact produit (7) et ce que les volumes
  publiés supposent (1). La décision est owner.
- **Que l'arbre du 06/09 soit l'arbre de référence.** Le bundle `review-v2.0.0-regen-20260912` n'est pas sur
  cette machine ; je n'ai pas pu mesurer l'arbre du 12/09. Les comptes de faces sont toutefois **re-dérivés
  du CSV `origin/master`** et concordent, ce qui rend peu probable une divergence de volume — mais ce n'est
  pas une vérification.
- **Le grammage, la finition et le pelliculage.** Proposés le 28/08, jamais validés.
- **Le type de boîte, le budget, le façonnier.** Décisions 4 et 5, non tranchées.
- **Le sort du défaut de police `ar`/`fa`/`zh`.** Non re-testé ici.
- **Aucun verdict de fabrication**, aucun contact façonnier, aucun devis demandé.

---

## §9 — Gates restants avant envoi aux façonniers

1. 🔴 **Arbitrage owner Memo 1-vs-7** — change 6 cartes par boîte (§2). **À trancher avant l'envoi** : c'est
   exactement le cas qu'un second tour de consultation rend coûteux.
2. 🔴 **Fonds perdu** — décider s'il est produit par le pipeline ou ajouté par le façonnier (§3).
3. 🔴 **Décision 3 — Vertus** — tranchée avec les devis des deux configurations sous les yeux.
4. 🔴 **Boîte** (type, grammage, finition, EN71) — décision 4.
5. ⏳ **Bundle CMYK vérifié** — contrôle explicite de présence DeviceCMYK, pas la seule sortie 0 du pipeline.
6. ⏳ **Régénération post-décisions** — aucun PDF ne doit partir chez le façonnier avant que les décisions
   §9.1 et §9.2 aient atterri : les deux changent le fichier livré.
7. ⏳ **Organe épinglant les cinq comptes** (préalable DoD posé le 28/08) — un compte de cartes qui dérive en
   silence entre deux consultations est précisément ce que ce dossier vient de rattraper à la main.
8. ⏳ **BAT physique validé par les trois** (jsboige, Adeline, Thomas).

---

*master `769a373d` · arbre mesuré : `.regen-1292-worktree` (Release, 06/09) · instruments : PyMuPDF (rendu
25 dpi + MD5 par page), lecture CSV, `git grep` sur `origin/master` · lecture seule : ⛔ aucune régénération,
aucune publication, aucune écriture CSV, aucun contact façonnier, aucun devis, aucune valeur de prix ·
verdict visuel et décisions : ai-01 / owner.*
