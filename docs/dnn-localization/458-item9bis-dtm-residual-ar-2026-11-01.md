# #458 module-level — lot 4a ar-SA : résidu de `DesktopModules` (37 resx, 689 clés)

**Date :** 2026-11-01 · **Lane :** po-2023 · **PR :** `feat/458-modulelevel-residual-ar`
**Base :** master `3554741b` · **Dépense API :** 0 $ (traduction authorée lane)

Le lot est le **jumeau arabe** du lot zh-CN livré le même jour
(`458-item9bis-dtm-residual-zh-2026-11-01.md`) : **mêmes 37 fichiers, mêmes 689
clés, même référence — le NEUTRE**. Tout ce qui suit décrit les écarts entre les
deux lots, pas le périmètre, qui est identique.

---

## 1. Périmètre — repris tel quel, et pourquoi

37 fichiers / 689 clés, réparti sur `Admin/` (19 fichiers / 446 clés),
`AuthenticationServices/` (8 / 16), `Connectors/` (3 / 28), `ResourceManager/`
(4 / 130), `SiteExportImport/` (1 / 10), `TelerikRemoval/` (1 / 45),
`ToSic.Sxc/` (1 / 14). Le périmètre est **dérivé du même ensemble neutre** que le
lot zh : un fichier sans jumeau zh est aussi un fichier sans jumeau ar, la
condition portant sur les sœurs de culture en général. Aucune re-mesure propre
n'était donc nécessaire — et la garde le vérifie de toute façon fichier par
fichier contre le neutre, pas contre une liste.

Sept fichiers n'avaient aucune sœur de culture (ni fr-FR, ni ru-RU, ni pt-BR, ni
es-ES) : du **jamais-localisé**, pas un trou ar.

`Admin/HtmlEditorManager/Controls/App_LocalResources/InvalidConfiguration.ascx`
porte **0 clé** et reste hors livraison, déclaré : un `.ar-SA.resx` vide n'aurait
aucun sens. La garde vérifie que ce neutre est toujours vide.

## 2. Livraison — cinq règles de transport

1. **Le fichier produit est son neutre octet pour octet**, sauf le contenu interne
   de chaque `<value>`. Mesuré sur les 37 paires : squelette **identique** après
   normalisation des fins de ligne des deux côtés (`lot4a_skeleton.py`).
2. **Substitution textuelle, jamais `ET.tostring`.** Réécrire l'arbre produirait un
   diff de 100 % sur chaque fichier (réindentation, réordonnancement d'attributs,
   perte des commentaires de schéma ResX). Le writer ne remplace que les octets
   entre `<value>` et `</value>`, avec échappement `&`/`<`/`>`.
3. **BOM dérivé du neutre** — **30 fichiers avec, 7 sans**, exactement la même
   répartition que le lot zh (ce sont les mêmes fichiers). La règle est la
   dérivation, pas la liste : 37/37 sans écart.
4. **LF toujours**, y compris quand le neutre est commité en **CRLF**
   (`ToSic.Sxc/App_LocalResources/View.ascx.resx`, 102 CR dans le blob). Voir §4
   pour le défaut que ce point a réellement produit.
