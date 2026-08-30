# #498 — AIF two-layer reconciliation, P2 (sérialisation SKOS des 46 « Abus de langage » attack-only)

**Worker** po-2024 · **Date** 2026-08-29 · **Base** master `c7634007` (#1218) · **Statut** proposition **gated** (0 write prod CSV dans ce PR ; write = gate distinct owner) · **Track** GO ai-01 `msg-20260829T035617-4kx29h` « #498, tranche suivante — Écriture autorisée **dans `docs/taxonomy/` uniquement**. ⛔ Rien dans le CSV de production sans un GO distinct de l'owner ».

> Scope de ce PR : **docs + annotations + apply-script (dry-run)**. 0 write prod. Sérialisation prod gated (relais ai-01).

---

## 0. TL;DR

La réconciliation deux-couches a fait son **volet attack** (P1 tranches 1..1g : back-fill `AIF_attackType`/`AIF_attackedNode` sur les 52 skos-only ; **clôturé 2026-07-11** — attack 93 → 145, skos-only → 0). Il reste le **volet skos en sens inverse** : les **75 lignes attack-only** (attack typé mais aucune cellule `AIF_skos*`). C'est la direction P2 du census §5 (Priorite 2 — « sérialisation d'un contenu vetté, pas de re-modélisation »).

