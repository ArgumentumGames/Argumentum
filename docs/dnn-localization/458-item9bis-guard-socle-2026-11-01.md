# #458 grain module-level — socle des gardes `*ResxGuardTests` (2026-11-01)

Traite le volet « socle des gardes » du pool #458 **et** les deux notes 🟡 de la
revue NanoClaw sur #2105 (c. de revue du 2026-10-31).

## Le problème, mesuré

`CommittedBlob` — le seul lecteur `git cat-file -p HEAD:<path>` du dépôt — était
**copié à l'identique dans 14 fichiers de test**, aux retours à la ligne et à
l'accentuation près (`echoue` vs `échoué`). Chaque copie portait le même défaut :

```csharp
process.WaitForExit(30_000);          // ← valeur de retour ignorée
if (process.ExitCode != 0) ...         // ← lève sur un processus ENCORE VIVANT
```

Si git dépassait 30 s, `ExitCode` lève une `InvalidOperationException`
générique (« Process must exit before requested information can be determined »)
qui ne dit **ni l'outil, ni le chemin, ni la cause** — et le processus reste
orphelin. Sur un `cat-file` local, l'incident est rare ; il n'en est pas moins
la seule ligne de diagnostic disponible le jour où un `index.lock` bloque.

14 copies = 14 corrections le jour où l'une se révèle fausse. C'est la
définition d'un socle manquant.

## Ce que le lot livre

| Fichier | Rôle |
|---|---|
| `Dnn/DnnResxGuardSocle.cs` | la primitive, une seule fois |
| `Dnn/DnnResxGuardSocleTests.cs` | 4 faits qui mesurent le socle lui-même |
| 14 gardes migrées | `private/protected static byte[] CommittedBlob(string rel) => DnnResxGuardSocle.CommittedBlob(rel);` |

Diff : **−298 / +14** lignes sur les 14 gardes (le corps disparaît, la
délégation d'une ligne reste pour que 100 % des appels existants compilent sans
être touchés). Aucun appelant modifié, aucun test modifié — sauf le fait de
comptage ci-dessous, qui est précisément la note 🟡 n° 2.

### Le défaut corrigé

- `WaitForExit` **testé** : au dépassement, on tue l'arbre de processus et on
  lève un message qui nomme la commande, le chemin, la durée et la cause
  probable (verrou d'index, lecteur réseau lent).
- `ExitCode != 0` enrichi : le message dit **« committer AVANT de mesurer »** —
  le cas le plus fréquent en pratique, celui d'une garde lancée sur un ajout non
  committé, qui rendait sinon un exit 128 opaque.
- Git absent du PATH : `InvalidOperationException` explicite plutôt qu'un
  `Win32Exception` brut.
- Chemin vide refusé **avant** d'appeler git (`HEAD:` désignerait un objet
  ambigu).

## Les deux notes 🟡 de #2105

| Note | Traitement |
|---|---|
| `WaitForExit` ignore sa valeur de retour | corrigée **une fois**, dans le socle — les 14 gardes en héritent |
| « HUIT répertoires » vivait en prose, rien ne le pinçait | `Covered.Select(c => c.Dir).Distinct().Should().HaveCount(8)` dans `DnnModuleLevelDtmModulesZhResxGuardTests.TheLotCovers…` — une entrée vers un 9ᵉ répertoire rougit désormais |

La seconde était la plus sérieuse des deux malgré son étiquette « non
bloquant » : le compte de fichiers (27) et de clés (668) étaient épinglés, donc
une 28ᵉ entrée **dans un répertoire existant** aurait rougi — mais la même
entrée dans un répertoire neuf passait tous les tests. Le seul angle mort était
celui d'un lot suivant, c'est-à-dire l'usage normal de cette garde.

## Le socle est mesuré, pas supposé

Un socle non mesuré est une hypothèse partagée par 14 gardes. Le piège propre à
ceux-ci : un socle qui rendrait un **tableau vide** ferait passer « 0 CR
trouvé » et « BOM suit le neutre » sur zéro octet — un vert par vacuité, la
famille exacte de #3174 / `feedback-zero-calibrated-on-neighbour-corpus`.

`DnnResxGuardSocleTests` ferme les quatre portes :

1. `TheSocleReturnsTheRealBytesOfACommittedFile` — un fichier connu rend
   **> 500 octets**, avec `<root>` et `text/microsoft-resx` : le socle lit bien
   le fichier demandé, et pas un objet vide.
2. `TheSocleNamesTheCauseWhenThePathIsNotInHead` — un chemin absent de HEAD
   lève, et le message contient `git cat-file` **et** le chemin.
3. `TheSocleRefusesAnEmptyPath` — un chemin blanc lève `ArgumentException`.
4. `TheTimeoutIsShortEnoughToFailFastAndLongEnoughForARealRepo` — 30 s épinglé :
   changer la valeur est une décision.

## Mesures

| Instrument | Résultat |
|---|---|
| Build | 0 avertissement / 0 erreur |
| `DnnResxGuardSocleTests` | **4/4** |
| Suite complète (branche socle, master `c174d5f6` + ce lot) | **3 804 = 3 799 / 0 / 5** |
| Gardes migrées | **14** (−298 / +14) |

Le total se vérifie par arithmétique : master `c174d5f6` porte 3 800 tests (les
3 927 mesurés avec le lot Admin fa-IR de #2111 moins ses 127), et ce lot ajoute
les 4 faits du socle → **3 804**. Les 5 ignorés (6 sur ai-01) restent **locaux à
la machine** (conditionnels `python`/index git).

Aucun des 14 fichiers migrés ne change de comportement : la délégation rend
exactement les mêmes octets, et la suite complète reste verte là où elle l'était.

## Limite déclarée

`DnnModuleLevelDtmAdminFaResxGuardTests` — la garde du lot Admin fa-IR, qui vit
dans **#2111**, pas ici — porte encore sa copie locale : au moment d'ouvrir ce
lot, #2111 était en CI et réécrire sa branche l'aurait rendu non mergeable sans
nécessité. La conversion est un suivi d'une ligne, à faire **après** le merge de
#2111. Déclaré ici pour que la 15ᵉ copie ne se perde pas dans un angle mort —
c'est exactement le défaut que ce lot répare.
