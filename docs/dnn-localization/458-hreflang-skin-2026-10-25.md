# `hreflang` émis par la skin (2shineBS5) — alternates, canonical, x-default

**Date :** 2026-10-25 · **Lane :** po-2023 · **Pool :** #458 item 4, décision ai-01 n°2 (c.6400168954) · **Base :** `origin/master` `dc42437c`
**Portée :** préprod (`dnn.argumentum.myia.io`). **Prod non touchée** — décision 2 : le déploiement en prod est gaté sur le go-live.

---

## 1. Ce que l'item demandait

« `hreflang` : GO pour les alternates émis par la skin en préprod. Langues dérivées du portail,
`x-default` = langue par défaut, `canonical` auto-référente, garde de contrat. En prod uniquement
avec le go-live. »

## 2. Mesures préalables — et pourquoi l'émetteur est la skin

| Mesure | Résultat | Instrument |
| --- | --- | --- |
| `<link rel="canonical\|alternate">` servi | **0** sur les routes mesurées | HTML servi (rendu frais) |
| Émetteur compilé | **aucun** — ni webroot ni `bin` (seul `AngleSharp.dll` porte la chaîne) | balayage |
| Skin réellement servi | **`[G]skins/2shinebs5/default.ascx`** | `PortalSettings.DefaultPortalSkin` |
| → fichier à modifier | **`controls/theme-body.ascx`** (inclus par `default.ascx`) | include |
| Langues du portail | **8** (`ar-SA`, `en-US`, `es-ES`, `fa-IR`, `fr-FR`, `pt-BR`, `ru-RU`, `zh-CN`) | `PortalLocalization`, `PortalID=1` |
| Langue par défaut | **`fr-FR`** | `Portals.DefaultLanguage` |

⚠️ `Landingpage.ascx` **n'est pas** le skin servi (0 page ne le sélectionne, cf. #1984) : y ajouter
l'émetteur aurait produit un correctif inerte.

API vérifiée **par réflexion sur `DotNetNuke.dll` 10.3.2**, pas par souvenir :

- `Globals.NavigateURL(int tabID, bool isSuperTab, PortalSettings, string controlKey, string language, string[] additionalParameters)` — le paramètre `language` est le point qui rend l'URL par culture ;
- `LocaleController.Instance.GetLocales(int portalID) → Dictionary<string, Locale>` — lit `PortalLocalization`, donc **dérivé du portail** ;
- `TabInfo.IsSuperTab`, `PortalSettings.DefaultLanguage` — présents.

## 3. Deux pièges d'instrument rencontrés, et ce qu'ils ont changé

**(a) Le token `[URL]` nomme une URL qui n'existe pas.** Le sélecteur de langue du site utilise
`LanguageTokenReplace.ReplaceEnvironmentTokens("[URL]")`. Mesuré sur la page servie : le token rend
l'URL **de la requête courante**, query string incluse, et sur une culture non défaut DNN
l'**absorbe dans le chemin** — `/en/Actus?cb=a38f7013` est devenu `/en/Actus/cb/a38f7013`. Un
canonical bâti là-dessus nommerait une URL inexistante ; pire, il ferait de chaque URL paramétrée
son propre canonical. ⇒ Les URLs sont construites par `NavigateURL(tab, …culture…)`, **nue par
construction**.

**(b) Le compte d'alternates inclut le `x-default`.** Première mesure : `alternates = 9` là où
j'attendais 8. Ce n'était pas un défaut du code mais **de mon assertion** — le motif
`<link rel="alternate" hreflang="…">` matche aussi le `x-default`. Le vrai compte est **8 langues +
1 x-default = 9**, et la mesure le dit désormais explicitement (`total=9 distincts=…, x-default`).
Leçon générale : un compte qui ne nomme pas ce qu'il compte ne prouve rien.

## 4. Ce qui a été écrit

Un émetteur dans `controls/theme-body.ascx`, appelé depuis `OnPreRender` (là où la skin localise
déjà les méta) :

- **langues** : `LocaleController.Instance.GetLocales(portal.PortalId)` — jamais l'attribut
  `Languages="fr-FR:FR,…"` du sélecteur, qui ne liste que **5** cultures parce que zh/ar/fa restent
  gated à l'écran jusqu'au chrome minimal. hreflang s'adresse aux moteurs, le sélecteur aux
  visiteurs : deux surfaces, deux populations. Épingler les alternates au menu **sous-déclarerait le
  site de trois cultures** ;
