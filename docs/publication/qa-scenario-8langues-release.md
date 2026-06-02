# Scénario de vérification QA — Release 8 langues

> **Date** : 2026-06-02 · **Build** : Release (CMYK/PNG) · **Commit** : `3aade349`
> **PDFs** : 64 (8 langues × 8 documents) · **Total** : ~11.4 GB
> **Emplacement** : `bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/`

---

## 0. Setup

Ouvrir un explorateur sur le dossier Target :

```
Generation\Converters\Argumentum.AssetConverter\bin\Release\net9.0-windows\Target\
```

PDF reader recommandé : Acrobat Reader (les PDF sont lourds, 100-230 MB pour les Tarot).

**8 PDFs par langue** (mêmes noms, seul le suffixe `_xx` change) :

| # | Fichier | Risque principal | Pages |
|---|---------|-----------------|-------|
| 1 | `Argumentum_TarotCards_xx.pdf` | Rules + Memo + Fallacies — le plus gros | ~180 |
| 2 | `Argumentum_TarotCards_Virtues_xx.pdf` | **Overflow body #190** — le plus fragile | ~120 |
| 3 | `Argumentum_PokerCards_xx.pdf` | Scenarii — longs paragraphes | ~100 |
| 4 | `Argumentum_Fallacies_Web_A4_xx.pdf` | Grille dense 66×66 mm | ~8 |
| 5 | `Argumentum_Fallacies_Web_A0_xx.pdf` | Poster 12 colonnes | 1 |
| 6 | `Argumentum_Fallacies_Web_Thumbnails_A4_xx.pdf` | Vignettes 50×50 | ~4 |
| 7 | `Argumentum_TarotCards_Print&Play_A4_xx.pdf` | Recto-verso maison | ~40 |
| 8 | `Argumentum_PokerCards_Print&Play_A4_xx.pdf` | Scenarii maison | ~12 |

---

## 1. FR — Baseline de référence (~5 min)

> **Objectif** : mémoriser la position/taille du texte sur 2-3 cartes types pour comparer les autres langues.

### Ouvrir : `TarotCards_fr.pdf`

1. **Page 1** : couverture Rules — vérifier « L'École des Menteurs »
2. **Pages 2-3** : Rules cards — noter la position du texte
3. **Naviguer aux Fallacies** (~page 5+) :
   - Repérer 1 carte par famille (couleurs : gris / violet / rose / turquoise / vert / bleu / jaune / rouge)
   - Noter la taille relative du **titre** dans le bandeau et du **corps** de la carte
4. **PK457 « Déstabilisation »** : chercher cette carte dans la famille « Influence » — elle doit avoir un fond coloré (pas blanc)

### Ouvrir : `TarotCards_Virtues_fr.pdf`

1. **PK 197** (Échange enrichissant) : description 159 chars — la plus longue FR. Vérifier que le texte tient dans la carte sans déborder.
2. Noter la zone occupée par le texte sur cette carte = **référence max** pour les autres langues.

---

## 2. Quick scan universel — 8 langues (~10 min)

> **Objectif** : en ~1 min/langue, parcourir 100% des pages du **TarotCards** pour les drapeaux rouges.

Pour chaque langue `fr → en → pt → es → ru → ar → fa → zh` :

1. Ouvrir `TarotCards_xx.pdf`
2. **Defiler toutes les pages** en vue page-entière (~1 carte/sec)
3. Chercher :

