# Argumentum v0.9.0 — Parcours de session GO visuel (jsboige)

**Objet** : guide d'action (~45 min) pour la **session de validation visuelle finale** de jsboige avant
de poser le tag v0.9.0 (#134). Calendaire : **sem. du 13/07**. Ce n'est pas un dossier technique — c'est
un **parcours guidé** « ouvre ceci, vérifie ça, voici ce qui est normal, voici les décisions à prendre ».

> **Positionnement (anti-doublon)** : ce doc est **complémentaire**, pas substitut.
> - [RELEASE-VALIDATION-v0.9.0.md](RELEASE-VALIDATION-v0.9.0.md) = dossier technique complet (inventaire,
>   preuves, verdicts). **Source de vérité** pour le détail.
> - [RELEASE-VERIFICATION-INDEX-v0.9.0.md](RELEASE-VERIFICATION-INDEX-v0.9.0.md) = parcours de review des
>   **docs** release (un lien + une ligne « à vérifier » par doc).
> - **Ce doc** = parcours de review des **assets visuels** (PDFs/SVGs du bundle GDrive) — la partie que
>   les deux autres ne couvrent pas : *que regarder dans les PDFs, et comment juger le non-latin*.

**Verdicts déjà rendus (ne pas re-déclarer — pointer)** : contenu #140 = **PASS** (ai-01, 2026-07-03,
8 langues) · colorimétrie CMYK #632 = **PASS** (ai-01, 2026-07-03). Cette session = **eyeball jsboige
final** + **4 arbitrages GO/no-GO**.

---

## §1 — Prérequis (2 min)

| Quoi | Où |
|------|----|
| **Bundle assets** (GDrive) | dossier **`review-v0.9.0-RELEASE-bundle-v3-2026-07-03/`** — 6,5 GB (80 PDFs CMYK + 7 samples + `CMYK_COLOR_PROOF.txt`). Bundles précédents (Debug `…-2026-06-28/`, Release v1 `…-2026-07-01/`) préservés mais **v3 = canonical** (80/80 CMYK). |
| **Dossier technique** | [RELEASE-VALIDATION-v0.9.0.md](RELEASE-VALIDATION-v0.9.0.md) (§3.3 = PDFs, §3.6 = finding PT) |
| **Lecteur PDF** | un viewer qui rend les CJK + RTL (Acrobat / SumatraPDF). ⚠ un viewer sans font CJK = faux « tofu □ » qui n'est **pas** un défaut asset (cf §4). |
| **Langues** | 8 : FR (canonical) · EN · RU · PT · ES · AR · FA · ZH |

---

## §2 — Parcours visuel (~20 min) — 6 arrêts prioritaires

Pas besoin de regarder les 80 PDFs. Les 6 ci-dessous couvrent tous les cas-limites (densité, RTL, CJK,
cyrillique, covers, finding PT, Rules-first). Si ces 6 sont bons, le reste l'est (même pipeline, même
fonts).

