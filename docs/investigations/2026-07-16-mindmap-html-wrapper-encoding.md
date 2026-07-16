# Encoding check — #819 integrated HTML mindmap wrappers (read-only, 0 write)

**Date** : 2026-07-16
**Auteur** : po-2024 (tick 29, jsboige work-session request — extends #818 encoding thread)
**Posture** : read-only corroboration. Aucun write prod. Aucun regen.

## Question

PR #819 a régénéré les **16 wrappers HTML intégrés** mindmap (8 langues × 2 familles, Fallacies + Virtues) — le format servi en prod **et relu en navigateur par Thomas & Adeline** (véhicule de relecture gate (b)). Or ces wrappers sont construits **depuis les `.content.svg`** qui portent le **résidu #804-#3** identifié par ai-01 Cycle H : ils déclarent `encoding="utf-16"` mais sont en réalité **UTF-8+BOM** (byte-prouvé, systématique sur les 8 `Virtues_MindMap_*.content.svg`).

**Le latent s'est-il propagé dans le véhicule de relecture T&A ?**

## Méthode

Script read-only (`scratchpad/check-mindmap-html-encoding.py`, Python stdlib) sur les 29 wrappers HTML intégrés présents sous `Cards/Fallacies/Mindmaps/<lang>/*.html` (hors `included.html` = template shell). Pour chacun : BOM réel, codec qui décode le fichier entier sans erreur, déclaration `charset` (xml/meta).

## Résultat — 0 propagation

| Métrique | Valeur |
|---|---|
| Wrappers vérifiés | 29 |
| Réellement UTF-8 | **29/29** |
| Déclarent `charset=UTF-8` (meta) | **29/29** |
| Mismatch `utf-16` déclaré / `utf8` réel | **0** |

**Le latent #804-#3 n'atteint pas le véhicule de relecture.** Cause : `MindMapHtmlWrapper.FormatWrapper` strip le prologue `<?xml ?>` des `.content.svg` et injecte le SVG dans un shell HTML qui porte sa propre déclaration `charset=UTF-8`. La déclaration `utf-16` erronée des `.content.svg` sources est donc **éliminée par le wrapper**, pas héritée. Le véhicule T&A est encoding-clean.

Cela **corrobore indépendamment** la certification ai-01 Cycle H (localisation 8/8 doublement certifiée : SHA256 #636 + composition de script), sur l'artefact le plus frais (#819, post-Cycle-H) — c'est-à-dire exactement le fichier que T&A ouvrent.

## Observation (TRACKED, NON-FIX)

**4/29 wrappers portent un BOM** UTF-8 : `Fallacies_{ar,es,fa,zh}.html`. Inoffensif pour la relecture (meta `charset=UTF-8` + précédence BOM navigateur → rendu correct), mais **incohérent** avec les 25 autres (no-BOM).

**Pas de fix dans cette PR** : la source du BOM est l'amont (`.content.svg` UTF-8+BOM / wrapper template), dont la régénération est la **lane po-2023** (mindmaps regen, per lane separation 10/07) — pas la lane po-2024 (Cards/game-content). Tracé pour folder dans la pass regen mindmap post-tag de po-2023 (même pass que résidus #804 #1/#2/#3).

## Out of scope

- ⛔ Régénération wrappers / `.content.svg` (lane po-2023).
- ⛔ Modif `MindMapHtmlWrapper` (lane code mindmap).
- ⛔ Strip BOM sur les 4 wrappers (décision + regen po-2023).
- ⛔ Déclaration encoding des `.content.svg` sources (résidu #804-#3, déjà tracé ai-01 sur #804 + #458, post-tag lane po-2023).

## Verdict

✅ **Gate (b) encoding-safe sur le véhicule T&A.** Le latent #804-#3 des `.content.svg` sources est neutralisé par le wrapper pipeline. 0 mismatch. 1 observation hygiène (BOM 4/29) tracée post-tag lane po-2023, non-bloquante pour la relecture.

## Refs

- #819 (regen HTML wrappers 8 langues × 2 familles)
- #804 résidu #3 (encoding `utf-16`/UTF-8+BOM sur `.content.svg`, ai-01 Cycle H)
- #818 (encoding inventory Cards CSVs — #818 thread, read-path C# analysis)
- #458 (tracks release v0.9.0)

— po-2024 (tick 29, jsboige work-session request)
