# Skill: Pipeline Validate

Valide visuellement les PDFs et images du pipeline Argumentum en utilisant Playwright et la vision.

## Usage

```
/pipeline-validate [language] [cardset]
```

**Exemples:**
- `/pipeline-validate` - Valide FR complet (defaut)
- `/pipeline-validate en` - Valide generation EN
- `/pipeline-validate fr Poker` - Valide uniquement PokerCards FR

## Pre-requis

- Serveur HTTP local sur les fichiers generes:
  ```bash
  cd "D:\Dev\Argumentum\Generation\Converters\Argumentum.AssetConverter\bin\Debug\net9.0\Target"
  python -m http.server 8765
  ```
- MCP Playwright disponible

## Workflow Automatique

### Phase 1: Inventaire des Fichiers Generes

Utilise **Glob** pour recenser:

```
Target/{lang}/Images/density-0/**/*.png
Target/{lang}/Documents/density-0/*.pdf
```

**Compter et rapporter:**

| CardSet | Images face | Images back | Total | PDFs |
|---------|------------|-------------|-------|------|
| Fallacies | ~177 faces | ~7 backs | ~184 | TarotCards, FallaciesWeb_A0/A4 |
| Scenarii | ~97 faces | ~7 backs (par categorie) | ~104 | PokerCards |
| Virtues | ~113 faces | 0 back | ~113 | TarotCards_Virtues |
| Rules | ~6 faces | 0 back | ~6 | Dans TarotCards |
| Memo | ~1 face | ~1 back | ~2 | Dans TarotCards |

### Phase 2: Verification des Dimensions Images

Pour chaque CardSet, verifier avec Python/Pillow les dimensions pixel des images face ET back:

```bash
python -c "from PIL import Image; i=Image.open('path'); print(i.size)"
```

**Regle critique:** Les images face et back d'un meme CardSet DOIVENT avoir les MEMES dimensions en pixels.

| CardSet | Dimensions attendues face | Dimensions attendues back | Tolerance |
|---------|--------------------------|--------------------------|-----------|
| Fallacies Tarot | ~2659x1411px | IDENTIQUE | 0px |
| Scenarii Poker | ~1447x1947px | IDENTIQUE | 0px |
| Virtues Tarot | ~2659x1411px | N/A (FacesOnly) | N/A |

**Si mismatch detecte:** RAPPORTER IMMEDIATEMENT comme erreur critique.

### Phase 3: Verification Visuelle via Playwright

**Demarrer le serveur HTTP** (si pas deja fait):
```bash
cd "D:\Dev\Argumentum\Generation\Converters\Argumentum.AssetConverter\bin\Debug\net9.0\Target"
python -m http.server 8765 &
```

**Pour chaque PDF:**

1. **Naviguer** vers `http://localhost:8765/{lang}/Documents/density-0/{pdf_name}`
2. **Attendre le chargement** (le viewer PDF Chromium met 2-3s)
3. **Prendre un screenshot** de la premiere page
4. **Scroller** pour voir pages suivantes (faces, dos, alternance)
5. **Prendre des screenshots** des premieres pages visibles

**Checklist par type de PDF:**

#### PokerCards (AlternateFaceAndBack)
- [ ] Page 1 = face de carte Scenarii (titre, contexte, personnages)
- [ ] Page 2 = dos de carte correspondant a la FAMILLE (pas toujours le meme dos!)
- [ ] Les faces et dos ont la MEME TAILLE sur chaque page
- [ ] 7 types de dos differents: Histoire, Mythologie, Politique, Pop Culture, Relation Intime, Vie Personnelle, Vie Professionnelle
- [ ] Nombre total de pages = 2 * nombre de scenarii (face + dos pour chaque)

#### TarotCards (AlternateFaceAndBack)
- [ ] Alternance face/dos correcte
- [ ] Dos varies par famille de sophisme (7 familles)
- [ ] Taille face = taille dos
- [ ] Cartes Rules et Memo incluses (FacesOnly, sans dos)

#### FallaciesWeb A0 (PrintAndPlay)
- [ ] 1 seule page
- [ ] Logo/QR code visible en haut (header)
- [ ] 11-12 colonnes de cartes
- [ ] Cartes 69x69mm lisibles

