# Route `fallacies_cards_fr.html` — provenance et rattachement au livrable (#802/#830)

**Auteur** : po-2023 (worker) · **Date** : 2026-09-14 · **Base** : `origin/master` `21a72385`
**Nature** : **mesure**, lecture seule. Aucune régénération, aucun dépôt, aucune publication.
**Portée** : grain ④ **[idle]** de la deep-queue po-2023 (`#458` c.`5656689863`), option « instruire la route
cards FR ». Le grain ① avait laissé la question explicitement ouverte (`§7`) : *je n'établis pas si ces routes
appartiennent au livrable #830/#802*.

**Réponse : oui, elles en font partie.** Ce n'est pas un fichier orphelin.

---

## §1 — Ce que la route est

| | |
|---|---|
| chemin committé | `Cards/Fallacies/Mindmaps/fr/Fallacies_cards_fr.html` (+ `…_ext.html`) |
| variante | **FR uniquement, par configuration** — la source `.mm` est `Argumentum_Fallacies_MindMap_cards_fr.mm` (`FallacyMindMapCreatorConfig.cs:110`) |
| généré par | `DocumentName = "Fallacies_cards_[LANGUAGE].html"` / `…_[LANGUAGE]_ext.html` (`:160` et `:167`), dans le même bloc de configuration que la carte mindmap des cartes |
| servi par | **personne** — `404` sur `www.argumentum.games` **et** sur `dnn.argumentum.myia.io` (grain ①) |

Aucun `Fallacies_cards_<autre langue>.html` n'existe : la variante est FR *par configuration*, pas par
déploiement partiel. Le triplet SVG de la même carte
(`Argumentum_Fallacies_MindMap_cards_fr{.svg,.content.svg,.links.svg}`) est présent, lui aussi, en FR seul.

## §2 — Provenance : introduite délibérément, et récemment

| objet | commit | nature |
|---|---|---|
| wrappers HTML `Fallacies_cards_fr.html` + `_ext` | **`ceb572c8`** — PR **#1285**, *« regenerate 8-language mindmaps — cards viewer triplet + wrappers (#1269) »*, mergée **2026-09-05** | ajout |
| SVG `Argumentum_Fallacies_MindMap_cards_fr.svg` | `d206e59c` (*« add multilingual SVG mind maps… »*) | antérieur |

L'issue porteuse est **#1269** (MERGED) : *« la carte mindmap des cartes n'avait aucun viewer »*. Les
wrappers sont donc la **réponse** à un défaut constaté — l'absence de visionneuse pour la carte des cartes —
et non un résidu de génération.

## §3 — ⚠️ Elles existent dans `origin/master` mais **pas dans le HEAD local**

```
git cat-file -e HEAD:Cards/…/Fallacies_cards_fr.html          → ABSENT
git cat-file -e origin/master:Cards/…/Fallacies_cards_fr.html → PRESENT
```

Ce n'est pas une anomalie de dépôt : le **checkout local est le webroot vivant de préprod**, en retard
volontaire de 62 commits (jamais de pull). Les wrappers cards FR font partie des commits que ce checkout
n'a pas. C'est la même couche que celle mesurée au grain ② — et **c'est une explication suffisante du 404** :
la variante a été ajoutée le 05/09, après la dernière migration du webroot (28/08).

## §4 — Rattachement : oui, au livrable #802/#830

| issue | objet | rattachement |
|---|---|---|
| **#802** (OPEN) | *Relecture visuelle — paquet complet du jeu (64 PDF + **mindmaps interactives**, 8 langues) — pour Thomas & Adeline* | la carte des cartes est **une** des mindmaps interactives du paquet ⇒ la route **appartient** au livrable à relire |
| **#830** (OPEN) | *golden-master COMPORTEMENTAL — 9 capacités mesurées à chaque régén* | les wrappers cards FR relèvent de la même famille d'artefacts que ceux que #830 mesure |

⇒ Le 404 n'est **pas** un fichier en trop : c'est un **écart de déploiement**. La route est committée,
générée et voulue ; elle n'a simplement jamais atteint le webroot.

## §5 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **positif** | un artefact de la même famille, lui, présent | `Fallacies_fr.html` PRESENT dans HEAD **et** `origin/master` ✅ |
| **négatif** | la comparaison de refs doit discriminer | `git cat-file -e` rend ABSENT sur `HEAD` / PRESENT sur `origin/master` — deux réponses distinctes ✅ |
| **portée FR** | la variante ne doit pas exister dans les 7 autres langues | aucune `Fallacies_cards_<lang>.html` hors `fr/` dans `origin/master` ✅ |
| **croisement** | le 404 HTTP doit être cohérent avec l'état des refs | 404 sur les deux hôtes (grain ①) ⇔ absent du webroot, qui sert `de763aa9` (antérieur au 05/09) ✅ |

## §6 — Ce que je n'établis PAS

- **L'intention de déploiement.** Je n'établis pas si l'absence au webroot est un oubli ou un report
  délibéré : la mesure constate l'écart, elle ne juge pas.
- **Le contenu** des wrappers cards FR vis-à-vis de `included.html` / `external.html` (leurs gabarits
  déclarés) : non diffé.
- **Le sort de `_ext`** pour cette variante : même provenance (`ceb572c8`), même absence de service.
- **Aucun verdict de rendu** — la relecture visuelle est #802, côté humain.

---

*master `21a72385` · lecture seule : `ls-tree`, `cat-file`, `log --diff-filter=A`, lecture de configuration ·
⛔ aucune régénération, aucun dépôt, aucune publication · verdict visuel : Thomas & Adeline (#802) / ai-01.*
