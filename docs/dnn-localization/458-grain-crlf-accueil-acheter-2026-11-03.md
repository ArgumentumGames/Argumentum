# #458 — Grain CRLF du clone webroot, typo de l'accueil, et dossier « Buy the game »

**Date :** 2026-11-03 · **Lane :** po-2023 · **Base :** `d8c595ac` · **Portée :** préprod uniquement (prod gelée #972)

Trois grains du pool `ai01-dispatch-po2023-20261103c` / `...-d`. Deux gestes faits, un
dossier mesuré sans geste (consigne explicite d'ai-01).

---

## 1. Geste CRLF sur le clone webroot — fait, prédiction affinée de 21 à 22

### Ce qui était demandé

Passer le clone en `core.autocrlf=true`, puis réinscrire à l'index **seulement** les fichiers
dont le contenu converti est égal au blob de `HEAD`. Prédiction d'ai-01 : **1 751** après la
config seule, puis **21** après le `add`, avec arrêt si le compte baissait dès la config.

### Ce qui a été mesuré

| Étape | Instrument | Résultat |
|---|---|---:|
| État initial | `git status --porcelain` (suivis) | **1 751** |
| Config posée, avant tout `add` | `git status --porcelain` (suivis) | **1 751** ✅ prédiction tenue |
| Décision par fichier | `hash-object` converti **=** blob `HEAD` ? | **1 729 égaux / 22 différents** |
| Après `git add` des 1 729 | `git status --porcelain` (suivis) | **22** |
| Aucun changement indexé | `git diff --cached --name-only` | **0** |

**La cible n'est pas 21 mais 22.** L'arithmétique se referme : `1 729 + 22 = 1 751`.

### Pourquoi 22 et non 21 — le 22ᵉ fichier, nommé

Les 21 fichiers sous `DNNPlatform/Portals/_default/` sont bien ceux de #2127 (templates de site,
CSS `WebControlSkin`, 9 PNG, `subhost.aspx`). Le 22ᵉ n'est **pas** sous `Portals/` :

> `DNNPlatform/admin/Security/module.css`

Son blob `HEAD` se termine **sans saut de ligne final** (416 o, 7 LF, dernière ligne non
terminée) ; le disque porte un CRLF final (425 o, 8 CRLF). La conversion `autocrlf` ne peut donc
pas le ramener au blob : elle rend 417 o contre 416.

Trois instruments, trois verdicts sur **ce seul fichier** :

| Instrument | Verdict |
|---|---|
| `diff --numstat` avec conversion (`autocrlf=true`) | **1** ligne changée |
| `diff --numstat --ignore-cr-at-eol` (`autocrlf=false`) | **0** — il le tait |
| `diff --numstat` brut (`autocrlf=false`) | **8** lignes changées |
| `hash-object --path` converti vs `rev-parse HEAD:` | **différent** → exclu |

C'est **le test de hash qui a tranché**, pas l'appartenance à une liste. Le chiffre « 21 » venait
de l'instrument `--ignore-cr-at-eol`, qui sous-compte ici d'une unité. Je constate le désaccord
sans prétendre expliquer l'interne de git : le fait mesuré suffit, et la règle de décision
n'en dépend pas.

### Contrôles de non-régression (tous les quatre)

| Contrôle | Attendu | Mesuré |
|---|---|---|
| `git diff --cached --name-only` | 0 | **0** ✅ |
| Suivis modifiés | 22 | **22** ✅ |
| Non suivis (lignes `??`) | 49 (inchangé) | **49** ✅ |
| **Disque** : sha256 des 1 751 fichiers | 1 751 identiques | **1 751 / 1 751**, 0 changé, 0 manquant ✅ |
| Pages `/en/` servies | marqueurs identiques | identiques ✅ |

⚠️ **L'oracle « sha de page entière » n'est pas valide ici, et c'est prouvé, pas supposé** : deux
GET consécutifs de `/en/…/Terms` **sans aucun geste entre eux** rendent des octets différents
(même taille, 54 475 o). Le contrôle porte donc sur les **marqueurs** : `ARGUMENTUM SAS` = 1,
`922020276` = 1, gabarit DNN = 0 — identiques avant et après. Sur Privacy, `ARGUMENTUM SAS` = 0,
ce qui est **correct** (le blob master ne porte pas la chaîne).

### Exclusions et retour arrière

- **0** fichier exclu n'a été rencontré : aucun `web.config`, aucun `*.key`, aucun `machinekey`
  parmi les 1 751. La garde existe dans le script et n'a rien eu à écarter.
- L'avertissement `git add` listant 31 répertoires `Scripts/` « ignorés » (`.gitignore:234`) est
  **sans effet sur un fichier suivi** : vérifié sur `…/Console/Scripts/jquery.console.js` — suivi,
  sha d'index **égal** à `HEAD`, statut vide.
- Sauvegarde de l'index : `.tmp/crlf-20261103/index.backup` (1 878 298 o, taille identique).
  Manifeste sha256 avant : `.tmp/crlf-20261103/manifest-before.json`.
  Retour arrière = restaurer l'index puis `core.autocrlf=false`.

⚠️ **Effet de bord à connaître** : le clone étant désormais en `autocrlf=true`, tout fichier
réécrit par un futur `pull`/`checkout` sera matérialisé **en CRLF sur disque**. C'est le
comportement Windows attendu et sans effet sur le rendu servi ; il est noté ici pour que personne
ne le lise plus tard comme une nouvelle dérive.

---

## 2. « Vous defendez » → « Vous défendez » sur l'accueil

### Localisation (stockage EAV 2sxc)

Recherche **globale**, pas seulement dans l'entité attendue :

| Magasin | Occurrences de `defendez` |
|---|---:|
| `TsDynDataValue` (EAV 2sxc) | **1** |
| `HtmlText` | 0 |
| `Tabs` (titre/description) | 0 |

Valeur unique : **`ValueId 23034`**, `EntityId 10055`, app **33** (`Content`), `AttributeId 1820`
(`Text`), **aucune rangée de dimension** ⇒ c'est la **valeur de base** (celle servie en FR).
Aucun doublon d'entité ne porte le texte — le piège documenté a été vérifié, pas supposé.

### Geste

1. Backup JSON des 18 valeurs de l'entité : `.tmp/eav-backup-20261103/entity-10055.json`.
2. Dry-run gardé : « `Vous defendez` » présent **exactement 1 fois** dans la valeur (1 380 o,
   690 caractères) ; forme corrigée absente. Abandon sinon.
3. `UPDATE` **ciblé par `ValueId`**, en transaction, avec la garde dans le `WHERE`
   (compte d'occurrences = 1) et rollback si `@@ROWCOUNT ≠ 1`. → **1 ligne**, validée.
4. Recycle d'app-domain : `LastWriteTime` de `web.config` touché, **sha256 inchangé**
   (`B59E9AFB…`), jamais `iisreset`.
5. Contrôle **servi** (et non pas fichier).

Forme retenue : `Vous d&eacute;fendez` — la **forme dominante de la même valeur** (5 × `&eacute;`
contre 2 × `é` brut). Longueur 690 → **697** (delta +7), vérifiée en base et sur la page.

### Contrôle servi

| Mesure | Avant | Après |
|---|---:|---:|
| `Vous defendez` | 1 | **0** |
| `Vous d&eacute;fendez` | 0 | **1** |

Trois GET consécutifs concordent. ⚠️ Le premier appel après recycle paie le démarrage à froid :
mesuré **76,3 s**, puis **0,27 s** et **0,36 s** — ne pas lire ce délai comme une panne.

### Le pivot est corrigé aussi, et c'est le point qui compte

`docs/dnn-localization/eav-content.csv` est le **pivot d'injection** (c'est de lui que partent les
écritures en base). Laissé avec le typo, il le **ré-injectait** à la passe suivante : un correctif
en base seule aurait été défait sans que personne ne s'en aperçoive.

Correction **en octets** : 1 occurrence remplacée, **+7 octets**, fins de ligne préservées à
l'identique (**216 CRLF / 4 867 LF** avant et après), `git diff --numstat` = **`1 1`**.

---

## 3. Fiche « Buy the game » sur `/en/` — mesurée, **aucun geste**

Consigne d'ai-01 : mesurer, ne rien toucher avant sa décision. Rien n'a été modifié.

### Ce qui est servi

Le lien « Buy the game » de la page `/en/` pointe vers une URL **française** :

```
/Acheter-le-jeu/Detail/catid/140/eid/130/le-jeu-de-cartes-argumentum
```

| Mesure sur cette page | Valeur |
|---|---|
| HTTP | 200, 38 935 o |
| `<html lang>` | **`fr-FR`** |
| Marqueurs français (`le la les des du et pour vous nous jeu cartes`) | **51** |
| Marqueurs anglais (`the and for with you our game cards`) | **0** |
| Caractères accentués FR | **39** |
| `/en/Acheter-le-jeu/…` | **200 mais redirigé** — l'URL finale perd `/en/`, `lang` reste `fr-FR` |
| `…?language=en-US` | 200, `lang` reste **`fr-FR`** |

⇒ La page n'est **pas routée en anglais du tout** : ce n'est pas une fiche EN mal traduite, c'est
une fiche **sans variante EN**, servie en repli français.

### Pourquoi — le magasin, et la cause exacte

Ce contenu ne vit ni dans l'EAV ni dans `HtmlText` : il est dans **`NBrightBuy`** (boutique),
colonne `XMLData` (type `xml`, à caster), `TypeCode = PRDLANG`.

| ItemId | Lang |
|---:|---|
| **131** | **fr-FR** ← le produit, **seule ligne** |
| 148 | en-US |
| 149 | en-US |

Le produit **131 n'a aucun frère `en-US`** : les deux lignes anglaises appartiennent à d'autres
articles. D'où le repli français. ⚠️ Piège de repérage pour la suite : l'URL porte `eid/130` alors
que la ligne de boutique est **`ItemId=131`** — chercher `130` dans `NBrightBuy` rend **0**.

Contenu actuel de la fiche 131 : `En Rupture — Nous vous prions de nous excuser pour le
retard…` (rupture de stock), donc une fiche à réécrire, pas seulement à traduire.

**Rien n'a été touché** : la décision (traduire la fiche, créer la variante `en-US`, ou retirer le
lien de la navigation EN) appartient à ai-01.

---

## 4. Ce qui reste du pool

- **Lot 4a fa-IR** : 679 cellules sur 37 fichiers, base close par #2131. Non entamé ici —
  le livrer à moitié serait pire que ne rien livrer, la garde exige le lot entier.
- **Grains ③④②** : non traités.

---

*Mesures rejouables : scripts sous `.tmp/crlf-20261103/` et `.tmp/eav-backup-20261103/`.*
*Lane po-2023.*
