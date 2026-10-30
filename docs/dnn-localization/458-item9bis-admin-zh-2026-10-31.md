# #458 grain module-level, lot 2 — `admin` zh-CN (27 fichiers, 328 clés livrées)

**Date :** 2026-10-31 · **Lane :** po-2023 · **Culture :** `zh-CN` seule (ar-SA et fa-IR non touchés)
**Périmètre :** `DNNPlatform/admin/{Modules,Portal,Security,Skins,Tabs,Users}/App_LocalResources/` — les **27 familles CORE** du grain
**Dénominateur :** mesuré par #2072 (CORE sans `Install/` : 94 fichiers / 2 168 clés par langue) — re-mesuré à l'identique ce jour : **27 fichiers / 338 clés neutres** (`Login.ascx` de Skins exclu : ses ar/fa/zh existent déjà)
**Origine :** ordre d'ai-01 du 30/10 (rang ③, *« #2072 modules ar/fa/zh »*). `controls` ar-SA/fa-IR restent en file **derrière le merge de #2087** (règle de séquençage : jamais deux lots empilés sur la même garde) — `admin` est un dossier et une garde distincts.

---

## 1. Ce qui est livré

**27 `.resx` créés, 328 clés** (338 neutres − 10 déclarées non livrées, §3) :

| Répertoire | Fichiers | Clés livrées |
|---|---:|---:|
| `Modules` | 6 (dont `ModuleSettings` 114) | 165 |
| `Portal` | 2 | 2 |
| `Security` | 3 | 54 |
| `Skins` | 13 | 52 |
| `Tabs` | 2 (dont `Import` 36) | 53 |
| `Users` | 1 | 2 |

⚠️ **Quatre stems collident entre répertoires** : `Export` et `Import` (Modules × Tabs),
`Privacy` et `Terms` (Portal × Skins). Le constructeur mono-`--root` a donc tourné **six fois**
(un couple répertoire/JSON par passe) et la garde identifie chaque fichier par le
**couple (répertoire, stem)**, jamais par le stem seul.

## 2. Le constructeur n'a toujours PAS été doublé

