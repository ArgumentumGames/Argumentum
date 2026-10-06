# Manifeste des corrections de contenu — préprod #1781 (06/10, v2 — carte de stockage)

**Règle de rejouabilité** : la bascule reprend le contenu de la **préprod** (mesuré 06/10 — le texte visible des 6 pages clés est identique au caractère près entre prod et préprod). Ce manifeste est donc **exécutable en préprod seulement**, et les corrections y restent. Si la bascule recopiait la prod (changement de procédure), chaque ligne ci-dessous se rejoue à l'identique sur la prod : la source de chaque correction est citée, l'avant/après est exact, aucune dépendance au contexte préprod.

**Norme typographique** (owner, règle des cartes, transposée au site) : en français, espace **insécable** avant `; : ! ?`, majuscule aux noms propres, accents partout. Aucune correction de fond : les questions de fond (contenu, liens cassés, structure) sont des décisions owner, listées en fin de document.

**Source de chaque correction** : `addinette` = relevé d'Adeline sur #1781 · `ai-01` = vérification sur les deux sites · `po-2023` = relecture typographique complète.

**Carte de stockage** (re-mesurée 06/10 contre la base) : tout le contenu éditable vit dans le stockage EAV `TsDynDataValue`/`TsDynDataEntity` (DB `ArgumentumGames`, `localhost\SQLEXPRESS`). Chaque ligne donne **l'entité EAV et l'attribut** à modifier — le contrôle = diff du texte visible = lignes du manifeste exactement. Export avant modification (dump JSON de l'entité) vers `Logs/`.

> ⚠️ Les entités `Content` (App 33) portent le **corps accent-pauvre** (HTML-entités `&eacute;` etc. mélangeés à des fautes d'accent nues : `eleves`, `deja`, `tres`, `societe`, `decouvrir`…). Adeline a relevé « la nouvelle version a perdu de nombreux accents » : c'est l'état du texte, pas une régression. Le périmètre des corrections 1-18 est **ciblé** (les coquilles explicites), pas « accentuer tout le site » — une passe globale d'accentuation est une décision owner séparée (elle touche ~400 entités).

---

## Page Accueil (`/`)

| # | Avant | Après | Source | Entité EAV (App 33 `Content`) | Note |
|---|---|---|---|---|---|
| 1 | `lesinne` | `lésine` | addinette | E10050 `Text` (V27511) | orthographe |
| 2 | `ce qui le rendant` | `ce qui le rend` | addinette | E10050 `Text` (V27511) | grammaire |
| 3 | `nouveaute /et` | `nouveauté et` | addinette | E10050 `Text` (V27511) | accent + espace parasite |
| 4 | `Etudiant ou passionné` | `Étudiant ou passionné` | addinette | E10015 + E10012 `Title` (doublon) ; **+ 6 autres titres** (E9996, E9999, E10017, E10025, E10032, E10035) | seul le É manquait — « passionné » était déjà accentué ; étendu aux 7 titres « Etudiant » du site (même règle) |
| 5 | `philosophiques? argumentum` | `philosophiques ? Argumentum` | addinette | E10015 `Text` (V22914) | espace insécable + majuscule |
| 6 | `puis aptes` | `plus aptes` | addinette | E10015 `Text` (V22914) | orthographe |
| 7 | `en tout genre` (fin du corps « Sociologue ») | `en tout genre.` | addinette | E10045 `Text` (V23008) | point final manquant — le titre accordéon `Sociologue` (E10042/E10045 `Title`) reste sans point |
| 8 | `general ?Argumentum` | `général ? Argumentum` | addinette | E10055 `Text` | accent + espace insécable |
| 9 | `Retrouvez nous` | `Retrouvez-nous` | addinette | E10065 `Title` (V27676) | trait d'union (pied de page commun, toutes pages) |
| 10 | `? argumentum` (×6) / `?Argumentum` (×1) | `? Argumentum` | addinette | E9999, E10010, E10015, E10025, E10030, E10040 (`? argumentum`) + E10055 (`?Argumentum`) `Text` | majuscule au nom propre + insécable (6 occurrences servies ; E10050 utilise déjà `? Argumentum`) |
| 11 | `égalament` | `également` | ai-01 | E10081 `Content` (News, App 52) | ⚠️ **vit dans l'article « Lancement d'Argumentum — Interview des co-créateurs »**, pas sur l'Accueil |
| 12 | `le Schtroumpf costaux` | `le Schtroumpf costaud` | ai-01 | E10206 `Content` (News, App 52) | ⚠️ **corrigé** : c'est une liste de **personnages à incarner** (« …pour Jules César, le Schtroumpf costaud, ou un mari adultère ») — le singulier est correct, la coquille est `costaux` → `costaud`, pas le pluriel |
| 13 | `l'extension que nous avons choisi` | `l'extension que nous avons choisie` | ai-01 | E11897 `Content` (News, App 52) | accord du participe — vit dans l'article « On peut commander le jeu de cartes sur ce site » |

## Page Actus (`/Actus`) — les corrections 14-16 vivent dans les **articles détail**, pas la liste

| # | Avant | Après | Source | Entité EAV (News, App 52) |
|---|---|---|---|---|
| 14 | `le plateforme DNN` | `la plateforme DNN` | addinette | E11897 `Content` — article « On peut commander le jeu… » |
| 15 | `belgique` | `Belgique` | addinette | E10206 `Content` — article « Argumentum aux REC 2022 » |
| 16 | `etats-unis` | `États-Unis` | addinette | E10206 `Content` — article « Argumentum aux REC 2022 » |

## Page Règles (`/Règles`)

*Aucune correction de contenu relevée — la page est déjà conforme (vérifié ai-01 05/10, et le corpus CSV Rules a été corrigé séparément par #1768/#1778).*

## Page Argumentation (`/Argumentation`)

*Aucune correction de contenu relevée.*

## Page Téléchargements (`/Téléchargements`)

*Aucune correction de contenu relevée (les 2 zip manquants sont restaurés, voir grain A).*

## Page Amis (`/Amis`)

| # | Avant | Après | Source | Entité EAV | Note |
|---|---|---|---|---|---|
| 17 | `Le Drenche est un journal visant à aider les citoyens à se forger leur opinion` | `… leur opinion.` | addinette | E10164 `Summary` (V23346, App 59 `Term`) | point final manquant — le titre `Drenche (le)` reste inchangé |

## Pied de page commun (toutes pages)

| # | Avant | Après | Source | Entité EAV | Note |
|---|---|---|---|---|---|
| 18 | `Retrouvez nous` | `Retrouvez-nous` | addinette | E10065 `Title` (V27676, App 33 `Content`) | = correction 9, même entité |

---

## Coquilles supplémentaires relevées pendant la localisation (à trancher — petites, même famille)

Ces fautes sont dans le **même texte** que les corrections 1-18 mais **hors du relevé explicite d'Adeline**. Pose seulement si l'owner le valide (elles restent dans la même entité, réversibles, même méthode d'export).

| Avant | Après | Entité | Note |
|---|---|---|---|
| `l'intégralité notre notre présentation` | `l'intégralité de notre présentation` | E10081 `Content` | double « notre » + « de » manquant |
| `plusieurs notions suplémentaires` | `plusieurs notions supplémentaires` | E10081 `Content` | accent manquant |
| `Vous defendez l'esprit critique et souaitez` | `… souhaitez` | E10055 `Text` | coquille « souaitez » |

*(La passe d'accentuation globale — `eleves`/`deja`/`tres`… — est une décision owner séparée, volontairement hors périmètre ici.)*

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
| A | Ticket Print & Play 404 | **ouvert** | Zips restaurés sur disque, mais le **lien** reste cassé (ticket DES `FileID=-1`) — la correction = régénérer le lien en direct (modèle des 4 autres zips), posée en préprod 06/10 |

---

*v2 (06/10, po-2023) : ajoute la carte de stockage EAV (entité + attribut par correction), corrige le périmètre (11-16 vivent dans les articles Actus détail, pas l'Accueil/liste ; 4 = seul le É manque ; 12 à re-valider pour l'accord de l'article), et signale 3 coquilles adjacentes hors relevé. Les corrections 1-18 se posent en préprod seulement, avec export de chaque entité (dump JSON) avant modification.*
