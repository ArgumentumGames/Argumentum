# Pool v22 — Grain ③ : fidélité des 175 titres portugais du deck (mesure 0-écriture)

**Arbre** : master `d9499158` (2026-09-27) · **Date** : 2026-09-27 · **Lane** : po-2024
**Méthode** : #1597 (grain ⑧) appliquée au portugais — question posée aux **175 cartes deck** :
le titre portugais porte-t-il le sens du titre français ? Jugement outillé par `desc_fr`/`desc_pt` citées.
Têtes de famille d'abord (profondeur croissante) : leurs titres sont les bandeaux imprimés sur les autres cartes.
Mesure faite sur l'arbre post-#1601 (casse de phrase pt appliquée).

## Synthèse

| Verdict | Rangées | Lecture |
|---|---:|---|
| **A — écart réel, proposition** | 4 | proposition par rangée ci-dessous, décision ai-01 |
| **P — déjà décidé, gardé** | 1 | 977 « Falácia Nirvana » (nom établi, décision #1600, appliquée #1601) |
| **C-note — fidèle, observation transverse** | 13 | 4 familles d'observations, aucune perte de sens mesurée |
| **✓ — fidèle** | 157 | traduction directe, terme établi ou reformulation fonctionnelle |

## [A] Écarts réels — propositions par rangée (décision : ai-01)

### PK 696 — `4` (profondeur 1, carte 1) « Erreur de raisonnement » || « Lógicas defeituosas »

- desc_fr : « Votre thèse repose sur un raisonnement incohérent. »
- desc_pt : « Chegar a uma conclusão através de um raciocínio falho. »
- « Lógicas defeituosas » est le **jumeau exact du défaut en corrigé** : « Faulty logics »
  (agrammatical — logic est indénombrable, proposition A du grain ⑧, tranché « Faulty reasoning »
  par #1598). La desc_pt elle-même dit « raciocínio falho ». C'est la **tête de famille de niveau 1** :
  le bandeau imprimé de tout le sous-arbre 4 porte le défaut. Proposition : **Raciocínio falho**.

### PK 677 — `3.3.1.3.1` (profondeur 4, carte 1) « Pente glissante » || « Pente fino »

- desc_fr : « Vous rejetez une proposition en prédisant une série peu probable d'événements négatifs. »
- desc_pt : « Vocês rejeitam uma proposta prevendo uma sequência improvável de eventos negativos. »
- « Pente fino » désigne en pt le **peigne fin** (dépistage minutieux) — aucun rapport avec la pente
  glissante ; ce n'est pas un terme du sophisme. L'es voisin dit « Pendiente resbaladiza » (correct),
  le terme établi pt est « rampa deslizante » (argumento da rampa deslizante). Seul cas du deck où le
  titre nomme un **autre concept** que la carte. Proposition : **Rampa deslizante**.

### PK 796 — `4.3.3.3.1` (profondeur 4, carte 2) « Quaternio terminorum » || « Falácia dos quatros termos »

- desc_pt : « O seu raciocínio utiliza quatro termos, enquanto um silogismo válido contém apenas três. »
- le numéral pt est invariable : « quatro termos », pas « quatros » — la desc_pt elle-même écrit
  « quatro ». Défaut de niveau coquille (règle typographie : on corrige toujours). Proposition :
  **Falácia dos quatro termos**.

### PK 1373 — `7.3.2.1.1` (profondeur 4, carte 2) « Reductio ad Hitlerum » || « Cartão Hitler »

- desc_fr : « Vous disqualifiez un argument en l'associant injustement à Hitler ou au nazisme. »
- desc_pt : « Vocês descartam um argumento dizendo que Hitler teria apoiado. »
- fr, es et en gardent tous le latin « Reductio ad Hitlerum ». Le pt dit « Cartão Hitler » — mais
  « cartão » en pt est la carte bancaire/de fidélité ; la carte à jouer est « carta » (« jogar a
  carta de Hitler »). Le sens (association à Hitler) est présent, la forme est un faux ami lexical.
  ⚠️ Interaction garde : « Cartão Hitler » est l'une des **3 exceptions nommées vivantes** de
  `CassePhraseGuardTests` (pt deck) — si ai-01 retient la proposition, l'exception sort de la liste
  au même geste. Proposition : **Reductio ad Hitlerum** (alignement fr/es/en, l'IRI OWL qui dérive du
  titre en « straw-man-casing » n'est pas déplacé — le latin est invariant en casse d'initiale).

## [P] Déjà décidé — non re-proposé

- PK 977 `6.2.1.1.1.1` « Solution parfaite » || « Falácia Nirvana » : décision ai-01 du 27/09 (#1600) —
  nom établi du sophisme, gardé ; casse revue par #1601.

## Annexe hors deck — rangée signalée (pas dans les 175)

- **PK 1005** `6.2.2.5.1` (hors deck) « Argument après contestation » || « Alterar os Postes da Baliza » —
  le titre pt est la traduction de l'**ancien** titre en (« Moving the goalposts ») que #1598 vient de
  remplacer par « Backpedaling » ; l'es voisin est déjà aligné (« Argumento post disputa »), le pt ne
  l'est pas. Desc_pt : « Refinando um argumento depois de ter sido refutado… » (reformulation après
  réfutation = backpedaling, pas déplacement de poteaux). Matière pour le correctif de fidélité pt ;
  la casse Title-Case de cette rangée relève du grain ⑤ (hors deck).

## [C-note] Observations transverses (aucune perte de sens mesurée)

- **n-pluriel** (PK 1024, 1174, 1242) — têtes de biais 6.3 : « Viés humano » et « Viés cultural »
  singuliers, « Vieses teóricos » pluriel (fr : pluriel partout). Même asymétrie que la famille en
  du grain ⑧.
- **n-lexique** (PK 974, 1011, 154, 1288, 1301, 322) — 974/1011 paire asymétrique (« Exigências
  aumentadas » / « Demandas mais fracas », miroir du couple en « Stronger requirements »/« Weaker
  demands ») ; 154 « Falácia do falacioso » là où l'es dit « Falacia del falacista » (le nom d'agent
  pt serait « falacista ») ; 1288 « inconfutável » (irréfutable) vs fr « infalsifiable »/en
  « unfalsifiability » (« infalsificável » existe) ; 1301 « Argumento ad nauseam » (latin + pt
  mélangés, fr garde « Argumentum ad nauseam ») ; 322 « Repulsão » nomme l'émotion là où fr/en/es
  nomment le dispositif (repoussoir/foil/espantajo) — la desc_pt porte le sens.
- **n-forme** (PK 690, 1004) — 690 « Raciocínio matemático inadequado » : titre descriptif long
  (3 mots, bandeau imprimé) mais le **sens mathématique est gardé** — le fr dit « opération », le
  pt dit « raisonnement mathématique » ; 1004 « Esquivando » gérondif là où fr/es disent
  « Couverture »/« Cobertura » — la desc_pt dit « evitar críticas », le sens est porté.
- **n-calque** (PK 994, 956) — 994 « Linguagem madeira » (le « de » manque ; es « Lengua de madera »
  l'a) ; 956 « Pleito especial » (registre juridique, es « Aceptación especial », en établi
  « special pleading » ≈ « súplica especial »).

## Ce que cette mesure n'établit pas

- Elle ne décide rien : les 4 propositions A sont de la matière à arbitrage ai-01 (règle C — garder
  par défaut ; une correction de titre deck doit vérifier collision, bandeaux qui suivent et liste
  d'exceptions de garde au moment de l'application, leçons #1595/1005 et ⑨).
- Le jugement repose sur les définitions du corpus et des termes établis ; une relecture native pt
  reste souhaitable avant correction (« Rampa deslizante », « Raciocínio falho » notamment).
- 0 unicité vérifiée pour les propositions.
- Une candidate éliminée par vérification d'octets : 153 « Falácia de **más** razões » — « más »
  (mauvais, pt littéraire) est correct et porte le sens du fr « mauvaises raisons » ; la confusion
  visuelle avec « mais » (plus) ne résiste pas au contrôle caractère par caractère. Pas un défaut.
- Ne mesure que le deck pt ; les titres pt hors deck (casse : grain ⑤ ; fidélité : non mesurée
  sauf 1005 ci-dessus) et les 6 autres langues sont hors périmètre.

## Annexe — les 175 rangées, verdict chacune

Têtes de famille d'abord (profondeur croissante, puis path). ✓ = fidèle · A# = proposition ci-dessus ·
P = décidé gardé · n-xxx = observation transverse.

| PK | path | c | titre fr | titre pt | verdict |
|---|---|---|---|---|---|
| 1 | `1` | 1 | Insuffisance | Insuficiência | ✓ |
| 175 | `2` | 1 | Influence | Influência | ✓ |
| 594 | `3` | 1 | Erreur mathématique | Erro matemático | ✓ |
| 696 | `4` | 1 | Erreur de raisonnement | Lógicas defeituosas | **A** |
| 798 | `5` | 1 | Abus de langage | Abuso da linguagem | ✓ |
| 887 | `6` | 1 | Tricherie | Trapaça | ✓ |
| 1280 | `7` | 1 | Obstruction | Obstrução | ✓ |
| 2 | `1.1` | 1 | Argument bâclé | Argumento descuidado | ✓ |
| 70 | `1.2` | 1 | Préjugé | Preconceito | ✓ |
| 133 | `1.3` | 1 | Surinterprétation | Superinterpretação | ✓ |
| 176 | `2.1` | 1 | Procédé rhétorique | Dispositivo retórico | ✓ |
| 299 | `2.2` | 1 | Appel à l’émotion | Apelo à emoção | ✓ |
| 356 | `2.3` | 1 | Manipulation mentale | Manipulação psicológica | ✓ |
| 595 | `3.1` | 1 | Généralisation abusive | Generalização abusiva | ✓ |
| 632 | `3.2` | 1 | Mauvaise interprétation | Falácia de interpretação | ✓ |
| 666 | `3.3` | 1 | Résultat invalide | Resultado inválido | ✓ |
| 697 | `4.1` | 1 | Causalité douteuse | Causalidade questionável | ✓ |
| 726 | `4.2` | 1 | Composition fautive | Composição falaciosa | ✓ |
| 758 | `4.3` | 1 | Déduction invalide | Dedução incorreta | ✓ |
| 799 | `5.1` | 1 | Définition inexacte | Definição inexata | ✓ |
| 833 | `5.2` | 1 | Comparaison fallacieuse | Comparação falaciosa | ✓ |
| 846 | `5.3` | 1 | Ambiguïté | Ambiguidade | ✓ |
| 888 | `6.1` | 1 | Arranger les faits | Manipulação de fatos | ✓ |
| 973 | `6.2` | 1 | Changement de cap | Mudança de meta | ✓ |
| 1023 | `6.3` | 1 | Raisonnement biaisé | Pensamento tendencioso | ✓ |
| 1281 | `7.1` | 1 | Refus du débat | Recusa do debate | ✓ |
| 1312 | `7.2` | 1 | Sabotage du débat | Sabotar o debate | ✓ |
| 1360 | `7.3` | 1 | Ad hominem | Ad hominem | ✓ |
| 3 | `1.1.1` | 1 | Argument vide | Argumento vazio | ✓ |
| 33 | `1.1.2` | 2 | Justification triviale | Justificação trivial | ✓ |
| 55 | `1.1.3` | 2 | Sauvetage ad hoc | Resgate ad hoc | ✓ |
| 71 | `1.2.1` | 2 | Argument d’autorité | Argumento de autoridade | ✓ |
| 112 | `1.2.3` | 2 | Sophisme moraliste | Sofisma moralista | ✓ |
| 134 | `1.3.1` | 1 | Sophisme ludique | Falácia lúdica | ✓ |
| 153 | `1.3.2` | 2 | Argument des mauvaises raisons | Falácia de más razões | ✓ |
| 165 | `1.3.3` | 1 | Manque de parcimonie | Falta de parcimônia | ✓ |
| 177 | `2.1.1` | 2 | Langage persuasif | Linguagem carregada | ✓ |
| 219 | `2.1.2` | 1 | Humour | Humor | ✓ |
| 247 | `2.1.3` | 2 | Poésie | Poesia | ✓ |
| 300 | `2.2.1` | 2 | Connivence | Connivência | ✓ |
| 322 | `2.2.2` | 2 | Repoussoir | Repulsão | n-lexique |
| 340 | `2.2.3` | 2 | Appel aux conséquences | Apelo às consequências | ✓ |
| 357 | `2.3.1` | 2 | Conditionnement | Condicionamento | ✓ |
| 420 | `2.3.2` | 2 | Jeu de pouvoir | Jogos de poder | ✓ |
| 511 | `2.3.3` | 1 | Communication non verbale | Comunicação não verbal | ✓ |
| 596 | `3.1.1` | 2 | Échantillon biaisé | Viés de amostragem | ✓ |
| 614 | `3.1.2` | 1 | Sophisme de l’accident | Falácia do acidente | ✓ |
| 621 | `3.1.3` | 2 | Transfert illicite | Transferência ilícita | ✓ |
| 633 | `3.2.1` | 2 | Relation infondée | Relação infundada | ✓ |
| 644 | `3.2.2` | 2 | Probabilités faussées | Falácia probabilística | ✓ |
| 658 | `3.2.3` | 2 | Infini trompeur | Infinito enganoso | ✓ |
| 667 | `3.3.1` | 2 | Imprécision | Imprecisão | ✓ |
| 681 | `3.3.2` | 2 | Erreur de calcul | Erro de cálculo | ✓ |
| 690 | `3.3.3` | 2 | Opération inappropriée | Raciocínio matemático inadequado | n-forme |
| 698 | `4.1.1` | 2 | Pétition de principe | Petição de princípio | ✓ |
| 707 | `4.1.2` | 2 | Inversion de causalité | Causalidade invertida | ✓ |
| 719 | `4.1.3` | 1 | Effet cigogne | Efeito cegonha | ✓ |
| 727 | `4.2.1` | 2 | Erreur de logique propositionnelle | Erro de lógica proposicional | ✓ |
| 735 | `4.2.2` | 2 | Erreur de quantification | Erro de quantificação | ✓ |
| 750 | `4.2.3` | 2 | Erreur de modalité | Erro de modalidade | ✓ |
| 759 | `4.3.1` | 2 | Conclusion hâtive | Conclusão precipitada | ✓ |
| 777 | `4.3.2` | 1 | Inconsistance | Inconsistência | ✓ |
| 784 | `4.3.3` | 1 | Syllogisme invalide | Falácia silogística | ✓ |
| 800 | `5.1.1` | 2 | Acception vague | Definição vaga | ✓ |
| 804 | `5.1.2` | 2 | Acception arbitraire | Definição arbitrária | ✓ |
| 826 | `5.1.3` | 1 | Définition incohérente | Definição inconsistente | ✓ |
| 834 | `5.2.1` | 2 | Comparaison abusive | Comparação abusiva | ✓ |
| 839 | `5.2.2` | 1 | Fausse analogie | Falsa analogia | ✓ |
| 844 | `5.2.3` | 2 | Sophisme d’association | Sofisma de associação | ✓ |
| 847 | `5.3.1` | 2 | Amphibologie | Ambiguidade sintática | ✓ |
| 855 | `5.3.2` | 1 | Équivoque | Ambiguidade semântica | ✓ |
| 876 | `5.3.3` | 2 | Ambiguïté narrative | Ambiguidade narrativa | ✓ |
| 889 | `6.1.1` | 1 | Mensonge | Mentira | ✓ |
| 942 | `6.1.2` | 2 | Fausse attribution | Atribuição falsa | ✓ |
| 953 | `6.1.3` | 2 | Attention sélective | Atenção seletiva | ✓ |
| 974 | `6.2.1` | 2 | Exigence renforcée | Exigências aumentadas | n-lexique |
| 992 | `6.2.2` | 2 | Vouloir le beurre et l’argent du beurre | Ter o bolo e comê-lo também | ✓ |
| 1011 | `6.2.3` | 1 | Exigence relâchée | Demandas mais fracas | n-lexique |
| 1024 | `6.3.1` | 2 | Biais naturels | Viés humano | n-pluriel |
| 1174 | `6.3.2` | 1 | Biais culturels | Viés cultural | n-pluriel |
| 1242 | `6.3.3` | 1 | Biais théoriques | Vieses teóricos | n-pluriel |
| 1282 | `7.1.1` | 1 | Relativisme abusif | Relativismo abusivo | ✓ |
| 1287 | `7.1.2` | 2 | Sophisme d’Explication | Sofisma de explicação | ✓ |
| 1297 | `7.1.3` | 2 | Preuve par assertion | Prova por afirmação | ✓ |
| 1313 | `7.2.1` | 1 | Évasion | Evasão | ✓ |
| 1345 | `7.2.2` | 2 | Complication exagérée | Complexificação exagerada | ✓ |
| 1352 | `7.2.3` | 1 | Empoisonnement du puits | Envenenando o poço | ✓ |
| 1361 | `7.3.1` | 2 | Procès en incohérence | Acusação de incoerência | ✓ |
| 1371 | `7.3.2` | 1 | Sophisme génétique | Sofisma genético | ✓ |
| 1398 | `7.3.3` | 1 | Attaque personnelle | Ataque pessoal | ✓ |
| 34 | `1.1.2.1` | 2 | Preuve anecdotique | Prova anedótica | ✓ |
| 43 | `1.1.2.2` | 1 | Pratique courante | Apelo à prática comum | ✓ |
| 51 | `1.1.2.3` | 2 | Sophisme du psychologue | Sofisma do psicólogo | ✓ |
| 78 | `1.2.1.2` | 1 | Appel au respect | Apelo ao respeito | ✓ |
| 98 | `1.2.2.2` | 2 | Appel à la majorité | Apelo à maioria | ✓ |
| 104 | `1.2.2.3` | 1 | Appel à la tradition | Apelo à tradição | ✓ |
| 108 | `1.2.2.4` | 1 | Appel à la nature | Apelo à natureza | ✓ |
| 154 | `1.3.2.1` | 1 | Sophisme du sophisme | Falácia do falacioso | n-lexique |
| 184 | `2.1.1.3` | 2 | Définition persuasive | Definição persuasiva | ✓ |
| 185 | `2.1.1.4` | 1 | Poncif anticritique | Clichê para encerrar pensamento | ✓ |
| 313 | `2.2.1.3` | 1 | Flatterie | Lisonja | ✓ |
| 319 | `2.2.1.4` | 1 | Appel à la pitié | Apelo à piedade | ✓ |
| 323 | `2.2.2.1` | 1 | Appel au mépris | Apelo ao desprezo | ✓ |
| 337 | `2.2.2.4` | 2 | Appel à la terreur | Apelo ao medo | ✓ |
| 358 | `2.3.1.1` | 2 | Cadrage | Enquadramento | ✓ |
| 421 | `2.3.2.1` | 2 | Charme personnel | Charme pessoal | ✓ |
| 622 | `3.1.3.1` | 2 | Sophisme de composition | Sofisma da composição | ✓ |
| 625 | `3.1.3.2` | 1 | Sophisme de division | Sofisma da divisão | ✓ |
| 636 | `3.2.1.3` | 2 | Sophisme de régression | Sofisma da regressão | ✓ |
| 659 | `3.2.3.1` | 1 | Sophisme du continuum | Falácia do continuum | ✓ |
| 670 | `3.3.1.2` | 1 | Faux équilibre | Falso equilíbrio | ✓ |
| 699 | `4.1.1.1` | 1 | Argument circulaire | Raciocínio circular | ✓ |
| 708 | `4.1.2.1` | 1 | Affirmation du conséquent | Afirmação do consequente | ✓ |
| 713 | `4.1.2.5` | 2 | Sophisme de la double faute | Sofisma dos dois erros | ✓ |
| 740 | `4.2.2.2` | 2 | Inférence immédiate erronée | Inferência imediata errada | ✓ |
| 768 | `4.3.1.2` | 1 | Fausse équivalence | Falsa equivalência | ✓ |
| 802 | `5.1.1.2` | 1 | Indéfinissabilité | Indefinibilidade | ✓ |
| 808 | `5.1.2.2` | 2 | Sophisme des corrélatifs | Sofisma dos correlativos | ✓ |
| 837 | `5.2.1.3` | 2 | Comparaison incohérente | Comparação inconsistente | ✓ |
| 845 | `5.2.3.1` | 1 | Amalgame | Amálgama | ✓ |
| 848 | `5.3.1.1` | 2 | Ponctuation ambiguë | Pontuação ambígua | ✓ |
| 908 | `6.1.1.2` | 2 | Factoïde | Factoide | ✓ |
| 943 | `6.1.2.1` | 1 | Décontextualisation | Descontextualização | ✓ |
| 989 | `6.2.1.4` | 1 | Renverser la charge de la preuve | Inverter o ônus da prova | ✓ |
| 1004 | `6.2.2.5` | 2 | Couverture | Esquivando | n-forme |
| 1015 | `6.2.3.3` | 2 | Argument de l’effort notable | Argumento do esforço notável | ✓ |
| 1020 | `6.2.3.4` | 1 | Sophisme des coûts irrécupérables | Falácia dos custos irrecuperáveis | ✓ |
| 1288 | `7.1.2.1` | 1 | Postulat infalsifiable | Postulado inconfutável | n-lexique |
| 1291 | `7.1.2.2` | 2 | Profondeur limitée | Profundidade limitada | ✓ |
| 1301 | `7.1.3.3` | 1 | Argumentum ad nauseam | Argumento ad nauseam | n-lexique |
| 1314 | `7.2.1.1` | 2 | Fausse piste | Pista falsa | ✓ |
| 1355 | `7.2.3.3` | 2 | Projection psychologique | Projeção psicológica | ✓ |
| 1357 | `7.2.3.4` | 2 | Arguments factices | Argumentos falsos | ✓ |
| 1362 | `7.3.1.1` | 2 | Tu quoque | Tu quoque | ✓ |
| 1365 | `7.3.1.2` | 1 | Homme de paille | Homem de palha | ✓ |
| 79 | `1.2.1.2.1` | 2 | Argument d’accomplissement | Argumento de realização | ✓ |
| 105 | `1.2.2.3.1` | 1 | Appel au précédent | Apelo ao precedente | ✓ |
| 121 | `1.2.3.3.2` | 2 | Politiquement correct | Correção política | ✓ |
| 128 | `1.2.3.5.1` | 2 | Raison du plus riche | Apelo à riqueza | ✓ |
| 179 | `2.1.1.1.1` | 2 | Question piège | Questão carregada | ✓ |
| 182 | `2.1.1.1.3` | 1 | Fausse alternative | Alternativa falsa | ✓ |
| 304 | `2.2.1.2.1` | 2 | Vœu pieux | Pensamento desejoso | ✓ |
| 343 | `2.2.3.1.2` | 1 | Argument du bâton | Argumento do bastão | ✓ |
| 376 | `2.3.1.1.4` | 2 | Programmation neurolinguistique | Programação neurolinguística | ✓ |
| 432 | `2.3.2.2.1` | 2 | Engagement | Comprometimento | ✓ |
| 492 | `2.3.2.3.4` | 2 | Jouer la victime | Jogar a vítima | ✓ |
| 598 | `3.1.1.1.1` | 2 | Généralisation hâtive | Generalização apressada | ✓ |
| 603 | `3.1.1.2.1` | 1 | Picorage de données | Cata de cerejas | ✓ |
| 638 | `3.2.1.4.1` | 1 | Tireur d’élite texan | Atirador de elite texano | ✓ |
| 653 | `3.2.2.3.1` | 1 | Main chaude | Falácia da mão quente | ✓ |
| 673 | `3.3.1.2.2` | 2 | Juste milieu | Justo meio | ✓ |
| 677 | `3.3.1.3.1` | 1 | Pente glissante | Pente fino | **A** |
| 680 | `3.3.1.3.4` | 2 | Hypothèse peu plausible | Hipótese improvável | ✓ |
| 729 | `4.2.1.1.1` | 2 | Négation de l’antécédent | Negação do antecedente | ✓ |
| 733 | `4.2.1.3.1` | 1 | Affirmation d’une disjonction | Afirmação de uma disjunção | ✓ |
| 752 | `4.2.3.1.1` | 2 | Sophisme de l’homme masqué | Sofisma do homem mascarado | ✓ |
| 781 | `4.3.2.2.1` | 2 | Logique du chaudron | Lógica do caldeirão | ✓ |
| 796 | `4.3.3.3.1` | 2 | Quaternio terminorum | Falácia dos quatros termos | **A** |
| 809 | `5.1.2.2.1` | 1 | Contraste perdu | Contraste perdido | ✓ |
| 813 | `5.1.2.2.3` | 2 | Sophisme du vrai Écossais | Falácia do verdadeiro escocês | ✓ |
| 814 | `5.1.2.2.4` | 1 | Faux dilemme | Falso dilema | ✓ |
| 878 | `5.3.3.1.1` | 1 | Argument par l’insinuation | Argumento por insinuação | ✓ |
| 994 | `6.2.2.1.1` | 1 | Langue de bois | Linguagem madeira | n-calque |
| 1330 | `7.2.1.2.2` | 1 | Noyer le poisson | Defesa de Chewbacca | ✓ |
| 1373 | `7.3.2.1.1` | 2 | Reductio ad Hitlerum | Cartão Hitler | **A** |
| 1388 | `7.3.2.3.2` | 2 | Attaque de la confiance en soi | Ataque à confiança | ✓ |
| 361 | `2.3.1.1.1.2` | 2 | Appât et substitution | Isca e troca | ✓ |
| 787 | `4.3.3.1.1.1` | 2 | Syllogisme du politicien | Silogismo do político | ✓ |
| 869 | `5.3.2.3.2.1` | 2 | Réification | Reificação | ✓ |
| 900 | `6.1.1.1.3.3` | 2 | Restriction mentale | Restrição mental | ✓ |
| 956 | `6.1.3.1.1.1` | 1 | Plaidoirie spéciale | Pleito especial | n-calque |
| 977 | `6.2.1.1.1.1` | 2 | Solution parfaite | Falácia Nirvana | P (gardé #1600) |
| 1092 | `6.3.1.2.1.1` | 2 | Biais de négativité | Viés de negatividade | ✓ |
| 1120 | `6.3.1.2.2.1` | 2 | Pensée dichotomique | Pensamento dicotômico | ✓ |
| 362 | `2.3.1.1.1.2.1` | 2 | Sandwich de louanges | Sanduíche de elogios | ✓ |

*po-2024 — mesure, 0 écriture sur le corpus · grain ③ · 175 rangées : 4 A / 1 P / 13 C-note / 157 ✓ (+ 1 hors deck signalée)*
