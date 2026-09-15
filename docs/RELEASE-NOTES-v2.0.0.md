# Argumentum v2.0.0 — Notes de version

> **Statut** : DRAFT à relire par jsboige (#999 DoD 3 — la relecture owner ferme la DoD).
> **Auteur** : po-2024 (pool #458, grain ⑦) · **Rédigé** : 2026-09-15, base master `c089d526`.
> Chiffres re-mesurés le 15/09 depuis les sources (qualification MESURÉ/RAPPORTÉ/DÉCIDÉ en annexe A).
> Historique de version : le tag a été re-scopé v0.9.0 → **v2.0.0** (jsboige 2026-08-06, #999).

---

## Vous connaissiez déjà Argumentum ? Voici ce que change la v2

La v2.0.0 ne change pas les règles du jeu : elle change **le matériel** et **qui peut y jouer**.

### Le jeu parle maintenant 8 langues

Français, anglais, russe, portugais, espagnol, arabe, farsi et chinois — cartes, dos, scénarios,
règles et cartes memo inclus, avec le rendu qui va avec (droite-à-gauche pour l'arabe et le farsi,
polices CJK pour le chinois). L'édition précédente était francophone.

### Ce qu'il y a dans la boîte (par langue)

| | cartes | détail |
|---|---:|---|
| Deck principal (tarot) | **197** | **175** cartes de sophismes (un vrai doublon, « Appel à la nature », a été retiré) + **15** cartes de règles + la carte memo imprimée **×7** |
| Deck scénarios (poker) | **167** | un scénario par carte, 7 dos par catégorie |
| Extension Vertus | **131** | l'arbre des 8 familles d'arguments valables |
| **Boîte complète sans Vertus** | **364** | 197 + 167 |
| **Boîte complète avec Vertus** | **495** | 364 + 131 |

Ces volumes sont les **volumes de fabrication décidés** (#1187, 14/09) : ils comptent les sept
exemplaires réellement imprimés de la memo. Les chiffres 191/358/489 publiés début septembre sont
périmés (ils comptaient une seule memo).

### Huit façons d'imprimer

**80 PDF** = 10 formats × 8 langues : tarot à imprimer et découper (recto-verso A4, version
standard et version **Light** économe en encre), deck poker, posters A0 et A4 des 175 sophismes,
planche de vignettes. Les PDF d'impression professionnelle sortent en **CMYK avec OutputIntent**
(graphisme prêt pour l'imprimeur), les cartes en **300 dpi** natifs.

### Deux outils nouveaux dans le dépôt

- **Cartes mentales interactives** : pour chaque langue, la carte du monde des sophismes et celle
  des vertus, navigables dans le navigateur (HTML + SVG).
- **Ontologie OWL/AIF** : la taxonomie complète des sophismes et vertus, mappée sur le vocabulaire
  Argument Interchange Framework — pour l'enseignement et le traitement automatique.

### Ce qui ne change pas

Les sophismes eux-mêmes, la structure des familles, les règles du jeu. Si vous jouiez à l'édition
précédente, vous jouez de la même façon — avec un deck nettoyé d'un doublon et une boîte qui
contient enfin tout.

---

## Téléchargement

La release GitHub `v2.0.0` (à venir) servira les 80 PDF. ⚠️ En attendant le tag, la pré-release
`v0.9.0-review` de juillet **ne reflète pas** l'état actuel (64 actifs datés du 24-25/08, 16 formats
Light manquants, contenus antérieurs aux corrections de septembre) — voir
[`134-release-assets-drift-2026-09-15.md`](release-dossier/134-release-assets-drift-2026-09-15.md) (PR #1383).
Ne distribuez pas les actifs de la pré-release : ils précèdent notamment la pseudonymisation #1189.

---

## Annexe A — Qualification des chiffres (re-mesure du 15/09, master `c089d526`)

| chiffre | statut | source |
|---|---|---|
| 175 sophismes | **MESURÉ** | CSV Taxonomy, `carte` non vide = 175 PK uniques ; PK 96 conservé au corpus mais hors deck (#1288, 05/09) |
| 15 règles | **MESURÉ** | CSV Rules = 15 rangées |
| memo ×7 | **DÉCIDÉ** | owner 14/09, #1187 c.`5665864605` — `NbCopies = 7` retail (P&P : 5 et 1 inchangés) |
| 167 scénarios | **MESURÉ** | CSV Scenarii = 167 rangées |
| 131 Vertus | **RAPPORTÉ** | mesure arbre Release 22/08 (CLAUDE.md « État actuel par CardSet ») — arbre local nettoyé, non re-mesurable ce jour |
| **197 / 364 / 495** | **DÉCIDÉ** + arithmétique vérifiée | #1187 c.`5665864605` : volumes de fabrication retail ; 197 = 175+15+7, 364 = +167, 495 = +131 |
| 80 PDF | **MESURÉ** | 10 types (`package-v2.0.0.ps1`) × 8 langues |
| CMYK + OutputIntent, 300 dpi | **RAPPORTÉ** | bundle `review-v2.0.0-regen-20260912` (80/80), critères du guide de validation éd. 3 (#1380) |

*Errata datés posés le 15/09 sur les documents portant 191/358/489 : CLAUDE.md, `cards-catalog.fr.md`, `REGEN_RELEASE_RUNBOOK.md`, guide de validation éd. 3.*

---

🤖 Generated with [Claude Code](https://claude.com/claude-code)
