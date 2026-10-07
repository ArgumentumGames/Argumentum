# Manifeste des corrections de contenu — préprod #1781 (07/10, v3 — trace de tout ce qui est posé)

**Ce document est la trace de ce qui est *posé* en préprod**, grain par grain, avec la date et l'entité modifiée. Chaque ligne est rejouable à l'identique : l'avant et l'après sont exacts, la source est citée, l'entité est nommée.

**Règle de rejouabilité** : la bascule reprend le contenu de la **préprod** (mesuré 06/10 — le texte visible des 6 pages clés est identique au caractère près entre prod et préprod). Ce manifeste est donc **exécutable en préprod seulement**, et les corrections y restent. Si la bascule recopiait la prod (changement de procédure), chaque ligne ci-dessous se rejoue à l'identique : aucune dépendance au contexte préprod.

**Norme typographique** — c'est la règle du projet, transposée des cartes du jeu au site : en français, espace **insécable** avant `; : ! ?`, majuscule aux noms propres, accents partout. **Elle s'applique sans arbitrage** : la typographie et l'orthographe se corrigent toujours, accents compris. Seules les questions de **fond** (contenu, liens morts, structure, mentions légales) remontent à l'owner — elles sont listées en fin de document.

**Source de chaque correction** : `addinette` = relevé d'Adeline sur #1781 · `ai-01` = vérification sur les deux sites · `po-2023` = relecture typographique complète.

**Carte de stockage** (re-mesurée 06/10 contre la base) : le contenu éditable vit dans trois magasins de la DB `ArgumentumGames` (`localhost\SQLEXPRESS`) —

| Magasin | Table | Ce qu'il porte | Type de colonne |
|---|---|---|---|
| **EAV** (2sxc) | `TsDynDataValue` | tout le contenu éditable des pages (pied de page, articles, fiches Amis, encarts) | `ntext` |
| **HtmlText** | `HtmlText` (`ModuleID-ItemID`) | les pages de texte long servies par le module Html | `ntext` |
| **NBrightBuy** | `NBrightBuy` (`ItemId`, `Lang`) | la fiche article de la boutique | `xml` |

Chaque ligne du manifeste donne **l'entité/la valeur à modifier** — le contrôle = diff du texte visible = lignes du manifeste exactement. Export avant modification (dump JSON) vers `Logs/`.

> ⚠️ **Traçabilité — incident de dump du 06/10 (constaté le 07/10).** Les **quatre** exports pris le 06/10 sont des **sorties console inutilisables**, vérifiées une par une : `grainL-eav-backup-20261006164900.json` et `grainA-entity10072-backup.json` portent l'entête sqlcmd `JSON_F52E2B61-…` suivi de lignes de 8 000 caractères d'espaces ; `N-e10080-V27402-backup-20261006163954.txt` et `N-nbuy-131-backup-20261006163925.xml` commencent par une colonne tronquée (`Val`, `X`) noyée dans le même remplissage. **L'état d'avant des grains du 06/10 n'est donc pas rejouable depuis ces fichiers** — les corrections elles-mêmes sont établies par le contrôle sur le texte servi et par les commentaires de #1781. Les exports du **07/10** (passe L-bis) sont, eux, complets et exploités ci-dessous. Les deux lignes `V23272`/`V23351` (« au sciences ») n'ont aucun dump : leur forme d'avant est connue par le commentaire #1781 du 06/10 22:57, pas par un fichier.

---

## État d'ensemble — ce qui est posé

| Grain | Objet | Posé le | Cible | Contrôle servi |
|---|---|---|---|---|
| **B** | 18 corrections de contenu relevées par Adeline + 3 coquilles adjacentes | 06/10 | 24 valeurs EAV (Accueil, Actus, Amis, pied de page) | ✅ |
| **L** | accents et ponctuation, 3 passes : 8 fautes + `au sciences` (06/10), puis 95 remplacements sur 32 lignes (07/10), puis 4 libellés de catégorie (07/10) | 06/10 et 07/10 | EAV + HtmlText (`/terms`) + NBrightBuy | ✅ |
| **H** | encart « Bientôt de retour » au-dessus de l'article de vente | 06/10 | E11897 / V27785 | ✅ |
| **I** | lien mairie 404 → copie Wayback | 06/10 | E10080 / V27402 | ✅ |
| **J** | logos des 3 fiches Amis en ligne, « en savoir plus » retirés | 06/10 | E10155, E10164, E10166 (`Details`) | ✅ |
| **K** | page de présentation de l'ontologie (le menu servait le fichier brut) | 06/10 | TabID 171 | ✅ |
| **A** | lien « Print and Play » réparé (ticket DNN illisible → lien direct) | 06/10 | E10072 / V23152 | ✅ |
| **M** | mentions légales de `/terms` rétablies depuis la prod | 06/10 | `GlobalResources.fr-FR.resx` | ✅ |
| **N** | `## En Rupture ##` brut retiré + point sorti du lien Fanny Bénard | 06/10 | NBrightBuy 131 · E10080 / V27402 | ✅ |
| **O** | cache des `.html` aligné sur la prod (1 an → 10 min) | 07/10 | `web.config` (17 blocs) + TabID 171 | ✅ |
| **P** | les 5 pages de règles cessent d'afficher une erreur 2sxc (`Kit.Convert`) + 8 coquilles du texte de règles | 07/10 | `_RulesExplorer_RuleDetail.cshtml` (webroot + PR #1791) · 6 valeurs EAV | ✅ |