Ce PR livre la **tranche P2-A = les 46 lignes attack-only de la famille « Abus de langage »** (`soussousfamille`: Acception vague/arbitraire, Définition inconsistante, Comparaison abusive, Fausse analogie, Sophisme d'association, Amphibologie, Equivoque, Ambiguïté narrative) — le **groupe ML complet, borné et doc-vetté** :

| pk | sophisme (text_fr) | sous-famille | shape | DirectRef | ExceptionRef | MappingType | conf |
|---:|---|---|---|---|---|---|---|
| 801 | Défaut d'élucidation | Acception vague | dirc | VagueVerbalClassification_Inference | | narrow | HIGH |
| 802 | Indéfinissabilité | Acception vague | dirc | VagueVerbalClassification_Inference | | broad | HIGH |
| 803 | Concept essentiellement contesté | Acception vague | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 805 | Incongruité | Acception arbitraire | full | ArbitraryVerbalClassification_Inference | PropertyNotExistant_Conflict | close | HIGH |
| 806 | Définition trop large | Acception arbitraire | full | ArbitraryVerbalClassification_Inference | PropertyNotExistant_Conflict | narrow | HIGH |
| 807 | Définition trop restrictive | Acception arbitraire | full | ArbitraryVerbalClassification_Inference | PropertyNotExistant_Conflict | narrow | HIGH |
| 826 | Définition incohérente | Définition inconsistante | full | Logical_Conflict | VerbalClassification_Inference | close | HIGH |
| 827 | Conditions conflictuelles | Définition inconsistante | dirc | Logical_Conflict | | broad | HIGH |
| 828 | Concept volé | Définition inconsistante | dirc | InconsistentCommitment_Inference | | close | HIGH |
| 830 | Argument du dictionnaire | Définition inconsistante | full | ArbitraryVerbalClassification_Inference | VerbalClassification_Inference | narrow | HIGH |
| 831 | Sophisme définiste | Définition inconsistante | dirc | VagueVerbalClassification_Inference | | narrow | HIGH |
| 834 | Comparaison abusive | Comparaison abusive | dirc | ExceptionSimilarityCase_Conflict | | broad | **MED** |
| 836 | Classification non exclusive | Comparaison abusive | full | BiasedClassification_Conflict | VerbalClassification_Inference | close | HIGH |
| 838 | Distinction sans différence | Comparaison abusive | full | PropertyNotExistant_Conflict | ArbitraryVerbalClassification_Inference | broad | HIGH |
| 841 | Analogie étendue | Fausse analogie | full | DifferencesUndermineSimilarity_Conflict | Analogy_Inference | narrow | HIGH |
| 842 | Argument de la similarité fallacieuse | Fausse analogie | full | DifferencesUndermineSimilarity_Conflict | Analogy_Inference | broad | HIGH |
| 843 | Fausse équivalence | Fausse analogie | full | DifferencesUndermineSimilarity_Conflict | Analogy_Inference | close | HIGH |
| 844 | Sophisme d'association | Sophisme d'association | full | BiasedClassification_Conflict | VerbalClassification_Inference | close | HIGH |
| 845 | Amalgame | Sophisme d'association | full | ExceptionSimilarityCase_Conflict | ArbitraryVerbalClassification_Inference | close | HIGH |
| 847 | Amphibologie | Amphibologie | exc | | VerbalClassification_Inference | close | **MED** |
| 849 | Sophisme de portée modale | Amphibologie | full | Logical_Conflict | Deductive_Inference | close | HIGH |
| 850 | Glissement du quantificateur | Amphibologie | full | Logical_Conflict | Deductive_Inference | narrow | HIGH |
| 851 | Accent | Amphibologie | dirc | ArbitraryVerbalClassification_Inference | | close | HIGH |
| 852 | Contraste illicite | Amphibologie | dirc | ArbitraryVerbalClassification_Inference | | narrow | HIGH |
| 854 | Barbarisme | Amphibologie | dirc | VagueVerbalClassification_Inference | | narrow | HIGH |
| 855 | Équivoque | Equivoque | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 857 | Fausse précision | Equivoque | dirc | ArbitraryVerbalClassification_Inference | | close | HIGH |
| 859 | Argument de la barbe | Equivoque | exc | | VerbalSlipperySlope_Inference | close | HIGH |
| 860 | Glissement lexical polysémique | Equivoque | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 862 | Homonymie | Equivoque | dirc | VagueVerbalClassification_Inference | | broad | HIGH |
| 863 | Hétérosémie | Equivoque | dirc | VagueVerbalClassification_Inference | | narrow | HIGH |
| 864 | Sophisme jingle-jangle | Equivoque | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 865 | Changement sémantique | Equivoque | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 866 | Métonymie | Equivoque | dirc | VerbalClassification_Inference | | broad | HIGH |
| 867 | Sophisme de confusion type-exemple | Equivoque | dirc | Example_Inference | | close | HIGH |
| 875 | Sophisme de la motte castrale | Equivoque | dirc | VagueVerbalClassification_Inference | | close | HIGH |
| 877 | Fausse implication | Ambiguïté narrative | dirc | ConflictingGoals_Conflict | | narrow | HIGH |
| 878 | Argument par l'insinuation | Ambiguïté narrative | dirc | ConflictingGoals_Conflict | | close | HIGH |
| 879 | Compliment empoisonné | Ambiguïté narrative | dirc | OpposedCommitment_Conflict | | close | HIGH |
| 880 | Indiscrétion anonyme | Ambiguïté narrative | full | LackOfCompleteKnowledge_Conflict | PositionToKnow_Inference | narrow | HIGH |
| 881 | Propagande grise | Ambiguïté narrative | full | ExpertiseInconsistency_Conflict | ExpertOpinion_Inference | narrow | HIGH |
| 882 | Campagne de murmures | Ambiguïté narrative | full | LackOfCompleteKnowledge_Conflict | PositionToKnow_Inference | narrow | HIGH |
| 883 | Propagande noire | Ambiguïté narrative | full | ExpertiseInconsistency_Conflict | ExpertOpinion_Inference | close | HIGH |
| 884 | Interrogatoire clandestin | Ambiguïté narrative | full | ConflictingGoals_Conflict | PracticalReasoning_Inference | narrow | HIGH |
| 885 | Prêcher le faux pour savoir le vrai | Ambiguïté narrative | dirc | Bias_Inference | | narrow | HIGH |
| 886 | Polytélie | Ambiguïté narrative | dirc | ConflictingGoals_Conflict | | close | HIGH |

**shapes = dirc 25 / full 19 / exc 2** (PAS uniforme — le census n'applique PAS l'heuristique simpliste « direct-conflict » ici ; les docs rattés ont été vettés avec un mix). **MappingType = close 23 / narrow 16 / broad 7**.

**Contenu transcrit VERBATIM** des docs cluster rattés (PR-1..PR-12), lignes `Proposal:` explicites par feuille — **0 re-modélisation**, **0 token novel** (whitelist native 60, #677).

**Couche après write** : skos **70 → 116**, fully-modeled **70 → 116**, attack **145 inchangé**, attack-only **75 → 29** (= les 29 non-ML, familles sans doc vetté pour ce PK → re-modélisation fraîche, tranche P3).

**2 lignes FAIL-LOUD partielles** (sérialisées telles que proposées) : **834** (DirectRef-only, doc FAIL-LOUD sur le scheme) et **847** (ExceptionRef-only, doc FAIL-LOUD sur la CQ syntaxique) — **MED**, ~~à revoir par ai-01~~ **arbitrées 2026-08-30 : RATIFIÉES, `AIF_skosOther` rempli** (§2 Arbitrage).

**1 flag sémantique** : 805-807 placent `PropertyNotExistant_Conflict` en **ExceptionRef** (cf. §3) — ~~à confirmer~~ **arbitré 2026-08-30 : VERBATIM sans swap + note de divergence dans `AIF_skosOther`** (§2 Arbitrage).

---

## 1. Fondation

- **Direction P2** (census §5 Priorite 2) : *miroir* de P1. P1 = back-fill attack sur skos-only ; **P2 = deep-sérialise skos sur attack-only**. Contenu vetté déjà écrit dans les docs cluster → transcription, pas de re-modélisation.
- **Méthode** (#769 §2, héritée) : chaque ligne dérive de **sa propre** signature, 0 héritage d'anchor. Ici le contenu provient des docs cluster rattés via leurs lignes `Proposal:` explicites (0 fabrication, #677).
- **Colonne sémantique (lock prod)** : scheme légitime → `ExceptionRef` ; CQ/conflict ou scheme défectueux → `DirectRef`. Ancres : 839 (Exc=`Analogy_Inference`, Dir=`DifferencesUndermineSimilarity_Conflict` = exception à analogie) et 800 (direct-conflict, Dir-only).
- **Whitelist native** : 60 tokens en prod. P2 vérifie **0 novel** — chaque token proposé existe déjà dans une cellule `AIF_skos*` en prod (test `--write` gated refait ce check).

---

## 2. Les 46 lignes — transcrites des docs rattés

Contenu extrait des docs cluster (PR-1..PR-12), lignes `Proposal:` par feuille. Famille uniforme « **Abus de langage** », attack layer **déjà en prod** (undercut/RA majoritaire ; **838 + 843 = undermine/I**), cette tranche ajoute **la couche skos seule**.

**2 shapes partielles (FAIL-LOUD doc, sérialisées telles que proposées)** :

| pk | shape | ce qui est rempli | pourquoi partiel |
|---:|---|---|---|
| 834 | dirc | DirectRef seulement | doc `faulty-comparison` FAIL-LOUD sur le scheme (l'exception n'a pas de scheme légitime candidat) → DirectRef-loose, pas d'ExceptionRef |
| 847 | exc | ExceptionRef seulement | doc `amphibologie` FAIL-LOUD sur la CQ syntaxique (pas de native CQ) → ExceptionRef légitime, DirectRef indisponible |

**1 flag sémantique — `PropertyNotExistant_Conflict` en ExceptionRef (805-807)** : contrairement au pattern standard (ExceptionRef = scheme légitime), le doc `arbitrary-definition` propose `PropertyNotExistant_Conflict` comme ExceptionRef. Sémantique : « la propriété invoquée n'existe pas » est **lui-même un déclencheur de défaut** — la définition arbitraire est une exception à la classification verbale déclenchée par la non-existence de la propriété. Token natif (0 fabrication) mais **sémantique non-standard à faire confirmer par ai-01**.

### ⚖️ Arbitrage rendu (ai-01, 2026-08-30, `msg-20260829T222753-seo82u`) — les 3 flags clos

- **Flags 1-2 (834, 847) : RATIFIÉ — `AIF_skosOther` REMPLI.** Raison d'ai-01 : une ligne FAIL-LOUD dont le `AIF_skosOther` est vide ne se distingue plus d'une ligne ordinaire à référence manquante — or la note **est** la sérialisation du gap (voie #677). Les chaînes exactes des proposals des docs sont transcrites dans les annotations (5 cellules au total, §Arbitration ci-dessous).
- **Flag 3 (805-807) : VERBATIM, PAS DE SWAP + note de divergence.** La prémisse mécanique « hazard d'ordre de colonnes » est **réfutée par la mesure d'ai-01** : les 23 docs `498-aif-*.md` portant une table utilisent tous l'ordre `|DirectRef|ExceptionRef|` — l'inversion est dans le **contenu** de la proposal, pas dans sa mise en table. Décision : sérialiser verbatim (le choix **réversible** — normaliser détruirait l'information que le doc disait autre chose), et inscrire la divergence dans le `AIF_skosOther` des 3 lignes pour qu'elle porte sa propre alerte au lieu de dépendre d'un message : *orientation inversée vs ancre 804 et miroir 838 (même tranche), verbatim conservé, à trancher au GO propriétaire*.
- Les 5 cellules `AIF_skosOther` (834, 847, 805×3) sont portées par `498-reconciliation-p2-annotations.csv` ; l'apply-script les re-quote à l'écriture (valeurs à virgules, convention prod des cellules multi-tokens). Dry-run inchangé sur tous les autres gates (0 mismatch, 1409×104, whitelist 60, skos 70→116).

---

## 3. Distribution + flags

- **shapes** : 25 direct-conflict / 19 full / 2 exception. La famille ML n'est PAS uniforme (contrairement à une lecture rapide du census qui classe tout l'Abus de langage en « direct-conflict »). Les docs vettés distinguent réellement les cas.
- **attack layer déjà en prod** : 44 undercut/RA + **2 undermine/I** (838 Distinction sans différence, 843 Fausse équivalence) — cohérent, ce ne sont pas des anomalies.
- **2 MED load-bearing** : 834, 847 (variantes partielles, FAIL-LOUD doc) — **arbitrées 2026-08-30 (ai-01) : RATIFIÉES, `AIF_skosOther` rempli** (§2 Arbitrage).
- **1 flag sémantique** : 805-807 `PropertyNotExistant_Conflict` en ExceptionRef (§2) — **arbitré 2026-08-30 : VERBATIM sans swap + note de divergence** (§2 Arbitrage).
- **✓ 0 fabrication token** (#677) — whitelist native 60, 0 novel (vérifié par le générateur d'annotations ET re-vérifié par l'apply-script).

---

## 4. Sérialisation

`docs/taxonomy/498-reconciliation-p2-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1g-apply.py` **mais placé sous `docs/taxonomy/`** conformément à la lettre du GO (« écriture dans `docs/taxonomy/` uniquement »). Il peut être déplacé vers `tools/` au moment de la sérialisation prod (il n'importe rien de spécifique à son emplacement).

- lit `498-reconciliation-p2-annotations.csv` et **re-vérifie** la MAP 46/46 ;
- splitters byte-exact (quotes doublées + LF embarqué), cell-fill des seules colonnes `AIF_skosDirectRef`/`AIF_skosExceptionRef`/`AIF_skosMappingType`/`AIF_skosOther` des 46 PK ;
- pre-state : les 46 PK **attack-typed** + **skos vide** (attack-only — le miroir exact de P1g) ;
- re-vérifie la **whitelist native** (0 novel, #677) ;
- preuve **byte-preservation** (0 mismatch), well-formedness 104 cols, BOM+CRLF ;
- `--write` **gaté** (owner), backup `tmp/Fallacies-backup-pre-p2.csv`.

**Lemma de correction de la whitelist** : en prod les cellules skos multi-tokens sont **quotées** (`"A, B"`). Le splitter préserve les guillemets de champ → naïvement `"A` / `B"` ; le script **stripe la paire de guillemets** avant de splitter, ce qui donne la whitelist native correcte (60 tokens, pas 74 pollués).

Dry-run actuel (base `c7634007`) — **tout vert** :

```text
annotation CSV re-verified: 46/46 PKs, >=1 skos cell each, 0 novel token (whitelist 60) — OK
pre-state: all 46 target PKs attack-typed + skos-empty (attack-only): OK
apply_set: 46 PKs x skos cells — shapes: {'direct-conflict': 25, 'full': 19, 'exception': 2}
MappingType distribution: {'skos:narrowMatch': 16, 'skos:broadMatch': 7, 'skos:closeMatch': 23}
byte-preservation mismatches: 0 (must be 0)
well-formedness: 1409 rows x 104 cols, CRLF(True)+BOM(True) preserved
layers: skos 70 -> 116 | attack 145 (unchanged) | fully-modeled 70 -> 116
attack-only after: 29 (was 75; non-ML remainder = 29 for a future P3 tranche, fresh modeling)
DRY-RUN — no file written (pass --write after the owner GO).
```

```bash
python docs/taxonomy/498-reconciliation-p2-apply.py            # dry-run (0 write prod) — ce PR
python docs/taxonomy/498-reconciliation-p2-apply.py --write    # APPLY 46x3 cellules (GATÉ — relais owner)
```

---

## 5. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (`git diff c7634007 -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — transcription verbatim depuis docs rattés ; whitelist native 60, 0 novel.
- ✅ Code=truth : tokens/labels lus du CSV master `c7634007` ; pré-state attack-typed + skos-vide vérifié 46/46.
- ✅ **Contenu vetté** — chaque ligne provient d'une doc cluster ratée (PR-1..PR-12), ligne `Proposal:` explicite.
- ⚠️ **2 partielles FAIL-LOUD** (834, 847) documentées, MED — à revoir par ai-01.
- ⚠️ **1 flag sémantique** `PropertyNotExistant_Conflict`-en-ExceptionRef (805-807) — à confirmer.
- ✅ **Bornes** : 46 ML attack-only uniquement ; **29 non-ML attack-only restants** = tranche P3 (re-modélisation fraîche, hors périmètre de la sérialisation).
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante **gated** (GO distinct owner, cf. dispatch 4kx29h).

**⚠️ Correction de prémisse (à porter à ai-01)** : le dispatch répète « les 20 SUFFIX-ONLY restantes ». Cette prémisse est **périmée** — le back-fill SUFFIX-ONLY (P1 tranches 1b..1g = 36 rows) est **clôturé 2026-07-11** (voir `498-reconciliation-p1g.md`, « Dernière famille SUFFIX-ONLY ») ; attack = **145** sur master, **0 skos-only restant**. La vraie tranche suivante est bien la **direction P2** (skos sur les 75 attack-only), exécutée ici sur les 46 ML.

🤖 Worker po-2024 — réconciliation P2 (sérialisation SKOS des 46 « Abus de langage » attack-only, transcription doc-vettée, GATED).
