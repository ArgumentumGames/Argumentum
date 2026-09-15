# #802 — Proposition des libellés de rôles : inventaire mesuré, cible par rôle/langue

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `c089d526`
**Instrument** : mesure directe des CSV (`Scenarii - Cards.csv` 167×70, `Rules - Cards.csv` 15) —
regex par candidat, découverte empirique des équivalents **avant** comptage (spéc pool : « inventaire
courant défini avant compte »).
**Statut** : **PROPOSITION** — ⛔ `0` CSV écrit, ⛔ `0` prompt modifié. Les libellés sont **ratifiables
tels quels** ; seuls les cas RU/ES demandent une décision owner.

---

## §0 Le résultat en une phrase

Les deux rôles du jeu sont **déjà homogènes dans 6 des 8 langues** (Scenarii ET Rules) — la proposition
les **ratifie tels quels** ; restent **deux décisions owner** : l'ellipse ES du sujet-rôle (27 rangées)
et l'homonymie RU « софист » ; et **un écart structurel EN** : la colonne CSV dit `drawer`, les textes
disent `reader`.

## §1 Rôle 1 — le Baratineur (celui qui défend la thèse)

### Scenarii — occurrences mesurées sur contexte + enjeu/issue + suggestion (167 rangées)

| langue | libellé mesuré | occ. | homogénéité |
|---|---|---:|---|
| FR | `baratineur` (minuscule systématique, 0 majuscule) | 177 | ✔ |
| EN | `smooth talker` | 175 | ✔ — 7 rangées élide le sujet comme l'ES (cf. §3) |
| RU | `софист` | 183 | ✔ homogène — ⚠️ homonymie, cf. §3 |
| PT | `embromador` | 178 | ✔ (`charlatão` ×1 = usage littéral, pas un rôle) |
| ES | `embaucador` | 151 | ⚠️ 27/150 rangées sans sujet-rôle, cf. §3 |
| AR | `المغالِط` (kasra systématique) | 180 | ✔ |
| ZH | `诡辩者` | 181 | ✔ (`骗子` ×2 = « escroc » dans la phrase, légitime) |
| FA | `چرب‌زبان` | 179 | ✔ |

Variantes concurrentes testées et **absentes** : EN sweet talker/charmer 0 · RU болтун/баратинёр/
говорун/краснобай 0 · PT enganador 0 · ES engatusador 0 · AR المخادع/المحتال 0 · ZH 花言巧语 0 ·
FA زبان‌باز 0 (le candidat initial était un artefact de tokenisation du ZWNJ de `چرب‌زبان`).

### Rules — occurrences du libellé du rôle 1 par langue

FR 28 (`baratineur`) · EN 29 (`smooth talker`) · RU 31 (`софист`) · PT 29 (`embromador`) · ES 27
(`embaucador`) · AR 35 (`المغالِط`) · ZH 27 (`诡辩者`) · FA 30 (`چرب‌زبان`) — même libellé que les
Scenarii dans les 8 langues ; les écarts de compte (27-35) suivent les reformulations des règles,
pas des changements de vocabulaire.

## §2 Rôle 2 — le Piocheur (celui qui pioche/relit)

**Le rôle 2 ne vit pas dans les Scenarii** (mesuré : `piocheur` = 0 occurrence dans contexte+enjeu+
suggestion FR ; idem EN) — il vit dans les **Rules** et dans les **noms de colonnes** du CSV.

| langue | libellé mesuré (Rules) | occ. | note |
|---|---|---:|---|
| FR | `piocheur` | 29 | nom de colonne CSV : `piocheur` ✔ |
| EN | `reader` | 19 | ⚠️ nom de colonne CSV : `drawer` — écart structurel, cf. §4 |
| RU | `Берущий карту` (périphrase « celui qui prend la carte ») | — | pas de nom unique ; cf. §3 |
| PT | `leitor` (lecteur) | 30 | |
| ES | `lector` | 18 | |
| AR | `القارئ` (lecteur) | 35 | |
| ZH | `读者` (lecteur) | 64 | |
| FA | `خواننده` (lecteur) | 55 | |

