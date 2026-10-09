# R-site — mesure du magasin des Règles et plan d'écriture (#1502, 09/10/2026)

**Grain** : « Site ← cartes, Règles FR » posé par l'analyse #1502 (c.6056760508) — les pages Règles de la
**préprod** reprennent le texte des cartes (139 formulations en retard + 4 nouveautés + mémo selon la
décision owner du 24/09), **sauf les phrases 3 et 4** (attente owner). Ce document couvre la **phase
mesure** : lecture seule stricte (SELECT + GET), **aucune écriture** préprod.

## 1. Instrument rejoué sur la préprod — structure saine, compteurs déjà alignés

`docs/corpus/rules-site-vs-cards.py` (master `240b69fd`) avec
`--base https://dnn.argumentum.myia.io/R%C3%A8gles` (drapeau ajouté par cette PR — la DoD du grain
exige un rejeu préprod, impossible sans lui) :

- **rc=0**, 43 paires appariées (garde ≥30 satisfaite).
- **Compteurs joueurs : 5/5 identiques** — la préprod confirme l'alignement du 06/10 (c.6034943515) :
  école 3-10 · bingo 1-20 · moulin 2-8 · beau 1-8 · parlote 4-4.
- Verdicts des 43 paires : **3 IDENT · 9 proches · 27 réécrits · 4 ancres** (titre de niveau sans corps),
  plus **8 sections web-only** sans contrepartie carte (« Contenu » ×5, « Carte Mémo »,
  « Conditions de Victoire », « Nombre de pioches » sur l'École). Les 139 éléments en retard vivent
  dans les 27 réécrits + 9 proches ; relevé complet : sortie de l'instrument, § Reproductibilité.
  ⚠️ **Lecture pré-écriture à l'instrument d'avant-fix** : le compte « 27 réécrits » est contaminé par
  le défaut `autojunk` (erratum ci-dessous) — l'**ampleur** ne survit pas telle quelle ; les
  compteurs, l'appariement et les sections web-only survivent.

### Erratum (09/10 soir) — le « 27 réécrits » était en partie un artefact d'instrument

La ligne ci-dessus a été mesurée **avant l'écriture** et avec `SequenceMatcher(autojunk=True)` —
défaut corrigé depuis par #1835 (mergée `a9be2d11`) : l'autojunk marque comme « junk » tout élément
de `b` comptant plus de `len(b)//100 + 1` occurrences dès que `len(b) ≥ 200` — soit, sur nos chaînes,
toutes les lettres fréquentes du français — et les seeds détruites font rater les blocs identiques
**quand la divergence est en tête** (mesuré : École `Variants` **0.109** au lieu de **0.949**). Une
partie des 27 « réécrits » était donc une panne d'outil, pas un écart réel ; la matrice pré-écriture
corrigée n'est pas re-dérivable du site servi (l'écriture #1835 a changé l'état mesuré).

**Ce qui survit / ce qui ne survit pas** :

- survivent (comparaisons discrètes, insensibles à `sim()`) : les **compteurs 5/5**, les **43 paires**
  appariées, les **4 ancres**, les **8 sections web-only**, le plan d'écriture et ses exceptions ;
- ne survit pas : l'**ampleur** « 27 réécrits + 9 proches » comme mesure des 139 éléments en retard
  (l'analyse #1502, qui a posé le chiffre 139, procédait élément par élément, pas par similarité).

**Rejeu à l'instrument corrigé (cette PR : `autojunk=False` + `--base`), état post-écriture #1835** :
rc=0, 43 paires, compteurs 5/5 identiques — **34 IDENT · 5 proche · 0 réécrit · 4 ancres**. Les 5
« proche » résiduels : `Matériel` ×3 (0.983-0.984), `Variantes` École (0.949 — les 2 items owner,
attendus), `Fin de partie et décompte` (0.984) — écart de ponctuation/liste, mesuré 1.000 sans les
tirets `<li>`. Cohérent avec le DoD de #1835.

## 2. Le magasin — mesuré, pas déduit

Les 6 pages sont servies par **un seul module** (TabModule 310, module 602, page `Règles` TabID 152,
`ContentPane`) : les URL `details/<slug>/mid/602` sont des **vues détaillées 2sxc** du même module.
Son app : `TsDynDataApp = bbc87873-645d-4c08-8a6b-418086567554` = **app 60, zone 3** (celle de
l'inventaire I2 ; le même GUID existe en zone 2 sous app 29 — portail mort, cf. I2).

Dans cette app, le content-type **`Game Rule` (id 377)** porte exactement **5 entités publiées** —
une par variante :

| EntityId | GUID | UrlKey | Valeurs | Version |
|---|---|---|---:|---:|
| 11378 | `AE1EDEFA-6F1B-4593-8230-97FA1EDF4F78` | `l-ecole-des-menteurs` | 11 | 11 |
| 11380 | `36AF82F9-4DA4-4CA8-81A7-231FB936DF5D` | `le-bingo-mixologie-argumentative` | 10 | 3 |
| 11387 | `64E365CA-D01D-4C07-8108-4A0782561D9E` | `le-dernier-beau-parleur` | 10 | 2 |
| 11388 | `0167188C-C8A7-4CDB-BE0F-FC3926979996` | `le-moulin-a-baratin` | 11 | 3 |
| 11389 | `430DEE14-DCC8-490D-AA45-6721D04EFF89` | `la-parlote-coinchee` | 10 | 3 |

**15 attributs**, dont 6 texte portent la matière à resynchroniser :

| Attribut | Type | Contenu (mesuré sur l'École) | ↔ section carte |
|---|---|---|---|
| `Summary` | String | 774 car., `<p>…</p>` | « Résumé du jeu » |
| `Material` | String | 336 car., `<ul><li>` | « Matériel » |
| `Installation` | String | 1 069 car., `<p>` | « Installation » |
| `Content` | String | **15 732 car.**, `<h2>/<h3>/<p>` | Déroulé + sections numérotées |
| `Variants` | String | 1 113 car., `<ul><li>` | « Variantes : » |
| `Memo` | String | 310 car., `<h3>` + listes | (web-only : Conditions de Victoire, Nombre de pioches) |

Autres : `Title`, `MinNbPlayers`/`MaxNbPlayers` (déjà alignés), `Date` (**2022-05-02** — confirme la
généalogie de l'analyse #1502), `UrlKey`, `Parent`/`Author`/`Licence`/`Original` (Entity — ne pas toucher).

**Format** : HTML aux **entités nommées** (`&eacute;`, `&rsquo;`, `&agrave;` — la forme stockée de la
mémoire EAV), emojis en clair (🥇👇🎭). **0 valeur dimensionnée dans l'app 60** (requête
`TsDynDataValueDimension` = 0, conforme I2) : chaque champ porte **une seule valeur FR** — pas de
complexité langue à l'écriture.

## 3. Plan d'écriture (phase suivante — non exécuté ici)

1. **Sauvegarde d'abord** (règle du grain) : dump JSON de chaque entité (ValueId, AttributeId, Value)
   vers `Logs/`, empreintes sha256 consignées.
2. **Régénérer** les champs texte depuis la colonne `Text` du CSV (`Cards/Rules/Argumentum Rules -
   Cards.csv`, master) : markdown → HTML aux entités nommées, sections appariées par le même `nkey`
   que l'instrument. L'École = `Content` 15,7 ko, les 4 autres proportionnées.
3. **Exceptions, nominatives** :
   - **Phrases 3 et 4** (attente owner) vivent dans **`Variants` de l'École (11378)** : la variante
     « le baratineur peut poser plusieurs cartes » et l'item « A 5 joueurs et plus… ». Ces **deux items
     de liste restent byte-identiques** ; les autres items de `Variants` se synchronisent normalement.
   - **`Memo` (11378)** porte les pictogrammes « Nombre de pioches » **avec le « + » que le CSV a
     perdu** (perte `ac3e2c5a`, cf. #1502) : le site est **correct** sur ce point — **ne pas
     resynchroniser `Memo` depuis le CSV** tant que R-cartes n'a pas rendu le « + » (sinon on
     répliquerait la perte sur le site). La mémo suit en outre la décision owner du 24/09 (24/09,
     #1539/#1543) — reprise telle quelle.
   - Sections web-only (« Contenu », « Carte Mémo » sur les 5 pages, « Conditions de Victoire »,
     « Nombre de pioches ») : **préservées** (les « 4 éléments propres au web » de la DoD).
4. **Recycle d'app-domain** (cache LightSpeed 2sxc) + contrôle sur le **servi**, pas sur la DB.
5. **DoD** : l'instrument (`--base` préprod) ne rend plus d'écart, hors phrases 3/4 et sections
   web-only ; les 5 compteurs restent identiques ; `Variants`/`Memo` de l'École inchangés au sha256.

## 4. Ce que cette mesure n'établit pas

- La **conversion markdown → HTML** n'est pas écrite : le plan la suppose mécanique (titres `#`→`<h3>`,
  listes `-`→`<li>`, gras/emphase à vérifier sur le corpus réel), à valider sur UNE section avant de
  dérouler.
- L'**exhaustivité** de la liste d'écarts reste celle de l'analyse #1502 (l'instrument apparie par
  section, pas par élément).
- La **prod** n'est pas mesurée ici (elle est gelée #972 ; l'analyse du 08/10 l'a déjà relevée).

## Reproductibilité

```bash
python docs/corpus/rules-site-vs-cards.py --base https://dnn.argumentum.myia.io/R%C3%A8gles
python docs/corpus/rules-site-vs-cards.py            # défaut = prod, comportement inchangé
```

L'instrument embarque ici deux couches : le drapeau `--base` (cette PR) et le correctif
`autojunk=False` (#1835, mergée — la branche de cette PR est synchronisée sur master). Le rejeu du
09/10 soir (matrice post-écriture ci-dessus) a été fait avec les deux.

Sondes DB (lecture seule, exécutées via la chaîne `SiteSqlServer` du webroot, secret jamais affiché) :
module 602 / `ModuleSettings` → app GUID ; `TsDynDataApp` (app 60) ; `TsDynDataContentType`/`TsDynDataAttribute`
(id 377) ; `TsDynDataEntity`/`TsDynDataValue` (5 entités) ; `TsDynDataValueDimension` (0 ligne).

---

*po-2023 (worker lane `argumentum-31`), 2026-10-09. Mesure lecture seule ; l'écriture attend son
créneau dédié.*
