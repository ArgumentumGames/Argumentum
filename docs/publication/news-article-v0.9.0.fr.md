# Article d'annonce — Argumentum v0.9.0 (brouillon FR canonique)

> **Statut : BROUILLON (prep #135).** Article public-facing d'annonce de la release v0.9.0, à publier
> sur le module **News5** du portail DNN. Version FR canonique (le portail sert le FR en primaire).
> **La publication est GATED** sur #134 (tag GitHub Release), #132 (déploiement prod) et #131 (DNN
> 10.3.2 live) — voir « Checklist de publication » en bas. Ce fichier est la préparation du texte ;
> il ne publie rien.
>
> **Source de vérité :** [RELEASE-NOTES-v0.9.0.md](../../RELEASE-NOTES-v0.9.0.md) (technique),
> [docs/release-dossier/README.md](../release-dossier/README.md) (dossier + snippet downloads §5),
> [cards-catalog.fr.md](cards-catalog.fr.md) (formats). Cet article est le **volet public** — il
> vulgarise, ne duplique pas le technique. Tout chiffre cité est vérifié dans ces sources au
> 2026-06-29 ; les champs `[PLACEHOLDER]` sont à remplir au moment du tag.

---

## Méta-données SEO / CMS (à remplir au publish)

| Champ | Valeur |
|-------|--------|
| **Titre SEO** (`<title>`) | Argumentum v0.9.0 — le jeu de cartes sur les sophismes, désormais en 8 langues |
| **Meta description** (<160 car.) | Argumentum v0.9.0 : 8 langues, 4 nouvelles variantes de jeu, mind maps et ontologie mises à jour. Matériel Print & Play gratuit à télécharger. |
| **Slug URL** | `argumentum-v0-9-0-8-langues` |
| **og:image** | `[PLACEHOLDER — vignette A0 FR ou mosaïque 4 variantes, ~1200×630]` |
| **twitter:card** | `summary_large_image` |
| **og:locale** | `fr_FR` (variantes `en_US`, `ru_RU`, `pt_PT`, `es_ES`, `ar_AR`, `fa_IR`, `zh_CN` aux miroirs traduits) |
| **Date de publication** | `[PLACEHOLDER — jour du tag v0.9.0]` |
| **Auteur CMS** | Argumentum Games |

> **Sitemap :** ajouter l'URL canonique au sitemap DNN au publish. Les variantes traduites (EN/RU/PT/
> ES/AR/FA/ZH) devront avoir leurs `hreflang` alternates déclarés (voir « Traductions » en bas).

---

## Corps de l'article (FR)

### Argumentum v0.9.0 — le jeu de cartes sur les sophismes, désormais en 8 langues

**Argumentum**, le jeu de cartes pédagogique qui apprend à repérer les sophismes et à défendre des
arguments rigoureux, sort en version **0.9.0**. Cette mise à jour la plus ambitieuse à ce jour
étend la couverture linguistique de 4 à **8 langues**, ajoute **quatre nouvelles variantes de jeu**,
et met à jour les cartes, les cartes mentales et l'ontologie de la taxonomie.

Le matériel complet — cartes à imprimer, cartes mentales et ontologie — est disponible en téléchargement
gratuit sous licence ouverte.

#### 🌍 Huit langues, un seul matériel

Argumentum est désormais généré intégralement en **8 langues** : français (langue source), anglais,
russe, portugais, espagnol, arabe, persan et chinois. Toutes les données du jeu — les **1408 nœuds**
de la taxonomie des sophismes, les **223 nœuds** des vertus argumentatives, les **167 scénarios** de
jeu et les règles — sont traduites à 100 % dans chacune de ces langues, y compris les scripts
non latins (cyrillique, arabe, persan, chinois).

| Langue | Script | |
|--------|--------|---|
| Français | Latin | langue source |
| English · Português · Español | Latin | |
| Русский | Cyrillique | |
| العربية · فارسی | RTL (droite-à-gauche) | |
| 中文 | CJK | |

#### 🃏 Quatre nouvelles variantes de jeu

Le deck **Règles** s'enrichit de quatre modes de jeu inédits qui renouvellent les parties :

- **Bingo mixologie argumentative**
- **Dernier Beau Parleur**
- **Moulin à Baratin**
- **Parlote Coinchée**

Chaque variante est livrée comme carte-règle dans le deck Tarot (et le livret Print & Play A4).

#### 📚 Taxonomie enrichie, cartes mentales et ontologie

La taxonomie des sophismes a été consolidée : les 7 racines de familles FR ont été révisées
cellule par cellule, et la cohérence des traductions est désormais déterministe (aucun artefact de
traduction automatique, scripts corrects pour les langues non latines). Les **vertus argumentatives**
et les **167 scénarios** (précédemment traduits à 54 %) sont désormais couverts à 100 %.

