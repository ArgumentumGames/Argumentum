# Collisions de titres dans les 7 autres langues — mesure 0-écriture (pool v18, grain 3)

Arbre master `b29c1b9e` (2026-09-26) · même fenêtre et méthode que le grain 2 (93 snapshots, convergence
par colonne `text_<lang>`) · dispatch [#458 c.5845739927](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5845739927).

> **Correction du 26/09 (revue [#1585](https://github.com/ArgumentumGames/Argumentum/pull/1585))** —
> même piège que le grain 2 : la première version suivait les rangées par **PK** et a lu la renumérotation
> `f38cfed9` comme un changement de titre espagnol (« Equívoco antitético », faux MOVER « Negación no
> negada »). Mesuré sur `f38cfed9^` : la rangée `6.2.2.2.3` porte déjà « Equívoco antitético », c'est l'autre
> rangée PK 1000 (`6.2.2.3`, « Negación no negada ») qui devient la 1407. Cette version suit les rangées par
> **`path`** et classe « d'origine » toute collision déjà présente à l'import `e8482fe5`. Comptes espagnols :
> 73 d'origine / **2 vraies créations** ; total toutes langues : **18 vraies créations, dont 14 héritées** du français.

Discriminante : une collision `<lang>` est **héritée du FR** (les mêmes rangées partagent aussi leur titre fr —
la traduction a été fidèle à une collision fr existante) ou **créée par la traduction** (les titres fr de ces
rangées différaient — deux libellés distincts ont convergé en `<lang>`).

## Synthèse par langue

| Langue | Groupes ≥2 rangées | À l'origine | Réécrits | Créés modulo écriture | Vraies créations | dont héritées du FR | créées par la traduction |
|---|---:|---:|---:|---:|---:|---:|---:|
| en | 93 | 89 | 1 | 0 | 3 | 2 | 1 |
| ru | 55 | 51 | 0 | 0 | 4 | 3 | 1 |
| pt | 39 | 37 | 1 | 1 | 0 | 0 | 0 |
| es | 75 | 73 | 0 | 0 | 2 | 2 | 0 |
| ar | 46 | 43 | 0 | 0 | 3 | 3 | 0 |
| fa | 36 | 34 | 0 | 0 | 2 | 2 | 0 |
| zh | 52 | 48 | 0 | 0 | 4 | 2 | 2 |

« héritées du FR » : la collision fr existait sur les mêmes rangées — le remède côté FR (règle C, grain 2)
la résout mécaniquement dans la langue. « créées par la traduction » : la langue porte une collision que le
FR n'a PAS — matière de renommage propre. **Décisions règle C rendues le 26/09 (ai-01, revue #1585)** sur les
4 créées par la traduction : retour pour ru « Ошибка игрока » (717 redevient « Софизм игрока », la passe #397
avait effacé une distinction que le français garde) ; gardées : en « Deepity » (remplaçait une autre collision),
zh « 循环论证 » (corrigeait une faute de sens), zh « 完美方案谬误 » (définitions identiques octet pour octet).

## en — 93 groupes, 193 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «Loaded question» — HÉRITÉE du FR
  - PK 178 (`2.1.1.1`) **MOVER** : «Argument by question» → «Loaded question»
  - PK 179 (`2.1.1.1.1`) portait déjà «Loaded question»
  - convergence : `95db4425` 2026-05-30 PR #396
- «Deepity» — CRÉÉE par la traduction (fr : «Effet puits» vs «Appel à la pseudo-profondeur»)
  - PK 206 (`2.1.1.5.2.3`) **MOVER** : «Twaddle tendency» → «Deepity»
  - PK 307 (`2.2.1.2.2.2`) portait déjà «Deepity»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «Modal scope fallacy» — HÉRITÉE du FR
  - PK 757 (`4.2.3.5`) portait déjà «Modal scope fallacy»
  - PK 849 (`5.3.1.2`) **MOVER** : «Modal scope» → «Modal scope fallacy»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### Réécrits
- «Dichotomous thinking» (héritée du FR) — avant : «All-or-Nothing Thinking» ; `5e2477b5` 2026-05-31 PR #397

### À l'origine (89 groupes)
- héritées du FR : 72 / 89
- «Ableism» [fr=colliding] — 132, 344
- «Affirming the consequent» [fr=colliding] — 708, 731
- «Always being right» [fr=colliding] — 706, 1171
- «Amazing familiarity» [fr=distinct] — 64, 764
- «Anecdotal evidence» [fr=colliding] — 34, 1087
- «Apophenia» [fr=colliding] — 172, 1083
- «Appeal to accomplishment» [fr=distinct] — 1386, 79
- «Appeal to confidence» [fr=distinct] — 301, 75
- «Appeal to identity» [fr=colliding] — 1018, 1378
- «Appeal to minority» [fr=colliding] — 120, 316
- «Appeal to novelty» [fr=colliding] — 115, 1069
- «Appeal to the stick» [fr=colliding] — 343, 468
- «Appeal to the stone» [fr=colliding] — 1292, 17
- «Argument of the beard» [fr=colliding] — 660, 859
- «Begging the question» [fr=colliding] — 698, 183
- «Blind Item» [fr=colliding] — 880, 924
- «Brainwashing» [fr=colliding] — 1302, 478
- «Buck passing» [fr=colliding] — 1344, 1166
- «Circular reasoning» [fr=distinct] — 699, 829
- «Clustering Illusion» [fr=colliding] — 174, 643
- «Confirmation bias» [fr=colliding] — 602, 965
- «Contrast effect» [fr=distinct] — 390, 1040
- «Damning with faint praise» [fr=colliding] — 1387, 363, 879
- «Dead cat strategy» [fr=colliding] — 914, 1318
- «Denying the antecedent» [fr=colliding] — 729, 722
- «Digression» [fr=colliding] — 297, 1316
- «Emotional reasoning» [fr=colliding] — 53, 1119
- «Emotive conjugation» [fr=distinct] — 218, 812
- «Exaggeration» [fr=colliding] — 233, 893
- «Experimenter effect» [fr=colliding] — 971, 411, 1056
- «False equivalence» [fr=colliding] — 768, 843
- «False precision» [fr=colliding] — 668, 857
- «Far-fetched hypothesis» [fr=colliding] — 63, 763
- «Faulty generalisation» [fr=colliding] — 595, 1123
- «Fear, uncertainty and doubt» [fr=colliding] — 338, 920
- «Fearmongering» [fr=colliding] — 418, 919
- «Firehose of falsehood» [fr=distinct] — 915, 460
- «Gambler's fallacy» [fr=distinct] — 717, 654
- «Genetic fallacy» [fr=colliding] — 1371, 761
- «Gish gallop» [fr=colliding] — 772, 1331, 475
- «Gossip» [fr=colliding] — 491, 923
- «Gratitude trap» [fr=colliding] — 451, 1324, 1103
- «Halo effect» [fr=colliding] — 302, 1231
- «Hasty conclusion» [fr=colliding] — 759, 1127
- «Hindsight bias» [fr=colliding] — 141, 1155
- «Idiosyncratic language» [fr=distinct] — 1304, 821
- «Illusory correlation» [fr=colliding] — 642, 1085
- «Illusory truth effect» [fr=colliding] — 368, 1067
- «Infinite regress» [fr=colliding] — 1349, 979
- «Insensitivity to sample size» [fr=colliding] — 600, 1084
- «Kafkatrap» [fr=colliding] — 986, 161
- «Leap of faith» [fr=colliding] — 22, 770
- «Less-is-better effect» [fr=distinct] — 1239, 1042
- «Magnification and minimization» [fr=colliding] — 895, 1101
- «Mental reservation» [fr=colliding] — 900, 1329
- «Metonymy» [fr=colliding] — 295, 866
- «Mind projection fallacy» [fr=colliding] — 52, 1164
- «Minimisation» [fr=colliding] — 496, 892
- «Moral equivalence» [fr=colliding] — 769, 1321
- «Moral panic» [fr=colliding] — 339, 922
- «Moving the goalposts» [fr=distinct] — 973, 1005
- «No true Scotsman» [fr=colliding] — 65, 616, 813
- «Not invented here» [fr=distinct] — 106, 1187
- «On the spot fallacy» [fr=distinct] — 988, 1348, 477
- «Personalization» [fr=colliding] — 723, 1169
- «Persuasive definition» [fr=colliding] — 184, 819
- «Political correctness» [fr=colliding] — 121, 1341
- «Politician's syllogism» [fr=colliding] — 21, 787
- «Proof Surrogate» [fr=colliding] — 771, 19
- «Pseudoscience» [fr=distinct] — 695, 1271
- «Psychological projection» [fr=colliding] — 1355, 1165
- «Psychologist's fallacy» [fr=colliding] — 51, 1055
- «Quantifier shift» [fr=colliding] — 749, 850
- «Rationalization» [fr=colliding] — 62, 762
- «Scapegoating» [fr=colliding] — 501, 960
- «Shifting ground» [fr=colliding] — 983, 1319
- «Should/shouldn't and must/mustn't statements» [fr=colliding] — 756, 1102
- «Slippery slope» [fr=colliding] — 705, 677
- «Sound bite» [fr=distinct] — 187, 944
- «Spreading» [fr=distinct] — 1300, 476
- «Stereotype» [fr=colliding] — 619, 1199
- «Straw man» [fr=colliding] — 1365, 168, 894
- «Streetlight effect» [fr=colliding] — 138, 1074
- «Substituting Explanation for Premise» [fr=colliding] — 155, 767
- «Sunk Cost Fallacy» [fr=colliding] — 1020, 440
- «Tu quoque» [fr=colliding] — 1362, 714
- «Two wrongs make a right» [fr=colliding] — 713, 1325
- «Vagueness» [fr=distinct] — 667, 856
- «Whispering campaign» [fr=colliding] — 882, 918

## ru — 55 groupes, 113 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «Вопрос-ловушка» — HÉRITÉE du FR
  - PK 178 (`2.1.1.1`) **MOVER** : «Аргумент через вопрос» → «Вопрос-ловушка»
  - PK 179 (`2.1.1.1.1`) portait déjà «Вопрос-ловушка»
  - convergence : `95db4425` 2026-05-30 PR #396
- «Акт веры» — HÉRITÉE du FR
  - PK 22 (`1.1.1.3`) portait déjà «Акт веры»
  - PK 770 (`4.3.1.3`) **MOVER** : «Скачок веры» → «Акт веры»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «Моральная эквивалентность» — HÉRITÉE du FR
  - PK 769 (`4.3.1.2.1`) **MOVER** : «Моральный эквивалент» → «Моральная эквивалентность»
  - PK 1321 (`7.2.1.1.2`) portait déjà «Моральная эквивалентность»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «Ошибка игрока» — CRÉÉE par la traduction (fr : «Sophisme du joueur» vs «Erreur du parieur»)
  - PK 717 (`4.1.2.6`) **MOVER** : «Софизм игрока» → «Ошибка игрока»
  - PK 654 (`3.2.2.3.2`) portait déjà «Ошибка игрока»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### À l'origine (51 groupes)
- héritées du FR : 41 / 51
- «Анекдотичное обоснование» [fr=colliding] — 34, 1087
- «Анимистический софизм» [fr=colliding] — 170, 1088
- «Апелляция к идентичности» [fr=colliding] — 1018, 1378
- «Апелляция к камню» [fr=colliding] — 1292, 17
- «Апелляция к моральной панике» [fr=colliding] — 339, 922
- «Апелляция к уверенности» [fr=distinct] — 301, 75
- «Апофения» [fr=colliding] — 172, 1083
- «Аргумент бороды» [fr=colliding] — 660, 859
- «Аргумент палки» [fr=colliding] — 343, 468
- «Бесконечное обоснование» [fr=colliding] — 1349, 665, 979
- «Всегда быть правым» [fr=colliding] — 706, 1171
- «Генетический софизм» [fr=colliding] — 1371, 761
- «Дихотомическое мышление» [fr=colliding] — 817, 1120
- «Злоупотребление обобщением» [fr=colliding] — 595, 1123
- «Иллюзия кластеризации» [fr=colliding] — 174, 643
- «Иллюзорная корреляция» [fr=colliding] — 642, 1085
- «Ирония» [fr=distinct] — 238, 243
- «Ловушка благодарности» [fr=colliding] — 451, 1324, 1103
- «Ложная точность» [fr=colliding] — 668, 857
- «Метонимия» [fr=colliding] — 295, 866
- «Минимизация» [fr=colliding] — 496, 892
- «Надуманная гипотеза» [fr=colliding] — 63, 763
- «Перекладывание ответственности» [fr=colliding] — 1344, 1166
- «Персонализация» [fr=colliding] — 723, 1169
- «Пожарный шланг лжи» [fr=distinct] — 915, 460
- «Политкорректность» [fr=colliding] — 121, 1341
- «Поспешные выводы» [fr=colliding] — 759, 1127
- «Преувеличение» [fr=colliding] — 233, 893
- «Приманка» [fr=distinct] — 361, 439
- «Промывание мозгов» [fr=colliding] — 1302, 478
- «Псевдонаука» [fr=distinct] — 695, 1271
- «Психологическая проекция» [fr=colliding] — 1355, 1165
- «Распространение» [fr=distinct] — 1300, 476
- «Рационализация» [fr=colliding] — 62, 762
- «Силлогизм политика» [fr=colliding] — 21, 787
- «Скользкая дорожка» [fr=colliding] — 705, 677
- «Софизм ассоциации» [fr=distinct] — 844, 1372
- «Софизм на месте» [fr=distinct] — 988, 1348
- «Софизм психолога» [fr=colliding] — 51, 1055
- «Сплетни» [fr=colliding] — 491, 923
- «Стереотип» [fr=colliding] — 619, 1199
- «Стратегия мертвого кота» [fr=colliding] — 914, 1318
- «Суррогат доказательства» [fr=colliding] — 771, 19
- «Украденное понятие» [fr=colliding] — 779, 828
- «Умственное удержание» [fr=colliding] — 900, 1329
- «Эмотивная конъюгация» [fr=distinct] — 218, 812
- «Этноцентризм» [fr=distinct] — 1174, 1202
- «Эффект гало» [fr=colliding] — 302, 1231
- «Эффект иллюзорной правды» [fr=colliding] — 368, 1067
- «Эффект уличного фонаря» [fr=colliding] — 138, 1074
- «Эффект экспериментатора» [fr=colliding] — 971, 411, 1056

## pt — 39 groupes, 78 rangées porteuses


### Créés modulo écriture (collision préexistante, octets alignés)
- «Pensamento dicotômico» (héritée du FR) — PK 817 «Pensamento Tudo ou Nada» vs PK 1120 «Pensamento tudo ou nada» ; `5e2477b5` 2026-05-31 PR #397

### Réécrits
- «Equivalência moral» (héritée du FR) — avant : «Equivalência Moral» ; `5e2477b5` 2026-05-31 PR #397

### À l'origine (37 groupes)
- héritées du FR : 31 / 37
- «Afirmação do consequente» [fr=colliding] — 708, 731
- «Amplificação e minimização» [fr=colliding] — 895, 1101
- «Apelo à Novidade» [fr=colliding] — 115, 1069
- «Apelo à confiança» [fr=distinct] — 301, 75
- «Apofenia» [fr=colliding] — 172, 1083
- «Armadilha Kafkaiana» [fr=colliding] — 986, 161
- «Capacitismo» [fr=colliding] — 132, 344
- «Conjugação Emotiva» [fr=distinct] — 218, 812
- «Correlação Ilusória» [fr=colliding] — 642, 1085
- «Definição Persuasiva» [fr=colliding] — 184, 819
- «Digressão» [fr=colliding] — 297, 1316
- «Efeito do experimentador» [fr=colliding] — 971, 1056
- «Estereótipo» [fr=colliding] — 619, 1199
- «Evasão» [fr=distinct] — 1313, 1222
- «Exagero» [fr=colliding] — 233, 893
- «Falácia Nirvana» [fr=colliding] — 1350, 977
- «Falácia do Verdadeiro Escocês» [fr=colliding] — 65, 813
- «Fofoca» [fr=colliding] — 491, 923
- «Gish Gallop» [fr=colliding] — 772, 475
- «Indiscrição Anônima» [fr=colliding] — 880, 924
- «Metonímia» [fr=colliding] — 295, 866
- «Minimização» [fr=colliding] — 496, 892
- «Mudança de Terreno» [fr=colliding] — 983, 1319
- «Negação do antecedente» [fr=colliding] — 729, 722
- «Pensamento clichê» [fr=distinct] — 26, 188
- «Personalização» [fr=colliding] — 723, 1169
- «Petição de princípio analógica» [fr=colliding] — 703, 840
- «Projeção psicológica» [fr=colliding] — 1355, 1165
- «Prova Anedótica» [fr=colliding] — 34, 1087
- «Pseudociência» [fr=distinct] — 695, 1271
- «Punho erguido» [fr=distinct] — 559, 570
- «Racionalização» [fr=colliding] — 62, 762
- «Restrição mental» [fr=colliding] — 900, 1329
- «Silogismo do Político» [fr=colliding] — 21, 787
- «Substituição da Explicação pela Premissa» [fr=colliding] — 155, 767
- «Substituto de Prova» [fr=colliding] — 771, 19
- «Tu quoque» [fr=colliding] — 1362, 714

## es — 75 groupes, 153 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «Acto de fe» — HÉRITÉE du FR
  - PK 22 (`1.1.1.3`) portait déjà «Acto de fe»
  - PK 770 (`4.3.1.3`) **MOVER** : «Salto de fe» → «Acto de fe»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «Efecto de verdad ilusoria» — HÉRITÉE du FR
  - PK 368 (`2.3.1.1.1.8`) portait déjà «Efecto de verdad ilusoria»
  - PK 1067 (`6.3.1.1.2.1.1`) **MOVER** : «Efecto de la verdad ilusoria» → «Efecto de verdad ilusoria»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### À l'origine (73 groupes)
- héritées du FR : 58 / 73
- «Afirmación del consecuente» [fr=colliding] — 708, 731
- «Apelación a la confianza» [fr=distinct] — 301, 75
- «Apelación a la identidad» [fr=colliding] — 1018, 1378
- «Apelación a la minoría» [fr=colliding] — 120, 316
- «Apelación a la novedad» [fr=colliding] — 115, 1069
- «Apoyo subvertido» [fr=distinct] — 704, 1296
- «Argumento de la barba» [fr=colliding] — 660, 859
- «Broma» [fr=distinct] — 32, 221
- «Cambio de terreno» [fr=colliding] — 983, 1319
- «Campaña de susurros» [fr=colliding] — 882, 918
- «Capacitismo» [fr=colliding] — 132, 344
- «Chisme» [fr=colliding] — 491, 923
- «Chivo expiatorio» [fr=colliding] — 501, 960
- «Concepto robado» [fr=colliding] — 779, 828
- «Conjugación emotiva» [fr=distinct] — 218, 812
- «Corrección política» [fr=colliding] — 121, 1341
- «Cultura del miedo» [fr=colliding] — 418, 919
- «Definición persuasiva» [fr=colliding] — 184, 819
- «Desplazamiento del cuantificador» [fr=colliding] — 749, 850
- «Digresión» [fr=colliding] — 297, 1316
- «Dos errores hacen un acierto» [fr=colliding] — 713, 1325
- «Efecto de contraste» [fr=distinct] — 390, 1040
- «Efecto del experimentador» [fr=colliding] — 971, 411, 1056
- «Efecto del farol» [fr=colliding] — 138, 1074
- «Efecto halo» [fr=colliding] — 302, 1231
- «Elogio envenenado» [fr=colliding] — 1387, 363, 879
- «Equivalencia moral» [fr=colliding] — 769, 1321
- «Equívoco antitético» [fr=colliding] — 1000, 360
- «Estereotipo» [fr=colliding] — 619, 1199
- «Estrategia del gato muerto» [fr=colliding] — 914, 1318
- «Evasión» [fr=distinct] — 1313, 1222
- «Exageración» [fr=colliding] — 233, 893
- «Extensión» [fr=distinct] — 1366, 476
- «Falacia de proyección mental» [fr=colliding] — 52, 1164
- «Falacia del costo hundido» [fr=colliding] — 1020, 440
- «Falacia del jugador» [fr=distinct] — 717, 654
- «Falacia del psicólogo» [fr=colliding] — 51, 1055
- «Falacia genética» [fr=colliding] — 1371, 761
- «Falsa equivalencia» [fr=colliding] — 768, 843
- «Falsa precisión» [fr=colliding] — 668, 857
- «Galope de Gish» [fr=colliding] — 772, 1331
- «Hombre de paja» [fr=colliding] — 1365, 168, 894
- «Indiscreción anónima» [fr=colliding] — 880, 924
- «Insensibilidad al tamaño de la muestra» [fr=colliding] — 600, 1084
- «Insulto» [fr=distinct] — 1403, 1404
- «Justificación infinita» [fr=colliding] — 1349, 979
- «Lavado de cerebro» [fr=colliding] — 1302, 478
- «Lenguaje idiosincrásico» [fr=distinct] — 1304, 821
- «Manguera de mentiras» [fr=distinct] — 915, 460
- «Metonimia» [fr=colliding] — 295, 866
- «Minimización» [fr=colliding] — 496, 892
- «Negación del antecedente» [fr=colliding] — 729, 722
- «Ningún verdadero escocés» [fr=colliding] — 616, 813
- «Pendiente resbaladiza» [fr=colliding] — 705, 677
- «Pensamiento cliché» [fr=distinct] — 26, 188
- «Pensamiento dicotómico» [fr=colliding] — 817, 1120
- «Personalización» [fr=colliding] — 723, 1169
- «Petición de principio» [fr=colliding] — 698, 183
- «Pregunta capciosa» [fr=colliding] — 701, 179
- «Proyección psicológica» [fr=colliding] — 1355, 1165
- «Prueba anecdótica» [fr=colliding] — 34, 1087
- «Pseudociencia» [fr=distinct] — 695, 1271
- «Racionalización» [fr=colliding] — 62, 762
- «Razonamiento circular» [fr=distinct] — 699, 829
- «Razonamiento emocional» [fr=colliding] — 53, 1119
- «Reserva mental» [fr=colliding] — 900, 1329
- «Sesgo de confirmación» [fr=colliding] — 602, 965
- «Sesgo retrospectivo» [fr=colliding] — 141, 1155
- «Silogismo del político» [fr=colliding] — 21, 787
- «Sustituto de prueba» [fr=colliding] — 771, 19
- «Tener siempre la razón» [fr=colliding] — 706, 1171
- «Trampa de la gratitud» [fr=colliding] — 1324, 1103
- «Verdad a medias» [fr=distinct] — 890, 1284

## ar — 46 groupes, 96 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «التفكير الثنائي» — HÉRITÉE du FR
  - PK 817 (`5.1.2.2.4.3`) **MOVER** : «التفكير الديكوتومي» → «التفكير الثنائي»
  - PK 1120 (`6.3.1.2.2.1`) portait déjà «التفكير الثنائي»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «التكافؤ الأخلاقي» — HÉRITÉE du FR
  - PK 769 (`4.3.1.2.1`) **MOVER** : «تكافؤ أخلاقي» → «التكافؤ الأخلاقي»
  - PK 1321 (`7.2.1.1.2`) portait déjà «التكافؤ الأخلاقي»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «قفزة إيمانية» — HÉRITÉE du FR
  - PK 22 (`1.1.1.3`) portait déjà «قفزة إيمانية»
  - PK 770 (`4.3.1.3`) **MOVER** : «قفزة الإيمان» → «قفزة إيمانية»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### À l'origine (43 groupes)
- héritées du FR : 24 / 43
- «أنت أيضًا» [fr=colliding] — 1362, 714
- «إشاعة» [fr=distinct] — 916, 913
- «إنكار المقدمة» [fr=colliding] — 729, 722
- «استبدال» [fr=distinct] — 290, 286
- «استبدال التفسير بالمقدمة» [fr=colliding] — 155, 767
- «استنتاج خاطئ» [fr=distinct] — 758, 765
- «استنتاج متسرع» [fr=colliding] — 759, 1127
- «الإسقاط النفسي» [fr=colliding] — 1355, 1165
- «الاحتكام إلى العادة» [fr=distinct] — 43, 46
- «الارتباط الوهمي» [fr=distinct] — 172, 1085
- «التعصب» [fr=distinct] — 1243, 1264
- «التفكير العاطفي» [fr=colliding] — 53, 1119
- «التملق» [fr=distinct] — 422, 510
- «السخرية» [fr=distinct] — 1401, 238
- «الصواب السياسي» [fr=colliding] — 121, 1341
- «المبالغة» [fr=colliding] — 233, 893
- «تأثير التباين» [fr=distinct] — 390, 1040
- «تأثير الحقيقة الوهمية» [fr=colliding] — 368, 1067
- «تأثير المجرب» [fr=colliding] — 971, 411, 1056
- «تأثير الهالة» [fr=colliding] — 302, 1231
- «تأثير ضوء الشارع» [fr=colliding] — 138, 1074
- «تأكيد النتيجة» [fr=colliding] — 708, 731
- «تحامل» [fr=distinct] — 70, 298
- «تزوير» [fr=distinct] — 931, 938
- «تغيير الأرضية» [fr=colliding] — 983, 1319
- «تنافر» [fr=distinct] — 805, 251
- «حملة الهمس» [fr=colliding] — 882, 918
- «دقة زائفة» [fr=colliding] — 668, 857
- «ذاكرة زائفة» [fr=distinct] — 906, 952
- «رجل القش» [fr=colliding] — 1365, 168, 894
- «عدم الحساسية لحجم العينة» [fr=colliding] — 600, 1084
- «غسيل الدماغ» [fr=colliding] — 1302, 478
- «غموض» [fr=distinct] — 846, 667
- «فخ الامتنان» [fr=colliding] — 451, 1324, 1103
- «فرضية بعيدة المنال» [fr=colliding] — 63, 763
- «مجاز مرسل» [fr=distinct] — 866, 296
- «مغالطة الاسكتلندي الحقيقي» [fr=colliding] — 65, 616, 813
- «مغالطة التكاليف الغارقة» [fr=colliding] — 1020, 440
- «مغالطة الحزمة» [fr=distinct] — 773, 1278
- «مغالطة المقامر» [fr=distinct] — 717, 654
- «مغالطة في الحال» [fr=distinct] — 988, 1348
- «نداء إلى الأقلية» [fr=colliding] — 120, 316
- «نداء إلى الثقة» [fr=distinct] — 301, 75

## fa — 36 groupes, 75 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «جهش ایمانی» — HÉRITÉE du FR
  - PK 22 (`1.1.1.3`) portait déjà «جهش ایمانی»
  - PK 770 (`4.3.1.3`) **MOVER** : «پرش به ایمان» → «جهش ایمانی»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «هم‌ارزی اخلاقی» — HÉRITÉE du FR
  - PK 769 (`4.3.1.2.1`) **MOVER** : «برابری اخلاقی» → «هم‌ارزی اخلاقی»
  - PK 1321 (`7.2.1.1.2`) **MOVER** : «معادل اخلاقی» → «هم‌ارزی اخلاقی»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### À l'origine (34 groupes)
- héritées du FR : 15 / 34
- «ابهام» [fr=distinct] — 846, 667
- «اثر آزمایشگر» [fr=colliding] — 971, 1056
- «اثر تضاد» [fr=distinct] — 390, 1040
- «استدلال احساسی» [fr=distinct] — 53, 1370
- «استدلال دوری» [fr=distinct] — 699, 829
- «استدلال ریش» [fr=colliding] — 660, 859
- «استراتژی گربه مرده» [fr=colliding] — 914, 1318
- «اغراق» [fr=distinct] — 264, 893
- «افراط‌گرایی» [fr=distinct] — 84, 1263
- «تحقیر» [fr=distinct] — 325, 469
- «ترساندن» [fr=colliding] — 418, 919
- «تغییر زمین» [fr=colliding] — 983, 1319
- «تفکر کلیشه‌ای» [fr=distinct] — 26, 188
- «تکرار» [fr=distinct] — 255, 259
- «دام قدردانی» [fr=colliding] — 451, 1324
- «شایعه» [fr=distinct] — 916, 913
- «شبه‌علم» [fr=distinct] — 695, 1271
- «شخصی‌سازی» [fr=colliding] — 723, 1169
- «شستشوی مغزی» [fr=colliding] — 1302, 478
- «طنز» [fr=distinct] — 219, 231, 238, 275
- «فرضیه بعید» [fr=colliding] — 63, 763
- «قیاس سیاستمدار» [fr=colliding] — 21, 787
- «قیاس نادرست» [fr=distinct] — 784, 834
- «لطیفه» [fr=distinct] — 232, 294
- «مجاز» [fr=colliding] — 295, 866
- «مغالطه اسکاتلندی واقعی» [fr=colliding] — 616, 813
- «مفهوم دزدیده‌شده» [fr=colliding] — 779, 828
- «ناسازگاری» [fr=distinct] — 777, 805
- «نتیجه‌گیری نادرست» [fr=distinct] — 758, 1367
- «نفی نادرست» [fr=distinct] — 791, 793
- «پوپولیسم» [fr=distinct] — 309, 1211
- «چاپلوسی» [fr=distinct] — 313, 312, 510
- «کلیشه» [fr=colliding] — 619, 1199
- «کمپین زمزمه» [fr=colliding] — 882, 918

## zh — 52 groupes, 109 rangées porteuses

### Vraies créations (ancien → nouveau cités, verdict FR)
- «循环论证» — CRÉÉE par la traduction (fr : «Pétition de principe» vs «Définition circulaire»)
  - PK 698 (`4.1.1`) portait déjà «循环论证»
  - PK 183 (`2.1.1.2`) **MOVER** : «循证论证» → «循环论证»
  - PK 829 (`5.1.3.2`) portait déjà «循环论证»
  - convergence : `95db4425` 2026-05-30 PR #396
- «二分法思维» — HÉRITÉE du FR
  - PK 817 (`5.1.2.2.4.3`) portait déjà «二分法思维»
  - PK 1120 (`6.3.1.2.2.1`) **MOVER** : «非此即彼的思维» → «二分法思维»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «完美方案谬误» — CRÉÉE par la traduction (fr : «Solution parfaite» vs «Sophisme de la solution parfaite»)
  - PK 1350 (`7.2.2.2`) portait déjà «完美方案谬误»
  - PK 818 (`5.1.2.2.5`) **MOVER** : «完美解决» → «完美方案谬误»
  - convergence : `5e2477b5` 2026-05-31 PR #397
- «虚假真相效应» — HÉRITÉE du FR
  - PK 368 (`2.3.1.1.1.8`) portait déjà «虚假真相效应»
  - PK 1067 (`6.3.1.1.2.1.1`) **MOVER** : «虚假真理效应» → «虚假真相效应»
  - convergence : `5e2477b5` 2026-05-31 PR #397

### À l'origine (48 groupes)
- héritées du FR : 30 / 48
- «个性化» [fr=colliding] — 723, 1169
- «事后偏见» [fr=colliding] — 141, 1155
- «人身攻击» [fr=distinct] — 1360, 1398
- «伪科学» [fr=distinct] — 695, 1271
- «光环效应» [fr=colliding] — 302, 1231
- «刻板印象» [fr=colliding] — 619, 1199
- «卡夫卡陷阱» [fr=colliding] — 986, 161
- «双关语» [fr=distinct] — 228, 229
- «反语» [fr=distinct] — 275, 294
- «合理化» [fr=colliding] — 62, 762
- «否定前件» [fr=colliding] — 729, 722
- «含糊其辞» [fr=distinct] — 667, 820
- «吹牛» [fr=distinct] — 423, 425
- «回归谬误» [fr=colliding] — 636, 1196
- «夸张» [fr=colliding] — 233, 893
- «奉承» [fr=distinct] — 313, 422
- «实验者效应» [fr=colliding] — 971, 411, 1056
- «对比效应» [fr=distinct] — 390, 1040
- «少即是多效应» [fr=distinct] — 1239, 1042
- «心理学家的谬误» [fr=colliding] — 51, 1055
- «心理投射» [fr=colliding] — 1355, 1165
- «感恩陷阱» [fr=colliding] — 1324, 1103
- «推卸责任» [fr=distinct] — 1006, 1344, 1166
- «政治家的三段论» [fr=colliding] — 21, 787
- «政治正确» [fr=colliding] — 121, 1341
- «文字游戏» [fr=distinct] — 227, 253, 262
- «替罪羊» [fr=colliding] — 501, 960
- «死猫策略» [fr=colliding] — 914, 1318
- «沉没成本谬误» [fr=colliding] — 1020, 440
- «洗脑» [fr=colliding] — 1302, 478
- «滑坡谬误» [fr=colliding] — 705, 677
- «特殊辩护» [fr=distinct] — 55, 956
- «真正的苏格兰人谬误» [fr=colliding] — 65, 616, 813
- «离题» [fr=colliding] — 297, 1316
- «稻草人» [fr=colliding] — 1365, 894
- «聚类错觉» [fr=colliding] — 174, 643
- «草率结论» [fr=colliding] — 759, 1127
- «讽刺» [fr=distinct] — 231, 241
- «诉诸新颖» [fr=colliding] — 115, 1069
- «词义双关» [fr=distinct] — 278, 263
- «谣言» [fr=distinct] — 916, 913
- «赌徒谬误» [fr=distinct] — 717, 654
- «路灯效应» [fr=colliding] — 138, 1074
- «转喻» [fr=colliding] — 295, 866
- «轶事证据» [fr=colliding] — 34, 1087
- «道德恐慌» [fr=colliding] — 339, 922
- «错误前提» [fr=distinct] — 1295, 903
- «错误的比较» [fr=distinct] — 833, 834

## Ce que cette mesure n'établit pas

- Elle ne décide rien (règle C = ai-01 ; les décisions du 26/09 sont citées en synthèse).
- « À l'origine » = présente dès la première apparition de la cellule dans l'historique du fichier ;
  pour es/ar/fa/zh, colonnes remplies par passes massives récentes — « origine » y signifie « dès le remplissage »
  (l'import `e8482fe5` ne portait pas encore ces colonnes : la règle d'origine à l'import ne s'y déclenche pas).
- Le verdict FR se lit sur le master ACTUEL : une collision fr résolue plus tard (ex. 661/664, #1581)
  apparaît ici comme héritée — la ligne de remède est le dossier FR (grain 2), pas celui-ci.
- Ne mesure pas la qualité des traductions, seulement les égalités de titres par langue.
- ⚠️ **Piège d'instrument (corrigé ici, même famille que le grain 2)** : suivre les rangées par **PK** traverse mal
  une renumérotation. `f38cfed9` portait les PK 1000 et 520 sur deux rangées chacun ; l'index par PK lisait la
  rangée voisine comme l'ancien état de la même rangée. Cette version suit les rangées par `path`.

## Annexe — instrument rejouable (version corrigée)

```python
# Rejouer : python g3b_collisions_langs_fix.py (depuis la racine du repo, branche docs/v18-g3-collisions-7langues)
# CORRECTION (revue #1585, 26/09) : meme piege que le grain 2 — suivi des rangees par code hierarchique
# (`path`) au lieu du PK (f38cfed9 portait les PK 1000 et 520 sur deux rangees chacune), et regle d'origine
# a l'import e8482fe5 (tous les membres portent le titre actuel a l'import => origine, meme defaite puis refaite).
import subprocess, csv, io, collections, re, unicodedata

CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
LANGS = ["en", "ru", "pt", "es", "ar", "fa", "zh"]
IMPORT_PREFIX = "e8482fe5"

def git(*args):
    return subprocess.run(["git", *args], capture_output=True, text=True, encoding="utf-8", errors="replace").stdout

commits = []
for line in git("log", "--reverse", "--format=%H|%ad|%s", "--date=short", "--", CSV_PATH).strip().splitlines():
    h, d, s = line.split("|", 2)
    commits.append((h, d, s))

def parse_snapshot(text):
    """-> {path: {lang: title, "pk": pk}} — rangees suivies par path, jamais par PK."""
    if not text.strip():
        return {}
    reader = csv.reader(io.StringIO(text))
    try:
        header = next(reader)
    except StopIteration:
        return {}
    norm = [c.strip().lstrip("﻿").strip().lower() for c in header]
    if "path" not in norm:
        return {}
    i_pk, i_path = norm.index("pk") if "pk" in norm else None, norm.index("path")
    lang_cols = {}
    for lang in ["fr"] + LANGS:
        key = f"text_{lang}"
        if key in norm:
            lang_cols[lang] = norm.index(key)
    if not lang_cols:
        return {}
    need = max([i_path] + ([i_pk] if i_pk is not None else []) + list(lang_cols.values()))
    out = {}
    for row in reader:
        if not row or len(row) <= need:
            continue
        pathv = row[i_path].strip()
        if not pathv:
            continue
        vals = {"pk": row[i_pk].strip() if i_pk is not None else ""}
        for lang, idx in lang_cols.items():
            v = row[idx].strip()
            if v:
                vals[lang] = v
        out[pathv] = vals
    return out

snaps = [(h, d, s, parse_snapshot(git("show", f"{h}:{CSV_PATH}"))) for h, d, s in commits]
assert snaps[0][0].startswith(IMPORT_PREFIX), f"import attendu {IMPORT_PREFIX}, trouvé {snaps[0][0][:8]}"
IMPORT = snaps[0][3]
cur = snaps[-1][3]

# Verification du fait cite par la revue : a f38cfed9^, 6.2.2.2.3 porte deja « Equívoco antitético » en es.
f_before = parse_snapshot(git("show", "f38cfed9^:" + CSV_PATH))
assert f_before["6.2.2.2.3"].get("es") == "Equívoco antitético", f_before["6.2.2.2.3"].get("es")
assert f_before["6.2.2.3"].get("es") == "Negación no negada", f_before["6.2.2.3"].get("es")
print("[verify] revue #1585 OK: 6.2.2.2.3@f38cfed9^ es='Equívoco antitético' ; 6.2.2.3 es='Negación no negada'")

PR_RE = re.compile(r"\(#(\d+)\)\s*$")

def title_at_lang(pathv, i, lang):
    for j in range(i, -1, -1):
        row = snaps[j][3].get(pathv)
        if row is not None and lang in row:
            return row[lang]
    return None

def first_seen_with(pathv, lang):
    for j, (_, _, _, rows) in enumerate(snaps):
        row = rows.get(pathv)
        if row is not None and lang in row:
            return j
    return None

def norm_typo(t):
    if t is None:
        return None
    t = t.replace("’", "'").casefold()
    t = unicodedata.normalize("NFD", t)
    return "".join(c for c in t if not unicodedata.combining(c))

report = {}
for lang in LANGS:
    groups = collections.defaultdict(list)
    for pathv, vals in cur.items():
        t = vals.get(lang)
        if t:
            groups[t].append(pathv)
    shared = {t: paths for t, paths in groups.items() if len(paths) >= 2}
    created, rewritten, origin = [], [], []
    for title, paths in sorted(shared.items()):
        members = []
        for pathv in sorted(paths, key=lambda p: (len(p.split(".")), [int(x) for x in p.split(".")])):
            fs = first_seen_with(pathv, lang)
            conv = None
            if fs is not None:
                for j in range(len(snaps) - 1, fs, -1):
                    if title_at_lang(pathv, j, lang) == title and title_at_lang(pathv, j - 1, lang) != title:
                        conv = j
                        break
            members.append({"path": pathv, "pk": cur[pathv]["pk"], "fs": fs, "conv": conv})
        # Regle d'origine corrigee : tous les membres portent le titre actuel a l'import.
        if all(IMPORT.get(m["path"]) and IMPORT[m["path"]].get(lang) == title for m in members):
            fr_titles = {m["pk"]: cur.get(m["path"], {}).get("fr") for m in members}
            fr_collides = len({v for v in fr_titles.values() if v}) == 1 and None not in fr_titles.values()
            origin.append({"title": title, "members": members, "fr": fr_titles, "fr_collides": fr_collides})
            continue
        introduced = [m for m in members if m["conv"] is not None]
        fr_titles = {m["pk"]: cur.get(m["path"], {}).get("fr") for m in members}
        fr_collides = len({v for v in fr_titles.values() if v}) == 1 and None not in fr_titles.values()
        if not introduced:
            origin.append({"title": title, "members": members, "fr": fr_titles, "fr_collides": fr_collides})
            continue
        j = max(m["conv"] for m in introduced)
        h, d, s, _ = snaps[j]
        m = re.search(PR_RE, s)
        rec = {"title": title, "members": members,
               "olds": {mm["path"]: title_at_lang(mm["path"], j - 1, lang) for mm in members},
               "sha": h[:8], "date": d, "pr": m.group(1) if m else None, "subj": s,
               "fr": fr_titles, "fr_collides": fr_collides}
        pre = [rec["olds"][mm["path"]] for mm in members]
        strict_equal = len(set(pre)) == 1 and pre[0] is not None
        typo_equal = len({norm_typo(v) for v in pre}) == 1 and pre[0] is not None
        rec["typo"] = not strict_equal and typo_equal
        (rewritten if strict_equal else created).append(rec)
    report[lang] = {"shared": shared, "origin": origin, "rewritten": rewritten,
                    "created": created,
                    "true_created": [r for r in created if not r["typo"]],
                    "typo_created": [r for r in created if r["typo"]]}

tot_vraies = sum(len(report[l]["true_created"]) for l in LANGS)
tot_heritees = sum(1 for l in LANGS for x in report[l]["true_created"] if x["fr_collides"])
print(f"es: {len(report['es']['shared'])} groupes | origine {len(report['es']['origin'])} | vraies {len(report['es']['true_created'])}")
print(f"TOTAL vraies {tot_vraies}, héritées {tot_heritees}, par la traduction {tot_vraies - tot_heritees}")
assert len(report["es"]["shared"]) == 75 and len(report["es"]["origin"]) == 73 and len(report["es"]["true_created"]) == 2, "comptes es attendus 73 origine / 2 vraies"
assert tot_vraies == 18 and tot_heritees == 14, f"total attendu 18 vraies dont 14 héritées, obtenu {tot_vraies}/{tot_heritees}"

L = []
A = L.append
A("# Collisions de titres dans les 7 autres langues — mesure 0-écriture (pool v18, grain 3)")
A("")
A("Arbre master `b29c1b9e` (2026-09-26) · même fenêtre et méthode que le grain 2 (93 snapshots, convergence")
A("par colonne `text_<lang>`) · dispatch [#458 c.5845739927](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5845739927).")
A("")
A("> **Correction du 26/09 (revue [#1585](https://github.com/ArgumentumGames/Argumentum/pull/1585))** —")
A("> même piège que le grain 2 : la première version suivait les rangées par **PK** et a lu la renumérotation")
A("> `f38cfed9` comme un changement de titre espagnol (« Equívoco antitético », faux MOVER « Negación no")
A("> negada »). Mesuré sur `f38cfed9^` : la rangée `6.2.2.2.3` porte déjà « Equívoco antitético », c'est l'autre")
A("> rangée PK 1000 (`6.2.2.3`, « Negación no negada ») qui devient la 1407. Cette version suit les rangées par")
A("> **`path`** et classe « d'origine » toute collision déjà présente à l'import `e8482fe5`. Comptes espagnols :")
A("> 73 d'origine / **2 vraies créations** ; total toutes langues : **18 vraies créations, dont 14 héritées** du français.")
A("")
A("Discriminante : une collision `<lang>` est **héritée du FR** (les mêmes rangées partagent aussi leur titre fr —")
A("la traduction a été fidèle à une collision fr existante) ou **créée par la traduction** (les titres fr de ces")
A("rangées différaient — deux libellés distincts ont convergé en `<lang>`).")
A("")
A("## Synthèse par langue")
A("")
A("| Langue | Groupes ≥2 rangées | À l'origine | Réécrits | Créés modulo écriture | Vraies créations | dont héritées du FR | créées par la traduction |")
A("|---|---:|---:|---:|---:|---:|---:|---:|")
for lang in LANGS:
    r = report[lang]
    tc = r["true_created"]
    inherited = sum(1 for x in tc if x["fr_collides"])
    by_trad = len(tc) - inherited
    A(f"| {lang} | {len(r['shared'])} | {len(r['origin'])} | {len(r['rewritten'])} | {len(r['typo_created'])} | {len(tc)} | {inherited} | {by_trad} |")
A("")
A("« héritées du FR » : la collision fr existait sur les mêmes rangées — le remède côté FR (règle C, grain 2)")
A("la résout mécaniquement dans la langue. « créées par la traduction » : la langue porte une collision que le")
A("FR n'a PAS — matière de renommage propre. **Décisions règle C rendues le 26/09 (ai-01, revue #1585)** sur les")
A("4 créées par la traduction : retour pour ru « Ошибка игрока » (717 redevient « Софизм игрока », la passe #397")
A("avait effacé une distinction que le français garde) ; gardées : en « Deepity » (remplaçait une autre collision),")
A("zh « 循环论证 » (corrigeait une faute de sens), zh « 完美方案谬误 » (définitions identiques octet pour octet).")
A("")
for lang in LANGS:
    r = report[lang]
    A(f"## {lang} — {len(r['shared'])} groupes, {sum(len(p) for p in r['shared'].values())} rangées porteuses")
    A("")
    tc = r["true_created"]
    if tc:
        A("### Vraies créations (ancien → nouveau cités, verdict FR)")
        for rec in sorted(tc, key=lambda x: x["date"]):
            verdict = "HÉRITÉE du FR" if rec["fr_collides"] else "CRÉÉE par la traduction (fr : " + " vs ".join(f"«{v}»" for v in dict.fromkeys(rec["fr"].values())) + ")"
            A(f"- «{rec['title']}» — {verdict}")
            for m in rec["members"]:
                old = rec["olds"][m["path"]]
                if m["conv"] is not None:
                    A(f"  - PK {m['pk']} (`{m['path']}`) **MOVER** : «{old}» → «{rec['title']}»")
                else:
                    A(f"  - PK {m['pk']} (`{m['path']}`) portait déjà «{old or rec['title']}»")
            pr = f"PR #{rec['pr']}" if rec["pr"] else "sans PR au subject"
            A(f"  - convergence : `{rec['sha']}` {rec['date']} {pr}")
    if r["typo_created"]:
        A("")
        A("### Créés modulo écriture (collision préexistante, octets alignés)")
        for rec in r["typo_created"]:
            verdict = "héritée du FR" if rec["fr_collides"] else "fr non colliding"
            A(f"- «{rec['title']}» ({verdict}) — " + " vs ".join(f"PK {cur[mm['path']]['pk']} «{rec['olds'][mm['path']]}»" for mm in rec["members"]) + f" ; `{rec['sha']}` {rec['date']} PR #{rec['pr']}")
    if r["rewritten"]:
        A("")
        A("### Réécrits")
        for rec in r["rewritten"]:
            verdict = "héritée du FR" if rec["fr_collides"] else "fr non colliding"
            A(f"- «{rec['title']}» ({verdict}) — avant : «{list(rec['olds'].values())[0]}» ; `{rec['sha']}` {rec['date']} PR #{rec['pr']}")
    A("")
    A(f"### À l'origine ({len(r['origin'])} groupes)")
    inherited_o = sum(1 for x in r["origin"] if x["fr_collides"])
    A(f"- héritées du FR : {inherited_o} / {len(r['origin'])}")
    for x in r["origin"]:
        mark = "fr=colliding" if x["fr_collides"] else "fr=distinct"
        A(f"- «{x['title']}» [{mark}] — " + ", ".join(m["pk"] for m in x["members"]))
    A("")
A("## Ce que cette mesure n'établit pas")
A("")
A("- Elle ne décide rien (règle C = ai-01 ; les décisions du 26/09 sont citées en synthèse).")
A("- « À l'origine » = présente dès la première apparition de la cellule dans l'historique du fichier ;")
A("  pour es/ar/fa/zh, colonnes remplies par passes massives récentes — « origine » y signifie « dès le remplissage »")
A("  (l'import `e8482fe5` ne portait pas encore ces colonnes : la règle d'origine à l'import ne s'y déclenche pas).")
A("- Le verdict FR se lit sur le master ACTUEL : une collision fr résolue plus tard (ex. 661/664, #1581)")
A("  apparaît ici comme héritée — la ligne de remède est le dossier FR (grain 2), pas celui-ci.")
A("- Ne mesure pas la qualité des traductions, seulement les égalités de titres par langue.")
A("- ⚠️ **Piège d'instrument (corrigé ici, même famille que le grain 2)** : suivre les rangées par **PK** traverse mal")
A("  une renumérotation. `f38cfed9` portait les PK 1000 et 520 sur deux rangées chacun ; l'index par PK lisait la")
A("  rangée voisine comme l'ancien état de la même rangée. Cette version suit les rangées par `path`.")
A("")
A("## Annexe — instrument rejouable (version corrigée)")
A("")
A("``\`python")
A(open(__file__, encoding="utf-8").read().replace("``\`", "``\\`"))
A("``\`")
A("")
A(f"*po-2024 — mesure, 0 écriture sur le corpus · grain 3, corrigé revue #1585 · {tot_vraies} vraies créations"
  f" dont {tot_heritees} héritées*")

with open("docs/corpus/458-v18-g3-collisions-7langues-2026-09-26.md", "w", encoding="utf-8", newline="\n") as f:
    f.write("\n".join(L) + "\n")
print(f"dossier: {len(L)} lignes")

```

*po-2024 — mesure, 0 écriture sur le corpus · grain 3, corrigé revue #1585 · 18 vraies créations dont 14 héritées*
