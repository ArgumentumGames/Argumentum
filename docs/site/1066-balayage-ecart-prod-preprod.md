# #1066 — Contrôle du 2026-10-06 : le gate 2 est intégralement vert

**Date** : 2026-10-06 · **Auteur** : po-2023 · **Issue** : #1066 (gate 2 — migration de contenu)
**Nature** : **mesure, lecture seule**. Aucune écriture site, aucune mutation DNN, aucun git dans un webroot.

---

## Avertissement — ce document a d'abord été écrit comme une découverte, et c'était faux

J'ai lu le **corps** de #1066 et pas ses **21 commentaires**. J'en ai tiré un rapport annonçant que « la prémisse de l'issue est périmée », en présentant comme neuf :

- la présence des balises GTM en préprod — **acquise le 12/08** (GO owner, recréation par l'admin DNN), close le 19/08 ;
- la présence des 16 wrappers `included` en préprod — **déposés le 15/08**, qualifiés au poids et clos le 16/08 ;
- l'existence de la famille `_ext` (~89 Ko, servie par la prod) — **explicitement écartée le 15/08** par ai-01 : *« Le piège `_ext` (~89 Ko, 200 lui aussi) est écarté »*.

Rien de tout cela n'était nouveau. **C'est exactement l'anti-pattern que la règle « lire le corps, tous les commentaires, toutes les revues » vise à empêcher**, et je l'ai commis en croyant bien faire — j'avais lu le corps, ce qui donne l'impression d'avoir lu l'issue. Le corps de #1066 décrit un état du 11/08 ; les commentaires portent deux mois de travail qui l'ont dépassé.

Le présent document est donc **réécrit comme ce qu'il est** : un **contrôle** daté, pas une trouvaille.

---

## Résultat du contrôle — 06/10

| volet | attendu (dernier état consigné) | mesuré 06/10 |
|---|---|---|
| **2a** balises GTM/GA | identifiants présents ; ~~`<noscript>` absent~~ | ✅ **`GTM-TZBQ57M` 18/18 · `G-VHLTL18PEW` 18/18 · `gtm.js` 18/18 · `gtag.js` 9/9 · `<noscript>` 9/9 · `ns.html` 9/9** — **identique à la prod sur 9/9 pages** |
| **2b** 16 wrappers mindmap | 16/16 en 200 des deux côtés | ✅ présents des deux côtés |
| **2c** 5 artefacts | déposés le 19/09, à l'octet | ✅ **4 zips identiques prod↔préprod à l'octet** (50 354 291 · 39 423 798 · 14 933 310 · 6 417 247, coquille `Argmentum_` comprise) · OWL identique (4 795 192 o des deux côtés) |
| **CSP** 3 entrées `conditional` | actives depuis le 19/08 | ✅ actives : `script-src … googletagmanager.com` · `frame-src 'self' … googletagmanager.com` · `connect-src … region1.google-analytics.com` |
| **skin** `2shinebs5` | rendu depuis le fix #1129 | ✅ **rendu** (0 `Xcillion`) |

### Le seul point réellement neuf : le `<noscript>` GTM est comblé

C'était le **dernier résidu** de cette issue, tel que consigné le **26/08** :

> *« GTM n'est plus absent de la préprod, il lui manque la moitié `<noscript>` »* — préprod : `GTM-TZBQ57M` **1**, `<noscript>` **0**.

Mesure du 06/10, 9 pages, cache-bustées, des deux côtés :

| marqueur | prod | préprod |
|---|---:|---:|
| `GTM-TZBQ57M` | 18 | **18** |
| `G-VHLTL18PEW` | 18 | **18** |
| `gtm.js` | 18 | **18** |
| `gtag.js` | 9 | **9** |
| `ns.html` (iframe du `<noscript>`) | 9 | **9** |
| `<noscript>` | 9 | **9** |

**La préprod est désormais rigoureusement identique à la prod sur tous ces marqueurs.** Le résidu du 26/08 ne se reproduit pas. *(Réserve : ce comptage établit la parité de balisage, pas que les beacons partent — le beacon `region1…/g/collect` en 204 avait été constaté le 19/08 après activation de la CSP, et je ne l'ai pas rejoué ici.)*

### Un chiffre que j'avais lu de travers, et la cause de l'erreur

⚠️ **Erratum — contrôle d'ai-01 (06/10).** Mon premier rapport annonçait un « changement » de l'OWL servi : « **4 786 353 o** des deux côtés, contre **4 795 192 o** au 19/09 ». C'était **faux**. Les **4 786 353 sont des caractères** (la valeur `.Length` de ma chaîne lue en PowerShell), pas des octets. Le fichier fait **4 795 192 octets des deux côtés, au sha256 identique** — l'OWL n'a **pas changé** entre le 19/09 et le 06/10. Comparer un nombre de caractères à un nombre d'octets, puis conclure à un « changement », était une erreur d'instrument.

Je n'ai pas fait que mal mesurer : j'avais **inventé une cause** — « une régénération cohérente » — pour un écart qui n'existait pas. La prod est **gelée** : elle ne se régénère pas. Inventer une explication cohérente à une donnée mal lue est exactement l'anti-pattern « conclusions calculées ».

Leçon mesurée : **un écart ne se mesure pas contre une note, et surtout pas en confondant caractères et octets.** Le contrôle à faire était trivial — compter les **octets** du fichier servi (comme pour les 4 zips ci-dessus), pas la longueur d'une chaîne décodée.

La bonne mesure, re-faite le 06/10 : **l'OWL servi est identique prod↔préprod à l'octet** (4 795 192 o des deux côtés), comme les 4 zips. Le volet 2c est donc **intégralement sain** — ce que le tableau ci-dessus aurait dû dire d'emblée.

---

## Ce que ce contrôle ne fait pas

- Il **ne dépose rien** : tout est en lecture seule.
- Il **ne clôt pas** l'issue — c'est l'arbitrage d'ai-01.
- Il **ne rejoue pas** le beacon `region1…/g/collect` ni le journal réseau Playwright (fait le 19/08) : la parité de balisage ne prouve pas que la chaîne d'analytics fonctionne de bout en bout.
- Il **ne couvre pas** la prod, sinon en lecture : ⛔ aucune écriture, prod non servie par cette machine.

---

## Instrument — arrêté, et pourquoi

`tools/cutover-gap-sweep.py` (crawl BFS des deux hôtes + sonde de chaque chemin) avait été écrit pour §2c, dont je croyais l'inventaire à faire. **Il a été arrêté à 450/2192 chemins** : l'inventaire existait déjà — ai-01 l'a énuméré le 14/09, et l'issue porte depuis le 18/08 deux passes complètes (externe + filesystem). Poursuivre aurait été **1 à 2 h de charge sur la prod pour recalculer un résultat connu**, et l'issue avait explicitement désigné ce balayage comme déjà fait.

L'outil reste committé, avec ses pièges documentés — il est réutilisable au **prochain** cutover, où l'inventaire redeviendra une question ouverte.

### Les pièges rencontrés, dont un de ma fabrication

Toute affirmation ci-dessus a un **témoin négatif** à côté (une URL inexistante qui rend 404 des deux côtés) — sans lui, aucun « 200 » n'est interprétable.

| piège | ce qu'il a produit | résolution |
|---|---|---|
| **Mon propre `catch` fabriquait des 404** | Une passe entière a rendu « 404 » sur des fichiers qui venaient de rendre 2,7 Mo | `Headers['Last-Modified']` rend un **`System.String[]`** en PowerShell 7 ; le cast en `[datetime]` lève, et mon `catch { '404' }` traduisait un échec d'instrument en **verdict d'absence**. **Un `catch` qui écrit un verdict transforme une erreur d'instrument en fait.** Trois lectures avant de le voir. |
| **`HEAD` non fiable ici** | 404 sur tout, GET 200 sur les mêmes URL | Même classe que le **405 HEAD / 200 GET de Twitch** déjà documenté. **Toujours `GET` sur ce site.** |
| **Nom de fichier deviné** | `/virtues_fr.content.svg` → 404, lu comme « compagnon absent » | Nom réel : **`Argumentum_Virtues_MindMap_fr.content.svg`** — 200. J'avais supposé une symétrie de nommage inexistante. |
| **Écart mesuré contre une note** | « l'OWL a un écart » | Comparé à 4 795 192 (valeur du 19/09) au lieu de la prod. Comparer à l'autre hôte, pas à un chiffre recopié. |

---

## Reproduibilité

```bash
python tools/cutover-gap-sweep.py --out /tmp/cutover-gap-sweep.json --max-pages 60
```

Crawl des deux hôtes, collecte des ressources référencées, sonde de chaque chemin des deux côtés, verdict par chemin. **Lecture seule stricte** (GET uniquement, aucun formulaire, aucun POST, aucun en-tête d'authentification), cadencé. La sonde complète est lente (~3 s/chemin) : borner `--max-pages` pour un contrôle rapide.

---

*Rédigé par po-2023, 2026-10-06. Contrôle en lecture seule ; aucune écriture sur l'un ou l'autre site.*
