# Argumentum v0.9.0 — Relecture visuelle complète (Thomas & Adeline)

Merci de prêter vos yeux à **l'ensemble des livrables** du jeu avant la publication de la **v0.9.0** 🙏
Ce dossier est un **parcours guidé** : *où récupérer chaque document, quoi vérifier, ce qui est normal,
ce qu'il ne faut pas signaler*.

⚠️ **Important** : il ne s'agit **pas** que des cartes Tarot. Tout le paquet doit être contrôlé avant le tag :
**8 types de documents × 8 langues (64 PDF) + les 2 mindmaps** (Sophismes + Vertus).

Langues : 🇫🇷 FR (référence) · 🇬🇧 EN · 🇷🇺 RU · 🇵🇹 PT · 🇪🇸 ES · 🇸🇦 AR · 🇮🇷 FA · 🇨🇳 ZH.

---

## 1. Où récupérer les livrables

### 📄 Les 64 PDF — pre-release GitHub

> 👉 **[Pre-release « v0.9.0 — Visual Review »](https://github.com/ArgumentumGames/Argumentum/releases/tag/v0.9.0-review)** → section **Assets** en bas de page.
>
> 💬 **Issue de suivi (à commenter)** : **[#802](https://github.com/ArgumentumGames/Argumentum/issues/802)**

Chaque fichier est nommé `Argumentum_<Type>_<langue>.pdf`. **8 types × 8 langues** :

| # | Type de PDF | Ce que c'est |
|---|-------------|--------------|
| 1 | `TarotCards` | **Paquet principal** : cartes Sophismes (Tarot) + cartes Règles + Mémo |
| 2 | `TarotCards_Virtues` | **Paquet Vertus** (Tarot) — les vertus argumentatives |
| 3 | `PokerCards` | **Scénarios** de jeu (format Poker) |
| 4 | `TarotCards_Print&Play_A4` | Version **imprimable maison** du Tarot (A4, recto-verso) |
| 5 | `PokerCards_Print&Play_A4` | Version **imprimable maison** des Scénarios (A4) |
| 6 | `Fallacies_Web_A0` | **Poster A0** de la taxonomie des sophismes (grand format) |
| 7 | `Fallacies_Web_A4` | Poster **A4** de la taxonomie |
| 8 | `Fallacies_Web_Thumbnails_A4` | **Planche de vignettes** A4 |

### 🧠 Les 2 mindmaps — dans le dépôt (rendu direct dans le navigateur)

Ouvrez le dossier de votre langue puis cliquez les fichiers `.svg` (GitHub les affiche directement) :

> 👉 `https://github.com/ArgumentumGames/Argumentum/tree/master/Cards/Fallacies/Mindmaps/<langue>`
> (remplacez `<langue>` par `fr`, `en`, `ru`, `pt`, `es`, `ar`, `fa`, `zh`)

- **`Fallacies_<langue>.svg`** → la carte mentale des **sophismes**
- **`Argumentum_Virtues_MindMap_<langue>.content.svg`** → la carte mentale des **vertus**

> ⚠️ **Lecteur PDF** : ouvrez les PDF avec un viewer qui embarque les polices CJK + arabe
> (**Acrobat Reader** ou **SumatraPDF**). Un viewer sans ces polices affiche de faux « carrés vides □ »
> (*tofu*) qui **ne sont pas** un défaut du fichier (voir §5).

---

## 2. Comment se répartir le travail

Le paquet est gros (64 PDF). Deux façons de se répartir — au choix :

- **Par langue** : chacun prend les langues qu'il peut juger et regarde **tous les types** pour ces langues.
- **Par type** : l'un prend les cartes (Tarot/Vertus/Poker/Print&Play), l'autre les posters + mindmaps, sur les 8 langues.

Pas besoin d'être exhaustif à la carte près : pour chaque type, **2-3 pages suffisent** à juger si le rendu global
tient (même pipeline, mêmes polices). Signalez ce qui saute aux yeux.

---

## 3. Quoi vérifier, par type de livrable

| Type | À vérifier en priorité | Cas délicat |
|------|------------------------|-------------|
| **TarotCards** | cover lisible · cartes **Règles** en premier · recto-verso aligné · densité de texte OK | AR/FA (RTL), ZH (CJK) |
| **TarotCards_Virtues** | les 8 familles de vertus ont leur **couleur de fond** (pas de carte blanche) · titres non tronqués | RU (titres longs), overflow |
| **PokerCards** (Scénarios) | texte du scénario complet, pas coupé · noms propres corrects | EN/PT (noms propres FR légitimes : Sherlock, Jeanne d'Arc…) |
| **Print&Play A4** (Tarot + Poker) | recto-verso **s'aligne au pliage** · marges de découpe présentes | tous |
| **Poster A0 / A4** | taxonomie lisible · pas de texte qui déborde des cases · couleurs de familles | **overflow** (cases denses) |
| **Thumbnails A4** | toutes les vignettes présentes · lisibles en petit | — |
| **Mindmap Sophismes** (SVG) | branches lisibles · libellés traduits (pas de fallback FR) · pas de chevauchement | RU/AR/FA/ZH |
| **Mindmap Vertus** (SVG) | idem · les 7-8 familles bien distinctes | — |

---

## 4. Les arrêts prioritaires (si vous manquez de temps)

Ces cartes couvrent les cas-limites. Si elles sont bonnes, le reste l'est très probablement.

| # | Fichier | Quoi vérifier |
|---|---------|---------------|
| 1 | `TarotCards_fr` p1-2 | référence FR : cover + recto-verso |
| 2 | `TarotCards_Virtues_fr` | les 8 familles ont bien leur couleur |
| 3 | `TarotCards_ar` / `_fa` (carte dense) | **arabe / persan** de droite à gauche, aligné à droite |
| 4 | `TarotCards_zh` (carte dense) | **chinois** rendu, pas de débordement hors cadre |
| 5 | `TarotCards_ru` (carte dense) | **cyrillique** + titres longs non tronqués |
| 6 | `Fallacies_Web_A0_fr` | **poster** : aucune case ne déborde |
| 7 | mindmap `Fallacies_ru.svg` + `_zh.svg` | libellés traduits, pas de chevauchement |

---

## 5. Non-latin : « normal » vs « cassé »

Le principal risque d'une relecture multilingue, c'est le **faux positif** (signaler comme cassé ce qui est
correct dans une écriture qu'on ne lit pas). Repères :

### 🇸🇦 Arabe / 🇮🇷 Persan (de droite à gauche)
- **Normal** : le texte court **de droite à gauche**, aligné à droite. Les **chiffres restent de gauche à
  droite** (standard Unicode, pas un bug). FA ajoute 4 lettres (پ چ ژ گ) — vérifier qu'elles s'affichent.
- **Cassé** (à signaler) : texte de gauche à droite sur une page RTL · carrés vides □ · lettres détachées
  qui devraient être liées.

### 🇨🇳 Chinois (CJK)
- **Normal** : caractères en blocs carrés, **plus larges** que le latin → un texte « tassé » n'est pas un bug.
- **Cassé** (à signaler) : carrés vides □ · demi-caractères · police latine au milieu du chinois · **débordement hors cadre**.

### 🇷🇺 Cyrillique
- **Normal** : alphabet А-Я а-я, accents, ё / й.
- **Cassé** (à signaler) : carrés vides · lettre latine à la place d'une cyrillique qui se ressemble (`a` au lieu de `а`).

### 🇪🇸 Espagnol / 🇵🇹 Portugais (latin étendu)
- **Normal** : accents (é è ê, ã õ, ç, ñ, à). PT = brésilien.
- **Cassé** (à signaler) : accents manquants ou mojibake · « ? » ou carré noir à la place d'un accent.

---

## 6. Déjà connu — **ne pas signaler** comme défaut

| Ce que vous pourriez voir | Pourquoi | Action |
|---------------------------|----------|--------|
| Carrés vides □ sur du chinois / de l'arabe | police **de votre lecteur**, pas du fichier | ouvrez dans Acrobat/SumatraPDF |
| Carte de règles EN : titre « Roll of the English Channel » | mistraduction résiduelle **déjà corrigée** en amont | connu — inutile de le remonter |
| Mnémoniques latins des cartes Vertus (mots type « bArbArA ») | choix éditorial assumé (latin conservé) | normal |
| Poster A0 très lourd à ouvrir | ~18 Mo/langue, haute résolution d'impression | normal |
| Mindmap : les liens Wikipédia pointent vers le FR | résidu connu ([#804](https://github.com/ArgumentumGames/Argumentum/issues/804)), invisible à l'impression | connu, post-tag |

---

## 7. Comment nous remonter vos retours

- **Idéal** : commentez directement sur l'**[issue #802](https://github.com/ArgumentumGames/Argumentum/issues/802)**.
  Un tableau à cocher (langue × type de document) est fourni : voir [`CHECKLIST.md`](CHECKLIST.md).
- **Sinon** : envoyez vos remarques à jsboige (WhatsApp), on les reporte pour vous.

**Format utile** : *langue + type de document + page + ce qui cloche* (une phrase suffit).
Exemple : « ZH — TarotCards p51 — le titre déborde en bas du cadre » ou « A0 EN — case "Ad Hominem" déborde ».

---

## 8. Ce qui est déjà validé en interne (pour info)

- ✅ **Colorimétrie CMYK** : les PDF sont en CMYK + profil d'impression SWOP.
- ✅ **Structure** : cartes de règles en premier · recto-verso aligné · 300 PPI.
- ✅ **Pas de fuite du français** dans les autres langues (structure multilingue intacte).
- ✅ **Mindmaps** : contenu localisé par langue (rendus distincts, pas de clone du FR).

Votre relecture = le **dernier regard humain** sur l'ensemble du paquet avant le tag. Merci encore 🎴✨
