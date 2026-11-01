# #458 grain module-level — périmètre résiduel ar-SA / fa-IR, mesuré (2026-11-01)

Entrée du volet 4 du pool (« résiduels ar-SA puis fa-IR, jumeaux de #2106 »).

> ⚠️ **Version 2 — le chiffre de la version 1 était faux et a été rejeté par
> ai-01.** Le périmètre « 197 fichiers / 10 408 clés » annoncé d'abord comptait
> des fichiers déjà localisés dans d'autres cultures comme s'ils étaient
> neutres. Le vrai résidu est de **102 fichiers / 5 679 clés**, et le lot
> effectivement à livrer de **37 fichiers / 689 clés**. L'erreur et ce qui l'a
> rendue possible sont détaillés en fin de note : ils sont la partie utile.

## Ce qui reste à traduire sous `DesktopModules`

Mesuré sur master `3554741b`, **après** merge des lots Admin ar-SA (#2109) et
fa-IR (#2111) :

| Objet | Fichiers |
| --- | ---: |
| `*.resx` sous `DNNPlatform/DesktopModules` (toutes cultures) | 870 |
| dont **neutres** (aucun suffixe de culture) | **151** |
| Neutres **sans sœur `ar-SA`** | **102** |
| Neutres **sans sœur `fa-IR`** | **102** |

Le résidu vaut identiquement pour les deux langues : aucun des deux packs
n'avance sur l'autre.

### Décomposition du résidu

| Groupe | Fichiers | Clés | Dans le périmètre ? |
| --- | ---: | ---: | --- |
| Neutres portant déjà une sœur `zh-CN` | **37** | **689** | **OUI — c'est le lot 4a** |
| `Admin/Dnn.PersonaBar/Modules/*` | 25 | 3 304 | non (exclu comme en zh) |
| `NBright/*` | 33 | 1 674 | non (exclu comme en zh) |
| Reste (gabarits ModuleCreator ×6, `InvalidConfiguration.ascx`) | 7 | 12 | non (gabarits techniques) |
| **Total** | **102** | **5 679** | |

Le lot 4a est donc **un lot ordinaire de 37 fichiers / 689 clés** — exactement
les 37 fichiers que #2106 a livrés en `zh-CN`. La traduisibilité est prouvée par
la culture sœur ; il n'y a rien à arbitrer fichier par fichier.

## Réconciliation avec la mesure d'ai-01

| Poste | Ici | ai-01 | Écart |
| --- | ---: | ---: | --- |
| Sœur `zh-CN` | 37 / 689 | 37 / 689 | **identique** |
| Reste (gabarits) | 7 / 12 | 7 / 12 | **identique** |
| PersonaBar + NBright | 58 / 4 978 | 57 / 4 719 | 1 fichier / 259 clés |
| **Total** | 102 / 5 679 | 101 / 5 420 | idem |

L'écart vit **entièrement** dans le bucket PersonaBar + NBright, qui est **hors
périmètre** : il ne change ni le lot 4a, ni son volume. Les deux mesures tombent
d'accord sur tout ce qui sera livré.

Une cause candidate, **mesurée mais non attribuée** : `NBright/NBrightBuy/App_LocalResources/CountryNames.ascx.resx`
porte **242 blocs `<data>` dont les 242 valeurs sont vides**, et sa sœur de
culture est écrite **`ua-UK`** au lieu de `uk-UA` (coquille présente au dépôt).
Sur ce fichier, « nombre de clés » dépend de la définition retenue (blocs
`<data>` vs valeurs non vides) : 242 ou 0. Je ne prétends pas que c'est la cause
de l'écart d'ai-01 — je constate qu'un fichier de cette forme suffit à le
produire, et qu'il faudra le traiter explicitement le jour où NBright entrera
dans un périmètre.

## L'erreur de la version 1, et ce qui l'a rendue possible

Deux fautes, de la même famille — un instrument qui définit mal son objet :

1. **Périmètre** : le calcul retirait **7 cultures** du total
   (`ar-SA`, `fa-IR`, `zh-CN`, `fr-FR`, `ru-RU`, `pt-BR`, `es-ES`). Les neutres
   déjà traduits en `it-IT` (21), `uk-UA` (20), `nl-NL` (20), `en-GB` (3),
   `de-DE` (1) et `ua-UK` (1) étaient donc comptés **comme à traduire**.
2. **Sœurs ignorées** : 29 neutres avaient déjà leur sœur `ar-SA` **et**
   `fa-IR` — le calcul ne les retirait pas.

Résultat : 197 fichiers au lieu de 102, 10 408 clés au lieu de 5 679.

### Ce que la « reproductibilité » n'a PAS protégé

La note v1 livrait deux commandes d'une ligne censées rendre le chiffre
reproductible :

```text
git ls-tree … | grep -c '\.resx$'                                    # 830
git ls-tree … | grep -cE '\.(ar-SA|fa-IR|zh-CN|fr-FR|ru-RU|pt-BR|es-ES)\.resx$'   # 613
```

Elles rendaient bien 830 et 613 — **parce qu'elles utilisaient la même
définition fausse que le calcul**. Une vérification qui réutilise la définition
qu'elle prétend contrôler ne contrôle rien : elle rend le même nombre faux, deux
fois, avec l'autorité d'un second instrument.

⭐ **La règle qui manquait** : un contrôle de reproductibilité doit être
**indépendant de la définition testée**. Ici, la commande qui aurait vu le
défaut est celle qui **énumère les suffixes réellement présents** :

```bash
git ls-tree -r --name-only HEAD -- DNNPlatform/DesktopModules \
  | grep '\.resx$' | sed -E 's/.*\.([A-Za-z]{2}-[A-Za-z]{2})\.resx$/\1/; t; s/.*resx$/NEUTRE/' \
  | sort | uniq -c | sort -rn
```

Elle rend **151 NEUTRE**, 134 `es-ES`, 133 `fr-FR`, 112 `ru-RU`, 90 `pt-BR`,
86 `zh-CN`, 49 `fa-IR`, 49 `ar-SA`, 21 `it-IT`, 20 `uk-UA`, 20 `nl-NL`,
3 `en-GB`, 1 `ua-UK`, 1 `de-DE` — et c'est cette liste qui dit qu'il y a **14**
suffixes, pas 7. Énumérer l'ensemble des valeurs possibles d'un champ avant de
filtrer dessus aurait suffi.

La même leçon s'était déjà présentée sous une autre forme dans la v1 (clé du
neutre gardant son `.resx`, donc aucune paire ne se formait, donc « 0 sœur
`zh-CN` » par construction). Deux fois dans la même note : **c'est la définition
de l'objet qui casse, pas le code qui l'applique.**

## Reproductibilité (vraie)

```bash
# 1. inventaire des suffixes — contrôle indépendant de toute définition
git ls-tree -r --name-only HEAD -- DNNPlatform/DesktopModules | grep '\.resx$' \
  | sed -E 's/.*\.([A-Za-z]{2}-[A-Za-z]{2})\.resx$/\1/; t; s/.*resx$/NEUTRE/' | sort | uniq -c

# 2. neutres dépourvus de sœur ar-SA (le reste se dérive par ensembles)
#    neutres = total − tous les fichiers portant un suffixe de culture (14 mesurés)
```

Résultats attendus sur `3554741b` : 870 total, 151 neutres, 102 sans sœur
`ar-SA`, 102 sans sœur `fa-IR`.

### Compter les clés : les éléments `<data>`, jamais la sous-chaîne `<data`

Troisième occurrence de la même famille, rencontrée en re-mesurant ce périmètre
avec un second instrument : compter la sous-chaîne `<data` **en brut** rend
**6 059** clés au lieu de **5 679**. L'écart de 380 n'est pas du bruit — c'est
le **bloc de documentation du schéma ResX** que DNN embarque en tête de chaque
fichier, et dont les exemples sont eux-mêmes des `<data>` :

```xml
<!--
  Microsoft ResX Schema
  ...
  <data name="Name1"><value>this is my long string</value>...</data>
  <data name="Color1" type="System.Drawing.Color, ...">Blue</data>
  <data name="Bitmap1" mimetype="...">...</data>
  <data name="Icon1" type="System.Drawing.Icon, ...">...</data>
-->
```

Ces quatre lignes sont **dans un commentaire XML** : elles gonflent le compte
brut et disparaissent dès qu'on parse. Le compte à retenir est celui des
éléments `<data>` **enfants de la racine** ; il rend **689** pour le lot 4a et
**12** pour les gabarits — c'est-à-dire, très exactement, les chiffres d'ai-01.

⭐ Le motif est le même trois fois dans cette note : le périmètre (suffixes
oubliés), les sœurs (clés de forme différente de part et d'autre), et
maintenant les clés (commentaire vs contenu). **À chaque fois c'est la
définition de l'objet qui est fausse, jamais l'outil qui l'applique.**

