# #1066 — Balayage systématique de l'écart de contenu prod ↔ préprod

**Date** : 2026-10-06 · **Auteur** : po-2023 · **Issue** : #1066 (gate 2 — migration de contenu)
**Nature** : **mesure, lecture seule**. Aucune écriture site, aucune mutation DNN, aucun git dans un webroot.

---

## En une phrase

Le balayage demandé par §2c a été construit et exécuté. **Avant lui**, les deux écarts déjà connus ont été re-mesurés un par un — et **les deux sont périmés** : les balises GTM/GA sont présentes et bien formées sur la préprod (9/9 pages), et la famille de wrappers que l'issue décrit comme manquante (`included`) y est déployée **depuis le 21/09**. L'écart réel porte sur **une autre famille** (`_ext`) et sur ses compagnons `*.content.svg`.

Les deux constats qui suivent **ne retirent rien** à #1066 : ils déplacent sa cible.

---

## Instrument — quatre pièges traversés, trois écartés par un témoin

Toute affirmation ci-dessous a été obtenue avec un **témoin négatif** à côté : une URL délibérément inexistante (`/ce-fichier-nexiste-pas-12345.html`) qui rend **404 des deux côtés**. Sans ce témoin, aucun « 200 » n'est interprétable.

| Piège | Symptôme | Résolution |
|---|---|---|
| **404 fabriqué par mon propre `catch`** | Une passe entière rend « 404 » sur des fichiers qui venaient de rendre 2,7 Mo | `Headers['Last-Modified']` rend un **`System.String[]`** en PowerShell 7 ; le cast en `[datetime]` lève, et mon `catch { '404' }` traduisait l'échec de cast en « fichier absent ». **Un `catch` qui écrit un verdict transforme une erreur d'instrument en fait.** Trois lectures avant de le voir. |
| **`HEAD` non fiable ici** | `HEAD` rend 404 sur tout, `GET` rend 200 sur les mêmes URL | Même classe que le **405 HEAD / 200 GET de Twitch** déjà documenté (grain C). **Toujours `GET` sur ce site.** |
| **Nom de fichier deviné** | `/virtues_fr.content.svg` → 404, donc « compagnon absent » | Le nom réel est **`Argumentum_Virtues_MindMap_fr.content.svg`** — présent, **200**, 432 907 o. J'avais supposé une symétrie de nommage qui n'existe pas. **Un 404 sur un nom qu'on a deviné ne prouve rien.** |
| **Redirection non suivie** | déjà documenté sur ce site | `-L` / redirections suivies partout. |

---

## §2a — Balises Google Tag Manager / Analytics : **déjà en place sur la préprod**

L'issue (mesure du 2026-08-11) annonçait **7 occurrences × 9/9 pages en prod, 0 × 9/9 en préprod**.
Re-mesuré le 06/10, cache-busté, redirections suivies, 9 pages de chaque côté :

| page | prod `GTM-TZBQ57M` / `G-VHLTL18PEW` | préprod `GTM-TZBQ57M` / `G-VHLTL18PEW` |
|---|---|---|
| `/` · `/actus` · `/Règles` · `/Argumentation` · `/Téléchargements` · `/Amis` · `/Acheter-le-jeu` · `/terms` · `/privacy` | **2 / 2** (HTTP 200, 9/9) | **2 / 2** (HTTP 200, 9/9) |

