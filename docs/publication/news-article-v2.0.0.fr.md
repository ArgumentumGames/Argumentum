# Article d'annonce — Argumentum v2.0.0 (brouillon FR canonique)

> **Statut : BROUILLON (prep #135, reprise #999).** Article public-facing d'annonce de la release
> **v2.0.0** (re-scope v0.9.0 → v2.0.0, jsboige 2026-08-06), à publier sur le module **News5** du
> portail DNN. Version FR canonique (le portail sert le FR en primaire). **La publication est GATED**
> sur #134 (tag GitHub Release), #132 (déploiement prod) et #131 (DNN 10.3.2 live) — voir
> « Checklist de publication » en bas. Ce fichier est la préparation du texte ; il ne publie rien.
>
> **Source de vérité :** [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) (volet lecteur,
> chiffres re-mesurés 15/09 + annexe de qualification), [cards-catalog.fr.md](cards-catalog.fr.md)
> (formats + volumes de fabrication), [134-release-assets-drift-2026-09-15.md](../release-dossier/134-release-assets-drift-2026-09-15.md)
> (état des assets servis). Cet article est le **volet public** — il vulgarise, ne duplique pas le
> technique. Les champs `[PLACEHOLDER]` sont à remplir au moment du tag.
>
> Supersede le brouillon [news-article-v0.9.0.fr.md](news-article-v0.9.0.fr.md) (13/09) — corps repris
> en v2.0.0, chiffres re-mesurés, vertus-mindmaps corrigées (localisées 8 langues depuis #665).

---

## Méta-données SEO / CMS (à remplir au publish)

| Champ | Valeur |
|-------|--------|
| **Titre SEO** (`<title>`) | Argumentum v2.0.0 — le jeu de cartes sur les sophismes, désormais en 8 langues |
| **Meta description** (<160 car.) | Argumentum v2.0.0 : 8 langues, 197 cartes par boîte, variantes inédites, mind maps et ontologie. Print & Play gratuit à télécharger. |
| **Slug URL** | `argumentum-v2-0-0-8-langues` |
| **og:image** | `[PLACEHOLDER — vignette A0 FR ou mosaïque 4 variantes, ~1200×630]` |
| **twitter:card** | `summary_large_image` |
| **og:locale** | `fr_FR` (variantes `en_US`, `ru_RU`, `pt_PT`, `es_ES`, `ar_AR`, `fa_IR`, `zh_CN` aux miroirs traduits) |
| **Date de publication** | `[PLACEHOLDER — jour du tag v2.0.0]` |
| **Auteur CMS** | Argumentum Games |

> **Sitemap :** ajouter l'URL canonique au sitemap DNN au publish. Les variantes traduites (EN/RU/PT/
> ES/AR/FA/ZH) devront avoir leurs `hreflang` alternates déclarés (voir « Traductions » en bas).

---

## Corps de l'article (FR)

### Argumentum v2.0.0 — le jeu de cartes sur les sophismes, désormais en 8 langues

**Argumentum**, le jeu de cartes pédagogique qui apprend à repérer les sophismes et à défendre des
arguments rigoureux, sort en version **2.0.0**. Cette mise à jour la plus ambitieuse à ce jour étend
la couverture linguistique de 4 à **8 langues**, ajoute **quatre nouvelles variantes de jeu**, et met
à jour les cartes, les cartes mentales et l'ontologie de la taxonomie.

Le matériel complet — cartes à imprimer, cartes mentales et ontologie — est disponible en téléchargement
gratuit sous licence ouverte.

#### 🌍 Huit langues, un seul matériel

Argumentum est désormais généré intégralement en **8 langues** : français (langue source), anglais,
russe, portugais, espagnol, arabe, persan et chinois. Toutes les données du jeu — la taxonomie des
sophismes (**1408 nœuds**), les **223 nœuds** des vertus argumentatives, les **167 scénarios** de jeu
et les règles — sont traduites à 100 % dans chacune de ces langues, y compris les scripts non latins
(cyrillique, arabe, persan, chinois).

| Langue | Script | |
|--------|--------|---|
| Français | Latin | langue source |
| English · Português · Español | Latin | |
| Русский | Cyrillique | |
| العربية · فارسی | RTL (droite-à-gauche) | |
| 中文 | CJK | |

#### 🃏 Ce qu'il y a dans la boîte

Le deck principal compte **197 cartes** : **175** cartes de sophismes (un vrai doublon a été retiré
de cette édition), **15** cartes de règles — dont **quatre variantes de jeu inédites** qui renouvellent
les parties : *Bingo mixologie argumentative*, *Dernier Beau Parleur*, *Moulin à Baratin* et
*Parlote Coinchée* — et la carte memo imprimée en **7 exemplaires**. Avec le deck de **167 scénarios**
(7 dos par catégorie), la boîte complète atteint **364 cartes** — **495** avec l'extension des
131 cartes de **Vertus**.

#### 📚 Taxonomie enrichie, cartes mentales et ontologie

La taxonomie des sophismes a été consolidée : les racines de familles FR ont été révisées cellule par
cellule, et la cohérence des traductions est désormais déterministe (aucun artefact de traduction
automatique, scripts corrects pour les langues non latines). Les **vertus argumentatives** et les
**167 scénarios** (précédemment traduits à 54 %) sont désormais couverts à 100 %.

Les **cartes mentales** ont été régénérées au format SVG FreeMind **dans les 8 langues** — sophismes
comme vertus — et l'**ontologie OWL** (avec alignements SKOS et références AIF) documente la structure
formelle de la taxonomie en français et en anglais — un socle pour la recherche en argumentation
computationnelle.

#### 🖨 Print & Play

Tout le matériel est disponible en **Print & Play A4** : impression recto-verso sur papier épais
(160–250 g/m²), découpe, et jouer. Quatre livrets, dans chaque langue :

- `TarotCards_Print&Play_A4` — Règles + Mémo + Fallacies, et sa version **Light** économe en encre
- `PokerCards_Print&Play_A4` — Scénarios, et sa version **Light**

#### 📦 Téléchargements

Les paquets seront hébergés sur la page [Releases GitHub](https://github.com/ArgumentumGames/Argumentum/releases)
`[PLACEHOLDER — lien vers la release v2.0.0 une fois taguée]`.

| Paquet | Contenu | Langues |
|--------|---------|---------|
| **Complet** | Tout le matériel (Tarot, Poker, Print & Play, FallaciesWeb A0/A4, Thumbnails) | les 8 |
| **Print & Play** | PDFs Print & Play A4 (standard + Light) | les 8 |
| **Par langue** | Matériel complet pour une langue | au choix |
| **Cartes mentales** | SVG Fallacies + Virtues | les 8 (FR/EN/RU/PT/ES/AR/FA/ZH) |
| **Ontologie** | `argumentum.owl` + `argumentum_virtues.owl` (SKOS + AIF) | FR · EN |

**80 PDFs au total** = 8 langues × 10 types de documents, parité vérifiée ; les PDF d'impression
professionnelle sont convertis en DeviceCMYK avec OutputIntent (prêts pour l'imprimeur), les cartes
en 300 dpi natifs.

Détail par format et instructions d'impression : voir le [catalogue des cartes](cards-catalog.fr.md).

> ⚠️ **Jusqu'au tag v2.0.0**, ne distribuez pas les actifs de la pré-release `v0.9.0-review` : ils
> datent du 24-25/08 et précèdent les corrections de septembre (état détaillé dans le
> [dossier drift](../release-dossier/134-release-assets-drift-2026-09-15.md)).

#### 💬 Rejoindre la communauté

`[PLACEHOLDER — lien communauté / Discord / GitHub Discussions selon décision]`

---

## Checklist de publication (gates #134 / #132 / #131)

À cocher au moment du tag — **ne pas publier tant que tout n'est pas vert** :

- [ ] **#134** — Tag `v2.0.0` posé + GitHub Release créée (**80 assets** re-uploadés frais — pas la
      pré-release du 24/08, voir le dossier drift ; nommage `Print.Play` vs `Print&Play` tranché).
- [ ] **Relecture jsboige des notes v2.0.0** (#999 DoD 3) — condition posée par le CHANGELOG.
- [ ] **#131** — DNN **10.3.2 + 2sxc 21** live en production (couplage release validé par jsboige).
- [ ] **#132** — Déploiement prod complet (runbook Phase 5).
- [ ] Remplacer tous les `[PLACEHOLDER]` : URL release v2.0.0, date, og:image, lien communauté.
- [ ] Charger l'`og:image` dans le media DNN et référencer son URL finale.
- [ ] Créer le post dans le module **News5** (DNN), coller le corps FR, régler slug + méta.
- [ ] Ajouter l'URL canonique au sitemap DNN ; déclarer les `hreflang` aux variantes traduites.
- [ ] **Verdict visuel final** = jsboige / ai-01 (le worker signale, ne déclare pas PASS).
- [ ] Mettre à jour la page **Téléchargements** du site avec les liens v2.0.0 (issue #135 §Downloads).

## Traductions (miroirs)

Conformément à la convention `docs/publication/` (FR canonique + miroir EN dans le même PR), un
**miroir anglais** accompagne ce fichier : [news-article-v2.0.0.en.md](news-article-v2.0.0.en.md).

Les **6 autres langues** (RU/PT/ES/AR/FA/ZH) suivront au publish via le pipeline `DatasetUpdater`
(même discipline que la native-ratification #192 : traduction puis validation humaine des scripts
non latins, en particulier RTL/CJK). À planifier post-tag, non bloquant pour la publication FR+EN.

## Sources

- [RELEASE-NOTES-v2.0.0.md](../RELEASE-NOTES-v2.0.0.md) — volet lecteur, chiffres re-mesurés 15/09 sur `c089d526` (175/15/167 MESURÉ, memo ×7 et 197/364/495 DÉCIDÉ #1187 c.5665864605, Virtues 131 RAPPORTÉ, 80 PDF MESURÉ) ; annexe de qualification incluse.
- [cards-catalog.fr.md](cards-catalog.fr.md) — formats, dimensions physiques, volumes de fabrication (erratum 15/09).
- [134-release-assets-drift-2026-09-15.md](../release-dossier/134-release-assets-drift-2026-09-15.md) — pourquoi la pré-release n'est pas distribuable en l'état (PR #1383).
- Issue [#135](https://github.com/ArgumentumGames/Argumentum/issues/135) — cahier des charges (corps d'origine stale : « 4 langues » ; scope réel = 8).
- Issues de dépendance : #134 (release), #131 (DNN), #132 (deploy), #999 (renumérotation v2.0.0).
