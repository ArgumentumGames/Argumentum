# ⑨ Paquets de langue ar/fa pour le chrome de base — l'état mesuré

**Date :** 2026-10-25 · **Lane :** po-2023 · **Pool :** #458, item ⑨ · **Base :** `origin/master` `9d761f64`
**Nature :** constat, **lecture seule**. Aucune écriture en base, aucun resx créé ni modifié, aucun pack installé.

---

## 1. Ce qui est installé — zh/ar/fa ont zéro resx de chrome

Recensement de **tout** le webroot (`DNNPlatform/`), tous répertoires, hors rail skin `2shine.*` :

| Culture | Fichiers resx | Clés `<data>` | `Packages` |
| --- | ---: | ---: | --- |
| fr-FR | 189 | 9 672 | 17 packs `*_fr-FR v9.2.0` |
| es-ES | 191 | 9 692 | ~28 packs `*_es-ES v10.3.1` |
| pt-BR | 139 | 7 141 | 15 packs `*_pt-BR v9.13.0` |
| ru-RU | 166 | 7 006 | 1 pack maison `Argumentum.DNNCE.Core.ru-RU v0.1.0` |
| **zh-CN** | **0** | **0** | **aucun** |
| **ar-SA** | **0** | **0** | **aucun** |
| **fa-IR** | **0** | **0** | **aucun** |

`Languages` (hôte) porte bien les 8 rangées (ar id 7, fa id 8, zh id 6 — le `CultureName` mojibake de zh,
déjà connu) et `PortalLanguages` = 8/8 actives. Les langues **existent**, leurs ressources de chrome
**non** — le seul existant pour elles est le **rail skin** `2shine.{zh-CN,ar-SA,fa-IR}.resx` (libellés de
menu + méta, #1950/#1956), qui n'est pas le chrome DNN.

## 2. Ce qui est servi — le chrome tombe en **anglais**, pas en français

`/xx/Login` sur les 8 alias (25/10, HTML décodé) :

| Alias | `<title>` | Libellés du formulaire |
| --- | --- | --- |
| racine (fr) | Identification | Adresse de Courriel : · Mot de passe : · Reconnexion automatique |
| `/en` | User Log In | Email Address: · Password: · Remember Login *(invariant = correct pour en)* |
| `/es` | Iniciar sesión | Email: · Contraseña: · Recordar inicio de sesión |
| `/pt` | Iniciar sesión | Email: · **Contrasenha**¹ · Lembrar login |
| `/ru` | Вход | Эл. почта: · Пароль: · Запомнить меня |
| `/ar` | **User Log In** | **Email Address: · Password: · Remember Login** |
| `/fa` | **User Log In** | **Email Address: · Password: · Remember Login** |
| `/zh` | **User Log In** | **Email Address: · Password: · Remember Login** |

Sur la **page d'accueil** `/ar` : la recherche est bien arabe (`بحث` ×2, rail skin), les liens de pied de
page aussi (`إشعار قانوني`), mais l'entrée de connexion rend **« Login » ×5 en anglais**. Contrôle sur la
racine : le **même élément** (`dnn_DnnLogin_loginLink`) y rend **« Connexion »** — l'élément suit la
chaîne resx **cœur** de la culture, et sans pack ar/fa/zh il retombe sur l'invariant (anglais). Le défaut
n'est donc pas « chrome français » mais « chrome **anglais** » sur des pages ar/fa/zh.

¹ Observation latérale, hors périmètre : le libellé pt `Contrasenha` (sans cédille, mot inhabituel —
pt-BR attend « Senha » ou « Palavra-passe ») vient du pack communautaire pt-BR v9.13.0 ; à traiter comme
grain séparé si confirmé.

## 3. Les deux porteurs du « Login » anglais — dont un est dans le dépôt

1. **L'objet DNN Login** (`dnn_DnnLogin_loginLink`, texte + `title`) lit les resx **cœur**
   (`SharedResources`/`GlobalResources`). Sans pack ar/fa/zh ⇒ invariant anglais. Ce porteur exige un
   pack (ou un resx cœur minimal).
2. **Le gabarit de la skin code « Login »/« Logoff » en dur** : `Title="Login"` /
   `Title="Logoff"` dans `controls/theme-body.ascx:50/63` et `Landingpage.ascx:66/79`. Ces tooltips sont
   anglais **sur toutes les cultures** (y compris la racine — mesuré `Login` ×3 sur `/`). Corrigible
   dans le dépôt, indépendamment de tout pack.

## 4. Amont : il n'existe **aucun** pack ar-SA, fa-IR ou zh-CN à importer

Mesuré le 25/10 :

- **org `dnnsoftware`** : exactement 8 dépôts `Language-Pack-*` — de-DE, es-ES, fr-FR, it-IT, nl-NL,
  pt-BR, pt-PT, vi-VN. **Pas de zh, pas d'ar, pas de fa.**
- **`dnnsoftware/Dnn.Platform`** (`DNN Platform/Website/App_GlobalResources`) : **0** resx culturel —
  la plateforme ne livre que la base en-US.
- **`r7-labs/R7.Dnn.Localization`** (la source du pack ru, MIT) : ru-RU uniquement.
- Recherche code GitHub entière : **aucun** `GlobalResources.ar-SA.resx` nulle part ; les
  `SharedResources.fa-IR.resx` trouvés appartiennent à d'autres produits (Oqtane & co — univers de clés
  différent, non réutilisable).