---

## Grain B — les 18 corrections relevées par Adeline (posées 06/10)

### Page Accueil (`/`)

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

### Page Actus (`/Actus`) — les corrections 14-16 vivent dans les **articles détail**, pas la liste

| # | Avant | Après | Source | Entité EAV (News, App 52) |
|---|---|---|---|---|
| 14 | `le plateforme DNN` | `la plateforme DNN` | addinette | E11897 `Content` — article « On peut commander le jeu… » |
| 15 | `belgique` | `Belgique` | addinette | E10206 `Content` — article « Argumentum aux REC 2022 » |
| 16 | `etats-unis` | `États-Unis` | addinette | E10206 `Content` — article « Argumentum aux REC 2022 » |

### Page Amis (`/Amis`)

| # | Avant | Après | Source | Entité EAV | Note |
|---|---|---|---|---|---|
| 17 | `Le Drenche est un journal visant à aider les citoyens à se forger leur opinion` | `… leur opinion.` | addinette | E10164 `Summary` (V23346, App 59 `Term`) | point final manquant — le titre `Drenche (le)` reste inchangé |

### Pied de page commun (toutes pages)

| # | Avant | Après | Source | Entité EAV | Note |
|---|---|---|---|---|---|
| 18 | `Retrouvez nous` | `Retrouvez-nous` | addinette | E10065 `Title` (V27676, App 33 `Content`) | = correction 9, même entité |

### Coquilles adjacentes — **posées** (même famille, même texte, relevées pendant la localisation)

| Avant | Après | Entité | Note |
|---|---|---|---|
| `l'intégralité notre notre présentation` | `l'intégralité de notre présentation` | E10081 `Content` | double « notre » + « de » manquant |
| `plusieurs notions suplémentaires` | `plusieurs notions supplémentaires` | E10081 `Content` | accent manquant |
| `You defendez l'esprit critique et souaitez` | `… souhaitez` | E10055 `Text` | coquille « souaitez » |

*Pages Règles, Argumentation et Téléchargements : aucune correction de contenu relevée (le corpus CSV Rules a été corrigé séparément par #1768/#1778 ; les 2 zip manquants sont restaurés, voir grain A).*

---

## Grain L — accents et ponctuation (posé 06/10 puis 07/10)

### Passe du 06/10 — 8 fautes + `au sciences`

Rapportée à Adeline le 06/10 : **8 fautes d'accent** (« élèves » ×3, « générale », « intéressez », « déjà », « même » ×2), **EAV seul** (HtmlText et NBrightBuy étaient propres). S'y ajoute `au sciences` → `aux sciences`, relevé par ai-01 le 06/10 22:57 et **posé** (V23351 « Les petits débrouillards… l'éducation populaire **aux** sciences. » ; V23272 Chiasma « …l'éducation **aux** sciences cognitives… »).

**Réserve de traçabilité** : ces deux dernières lignes n'ont **aucun dump** (les exports du 06/10 sont inutilisables, cf. avertissement en tête). La forme d'avant est établie par le commentaire #1781, la forme d'après par la base — la correction est vérifiée, mais pas rejouable depuis un fichier.

### Passe L-bis du 07/10 — méthode

Le correctif précédent cherchait **une liste de mots fixée à l'avance** : il ne pouvait trouver que les mots déjà connus. La passe L-bis repart d'un **dictionnaire français complet** (Lexique 3 / `fr_FR.dic`, 82 156 formes) : tout le texte servi est découpé, et **tout mot absent du dictionnaire est relu dans sa phrase**. Les mots anglais sont écartés par une liste dédiée (`words_alpha.txt`, 370 105 formes).

**Contrôle inverse** — la garantie que l'instrument n'est pas aveugle : l'outil devait retrouver **tous** les défauts déjà signalés. Résultat : **42 sur 42**. Sans ce contrôle, on ne saurait pas si l'outil voit ; c'est précisément ce qui manquait à la passe du 06/10.

**Décisions au cas par cas, jamais en bloc** : les formes ambiguës (accent ou non) sont tranchées par le contexte. Exemple écarté volontairement : `facilite` ×3 — les trois contextes servis sont **le verbe** (« il facilite leur mémorisation », « facilite leur reconnaissance », « facilite la prise de connaissance ») : **pas d'accent**. Exemple corrigé : `arme` → `armé` dans « être armé » (participe, pas le nom).

### Résidu fermé le 07/10 — les libellés de catégorie

Cinq **libellés de catégorie** sont rendus sur l'accueil et la page Actus, avec leur lien de filtre. Deux d'entre eux étaient des mots français sans accent. Ils vivent dans l'EAV (App 52 News, entités de catégorie), attributs **`Name`** (affichage) et **`PageTitle`** (titre d'onglet) — le troisième, **`UrlKey`**, est le **slug d'URL** et n'a **pas** été touché.

