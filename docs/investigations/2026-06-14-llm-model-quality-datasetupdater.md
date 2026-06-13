# Investigation — LLM Model Quality for DatasetUpdater Translations (#299)

**Date** : 2026-06-14
**Issue** : [#299](https://github.com/ArgumentumGames/Argumentum/issues/299) — *Investigate LLM model quality for DatasetUpdater translations*
**Auteur** : ai-01 (Claude Opus)
**Branche** : `docs/299-llm-quality-investigation` (off master `c9197f15`)
**Type** : DOC-ONLY — aucune modification de code/CSV/prompt.

> **Méthode** : grounding technique (lecture du code, chemins `fichier:ligne`), croisement avec les défauts qualité connus (#411), les PRs de traduction (#290/#295/#302/#357) et l'historique git `*DatasetUpdater*`. Chaque affirmation est qualifiée **VÉRIFIÉ** (lu dans le code/git), **RAPPORTÉ** (issue/PR/commentaire) ou **SUPPOSÉ** (déduction non confirmée par exécution).

---

## 0. Résumé exécutif

- Le pipeline `DatasetUpdater` contient **47 task-configs** (`DatasetUpdaterRootConfig.cs`), toutes `Enabled = false` au repos. **VÉRIFIÉ** (`grep -c "new DatasetUpdaterConfig"` = 47 ; 47 × `Enabled = false`).
- La grande majorité des tâches de traduction/correction est désormais sur **`gpt-5.5`** (33 tâches), avec un résidu **`gpt-5.4-mini`** (10 tâches, surtout RU/PT/ZH/ES/FA legacy) et **`gpt-5.4`** (4 tâches FR). **VÉRIFIÉ** (comptage des `Model =` ci-dessous).
- **Aucune tâche ne définit `BaseUrl`** : malgré le support multi-provider ajouté en #302, toutes les tâches tournent par défaut sur l'endpoint OpenAI direct. **VÉRIFIÉ** (`grep "BaseUrl =" DatasetUpdaterRootConfig.cs` = 0 résultat ; cf. §2.3 — le routage OpenRouter, s'il existe, passe par le **contenu du fichier clé**, pas par la config).
- Le hint de l'issue #299 (« utilise `gpt-5.4-mini` ») est **périmé** : le code a migré vers `gpt-5.5` (commits `bb2040e8`, `0b30c57b`, PR #302). **VÉRIFIÉ**.
- Les défauts de #411 sont **partiellement** couverts par les tâches « cosmetic polish » : Cat B (PT register) + Cat C (FA/ZH) ont des tâches dédiées (44–47), mais **Cat A (ES accent PK2, EN point final PK800) n'a aucune tâche ciblée** — `text_es`/`text_en` n'apparaissent dans aucun `FieldsToUpdate` cosmétique. **VÉRIFIÉ** (cf. §4.1).

---

## 1. Architecture d'appel (rappel technique)

| Élément | Fichier:ligne | Détail | Qualif. |
|---|---|---|---|
| Boucle d'orchestration | `DatasetUpdaterRootConfig.cs:9-20` | itère `DatasetUpdaterConfigs`, exécute uniquement celles `Enabled` | VÉRIFIÉ |
| Modèle par défaut (classe) | `DatasetUpdaterConfig.cs:32` | `Model = "gpt-5.4-mini"` (valeur d'usine, overridée par chaque task) | VÉRIFIÉ |
| Modèle par défaut (Prompt) | `Prompt.cs:17` | `Model = "gpt-4.1-mini"` (jamais utilisé : la task passe toujours son `Model`, `DatasetUpdaterConfig.cs:479`) | VÉRIFIÉ |
| Chemin clé par défaut | `DatasetUpdaterConfig.cs:30` | `G:\Mon Drive\MyIA\Argumentum\Fallacies\Gestion\OpenAI-Key.txt` (GDrive) | VÉRIFIÉ |
| `BaseUrl` (optionnel) | `DatasetUpdaterConfig.cs:34`, propagé `:477` → `Prompt.cs:21,39-48` | si non-vide → endpoint custom via `ApiKeyCredential` ; sinon `new OpenAIClient(ApiKey)` (OpenAI direct) | VÉRIFIÉ |
| Lecture clé | `DatasetUpdaterConfig.cs:92` | `File.ReadAllTextAsync(OpenAIKeyPath)` — la clé est le **contenu** du fichier, pas une variable d'env | VÉRIFIÉ |
| Appel chat | `Prompt.cs:58-133` (`Send`) | construit `system + dialog few-shot + user`, options `MaxOutputTokenCount`, tools | VÉRIFIÉ |
| Function calling | `Prompt.cs:97-121` + `DatasetUpdaterConfig.cs:486-499,544-565` | tool unique `UpdateRecord(primaryKey, fieldName, newValue)` ; suffixe langue injecté dans la description | VÉRIFIÉ |
| Découpage en chunks | `DatasetUpdaterConfig.cs:204-223` | `PKHierarchicalChar` (par branche taxonomique) ou `SequentialChunks` | VÉRIFIÉ |
| Filtre cellules vides | `DatasetUpdaterConfig.cs:225-228` | `SelectEmptyTargets` ne garde que les groupes contenant ≥1 cellule cible vide | VÉRIFIÉ |
| Passes multiples | `DatasetUpdaterConfig.cs:143,88` | `NbGlobalPasses` boucle l'intégralité du dataset N fois | VÉRIFIÉ |
| Rate limiting | `DatasetUpdaterConfig.cs:263,507` + `TokenManager.cs` | `MaxTokensPerMinute = 70000` par défaut | VÉRIFIÉ |
| Parallélisme | `DatasetUpdaterConfig.cs:265-276` | `MaxDegreeOfParallelismWebService = 2` par défaut | VÉRIFIÉ |

**Point d'attention (VÉRIFIÉ)** : le mode function-calling ne **vérifie pas** que toutes les cellules ont bien été mises à jour. `Prompt.cs:114-121` exécute les tool-calls retournés mais ne ré-émet rien si le modèle « oublie » un champ. Le compteur `recordsUpdator.CallCount` / `FilledOverwriteCount` est seulement **loggé** (`DatasetUpdaterConfig.cs:513-516`), jamais utilisé pour relancer. ⇒ silence partiel possible (modèle qui ne couvre pas tout le chunk).

---

## 2. Inventaire des modèles par tâche (table sourcée)

### 2.1 Comptage global

| Modèle | Nb tâches | Qualif. |
|---|---|---|
| `gpt-5.5` | 33 | VÉRIFIÉ (`grep -c 'Model = "gpt-5.5"'`) |
| `gpt-5.4-mini` | 10 | VÉRIFIÉ |
| `gpt-5.4` | 4 | VÉRIFIÉ |
| **Total** | **47** | VÉRIFIÉ |

> Le fichier ne contient **aucune** référence à `gpt-4.1` / `gpt-4.1-mini` / `claude-*` / `glm-*` en valeur effective : la seule mention est un commentaire `// Fallback: gpt-4.1-mini` sur la tâche Rules PT (`DatasetUpdaterRootConfig.cs:798`). **VÉRIFIÉ**. La table providers de #299 (OpenRouter/Myia/ZAI) **n'est pas câblée** dans la config courante (aucun `BaseUrl`).

### 2.2 Détail par tâche (47 tâches)

Légende : **EO** = `SelectEmptyTargets=true` (remplit les vides), **GP** = `NbGlobalPasses`, modèle entre crochets.

| # | Ligne | Nom de tâche | Modèle | Dataset / langue | Prompt user (few-shot) |
|---|---|---|---|---|---|
| 1 | 26 | Update Virtues Taxonomy by chunks 1-shot | `gpt-5.4` | Virtues / FR | `VirtuesJsonPromptSystem` (no func) |
| 2 | 71 | Update Fallacies French Description by branch | `gpt-5.4` | Fallacies / FR | `PromptInstructionsUserDescription` |
| 3 | 136 | Update Fallacies French example by branch | `gpt-5.4` | Fallacies / FR | `PromptInstructionsLightUserExamples` (EO) |
| 4 | 202 | Translate Fallacies → EN, empty-only | `gpt-5.4-mini` | Fallacies / EN | `PromptTranslateFrEnInstructionsUser` (EO) |
| 5 | 266 | Translate Fallacies → RU, empty-only | `gpt-5.4-mini` | Fallacies / RU | `PromptTranslateRuInstructionsUser` (EO) |
| 6 | 330 | Translate Fallacies → PT, empty-only | `gpt-5.4-mini` | Fallacies / PT | `PromptTranslatePtInstructionsUser` (EO, GP=2) |
| 7 | 395 | Cleanup Fallacies translations | `gpt-5.4` | Fallacies / multi | `PromptTranslateCleanupInstructionsUser` (GP=2) |
| 8 | 468 | Translate Virtues → EN, empty-only | **`gpt-5.5`** | Virtues / EN | `PromptVirtuesTranslateEnUser` (EO, key `.keys\openai-key.txt`, MaxOut=4096) |
| 9 | 534 | Translate Virtues → RU, empty-only | `gpt-5.4-mini` | Virtues / RU | `PromptVirtuesTranslateRuUser` (EO) |
| 10 | 598 | Translate Virtues → PT, empty-only | `gpt-5.4-mini` | Virtues / PT | `PromptVirtuesTranslatePtUser` (EO) |
| 11 | 662 | Generate Virtues PT Wikipedia links | `gpt-5.4-mini` | Virtues / PT links | `PromptVirtuesLinksPtUser` (EO) |
| 12 | 709 | Translate Virtues → ES, empty-only | **`gpt-5.5`** | Virtues / ES | `PromptVirtuesTranslateEsUser` (EO, MaxOut=4096) |
| 13 | 772 | Translate Rules → PT | **`gpt-5.5`** | Rules / PT | `PromptRulesTranslatePtUser` (commentaire fallback gpt-4.1-mini) |
| 14 | 815 | Translate Scenarii → EN, empty-only | **`gpt-5.5`** | Scenarii / EN | `PromptScenariiTranslateEnUser` (EO, key `.keys\openai-key.txt`, MaxOut=4096) |
| 15 | 885 | Translate Scenarii → RU, empty-only | `gpt-5.4-mini` | Scenarii / RU | `PromptScenariiTranslateRuUser` (EO) |
| 16 | 953 | Translate Scenarii → PT, empty-only | `gpt-5.4-mini` | Scenarii / PT | `PromptScenariiTranslatePtUser` (EO) |
| 17 | 1020 | Refine Virtues ES translations | **`gpt-5.5`** | Virtues / ES | `PromptVirtuesRefineEsUser` (MaxOut=4096) |
| 18 | 1084 | Translate Virtues → AR, empty-only | **`gpt-5.5`** | Virtues / AR | `PromptVirtuesTranslateArUser` (EO, MaxOut=4096) |
| 19 | 1148 | Translate Virtues → FA, empty-only | **`gpt-5.5`** | Virtues / FA | `PromptVirtuesTranslateFaUser` (EO, MaxOut=4096) |
| 20 | 1212 | Translate Virtues → ZH, empty-only | **`gpt-5.5`** | Virtues / ZH | `PromptVirtuesTranslateZhUser` (EO, MaxOut=4096) |
| 21 | 1276 | Translate Rules → ES | `gpt-5.4-mini` | Rules / ES | `PromptRulesTranslateEsUser` |
| 22 | 1319 | Translate Rules → AR | `gpt-5.4-mini` | Rules / AR | `PromptRulesTranslateArUser` |
| 23 | 1362 | Translate Rules → FA | `gpt-5.4-mini` | Rules / FA | `PromptRulesTranslateFaUser` |
| 24 | 1405 | Translate Rules → ZH | `gpt-5.4-mini` | Rules / ZH | `PromptRulesTranslateZhUser` |
| 25 | 1448 | Translate Scenarii → ES, empty-only | **`gpt-5.5`** | Scenarii / ES | (MaxOut=4096, EO) |
| 26 | 1513 | Translate Scenarii → AR, empty-only | **`gpt-5.5`** | Scenarii / AR | (MaxOut=4096, EO) |
| 27 | 1578 | Translate Scenarii → FA, empty-only | **`gpt-5.5`** | Scenarii / FA | (MaxOut=4096, EO) |
| 28 | 1643 | Translate Scenarii → ZH, empty-only | **`gpt-5.5`** | Scenarii / ZH | (MaxOut=4096, EO) |
| 29 | 1708 | Rules FR clarity review | **`gpt-5.5`** | Rules / FR | `PromptRulesFrClarityUser` (MaxOut=4096) |
| 30 | 1751 | Scenarii FR clarity review | **`gpt-5.5`** | Scenarii / FR | `PromptScenariiFrClarityUser` (MaxOut=8192) |
| 31 | 1802 | Scenarii cascade multi-lang | **`gpt-5.5`** | Scenarii / 7-lang | `PromptScenariiCascadeDriftUser` (MaxOut=8192) |
| 32 | 1859 | Fallacies FR clarity review | **`gpt-5.5`** | Fallacies / FR | `PromptFallaciesFrClarityUser` (MaxOut=8192) |
| 33 | 1908 | Fallacies cascade multi-lang | **`gpt-5.5`** | Fallacies / 7-lang | `PromptFallaciesCascadeDriftUser` (MaxOut=8192) |
| 34 | 1965 | Virtues FR clarity review | **`gpt-5.5`** | Virtues / FR | `PromptVirtuesFrClarityUser` (MaxOut=8192) |
| 35 | 2012 | Rules cascade multi-lang | **`gpt-5.5`** | Rules / 7-lang | `PromptRulesCascadeDriftUser` (MaxOut=4096) |
| 36 | 2059 | Virtues cascade multi-lang | **`gpt-5.5`** | Virtues / 7-lang | `PromptVirtuesCascadeDriftUser` (MaxOut=8192) |
| 37 | 2114 | Rules cascade EN-only | **`gpt-5.5`** | Rules / EN | `PromptRulesCascadeDriftUser` (MaxOut=4096) |
| 38 | 2158 | Virtues cascade EN-only | **`gpt-5.5`** | Virtues / EN | `PromptVirtuesCascadeDriftUser` (MaxOut=8192) |
| 39 | 2202 | Scenarii cascade EN-only | **`gpt-5.5`** | Scenarii / EN | `PromptScenariiCascadeDriftUser` (MaxOut=8192) |
| 40 | 2248 | Fallacies cascade EN-only | **`gpt-5.5`** | Fallacies / EN | `PromptFallaciesCascadeDriftUser` (MaxOut=8192) |
| 41 | 2294 | Scenarii PT refine | **`gpt-5.5`** | Scenarii / PT | `PromptScenariiPtRefineUser` (MaxOut=8192) |
| 42 | 2341 | Fallacies AUDIT_FR cascade 7-lang | **`gpt-5.5`** | Fallacies / 7-lang | `PromptFallaciesCascadeDriftUser` (MaxOut=8192, EO) |
| 43 | 2397 | RulesP&P blank-fill AR/ES/ZH/FA | **`gpt-5.5`** | RulesPrintAndPlay / 4-lang | `PromptRulesPPTranslateMultiUser` (MaxOut=8192, EO) |
| 44 | 2441 | Fallacies cosmetic polish PT/FA/ZH | **`gpt-5.5`** | Fallacies / PT,FA,ZH | `PromptCosmeticPolishUser` (MaxOut=8192) |
| 45 | 2491 | Virtues cosmetic polish PT register | **`gpt-5.5`** | Virtues / PT | `PromptCosmeticPolishPtRegisterUser` (MaxOut=8192) |
| 46 | 2538 | Scenarii cosmetic polish PT register | **`gpt-5.5`** | Scenarii / PT | `PromptCosmeticPolishPtRegisterUser` (MaxOut=8192) |
| 47 | 2587 | Fallacies cosmetic polish PT register | **`gpt-5.5`** | Fallacies / PT | `PromptCosmeticPolishPtRegisterUser` (MaxOut=8192) |

*(Toutes les lignes/valeurs ci-dessus : **VÉRIFIÉ** par lecture directe de `DatasetUpdaterRootConfig.cs`.)*

### 2.3 Modèle par langue — synthèse

| Langue | Initial translate | Cascade/polish (passe 2+) | Modèle dominant | Qualif. |
|---|---|---|---|---|
| **EN** | `gpt-5.4-mini` (Fallacies #4) → `gpt-5.5` (Virtues #8, Scenarii #14) + cascade EN-only #37-40 | `gpt-5.5` | `gpt-5.5` | VÉRIFIÉ |
| **RU** | `gpt-5.4-mini` (#5, #9, #15) | cascade multi-lang `gpt-5.5` (#31,33,35,36,42) | mixte (initial mini, polish 5.5) | VÉRIFIÉ |
| **PT** | `gpt-5.4-mini` (#6,#10,#16) ; Rules `gpt-5.5` (#13) | refine/polish `gpt-5.5` (#41,44-47) | mixte (initial mini, polish 5.5) | VÉRIFIÉ |
| **ES** | `gpt-5.4-mini` (Rules #21) ; `gpt-5.5` (Virtues #12, Scenarii #25) | refine `gpt-5.5` (#17) | mixte | VÉRIFIÉ |
| **AR/FA/ZH** | `gpt-5.4-mini` (Rules #22-24) ; `gpt-5.5` (Virtues #18-20, Scenarii #26-28) | `gpt-5.5` (cascade, RulesP&P #43) | mixte | VÉRIFIÉ |

**Constat clé (VÉRIFIÉ)** : les colonnes **RU et PT initiales des 3 gros datasets (Fallacies/Scenarii/Virtues) ont été produites par `gpt-5.4-mini`**, tandis que EN initial a basculé sur `gpt-5.5` (#302). C'est cohérent avec l'asymétrie de qualité rapportée : EN ~« bien avancé », RU/PT « nécessitent plusieurs passes » (#192). Les passes de correction ultérieures (cascade drift, PT refine, cosmetic polish) sont elles toutes en `gpt-5.5`.

### 2.4 Provider / endpoint

- **VÉRIFIÉ** : 0 `BaseUrl` dans `DatasetUpdaterRootConfig.cs` ⇒ par défaut **OpenAI direct** (`Prompt.cs:46`).
- **VÉRIFIÉ** : 2 tâches (#8, #14) overrident `OpenAIKeyPath = .keys\openai-key.txt` (clé locale), les 45 autres utilisent le fichier GDrive par défaut.
- **RAPPORTÉ** (mémoire projet `reference_datasetupdater_openrouter_key.md`, non re-vérifié ici) : en pratique le fichier clé peut contenir une clé **OpenRouter** et le `BaseUrl` OpenRouter, ce qui routerait `gpt-5.5` vers OpenRouter. **Mais le code courant ne fixe aucun `BaseUrl`** — donc soit la clé est une vraie clé OpenAI, soit l'opérateur édite la config hors-git avant un run. **À clarifier** : il y a une incohérence entre la mémoire (« BaseUrl OpenRouter ») et le code (BaseUrl null partout). Sans exécution, statut = **non concluant**.

---

## 3. Prompts — état réel (sourcé)

| Famille de prompt | Fichier (Resources/) | Rôle | Observation | Qualif. |
|---|---|---|---|---|
| Système générique | `PromptGeneralSystem.txt` | impose function-calling exclusif (`UpdateRecord`), interdit le texte libre | utilisé par presque toutes les tâches | VÉRIFIÉ |
| Scenarii→EN | `PromptScenariiTranslateEnUser.txt` | mapping de champs explicite, glossaire baratineur/piocheur, ton ludique | bien spécifié (cohérence lexicale §3, noms propres §5) | VÉRIFIÉ |
| Rules→PT | `PromptRulesTranslatePtUser.txt` | pt-PT (européen, pas BR), préserve markdown + emoji, familles=noms propres | **incohérence registre** : exige pt-PT alors que cosmetic polish #45-47 impose **pt-BR (você)** | VÉRIFIÉ |
| Scenarii PT refine | `PromptScenariiPtRefineUser.txt` | passe qualitative ciblée, liste de calques fautifs, conservatisme, sortie 0–25% | prompt mûr, exemples concrets | VÉRIFIÉ |
| Cascade drift (Fallacies) | `PromptFallaciesCascadeDriftUser.txt` | re-aligner les 7 langues après simplif FR, **max 1 cellule/nœud/passe** | convergence progressive bien pensée ; FR = source de vérité | VÉRIFIÉ |
| Cosmetic polish Cat B/C | `PromptCosmeticPolishUser.txt` | PT register + FA title-style + ZH disambiguation | **dit « Return a JSON array »** alors que la tâche #44 a `UseFunctionCalling=true` + system « JAMAIS de texte libre » | VÉRIFIÉ |
| Cosmetic PT register | `PromptCosmeticPolishPtRegisterUser.txt` | harmonise vers **você/teu (pt-BR singulier informel)** | conflit pt-PT/pt-BR (voir Rules→PT) | VÉRIFIÉ |
| RulesP&P multi | `PromptRulesPPTranslateMultiUser.txt` | AR/ES/ZH/FA en un appel/section, préserve markdown/emoji | OK | VÉRIFIÉ |

**Incohérences de prompt repérées (toutes VÉRIFIÉES par lecture) :**

1. **Registre PT contradictoire.** `PromptRulesTranslatePtUser.txt` §4 impose **portugais européen (pt-PT, PAS brésilien)** ; les prompts cosmetic `PromptCosmeticPolishUser.txt`/`PromptCosmeticPolishPtRegisterUser.txt` imposent **pt-BR (você/teu, singulier informel)**. Une retraduction puis un polish sur le même dataset PT tirent donc dans deux directions opposées. #411 §3 demande *« harmoniser vers un seul registre »* — le code applique **deux** standards selon la tâche.

2. **Format de sortie vs mécanisme.** `PromptCosmeticPolishUser.txt` + `PromptCosmeticPolishAssistant.txt` décrivent une **réponse JSON array**, mais la tâche #44 (`DatasetUpdaterRootConfig.cs:2478`) active `UseFunctionCalling=true` et utilise `PromptGeneralSystem.txt` qui interdit explicitement le texte libre. Le few-shot assistant (JSON) contredit le system (function-calls only). Risque : le modèle hésite entre émettre du JSON et appeler `UpdateRecord` → couverture partielle.

---

## 4. Défauts par langue (#411) corrélés au modèle/prompt

> #411 = audit Fallacies (5 échantillons, 150+ entrées × 8 langues). Score : Structure 4.5/5, Traductions 4/5 (≥95% correctes). Tous les PK ci-dessous sont **RAPPORTÉS** depuis #411 (non re-vérifiés cellule par cellule dans cette investigation DOC-ONLY).

| # | Défaut (#411) | Champ | PK | Modèle producteur probable | Couverture corrective actuelle | Qualif. |
|---|---|---|---|---|---|---|
| 1 | ES — accent FR parasite (`Generalización hâtive`) | `text_es` | 2 | `gpt-5.5` (Fallacies ES via cascade #33/#42) | **AUCUNE tâche cosmétique ne cible `text_es`** | VÉRIFIÉ (gap) |
| 2 | EN — point final parasite dans titre | `text_en` | 800 | `gpt-5.5` (cascade EN-only #40) / legacy `gpt-5.4-mini` #4 | **AUCUNE tâche cosmétique ne cible `text_en`** | VÉRIFIÉ (gap) |
| 3 | PT — mix de registres (sing/plur/archaïque) | `desc_pt` | path 7.1.1.1 / 1.1.2 / 3.1.1 / 5.x | initial `gpt-5.4-mini` (#6) | tâches #44 + #47 (`gpt-5.5`, `desc_pt`) | VÉRIFIÉ (couvert) |
| 4 | FA — titres-phrases au lieu de titres-noms | `text_fa` | 802 | Virtues/Scenarii FA `gpt-5.5` ; Rules FA `gpt-5.4-mini` (#23) | tâche #44 (`gpt-5.5`, `text_fa`) | VÉRIFIÉ (couvert) |
| 5 | ZH — même terme pour 2 concepts | `text_zh` | 2, 176 | `gpt-5.5` | tâche #44 (`gpt-5.5`, `text_zh`) | VÉRIFIÉ (couvert) |
| 6 | FA — translittérations divergentes d'un même concept | `text_fa` | 614, 615 | idem #4 | tâche #44 (`text_fa`) — **mais prompt cosmetic ne mentionne pas l'harmonisation intra-sous-famille** | VÉRIFIÉ (partiel) |

### 4.1 Constat majeur — Cat A non couverte

**VÉRIFIÉ** : `text_es` et `text_en` n'apparaissent dans **aucun** `FieldsToUpdate` de tâche cosmétique. Ils figurent seulement dans les cascades complètes (#33 Fallacies multi-lang, #42 AUDIT_FR) — lesquelles re-traduisent largement et risquent de **réintroduire** des artefacts plutôt que de retirer chirurgicalement un accent (PK2) ou un point final (PK800). La proposition de #411 (« Cat A : correction mécanique 0 drift ») n'est donc **pas implémentée**. Pour 2 cellules connues, un fix CSV déterministe serait plus sûr qu'un appel LLM.

### 4.2 Corrélation modèle → défaut

- Les défauts **PT** (registre #3) pèsent sur les colonnes **produites initialement par `gpt-5.4-mini`** (#6/#10/#16). C'est le signal le plus net : *mini* est le maillon faible pour PT/RU. **VÉRIFIÉ** (modèle) / **RAPPORTÉ** (corrélation au défaut).
- Les défauts **FA/ZH/ES** (#1,4,5,6) touchent des colonnes en partie produites par `gpt-5.5` ⇒ ce ne sont pas des défauts de « petit modèle » mais des défauts de **prompt** (ex. titre vs phrase non contraint en longueur ; cohérence terminologique intra-taxonomie non imposée). **SUPPOSÉ** (plausible, non prouvé par exécution).

---

## 5. Recommandations actionnables pour #192 (multi-passe)

> #192 = amélioration multi-passe EN/RU/PT. Les recos ci-dessous sont **concrètes et localisées au code existant** ; elles restent des propositions (cette PR est DOC-ONLY).

### Reco 1 — Faire passer RU/PT initial sur `gpt-5.5`, comme EN (#302)

**Où** : `DatasetUpdaterRootConfig.cs` tâches **#5, #6, #9, #10, #15, #16** (Fallacies/Virtues/Scenarii RU+PT) — actuellement `gpt-5.4-mini`.
**Pourquoi** : EN a basculé `gpt-5.5` en #302 et est jugé « bien avancé » (#192) ; RU/PT sont restées `mini` et sont jugées « 2-3 passes nécessaires ». L'asymétrie modèle ↔ qualité est directe. **VÉRIFIÉ** (asymétrie modèle), **SUPPOSÉ** (gain qualité).
**Où la 2e passe gpt-5.5 aide le plus** : **PT** d'abord (défaut #411 #3 confirmé, registre), puis **RU**. La passe la plus rentable est `PromptScenariiPtRefineUser.txt` (#41) — prompt déjà mûr, ciblé, conservateur — à étendre au pattern Fallacies/Virtues PT s'il n'existe pas d'équivalent.

### Reco 2 — Ajouter une tâche cosmétique Cat A (ES accent + EN point final) OU fix CSV déterministe

**Où** : aucune tâche ne cible `text_es`/`text_en` (gap §4.1).
**Quoi** :
- Soit une tâche `gpt-5.5` `SelectEmptyTargets=false` avec un prompt « typo-only » strict (retirer accents FR parasites dans `text_es`, retirer points finaux parasites dans `text_en`), **AutoCompare=true** pour review cell-by-cell (cf. mécanisme `CompareMode`, `DatasetUpdaterConfig.cs:290-403`).
- Soit — plus sûr pour des défauts connus et ponctuels (PK2, PK800) — un **fix CSV déterministe** hors LLM (regex accent circonflexe dans colonne `text_es`, strip `.` final dans `text_en`). Le LLM est inutile et risqué pour 2 cellules identifiées.
**Recommandation** : déterministe pour les PK connus ; LLM seulement si un scan révèle de nombreuses occurrences. **VÉRIFIÉ** (gap) / proposition.

### Reco 3 — Lever les 2 incohérences de prompt avant toute relance PT

**Où** : `PromptRulesTranslatePtUser.txt` (pt-PT) vs `PromptCosmeticPolish*` (pt-BR você) ; et `PromptCosmeticPolishUser.txt` (« JSON array ») vs `UseFunctionCalling=true` sur #44.
**Quoi** :
1. **Trancher pt-PT vs pt-BR une fois pour toutes** (#411 §3 demande UN registre). Tant que les deux coexistent, chaque passe défait la précédente. Décision produit requise (le projet vise quel public PT ?).
2. **Aligner le few-shot cosmetic sur le function-calling** : `PromptCosmeticPolishAssistant.txt` montre un JSON array alors que le system impose `UpdateRecord`. Soit passer #44 en `UseFunctionCalling=false` (et parser le JSON), soit réécrire le few-shot en appels de fonction. **VÉRIFIÉ** (contradiction) / proposition.

### Où une 2e passe `gpt-5.5` cell-by-cell aide le plus (synthèse #192)

| Priorité | Cible | Mécanisme existant à réutiliser | Justification |
|---|---|---|---|
| **P1** | **PT** (Fallacies `desc_pt`, Scenarii `*_pt`) | `Scenarii PT refine` #41 + cosmetic #44/#47 | défaut #411 #3 confirmé, colonnes ex-`mini` |
| **P2** | **RU** (Fallacies/Scenarii/Virtues) | cascade drift multi-lang #31/#33/#36 (1 cellule/nœud/passe) | colonnes ex-`mini`, pas d'audit RU détaillé ⇒ passe drift conservatrice |
| **P3** | **EN** (titres) | tâche Cat A à créer (Reco 2) | défaut ponctuel PK800, EN sinon mûr |

**Prompts à durcir** (VÉRIFIÉ par lecture) :
- `PromptCosmeticPolishUser.txt` (FA title-style) : **imposer une contrainte de longueur explicite** (« `text_fa` ≤ N caractères / forme nominale, jamais une phrase complète ») et **l'harmonisation intra-sous-famille** des translittérations (#411 #6, PK614/615) — actuellement absente.
- Cascade drift : la règle « max 1 cellule/nœud/passe » (`PromptFallaciesCascadeDriftUser.txt` §3) est excellente pour la convergence mais **lente** pour combler un retard RU/PT massif ⇒ pour le **fill** initial, garder le prompt « full retranslation » (`PromptFallaciesFullRetranslationUser.txt`) + `SelectEmptyTargets`, et réserver le drift à la convergence post-fill (déjà la conception, cf. mémoire `project_cascade_drift_prompt_design.md`). **RAPPORTÉ**.

---

## 6. Limites de cette investigation

- **DOC-ONLY, pas d'exécution** : aucun appel LLM lancé ici. Les corrélations « modèle → défaut » sont des hypothèses fondées sur le code + #411, non des A/B benchmarks. Le benchmark live (3 records, comparaison providers) figure déjà dans le **commentaire de #299** — il est **RAPPORTÉ**, non re-vérifié.
- **Incohérence mémoire/code sur OpenRouter** (§2.4) non tranchée : nécessite d'inspecter le contenu réel du fichier clé (hors git, GDrive) — hors scope DOC-ONLY.
- Les PK de #411 ne sont **pas** re-vérifiés cellule par cellule dans la CSV (audit séparé recommandé avant correction).

---

## 7. Références (code = vérité)

- `Generation/Converters/Argumentum.AssetConverter/DatasetUpdater/DatasetUpdaterRootConfig.cs` — 47 task configs
- `…/DatasetUpdater/DatasetUpdaterConfig.cs` — moteur d'une tâche (chunks, empty-targets, function-calling, key/BaseUrl)
- `…/DatasetUpdater/Prompt.cs` — appel OpenAI SDK, branchement BaseUrl, exécution des tool-calls
- `…/DatasetUpdater/Resources/*.txt|*.json` — 97 fichiers de prompts
- Issues : #299 (cette investigation), #411 (défauts cosmétiques), #192 (multi-passe), #211 (Rules PT), #297 (fix prompt PT)
- PRs : #290/#295 (Virtues fill), #302 (multi-provider + gpt-5.5 EN), #357 (desc_en house style), #447 (PT register polish), #405 (Virtues FA + RulesP&P)
- Git : `git log --oneline -30 -- '*DatasetUpdater*'` — migration Betalgo→OpenAI SDK (`fd2aef10`), GPT-5.x (`bb2040e8`), multi-provider (`0b30c57b`)
