# Pool #458 item 5 — mesures Tier 3 : contact Mobius5 + newsletter Brevo (lecture seule)

**Date :** 2026-10-25 · **Machine :** myia-po-2023 · **Grain :** mesure seule (arbitrage 3) — aucun
écrit en base, aucun code, aucun déploiement. **Portée :** préprod `dnn.argumentum.myia.io`, master
`10aeb190` servi.

Objet : établir, pour les deux surfaces Tier 3, **ce qui est réellement servi**, **où vivent les
chaînes**, et **ce qui ne doit jamais bouger** quand on les localisera (garde demandée sur l'`action`
et les champs cachés).

## 1. Instruments

| Quoi | Comment |
|---|---|
| HTML servi | 8 routes (`/`, `/en`, `/es`, `/pt`, `/ru`, `/zh`, `/ar`, `/fa`), cache-bustées, **200 ×8**, octets décodés explicitement en UTF-8 |
| Stockage | lecture seule de la base servie (chaîne lue depuis le `web.config` vivant, jamais écrite ici) |
| Sources applicatives | `Portals/1/2sxc/MobiusForms5/` sur disque |
| Données personnelles | **jamais lues au-delà d'un décompte, jamais citées, jamais commitées** |

## 2. Newsletter Brevo — servie, non localisée (8/8)

Bloc délimité `<form id="sib-form" …>` → `<!-- End Sendinblue Form -->` : **8 844 caractères**,
`sha256` (16 premiers) **`4D152C2C22BCDEC1`**, **identique sur les 8 langues**. Une seule occurrence
du bloc par page (1 marqueur de début, 1 `id="sib-form"`, 1 marqueur de fin).

**Identité de la copie servie — mesurée, pas supposée.** Deux copies existent en base :

| Copie | Contenu | Tranche `<form id="sib-form">` → fin | Dimensions |
|---|---|---|---|
| **app 48**, `HtmlFragment` 10181 (`Title` = « Newsletter ») | `Html` 13 314 car. | **8 844** — égale exactement le servi | **0** |
| app 33, `Content` 10176 | `Text` 5 763 car. (+ 7 dimensions) | **absente** du bloc | 7 (`en-US`, `ru-RU`, `pt-BR`, `es-ES`, `ar-SA`, `fa-IR`, `zh-CN`) |

⇒ la copie servie est **app 48** ; la copie localisée d'app 33 est **une autre variante**, qui ne
contient pas ce formulaire (0 occurrence de sa signature sur la page servie). Conséquence pratique :
une passe de traduction qui viserait app 33 « parce qu'elle est déjà localisée » ne toucherait pas ce
que le visiteur voit.

### 2.1 Garde — ce qui ne doit pas bouger

| Élément | Valeur mesurée |
|---|---|
| `id` / `method` / `data-type` | `sib-form` / `POST` / `subscription` |
| `action` | `https://d426a943.sibforms.com/serve/MUIEALay…` (hôte + jeton Brevo, externe) |
| champ caché | **exactement 1** : `<input type="hidden" name="locale" value="fr">` |
| anti-robot | `<input type="text" name="email_address_check" value="" class="input--hidden">` — présent et vide |
| consentement | `<input type="checkbox" … id="OPT_IN" name="OPT_IN" value="1">` |
| saisie | `<input … id="EMAIL" name="EMAIL" … placeholder="EMAIL" data-required="true" required>` |
| envoi | bouton de valeur `S&#039;INSCRIRE` |