#### FallaciesWeb A4 (PrintAndPlay)
- [ ] Plusieurs pages (grille de cartes sur chaque page)
- [ ] Meme cartes que A0 mais sur plusieurs pages A4

#### Print&Play A4 (PrintAndPlay)
- [ ] Recto-verso: faces d'un cote, dos de l'autre
- [ ] Logo/header visible

#### TarotCards_Virtues (FacesOnly)
- [ ] Faces uniquement (pas de dos)
- [ ] 7 couleurs de famille presentes
- [ ] 113 cartes

### Phase 4: Verification des Dos par Famille

**Test critique pour PokerCards et TarotCards:**

Les cartes doivent avoir des dos differents selon leur famille/categorie.

**Methode de verification:**
1. Lister les images de dos generees: `ls Target/{lang}/Images/density-0/Scenarii/*.png` (sans `_face`)
2. Verifier qu'il y a 7 dos differents (un par categorie)
3. Dans le PDF, scroller pour verifier que les dos alternent (pas toujours le meme)

**Si tous les dos sont identiques:** C'est un BUG CRITIQUE.

**Causes connues:**
- `RowsetNb` manquant pour BackCardSetInfo (template back utilise `{{rowset.[0].categorie}}`)
- Chemins d'assets relatifs dans template back (doivent etre absolus GitHub URLs)
- Matching face/dos casse (IDs sans categorie apres tiret)

### Phase 5: Rapport de Validation

**Creer fichier:** `Target/{lang}/validation-report-{timestamp}.md`

**Structure du rapport:**

```markdown
# Pipeline Validation Report

**Date:** YYYY-MM-DD HH:MM
**Language:** fr
**Validator:** Playwright + Vision

## Resume

| Verification | Statut | Details |
|-------------|--------|---------|
| Images generees | OK/KO | X face + Y back |
| Dimensions uniformes | OK/KO | Face WxH vs Back WxH |
| PDFs generes | OK/KO | N documents |
| Alternance face/dos | OK/KO | Verifie visuellement |
| Dos varies par famille | OK/KO | N types de dos |
| Logo A0 | OK/KO | Taille header |
| Print&Play recto-verso | OK/KO | Verifie visuellement |

## Problemes Detectes

### [CRITIQUE] Titre du probleme
- **CardSet:** ...
- **Description:** ...
- **Cause probable:** ...
- **Action suggeree:** ...

## Screenshots

- `screenshot_poker_page1.png` - Face Scenarii
- `screenshot_poker_page2.png` - Dos Scenarii
- ...
```

## Criteres de Qualite (Checklist Complete)

### Par CardSet

| # | Verification | Commande/Methode |
|---|-------------|-----------------|
| 1 | Nombre d'images correct | `ls -1 | wc -l` |
| 2 | Dimensions face et back identiques | Python PIL |
| 3 | Pas d'images vides (>1KB) | `ls -la | awk '$5<1000'` |
| 4 | Contenu visible (pas tout blanc/noir) | Screenshot Playwright |
| 5 | Texte lisible | Screenshot Playwright |
| 6 | Couleurs de famille presentes | Screenshot Playwright |
| 7 | Assets charges (images, icones) | Screenshot Playwright |

### Par PDF

| # | Verification | Commande/Methode |
|---|-------------|-----------------|
| 1 | PDF non vide (>100KB) | `ls -la` |
| 2 | Nombre de pages attendu | Playwright (compteur en haut) |
| 3 | Face/dos meme taille | Screenshot Playwright |
| 4 | Dos varies par famille | Scroll + screenshots |
| 5 | Pas de pages blanches | Scroll + screenshots |
| 6 | Pas de debordement de texte | Screenshots |
| 7 | 1 page pour A0 | Playwright (compteur) |

## Rappels Critiques

- **Serveur HTTP obligatoire:** Playwright ne peut pas ouvrir `file://` URLs
- **Attendre le rendu PDF:** Prendre screenshot 2-3s apres navigation
- **Verifier face ET back:** Le probleme le plus frequent est le mismatch de taille
- **Scroller dans le PDF:** Ne pas se contenter de la page 1
- **Compter les types de dos:** 7 familles = 7 dos differents minimum
- **Documenter avec screenshots:** Chaque probleme doit avoir une capture
