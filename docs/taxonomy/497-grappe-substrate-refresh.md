# #497 grappe substrate — REFRESH coverage report (po-2024, code=truth master `b442c658`)

**Objet** : re-run du générateur `497-grappe-substrate.py` (ai-01 lane, inchangé) contre le CSV
Fallacies courant, pour actualiser le substrat de curation grappe-à-grappe. Le doc
`497-grappe-substrate.md` (snapshot initial) mesurait **22 crossLinks** — c'était un état
pré-modelling. Ce refresh quantifie la densité relationnelle réelle présente aujourd'hui.

**Posture** : read-only, 0 fabrication. Le script re-agrège les `crossLink_*` *déjà présents*
dans le CSV de prod (que les tranches AIF/crosslinks ont remplis via #753/#760/#763 + tranches
P1). Aucun nouveau lien n'est proposé ici — on mesure ce qui existe.

**Reproductibilité** : `python docs/taxonomy/497-grappe-substrate.py` (depuis la racine repo).
Écrase `497-grappe-inventory.csv` + `497-responding-grappes.csv` (artefacts régénérables).

---

## 1. Delta coverage (avant → après refresh)

| Métrique | Snapshot initial (stale) | Refresh (master `b442c658`) | Δ |
|----------|-------------------------:|----------------------------:|---:|
| Bubble-up grappe-edges | 16 | **107** | +91 |
| dont inter-familles | 14 | (cf. §3) | — |
| Σ crossLinks feuilles (cellules `crossLink_*`) | ~22 | **~1179** | +1157 |
| Grappes depth-2 | 21 | 21 | 0 (stable) |
| Nœuds total | 1408 | 1408 | 0 (stable) |

> Cohérent avec le README ontology (§2 Layer 2) : **844 fallacies (59.9 %)** portent ≥1 crossLink,
> **1985 raw** assertions (OWL dédup = 1734 distinct). Le script capte les cellules CSV feuilles
> (~1179 cellules non-vides agrégées en 107 arêtes grappe↔grappe).

### Per-grappe coverage (colonne `existing_crosslinks`)

| Grappe | Famille | size | cl stale | cl refreshed | Δ |
|--------|---------|-----:|---------:|-------------:|---:|
| 2.3 Manipulation mentale | Influence | 239 | 7 | **195** | +188 |
| 6.3 Raisonnement biaisé | Tricherie | 257 | 0 | **204** | +204 |
| 6.1 Présentation trompeuse | Tricherie | 85 | 0 | **84** | +84 |
| 2.1 Technique rhétorique | Influence | 123 | 0 | **81** | +81 |
| 6.2 Déplacement des critères | Tricherie | 51 | 2 | 53 | +51 |
| 1.1 Généralisation hâtive | Insuffisance | 68 | 2 | 44 | +42 |
| 7.3 Ad hominem | Obstruction | 46 | 2 | 40 | +38 |
| 1.2 Préjugé | Insuffisance | 63 | 0 | 40 | +40 |
| 4.3 Déduction invalide | Err. raisonnement | 40 | 0 | 32 | +32 |
| 4.2 Composition fautive | Err. raisonnement | 32 | 0 | 30 | +30 |
| 5.1 Définition biaisée | Abus de langage | 34 | 2 | 28 | +26 |
| 1.3 Surinterprétation | Insuffisance | 42 | 3 | 28 | +25 |
| 5.3 Ambiguïté | Abus de langage | 41 | 0 | 26 | +26 |
| 3.1 Généralisation abusive | Err. mathématique | 37 | 0 | 36 | +36 |
| 7.2 Sabotage du débat | Obstruction | 48 | 0 | 31 | +31 |
| 2.2 Appel à l'émotion | Influence | 57 | 2 | 37 | +35 |
| 3.2 Interprétation quantitative | Err. mathématique | 34 | 0 | 24 | +24 |
| 4.1 Causalité douteuse | Err. raisonnement | 29 | 0 | 20 | +20 |
| 7.1 Refus du débat | Obstruction | 31 | 1 | 23 | +22 |
| 3.3 Conclusion math. invalide | Err. mathématique | 30 | 1 | 14 | +13 |
| 5.2 Comparaison fallacieuse | Abus de langage | 13 | 0 | 9 | +9 |

---

## 2. Finding structurel central — la densité est INTRA-grappe

Les **top arêtes** du bubble-up sont **quasi toutes intra-grappe** (une grappe vers elle-même) :

