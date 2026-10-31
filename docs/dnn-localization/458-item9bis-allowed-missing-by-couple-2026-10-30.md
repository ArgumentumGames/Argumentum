# #458 grain ⑨-bis — l'ensemble manquant déclaré passe de « par fichier » à « par couple (fichier, culture) »

**Date :** 2026-10-30 · **Lane :** po-2023 · **Nature :** garde de test seulement — **aucun `.resx` touché**
**Fichier :** `Generation/Converters/Argumentum.AssetConverter.Tests/Dnn/DnnAgrCoreResxIntegrityGuardTests.cs`
**Origine :** handing-off de myia-web2 sur la contre-revue de #2084 — *« `AllowedMissingByStem` à passer en
(stem, culture), avant que les lots ne divergent »*.

---

## 1. Le défaut, tel qu'il a été nommé

La garde issue de la contre-revue web2 sur #2079 déclarait l'ensemble des clés autorisées à manquer
**par fichier** :

```csharp
private static readonly Dictionary<string, string[]> AllowedMissingByStem = …
```

C'était juste **aujourd'hui** : le neutre est un fichier unique, partagé par les trois cultures, donc
les causes de manque (sept clés à valeur vide, dix blocs volontairement non traduits) sont les mêmes
pour toutes. Les trois cultures livrent d'ailleurs exactement le même ensemble manquant, sur les cinq
fichiers.

Le défaut n'est pas dans ce qui est mesuré, il est dans **ce que la garde pourra dire au lot suivant**.
Le jour où une culture garde légitimement une clé non traduite alors qu'une autre la traduit, la garde
n'a que deux issues :

- **rougir** alors que le livrable est correct — et le réflexe sera alors de retirer la clé de la
  déclaration commune, ce qui **élargit le trou pour les trois cultures** ;
- ou être **élargie silencieusement** pour faire passer le lot, avec le même effet.

Dans les deux cas, une décision prise pour **une** culture est appliquée aux trois, sans que rien ne
le dise. C'est la famille de défaut que la lane a déjà payée ailleurs : ⭐ *un défaut décrit ce qui est
possible, pas ce qui est pratiqué* — et une déclaration partagée décrit ce qui est **toléré**, pas ce
qui est **justifié**.

## 2. Ce qui change

### 2.1 Deux déclarations, une résolution

| Rôle | Symbole | Portée |
|---|---|---|
| Défaut | `AllowedMissingDefaultByStem` | par **fichier** — le neutre est commun, la déclarer cinq fois la ferait dériver |
| Dérogation | `AllowedMissingByFileAndCulture` | par **couple (fichier, culture)** — **vide aujourd'hui**, et c'est un état, pas un oubli |
| Résolution | `AllowedMissing(stem, culture)` | dérogation si déclarée, sinon défaut du fichier, sinon **exception explicite** |

La garde ne lit plus le dictionnaire par indexation directe : elle passe **par la résolution**. Une
dérogation déclarée ne peut donc pas rester sans effet — c'est exactement le mode d'échec qu'une
déclaration à deux niveaux produit quand on oublie de brancher le second.

Le défaut reste déclaré **une seule fois** parce que la duplication est elle-même un risque mesuré
dans ce dépôt : les huit jetons du pack `SharedResources` sont écrits une fois et concaténés par
culture pour cette raison précise (lot 5). Le couple est la **clé de résolution**, le fichier reste
l'unité de déclaration tant que rien ne diverge.

### 2.2 Le message d'échec nomme le périmètre appliqué

Un message qui dit seulement « l'ensemble a changé » envoie corriger le mauvais endroit quand une
dérogation est en cause. Il distingue désormais :

```text
… mesuré contre la dérogation déclarée pour le couple (FileUpload, zh-CN). …
… mesuré contre le défaut du fichier « FileUpload » (aucune dérogation pour zh-CN). …
```

### 2.3 Trois gardes de la déclaration elle-même

