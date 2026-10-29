# #458 grain ⑨-bis — famille AGR cœur : mesure réelle et dénominateur corrigé

**Date :** 2026-10-30 · **Lane :** po-2023 · **Portée :** lecture seule sur `DNNPlatform/App_GlobalResources`
**Objet :** fixer le dénominateur du Tier 2 ar/fa/zh avant d'ouvrir les 3 PR de pack.

---

## 1. Le dénominateur est **1 227 clés par langue**, pas 1 261

Mesuré sur les fichiers eux-mêmes (script `scripts/agr-core-extract.py`), jeu **identique** pour `ar-SA`, `fa-IR`, `zh-CN` :

| Fichier | clés neutres | dont non vides | **à produire / langue** | état ar/fa/zh aujourd'hui |
|---|---:|---:|---:|---|
| `SharedResources.resx` | 617 | 610 | **607** | 3 clés (Tier 1, #1995) |
| `List_Country.resx` | 256 | **0** | **256** | fichier **absent** |
| `GlobalResources.resx` | 182 | 180 | **178** | 2 clés (Tier 1) |
| `Exceptions.resx` | 132 | 132 | **132** | fichier **absent** |
| `WebControls.resx` | 50 | 50 | **50** | fichier **absent** |
| `FileUpload.resx` | 4 | 4 | **4** | fichier **absent** |
| **Total** | | | **1 227** | |

Le dispatch du pool (#458 c.6509704094 §4) annonce 1 261 clés, décomposées en `Shared 621 · Countries 256 · Global 186 · Exceptions 136 · WebControls 54 · FileUpload 8`. Les écarts sont **systématiques** : +14, +8, +4, +4, +4. Les chiffres du §2 du même commentaire (617 / 182) sont, eux, exacts — c'est le §4 qui dérive. **L'origine de l'écart n'est pas établie ici** (comptage sur DLL ? entrées hors `<data>` ?) ; ce document livre ce que les fichiers portent.

## 2. `Countries.resx` n'existe pas ; le fichier réel est `List_Country.resx`

Aucun `Countries*.resx` n'existe dans le dépôt (`find -iname` rend vide). Ne pas créer un fichier sous ce nom : le nom est la clé de résolution du fournisseur de ressources.

## 3. `List_Country` est une **coquille** — et c'est un piège d'instrument

`List_Country.resx` porte **256 clés, les 256 à valeur VIDE**. Le contenu vit **uniquement** dans les fichiers culturels :

| Culture | clés | non vides |
|---|---:|---:|
| `fr-FR` | 256 | 256 |
| `ru-RU` | 256 | 256 |
| `pt-BR` | 256 | 256 |
| `es-ES` | 256 | 256 |
| `ar-SA` / `fa-IR` / `zh-CN` | — | fichier **absent** |

Exemples (clé `AC.Text` / `AD.Text`) : « Île de l'Ascension » · « о-в Вознесения » · « Ilha da Ascensão » · « Isla de la Ascensión ». Le style varie d'une culture à l'autre (le russe abrège : `AE.Text` = « ОАЭ »), donc la consigne de style doit être posée par langue, pas recopiée du français.

⚠️ **Le piège, et c'est celui qui produit un faux « propre »** : un comptage qui compare chaque culture au **neutre non vide** rend `manquantes = 0` sur `List_Country`, puisque ce neutre est vide. Un `0` n'est une absence que si l'instrument pouvait voir un `1` — ici il ne pouvait pas. Mesuré **contre les fichiers culturels**, les 256 clés sont bel et bien à produire.

## 4. Faits à porter dans la DoD

- **202 des 1 227 clés portent un placeholder** `{0}` / `{1}` — concentrées dans `Exceptions` (ex. `The extension '{0}' is not allowed.  The file has not been added.`). Une traduction qui « déballe » le placeholder est le cas de faux vert le plus probable : la garde doit asserter **égalité exacte du multiset de placeholders** entre source et cible, pas seulement leur présence textuelle.
- **0 clé neutre vide hors `List_Country`** ⇒ la DoD « toute clé neutre non vide présente et traduite » est bien définie sur **971** clés, et les **256** noms de pays relèvent d'une autre règle (source = code ISO + référence `fr-FR`).
- Les valeurs neutres vides qui subsistent (`SharedResources` 7, `GlobalResources` 2) ne sont **pas** de la matière à traduire : elles sont vides dans la source.

## 5. Reproduction

```bash
python -I docs/dnn-localization/scripts/agr-core-extract.py --report   # comptes
python -I docs/dnn-localization/scripts/agr-core-extract.py --write <chemin.json>   # liste de travail
```

Le script est en **lecture seule** sur `DNNPlatform/` ; il n'écrit que le fichier de liste de travail demandé.

---

*po-2023*
