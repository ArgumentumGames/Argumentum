# Orphelins du webroot prod — copies de préservation byte-exactes

> **Décision owner** (12/09/2026) : *« Sauve ce qui est sauvable. »* — conservation des 2 fichiers servis à la racine de `www.argumentum.games` qui **n'existent dans aucun commit** du dépôt (preuve : balayage de l'objet-DB entière, [#830 c.5646564558](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5646564558)).
> **Rôle secondaire** : ces deux fichiers sont les **sentinelles d'additivité** du redéploiement ([runbook §5](../redeploy-mindmaps-runbook.md)) — toute divergence entre le servi et les empreintes ci-dessous après un geste de dépôt prouve qu'un miroir est passé.

## Empreintes (mesurées au téléchargement, cache-buster)

| Fichier | Taille servie (o) | sha256 | `Last-Modified` servi |
|---|---:|---|---|
| [`fallacies_fr.links.svg`](fallacies_fr.links.svg) | 1 552 866 | `e47d5c6aa4e9bed90a715c186f72b17457af36697f206e855d612ec3402094f6` | sam. 02 mars 2024 01:23:11 GMT |
| [`virtues_fr.svg`](virtues_fr.svg) | 555 208 | `b6df4cab02b3277331ed9b97e8c865fa1a6009863dcf20a7e4f10d68586beee8` | lun. 15 janv. 2024 17:24:13 GMT |

Copies prises le **2026-09-13** (re-sonde `?cb=` ; tailles + sha256 + `Last-Modified` vérifiés conformes aux empreintes publiées le 12/09). Les canon servis, eux, datent du 10/08/2026 16:55:44 GMT — les deux orphelins sont des **vestiges du site originel 2024** jamais écrasés depuis.

## Caractérisation (résumé — dossier complet sur #830)

Verdicts binaires établis le 12/09 ([#830 c.5648224120](https://github.com/ArgumentumGames/Argumentum/issues/830#issuecomment-5648224120)) :

- **`fallacies_fr.links.svg`** — rendu liens de **mars 2024** du corpus Fallacies (pré-golden-master). L'écart apparent avec le committé (« 1 408 liens vs 1 400 ») se dissout en artefacts d'emballage (14 hrefs multi-URL, mojibake `A13%`↔`%C3%`, fragments, schémas) ; résidu réel **11 cibles en plus / 6 en moins, toutes couvertes** par `CSV actuel ∪ historique git` (les 4 absentes du CSV actuel vivaient de 2022 au restore golden-master `74557ea6` du 14/03/2026). **Aucun travail absent du dépôt.**
- **`virtues_fr.svg`** — rendu mindmap de **janvier 2024** du CSV Virtues pré-#367 (204/233 labels intacts à `b76af806^` ; les 5 signatures de typos disparaissent toutes dans #367 du 28/05/2026 ; familles pré-renommage d'août 2026). **Reproductible depuis l'historique du dépôt** : `git show 'b76af806^:Cards/Fallacies/Argumentum Virtues - Taxonomy.csv'` → `MindMapCreator` (FreeMind/Batik) → SVG content.

Ces fichiers sont archivés **tels quels** (octets servis exacts, artefacts d'emballage compris) — ils sont la preuve de l'état du servi, pas une version nettoyée.
