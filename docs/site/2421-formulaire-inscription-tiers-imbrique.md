# Le formulaire d'inscription tiers ne fonctionne pas : le `<form>` Brevo est imbriqué dans le formulaire DNN (#2421)

**Statut** : cause mesurée, remède proposé avec son geste exact. **Aucune écriture** n'a été faite — ni sur le site servi, ni en base, ni dans le dépôt hors de ce document. La mesure est **en lecture seule** (aucune soumission de formulaire, aucun login).

**Nature des affirmations** : chaque énoncé porte sa nature — *mesuré* (relevé sur les octets servis ou dans le DOM), *dérivé* (conséquence logique d'un fait mesuré), *rapporté* (reprise d'un rapport d'une autre lane, non remesuré).

**Instrument principal** : le HTML brut servi (lecture HTTP), analysé avec `DOMParser` dans la page (parseur HTML5 local, sans ressource) ; le DOM vivant (Chromium, Playwright) ; les octets du script tiers (téléchargement direct, `sha256` comparé) ; le témoin `docs/dnn-localization/dnn-page-witness.py` ; l'outil `tools/458-eav-reinject.py` en dry-run (SELECT seuls).

**Horodatage** : 28/11/2026, ~15:35–16:20Z ; **re-mesures du ~17:10Z** après la contre-revue de po-2024 (§1.2, §5.2, §6.4). Base du dépôt : `origin/master` `21d64b7d`.

---

## 1. La cause, en une phrase

Le skin 2shineBS5 enveloppe **tout le corps** de la page dans un formulaire DNN (`<form id="Form">`) ; le bloc « Newsletter Sign-Up » y insère un **second** `<form id="sib-form">` (Balpech/Brevo) ; le parseur HTML5 **ignore** un `<form>` imbriqué, donc `#sib-form` **n'entre jamais dans le DOM** ; la toute première instruction du script tiers fait `document.querySelector("#sib-form").setAttribute(...)` sur `null` → `TypeError`, et l'initialisation s'interrompt **avant** de brancher l'envoi.

### 1.1 Ce que le script attend, au juste

Fichier servi : `https://sibforms.com/forms/end-form/build/main.js` — 461 411 octets, `sha256` `8a29b2ee61e93fda…` (*mesuré* ; identique octet pour octet au fichier re-téléchargé pendant ce tick, et à celui déjà téléchargé plus tôt dans la journée).

L'instruction fautive est la première du module webpack **91387** (*mesuré* — le nom de module est celui que Chromium affiche dans la frame `at 91387 (…)`) :

```js
const b=document.querySelector("#sib-form");b.setAttribute("novalidate","true");
const w=document.querySelector("form#sib-form input[name='email_address_check'].input--hidden");w&&w.setAttribute("aria-hidden","true")
```

Deux éléments sont donc attendus :

| Attendu | Rôle | Présent sur la page ? |
|---|---|---|
| `#sib-form` (l'élément `form`) | cible du premier `setAttribute`, puis support du `addEventListener("submit", …)` | **non** — *mesuré* |
| `form#sib-form input[name='email_address_check'].input--hidden` | `aria-hidden` (protégé par `w&&…`) | sans objet : le `form` n'existe pas — *mesuré* |

Le bundle compte **17** appels `.setAttribute(` ; **un seul** est non protégé sur le résultat d'un `querySelector` — celui-ci. Les 16 autres sont sur des éléments известs ou protégés (*mesuré*).

**Coordonnées.** Chromium rapporte la frame `at 91387 (…/main.js:1:25338)` via `pageerror`, et la console MCP affiche `…/main.js:1:426617` (frame du haut, dans le bootstrap webpack). Le rapport d'ai-01 du 28/11 12:25Z cite `2:25339`. Le **numéro de ligne diffère d'une unité entre deux lectures du même fichier** (les octets, eux, sont les mêmes : `sha256` identique). ⇒ **citer l'instruction, pas les coordonnées** ; c'est l'instruction qui est la preuve.

### 1.2 Pourquoi `#sib-form` n'existe pas

Le HTML brut servi de `/en/` porte **2** ouvertures `<form` et **2** fermetures `</form>`, dans cet ordre (*mesuré*, offsets en **octets**).

⚠️ **Les offsets sont datés, pas constants** : la page servie varie d'une requête à l'autre — elle a gagné 1 493 octets entre 15:35–16:20Z et 16:46Z (cause non mesurée) et deux lectures à quelques minutes d'écart ont la **même longueur** mais des **empreintes distinctes**. **L'invariant à citer est l'ordre et l'imbrication, pas la position** ; les deux colonnes ci-dessous sont deux mesures datées du même fait.

| Ordre | Balise | 15:35–16:20Z (89 647 o) | ~17:10Z (91 140 o) |
|---:|---|---:|---:|
| 1 | `<form method="post" action="/en/" id="Form" enctype="multipart/form-data">` — le formulaire du skin 2shineBS5 | 4 416 | 4 423 |
| 2 | `<form id="sib-form" method="POST" action="https://d426a943.sibforms.com/serve/MUIE…">` — le formulaire Brevo, **à l'intérieur** | 75 952 | 77 354 |
| 3 | `</form>` — **la balise de fin du Brevo** : c'est elle qui ferme le formulaire DNN | 83 551 | 84 953 |
| 4 | `</form>` — la vraie fin du formulaire DNN | 89 618 | 91 107 |

Règle HTML5 (« *A start tag whose tag name is 'form': if the form element pointer is not null, ignore the token* ») : un `<form>` imbriqué **n'entre pas dans le DOM**, et la `</form>` qui le suit ferme le formulaire englobant (*mesuré*, par le contrôle apparié ci-dessous).

**Contrôle apparié, sur les octets bruts** (le DOMParser de la page, sans recharger de ressource) — un seul changement entre les deux bras : l'ouverture et la fermeture du formulaire DNN.

| Bras | Formulaires dans le DOM | `#sib-form` |
|---|---|---|
| Production (source telle que servie) | 1 — `#Form` | **absent** |
| Témoin (mêmes octets, balises `<form id="Form">` et `</form>` du skin retirées) | 1 — `sib-form`, action Brevo | **présent** |

La causalité est donc établie sur la seule imbrication, pas sur une coïncidence de version (*mesuré*).

**Pourquoi le script est-il là sans que son élément le soit ?** Le contenu servi vient du bloc 2sxc `data-cb-instance='600' data-cb-id='-10179'` (instance d'app **600**, module 600), dans le panneau d'accordéon « **Newsletter Sign-Up** » (enfant `data-accordion-child="10172"`), à côté du panneau « Contact Us » (Mobius). Le registre EAV porte bien la balise `<form id="sib-form">` dans ce contenu : c'est **la source** qui est imbriquée, pas un script orphelin chargé à côté.

---

## 2. Les pages concernées

Balayage **lecture seule** : 6 parcours publics × 8 langues, sur les deux environnements, 48 pages par environnement, toutes en HTTP 200 (*mesuré*).

| Environnement | Pages qui **chargent** le script | Pages qui portent la balise `<form id="sib-form">` | Pages où cette balise est **imbriquée** dans `#Form` |
|---|---:|---:|---:|
| prod `https://www.argumentum.games` | **48 / 48** | **48 / 48** | **48 / 48** |
| préprod `https://dnn.argumentum.myia.io` | **48 / 48** | **48 / 48** | **48 / 48** |

Parcours : `accueil`, `Règles`, `Argumentation`, `actus`, `Amis`, `Téléchargements` — langues `fr`, `en`, `es`, `pt`, `ru`, `zh`, `ar`, `fa`.

Également mesuré : les pages de détail d'actualité portent le même bloc (`/en/Actus/details/le-site-argumentum-en-huit-langues` → HTTP 200, script présent, balise imbriquée). En revanche `/terms`, `/privacy` et `/en/Login` ne chargent **pas** le script (ce ne sont pas des pages du skin) — *mesuré*.

Conséquence : **le défaut est systématique, pas occasionnel**. Sur chaque page du corpus, le script s'exécute, ne trouve pas son formulaire et lève l'erreur.

### 2.1 Le chiffre « 17 occurrences » n'est pas reproduit

Rapporté d'ai-01 (28/11 12:25Z) : « 17 occurrences sur 10 pages ». Mesuré : **1 erreur par chargement de page** — contrôlé sur `/en/`, `/en/Amis`, `/en/Règles` (prod) et `/en/` (préprod), via `pageerror` : une seule `TypeError` à chaque fois (*mesuré*).

L'origine du « 17 » n'est pas établie : le compteur de la console d'une session de navigateur cumule les lignes de toutes les navigations, alors que le défaut est par page. Je ne l'attribue donc à rien (*non établi*) — mais le chiffre **1 par page** est le mien, mesuré.

⚠️ **Ce que le « 1 » compte** : les **exceptions non rattrapées** (`pageerror`), pas les lignes de la console — un lecteur peut lire « 1 erreur console » et obtenir un autre nombre avec un autre instrument. La contre-revue de po-2024 (*rapporté*, 16:46–16:52Z) voit **3** lignes d'erreur par page sur son instrument, les 2 autres étant « Permissions policy violation: unload is not allowed », sans rapport avec l'issue. **Non reproduit sur l'instrument de po-2023** (*mesuré*, ~17:02Z, `/en/` et `/en/Amis`, prod) : **1** exception `setAttribute` par page et **0** ligne d'erreur de console. L'écart entre les deux instruments n'est pas expliqué ; il ne touche pas le défaut, qui est l'exception `setAttribute` — **1 par page, sur les deux instruments**.

---

## 3. Ce que le défaut casse (et ce qu'il ne casse pas)

| Constat | Nature | Mesure |
|---|---|---|
| L'initialisation du script tiers s'arrête à la première instruction | mesuré | `TypeError: Cannot read properties of null (reading 'setAttribute')`, 1 par page |
| Le gestionnaire d'envoi du formulaire tiers n'est donc jamais branché | dérivé | `b.addEventListener("submit", …)` est **après** l'instruction fautive, dans le même module |
| Le bouton « SUBSCRIBE » appartient au formulaire **DNN** | mesuré | `button.sib-form-block__button` (`type=submit`), `closest('form').id === "Form"`, action `/en/`, méthode `post` |
| ⇒ un clic sur « SUBSCRIBE » renverrait le formulaire du site vers `/<langue>/` au lieu de Brevo | **dérivé** (jamais exécuté : aucune soumission) | le propriétaire du bouton est mesuré ; l'effet du clic ne l'est pas |
| La `</form>` du Brevo ferme le formulaire DNN **avant** la fin du corps | mesuré | 6 067 octets avant la fin (15:35–16:20Z), 6 154 à la re-mesure du ~17:10Z — un chiffre **daté**, comme les offsets du §1.2 ; reste hors formulaire : `FOOTER#to-shine-page-footer`, `INPUT#ScrollTop`, `INPUT#__dnnVariable`, un `LINK`, 5 `SCRIPT` |
| Aucun `__doPostBack` dans cette queue | mesuré | 0 occurrence | 
| Le formulaire de contact Mobius reste dans le formulaire DNN | mesuré | `#Subject`, `#SenderName`, `#SenderMail` → `closest('form').id === "Form"` |

La dernière ligne de la queue (`INPUT#__dnnVariable`, `INPUT#ScrollTop`) est hors formulaire à cause de la fermeture anticipée. L'effet visible est faible (aucun `__doPostBack` ne s'y trouve) — mais la cause et la queue sont, elles, mesurées.

---

## 4. Mobius : la prémisse de l'issue est à corriger

L'issue parle d'un « mode débogage de Mobius ». **Il n'y en a pas** (*mesuré*) :

- le bundle `Portals/1/2sxc/MobiusForms5/live/dist/scripts.min.js` fait 13 467 octets et ne contient **qu'une seule occurrence** du jeton `debug` : la chaîne de la ligne de console elle-même (*mesuré* sur le fichier téléchargé) ;
- l'appelant est une directive 2sxc `turnOn` : `appMobius5.init()` avec `{"domAttribute": "mobius-600"}` (*mesuré*, dans le HTML servi) ; `init` ne lit que `domAttribute` — aucun drapeau de débogage n'est lu.

**Contrôle apparié** (prod `/en/`, script Mobius bloqué par interception de requête, puis servi) :

| Bras | Lignes « Mobius5 loading, debug is enabled » | `novalidate` sur `#Form` | Erreur `setAttribute` |
|---|---:|---|---|
| Mobius bloqué | **0** | **absent** | présente |
| Mobius servi (témoin) | **1** | **présent** | présente |

Ce que fait Mobius, mesuré : un `console.log` par page, et `novalidate` posé sur **le premier formulaire de la page** — ici le formulaire DNN (le HTML brut ne contient aucun `novalidate`). L'erreur `setAttribute` est **identique dans les deux bras** : les deux symptômes sont **indépendants**.

Le formulaire de contact Mobius, lui, est bien présent (`div.app-mobius5-wrapper`, attribut `mobius-600`, service `app/auto/live/api/Form/ProcessForm?workflowId=ContactDefault`) et n'a pas à être réparé.

**Remède Mobius : rien à faire en production.** Il n'existe pas de réglage à « désactiver ». Faire disparaître la ligne supposerait de patcher un bundle tiers — à ne pas faire pour une ligne de console.

---

## 5. Le remède

### 5.1 Ce qui est impossible d'abord

| Option | Verdict | Pourquoi |
|---|---|---|
| Sortir le bloc Brevo du formulaire DNN | **impossible** | le skin 2shineBS5 enveloppe tout le corps : aucun bloc de contenu n'est hors de `#Form` (*mesuré*) |
| Supprimer le script tiers | **à écarter** | le formulaire reste sans gestionnaire d'envoi : le bouton renverrait le formulaire du site (*mesuré* §3) |
| Renoncer au formulaire (lien vers la page Brevo) | **à écarter** | le visiteur quitte le site ; la page hébergée Brevo est par ailleurs en français pour tout le monde (*mesuré* §5.3) |
| Faire poster vers Brevo sans formulaire | **hors périmètre** | supposerait notre propre JavaScript et un couplage à une API Brevo non documentée publiquement |

### 5.2 Option A — l'`<iframe>` (recommandée)

**La variante iframe du même formulaire existe déjà dans le registre EAV.** Trois copies du contenu « Newsletter » y coexistent (*mesuré*) :

| Clé du registre | App | Type | Contenu | Langues |
|---|---:|---|---|---|
| `FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91\|Html` | 48 | HtmlFragment | le snippet **avec** `<form id="sib-form">` et le script — **c'est celui qui est servi** | 8 valeurs distinctes : 12 476 – 13 314 **caractères** (13 002 – 13 938 octets UTF-8) |
| `1D949D90-51BF-48EB-A71D-3B5F6D29B5A9\|Text` | 33 | Content | **l'`<iframe>`** vers la page hébergée Brevo (447 caractères = 447 octets) | 8 (valeur **identique** dans les 8) |
| `44088360-FA3E-45CB-BFA7-81440906202D\|Text` | 33 | Content | le snippet **sans** `<form>` et **sans** le script | 8 valeurs distinctes : 5 029 – 5 763 **caractères** (5 418 – 6 117 octets UTF-8) |

L'URL du formulaire de la copie servie et l'`src` de l'iframe de la copie app 33 sont **le même octet** : 244 caractères, `https://d426a943.sibforms.com/serve/MUIEALayZPjEZxs-opk0OzYmVW3jhLQf3cnrC_OTywqy0s99xaHFqf0z3OqsCCsKf36Qvvfk6NepqHRTDHt2rWcQ9vhCIEGD4X4PM-rwWg_VZcUIRiraY8iJ9DvWbCZCFFw36up8e7ee5FPY8iDQ5sOIYn_Y0kzhxy4m3xQOfM94NmhfbKZ76hlh52U7D-GrcUHrxTpD2uD59W6O` (*mesuré*). **On ne remplace donc pas un formulaire par un autre : on rend le sien à son propre rendu.**

**Contrôle positif, mesuré** — la page Brevo hébergée, ouverte dans Chromium : HTTP 200, **0 erreur**, un formulaire `#sib-form` présent (action relative `/serve/MUIE…` sur l'hôte Brevo), bouton « S'INSCRIRE », le même `main.js` et le même `sib-styles.css` demandés. Le même script, dans un document où le formulaire n'est pas imbriqué : il trouve son élément. **C'est la démonstration que le remède fonctionne, sans rien écrire.**

**Ce que l'option A coûte** : le formulaire hébergé est en **français pour tout le monde** — la réponse a la **même longueur (15 611 octets)** pour `Accept-Language: fr-FR`, `en-US` et `zh-CN`, bouton « S'INSCRIRE » dans les trois cas (*mesuré*).

⚠️ **« Même longueur » ne veut pas dire « mêmes octets »** : le jeton Cloudflare `data-cfemail` change **à chaque requête, même langue comprise** — deux lectures `fr-FR` à quelques secondes d'écart diffèrent d'**une seule ligne sur 398**, et cette ligne est la seule (*mesuré*, ~17:05Z ; empreintes des quatre requêtes toutes distinctes). Le libellé des autres champs n'a pas été relu ligne à ligne entre les langues : le constat « français pour tout le monde » repose sur le bouton « S'INSCRIRE » lu dans les trois langues. Les 7 accueil non francophones perdent donc les libellés localisés du formulaire (le snippet in-page les porte : 8 valeurs distinctes, 12 476 – 13 314 caractères).

### 5.3 Option A′ — l'`<iframe>` qui héberge **notre** snippet localisé

Si l'owner veut garder les 8 localisations : servir les 8 valeurs localisées du registre comme pages statiques **de notre domaine**, et y faire pointer 8 `<iframe>` (une par langue). Dans ce document, le `<form id="sib-form">` est le seul formulaire : le script trouve son élément, aucune erreur, et les libellés restent dans la langue de la page.

Coût : 8 fichiers statiques dans le webroot servi (écriture web2, hors `web.config` gelé) + 8 cellules du registre. Le script tiers reste chargé, mais **dans l'iframe**, plus dans la page.

### 5.4 Comparatif

| | A — iframe Brevo | A′ — iframe sur notre snippet |
|---|---|---|
| Fichiers touchés | 8 cellules du registre | 8 cellules du registre + **8 fichiers statiques** |
| Écriture de code | aucune | un fichier HTML par langue |
| Libellés du formulaire | français partout (*mesuré*) | 8 langues conservées |
| Erreur console | 0 (*mesuré* sur la page hébergée) | 0 (le formulaire est présent dans l'iframe) |
| Retour arrière | réinjecter les valeurs d'avant | + supprimer les 8 fichiers |
| **Recommandation** | **oui** — contenu seul, rien d'inventé | si l'owner juge la localisation importante |

Ma recommandation : **A**, parce qu'elle ne demande qu'un contenu, que la valeur exacte existe déjà dans le registre, et que le contrôle positif est mesuré. La localisation est un choix produit : il appartient à l'owner et à ai-01, pas à la mesure.

---

## 6. Le geste exact

> **Le geste en production revient à web2**, après relecture du dry-run par ai-01 (règle de l'issue #2421). **po-2023 n'écrit ni en base ni sur le site.** La commande ci-dessous est celle que j'ai **exécutée en dry-run** (§6.2) ; la variante `--apply` reste à web2.

### 6.1 Étape 0 — la décision (ai-01 / owner)

A ou A′ (§5.4). Tout le reste en découle ; le geste ne change pas.

### 6.2 Étape 1 — le dry-run de l'outil de ré-injection (SELECT seuls)

État mesuré sur ce siège, base locale `ArgumentumGames` (⚠️ **ce n'est pas** la base servie : web2 devra rejouer ce dry-run sur la base servie `ArgumentumGames_staging`) :

```
registre: 673 lignes, 1806 cellules traductibles non vides, 518 cellules de base fr
carte d'apps (registre -> cible, mesuree a l'execution) :
  app 48 -> 48 (fr reconnu 1/1 ; candidats 48:1/1)
...
  SKIP_SAME: 7
base fr (dim-0) :
  FR_SAME: 1
preservation AVANT: #2181 value_id=27942 len=4421 sha256=36d90650f10e…, #2182 entites=7/7
DRY-RUN: 0 ecriture(s) dimensionnees prevues (0 INSERT + 0 UPDATE) + 0 REECRITURE(S) de base fr
VERDICT: plan coherent, pret pour --apply
```

Ce que cela dit : **l'entité se résout sans ambiguïté** dans la cible (app 48 → 48, 1/1) et **le registre et la base portent aujourd'hui la même valeur** dans les 8 langues. Une fois le registre changé (étape 2), le même dry-run doit montrer 8 cellules en conflit — c'est l'attente, pas une anomalie.

Commande (identique en dry-run et en apply, `--apply` ajouté) :

```
python tools/458-eav-reinject.py \
  --config <web.config du site servi> \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:fr \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:en \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:ru \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:pt \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:es \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:ar \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:fa \
  --only-cell FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:zh
```

Les 8 langues sont `fr` (base, dim-0) et les 7 de `LANGS` (`en ru pt es ar fa zh`) ; un nom hors de cette liste est **refusé** par l'outil, et un nom qui ne désigne aucune cellule du plan aussi (faute de frappe). Le mode `--only-cell` borne **l'écriture** ; la carte d'apps, la lecture de la cible et la préservation restent mesurées sur le registre complet (*comportement documenté de l'outil*).

### 6.3 Étape 2 — le changement de registre (dépôt)

La valeur à écrire dans les 8 cellules `FB8362B1-…|Html` est **exactement** celle de la copie app 33 (`1D949D90-…|Text`, 447 caractères = 447 octets, identique dans les 8 langues) :

```html
<p><iframe style="display: block; margin-left: auto; margin-right: auto; max-width: 100%;" src="https://d426a943.sibforms.com/serve/MUIEALayZPjEZxs-opk0OzYmVW3jhLQf3cnrC_OTywqy0s99xaHFqf0z3OqsCCsKf36Qvvfk6NepqHRTDHt2rWcQ9vhCIEGD4X4PM-rwWg_VZcUIRiraY8iJ9DvWbCZCFFw36up8e7ee5FPY8iDQ5sOIYn_Y0kzhxy4m3xQOfM94NmhfbKZ76hlh52U7D-GrcUHrxTpD2uD59W6O" width="540" height="305" frameborder="0" scrolling="auto" allowfullscreen="allowfullscreen"></iframe></p>
```

Le registre est le **producteur**, jamais un miroir : la modification se fait dans `docs/dnn-localization/eav-content.csv`, par une PR — **une seule PR à la fois sur ce fichier** (règle de la lane). L'outil accepte `--registry <chemin>` : le dry-run peut donc porter sur une **copie modifiée** du registre, sans toucher `master` ni attendre une fusion.

⚠️ **Fenêtre à tenir** : le registre et la base doivent changer **ensemble**. Entre les deux, tout dry-run de ré-injection verra 8 conflits sur cette cellule et **refusera** un apply — c'est le transitoire attendu, pas un nouveau défaut. Le plus simple : préparer la copie modifiée, la réinjecter, **puis** fusionner la PR du registre.

### 6.4 Étape 3 — l'écriture (web2)

```
python tools/458-eav-reinject.py \
  --config <web.config du site servi> \
  --only-cell … (les 8) \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:en \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:ru \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:pt \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:es \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:ar \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:fa \
  --force FB8362B1-1A0D-4621-8F64-5CCB1D6E2F91:Html:zh \
  --apply --backup-dir <hors dépôt>
```

`--force` est indispensable **pour les 7 traductions** : les deux côtés sont pleins et différents, donc l'outil refuse par défaut (CONFLICT) ; le nom de la cellule autorise **cette** cellule et rien d'autre.

⚠️ **Pas de `--force …:Html:fr`.** La langue d'un `--force` doit être l'une des **7** de `LANGS` (`en ru pt es ar fa zh`) : `parse_force` rend `None` pour `fr` et l'outil sort en `REFUS: --force mal forme` — **avant toute connexion**, donc avant toute écriture (*mesuré*, ~17:00Z, sur `master`, sans `--apply` et avec un `--config` inexistant : le refus tombe en rc 1 ; **contrôle positif** : la même commande avec `--force …:Html:en` franchit cette étape et va lire le `web.config` — l'instrument distingue bien les deux cas). La **base `fr`** de la cellule est portée par `--only-cell …:Html:fr` seule — `parse_only_cell` accepte `fr` pour la cellule de base (dim-0) — et sa dérive (`FR_DRIFT`) est **réécrite à l'apply sans `--force`** (`fr_writes`, même outil). *Signalé par la contre-revue de po-2024, rejoué et confirmé ici.*

`--backup-dir` est **exigé** en apply et doit être hors du dépôt : l'ancienne valeur des 8 cellules y est écrite **avant** la transaction (la valeur d'avant, pas l'après : c'est la base de retour arrière). L'UPDATE ré-pince la ligne lue par le dry-run, sous verrou, et exige qu'elle porte encore exactement la valeur mesurée.

### 6.5 Étape 4 — le contrôle après écriture

Sur les 8 pages d'accueil (une par langue), en lecture seule :

1. **0** `TypeError` `setAttribute` dans la console (aujourd'hui : 1 par page) ;
2. **aucune** requête vers `https://sibforms.com/forms/end-form/build/main.js` ni `sib-styles.css` depuis la page (aujourd'hui : 2 requêtes) ;
3. un `<iframe>` dont le `src` est l'URL Brevo, dans le panneau « Newsletter Sign-Up » ;
4. `document.forms` : **1** formulaire, `id="Form"` — et surtout, plus de fermeture anticipée : le `<footer>` est **dedans** (aujourd'hui il est dehors) ;
5. **verdict visuel d'ai-01** : le formulaire s'affiche dans l'accordéon, aux bonnes dimensions (l'iframe est 540 × 305, `max-width: 100%`) — la maquette change, c'est un rendu attendu et non un défaut.

Le témoin `dnn-page-witness.py --env prod` doit rester à son niveau actuel : les pages gardent leur marqueur 2sxc, leur bouton FR et leurs traductions.

### 6.6 Retour arrière

Les 8 valeurs d'avant sont dans le `--backup-dir` (JSON, une entrée par valeur touchée) ; il suffit de réinjecter ces valeurs par le même chemin (`--only-cell` sur les 8, `--force` sur les **7 traductions** seulement — la même règle qu'au §6.4 — et `--apply`), ou de restaurer les lignes par SQL. **Le gel #972 (le `web.config`) n'est pas touché** par ce geste : ni le fichier, ni la base lisent ce que l'outil ne réécrit pas (la préservation est vérifiée avant et après, dans la même transaction).

---

## 7. Ce que je n'ai pas mesuré

- **Aucun formulaire n'a été soumis**, ni en prod ni sur la page Brevo : la réception réelle d'une inscription n'est pas établie (*elle ne l'est pas non plus après le remède : il faudra une soumission volunteers pour le prouver, et elle n'appartient pas à cette mesure*).
- Le **rendu visuel** de l'iframe dans l'accordéon (dimensions, alignement, contraste) : c'est le verdict d'ai-01.
- L'**origine du « 17 »** d'ai-01 : non établie (§2.1).
- Le comportement du formulaire de contact Mobius **après** le remède : son `<form>` reste imbriqué, et il n'a pas à être réparé ; son fonctionnement n'a pas été testé (aucune soumission).
- Les pages **hors corpus** (pages d'article en détail : une seule mesurée ; pages du shop ; `?ctl=login` ; pages de compte) : le bloc est porté par le module 600, donc toute page du skin qui l'inclut est concernée — l'inventaire exhaustif des pages du skin n'a pas été fait.
- La **base servie** (`ArgumentumGames_staging`) n'a pas été interrogée : le dry-run de §6.2 a porté sur la base locale `ArgumentumGames` de ce siège. Web2 doit rejouer le dry-run sur la base servie avant l'apply.

---

## 8. Constat adjacent (cause distincte)

Le témoin de pages, rejoué en prod le 28/11 (dispatch de 12:54Z, grain #2181), trouve **8 échecs** : sur la page **Amis**, dans les 8 cultures, une réponse HTTP 200 porte le marqueur `<!--2sxc:error-->` (« Error Showing Content - please login as admin for details. »). Il s'agit du module 2sxc **597** (`data-cb-instance='597' data-cb-id='597'`), **distinct** du module 600 qui porte le bloc newsletter : sur la page Amis, le module 600 rend (le script tiers y est), et le module 597 est celui qui échoue. C'est le défaut de la famille **#2398**, pas celui de #2421 ; il est signalé sur #2398.

---

*po-2023 — mesure en lecture seule, 28/11/2026. Aucune écriture en production, aucune fusion, aucun formulaire soumis.*
