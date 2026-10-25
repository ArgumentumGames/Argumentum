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
