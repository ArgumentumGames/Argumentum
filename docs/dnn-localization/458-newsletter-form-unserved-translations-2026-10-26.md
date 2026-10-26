# Pool #458 — le formulaire newsletter est **traduit mais non servi** (mesure du 2026-10-26)

**Nature : mesure seule. Aucun geste, aucune écriture, aucune soumission de formulaire, `RawData` non lue.**
Machine : `myia-po-2023`. Cible : `https://dnn.argumentum.myia.io` (préprod).

> ⚠️ **Attribution corrigée le même jour — lire d'abord**
> [`458-newsletter-rendered-store-identified-2026-10-26.md`](458-newsletter-rendered-store-identified-2026-10-26.md)
> corrige ce document : la conclusion de **§3** (« app 33, entité 10176, valeurs non sélectionnées ») est
> **fausse**. Le magasin réellement rendu est l'**app 50 MobiusForms5** (`Resources.*`), dont les 25
> attributs `Message*`/`Button*`/`Label*` n'existent **qu'en dim 0** — le français servi sur 8 cultures est
> une **absence de traductions**, pas un défaut de sélection. L'entité 10176 n'a **aucun consommateur**
> (référencée par zéro JSON de groupe) : ses 14 valeurs localisées ne sont jamais rendues, et les 7/7
> sondes de §2 prouvaient une **copie** du markup du formulaire, pas un rendu (3ᵉ instance du piège
> d'égalité de texte). Les trois candidats de **§5** sont **clos** par le document de correction ; §1 et §2
> restent valides. Dans **§4**, seule la 2ᵉ puce tient (l'attribution à l'app 48 reste réfutée) : la 1ʳᵉ
> (« non-sélection, pas manque de traduction ») est **renversée** — c'est bien une **absence de traductions**
> dans le magasin rendu. Ce document est conservé tel quel : c'est la trace de la mesure et du piège.

## 1. Ce qui est servi aux visiteurs

Le bloc newsletter (formulaire Brevo/Sendinblue) est présent sur **toutes les pages** et son texte visible
est **identique et français sur les 8 cultures** : 821 caractères visibles, mêmes messages, même
paragraphe de consentement.

| Route | Statut | Texte visible du bloc | Marqueurs français |
|---|---:|---:|---|
| `/` | 200 | 821 car. | 4/11 |
| `/en/` | 200 | 821 car. | 4/11 |
| `/es/` | 200 | 821 car. | 4/11 |
| `/pt/` | 200 | 821 car. | 4/11 |
| `/ru/` | 200 | 821 car. | 4/11 |
| `/zh/` | 200 | 821 car. | 4/11 |
| `/ar/` | 200 | 821 car. | 4/11 |
| `/fa/` | 200 | 821 car. | 4/11 |

Il en va de même sur `/Actus` et `/en/Actus`. Modules 2sxc présents sur la page de détail d'article :
**588** (app 52), **587** (2sxc `Content` → app 33 « Default » de la zone 3), **600** (2sxc `App` → app 35).

## 2. Ce qui est stocké — les traductions existent

Schéma EAV 2sxc : `TsDynDataValue(ValueId, EntityId, AttributeId, Value)` + `TsDynDataValueDimension(ValueId, DimensionId, ReadOnly)`.
Dimensions actives de la zone 3 : `7` en-US, `8` ru-RU, `9` pt-BR, `10` es-ES, `11` ar-SA, `12` fa-IR, `13` zh-CN
(`3` et `6` sont des résidus `Culture Root` / `français`).

**app 33, entité `10176`, type de contenu `Content`** — le bloc newsletter :

| ValueId | Attribut | Dimension | Longueur brute | Marqueurs français |
|---|---|---:|---:|---:|
| 23430 | `Title` | *(base fr)* | 10 | — |
| 23431 | `Text` | *(base fr)* | 5 763 | **4/4** |
| 23432 / 23433 | `Image` / `ImageCaption` | *(base fr)* | 0 / 0 | — |
| 28317 | `Title` | 7 (en) | 10 | 0 |
| 28318 | `Title` | 8 (ru) | 8 | 0 |
| 28319 | `Title` | 9 (pt) | 10 | 0 |
| 28320 | `Title` | 10 (es) | 19 | 0 |
| 28321 | `Title` | 11 (ar) | 16 | 0 |
| 28322 | `Title` | 12 (fa) | 7 | 0 |
| 28323 | `Title` | 13 (zh) | 4 | 0 |
| 28310 | `Text` | 7 (en) | 5 480 | **0** |
| 28311 | `Text` | 8 (ru) | 5 521 | **0** |
| 28312 | `Text` | 9 (pt) | 5 491 | **0** |
| 28313 | `Text` | 10 (es) | 5 566 | **0** |
| 28314 | `Text` | 11 (ar) | 5 377 | **0** |
| 28315 | `Text` | 12 (fa) | 5 434 | **0** |
| 28316 | `Text` | 13 (zh) | 5 029 | **0** |

Toutes les lignes `TsDynDataValueDimension` de ces 14 valeurs portent `ReadOnly = 0`.

⇒ **Titre et corps sont traduits dans les 7 cultures**, et aucune des 7 traductions ne contient la phrase
française de consentement Brevo.

**Deux autres magasins portent la même copie française** (même texte visible, HTML différent) :

| Magasin | Entité | Attribut | Longueur brute | Dimensions | Remarque |
|---|---|---:|---:|---|---|
| app 48 | 10181 | `Html` (`HtmlFragment`) | 13 314 | **aucune** | copie unique, non localisable en l'état |
| app 35 | 10172 | `Title` ×3 | 27 / 18 / 23 | — | en-têtes d'accordéon seulement, pas le formulaire |

## 3. Ce qui est réellement rendu — les traductions ne sont pas sélectionnées

> ⚠️ **Section réfutée le jour même** — le rendu ne vient pas de l'app 33. Voir le document de correction (en-tête).

Discriminant : on prend les phrases les plus longues de chaque copie stockée et on les cherche
**verbatim** dans le texte servi de `/zh/Actus` (normalisation : balises retirées, entités décodées,
espaces condensés).

| Source | Sondes | Trouvées dans le servi `/zh/` |
|---|---:|---:|
| app 33 `Text` base fr (23431) | 7 | **7/7** |
| app 33 `Text` zh (28316) | 2 | **0/2** |
| app 33 `Text` en (28310) | 7 | 0/7 |
| app 48 `Html` (23452) | 7 | 3/7 (sous-ensemble : même texte visible, HTML plus large) |

⇒ la culture `zh-CN` reçoit la **valeur de base française** de l'app 33 ; sa valeur localisée existe,
est valide, et **n'est pas servie**.

## 4. Ce que cette mesure réfute

> ⚠️ **1ʳᵉ puce renversée** par le document de correction (en-tête) : le magasin rendu n'a aucune traduction. La 2ᵉ puce (app 48) reste valide.

- **« Le formulaire newsletter n'est pas traduit »** — faux : 14 valeurs localisées existent (7 `Title` + 7 `Text`).
  Le défaut est une **non-sélection**, pas un manque de traduction.
- **L'attribution de l'Item 8** (`docs/dnn-localization/458-tier3-mobius5-brevo-2026-10-25.md`, §2) : ce document
  identifie la copie servie à l'app 48 `HtmlFragment` sur la base d'une égalité de **texte visible**
  (8 844 car.). Cette égalité est vraie aussi pour l'app 33 base fr — le test ne pouvait pas distinguer les
  deux magasins. Traduire l'app 48 ne corrigerait donc pas la page servie si celle-ci vient de l'app 33.
  ⛔ Ne pas écrire dans app 48 avant d'avoir établi lequel des deux magasins alimente le rendu.

## 5. Ce qui reste ouvert (non établi, ne pas présenter comme un diagnostic)

> ⚠️ **Soldée le jour même** — les trois candidats de cette section sont clos par le document de correction (en-tête) ; §4 reste valide.

Pourquoi les valeurs dimensionnelles de l'entité 10176 ne sont pas sélectionnées. Les injections
précédentes de la lane (`app 52` h1, `app 60` RuleMemo) ont écrit sur les mêmes dimensions 7-13 de la
**même zone 3** et sont servies traduites : la carte langue → dimension fonctionne donc dans cette zone.
Ce qui reste à établir :

1. le magasin réellement rendu par module (l'attribution par position DOM donne 600/app 35, qui ne porte
   que les en-têtes d'accordéon — l'attribution est donc à re-mesurer, pas à croire) ;
2. si l'entité 10176 est marquée « partagée entre langues » au niveau de son type de contenu, ce qui
   court-circuiterait la résolution dimensionnelle ;
3. si les 14 valeurs localisées ont été écrites par une duplication DNN (`ContentLocalizationEnabled`)
   plutôt que par une traduction 2sxc, avec un lien de valeur incomplet.

Aucun de ces trois points n'est tranché par la mesure ci-dessus.

**Rétrécissement mesuré depuis (26/10, même tick)** — la résolution dimensionnelle **fonctionne** pour un
autre type de contenu de la **même app 33, même zone 3** : la page `/Téléchargements` (module 593) est
rendue depuis le type `Link` (352), et ses libellés sortent bien traduits — `/zh/Téléchargements` sert
`“打印即玩”版本` (valeur zh `28554`) et `下载` ×9 (valeur zh `28547`), `/en/` sert `"Print and Play" Version`
et `download` ×6, tandis que `/` sert le français. ⇒ l'hypothèse « la carte langue → dimension est cassée
dans cette zone » est **éliminée** ; le défaut est propre à l'entité 10176 / au type `Content`, ce qui
laisse les points 2 et 3 ci-dessus comme seuls candidats, et rend le point 1 moins probable.
Corollaire opérationnel : **écrire des valeurs dimensionnelles sur un attribut de l'app 33 est un levier
qui sert** — mesuré sur ce type, pas seulement supposé.

## 6. Portée et suite

- Rien n'a été écrit. Aucune soumission de formulaire. `RawData` (soumissions visiteurs) non lue.
- Le correctif n'est pas une traduction : c'est la **sélection** de valeurs déjà présentes. Il demande un
  arbitrage ai-01 sur le mécanisme, puis un geste transactionnel avec sauvegarde et relecture octet.
- À rapprocher de l'item de pool « app 33 `Text` ×2 en attente des valeurs côte à côte » : c'est la même
  app et le même type de contenu, et ce bloc-ci montre que l'app 33 porte **14** valeurs localisées, pas 2.

*Instrument : lectures HTTP sans cache + `sqlcmd` en lecture seule sur `ArgumentumGames`. Les longueurs et
les décomptes de marqueurs sont publiés ; aucun secret de `web.config` n'apparaît ici.*
