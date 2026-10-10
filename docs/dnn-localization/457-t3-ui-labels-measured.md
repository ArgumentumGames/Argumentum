# #457 T3 — les libellés du site : ce qui est fait, ce qui est un no-op, ce qui reste

**Statut : mesure seule, 0 écriture, 0 appel payé, 0 geste DNN.**

T3 est un **nom** dans le plan, pas un geste. Ce document le convertit en objet mesuré, et
répond à une seule question : *que reste-t-il à faire si l'on dispatch « T3 » aujourd'hui ?*
Réponse mesurée : **rien sur la tranche dépôt, et une tranche DB bloquée sur la même brique
que T2.**

Définition autoritaire (plan `#457 c.6056530803`, amendée `#458 c.6056545021`) :

> **T3 — Libellés** `res.*`, `ui.*` et `GlobalResources.<culture>.resx` par la tâche
> « DNN UI strings » (#487), même boucle.

---

## 1. La tranche dépôt est DÉJÀ livrée — et elle est complète

`docs/dnn-localization/dnn-ui-strings.csv` (11 rangées) porte **2 `ui.*` + 9 `res.*`**,
colonnes `key,context,source_file,fr,en,ru,pt,es,ar,fa,zh,notes`.

Mesure des cellules vides, colonne par colonne :

| `en` | `ru` | `pt` | `es` | `ar` | `fa` | `zh` | `fr` (source) |
|-----:|-----:|-----:|-----:|-----:|-----:|-----:|--------------:|
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

**11 rangées × 7 cibles = 77 cellules, 77 remplies, 0 vide.** L'artefact a été rempli et
mergé (#490), `fr` compris.

## 2. ⭐ Le fait décisif : la tâche telle que configurée est un **no-op strict**

`DatasetUpdaterRootConfig.cs:2635-2695` définit la tâche #487 :

| Champ | Valeur | Conséquence |
|---|---|---|
| `Name` | « Translate DNN UI strings FR to all languages (en/ru/pt/es/ar/fa/zh) empty-only multi » | — |
| `SourceDataset` | `KnownDataSets.DnnUiStrings` | lit `docs/dnn-localization/dnn-ui-strings.csv` |
| `FieldsToUpdate` | `en, ru, pt, es, ar, fa, zh` | 7 cibles |
| **`SelectEmptyTargets`** | **`true`** | **ne remplit que les cellules VIDES** |
| `Enabled` | `false` | — |
| `Model` | `gpt-5.6-sol`, clé `.keys\openai-key.txt` | appel payé |

`SelectEmptyTargets = true` **et** 0 cellule vide (§1) ⇒ la tâche sélectionne **0 rangée à
traiter**. Un dispatch « exécuter #487 sur le corpus actuel » produirait un run payé qui ne
traduit rien, ou plus probablement un run vide — dans les deux cas, **un cycle perdu**.

⚠️ **Le nom de la tâche porte la raison** : « *empty-only multi* ». Elle a été écrite pour
**remplir** un CSV vide, pas pour le réviser. Le corpus a été rempli par une autre voie
(#490), donc la tâche a été **satisfaite par un autre chemin que le sien**. Forme générale,
déjà rencontrée sur ce pool : *une entrée de pool peut précéder sa propre satisfaction, et
l'instrument qui la lit ne le voit pas.*

## 3. Ce qui reste réellement — deux tranches, mesurées

### 3(a) Les **valeurs** `res.*` — DB-only, bloquées

L'extracteur le dit lui-même (`extract_dnn_ui_strings.py`, docstring) :

> « `res.*` — `@Resources.<Key>` references. The **KEY** is in the repo; the canonical FR
> **VALUE** lives in SQL (2sxc App Resources) — **DB-only**. »
> « The bulk of DNN strings (glossary, FAQ, homepage, per-rule content, **the App resource
> VALUES**) is DB-only and requires a portal/2sxc export (jsboige, gated). »

⇒ Cette tranche est **bloquée sur le même export que T2**. Les deux grains T2 et T3
attendent **une seule et même brique** — ce que la file ne dit pas aujourd'hui.

### 3(b) `GlobalResources.<culture>.resx` — nommé par le plan, **non outillé, non mesuré jusqu'ici**

`DNNPlatform/App_GlobalResources/` — **15 fichiers `.resx` (+ `TimeZones.xml`), 2 707 entrées**,
et **une seule culture présente, `fr-FR`** :

| Fichier (base = EN/invariant) | base | `fr-FR` | cultures manquantes |
|---|---:|---:|---|
| `Exceptions` | 132 | 130 | ru pt es ar fa zh |
| `FileUpload` | 4 | 4 | ru pt es ar fa zh |
| `GlobalResources` | 182 | 246 | ru pt es ar fa zh |
| `List_Country` | 256 | 256 | ru pt es ar fa zh |
| `List_BannedPasswords-1` | — | 1 | (fr-FR seul) |
| `List_ProfanityFilter-1` | — | 1 | (fr-FR seul) |
| `Prompt` | 145 | — | ru pt es ar fa zh |
| `SharedResources` | 617 | 628 | ru pt es ar fa zh |
| `WebControls` | 50 | 55 | ru pt es ar fa zh |
| **Base des 6 fichiers à variantes** | **1 241** | — | — |

⇒ Le volume d'une production `<culture>.resx` pour les 6 langues manquantes serait
**1 241 entrées × 6 = 7 446 cellules** — soit **un ordre de grandeur au-dessus du volume de
T2**, que le plan lui-même qualifie de « faible » (« Le volume est mesuré par #1810 et est
faible »).

> **Erratum 2026-10-10** (contre-revue po-2023 c.6087370744, re-mesure à deux instruments).
> La v1 de ce tableau disait « 16 fichiers » (le répertoire en compte 16, mais l'un est
> `TimeZones.xml` : **15 resx**) et « base des 7 fichiers à variantes = 1 386 » — elle
> additionnait `Prompt.resx` (145 entrées), qui n'a **pas** de variante `fr-FR` (la table
> elle-même le montrait). Le découpage juste : **6 fichiers à variantes / 1 241 entrées**.
> Sur le total, les deux instruments se réconcilient exactement : **2 731** en comptage brut
> `<data ` sur les octets (chiffre de la contre-revue) = 2 707 éléments réels **+ 24 fantômes**
> — le commentaire de schéma MSDN présent dans 6 fichiers porte 4 littéraux `<data name=`
> d'exemple chacun (4 × 6 = 24). Ce document compte **après retrait des commentaires**
> (self-test : un bloc commenté contenant `<data name=` n'incrémenté pas le compteur).
> Sur les seuls fichiers à variantes, le brut donne 1 261 = 1 241 + 20 fantômes (5 fichiers
> commentés). À noter enfin : `GlobalResources.fr-FR` (246) **dépasse** sa base (182) — la
> variante française porte 64 clés absentes de la base, donc le volume réel d'une culture
> peut excéder le compte de la base. Ordres de grandeur et routage : **inchangés**.

⚠️ **Trois observations qui ne sont pas des mesures, et qui décident pourtant du grain :**

1. **Ce sont des ressources du *framework* DNN**, pas du contenu Argumentum. Le plan range
   cette couche dans « Réglages et modules non-2sxc — au cas par cas, selon ce que chaque
   module sait faire (titres de page, menus, `resx`) ».
2. **DNN amont publie des paquets de langue officiels.** Les produire au LLM serait une
   seconde source là où il en existe une première — à confronter **avant** d'écrire, pas après.
3. **Le plan ne nomme que `GlobalResources`**, alors que `fr-FR` existe pour **6** familles
   (`Exceptions`, `FileUpload`, `GlobalResources`, `List_Country`, `SharedResources`,
   `WebControls`). Appliqué à la lettre, T3 laisserait ces cinq autres en anglais : la portée
   écrite est **plus étroite que la portée observée**. Même forme que le §8.4 de #1810
   (« 16 gabarits » vs 36 fichiers).

## 4. Un piège de chemin, signalé sans être exercé

`AssetConverterConfig.cs:109` déclare pour ce dataset un `ReleaseFilePath` en **URL absolue
`raw.githubusercontent.com/…/master/…/dnn-ui-strings.csv`**. Exactement le piège documenté
(#1225/#1228, `JsonFilePathRelease`) : **en Release, la tâche lit la version de `master`, pas
l'arbre de travail**. Une correction du CSV sur une branche serait donc **invisible** à un run
Release — le run réussirait, sur l'ancien contenu.

## 5. Routage

| Volet | État | Geste |
|---|---|---|
| `ui.*` / `res.*` (tranche dépôt, 11 rangées) | ✅ **fait** (#490) | **ne pas dispatcher** |
| Tâche #487 sur le corpus actuel | ⛔ **no-op** (`SelectEmptyTargets=true`, 0 vide) | **ne pas exécuter** |
| Valeurs `res.*` (DB-only) | ⏳ bloqué | débloqué par **le même export que T2** |
| `GlobalResources.<culture>.resx` ×6 | ❓ nommé, volume mesuré, **outillage absent** | **dossier d'arbitrage livré le 10/10** : [`457-t3-globalresources-arbitrage-dossier-2026-10-10.md`](457-t3-globalresources-arbitrage-dossier-2026-10-10.md) — composition (5 910 traduisables / 1 536 données), surface référencée quasi nulle côté Argumentum, amont = 8 cultures officielles dont **es et pt**, aucune pour ru/ar/fa/zh. Reco : installs d'amont es + pt, statu quo documenté pour les 4 autres jusqu'à mesure au rendu. GO owner requis pour tout geste |

**Ce que ce document a établi** : T3 n'est pas un grain prêt. Sa tranche dépôt est faite, son
mécanisme est vacant sur le corpus actuel, et sa tranche restante est soit bloquée sur une
brique partagée avec T2, soit une décision de politique (amont vs LLM) qui n'appartient pas à
cette lane.

**Ce que ce document n'a PAS établi** (à ne pas lui faire dire) :

- Que les 6 cultures du framework **doivent** être produites : le besoin n'est pas mesuré — le
  repli de DNN sur l'invariant anglais n'a pas été observé au rendu.
- Que les paquets de langue DNN amont couvrent ces 6 langues, ni à quelle version : **non
  vérifié**.
- Le rendu : aucune page n'a été ouverte, aucune culture activée. La mesure porte sur des
  fichiers, jamais sur un site servi.
- Le compte de 1 241 : il additionne **toutes** les entrées des bases à variante, y compris
  des fichiers dont une partie n'est pas de la prose d'interface (`List_Country` = noms de
  pays, qu'un paquet amont fournit mieux qu'un LLM).

---

## Sources

- `docs/dnn-localization/dnn-ui-strings.csv` — l'artefact #490 (11 rangées)
- `Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/DatasetUpdaterRootConfig.cs:2635-2695` — la tâche #487
- `Generation/Converters/Argumentum.AssetConverter/AssetConverterConfig.cs:105-111` — le dataset et ses deux chemins
- `tools/dnn_i18n/extract_dnn_ui_strings.py` — l'extracteur et sa déclaration DB-only
- `DNNPlatform/App_GlobalResources/` — les 16 `.resx` et leurs variantes de culture
- `docs/dnn-localization/README.md` §6 — contrainte « never touch `dnn-ui-strings.csv` »
  (respectée : ce document ne le modifie pas)
- Plan : `#457 c.6056530803` · amendement : `#458 c.6056545021`