⇒ Les précédents ne se reproduisent pas : es = release officielle, pt-BR = dérivé du dépôt amont
`develop`, ru = import MIT + complétion. Pour **ar/fa il n'y a rien à dériver** : un pack se traduit
**from scratch**.

## 5. Échelle réelle — et la référence « ~1 007 clés » du pool ne correspond à rien de mesurable

Le pool donne « le paquet zh (~1 007 clés) » comme référence de taille. Mesuré : ce paquet **n'existe ni
dans l'install** (0 resx zh, 0 rangée `Packages`) **ni dans aucun amont accessible**. Le chiffre est
invérifiable ici ; les ancres **réelles**, mesurées sur cet install, sont :

| Périmètre | Clés (base en) |
| --- | ---: |
| Chrome cœur AGR, famille des 6 fichiers `App_GlobalResources` (Shared 621 · Countries 256 · Global 186 · Exceptions 136 · WebControls 54 · FileUpload 8) | **1 261** |
| Pack complet (référence fr / es) | 9 672 / 9 692 |

L'ordre de grandeur du pool (~10³) correspond à la **famille AGR cœur** (1 261) — c'est l'ancre honnête.
Mais le sous-ensemble **visible pour un visiteur** (page Login + lien login/logout) est bien plus petit :
quelques dizaines de clés de `GlobalResources`/`SharedResources`.

## 6. Options qui se dégagent (aucune exécutée — décision ai-01)

- **(a) Chrome minimal** : créer en dépôt `SharedResources.ar-SA.resx` + `GlobalResources.ar-SA.resx`
  (et fa-IR) ne portant que le sous-ensemble visible — login, logout, termes de formulaire. Effort de
  traduction ~10¹ clés ; déploiement = resx ⇒ **restart** (~33 s, quatrième magasin, connu). Effet
  immédiat sur la page Login et le lien de connexion des 2 langues.
- **(b) Pack complet from scratch** (~9,7 k clés × 2 langues, aucune source amont) : effort d'un autre
  ordre ; à comparer au reste du pool.
- **(c) Grain séparé dans le dépôt** : les tooltips codés en dur `Title="Login"/"Logoff"` de la skin
  (anglais sur 8/8 cultures) — corrigeable sans pack, même mécanique que #1950.
- L'item bloqué « `ar-SA:AR,fa-IR:FA` au sélecteur » cite « il reste les paquets (item 9) » : le lien
  est réel pour la **page Login**, mais le **menu/sélecteur lui-même est déjà localisé** par le rail
  skin — l'ajout au sélecteur ne dépend pas d'un pack complet, seulement du chrome que (a) couvre.

## 7. Gates

❌ aucune écriture en base · ❌ aucun resx créé/modifié · ❌ aucun pack installé · prod non touchée ·
aucun verdict visuel signé (ai-01). Toutes les mesures : fichiers + DB en lecture seule + GET HTTP.
