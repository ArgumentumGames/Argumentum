# #682 → #685 — dry-run de la fenêtre owner (provisioning `Title_<lang>` + contrôle runtime)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-15 · **Base** : `origin/master` `5bdbdfd1`
**Nature** : **préparation, lecture seule.** Zéro écriture DB, zéro geste admin, **zéro UAC**, zéro réimport,
zéro traduction. **Aucun verdict visuel** (réservé ai-01).
**Portée** : grain ② du pool po-2023 (`#458` c.`5666259809`).
**Consomme** : dossier groupé [`dossier-groupe-682-685-fenetre-owner.md`](dossier-groupe-682-685-fenetre-owner.md)
(#1373, mergé) · PR #674 mergée (`723c52c5`) · décision owner #682 c.`5665864947` (14/09).

---

## §0 — Ce que ce dry-run est, et ce qu'il n'est pas

Le code (#674) et le dossier (#1373) sont mergés : **il ne reste que la fenêtre**. Ce document ne la remplace
pas — il la **rend exécutable**, en transformant « provisionner 49 attributs suffixés » en une **liste
nommée, ordonnée, vérifiable**, et en donnant à la fenêtre un **critère de sortie mesuré** plutôt qu'une
impression de réussite.

Ce qu'il **n'est pas** : un bundle 2sxc prêt à importer. L'export committé
(`11-game-rule-schema.json`) est un **inventaire de lecture** (`contentType`, `attributeSetId`, `appId`,
`staticName`, attributs avec `SortOrder`/`AttributeID`/`StaticName`/`Group`/`IsTitle`/`Type`) — **ce n'est pas
un format d'import**. Fabriquer un bundle dans un format que nous n'avons jamais vu serait exactement le
défaut que le dossier §7 refuse. **Le chemin de provisioning (UI 2sxc / import de content-type / SQL) reste
l'arbitrage de l'owner** ; ce dry-run lui donne l'entrée, pas la mécanique.

---

## §1 — L'instrument, et ses deux moitiés

`tools/dnn-682-schema-delta.py` — **lecture seule** : n'ouvre aucune base, n'écrit aucun schéma. Il lit un
inventaire au format d'export et calcule le delta entre le schéma mesuré et le plan de 49 attributs.

Le **même** instrument sert **avant** la fenêtre (plan) et **après** (vérification), ce qui fait du critère
de sortie une mesure et non une affirmation.

| Exécution | Commande | Résultat mesuré |
|---|---|---|
| état **pré-fenêtre** | `--expect 49` | **49 manquants** sur 15 attributs · exit **0** |
| état **post-fenêtre** | `--expect 0` | non atteignable aujourd'hui : **49 manquants** → `ECART`, exit **1** ✅ témoin |
| self-test (contrôles) | `--self-test` | **7/7 VERT** |

**Self-test (contrôles de mutation, schéma synthétique en mémoire — aucun fixture sur disque, aucune base)** :

| Contrôle | Énoncé | Résultat |
|---|---|---|
| 0 | état pré-fenêtre : 15 attributs → 49 manquants | ✅ |
| 1 | état post-fenêtre : 64 attributs → 0 manquant, gardes vertes | ✅ |
| 2 | retirer `Memo_zh` → **1** manquant (l'instrument discrimine) | ✅ |
| 3 | injecter `EntityTitle_en` → **garde rouge** (option B écartée par l'owner) | ✅ |
| 4 | injecter `UrlKey_en` (hors des 7 champs) → **garde rouge** | ✅ |
| 5 | injecter un second `IsTitle` → **garde rouge** | ✅ |

Le contrôle 2 est celui qui compte : un instrument qui rendrait « 49 » quoi qu'il arrive, ou « 0 » quoi qu'il
arrive, passerait les contrôles 0 et 1. C'est la mutation d'**un seul** attribut qui prouve qu'il mesure.

---

## §2 — Le delta : 49 attributs, nommés et ordonnés

`7 champs prose × 7 langues cibles` = **49**. Les 7 champs sont ceux du content-type mesuré qui portent de la
prose ; les 8 autres attributs ne sont pas localisables (`Parent`/`Author`/`Licence`/`Original` = Entity,
`MinNbPlayers`/`MaxNbPlayers` = Number, `Date` = DateTime, `UrlKey` = slug). `fr` est la valeur **canonique
non suffixée** déjà en base.

| SortOrder | StaticName | champ |
|---:|---|---|
| 15–21 | `Title_{en,ru,pt,es,ar,fa,zh}` | **`Title`** (l'attribut réel, décision owner) |
| 22–28 | `Summary_{…}` | `Summary` |
| 29–35 | `Material_{…}` | `Material` |
| 36–42 | `Installation_{…}` | `Installation` |
| 43–49 | `Content_{…}` | `Content` |
| 50–56 | `Variants_{…}` | `Variants` |
| 57–63 | `Memo_{…}` | `Memo` |

La liste complète, nom par nom et dans l'ordre, sort de l'instrument (§1) : elle n'est pas recopiée ici pour
qu'il n'existe pas deux vérités à diverger. **Le titre est `Title_<lang>` — jamais `EntityTitle_<lang>`.**

---

## §3 — La forme cible du schéma

| | mesuré (07/07) | cible après fenêtre |
|---|---:|---:|
| attributs | **15** | **64** |
| attribut de titre (`IsTitle`) | `Title` ×1 | `Title` ×1 — **inchangé** |
| `SortOrder` occupés | 0–14 | 0–63 |

Valeurs prévues pour les 49 : `Group = "Default"` (comme les 15 mesurés) · `IsTitle = false` · `Type = "String"`.

⚠️ **Deux champs ne sont PAS prédits, et c'est délibéré** :

- **`AttributeID` n'est pas prédit.** La base l'assigne. Inventer des identifiants produirait un plan qui a
  l'air précis et qui est de la fiction — et un identifiant fabriqué se lit comme mesuré, ce qui est pire
  qu'un identifiant absent.
- **`Type = "String"` est une inférence, pas une mesure** (dossier §7 : les 7 champs source sont `String`
  dans le schéma mesuré, donc le suffixe hérite `String` — haute confiance, non mesuré). #687 reste le
  débloqueur sysadmin si un type s'avère différent.

---

## §4 — Trois gardes, exécutées par l'instrument

| garde | ce qu'elle attrape | pourquoi elle existe |
|---|---|---|
| `EntityTitle*` | l'option (B) **écartée** par la décision owner du 14/09 | un second attribut de titre coexisterait avec `Title` ; 7 attributs morts par langue |
| suffixe hors des 7 champs | une coquille de suffixe sur un attribut non-prose (ex. `UrlKey_en`) | il serait provisionné, traduit, puis **jamais lu** |
| `IsTitle` ≠ `['Title']` | un second attribut de titre, quel qu'en soit le nom | 2sxc ne titre que sur un attribut ; deux titres = comportement indéfini |

Les trois sont **rouges par construction** sur les mutations du self-test (§1) — une garde qu'on ne peut pas
faire rougir n'est pas une garde.

---

## §5 — Ce que ce dry-run n'établit PAS

- **La mécanique de provisioning.** UI / import de content-type / SQL : non tranché, non inventé (§0).
- **Les `AttributeID`** — assignés par la base (§3).
- **Les types 2sxc exacts des 49** — inférence (§3), #687 pour la mesure.
- **L'existence et l'homonymie des setIDs 210/231** — **RAPPORTÉES** par le transfert owner, non dérivées de
  l'export du dépôt, qui n'identifie que **377** (dossier §7). La consigne « ne pas toucher 210/231 » reste
  justifiée par prudence, mais son antécédent n'est pas mesuré ici.
- **Que la fenêtre est sans risque.** Provisionner un schéma 2sxc **modifie une base de production** ; ce
  document prépare et vérifie, il ne garantit rien.
- **Aucun verdict visuel, aucune écriture, aucun geste admin.** Le contrôle runtime du §7 est ai-01/owner.

---

## §6 — Checklist owner — la fenêtre en un écran

**Entrée** : décision §3.1 du dossier = `Title` ✅ (14/09) · dossier mergé ✅ · code #674 mergé ✅ ·
dry-run publié ✅ · **export du 07/07** ⚠️ décrit un état **pré-migration** — si le FR canonique a bougé,
**nommer la base cible et re-exporter** avant d'écrire.

| # | Coche | Geste | Base | Vérification |
|---:|---|---|---|---|
| 1 | ☐ | Provisionner les **49** attributs du §2 sur le content-type `Game Rule` **setID 377, app 60** | `ArgumentumGames` | `python tools/dnn-682-schema-delta.py --expect 0` → **0 manquant** |
| 2 | ☐ | Confirmer que **`Title` reste le seul** `IsTitle` | idem | la garde 3 de l'instrument reste verte |
| 3 | ☐ | **Ne toucher aucun setID homonyme** (210, 231) | idem | périmètre = 377 uniquement |
| 4 | ☐ | Le content-type passe de **15 → 64** attributs | idem | compteur de l'instrument : `measured : 64 attributs` |
| 5 | ☐ | **Contrôle runtime** (§7) — écrire **une** valeur localisée et la voir servie | idem | `Loc()` sert la valeur, pas le FR |
| 6 | ☐ | #683 (cultures + routage + switcher) — **parallélisable**, n'importe quel ordre | DNN Admin | `/en-US/...` rend 200, `<html lang>` suit |
| 7 | ☐ | **Après** : ré-exporter le schéma et l'attacher, pour que le prochain lecteur mesure au lieu de croire | dépôt | nouvelle exécution `--expect 0` sur l'export **frais** |

La ligne 7 n'est pas décorative : l'export committé date du **07/07** et c'est **lui** qui a fait établir
« 15 attributs ». Sans ré-export, la fenêtre n'a pas de preuve post-état — seulement une impression.

---

## §7 — Le contrôle runtime minimal (le seul qui lève `[runtime pending]`)

C'est le contrôle que #682 prescrit, retargeté sur l'attribut que la décision owner a choisi :

1. écrire **une** valeur : **`Title_en`** sur **une** des 5 entités (`11378`, `11380`, `11387`, `11388`, `11389`) ;
2. rendre `RuleDetail` avec `?language=en-US` ;
3. confirmer que `Loc(ruleEntity, "Title")` **sert la valeur DB** — et non le repli `Title_fr`/`Title`.

`Title_en` est la sonde la plus informative parce qu'elle porte **exactement** ce que la décision a changé :
`Title` est l'attribut `IsTitle = true`, celui dont l'alias `EntityTitle` avait fait diverger le code.
`Summary_en` (la sonde d'origine du dossier §4 étape 2.3) reste un second point de contrôle trivial si
l'owner veut doubler la mesure.

⚠️ **Ce contrôle ne peut pas être remplacé par une lecture de code.** La question d'origine — « l'alias
`EntityTitle` est-il exposé dans le dictionnaire 2sxc ? » — est devenue **sans objet** par le retarget, mais
la cascade retargetée, elle, n'a **jamais** été exécutée : c'est précisément ce que `[runtime pending]` dit.

---

## §8 — UAC

Ce dry-run **ne déclenche aucun UAC** : il ne lit qu'un fichier JSON committé et n'ouvre aucune connexion.
Conformément au pool (« dry-run publié sur le dashboard **avant tout UAC**, un seul UAC pour la lane »),
l'élévation éventuelle appartient à la fenêtre elle-même et reste **unique** ; aucune étape préparatoire de
cette lane ne doit en produire.

---

## §9 — Ce que la fenêtre dégage

| après | effet |
|---|---|
| §6 lignes 1–4 | #682 : le content-type porte les 49 attributs, `Title_<lang>` compris |
| §6 ligne 5 | PR #674 : `[runtime pending]` **levé** — la cascade retargetée a été exécutée |
| §6 ligne 6 | #683 : cultures servies, switcher fonctionnel |
| ensuite | #684 (traduction 161–196 unités + 63 cellules `res.Rule*`, puis réimport) → #685 (verdict visuel, **ai-01**) |

---

*master `5bdbdfd1` · export `2ff703f1` (07/07/2026) · lecture seule : parsing JSON, self-test en mémoire ·
⛔ aucune écriture DB, aucun geste admin DNN, **aucun UAC**, aucun réimport, aucune traduction, aucun verdict
visuel · provisioning DB et admin : jsboige · verdict visuel : ai-01.*
