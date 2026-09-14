# Dossier groupé #682 → #685 — fenêtre owner (portage i18n DNN, étapes 2 à 6)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-14 · **Base** : `origin/master` `769a373d`
**Nature** : mesure, lecture seule. Aucune écriture DB, aucun provisioning, aucun geste d'admin DNN,
aucune traduction, aucun réimport. **Aucun verdict visuel** (réservé ai-01).
**Portée** : grain ⑥ de la deep-queue po-2023 (`#458` c.`5657210653`), après #1187.
**Sources** : export `2ff703f1` (#774) · PR #674 · PR #694 · transferts #681 c.`5657196302` / c.`5657196458`.

---

## §0 — Objet : une seule fenêtre, cinq étapes, et deux décisions à prendre avant d'y entrer

Le portage i18n du site DNN est un enchaînement de 6 étapes (#669) dont 4 restent ouvertes
(#682 · #684 · #683 · #685) plus PR #674. Elles se répartissent en **deux natures de travail** :

- ce qui exige **jsboige** (provisioning 2sxc en base, admin DNN, réimport) — donc une **fenêtre**, avec un
  coût d'entrée fixe ;
- ce qui est **mécanique** (traduction, rebase, vérifications) — donc faisable par un worker **avant** la
  fenêtre, pour que la fenêtre ne serve qu'à ce que seul l'owner peut faire.

Ce dossier ordonne les deux, et il **remontait deux décisions à prendre avant d'entrer** (§3) : entrer dans la
fenêtre sans les avoir tranchées revient à provisionner 49 attributs dont un tiers ne sera pas lu.

> ✅ **État au 15/09** : les deux décisions sont tranchées — **§3.1 `Title` vs `EntityTitle` → `Title`**
> (owner 14/09, #682 c.`5665864947`) · **§3.2 « 49 » → levée par la mesure** (transfert confirmé, manifeste
> à corriger). Ce qui reste ouvert n'est plus une décision mais une **exécution séquencée** : provisionner
> `Title_<lang>` (owner), retargeter #674 (worker), puis cultures/traduction/réimport.

---

## §1 — Graphe de dépendances (mesuré, pas recopié)

```
#681 export ──✅ livré (2ff703f1, app 60, setID 377)
   │
   ├─► #682 [2/6] décision + PROVISIONING 49 attributs suffixés ─┐
   │        └─► PR #674 [runtime pending] validable au runtime ──┤
   │                                                             ├─► #684 [5/6] traduction + RÉIMPORT
   └─► #683 [4/6] cultures DNN + routage + switcher ─────────────┘        │
        (indépendant — parallélisable, aucune dépendance amont)           ▼
                                                                   #685 [6/6] validation visuelle (ai-01)
                                                                          │
                                                                          ▼
                                                              release v2.0.0 couplée au go-live DNN
```

- **#683 est indépendant** : sa propre DoD le dit (« can run in parallel with #A and #B »). Il ne bloque pas
  la traduction, seulement sa **vérification**.
- **#684 est bloqué trois fois** : #682 (les champs), PR #674 (la vue qui les lit), #683 (les cultures servies).
- **#685 est bloqué par #684 + #683**, et il est **ai-01 uniquement**.

---

## §2 — État mesuré des prémisses (re-mesuré, pas reporté)

Toutes les valeurs ci-dessous sont lues dans l'export commité `2ff703f1`
(`docs/dnn-localization/release-validation/exports/DNN-Argumentum-export-2026-07-07/`).

### §2.1 — Le content-type servi

`11-game-rule-schema.json` : `Game Rule`, **attributeSetId 377**, **appId 60**, 15 attributs.

| # | StaticName | Type | |
|---:|---|---|---|
| 0 | `Parent` | Entity | |
| 1 | **`Title`** | String | **`IsTitle = true`** |
| 2 | `Summary` | String | prose |
| 3 | `Material` | String | prose |
| 4 | `MinNbPlayers` | Number | |
| 5 | `MaxNbPlayers` | Number | |
| 6 | `Installation` | String | prose |
| 7 | `Content` | String | prose |
| 8 | `Variants` | String | prose |
| 9 | `Memo` | String | prose |
| 10 | `Date` | DateTime | |
| 11 | `Author` | Entity | |
| 12 | `Licence` | Entity | |
| 13 | `Original` | Entity | |
| 14 | `UrlKey` | String | slug |

⇒ L'attribut de titre s'appelle **`Title`**. `EntityTitle` **n'est pas un attribut** (vérifié : absent de la
liste des 15). C'est un **alias 2sxc** — le code livré sur master utilise `@ruleEntity.EntityTitle`, ce qui
fonctionne au runtime, mais ce n'est pas un nom provisionnable.

### §2.2 — Le contenu à traduire : 5 entités, 28/35 cellules

`12-game-rule-content-items.json` : **5 entités** publiées.

| eid | `Title` (FR canonique) | Summary | Material | Installation | Content | Variants | Memo |
|---:|---|:-:|:-:|:-:|:-:|:-:|:-:|
| 11378 | L'école des menteurs | X | X | X | X | X | X |
| 11380 | Le Bingo mixologie argumentative | X | X | X | X | . | . |
| 11387 | Le dernier beau parleur | X | X | X | X | . | . |
| 11388 | Le moulin à baratin | X | X | X | X | X | . |
| 11389 | La parlote coinchée | X | X | X | X | . | . |

| attribut | remplies |
|---|---|
| Title | 5/5 |
| Summary | 5/5 |
| Material | 5/5 |
| Installation | 5/5 |
| Content | 5/5 |
| Variants | 2/5 |
| Memo | 1/5 |
| **total** | **28/35** — **7 vides** |

**Le « 23/30 » du transfert est exact, sur un autre ensemble** : 30 = 5 entités × 6 champs **hors `Title`**,
et 23 = 28 − 5. Les deux chiffres se réconcilient ; ils ne se contredisent pas. Idem le volume de traduction
annoncé « 161–196 unités » = **23 × 7 = 161** (hors titres) à **28 × 7 = 196** (titres inclus).

⇒ **Les 5 règles du site sont 5 des 15 cartes Rules du CSV** (`Rules_01`, `_07`, `_09`, `_11`, `_13` — les
variantes de jeu). C'est l'illustration mesurée de #669 §2c : **le contenu « Règles » du site n'est pas le CSV
Rules**. Les 8 traductions existantes du CSV Rules **ne sont pas réutilisables** telles quelles.

### §2.3 — Le rail `res.*` : 11 clés, 0 traduite

`13-app60-resources.csv` : **11 clés** (`fr` rempli, **`en`…`zh` vides**) — 9 `res.Rule*` + `Author` +
`Licence`.

```
RuleContent · RuleInstallation · RuleMaterial · RuleMemoCard · RuleMemoCardDownload
RuleMemoCardFileNamePrefix · RuleMemoInstructions · RuleSummary · RuleVariants   (9 « res.Rule* »)
Author · Licence                                                                  (2 génériques)
```

⇒ **63 cellules** (9 × 7) sur le rail `res.Rule*`, plus 2 clés génériques. `RuleMemoInstructions` en fait
partie — #490 l'avait laissé de côté, #684 le réclame explicitement.

### §2.4 — PR #674

| | |
|---|---|
| état | **OPEN**, `[runtime pending]` |
| `mergeStateStatus` | **`DIRTY`** |
| base | `a41cbda6` — **474 commits** en retard sur `origin/master` |
| conflit | **1 fichier** : `_RulesExplorer_RuleDetail.cshtml` — ⚠️ **la propriété vraie n'est pas « le seul fichier de vue qu'elle modifie »** (#674 modifie **aussi** `_RuleList.cshtml`) **mais « le seul fichier de vue que `master` a touché depuis la merge-base »** — c'est *cette* propriété qui en fait le seul fichier en conflit |
| dernier changement master sur ce fichier | `f34ac77c` (#772, deltas `res.Rule*`) |

⚠️ **La PR modifie exactement le fichier que #772 a modifié sur master.** Le conflit n'est donc pas
accidentel : les deux touchent la même zone. Un rebase est **requis avant** la fenêtre, et il n'est pas
trivial (474 commits).

---

## §3 — Les deux décisions à prendre AVANT d'entrer dans la fenêtre — **état au 15/09 : §3.1 tranchée, §3.2 levée**

### §3.1 — ✅ `EntityTitle` ou `Title` ? — **TRANCHÉ le 14/09 : l'attribut réel `Title`**

> ✅ **Décision owner du 14/09** (#682 c.`5665864947`, reçue interactive, relais ai-01) : **utiliser
> l'attribut réel `Title` et retargeter la cascade vers `Title_<lang>`**. Conséquences actées :
> **aucun second attribut `EntityTitle`** · provisionner les variantes localisées de **`Title`** sur le
> content-type setID 377 selon le périmètre validé · **corriger #674** pour appeler la cascade sur `Title`
> en conservant l'ordre **`Title_<lang> → Title_en → Title_fr → Title`** · rebaser #674 en résolvant
> **délibérément** le conflit `_RulesExplorer_RuleDetail.cshtml` avec les apports de #772 (aucun force-push
> sur branche partagée) · **aucune fenêtre DB/runtime** avant dossier corrigé, dry-run et gates UAC.

Trois artefacts, deux noms :

| artefact | nom employé |
|---|---|
| **schéma mesuré** (export `11-game-rule-schema.json`, setID 377) | **`Title`** (`IsTitle = true`) |
| **entrée de décision** (#682 → PR #694) — liste des 7 champs à suffixer | **`EntityTitle`** |
| **PR #674**, code de la vue | `Loc(ruleEntity, "EntityTitle")` |

La cascade `Loc()` construit `field + "_" + lang`, puis `field + "_en"`, puis `field + "_fr"`, puis `field`.
Avec `field = "EntityTitle"`, elle cherche donc `EntityTitle_en` — **un nom qui ne peut pas exister comme
attribut suffixé**, puisque l'attribut s'appelle `Title`.

**Deux issues, et elles ne sont pas équivalentes :**

- **(A) Provisionner `Title_en` …** (le nom réel de l'attribut) ⇒ la vue **ne trouvera jamais** ces valeurs :
  `Loc(ruleEntity, "EntityTitle")` n'a aucune raison de lire `Title_en`. **Le titre serait le seul champ à ne
  jamais se localiser** — et il se localiserait silencieusement en FR, **parce que la vue retomberait sur l'alias**.
  ⚠️ **Cette retombée silencieuse est conditionnelle, et la condition n'est pas mesurée** : elle suppose que
  l'alias `EntityTitle` soit **exposé dans le dictionnaire** que `Loc()` interroge — c'est précisément ce que le
  §7 déclare **non mesurable** depuis le dépôt. Si l'alias n'y est **pas** exposé, il n'y a pas de retombée :
  **le titre n'est pas « localisé en FR », il est vide.** Les deux issues de (A) divergent donc, et seule la
  validation runtime de l'étape 2 les sépare. *(En revanche, la recommandation de retargeter le code sur `Title`
  ne dépend pas de cette inconnue : elle reste la plus robuste dans les deux cas.)*
- **(B) Provisionner `EntityTitle_en` …** ⇒ il faut créer un attribut **neuf** nommé `EntityTitle` en plus de
  `Title`, et le titre `IsTitle` reste FR-only. La traduction s'affiche, mais deux champs de titre coexistent.

⇒ **Décision actée le 14/09 — c'est le retarget sur `Title` qui est choisi** : la cible du code est renommée
en `Title` (l'attribut réel, dont `UrlKey`/`IsTitle` sont déjà cohérents), la cascade devient
**`Title_<lang> → Title_en → Title_fr → Title`**, et **(B) est écarté** — aucun second `EntityTitle`.
L'inconnue de l'option (A) — l'alias `EntityTitle` est-il exposé dans le dictionnaire ? — est rendue **sans
objet** : le code livré ne lira plus jamais `EntityTitle`, donc que l'alias résolve ou non n'a plus d'effet.
Le risque « 7 attributs morts par langue, ou un titre qui ne se traduit jamais » est levé par construction :
on provisionne **et** on lit **le même nom**.

### §3.2 — ✅ « 49 » : contradiction levée par la mesure

Le manifeste d'export porte un avertissement (`caveat_fieldCount`) affirmant que « 49 » désigne
probablement les ressources `res.*`. Le transfert #681 affirme l'inverse : « 49 = 7 champs × 7 langues, pas
les 9 clés `res.*` ».

**Mesure** : le rail `res.*` compte **9 clés `Rule*`** (§2.3) — et **non 49**. **Le transfert a raison, le
manifeste a tort.** Les deux rails sont réels et distincts :

| rail | volume | nature |
|---|---:|---|
| attributs suffixés du content-type | **49** = 7 champs × 7 langues | provisioning 2sxc (schema) |
| clés `res.Rule*` | **9** clés (63 cellules) | valeurs de ressources (traduction) |

⇒ À corriger dans le manifeste d'export pour que le prochain lecteur ne reprenne pas le caveat.

---

## §4 — La fenêtre owner, dans l'ordre

Chaque étape porte sa **vérification** : c'est elle qui autorise à passer à la suivante.

### Étape 1 — #682 : ratifier et provisionner · **jsboige** (2sxc, base `ArgumentumGames`)

1. **Trancher §3.1** (`Title` vs `EntityTitle`) — ✅ **fait le 14/09** (#682 c.`5665864947`) : **`Title`**.
2. **Provisionner les 49 attributs suffixés** sur le content-type `Game Rule` **setID 377, app 60** —
   `{champ}_{en,ru,pt,es,ar,fa,zh}` pour les 7 champs prose retenus, **le titre provisionné étant
   `Title_<lang>`** (jamais `EntityTitle_<lang>` — décision owner).
3. **Vérification** : le content-type passe de 15 à 64 attributs ; un `Get` sur une entité rend les 49 clés.
4. **Ne toucher aucun setID homonyme** (210, 231) — ⚠️ **RAPPORTÉ depuis le transfert owner, non dérivé de
   l'export committé** : le seul setID que l'export du dépôt permet d'identifier comme servi est **377**. Que
   210/231 existent *et* soient homonymes est une information de la fenêtre, pas de la mesure.

### Étape 2 — PR #674 : rebase puis validation runtime · **worker (rebase) + jsboige (runtime)**

1. **Rebaser** la branche `fix/649-rules-explorer-i18n-loc` sur `origin/master` (474 commits ; **1 fichier en
   conflit**).
2. **Aligner le nom de champ** sur la décision §3.1 — **actée** : cascade sur **`Title_<lang> → Title_en →
   Title_fr → Title`**, zéro `EntityTitle` résiduel, apports #772 préservés dans la résolution du conflit
   (owner : résolution **délibérée des deux côtés**, aucun force-push sur branche partagée).
3. **Validation runtime minimale** (celle que #682 prescrit) : écrire **une** valeur
   (`Summary_en` = « test »), rendre `RuleDetail` en `?language=en-US`, confirmer que `Loc()` la sert.
   ⇒ C'est **le seul contrôle qui lève `[runtime pending]`** ; il ne peut pas être remplacé par une lecture de
   code. *(La question d'origine — « la clé `EntityTitle` existe-t-elle dans le dictionnaire 2sxc ? » — est
   **sans objet depuis la décision** : le code ne la lira plus. Le contrôle reste requis pour `Title_en` et
   la cascade retargetée.)*

### Étape 3 — #683 : cultures, routage, switcher · **jsboige** (DNN Admin) — **parallélisable**

Indépendant de #682 et #674 : peut être fait dans la même fenêtre, dans n'importe quel ordre.

1. Activer les **7 cultures** (en, ru, pt, es, ar, fa, zh) — `fr` est déjà canonique.
2. **Routage** : `/en-US/Règles` doit rendre le contenu EN, sans 404, avec repli propre là où rien n'est traduit.
3. **`<html lang>`** reflète la culture servie ; **`dir="rtl"`** pour `ar`/`fa`.
4. **Switcher** peuplé et cliquable (`<div class="language">` livré par #490, aujourd'hui vide) — le changement
   de culture **ne doit pas perdre la page**.
5. **Vérification** : `/en-US/...` rend 200 ; `<html lang="en-US">` ; le switcher change de culture et reste
   sur la même page.

### Étape 4 — #684 : traduire puis réimporter · **worker (traduction) + jsboige (réimport DB)**

1. **Traduction** — 7 langues × (23 à 28 cellules de prose) = **161 à 196 unités**, plus **9 clés `res.Rule*`**
   × 7 = **63 cellules**. Harness chunké type `DatasetUpdater` (patron #490).
2. **Périmètre strict** : app **60**, setID **377** uniquement. **Ne pas retraduire 210/231** (setIDs
   **rapportés** par le transfert owner — cf. §4 étape 1, point 4 : ils ne sont pas établis par l'export du dépôt).
3. **Réimport** — l'export **ne porte pas de `ValueId`**. Path A (attributs suffixés) ne l'exige pas ; **toute
   écriture par dimensions EAV l'exigerait** ⇒ si on écrit par dimensions, **ré-exporter d'abord une adresse
   réinjectable**.
4. ⚠️ **L'export date du 07/07/2026** et décrit un état **pré-migration**. Si le FR canonique a bougé depuis,
   **nommer la base cible et re-exporter** avant de réimporter.
5. **Vérification** : parité de nombre d'entités, aucune perte de champ, 0 cellule contaminée en FR.

### Étape 5 — #685 : validation visuelle · **ai-01 uniquement**

8 langues (fr baseline + 7) · RTL `ar`/`fa` (direction, miroir de mise en page) · CJK `zh` (absence de tofu) ·
cyrillique `ru` · **détection de fuite FR** (garde #216). Le switcher doit fonctionner **par page**.

---

## §5 — Ce qu'un worker peut avancer AVANT la fenêtre

| travail | état | gate |
|---|---|---|
| Rebase de PR #674 | faisable | conflit d'1 fichier, 474 commits — **mais bloqué par §3.1** (le nom de champ) |
| Harness de traduction #684 (chunking, prompts, vérif cellule à cellule) | faisable | la **liste exacte des 7 champs** dépend de §3.1 |
| Inventaire des cellules à traduire | ✅ **fait ici** (§2.2, §2.3) | — |
| Correction du caveat du manifeste (§3.2) | faisable, 1 ligne | — |

---

## §6 — Contrôles

| contrôle | énoncé | résultat |
|---|---|---|
| **Cap 0** | les chiffres viennent de l'export commité, pas d'un résumé | les 4 fichiers JSON/CSV de `2ff703f1` ont été ouverts et parsés ✅ |
| **positif** | les comptes doivent se réconcilier avec les transferts | 5 entités ✅ · 7 cellules vides ✅ · `161–196` = 23–28 × 7 ✅ · `9` clés `Rule*` ✅ |
| **négatif** | une contradiction doit être détectable | manifeste vs transfert sur « 49 » : **tranchée par la mesure** (9 ≠ 49) ✅ |
| **nom de champ** | un nom employé doit exister dans le schéma | `EntityTitle` **absent** des 15 attributs ⇒ §3.1 est fondé ✅ |
| **conflit** | l'affirmation `DIRTY` doit être reproduite | `git merge-tree` : **1 fichier**, `_RulesExplorer_RuleDetail.cshtml` ✅ |

---

## §7 — Ce que je n'établis PAS

- **Que `EntityTitle` échoue au runtime.** Je constate qu'il n'est pas un attribut provisionnable et que la
  cascade cherchera `EntityTitle_<lang>` ; **le comportement du dictionnaire 2sxc face à l'alias n'est pas
  mesurable en lecture** — c'est exactement ce que `[runtime pending]` désigne, et l'étape 2 le tranche.
  *(Depuis la décision du 14/09, cette inconnue est **sans objet** pour le code livré — le retarget sur
  `Title` fait qu'aucun chemin n'exécute plus `EntityTitle`. Elle reste consignée : si un jour le retarget
  est annulé, elle redevient vivante.)*
- **Quels sont les types 2sxc exacts des 49 attributs.** L'inférence « tous String » est de haute confiance
  (`@Html.Raw`), pas une mesure ; #687 reste le débloqueur sysadmin.
- **Que l'export reflète l'état courant du FR.** Il est daté du **07/07/2026** (§4 étape 4.4).
- **L'existence et l'homonymie des setIDs 210/231.** Elles sont **RAPPORTÉES** par le transfert owner, non
  dérivées de l'export committé — qui n'identifie que **377**. Le garde-fou « ne pas toucher 210/231 » reste
  justifié (il est peu coûteux et prudent) mais son **antécédent n'est pas mesuré ici**.
- **Aucun verdict visuel**, aucune écriture DB, aucun provisioning, aucune traduction, aucun réimport.

---

*master `769a373d` · export `2ff703f1` (#774) · lecture seule : parsing JSON/CSV, `git merge-tree`,
`git grep` sur `origin/master` · ⛔ aucune écriture DB, aucun geste admin DNN, aucun réimport, aucun verdict
visuel · décisions DB et admin : jsboige · verdict visuel : ai-01.*
