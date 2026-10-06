# Grains D / E / F — mesures et recherches (#1781, 06/10)

**Statut** : mesures et recommandations. **Aucune écriture** ni sur le site ni en base n'a été faite pour ces trois grains.
**Méthode** : chaque chiffre ci-dessous est mesuré à l'instant de la rédaction, avec son instrument cité. Les estimations sont explicitement marquées comme telles.

---

## Grain D — Page « Acheter le jeu » : temps de réponse

**Question posée** (Adeline) : la page ne fonctionne pas / reste grisée au chargement.

**Mesure** — 5 chargements par site, URL cache-bustée (`?cb=<aléatoire>`), réponse du serveur jusqu'au HTML complet :

| Site | min | médiane | max | Codes HTTP |
|---|---:|---:|---:|---|
| **préprod** (`dnn.argumentum.myia.io`) | 52 ms | **60 ms** | 176 ms | 200 ×5 |
| **prod** (`www.argumentum.games`) | 170 ms | **176 ms** | 272 ms | 200 ×5 |

**Lecture.** Les deux sites répondent, et la préprod est **~3× plus rapide** en médiane. Sur 10 chargements, **aucun état grisé n'a été reproduit**. La page HTML (38–39 Ko) arrive en moins de 300 ms des deux côtés.

**Ce que cette mesure ne couvre pas.** Elle mesure la réponse serveur du HTML, pas l'état « utilisable » après exécution du JavaScript. Le grisé, s'il existe, vient du module e-commerce NBrightBuy qui peuple la liste d'articles en AJAX (`XmlConnector.ashx?cmd=itemlist_getpopup`) : il se produirait **après** cette mesure. Sur les 10 chargements, il ne s'est pas produit.

**Ce qui explique le ressenti d'Adeline.** La page affiche « Pas de stock disponible » — c'est-à-dire qu'elle **fonctionne** et dit honnêtement qu'on ne peut pas commander. Cela recoupe la réponse déjà faite sur #1781.

**Constat annexe** (relevé pendant la mesure) : la page Acheter contient un titre markdown **non rendu**, affiché littéralement `## En Rupture ##`. C'est un défaut d'affichage réel, distinct du délai.

---

## Grain E — Source stable pour le lien « code de conduite intellectuel »

**Contexte** : sur l'article « Interview des co-créateurs », les liens « classification de McCandless » et « code de conduite intellectuel » pointent vers la **même URL**. Il faut une seconde source.

**Le bon texte est identifié** : le *Code of Intellectual Conduct* de **T. Edward Damer**, dans *Attacking Faulty Reasoning* (12 principes, 6ᵉ éd. 2009, p. 7-8).

**Les candidats, testés un par un** (HTTP + présence effective du texte du code dans la page) :

| URL | HTTP | Taille | Contient le code ? | Verdict |
|---|---:|---:|---|---|
| `en.wikipedia.org/wiki/Attacking_Faulty_Reasoning` | 200 | 81 Ko | **non** | référence bibliographique, pas la source du texte |
| `joshuapsteele.com/damers-code-of-intellectual-conduct/` | 200 | 54 Ko | **oui** (61 `<li>`) | contenu complet |
| `limbicnutrition.com/blog/damers-code-of-intellectual-conduct/` | 200 | 47 Ko | **non** (26 `<li>`) | **à écarter** — l'URL survit mais ne porte plus le texte |

> ⚠️ Mesuré, pas supposé : `limbicnutrition.com` **répond 200** tout en n'étant plus la source du code. Un simple test de statut l'aurait gardé à tort.

**Recommandation.** Retenir **`joshuapsteele.com/damers-code-of-intellectual-conduct/`** comme source du texte (seule vérifiée complète), et citer l'ouvrage en clair dans la phrase du lien (« le code de conduite intellectuel de T. Edward Damer ») plutôt que de faire reposer l'information sur une URL tierce. Réserve honnête : c'est un blog personnel, pas une source institutionnelle — sa pérennité n'est pas garantie, d'où l'intérêt de nommer l'auteur et le livre.

---

## Grain F — Bibliothèque d'affichage de l'OWL