| Poids | src → tgt | Lecture |
|------:|-----------|---------|
| 137× | 2.3 → 2.3 (Manipulation mentale) | liens internes (technique ↔ technique) |
| 134× | 6.3 → 6.3 (Raisonnement biaisé) | liens internes (biais ↔ biais) |
| 49× | 6.1 → 6.1 (Présentation trompeuse) | liens internes |
| 36× | 2.1 → 2.1 (Technique rhétorique) | liens internes |
| 28× | 1.1 → 1.1 (Généralisation hâtive) | liens internes |
| 26× | 7.3 → 7.3 (Ad hominem) | liens internes |

**Conséquence curationnelle** : la densification relationnelle s'est concentrée *au sein* des
grandes grappes (notamment `2.3 Manipulation mentale` et `6.3 Raisonnement biaisé`, qui sont aussi
les deux plus grosses : 239 et 257 nœuds). Le **maillage inter-grappes** — les ponts entre familles
distinctes — reste **clairsemé** malgré la croissance brute.

C'est précisément le signal inverse du snapshot initial : celui-ci pointait l'ancre inter-familles
`2.3 ↔ 6.3` (poids 7) comme « point d'ancrage du chantier ». Après densification, cette ancre
inter-familles est **toujours** le pont principal — mais les 130+ autres arêtes sont de l'auto-link
intra-grappe.

---

## 3. Ponts inter-familles (le vrai objet de la curation #497)

Le bubble-up révèle que les arêtes *inter-familles* significatives sont rares. La plus marquante
au-delà de l'ancre historique :

- `2.1 Technique rhétorique [Influence] → 5 Abus de langage` (26×) — pont rhétorique↔langagier.

L'affinité lexicale raffinée (65 candidats cross-family, Jaccard sur vocabulaire distinctif
hors-stopwords argumentatifs) confirme les **pistes plausibles** déjà identifiées au snapshot
initial (à valider en curation, pas des liens auto-déclarés) :

| Jaccard | Grappe A | Grappe B | termes distinctifs partagés |
|--------:|----------|----------|------------------------------|
| 0.19 | Généralisation hâtive [Insuff.] | Refus du débat [Obstr.] | preuve, prétendez, rejetez, fournir |
| 0.16 | Interprétation quantitative [Err.math] | Causalité douteuse [Err.rais.] | causalité, cause, résultat, attribuez, tort |
| 0.16 | Appel à l'émotion [Infl.] | Ad hominem [Obstr.] | adversaire, discréditer, interlocuteur, lieu, proposition |
| 0.16 | Conclusion math. invalide [Err.math] | Ambiguïté [Abus] | argumentation, plusieurs, faites, utilisez |
| 0.13 | Technique rhétorique [Infl.] | Définition biaisée [Abus] | argumentation, langage, sens, utilisez |
| 0.13 | Généralisation hâtive [Insuff.] | Raisonnement biaisé [Trich.] | croyances, monde, pensez, considérez |

> ⚠ Heuristique = **générateur de pistes, pas preuve**. Le bruit résiduel (verbes de second-personne
> « faites », « utilisez », « votre ») reste présent. Les pistes « causales » (Interprétation quant
> ↔ Causalité douteuse) et « attaque-affect » (Appel émotion ↔ Ad hominem) sont les plus défendables.

---

## 4. Recommandation pour la curation multi-sessions

1. **Ne pas densifier les intra-grappes** (2.3, 6.3 déjà saturées : 195 et 204 liens internes).
   Le ROI marginal y est faible.
2. **Cibler les ponts inter-familles** — c'est là que le signal relationnel manque le plus.
   L'ancre `2.3 ↔ 6.3` (PredatesOn, « manipulation exploite biais ») reste le point d'entrée
   naturel ; la étendre en *mapping systématique* technique↔biais (déjà partiellement tracée).
3. **Valider les 2 pistes lexicales défendables** (causales, attaque-affect) en curation
   before toute écriture — ce sont des *propositions*, pas des liens auto-établis.
4. **Gate inchangée** : toute écriture dans le CSV de prod = spot-check ai-01 + nod jsboige.

---

## 5. Gate boundaries (HARD)

- ❌ Aucune écriture dans le CSV de prod, OWL, DB, mindmaps. Post-tag (contenu sous review T&A).
- ✅ Tous les chiffres dérivés **code=truth** (CSV scan master `b442c658`, re-run du script ai-01).
- ✅ Aucun token AIF fabriqué, aucun crosslink inventé. Re-agrégat pur de l'existant.
- ✅ Artefacts régénérables : `497-grappe-inventory.csv`, `497-responding-grappes.csv` (le script
  écrase, idempotent).

Relates : #497 (chantier grappe-à-grappe), #828/#829 (export AIF), #7289 Phase-B (consommateur
aval strate-6), #763 (OWL crosslinks 59.9 %), `497-grappe-substrate.md` (doc méthode initial).

🤖 po-2024 — lane #497 refresh (dispatch ai-01 `msg-ucdwi7`)
