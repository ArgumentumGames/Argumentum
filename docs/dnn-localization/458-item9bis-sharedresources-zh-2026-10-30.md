# #458 grain ⑨-bis — `SharedResources` zh-CN, le premier pack **complet** de la famille AGR cœur

> **Statut :** pack **complet** livré pour **une** culture (zh-CN, 610 clés). Le webroot servi et
> la préprod ne sont **pas** touchés : un `.resx` est un fichier du dépôt, sa prise en compte par
> IIS demande un recyclage du pool — geste distinct. Suivi :
> [#458](https://github.com/ArgumentumGames/Argumentum/issues/458). Dispatch : ai-01, pool v3
> (c.6368412786), grain ⑨-bis. Mesure du dénominateur : PR #2065.
>
> Les cultures **ar-SA** et **fa-IR** ne sont **pas** dans cette PR : la consigne est de les
> traiter **séquentiellement**, jamais empilées.

---

## 1. Ce que cette PR livre

`DNNPlatform/App_GlobalResources/SharedResources.zh-CN.resx` passe de **3 clés** (fichier
partiel du Tier 1) à **610 clés** — le chrome de l'interface DNN : boutons, messages de
validation, gabarits de courriel d'authentification, libellés d'administration, messages de
l'installateur.

| Fichier | Avant | Après | Neutre | Reste |
|---|---:|---:|---:|---:|
| `SharedResources.zh-CN.resx` | 3 | **610** | 617 | 0 traduisible |

⚠️ **Le diff paraît plus gros que le travail — c'est du réalignement, pas 1 950 clés.** Le
fichier de 3 clés du Tier 1 avait été **écrit à la main**, avec un en-tête XML abrégé
(`<?xml version='1.0' …?>`) et sans le bloc de schéma `resx`. Le constructeur repart du
**neutre** : l'en-tête redevient celui de tous les `.resx` DNN, et les blocs `<data>` perdent
l'indentation de deux espaces qu'ils avaient dans le neutre (comportement **préexistant** du
constructeur, visible à l'identique dans `GlobalResources.zh-CN.resx` — 171 blocs sur 172 non
indentés). Le fichier livré a donc exactement la forme de ses **quatre frères** déjà mergés.
Corriger cette indentation demanderait de réécrire les cinq packs dans la même PR : hors sujet
ici, et à faire en un geste unique si on le veut.

## 2. Deux comptes différents, tous les deux vrais — ne pas les confondre

Le fichier neutre porte **617** clés. Le fichier zh-CN en portait 3 ⇒ **614 manquantes**.
Mais **7 de ces 614 ont une valeur neutre vide** :

```text
File.Text
DefaultWorkflowState1.OnDiscardMessageBody     DefaultWorkflowState1.OnDiscardMessageSubject
DefaultWorkflowState3.OnCompleteMessageBody    DefaultWorkflowState3.OnCompleteMessageSubject
DefaultWorkflowState3.OnDiscardMessageBody     DefaultWorkflowState3.OnDiscardMessageSubject
```

Ces clés sont des emplacements que le produit ne remplit jamais. Le constructeur **refuse** de
les traduire (« un neutre vide hors coquille — rien à traduire ») : les remplir serait inventer
du texte. Le dénominateur réel est donc **610**, et c'est ce que la garde épingle.

⇒ *« 614 manquantes » et « 610 livrées » ne se contredisent pas* : le premier compte mesure un
écart de fichiers, le second un travail possible. Un rapport qui ne cite que l'un des deux laisse
croire à 4 clés perdues.

## 3. Méthode — le `.resx` n'est pas recréé

Inchangée depuis les lots 1-3, et c'est le point qui compte : on part du fichier **neutre**, on
ne remplace que le contenu des `<value>` des clés traduites ; en-tête, schéma `xsd`, `resheader`,
**ordre des clés**, **BOM UTF-8** et fins de ligne **LF** sont préservés.

```bash
python -I docs/dnn-localization/458-item9bis-langpack-build.py \
  --lang zh-CN --translations docs/dnn-localization/458-item9bis-zh-CN.translations.json \
  --root <webroot>/DNNPlatform/App_GlobalResources
```

Le dictionnaire `458-item9bis-zh-CN.translations.json` est étendu d'une section
`files.SharedResources` (610 entrées) ; il reste **la** source de vérité du pack, et il porte
désormais les quatre familles du grain en un seul objet.

### Reproducibilité — vérifiée, pas supposée

Le constructeur rejoué **depuis le dictionnaire committé** réécrit les **cinq** `.resx` zh-CN
(`FileUpload`, `WebControls`, `Exceptions`, `GlobalResources`, `SharedResources`) et les rend
**byte-identiques** (`sha256` comparés avant/après, 5/5 `OK`). Un pack qu'on ne peut pas
reconstruire à partir de son dictionnaire n'est pas relisible : il n'est qu'un artefact.

## 4. Contrôles indépendants du constructeur

Mesurés sur le fichier produit, par un instrument distinct (parsing XML, pas le rapport du
script) :

| Contrôle | Résultat |
|---|---|
| Clés livrées | **610** |
| Clés manquantes vs neutre | **7** — exactement les 7 à neutre vide |
| Clés inventées / hors neutre | **0** |
| Valeurs vides | **0** |
| Multiset des placeholders `{0}`/`{1}` | **0 écart** |
| Identité des jetons DNN (`[Custom:…]`, `[Portal:…]`) | **0 écart** |
| BOM UTF-8 / fins de ligne | présent / **LF seul**, 0 CRLF |
| Valeurs Tier 1 déjà arbitrées (`Password.Text`, `cmdCancel.Text`, `Home.Text`) | **identiques** |

### Huit valeurs légitimement identiques à l'anglais — déclarées, pas tolérées

`True.Text`, `False.Text`, `OAuthConfigBase_APIKey.Text` (« APP ID »),
`DropDownList.SortAscendingButtonTitle` (« A-Z »), `SizeB/Kb/Mb/Gb.Text`.

Les deux littéraux booléens sont **parsés par nom** dans les listes déroulantes : les traduire
changerait le comportement, pas la langue. Les six autres sont des jetons, une marque produit,
un glyphe de tri et des symboles d'unité. Ils sont inscrits nominativement dans
`VerbatimByDesign["zh-CN"]` de la garde — une copie volontaire se **déclare**, elle ne se
tolère pas.

## 5. Évolution d'une garde écrite par une autre lane — explicite, pas silencieuse

`ChromeTier1LocalizationContractTests.ScopeIsThirteenKeysAndPartialFilesContainNoExtraKeys`
épinglait **exactement 3 clés** dans `SharedResources.{zh-CN,ar-SA,fa-IR}.resx`. Cette assertion
rendait le fichier immuable : elle interdisait d'ajouter la moindre clé. Le pack complet la rend
donc fausse **par construction**.

Ce qui change, et ce qui ne change pas :

- **Ne change pas** : les 13 clés Tier 1 gardent leur **valeur revue**, assertée cellule par
  cellule (`EachTier1CellHasItsReviewedValue`, 39 cellules) — c'était l'intention de la garde.
- **Change** : l'égalité exacte d'ensemble ne s'applique plus qu'aux fichiers **encore partiels**.
  Une liste nommée `CompletedPacks` déclare l'exception ; sur ces couples, la garde exige que les
  clés Tier 1 soient **présentes** et que le fichier porte **plus** que les 3 clés (sinon la
  déclaration « complet » serait fausse).
- **Se déplace** : la santé des clés ajoutées (non-vide, non-copie accidentelle, placeholders,
  jetons) appartient à `DnnAgrCoreResxIntegrityGuardTests`, qui gagne
  `("zh-CN", "SharedResources", 610)`.

⚠️ **Une garde d'égalité exacte est un verrou, pas un contrôle de qualité** : sur un fichier
partiel elle empêche l'ajout ; sur un fichier complet elle échoue à chaque clé ajoutée sans rien
dire de sa justesse. La remplacer par un plancher explicite (*les clés revues sont là, et il y en
a d'autres*) conserve l'intention et libère la croissance. C'est le même motif que la liste
blanche `VerbatimByDesign` : **déclarer**, jamais assouplir en silence.

## 6. Tests

| Périmètre | Résultat |
|---|---|
| `ChromeTier1LocalizationContractTests` + `DnnAgrCoreResxIntegrityGuardTests` | **75 / 75** |
| Famille `Dnn*` (tout le dossier Dnn des tests) | **484 / 484** |

## 7. Ce que cette PR ne fait pas

- **Aucun geste préprod** : les `.resx` sont des fichiers du dépôt ; leur compilation par IIS
  demande un recyclage du pool applicatif, qui est un geste **distinct** et séparé.
- **ar-SA et fa-IR non touchés** : traitement séquentiel exigé par le dispatch.
- **`List_Country` non traité** : les trois cultures n'ont pas de fichier du tout (256 clés),
  c'est un grain différent.
- **Le pack n'est pas « testé à l'écran »** : la validation visuelle reste côté ai-01.

## 8. Suite

1. `SharedResources` **ar-SA** (même dénominateur, 610).
2. `SharedResources` **fa-IR**.
3. Puis `List_Country` (256 clés) et le grain module-level (96 fichiers, 2 355 clés/langue).
