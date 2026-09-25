# #1188 — Dossier d'arbitrage : quel canal de distribution

**Auteur** : po-2024 (worker) · **Date** : 2026-09-21 · **Base** : master `cea6e699`
**Mise à jour** : 25/09/2026, base `9e79e88d` — §1.a réécrit, matrice §2 et §4/§6 alignés (la version du 21/09 mesurait juste **avant** la fusion de `LICENSE-CONTENT.md`)
**Grain** : pool #458 v7 ⑦ puis v13 ④ · **Statut** : **dossier d'arbitrage — ⛔ aucun contact pris, aucune décision prise**

---

## Ce que ce document est, et ce qu'il n'est pas

Les quatre voies, leurs acteurs et leurs coûts généraux sont **déjà écrits dans #1188** — ce dossier ne les recopie pas. Il ajoute ce que l'issue ne pouvait pas porter : **ce que le dépôt mesure déjà** (licence, catalogue, volumétrie), et une **matrice de préparation** qui dit, voie par voie, ce qui est prêt, ce qui bloque, et **où la mesure se lit**.

⛔ **Aucun contact n'a été pris** (ni distributeur, ni Canopé, ni éditeur). ⛔ Aucun prix n'est fixé. ⛔ Aucune voie n'est recommandée : l'arbitrage revient à l'owner.

---

## 1. Trois mesures qui déplacent l'arbitrage

### 1.a — La licence du corpus est **documentée** : CC BY-SA 4.0 sur 7 fichiers énumérés — la voie 3 change de forme

*Corrigé le 25/09/2026 : la version du 21/09 concluait « LGPL-3.0, blocage dur de la voie 3 » — elle mesurait juste **avant** que [`LICENSE-CONTENT.md`](../../LICENSE-CONTENT.md) ne soit fusionné (24/09, Q-15a).*

Le dépôt porte **deux licences**, et le corpus n'est plus dans l'angle mort :

| Ce qui | Licence | Où c'est écrit |
|---|---|---|
| **Corpus éditorial** — les **5 CSV de texte des cartes** + les **2 ontologies OWL dérivées** (Q-15a) | **CC BY-SA 4.0** | `LICENSE-CONTENT.md` §1 (énumération fichier par fichier) |
| **Tout le reste**, y compris le code, les gabarits, les illustrations | LGPL-3.0 (code) ou régime propre | `LICENSE` ; `README.md:216-230` porte la même table |

Le périmètre CC BY-SA est **énuméré, jamais déduit d'un répertoire** (`Cards/` compte 803 fichiers dont 617 images) ; en sont retirés délibérément les illustrations, le façonnage, les polices, les gabarits et **la prose des règles** (`regles.md`/`rules.md`, co-écrites — 57 % et 10 % par un co-auteur, accord nommé requis). Les 5 CSV de texte sont, eux, mesurés à **100 % d'écriture du titulaire** — c'est ce qui rend leur cession possible aujourd'hui.

Ce que la licence déclarée dit, **textuellement** — CC BY-SA 4.0, **Art. 2(a)(1)** :

> « le Donneur de licence Vous autorise à exercer pour le monde entier, à titre gratuit, non sous-licenciable, **non exclusif, irrévocable**, les Droits accordés par la licence afin de : (1) reproduire et Partager l'Œuvre sous licence, en tout ou partie ; et (2) produire, reproduire et Partager l'Œuvre dérivée. »

Trois conséquences mécaniques — elles remplacent le « blocage dur », sans basculer dans l'excès inverse :

