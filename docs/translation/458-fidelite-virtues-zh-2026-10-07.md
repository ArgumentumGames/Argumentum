# Campagne de fidélité Vertus — **zh** (0 écriture, lecture trois voies)

**Mandat** : pool #458, dispatch ai-01 07/10 — item 1 « Vertus ar/fa/zh » (troisième et
dernier dossier de la campagne, après ar (#1800) et fa — l'EN étant le dossier de référence
`#1793`).
**Objet** : `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` — 223 rangées, 82 colonnes,
BOM (la première clé d'en-tête est `﻿pk`) ; le **deck = 131 rangées** à `card` non vide.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

Reprise de l'existant : les passes ⑯/⑰ (titres ar/fa/ru/zh, c.5944233865) et ⑱ (7 cellules
arbitrées owner) sont vérifiées mergées sur `f15c9360` — pk 175 (zh). La **zh n'avait jamais
été lue en entier**.

---

## 1. Méthode

Écrans mécaniques sur le **corpus entier (223 rangées)** — même passe instrumentale que les
trois dossiers précédents : cellules vides, ZH == FR, mojibake/caractères de contrôle, point
final absent (terminaison étendue au `。` U+3002 — sans elle, le premier passage
sur-comptait la famille zh à 223/223, faux positif corrigé avant classification), lettres
latines, cohérence des familles. Puis **lecture intégrale du deck (131/131 rangées)** en 3
lots, `title` + `description` systématiquement, `remark` intégral dès que l'écart dépassait
le seuil — **avec la réserve d'instrument suivante : les comptes de caractères ne sont pas
comparables entre écritures** (un caractère chinois porte ~2-3 caractères latins
d'information) ; la comparaison zh se fait **sur le contenu** (chaque élément du FR :
présent / absent / ajouté), jamais sur les longueurs.

**Trois voies** : FR (source) et ZH (cible) systématiques ; EN (`#1793`) consulté sur les
rangées où les dossiers EN/ar/fa documentent un écart — pks 54, 128, 162, 176, 205, 199,
217, 180. Base mesurée : `f15c9360`.

## 2. Écran mécanique (corpus 223)

- **0 cellule vide**, **0 ZH == FR**, **0 mojibake / caractère de contrôle** sur les champs
  title/description/remark × fr/zh.
- **Famille « point final absent » : 8 cellules, toutes `description_zh`** — deck pks
  4, 6, 7, 8, 9, 10 ; hors deck 3, 11. Extrémités mesurées : `…逻辑地推出结论的论证` (4),
  `…支持论证的证据` (6), `…各方立场的精确确立` (7), `…使其更易理解` (8),
  `…来增强论证` (9), `…内容或语境` (10).
- **Aucun jumeau FR de ces 8 pks n'est sans point** — asymétrie ZH-only ; les `remark_zh`
  ponctuent **tous** (0/223 sans `。`). Norme : 215/223 descriptions terminent par `。`.
- **17 occurrences de lettres latines, toutes légitimes** : les mnémoniques de logique
  formelle du corpus (Modus Barbara 106, Celarent 107, Darii 108, Ferio 109, Cesare 111,
  Camestres 112, Darapti 115, Disamis 118 ×2) et latin scolaire (*cum* 64) — mêmes termes
  qu'en FR/EN, pas une contamination.
- **Familles : 8/8, mapping 1:1 vérifié avec le FR** (有效论证 ↔ Argument valable, 相关论证
  ↔ Argument pertinent, 诚实陈述 ↔ Présentation intègre, 定量推理 ↔ Sens quantitatif,
  精熟推断 ↔ Inférence maîtrisée, 词汇精确性 ↔ Justesse lexicale, 智识诚实 ↔ Honnêteté
  intellectuelle, 充实性交流 ↔ Échange enrichissant) ; comptes 1/33/25/20/55/18/27/44 =
  223, identiques au FR.

## 3. Défauts établis — 0 sémantique, 1 typographique

**Aucune inversion de stance, aucun contresens établi sur les 131 rangées lues.** La zh est
la plus fidèle des quatre langues lues (voir §4 : les trois écarts partagés EN/ar/fa
n'existent pas en zh).

### V-ZH-01 · famille typographique — 8 points finals absents (§2)

Famille mécanique, sans ambiguïté de sens, **prête pour le grain 10** (une PR, 8 cellules
dont 6 au deck) ; langue ZH imprimée → GO owner requis. Recoupement : **deck identique à la
famille ar** {4, 6, 7, 8, 10} + 9, et 6 pks partagés avec l'EN — même génération.

## 4. Observations (neutres — aucune correction proposée ici)

- **pk 176 — la zh garde « parties prenantes »** : titre « 考虑**相关方**的意识形态偏见 »
  (les parties concernées), description identique. C'est la **seule des quatre langues** à
  ne pas rétrécir vers « l'adversaire » (EN "the opponent's", ar « الخصم », fa « طرف مقابل
  ») — la traduction directe existait donc ; les trois rétrécissements deviennent des
  candidats grain 10 renforcés, et l'incohérence interne titre/description disparaît en zh.
- **pk 205 — la zh n'ajoute pas le cadrage** : « 以最有力且最连贯的版本来解释对话者的论证 »
  = interpréter dans leur version la plus forte et la plus cohérente, miroir exact du FR ;
  le cadrage « approche intellectuellement honnête » ajouté par EN/ar/fa est absent. Titre
  「善意原则」 = terme standard exact.
