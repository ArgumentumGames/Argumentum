# Pool v22 — grain ⑬ : fidélité des 175 titres arabes du deck (mesure 0-écriture)

**Base** : `b646c8fd` (29/09). **Méthode** : grains ⑪/⑫ — jugement par rangée adossé à `desc_fr`/`desc_ar`, formes établies vérifiées par source externe (ar.wiki « المصادرة على المطلوب », « التشيؤ », glossaires Obstan, shamela « مختصر المغالطات المنطقية », trad. arabe de l'Encyclopédie Stanford). **Nature** : mesure, aucune écriture — les propositions attendent l'arbitrage ai-01 (Règle C), l'écriture suivra en grain dédié derrière la série CSV.

Première passe de fidélité sur le deck ar. Barème ⑪ : A (écart réel sourcé), M (mécanique), C-note (transverse/à trancher), ✓.

## Synthèse — 7 A / 2 M / 21 C-note / 145 ✓ (175)

| Verdict | Rangées |
|---|---:|
| **A — écart réel, proposition sourcée** | 7 |
| **M — mécanique (espace/grammaire), toujours corriger** | 2 |
| **C-note — observation transverse ou proposition à trancher** | 21 |
| **✓ — fidèle** | 145 |

## A — écarts réels, propositions sourcées

| PK | path | fr | ar actuel | Proposition | Preuve |
|---|---|---|---|---|---|
| 698 | `4.1.1` | Pétition de principe | « مغالطة التوسل » *« fallace de la supplication » — vague* | **المصادرة على المطلوب** | LE terme établi de la logique arabe, classique à aujourd'hui : ar.wiki (lemma), al-Jurjânî « التعريفات », shamela, trad. arabe de l'Encyclopédie Stanford, cours universitaires ; le descendant 4.1.1.1 (جدلية دائرية) présuppose ce lexique |
| 1288 | `7.1.2.1` | Postulat infalsifiable | « مبدأ غير قابل للإثبات » *« qui ne peut pas être PROUVÉ » — inversion de la falsifiabilité (preuve ≠ réfutation, cœur du poppérien)* | **مبدأ غير قابل للتكذيب** (alt. الدحض) | desc_fr « impossible à réfuter » ; الإثبات = prouver, التكذيب/الدحض = réfuter — la cellule actuelle décrit l'opposé épistémologique |
| 869 | `5.3.2.3.2.1` | Réification | « تصييغ » *« formulation/phrasé » — pas le concept* | **التشيؤ** | ar.wiki (lemma تشيؤ = trad. de reification/Verdinglichung), corpus lukácsien et école de Francfort en arabe (ASJP, Doha Institute) — le terme philosophique établi |
| 750 | `4.2.3` | Erreur de modalité | « مغالطة منطقية شكلية » *« fallace logique FORMELLE » = la CLASSE ENTIÈRE, terme générique — dégénéré* | **مغالطة الجهة** *(SUPPOSÉ)* | la modalité en logique arabe scolastique = الجهة ; même famille de défaut que zh 719 因果谬误 (titre = terme du parent/classe) |
| 667 | `3.3.1` | Imprécision | « غموض » **COLLISION imprimée avec PK 846** `5.3` (Ambiguïté = غموض) | **عدم الدقة** *(SUPPOSÉ)* | seule collision du deck ar (l'instrument zh ⑫ en trouvait 3) ; 846 garde غموض (ambiguïté = forme correcte) |
| 595 | `3.1` | Généralisation abusive | « تعميم مُتعجل » — **quasi-collision parent/enfant avec 598** تعميم متسرع *(synonymes : « hâtive » pour les deux)* | **التعميم المفرط** | le fr distingue abusive (excessive) / hâtive (précipitée) ; la desc_ar de 595 parle de قاعدة عامة مفرطة في التعميم — le mot est déjà dans la desc |
| 733 | `4.2.1.3.1` | Affirmation d'une disjonction | « تأكيد التعارض » *« affirmation de la CONTRADICTION » — mauvais connecteur (disjonction ≠ contradiction)* | **تأكيد المنفصل** *(SUPPOSÉ)* | la logique arabe nomme la disjonction المنفصلة (al-faṣl) ; التعارض = opposition/contradiction, une autre relation — la cellule décrit un autre sophisme |

## M — mécanique (toujours corriger, pas d'arbitrage)

| PK | path | ar actuel | Correction | Nature |
|---|---|---|---|---|
| 839 | `5.2.2` | « تناظركاذب » | **« تناظر كاذب »** | espace manquant (تناظر + كاذب soudés) ; sémantique déjà correcte (fausse analogie = التناظر الكاذب) |
| 796 | `4.3.3.3.1` | « مغالطة لأربعة مصطلحات » | **« مغالطة المصطلحات الأربعة »** | construction génitive non idiomatique (لام de spécification insolite) ; quaternio terminorum = مغالطة المصطلحات الأربعة |

## C-notes — observations transverses (à trancher ou signaler)

1. **Famille « Appel à » à QUATRE rendements** : نداء إلى (×8 : 104, 105, 108, 299, 319, 337, 340, 343) · الاحتكام إلى (×3 : 43, 98, 128) · حجة من/الإنجاز (×3 : 71, 78, 79) · الاحتجاج بـ (×1 : 323). Les glossaires arabes établis (Obstan, shamela) usent de الاحتكام إلى — harmonisation possible.
2. `71` « حجة من نص سلطة » : ajoute « نص » (texte) — l'autorité n'est pas forcément un texte. Forme établie : الاحتكام إلى السلطة.
3. `134` « مغالطة لوديكية » : translittération non intégrée du fr « ludique » → مغالطة اللعب (SUPPOSÉ).
4. `154` « مغالطة المغالط » : المغالط = le sophiste (PERSONNE) ; le fallacy-fallacy = مغالطة المغالطة (ar.wiki).
5. `777` « التناقض » (contradiction) alors que l'arbre rend incohérence par عدم الاتساق (826, 837, 1361) — vocabulaire de l'arbre à suivre : عدم الاتساق. *(Miroir de l'A ru 4.3.2.)*
6. **« Acception » rendu par تعريف (définition) — 2 cellules** : `800` تعريف غامض, `804` تعريف تعسفي. Le fr distingue définition (5.1 parent) / acception (usage) → المعنى الغامض / الاصطلاح التعسفي.
7. `727` « منطق المقترحات » : المقترحات = *propositions-parlementaires* ; le terme de logique est القضايا (منطق القضايا).
8. `740` « استنتاج فوري » : « immédiat-temporel » ; le terme de logique est المباشر (الاستنتاج المباشر = immediate inference).
9. `603` « الانتقاء التحكمي » : cherry picking → تصيد الكرز (calque attesté, الرنجة الحمراء = red herring est dans les glossaires) ou الانتقائية (SUPPOSÉ). *(Miroir C16 ru / C6 zh.)*
10. `614` « مغالطة الحادث » : l'accident aristotélicien = العارض (terminologie scolastique) ; الحادث (l'incident) est compréhensible — garde possible. *(Contraste : zh ⑫ A sur la même rangée.)*
11. `638` « رامي السهام التكساسي » : archer (arc et flèches) pour sharpshooter (tireur d'élite = القناص) — la métaphore survit, glissement d'arme (léger).
12. `1371` « المغالطة الجينية » : جينية = génétique-BIOLOGIQUE ; la forme des glossaires est **مغالطة المنشأ** (shamela, cours « المغالطات المنطقية »). Sourcé — candidat A si l'arbitrage veut suivre les glossaires.
13. `1357` « مزاعم كاذبة » : مزاعم = *affirmations/claims* ; le fr = ARGUMENTS factices → حجج مصطنعة.
14. `994` « لغة خشبية » : calque littéral de « langue de bois » — compréhensible dans les contextes traduits (presse francophone arabe) ; proposé P (garder), équivalent libre = الخطاب الفارغ.
15. **Asymétrie de sœurs 6.2.1/6.2.3** : `974` زيادة المتطلبات (verbe) vs `1011` متطلبات منخفضة (nom) — même asymétrie que zh. *(Miroir C3 zh.)*
16. `1015` « جهد ملحوظ » : perd « argument » → حجة الجهد الملحوظ. *(Miroir C6 ru / C14 zh.)*
17. **Singulier/pluriel 6.3.x** : `1024` التحيزات, `1092` تحيز, `1174` تحيز, `1242` التحيزات — 2 pluriels / 2 singuliers sur des nœuds de même niveau.
18. `112` « المغالطة الأخلاقية » : « fallace morale » pour moralISTIC (dériver le fait de la morale) — glissement léger. *(Miroir C18 ru.)*
19. `1360`/`1398` : الحجة الشخصية (ad hominem) vs شخصنة (attaque personnelle) — aucun doublon (✓), mais l'ironie veut que شخصنة, terme standard arabe d'ad hominem, siège sur l'enfant. Fonctionne ; à laisser ou permuter à l'arbitrage.
20. `1301` « الحجة بالإلحاح » (ad nauseam) : « argument par l'insistance » — capturait bien la répétition ; le pt garde le latin depuis #1606. *(Miroir C7 ru.)*
21. **Constats positifs** — cellules établies que ru ⑪ a dû corriger et que ar porte déjà : `719` **تأثير اللقلق** (l'effet cigogne GARDÉ — lqlq = cigogne, là où zh et ru l'ont perdu), `1330` **دفاع تشوباكا** (la défense Chewbacca — exactement la correction qu'ai-01 a arbitrée pour ru ⑪c !), `677` المنحدر الزلق (pente glissante, forme établie), `1370` رجل القش (homme de paille, établi — attesté glossaires), `653` اليد الساخنة, `596` انحياز العينة, `1020` التكاليف الغارقة (économie arabe standard), `989` قلب عبء الإثبات, `943` إخراج عن السياق, `1355` الإسقاط النفسي (terme psychanalytique arabe), `781` منطق الإبريق (chaudron), `992` أخذ الزبدة وثمنها (miroir idiomatique du beurre), `713` الخطأين يصنعان صوابًا. **Le deck ar, comme le zh, est dans le haut du panier des 8 langues.**

## ✓ — fidèles (145)

Toutes les autres rangées portent soit la forme établie (تواطؤ, التفكير بالتمني, التعريف الإقناعي, مغالطة الرجل المقنع, مغالطة الاسكتلندي الحقيقي, مغالطة الكمال, الالتباس نحوي, إسناد زائف, أنت أيضًا, اختزال إلى هتلر, الهجوم على الثقة بالنفس — exact là où zh ⑫ a trouvé une inversion…), soit un miroir fidèle du fr (كليشيه مضاد للنقد, طُعم وتحويل — appât ET substitution préservés, شطيرة المديح, نداء إلى العصا, البرمجة اللغوية العصبية — nom NLP établi), soit un descriptif juste (الإفراط في التأويل, تغيير الأهداف, التملص, تخريب النقاش).

## Ce que cette mesure n'établit pas

- Les ✓ ne disent rien des `desc_ar`/`example_ar` (seuls les titres ont été balayés).
- Les propositions marquées SUPPOSÉ (750 مغالطة الجهة, 667 عدم الدقة, 733 تأكيد المنفصل, 134, 603) n'ont pas de page de glossaire directe — à trancher sur le texte.
- Le sens des diacritiques (تشدد) n'a pas été audité séparément.
- Aucune écriture : tout attend l'arbitrage ai-01 (Règle C). La **garde de collision de titres par langue** proposée en ⑫ couvre ar dès sa création (elle aurait attrapé 667/846).

*po-2024*
