# #458 grain module-level — périmètre résiduel ar-SA / fa-IR, mesuré (2026-11-01)

Entrée du volet 4 du pool (« résiduels ar-SA puis fa-IR, jumeaux de #2106 »).

> ⚠️ **Version 3 — les deux précédentes étaient fausses, et de deux façons
> différentes.** La v1 annonçait « 197 fichiers / 10 408 clés » (cultures
> retirées à la main, sœurs ignorées). La v2 corrigeait en « 102 / 5 679 » et
> croyait avoir trouvé la cause. La v2 se trompait encore de **trois fichiers**
> et de **15 clés** : son instrument ne reconnaissait comme culture que le motif
> `xx-XX`, donc une culture **de langue seule** (`nl`) et une culture **suivie
> d'un suffixe de portail** (`fr-FR.Portal-1`) passaient pour des neutres.
>
> Le chiffre juste est **99 fichiers / 5 664 clés**. Les 3 versions et les
> instruments qui les ont produites sont détaillés en fin de note : c'est la
> partie utile, parce que **les trois erreurs sont de la même famille**.

## Ce qui reste à traduire sous `DesktopModules`

Mesuré sur master `3554741b`, **après** merge des lots Admin ar-SA (#2109) et
fa-IR (#2111) :

| Objet | Fichiers |
| --- | ---: |
| `*.resx` sous `DNNPlatform/DesktopModules` (toutes cultures) | 870 |
| dont **neutres** (aucun suffixe de culture) | **148** |
| Neutres **sans sœur `ar-SA`** | **99** |
| Neutres **sans sœur `fa-IR`** | **99** |

Le résidu vaut identiquement pour les deux langues : aucun des deux packs
n'avance sur l'autre.

### Décomposition du résidu

| Groupe | Fichiers | Clés | Dans le périmètre ? |
| --- | ---: | ---: | --- |
| Neutres portant déjà une sœur `zh-CN` | **37** | **689** | **OUI — c'est le lot 4a** |
| `Admin/Dnn.PersonaBar/Modules/*` | 25 | 3 304 | non (exclu comme en zh) |
| `NBright/*` | 30 | 1 659 | non (exclu comme en zh) |
| Reste (gabarits ModuleCreator ×6, `InvalidConfiguration.ascx`) | 7 | 12 | non (gabarits techniques) |
| **Total** | **99** | **5 664** | |

Le lot 4a est donc **un lot ordinaire de 37 fichiers / 689 clés** — exactement
les 37 fichiers que #2106 a livrés en `zh-CN`. La traduisibilité est prouvée par
la culture sœur ; il n'y a rien à arbitrer fichier par fichier. *(Inchangé
depuis la v2 : cette partie n'a jamais été fausse.)*

## Réconciliation avec la mesure d'ai-01 — exacte, et reproductible

La v2 présentait un écart « 1 fichier / 259 clés » comme *attribué*. Il ne
l'était pas : l'attribution tombait juste par compensation de deux erreurs. Le
compte correct se referme **exactement**, par deux différences de définition
nommées :

| Poste | Ici | ai-01 | Différence |
| --- | ---: | ---: | --- |
| Fichiers | 99 | 101 | **+2** — les deux `*.fr-FR.Portal-1.resx` |
| Clés | 5 664 | 5 420 | voir ci-dessous |

```text
5 664   blocs <data> des 99 neutres sans sœur ar-SA
-  250   blocs à valeur VIDE (242 CountryNames + 8 dans 4 fichiers PersonaBar)
= 5 414
+    6   blocs des 2 fichiers Portal-1, que le motif xx-XX comptait comme neutres
= 5 420   ← la mesure d'ai-01, à la clé près
```

Les 6 blocs sont bien **non vides** (1 dans `CartView.ascx.fr-FR.Portal-1.resx`,
5 dans `Notification.ascx.fr-FR.Portal-1.resx` — des messages GDPR / commande en
français), sinon l'arithmétique se refermerait sur du vide.

⭐ **Ce que « réconcilié » veut dire ici** : la mesure d'ai-01 se **recalcule**
depuis cet instrument en annulant les deux corrections (recompter `Portal-1`
comme neutre, exclure les valeurs vides). Ce n'est pas un écart toléré, c'est
un écart **reproduit**. Et les deux différences vivent dans des buckets **hors
périmètre** : elles ne changent ni le lot 4a, ni son volume.

## État après livraison du lot 4a (`623f8fd8`)

Le lot 4a ar-SA a été livré et mergé (#2115) ; master est passé à `623f8fd8`.

| Objet | `3554741b` | `623f8fd8` |
| --- | ---: | ---: |
| `*.resx` toutes cultures | 870 | **907** (+37) |
| Neutres | 148 | 148 |
| Neutres sans sœur **ar-SA** | 99 / 5 664 | **62 / 4 975** |
| Neutres sans sœur **fa-IR** | 99 / 5 664 | **99 / 5 664** *(inchangé)* |

⭐ Le résidu ar-SA rejoint **exactement** le résidu `zh-CN` (62 / 4 975 = 25
PersonaBar + 30 NBright + 7 gabarits) : les deux langues en sont maintenant au
même point, ce qui est précisément la définition d'un « jumeau de #2106 ». Le
résidu fa-IR, lui, n'a pas bougé — c'est le prochain lot.

## L'erreur de la v1, et ce qui l'a rendue possible

Deux fautes, de la même famille — un instrument qui définit mal son objet :

1. **Périmètre** : le calcul retirait **7 cultures** du total
   (`ar-SA`, `fa-IR`, `zh-CN`, `fr-FR`, `ru-RU`, `pt-BR`, `es-ES`). Les neutres
   déjà traduits en `it-IT` (21), `uk-UA` (20), `nl-NL` (20), `en-GB` (3),
   `de-DE` (1) et `ua-UK` (1) étaient donc comptés **comme à traduire**.
2. **Sœurs ignorées** : 29 neutres avaient déjà leur sœur `ar-SA` **et**
   `fa-IR` — le calcul ne les retirait pas.

Résultat : 197 fichiers au lieu de 99, 10 408 clés au lieu de 5 664.

## La famille d'erreurs d'instrument — quatre occurrences, une seule cause

| # | Version | Objet mal défini | Ce que l'instrument rendait | Le vrai chiffre |
| --- | --- | --- | --- | --- |
| ① | v1 | **périmètre** — 7 cultures en dur | 197 fichiers | 99 |
| ② | v1 | **sœurs** — clé gardant son `.resx` | 0 sœur `zh-CN` (par construction) | 37 |
| ③ | v2 | **clés** — sous-chaîne `<data` en brut | 6 059 clés | 5 664 |
| ④ | v3 | **suffixes** — motif `xx-XX` seul | 151 neutres / 102 sans sœur | 148 / 99 |

④ est la plus instructive, parce que **c'est l'instrument de contrôle introduit
en v2 pour réparer ① qui était lui-même aveugle**. La v2 avait retenu la bonne
règle — *énumérer l'ensemble des valeurs possibles d'un champ avant de filtrer
dessus* — mais l'avait appliquée avec `[^./]+`, qui capture le composant avant
`.resx` sans le **classer** :

```text
    105 ascx        ← l'EXTENSION du gabarit (Settings.ascx.resx), pas une culture
     43 NEUTRE      ← les neutres dont le nom n'a qu'un point
      2 Portal-1    ← un suffixe de PORTAIL, derrière une vraie culture (fr-FR)
```

Aucun de ces trois libellés n'est une culture. Le motif `xx-XX`, lui, ne voyait
ni `nl` (langue seule) ni `fr-FR` dans `….fr-FR.Portal-1.resx`.

⭐ **La règle corrigée** : énumérer ne suffit pas, il faut **classer** chaque
composant — et un suffixe de portail se retire **avant** de lire la culture.
Énumérer sans classer produit une liste qui a l'air d'un inventaire et qui, en
réalité, mélange des extensions de fichier et des codes de langue.

## Reproductibilité (vraie)

```bash
# 1. inventaire des suffixes — classement explicite, pas un filtre unique
#    (`Portal-N` se retire avant de lire la culture ; `ll` seul est une culture)
git ls-tree -r --name-only HEAD -- DNNPlatform/DesktopModules | grep '\.resx$' \
  | sed -E 's/\.resx$//' \
  | awk -F. '{ n=NF; if ($n ~ /^Portal-[0-9]+$/) n--;
      if ($n ~ /^[a-z]{2}$/ || $n ~ /^[a-z]{2}-[A-Z]{2}$/) print $n; else print "NEUTRE" }' \
  | sort | uniq -c | sort -rn
```

Rendus sur `3554741b` — **rejoués, pas recopiés** :

```text
148 NEUTRE   135 fr-FR    134 es-ES    112 ru-RU    90 pt-BR    86 zh-CN
 49 fa-IR     49 ar-SA     21 it-IT     20 uk-UA    20 nl-NL     3 en-GB
  1 ua-UK      1 nl         1 de-DE
```

**14 cultures** — et c'est le nombre que la v2 annonçait déjà, alors que sa
propre liste n'en montrait que 13 : elle était juste par accident, parce que le
`nl` manquait à l'affichage et que `ascx`/`Portal-1` n'étaient pas des cultures.
`fr-FR` vaut **135** et non 133 : les deux fichiers `Portal-1` en portent la
culture. `ua-UK` est une **coquille du dépôt** pour `uk-UA` (1 fichier,
`NBright/NBrightBuy/App_LocalResources/CountryNames.ascx.ua-UK.resx`) —
à ne pas corriger ici : elle est hors périmètre et changerait un nom de fichier.

```bash
# 2. neutres dépourvus de sœur ar-SA : neutres − ceux qui portent la culture
#    (les clés se comptent sur les éléments <data> ENFANTS DE LA RACINE)
```

Résultats attendus sur `3554741b` : 870 total, 148 neutres, **99 sans sœur
`ar-SA`**, 99 sans sœur `fa-IR`, 5 664 clés.

### Compter les clés : les éléments `<data>`, jamais la sous-chaîne `<data`

Occurrence ③ de la même famille. Compter la sous-chaîne `<data` **en brut**
rend **6 059** clés au lieu de **5 664**. L'écart n'est pas du bruit — c'est
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
**12** pour les gabarits.

### Et les 250 valeurs vides

`CountryNames.ascx.resx` porte **242 blocs dont les 242 valeurs sont vides** ;
quatre fichiers PersonaBar en portent **8** de plus (Extensions 3, Security 3,
PersonaBar 1, Roles 1). Une valeur vide est comptée ici comme une clé (le bloc
existe) et exclue par ai-01 : c'est **la** différence de définition qui explique
les 250 clés, et elle est nommée plutôt que tolérée. Sur ce fichier,
« nombre de clés » vaut 242 ou 0 selon la définition — à trancher explicitement
le jour où NBright entrera dans un périmètre.

⭐ Le motif est le même quatre fois dans cette note : le périmètre (suffixes
mal classés), les sœurs (clé de forme différente de part et d'autre), les clés
(commentaire vs contenu), et le contrôle censé réparer le premier (filtre unique
au lieu d'un classement). **À chaque fois c'est la définition de l'objet qui est
fausse, jamais l'outil qui l'applique.**