- **pk 199 / pk 217 — la zh n'ajoute ni closing ni « rancune »** : le remark pk 199 s'arrête
  sur l'évitement du déraillement (comme le FR), sans la phrase de fermeture ajoutée par
  EN ("By respecting these practices…") et reprise mot à mot en fa ; le pk 217 n'a pas le
  « malentendus ou rancune » ("misunderstandings or resentment", EN/fa).
- **pk 128 — la zh garde l'intensité** : « Logique informelle **solide** » → « **扎实的**
  非形式逻辑 » (solide). Comme la fa (استوار) ; l'affaiblissement EN/ar (« acceptable »)
  n'existe ni en fa ni en zh.
- **pk 54 / pk 162 — fidèles** : stance du pk 54 préservée (« 拒绝…操纵对话者 » = refuser
  de manipuler) et pk 162 fidèle (« 清晰、可衡量且公平的标准 »). **Quatre témoins
  désormais : l'anomalie EN V-EN-01 (pk 54) est isolée à la cellule EN.**
- **Remarks : deux populations.** (1) **Majorité fidèle** — chaque élément du FR présent,
  sans ajout (pks 0, 1, 2, 24, 25, 65, 155… — la concision apparente des comptes de
  caractères est la densité de l'écriture, pas des omissions). (2) **Rangées amplifiées
  partageant la structure ar/fa mot à mot** : pks 167, 170, 171, 174, 175, 189, 192 —
  ex. pk 192, la clôture « 这构成了富有成效的对话以及在追求真理中实现集体进步的基础 »
  (fondement d'un dialogue fécond et du progrès collectif dans la recherche de la vérité)
  est l'équivalent exact de l'ar « وهو ما يشكل أساس حوار مثمر وتقدم جماعي في طلب الحقيقة »
  et de la fa « بنیان گفت‌وگویی ثمربخش و پیشرفتی جمعی در جست‌وجوی حقیقت » — les trois
  langues héritent d'une même source amplifiée (l'EN documente la même famille, obs.
  `#1793`). Contenu FR préservé partout.
- **Glissement vous→nous** : pk 192 « **我们的**知识 » (nos connaissances) — même famille
  qu'ar/fa.
- **pk 191 `title`** : « 表现共情 » (montrer de l'empathie) — verbalisation, miroir de l'ar
  (إظهار التعاطف).
- **pk 28 `remark`** : ajoute 「优秀的解释者」(le bon interprète) comme sujet — petit ajout
  absent du FR, le reste est fidèle.
- **pk 180** : desc ajoute 「强调提出并分析」(mettre en avant l'analyse) et remark 「以促进
  思想协同」(promouvoir la synergie des idées) — ce dernier est l'équivalent de l'ajout EN
  "synergy of ideas" (obs. `#1793`) ; miroir ar/fa.
- **pk 157 `description`** : ajoute 完整 (complète) + 呈现 (présentation) — même
  enrichissement que l'ar.
- **pk 186 `remark`** : ajoute 「建设性怀疑」(doute constructif) — petit ajout absent du FR.

## 5. Ce que ce dossier n'établit pas

- Les 92 rangées hors deck : écrans mécaniques seulement, pas de lecture champ à champ.
- La qualité stylistique de la ZH **en soi** (grammaire, idiomatique chinoise, choix
  entre chinois simplifié/traditionnel) : la lecture compare au FR.
- L'EN n'a été consulté que sur les 8 rangées listées en §1.
- **La campagne est complète** (EN `#1793` + ar #1800 + fa + zh) — synthèse inter-langues
  en §6 pour le grain 10.

## 6. Synthèse inter-langues de la campagne (grain 10)

| Sujet | EN | ar | fa | zh | Conséquence |
|---|---|---|---|---|---|
| **Points finals absents** (`description_*`) | 13 (deck 11) | 8 (deck 6) | 18 (deck 14) | 8 (deck 6) | **47 cellules, 4 PR mécaniques prêtes** (ou 1 par langue) — GO owner, langues imprimées |
| **pk 54 inversion de stance** | **inversé** (V-EN-01) | fidèle | fidèle | fidèle | Correction EN grain 10 confirmée par 3 témoins |
| **pk 128 affaiblissement « acceptable »** | affaibli | affaibli | **solide** | **solide** | Candidat EN+ar grain 10 — fa/zh prouvent que « solide » se traduisait sans perte |
| **pk 176 rétrécissement « l'adversaire »** | rétréci | rétréci | rétréci | **parties prenantes** | Candidat EN/ar/fa grain 10 — zh prouve la traduction directe |
| **pk 205 cadrage ajouté** | ajouté | ajouté | ajouté | **sans ajout** | Candidat EN/ar/fa grain 10 (lecture-owner) |
| pk 199/217 closings/ajouts remarks | ajoutés | — | ajoutés | **sans ajout** | Style, contenu préservé — pas de défaut établi |
| Amplification remarks | systématique | bidirectionnelle | systématique | deux populations (fidèle majoritaire) | Style d'une génération partagée, documenté — pas de défaut établi |

Aucune cellule CSV n'a été modifiée par la campagne — les quatre dossiers sont 0 écriture.

---

*po-2023*
