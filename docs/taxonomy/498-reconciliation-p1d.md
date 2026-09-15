# #498 — AIF two-layer reconciliation, P1 tranche-1d (SUFFIX-ONLY, famille « Erreur mathématique »)

**Worker** po-2024 · **Date** 2026-07-11 · **Base** master `7406bb8e` (tranche-1 en prod via #771 ; tranche-1b proposition #773 mergée, write prod #776 en attente ; tranche-1c proposition #775 en revue) · **Statut** proposition **gated** (0 write prod CSV dans ce PR) · **Track** GO ai-01 « suite SUFFIX-ONLY par sous-lots famille (autonome) », couvert par le pilote GO #498 (jsboige 2026-06-17).

> Scope de ce PR : **docs + apply-script (dry-run)**. Aucune cellule du CSV de production n'est modifiée dans le diff. La sérialisation prod suit le flow #753/#760 (gate ai-01), après revue de cette proposition.

---

## 0. TL;DR

Suite de la réconciliation P1 (back-fill de la couche attack pour les 52 lignes skos-only). La **tranche-1** (#769/#771) a livré les **14 PRECEDENT** ; la **tranche-1b** (#773/#776) livre **2 PREC-TIE + 5 SUFFIX-ONLY** ; la **tranche-1c** (#775, en revue) livre la famille **« Erreur de raisonnement »** (7 SUFFIX-ONLY). Ce PR livre la **tranche-1d = 4 lignes SUFFIX-ONLY**, la **famille « Erreur mathématique » complète** :

| pk | sophisme | sous-famille |
|---:|---|---|
| 595 | Généralisation abusive | Généralisation abusive |
| 614 | Sophisme de l'accident | Généralisation abusive / Sophisme de l'accident |
| 632 | Interprétation quantitative erronée | Mauvaise interprétation |
| 677 | Pente glissante | Résultat invalide / Imprécision |

**Aucun** token de ces 4 lignes n'a de précédent exact in-set (vérifié précédent-check contre l'ensemble fully-modeled élargi : tous `precedent_votes={}`). Modélisation Walton **au cas par cas** depuis le scheme propre + `desc_fr` (« que défait le CQ »). Le **prior de suffixe est proscrit** (contre-preuve in-set pk804 `_Conflict`→undercut, audit #770).

Distribution : **4 undercut / 0 undermine / 0 rebut**. Comme « Erreur de raisonnement » (tranche-1c), c'est une famille dont le défaut porte **structurellement sur le pas inférentiel** (§3 : signature de famille vérifiée, pas défaut par défaut ; ancres in-set pk858 + principe ASPIC+ exception=undercut ; **une** nuance flaguée MED). `attackType` = **jugement neuf** (0-risque token, pas 0-risque modélisation) ; **0 fabrication de token** (#677). `attackedNode` déterministe (#707§4 a) → tous **RA-node**.

Back-fill **+4** : attack-typed **107 → 111** (baseline dynamique, §5). Reste après ce sous-lot : **20 SUFFIX-ONLY** (Tricherie 8, Abus de langage 7, Insuffisance 5) — dont la distribution ne sera **pas** uniforme (§3).

---

## 1. Fondation (héritée de #769 + #770)

- **Méthode** (#769 §2) : l'`attackType` de chaque ligne skos-only se dérive de **sa propre** signature skos, ancrée sur les **18 lignes fully-modeled** (ground truth token→type). Pas d'héritage mécanique du sous-sous anchor (0/19 alignées, #769 §1).
- **Audit des 18 anchors** (#770) : **16 CLEAN / 2 SOFT / 0 erreur franche, node-map 18/18 propre**. Deux trouvailles de l'audit **ancrent directement** ce sous-lot inférentiel :
  1. **suffixe non décisif** (pk804 `PropertyNotExistant_Conflict`→undercut) → le prior de suffixe est proscrit ;
  2. **slippery-slope penche undercut** (pk858 `VerbalSlipperySlope_Inference`→undercut) → ancre in-set directe (même genre) pour **pk677**.
- **Précédent-check tranche-1d** : les 4 tokens-signatures ont été confrontés à l'**ensemble fully-modeled élargi** (39 lignes = 18 anchors natifs + 21 dérivées tranche-1/1b) ; **aucun** partage un token exact → `precedent_votes={}` pour les 4 → **SUFFIX-ONLY confirmé** (la dérivation ne repose sur aucun héritage, seulement sur la modélisation Walton propre + principes ASPIC+).

Node déterministe (tous undercut ici) :

| attackType | attackedNode | composant attaqué |
|---|---|---|
| undercut | `RA-node` | inférence (la règle ne s'applique pas) |
| undermine | `I-node` | prémisse (acceptabilité contestée) |
| rebut | `CA-node` | conclusion (contre-conclusion / blocage du dialogue) |

---

## 2. SUFFIX-ONLY — famille « Erreur mathématique », modélisation Walton (4 lignes)

Aucun token de ces lignes n'a de précédent in-set. On modélise **depuis le scheme propre + `desc_fr`** (« que défait le CQ »). **Prior de suffixe proscrit.**

| pk | sophisme | scheme propre (signature skos) | `desc_fr` (abrégé) | → type | node | conf |
|---:|---|---|---|---|---|---|
| 595 | Généralisation abusive | `Example_Inference` + `InductiveInference_Scheme` + `EvidenceToHypothesis_Inference` | « une généralisation qui n'est pas valable dans le cas considéré » | **undercut** | RA | HIGH |
| 614 | Sophisme de l'accident | `ExceptionalCase_Inference` + `ExceptionSimilarityCase_Conflict` | « vous appliquez une règle générale à un cas particulier, sans tenir compte des exceptions » | **undercut** | RA | HIGH |
| 632 | Interprétation quantitative erronée | `EvidenceToHypothesis_Inference` | « vous établissez des relations quantitatives inexactes entre des données » | **undercut** | RA | MED |
| 677 | Pente glissante | slippery-slope inferences (Precedent/Full/Causal) + `WeakestLink_Conflict` | « vous rejetez une proposition en prédisant une série peu probable d'événements négatifs » | **undercut** | RA | HIGH |

- **595** — le **pas inductif** (des exemples/données vers une hypothèse générale) est injustifié : les trois tokens sont des schemes inférentiels (Example/Inductive/EvidenceToHypothesis). Le défaut est la **généralisation elle-même**, pas la fausseté d'une prémisse (undermine écarté) ni une contre-conclusion (rebut écarté). Principe ASPIC+ : les CQ d'une généralisation inductive challengent la **suffisance/validité du pas inductif** → undercutters → RA.
- **614** — **cas d'école de l'undercut** (secundum quid / sophisme de l'accident). En ASPIC+, une **exception à une règle défaisable est exactement un undercutter** : elle défait l'**application** de la règle à ce cas (la garantie ne tient pas ici), sans que les prémisses (la règle générale, le cas) soient fausses. `ExceptionalCase_Inference` + `ExceptionSimilarityCase_Conflict` = l'exception vise la règle d'inférence → RA. Ni undermine (prémisses vraies) ni rebut (pas de contre-conclusion, juste une mauvaise application).
- **632** — l'inférence des **données (evidence) vers une relation/hypothèse quantitative** est erronée → `EvidenceToHypothesis_Inference` est l'inférence challengée → RA. **Flag MED** : alternative undermine possible si la « relation quantitative inexacte » est lue comme une **prémisse fausse** (un chiffre fabriqué) ; écartée car le nom (« Interprétation … erronée ») et la sous-famille (« Mauvaise interprétation ») désignent un **acte inférentiel** (dériver une relation depuis des données), pas l'assertion d'un datum faux.
- **677** — pente glissante : la **chaîne** inférentielle (chaque maillon → le suivant) est le défaut ; `WeakestLink_Conflict` challenge le maillon le plus faible → attaque de la garantie → RA. **Ancre in-set pk858** `VerbalSlipperySlope_Inference`→undercut (audit #770, même genre de sophisme, vérité native). Le mot « rejeter » (`desc_fr`) pourrait évoquer un **rebut** (contre-conclusion), MAIS le rejet **repose sur la chaîne faible** (undercut de la garantie), pas sur une contre-argumentation indépendante avec ses propres fondements → **undercut, pas rebut**. Cohérent avec tranche-1c pk705 (Pente glissante, undercut) et le contre-exemple canonique mémoire (677/705 : ne pas hériter l'undermine, le modeling propre donne undercut).

Détail complet en colonne `justification` de [`498-reconciliation-p1d-annotations.csv`](498-reconciliation-p1d-annotations.csv).

---

## 3. Uniformité undercut — signature de famille (2ᵉ famille inférentielle), pas défaut par défaut

Ce sous-lot ressort **4/4 undercut**. C'est la **deuxième** famille uniforme-undercut après « Erreur de raisonnement » (tranche-1c) — d'où l'importance d'être explicite : ce n'est **pas** un biais du modélisateur vers undercut, c'est la **structure de la taxonomie**.

- **Sémantique** : « Erreur mathématique » regroupe des défauts du **pas inférentiel** — généralisation inductive invalide (595), mauvaise application d'une règle avec exception (614), interprétation quantitative erronée (632), chaîne de pente glissante (677). Le défaut porte structurellement sur la **règle d'inférence** (undercut/RA), pas sur l'acceptabilité d'une prémisse (undermine/I) ni sur une contre-conclusion/blocage de dialogue (rebut/CA).
- **Ancrage in-set + théorique** : pk677 est ancré sur l'in-set pk858 (slippery-slope→undercut, audit #770, vérité native) ; pk614 est le **cas canonique** de l'undercut en théorie de l'argumentation (une exception à une règle défaisable **est** un undercutter en ASPIC+). La famille hérite de cette signature par **cohérence vérifiée**, pas par prior de suffixe (proscrit).
- **Garde-fou** : chaque ligne est modélisée séparément et **une nuance est flaguée MED** avec son alternative explicite — **632** (vs undermine si lu comme un datum faux ; écarté par « interprétation »/« mauvaise interprétation » = acte inférentiel). Les trois autres (595/614/677) sont HIGH.
- **Preuve que la méthode discrimine** : les **20 SUFFIX-ONLY restants ne seront pas uniformes**. Anticipé (à modéliser au cas par cas dans les prochains sous-lots) : **Abus de langage** contient **814 Faux dilemme** (prémisse disjonctive fausse → **undermine**/I) à côté d'undercut (analogie/classification) ; **Tricherie** contient **888 Présentation trompeuse des faits** (faits déformés → **undermine**/I) et des mouvements de dialogue (973 déplacement des critères) potentiellement rebut ; **Insuffisance** est précisément à la frontière undermine/undercut (evidence insuffisante). La méthode type undercut **ici** parce que la famille est inférentielle, pas par défaut.

**Le MED-flag load-bearing à revoir en priorité par ai-01 : 632.**

---

## 4. Justification par ligne

Voir colonne `justification` du CSV machine-readable. Mécanisme unique de ce sous-lot : **la règle d'inférence ne tient pas (undercut/RA)** —

- **saut inductif invalide** : 595 (généralisation abusive) ;
- **exception défait l'application de la règle** : 614 (sophisme de l'accident) ;
- **inférence données⇒quantité erronée** : 632 (interprétation quantitative) ;
- **chaîne inférentielle faible** : 677 (pente glissante).

---

## 5. Sérialisation (flow #753/#760)

`tools/498-p1d-apply.py` — **gated, dry-run par défaut**, miroir de `tools/498-p1c-apply.py` :

- lit `498-reconciliation-p1d-annotations.csv` et **re-vérifie** que sa carte interne concorde 4/4 (assertion load-bearing) ;
- splitters byte-exact (guillemets doublés + LF encadrés), cell-fill des seules colonnes `AIF_attackType`/`AIF_attackedNode` des 4 PK ;
- pre-state : les 4 PK **vides** (fill, pas overwrite) + **portent une skos** (back-fill skos-only) ;
- preuve de **byte-preservation** (0 mismatch hors les 2 cellules × 4 lignes), well-formedness 104 cols, BOM+CRLF préservés ;
- `--write` **gaté** (ai-01), backup `tmp/Fallacies-backup-pre-p1d.csv` avant écriture pour vérif indépendante.

**Baseline dynamique (pilotée par les `--write`, pas par les merges).** Les 4 PK sont **disjoints** des 14 de la tranche-1 (#771), des 7 de la tranche-1b (#773/#776) et des 7 de la tranche-1c (#775). La baseline dépend des **sérialisations prod `--write` effectuées**, pas des PR mergées : chaque proposition merge d'abord (docs + dry-run), son `--write` est une **étape gated séparée** (relais ai-01). Le script lit le compte **réel** et rapporte `total → total+4` sans jamais coder en dur la baseline (ex. `107→111` si seule la tranche-1 est écrite ; `114→118` si 1b l'est aussi ; etc.). L'ordre est donc sans impact sur la correction (cellules disjointes). Dry-run actuel (master `7406bb8e`, baseline 107) : `107 → 111`, delta **+60 B**, **0 mismatch**, **1409×104**, CRLF+BOM préservés.

```bash
python tools/498-p1d-apply.py            # dry-run (0 write prod) — ce PR
python tools/498-p1d-apply.py --write    # APPLY 4 cellules (GATÉ — relais ai-01)
```

---

## 6. Bornes du gate

- ✅ **0 write prod CSV** dans ce PR (docs + apply-script dry-run uniquement — `git diff 7406bb8e -- Cards/…/Taxonomy.csv` vide).
- ✅ **0 fabrication token #677** — aucun token AIF créé ; on type des lignes déjà skos-vettées.
- ✅ Code=truth : tokens/labels/`desc_fr` lus du CSV master `7406bb8e` ; précédents = les 18 fully-modeled (audités CLEAN, #770) ; SUFFIX-ONLY confirmé par précédent-check (tous `precedent_votes={}`).
- ✅ Prior de suffixe **proscrit** ; uniformité undercut = signature de famille vérifiée (2ᵉ famille inférentielle), chaque ligne modélisée, 1 MED flaguée, méthode discriminante (§3).
- ❌ #674/#666/#596 non touchés (HOLD). Pas de self-merge — verdict QA ai-01.
- ⏸️ Sérialisation prod = étape suivante gated (relais ai-01), pas dans ce PR.

🤖 Worker po-2024 — réconciliation P1 tranche-1d (famille « Erreur mathématique », 4 SUFFIX-ONLY).
