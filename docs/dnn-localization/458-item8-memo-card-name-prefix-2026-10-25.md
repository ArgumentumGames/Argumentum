# ⑧ `RuleMemoCardFileNamePrefix` — le préfixe du nom de carte reste français sur ru/ar/fa/zh

**Date :** 2026-10-25 · **Lane :** po-2023 · **Pool :** #458, item ⑧ · **Base :** `origin/master` `9d761f64`
**Nature :** constat, **lecture seule**. Aucune écriture en base, pivot inchangé, aucun fichier de gabarit modifié.

---

## 1. Ce qui est servi, mesuré sur les 8 alias

La page de détail d'une règle rend le nom de la carte mémo dans
`<div class="cardName">@Resources.RuleMemoCardFileNamePrefix - @ruleEntity.Title</div>`
(`Portals/1/2sxc/Argumentum/_RulesExplorer_RuleDetail.cshtml:117`). Sondé sur
`/Règles/details/l-ecole-des-menteurs/mid/602` et ses 7 alias culturels, HTML **décodé** :

| Alias | `cardName` servi | Préfixe attendu | Verdict |
| --- | --- | --- | --- |
| racine (fr) | `Argumentum - Règle - L'école des menteurs` | `Argumentum - Règle` | ✔ (témoin) |
| `/en` | `Argumentum - Rule - The school of liars` | `Argumentum - Rule` | ✔ |
| `/ru` | `Argumentum - **Règle** - Школа лжецов` | `Argumentum - Правило` | ✘ |
| `/pt` | `Argumentum - Regra - A escola dos mentirosos` | `Argumentum - Regra` | ✔ |
| `/es` | `Argumentum - Regla - La escuela de los mentirosos` | `Argumentum - Regla` | ✔ |
| `/ar` | `Argumentum - **Règle** - مدرسة الكاذبين` | `Argumentum - قاعدة` | ✘ |
| `/fa` | `Argumentum - **Règle** - مدرسهٔ دروغ‌پردازان` | `Argumentum - قانون` | ✘ |
| `/zh` | `Argumentum - **Règle** - 说谎者学校` | `Argumentum - 规则` | ✘ |

Le défaut est **isolé** : sur ces quatre pages, le champ **voisin** `RuleMemoCard` (le titre `<h2>` de
la section) **est** localisé — `Краткое содержание`, `ملخص اللعبة`, `خلاصهٔ بازی`, `概述`. Le titre de la
règle est localisé aussi (`Школа лжецов`, `说谎者学校`). C'est donc le seul segment de la page qui reste
français, pas une page non traduite.

**Ce n'est pas qu'un libellé.** `dist/frame.js:25-29` lit ce même `.cardName` et s'en sert comme **nom
de fichier** :

```js
var idNode = nodes[0].querySelector('.cardName');
…  cardName = idNode.innerHTML;
var fileName = cardName + ".png";
saveAs(fileBlob, fileName);
```

⇒ sur ru/ar/fa/zh, le PNG téléchargé s'appelle `Argumentum - Règle - <titre localisé>.png`.

## 2. La mesure en base : exactement une rangée manquante par langue

Entité `App-Resources` `DEA9410D-274B-4A4E-B47C-7D5D00692348` (eid 10340), app 60, lecture seule :

| Champ | base | en | ru | pt | es | ar | fa | zh |
| --- | --- | :-: | :-: | :-: | :-: | :-: | :-: | :-: |
| `RuleMemoCard` | `Carte Mémo` | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ |
| `RuleMemoCardDownload` | `Télécharger la carte mémo` | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ |
| **`RuleMemoCardFileNamePrefix`** | `Argumentum - Règle` | ✔ | **—** | ✔ | ✔ | **—** | **—** | **—** |

Corroboration indépendante, sur l'app entière : rangées de dimension sous `AppId 60` = **39** en
`en-US`/`pt-BR`/`es-ES` contre **38** en `ru-RU`/`ar-SA`/`fa-IR`/`zh-CN`. L'écart d'exactement une
rangée par langue est ce champ — et les deux champs témoins ci-dessus montrent que l'instrument voit
bien les dimensions (il ne rend pas 0 partout).

## 3. Deux référentiels divergent, et le pivot est celui qui s'injecte

Le même champ est porté par **deux** fichiers du dépôt, et ils ne disent pas la même chose :

