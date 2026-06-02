# Guide de validation visuelle — versions localisées (8 langues)

> **À qui s'adresse ce document.** À toi (relecteur final) pour valider les PDF générés **avant release**, y compris dans les langues que tu ne lis pas. L'idée : pour chaque langue, te donner des **signaux vérifiables à l'œil** qui ne demandent pas de comprendre le texte.
>
> **Statut.** Procédure de validation — stable indépendamment du contenu. À utiliser après une régénération complète. Sert l'issue **#140** (QA multilingue) et le gate release **#134**.
>
> **Date.** 2026‑05‑31 ; table de couverture §4 rafraîchie 2026‑06‑02 ; §1 corrigé 2026‑06‑02 (chemin → build `Release`, post‑#424 8 langues) (`master`). Auteur : ai‑01.

---

## 1. Où regarder — répertoire explicite

Les PDF sont générés ici (un sous‑dossier par langue) :

```
Generation/Converters/Argumentum.AssetConverter/bin/Release/net9.0-windows/Target/<lang>/Documents/density-0/
```

`<lang>` ∈ `fr en ru pt es ar fa zh` pour les **cartes** (8 langues), et `fr en ru pt` pour les **mind maps**.

Chaque dossier langue contient **8 PDF** (mêmes noms, suffixe de langue) :

| Fichier (suffixe `_<lang>`) | Contenu | Le plus à risque en localisation |
|---|---|---|
| `Argumentum_TarotCards_<lang>.pdf` | Rules + Memo + Fallacies | titres longs, débordement |
| `Argumentum_TarotCards_Virtues_<lang>.pdf` | Virtues | **overflow #190**, cellules vides #403 |
| `Argumentum_PokerCards_<lang>.pdf` | Scenarii | paragraphes longs (contexte/enjeu) |
| `Argumentum_TarotCards_Print&Play_A4_<lang>.pdf` | Tarot maison | idem Tarot |
| `Argumentum_PokerCards_Print&Play_A4_<lang>.pdf` | Scenarii maison | idem Poker |
| `Argumentum_Fallacies_Web_A4_<lang>.pdf` | FallaciesWeb | densité texte carrée 66×66 |
| `Argumentum_Fallacies_Web_A0_<lang>.pdf` | Poster A0 | entête logo+QR, 12 colonnes |
| `Argumentum_Fallacies_Web_Thumbnails_A4_<lang>.pdf` | Vignettes | lisibilité 50×50 |

> **Release vs Debug — quelle build valider.** La validation release se fait sur **`Release/…`** (CMYK/PNG sans perte, `-c Release`) : c'est la seule build qui contient les **8 langues** et reflète la dernière décision éditoriale (#424 — racines de famille blankées). Au 2026‑06‑02 c'est la build de référence (post‑#424, 8 langues, datée du jour). La build `Debug/…` (RGB/JPEG) convient pour un contrôle rapide *contenu/layout* **seulement si elle est à jour** — ⚠️ sur cette machine elle est partielle (4 langues `fr en ru pt`, antérieure à #424) : **ne pas valider la release dessus**. **Avant validation : confirmer les 8 sous‑dossiers `fr en ru pt es ar fa zh` + la date des PDF (= dernière régén).**

---

## 2. Drapeaux rouges universels (toutes langues, sans rien lire)

Avant même de regarder une langue en particulier, ces défauts sautent aux yeux :

| Symptôme visuel | Cause probable | Verdict |
|---|---|---|
| **Carte au fond blanc** (devrait avoir une couleur de famille) | classe CSS de famille manquante | ❌ bloquant |
| **Carte quasi vide** — seules les icônes/cadres statiques apparaissent | contenu CSV non injecté / cellule vide | ❌ bloquant |
| **Texte qui sort du cadre** de la carte (déborde en bas/à droite) | overflow (cf. #190 Virtues, #316 titres RU) | ❌ bloquant |
| **Carrés vides `□□□`** ou losanges `◌` à la place des lettres | la police ne couvre pas ce script | ❌ bloquant |
| **« Mojibake »** : `Ã©`, `Ð¿`, `â€™` au lieu d'accents | problème d'encodage (BOM / charset) | ❌ bloquant |
| **Mot coupé brutalement** sans césure, ou collé au bord | gestion d'overflow/--word-break | ⚠ à signaler |
| **Texte d'une AUTRE langue** sur la carte (ex. anglais sur une carte russe) | fuite de langue (cf. bug #216) | ❌ bloquant |

> **Méthode rapide.** Ouvre le PDF, fais défiler 100 % des pages en survol (≈ 1 carte/seconde). Les drapeaux ci‑dessus se repèrent sans lire. Pour un échantillon ciblé, regarde **les premières + dernières cartes** de chaque famille (les changements de couleur de famille révèlent les classes CSS manquantes).

---

## 3. Par langue — ce que tu peux vérifier sans la lire

> Légende : 🟢 tu peux valider seul · 🟡 vérif partielle (forme, pas le sens) · 🔴 demande un relecteur natif pour le *fond*.

### `fr` — Français (canonique) 🟢
Ta langue de référence. Validation de fond complète. **Sert de baseline** : mémorise la position/taille du texte sur 2‑3 cartes types — les autres langues doivent occuper grosso modo la même zone.

### `en` — Anglais (latin) 🟢
Tu lis → validation de fond. Points spécifiques :
- **Aucun résidu français** (bug #216 historique : du FR fuyait dans les PDF EN). Si tu vois un mot français → ❌.
- Layout = même contrainte que `fr` (latin). Débordements rares.

### `pt` — Portugais (latin) 🟡
- Accents portugais bien rendus : **ã õ ç áê** (pas de tofu, pas de mojibake).
- Pas de résidu FR/EN.
- Longueur proche du FR → layout stable.

### `es` — Espagnol (latin) 🟡
- Caractères : **ñ ¿ ¡ á í** rendus correctement.
- Pas de résidu FR/EN.

### `ru` — Russe (cyrillique) 🟡
Tu ne lis pas, mais tu vérifies la **forme** :
- **C'est bien du cyrillique** (`Привет`-style), **pas du latin résiduel**.
- **⚠ TITRES : aucun débordement.** C'était LE bug **#316** (titres RU 30‑40 % plus longs qu'en FR). Le correctif **auto‑shrink** (#400, déployé) doit réduire automatiquement la police des titres trop longs. **À confirmer sur PDF réel** : les titres tiennent dans le bandeau, sans déborder ni chevaucher.
- Glyphes cyrilliques nets, pas de `□`.

### `pt`/`es`/`ru` — résumé latin+cyrillique
Scripts à sens de lecture **gauche→droite** comme le FR. Le layout ne devrait pas « basculer ». Si une carte est alignée à droite ou semble en miroir → anomalie.

### `ar` — Arabe (RTL, cursif) 🔴 forme / fond natif
Script **droite‑à‑gauche** et **cursif** (les lettres se lient). Vérifie la **forme** :
- **Sens RTL** : le texte commence **à droite**, l'alignement de paragraphe est **à droite**. Si le texte démarre à gauche comme du français → ❌ shaping RTL cassé.
- **Lettres connectées** : l'arabe s'écrit lié. Si les lettres sont **détachées/isolées** (espaces entre chaque), la police ou le moteur de rendu ne fait pas le *shaping* → ❌.
- Pas de `□`/tofu ; pas de blocs vides (#403 = colonnes ar non remplies → carte vide).
- Le texte **ne sort pas du cadre** (un layout pensé LTR peut casser en RTL).
- ⚠ **Fond (sens des phrases) : relecteur arabophone requis** — hors de ta portée et de la mienne.

### `fa` — Persan/Farsi (RTL, cursif) 🔴 forme / fond natif
Comme l'arabe (RTL + cursif), **plus** des lettres propres au persan : **پ چ ژ گ** et le `ی`/`ک` persans. Vérifie :
- Mêmes contrôles RTL que l'arabe.
- Les 4 lettres persanes ci‑dessus s'affichent (pas de tofu) — c'est le test « la police couvre‑t‑elle le persan, pas seulement l'arabe ».
- ⚠ Fond : relecteur persanophone requis.

### `zh` — Chinois (CJK) 🔴 forme / fond natif
Sinogrammes, sens gauche→droite. Vérifie la **forme** :
- **Sinogrammes rendus** (pas `□`). Test rapide : aucune carte ne doit montrer une rangée de carrés.
- **⚠ Pas de cellule vide** : le risque principal `zh` est le **#403** (colonnes non remplies → carte avec seulement les icônes). Compte grossièrement : autant de cartes « pleines » qu'en FR.
- **Pas de césure au milieu d'un caractère** ni caractère coupé en deux par le bord.
- Ponctuation pleine largeur (`。` `，` `、`) — normale en chinois, pas un bug.
- ⚠ Fond : relecteur sinophone requis.

---

## 4. Tableau de couverture attendu (avant release)

À régénérer/recopier depuis l'audit `argu_i18n_audit2.py` (logique fr‑relative : *gap = cellule FR non‑vide ET cible vide*). Cible **release = 0 gap** sur les 8 langues (hors champs exclus : `Simple_name_en`, `political_example_en`, `link_*` suivis par #333).

| Dataset | fr | en | ru | pt | es | ar | fa | zh |
|---|---|---|---|---|---|---|---|---|
| Fallacies — contenu (text/desc/example) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Fallacies — taxonomie | ✅ | ✅ | ✅ #408 | ✅ #408 | ✅ | ✅ | ✅ | ✅ |
| Virtues | ✅ | ✅ #407 | ✅ #407 | ✅ #407 | ✅ | ✅ #407 | ✅ #407 | ✅ #407 |
| Scenarii | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules (deck) | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| Rules Print&Play | ✅ | ✅ | ✅ | ✅ | ✅ #405 | ✅ #405 | ✅ #405 | ✅ #405 |

**Couverture i18n = 0 gap fr‑relatif sur les 8 langues** (audit fr‑relatif rejoué le 2026‑06‑02 sur `master`, post‑#405/#407/#408). Provenance : #405 (Rules P&P es/ar/fa/zh), #407 (Virtues blank‑fill), #408 (taxonomie ru/pt). ⚠️ Résidu réel rattrapé : PK 457 (« Déstabilisation ») avait gardé ses 3 labels de hiérarchie RU vides — oubliés par #408 — comblés par propagation de label sœur. La case « taxonomie ru » n'est donc **réellement** ✅ qu'après ce rattrapage. Volet **couverture** de #403 atteint (mandat *100 %*) ; la **qualité** de traduction reste suivie par #192/#299 et les résidus cosmétiques par #411. Mind maps : `fr/en/ru/pt` uniquement (pas de version es/ar/fa/zh — c'est voulu, cf. catalogue).

---

## 5. Protocole de validation conseillé

> **Playbook exécutable** : le fichier [`qa-scenario-8langues-release.md`](qa-scenario-8langues-release.md) contient le scénario pas‑à‑pas (~25‑30 min) avec PK ciblés, grille de résultat et actions correctives. Ce qui suit est le protocole général.

1. **Vérifier la couverture CSV d'abord** (avant de regarder un PDF) : relancer l'audit fr‑relatif → 0 gap. Inutile de valider visuellement des cartes dont on sait qu'elles sont vides.
2. **Régénération complète** des 8 langues en `-c Release` (CMYK/PNG) depuis un `master` à jour.
3. **Échantillonnage par langue** : pour chaque langue, ouvrir au minimum **Tarot + Virtues + Poker** (les 3 decks principaux). Survoler 100 % des pages pour les drapeaux universels (§2), puis appliquer les contrôles spécifiques (§3).
4. **Focus AR/FA/ZH** : ai‑01 fournira un échantillon ciblé (~3‑5 cartes/langue) rendu et capturé via Playwright sur le CardPen déployé — pour pré‑filtrer RTL/CJK/glyphes/overflow avant ta validation.
5. **Confirmer #316 (RU)** sur PDF réel : titres russes longs qui tiennent grâce à l'auto‑shrink.
6. **Fond AR/FA/ZH** : marquer comme « forme validée, fond en attente relecteur natif » — ne pas bloquer la release *forme* sur l'absence de relecteur natif, mais le documenter.

---

## 6. Issues liées

- **#140** — QA multilingue (ce guide en est l'instrument).
- **#134** — GitHub Release v0.9.0 (gate : ce guide + couverture 0 gap + tests).
- **#403** — Remplissage i18n à 100 % (Virtues ar/fa/zh, Fallacies taxo ru/pt, RulesP&P es/ar/fa/zh).
- **#316** — Overflow titres RU (auto‑shrink #400) — à confirmer sur PDF réel.
- **#190** — Overflow + copy Virtues.
- **#192 / #299** — Qualité de traduction (repasse de finition, modèle).
- **#216** (clos) — fuite de langue FR dans PDF EN/RU/PT — vérifier la non‑régression.

> **Note.** Une version anglaise (`validation-guide.en.md`) pourra être ajoutée si l'on veut publier ce guide dans le set de docs publiques ; pour l'usage interne de validation, le FR suffit.