Convergence mesurée : **6 langues sur 8 rendent le piocheur par « lecteur »** (reader/leitor/lector/
القارئ/读者/خواننده) — le FR « piocheur » (l'acte de piocher la carte) et la périphrase RU sont les
deux exceptions.

## §3 Les deux cas difficiles (décision owner) + l'ellipse EN

**ES — l'ellipse du sujet-rôle (27/150 rangées, +7 EN du même type).** Là où le FR écrit
« **Le baratineur** doit dissuader César… », l'ES écrit « **Disuadirlo** de acercarse a Cleopatra… »
(infinitif impersonnel) — 27 rangées ES élide le sujet-rôle, 7 aussi en EN. Ce n'est pas un mauvais
libellé : c'est une tournure idiomatique. **Options** : (a) *statu quo documenté* — la carte porte le
rôle ailleurs et l'espagnol naturel prime *(recommandé : 0 cellule à réécrire)* ; (b) aligner
« El embaucador debe + infinitif » sur les 27 — parité de formulation entre langues, 27 cellules ES
(+7 EN) à réécrire. **La reco du worker est (a)** ; le propriétaire tranche.

**RU — l'homonymie « софист ».** Le libellé RU du rôle 1 est `софист` (183+31 occurrences, aucune
variante) — mais `софист` signifie aussi « **sophiste** », le concept même que le jeu apprend à
détecter : dans un jeu où l'on dénonce les sophismes, appeler le joueur « le sophiste » est
thématiquement défendable (il endosse le rôle pour l'exercice) mais peut prêter à confusion
carte/joueur. **Options** : (a) *ratifier `софист` tel quel* *(recommandé : homogène, établi, et
l'ambiguïté est productrice — c'est le jeu)* ; (b) changer (p. ex. `профессор баратинёр`-like) =
~214 cellules touchées, à ne faire que sur GO explicite. Le rôle 2 RU n'a **pas de nom unique**
(« Первым Берущим карту… ») — statu quo ou adoption de « Читающий » aligné sur la famille
« lecteur » des 6 autres langues : **reco = statu quo** (la périphrase est claire dans son contexte).

## §4 Écarts structurels documentés (aucune action dans ce grain)

- **EN colonne vs texte** : la colonne Scenarii s'appelle `drawer`, les textes Rules disent
  `reader`. Renommer la colonne = écriture CSV + mapping CsvHelper — **hors périmètre** (zéro CSV) ;
  le libellé public à ratifier est **reader** (celui que lit le joueur).
- **Glossaires prompts (second référentiel)** : les prompts Scenarii/Rules imposent « la cohérence
  des rôles "baratineur" et "piocheur" » (`PromptScenariiTranslateRuAssistant.txt:5`) et « Les rôles
  comme "Baratineur" et "Mixologue" doivent être traduits en équivalents naturels »
  (`PromptRulesTranslateEsUser.txt:11`) — **sans jamais fixer les équivalents**. Proposition associée
  (post-ratification) : figer la table §1/§2 dans les prompts des 7 langues cibles pour verrouiller
  la cohérence aux futures passes DatasetUpdater. **Non exécuté ici.**
- **Mixologue** (variante Bingo) : présent dans Rules FR 1 + `mixolog*` EN/PT/ES 1 chacun — libellé
  unique par langue, homogène, rien à proposer.

## §5 Ce que cette proposition n'établit pas

⛔ Zéro CSV, zéro prompt, zéro template écrit · ⛔ aucune des deux décisions RU/ES prise ici (feuille
§3 pour jsboige) · ⛔ ne mesure pas les mindmaps ni les PDF générés (les libellés des textes y
dérivent du CSV mesuré) · le renommage de colonne `drawer` est documenté, pas proposé pour exécution.

---

*po-2024 — pool #458, grain ⑩. Le worker mesure et propose ; les libellés et les deux cas difficiles
sont à l'owner.*