| Id | Entité | Attribut | Avant | Après |
|---|---|---|---|---|
| V23207 | 10078 | `Name` | `Video` | `Vidéo` |
| V23208 | 10078 | `PageTitle` | `Video` | `Vidéo` |
| V27735 | 11896 | `Name` | `Evolution` | `Évolution` |
| V27736 | 11896 | `PageTitle` | `Evolution` | `Évolution` |
| V23209 · V27737 | — | **`UrlKey`** | `video` · `evolution` | **inchangés** (le filtre `?category=video` continue de fonctionner) |

**Pourquoi le slug n'est pas touché** : le lien servi est `?category=video` en minuscules alors que le libellé est `Video` — le filtre porte donc sur `UrlKey`, pas sur `Name`. Accentuer la clé aurait cassé les liens existants sans rien apporter.

**Contrôle** : les 5 libellés servis sont désormais `À l'affiche`, `Sorties`, `Évènements`, `Vidéo`, `Évolution` · **0** libellé nu (`>Video<`, `>Evolution<`) dans le HTML servi · les deux filtres rendent **le même nombre d'articles qu'avant** (2 pour `video`, 1 pour `evolution`) · `UrlKey` relu en base, inchangé.

### Ce qui est posé — 95 remplacements sur 32 lignes, plus 4 libellés de catégorie

`~` = espace insécable. Tailles = caractères, avant → après.