Une déclaration à deux niveaux crée deux façons de mentir, qui n'existaient pas avant :

| Test | Ce qu'il interdit |
|---|---|
| `TheMissingSetDeclarationCoversExactlyTheGuardedFiles` | un fichier couvert **sans** défaut (résolution en exception au milieu de vingt tests) et un défaut **sans** fichier couvert (déclaration morte) |
| `EveryOverrideIsLive` | une dérogation qui vise un couple **non couvert**, ou qui **recopie** son défaut — elle ne change aucun test et donne l'illusion d'un cas particulier traité |
| `OverrideWinsOverTheFileDefault` | la **précédence** elle-même, prouvée sur des dictionnaires **locaux** |

Le troisième mérite un mot : la précédence ne pouvait pas être prouvée sur l'état réel, puisque la
dérogation est vide et que chaque couple couvert a un défaut valide — **tous** les tests resteraient
verts si `Resolve` renvoyait le défaut avant la dérogation. La résolution est donc extraite en
fonction **pure** (`Resolve(overrides, defaults, stem, culture)`) et testée avec des dictionnaires
jetables, sans muter un état statique partagé (les tests d'une même collection s'exécutent en
parallèle).

## 3. Contrôles inverses — la garde peut rougir, et rougit pour la bonne raison

Un vert ne prouve rien sans son rouge. Deux mutations temporaires ont été appliquées au fichier, puis
**restaurées par copie de sauvegarde** (jamais `git checkout --`, qui restaure depuis l'index et efface
tout le non-committé) — `sha256` identique avant et après : `62BADC87…36B0`.

| Mutation | Attendu | Mesuré |
|---|---|---|
| Dérogation **identique** au défaut (`FileUpload`, `zh-CN`) | `EveryOverrideIsLive` rouge, **lui seul** | ✅ 1 échec / 50 réussites, sur `EveryOverrideIsLive` |
| Dérogation **divergente** (`Cle.Qui.NExiste.Pas`) | la dérogation est **appliquée** ⇒ `TheMissingKeysAreExactlyTheDeclaredOnes` rouge **et** le message nomme le couple | ✅ 1 échec / 50, message : *« mesuré contre la dérogation déclarée pour le couple (FileUpload, zh-CN) »* |

Le second contrôle est le plus important : il prouve que le chemin de dérogation est **vivant** dans
le test réel, et pas seulement dans le test unitaire de `Resolve`.

## 4. Ce que cette PR ne fait pas

- **Aucun `.resx` touché**, aucun dictionnaire de traduction, aucun contenu de pack.
- **Aucune dérogation déclarée** : en ajouter une sans divergence réelle serait précisément la
  déclaration morte que `EveryOverrideIsLive` refuse.
- **Rien côté préprod** : c'est une garde de test.

## 5. Ce qui reste fragile, et comment cela se verra

- La **cause** du manque reste décrite en commentaire (sept clés à neutre vide ; dix blocs non
  traduits), pas assertée par classe. Une clé à neutre **non vide** déclarée manquante est un choix
  éditorial ; une clé à neutre **vide** est structurelle. Les deux sont aujourd'hui dans le même sac
  et la distinction ne vit que dans les commentaires : le jour où l'un des dix blocs de
  `GlobalResources` devra être traduit, retirer sa clé de la déclaration sera un geste à la main, sans
  que la garde dise lequel des deux motifs s'applique. Piste nommée, non ouverte ici : séparer les
  deux ensembles (`EmptyNeutral` / `DeliberatelyUntranslated`).
- Le défaut de fichier reste la déclaration par défaut : une culture qui diverge **doit** passer par
  `AllowedMissingByFileAndCulture`, mais rien n'empêche un lot futur d'élargir directement le défaut
  et d'affecter les trois cultures d'un coup. La garde ne peut pas l'interdire — elle peut seulement
  rendre le chemin étroit explicite, ce qui est fait ici.