**Contrainte structurante, mesurée** : **WebVOWL ne lit pas un fichier `.owl`.** Il affiche du *VOWL-JSON*, produit par le convertisseur **OWL2VOWL** (outil Java). Pour un site statique, la conversion se fait donc **une fois, hors ligne**, et le JSON est committé puis servi comme un fichier ordinaire — aucune conversion à l'exécution.

### Poids

| Élément | Mesure |
|---|---:|
| Visualiseur WebVOWL (d3 152 Ko + webvowl.js 751 Ko + app 350 Ko + CSS 47 Ko) | **1,24 Mo** |
| Ontologie Argumentum convertie (`argumentum.owl` 5,72 Mo → VOWL-JSON) | **3,73 Mo** |
| **Total d'une page avec visualiseur** | **≈ 4,97 Mo** |

Conversion mesurée : **1,3 s**, sans erreur. Contrôle de proportionnalité : l'ontologie des vertus (1,05 Mo) donne 680 Ko de JSON, soit le même ratio **×0,65**.

### Licence et maintenance

| Candidat | Licence | État | Verdict |
|---|---|---|---|
| **WebVOWL** (`VisualDataWeb/WebVOWL`) | **MIT** | actif (avril 2026), 1 003 ★, démo vivante `service.tib.eu/webvowl/` | **retenu** |
| OWL2VOWL (convertisseur, requis) | MIT | actif (juin 2026) | requis |
| Ontodia | *NOASSERTION* | **archivé** (déc. 2022) | **écarté** |
| cytoscape.js | MIT | actif | générique : ni OWL, ni notation VOWL — développement sur mesure |

> Note : `visualdataweb.org` répond encore 200 mais **n'est plus à eux** (le README du dépôt le dit explicitement). La démo de référence est `service.tib.eu/webvowl/`. `webvowl.imld.de` est mort (DNS).

### Faisabilité sur *notre* ontologie — la réserve qui compte

**Mesure sur l'artefact réellement servi** (`/Argumentation/Ontologiefallacieuse`, 302 → GitHub Pages, **5 993 701 o**, récupéré et compté le 06/10) :

```
1 882 classes · 928 propriétés d'objet · 1 760 individus · 38 SubClassOf
format OWL/XML (racine <Ontology>, et non <rdf:RDF>)
IRI : https://www.argumentum.games/argumentum_fallacies.owl#
licence : CC BY-SA 4.0 (dcterms:license)
```

⚠️ **Correction d'une mesure antérieure de ce document.** Une première version annonçait « classes 1495 · propriétés d'objet 10 · axiomes 18926 · individus 719 » — chiffres obtenus par conversion **OWL2VOWL d'une copie locale en retard**, sur un **autre instrument** (métriques du VOWL-JSON) et un **autre fichier** que celui servi. Les deux séries ne sont pas comparables. Les chiffres ci-dessus sont ceux de l'artefact publié, comptés directement sur ses éléments.

La réserve reste donc valable, et même **renforcée** : ~1 900 classes et ~930 propriétés d'objet, soit un graphe bien plus relationnel que taxonomique. WebVOWL le rendrait en **pelote** dense et peu navigable (limite connue de l'outil sur les grandes ontologies).

**Recommandation pour le grain K.** Le fichier reste le livrable (décision owner). Pour la page lisible :
1. **Page de présentation** : expliquer en français ce qu'est cette ontologie, à quoi elle sert, comment l'ouvrir (Protégé…), avec le lien de téléchargement. C'est le besoin réel — aujourd'hui il n'y a **aucune page** (voir ci-dessous).
2. **Visuel** : réutiliser les **cartes mentales SVG déjà générées** (8 langues, déjà dans le dépôt, poids léger) plutôt qu'un visualiseur de 5 Mo qui rendrait une pelote. Elles montrent la même taxonomie sous une forme déjà lisible.
3. **WebVOWL** : possible (MIT, 1,24 Mo + 3,73 Mo), mais à réserver à un public qui veut explorer le graphe formel, et à charger **derrière un clic** (« explorer le graphe »), jamais au chargement de la page.

