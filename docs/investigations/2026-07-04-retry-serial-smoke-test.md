# Smoke-test du chemin retry-serial #676 (#613 Option C) — investigation

**Date** : 2026-07-04
**Auteur** : po-2023 (dispatch ai-01 `98vo07`, primaire)
**Base** : master `d5913862` (contient #676 merged)
**Statut** : analyse statique du chemin retry + rationale du HOLD runtime pre-tag + plan de validation runtime post-tag. **Signal métrique, PAS verdict** (po-2023 signale, ai-01 verdicte).

---

## 1. Objectif

Le dispatch `98vo07` demande un **smoke-test du retry-serial #676** : valider end-to-end que la passe de retry récupère bien les sets qui timeout sous parallélisme, sur un gros card set type Fallacies/A0. DoD : rapport court avec logs `[HARVEST-RETRY]`, combien de sets récupérés au 1er retry, timing.

Ce document livre (a) l'analyse statique du chemin retry (guards + data-flow + conditions de déclenchement), (b) le rationale du HOLD d'un smoke-test runtime pre-tag, (c) la couverture déterministe immédiate via tests unitaires `RetryAsync`, (d) le plan de capture des logs `[HARVEST-RETRY]` au prochain run release post-tag.

---

## 2. Mécanisme retry-serial — analyse statique (code = vérité)

### 2.1 Config (3 guards) — `WebBasedGeneratorConfig.cs`

| Propriété | Défaut | Rôle |
|-----------|--------|------|
| `ContinueOnHarvestSetFailure` (L45) | `true` | **Guard #614**. Si `true`, un set qui lève dans la boucle parallèle est capturé dans `failedSets` au lieu d'aborter tout le harvest. Si `false`, l'exception propage (retry jamais atteint). |
| `HarvestSetRetryAttempts` (L55) | `1` | Nombre de passes de retry serial par set failed. `0` désactive le retry (comportement pré-#676). |
| `HarvestSetRetryBackoffSeconds` (L62) | `30` | Backoff entre attempts d'un même set. |

### 2.2 Le seam — `HarvestManager.HarvestImages()` (L88–L128)

```
L88   var failedSets = new ConcurrentBag<(string cardSet, string language, string error)>();
L90   var parallelOptionsCardset = { MaxDegreeOfParallelism = EnableParallelism ? MaxDegreeOfParallelismCardpen : 1 };
L91   await Parallel.ForEachAsync(targetCardSets, parallelOptionsCardset, async (configCardSet, token) =>
L95     await Parallel.ForEachAsync(targetLanguages, ..., async (currentLanguage, newToken) =>
L97       try { await ProcessLocalizedHarvest(...); }
L101      catch (Exception ex) when (Config.ContinueOnHarvestSetFailure)   ← guard #614
L104        failedSets.Add((configCardSet.Name, currentLanguage, ex.Message));
L105        Logger.Log("[HARVEST-FAILURE] ...");
L108  });  ← fin du drain parallèle

L114  if (Config.HarvestSetRetryAttempts > 0 && !failedSets.IsEmpty)      ← guard retry + non-vide
L116    failedSets = await RetryFailedHarvestSetsAsync(failedSets, targetCardSets, harvestDictionary, funcBrowser);

L119  if (!failedSets.IsEmpty)                                            ← résiduel → throw agrégé
L122    Logger.Log("[HARVEST-PARTIAL] N card set(s) failed ...");
L125    throw new ApplicationException("Harvest completed with N failed card set(s) ...");
```

### 2.3 `RetryFailedHarvestSetsAsync` — itération serial post-loop (L141–L181)

- Ordonnance le bag failed par `(cardSet, language)` (déterminisme pour logs).
- Pour chaque entrée : lookup `CardSetJob` par `.Name` dans `targetCardSets` → appelle `RetryAsync(() => ProcessLocalizedHarvest(...))` serial (degré = 1, pas de parallélisme).
- **Pourquoi le retry re-rend correctement** : `ProcessLocalizedHarvest` (L174 du code merged) vérifie `!harvestDictionary.ContainsKey(...)` avant de générer. Un set **failed** n'a jamais ajouté sa clé au dictionary (le `catch` L101 court-circuite avant l'ajout) → le guard `ContainsKey` passe au retry → le set est re-rendu from scratch. **Pas de double-génération ni de stale-cache pour les sets réussis** (leur clé existe déjà → skip).
- Retourne un bag **résiduel** (sets encore failed après retry) → remplace `failedSets` → le throw `[HARVEST-PARTIAL]` L125 ne reporte que ce qui échoue réellement après retry.

### 2.4 `RetryAsync` — contrat pur (L192–L224)

Helper `internal static` extrait pour testabilité (cf. §4). Contrat :
- `action × attempts`, `true` au premier succès.
- Backoff entre attempts (skip si `TimeSpan.Zero`).
- **Non-throwing** : échec total → `false` + dernière exception loggée/avalée (pour que le caller construise le bag résiduel au lieu d'aborter sur la 1re failure résiduelle).
- Clamp `attempts < 1` → 1.

### 2.5 Conditions de déclenchement du retry (les 3 doivent tenir)

1. `ContinueOnHarvestSetFailure == true` (guard #614 actif — défaut `true`).
2. `HarvestSetRetryAttempts > 0` (défaut `1`).
3. `!failedSets.IsEmpty` → **au moins un set a échoué** dans la boucle parallèle.

La condition 3 est l'élément **non-déterministe** : elle requiert qu'un set timeout réellement sous parallélisme. Sur une machine véloce, un run complet peut produire **0 set failed** → le retry ne se déclenche pas → aucun log `[HARVEST-RETRY]`.

---

## 3. Rationale du HOLD d'un smoke-test runtime pre-tag

Le rappel HARD du dispatch stipule : **« Régén complète = HOLD jusqu'au tag/contenu final (le smoke-test cible juste le chemin retry, pas une régén release) »**.

Un smoke-test runtime du retry path se heurte à deux verrous :

### 3.1 Régén complète = HOLD

Le retry path ne s'active **que** dans une exécution de `HarvestImages()` réelle (Playwright + CardPen + Chromium). Les options pour le déclencher :

- **(a) Régén complète release** : explicitement HOLD jusqu'au tag (contenu final pas encore gelé). ❌
- **(b) Régén partielle ciblée** (1 seul card set, `Mode=WebBasedImageGeneration`) : techniquement possible, mais reste une exécution Playwright lourde (Chromium, CardPen local, ~minuttes par set) — et la règle worker « ne jamais lancer de regeneration lourde sans coordination préalable » s'applique. De plus, pour déclencher le retry il faut qu'un set échoue, ce qui ramène au point 3.2.

### 3.2 Non-déterminisme du timeout sous parallélisme

Même en lançant un run Fallacies/A0 sous `EnableParallelism=true`, le timeout d'un set dépend de la charge machine, du nombre de cores, de l'état du pool Playwright. Sur une machine idle, aucun set ne timeout → `failedSets` vide → **le retry path n'est pas exercé** → aucun log `[HARVEST-RETRY]` à capturer. Un smoke-test runtime « réussi » (0 retry fired) ne prouve donc rien sur le chemin retry lui-même — seulement que rien n'a timeout.

**Forcer** un timeout de façon déterministe nécessiterait soit (i) un card set corrompu (échec non-timeout, ne valide pas le scénario timeout-under-parallelism), soit (ii) un `MaxDegreeOfParallelismCardpen` artificiellement saturé + un set synthétique géant — engineering non trivial, hors scope d'un smoke-test.

### 3.3 Conclusion HOLD

Le smoke-test runtime déterministe du retry path n'est pas réalisable pre-tag sans violer le HOLD régén ou investir dans un harness de timeout synthétique. **Différé au prochain run release post-tag** (§5).

---

## 4. Couverture déterministe immédiate — tests unitaires `RetryAsync` (PR #678)

Le contrat retry/backoff est extrait dans le helper pur `RetryAsync` (`internal static`) **explicitement pour être testable sans browser** (précédent `ComputeExpectedImageCount`). La PR companion **#678** livre 8 cas xUnit (5 méthodes, theories expansées) :

| Cas | Asserte |
|-----|---------|
| `SucceedsOnFirstAttempt` | `true`, invoque 1×, no delay |
| `SucceedsOnNthAttempt` (2, 3) | `true`, invoque exactement N× |
| `AlwaysFails` | `false`, **jamais de throw**, invoque `attempts`× |
| `BackoffZero` | pas de delay (fast path) |
| `BackoffPositive` | 2 délais inter-attempts appliqués pour 3 attempts |
| `AttemptsClampedToAtLeastOne` (0, −1) | clamp défensif → exactement 1 invocation |

**Filter run : 8 pass / 0 fail**, 0 warning sur le fichier. Le cas `AlwaysFails` couvre la garantie **non-throwing** critique (le caller construit le bag résiduel sur un `bool`, pas sur une exception).

> Ce que ces tests couvrent : le **contrat** retry/backoff (cœur testable). Ce qu'ils ne couvrent pas : le **wiring** du seam (L114–L117) et le fait que `RetryFailedHarvestSetsAsync` remplace bien le bag — qui relèvent d'un test d'intégration runtime (§5).

---

## 5. Plan de validation runtime post-tag

Au prochain run release post-tag (la régén qui repartira naturellement), capturer les métriques retry :

1. **Activer** la capture des lignes `[HARVEST-RETRY]` dans la sortie pipeline (déjà loggées par `Logger.Log` aux points : début passe, attempt failed + backoff, attempt succeeded, exhausted, complete avec N recovered / M still failing).
2. **Métriques à rapporter** :
   - Nombre de sets entrés en retry (= taille du bag failed initial → log `[HARVEST-FAILURE]` count).
   - Nombre récupérés au retry (log `[HARVEST-RETRY] Retry pass complete: N recovered, M still failing`).
   - Timing de la passe retry (serial, somme des backoffs + temps Playwright).
   - Sets résiduels éventuels (reportés dans le throw `[HARVEST-PARTIAL]`).
3. **Verdict runtime** = ai-01 (po-2023 signale les métriques, ne déclare pas PASS).

Si le run release ne produit **aucun** `[HARVEST-RETRY]` (0 set failed), c'est un signal positif (rien n'a timeout) mais qui n'exerce pas le chemin retry — il faudra alors soit attendre un run sous charge, soit utiliser un harness de timeout synthétique (cf. §3.2).

---

## 6. Interaction connue — bug #630

Le bug #630 (Spectre `[HARVEST-FAILURE]` fatal) court-circuite #614 sur le path *fatal* : une exception fatale dans le handler `page.Console` n'est pas rattrapée par le `catch` L101 → propage hors de la boucle parallèle → le retry path (qui suppose un drain propre du loop) n'est pas atteint. Le retry est correct et valuable seul (fires sur le path #614 non-fatal = cas commun), mais un fix complet du path fatal requiert #630 (merged #655 le 03/07 — vérifier qu'il couvre bien le cas fatal résiduel au runtime post-tag).

---

## 7. Verdict (signal, pas PASS)

- **Chemin retry** : analysé statiquement, guards documentés, data-flow vérifié (ré-render correct via guard `ContainsKey`, bag résiduel reporté par le throw `[HARVEST-PARTIAL]`).
- **Contrat retry/backoff** : couvert déterministement par 8 tests unitaires verts (PR #678).
- **Smoke-test runtime** : HOLD pre-tag (régén HOLD + non-déterminisme timeout). Différé post-tag (§5).

🤖 Worker po-2023 — signal métrique, PAS verdict QA. Verdict runtime = ai-01 post-tag.
