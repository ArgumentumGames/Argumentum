# Pool v22 — grain ⑯ : fidélité des 131 titres ru du deck Virtues (mesure 0-écriture)

**Base** : `fd620fc4` (01/10). **Méthode** : grains ⑪-⑮ — jugement par rangée adossé à `description_fr`/`description_ru`, collisions balayées sur la valeur entière. **Nature** : mesure, aucune écriture — pas de référence imprimée pour les Virtus, l'écriture suivra l'arbitrage ai-01 (Règle C).

Deuxième passe de fidélité sur le deck **Virtues** (131 cartes), en **ru** — première langue non latine mesurée sur ce deck.

## Synthèse — 0 A / 0 M / 7 C-note / 124 ✓ (131)

| Verdict | Rangées |
|---|---:|
| **A — écart réel, proposition sourcée** | 0 |
| **M — mécanique (orthographe/grammaire), toujours corriger** | 0 |
| **C-note — observation transverse ou proposition à trancher** | 7 |
| **✓ — fidèle** | 124 |

**Le deck Virtues ru est le plus propre mesuré à ce jour** — plus propre que pt/es (⑮ : 2 M) : zéro faute mécanique, zéro collision (balayage valeur-entière : 0), aucun concept erroné sourcé. Les 7 C-notes sont des calques raides ou des incohérences de rendu, pas des fautes.

## A — écarts réels

**Aucun.**

## M — mécanique

**Aucune.** Chaque titre est du russe grammatical ; pas de forme inventée (contrairement au pt « nuanceadas » ⑮ : ici pk 157 « Нюансированные соображения » est correct).

## C-notes — observations (à trancher ou signaler)

1. **pk 191 « Проявлять эмпатию »** pour fr « Empathie » : proposition verbale (« montrer de l'empathie ») pour un nom — **miroir exact de ⑮ C-note 4** (pt « Dar provas de empatia » / es « Mostrar empatía », même rangée, même ajout). Proposition : **« Эмпатия »** *(SUPPOSÉ)* — à arbitrer une fois pour les trois langues.
2. **pk 128 « Приемлемая неформальная логика »** pour « Logique informelle solide » : « solide » rendu par « приемлемая » (acceptable) — **même glisse que l'es ⑮ C-note 3** (« aceptable »), desc « Raisonnement valable » la défend partiellement. Proposition : **« Надёжная неформальная логика »** *(SUPPOSÉ)*.
3. **pk 38 « Уместный словарь » vs pk 41 « Нейтральная лексика »** : deux rendus de « Vocabulaire » (словарь / лексика) — « словарь » signifie aussi « dictionnaire » ; pk 137/138 rendent « Langage » par « язык ». Harmoniser 38 vers **« Уместная лексика »** *(SUPPOSÉ)*.
4. **pk 67 « Освоенные вероятности »** pour « Probabilités maîtrisées » : « освоенные » (assimilé — territoires, compétences) appliqué à des probabilités est inidiomatique. Proposition : **« Владение вероятностями »** *(SUPPOSÉ)*.
5. **pk 164 « Непотворствующая цель »** pour « Objectif non complaisant » : calque raide (потворствовать = céder aux caprices). La desc (« même niveau d'exigence qu'à votre adversaire ») vise l'absence de double standard. Proposition : **« Цель без послаблений »** *(SUPPOSÉ)*.
6. **pk 165 « Определение точек внимания »** pour « Identifier les points de vigilance » : « точки внимания » n'est pas établi ; la desc parle de risques et mesures préventives. Proposition : **« Определение зон риска »** *(SUPPOSÉ)*.
7. **pk 170 « Понимание личных искажений других »** : « личных … других » maladroite (« чужих » serait naturel) et asymétrique avec pk 169 qui ajoute « когнитивных » (посылки desc). Léger.

## Constats positifs (le deck est sain)

- **pk 89 « Поэтапное рассуждение »** est la solution *correcte* que ⑮ C-note 1 proposait d'importer en pt/es (« Raciocínio por etapas ») — le ru montre la voie pour le gallicisme « jalonado ».
- **pk 208** ru « Справедливая оценка позиции оппонента » rend « loyal(e) » plus fidèlement que pt/es ⑮-6 (« razoable/razonable » = raisonnable).
- Termes établis vérifiés présents : бремя доказательства (161), бритва Оккама/Хэнлона (32/33), активное слушание (190), принцип фаллибилизма (192), презумпция добросовестности (206), репрезентативная выборка (61), формальная валидность (94), валидный силлогизм (104).
- Famille 3 (quantitatif) : lexique statistique serré et cohérent (точные измерения, доказанные корреляции, ясное представление данных).
- Rangées 81/85/86/87/92/210 : reformulations nominales fidèles **à la description** (pattern déjà consigné ⑮ C-note 8 comme choix éditorial systématique des familles 4.x) — ✓.

## Instrument

- Extraction : 131 rangées deck (colonne `card` non vide) sur `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv`, champs pk/path/title_fr/title_ru/description_fr/description_ru (tronqués 180), JSON scratchpad `g16_virtues_ru_deck.json`.
- Collisions : `Counter` sur la valeur entière title_ru des 131 rangées — **0**.
- Jugement : lecture intégrale des 131 paires (fr, ru) adossée à desc_fr ; les propositions sont marquées *(SUPPOSÉ)*, l'arbitrage ai-01 tranche (aucune écriture dans ce grain).

Refs #458 · Refs #1499
