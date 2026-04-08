---
name: pipeline-validate
description: Valide visuellement PDFs et images du pipeline Argumentum via Playwright + vision. Verifie dimensions, alternance face/dos, dos par famille, contenu visible, et genere un rapport de validation. A utiliser avant toute publication.
---

# Skill : Pipeline Validate

Valide visuellement les PDFs et images du pipeline Argumentum. Produit un rapport de validation avec problemes classes par priorite.

---

## Usage

```
/pipeline-validate [language] [cardset]
```

- `/pipeline-validate` — FR complet (defaut)
- `/pipeline-validate en` — Generation EN complete
- `/pipeline-validate fr Poker` — Uniquement PokerCards FR

---

## Pre-requis

- Serveur HTTP local (Playwright ne peut pas ouvrir `file://`) :
  ```bash
  cd "D:/Argumentum/Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target"
  python -m http.server 8765
  ```
- MCP Playwright disponible

---

## Workflow

### Phase 1 — Inventaire

```
Target/{lang}/Images/density-0/**/*.png
Target/{lang}/Documents/density-0/*.pdf
```

| CardSet | Face | Back | PDFs associes |
|---------|------|------|---------------|
| Fallacies | ~177 | ~7 | TarotCards, FallaciesWeb A0/A4/Thumbnails |
| Scenarii | ~97 | ~7 (par categorie) | PokerCards |
| Virtues | ~113 | 0 | TarotCards_Virtues |
| Rules | ~6 | 0 | Dans TarotCards |
| Memo | ~1 | ~1 | Dans TarotCards |

### Phase 2 — Dimensions images

```bash
python -c "from PIL import Image; i=Image.open('path'); print(i.size)"
```

**Regle critique** : face et back d'un meme CardSet DOIVENT avoir la meme taille en pixels. Mismatch = erreur critique.

| CardSet | Dimensions attendues |
|---------|----------------------|
| Fallacies Tarot | ~2659x1411px face ET back |
| Scenarii Poker | ~1447x1947px face ET back |
| Virtues Tarot | ~2659x1411px (FacesOnly) |

### Phase 3 — Verification Playwright + vision

Naviguer vers `http://localhost:8765/{lang}/Documents/density-0/{pdf_name}`, attendre 2-3s le rendu, screenshot, scroll, verifier.

#### Checklist PokerCards (AlternateFaceAndBack)
- [ ] Page 1 = face (titre, contexte, personnages)
- [ ] Page 2 = dos de la BONNE famille (pas toujours le meme dos)
- [ ] Face et dos meme taille
- [ ] 7 dos differents (Histoire, Mythologie, Politique, Pop Culture, Relation Intime, Vie Personnelle, Vie Professionnelle)
- [ ] Pages totales = 2 × nb scenarii

#### Checklist TarotCards (AlternateFaceAndBack)
- [ ] Alternance face/dos
- [ ] 7 dos par famille de sophisme
- [ ] Taille face = taille dos
- [ ] Rules et Memo inclus (FacesOnly)

#### Checklist FallaciesWeb A0
- [ ] 1 seule page
- [ ] Header (logo + QR code)
- [ ] 11-12 colonnes
- [ ] Cartes 69x69mm lisibles

#### Checklist FallaciesWeb A4
- [ ] Plusieurs pages (grille)
- [ ] Meme cartes que A0

#### Checklist Print&Play A4
- [ ] Recto-verso : faces d'un cote, dos de l'autre
- [ ] Header visible

#### Checklist TarotCards_Virtues (FacesOnly)
- [ ] Pas de dos
- [ ] 7 couleurs de famille
- [ ] 113 cartes

### Phase 4 — Dos par famille

Test critique PokerCards/TarotCards : les dos doivent varier selon la famille.

1. Lister les dos : `ls Target/{lang}/Images/density-0/{CardSet}/*.png` (sans `_face`)
2. Verifier 7 dos differents
3. Scroller dans le PDF pour confirmer l'alternance

**Si tous identiques** = BUG CRITIQUE. Causes connues :
- `RowsetNb` manquant pour BackCardSetInfo
- Chemins relatifs dans template back (doivent etre URLs GitHub absolus)
- Matching face/dos casse (IDs sans categorie)

### Phase 5 — Mind maps (SVG + HTML wrappers)

```
Cards/Fallacies/Mindmaps/{lang}/
  Fallacies_{lang}.svg            (Batik, avec interactions)
  Fallacies_{lang}.content.svg    (semantique)
  Fallacies_{lang}.links.svg      (avec <a> wrappers)
  Argumentum_Virtues_MindMap_{lang}.content.svg
  Argumentum_Virtues_MindMap_{lang}.links.svg
```

Et HTML wrappers :
```
Target/{lang}/Mindmaps/*.html
```

- [ ] Ouvrir chaque SVG en navigateur : tous les noeuds presents
- [ ] Ouvrir HTML included : fonctionne standalone
- [ ] Tester interactivite (hover, clic, liens)

### Phase 6 — OWL ontology

```
docs/ontology/argumentum.owl
```

- [ ] Ouvrir dans Protege
- [ ] Verifier structure SKOS (ConceptScheme, Concepts, narrower/broader)
- [ ] Verifier mapping AIF (URIs `http://www.arg.dundee.ac.uk/aif#`)
- [ ] Verifier labels multilingues (fr, en)

### Phase 7 — Rapport

Creer `Target/{lang}/validation-report-{timestamp}.md` :

```markdown
# Pipeline Validation Report

**Date:** YYYY-MM-DD HH:MM
**Language:** fr
**Validator:** Playwright + Vision

## Resume

| Verification | Status | Details |
|--------------|--------|---------|
| Images | OK/KO | X face + Y back |
| Dimensions | OK/KO | Face WxH vs Back WxH |
| PDFs | OK/KO | N documents |
| Alternance face/dos | OK/KO | ... |
| Dos varies | OK/KO | N types |
| Mind maps SVG | OK/KO | ... |
| OWL | OK/KO | ... |

## Problemes detectes

### [CRITIQUE] Titre
- **CardSet:** ...
- **Description:** ...
- **Cause probable:** ...
- **Action suggeree:** ...

## Screenshots

- `screenshot_*.png`
```

---

## Criteres de qualite

### Par CardSet
| # | Verification |
|---|--------------|
| 1 | Nombre d'images correct |
| 2 | Dimensions face == back |
| 3 | Pas d'images vides (>1KB) |
| 4 | Contenu visible |
| 5 | Texte lisible |
| 6 | Couleurs de famille presentes |
| 7 | Assets charges (icones) |

### Par PDF
| # | Verification |
|---|--------------|
| 1 | PDF non vide (>100KB) |
| 2 | Nb de pages attendu |
| 3 | Face/dos meme taille |
| 4 | Dos varies par famille |
| 5 | Pas de pages blanches |
| 6 | Pas de debordement |
| 7 | 1 page pour A0 |

---

## Rappels critiques

- **Serveur HTTP obligatoire** (pas de `file://`)
- **Attendre 2-3s** apres navigation avant screenshot
- **Face ET back** : mismatch = bug le plus frequent
- **Scroller** : pas que la page 1
- **7 dos minimum** par CardSet multi-familles
- **Screenshots** : chaque probleme documente visuellement
- **Validation visuelle reste ai-01** : ne pas deleguer a po-2023 (pas de vision)
