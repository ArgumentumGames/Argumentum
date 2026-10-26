# Liste des clés « chrome minimal » zh / ar / fa — 13 clés, 4 fichiers, 39 triples

**Date :** 2026-10-25 · **Lane :** po-2023 · **Pool :** #458 · **Base :** `origin/master` `029d3ae2`
**Nature :** constat, **lecture seule** (GET HTTP, fichiers, DB en lecture). Aucun resx créé, aucune écriture.
**Pour :** ai-01 — cette liste est l'entrée du dispatch de traduction à po-2024 (arbitrage 5, option a).

---

## 1. Méthode — les clés sont prouvées par le rendu, pas déduites du module

Un inventaire « d'après le code » se trompe (deux fichiers portent la même valeur anglaise). Protocole :

1. page servie (`/ar/Login`, `/fa/Login`, `/zh/Login`) → **chaînes visibles** extraites du HTML **décodé** (38 chaînes uniques) ;
2. chaque chaîne remontée à sa **clé** par égalité exacte avec la valeur d'un resx de **base** (152 fichiers de base indexés : `App_GlobalResources` + `DesktopModules/**/App_LocalResources`) ;
3. **preuve du porteur** : le fichier candidat doit porter la clé dans les cultures qui **rendent déjà** (fr/es/pt/ru), avec la valeur réellement servie. Exemple : `ControlTitle_.Text` → fr « Identification », ru « Вход » = les `<title>` mesurés.

Les 12 chaînes anglaises visibles sur `/ar/Login` se répartissent entre **4 fichiers** ; les autres chaînes de la page sont déjà arabes (rail skin) ou non-resx.

## 2. Tier 1 — les 13 clés à créer (39 triples fichier × clé × culture)

Chemins relatifs à `DNNPlatform/`. Pour chaque fichier, les cultures fr-FR/es-ES/pt-BR/ru-RU **existent** (sauf mention) ; **ar-SA, fa-IR, zh-CN manquent partout** ⇒ repli invariant = **anglais** servi, pas français.

| # | Fichier porteur | Clé | Valeur base (EN) servie | Ce que l'utilisateur voit | fr (preuve) |
| ---: | --- | --- | --- | --- | --- |
| 1 | `admin\Skins\App_LocalResources\Login.ascx.resx` | `Login.Text` | Login | **lien de connexion** (chaque page, via `<dnn:login>` de `theme-body.ascx:153`) | Connexion |
| 2 | idem | `Logout.Text` | Logout | lien de déconnexion (utilisateur connecté) | Déconnexion |
| 3 | `DesktopModules\Admin\Authentication\App_LocalResources\Login.ascx.resx` | `ControlTitle_.Text` | User Log In | **titre de la page /Login** (`<title>`, `h1`) | Identification |
| 4 | idem | `ControlTitle_login.Text` | User Log In | même titre, variante « login » (mode) | Identification |
| 5 | idem | `ControlTitle_signin.Text` | User Log In | même titre, variante « signin » | Identification |
| 6 | `DesktopModules\AuthenticationServices\DNN\App_LocalResources\Login.ascx.resx` | `cmdLogin.Text` | Login | bouton de soumission du formulaire | Connexion |
| 7 | idem | `Email.Text` | Email Address: | libellé du champ identifiant | Adresse de Courriel : |
| 8 | idem | `cmdPassword.Text` | Reset Password | lien « mot de passe oublié » | Réinitialiser le mot de passe |
| 9 | idem | `cmdRegister.Text` | Register | lien d'inscription | Inscription |
| 10 | idem | `Remember.Text` | Remember Login | case « se souvenir de moi » | Reconnexion automatique |
| 11 | `App_GlobalResources\SharedResources.resx` | `Password.Text` | Password: | libellé du champ mot de passe | Mot de passe : |
| 12 | idem | `cmdCancel.Text` | Cancel | bouton annuler du formulaire | Annuler |
| 13 | idem | `Home.Text` | Home | **fil d'Ariane** de la page (skin) | Accueil |