| Référentiel | en | ru | pt | es | ar | fa | zh | Note |
| --- | :-: | :-: | :-: | :-: | :-: | :-: | :-: | --- |
| `docs/dnn-localization/dnn-ui-strings.csv` (rail UI, #457 L1) | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ | ✔ | « DELTA D3 **RESOLVED** — jsboige acté brand-translated policy (Argumentum = brand verbatim ; descripteur localisé) », apporté par **#772** (`f34ac77c`, 2026-07-11) |
| `docs/dnn-localization/eav-content.csv` (pivot, **source de la passe d'injection**) | ✔ | — | ✔ | ✔ | — | — | — | vide sur les 4 |
| **base de préprod** | ✔ | — | ✔ | ✔ | — | — | — | suit le pivot, pas le rail UI |

Valeurs déjà arbitrées par l'owner, telles que le rail UI les porte :
`ru = Argumentum - Правило` · `ar = Argumentum - قاعدة` · `fa = Argumentum - قانون` · `zh = Argumentum - 规则`.

**Le champ est le seul dans ce cas.** Balayage des 11 clés `App-Resources` de l'app 60, colonne par
colonne : partout ailleurs le rail UI et le pivot sont à 7/7 (`RuleSummary`, `RuleMaterial`,
`RuleInstallation`, `RuleContent`, `RuleVariants`, `RuleMemoCard`, `RuleMemoInstructions`,
`RuleMemoCardDownload` ; `Licence`/`Author` sont absents du rail UI mais 7/7 au pivot).
`RuleMemoCardFileNamePrefix` est la **seule** clé où le rail a 7 et le pivot 3.

⚠️ Le document de réconciliation `490-res-rule-reconciliation.md` porte encore, lignes 156/176/208,
`HOLD ⚠Δ3 … brand-policy arbitration jsboige (7 langs HOLD)`. C'était vrai le 2026-07-09 ; **#772 l'a
arbitré deux jours plus tard** et n'a pas annoté ce document. Un lecteur qui dispatche depuis la
ligne 176 rouvre une arbitration déjà rendue. Le document est un rapport daté — je ne le réécris pas ;
le pointeur est posé ici.

## 4. La justification `NEVER` de l'outil est factuellement fausse

`tools/458-eav-translate.py:56-58` exclut le champ de la traduction avec ce motif :

> « it is the *file name* the download link resolves to, so translating it would point at an asset that
> need not exist (ar/fa/ru/zh keep it empty → FR fallback name, which is the served-file reality). »

Mesure : il n'y a **aucun asset** à résoudre. Le fichier est fabriqué **dans le navigateur**
(`dist/frame.js` : `dom-to-image` → `FileSaver.saveAs`), à partir du texte affiché. Aucun `href` de
téléchargement ne pointe vers un nom calculé depuis ce champ — le seul `href` du bloc est `@qrLink`
(`:120`, l'URL de la page elle-même). Traduire le préfixe change le **nom du fichier produit**, il ne
peut pas rendre un lien mort.

La seconde moitié de la phrase est **circulaire** : « les quatre langues le gardent vide ⇒ repli FR,
qui est la réalité du fichier servi » décrit la **conséquence** de l'absence de traduction comme si
c'était la raison de ne pas traduire.

Je ne modifie pas le `NEVER` : c'est le contrat de l'outil, et changer un contrat sans arbitrage est
hors de ma portée. Le constat est publié pour que la décision se prenne sur ce motif, pas sur celui
qui est écrit.

## 5. Le correctif, prêt, non exécuté

Quatre cellules, **pure addition** (aucune rangée de dimension existante sur ces couples ⇒ rien
d'édité à la main ne serait écrasé) :

1. **pivot** `eav-content.csv`, rangée `DEA9410D…|RuleMemoCardFileNamePrefix` : remplir `ru`/`ar`/`fa`/`zh`
   avec les valeurs du rail UI ci-dessus (le pivot est la source de la passe, le laisser vide alors que
   la base est remplie — ou l'inverse — recrée exactement la divergence mesurée ici) ;
2. **base préprod** : 4 `INSERT` `TsDynDataValue` + 4 `TsDynDataValueDimension` (dims 8/11/12/13),
   même mécanique que l'injection app 59 ([`458-24b-4-app59-injection-2026-10-24.md`](458-24b-4-app59-injection-2026-10-24.md)),
   relecture octet à octet et identifiants de rollback hors dépôt.

Effet attendu, à re-mesurer sur les 8 alias : `/ru`, `/ar`, `/fa`, `/zh` servent leur préfixe ; `/`,
`/en`, `/pt`, `/es` **inchangés** (les 4 sont le contrôle de non-régression). Le nom du PNG téléchargé
suit.

Ce qui reste ouvert et n'est **pas** à moi : (a) modifier le commentaire et le `NEVER` de l'outil,
(b) annoter ou non le document `490`, (c) le go/no-go de l'injection. L'item de pool disait
« constat » : je livre le constat, pas le geste.

## 6. Piège d'instrument traversé (encore le même)

Mon premier comptage cherchait la chaîne `Règle` dans les octets bruts : il rendait **0** pour
`/ru`, `/ar`, `/fa`, `/zh` — c'est-à-dire l'inverse de la vérité. Le serveur émet
`Argumentum - R&#232;gle`. La mesure juste est sur le HTML **décodé** ; les 4 défauts réapparaissent
alors, et le témoin `/` reste à 1. Même famille que `Diret&#243;rio` sur l'injection app 59.

Contrôle inverse de l'instrument : `en`/`pt`/`es` portent bien leur préfixe ⇒ l'instrument qui rend
« français » sait aussi rendre « localisé ».

## 7. Gates

❌ aucune écriture en base · ❌ pivot/CSV/gabarits inchangés · ❌ zéro donnée membre/commande/utilisateur
· ❌ aucun verdict visuel signé (ai-01) · lecture seule sur la préprod, prod non touchée.