- **une `alternate` par culture**, URL absolue via `NavigateURL(tab, isSuperTab, settings, "", culture, new string[0])` ;
- **`canonical` auto-référente** = l'alternate de la culture courante, sans query string ;
- **`x-default`** = `portal.DefaultLanguage` (`fr-FR`) ;
- **une émission par requête** (drapeau d'instance), et tout est dans un `try/catch` : une erreur ici
  ne doit jamais faire tomber la page — c'est du SEO, pas du rendu.

## 5. Preuve servie — déploiement temporaire, puis restauration

Le changement a été **copié dans l'arbre servi**, l'app-domain recyclé (mtime de `web.config`,
`sha256` inchangé), mesuré, puis **restauré depuis `HEAD`** (`git show HEAD:…`, jamais
`git checkout --`). L'arbre servi est reparti propre, et la disparition de l'émission après
restauration a été **mesurée** (0 alternate, 0 canonical) — sans quoi « restauré » n'aurait été
qu'une intention.

Bloc servi sur `/en/Actus` :

```html
<link rel="alternate" hreflang="ar-SA" href="https://dnn.argumentum.myia.io/ar/Actus" />
<link rel="alternate" hreflang="en-US" href="https://dnn.argumentum.myia.io/en/Actus" />
<link rel="alternate" hreflang="es-ES" href="https://dnn.argumentum.myia.io/es/Actus" />
<link rel="alternate" hreflang="fa-IR" href="https://dnn.argumentum.myia.io/fa/Actus" />
<link rel="alternate" hreflang="fr-FR" href="https://dnn.argumentum.myia.io/actus" />
<link rel="alternate" hreflang="pt-BR" href="https://dnn.argumentum.myia.io/pt/Actus" />
<link rel="alternate" hreflang="ru-RU" href="https://dnn.argumentum.myia.io/ru/Actus" />
<link rel="alternate" hreflang="zh-CN" href="https://dnn.argumentum.myia.io/zh/Actus" />
<link rel="canonical" href="https://dnn.argumentum.myia.io/en/Actus" />
<link rel="alternate" hreflang="x-default" href="https://dnn.argumentum.myia.io/actus" />
```

Mesure sur **9 routes** (accueil + `Actus` pour fr, `Actus` pour les 7 autres cultures) :

| Invariant | Résultat |
| --- | --- |
| HTTP 200 | **9/9** |
| 8 alternates de langue + 1 `x-default` | **9/9** |
| `canonical` exactement 1 | **9/9** |
| `canonical` **sans query string** | **9/9** |
| `canonical` et alternates **absolus** | **9/9** |
| `canonical` **== l'alternate de la culture courante** | **9/9** |
| `x-default` == l'alternate `fr-FR` | **9/9** |
| `Error Showing Content` | **0** |
| `<html lang>` distincts | **8** |

Après restauration, non-régression **16/16** routes (8 cultures × accueil + `Actus`),
`app-news5-details-link` = 5 partout, `Error Showing Content` = 0, 8 `lang` distincts.

`fr-FR` sort en `/actus` (minuscule) alors que les autres cultures gardent `Actus` : c'est la casse
que DNN lui-même sert pour la culture par défaut — le sélecteur produit la même. IIS ne distingue
pas la casse ; les deux alternates `fr-FR` et `x-default` sont **identiques**, ce qui est l'invariant
utile.

## 6. Garde de contrat, avec contrôle inverse

`SkinHreflangContractTests` (8 faits, verts) — les invariants sont des **fonctions pures du texte**
de l'émetteur, et chacune est exercée **deux fois** : sur le fichier réel (doit tenir) et sur une
copie **mutée** de ce même texte (doit casser).

| Fait | Ce qu'il épingle |
| --- | --- |
| `TheServedSkinEmitsAlternatesCanonicalAndXDefault` | les trois formes sont émises |
| `LanguagesAreDerivedFromThePortalAndNeverHardcoded` | `GetLocales` + `portal.PortalId`, **aucun** littéral de culture ; `x-default` ← `portal.DefaultLanguage` ; cible du canonical calculée |
| `TheAlternatesAreAddedToThePageHeadAndGuardedAgainstFailure` | ajout dans `Head`, `catch`, drapeau d'unicité |
| 5 contrôles inverses | sans `x-default` / sans `canonical` / liste de cultures en dur / culture `x-default` en dur / canonical littéral ⇒ **rouge** |

La mutation est **assertée appliquée** : un contrôle inverse dont la mutation ne mute rien laisserait
l'invariant vert et se lirait « contrôle passé » — c'est précisément la façon dont un contrôle
inverse ment. Le bloc est délimité à l'émetteur : le fichier porte aussi le `Languages="fr-FR:FR,…"`
du menu, légitimement en dur, et une assertion sur le fichier entier le confondrait avec une
violation.

## 7. État de déploiement — à lire avant de conclure

- **Préprod** : la fonction est **mesurée mais non déployée**. Le fichier a été restauré ; elle
  arrivera par le merge puis le fast-forward du webroot. C'est l'ordre voulu : une copie non suivie
  dans l'arbre servi est invisible à git et bloque le `pull --ff-only` du tick suivant.
- **Prod** : rien, et rien tant que le go-live n'est pas décidé (gel #972). Rappel de la décision 2.

## 8. Comment vérifier que ce constat est mort

```powershell
# sur une route servie, après merge + ff + recycle
$h = (Invoke-WebRequest 'http://dnn.argumentum.myia.io/en/Actus?cb=1' -UseBasicParsing).Content
[regex]::Matches($h, '<link rel="(alternate|canonical)"[^>]*>').Count   # attendu 10
```

Puis, par route : `canonical` == l'`alternate` de la culture courante, `x-default` == l'`alternate`
`fr-FR`, et `canonical` sans `?`. Si ces trois invariants tombent, c'est la skin qui a régressé —
la garde de contrat le dira avant la page.

---

## 9. REPRISE 26/10 02:xx — deux défauts de la review ai-01 (c.6405462862), corrigés et prouvés servis

La PR n'avait pas été mergée : ai-01 a mesuré sur la **prod** que les articles vivent sur le même
onglet (`/actus/details/<slug>`, 5 liens réels), donc un `canonical` bâti sur onglet+culture
déclarerait chaque article **doublon de la liste** (désindexation) ; et #1991 a mesuré que
`NavigateURL(…, culture)` rend des URL **neutres** sous `ContentLocalizationEnabled=False`, où les
8 alternates s'effondreraient sur une seule. Choix de lane : **l'hybride** recommandé par web2
(c.czsOw) —

1. **`canonical` = chemin de LA REQUÊTE, query retirée** (`Request.RawUrl`, pas `Request.Url`) :
   correct sur toute vue, liste comme détail — l'article se déclare lui-même, jamais la liste.
2. **`alternate`/`x-default` sur la VUE NUE d'onglet uniquement** (chemin de requête == URL composée
   de la culture courante, insensible à la casse) : un slug n'a pas de traduction, une alternate sur
   une vue détail ne pourrait pointer que vers la liste.