| ❌ Drapeau rouge | Ce que tu vois | Cause |
|---|---|---|
| Carte fond blanc | Pas de couleur de famille | CSS classe manquante |
| Carte quasi vide | Seuls les cadres/icônes | CSV non injecté / cellule vide |
| Texte hors cadre | Déborde en bas/à droite | Overflow (#190, #316) |
| Carrés `□□□` | Losanges à la place des lettres | Police manquante |
| Mojibake `Ã©`, `Ð¿` | Caractères illisibles | Encodage cassé |
| Texte d'une autre langue | ex. anglais sur carte russe | Fuite de langue (#216) |

**Résultat attendu** : aucune carte ne doit déclencher un ❌. Si une seule carte pose problème → noter la page et le symptôme.

---

## 3. Vérifications ciblées par langue

### 3a. RU — Auto-shrink titres longs (~3 min)

> **Risque #316** : les titres russes sont 30-40% plus longs que le FR. Le correctif auto-shrink (#400, #420) doit réduire la police automatiquement.

**Ouvrir** : `TarotCards_ru.pdf`

Chercher ces **10 titres les plus longs** (nav page / Ctrl+F) :

| # | Decimal path | Titre RU | Chars |
|---|---|---|---|
| 1 | 3.1321400 | Путаница между индивидуальными и агрегированными корреляциями | 61 |
| 2 | 4.3321000 | Недопустимое утверждение на основе отрицательной посылки | 56 |
| 3 | 3.1321300 | Путаница между средним значением для группы и для всего | 55 |
| 4 | 4.3322000 | Недопустимое отрицание на основе утвердительных посылок | 55 |
| 5 | 2.1156000 | Обращение к многочисленным авторитетным источникам | 50 |
| 6 | 6.3121113 | Ложное убеждение о фиксированном объеме труда | 45 |
| 7 | 7.2112200 | Два неправильных действия образуют правильное | 45 |
| 8 | 2.3221111 | Неявное использование техники "ноги в двери" | 44 |
| 9 | 1.1312000 | Путаница между необъясненным и необъяснимым | 43 |
| 10 | 4.3312100 | Неправильное распределение старшего термина | 43 |

**Critères PASS** :
- ✅ Le titre tient **entièrement** dans le bandeau de la carte
- ✅ La police est plus petite que le FR (auto-shrink visible)
- ✅ Pas de débordement en bas ni à droite
- ❌ Le titre coupe en milieu de mot ou déborde → FAIL

**PK457 vérif** : la carte « Déstabilisation » (chemin 2.3231000) doit afficher la famille RU « Влияние → Манипуляция сознанием → Игра престолов » (pas de labels vides).

### 3b. AR — RTL + cursif (~3 min)

**Ouvrir** : `TarotCards_ar.pdf` + `TarotCards_Virtues_ar.pdf`

| Check | Attendu | ❌ Fail si |
|-------|---------|-----------|
| **Sens du texte** | Texte commence à **droite**, aligné droite | Texte aligné gauche comme FR |
| **Lettres connectées** | Arabe cursif, lettres liées | Lettres détachées/espacées |
| **Pas de tofu** | Aucun `□` | Carrés visibles |
| **Pas de cellule vide** | Toutes les cartes ont du contenu texte | Cartes avec seulement les icônes |
| **Overflow** | Texte dans le cadre | Débordement |

**Cartes ciblées Virtues AR** :
- PK 175 (Honnêteté intellectuelle) : description AR 141 chars — la plus longue
- PK 212 (Échange enrichissant) : description AR 129 chars

### 3c. FA — RTL + persan (~3 min)

**Ouvrir** : `TarotCards_fa.pdf` + `TarotCards_Virtues_fa.pdf`

| Check | Attendu | ❌ Fail si |
|-------|---------|-----------|
| **Sens RTL** | Même que AR | Texte aligné gauche |
| **Lettres persanes** | **پ چ ژ گ** bien rendues | Tofu sur ces 4 lettres |
| **ی/ک persans** | Formes persanes (pas arabes) | Pas distinguables |
| **Overflow** | Texte dans le cadre | Débordement |

**Cartes ciblées Virtues FA** :
- PK 176 (Honnêteté intellectuelle) : description FA 174 chars — la plus longue
- PK 175 (Honnêteté intellectuelle) : description FA 151 chars

### 3d. ZH — CJK (~3 min)

**Ouvrir** : `TarotCards_zh.pdf` + `TarotCards_Virtues_zh.pdf`

| Check | Attendu | ❌ Fail si |
|-------|---------|-----------|
| **Sinogrammes rendus** | Aucun `□` | Carrés visibles |
| **Pas de cellule vide** | Autant de cartes pleines qu'en FR | Cartes avec seulement icônes |
| **Pas de caractère coupé** | Sinogrammes entiers | Caractère coupé en deux par le bord |
| **Ponctuation pleine largeur** | `。` `，` `、` visibles | Absents ou carrés |

**Note** : les descriptions ZH sont plus courtes (~47 chars max) → overflow peu probable. Le risque principal est le **tofu** (police CJK manquante) et les **cellules vides** (#403).

### 3e. EN — Pas de résidu FR (~1 min)

**Ouvrir** : `TarotCards_en.pdf` (pages Fallacies, ~page 5+)

- **Critère unique** : aucune carte ne doit contenir de mot français. Si tu vois « Définition », « Exemple », « Sophisme » → ❌ régression #216.

### 3f. PT / ES — Accents (~1 min chacun)

**Ouvrir** : `TarotCards_pt.pdf` puis `TarotCards_es.pdf`

| Langue | Caractères à vérifier |
|--------|----------------------|
| PT | **ã õ ç á ê é í ó ú** — pas de tofu, pas de mojibake |
| ES | **ñ ¿ ¡ á í ó ú ü** — pas de tofu, pas de mojibake |

---

## 4. FallaciesWeb A0 — poster géant (~2 min)

> Risque : l'A0 tient sur **1 seule page** (841×1189 mm). Toute anomalie est visible immédiatement.

**Ouvrir** : `Fallacies_Web_A0_xx.pdf` pour FR + AR + ZH (3 langues risquées)

| Check | Attendu |
|-------|---------|
| 12 colonnes complètes | Pas de colonne vide |
| Entête logo + QR code | Visible en haut |
| Texte dans les cellules 69×69 mm | Pas de débordement |
| AR : direction RTL du contenu | Cellules arabes alignées droite |
| ZH : sinogrammes dans chaque cellule | Pas de tofu |

---

## 5. Grille de résultat

Copier et cocher :

```
LANGUE    | TarotCards | Virtues | Poker | A0  | P&P | Verdict
----------|------------|---------|-------|-----|-----|--------
FR        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
EN        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
PT        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
ES        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
RU        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
AR        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
FA        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
ZH        | ☐          | ☐       | ☐     | ☐   | ☐   | ☐ PASS / ☐ FAIL
```

**Temps estimé total** : ~25-30 minutes

---

## 6. Si FAIL — que faire

| Symptôme | Issue | Action |
|----------|-------|--------|
| Overflow titre RU | #316 / #400 | Vérifier auto-shrink dans `virtues-card.css` — la regex `font-size` |
| Carte vide AR/FA/ZH | #403 | Relancer audit `argu_i18n_audit2.py` — colonne cible vide |
| Tofu CJK/RTL | Nouveau | Font-face manquante dans template CardPen — ajouter Google Font |
| Fuite FR dans EN | #216 | Vérifier `CardSetLocalizations.FrontFieldConversions` dans config |
| Overflow Virtues body | #190 / #420 | Vérifier auto-shrink body PR #420 — `minFontSize: 7` |

---

## Issues liées

- **#134** — GitHub Release v0.9.0 (gate = ce scénario PASS)
- **#140** — QA multilingue (ce scénario en est l'instrument)
- **#316** — Overflow titres RU (auto-shrink)
- **#190** — Overflow Virtues
- **#420** — Auto-shrink body Virtues
- **#421** — PK457 RU labels
- **#216** — Fuite de langue FR
