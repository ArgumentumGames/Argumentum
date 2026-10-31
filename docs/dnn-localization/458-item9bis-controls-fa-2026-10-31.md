# #458 grain module-level — lot `controls` fa-IR (2026-10-31)

Troisième culture du grain `controls` (après zh-CN #2087 et ar-SA #2092), livrée sur la branche `feat/458-modulelevel-controls-fa`. Mêmes 11 fichiers / 133 clés, même constructeur, même contrat.

## Livraison

| Fichier | Clés |
|---|---:|
| Address.ascx | 34 |
| URLControl.ascx | 27 |
| User.ascx | 24 |
| TextEditor.ascx | 13 |
| UrlTrackingControl.ascx | 9 |
| SkinControl.ascx | 7 |
| DualListControl.ascx | 6 |
| filepickeruploader.ascx | 4 |
| Help.ascx | 4 |
| ModuleAuditControl.ascx | 3 |
| LocaleSelectorControl.ascx | 2 |
| **Total** | **133** |

Source de traduction : `458-item9bis-controls-fa-IR.translations.json` (authorée par la lane, 0 appel API). Constructeur inchangé : `--lang fa-IR --root DNNPlatform/controls/App_LocalResources --bom mirror` — les 9 contrôles bloquants sont passés du premier coup.

## Contrôles indépendants (parse XML + octets, hors constructeur)

- Jeu de clés == neutre, fichier par fichier (11/11), comptes == déclarés (133).
- 0 valeur vide.
- Multisets de placeholders `{0}`/`{1}` : 0 écart par clé.
- Jetons `[ORGANIZATION]`…`[SRC]` (les 8 de `Help:lblInfo.Text`, hors regex du constructeur) : 0 écart.
- Balises HTML (`<br>`, `<br/>`, `<br />`, `<b>`, `<h1>`, `<p>`) : 0 écart par clé.
- BOM du produit == BOM de SON neutre, fichier par fichier (Address sans BOM, URLControl avec).
- LF seul, aucun CR.

## Écriture persane

Le persan vit dans le bloc Arabic (U+0600–U+06FF) : lettres partagées avec l'arabe plus پ چ ژ گ ی ک propres au persan. La règle d'écriture est donc le même intervalle que celui du lot ar-SA — `EveryNonVerbatimValueCarriesPersianScript`. Comme au lot ar, elle voit ce que la règle « valeur ≠ anglais » ne peut pas voir : une valeur moitié traduite (« Website: وب‌سایت ») passerait la règle de copie mais pas la règle d'écriture.

## Contrôles inverses (5, mesure réelle)

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| 1 | `{0}` → `{9}` dans `TextEditor:EditorTitleLabel.Text` | 1 rouge (placeholder) nommant la clé | 1 rouge / 57 verts |
| 2 | `User:plWebsite.Text` → anglais `Website:` | 2 rouges (copie ET écriture) | 2 rouges / 56 verts |
| 3 | `User:plWebsite.Text` → `Aperçu:` (latin NON anglais) | 1 rouge (écriture seule) | 1 rouge / 57 verts |
| 4 | BOM ajouté dans l'ARBRE seul (Address, neutre sans BOM) | 0 rouge — la garde lit le blob | 58/58 verts |
| 5 | mêmes octets COMMITTÉS (commit temporaire jamais poussé) | 1 rouge nommant le fichier | 1 rouge / 57 verts |

Restaurations par copie + sha256 (jamais `git checkout --`) ; le commit temporaire du contrôle 5 est retiré par `git reset --soft HEAD~1` + restore + `git add`, arbre et index revenus propres (sha256 conforme au backup).

## Garde

- `DnnModuleLevelControlsFaResxGuardTests` : **4 lignes d'héritage** du socle `DnnModuleLevelControlsResxGuardBase` (`Culture => "fa-IR"`) + la règle d'écriture persane (théorie ×11) + la parité verbatim (1 fait).
- La garde factorisée n'a pas été recopiée — c'était le but du socle livré au lot ar : ce lot ajoute 58 tests sans dupliquer une seule règle.
- Filtrée `DnnModuleLevelControls*` : **162/162** (46 zh + 58 ar + 58 fa), zéro warning.
- Suite complète : **2929 = 2924 réussis / 0 échec / 5 ignorés** (2 m 46 s) — arithmétique exacte vs master mesuré par ai-01 (2871 + 58 = 2929). Un skip de la mesure ai-01 est passé ici : variance d'environnement locale, la branche ne modifie aucun test existant (invariant 0 échec tenu).

## Périmètre hors lot

- Les 2 valeurs verbatim déclarées : `TextEditor:Html.Text` (HTML), `UrlTrackingControl:URL.Text` (URL) — les mêmes d'une culture à l'autre.
- Les autres 96−11 familles CORE du périmètre #2072 restent hors grain (mesure : `458-item9bis-modulelevel-mesure-2026-10-30.md`).

## Prochaine étape

`admin` fa-IR (même motif, sous-classe du socle `DnnModuleLevelAdminResxGuardBase` + 328 clés), puis `DesktopModules` zh lot 3c (sœur).