5. **Espaces signifiants préservés** : `ResourceManager` porte deux valeurs
   concaténées côté client — `FileSizeErrorMessage.Text` («  كبير جدًا…هو ")
   commence **et** finit par une espace, `InvalidExtensionMessage.Text`
   («  ليس من نوع مسموح بتحميله ») commence par une espace. Un `strip()` les
   casserait en silence.

**Traduction** : 689 valeurs authorées lane. Terminologie alignée sur les lots ar
précédents (`تسجيل الدخول`, `حفظ`, `إلغاء`, `الملف`, `المجلد`, `الوصف`, `الأمان`…).
Les exemples de syntaxe de recherche (`kettle~`, `cat*`, `"dnn is awesome"`) sont
**laissés tels quels** : ce sont des jetons de démonstration, les traduire
casserait l'exemple.

### 🔀 Le sens du contrôle d'écriture est INVERSÉ (décision 33)

Pour le lot zh la règle était « chaque valeur porte au moins un caractère
chinois ». Pour ar-SA **c'est l'inverse qui est vrai**, et c'est le point qui
aurait fait livrer du persan si on l'avait recopié :

| | ar-SA | fa-IR |
|---|---|---|
| ي U+064A · ك U+0643 · ى U+0649 (exclusives de l'arabe) | **attendues** | proscrites |
| پ چ ژ گ ک ی (exclusives du persan) | **proscrites** | attendues |

Mesuré sur ce lot : **418/679** valeurs non verbatim portent une lettre arabe
exclusive (**61,6 %**), et **0/689** portent une lettre persane. Le piège concret :
`ک` (U+06A9, persan) et `ك` (U+0643, arabe) sont **presque indiscernables à l'œil
nu** — un pack ar produit depuis un clavier persan est indétectable en relecture,
seulement par un instrument.

### Dix valeurs verbatim, épinglées

| Nature | Valeurs |
|---|---|
| Jetons de remplacement DNN | `[PortalRoot]`, `[UserFolder]` |
| Noms de marques | `Facebook`, `Google`, `Live`, `Twitter` (×`Login.ascx:Title.Text`) |
| Noms de langages | `C#`, `Visual Basic` (×`CreateModule`, ×`ViewSource`) |

⚠️ **Dix, pas onze.** Le lot zh en compte onze : il ajoute le marqueur de tri
`A-Z` (`pagePicker_sortAscendingButtonTitle.Text`). En arabe ce marqueur est
**traduit en « أ-ي »** — l'alphabet arabe n'a pas le même ordre, et `A-Z`
n'y désignerait rien. Recopier onze ferait échouer le fait de comptage : c'est
voulu, une dispense est une décision.

## 3. Vérificateur indépendant — trois instruments distincts du writer

Aucune fonction partagée avec le constructeur ; le périmètre est dérivé de
`git ls-tree`, pas de la liste du writer.

| Instrument | Ce qu'il mesure | Résultat |
|---|---|---|
| `lot4a_verify.py` | jeu de clés == neutre · BOM == neutre · 0 lettre persane · ≠ anglais hors dispense · pas de valeur vidée | **0 écart** |
| `lot4a_skeleton.py` | squelette hors `<value>` identique à l'octet (regex DOTALL non-greedy), LF normalisé des deux côtés | **37/37 identiques** |
| contrôle ad hoc | multiset des placeholders `{n}` et des jetons DNN | **0 écart** |

Le détecteur persan porte son **contrôle inverse dans le script** : il vérifie
qu'il reconnaît sa propre sonde (پ چ ژ گ ک ی) **et** qu'il ne signale pas les
lettres arabes attendues (ي ك ى). Sans la seconde assertion, un détecteur qui
signalerait tout le bloc U+0600–U+06FF serait vert sur un pack persan.

## 4. Deux défauts de livraison, tous les deux dans l'OUTIL

Les deux ont été trouvés **avant** la garde, par mesure, et corrigés dans le
constructeur. Ils sont consignés parce qu'ils se reproduiront.

**① Un `\n` littéral transformé en vrai saut de ligne.**
`ProviderConfiguration.ascx:StripFormattingOptions.Help` contient, dans le neutre,
la séquence de **deux caractères** `\` `n` (`…new lines (\n) on paste.`). Ma table
de traduction, écrite en JSON, a encodé `\n` en **saut de ligne réel** : le fichier
produit comptait **579 lignes contre 578** au neutre.

> ⚠️ **Le contrôle de squelette ne peut PAS voir ce défaut** : la ligne
> surnuméraire est *à l'intérieur* du `<value>`, que le contrôle retire avant de
> comparer. Ce qui l'a vu est le **compte de lignes** (578 vs 579). Corollaire
> général : un contrôle qui normalise ou retire une zone est **aveugle par
> construction** à tout défaut situé dans cette zone — il faut un second
> instrument qui les compte.

**② Un CRLF propagé.** Le neutre `ToSic.Sxc/View.ascx.resx` est commité en CRLF
(102 CR) ; le writer recopiait le neutre à l'octet et propageait donc ses CR dans
la sœur arabe. La garde a rougi (`BomMirrorsTheNeutralAndLineEndingsAreLf`), à
juste titre. Correction : normalisation LF à la sortie. **0 fichier ne porte plus
de CR.**

## 5. Contrôles inverses — 5 mutations

Banc `mutation_bench.py` : mutation sur le XML brut interne d'un fichier, sauvegarde
+ empreinte sha256, restauration par `cp` avec **sha256 identique vérifié** —
**jamais** `git checkout --`.

| # | Mutation | Attendu | Mesuré |
|---|---|---|---|
| ① | placeholder retiré (`ResourceManager:StatusBarMessage.Text` → « عرض {0} من عنصرًا ») | rouge nommant la clé | **1 rouge**, `multiset des placeholders modifié` — 225 verts |
| ② | copie **exacte** du neutre (`ViewConsole:ControlTitle_.Text` → « Basic Features ») | deux assertions | **2 rouges** : « copie de l'anglais » **et** « aucun caractère arabe » — 224 verts |
| ③ | latin **non** anglais (`ViewConsole:ControlTitle_.Text` → « Fonctionnalites de base ») | rouge écriture seule | **1 rouge**, `la valeur ne porte aucun caractère arabe` — l'assertion « ≠ anglais » ne tire pas : c'est la règle d'écriture seule qui voit ce cas |
| ④ | BOM ajouté à `ResourceManager.ar-SA.resx`, **arbre de travail seul** | **vert** (la garde lit le blob `HEAD`) | **226/226 verts** |
| ⑤ | lettre persane `گ` injectée dans `Logoff.ascx.ar-SA.resx` | rouge nommant le fichier | **1 rouge**, `NoDeliveredValueUsesPersianOnlyLetters` — 225 verts |

⚠️ **Erratum de banc** : ma première passe de ② injectait « Core Features », qui
n'est **pas** la valeur neutre (« Basic Features »). Elle mesurait donc la même
chose que ③, et l'étiquette « copie de l'anglais » était fausse. Détecté en
comparant l'injection au neutre, rejoué avec la valeur réelle (`mutation2bis.py`).
Un banc de mutation qui ne vérifie pas sa propre cible mesure ce qu'il croit
mesurer, pas ce qu'il annonce.

## 6. Garde

`DnnModuleLevelDtmResidualArResxGuardTests` — **226 tests** :

| Fait / théorie | Portée |
|---|---|
| `TheLotCoversThirtySevenFilesAndSixHundredEightyNineKeys` | 37 couples uniques, somme = 689 dérivée de `Covered` |
| `PackCoversTheDeclaredKeyCount` (×37) | compte par fichier |
| `TheDeliveredKeySetIsExactlyTheNeutralOne` (×37) | livré == neutre, aucun manquant |
| `EveryDeliveredValueIsSaneAgainstTheNeutral` (×37) | non vide, placeholders, jetons DNN, ≠ anglais hors dispense |
| `EveryNonExemptValueCarriesArabicScript` (×37) | écriture arabe, dispenses épinglées |
| `NoDeliveredValueUsesPersianOnlyLetters` (×37) | **0 lettre persane** — la règle propre à ce lot |
| `ThePersianInstrumentRejectsThisArabicLot` | contrôle inverse : `PersianScriptMarkers` appliqué à CE lot échoue aux deux critères (part persane 0 % < 80 %, `arabicOnly` > 0) |
| `BomMirrorsTheNeutralAndLineEndingsAreLf` (×37) | **blob committé**, BOM dérivé du neutre, 0 CR |
| `TheVerbatimListCannotGrowSilently` | 10 verbatim, 10 dispenses d'écriture |
| `TheDeclaredEmptyNeutralIsStillEmpty` | le neutre à 0 clé l'est toujours |

La garde est **autonome** (miroir du patron des lots précédents), pas sur le socle :
la factorisation des gardes `DesktopModules` reste un grain distinct (décision 34).

## 7. Suites

| Mesure | Résultat |
|---|---|
| Garde seule | **226 / 226** (3 s) |
| Suite complète locale | **4 295 = 4 290 / 0 / 5** (1 min 31 s) |

Base avant le lot : **4 069 = 4 064 / 0 / 5**, dérivée par soustraction des 226
tests du lot — la répartition d'ignorés est **locale à la machine** (les `Skip`
sont conditionnels à `python` et à l'index git) : elle ne se compare pas entre hôtes.

## 8. Non-objectifs

- Aucune installation préprod (fenêtre = jour de semaine, items 6/7 du pool).
- Aucune factorisation de garde (grain « socle », item 5).
- Aucun `PersonaBar` / `NBright` (exclus #2072).
- Aucune re-traduction des lots antérieurs : leurs gardes restent seules juges.
- Le lot **fa-IR** du même périmètre est un objet distinct, pas encore livré.
