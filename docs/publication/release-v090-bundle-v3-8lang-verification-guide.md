# Guide de vérification release v0.9.0 — bundle v3 (8 langues, pour jsboige)

**Objet** : permettre à jsboige de valider le **bundle GDrive `review-v0.9.0-RELEASE-bundle-v3-2026-07-03/`**
(80 PDFs CMYK, 10 types × 8 langues) **en minutes**, y compris les langues qu'il ne lit pas
(ar/fa/zh, +ru). Prolonge le dossier v4.1 (`docs/RELEASE-VALIDATION-v0.9.0.md`, #698). Mandat
« présenter le dossier proprement au tag » (dispatch `yj7u3j`, secondary).

**Base** : master `dc02e847`, bundle v3 régénéré `27442add` + post-process Ghostscript CMYK #632/#652.

> **Prérequis** : ouvrir les PDFs depuis le dossier GDrive. Pour les checks DevTools
> (`dir`/`lang`), un viewer PDF avec inspection texte suffit (pdftotext, ou ouvrir dans Chrome).
> Ce guide est complémentaire du `validation-guide.fr.md` (procédure générale, 2026-05-31) et du
> `non-latin-verification-guide.md` (chaînes UI site DNN) — il est **ciblé bundle v3** (10 types × 8
> langs, post-expansion P&P #648-650 + CMYK).

---

## TL;DR — quoi sampler par langue (5 min/lang)

Pour chaque langue, **1 seule carte-densité** (`Fallacies_Web_Thumbnails_<lang>.pdf` page 1) suffit à
valider la localisation, la direction, et l'absence de tofu. Ajouter **1 page Rules**
(`TarotCards_<lang>.pdf` page 1) pour le contenu Rules. **Total : 16 PDFs ouverts (2 par langue) =
validation 8-lang en ~15 min.**

| Langue | Quoi regarder | Attendu (PASS) | Échec typique |
|--------|---------------|----------------|---------------|
| **fr** | `TarotCards_fr.pdf` p1, `Fallacies_Web_Thumbnails_fr.pdf` p1 | référence canonique | — |
| **en** | `Fallacies_Web_Thumbnails_en.pdf` p1 | contenu EN (ex. "Fallacy" titres), pas de FR | fuite FR (#216) |
| **ru** | `Fallacies_Web_Thumbnails_ru.pdf` p1 | **Cyrillique** rendu ("Софизм"), pas de tofu □ | glyphe absent → □ |
| **pt** | `TarotCards_pt.pdf` p1 (cover Rules) | titre « A Escola dos Mentirosos » (pas "Liars' School" EN) | fuite EN résiduelle |
| **es** | `Fallacies_Web_Thumbnails_es.pdf` p1 | contenu ES ("Falacia") | fuite FR |
| **ar** | `Fallacies_Web_Thumbnails_ar.pdf` p1 | **RTL** (texte commence à droite), police arabe (Noto Naskh), pas de tofu | RTL cassé / tofu |
| **fa** | `Fallacies_Web_Thumbnails_fa.pdf` p1 | **RTL**, police persane (Vazirmatn), pas de tofu | RTL cassé / tofu |
| **zh** | `Fallacies_Web_Thumbnails_zh.pdf` p1 | **CJK** ("谬误"), police SC (Noto Sans SC), pas de tofu | tofu □ |

---

## 1. Les 10 types de documents du bundle v3

Le bundle = **10 types × 8 langues = 80 PDFs** (expansion P&P #648-650 : +2 types vs les 8 historiques).

| Type | Fichier (suffixe `_<lang>`) | Risque localisation |
|------|------------------------------|---------------------|
| Tarot (Rules + Memo + Fallacies) | `Argumentum_TarotCards_<lang>.pdf` | titres longs, débordement, cover Rules |
| Tarot Virtues | `Argumentum_TarotCards_Virtues_<lang>.pdf` | noms de familles Virtues |
| Poker (Scenarii) | `Argumentum_PokerCards_<lang>.pdf` | contexte Scenarii |
| Fallacies Web A0 | `Fallacies_Web_A0_<lang>.pdf` (99 MB) | densité A0, 1408 nœuds |
| Fallacies Web A4 | `Fallacies_Web_A4_<lang>.pdf` | — |
| Fallacies Web Thumbnails | `Fallacies_Web_Thumbnails_<lang>.pdf` | **échantillon recommandé** (carte-densité p1) |
| P&P Standard | `PrintAndPlay_Standard_<lang>.pdf` | (#648) toutes les cartes |
| P&P Light | `PrintAndPlay_Light_<lang>.pdf` | (#649/#650) colonne `print_and_play` + Virtues overview |
| P&P Tarot | `PrintAndPlay_Tarot_<lang>.pdf` | recto-verso |
| P&P Poker | `PrintAndPlay_Poker_<lang>.pdf` | recto-verso |

> **Validation minimale** : les `Thumbnails` (1 par langue) + `TarotCards` p1 (1 par langue) couvrent
> Rules + Fallacies + Scenarii + Virtues + densité. Les A0/A4/P&P sont les mêmes contenus en
> autre format — pas besoin de tous les ouvrir pour valider la localisation.

---

## 2. Contrôles indépendants du sens (valider sans lire)

Trois contrôles qui ne demandent pas de comprendre la langue :

### 2.1 Direction (RTL pour ar/fa)
- **DevTools / inspection texte** : extraire le texte (`pdftotext` ou Chrome) ; pour ar/fa, la première
  ligne lue droite-à-gauche. Ou visuellement : le texte commence à **droite** de la carte.
- **Échec** : le texte ar/fa commence à gauche (direction LTR appliquée par erreur).

### 2.2 Glyphe + police (pas de tofu)
- **Tofu** = carrés vides □ = glyphe absent de la police. À l'œil : si les caractères sont des □, échec.
- **Polices attendues** : ar = Noto Naskh Arabic, fa = Vazirmatn, zh = Noto Sans SC, ru = police
  Cyrillique standard. Tofu = défaut de police viewer OU défaut asset (à distinguer).
- **Échec** : tofu massif sur une langue = défaut asset (pas seulement viewer).

### 2.3 Contenu localisé (pas de fuite FR #216)
- Pour en/ru/pt/es/ar/fa/zh : un **mot français** ("Sophisme", "les", "des", "école") dans une carte
  non-FR = fuite FR (bug #216, censé résolu).
- **Spot-check rapide** : ouvrir `Thumbnails_<lang>.pdf` p1, regarder les titres de cartes. Tous dans
  la langue cible, aucun mot FR.

---

## 3. Checks spécifiques par langue (quoi sampler précisément)

### fr (référence canonique)
- **Ouvrir** : `TarotCards_fr.pdf` p1, `Fallacies_Web_Thumbnails_fr.pdf` p1.
- **Attendu** : tout en FR. Cover Rules = « L'école des menteurs ». C'est la référence pour comparer
  les autres langues (même layout, contenu traduit).

### en
- **Ouvrir** : `Fallacies_Web_Thumbnails_en.pdf` p1.
- **Attendu** : titres "Fallacy" (pas "Sophisme"). Pas de FR leak.

### ru (Cyrillique)
- **Ouvrir** : `Fallacies_Web_Thumbnails_ru.pdf` p1.
- **Attendu** : titres en Cyrillique ("Софизм"). **Pas de tofu**. La micro-RU PK79 (`чшск-то` garble)
  est **résolue** sur le harvest frais (verdict ai-01 #140) — confirmer visuellement qu'aucun glyphe
  Cyrillique ne est corrompu.

### pt
- **Ouvrir** : `TarotCards_pt.pdf` p1 (cover Rules).
- **Attendu** : titre cover = **« A Escola dos Mentirosos »** (fix #306). **Pas de fuite EN** (« Liars'
  School ») — c'était le finding PT pré-fix.

### es
- **Ouvrir** : `Fallacies_Web_Thumbnails_es.pdf` p1.
- **Attendu** : titres "Falacia" (ES). Pas de FR leak.

### ar (RTL)
- **Ouvrir** : `Fallacies_Web_Thumbnails_ar.pdf` p1.
- **Attendu** : **RTL** (texte droite→gauche), police Noto Naskh Arabic, **pas de tofu**. Famille-racine
  = « السفسطة » (ar). Cover Rules = « مدرسة الكذابين ».
- **Vérifier DevTools** : `dir="rtl"` sur le conteneur (ou le texte commence visiblement à droite).

### fa (RTL)
- **Ouvrir** : `Fallacies_Web_Thumbnails_fa.pdf` p1.
- **Attendu** : **RTL**, police Vazirmatn, **pas de tofu**. Pas de FR leak.

### zh (CJK)
- **Ouvrir** : `Fallacies_Web_Thumbnails_zh.pdf` p1.
- **Attendu** : **CJK** ("谬误" = fallacy), police Noto Sans SC, **pas de tofu**. Les caractères zh sont
  compacts (plus courts en char-count que les autres langs — c'est normal, pas un défaut).

---

## 4. Vérification CMYK (1 check, transverse)

Le bundle v3 = **80/80 PDFs CMYK** (post-process Ghostscript #632/#652). Validation :

- **Ouvrir 1 PDF** (ex. `TarotCards_fr.pdf`) dans un viewer avec inspection colorspace, OU lancer :
  ```
  pdfimages -list TarotCards_fr.pdf | head
  ```
- **Attendu** : colonne `color` = `cmyk` (4-composantes), pas `rgb` (3-composantes). + OutputIntent
  SWOP présent (le PDF embarque le profil ICC SWOP).
- **Preuve ai-01 #632** : `TarotCards_fr` = DeviceCMYK 195 / DeviceRGB 0 / OutputIntent 3 / ICC SWOP 2.

> Si un PDF montre `rgb` : c'est qu'il n'est pas passé par le post-process Ghostscript (le path
> `DocumentCardSet.cs` oxymore PNG — voir CLAUDE.md). Le bundle v3 les a tous convertis ; un PDF rgb
  serait une régression.

---

## 5. Ordre de validation recommandé (15 min total)

1. **fr** (référence, 1 min) — `TarotCards_fr.pdf` p1 + `Thumbnails_fr.pdf` p1.
2. **CMYK** (1 min) — 1 check `pdfimages -list` sur `TarotCards_fr.pdf` (§4).
3. **en/ru/es** (3 min) — `Thumbnails_<lang>.pdf` p1 chacun (contenu + pas de FR leak + Cyrillique ru).
4. **pt** (1 min) — `TarotCards_pt.pdf` p1 (cover « A Escola dos Mentirosos »).
5. **ar/fa/zh** (5 min) — `Thumbnails_<lang>.pdf` p1 chacun (RTL + police + pas de tofu).
6. **Verdict** : si les 16 PDFs (2/lang) passent → bundle 8-lang validé. Les 64 autres PDFs (autres
   types/format) sont les mêmes contenus.

---

## 6. Si un défaut est trouvé

| Défaut | Probable cause | Action |
|--------|----------------|--------|
| Tofu massif (ar/fa/zh) | police absente viewer OU défaut asset | tester autre viewer d'abord ; si tofu persistant = défaut asset → blocker |
| FR leak (en/ru/pt/es/ar/fa/zh) | #216 régression (censé résolu) | blocker — spot-check la carte, ouvrir issue |
| RTL cassé (ar/fa) | `dir` non appliqué | blocker (mais les PDFs ont le RTL figé en absolu — rare) |
| Cover PT = "Liars' School" | fuite EN résiduelle | finding PT (§3.6 dossier, déjà known) |
| RGB au lieu de CMYK | post-process GS manquant | blocker (mais bundle v3 = 80/80 CMYK, régression seulement) |

---

## Statut

Guide ciblé bundle v3 (80 PDFs, 10 × 8, post-P&P + CMYK). Complémentaire du `validation-guide.fr.md`
(procédure générale) et du `non-latin-verification-guide.md` (site DNN UI strings). Pas de double-emploi :
les guides existants datent d'avant l'expansion P&P (#648-650) et du post-process CMYK (#632/#652).

Relates: dispatch `yj7u3j` (secondary), #134 (release), #140 (QA multilingue), #632/#652 (CMYK),
#648-650 (P&P), #306 (PT cover fix), #216 (FR-leak guard), #698 (dossier v4.1).
