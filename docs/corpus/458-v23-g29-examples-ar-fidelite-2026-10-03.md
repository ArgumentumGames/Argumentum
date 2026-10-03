# Pool v23 — série ㉘ : fidélité des **exemples** arabes du deck (mesure 0-écriture)

**Base** : `master` `0f408864`. **Instrument** : [`definitions-fidelity-instrument.py`](definitions-fidelity-instrument.py), `--field example` (capacité portée par la branche #1724 — rejouer depuis sa tête tant qu'elle n'est pas mergée). **Nature** : mesure, **aucune écriture** — `git status --porcelain Cards/` vide après la passe.

Troisième grain de la série des exemples (dispatch c.5968567990). Même fait structurel que zh : **`example_ar` ABSENTE de toutes les archives** (REFERENCE EXPLOITABLE 0/175) — pas d'arbitre imprimé. Verdict court : **colonne saine** (0 A, 0 M) — et le grain apporte un **fait structurel neuf : 10 déviations partagées mot à mot avec le zh (㉗)**, qui désignent un ancêtre commun aux deux colonnes.

## Synthèse — 0 A / 0 M / 20 rangées observées / 148 ✓ (168 non vides)

| Verdict | Rangées |
|---|---:|
| **A — sens faux ou inversé** | **0** (une désambiguïsation consignée avec frontière : PK 846) |
| **M — contenu manquant ou ajouté** | **0** (glose 847 consignée avec frontière, précédent ru c.5968567847) |
| **C-note — observation consignée, aucune écriture proposée** | **20** |
| **✓ — fidèle** | **148** |
| État voulu (glose G2-C-W2 restaurée, gate-épinglée) | 1 (796) |
| Vide structurel (têtes de famille, `example_fr` vide aussi) | 7 (PK 1, 175, 594, 696, 798, 887, 1280 — mêmes que ru/zh) |

**Les 168 rangées non vides ont été lues, aucun échantillonnage** (cellules multi-lignes rejointes — leçon ㉗ appliquée : 6 cellules à sauts de ligne lues entières).

⚠️ Faiblesses structurelles : lecture **unique**, **lecteur non natif** — les deux « accords de genre » ci-dessous sont des probabilités de grammaire, pas des verdicts natifs. **Aucun arbitre imprimé** (ci-dessus).

## Le fait du grain : 10 jumeaux zh↔ar — la trace d'un ancêtre commun

Ces dix déviations sont **les mêmes** dans les deux langues, mot à mot. Un traducteur indépendant depuis le FR courant ne produit pas dix fois le même écart dans zh et ar : les deux colonnes descendent d'un **texte source commun** (le deck EN d'époque est le candidat — c'est la conclusion du ㉕ sur les définitions : les jumeaux y étaient des traductions d'un EN resté en 2024).

| PK | Déviation partagée | zh (㉗) | ar (ce grain) |
|---|---|---|---|
| 79 | « un album » → **disque de platine** | 白金唱片 | ألبوماً يصبح بلاتينياً |
| 154 | membres de la contradiction **miroirs** (descend/monte ↔ haut/bas) | ✓ | ✓ |
| 595 | hedging « **je pense** » ajouté à l'assertion | 我认为 | أعتقد أنّ |
| 659 | « quel **mal** » → « quelle **différence** » | 有什么区别 | ما الفرق |
| 638 | « une même entité » → **extraterrestres** explicites | 外星人 | كائن غريب |
| 707 | attribution à « certains » / « on voit comme » ajoutée | 有些人认为 | يُرى...على أنه |
| 726 | « fromage » → **fromage suisse** (à trous) | 瑞士奶酪 | الجبن السويسري |
| 814 | « rien ne vous empêche » → « vous **pouvez envisager** » | 你可以考虑 | يمكنك التفكير في |
| 977 | « n'empêche **nullement** » → « ne peut pas empêcher **tous les** » | 不能防止所有的 | لا تمنع جميع |
| 994 | « Votre colère est légitime » → « **je reconnais/je comprends** que… » | 我理解 | أعترف بـ |

Conséquence : corriger l'une de ces cellules dans une seule langue recrée un écart avec l'autre. **Toute arbitration sur ces dix PK doit trancher les deux langues ensemble** (et probablement fa aussi — mesure ㉙ à venir).

## Drapeaux — 47, tous lus

- **35+2 `POLARITY`** : **parité respectée** (لا/لم/لن/لستُ/لا يمكن vs négations lexicalisées FR — même famille de FP que zh : le détecteur compte des caractères, pas des polarités).
- **7 `EMPTY-target`** : vides structurels.
- **1 `LENDEV+CLAUSES`** (796) : glose restaurée G2-C-W2, état voulu gate-épinglé.
- **1 `LENDEV(+2.6)`** (847) : la glose d'amphibologie — voir C-notes.
- **1 `CLAUSES(3<6)`** (658) : FP — le régress infini est **complet** en ar (و… وكيف تحققت من التحقق من هذا التحقق؟ …), l'écran compte des points finaux, l'arabe en met moins.
- **1 `NUM-only-FR`** (644) : les 10 %/90 % sont **écrits en lettres** (عشرة بالمائة، تسعين بالمائة) — convention arabe, raisonnement intact (le sophisme du procureur est bien rendu). FP.