| Magasin | Id | Taille | Remplacements |
|---|---|---:|---|
| EAV | 22833 | 26 → 26 | `A` → `À` (×2) |
| EAV | 22861 | 352 → 378 | `l'eloquence` → `l'éloquence` · `vertueuse?` → `vertueuse~?` · `etre arme` → `être armé` |
| EAV | 22872 | 558 → 642 | `generations` → `générations` (×2) · `aquérir` → `acquérir` · `democratiques/citoyens` → `démocratiques/citoyens` · `qualite` → `qualité` · `societales` → `sociétales` · `societe` → `société` · `familisariser` → `familiariser` · `differents` → `différents` · `difficultes` → `difficultés` · `debat` → `débat` |
| EAV | 22881 | 558 → 642 | *(mêmes 11 remplacements que 22872 — entité jumelle)* |
| EAV | 22898 | 347 → 375 | `opportunites` → `opportunités` · `cle` → `clé` · `élèves?` → `élèves~?` · `l'occurence` → `l'occurrence` · `saynetes` → `saynètes` |
| EAV | 22914 | 344 → 351 | `deceler` → `déceler` |
| EAV | 22941 | 419 → 460 | `rhetorique` → `rhétorique` · `debats` → `débats` · `lies` → `liés` · `pretexte` → `prétexte` · `reconnaitre` → `reconnaître` · `pieger` → `piéger` |
| EAV | 22969 | 339 → 359 | `interpretations` → `interprétations` · `egard?` → `égard~?` |
| EAV | 22982 | 422 → 456 | `complementaire` → `complémentaire` · `presente` → `présente` · `memorisation` → `mémorisation` · `mecanique` → `mécanique` |
| EAV | 22995 | 453 → 481 | `qualite` → `qualité` · `decoulent` → `découlent` · `l'agentivite` → `l'agentivité` · `echanges` → `échanges` |
| EAV | 23008 | 226 → 248 | `etudier` → `étudier` · `socilogie` → `sociologie` · `systematique` → `systématique` · `rhetorique` → `rhétorique` |
| EAV | 23034 | 630 → 690 | `d'hygiene` → `d'hygiène` · `s'etendent` → `s'étendent` · `societe` → `société` · `cherchiez:` → `cherchiez~:` · `etant drole` → `étant drôle` · `possible;` → `possible~;` · `demystifie` → `démystifie` |
| EAV | 23047 | 370 → 384 | `langagiere` → `langagière` · `nefastes` → `néfastes` |
| EAV | 23071 | 10 → 10 | `Evènements` → `Évènements` |
| EAV | 23072 | 10 → 10 | `Evènements` → `Évènements` |
| EAV | 23179 | 470 → 469 | `noeud` → `nœud` |
| EAV | 23190 | 317 → 317 | `A` → `À` |
| EAV | 23377 | 11 → 11 | `A` → `À` |
| EAV | 23431 | 5 752 → 5 758 | `données;` → `données~;` |
| EAV | 23452 | 13 303 → 13 309 | `données;` → `données~;` |
| EAV | 23922 | 204 → 204 | `A` → `À` |
| EAV | 27116 | 253 → 254 | `dArgumentum` → `d'Argumentum` |
| EAV | 27140 | 57 → 58 | `A` → `À` · `Argumentum?` → `Argumentum~?` |
| EAV | 27511 | 1 002 → 1 085 | `decouvrir` → `découvrir` · `concept/theme` → `concept/thème` · `different` → `différent` · `saynetes` → `saynètes` · `pretexte` → `prétexte` · `role` → `rôle` · `simplifiee` → `simplifiée` · `caractere` → `caractère` · `tres` → `très` · `theme,` → `thème,` · `cles` → `clés` · `ecouter` → `écouter` |
| EAV | 27624 | 178 → 185 | `scenario` → `scénario` |
| EAV | 27729 | 582 → 587 | `l'heuvre` → `l'œuvre` |
| EAV | 27815 | 795 → 801 | `baratineurs!` → `baratineurs~!` |
| EAV | 27942 | 4 478 → 4 490 | `suivantes:` → `suivantes~:` · `formats:` → `formats~:` |
| HtmlText | 599-6 | 15 647 → 15 653 | `données;` → `données~;` |
| HtmlText | 599-7 | 6 225 → 6 231 | `données;` → `données~;` |
| HtmlText | 599-8 | 15 578 → 15 584 | `données;` → `données~;` |
| NBrightBuy | 131 | 4 589 → 4 589 | `scenarii` → `scénarii` (×2) |

