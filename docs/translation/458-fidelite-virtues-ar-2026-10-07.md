# Campagne de fidélité Vertus — **ar** (0 écriture, lecture trois voies)

**Mandat** : pool #458, dispatch ai-01 07/10 — item 1 « Vertus ar/fa/zh » (dossiers de fidélité,
aucune écriture ; la correction « pas lié à #802 » est actée — le gel #802 couvre le paquet
d'impression, pas l'analyse).
**Objet** : `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` — 223 rangées, 82 colonnes,
BOM (la première clé d'en-tête est `﻿pk`) ; le **deck = 131 rangées** à `card` non vide.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

Reprise de l'existant : les passes ⑯/⑰ (titres ar/fa/ru/zh, c.5944233865) et ⑱ (7 cellules
arbitrées owner) sont vérifiées mergées sur `f15c9360` — pks 175 (ar), 167 (ar
السعي إلى الموضوعية, relu intégral ci-dessous) ; l'**ar n'avait jamais été lu en entier** :
les passes précédentes couvraient les titres et les écrans mécaniques.

---

## 1. Méthode

Écrans mécaniques sur le **corpus entier (223 rangées)** : cellules vides, AR == FR
(non-traduit), mojibake/caractères de contrôle, point final absent (terminaisons étendues à
U+201D `”`, `。`, `؟` — le premier passage sans `。` sur-comptait la famille zh, corrigé avant
classification), lettres latines, cohérence des familles. Puis **lecture intégrale du deck
(131/131 rangées)** en 3 lots, `title` + `description` systématiquement, `remark` intégral
dès que l'écart de longueur dépassait le seuil (±55 ou +30 caractères), sinon extrait + compte
de caractères — chaque écart relu contre le FR complet.

**Trois voies** : FR (source) lu systématiquement ; AR (cible) lu systématiquement ; EN
(référence, vérifiée par le dossier `#1793`) consulté sur les rangées où ce dossier documente
un écart EN — pks 54, 128, 162, 176, 205 — pour arbitrer si l'écart ar est une divergence
propre ou un miroir de l'EN. Base mesurée : `f15c9360`.

## 2. Écran mécanique (corpus 223)

- **0 cellule vide**, **0 AR == FR**, **0 mojibake / caractère de contrôle** sur les champs
  title/description/remark × fr/ar.
- **Famille « point final absent » : 8 cellules, toutes `description_ar`** — deck pks
  4, 5, 6, 7, 8, 10 ; hors deck 3, 11. Extrémités mesurées : `…مقبولة بوصفها صحيحة` (4),
  `…مع بقائها قابلة للمراجعة` (5), `…قابلة للتحقق` (6), `…مواقف الأطراف` (7),
  `…أكثر قابلية للفهم` (8), `…لمحتواها أو سياقها` (10).
- **Aucun jumeau FR de ces 8 pks n'est sans point** — l'asymétrie est AR-only : la
  description FR ponctue systématiquement, l'AR oublie le point sur 8 rangées. Les
  `remark_ar` ponctuent **tous** (0/223 sans point). Norme : 215/223 descriptions
  terminent par une ponctuation finale.
- **7 occurrences de lettres latines, toutes légitimes** : mnémoniques de logique formelle
  (Modus Barbara pks 96/106, Celarent 107, Cesare 111, Darapti 115) et latin scolaire
  (*cum* 64, *post hoc* 82) — vocabulaire du corpus FR/EN, pas une contamination.
- **Familles : 8/8, mapping 1:1 vérifié avec le FR** (حجة معتبرة ↔ Argument valable, حجة
  ملائمة ↔ Argument pertinent, عرض نزيه ↔ Présentation intègre, استدلال كمي ↔ Sens
  quantitatif, إتقان الاستنتاج ↔ Inférence maîtrisée, دقة لفظية ↔ Justesse lexicale,
  أمانة فكرية ↔ Honnêteté intellectuelle, تبادل مُثرٍ ↔ Échange enrichissant) ; comptes
  1/33/25/20/55/18/27/44 = 223, identiques au FR.

## 3. Défauts établis — 0 sémantique, 1 typographique

**Aucune inversion de stance, aucun contresens établi sur les 131 rangées lues.** L'ar est
la plus fidèle des trois langues lues à ce jour : là où l'EN avait dérivé, l'ar suit le FR
(voir §4, pks 54 et 162 — données croisées).

### V-AR-01 · famille typographique — 8 points finals absents (§2)

