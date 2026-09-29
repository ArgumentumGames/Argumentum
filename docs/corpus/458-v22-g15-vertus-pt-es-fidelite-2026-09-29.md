# Pool v22 — grain ⑮ : fidélité des 131 titres pt/es du deck Virtues (mesure 0-écriture)

**Base** : `7d4ac363` (29/09). **Méthode** : grains ⑪-⑭ — jugement par rangée adossé à `description_fr`/`description_pt`/`description_es`, collisions par langue balayées sur la valeur entière. **Nature** : mesure, aucune écriture — les propositions attendent l'arbitrage ai-01 (Règle C).

Première passe de fidélité sur le deck **Virtues** en **pt** et **es** (131 cartes — la seule langue latine non encore mesurée avec l'es). Barème ⑪ : A (écart réel), M (mécanique), C-note (transverse/à trancher), ✓.

## Synthèse — 0 A / 2 M / 10 C-note / 119 ✓ (131)

| Verdict | Rangées |
|---|---:|
| **A — écart réel, proposition sourcée** | 0 |
| **M — mécanique (orthographe), toujours corriger** | 2 |
| **C-note — observation transverse ou proposition à trancher** | 10 |
| **✓ — fidèle** | 119 |

**Le deck Virtues pt/es est le plus propre mesuré** : aucune collision de titres (balayage valeur-entière : 0 en pt, 0 en es), aucun concept erroné sourcé. Les défauts sont orthographiques (M) ou stylistiques (C-note).

## A — écarts réels

**Aucun.**

## M — mécanique (toujours corriger, pas d'arbitrage)

| PK | path | fr | pt actuel | Correction | Nature |
|---|---|---|---|---|---|
| 7 | `1.1.2.1` | Objectif clair | « Objectivo estabelecido » | **« Objetivo claro »** | (1) **« Objectivo »** = orthographe pré-AO90 (Acordo Ortográfico 1990) — le corpus utilise « Objetivo » (pk 164) **19× contre 5×** file-wide, et es voisin = « Objetivo claro ». (2) « estabelecido » ≠ « clair » : la desc_fr porte « exprimé explicitement » — « claro » est le mot juste. La forme pré-AO90 est un reliquat isolé (les 4 autres occurrences « Objectivo » sont dans des notes, pas des titres de deck). |
| 157 | `6.1.3.1` | Considération nuancée | « Considerações **nuanceadas** » | **« Considerações nuançadas »** | « nuanceadas » n'existe pas en portugais (forme hybride français→portugais ; la forme correcte est « nuançadas » — « nuançar » = nuancer). es voisin « matizadas » (correct, dérivé de matiz). |

## C-notes — observations transverses (à trancher ou signaler)

1. **pk 89 « Raciocínio jalonado » / es « Razonamiento jalonado »** : « jalonado » est un gallicisme dans **les deux langues** (pt « jalonar » et es « jalonar » existent mais au sens géodésique/technique, pas « jalonné d'étapes »). La desc_fr porte « inférences intermédiaires menant à une conclusion ». Proposition : pt **« Raciocínio por etapas »**, es **« Razonamiento por etapas »** *(SUPPOSÉ)*. Même correction dans les deux langues = à arbitrer.
2. **pk 94 « pt Lógica formal válida » vs es « Validez formal »** : deux renditions divergentes pour le même fr « Validité formelle » — pt reformule (« logique formelle valide »), es calque. À harmoniser : es « Validez formal » ✓ vs pt « Validade formal » *(SUPPOSÉ)*.
3. **pk 128 « es Lógica informal aceptable »** : « aceptable » pour « solide » (fr « Logique informelle solide ») — pt voisin « sólida » (correct). es **« sólida »** *(SUPPOSÉ)*.
4. **pk 191 « pt Dar provas de empatia » / es « Mostrar empatía »** : le fr « Empathie » est un nom, les deux langues le rendent par une phrase (« donner des preuves » / « montrer ») — ajout non demandé. Proposition : pt **« Empatia »**, es **« Empatía »** *(SUPPOSÉ)*.
5. **pk 200 « pt Adesão ao tema » / es « Adherencia al tema »** : « respect du sujet » rendu « adhésion au thème » — le respect (n'importuner, ne pas dériver) est absorbé dans l'adhésion. Défendable (la desc porte « garder centré ») mais glisse le pivot. À trancher : pt **« Respeito pelo tema »**, es **« Respeto del tema »** *(SUPPOSÉ)* — ou garder (miroir ru ⑪ « gardé »).
6. **pk 208 « pt Avaliação razoável » / es « Evaluación razonable »** pour « Évaluation loyale » : « loyale » (fair, sans déformer) rendu « raisonnable » (sensée) — la desc_fr porte « avec justesse, sans déformer ». Proposition : pt **« Avaliação leal »**, es **« Evaluación leal »** *(SUPPOSÉ)*.
7. **Famille 6.3 « biais » rendue par « enviesamento » (pt) ×7 file-wide** : forme attestée en portugais européen (« enviesamento »), brésilien préfère « viés »/« vieses ». es utilise « sesgo » (correct, standard). À arbitrer : le pt du corpus est-il PT-PT (enviesamento ✓) ou PT-BR (viés) ? Les autres rangées pt (acento PT-PT « factos », « Objectivo » pré-AO90 mais « Objetivo » majoritaire) suggèrent un mélange.
8. **pk 40/80/81/84-87 — titres pt/es renommés (forme complète, pas tronquée)** : la série 4.2-4.3 porte des titres qui reformulent le fr (« Énoncés rigoureux » → pt « Construção rigorosa dos enunciados » — ajoute « construção »). Cohérent entre pt et es (les deux ajoutent), fidèle au sens. ✓ stylistique — mais le renommage est systématique sur la famille 4.2-4.3 : à signaler comme choix éditorial cohérent.
9. **pk 213 « pt Principio da não desqualificação » / es « Principio de no descalificación »** : « non-disqualification » est un néologisme fr ; pt garde « desqualificação » (correct), es « descalificación » (correct — les deux traduisent). ✓.
10. **Constats positifs** — cellules établies : pk 0/1/2 racines exactes, famille 3 « quantitatif » cohérente (pk 59-77 : amostragem/muestreo, probabilidades, medições/mediciones), famille 5 lexique serré (definições/acepções, sintaxe/sintaxis), famille 7 complète (escuta activa/escucha activa, não interromper/no interrumpir). **Aucun des A de Fallacies (⑪-⑭) n'a d'équivalent ici** — le deck Virtues n'a pas la contamination sémantique que le deck Fallacies a montrée en zh/ar/fa.

## ✓ — fidèles (119)

## Instrument

- Extraction : 131 rangées deck (colonne `card` non vide), champs pk/path/title_{fr,pt,es}/description_{fr,pt,es} (tronqués 180), JSON scratchpad `g15_virtues_pt_es_deck.json`.
- Collisions : `Counter` sur la valeur entière title_pt / title_es des 131 rangées — 0 en pt, 0 en es.
- Orthographe : « Objectivo » vs « Objetivo » comptés file-wide sur toutes colonnes `_pt` (5 vs 19) ; « nuanceadas » n'a aucune attestation dans le corpus (recherche) ; « enviesamento » attesté 7× en title_pt.
- Les deux M et les propositions SUPPOSÉ des C-notes sont marquées ; l'arbitrage ai-01 tranche.
