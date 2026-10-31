# #458 item 9bis — module-level lot 4 : `admin` ar-SA (27 fichiers, 328 clés)

**Date :** 2026-10-31 · **PR :** suite de #2088 (admin zh-CN) · **Dossier :** `DNNPlatform/admin/{Modules,Portal,Security,Skins,Tabs,Users}/App_LocalResources` · **Culture :** `ar-SA`

Périmètre identique au lot zh-CN #2088 : 27 fichiers, 328 clés livrées sur 338 neutres (10 déclarées manquantes — repli neutre DNN, mêmes clés que zh). Quatre stems collident entre répertoires (`Export`, `Import`, `Privacy`, `Terms`) : la couverture est identifiée par le couple (répertoire, stem).

## Livraison

| Répertoire | Fichiers | Clés |
|---|---:|---:|
| Modules | 6 (Export, Import, ModuleLocalization, ModulePermissions, ModuleSettings, ViewSource) | 165 |
| Portal | 2 (Privacy, Terms) | 2 |
| Security | 3 (AccessDenied, PasswordReset, SendPassword) | 54 |
| Skins | 14 (Copyright, Language, LinkToFull/Mobile/TabletSite, Privacy, Search, Tags, Terms, Toast, TreeViewMenu, User, UserAndLogin) | 52 |
| Tabs | 2 (Export, Import) | 53 |
| Users | 1 (ViewProfile) | 2 |
| **Total** | **27** | **328** |

Sources : 6 JSON `458-item9bis-admin-ar-SA.{modules,portal,security,skins,tabs,users}.translations.json` (jeux de clés vérifiés identiques au lot zh, stem par stem). Constructeur exécuté une fois par répertoire, `--bom mirror`. Espaces signifiants du neutre reflétés : `Language.ascx:Label.Text` « اختر اللغة:  » (espace final), `UserAndLogin` «  رسائل » / «  إشعارات » (espaces initiaux).

## Contrôles indépendants (hors garde)

| Contrôle | Résultat |
|---|---|
| Jeux de clés == lot zh livré, par (dir, stem) | 27/27 identiques |
| Valeurs vides | 0 |
| Placeholders `{n}` / jetons nus `[ORGANIZATION]`…/ balises HTML | 0 écart |
| BOM par fichier vs neutre | 27/27 miroir (5 neutres sans BOM : PasswordReset, SendPassword, LinkToFull/Mobile/TabletSite, ViewProfile) |
| Fins de ligne | LF uniquement |
| Copies anglaises | 0 |
| Couverture écriture arabe `[؀-ۿ]` | 328/328 |

## Garde factorisée — sous-classe, pas copie

La garde zh (#2088, 111 tests) est devenue **socle abstrait** `DnnModuleLevelAdminResxGuardBase` (motif acté pour `controls` le 31/10, PR #2092) :

- le socle porte `Covered` (27 triplets), `DeclaredMissing` (3 fichiers / 10 clés), les 4 théories × 27 fichiers et les 3 faits épingles — **le contrat ne dépend pas de la culture** ;
- `DnnModuleLevelAdminZhResxGuardTests` est réduite à `Culture => "zh-CN"` ;
- `DnnModuleLevelAdminArResxGuardTests` ajoute `Culture => "ar-SA"` + la théorie d'écriture arabe (27 fichiers) + le fait de parité verbatim-vide.

Résultat : **250 tests** (111 zh + 111 ar + 27 écriture + 1 parité), zéro avertissement (CS0108 du premier jet corrigé par `MemberData(MemberType=…)` canonique ; xUnit1014 résolu par `nameof`).

## Contrôles inverses (la garde voit-elle ce qu'elle prétend voir ?)

| # | Mutation | Attendu | Obtenu |
|---|---|---|---|
| 1 | `Copyright {1}`→`{2}` (arbre + commit via le pack) | 1 rouge : multiset placeholder | 1 rouge `EveryDeliveredKeyIsSaneAgainstTheNeutral` ✓ |
| 2 | Valeur ar → anglais exact | 2 rouges : copie + écriture | 2 rouges ✓ (honnête : une copie intégrale viole les deux règles — isolées ci-dessous) |
| 3 | Valeur ar → `Aperçu…` (latin NON anglais) | 1 rouge : écriture seule | 1 rouge `EveryValueCarriesArabicScript` ✓ — c'est le cas que la règle « ≠ anglais » du socle ne voit pas |
| 4a | BOM ajouté sur `ViewProfile` **dans l'arbre seulement** | vert (la garde lit le blob) | 27/27 verts ✓ |
| 4b | Mêmes octets **committés** (commit temporaire) | 1 rouge nommant le fichier | 1 rouge `Users/ViewProfile.ascx` ✓ |

Nettoyage 4b : `git reset --soft HEAD~1` + restauration cp + `git add`, sha256 des deux fichiers cibles identiques aux empreintes pré-mutation, `git status` propre, 250/250 verts.

## Tests

- Garde filtrée : **250/250** (0 échec, 0 ignoré).
- Suite complète : **2712 total = 2706 réussite / 0 échec / 6 ignorés** (master 2573 + 139 : la sous-classe ar hérite des 111 du socle + 27 écriture + 1 parité).

## Hors périmètre

- `DesktopModules` et `controls` fa-IR : gardes/socles distincts — séquencement « un lot par module/culture, jamais empilé » (fa-IR controls attend le merge de #2092, même garde).
- Le lot n'installe rien sur la préprod : resx = restart DNN, geste po-2023 avec [INFO] au préalable — jamais dans ce lot.

## Suivant

`admin` fa-IR : sous-classe du socle présent (3 lignes de garde + pack fa) — mais séquencement derrière le merge de ce lot et de #2092 par ai-01.
