# Argumentum v0.9.0 — Relecture visuelle (Thomas & Adeline)

Merci de prêter vos yeux aux visuels du jeu avant la publication de la **v0.9.0** 🙏
Ce dossier est un **parcours guidé** (~45 min) : *ouvre ceci, vérifie ça, voici ce qui est normal,
voici ce qu'il ne faut pas signaler*. Pas besoin d'être exhaustif — 6 cartes clés suffisent.

Le jeu est décliné en **8 langues** : 🇫🇷 FR (référence) · 🇬🇧 EN · 🇷🇺 RU · 🇵🇹 PT · 🇪🇸 ES · 🇸🇦 AR · 🇮🇷 FA · 🇨🇳 ZH.

---

## 1. Où récupérer les PDFs (2 min)

Les cartes prêtes à imprimer sont en **assets de la pre-release GitHub** :

> 👉 **[Pre-release « v0.9.0 — Visual Review »](RELEASE_URL_PLACEHOLDER)** *(lien mis à jour à la publication)*

Pour la première passe, on met en ligne les **`Argumentum_TarotCards_<langue>.pdf`** (les 8 langues) — elles
couvrent tous les cas délicats (densité, écritures non-latines, covers, cartes de règles). Les autres decks
(Poker/Scénarios, posters Web A0/A4, Print & Play, Vertus) sont disponibles **sur demande** — dites-le et on
les ajoute.

> ⚠️ **Lecteur PDF** : ouvrez avec un viewer qui embarque les polices CJK + arabe (**Acrobat Reader** ou
> **SumatraPDF**). Un viewer sans ces polices affiche de faux « carrés vides □ » (*tofu*) qui **ne sont pas**
> un défaut du fichier (voir §4).

---

## 2. Les 6 arrêts prioritaires (~20 min)

Pas besoin de tout regarder. Ces 6 cartes couvrent les cas-limites. Si elles sont bonnes, le reste l'est
(même pipeline, mêmes polices).

| # | Fichier | Page | Quoi vérifier | Cas couvert |
|---|---------|------|---------------|-------------|
| 1 | `Argumentum_TarotCards_fr.pdf` | p1-2 | cover FR + recto-verso aligné | référence FR |
| 2 | `Argumentum_TarotCards_pt.pdf` | p4 | titre de la carte de **règles PT** lisible et correct | PT / règles |
| 3 | `Argumentum_TarotCards_ar.pdf` | p51 (carte dense) | texte **arabe** de droite à gauche, aligné à droite | AR (RTL) |
| 4 | `Argumentum_TarotCards_fa.pdf` | p51 | **persan** de droite à gauche + lettres persanes (پ چ ژ گ) | FA (RTL) |
| 5 | `Argumentum_TarotCards_zh.pdf` | p51 | caractères **chinois** rendus, pas de débordement hors cadre | ZH (CJK) |
| 6 | `Argumentum_TarotCards_ru.pdf` | p1 + une carte dense | **cyrillique** + accents | RU |

**Après ces 6 arrêts** : si tout est visuellement correct, on considère les autres cartes fiables.
Si un problème → notez la langue + la carte, ce n'est probablement pas une régression globale.

---

## 3. Non-latin : « normal » vs « cassé »

Le principal risque d'une relecture multilingue, c'est le **faux positif** (signaler comme cassé ce qui est
correct dans une écriture qu'on ne lit pas). Repères :

### 🇸🇦 Arabe / 🇮🇷 Persan (de droite à gauche)
- **Normal** : le texte court **de droite à gauche**, aligné à droite. Les **chiffres restent de gauche à
  droite** dans le flux (standard Unicode, ce n'est pas un bug). AR et FA partagent l'alphabet ; FA ajoute
  4 lettres (پ چ ژ گ) — vérifier qu'elles s'affichent.
- **Cassé** (à signaler) : texte de gauche à droite sur une page censée être RTL · carrés vides □ · lettres
  détachées qui devraient être liées (ligatures cassées).

### 🇨🇳 Chinois (CJK)
- **Normal** : caractères en blocs carrés. Le chinois est **plus large** que le latin → un texte « tassé »
  ou qui touche le bord n'est pas forcément un bug.
- **Cassé** (à signaler) : carrés vides □ · demi-caractères · une police latine visible au milieu du chinois ·
  un vrai **débordement hors du cadre** de la carte.

### 🇷🇺 Cyrillique
- **Normal** : alphabet А-Я а-я, accents, ё / й.
- **Cassé** (à signaler) : carrés vides · lettres latines à la place de cyrilliques qui se ressemblent
  (un `a` latin au lieu d'un `а` cyrillique).

### 🇪🇸 Espagnol / 🇵🇹 Portugais (latin étendu)
- **Normal** : accents (é è ê, ã õ, ç, ñ, à). PT = brésilien.
- **Cassé** (à signaler) : accents manquants ou mojibake · un « ? » ou un carré noir à la place d'un accent.

---

## 4. Déjà connu — **ne pas signaler** comme défaut

| Ce que vous pourriez voir | Pourquoi | Action |
|---------------------------|----------|--------|
| Carrés vides □ sur du chinois / de l'arabe | police **de votre lecteur**, pas du fichier | ouvrez dans Acrobat/SumatraPDF ; ne pas signaler |
| Sur une carte de règles EN, un titre bizarre (« Roll of the English Channel ») | mistraduction résiduelle **déjà en cours de correction** | connu, correctif en route — inutile de le remonter |
| Les mnémoniques latins des cartes Vertus (mots type « bArbArA ») | choix éditorial assumé (latin conservé) | normal |
| Poster A0 très lourd à ouvrir | 106 Mo, haute résolution d'impression | normal |

---

## 5. Comment nous remonter vos retours

- **Idéal** : commentez directement sur l'**issue GitHub de suivi** (lien dans la pre-release / donné par
  jsboige). Une case à cocher par langue + un espace pour vos remarques : voir [`CHECKLIST.md`](CHECKLIST.md).
- **Sinon** : envoyez vos remarques à jsboige (WhatsApp), on les reporte pour vous.

**Format utile pour une remarque** : *langue + fichier + page + ce qui cloche* (une phrase suffit).
Exemple : « ZH — TarotCards_zh p51 — le titre déborde en bas du cadre ».

---

## 6. Ce qui est déjà validé en interne (pour info, inutile de re-vérifier)

- ✅ **Contenu 8 langues** (relecture interne, 03/07) : cartes denses, RTL/CJK, covers, règles.
- ✅ **Colorimétrie CMYK** : 80/80 PDFs en CMYK + profil d'impression SWOP.
- ✅ **Structure** : les cartes de règles apparaissent en premier · recto-verso aligné · 300 PPI.
- ✅ **Pas de fuite du français** dans les autres langues (structure multilingue intacte).

Votre relecture = le **dernier regard humain** avant le tag. Merci encore 🎴✨