« La typographie se corrige toujours » (29/09). Famille mécanique, sans ambiguïté de sens,
**prête pour le grain 10** (une PR, 8 cellules dont 6 au deck, aucune lecture
supplémentaire nécessaire) ; langue AR imprimée → GO owner requis. Recoupement : la famille
EN (13 cellules, dossier `#1793`) partage 6 pks avec elle (3, 5, 6, 7, 8, 11) — même
génération, listes propres à chaque langue.

## 4. Observations (neutres — aucune correction proposée ici)

- **Remaniements des remarks, dans les deux sens** — mesuré : amplifications 161→347 (pk 167),
  159→315 (175), 148→312 (170), 140→263 (155), 130→229 (189), 107→197 (221), 96→136 (219) ;
  abrègements 257→185 (51), 207→139 (165), 153→90 (163), 139→69 (209). À la différence de
  l'EN (amplification systématique, dossier `#1793`), l'ar réécrit dans les deux directions ;
  **le contenu FR est préservé partout** — style, pas des inversions.
- **pk 54 — l'ar préserve la stance que l'EN a perdue** (V-EN-01) : AR « رفض التلاعب
  بالمخاطَب عبر تهديدات أو وعود مرتبطة بالعواقب » = refuser de manipuler, miroir exact du
  FR. Donnée croisée : l'inversion est bien isolée à la cellule EN, pas une dérive source.
- **pk 162 — l'ar reste fidèle là où l'EN a dérivé** : AR « اشتراط معايير واضحة وقابلة
  للقياس ومنصفة لتقييم نجاح الحجة » = critères clairs, mesurables et équitables, miroir du
  FR ; le cadrage « adversaire/atteignable » introduit par l'EN (obs. `#1793`) est absent.
- **pk 128 `title` — affaiblissement lexical, miroir de l'EN** : « Logique informelle
  **solide** » → « منطق غير صوري **مقبول** » (acceptable) ; même observation que l'EN
  ("Acceptable", obs. `#1793`). La description ne suit pas (صحيحة = correcte).
- **pk 176 `title` — rétrécissement, miroir de l'EN** : FR « parties prenantes » → AR
  « لدى **الخصم** » (l'adversaire) ; incohérence interne à la carte : la description AR dit
  elle-même « أطراف التبادل » (parties de l'échange). Même observation que l'EN (obs.
  `#1793`). Candidat lecture-owner au grain 10.
- **pk 205 `description` — cadrage ajouté, miroir de l'EN** : AR « مقاربة **أمينة فكرياً**
  تعتمد أقوى تفسير… » (une approche intellectuellement honnête qui…) ; même ajout que l'EN
  (obs. `#1793`), le reste est fidèle.
- **Glissement de personne vous→nous** sur quelques rangées (pks 175 : « قناعاتنا » nos
  convictions, 176 : « حججنا » nos arguments) — registre ar courant, substance intacte ;
  documenté pour que les passes fa/zh ne le re-signalent pas.
- **Titres enrichis** : pk 81 « استقلال المقدمات **عن النتيجة** » (indépendance… de la
  conclusion — présent dans la description FR), pks 85/86 « **محكمة الصياغة** » (bien
  formulés), pk 211 « دحض محترم **للحجج المخالفة** » (des arguments contraires — présent
  dans la description FR). Enrichissements défendables, tous ancrés dans la rangée.
- **pk 219 `remark` — relexicalisation** : « débat équitable » → « الأداء الديمقراطي للنقاش »
  (la tenue démocratique du débat) + amplification ; contenu (aucune interruption, aucune
  marginalisation) intégralement conservé.
- **pk 212 `description`** : ajoute « وإسهامها إن كان يفيد في فهمها » (et sa contribution
  si elle aide à la comprendre), absent du FR ; le reste est fidèle.

## 5. Ce que ce dossier n'établit pas

- Les 92 rangées hors deck : écrans mécaniques seulement, pas de lecture champ à champ.
- La qualité stylistique de l'AR **en soi** (grammaire, idiomatique arabe) : la lecture
  compare au FR ; elle ne relit pas l'AR comme un texte natif.
- L'EN n'a été consulté que sur les 5 rangées où le dossier `#1793` documente un écart —
  pas relu intégralement ici (c'est l'objet de ce dossier-là).
- **fa et zh** : dossiers suivants de la même campagne (les écrans mécaniques y sont déjà
  mesurés — familles de points finals : 18 cellules fa, 8 cellules zh — les lectures et
  classifications restent à faire).

---

*po-2023*
