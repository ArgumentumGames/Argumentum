# #458 grain module-level — périmètre résiduel ar-SA / fa-IR, mesuré (2026-11-01)

Entrée du volet 4 du pool (« résiduels ar-SA puis fa-IR, jumeaux de #2106 »).
**Mesure, pas estimation** : elle remplace le chiffre de pool, qui ne nommait
aucun volume.

## Ce qui reste à traduire sous `DesktopModules`

Mesuré sur master `c174d5f6`, **après** application des deux lots Admin ouverts
(#2109 ar-SA et #2111 fa-IR, 20 fichiers identiques chacun) :

| Objet | Fichiers | Clés |
|---|---:|---:|
| `*.resx` sous `DNNPlatform/DesktopModules` (toutes cultures) | 830 | — |
| dont **neutres** (à traduire) | **217** | — |
| dont couverts par #2109 / #2111 | **20** | 645 |
| **Résidu à livrer, par langue** | **197** | **10 408** |

Le résidu vaut **identiquement pour `ar-SA` et pour `fa-IR`** : aucun des deux
n'avance sur l'autre — les 217 neutres sont sans sœur dans les deux cultures.

### Le sous-ensemble « jumeaux de #2106 »

Le pool désigne le volet 4 comme « jumeaux de #2106 ». Mesuré :

| Objet | Fichiers | Clés |
|---|---:|---:|
| Résidu total | 197 | 10 408 |
| **dont portant déjà une sœur `zh-CN`** | **66** | **1 416** |
| Reste (aucune sœur zh : jamais livré dans aucune culture de ce grain) | 131 | 8 992 |

⚠️ Les **66 ne sont pas « les 37 de #2106 »** : #2106 en a livré 37, et le dépôt
porte 86 `zh-CN` sous `DesktopModules` — dont des sœurs héritées, antérieures au
grain. Le motif du lot 3c (un neutre dont on sait qu'il est traduisible parce
qu'une sœur zh existe) couvre donc 66 fichiers, pas 37. Confondre les deux
ferait un lot de 37 et laisserait 29 fichiers « résiduels » sans nom.

## Ce que la mesure change pour le découpage

Le pool ordonne « item 4 : résiduels ar-SA puis fa-IR ». À 10 408 clés par
langue, ce n'est pas un lot : c'est **deux fois le grain module-level entier**
(les lots Admin pèsent 645 clés). Le découpage doit donc se faire sur la
sous-structure, pas sur la langue.

Proposition, dérivée des chiffres ci-dessus :

| Volet | Fichiers | Clés / langue | Motif |
|---|---:|---:|---|
| 4a | 66 | 1 416 | **sœur `zh-CN` existante** — motif #2106, la règle de traduction est déjà prouvée par une culture sœur |
| 4b | 131 | 8 992 | **aucune sœur** — chaque fichier demande un arbitrage propre (traduisible ? chaîne technique ?) |

Le volet 4a est celui que le pool nomme. Le volet 4b existe, il est plus gros
que tout ce qui a été livré, et il n'a **jamais été chiffré** : c'est la
découverte de cette mesure.

## Le piège qui a produit un faux chiffre

Le premier passage de ce census a rendu **229 fichiers sans sœur `ar-SA`,
0 avec sœur `zh-CN`**. Les deux chiffres étaient faux, pour une raison unique :
la clé du **neutre** gardait son extension (`X.ascx.resx`) tandis que la clé de
la **culture** était le stem nu (`X.ascx`). Aucune paire ne se formait donc —
les 229 étaient les neutres *sans aucune* sœur, comptés deux fois par
répertoire, et le compteur `zh-CN` restait à zéro par construction.

Le zéro n'était pas un résultat : c'était l'instrument qui ne pouvait pas voir
un `1`. Le contrôle qui l'a révélé est **externe au script** — `git ls-tree |
grep -c "zh-CN.resx"` rendait **86** là où le census affirmait 0. Une mesure qui
contredit une commande d'une ligne est fausse, et c'est la commande d'une ligne
qu'il faut croire.

**Règle pour la suite** : compter les fichiers par `git ls-tree` (une ligne, non
ambiguë), et n'utiliser un walk de répertoire que pour ce que `ls-tree` ne donne
pas — le contenu des clés, jamais leur dénombrement.

## Reproductibilité

Les chiffres ci-dessus se re-mesurent par deux commandes, sans script :

```
git ls-tree -r --name-only HEAD -- DNNPlatform/DesktopModules | grep -c '\.resx$'          # 830
git ls-tree -r --name-only HEAD -- DNNPlatform/DesktopModules | grep -cE '\.(ar-SA|fa-IR|zh-CN|fr-FR|ru-RU|pt-BR|es-ES)\.resx$'   # 613
```

830 − 613 = **217 neutres**. Le reste (20 couverts, 197 résiduels, 66 jumelles
zh) se dérive par différence d'ensembles entre `HEAD` et les branches
`feat/458-modulelevel-dtm-admin-{ar,fa}`.
