# Guide de validation visuelle — versions localisées (8 langues)

> **À qui s'adresse ce document.** À toi (relecteur final) pour valider les PDF générés **avant release**, y compris dans les langues que tu ne lis pas. L'idée : pour chaque langue, te donner des **signaux vérifiables à l'œil** qui ne demandent pas de comprendre le texte.
>
> **Statut.** Procédure de validation — stable indépendamment du contenu. À utiliser après une régénération complète. Sert l'issue **#140** (QA multilingue) et le gate release **#134**.
>
> **Date.** 2026‑05‑31 ; table de couverture §4 rafraîchie 2026‑06‑02 ; §1 corrigé 2026‑06‑02 (chemin → build `Release`, post‑#424 8 langues) ; §2bis ajouté 2026‑06‑03 (pré‑validation mécanique harnais #412) ; **§2ter ajouté 2026‑06‑04 (statut #250 Rules + #435 Mémo résolus sur branches #438/#439, validés visuellement)** (`master`). Auteur : ai‑01.

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

> **Release vs Debug — quelle build valider.** La validation release se fait sur **`Release/…`** (CMYK/PNG sans perte, `-c Release`) : c'est la seule build qui contient les **8 langues** et reflète la dernière décision éditoriale (#424 — racines de famille blankées). Au 2026‑06‑02 c'est la build de référence (post‑#424, 8 langues, datée du jour). La build `Debug/…` (RGB/JPEG) convient pour un contrôle rapide *contenu/layout* **seulement si elle est à jour** — ⚠️ sur cette machine elle est partielle (4 langues `fr en ru pt`, antérieure à #424) : **ne pas valider la release dessus**. **Avant validation : confirmer les 8 sous‑dossiers `fr en ru pt es ar fa zh` + la date des PDF (= dernière régén).** ⚠️ **MAJ 2026‑06‑08 : la build `Release/…` locale (datée 02‑06) est ANTÉRIEURE à #443/#446 → Mémo Back stale FR** (cf. §2ter « Caveat build Release locale »). Un `-c Release` final post‑`dc01445f` avec clobber des harvests Mémo est requis avant tout sign‑off/tag.

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

## 2bis. Pré‑validation mécanique (harnais #412) — ce que la machine a déjà vérifié

> **But.** Avant que tu regardes une seule carte, le harnais mécanique #412 (`VisualQaHarness`, mergé PR #428) passe **toutes** les images générées au crible pixel. Objectif : te garantir qu'**aucune carte n'est absente ou corrompue** sur les 8 langues, pour que ton œil se concentre sur les exceptions (§2/§3) et pas sur l'inventaire.

**Ce qui a été passé** (test `VisualQa_FullGrid_AllCards_AllDetectors`, arbre `Release`, exécuté 2026‑06‑03) :
- **3905 images** analysées, **8 langues × 4 CardSets** (Fallacies‑Web, Rules, Scenarii, Virtues).

### Inventaire = ✅ PROPRE (le signal qui compte)
Aucune image **manquante**, **vide (0 octet)** ou **`data:,`** sur l'ensemble. Comptes confirmant la couverture 8 langues complète :

| CardSet | Cartes × langues | Lignes grille |
|---|---|---:|
| Fallacies‑Web | 176 × 8 | 1408 ✅ |
| Rules (Tarot) | 24 × 8 | 192 ✅ |
| Virtues | 114 × 8 | 912 ✅ |
| Scenarii | ~167 × 8 | 1365 ✅ |

→ **La masse des cartes est mécaniquement présente et non‑vide.** C'est la preuve d'inventaire sur laquelle s'appuie l'auto‑PASS de masse : tu n'as pas à vérifier carte par carte que le contenu est généré, seulement à traquer les défauts de §2/§3 sur les catégories à risque.

### ⚠ Caveat calibration — le binaire PASS/FLAG n'est PAS une liste de défauts
Le harnais sort un binaire `PASS/FLAG` par carte **et** des **valeurs brutes**. Même **après** la recalibration par‑CardSet (#431, mergée `e9600c0a` — seuils Rules `blank‑ratio` 92 % / `bottom‑sat` 12 %, autres par défaut 65 %/25 %), le binaire reste **sur‑déclenché : 3219/3905**. La raison de fond, mise à nu par l'extraction par‑CardSet × détecteur (run recalibré 2026‑06‑03) : **chaque détecteur flagge par *signature de construction*, pas par défaut.**

| CardSet | imgs | WhiteBand | BlankRatio | BottomSat | Détecteur discriminant |
|---|---:|---:|---:|---:|---|
| Fallacies‑Web (vignettes A0, fond image plein) | 1408 | **0** | 1238 | 1408 | aucun — tout est construction |
| Virtues (fond couleur famille) | 912 | 37 | **0** | 198 | BottomSat (longueur de texte) |
| Rules (texte sur blanc) | 192 | 184 | 74 | 13 | BottomSat |
| Scenarii (poker, fond blanc) | 1393 | 1336 | 1336 | 57 | BottomSat |

Lecture : sur les cartes **fond blanc** (Rules, Scenarii) `WhiteBand` + `blank‑ratio` flaggent ~96 % par construction (le blanc domine) → bruit. Sur les cartes **fond couleur** (Fallacies‑Web, Virtues) `blank‑ratio` est propre (Virtues **0**) mais `bottom‑sat` flagge l'image/le texte en bas. **Donc « 3219 flaggés » ≠ « 3219 défauts ».** Le seul détecteur qui isole de vraies exceptions est **`bottom‑sat`, lu par‑CardSet.**

### 🟢 Headline #188 — pas de régression bande‑blanche sur les cartes concernées
`WhiteBand` n'est interprétable que sur fond couleur (sur fond blanc il flagge tout). Là où il compte : **Fallacies‑Web = 0**, **Virtues = 37 singletons éparpillés** (1 carte/langue, aucun cluster 8‑langues). Une vraie régression #188 frapperait une carte sur ses 8 langues d'un coup → **ce pattern est absent**. ✅ Pas de régression bande‑blanche sur les cartes que #188 affectait.

### Liste d'exceptions réelles (signal `bottom‑sat`, actionnable)
- **Rules (13)** : `rules_01` cover 100 %×8 = titre plein‑cadre **par design** (non‑défaut) ; `rules_23` (Parlote) 12‑18 % FR/EN/ES/PT = saturation réelle → **c'est exactement #250 FIX 2** (proposition FR postée, en attente sign‑off) ; `rules_04` pt 13 % = singleton limite.
- **Scenarii (57)** : réparti **uniformément sur les 8 langues** (7‑8/langue), même jeu de ~7 cartes denses qui saturent à ~43 % dans **toutes** les langues (`histoire`/`mythologie`/`mitologia`/`cultura_pop`/`intimate_relations`…) → défaut **intrinsèque à la carte, indépendant de la langue** = contenu dense, **pas** un artefact de traduction. Candidat trim éditorial OU carte‑dense acceptée (décision à l'œil).
- **Virtues (198)** : `card_001` cover 100 %×8 = design ; le reste **se concentre en zh/ru/es/pt** (textes longs : zh 52, ru 47, es 43, pt 34 vs fr 6, en 14) à 12‑39 % → débordement par **longueur de traduction**, famille **#316/#353** (auto‑shrink JS RU déjà livré PR #400 ; zh/es/pt analogues à surveiller).

### Ce que le harnais apporte réellement à ce dossier
1. **Preuve d'inventaire** (présence / dimensions / non‑vide) → la masse est saine, auto‑PASS justifié.
2. **Filtre d'exceptions par‑CardSet** (post‑#431) → les 3 vrais foyers ci‑dessus, chiffrés, au lieu d'un binaire non‑discriminant.

> **Le harnais ne remplace pas le jugement visuel** (lane ai‑01, non déléguée). Il garantit l'inventaire et isole les foyers ; les verdicts « belle/moche/à‑risque » restent à l'œil (§2/§3). Section **Rules du dossier = « pending re‑check »** jusqu'au fix #250 (→ **levé**, voir §2ter) ; **Scenarii cartes denses** = nouveau point d'attention œil ; **Virtues zh/ru/es/pt** = vérifier le débordement bas sur 2‑3 cartes longues.

---

## 2ter. Statut #250 (Rules) + #435/#443 (Mémo) — MERGÉS sur master, validés sur full‑regen (maj 2026‑06‑08)

> **Pourquoi cette section.** Les 7 problèmes signalés sur **#250** (layout Rules) et **#435** (Mémo) — restés ~1 mois sans diagnostic précis ni validation multilingue — ont été **traités, mergés et validés à l'œil**. **MAJ 2026‑06‑08 : tout est sur `master` `dc01445f`** — #438 (`3a391996`, Rules 24→15), #439 (`ce63bcd3`, Mémo Face sélecteur), #443 (`81af2279`, Mémo Back StaticConversions), #446 (`2169d7b4`, Mémo Back taxonomie). La régén complète 8 langues confirme l'état mergé (voir bloc « Validation 2026‑06‑08 » plus bas). Le §2bis et les §2/§3/§4 décrivent un état antérieur (pré‑fix) ; cette section dit l'état courant.

### #250 Rules — refonte éditoriale 24 → 15 cartes ✅ (PR #438, supersede #437)
La restructuration `Cards/Rules/Argumentum Rules - Cards.csv` **24 → 15 cartes** + remap CSS 15‑cartes traite les **6 problèmes de layout** : décompte coupé, covers vides, couleurs (CSS 1‑18→remap 15), orphelins DBP/Moulin fusionnés, Parlote §3 déplacé. **Validé CardPen/Playwright** (15 cartes Rules_01–15, cover + carte 9 H1 dense DBP + carte 15). Miroir 8 langues po‑2023 (`0cee7d64`, déplacements de paragraphes purs, **invariant byte‑identique 7/7** EN/RU/PT/AR/ES/ZH/FA). Dossier : `docs/investigations/2026-06-04-rules-250-validation/`.
> **⚠️ Correction overflow (commit `74ef0971`).** La restructuration **réduit** la densité mais **n'élimine pas** les collisions corps/pied. La 1ʳᵉ mesure annoncée *« 0 px overflow FR (`scrollHeight == clientHeight`) »* **était fausse** : elle mesurait l'auto‑fit du conteneur (trivialement vrai sans hauteur bornée), pas le débordement du corps dans la bande de pied absolue. Mesure correcte (moteur `#190` live + domtoimage, CSV canonique, 8 langues) — **AVANT** : FR #9 +20px, #15 +22px · RU #9 +12px, #12 +23px · **PT #4 +31px, #9 +28px, #10 +39px, #12 +95px** · EN aucune. **APRÈS `74ef0971`** (corps en `.texte > .desc_fr` pour activer `#190` + `card:not([class~="1"]) .texte { max-height: 390px; overflow: hidden; }`, cover exclue) : **0 collision non‑cover sur les 8 langues** (eyeballé). C'est le check que le harnais mécanique #412/#431 manquait — il ne pilotait jamais le moteur `#190` live.
> **Conséquence sur §2bis.** L'inventaire harnais Rules (24 cartes, `rules_23` Parlote saturation, etc.) est **caduc dès le merge #438** : il n'y a plus 24 mais **15** cartes ; `rules_23` n'existe plus (Parlote §3 réabsorbé). La ligne « Rules = pending re‑check jusqu'au fix #250 » est **levée**.

### #435 Mémo — i18n du sélecteur de famille ✅ (PR #439)
Le Mémo Face ne rendait que FR 7/7, EN 2/7 (cognats), PT/RU 0/7. **Cause = sélecteur `{{#ifCond Famille "==" text_fr}}` non localisé symétriquement** (bug de config, pas de traduction manquante). **Fix = 1 espace `text_fr }}`** → sélecteur FR‑vs‑FR language‑invariant. **Régén Debug : 7/7 familles dans les 4 langues** (EN 2→7, PT 0→7, RU 0→7), texte localisé. Réserve mineure RU (7ᵉ famille Обструкция en pied de carte, cyrillique verbeux — non bloquant, surveiller à l'impression). Dossier + 5 PNG AFTER + table de correspondance des 7 en‑têtes par langue (pour vérifier RU/PT sans les lire) : `docs/investigations/2026-06-04-memo-435-i18n-gap/`.
> **Note §2bis.** Le Mémo était **absent** du harnais #412 (qui couvre Fallacies‑Web / Rules / Scenarii / Virtues). Après merge #439 + régén, le Mémo rejoint la grille de validation.

### ✅ Mémo **Back** — localisé (décision jsboige 2026‑06‑05, PR #443)
Décision : **localiser** EN/RU/PT (pas FR‑assumé). Le sous‑titre figé « L'art de jamais avoir tort » est traduit via `StaticConversions` (`LocalizationConfig`), les labels `{{Famille}}`/`{{Sous‑Famille}}`/`{{Soussousfamille}}` se localisent via les `FrontFieldConversions` existantes. Le sélecteur `ifCond` reste FR‑invariant (garantit le groupement 8/8 familles). PR #443.

### Séquence pour passer au vert — état 2026‑06‑08
1. ✅ **Merge #438 → #439 → #443 → #446** : faits, tous sur `master` `dc01445f`.
2. ⏳ **Régén `-c Release` sur un `Target` propre** — **PAS encore faite sur master post‑#446**. ⚠️ Voir « Caveat build Release locale » ci‑dessous : le `-c Release` final doit **clobber explicitement les harvests Mémo** (`rm Target/*/Harvest/*Memo*`) sinon il ré‑génère la Mémo Back stale FR.
3. ✅ **ai‑01 re‑check visuel 8 langues** : fait sur la **régén complète Debug** (po‑2023, `validation/regen-dc01445f` `e87d33e2`) — Rules/Mémo basculés ✅ (détail bloc suivant).

### ✅ Validation 2026‑06‑08 — full‑regen Debug 6/6 CardSets PASS
Régén complète 8 langues (po‑2023, master `dc01445f`, build Debug, **harvests Mémo clobbés**) → spot‑check visuel ai‑01 (vision + crops natifs) sur échantillon `validation/regen-dc01445f` (`e87d33e2`).
- **6/6 CardSets PASS** : Fallacy Tarot, Mémo Face, **Mémo Back**, Rules, Scenarii Poker, Virtues — sur les 4 langues validées (FR/EN/RU/PT), sans régression vs §2/§3.
- **Mémo Back localisé 8/8** : EN PREJUDICE/OVERINTERPRETATION · RU ПРЕДРАССУДОК/Домыслы · PT PRECONCEITO/SUPERINTERPRETAÇÃO · ES PREJUICIO/SOBREINTERPRETACIÓN · ZH 偏见/过度解读 · AR تحامل (RTL shapé) · FA persan shapé · FR référence. → **#446 confirmé sur livrable mergé**, et **aucune lacune CSV** (AR/ES/FA/ZH localisent = taxonomie traduite 100 %).
- AR/ES/FA/ZH : rendu + localisation OK ; reste pour ship 8‑langues = **QA contenu ai‑01** (jamais faite) + résidus #411 — seuil qualité, pas couverture.

### ⚠️ Caveat build Release locale (vérifié 2026‑06‑08)
La build **`Release/…` locale de cette machine est datée du 2026‑06‑02** (harvests Mémo `Memo_harvest_*.json` = 02‑06 18:40 ; TarotCards PDF = 02‑06 19:49) — **antérieure à #443/#446** (mergés vers `dc01445f` le 07‑06). **Vérifié au crop natif** : `Release/…/en/Images/density-0/Memo/memo_back.png` montre sous‑titre **« L'ART DE JAMAIS AVOIR TORT »** + taxonomie **FR** (PRÉJUGÉ · SURINTERPRÉTATION · INSUFFISANCE…) → **stale pré‑#443**. ⇒ **La build Release locale n'est PAS un artefact release valide pour la Mémo** ; ne pas l'utiliser pour valider/tagger. Le `-c Release` final doit être **re‑exécuté sur `dc01445f` avec clobber des harvests Mémo**. Seul artefact Mémo Back frais validé = la régén Debug clobbée ci‑dessus. (Détail méthode stale‑harvest : la régén `dotnet clean` ne vide PAS les `.harvest.json` runtime sous `Target/*/Harvest/` ; clobber explicite obligatoire.)

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
