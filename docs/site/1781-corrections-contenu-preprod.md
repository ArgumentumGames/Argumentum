# Manifeste des corrections de contenu — préprod #1781 (06/10)

**Règle de rejouabilité** : la bascule reprend le contenu de la **préprod** (mesuré 06/10 — le texte visible des 6 pages clés est identique au caractère près entre prod et préprod). Ce manifeste est donc **exécutable en préprod seulement**, et les corrections y restent. Si la bascule recopiait la prod (changement de procédure), chaque ligne ci-dessous se rejoue à l'identique sur la prod : la source de chaque correction est citée, l'avant/après est exact, aucune dépendance au contexte préprod.

**Norme typographique** (owner, règle des cartes, transposée au site) : en français, espace **insécable** avant `; : ! ?`, majuscule aux noms propres, accents partout. Aucune correction de fond : les questions de fond (contenu, liens cassés, structure) sont des décisions owner, listées en fin de document.

**Source de chaque correction** : `addinette` = relevé d'Adeline sur #1781 · `ai-01` = vérification sur les deux sites · `po-2023` = relecture typographique complète.

---

## Page Accueil (`/`)

| # | Avant | Après | Source | Note |
|---|---|---|---|---|
| 1 | `lesinne` | `lésine` | addinette | orthographe |
| 2 | `ce qui le rendant` | `ce qui le rend` | addinette | grammaire |
| 3 | `nouveaute /et` | `nouveauté et` | addinette | accent + espace parasite |
| 4 | `Etudiant ou passionne` | `Étudiant ou passionné` | addinette | accents |
| 5 | `philosophiques? argumentum` | `philosophiques ? Argumentum` | addinette | espace insécable + majuscule (×6 sur la page) |
| 6 | `puis aptes` | `plus aptes` | addinette | orthographe |
| 7 | `Sociologue` (fin de paragraphe) | `Sociologue.` | addinette | point final manquant |
| 8 | `general ?Argumentum` | `général ? Argumentum` | addinette | accent + espace insécable |
| 9 | `Retrouvez nous` | `Retrouvez-nous` | addinette | trait d'union (pied de page commun, toutes pages) |
| 10 | `argumentum` (×6, hors titre/logo) | `Argumentum` | addinette | majuscule au nom propre |
| 11 | `égalament` | `également` | ai-01 | orthographe |
| 12 | `Schtroumpf costaux` | `Schtroumpfs costaux` | ai-01 | pluriel |
| 13 | `l'extension que nous avons choisi` | `l'extension que nous avons choisie` | ai-01 | accord du participe passé |

## Page Actus (`/Actus`)

| # | Avant | Après | Source |
|---|---|---|---|
| 14 | `le plateforme DNN` | `la plateforme DNN` | addinette |
| 15 | `belgique` | `Belgique` | addinette |
| 16 | `etats-unis` | `États-Unis` | addinette |

## Page Règles (`/Règles`)

*Aucune correction de contenu relevée — la page est déjà conforme (vérifié ai-01 05/10, et le corpus CSV Rules a été corrigé séparément par #1768/#1778).*

## Page Argumentation (`/Argumentation`)

*Aucune correction de contenu relevée.*

## Page Téléchargements (`/Téléchargements`)

*Aucune correction de contenu relevée (les 2 zip manquants sont restaurés, voir grain A).*

## Page Amis (`/Amis`)

| # | Avant | Après | Source |
|---|---|---|---|
| 17 | `Le Drenche` (sans point final) | `Le Drenche.` | addinette |

## Pied de page commun (toutes pages)

| # | Avant | Après | Source |
|---|---|---|---|
| 18 | `Retrouvez nous` | `Retrouvez-nous` | addinette |

---

## Corrections de fond — décisions owner (à transmettre, pas à poser)

| # | Objet | État | Proposition |
|---|---|---|---|
| H | Page « On peut commander le jeu » | owner OK | Garder le texte, ajouter un encart « Bientôt de retour » rouge par transparence |
| I | Lien Fanny Benard (mairie, 404) | owner OK | Wayback Machine si copie existe, sinon retirer le lien en gardant nom+fonction |
| J | Page Amis | owner OK | Logo et texte directement sur la page, sans « en savoir plus » (3 fiches) |
| K | Ontologie fallacieuse | owner OK | OWL reste le livrable ; menu → page de présentation lisible + lien téléchargement ; en option : visualiseur JS (choix de la bibliothèque à motiver) |
| E | Lien « code de conduite intellectuel » (Interview) | ouvert | Même URL que McCandless ; proposer 1-2 sources stables (piste : Damer, *Attacking Faulty Reasoning*) |
| C | Bloc « Retrouvez-nous » / contact / newsletter | à tester | Vérifier chaque lien ; config formulaire/newsletter sans envoyer ; vrai envoi « TEST » proposé à Adeline |
| D | Page « Acheter le jeu » grisée au chargement | à mesurer | 5 chargements de chaque côté, temps jusqu'à affichage utilisable |
| A | Ticket Print & Play 404 | **résolu** | 2 zip restaurés depuis prod ; ticket servira après recycle pool (UAC en attente) |

---

*Généré par po-2023 le 06/10, à partir de #1781 + vérifications ai-01 + relecture typographique. Les corrections 1-18 se posent en préprod seulement, avec export de chaque module avant modification.*
