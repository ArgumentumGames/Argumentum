# Campagne de fidélité Vertus — **ru** (0 écriture, lecture à trois voies)

**Mandat** : pool #458, file profonde c.5993735448 — grains 6-9, dossiers de fidélité Vertus
**en / ru / pt / es**, aucune écriture. Le dossier **en** (PR #1793, mergée) sert de troisième
colonne : chaque écart ru est relu contre **FR | EN | ru**.
**Objet** : `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` — 223 rangées, 82 colonnes,
BOM (`﻿pk`) ; deck = **131 rangées** à `card` non vide. **Statut : 0 écriture.**

---

## 1. Méthode

Écrans mécaniques **corpus entier (223)** : cellules vides, ru == fr, cellule ru sans aucun
cyrillique (contamination latine), caractères de contrôle, cellule ru < 55 % du fr (omission),
ponctuation terminale (par langue — voir §6). Puis **lecture intégrale du deck (131/131)** en
6 lots, titre et description **trois voies** (FR | EN | ru), remark intégral dès que le rapport
de longueur ru/fr sortait de `[0,7 ; 1,45]`.

Sondes ciblées inter-langues (8 langues) sur les cellules où un défaut apparaît, pour établir
si le défaut est propre au ru, partagé, ou absent ailleurs. Portée déclarée : **seul le ru a été
lu intégralement** ; les 7 autres langues ne sont vues qu'à travers ces sondes.

## 2. Écran mécanique (corpus 223)

- **0 cellule vide**, **0 ru == fr**, **0 cellule ru sans cyrillique**, **0 caractère de contrôle**,
  **0 cellule ru sous 55 % du fr** sur les champs du deck.

## 3. Défauts établis (lecture) — 5 entrées

### V-RU-01 · pk 54 `description_ru` — renversement de stance (rangée partagée avec l'EN)

| Langue | `description` |
|---|---|
| fr | « **Refuser de manipuler** l'interlocuteur par des menaces ou des promesses **liées aux conséquences**. » |
| **en** | "**Manipulating** one's interlocutor by resorting to threats or promises constitutes an **affront** to honest and ethical exchange." |
| **ru** | « **Манипулирование** собеседником посредством угроз или обещаний является **посягательством** на честный и этический обмен. » |
| **pt** | "**Manipular** o interlocutor recorrendo a ameaças ou promessas é uma **violação** da troca honesta e ética." |
| **es** | "La **manipulación** del interlocutor mediante amenazas o promesas constituye un **atentado** contra el intercambio honesto y ético." |
| ar | «رفض **التلاعب** بالمخاطَب عبر تهديدات أو وعود **مرتبطة بالعواقب**.» |
| fa | «امتناع از **دست‌کاری** مخاطب از راه تهدیدها یا وعده‌های **مرتبط با پیامدها**.» |
| zh | 「拒绝通过**与后果相关的**威胁或承诺**操纵**对话者。」 |

**Mesure sur les 8 langues** : le renversement (le vice décrit à la place de la vertu) est présent
dans **exactement {en, ru, pt, es}** ; **ar, fa, zh portent la vertu** (« refuser / s'abstenir /
refuser de manipuler »). Les 4 langues fautives ont **aussi perdu « liées aux conséquences »**
(le titre, lui, le dit dans les 8 langues). ⇒ défaut de **rangée**, pas de langue : la correction
du grain 10 traite la rangée entière (règle du GO, c.5990803984) et **ar/fa/zh fournissent le
modèle d'ancrage** de la restauration, exactement comme 3.1.2 avait tranché « conquête » pour le
grain 5 des Scénarios.

### V-RU-02 · pk 172 `description_ru` — ouverture orpheline « Это означает »

« **Это означает** признание влияния собственной культуры… » : la description s'ouvre sur un
connecteur anaphorique **sans antécédent** — la carte est un texte autonome. Les 6 autres langues
attaquent directement par le nom (fr « Reconnaître l'influence… » ; en "Recognition of…" ;
es "Reconocimiento de…"). **Frère mesuré : pt** — « **Trata-se de** reconhecer a influência… »
même construction orpheline. Famille = {ru, pt}.

### V-RU-03 · pk 176 `title_ru` — rétrécissement à « l'opposant »

FR « Tenir compte des biais idéologiques » ; la **description ru est fidèle** (« присущих
**сторонам обмена** » = parties à l'échange) mais le **titre ru** dit « Учет идеологических
предубеждений **оппонента** ». Mesure 8 langues : rétrécissement à l'opposant en **en, ru, pt,
es, ar, fa** ; **zh seul est fidèle** (« 相关方 » = la partie concernée). Défaut de rangée là encore.

### V-RU-04 · pk 128 `title_ru` — « solide » affaibli en « Приемлемая »

FR « Logique informelle **solide** » → ru « **Приемлемая** неформальная логика » (acceptable).
Présent aussi en **en** ("Acceptable"), **es** ("aceptable"), **ar** («مقبول») ; **fidèles : pt
("sólida"), fa («استوار» = ferme), zh («扎实» = solide)**. Même geste que l'**observation pk 128** du dossier EN (qui n'y était pas classée défaut).

### V-RU-05 · famille typographique ru — 4 `description_ru` sans point final

pks **8, 12, 24, 144** (mesures des extrémités : «…делающими её более понятной » (8),
«…достоверными » (12), «…необходимых гипотезах » (24), «…уместного сходства » (144)).
Le FR jumeau de ces 4 ponctue. Mesure inter-langues complète en §6 — la famille n'est **pas**
propre au ru.

## 4. Observations (neutres — aucune correction proposée)

- **Amplification systématique des remarks ru**, de la même famille que l'EN : rapport
  ru/fr mesuré jusqu'à 2,8× (pk 167 : 161 → 459 ; pk 175 : 159 → 442 ; pk 65 : 168 → 361 ;
  pk 38 : 115 → 329). Le contenu FR est préservé ; le ru **ajoute** des phrases explicatives.
