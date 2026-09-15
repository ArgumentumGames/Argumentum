# #192 — Feuille de décision `link_*` i18n : 3 822 cellules, l'automatable déjà isolé (Wikipedia 88 %/74 %), 2 coches

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base re-mesure** : master `6fbde739`
**Grain** : pool #458 renouvelé (c.5666260217), grain ④. **Le dossier de décision demandé existe
déjà** — le scoping #192 ([`192-link-i18n-scoping.md`](../quality/192-link-i18n-scoping.md), 13/07,
mergé) apporte le chiffrage par langue×corpus, le split automatable/manuel et 4 options avec reco.
Ce tick le **déclare soldé-sur-existant** et livre la seule pièce manquante : la feuille à cocher.
**⛔ 0 write sans GO nommé.**

---

## §0 La lacune, re-mesurée ce jour (contrôle d'immobilité)

Re-mesure indépendante sur `6fbde739` (population : rangées où `link_fr` est non vide — Fallacies
637/1408, Virtues 216/223 ; Scenarii : **aucune colonne link**) : les comptes recoupent le scoping
#192 **à l'unité près** — lacune totale inchangée :

| Corpus | en | ru | pt | es | ar | fa | zh | Total |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| Fallacies | 24 | 533 | 550 | 546 | 550 | 556 | 561 | **3 320** |
| Virtues | 22+4 FR-URL | 20+6 | 31 | 63+2 | 124 | 116+3 | 111 | **502** |
| | | | | | | | | **≈ 3 822** |

(les « +n FR-URL » = cellules copiant l'URL FR sans localisation — même traitement, cf. #192 §0.)

## §1 La nature du travail (pourquoi ce n'est pas de la traduction)

Résoudre un `link_*` = **trouver l'article équivalent** dans la langue cible, pas traduire une
chaîne. Le scoping #192 §1 isole ce qui est automatisable : les FR-liens **Wikipedia/Wiktionary**
(Fallacies **562/637 = 88 %**, Virtues **161/216 = 74 %**) se résolvent par l'**API langlinks**
(zéro risque d'hallucination — l'API confirme l'article) ; les sources FR institutionnelles
(cortecs.org, service-public.fr, persee.fr… — 75 + 55 cellules) **n'ont pas d'équivalent à
traduire** : candidats drop (option D, éditorial).

## §2 Les options (reprises de #192 §2 — le détail y vit)

| | Option | Effort | Statut |
|---|---|---|---|
| **A** | Différer (statu quo documenté) | 0 | ✅ défaut pré-tag — la lacune est préexistante, documentée, ne bloque rien (prose 100 % propre) |
| **B** | Passe candidates LLM + vérif humaine | ~1 259 appels + vérif | ⚠️ risque d'hallucination de titres ; sans objet si C tourne |
| **C** | **Script API langlinks Wikipedia** (remplit uniquement les résolutions API-confirmées) | script + spot-check | ✅ **best ROI post-tag** — zéro hallucination, couvre le sous-ensemble 88 %/74 % |
| **D** | Vider les liens FR-institutionnels des colonnes non-FR | petit | ⚠️ éditorial : « un lien FR mort est pire que pas de lien » pour un lecteur natif |

## §3 Les coches (owner — 2 décisions)

- [ ] **Timing C** : ☐ post-tag v2.0.0 *(reco — changement corpus gated)* · ☐ statu quo indéfini
- [ ] **D éditorial** : ☐ vider les sources FR-institutionnelles des colonnes non-FR *(reco de #192, à exécuter avec C si coché)* · ☐ les garder (le lecteur FR-contexte peut encore les utiliser)

Si cochées : exécution worker en passe unique scriptée (langlinks + clear D), 0 write sans ce GO,
diff borné aux colonnes `link_*`, vérif `--report` du script #192 avant/après.

## §4 Ce que cette feuille n'établit pas

⛔ Le traitement n'est ni exécuté ni préparé au-delà de l'existant (le script #192 est déjà
committé) · ⛔ le taux de réussite attendu de C (l'API ne résoudra pas tout — le résidu exact se
mesurera au run) est SUPPOSÉ « majorité » d'après la structure du corpus, pas mesuré · ⛔ aucune
décision prise (les reco sont celles de #192, re-validées par la re-mesure d'immobilité) · ⛔ les
15+11 FR_URL nominatifs vivent en détail dans #192 (samples pk par pk).

---
*po-2024 — pool #458 renouvelé (c.5666260217), grain ④ soldé-sur-existant + feuille. Le grain
citait la re-mesure i18n du 13/09 comme source ; le scoping #192 du 13/07 porte déjà chiffrage,
options et reco — cette feuille en est l'extraction à cocher, comptes re-vérifiés immobiles le
15/09 au soir.*