Le champ `locale=fr` est le **seul** champ caché, et le seul dont la *valeur* est un levier de
langue (c'est lui qui pilote la langue de la page de confirmation Brevo). Toute modification de sa
valeur doit être **mesurée contre l'écho de Brevo**, jamais supposée.

### 2.2 Piège d'instrument — l'imbrication (corrigé en cours de mesure)

Le formulaire Brevo est **imbriqué dans le formulaire maître DNN** (`<form method="post"
action="/Argumentum/cdv/…" id="Form" enctype="multipart/form-data">` sur `/`, `/ar?cdv=…` sur `/ar`).
Une énumération par expression régulière ne voit **que 2 formulaires** et attribue les champs cachés
au mauvais propriétaire : une première lecture a compté `locale` parmi les champs cachés DNN.
Mesure corrigée, en excluant le bloc Brevo du formulaire maître : **DNN porte 8 champs cachés**
(`__EVENTTARGET`, `__EVENTARGUMENT`, `__VIEWSTATE`, `__VIEWSTATEGENERATOR`,
`__VIEWSTATEENCRYPTED`, `__EVENTVALIDATION`, `ScrollTop`, `__dnnVariable`) et **aucun `locale`**.

### 2.3 Surface traduisible

**10 nœuds de texte distincts** : titre « Newsletter » · accroche (« Inscrivez-vous à notre
newsletter… ») · deux consignes de saisie (dont un exemple `abc@xyz.com`) · consentement · mention de
désinscription · mention Sendinblue + lien « conditions générales d'utilisation » · bouton
`S'INSCRIRE`. Le `placeholder` vaut `EMAIL` (littéral technique, non traduisible).

## 3. Contact Mobius5 — servi, non localisé (8/8)

**Aucune balise `<form>`** : le module rend un conteneur piloté en JavaScript.

```html
<div class="app-mobius5-wrapper" mobius-600
     data-webservice="app/auto/live/api/Form/ProcessForm?workflowId=ContactDefault">
```

Initialisation : `turnOn turn-on='{ "run": "window.appMobius5.init()", "data": { "domAttribute":
"mobius-600" } }'`. Ressources : `/Portals/1/2sxc/MobiusForms5/live/dist/{styles,scripts}.min.*`.
Modèle : `Portals/1/2sxc/MobiusForms5/live/_Contact Form.cshtml` (1 262 octets) — `workflowId`
`ContactDefault` **en dur**, `domAttribute = "mobius-" + CmsContext.Module.Id`, champs
`Text("Subject")`, `Multiline("Message")`, `Text("SenderName")`, `EMail("SenderMail")`.

**Garde :** `data-webservice` (chemin + `workflowId=ContactDefault`), attribut `mobius-600`
(= `<ModuleId>`), et les 4 noms de champs `Subject` / `Message` / `SenderName` / `SenderMail` — ce sont
eux que le POST transporte, pas des libellés.

**Chaînes — mesurées identiques sur les 8 langues** : un **seul jeu de 12 valeurs** (empreinte du jeu
trié `58EBE3D6EE4D0EC1`), soit 4 libellés (`Sujet`, `Message`, `Nom`, `E-mail`) + 7 messages + le
bouton `Envoyer`. Le formulaire est **bilingue dans sa propre langue** : `msgRecap` (« Please fill in
all required fields before sending. ») et `msgError` (« An unexpected error occured while sending… »)
sont en anglais, tout le reste en français ; le message de succès porte des entités HTML (`&eacute;`).

**Stockage — EAV, pas le disque.** Les chaînes vivent en **app 50, entité 9169** : 43 champs non vides
(`LabelSubject`, `LabelMessage`, `LabelSenderName`, `LabelSenderMail`, `ButtonSend*`, `Message*`,
`Mail*`, `Owner*`, `Customer*`, `TermsLabel`, `GdprLabel`, `LabelValidEmail`…) **tous à `dims=0`**.
Recherche disque dans le dossier de l'app : **0 fichier** contient « Sujet », « Envoyer », « Entrez
tous les champs » ou « Message en cours d'envoi » — le modèle ne fait que câbler les champs.

**Données personnelles :** le même app 50 contient aussi les **soumissions** des visiteurs (entité
`RawData`). Elles n'ont pas été lues au-delà de leur existence, et **rien n'en est exporté, cité ni
commité** (le dépôt est public). Toute passe future doit garder cette frontière.

## 4. Levier identifié pour le grain de correction (non exécuté ici)

Les deux surfaces se localisent par le **même mécanisme** : une ligne de dimension par culture sur les
valeurs EAV concernées (app 48 `Html` ; app 50 `Label*` / `Message*` / `Button*`). Le précédent est
déjà mesuré sur cette installation — la copie app 33 porte **7 dimensions** (`en-US`, `ru-RU`,
`pt-BR`, `es-ES`, `ar-SA`, `fa-IR`, `zh-CN`) plus une base FR sans dimension.

Pour Brevo, la passe devra traduire **les seuls nœuds de texte** et préserver à l'octet près
`action`, `method`, `id`, `data-type`, le champ `email_address_check` (anti-robot) et `OPT_IN` ; la
valeur de `locale` est le point à mesurer avant de la faire varier.

## 5. Note d'instrument

Une passe intermédiaire a rendu **404 sur les 7 routes de culture d'affilée**, alors que `/ar` venait
de répondre 200 : c'était une **fenêtre de recycle** du domaine applicatif, pas une régression — la
re-mesure immédiate donne **200 ×8**. Un 404 **uniforme sur toutes les routes de culture** se lit donc
comme un redémarrage, jamais comme un défaut de contenu, et ne doit pas devenir un verdict.

## 6. Verdict

Deux surfaces Tier 3 mesurées, **toutes deux non localisées sur 8/8**, avec leurs gardes documentées
et la copie réellement servie identifiée sans ambiguïté. Aucun écrit, aucun code, aucun déploiement.

*po-2023*