Le même `458-item9bis-langpack-build.py`, avec `--bom mirror` (le BOM du produit suit SON
neutre — ici `PasswordReset`/`SendPassword`, `LinkToFull/Mobile/TabletSite`, `ViewProfile`
n'ont pas de BOM quand les 22 autres en ont un). **Le script est le blob exact de la branche
#2087** (`ec127736…`, copié par `git cat-file`) : les deux PR convergent sur un contenu
identique, la fusion est propre dans les deux sens.

Un seul différenciel fonctionnel, motivé : le seuil d'aveuglement `len(neutral) < 2` (écrit
pour l'AGR, où tout fichier fait des centaines de clés) confondait « fichier minuscule » et
« instrument qui ne voit rien » — `admin/Portal/Privacy.ascx` a **légitimement 1 clé**.
Corrigé en `< 1` (l'aveuglement réel = 0 clé parsée). La ligne modifiée est hors du patch
`--bom` : aucune intersection avec #2087.

## 3. Dix clés déclarées non livrées — repli neutre, pas « traduites »

Même philosophie que les packs AGR : une clé sans contenu localisable n'est pas traduite en
recopiant l'anglais, elle est **absente** du fichier produit et DNN retombe sur le neutre.

| Fichier | Clés | Raison |
|---|---|---|
| `Skins/Language.ascx` | `Footer/Header/SeparatorTemplate.Default` ×3 | **neutre vide** (structurel) |
| `Skins/Language.ascx` | `Alternate/CommonFooter/CommonHeader/Item/SelectedItemTemplate.Default` ×5 | **markup HTML pur** (`[CULTURE:…]`, `[URL]`, `[FLAGSRC]`) — zéro prose affichable |
| `Skins/Search.ascx` | `URL.Text` | **modèle d'URL Google** (`https://www.google.com/custom?q=[TEXT]…`) — zéro prose |
| `Skins/User.ascx` | `NewMessages.Text` | `" ({0})"` — **format pur** (parenthèses + placeholder), zéro prose |

La liste est **épinglée à 10** par la garde (`TheDeclaredMissingListIsLiveAndPinned`) : chaque
entrée vise un fichier du lot, chaque clé existe au neutre — une clé « oubliée » ne peut pas
se déguiser en repli déclaré.

⚠️ `NewMessages.Text` a été **attrapé par le vérificateur indépendant**, pas par le
constructeur : livré d'abord à l'identique du neutre (« copie de l'anglais non déclarée »).
Retiré du dictionnaire, fichier supprimé et reconstruit — le constructeur **préserve** les
clés d'un fichier culturel existant, retirer du dictionnaire ne retire pas d'un fichier déjà
écrit (comportement voulu pour les traductions neuves, piégeux en retrait).

## 4. Contrôles — instruments indépendants du constructeur

| Contrôle | Résultat |
|---|---|
| Fichiers produits / clés | **27 / 328** |
| Livré = neutre − déclarées manquantes (par fichier) | **27/27** — 0 manquante non déclarée, 0 inventée |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** |
| Identité des jetons DNN | **0 écart** |
| BOM suit le neutre, par fichier | **27/27** |
| Fins de ligne | **LF seul**, 0 CR |
| Copies de l'anglais non déclarées | **0** (verbatim par conception : **aucune** dans ce lot) |

> ℹ️ La garde C# mesure le BOM/EOL sur le **blob committé** (`git cat-file`) dès sa première
> rédaction — la règle transport payée 2× (#2078, #2087) est appliquée en amont cette fois.

## 5. La garde

`DnnModuleLevelAdminZhResxGuardTests` (3 faits + 27 × 4 théories = **111 tests**) — même
squelette que le lot `controls`, trois différences de fond :

1. **Couverture par couple (répertoire, stem)** — les collisions Export/Import/Privacy/Terms.
2. **`DeclaredMissing` vivante et épinglée** — le lot `controls` couvrait 133/133 sans repli ;
   l'admin introduit le repli déclaré, la garde le tient (10, ni plus ni moins).
3. **`VerbatimByDesign` vide, épinglée à 0** — le lot `controls` en portait 2 (« HTML »,
   « URL ») ; ici aucune valeur n'est légitimement identique à l'anglais.

### Contrôles inverses — trois mutations, trois règles distinctes

| Mutation | Attendu | Mesuré |
|---|---|---|
| `{0}` → `{9}` dans `Tabs/Export.ascx:ExportedMessage.Text` (forme échappée `&lt;b&gt;{0}&lt;/b&gt;` du resx) | multiset des placeholders rouge | ✅ 1 échec / 110 verts, message nommant la clé, `{0}` vs `{9}` à l'index 0 |
| `文件夹：` → `Folder:` dans `Tabs/Export.ascx:plFolder.Text` | copie de l'anglais rouge | ✅ 1 échec / 110 verts, message nommant la clé |
| BOM **ajouté** à `Security/PasswordReset.ascx.zh-CN.resx` (neutre sans BOM), **committé** via commit temporaire local jamais poussé puis retiré | BOM rouge sur le blob | ✅ 1 échec / 110 verts, `producedBom=True` vs `neutre=False` |

⚠️ La mutation placeholder a d'abord **échoué à s'appliquer** : le resx stocke les valeurs
**échappées** (`&lt;b&gt;{0}&lt;/b&gt;`, pas `<b>{0}</b>`) — un contrôle inverse qui mute la
forme brute ne mute rien et « passe » à vide. Toujours vérifier `MUTATION APPLIQUEE`.

Restauration par copie de sauvegarde, `sha256` revérifiés identiques (`ED8BCB57…`,
`AE663461…`), commit temporaire défait par `reset --soft`, `git status` propre sur les resx.

## 6. Ce que cette PR ne fait pas

- **`ar-SA` et `fa-IR` non touchés** : une culture par PR, jamais empilées.
- **Aucun autre dossier** : `DesktopModules` (56 fichiers / 1 704 clés) suit ; `Install/`
  (2 / 187) reste recommandé à l'exclusion (#2072, décision ai-01).
- **`controls` ar-SA/fa-IR attendent le merge de #2087** — la garde `controls` n'existe que
  sur cette branche ; empiler y créerait des conflits de fusion, pas des lots.
- **Rien côté préprod** : un `.resx` est un fichier du dépôt ; sa prise en compte par IIS
  demande un recyclage du pool, geste distinct. Le webroot servi n'est pas touché.
- **Pas de relecture native** : traduction produite par la lane, relecture éditoriale zh ouverte.

## 7. Suite

`DesktopModules` zh-CN (56 fichiers / 1 704 clés — le gros morceau, à découper si la garde
dépasse le raisonnable), puis `controls` ar-SA/fa-IR après merge de #2087, puis `admin`
ar-SA/fa-IR. Constructeur et squelette de garde prêts sans modification.
