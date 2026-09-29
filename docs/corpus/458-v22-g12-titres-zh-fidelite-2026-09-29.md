# Pool v22 — grain ⑫ : fidélité des 175 titres chinois du deck (mesure 0-écriture)

**Base** : `b646c8fd` (29/09). **Méthode** : grain ⑪ — jugement par rangée adossé à `desc_fr`/`desc_zh`, formes établies vérifiées par source externe (zh.wiki « 偶例谬误 », « 因果谬误 », « 假兩難推理 », 《逻辑思维与诡辩》 pour « 王顾左右而言他 », littérature de logique zh — Copi《逻辑学导论》trad.). **Nature** : mesure, aucune écriture — les propositions attendent l'arbitrage ai-01 (Règle C), l'écriture suivra en grain dédié derrière la série CSV (#1621 → #1610 → #1611 → #1612).

Le deck zh n'a jamais eu de passe de fidélité. Les grains antérieurs ont couvert noms propres (⑧), typographie (⑨), casse (⑩, hors deck). Cette passe mesure la **fidélité sémantique** : contresens, artefacts de traduction automatique, **collisions de titres imprimés** (défaut inédit : deux cartes du deck zh portent le même titre — trois paires trouvées), dérives systémiques de vocabulaire.

## Synthèse — 7 A / 1 M / 21 C-note / 146 ✓ (175)

| Verdict | Rangées |
|---|---:|
| **A — écart réel, proposition sourcée** | 7 |
| **M — mécanique (unification de terme), toujours corriger** | 1 |
| **C-note — observation transverse ou proposition à trancher** | 21 |
| **✓ — fidèle** | 146 |

## A — écarts réels, propositions sourcées

| PK | path | fr | zh actuel | Proposition | Preuve |
|---|---|---|---|---|---|
| 1330 | `7.2.1.2.2` | Noyer le poisson | « 车尾防御 » *« défense de l'arrière de voiture » — non-sens, artefact de traduction automatique ; miroir exact du « Компостирование рыбы » ru* | **搅浑水** (alt. 顾左右而言他) | 搅浑水 = idome établi (métaphore aquatique, miroir du fr) ; 《逻辑思维与诡辩》 emploie « 王顾左右而言他 » comme technique de sophistique évasive ; la desc_zh (用大量不相关的信息掩盖缺陷) soutient les deux ; le pire artefact du deck zh |
| 614 | `3.1.2` | Sophisme de l'accident | « 冒行谬误 » *« 冒行 » n'est pas un mot du dictionnaire* | **偶然谬误** | zh.wiki : lemma « 偶例谬误 », variantes « 偶然谬误/意外谬误 » ; la desc_zh (普遍规则应用到特定案例) est la définition exacte |
| 719 | `4.1.3` | Effet cigogne | « 因果谬误 » *(dégénéré : c'est le terme du PARENT — zh.wiki « 因果谬误 » = causal fallacy générique ; le titre perd la métaphore de la cigogne)* | **鹳鸟效应** *(SUPPOSÉ)* | la légende « 鹳鸟送子 » (la cigogne apporte les bébés) est attestée en zh (manuel de linguistique trad.) ; forme dérivée, pas de forme établie trouvée ; miroir du cas « Софизм » ru PK 154 et de l'alias en « Causalation » |
| 1388 | `7.3.2.3.2` | Attaque de la confiance en soi | « 诉诸信心 » *« appel à la confiance » — INVERSION : c'est une attaque, pas un appel ; la desc_zh dit elle-même 攻击论者…缺乏信心* | **打击自信** | desc_fr (discrédite en mettant en avant son manque de confiance) ; la cellule actuelle contredit sa propre desc |
| 55 | `1.1.3` | Sauvetage ad hoc | « 特殊辩护 » **COLLISION imprimée avec PK 956** *(deux cartes du deck portent le même titre)* | **特设辩解** | « ad hoc » = 特设 établi (特设假设 = ad hoc hypothesis, philosophie des sciences) ; PK 956 garde 特殊辩护 (special pleading, sa forme correcte) |
| 834 | `5.2.1` | Comparaison abusive | « 错误的比较 » **COLLISION imprimée avec le parent PK 833** `5.2` *(identiques)* | **滥用比较** | miroir du fr « abusive » ; cohérent avec la sœur 5.2.1.3 不一致的比较 ; le parent garde 错误的比较 |
| 1398 | `7.3.3` | Attaque personnelle | « 人身攻击 » **COLLISION imprimée avec le parent PK 1360** `7.3` *(identiques)* | **直接人身攻击** | 人身攻击 = forme établie d'ad hominem (le parent la garde) ; 7.3.3 = sous-famille de l'attaque DIRECTE (insultes), la distinguer |

Les trois collisions (55/956, 833/834, 1360/1398) sont des paires de **cartes imprimées distinctes portant le même titre zh** — l'équivalent layout de deux cartes fr homonymes, que le deck fr n'a pas. Une garde de collision de titres par langue est à envisager en grain d'écriture.

## M — mécanique (toujours corriger, pas d'arbitrage)

| PK | path | zh actuel | Correction | Nature |
|---|---|---|---|---|
| 134 | `1.3.1` | « 游戏谬论 » | **« 游戏谬误 »** | seule occurrence de 谬论 sur 175 (le terme du deck est 谬误, ×40+) ; 谬论 = « théorie fallacieuse », registre polémique et incohérent avec les 74 autres « Sophisme » rendus par 谬误 |

## C-notes — observations transverses (à trancher ou signaler)

1. **Sous-arbre 6.3 « biais » rendu par 偏见 (préjugé) — 5 cellules** : `1023` 偏见思维, `1024` 人性偏见, `1092` 负面偏见, `1174` 文化偏见, `1242` 理论偏见. Le terme établi pour *cognitive bias* est 偏差 (负面偏差, 认知偏差…) ; 偏见 est correct pour PK 70 Préjugé — le 6.3.x surcharge le mot et crée une fausse parenté lexicale avec 1.2. Si l'arbitrage retient la correction : 偏颇推理 / 天性偏差 / 负面偏差 / 文化偏差 / 理论偏差. *(Miroir de la C-note 1 ru — dérive « несостоятельн- » ×3.)*
2. **Famille « Appel à » à deux rendements** : X诉求 (78 尊重诉求, 79 成就诉求, 128 财富诉求) vs 诉诸X (诉诸多数/传统/先例/自然/怜悯/恐惧/后果… ×10). Harmonisation possible en 诉诸尊重/诉诸成就/诉诸财富.
3. **Asymétrie de sœurs 6.2.1/6.2.3** : `974` 更高的要求 (groupe nominal) vs `1011` 降低要求 (verbe+complément). Proposition : 提高要求 pour la symétrie.
4. `361` « 诱导策略 » (Appât et substitution) : ne garde que l'appât, la substitution est perdue — même défaut que la C-note 3 ru (Приманка). Proposition : 挂羊头卖狗肉 (idome établi du bait-and-switch) ou 诱饵调包 (SUPPOSÉ).
5. `154` « 谬误推理的谬误 » : la forme zh.wiki est 谬误谬误 (fallacy fallacy) — la cellule actuelle est correcte mais verbeuse.
6. `603` « 挑樱桃 » (Picorage de données) : calque ; usages établis 樱桃采摘 / 选择性引用. *(Miroir C-note 16 ru.)*
7. `708` « 确认后件 » : la sœur `729` 否定前件 suit le terme de logique standard ; la forme canonique est 肯定后件.
8. `733` « 肯定择一 » : le terme de logique standard est 肯定选言 (affirming a disjunct).
9. `750` « 模态逻辑谬误 » : se lit « fallace de la logique modale » (le champ entier) ; le concept est 模态谬误 (modal fallacy).
10. `814` « 错误二择一谬误 » : la forme zh.wiki est 假两难推理 ; la cellule actuelle est compréhensible mais non établie.
11. `644` « 错误概率 » : composé qui se lit comme le terme STATISTIQUE « probabilité d'erreur » ; le concept est 概率误用 / 概率错误. La desc_zh (错误地使用概率论) soutient la correction.
12. `177` « 煽动性语言 » (Langage persuasif) : « langage incendiaire » — registre plus fort que « persuasif » ; la desc_zh dit 情感充沛 (chargé d'émotion), ce qui défend la cellule. À trancher : garder ou 说服性语言.
13. `1004` « 回避 » (Couverture/Hedging) : quasi-collision avec `1313` 逃避 (Évasion) — un caractère d'écart sur des concepts distincts. Proposition : 避重就轻. ⚠️ 模糊其辞 est déjà pris par `667` (Imprécision). *(Miroir C-note 4 ru.)*
14. `1015` « 值得注意的努力 » (Argument de l'effort notable) : nomme l'effort, perd l'argument — proposition 努力论证. *(Miroir C-note 6 ru, miroir pt #1606.)*
15. `1352` « 毒害水井 » (Empoisonnement du puits) : calque direct ; zh.wiki 井中投毒 / usages 井水投毒. *(Miroir C-note 19 ru.)*
16. `1357` « 虚假命题 » (Arguments factices) : 命题 = *proposition* logique ; le concept est des ARGUMENTS — proposition 虚假论点 / 捏造论点.
17. `1301` « 重复到令人厌烦 » (ad nauseam) : descriptif ; le pt a gardé le latin depuis #1606 — même arbitrage probable. *(Miroir C-note 7 ru.)*
18. `680` « 最不可能假设 » (Hypothèse peu plausible) : « la MOINS plausible » vs fr « peu plausible » — glissement du degré.
19. `376` « 神经语言程序编制 » (PNL) : la forme établie est 神经语言程序设计 (NLP) ; 程序编制 = programmation logicielle.
20. `172` « 希特勒比附 » (Reductio ad Hitlerum) : élégant (比附 = raisonnement par analogie forcée) — proposé P (garder) ; le latin est conservé en pt depuis #1606, arbitrage ouvert. *(Miroir C-note 9 ru.)*
21. `992` « 鱼与熊掌兼得 » (Vouloir le beurre et l'argent du beurre) : **constat positif** — inversion parfaite du proverbe chinois 鱼与熊掌不可兼得 (Mencius) ; équivalent idiomatique exact, à citer comme référence de ce que la mesure appelle « fidèle ».

## ✓ — fidèles (146)

Toutes les autres rangées du deck portent soit la forme établie (诉诸多数, 诉诸自然, 稻草人, 滑坡谬误, 假两难… pour 814, 蒙面人谬误, 合成谬误, 分割谬误, 四词谬误, 真正的苏格兰人谬误, 断章取义, 混为一谈, 选择性注意, 沉没成本谬误, 热手谬误, 心理投射, 你也一样…), soit un miroir fidèle du titre français (影响力, 修辞手法, 权威论证, 诉诸棍棒, 赞美三明治, 得州神枪手谬误, 政治家的三段论…), soit un descriptif conceptuellement juste (缺乏简约, 有限深度, 不可证伪性…). Constats positifs notables — **cellules que la passe ru ⑪ a dû corriger et qui sont déjà établies en zh** : `1020` 沉没成本谬误 (ru : затонувших издержек), `653` 热手谬误 (ru : разогретые мышцы), `596` 抽样偏差 (ru : пример с искажениями), `845` 混为一谈 (ru : амальгама), `989` 转移举证责任 (ru : переворот доказательств), `185` 终结思考的陈词滥调 (terme exact de Lifton, ru : штамп). Le zh est globalement le deck le plus fidèle des 8 langues mesurées à ce jour.

## Ce que cette mesure n'établit pas

- Les ✓ sémantiques ne disent rien des `desc_zh`/`example_zh` (seuls les titres ont été balayés).
- La proposition 鹳鸟效应 (PK 719) est dérivée de la légende attestée, pas d'une forme de glossaire — SUPPOSÉ.
- Les propositions marquées SUPPOSÉ (C-notes 4, et le choix 搅浑水 vs 顾左右而言他 pour PK 1330) n'ont pas de forme de glossaire directe — à trancher sur le texte.
- Aucune écriture : les corrections attendent l'arbitrage ai-01 grain par grain (Règle C), en série derrière la série CSV. Une **garde de collision de titres** (aucun titre dupliqué dans le deck d'une langue) est à prévoir au grain d'écriture — elle aurait attrapé les 3 paires.

*po-2024*
