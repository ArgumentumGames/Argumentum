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

Métriques réelles extraites du VOWL-JSON produit :

```
classes 1495 · propriétés d'objet 10 · axiomes 18926 · individus 719
```

**1 495 classes pour 10 propriétés** : le graphe est un **arbre taxonomique**, pas un réseau relationnel. WebVOWL affiche les liens de sous-classe — à cette densité, il rend une **pelote** dense et peu navigable (limite connue et documentée de l'outil sur les grandes ontologies).

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
