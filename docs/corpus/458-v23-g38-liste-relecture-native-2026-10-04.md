# Pool #458 v23 — grain ㊱ (g38) : liste de relecture native (document de remise, 0 écriture)

**Date** : 2026-10-04 · **Branche** : `docs/458-liste-relecture-native` (depuis master, non empilée) · **Machine** : myia-po-2024
**Dispatch** : c.5971513525 item 6 — « rassembler dans un seul document les cellules que les dossiers ㉖-㉞ renvoient à un locuteur natif (ar 846, ar 855, ru 848, etc.), une ligne par cellule avec la question précise. 0 écriture. C'est ce qu'on remettra à un relecteur le jour où il y en a un. »
**Écritures CSV : 0** (`git status --porcelain Cards/` vide).

Ce document est la **file d'attente de relecture** issue de la série ㉖-㉞ (g27-g35). Trois états : **ouverte** (rien n'a pu trancher, la question est vivante), **confirmation** (une correction a été exécutée sur règle d'arbitrage, un natif peut l'infirmer), **tranchée par doctrine** (ne pas re-demander). Les citations sont re-mesurées sur le CSV de master `7dde6d8d`.

## 1. Questions ouvertes — la relecture décide (2)

| # | PK · colonne | Citation (état actuel) | Question précise |
|---|---|---|---|
| 1 | **846 · example_ar** | `رسم صورة السيدة بالسواد.` (« il a dessiné le portrait de la dame en noir ») | Le FR laisse l'ambiguïté ouverte (la dame en noir / le portrait en noir) — c'est le sophisme de la carte (amphibologie). L'arabe attache `بالسواد` à la peinture et **ferme** l'ambiguïté. **Un natif confirme-t-il que la phrase arabe n'est ambigüe nulle part ?** Si oui, proposer une formulation qui rouvre les deux lectures (le zh glose explicitement les deux — cf. g28 §1). |
| 2 | **855 · example_ar** | `ذلك الدب أكل «أفوكا».` (« cet ours a mangé un "avoka" ») | L'ar translittère le mot français **sans glose** : un lecteur arabe qui ne connaît pas le français perd le double sens (avocat/métier vs fruit ; l'arabe n'a pas d'homonyme : أفوكادو ≠ محامي). **Quelle stratégie : translittération gardée, glose ajoutée (comme zh/fa/es/ru), ou adaptation (comme pt « manga », qui possède un vrai calembour) ?** |

## 2. Confirmations — corrections exécutées, un natif peut infirmer (6)

Exécutées sur la règle d'arbitrage du 03/10 (coquilles : toujours corrigées ; jamais imprimé → correction sur délégation). La file garde leur trace : si un natif infirme, la cellule revient à l'état d'avant.

| # | PK · colonne | Correction | Statut |
|---|---|---|---|
| 3 | **848 · example_ru** | virgules normatives ajoutées autour du relatif « которые » : `Члены комитета, которые утвердили это досье, будут вызваны.` | **la moins sûre des trois ru** (g27) — corrigée par #1725 (mergé) |
| 4 | **361 · example_ru** | double « о » réparé (« удоо » → « удовлетворять ») + tiret cadratin | corrigée par #1725 (mergé) |
| 5 | **1352 · example_ru** | anglicisme médical : « васкулярная » → « сосудистая » (le mot de l'imprimé) | corrigée par #1725 (mergé) — l'imprimé v3 arbitrera tout désaccord |
| 6 | **622 · example_ar** | accord de genre : هذا → **هذه** الحديقة (jardin féminin) | corrigée par #1735 (PR ouverte au 04/10) |
| 7 | **900 · example_ar** | accord de genre : هذه → **هذا** بالتأكيد خيار (option masculin) | corrigée par #1735 (PR ouverte au 04/10) |
| 8 | **1330 · example_fa** | تیرهایم (mes flèches) → **تیله‌هایم** (mes billes — le français dit « billes ») | corrigée par #1735 (PR ouverte au 04/10) |

## 3. Tranchées par doctrine — ne pas re-demander (contexte pour le relecteur)

- **Gloses du jeu de mots « avocat »** (796/847/855 en zh, ar—855 reste ouvert §1—, fa, es, ru) : gardées (C-note définitive, c.5968567847).
- **fa 781 « منطق کتری »** (bouilloire) : nom canonique du sophisme (*kettle logic*, Freud) — gardé.
- **es 432** (« están a favor… levanten la mano quienes… ») : grammaticalement correct, deux personnes légitimes dans la phrase.
- **pt 855 « O urso comeu uma manga »** : vraie adaptation — *manga* (mangue/manche) porte un calembour en portugais.
- **en 1330 « Chewbacca defense »** : nom anglais usuel du sophisme, l'exemple n'a pas à citer Chewbacca.
- **Les 12 rangées jumeaux inter-langues** (134, 79, 154, 595, 638, 659, 699, 707, 726, 814, 977, 994) : sens conservé partout, origine mesurée (deck EN d'époque, g33) — statu quo.
- **6 cartes définition-pluriel / exemple-singulier es** (70, 176, 219, 299, 357, 1388) : gardées (deux situations de parole).
- **Registre es** : décision prise (définitions → « tú », tableau grain 4 en cours) — ce n'est plus une question de relecture.

## 4. Caveats de colonne — la mesure la plus forte reste une relecture complète

Toutes les lectures de la série ㉖-㉞ sont **uniques et non natives** ; zh, ar, fa et es n'ont **aucun arbitre imprimé** (colonnes absentes des archives). La liste ci-dessus n'est que ce que la lecture non native a repéré : elle ne remplace pas une passe complète par langue. Priorité suggérée si le temps de relecture est compté : **ar** (2 questions ouvertes + 2 confirmations), puis **fa** (1 confirmation + colonne sans arbitre), puis zh/es.

## N'établit pas

- Rien de nouveau : ce document **compile**, il ne mesure pas.
- Les colonnes hors Fallacies (Scenarii, Rules, Virtues) — hors périmètre de la série.
- L'existence d'un relecteur — c'est une file d'attente, pas une commande.

⛔ Gel `v2.0.0-review` respecté — aucune republication.

*po-2024*