Les **cartes mentales** ont été régénérées au format SVG FreeMind (Fallacies dans les 8 langues ;
cartes des Vertus en français, leur localisation dans les autres langues étant différée à une
version ultérieure), et l'**ontologie OWL** (avec alignements SKOS et références AIF) documente la
structure formelle de la taxonomie en français et en anglais — un socle pour la recherche en
argumentation computationnelle.

#### 🖨 Print & Play

Tout le matériel est disponible en **Print & Play A4** : impression recto-verso sur papier épais
(160–250 g/m²), découpe, et jouer. Deux livrets :

- `TarotCards_Print&Play_A4` — Règles + Mémo + Fallacies
- `PokerCards_Print&Play_A4` — Scénarios

#### 📦 Téléchargements

Les paquets sont hébergés sur la page [Releases GitHub](https://github.com/ArgumentumGames/Argumentum/releases)
`[PLACEHOLDER — lien vers la release v0.9.0 une fois taguée]`.

| Paquet | Contenu | Langues |
|--------|---------|---------|
| **Complet** | Tout le matériel (Tarot, Poker, Print & Play, FallaciesWeb A0/A4, Thumbnails) | les 8 |
| **Print & Play** | PDFs Print & Play A4 uniquement (impression maison, recto-verso) | les 8 |
| **Par langue** | Matériel complet pour une langue | au choix |
| **Cartes mentales** | SVG Fallacies + Virtues | les 8 (FR/EN/RU/PT/ES/AR/FA/ZH) |
| **Ontologie** | `argumentum.owl` + `argumentum_virtues.owl` (SKOS + AIF) | FR · EN |

Détail par format (Tarot, Poker, Print & Play, FallaciesWeb A0/A4/Thumbnails) et instructions
d'impression : voir le [catalogue des cartes](cards-catalog.fr.md) et le
[snippet downloads du dossier release](../release-dossier/README.md#5-readme-download-section-snippet-ready-to-paste--issue-134-asks-for-it).

> **80 PDFs au total** = 8 langues × 10 types de documents (incl. Print&Play Standard + Print&Play Light, #648-650), parité vérifiée. Les 80 PDFs convertis en DeviceCMYK + OutputIntent SWOP via le post-process Ghostscript (#632/#652).

#### 💬 Rejoindre la communauté

`[PLACEHOLDER — lien communauté / Discord / GitHub Discussions selon décision]`

---

## Checklist de publication (gates #134 / #132 / #131)

À cocher au moment du tag — **ne pas publier tant que tout n'est pas vert** :

- [ ] **#134** — Tag `v0.9.0` posé + GitHub Release créée (assets uploadés).
- [ ] **#131** — DNN **10.3.2 + 2sxc 21** live en production (couplage release validé par jsboige).
- [ ] **#132** — Déploiement prod complet (runbook Phase 5).
- [ ] Remplacer tous les `[PLACEHOLDER]` : URL release v0.9.0, date, og:image, lien communauté.
- [ ] Charger l'`og:image` dans le media DNN et référencer son URL finale.
- [ ] Créer le post dans le module **News5** (DNN), coller le corps FR, régler slug + méta.
- [ ] Ajouter l'URL canonique au sitemap DNN ; déclarer les `hreflang` aux variantes traduites.
- [ ] **Verdict visuel final** = jsboige / ai-01 (le worker signale, ne déclare pas PASS).
- [ ] Mettre à jour la page **Téléchargements** du site avec les liens v0.9.0 (issue #135 §Downloads).

## Traductions (miroirs)

Conformément à la convention `docs/publication/` (FR canonique + miroir EN dans le même PR), un
**miroir anglais** accompagne ce fichier : [news-article-v0.9.0.en.md](news-article-v0.9.0.en.md).

Les **6 autres langues** (RU/PT/ES/AR/FA/ZH) suivront au publish via le pipeline `DatasetUpdater`
(même discipline que la native-ratification #192 : traduction puis validation humaine des scripts
non latins, en particulier RTL/CJK). À planifier post-tag, non bloquant pour la publication FR+EN.

## Sources

- [RELEASE-NOTES-v0.9.0.md](../../RELEASE-NOTES-v0.9.0.md) — chiffres canoniques (8 langues, 64 PDFs, 4 variantes, ~9834 images, OWL 5.3 Mo).
- [docs/release-dossier/README.md](../release-dossier/README.md) — dossier de validation + snippet README downloads (§5) + gate checklist (§4).
- [cards-catalog.fr.md](cards-catalog.fr.md) — formats et dimensions physiques.
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — cahier des charges ( corps d'origine stale : « 4 langues » ; scope réel = 8).
- Issues de dépendance : #134 (release), #131 (DNN), #132 (deploy).