1. **Non exclusif et irrévocable** : les droits accordés sur le corpus **déjà publié** le sont définitivement, à tous. Une exclusivité **ne peut donc pas porter sur ces versions-là** — et ce n'est pas propre à CC BY-SA : c'est la propriété de la licence effectivement déclarée.
2. **La licence n'épuise pas les droits du titulaire** : une concession **non exclusive** laisse le Donneur libre de céder ou de licencier autrement — la voie 3 n'est donc pas fermée, elle est **bornée** : elle peut porter sur ce qui n'a pas été publié sous CC BY-SA, ou prendre la forme d'une concession non exclusive.
3. Les conditions de CC BY-SA (attribution, partage à l'identique) s'attachent au corpus : elles pèsent sur ce que la voie 3 pourrait vouloir faire d'une **version dérivée ou remaniée**, pas sur la cession de droits du titulaire.

⇒ Ce qui reste à arbitrer n'est plus « **quelle** licence s'applique » (c'est mesuré et documenté) mais **ce que la voie 3 cède exactement**, sous quel régime, et l'accord nommé du co-auteur pour les deux fichiers de prose. C'est un périmètre à poser, plus une inconnue à lever.

**Instrument** : `LICENSE-CONTENT.md` §1 (les 7 fichiers), §1.a (les 2 exclus), §2 (liens de licence), §3 (co-auteurs) ; `README.md:216-230` (table de double licence) ; citation lue sur le **legalcode FR** lié par `LICENSE-CONTENT.md` §2 ; `head -20 LICENSE` (LGPL-3.0, 29 June 2007) pour le code ; `git ls-files | grep -ciE '(^|/)(LICENSE|LICENCE|COPYING)'` → **128** fichiers de licence dans le dépôt au 25/09 (`-i` : insensible à la casse, `DNNPlatform/` inclus — la forme sans `-i` rend 52).

### 1.b — Le catalogue est déjà livré, et les quatre voies ne consomment pas les mêmes artefacts

Les **80 assets publiés** (`v2.0.0-review`) se décomposent en **3 formats × 8 langues** (+ les 8 Vertus) :

| Format livré | Assets | Ce qu'il sert |
|---|---:|---|
| **Web** — `Fallacies_Web_{A0,A4,Thumbnails_A4}` | 24 | diffusion libre, prescription, vitrine (voies 1 et 4) |
| **Print & Play** — `{PokerCards,TarotCards}_Print.Play{A4,_Light_A4}` | 32 | **un enseignant imprime** (voie 4) + téléchargement direct (voie 1) |
| **Decks imprimables** — `TarotCards`, `PokerCards`, `TarotCards_Virtues` | 24 | la **boîte physique** (voies 1, 2, 3) |
| **Total** | **80** | |

⇒ **La voie 4 (canal éducatif) est la seule dont les artefacts sont déjà 100 % produits** : ce qu'un enseignant utilise, c'est le **Print & Play A4**, et il existe en 8 langues. Les voies 1–3 consomment les decks destinés à la boîte, donc dépendent de #1187.

### 1.c — Le contenu de la boîte **couple** #1188 à deux autres chantiers ouverts

Le contrat de comptage (`PdfDeckCountContractTests`, #1176/#1288) fixe la boîte à **364 instances physiques** sans Vertus (`197 Tarot + 167 Scenarii`) et **495** avec. Par langue :

| | Cartes / boîte | × 8 langues |
|---|---:|---:|
| Tarot (175 Fallacies + 15 Rules + memo ×7) | 197 | 1 576 |
| Scenarii | 167 | 1 336 |
| **Boîte sans Virtues** | **364** | **2 912** |
| Vertus | 131 | **1 048** |
| **Boîte avec Virtues** | **495** | **3 960** |

Deux conséquences que #1188 ne porte pas :

1. **Mettre le deck Vertus (131 cartes) dans la boîte fige 1 048 cartes à l'impression** — or c'est précisément le deck dont le **principe générateur** est en cours de refondation (Epic C, #980 → C1 #986, dont le pré-draft est en PR #1463). ⇒ La décision « contenu de la boîte » de #1187 **détermine si C1 est une décision pré-impression ou post-impression**. À trancher explicitement, pas par défaut.
2. **Le nombre de références est le vrai facteur de coût, et c'est une variable de la décision de distribution** : une boîte par langue = **8 références** ; decks séparés = **16** ; avec Vertus = jusqu'à 24. Chaque référence multiplie les minima de tirage — et c'est la voie retenue qui fixe ce nombre, pas le façonnier.

---

## 2. Matrice de préparation — voie × prérequis

État : ✅ prêt · 🟡 partiel · ⛔ bloqué · ❔ non évaluable sans contact.

| Prérequis | 1. Direct | 2. Distributeur | 3. Cession éditeur | 4. Éducatif |
|---|---|---|---|---|
| Artefacts produits | ✅ 80 assets | ✅ 80 assets | ✅ 80 assets | ✅ **Print & Play ×8 langues** |
| Contenu de la boîte figé (#1187 déc. 1) | ⛔ | ⛔ | ⛔ | 🟡 (P&P, pas de boîte) |
| Fabrication / stock (#1187) | ⛔ série | 🟡 **~10 unités** (voir §3) | ⛔ série (éditeur) | 🟡 impression locale |
| **Licence du corpus** | ✅ (libre) | ✅ (libre) | 🟡 **CC BY-SA 4.0 — pas d'exclusivité sur le publié (§1.a)** | ✅ (libre) |
| Prix public | ❔ | ❔ | ❔ | ❔ **modèle par établissement** |
| Prix de cession | — (sans objet) | 🟡 **pas de cession en dépôt-vente** | ❔ royalties | — (sans objet) |
| Boutique / tunnel de paiement | ⛔ **dormant, 2 défauts** (#1180) | — | — | 🟡 bon de commande public |
| Conformité EN71 (public mineur) | ⛔ | ⛔ | ⛔ | ⛔ **obligatoire si scolaire** |
| Interlocuteur identifié | — | 🟡 liste dans #1188 | 🟡 carte des 259 | 🟡 Canopé/CSEN/éduscol cités |
| Solvabilité du partenaire vérifiée | — | ❔ **vérifiable gratuitement** (SIREN public) | ❔ | — |

**Lecture de la matrice** : aucune voie n'est prête. Mais elles ne sont pas bloquées par la même chose — la voie 3 par le **périmètre cédable** (§1.a), les voies 1–2 par la **fabrication**, la voie 4 par la **conformité** et un **modèle de prix**. Confondre ces blocages conduit à attendre #1187 pour tout, alors que #1187 ne bloque **que** les voies 1–3.

---

## 3. Le modèle de rémunération discrimine mieux que le nom du canal

#1188 présente le **risque de crédit** (affaire Makassar, juillet 2026) comme générique au canal distributeur : « confier son stock à un distributeur, c'est prendre un risque de crédit ». **C'est vrai, mais l'ampleur dépend entièrement du modèle de cession** — et la piste n°1 de l'issue le documente elle-même.

**Mesuré le 21/09 sur la page publique d'Anoukis Distribution** (`/fr/distribuer-mon-jeu`, lecture publique, ⛔ aucun contact) :

| Terme publié | Ce qu'il implique |
|---|---|
| **Dépôt-vente** : « aucun achat à l'avance, vous restez propriétaire de votre stock » | l'exposition au défaut est **bornée par le stock confié**, pas par un stock acheté |
| « une **dizaine d'unités** peut suffire pour commencer » | le **ticket d'entrée est proto-échelle**, pas série-échelle |
| « payé sur les ventes réalisées », commission réduite, **prix fixé par le créateur** | **il n'y a pas de prix de cession** — donc la question n°2 de #1188 n'a pas la même forme selon la voie |
| « quelques jours » pour le référencement | délai court, contraste avec les délais administratifs de la voie 4 |

⇒ **Trois déplacements** :

1. Le **risque Makassar ne disparaît pas** en dépôt-vente (on est payé sur les ventes réalisées — c'est exactement le cas des auteurs cités), mais il est **plafonné** par la taille du dépôt. La phrase de #1188 est à corriger d'un facteur ≈ *nombre d'unités en dépôt*, pas à retirer.
2. **La voie 2 ne dépend pas de la décision de série** : à ~10 unités on est dans le **proto numérique**, pas dans l'offset. Elle dépend en revanche du **contenu de la boîte** — le même blocage que #1187.
3. Le contrôle que la DoD réclame (« solidité financière vérifiée avant signature ») est **exécutable sans contact** : Anoukis publie **SIREN 490 471 950 00041** et **TVA FR67490471950**. Un distributeur est vérifiable sur registre public avant toute démarche.

**Ce que cette mesure n'établit pas** : Anoukis est la **seule** des sept pistes de #1188 dont les termes sont publics et chiffrés. Sur ABI Games (page d'accueil publique, même date), on ne trouve qu'un catalogue et un « espace pro » — **aucun chemin d'entrée indépendant visible**. ⇒ Les conditions des six autres **ne sont pas évaluables depuis l'extérieur** : c'est précisément ce qui rend les ≥ 3 contacts de la DoD nécessaires, et non optionnels.

---

## 4. Ce qui est mesurable maintenant, et ce qui exige un contact

| Question de #1188 | Décidable maintenant | Exige un contact |
|---|---|---|
| **1. Modèle** (direct / distributeur / cession / éducatif) | la **structure** et les **blocages** (§2) — mais pas le choix | les conditions réelles des partenaires |
| **2. Prix public + prix de cession** | qu'il n'y a **pas de cession en dépôt-vente** (§3) | tout le reste : chaque voie a un prix différent, aucun n'est mesurable à distance |
| **3. Périmètre géographique** | le catalogue existe en **8 langues** ; une boîte par langue = **8 références** (§1.c) | la disposition des distributeurs à porter du multilingue |
| **4. Licence du corpus** | ✅ **documenté** : CC BY-SA 4.0 sur 7 fichiers énumérés ; non exclusif + irrévocable ⇒ **pas d'exclusivité sur le publié** (§1.a) | ce que la voie 3 cède exactement — et, pour `regles.md`/`rules.md`, l'accord **nommé** du co-auteur |
| **5. Rouvrir la boutique** | les 2 défauts de fiche et l'état dormant sont **connus et localisés** (#1180) | rien — c'est un geste technique, pas une négociation |

---

## 5. Ce que ce dossier n'établit pas

⛔ **Aucun contact** : rien ici n'engage Argumentum vers un partenaire ; c'est aussi pourquoi le dossier **ne peut pas trancher** le modèle.
⛔ **Aucun prix** : ni public, ni de cession, ni coût de revient. Le **coût de revient n'existe pas encore** (il dépend des devis de #1187, non demandés) — toute grille de prix construite avant est un chiffre inventé.
⛔ **Aucun conseil juridique** : §1.a **cite** le texte de la licence déclarée et en tire les conséquences mécaniques (non exclusif + irrévocable ⇒ pas d'exclusivité sur le publié ; le titulaire garde la faculté de céder ou licencier autrement) — il n'apprécie pas les mérites d'une cession. L'arbitrage appartient à l'owner, avec conseil s'il l'estime nécessaire.
⛔ **Les termes d'Anoukis sont ceux de sa page publique au 21/09/2026** — ils peuvent changer, et ils ne remplacent pas des conditions contractuelles.
⛔ **La voie 4 n'est pas chiffrée** : le modèle « achat par établissement » n'a été ni tarifé ni documenté ici ; les assets éducatifs propres au corpus sont d'ailleurs **embryonnaires** — la colonne `KIDZ` porte **6 cellules remplies sur 223**, dont une est un marqueur de travail (« wording à travailler »), soit **5 entrées réelles**. En revanche le corpus **Print & Play** est, lui, complet : 24 (Vertus) + 35 (Fallacies) + 27 (Scenarii) + 3 (Rules P&P) lignes marquées.
⛔ **Les 2 défauts de fiche produit** (`## En Rupture ##`, « Nach oben ») sont repris de #1188/#1180, **non re-mesurés ici** — le site est un acte de production, hors périmètre de ce grain.

---

## 6. Instrument

| Mesure | Voie |
|---|---|
| Licences déclarées | `LICENSE-CONTENT.md` §1/§1.a/§2/§5 (les 7 fichiers sous CC BY-SA, les 2 exclus, les liens de licence, la non-rétroactivité) · `README.md:216-230` (table de double licence) · `head -20 LICENSE` (LGPL-3.0) · `git ls-files \| grep -ciE '(^\|/)(LICENSE\|LICENCE\|COPYING)'` → 128 au 25/09 (`-i`, insensible à la casse) |
| Catalogue d'assets | `gh release view v2.0.0-review --json assets --jq '.assets[].name'` → 80, classés par préfixe |
| Volumétrie boîte | `PdfDeckCountContractTests` (contrat pinné, #1176/#1288) : 197 + 167 = 364 instances ; Vertus 131 |
| Assets éducatifs | comptage des colonnes `KIDZ` et `print_and_play` sur les 4 CSV (`Cards/Fallacies/`, `Cards/Scenarii/`, `Cards/Rules/`) |
| Termes distributeur | lecture publique `anoukis-distribution.fr/fr/distribuer-mon-jeu` et `abigames.fr`, 21/09/2026 |

*Ce dossier est un instrument d'arbitrage : il dit ce que le dépôt sait déjà, et nomme explicitement ce qu'aucune mesure interne ne peut produire — les conditions réelles des partenaires, qui exigent une démarche et donc un GO owner.*