- **Nominalisation partagée avec l'EN** : pk 165 — FR impératif « Anticipez les risques… » →
  ru « Проактивный анализ рисков… », exactement comme l'EN ("Proactive analysis of risks").
  Même lignée de rédaction, pas un défaut isolé.
- **pk 0 `title_ru`** : « **Обоснованный** аргумент » là où l'EN dit "Valid argument". Le ru
  **évite** la collision `valable`/`valide` que le dossier EN signalait (le corpus réserve
  "validity" au sens technique à pk 94) — potentiellement **meilleur** que l'EN ; noté, non corrigé.
- **pk 162 `description_ru` fidèle là où l'EN dérive** : ru « Требование ясных, измеримых и
  справедливых критериев… » = le FR mot pour mot ; c'est l'EN seul qui introduit « the opponent »
  et « attainable ». ⇒ la dérive EN (observée au dossier #1793) est **une singularité de l'EN**,
  pas la norme du corpus. Le cadrage « adversaire » vit, lui, au FR de la rangée 164 — dont le ru
  est fidèle.
- **pk 133 `description_ru`** : « Находит соответствия… » — le même calque verbal que l'EN
  ("Finds correspondences") au lieu du nom sujet du FR ; partagé, donc lignée.

## 5. Ce que ce dossier n'établit pas

- Les 92 rangées hors deck : écrans mécaniques seulement.
- La qualité du ru **en soi** (grammaire, idiomatique) : la lecture compare au FR et à l'EN.
- **pt / es / ar / fa / zh n'ont pas été lus** : seules les sondes ciblées (§3, §6) les touchent.
  Les familles {ru, pt}, {en, ru, pt, es} et {en, ru, pt, es, ar, fa} sont des **mesures de
  cellule**, pas des lectures de langue — les dossiers pt/es (grains 8-9) les reprendront.

## 6. Famille typographique — mesure inter-langues (corrige la borne du dossier EN)

Descriptions sans ponctuation terminale, corpus 223, **jeu de ponctuation par langue**
(ASCII + `。！？` pour zh, `؟` pour ar/fa) :

| langue | fr | ru | zh | ar | en | pt | es | fa |
|---|---|---|---|---|---|---|---|---|
| descriptions sans point | **0** | 4 | 8 | 8 | 13 | 13 | 13 | 18 |

**Contrôles** : les **remarks** sont ponctués dans **les 8 langues** (0 manquant partout), et les
**titres** n'en portent **jamais** (223/223 dans les 8 langues) — le titre est **structurellement**
exempt, ce n'est pas une famille de défaut (l'écran ne doit pas la compter).

⚠️ **Deux pièges d'instrument payés ici, tous deux mesurés** :
1. Le premier écran ne connaissait que la ponctuation ASCII : il lisait **223/223 descriptions zh
   sans point final** — alors que **215/223 finissent par `。`**. Un `0` n'est une absence que si
   l'instrument pouvait voir un `1` ; ici c'était un `223` qui n'était pas un défaut.
2. Le dossier **en** (#1793, mergé) écrit « l'asymétrie est EN-only » — **erratum** : la borne
   implicite de cette phrase était la paire FR↔EN (vrai : aucun FR jumeau ne manque son point).
   La mesure inter-langues ci-dessus montre que **chaque langue a sa propre famille** ; le FR est
   le seul zéro. L'erratum est porté dans le dossier EN lui-même (doc vivante).

---

*po-2024*