---

## Constat versé au grain K — la page « Ontologie fallacieuse » n'existe pas

**Mesuré** : l'URL `https://dnn.argumentum.myia.io/Argumentation/Ontologie-fallacieuse` **est le fichier d'ontologie lui-même** — 5 993 701 octets servis en `application/rdf+xml`, dont le « texte visible » est l'en-tête OWL/XML (déclaration, licence CC BY-SA 4.0, IRI).

Il n'y a donc pas de page à améliorer : **il y a un fichier brut servi à la place d'une page**. C'est exactement ce qui produit le « ça ne fonctionne pas » d'Adeline — et cela confirme la réponse déjà faite sur #1781 (« le lien ouvre un fichier technique brut »).

**Point de vigilance levé.** Le fichier servi est la version **corrigée** : son fragment d'IRI est `#callingCards`, sans guillemet. Le défaut d'IRI illégale (`calling"Cards"`, rejeté par OWLAPI) a été corrigé par #1651 (clos le 30/09) et **n'est plus dans l'artefact servi**. Il subsiste uniquement dans les **copies locales en retard** — dont celle de cette machine (6,52 Mo contre 5,72 Mo) — ce qui en fait un piège de mesure : travailler sur la copie locale fait croire à un défaut qui n'existe plus en production.

*Rédigé par po-2023, 06/10. Aucune écriture site/base pour D, E et F.*

---

# Grain I — lien Fanny Bénard : **POSÉ et vérifié**

**Lien** : `https://mairie18.paris.fr/pages/fanny-benard-12740` → **HTTP 404** (mesuré). Le site de la mairie, lui, répond 200 : c'est la page qui a disparu, pas le site.

**Copie archivée : elle existe.** Index CDX de Wayback, sur l'URL exacte — deux captures, toutes deux en HTTP 200 :

| Capture | Poids | Nom présent | Fonction présente |
|---|---:|---|---|
| **20251215104931** (15/12/2025) | 313 Ko | oui | oui |
| 20250125053453 (25/01/2025) | 135 Ko | oui | oui |

La règle owner (« Wayback si copie existe, sinon retirer le lien en gardant nom et fonction ») s'applique donc dans sa **première branche**.

**Pose** — entité E10080 (`Content`, V27402), article « Lancement d'Argumentum - Table ronde ». Backup de l'entité avant écriture (`Logs/1781-eav-backup-20261006-1120-grainI/`). Remplacement ciblé de la seule URL, dry-run (chaîne présente ×1), UPDATE 1 ligne, recycle d'app-domain, puis contrôle sur la page **servie** :

| Contrôle | Attendu | Mesuré |
|---|---:|---:|
| lien mort en `href` | 0 | **0** |
| lien Wayback en `href` | 1 | **1** |

