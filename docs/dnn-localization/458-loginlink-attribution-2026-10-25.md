# Item 7 — Lien de connexion core `dnn_DnnLogin_loginLink` : attribution confirmée, déjà couverte par le chrome Tier 1 (#1972) — preuve servie 8/8 (mesuré 25/10, lecture seule)

Pool #458, item 7 : « Attribution du lien de connexion du cœur DNN
(`dnn_DnnLogin_loginLink`, 5/8) : nommer le resx du cœur et les cultures manquantes,
puis verser au fichier chrome de po-2024. »

**Verdict : l'attribution est établie, et elle est déjà versée.** Le document chrome
Tier 1 de po-2024 (`458-chrome-minimal-zh-ar-fa-2026-10-25.md`, PR #1972 **mergée**
25/10 08:21) nomme, à ses lignes 25-26, **exactement ce fichier, ces clés et cette
surface**. Il n'y a donc **pas de nouveau grain** : l'item 7 se solde en recoupement,
avec deux preuves que le document #1972 ne portait pas encore (mesure servie 8/8 du
lien, et code-behind relu au SHA du binaire).

## 1. Attribution (fichier + clés + cultures manquantes)

| Élément | Valeur mesurée |
|---|---|
| Surface | `<dnn:login id="DnnLogin" …>` dans `Portals/_default/Skins/2shineBS5/controls/theme-body.ascx:153` (aussi `Landingpage.ascx:128`, 0 page) |
| Contrôle core | `DotNetNuke.UI.Skins.Controls.Login` (`admin/Skins/Login.ascx.cs`, relu au SHA `4b0214b8` du binaire servi) |
| Résolution du texte | `Localization.GetString("Login", Localization.GetResourceFile(this, "Login.ascx"))` (l. 116 ; logoff : `"Logout"`, l. 93) — le skin ne passe **pas** `Text=`/`LogoffText=`, donc la branche resx est bien celle servie |
| **Fichier resx** | **`admin/Skins/App_LocalResources/Login.ascx.<culture>.resx`** (base : `Login.ascx.resx` = en) |
| **Clés** | **`Login.Text`** et **`Logout.Text`** |
| Fichiers présents | base (en) + `fr-FR`, `es-ES`, `pt-BR`, `ru-RU` = **5 cultures** |
| **Cultures manquantes** | **`zh-CN`, `ar-SA`, `fa-IR`** (0 fichier `Login.ascx.*.resx` pour ces cultures sous `admin/Skins/App_LocalResources/`) |

Valeurs extraites (XPath `//data[@name='Login.Text']/value`.InnerText) :

| Fichier | `Login.Text` | `Logout.Text` |
|---|---|---|
| `Login.ascx.resx` (en) | Login | Logout |
| `Login.ascx.fr-FR.resx` | Connexion | Déconnexion |
| `Login.ascx.es-ES.resx` | Iniciar | Salir |
| `Login.ascx.pt-BR.resx` | Iniciar | Salir |
| `Login.ascx.ru-RU.resx` | Войти | Выйти |

⚠️ pt-BR et es-ES portent la **même** valeur (`Iniciar`/`Salir`) — constat, pas un jugement
(pt-BR aurait « Entrar »/« Sair » en portugais courant ; grain éditorial séparé si retenu,
même famille que « Contrasenha »→« Senha » déjà au pool po-2024).

## 2. Preuve servie 8/8 (id `dnn_DnnLogin_loginLink`, texte du lien)

| Culture | Texte servi |
|---|---|
| fr-FR (racine) | **Connexion** |
| en-US | Login |
| es-ES | **Iniciar** |
| pt-BR | **Iniciar** |
| ru-RU | **Войти** |
| zh-CN | Login ← fallback EN |
| ar-SA | Login ← fallback EN |
| fa-IR | Login ← fallback EN |

Le « 5/8 » du pool est donc mesuré tel quel : 5 cultures localisées, 3 retombées anglais.
Le `href` suit la culture (`/en/Login?returnurl=…`, `/zh/Login?…`) — seule la **chaîne**
manque, pas le routage.

## 3. Versé au fichier chrome : déjà fait — ne pas doubler

Le document #1972 (mergé) ligne 25 : `admin\Skins\App_LocalResources\Login.ascx.resx` ·
`Login.Text` · **lien de connexion (chaque page, via `<dnn:login>` de `theme-body.ascx:153`)**
· « Connexion ». Ligne 26 : `Logout.Text` / « Déconnexion ».

⇒ L'item 7 du pool po-2023 est **consommé par l'item 3 du pool po-2024** (chrome Tier 1,
12 resx, 39 triples). Quand po-2024 pose les 3 fichiers `Login.ascx.{zh-CN,ar-SA,fa-IR}.resx`
avec les 2 clés, le lien passe à 8/8 sans autre geste. **Vérif post-geste attendue** :
`/zh/` lien = texte zh (≠ `Login`), `/ar/` et `/fa/` idem, `Logout.Text` présent (couvre
l'utilisateur connecté, non mesurable anonymement). resx = restart ~33 s (protocole du
lane, `[INFO]` avant geste, NR 16 routes après).

## 4. Distinction des deux surfaces de login (à ne pas fusionner)

- **Tooltip** du cadenas : `<a Title="<%= LocalizeString("Login.Tooltip") %>">` dans le
  même `theme-body.ascx:63` — **skin**, déjà localisé **8/8** par #1973 (resx du thème).
- **Lien texte** `<dnn:login>` (cet item) — **core**, 5/8, resx `admin/Skins/…`.

Deux magasins différents, deux historiques différents ; le premier est clos, le second
attend la pose des fichiers chrome Tier 1.
