# #458 9-bis — `List_Country` ar-SA / fa-IR / zh-CN : 3×256 clés depuis CLDR release-47

**2026-10-30, lane po-2023.** Grain « tête de file » du pool après les packs AGR cœur
(zh-CN #2079, ar-SA #2080, fa-IR #2081). Mesure d'entrée (#2065) : `List_Country` =
**256 clés à produire par langue, aucun fichier pour ar/fa/zh**.

## L'objet : une coquille, pas un neutre rempli

`List_Country.resx` (neutre) porte **256 clés ISO alpha-2 (`XX.Text`) dont les valeurs
sont TOUTES vides** — mesure #2065, re-mesurée ici avant d'agir. Le contenu vit dans
les fichiers de culture : `fr-FR` / `ru-RU` / `pt-BR` / `es-ES`, chacun **256/256
remplis** (témoin de la convention : fr-FR).

Conséquence de méthode : la garde « une clé à neutre vide n'est jamais livrée » de
`DnnAgrCoreResxIntegrityGuardTests` **ne s'applique pas** — elle protège les fichiers à
neutre non vide. Ici l'inverse est la convention : le neutre vide est le porte-clés, et
comparer un pack au « neutre non vide » rendrait un 0 qui ne prouve rien.

## Source : CLDR release-47, cohérente avec le précédent ru

Le précédent `List_Country.ru-RU.resx` (#1907, `c2b1d849e`) : 254/256 valeurs du CLDR
main `ru.xml` + `AN`/`CS` codes ISO abrogés déclarés **[K]**.

- **Couverture mesurée** : `main/{ar,fa,zh}.xml` (release-47, `<territory>` sans `alt`)
  couvrent **254/256** chacune — il ne manque que `AN` (abrogé 2010) et `CS`
  (abrogé 2006), les deux mêmes que ru.
- **Contrôle de version (décisif)** : le pack ru-RU **committé** reproduit
  `cldr-ru.xml` release-47 **byte-exact sur les 254 clés CLDR** — la release-47 est
  donc la version déjà en vigueur dans la famille, pas un choix nouveau.
- **Locales régionales** : release-47 ne shippe **aucun** `main/{zh-CN,ar-SA,fa-IR}.xml`
  (inventaire mesuré de `common/main`, 1000 fichiers) — la locale de base EST la
  résolution CLDR pour ces cultures.
- **AN/CS [K]** (valeurs historiques établies, hors CLDR) : `جزر الأنتيل الهولندية` /
  `صربيا والجبل الأسود` (ar), `آنتیل هلند` / `صربستان و مونته‌نگرو` (fa, ZWNJ dans
  مونته‌نگرو), `荷属安的列斯` / `塞尔维亚和黑山` (zh).

## Méthode — le `.resx` n'est pas recréé

Même discipline que les packs AGR : on part du **neutre** et on ne remplace que le
texte des `<value>` (blocs vides `<value></value>` → valeur). En-tête, `resheader`,
ordre des clés, BOM UTF-8 et LF sont préservés **octet par octet**.

Source de vérité reproductible committée :

| Fichier | Rôle |
|---|---|
| `458-item9bis-list-country-cldr-47.json` | 3×256 valeurs + provenance (version, téléchargement, [K], identités croisées) |
| `458-item9bis-list-country-build.py` | constructeur : neutre + JSON → les 3 `.resx`, assert unicité de chaque bloc remplacé, re-parse XML et égalité clé par clé après écriture |

**Idempotence prouvée** : `--verify` rejoué après construction = **3× byte-identical**.
Le banc de mutations ci-dessous restaure chaque fichier par reconstruction, jamais à
la main.

## Contrôles — instrument indépendant du constructeur

Parsing XML des fichiers produits (pas le rapport du script) :

| Contrôle | Résultat |
|---|---|
| Clés livrées | **256** par culture, jeu == neutre, **ordre == neutre** |
| Valeurs vides | **0** |
| Écriture cible | **256/256** portent l'écriture arabe (ar, fa) / CJK (zh) — mesuré sur la source CLDR |
| Identité au témoin fr-FR | **0/256** (écritures disjointes : une recopie serait visible) |
| Identités croisées | zh vs ar/fa : **0** · ar↔fa : **9** translittérations légitimes (EG « مصر », BT « بوتان », BW, BM, HN…) — partagées par nature, pas un défaut, non assertées |
| BOM / fins de ligne | présent / **LF seul**, sur le **blob committé** (voir garde) |
| Échappement XML nécessaire | **aucun** (mesuré : aucun `&`, `<`, `>` dans les 3×254 valeurs CLDR) |

## Garde — `DnnListCountryLocalizationGuardTests` (7 faits + 27 pins)

Nouveau fichier de test, indépendant de la garde AGR (qui resterait muette ici, par
conception). Chaque règle vise un défaut distinct :

1. `NeutralIsTheEmptyShellOf256IsoCodes` — la coquille elle-même (256 clés, toutes
   vides) : si quelqu'un remplit le neutre, la convention des 7 fichiers change — la
   garde s'en réveille.
2. `EachPackCoversExactlyTheNeutralKeySetInTheSameOrder` — jeu == neutre ET ordre ==
   neutre (l'ordre fonde la reconstruction byte-exact).
3. `NoPackDeliversAnEmptyOrWhitespaceValue`.
4. `EveryValueCarriesTheLanguageScript` — ar/fa : écriture arabe ; zh : CJK.
5. `NoValueRecyclesTheFrenchWitness` — le défaut « texte recopié », observable entre
   écritures disjointes.
6. `ZhIsDisjointFromArabicScriptPacks` — zh ne peut légitimement coïncider avec
   ar/fa sur aucune valeur (mesuré 0).
7. `CommittedBlobsKeepBomAndLfEnvelope` — BOM + LF **sur le blob committé**
   (`git cat-file -p HEAD:<path>`), instrument du trou #2078 (remède `d4e6b345`,
   branche #2078 non encore mergée) : lire l'arbre de travail est **faux** sur CI
   (`core.autocrlf=true` convertit au checkout).

Plus 27 pins (US/FR/DE/GB/ZZ + IR/CN + AN/CS [K] × 3 cultures) — parité de forme avec
la garde ru (`ListCountryRu_256Keys_FilledFromCldr_SamplesPinned`).

## Contrôle inverse — 7 mutations, 7 rouges

Sans lui, la garde serait verte à vide. Chaque mutation est appliquée seule, le test
ciblé est lancé **filtré**, la restauration se fait par **reconstruction** (constructeur
rejoué, `--verify` byte-identical après) :

| # | Mutation | Test rougi | Verdict |
|---|---|---|---|
| 1 | une valeur vidée (`ZW.Text` zh) | `NoPackDeliversAnEmptyOrWhitespaceValue` | **1/1 rouge** |
| 2 | recopie fr (`FR.Text` zh = « France ») | `NoValueRecyclesTheFrenchWitness` + `EveryValueCarriesTheLanguageScript` | **2/2 rouges** |
| 3 | un bloc `<data>` retiré (ar) | `EachPackCoversExactlyTheNeutralKeySetInTheSameOrder` | **1/1 rouge** |
| 4 | pin dévié (`US.Text` fa = « آمریکا ») | `PinnedCldrValues` | **1/27 rouge** (la ligne mutée seule) |
| 5 | neutre rempli (`FR.Text` = « France ») | `NeutralIsTheEmptyShellOf256IsoCodes` | **1/1 rouge** |
| 6 | collision zh/ar (`EG.Text` zh = « مصر ») | `ZhIsDisjointFromArabicScriptPacks` | **1/1 rouge** |
| 7 | blob CRLF committé (commit temporaire, puis defait) | `CommittedBlobsKeepBomAndLfEnvelope` | **rouge** (exécuté au commit) |

Incident du banc, consigné : la mutation 5 a été restaurée par réflexe via
`git checkout -- <fichier>` — le motif que la règle interdit pour défair une mutation.
Le résultat était correct dans ce cas précis (index vierge, mutation seule modification
du fichier, égalité au blob HEAD **prouvée par empreinte** ensuite), mais le motif est
faux : les restaurations suivantes sont passées par la reconstruction. À ne pas
reproduire.

Avant commit, `CommittedBlobsKeepBomAndLfEnvelope` est rouge **par construction**
(les fichiers existent sur l'arbre mais pas dans HEAD) — démonstration incidente qu'il
lit bien le blob et non l'arbre.

## Ce que ce lot ne fait pas

- **Aucun geste préprod** : un `.resx` est un fichier du dépôt ; un pack mergé n'est
  pas un pack servi (recyclage de pool = geste distinct, resté côté po-2023 avec
  annonce).
- **Grain module-level** non traité (~96 fichiers, ~2 355 clés/langue, mesure #2072) :
  reste en tête de file.
- **Pas de validation visuelle** (ai-01) et **pas de relecture native** : les valeurs
  sont une extraction CLDR mécanique ; la relecture éditoriale ar/fa/zh ouverte pour
  les packs AGR s'étend de droit à celui-ci (mêmes langues, même canal).
- **Les dictionnaires `458-item9bis-{culture}.translations.json` ne sont pas touchés**
  (ils portent les traductions négociées des lots 1-6, dans la pile #2079/#2080/#2081) :
  la source de vérité de ce lot est le JSON CLDR dédié, hors pile — aucun conflit.

## CI

Base `master` directe (aucun fichier partagé avec la pile empilée) : les checks
s'attachent d'eux-mêmes au PR, pas de dispatch manuel.

*po-2023*
