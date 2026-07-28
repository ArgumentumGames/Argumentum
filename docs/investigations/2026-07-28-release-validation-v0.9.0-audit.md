# Audit du corps de `docs/RELEASE-VALIDATION-v0.9.0.md`

**Date** : 2026-07-28 · **Auditeur** : po-2024 · **Base** : master `3094d42c`
**Issue** : #134 (gate de release) · **Dispatch** : ai-01 (`msg-20260727T232817-gzd0d3`)

## Méthode & contrainte

Audit **read-only du corps** (§1–§8). L'en-tête et le §0 ont été corrigés par **#959** (refresh v5.3, ai-01) ; ils sont hors périmètre. **Aucune correction du dossier audité dans ce geste** (méthode ai-01 : un audit qui se corrige au fil de l'eau ne permet plus de compter les défauts). Ce fichier est **additif** — il ne modifie aucune ligne de `RELEASE-VALIDATION-v0.9.0.md`.

Chaque affirmation chiffrée ou vérifiable est re-mesurée **contre le code/les assets sur master `3094d42c`**, pas reprise d'un rapport intermédiaire. Pièges gardés en tête (ai-01, 28/07) : (1) chercher dans le dépôt avant de déclarer une absence ; (2) « fichier existe » ≠ « condition remplie » ; (3) correction réelle vs correction sans effet (vérifier le fichier que le runtime lit).

## Verdict global

| Verdict | Nombre |
|---|---:|
| 🟢 OK (re-mesuré exact) | 4 |
| 🔴 PÉRIMÉ (faux ou résolu sur master) | 6 |
| 🟡 NON RE-MESURÉ ce tick (probe imparfaite, cohérent) | 3 |

**6 affirmations périmées dans le corps** — dont une (**§3.2 « Virtues `.content.svg` FR-figés »**) est exactement le défaut que l'addendum **#960** a corrigé dans `RELEASE-VISUAL-GO-SESSION-v0.9.0.md`, mais qui n'a pas été reporté ici.

---

## 🟢 OK — re-mesuré exact sur `3094d42c`

| § | Affirmation citée | État mesuré | Preuve (commande) |
|---|---|---|---|
| §3.1 | Virtues **223 nœuds** | **223** | `tail -n +2 "Cards/Fallacies/Argumentum Virtues - Taxonomy.csv" \| grep -c .` |
| §3.4 | `argumentum.owl` **6 030 505 B** | **6 030 505** | `git cat-file -s $(git rev-parse HEAD:docs/ontology/argumentum.owl)` |
| §3.4 | `argumentum_virtues.owl` **862 709 B** | **862 709** | `git cat-file -s $(git rev-parse HEAD:docs/ontology/argumentum_virtues.owl)` |
| §3.4 | **145 `aifAttackType`**, **5 558 EN**, **4 861 FR**, **57 `broadMatch`** (Walton) | **145 / 5558 / 4861 / 57** | `git show HEAD:docs/ontology/argumentum.owl` puis `grep -c` |

Note : le refresh v5.2 (ai-01, 27/07) avait déjà rectifié §3.4 — **ces chiffres tiennent sur master actuel**, contrairement à v5.1 qui était faux.

---

## 🔴 PÉRIMÉ — faux ou résolu sur master `3094d42c`

### 1. §3.2 ligne 97 — « Virtues `.content.svg` sont **FR-figés** »

**État mesuré** : FAUX. Les Virtues `.content.svg` existent en **8/8 langues**, en contenu **native-script** (pas FR-clone).

