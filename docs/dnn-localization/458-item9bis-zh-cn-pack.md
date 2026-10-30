# #458 grain ⑨-bis — pack de langue zh-CN, famille AGR cœur (Tier 2)

> **Statut :** pack **partiel** livré (358 clés), préprod/webroot **non touché** (les `.resx`
> sont des fichiers du dépôt ; leur compilation par IIS se fait au recyclage du pool, geste
> po-2023 distinct). Suivi : [#458](https://github.com/ArgumentumGames/Argumentum/issues/458).
> Dispatch : ai-01, pool v3 (c.6368412786), grain ⑨-bis. Mesure du dénominateur : PR #2065.

---

## 1. Ce que cette PR livre

Des fichiers de ressources DNN **zh-CN** pour la famille `App_GlobalResources` — le chrome de
l'interface (boutons, messages, validation, courriels d'authentification). C'est du contenu
**statique de l'admin/du portail**, pas du contenu éditorial : il n'a pas d'impact sur les
cartes du jeu ni sur les PDF.

| Fichier | Clés livrées | Clés du neutre | Reste |
|---|---:|---:|---:|
| `FileUpload.zh-CN.resx` (nouveau) | 4 | 4 | 0 |
| `WebControls.zh-CN.resx` (nouveau) | 50 | 50 | 0 |
| `Exceptions.zh-CN.resx` (nouveau) | 132 | 132 | 0 |
| `GlobalResources.zh-CN.resx` (étendu) | 172 | 182 | 10 |
| `SharedResources.zh-CN.resx` | *non touché* | 617 | 614 |
| `List_Country.zh-CN.resx` | *non créé* | 256 | 256 |
| **Total** | **358** | **1 241** | **880** |

### Pourquoi `SharedResources` n'est pas touché dans cette PR

`ChromeTier1LocalizationContractTests.ScopeIsThirteenKeysAndPartialFilesContainNoExtraKeys`
épingle **exactement 3 clés** dans `SharedResources.{zh-CN,ar-SA,fa-IR}.resx` (« DNN falls back
per key, so these are deliberately partial files »). Étendre ce fichier exige de **faire évoluer
cette garde** — décision à prendre par ai-01, pas par le worker, sur un test écrit par une autre
lane. Le pack `SharedResources` (614 clés restantes) arrive donc dans une PR dédiée, avec
l'adaptation de la garde dans le même diff.

## 2. Méthode — le `.resx` n'est pas recréé

Les `.resx` DNN sont réécrits à la main très facilement et très mal. Ici :

1. on part du fichier **neutre** (`SharedResources.resx`…), on ne remplace que le contenu des
   `<value>` des clés traduites ;
2. l'en-tête boilerplate, le schéma `xsd`, les `resheader`, l'**ordre** des clés, le **BOM UTF-8**
   et les fins de ligne **LF** sont préservés octet pour octet ;
3. une clé **non traduite est absente** du fichier produit ⇒ DNN retombe sur le neutre anglais,
   **clé par clé**. C'est le même contrat que le pack Tier 1 ar/fa/zh déjà en ligne ;
4. les clés **déjà présentes** dans le fichier culturel sont **préservées** — c'est le cas des
   deux textes juridiques `MESSAGE_PORTAL_TERMS` / `MESSAGE_PORTAL_PRIVACY`, déjà traduits en
   chinois par le grain juridique et **arbitrés**. Ils ne sont ni réécrits ni replacés par
   l'anglais.

Script : `docs/dnn-localization/458-item9bis-langpack-build.py` — lecture seule sur le neutre,
écriture des seuls `.resx` de la culture cible. Dictionnaire source de vérité du pack :
`docs/dnn-localization/458-item9bis-zh-CN.translations.json` (358 entrées).

```bash
python -I docs/dnn-localization/458-item9bis-langpack-build.py \
  --lang zh-CN --translations docs/dnn-localization/458-item9bis-zh-CN.translations.json --dry-run
```

## 3. Les garde-fous du constructeur

Le constructeur **refuse d'écrire** et sort en erreur si l'un de ces cas se présente :

| Garde | Pourquoi |
|---|---|
| clé traduite absente du neutre | faute de frappe dans le dictionnaire |
| valeur vide | chaîne perdue |
| **multiset** des placeholders `{0}`/`{1}` différent du neutre | le faux vert le plus probable : une traduction qui « déballe » le placeholder reste verte si l'on ne compare que la **présence** |
| **identité** des jetons DNN différente (`[Portal:URL]`, `[Custom:4\|…]`) | un jeton perdu casse le rendu du courriel ; seule la partie **affichable** se traduit, jamais l'identité |
| XML produit non reparsable / compte de clés inattendu | fichier corrompu livré |
| clé du fichier culturel disparaît | perte silencieuse d'une valeur arbitrée |

La precedence est explicite : **la traduction gagne toujours** ; une clé du fichier culturel
absente du dictionnaire est préservée. L'inverse (un fichier périmé qui écrase une traduction
neuve) est exclu.

### Contrôle inverse — la garde n'est pas décorative