| # | Fichier du bundle | Quoi vérifier | Cas-limite couvert |
|---|-------------------|---------------|--------------------|
| 1 | `TarotCards_fr-1.pdf` p1-2 | cover FR + recto-verso aligné | baseline FR golden |
| 2 | `TarotCards_pt-1.pdf` p4 | **titre de la carte Rules PT** | ⚠ finding §3.6 « Roll of the English Channel » — voir §5 déc. (c) |
| 3 | `TarotCards_ar-1.pdf` p51 (carte dense) | texte **RTL** correct, alignement right-to-left | **AR RTL** |
| 4 | `TarotCards_fa-1.pdf` p51 | RTL + glyphes persans (différents de l'arabe) | **FA RTL** |
| 5 | `TarotCards_zh-1.pdf` p51 | caractères **CJK** rendus, pas de débordement (CJK plus large) | **ZH CJK** |
| 6 | `Fallacies_Web_Thumbnails_ru*.pdf` p1 | cyrillique + accents | **RU** |

> **Invariant structurel** (déjà vérifié ai-01, ne pas s'en inquiéter) : l'**image count** est identique
> dans les 8 langues — le multilingue n'a pas cassé la structure (#216 tenu). Rules apparaissent en
> premier (#119). 300 PPI. Recto-verso aligné.

**Après les 6 arrêts** : si tout est visuellement correct → les 74 autres PDFs sont fiables (même pipeline).
Si un problème → isoler la langue/le type, signaler (ce n'est probablement pas une régression globale).

---

## §3 — Guide vérif non-latin : « normal » vs « cassé »

Le risque principal d'une review visuelle multilingue = **faux positif** (signaler comme cassé ce qui est
correct dans une écriture qu'on ne lit pas). Référence rapide :

### AR / FA (Right-To-Left)
- **Normal** : le texte court de **droite à gauche**. Les titres, body, captions alignés à droite. Les
  chiffres restent LTR dans le flux RTL (standard Unicode bidi). AR et FA partagent l'alphabet arabe mais
  FA ajoute 4 lettres persanes (پ چ ژ گ) — vérifier qu'elles rendent (pas de tofu □).
- **Cassé** (signaler) : texte LTR dans une page censée être RTL · glyphes absents (□ tofu) · ligatures
  cassées (lettres détachées qui devraient être liées).

### ZH (CJK chinois)
- **Normal** : caractères Han rendus en blocs carrés. Le CJK est **plus large** que le latin à em-box égal →
  un texte qui semble « tassé » ou qui déborde légèrement n'est pas forcément un bug (mais un débordement
  hors cadre l'est). Polices Tahoma-capable.
- **Cassé** (signaler) : tofu □ · caractères moitiés · police de fallback latin sur du chinois.

### RU (cyrillique)
- **Normal** : alphabet cyrillique (А-Я а-я), accents, ё/й.
- **Cassé** (signaler) : tofu · caractères latins à la place de cyrilliques homographes (a au lieu de а).

### ES / PT (latin étendu)
- **Normal** : accents (é è ê, ã õ, ç, ñ espagnol, à). PT brésilien.
- **Cassé** (signaler) : accents manquants/mozailés · « ? » noir (UTF-8 cassé).

---

## §4 — Known-issues à NE PAS signaler comme régressions

| Symptôme | Cause | Action |
|----------|-------|--------|
| Tofu □ sur CJK/AR dans votre viewer | **font du viewer**, pas l'asset | ouvrir dans Acrobat/Sumatra ; ne pas signaler |
| SVG Virtues `.content.svg` **FR seulement** (pas i18n) | FR-frozen gap known (#636/#654) | **non-bloquant v0.9.0**, deferred — cf §5 déc. (a) |
| Titre PT « Roll of the English Channel » | homonyme « Manche » (round→géographie), cf §3.6 | **décision jsboige** block vs fast-follow — cf §5 déc. (c). Note : #640 a résolu le bulk (23 occ), le titre Tarot PT spécifique est possiblement déjà fixé — **confirmer visuellement** |
| OWL bilingue EN+FR seulement (pas 8 langues) | par construction (`OwlGeneratorConfig` mono-lang) | **honnête** : CSV/PDF/SVG = 8 langues, OWL = EN+FR. Ne pas s'attendre à du RU/ZH dans l'OWL. |
| 2 bugs post-tag tracés (#629 CardPen /Cards/ 404, #630 Spectre HARVEST-FAILURE) | bugs process, **non-assets** | ne bloquent pas le tag v0.9.0 assets |

---

## §5 — Les 4 arbitrages jsboige (GO/no-GO binaires, ~10 min)

À trancher **pendant ou après** le parcours. Aucun n'est un défaut technique rendu — ce sont des choix de
scope/couplage. Reco po-2023 en italique (cf [dossier §8](RELEASE-VALIDATION-v0.9.0.md)).

| # | Décision | Options | Reco |
|---|----------|---------|------|
| **(a)** | **SVG Virtues FR-frozen** (#636/#654) — l'asset `.content.svg` Virtues n'est localisé qu'en FR | (i) FreeMind GUI-interactif pour i18n maintenant · (ii) **defer post-tag** (ship-as-is + known-limitation) | *(ii) defer — non-bloquant géométrie* |
| **(b)** | **Mnémoniques Virtues** (#654) — 20 rows mnémoniques Latin (pks 106-127) | (A) no-op · (B) keep-Latin scope title-only · global (53 cells Latin) | *B keep-Latin — acté jsboige, script #695 `--apply` gated post-tag* |
| **(c)** | **Titre PT « Roll of the English Channel »** (§3.6) — confirmer visuellement si toujours présent (cf §4) | (i) **block le tag** (attendre fix PT) · (ii) **fast-follow post-tag** | *(ii) fast-follow — 1 carte/1 langue, non-bloqueur print* |
| **(d)** | **Couplage go-live DNN** — tagger v0.9.0 assets-only maintenant, ou aligner au go-live DNN multilingue | (i) tag immédiat (assets-only) · (ii) attendre portage i18n site (#669/#674) + go-live DNN | *(i) tag assets-only — DNN prod go-live = ops VPS jsboige, séparé ; migration full-IIS déjà LIVE en recette* |

---

## §6 — Ce qui est déjà PASS (pointer, pas re-vérifier)

Ces verdicts sont **rendus par ai-01** (technique) — jsboige peut s'y fier sans re-faire le travail :

- ✅ **Contenu #140** (8 langues, 2026-07-03) : carte dense p51 EN/ES/RU/AR/FA/ZH, RTL/CJK propre, covers,
  PT #306 fixé (« A Escola dos Mentirosos »), FR Rules 5 jeux, 300 PPI. → [dossier §3.3](RELEASE-VALIDATION-v0.9.0.md)
- ✅ **Colorimétrie CMYK #632** (2026-07-03) : 80/80 DeviceCMYK + OutputIntent SWOP, vérifié sur fr+ar+zh.
- ✅ **Géométrie #119** : Rules-first, recto-verso aligné.
- ✅ **#216** : pas de fuite FR (multilingue intact).
- ✅ **Tests** : 595 pass / 1 known-fail #133 (OWL round-trip, sans impact assets) / 5 skip. Build zéro-warning. (600 total, #789)

---

## §7 — Issue de session

- **Tous les assets visuels OK + arbitrages (a)(b)(c)(d) tranchés** → tag v0.9.0 **techniquement débloqué**
  (le seul arbitrage non-technique restant est calendaire : ta dispo pour le GO visuel).
- **Un problème visuel isolé** → ce n'est vraisemblablement pas une régression (pipeline stable, image-count
  invariant) ; isoler langue/type, signaler au cluster, fast-follow post-tag probable.
- **Un problème visuel structurel** (même défaut × plusieurs langues) → **bloqueur**, ne pas tagger,
  signaler URGENT.

---

*Parcours créé par po-2023 (dispatch `k0z7uj` secondaire). Worker signale, ne déclare pas PASS — les
verdicts ci-dessus sont rendus par ai-01. Ce doc est un guide d'action, pas une revue technique.*