**Preuve convergente** (3 signaux) :
- `git show 9f524464` — message : *« feat(mindmap): Virtue mindmaps ar/fa/zh — **native-script Batik SVGs** (#686)(#724) »* (deux-layer Virtue localization fix #715) ;
- Tailles distinctes du FR (un clone FR ferait ~431 931 B partout) : `fr 431 931 · ar 461 859 · fa 461 654 · zh 1 148 677` — `git cat-file -s` par blob ;
- Mémoire `virtues-mindmap-content-svg-fr-frozen.md` : *« RÉSOLU 2026-07-07 #665/#715/#724/#686 »*.

**Pourquoi ça compte** : c'est **exactement** le défaut corrigé par l'addendum **#960** dans le doc GO (`RELEASE-VISUAL-GO-SESSION-v0.9.0.md`) — prémisse (a) — mais **non reporté** dans `RELEASE-VALIDATION`. Un relecteur T&A (#802) lirait ici une *known-limitation* inexistante. (Mon `grep` de glyphes cyrilliques/arabes/Han rend 0 **y compris sur le FR** = sonde fausse : le SVG éclate le texte en nœuds — cf pattern *« le témoin révèle la sonde fausse »*. Les tailles + le commit message sont la preuve.)

### 2. §3.2 tableau — es/ar/fa/zh = **3 SVGs** chacun

**État mesuré** : **5 SVGs** chacun.

```
fr : 6   en : 5   ru : 5   pt : 5
es : 5   ar : 5   fa : 5   zh : 5
```
Preuve : `for l in fr en ru pt es ar fa zh; do git ls-files "Cards/Fallacies/Mindmaps/$l/*.svg" | wc -l; done`

Le sous-dénombrement « 3 » pour es/ar/fa/zh reflète l'état au moment de #565 ; les `.links.svg` Virtues ar/fa/zh ajoutés par `9f524464` portent le total à 5. Bénin (les assets dépassent le déclaré) mais périmé.

### 3. §4 ligne 159 — « Tests **596 pass / 0 fail / 5 skip / 601 total**, empirique master `84a529bf` »

**État mesuré** : **643 total / 638 pass / 0 fail / 5 skip**, en **CI**, legs Debug **et** Release.

**Preuve** : run `30280070312`, ligne `Total tests:` lue dans le log (post-#909 discipline).

**Contradiction interne** : le **§0** (corrigé par #959) et l'en-tête ligne 6 portent déjà **643/638/5** — mais le **§4 tableau** n'a pas été mis à jour. Deux compteurs coexistent dans le même dossier.

### 4. §4 ligne 161 — « Magick.NET **14.14.0** (bump 2026-07-01) »

**État mesuré** : **14.15.0** sur master.

**Preuve** : `grep Magick.NET Generation/Converters/Argumentum.AssetConverter/*.csproj` → `Version="14.15.0"` (#871, 25/07). L'en-tête ligne 6 le dit déjà ; le §4 tableau n'a pas suivi.

### 5. §3.6 + §5.8 + §7 ligne 198 + §8.2(c) — Finding titre PT « Roll of the English Channel » = **appel à décision jsboige** (block vs fast-follow)

**État mesuré** : **RÉSOLU** — la chaîne est **absente de tout `Cards/`**.

**Preuve** :
- `grep -rl "English Channel" Cards/` → 0 résultat ;
- `git log --oneline -S"English Channel"` → résolu via **#803 `7e72f3e5`** (*« fix(rules-i18n): correct EN MT-garbage in Rules template (#134 v0.9.0 BLOCK) »*) ;
- Addendum **#960** (ai-01) : la source de rendu `Argumentum Rules - Cards.csv` **ne l'a jamais portée** — dernier commit la touchant : `2079f0cc` (#640, EN propre « Round sequence » / « The drawer »). La bouillie MT n'a vécu que dans le snapshot ignoré du template (clé `csv` écrasée par `HarvestManager.cs:363`).

**4 occurrences du « finding ouvert / appel à décision »** dans le corps (§3.6, §5.8, §7, §8.2c) sont à retirer/transformer en « résolu ».

### 6. §5.4(a) + §8.2(a) — arbitrage **#636 §1 assets SVG Virtues** (FreeMind GUI-interactif vs defer post-tag)

**État mesuré** : **#636 = CLOSED**.

**Preuve** : `gh issue view 636 --json state,closedAt` → `state=CLOSED, closedAt=2026-07-06T17:11:06Z`. De plus, la prémisse jumelle (Virtues FR-figés, cf défaut 1) est **fausse** — l'arbitrage porte sur un manque déjà comblé. Même pattern que la rectification #960.

---

## 🟡 NON re-mesuré ce tick (probe imparfaite, cohérent avec dashboard 28/07)

| § | Affirmation | Pourquoi non re-mesuré | Cohérence |
|---|---|---|---|
| §3.1 | Fallacies **1 408 nœuds** | Mon `find` a attrapé un `Archive/2022` (32 lignes) au lieu du CSV de prod | Dashboard 28/07 = 1 408 |
| §3.1 | Scenarii **167 records** | Idem (Archive 2022 attrapé, 77 lignes) | §3.1 cite commits `7ed970a3`/`2a1b86bf`/`0dc838fb` vérifiés cell-by-cell `7206f2f9` |
| §3.4 | `prefLabel` 2 816 / `broader` 1 407 | `grep -c 'skos:prefLabel'` rend 0 = sonde fausse (namespace/éclatement en nœuds) | §3.4 documente la nuance d'unité + commande de référence `git cat-file -s` ; `broader` et `prefLabel` non re-dérivés ce tick |

Ces 3 affirmations ne sont **pas** marquées PÉRIMÉ — elles sont cohérentes avec l'état connu ; elles n'ont simplement pas été re-dérivées ce tick (honnêteté de méthode, pas un défaut).

---

## Recommendation (pour ai-01, décision = coordinator)

Les 6 défauts PÉRIMÉ sont **mécaniques et convergents** : 5/6 sont la même racine (prémisses de juillet dépassées par les merges de juillet — Virtues SVG 8-lang `9f524464`, Magick `14.15.0`, titre PT résolu `#803`, tests réels post-#911, #636 closed). Le 6e (Magick 14.14.0) est un oubli de sync.

Une **PR de correction** unique pourrait traiter les 6 en une passe — mais elle reste **à décider par ai-01** (owner du gate #134, zone pré-tag sensible). Ce rapport est livré séparément conformément à la contrainte de méthode : mesurer d'abord, corriger ensuite une fois l'ampleur vue.

— po-2024
