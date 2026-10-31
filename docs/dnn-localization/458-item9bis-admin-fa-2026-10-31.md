# #458 grain module-level — lot 6 : pack `admin` fa-IR

**Date :** 2026-10-31 · **Lane :** po-2023 · **Branche :** `feat/458-modulelevel-admin-fa`
**Périmètre :** `DNNPlatform/admin/{Modules,Portal,Security,Skins,Tabs,Users}/App_LocalResources`
**Livré :** 27 `.resx` fa-IR, **328 clés** · **0 $ d'API** (traductions authorées par la lane)

Troisième culture du pack `admin`, après zh-CN (#2088) et ar-SA (#2093) — périmètre **identique**, jeux de clés vérifiés égaux stem par stem.

## Livraison par répertoire

| Répertoire | Fichiers | Clés |
|---|---:|---:|
| `Modules` | 6 | 165 |
| `Portal` | 2 | 2 |
| `Security` | 3 | 54 |
| `Skins` | 13 | 52 |
| `Tabs` | 2 | 53 |
| `Users` | 1 | 2 |
| **Total** | **27** | **328** |

⚠️ Quatre stems collident entre répertoires (`Export`, `Import`, `Privacy`, `Terms`) : la couverture est identifiée par le **couple** (répertoire, stem), jamais par le stem seul — `Skins/Login.ascx.fa-IR.resx` existait déjà et n'est pas touché par ce lot.

## Constructeur (`458-item9bis-langpack-build.py`, inchangé)

Six invocations, une par répertoire racine, `--bom mirror` : **les 9 contrôles bloquants sont verts du premier coup** (clé inconnue, valeur vide, multiset de placeholders, identité des jetons, reparse == clés attendues, un fichier culturel existant ne perd aucune clé, seuil d'aveuglement < 1 clé parsée, accord blocs/clés).

## Vérification indépendante (parsing, pas le constructeur)

Instrument séparé qui **parse** les deux `.resx` (`json`/`ElementTree`, jamais une regex sur les octets bruts) et compare au **neutre** :

| Contrôle | Résultat |
|---|---|
| Jeu de clés = neutre − 10 déclarées non livrées | 0 écart |
| Comptes par stem | 0 écart (328) |
| Valeurs vides | 0 |
| Multiset des placeholders `{\d+}` | 0 écart |
| Identité des jetons DNN `[Portal:…]` | 0 écart (aucun dans ce périmètre) |
| Multiset des balises HTML | 0 écart |
| Valeur ≠ anglais | 0 copie |
| Écriture persane présente dans chaque valeur | 328/328 |
| Espaces signifiants (début/fin) alignés sur le neutre | 0 écart |
| BOM miroir du neutre + LF seul | 0 écart |

## Écriture persane — pourquoi une règle propre à la culture

Le persan vit **dans le bloc arabe U+0600–U+06FF** (lettres partagées + پ چ ژ گ ی ک) : aucune plage supplémentaire n'est nécessaire, et la règle ne recopie pas celle du lot ar — elle la **remplace** par culture. Le contrôle « ≠ anglais » du socle n'attrape que les copies **intégrales** ; une valeur à moitié traduite (libellé persan, reste du texte anglais) ne porte pas de caractère persan et tombe.

## Garde

`DnnModuleLevelAdminFaResxGuardTests : DnnModuleLevelAdminResxGuardBase` — **3 lignes d'héritage** + la règle d'écriture + un fait de parité verbatim. Aucune règle du socle n'est recopiée (c'est le but du socle livré avec #2093).

- **139 tests** ajoutés (111 du socle + 27 d'écriture + 1 de parité).
- Famille `DnnModuleLevelAdmin*` filtrée : **389/389** (111 zh-CN + 139 ar-SA + 139 fa-IR), 3 s, 0 warning.

### Suite complète

**3010 = 3005 réussis / 0 échec / 5 ignorés** (2 min 25 s). Arithmétique vs master mesuré par ai-01 (`9f60e1e3`, 2871 tests) : **2871 + 139 = 3010**, exacte.

⚠️ Le nombre d'ignorés diffère d'un cran (master 6, ici 5) : **variance d'environnement locale déjà constatée sur le lot controls fa-IR (#2096)**. La branche ne modifie **aucun test existant** — la cause de ce skip n'est pas établie et n'est pas inventée ici.

## Cinq contrôles inverses (mesurés, un par un)

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| C1 | `CustomSettings.Text` `{0}` → `{9}` | 1 rouge nommant la clé | **1 rouge / 139** — `multiset des placeholders modifié, {"{9}"} differs at index 0` |
| C2 | `Portal/Privacy` → anglais `Privacy Statement` | 2 rouges (copie **et** écriture) | **2 rouges / 139** — « copie de l'anglais par accident » + « aucun caractère persan » |
| C3 | `Portal/Terms` → latin **non anglais** `Conditions d'utilisation` | 1 rouge (écriture **seule**) | **1 rouge / 139** — invisible au contrôle « ≠ anglais », attrapé par l'écriture |
| C4 | BOM ajouté **dans l'arbre seul** (neutre `PasswordReset` sans BOM) | **vert** — la garde lit le blob committé | **139/139 verts** |
| C5 | BOM **committé** (commit jetable, jamais poussé) | 1 rouge nommant le fichier | **1 rouge / 139** — `le BOM doit suivre celui du neutre (neutre=False), but found True` |

Nettoyage C5 : `git reset --soft HEAD~1` + restauration par `cp` depuis la sauvegarde (jamais `git checkout --`) ; `sha256` du fichier restauré **identique** à la sauvegarde (`ecf9d52c…`), arbre et index propres.

## Hors périmètre — déclaré, pas « oublié »

10 clés neutres ne sont **pas** livrées, identiques aux lots zh/ar (propriétés du **neutre**, épinglées par la garde) :

- `Skins/Language.ascx` — 8 clés (3 neutres vides + 5 templates HTML purs sans prose affichable) ;
- `Skins/Search.ascx:URL.Text` — modèle d'URL Google ;
- `Skins/User.ascx:NewMessages.Text` — « ({0}) », format pur.

Une clé sans contenu localisable n'est pas « traduite » en recopiant l'anglais : le repli neutre DNN reste.

## Étape suivante

`DesktopModules` zh lot 3c (sœur) puis `DesktopModules` ar ; `admin` fa-IR solde l'item 4 du pool po-2023.
