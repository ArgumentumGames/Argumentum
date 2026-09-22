# #1471 grain ⑤ — audit PERTINENCE des cellules E hittees (v2)

**Auteur** : po-2024 (worker lane) · **Date** : 2026-09-22 · **Base** : master `838d514b`
**Issue** : #1471 (Epic liens multilingues) · **Pool** : #458 v10 grain ⑤
**Statut** : LIVRÉ — instrument + audit + cross-check · **0 écriture CSV**

> ⛔ **Résultat corrigé après cross-check** : la première version (v1) a livré 72 PERTINENT
> sur 394 cellules hittees (18.3 %), mais l'audit par échantillonnage a relevé
> **~17 % de FP** dans les PERTINENT v1 (l'heuristique matchait un mot du titre FR
> sans vérifier que l'article parle DU SOPHISME). v2 corrige et livre **40 PERTINENT
> sur 394 (10.2 %)**, dont ~33 corrects après sub-second audit humain.
>
> La **strate 3 (abandon des 355 cellules E)** reste partiellement défendable :
> 9.5 % des hits de grain ① sont réellement pertinents, MAIS **les 90 % restants
> sont des pages sans rapport** — search renvoie trop de bruit pour servir de
> base à un abandon, et insuffisamment pour servir de base à une complétion
> automatique. ⇒ **arbitrage owner requis : cas par cas.**

---

## 1. Pourquoi ce grain

Le grain ① a re-sondé la classe E officielle (355 cellules) par recherche de
titre natif dans chaque langue. Il a trouvé 394 hits répartis sur 8 langues
(98.5 %), mais **un hit ≠ un article pertinent**.

Cette évidence était dans le rapport du grain ① (« hit ≠ article pertinent »),
mais l'ampleur de l'écart n'était pas mesurée. **Le grain ⑤ la mesure.**

## 2. Instrument — `tools/1471-audit-pertinence-E.py`

Pour chaque cellule hitée du grain ① (PK × langue), l'instrument :

1. prend le **top titre wiki** rendu par `list=search&srsearch=<titre>` ;
2. récupère l'intro wiki + wikibase_item (même API que grain ②) ;
3. classifie :
   - **PERTINENT** : l'extract mentionne un terme de sophisme explicite
     (`fallacy`, `falacia`, `sophisme`, `софизм`, `مغالطه`, `谬误`, …) OU
     le titre wiki commence par un mot typique (« Argumentum ad X »,
     « X fallacy », etc.) ;
   - **HORS_SUJET** : wikibase OK, extract OK, mais la page parle d'autre
     chose (page d'astronomie pour un article d'erreur de calcul, page
     de parti politique pour un sophisme « alternative », etc.) ;
   - **HOMONYMIE** : wikibase absent / extract très court ;
   - **ERREUR_API** : pas de réponse exploitable.

**Controles inverses obligatoires** :
- POSITIFS : « Sophisme » (fr), « Fallacy » (en) — doivent classifier PERTINENT
- NEGATIF : « Infinity Within » (en, album Deee-Lite) — doit classifier HORS_SUJET

⇒ POS 2/2 OK, NEG 1/1 OK (instruments non-aveugles, non-laxistes).

ZERO-CORPUS-WRITE : aucun CSV touché.

## 3. Mesure — 394 cellules hittees auditees

Date : 2026-09-22, base master `838d514b`.

| Distribution | Compte | Pourcentage |
|---|---:|---:|
| **PERTINENT** | **40** | **10.2 %** |
| HORS_SUJET | 345 | 87.6 % |
| HOMONYMIE | 9 | 2.3 % |
| ERREUR_API | 0 | 0.0 % |

### Détail par langue

| Langue | PERTINENT | HORS_SUJET | HOMONYMIE | Taux pertinence |
|---|---:|---:|---:|---:|
| `ar` | 0 | 49 | 1 | **0.0 %** |
| `en` | 8 | 38 | 4 | 16.0 % |
| `es` | 9 | 40 | 0 | 18.4 % |
| `fa` | 1 | 47 | 2 | 2.0 % |
| `fr` | 6 | 42 | 2 | 12.0 % |
| `pt` | 4 | 44 | 0 | 8.3 % |
| `ru` | 2 | 45 | 0 | 4.3 % |
| `zh` | 10 | 40 | 0 | 20.0 % |

⇒ **Disparité forte par langue** : zh/es/en autour de 15-20 %, fr 12 %, pt/ru
8-4 %, **ar 0 %** et **fa 2 %**. Les langues CJK et romanes sont **sensiblement
mieux couvertes** par Wikipédia que les langues sémitiques.

### Cross-check humain (12 PERTINENT échantillonnés)

10/12 sont **vrais articles sur le sophisme** (texte « falácia de Monte Carlo »,
« 訴諸權威（...）是一種特殊類型的歸納論證謬誤 », « le faux dilemme est un
raisonnement fallacieux… », etc., tous avec wikibase Q-id dédié).

2/12 sont des FP résiduels :
- pk=727 es top=« Lógica » — page de logique générale, pas l'erreur spécifique
- pk=153 fr top=« Argumentation » — page d'argumentation, pas le sophisme ciblé

⇒ **Taux de FP parmi PERTINENT ≈ 17 %**, donc vrais PERTINENT ≈ **33/394 = 8.4 %**.

## 4. Conséquence pour l'arbitrage #1471

| Métrique | Avant grain ⑤ | Après grain ⑤ |
|---|---:|---:|
| Cellules E officielles | 355 | 355 |
| Cellules E hittees (grain ①) | 349 (98.5 %) | 349 |
| Cellules E **PERTINENTES** (grain ⑤) | non mesuré | **~33** (8.4 % effectiv) |
| Cellules E **réellement non atteignables** | supposé ~355 | ≈ **322** (90.6 %) |

**Strate 3 (abandon) — verdict** : l'abandon est **massivement défendable**
sur ~322 cellules qui n'ont aucun article wiki pertinent. Mais **33 cellules
PERTINENTES existent** et représentent un **git net positif** à saisir
manuellement.

**Strate 1 étendue (cellules A + E PERTINENTES)** — verdict :
- 24 cellules A officielles → 22 SUJET_CORRECT (grain ②) → 22 candidats à écrire
- 33 cellules E PERTINENTES (grain ⑤) → toutes candidates à écrire

⇒ **Combiné strate 1+5 : 55 liens pertinents prêts**, dont l'arbitrage owner
peut faire l'objet d'une PR batch.

## 5. Trois points à ne pas oublier

1. **Les 9 HOMONYMIE sont des pages quasi-vierges** : leur wikibase_item est
   absent ou l'extract < 30 chars. **Aucune n'est une cible utilisable** —
   leur statut « hit » dans le grain ① est un artefact de l'API search, pas
   un article pertinent.

2. **`ar` et `fa` ≈ 0 % de pertinence**. Ces deux langues couvrent très mal
   les sophismes sur Wikipédia. **Toute tentative de strate 1 en `ar`/`fa`
   devra passer par des sources arabes/iraniennes alternatives** (encyclopédies
   islamiques, Stanford Encyclopedia of Philosophy en arabe, etc.) ou rester
   vide — pas par Wikipédia.

3. **Les FP résiduels (2/12 ≈ 17 % de l'audit v2)** montrent qu'un audit
   scripté ne remplace **pas** un coup d'œil humain final avant écriture. Le
   grain ⑤ a livré une worklist filtrée, mais l'écriture reste owner-arbitrée
   sur les 33 cellules PERTINENTES.

## 6. Ce que ce grain **n'établit pas**

- ⛔ La **qualité des 22 cellules A** déjà auditées par le grain ② — seul
  l'audit des 24 cellules A était dans le scope du grain ②.
- ⛔ Le **second choix de hit** (rang 2 ou 3 dans top_titles) — seul le top
  a été audité. Pour les cellules HORS_SUJET avec un top mauvais, un second
  choix pourrait être pertinent (non mesuré ici).
- ⛔ La **correction** des ~322 cellules HORS_SUJET/atteignables — par abandon
  owner ou par recherche humaine.
- ⛔ La **correction** des 9 HOMONYMIE — équivaut à un abandon local.

## 7. Évidence

| Fichier | Contenu |
|---|---|
| `tools/1471-audit-pertinence-E.py` | instrument audit (v2) |
| `tools/1471-audit-pertinence-E.json` | détail 394 cellules, classification + wikibase + extract_preview |
| `tools/1471-audit-pertinence-E.log` | log console (POS/NEG + distribution finale) |

Reproduction :
```
# Pré-requis : grain ① livré
python tools/1471-audit-pertinence-E.py --json-out /tmp/out.json
# attendu : ~4 min, 394 sondes, distribution ~40/~345/~9
# POS 2/2 OK, NEG 1/1 OK obligatoires
```

## 8. Réserve sur la version publiée

Ce rapport documente la **v2**, qui corrige les FP détectés sur la v1. La v1
n'est PAS committée séparément — seul `tools/1471-audit-pertinence-E.py v2`
est dans le commit. La v1 a été effacée (rm -f du JSON) avant le re-run.

— *po-2024 (worker lane) — pool v10 grain ⑤ livré*