3. **URLs par culture composées des rangées `PortalAlias` culturelles** (préfixes
   `dnn.argumentum.myia.io/{ar,en,es,fa,pt,ru,zh}` et racine `fr-FR`, mesurées en base) : **aucune**
   lecture du drapeau, **un seul** appel `NavigateURL` — ancré sur la langue par défaut (jamais
   préfixée, donc neutre dans les deux états).

### Deux pièges d'instrument rencontrés pendant la preuve (mesurés, pas déduits)

- **`Request.Url` porte le chemin réécrit INTERNE** : la requête `/actus` sert
  `Request.Url = https://dnn.argumentum.myia.io/Default.aspx` — un canonical bâti dessus nomme une
  URL que le client n'a jamais demandée. `Request.RawUrl` porte l'URL amie demandée ; le canonical
  recompose autorité + `RawUrl` sans query (repli `Request.Url` si `RawUrl` vide).
- **Un ascx qui ne compile pas avale tout sans trace visible** : un espace de noms erroné
  (`DotNetNuke.Entities.PortalSettings`, sans `.Portals`) produit **0 balise** avec page 200 —
  l'erreur réelle vit dans `Logs/*.log.resources` (CS0234), pas dans la page. La « preuve servie »
  n'a de valeur qu'après avoir lu ce qu'elle émet, pas seulement son 200.

### Preuve servie — les DEUX états du drapeau, protocole réversible #1991

Copie temporaire dans l'arbre servi + recycle (mtime `web.config`, sha invariant), mesures, puis
restauration octet (sha == sauvegarde), recycle, re-mesure. `[INFO]` posté avant le geste.

| Route | `True` : canonical / alternates | `False` : canonical / alternates |
| --- | --- | --- |
| `/` | `https://…/` / **9** (8 + x-default) | identique |
| `/actus` | `https://…/actus` / **9** | identique |
| `/en/Actus` | `https://…/en/Actus` (casse demandée) / **9** | identique |
| `/actus/details/argumentum-est-vendu-sur-argumentum-games` | **URL de l'article** / **0** | identique |
| `/en/Actus/details/<même slug>` | **URL de l'article** / **0** | identique |

- **16/16 hrefs émis → 200** dans les deux états (8 cultures × accueil + actus) — ferme les 🟡 de
  NanoClaw (hôte/schéma, cultures non affichées au sélecteur) : chaque URL composée résout, sur
  l'hôte servi.
- Sous `False`, **aucun effondrement** : les alternates restent préfixées par culture (composition
  alias, pas `NavigateURL`) — l'effondrement mesuré par #1991 ne se reproduit pas.
- Après restauration du fichier : **0 alternate / 0 canonical** re-mesuré, arbre git propre,
  non-régression **16/16** routes, `Error Showing Content` = 0 partout.

### Garde étendue

`SkinHreflangContractTests` passe de 8 à **15 faits** : les trois prédicats nouveaux (canonical =
chemin de requête via `RawUrl` ; alternates gated vue nue ; composition depuis les rangées d'alias
culturelles, drapeau banni) ont chacun leur contrôle inverse — dont deux qui rejouent les défauts
réels : canonical onglet (pré-reprise) et `NavigateURL` par culture (effondrement #1991). Suite
complète : **voir le corps de la PR pour le compte courant** (chaque relecture le re-mesure).

### Ce qui reste vrai du §7

La fonction reste **mesurée mais non déployée** : elle arrive par merge puis fast-forward du
webroot. La prod reste gelée jusqu'au go-live (#972).