**La forme a été contrôlée, pas seulement la présence** (un identifiant dans un commentaire ne s'exécute pas). Sur la préprod, les balises sont **complètes et correctement construites** :

```html
<noscript><iframe src="https://www.googletagmanager.com/ns.html?id=GTM-TZBQ57M" …></iframe></noscript>
<script>(function(w,d,s,l,i){ … })(window,document,'script','dataLayer','GTM-TZBQ57M');</script>
<script async src="https://www.googletagmanager.com/gtag/js?id=G-VHLTL18PEW"></script>
```

⚠️ **Ce que cette mesure ne dit pas** : que GTM *fonctionne*. La présence du script est nécessaire, pas suffisante — les entrées CSP `conditional` de #1065 restent à activer (#1064). **§2a n'est pas « fait », il est « plus bloqué sur l'absence des balises ».**

---

## §2b — Wrappers de cartes mentales : **la cible de l'issue est la mauvaise famille**

L'issue demande de déposer « les **16** wrappers **`included`** », et avertit : *« c'est la famille `included` (~2,4 Mo, `<svg>` inline, autoportante) que sert la prod — **pas** `_ext` (~89 Ko, `<object>` → `.content.svg`) ; copier la mauvaise famille produit 16 pages vides »*.

Re-mesure complète, **8 langues × 2 familles × 2 hôtes**, avec témoin négatif :

| artefact (racine du site) | prod | préprod | verdict |
|---|---|---|---|
| `fallacies_<lg>.html` (famille **`included`**, 2,4–4,9 Mo) | 200 | **200** | **sain — 8/8** |
| `virtues_<lg>.html` (famille **`included`**) | 200 | **200** | **sain — 8/8** |
| `fallacies_<lg>_ext.html` (famille **`_ext`**, 89 Ko) | 200 | **404** | **MANQUE — 8/8** |
| `virtues_<lg>_ext.html` (famille **`_ext`**) | 200 | **404** | **MANQUE — 8/8** |
| `Fallacies_<lg>.content.svg` (2,3–4,9 Mo) | 200 | **404** | **MANQUE — 8/8** |
| `Argumentum_Virtues_MindMap_<lg>.content.svg` (411–1100 Ko) | 200 | **404** | **MANQUE — 8/8** |
| `Fallacies_fr.links.svg` | 200 | 200 | sain |
| `Fallacies_<lg>.links.svg` (autres langues) | 404 | 404 | absent des deux — pas un écart |

### Ce que ça change

1. **Le compte de 16 de l'issue est exact — la famille est fausse.** Les 16 wrappers absents de la préprod sont les **`_ext`** (8 `fallacies` + 8 `virtues`), pas les `included`. Ces derniers sont **présents des deux côtés**.
2. **La cause est datée.** Les `included` de la préprod portent `Last-Modified = **2026-09-21 16:14**` ; ceux de la prod `2026-09-13 01:16`. L'issue a été écrite le **11/08** : la préprod a reçu la famille `included` **après**. La prémisse n'était pas fausse, elle est **périmée**.
3. **L'avertissement de l'issue reste juste sur le mécanisme.** Les `_ext` utilisent `<object>` → `.content.svg`. Copier les **16 `_ext` sans les 16 `*.content.svg`** donne bien **16 pages vides** — c'est exactement le risque décrit. Le nom de la famille était inversé, pas le raisonnement.
4. **L'impact annoncé n'est pas reproduit.** L'issue écrit : *« `/Acheter-le-jeu` lie "taxonomie" → `fallacies_fr.html` et "plusieurs langues" → `fallacies_en.html`. Ces deux liens cassent post-cutover. »* Mesuré : ces deux URL rendent **200 sur la préprod aujourd'hui** (2 708 454 o et 2 568 441 o). **Aucun lien de `/Acheter-le-jeu` ne casse.**
5. **Rien ne référence la famille `_ext`.** 0 occurrence de `_ext.html` sur les 6 pages principales de la prod (`/`, `/Argumentation`, `/Acheter-le-jeu`, `/Règles`, `/Amis`, `/Téléchargements`) ; les wrappers `included`, eux, **sont** référencés (`/fallacies_fr.html` sur `/` et `/Argumentation`). Les 16 `_ext` de la prod sont donc des fichiers **servis mais non liés**.

### Précision sur « 3 compagnons par wrapper »

L'issue annonce **3 compagnons** par wrapper `_ext`. C'est le compte **côté dépôt** (`Fallacies_fr.content.svg`, `Fallacies_fr.links.svg`, `Fallacies_fr.svg`). **La prod n'en sert que deux** : `Fallacies_fr.svg` y rend **404** (les deux hôtes sont d'accord sur ce point). Copier « les 3 compagnons » déposerait donc **un fichier de trop**, et le strict nécessaire pour que les `_ext` s'affichent est **le `.content.svg`** (leur `<object>` ne référence que lui : 1 référence, contrôlée dans le HTML servi). Le `.links.svg` est un compagnon de la vue « liens », déjà présent sur la préprod pour `fr`.

### Portée honnête de ce constat

La mesure établit que **la préprod sert aujourd'hui ce que les pages référencent**. Elle **ne peut pas** établir que le cutover conservera ce service : si la bascule change la racine web ou la façon dont ces fichiers sont servis, un fichier présent aujourd'hui peut disparaître demain. **Ce point reste à vérifier au moment du cutover**, et c'est le seul risque résiduel de §2b.

---

## Reproductibilité

```bash
python tools/cutover-gap-sweep.py --out /tmp/cutover-gap-sweep.json
```

L'instrument crawle les deux hôtes, collecte toutes les ressources référencées, sonde chaque chemin des deux côtés et classe par verdict. Il est **en lecture seule** (GET uniquement, aucun formulaire, aucun POST, aucun en-tête d'authentification) et cadencé volontairement.

---

*Rédigé par po-2023, 2026-10-06. Mesures en lecture seule ; aucune écriture sur l'un ou l'autre site.*
