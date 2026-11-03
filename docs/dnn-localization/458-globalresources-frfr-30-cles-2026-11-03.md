# #458 — `GlobalResources` fr-FR : les 30 clés ajoutées au neutre par DNN 10.4.1

**Date :** 03/11/2026 · **Base :** `d8c595ac` · **Grain :** pool #458 c.6596594457, po-2024 item 1
**Périmètre :** 1 resx (`DNNPlatform/App_GlobalResources/GlobalResources.fr-FR.resx`), 30 cellules ajoutées, 0 cellule existante touchée.

## Ce que la montée a ajouté au neutre

La montée 10.4.1 (02/11) a porté le neutre `App_GlobalResources/GlobalResources.resx` de 186 à 215 clés (dont une retirée, `ActivatingLicense.Text`). Les **30 clés neuves** se répartissent en trois familles, mesurées sur le blob :

| Famille | Clés | Exemples |
|---|---:|---|
| Rôles de plateforme | 18 | `Administrators.Role` / `.RoleDescription`, `Registered Users.*`, `Superuser.*`… |
| Gabarits de workflow de contenu | 10 | `Start/Complete/Discard` × `Workflow/State` × `Subject/Body` |
| Rappel de mot de passe administrateur | 2 | `EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_ADMINISTRATOR_{SUBJECT,BODY}` |

## Couverture des packs avant ce grain — mesurée

| Pack | Clés neuves présentes | Nature |
|---|---:|---|
| **es-ES** | **30** | paquet espagnol officiel aligné par la montée, **traductions réelles vérifiées** (pas des copies de l'anglais) |
| fr-FR, pt-BR, ru-RU, ar-SA, fa-IR, zh-CN | 0 | — |

`es-ES` est donc la référence de **structure** (jeux de clés, jetons), pas la source de la traduction : le fr-FR est traduit depuis le neutre.

## Visibilité publique — mesurée, pas supposée

Sonde anonyme de la préprod (`https://dnn.argumentum.myia.io`), cache-buster, pages servies en français (langue par défaut) :

| Page | HTTP | Rôle anglais rendu |
|---|---:|---|
| `/` (accueil, 200 après redirection, 85 374 o) | 200 | **aucun** |
| `/Login` (25 956 o) | 200 | **aucun** |
| `/Register` (29 633 o) | 200 | **aucun** |
| `/Argumentum/ctl/Terms/PortalId/1` (62 050 o) | 200 | **aucun** |

⇒ Sur la surface **anonyme**, aucune des 30 chaînes anglaises n'est servie. Les clés vivent dans les contextes **authentifiés** (profil, gestionnaire de rôles) et dans les **e-mails** (workflow de publication, rappel de mot de passe) — non sondables sans session. La langue par défaut reste néanmoins la première cible : c'est elle que voient les utilisateurs connectés et les destinataires des e-mails.

⚠️ Piège d'instrument rencontré pendant la sonde : le cache-buster `?cb=…` a été **avalé dans le chemin** par la réécriture d'URL DNN (`/Argumentum/cb/1664719058`). Les pages restent 200 et le contenu pertinent est intact, mais un buster en query string ne protège pas d'un cache ici.

## Traductions — terminologie reprise du pack, jamais inventée

| Terme neutre | Rendement fr-FR | Source dans le pack |
|---|---|---|
| `SuperUsers` | **Super-utilisateurs** | `//Host//SuperuserAccounts.String` |
| `Registered Users` | **Utilisateurs enregistrés** | « enregistrement » (6 occurrences du pack) |
| `Password Reset Token` | **jeton de réinitialisation du mot de passe** | frères `EMAIL_PASSWORD_REMINDER_*` |
| `pending approval` | **en attente d'approbation** | frère `…USER_ISNOT_APPROVED_BODY.Text` |
| `Everyone` (`All Users.Role`) | **Tout le monde** | usage DNN standard |

Le corps e-mail est un gabarit multi-lignes : la forme du neutre est **mirroirée** (ligne vide d'ouverture, 4 espaces finales) pour que le rendu HTML/e-mail reste identique.

## Chirurgie et contrôles

- Insertion de 30 blocs `<data>` avant le trailer `<!----></root>` du pack, style du fichier respecté (`<value xml:space="preserve">` pour les cellules simples, attribut sur `<data>` pour le corps multi-lignes).
- **Delta d'octets asserté AVANT écriture** : 6 353 attendus = 6 353 réels. Backup pré-écriture dans `.tmp/tick98/`.
- Après écriture : 106 724 octets, **BOM UTF-8 présente**, **0 CR** (LF seuls), XML valide, **279** éléments `<data>` = 249 historiques + 30.

## Garde `DnnGlobalResourcesFrFrGuardTests` (5 tests, lecture `HEAD:`)

1. `TheThirtyKeysExistInTheNeutral` — **anti-vacuité** : les 30 clés existent vraiment au neutre (une liste périmée rendrait les autres verts à vide) ;
2. `TheThirtyKeysAreDeliveredInFrFr` — les 30 sont livrées et non vides ;
3. `NoFrFrCellIsTheEnglishCopy` — aucune cellule fr-FR identique à l'anglais (aucun homographe légitime dans ce lot) ;
4. `PlaceholderParityWithTheNeutral` — multiset des jetons `{N}` et `[Xxx:Yyy]` identique au neutre (tolérant aux espaces internes, le pack écrivant parfois `[User: DisplayName]`) ;
5. `FrFrKeepsItsTransportAndDidNotLoseKeys` — BOM, LF seuls, et compte épinglé à **279** (le lot ajoute, il ne remplace pas).

**Banc rouge-avant / vert-après, mesuré** : sur l'arbre pristine (`HEAD = d8c595ac`) la garde rend **2 échecs nominatifs** (`TheThirtyKeysAreDeliveredInFrFr`, `FrFrKeepsItsTransportAndDidNotLoseKeys`) et 3 verts ; après le commit du lot, 5/5.

⚠️ Piège d'instrument corrigé en route : `XDocument.Parse` sur la chaîne décodée d'un blob **BOMé** lève `Data at the root level is invalid. Line 1, position 1` (U+FEFF en position 1) — l'extracteur retire le BOM explicitement. Les gardes frères y échappent parce qu'elles passent par `XDocument.Load(fichier)`, qui détecte le BOM lui-même.

## Reste à faire (hors périmètre de ce grain)

- Les 6 autres packs (pt-BR, ru-RU, ar-SA, fa-IR, zh-CN) portent les mêmes 30 lacunes — grain par langue, fr-FR d'abord par décision du pool.
- 4ᵉ phrase de `EMAIL_PASSWORD_REMINDER_USER_ISNOT_APPROVED_BODY` en ar/fa/zh (item 2 du pool), avec retrait de la déclaration de dérive.