## C-notes — 20 rangées, aucune écriture proposée

### 1. Glose d'intraduisibilité (1) + sa sœur non glosée (1)

- **PK 847** — « أُفضّل الدجاج على الزيتون: **أي الدجاج مع الزيتون، أو الدجاج بدلًا من الزيتون** » : la glose rend les deux lectures (avec/préférence). **Proposition : garder, sur le précédent ru** (c.5968567847), arbitrage ai-01. (La glose n'est PAS dans le périmètre G2-C-W : ni 846 ni 847 ni 855 ne figurent dans les 11 du gate — elle est debout de tout temps.)
- **PK 855** — « ذلك الدب أكل **«أفوكا»** » : stratégie **inverse** de ru (glose) — l'ar garde le **mot français translittéré entre guillemets**, sans glose. Le locuteur arabe qui ne connaît pas le français perd le double sens (l'arabe n'a pas de mot commun avocat/métier : أفوكادو ≠ محامي). Frontière : intraduisibilité **non résolue** — c'est la carte elle-même qui ne marche plus, pas seulement l'exemple. Consigné, aucun geste proposé.

### 2. Désambiguïsation (1) — frontière A douce

- **PK 846** — « رسم صورة السيدة **بالسواد** » : l'ar attache le noir à la **peinture** (بالسواد = avec du noir), lisant la phrase là où le FR la laisse ambigüe (la dame en noir / le portrait en noir). La carte démontre l'ambiguïté — l'ar la **ferme**. Le sens n'est pas inversé : une des deux lectures est choisie. Frontière consignée ; si un locuteur natif confirme la fermeture, la carte ar ne démontre plus son sophisme.

### 3. Jumeaux zh↔ar (10) — voir le tableau ci-dessus

79, 154, 595, 659, 638, 707, 726, 814, 977, 994.

### 4. Accords de genre probables (2) — probabilité, pas verdict

- **PK 622** — « **هذا الحديقة** ستختفي » : الحديقة (jardin) est **féminin**, le démonstratif هذا est masculin — attendu هذه. Coquille probable.
- **PK 900** — « **هذه بالتأكيد خيار** » : خيار (option) est **masculin**, le démonstratif هذه est féminin — attendu هذا. Coquille probable, même famille. (Et le conditionnel FR « je n'aurais pas pensé » devient un passé simple « لم أفكر ».)

### 5. Variantes mineures propres à l'ar (6)

- **PK 71** — « اليوم » (aujourd'hui) ajouté ; **PK 511** — « au ciel » (levé les yeux **au ciel**) omis ; **PK 994** ci-dessus ; **PK 1291** — « parce que c'est un chien ! » → « parce que **c'est ce que font les chiens** ! » (reformulation, sens intact) ; **PK 1352** — « faut-il vous **rappeler** » → « devons-nous **nous souvenir** » (le destinataire devient « nous », l'insinuation survit) ; **PK 644** — nombres en lettres (FP NUM).

## Contrôle inverse

Réutilise celui du ㉗ (même instrument, même champ, même corpus-miroir) : inversion plantée → `POLARITY` lève ; troncature 50 % sur cellule à glose → `LENDEV` aveugle (bande calibrée). Ce grain n'a pas re-planté : **aucune réparation d'instrument** entre ㉗ et ㉘, les contrôles restent valides par construction.

## N'établit pas

- **Que la colonne ar soit sans défaut** — lecture unique, non native, sans arbitre imprimé.
- **Que les deux accords de genre soient des coquilles** — probabilité grammaticale, verdict natif requis.
- **Que les 10 jumeaux proviennent du deck EN** — candidat le plus probable (conclusion ㉕), mais la source n'a pas été relue pour ce grain.
- **Que la glose 847 soit gardée** — proposition sur précédent, arbitrage ai-01.
- **Rien sur** les 7 vides structurels, les `link_*`, les 1233 rangées hors deck.
- **Aucune écriture n'a eu lieu et aucune n'est proposée.**

⛔ Gel `v2.0.0-review` respecté — aucune republication.

## Reproductibilité

```
python docs/corpus/definitions-fidelity-instrument.py --self-test
python docs/corpus/definitions-fidelity-instrument.py --field example --lang ar --out <sortie>
```

- Dénominateur auto-vérifié ; 47 drapeaux ; jointure 168/175 (157 nom + 11 position) inchangée ; calibration arabe pente 0,700 σ 19,6.
- ⚠️ Nécessite la tête de #1724 tant que `--field` n'est pas sur master.