**Détail des magasins** : 28 lignes `TsDynDataValue` (EAV) · 3 lignes `HtmlText` (les trois pages de texte long, dont `/terms`) · 1 ligne `NBrightBuy` (`ItemId=131`, l'article de la boutique).

### Vérification (sur le texte **servi**, pas sur la base)

| Contrôle | Attendu | Mesuré |
|---|---|---|
| formes fautives encore présentes dans le texte servi | 0 | **0** (53 formes testées) |
| formes corrigées présentes | toutes | **toutes** |
| insécables avant `? ! ; :` | 15 | **15 posés** (le texte servi en porte **16** sites : le 16e, `? Argumentum` d'E10050, préexistait) |
| relevé des mots à relire | baisse | **439 → 393**, aucun mot nouveau apparu |

### Ce qui reste volontairement hors périmètre

1. **Six pages affichent « Success! If you are not registered yet… »** — message **en anglais** du module d'inscription (EAV), pas du texte français : la règle d'espace insécable ne s'y applique pas. Traduire est une décision de contenu.
2. **La page « Carte mentale fallacieuse »** reprend le contenu des cartes du jeu : ses accents se corrigent dans le **corpus des cartes**, pas dans le site — et ce corpus est aussi imprimé, donc cela revient à l'owner.
3. **La page Ontologie** affiche les noms des sophismes tels qu'ils figurent dans le corpus, même remarque.
4. Les jetons tronqués (`tudiant`, `chantillon`, `cossais`, `quivalence`…) sur la page Ontologie sont un **artefact de l'instrument de relecture** (la classe de caractères exclut les majuscules accentuées, donc « Étudiant » se découpe en « tudiant ») : ces pages sont correctes.

---

## Grain P — les 5 pages de règles (07/10) : l'angle mort de la passe L, et ce qu'il cachait

La passe L n'avait jamais pu scanner les pages `/Règles/details/<jeu>/mid/602` : elles servaient une **page d'erreur**, pas les règles. Un scan d'accents sur ces pages était donc impossible — et c'est ce qui a conduit à les ranger trop vite dans « hors périmètre ».

### P.1 — Les 5 pages ne compilaient plus (cause racine)

`_RulesExplorer_RuleDetail.cshtml` a été migré vers `@inherits Custom.Hybrid.Razor14` par **#418** (02/06/2026, « upgrade 2sxc 15→21.07 + Razor14 migration ») mais a gardé son appel `Convert.Json.ToJson(...)` de 2023 (`73b92cfed`).

| Classe de base | Expose |
|---|---|
| `Custom.Hybrid.Razor12` | `prop Convert : IConvertService` ← ce sur quoi le code de 2023 s'appuyait |
| `Custom.Hybrid.Razor14` | `prop Kit : ServiceKit14` — **aucune propriété `Convert`** |

Sur Razor14, l'identifiant nu `Convert` retombe donc sur `System.Convert`, qui n'a pas de membre `Json` :

```text
_RulesExplorer_RuleDetail.cshtml(98): error CS0117: 'Convert' does not contain a definition for 'Json'
```

La vue ne compile pas → DNN rend « Error Showing Content - please login as admin for details. » La page d'atterrissage `/Règles` n'a jamais été touchée : sa vue sœur `_RulesExplorer_RuleList.cshtml` ne fait aucun appel JSON.

**Correctif** : `Convert.Json.ToJson(...)` → `Kit.Convert.Json.ToJson(...)` — la traduction littérale que la migration devait faire (`ServiceKit14.Convert : IConvertService` → `.Json : IJsonService` → `ToJson(object)`). Un identifiant ajouté, aucun autre octet de la vue modifié. Posé au webroot (sauvegarde + empreinte) **et** au dépôt ([PR #1791](https://github.com/ArgumentumGames/Argumentum/pull/1791)) pour qu'il ne soit pas perdu à la prochaine mise en service.

**Ce n'est pas la récupération du 06/10.** Le fichier avait bien été réécrit pendant l'incident 4 (mtime 01:28), ce qui a fait croire à une régression fraîche. Le journal DNN dit le contraire : l'erreur de compilation est journalisée **depuis le 09/09 19:23:55** (même événement que l'entrée IIS du 09/09 17:23:55 UTC — le journal DNN est en heure locale, IIS en UTC), et le défaut est au niveau du source depuis #418. Une recompilation ne l'a que **révélé**. Le mention « runtime pending » des PR de migration Razor14 (#418, #596) est exactement cette faille : rien ne pointait vers ces pages, donc leur validation à l'exécution n'a jamais eu lieu.

### P.2 — Les coquilles que ces pages cachaient

Une fois les pages rendues, les 5 fautes que #1502 (T7) listait ont toutes été retrouvées — et le contexte en a fait apparaître 2 de plus. Corrigées par remplacement chirurgical (garde : identifiant **et** présence de la forme ancienne ; sauvegarde de 7 valeurs).

| # | Valeur | Entité | Attribut | Avant (tel que stocké) | Après | Page servie |
|---|---|---|---|---|---|---|
| 1 | V27687 | 11378 L'école des menteurs | `Content` | `en y posant **sont** petit objet` | `son petit objet` | école |
| 2 | V27687 | 11378 | `Content` | `les jur&eacute;s **plebicit&eacute;s**` | `pl&eacute;biscit&eacute;s` (×10) | école |
| 3 | V27688 | 11378 | `Installation` | `7 minutes **environs**` | `environ` | école |
| 4 | V27541 | 11380 Le Bingo mixologie | `Content` | `Si à **l'issu** d'un premier décompte` | `l'issue` | bingo |
| 5 | V27686 | 11378 | `Variants` | `la partie reprend **son cour**.` | `son cours.` | école |
| 6 | V27545 | 11380 | `Material` | `l'objet d'un **revisionage**` | `revisionnage` | bingo |
| 7 | V27568 | 11387 Le dernier beau parleur | `Content` | `En **comman&ccedil;ant** par le voisin` | `commen&ccedil;ant` | beau parleur |
| 8 | V27586 | 11389 La parlote coinchée | `Content` | `En **comman&ccedil;ant** par le voisin` | `commen&ccedil;ant` | parlote |

Deux remarques de méthode :

- **La valeur V27687 contient la même phrase 10 fois** : c'est un export Google-Sheets qui duplique le texte (bloc visible + `data-sheets-value` + `data-sheets-formula`). Les 10 occurrences sont la même faute ; le remplacement les corrige toutes, ce qui est le comportement voulu. Une garde « exactement 1 occurrence » aurait bloqué à tort — la garde compare donc au compte **attendu**.
- **La coquille `à l'issu` n'était pas là où l'entité le suggérait** : elle vit dans l'entité du **Bingo**, pas dans celle de l'école, et elle est stockée avec une apostrophe **droite littérale** (`l'issu`), pas `&rsquo;`. La chercher avec l'entité `&rsquo;` rendait 0 — un faux « absent ».

**Vérification (texte servi, page par page)** : les 7 graphies fautives ont disparu, les 7 formes corrigées sont servies sur la bonne page.

### P.3 — Les 5 mêmes coquilles sont dans le gabarit CardPen — non touché (gel)

Le gabarit `Cards/Rules/Argumentum_Rules_fr.json` porte **5 des 7** coquilles (`sont petit objet`, `revisionage`, `son cour`, `à l'issu`, `commançant`). Le CSV des cartes (`Argumentum Rules - Cards.csv`), lui, est **propre** pour ces fautes.

Le gabarit étant **sous gel** (⛔ PR CSV/gabarit/moteur CardPen), il n'a pas été touché : corriger le site sans le gabarit **crée une divergence** entre la page servie et les cartes imprimées. C'est un choix d'owner, car il implique une régénération. Signalé ci-dessous.

### P.4 — Ce que l'instrument ne couvrait pas (à savoir avant de conclure)

Le balayage par lexique de la passe L s'appuie sur `fr.dic`, qui est **compressé par affixes** : il contient les **radicaux**, pas les formes fléchies (`joueurs`, `sont`, `vont` sont absents ; `école`, `être` sont présents). Le « 82 156 formes » annoncé en passe L-bis est donc un compte de **radicaux**, et le canal « mot absent du dictionnaire » sur-détecte massivement (`joueurs`, `autres`, `appris`…). Les conclusions de la passe L n'en dépendent pas — elles sont prouvées par le différentiel avant/après de chaque remplacement et par le contrôle du texte servi — mais **aucune conclusion de type « 0 faute restante » ne peut s'appuyer sur ce canal**. Les 8 corrections ci-dessus viennent d'une liste de fautes **déjà relevées** (#1502 T7) et d'une recherche par motif, pas du dictionnaire.

---

## Grains H / I / J / K — posés

| Grain | Objet | Posé le | Cible | Contrôle servi |
|---|---|---|---|---|
| **H** | Encart « Bientôt de retour » (fond translucide `rgba(220,15,10,0.12)`) **au-dessus** de l'article qui annonce la boutique « enfin opérationnelle » alors que la page Acheter dit « Pas de stock disponible ». Le texte d'origine est intégralement conservé en dessous. | 06/10 | E11897 / V27785 (1 626 → 1 891 car.) | encart ×1, mention ×1, fond ×1 |
| **I** | Lien mort vers la mairie du 18e (HTTP 404) → copie **Wayback du 15/12/2025** (`20251215104931`, 313 Ko, nom + fonction présents). Première branche de la règle owner (« Wayback si copie existe »). | 06/10 | E10080 / V27402 | lien mort ×0, lien Wayback ×1 |
| **J** | Les **3 fiches Amis** (Chiasma, Drenche, Petits Débrouillards) portent désormais leur **logo** directement sur la page ; les 3 liens « en savoir plus » sont retirés. Les logos étaient déjà dans l'attribut `Details`, il suffisait de les rendre. | 06/10 | E10155, E10164, E10166 `Details` (App 59 `Term`, module 597) | 3 logos servis, 0 « en savoir plus » |
| **K** | L'entrée de menu « Ontologie fallacieuse » **n'était pas une page** : le champ `Url` de l'onglet pointait sur le **fichier brut** (5 993 701 o d'OWL/XML). Une page de présentation en français est créée et prend sa place ; l'onglet voisin (170) suivait déjà cette convention (`/fallacies_fr.html`). | 06/10 | TabID 171 → `/ontologie_fr.html` | page servie 200 · **lien de menu servi** `/ontologie_fr.html?v=2.0.0` (grain O) |

**Chiffres de l'ontologie (grain F, corrigés le 06/10)** — mesurés sur le fichier **servi**, en **entités distinctes** (déclarations à IRI unique) : **1 495 classes · 10 propriétés d'objet · 719 individus · 1 407 liens `skos:broader`**. Une version antérieure annonçait « 1 882 / 928 / 1 760 » : c'étaient des **occurrences de balises**, où une classe citée trois fois comptait trois. Correction reportée dans le dépôt (#1786) et dans le webroot préprod.

⚠️ **H est un encart d'état temporaire** : à retirer à la réouverture de la boutique.

---

## Grains A / M / N — posés

| Grain | Objet | Posé le | Cible | Contrôle servi |
|---|---|---|---|---|
| **A** | Le lien « Print & Play » passait par un **ticket DNN illisible** (log : `FileTicket … generated with DES algorithm → FileID=-1`) : il rendait 404 alors que les fichiers étaient bien sur le disque. Remplacé par le **lien direct** `/Portals/1/Downloads/Argumentum_Print%26Play.zip` (le `&` doit être écrit `%26`). Les 4 autres zip gardent leur jeton `?ver=`, c'est la forme normale du portail. | 06/10 | E10072 / V23152 | 200 · `Content-Length = 93 587 869` · sha256 identique au fichier prod (`47708620…`, mesuré par ai-01) · **0** `LinkClick.aspx` restant sur `/Téléchargements` |
| **M** | `/terms` servait un texte **générique de site web**, sans mentions légales. Rétablies **mot pour mot** depuis la prod (SAS, hébergeur, SIREN). | 06/10 | `GlobalResources.fr-FR.resx`, clé `MESSAGE_PORTAL_TERMS.Text` (fichier du site, pas EAV) | 15 655 caractères servis, identiques à la prod |
| **N** | Deux défauts d'affichage : les marqueurs markdown bruts `## En Rupture ##` s'affichaient tels quels, et le point final de Fanny Bénard était **à l'intérieur** du lien. Les marqueurs sont retirés ; le point est sorti (`…citoyenne</a>.`). | 06/10 | NBrightBuy 131 · E10080 / V27402 | `##` ×0 · `citoyenne</a>.` ×1 |

---

## Grain C — liens « Retrouvez-nous », contact, newsletter : **mesuré, aucun envoi**

| Lien du pied de page | Statut |
|---|---|
| GitHub `ArgumentumGames/Argumentum` | 200 |
| Twitter `argumenteam` | 200 |
| Facebook `argumenteam` | 200 |
| Youtube (chaîne) | 200 |
| Twitch `argumenteam` | **200 en GET** |

> ⚠️ Twitch renvoie **405 à une requête HEAD** et **200 à une requête GET**. Une sonde HEAD l'aurait déclaré cassé à tort : c'est une limite de l'instrument, pas du lien.

**Newsletter** : formulaire hébergé **Brevo/Sendinblue** (`<form id="sib-form" … action="https://d426a943.sibforms.com/serve/MUIE…">`), endpoint joignable (200). Champs `EMAIL`, `email_address_check`, `locale`, `OPT_IN`, `ScrollTop`, `Terms-600`. **Aucun envoi effectué** — un vrai test d'inscription est le seul probant, il reste proposé à Adeline (marqué « TEST »).

**Contact** : ce n'est pas une page (`/Contact`, `/Contactez-nous` → 404) mais un **accordéon** de l'accueil (`data-accordion-parent="10182"`), champs `Subject`, `SenderName`, `SenderMail` + case « J'accepte les termes et conditions » obligatoire. Pas de balise `<form>` : soumission en JavaScript par un module Form 2sxc (`…/app/auto/live/api/Form/ProcessForm?workflowId=ContactDefault`). **Non sollicité.**

**`/terms`** : depuis M, la page porte les mentions légales de la prod. La réserve éditoriale reste ouverte pour le reste du texte : c'est un **modèle générique**, sans vocabulaire de **vente** (`vente`, `livraison`, `prix`, `commande`, `remboursement` : 0 occurrence). Décision éditoriale/juridique, pas une correction technique.

---

## Grain D — page « Acheter le jeu » : **mesuré**

5 chargements par site, URL cache-bustée :

| Site | min | médiane | max | Codes |
|---|---:|---:|---:|---|
| **préprod** | 52 ms | **60 ms** | 176 ms | 200 ×5 |
| **prod** | 170 ms | **176 ms** | 272 ms | 200 ×5 |

Les deux sites répondent, la préprod est ~3× plus rapide. **Sur 10 chargements, aucun état grisé n'a été reproduit.** Ce que la mesure ne couvre pas : elle mesure la réponse serveur du HTML, pas l'état « utilisable » après le JavaScript du module e-commerce NBrightBuy (qui peuple la liste en AJAX) — ce dernier se produirait **après** la mesure, et ne s'est pas produit.

**Ce qui explique le ressenti** : la page affiche **« Pas de stock disponible »** — elle fonctionne et le dit. Le constat annexe relevé pendant la mesure (`## En Rupture ##` affiché brut) a depuis été corrigé (grain N).

---

## Grain E — source pour le lien « code de conduite intellectuel » : **recommandation, non posée**

Sur l'article « Interview des co-créateurs », les liens « classification de McCandless » et « code de conduite intellectuel » pointent vers la **même URL**. Le texte visé est le *Code of Intellectual Conduct* de **T. Edward Damer** (*Attacking Faulty Reasoning*, 6ᵉ éd. 2009, p. 7-8).

| URL candidate | HTTP | Contient le code ? | Verdict |
|---|---:|---|---|
| `en.wikipedia.org/wiki/Attacking_Faulty_Reasoning` | 200 | non | référence bibliographique |
| `joshuapsteele.com/damers-code-of-intellectual-conduct/` | 200 | **oui** (61 `<li>`) | **retenue** |
| `limbicnutrition.com/blog/damers-code-of-intellectual-conduct/` | 200 | **non** | **à écarter** — l'URL survit mais ne porte plus le texte |

> ⚠️ Mesuré, pas supposé : `limbicnutrition.com` **répond 200** tout en n'étant plus la source. Un simple test de statut l'aurait gardé à tort.

**Recommandation** : retenir `joshuapsteele.com` **et** citer l'ouvrage en clair dans la phrase (« le code de conduite intellectuel de T. Edward Damer ») plutôt que de faire reposer l'information sur une URL tierce — c'est un blog personnel, sa pérennité n'est pas garantie. **Le lien n'est pas modifié** : la décision revient à l'owner.

---

## Ce qui reste ouvert (owner)

| Objet | État | Proposition |
|---|---|---|
| Lien « code de conduite intellectuel » (grain E) | ouvert | Remplacer l'URL par `joshuapsteele.com` + nommer Damer dans la phrase |
| Texte de `/terms` au-delà des mentions légales | ouvert | Modèle générique sans vocabulaire de vente — décision éditoriale/juridique |
| Test réel d'inscription newsletter | ouvert | Un envoi « TEST » par Adeline est le seul contrôle probant |
| Encart « Bientôt de retour » (grain H) | **posé, temporaire** | À retirer à la réouverture de la boutique |
| Visualiseur WebVOWL derrière la page d'ontologie (grain K, option) | non posé | Possible (MIT, 1,24 Mo + 3,73 Mo pour le JSON) mais rendrait une « pelote » sur ~1 500 classes ; à charger **derrière un clic**, jamais au chargement. Les cartes mentales SVG déjà générées couvrent le besoin |
| Import de la passe d'accents dans le **corpus des cartes** (page Carte mentale, page Ontologie) | ouvert | Les accents de ces deux pages viennent du corpus imprimé : la correction touche les CSV et une régénération |
| **Les 5 mêmes coquilles dans le gabarit CardPen** (`Cards/Rules/Argumentum_Rules_fr.json` : `sont petit objet`, `revisionage`, `son cour`, `à l'issu`, `commançant`) | **non posé — gel du gabarit** | Le site a été corrigé (grain P), ce qui **crée une divergence** avec les cartes imprimées tant que le gabarit n'est pas aligné. Corriger le gabarit implique une régénération, donc un arbitrage owner. Le CSV des cartes, lui, est déjà propre sur ces 5 fautes |

---

*v3.1 (07/10, po-2023) : ajout du **grain P** — les 5 pages de règles ne compilaient plus (`Convert.Json` sur Razor14, défaut source depuis #418, journalisé depuis le 09/09, corrigé au webroot et au dépôt par PR #1791) et 8 coquilles du texte de règles, retrouvées une fois les pages rendues. Consigne aussi la limite de l'instrument de la passe L (`fr.dic` = radicaux seuls, canal « absent du dictionnaire » non concluant) et le signalement des 5 mêmes coquilles dans le gabarit CardPen sous gel.*

*v3 (07/10, po-2023) : le manifeste devient la **trace de tout ce qui est posé** — grains B, L (3 passes : 95 remplacements détaillés ligne par ligne + 4 libellés de catégorie), H, I, J, K, A, M, N et O, chacun avec sa date et son entité. Les mentions « décision owner » sur la typographie sont retirées (la règle s'applique sans arbitrage) ; les grains C, D et E renvoient à leurs mesures ; l'incident de dump du 06/10 est consigné. v2 (06/10) : carte de stockage EAV (entité + attribut par correction), périmètre corrigé (11-16 vivent dans les articles Actus détail ; 4 = seul le É manque ; 12 à re-valider pour l'accord de l'article), et signalement de 3 coquilles adjacentes.*