**39 triples** = 13 clés × 3 cultures. Un fichier de culture **partiel** est la norme DNN (les fichiers de skin ne portent que 13 clés) : le repli se fait **clé par clé** vers la base. Aucun pack complet n'est nécessaire.

**Déploiement** : resx ⇒ **restart app-domain** (~33 s), quatrième magasin déjà connu ([[reference-dnn-content-eav-storage-edit]]).

## 3. Tier 2 — lacunes **préexistantes**, à ne pas charger à ar/fa/zh

Mesuré : ces deux familles sont en anglais **sur des cultures qui rendent déjà** — c'est un défaut d'origine, pas un effet du pack manquant.

| Chaîne | Porteur | Cultures qui l'ont | Servie en anglais sur |
| --- | --- | --- | --- |
| `Skip to main content` (`SkipLink.MainContent`) | `Portals\_default\Skins\2shineBS5\App_LocalResources\2shine.resx` (base) | **aucune** culture | **fr aussi** (mesuré `/`, `/ar`, `/ru`) |
| `Login with Facebook` / `Sign in with Google` / `Sign In with Microsoft` | `DesktopModules\AuthenticationServices\{Facebook,Google,Live}\App_LocalResources\Login.ascx.resx` | **fr, es seulement** | **ru et pt mesurés** (`/ru/Login`, `/pt/Login`) |

⇒ Si ces 4 clés entrent dans le périmètre « chrome », elles se traitent en **grain séparé** et valent aussi pour fr/ru/pt — les mélanger à la liste ar/fa/zh ferait porter à po-2024 un défaut qui n'est pas le leur.

## 4. Tier 3 — ce qui reste français sur `/ar` et n'est **pas du resx**

Balayage de la home `/ar` (chaînes latines) — trois porteurs distincts, **aucun** n'est un resx de chrome :

| Bloc visible (FR) | Porteur mesuré | Grain |
| --- | --- | --- |
| formulaire contact : `Sujet` · `Message` · `Nom` · `E-mail` · `J'accepte les termes et conditions.` · `Entrez tous les champs avant l'envoi` · `Envoyer` | **app 2sxc Mobius5** (classes `app-mobius5-*`), valeurs base françaises — les messages de validation sont **doublés** dans le rendu (FR puis EN : `Please fill in all required fields before sending.`) | injection EAV (même mécanique que app 35/52/59) — **à identifier comme graine** |
| newsletter : `Inscrivez-vous à notre newsletter pour suivre les actualités d'Argumentum.` · `S'INSCRIRE` · `Votre inscription est confirmée.` | **formulaire Brevo embarqué** (`<form id="sib-form">`, styles inline), HTML figé dans le contenu | contenu, pas resx |
| `Dernières nouvelles` | h1 `Content.Title` de l'app 52, **absent du pivot** | déjà dispatché po-2024 (statut) |

Ces trois blocs sont **plus visibles** qu'une page Login pour un visiteur de la home — mais ils ne se corrigent pas par un fichier de langue. À arbitrer séparément.

## 5. Ce que ça change pour le sélecteur 8 langues

- Le **menu, la méta, l'imprint et le rail** sont **déjà complets** pour ar/fa/zh : `2shine.{ar-SA,fa-IR,zh-CN}.resx` portent **13/13 clés**, 0 manquante vs `en-US` (mesuré, comparaison de jeux de clés).
- Le seul chaînon manquant côté resx pour qu'un visiteur ar/fa/zh ne tombe pas sur de l'anglais est donc **ce Tier 1 (13 clés)**.
- L'ajout `ar-SA:AR,fa-IR:FA` au sélecteur reste gâté sur (a) déployé + verdict ai-01 (arbitrage 6) — cette liste **ne débloque rien à elle seule**, elle rend (a) chiffrable : 13 chaînes à traduire par langue, pas 9,7 k.

## 6. Gates

❌ aucune écriture en base · ❌ aucun resx créé/modifié · ❌ aucun pack installé · prod non touchée · aucun verdict visuel signé (ai-01). Mesures : GET HTTP + fichiers + DB en lecture seule.
