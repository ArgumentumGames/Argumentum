# #802 — Feuille de décision owner : libellés de rôles RU / ES (2 coches)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `608a0fd9`
**Source** : extraction du dossier mergé **#1388** (`5bdbdfd1`) —
[`802-libelles-roles-2026-09-15.md`](802-libelles-roles-2026-09-15.md) §1-§3 ; comptes
RAPPORTÉS du dossier (mesurés 15/09 sur `c089d526`, revus par ai-01 avant merge).
**Statut** : **support de décision uniquement** — ⛔ 0 CSV, ⛔ 0 prompt, ⛔ 0 corpus touché.

---

## Contexte en trois lignes

L'inventaire est joué : **rôle 1** (le baratineur) est **homogène et ratifiable tel quel dans
les 8 langues** (FR `baratineur` 177 · EN `smooth talker` 175 · RU `софист` 183 · PT
`embromador` 178 · ES `embaucador` 151 · AR `المغالِط` 180 · ZH `诡辩者` 181 · FA `چرب‌زبان`
179 — variantes concurrentes toutes à 0). **Rôle 2** (le piocheur) vit dans les Rules et
converge sur « lecteur » **6/8** (reader/leitor/lector/القارئ/读者/خواننده ; EN colonne `drawer`
vs texte `reader` ; RU périphrase). **Deux points appellent une coche owner** :

---

## Décision A — ES : l'ellipse du sujet-rôle (27/150 rangées, +7 EN du même type)

**Objet cité** — là où le FR écrit « **Le baratineur** doit dissuader César… », l'ES écrit
« **Disuadirlo** de acercarse a Cleopatra… » (infinitif impersonnel) : 27 rangées ES élide le
sujet-rôle ; 7 rangées EN font de même.

| branche | geste | coût |
|---|---|---|
| **(a) statu quo documenté** *(reco worker)* | rien — la carte porte le rôle ailleurs et l'espagnol naturel prime ; l'ellipse est une tournure idiomatique, pas un mauvais libellé | **0 cellule** |
| (b) aligner les formulations | « El embaucador debe + infinitif » sur les 27 (+7 EN) — parité de formulation entre langues | **27 cellules ES + 7 EN** à réécrire (post-tag, GO requis) |

**Coche owner** : ☐ (a) statu quo · ☐ (b) aligner

---

## Décision B — RU : l'homonymie « софист » (+ sous-cas rôle 2)

**Objet cité** — le libellé RU du rôle 1 est `софист` (183 occurrences Scenarii + 31 Rules,
aucune variante) ; or `софист` signifie aussi « **sophiste** », le concept central que le jeu
apprend à dénoncer : appeler le joueur « le sophiste » est thématiquement défendable (il endosse
le rôle pour l'exercice) mais peut prêter à confusion carte/joueur.

| branche | geste | coût |
|---|---|---|
| **(a) ratifier `софист` tel quel** *(reco worker — homogène, établi, l'ambiguïté est productrice : c'est le jeu)* | rien | **0 cellule** |
| (b) changer de libellé | ~214 cellules RU touchées, traduction à revalider | **≈ 214 cellules** (GO explicite requis) |

**Sous-cas — rôle 2 RU** : pas de nom unique (« Первым Берущим карту… », périphrase) ;
(a) **statu quo** *(reco : claire dans son contexte)* ou (b) adopter « Читающий » aligné sur la
famille « lecteur » des 6 autres langues.

**Coche owner** : ☐ (a) ratifier · ☐ (b) changer — rôle 2 : ☐ (a) statu quo · ☐ (b) « Читающий »

---

## Post-ratification (gated, NON exécuté ici)

Figer la table §1/§2 du dossier dans les **prompts DatasetUpdater** des 7 langues : aujourd'hui
ils imposent « la cohérence des rôles » **sans fixer les équivalents**
(`PromptScenariiTranslateRuAssistant.txt:5`, `PromptRulesTranslateEsUser.txt:11`) — un libellé
ratifié mais non figé se re-dérivera à la passe de traduction suivante. ⛔ Geste glossaire =
write → **attend un GO nommé**, séparé de ces coches.

## Ce que cette feuille n'établit pas

⛔ Aucune écriture (CSV/prompt/corpus) · ⛔ les comptes sont **RAPPORTÉS** du dossier mergé
(mesures 15/09 re-vues par ai-01), pas re-mesurés pour cette extraction · ⛔ aucune décision
prise — les deux reco sont celles du worker, les coches sont à jsboige · ⛔ le figeage des
prompts est gated et hors périmètre.

---
*po-2024 — pool #458 (c.5666260217), grain ⑧. Feuille extraite du dossier #802 mergé (#1388) ;
l'inventaire complet (§1-§2, tableaux par langue) vit dans le dossier source.*