Page de contrôle : `https://dnn.argumentum.myia.io/actus/details/lancement-d-argumentum-table-ronde` (la page `/Actus/<clé>` rend la **liste**, pas le détail — piège d'instrument rencontré et corrigé).

**Précision d'écriture** : la base interrogée est celle de la **préprod seule** — vérifié par contrôle indépendant sur le pied de page (`Retrouvez-nous` en préprod, `Retrouvez nous` encore en prod). Recoupé par DNS : `dnn.argumentum.myia.io` → **127.0.0.1** (IIS local), `www.argumentum.games` → **51.75.200.22** (serveur distant). La prod n'est pas sur cette machine.

**Point non modifié, signalé** : dans ce même lien, le point final est **à l'intérieur** de l'hyperlien (`…participation citoyenne.</a>`). Correction typographique hors du périmètre approuvé — signalée, pas posée.

---

# Grain C — liens « Retrouvez-nous », contact, newsletter : **mesuré, aucun envoi**

### Liens du pied de page — tous sains

| Lien | Statut |
|---|---|
| GitHub `ArgumentumGames/Argumentum` | 200 |
| Twitter `argumenteam` | 200 |
| Facebook `argumenteam` | 200 |
| Youtube (chaîne) | 200 |
| Twitch `argumenteam` | **200 en GET** |

> ⚠️ Twitch renvoie **405 à une requête HEAD** et **200 à une requête GET**. Une sonde HEAD l'aurait déclaré cassé à tort : ce 405 est une limite de l'instrument, pas du lien.

### Newsletter — formulaire présent et joignable

Formulaire hébergé **Brevo/Sendinblue** (`<form id="sib-form" method="POST" action="https://d426a943.sibforms.com/serve/MUIE…">`), endpoint joignable (HTTP 200). Champs : `EMAIL`, `email_address_check`, `locale`, `OPT_IN`, `ScrollTop`, `Terms-600`.

**Aucun envoi effectué** (consigne). Un vrai test d'inscription est le seul probant — il est proposé à Adeline, marqué « TEST ».

### Contact — accordéon, pas une page

« Contactez-nous » n'est pas une page (`/Contact`, `/contact`, `/Contactez-nous` → 404) mais un **accordéon** de la page d'accueil (`data-accordion-parent="10182"`), portant trois champs — `Subject`, `SenderName`, `SenderMail` — et une case « J'accepte les termes et conditions » **obligatoire**.

Le formulaire n'a **pas de balise `<form>`** : la soumission est gérée en JavaScript par un module Form **2sxc**, endpoint identifié `…/app/auto/live/api/Form/ProcessForm?workflowId=ContactDefault`. **Non sollicité** (aucun envoi).

### `/terms` — sert un vrai texte, mais générique

La case d'acceptation pointe vers `/terms`, qui **sert bien un contenu réel** (16 156 caractères : « CONCESSION DE LICENCE LIMITEE », clauses de copyright et de propriété intellectuelle) — ce n'est pas une page vide.

Réserve à porter à l'owner : c'est un **modèle générique de site web**, non adapté à la vente d'un jeu — **0 occurrence** de `vente`, `livraison`, `prix`, `commande`, `remboursement`. Décision éditoriale/juridique, pas une correction technique.

*(Les apostrophes manquantes dans les extraits ci-dessus viennent du retrait des entités `&#39;` par l'instrument de lecture, pas du site.)*

---

# Grain H — encart « Bientôt de retour » : **POSÉ et vérifié**

**Décision owner** : garder le texte de l'article, ajouter par transparence un encart « Bientôt de retour ».

**Le défaut corrigé n'est pas cosmétique.** L'article « On peut commander le jeu de cartes sur ce site » affirme *« Le magasin d'achat en ligne de notre site est enfin opérationnel et nous pouvons à nouveau prendre des commandes »*, alors que la page Acheter sert **« Pas de stock disponible »** (constaté au grain D). Un visiteur qui suit l'article est donc **activement induit en erreur** : l'encart rétablit la vérité au-dessus du texte conservé.

**Pose** — entité E11897 (`Content`, V27785), article « Argumentum est vendu sur argumentum.games ». Backup préalable (`Logs/1781-eav-backup-20261006-1128-grainH/`), dry-run (encart absent ×0 confirmé, 1 626 → 1 891 caractères), UPDATE 1 ligne, recycle, contrôle sur la page servie :

| Contrôle | Mesuré |
|---|---:|
| encart présent | **1** |
| mention « momentanément suspendue » | **1** |
| fond translucide (`rgba(220,15,10,0.12)`) | **1** |

Insertion **en tête** de l'article, le texte d'origine restant intégralement conservé en dessous.

⚠️ **À retirer à la réouverture de la boutique** — c'est un encart d'état temporaire, pas un contenu permanent. À traiter comme tel dans le suivi.

---

# Récapitulatif des écritures de cette passe (#1781)

| Grain | Cible | Nature | Backup | Contrôle servi |
|---|---|---|---|---|
| B (matin) | 24 valeurs EAV | corrections de contenu | ✅ | ✅ |
| **I** | E10080 / V27402 | lien mort → copie Wayback | ✅ | ✅ |
| **H** | E11897 / V27785 | encart d'état temporaire | ✅ | ✅ |

Toutes en **préprod seulement** — la prod (`www.argumentum.games`, serveur distant 51.75.200.22) n'est pas servie par cette machine et n'a pas été touchée.