Un dictionnaire volontairement amputé d'un jeton DNN (`[User:DisplayName]` retiré d'un corps de
courriel) est **refusé** avec le jeton fautif nommé. Un dictionnaire amputé d'un `{0}` est
**refusé** avec le multiset attendu et obtenu. Les deux contrôles ont été exécutés.

Deux pièges d'instrument rencontrés en cours de route, et corrigés **dans l'instrument** :

- l'en-tête boilerplate du format `.resx` contient **quatre exemples `<data>` dans un
  commentaire XML** (Name1, Color1, Bitmap1, Icon1). Compter les blocs sans les exclure
  **double** le dénominateur ⇒ le découpage ne commence qu'après le dernier `</resheader>` ;
- une regex de jeton trop large capture la prose entre crochets (`[NOTE: …]`) et fabrique un
  faux échec ⇒ seule l'identité (`namespace` + chemin avant `|`) est comparée.

## 4. Les 10 clés de `GlobalResources` non livrées

Elles sont **absentes** du pack ⇒ DNN sert l'anglais pour ces dix chaînes. Le motif est named,
pas un oubli :

| Clé | Motif |
|---|---|
| `License.Text` | texte de licence MIT : ne se traduit pas |
| `500.Error` | jeton pur `{0}` |
| `DefaultSynonymGroup.Text` | marques (`dnn,dotnetnuke`) |
| `EMAIL_MESSAGING_DISPATCH_ITEM.Text` | coquille HTML + jetons, mesuré **0 prose** |
| `EMAIL_MESSAGING_DISPATCH_BODY.Text` | coquille HTML/CSS d'un courriel |
| `404.Error`, `403_3.Error` | pages d'erreur **hébergeur** (diagnostic d'installation) |
| `DefaultStopwordGroup.Text` | **liste de mots vides** de l'indexeur : la version chinoise est un choix de produit (liste de mots outils), pas une traduction |
| `MESSAGE_PORTAL_TERMS.Text`, `MESSAGE_PORTAL_PRIVACY.Text` | **déjà traduits** (grain juridique) — conservés, pas re-écrits |

## 5. La garde d'intégrité

`Generation/Converters/Argumentum.AssetConverter.Tests/Dnn/DnnZhCnResxIntegrityGuardTests.cs`

Elle ne contrôle pas la **complétion** (le pack est partiel par construction) mais l'**intégrité**
de ce qui est livré :

- toute clé présente existe dans le neutre, et sa valeur est non vide ;
- le **multiset** des placeholders et l'**identité** des jetons DNN sont identiques au neutre ;
- **aucune valeur n'est une copie de l'anglais par accident** — une copie volontaire doit être
  déclarée dans la liste blanche `VerbatimByDesign` (une seule entrée : le sujet du courriel de
  bannière, qui n'est qu'un jeton `[Custom:0]`) ;
- les deux textes juridiques gardent leur valeur chinoise arbitrée et ne sont **pas** replacés
  par l'anglais.

### Les deux gardes existantes que ce pack fait évoluer

| Garde | État avant | Adaptation |
|---|---|---|
| `ChromeTier1LocalizationContractTests.ScopeIsThirteenKeysAndPartialFilesContainNoExtraKeys` | épingle **exactement 3 clés** dans `SharedResources.{zh,ar,fa}.resx` | **non touchée** : ce pack ne modifie pas `SharedResources` (§1) |
| `DnnLegalZhArFaGuardTests.Resx_Carries_Only_The_Two_Legal_Keys_Preserved` | épingle **exactement 2 clés** dans `GlobalResources.{zh,ar,fa}.resx` | assouplie **pour zh-CN seul** : les deux clés juridiques doivent toujours être présentes, mais le fichier zh-CN porte désormais le chrome Tier 2. **ar-SA et fa-IR restent épinglés à 2 clés** — ils ne sont pas touchés par ce PR |

Toutes les autres assertions de ces deux gardes (valeur exacte des 13/2 clés, `xml:space`,
séquence de balises, miroir RTL, multiset de chiffres) **continuent de s'appliquer et passent** —
c'est mesuré, pas supposé : la suite complète tourne avec 0 échec.

## 6. Suite

| Reste | Clés | Note |
|---|---:|---|
| `SharedResources.zh-CN` | 614 | exige l'évolution de `ChromeTier1LocalizationContractTests` (§1) |
| `List_Country.zh-CN` | 256 | 256 codes ISO ; le neutre est une **coquille** (valeurs vides) |
| `GlobalResources.zh-CN` | 10 | §4, à trancher par ai-01 (licence / indexeur / coquilles HTML) |
| `ar-SA`, `fa-IR` | 358 × 2 | même méthode, garde à paramétrer |

## 7. Portée et réversibilité

- **Aucune** valeur de production n'est touchée ; aucun `.resx` de la prod n'est modifié.
- Les fichiers modifiés sont **nouveaux** (3) ou **étendus** (1), tous sous
  `DNNPlatform/App_GlobalResources/`, versionnés, réversibles par un revert du commit.
- Le webroot servi (`D:\Dev\Argumentum`) **reste sur `master`** : ce travail a été fait dans un
  worktree détaché (`D:\A9ZC`), jamais dans le checkout servi.
- Aucune donnée membre, aucun envoi de formulaire, aucune valeur secrète.
