# #458 grain module-level, lot 3b — `DesktopModules` « Admin » zh-CN (20 fichiers, 640 clés livrées)

**Date :** 2026-10-31 · **Lane :** po-2023 (session sœur du lot 3a) · **Culture :** `zh-CN` seule
**Périmètre :** `DNNPlatform/DesktopModules/{Admin/Security, Admin/ViewProfile}/App_LocalResources/` — 17 + 3 familles CORE du grain
**Dénominateur :** re-mesuré ce jour via `458-item9bis-modulelevel-extract.py --json` sur master `991e7816` : **20 fichiers / 645 clés neutres** (Security 17/626, ViewProfile 3/19), à l'identique de la découpe annoncée. Le reste Admin (Authentication 2, Console 2, Dnn.AdminLogs 1, HtmlEditorManager 1, SearchResults 3) = **9 fichiers / 381 clés** pour le lot 3c.
**Origine :** ordre d'ai-01 du 30/10 (rang ③), file annoncée au DONE du lot 3a. Le lot 3a (#2089, base `a2567de8`) reste OUVERT : ce lot vit sur une branche **neuve basée sur master `991e7816`**, aucun fichier commun avec la file de merge (#2086→#2093), aucune garde partagée.

---

## 1. Ce qui est livré

**20 `.resx` créés, 640 clés** (645 neutres − 5 déclarées non livrées, §3) :

| Répertoire | Fichiers | Clés livrées |
|---|---:|---:|
| `Admin/Security` | 17 (dont `Profile` 93, `Users` 61, `EditProfileDefinition` 56, `EditRoles` 51, `ManageUsers` 51) | 622 |
| `Admin/ViewProfile` | 3 (dont `SharedResources` **0** — coquille, §3) | 18 |

⚠️ **Stem en collision dans le lot** : `SharedResources` ×2 (Security, ViewProfile) — la garde identifie par le **couple (répertoire, stem)**, jamais le stem seul. Le constructeur mono-`--root` a tourné **deux fois** (un répertoire/JSON par passe).

## 2. Le constructeur n'a toujours PAS été doublé

Même `458-item9bis-langpack-build.py` que #2087/#2088/#2089 (blob master, seuil d'aveuglement `< 1`), `--bom mirror` : **17 des 20 neutres n'ont PAS de BOM** (seuls `EditGroups`, `EditRoles`, `MemberServices` en portent un) — l'attente est dérivée du neutre fichier par fichier, jamais globale.

## 3. Cinq clés déclarées non livrées — repli neutre, pas « traduites »

| Fichier | Clé | Raison |
|---|---|---|
| `Security/ProfileDefinitions` | `Edit.Header`, `Del.Header`, `Dn.Header`, `Up.Header` | **neutres VIDES** — icônes de la grille, rien à localiser |
| `ViewProfile/SharedResources` | `DefaultTemplate.Text` | **jetons purs** — toute la valeur vit dans `[PROFILE:…]`/`[USER:…]`, aucun fragment anglais |

⚠️ Cas nouveau au niveau **fichier** : `ViewProfile/SharedResources` n'a QUE cette clé ⇒ le pack est livré comme **coquille à 0 clé** (scaffold resx valide, repli neutre DNN clé par clé). La famille reste couverte au dénominateur ; l'alternative — ne pas livrer le fichier — laisserait une cible « manquante » fantôme à l'instrument pour toujours.

## 4. Six verbatim déclarés — marques et filtre A–Z

`Profile:ProfileProperties_{Facebook,GooglePlus,LinkedIn,Twitter,Skype}.Text` (marques : une étiquette « Facebook » reste « Facebook » en chinois) et `Users:Filter.Text` (`A,B,C,…,Z` — le filtrage par initiale pinyin utilise les mêmes lettres latines). La liste est **épinglée à 6** par la garde : toute croissance est une décision, pas un constat. C'est la première livraison du grain avec une liste verbatim non vide (le lot 3a l'épinglait à 0).

## 5. Contrôles — instruments indépendants du constructeur

Vérificateur indépendant (`python -I`, relecture des produits) : **OK — 20 fichiers, 640 clés, 0 écart** (ensembles, vides, multiset placeholders, verbatim ⊆ déclarés, BOM miroir, LF seul, aucun zh-CN résiduel hors lot sous `Admin/` — les fichiers déjà suivis exemptés).

## 6. La garde

`DnnModuleLevelDtmAdminZhResxGuardTests` (3 faits + 20 × 4 théories = **83 tests**) — même squelette que 3a ; BOM/EOL mesurés sur le **blob committé** (`git cat-file`), règle de transport payée 3× appliquée en amont.

### Contrôles inverses — mesurés avant push

| Mutation | Attendu | Mesuré |
|---|---|---|
| `{0}` → `{9}` dans `Users:PagerTextFormat.Text` | multiset rouge nommant la clé | ✅ *« …Users.ascx.zh-CN.resx:PagerTextFormat.Text — multiset des placeholders modifié »* |
| `SecurityRoles:cmdValidate.Text` `验证` → `Validate` | copie de l'anglais rouge | ✅ *« Expected value not to be "Validate" because …cmdValidate.Text — copie de l'anglais par accident »* |
| BOM **ajouté** à `Users.ascx.zh-CN.resx` (neutre sans BOM), **committé** via commit temporaire local jamais poussé puis défait (`reset --soft` + `restore --staged`, BOM retiré, arbre propre revérifié) | BOM rouge sur le blob | ✅ 1 échec / 82 verts : *« le BOM doit suivre celui du neutre (neutre=False), but found True »* |

Les deux premières mutations dans un même run : **2 échecs / 81 verts**, application vérifiée (`count==1` avant écriture), restauration par inversion exacte + `git status` propre vs HEAD, garde re-mesurée **83/83**.

**Suite complète sur l'arbre de la PR : 2656 = 2650 réussis, 0 échec, 6 ignorés** (2 m 46 s) — master `991e7816` à 2573 + les 83 de cette garde, l'arithmétique tombe juste.

## 7. Ce que cette PR ne fait pas

- **`ar-SA` et `fa-IR` non touchés** ; **le lot 3c** (reste Admin, 9 fichiers / 381 clés) suit ; `controls` fa-IR et `admin` fa-IR restent la file de la session sœur.
- **`Install/`** reste recommandé à l'exclusion (#2072, décision ai-01).
- **Rien côté préprod** — travail en worktree détaché, le webroot servi n'est pas touché.
- **Pas de relecture native** : traduction produite par la lane, relecture éditoriale zh ouverte.
- **Dépense API : 0 $** — traductions authorées par la lane, aucun appel externe.

## 8. Suite

Lot 3c : reste Admin (Authentication 2, Console 2, Dnn.AdminLogs 1, HtmlEditorManager 1, SearchResults 3 — 9 fichiers / 381 clés), ce qui clôt le zh-CN des 56 familles CORE `DesktopModules` (avec #2089). Puis ar-SA/fa-IR sur le même périmètre selon la file ai-01.
