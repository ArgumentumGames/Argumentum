# Pool v22 — grain ⑲ : tri des bandeaux Virtues ≠ titre de l'ancêtre (mesure 0-écriture)

**Base** : `7d4ac363` (29/09). **Méthode** : miroir de ⑱ (#1635, Fallacies) — la colonne bandeau d'un nœud porte le titre de son **parent direct** ; toute valeur différente est une divergence à trier (alias périmé, vide, casse, autre concept). **Nature** : mesure, aucune écriture.

## ⚠️ Convention Virtues ≠ Fallacies — le bandeau est plafonné

Avant de compter, la convention réelle doit être **établie, pas supposée** — c'est le même piège que ⑱. Mesuré (jointure parent→titre sur les 3 colonnes `family/subfamily/subsubfamily`) :

| Profondeur | Colonne bandeau | Porte |
|---|---|---|
| 2 | `family_<lang>` | titre du parent (racine) |
| 3 | `subfamily_<lang>` | titre du parent |
| 4 | `subsubfamily_<lang>` | titre du parent |
| **5-7** | `subsubfamily_<lang>` | **plafonné** : titre d'un ancêtre plus haut (anc2/anc3) — la hiérarchie n'a que 3 colonnes |

**Conséquence** : les nœuds de profondeur 5-7 ont un bandeau qui ne PEUT PAS être le titre du parent direct (la colonne est déjà prise par un ancêtre). Les comparer au parent direct produit 448 « divergences » structurelles — un faux positif de périmètre, comme le total 34 vs 17 des wrappers inlining (#1438). **Le tri se scope donc sur depth 2-4** (où le bandeau doit égaler le titre du parent).

## Résultat — 14 divergences / 3 clusters (depth 2-4, 7 langues, hors fr)

| Cluster | Nœud → parent | Langues touchées | Nature |
|---|---|---|---|
| **5.1.2** | « Définition conforme » → parent « Définitions claires » | **7** (en/ru/pt/ar/es/zh/fa) | bandeau = « Acceptable/admissible definitions » vs titre « Clear definitions » — **alias périmé** (« acceptable » pour « clair ») |
| **3.2.3** | « Support fini » → parent « Interprétation rigoureuse des données » | **4** (ru/ar/zh/fa) | bandeau = « adequate interpretation » vs titre « strict/rigorous interpretation » — l'adjectif « strict » (строгая/صارم/严谨/دقیق) remplacé par « adéquat » (адекватная/ملائم/恰当/مناسب) — **alias périmé** ; en/pt/es convergent (« adequate » des deux côtés) |
| **6.2.1** | « Exigence réaliste » → parent « Clarté des enjeux » | **3** (ru/pt/zh) | bandeau = « clarté des questions/enjeux » (générique) vs titre précis — ru « Ясность вопросов » vs « Ясность сути дела » ; pt « dos desafios » vs « das questões em jogo » ; zh « 议题清晰 » vs « 利害关系清晰 » — **alias périmé** |

**0 vide, 0 casse-seule, 0 autre-concept** — les 14 sont des alias périmés (le bandeau n'a pas suivi un renommage de l'ancêtre), exactement la classe dominante de ⑱ Fallacies (320/422 alias périmés).

## Comparaison avec ⑱ (Fallacies)

| | ⑱ Fallacies | ⑲ Virtues |
|---|---:|---:|
| Cellules divergentes | 422 | **14** |
| Clusters | 29 | **3** |
| Vides | 94 (dont 2.3.2=64) | 0 |
| Casse-seule | 8 (pt) | 0 |
| Alias périmés | 320 | **14** |

Les Virtues sont **30× plus propres** que les Fallacies sur ce critère — la taxonomie Virtues a moins de profondeur effective (pas de sous-arbres profonds à bandeaux divergents type 2.3.2) et moins de renommages d'ancêtres non propagés.

## Correction proposée (attend l'arbitrage ai-01)

Aligner chaque bandeau sur le titre de l'ancêtre dans la langue — la règle déjà arbitrée pour ⑱ (« bandeaux hors deck alignés sur le titre de l'ancêtre, sans exception », #458 c.5882003715) s'applique telle quelle :

- 5.1.2 → `subfamily_<lang>` = titre de 5.1 (« Clear definitions » / « Ясные определения » / « Definições claras » / …)
- 3.2.3 → `subfamily_<lang>` = titre de 3.2 (ru « Строгая интерпретация данных », ar « تفسير صارم للبيانات », zh « 对数据的严谨解释 », fa « تفسیر دقیق داده‌ها »)
- 6.2.1 → `subfamily_<lang>` = titre de 6.2 (ru « Ясность сути дела », pt « Clareza das questões em jogo », zh « 利害关系清晰 »)

**Aucune exception à nommer** (contrairement à ⑱ qui en avait 2 : ru GoT, en Causalation) — les 3 clusters sont des alias périmés francs.

## Instrument

- Balayage : jointure `subfamily_<lang>` (depth 3) / `family_<lang>` (depth 2) / `subsubfamily_<lang>` (depth 4) contre `title_<lang>` du parent direct, 7 langues (fr exclu — la convention bandeau est fr-de-facto).
- Convention établie par mesure (comptage des colonnes matchant le parent par profondeur) AVANT le tri — le scope depth 2-4 est ce qui reste une fois le plafonnement depth 5-7 écarté.
- La fr n'est pas comptée : le bandeau fr suit toujours (c'est la langue de référence des renommages).

## Garde proposée (au grain d'écriture)

Une garde « bandeau Virtues (depth 2-4) = titre du parent dans la langue » — miroir de `DeckBannerPresenceGuardTests`/`FallaciesDeckBandInvariantGuardTests`, scopée depth ≤ 4 (le plafonnement depth 5-7 est structurel et ne doit PAS être gardé comme une divergence).
