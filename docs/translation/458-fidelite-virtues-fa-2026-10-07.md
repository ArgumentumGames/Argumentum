# Campagne de fidélité Vertus — **fa** (0 écriture, lecture trois voies)

**Mandat** : pool #458, dispatch ai-01 07/10 — item 1 « Vertus ar/fa/zh » (deuxième des trois
dossiers, après `458-fidelite-virtues-ar-2026-10-07.md`, PR #1800).
**Objet** : `Cards/Fallacies/Argumentum Virtues - Taxonomy.csv` — 223 rangées, 82 colonnes,
BOM (la première clé d'en-tête est `﻿pk`) ; le **deck = 131 rangées** à `card` non vide.
**Statut** : **0 écriture** — aucune cellule CSV n'a été modifiée.

Reprise de l'existant : les passes ⑯/⑰ (titres ar/fa/ru/zh, c.5944233865) et ⑱ (7 cellules
arbitrées owner) sont vérifiées mergées sur `f15c9360` — pks 175 (fa), 168 (fa جهان‌شمولی,
relu intégral ci-dessous), 162/164 (fa sans kasra) ; comptage kasra corpus re-vérifié : **0**.
La **fa n'avait jamais été lue en entier**.

---

## 1. Méthode

Écrans mécaniques sur le **corpus entier (223 rangées)** — même passe instrumentale que les
dossiers EN (`#1793`) et ar (#1800) : cellules vides, FA == FR, mojibake/caractères de
contrôle, point final absent (terminaisons étendues `”`/`。`/`؟`), lettres latines, cohérence
des familles. Puis **lecture intégrale du deck (131/131 rangées)** en 3 lots, `title` +
`description` systématiquement, `remark` intégral dès que l'écart de longueur dépassait le
seuil (±55 ou +30 caractères), sinon extrait + compte de caractères.

**Trois voies** : FR (source) et FA (cible) systématiques ; EN (référence, `#1793`) consulté
sur les rangées où ce dossier ou le dossier ar (#1800) documentent un écart — pks 54, 128,
162, 176, 205, 199, 217. Base mesurée : `f15c9360`.

## 2. Écran mécanique (corpus 223)

- **0 cellule vide**, **0 FA == FR**, **0 mojibake / caractère de contrôle** sur les champs
  title/description/remark × fr/fa.
- **Famille « point final absent » : 18 cellules, toutes `description_fa`** — deck pks
  2, 4, 5, 6, 7, 8, 9, 10, 12, 28, 29, 31, 32, 33 ; hors deck 3, 11, 19, 30. Extrémités
  mesurées : `…استدلال‌های استوار` (2), `…پذیرفته شده‌اند استنتاج می‌کند` (4),
  `…گشوده به بازنگری است` (5), `…استوار و معتبر ارزیابی می‌شوند` (12),
  `…یا بیش‌برآوردن آن` (29), `…مقتصدانه است` (31). **La plus étendue des trois langues
  lues (18 vs 13 EN, 8 ar)**.
- **Aucun jumeau FR de ces 18 pks n'est sans point** — asymétrie FA-only ; les `remark_fa`
  ponctuent **tous** (0/223 sans point). Norme : 205/223 descriptions terminent par une
  ponctuation finale.
- **5 occurrences de lettres latines, toutes légitimes** : mnemonics de logique formelle
  (*Modus Barbara* 106, *Celarent* 107, *Cesare* 111) et latin scolaire (*cum* 64, *post
  hoc ergo propter hoc* 82, explicité en persan dans la même phrase) — pas une contamination.
- **Familles : 8/8, mapping 1:1 vérifié avec le FR** (استدلال معتبر ↔ Argument valable,
  استدلال مرتبط ↔ Argument pertinent, ارائه صادقانه ↔ Présentation intègre, استدلال کمی
  ↔ Sens quantitatif, تسلط بر استنتاج ↔ Inférence maîtrisée, دقت واژگانی ↔ Justesse
  lexicale, صداقت فکری ↔ Honnêteté intellectuelle, تبادل غنی‌ساز ↔ Échange enrichissant) ;
  comptes 1/33/25/20/55/18/27/44 = 223, identiques au FR.

## 3. Défauts établis — 0 sémantique, 1 typographique

**Aucune inversion de stance, aucun contresens établi sur les 131 rangées lues.**

### V-FA-01 · famille typographique — 18 points finals absents (§2)

Famille mécanique, sans ambiguïté de sens, **prête pour le grain 10** (une PR, 18 cellules
dont 14 au deck — la plus étendue des trois langues, aucune lecture supplémentaire
nécessaire) ; langue FA imprimée → GO owner requis. Recoupement : partage 10 pks avec la
famille ar (3, 4, 5, 6, 7, 8, 10, 11 + cœur commun) et 6 avec l'EN — même génération,
listes propres à chaque langue.

## 4. Observations (neutres — aucune correction proposée ici)

- **Amplification systématique des remarks FA — la plus marquée des langues lues** :
  mesuré 161→380 (pk 167), 159→364 (175), 148→338 (170), 140→296 (155), 123→296 (174),
  168→296 (65), 170→242 (47), 148→224 (60). Deux closings miroir **mot à mot** de l'EN
  (obs. `#1793`) : pk 199 « با رعایت این شیوه‌هاست که یک بحث می‌تواند به‌راستی ثمربخش
  باشد » = "By respecting these practices, a debate can become truly fruitful" ; pk 217
  « سوءتفاهم یا رنجش » = "misunderstandings or resentment". **Le contenu FR est préservé
  partout** — style, pas des inversions.
- **pk 128 `title` — la fa GARDE l'intensité** : « Logique informelle **solide** » → « منطق
  غیرصوری **استوار** » (solide/ferme). L'affaiblissement constaté en EN ("Acceptable") et
  ar (« مقبول », #1800) **n'existe pas en fa** — divergence à trois voies qui renforce le
  candidat grain 10 EN/ar : la leçon fa montre que le mot se traduisait sans perte.
- **pk 176 `title` — rétrécissement, troisième miroir** : FR « parties prenantes » → fa «
  طرف مقابل » (la partie adverse) ; même rétrécissement que l'EN (obs. `#1793`) et l'ar
  (#1800). La description fa dit elle-même « طرف‌های درگیر در یک تبادل » (les parties de
  l'échange) — incohérence interne identique. Trois langues sur trois rétrécissent le même
  titre : motif systématique de traduction, candidat lecture-owner au grain 10.
- **pk 205 `description` — cadrage ajouté, troisième miroir** : fa « رویکردی **از نظر فکری
  صادقانه** که قوی‌ترین و منسجم‌ترین تفسیر… » (une approche intellectuellement honnête qui
  adopte l'interprétation la plus forte…) ; même ajout que l'EN et l'ar. NB : le titre fa
  « اصل حمل به احسن » est le **terme traditionnel idiomatique exact** (principe de charité
  en logique et rhétorique arabo-persane).
- **pk 54 / pk 162 — fidèles comme l'ar** : la stance du pk 54 est préservée (« امتناع از
  دست‌کاری مخاطب… » = s'abstenir de manipuler, miroir du FR) et le pk 162 garde « روشن،
  سنجش‌پذیر و منصفانه » (clairs, mesurables et équitables) sans le cadrage adversaire
  introduit par l'EN. **L'anomalie EN (V-EN-01, `#1793`) reste isolée à la cellule EN** —
  trois langues témoins désormais.
- **Glissement de personne vous→nous** : pks 175 (« باورها، اعتقادها… ما » nos convictions),
  176 (« دیدگاه‌های ما » nos points de vue) — même famille que l'ar (#1800) ; substance
  intacte.
- **Titres enrichis** : pk 81 « استقلال مقدمات **از نتیجه** », pk 86 « **خوش‌ساخت** » (bien
  formulé), pk 211 « رد محترمانه **استدلال‌های مخالف** » — miroirs des enrichissements ar
  (#1800), tous ancrés dans la description FR de la rangée.
- **pk 219 `remark` — relexicalisation miroir ar** : « débat équitable » → « کارکرد
  دموکراتیک یک مناظره » (la tenue démocratique d'un débat ; ar : الأداء الديمقراطي) +
  amplification ; contenu (aucune interruption, aucune marginalisation) conservé.
- **pk 196 `remark`** : ouvre par « این فضیلت » (cette vertu) — mot « vertu » absent du FR ;
  cadrage corpus-cohérent, anodin.
- **pk 203 `description`** : « Aider les participants » → « اطمینان از اینکه همه…
  » (garantir que tous…) — durcissement aider→garantir, miroir ar (#1800).

## 5. Ce que ce dossier n'établit pas

- Les 92 rangées hors deck : écrans mécaniques seulement, pas de lecture champ à champ.
- La qualité stylistique de la FA **en soi** (grammaire, idiomatique persane) : la lecture
  compare au FR ; elle ne relit pas la FA comme un texte natif.
- L'EN n'a été consulté que sur les 7 rangées listées en §1 — pas relu intégralement ici.
- **zh** : dernier dossier de la campagne (écran déjà mesuré — famille de 8 points finals ;
  lecture et classification à venir).

---

*po-2023*
