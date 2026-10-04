# Campagne de fidélité Scénarios — **es** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537. **Statut** : **0 écriture** — aucune cellule modifiée.

## 1. Instrument

Écran **multi-langue** comparant chaque cellule à **FR *et* EN**. Signaux : manque · identique
à une source · chiffre perdu/ajouté · ratio · ponctuation finale · espace CJK.

⚠️ L'écran a d'abord rendu **1169/1169** candidats sur l'espagnol : le signal « latin
résiduel » se déclenchait sur **la langue elle-même**, qui est en écriture latine. Signal
désactivé pour es/pt/en. Deux autres pièges corrigés en route (ponctuation **par écriture**,
et comparaison « cellule = FR » faussée sur le bloc EN) : détail dans le dossier `ar`.

## 2. Couverture

**1169 / 1169 cellules** (167 × 7 champs rendus) — **0 manquante.**

## 3. Candidats (29) — verdict

| Signal | Cellules | Verdict |
|---|---|---|
| `=FR` (20) · `=EN` (18) | noms propres : *Veto*, *Gretel*, *Judas*, *Hades*, *Loki*, *Thor*, *Zeus*, *Dorothy*, *Moriarty*, *Sherlock Holmes*, *Don Juan*, *Pollock*, *Ergo sum*, *Casper*, *Titanic*, *Ross*, *Rachel* | **légitimes** — noms propres identiques dans les 8 langues |
| `=FR` seul | 4.3.7 *Un fan* · 5.3.4 *Un anti-5G* · 7.1.1 *King size* | **légitimes** : cognat espagnol exact, et emprunt anglais courant en espagnol |
| `=EN` seul | 2.3.5 titre *Don Juan* (FR « Dom Juan ») | **légitime** : graphie espagnole correcte |
| `CHIFFRE-perdu` (1) · `+CHIFFRE` (2) | 1.3.2 *cinco estrellas* · 2.1.3 *1000 / 1001* · 2.2.1 *40* | **faux** : l'espagnol écrit le premier en mots et les deux autres en chiffres là où FR/EN les écrivent en mots |
| `PONCT` (5) | 1.2.3 contexte + enjeu · 2.1.8 · 2.2.1 · **7.2.3** | voir §4 |

**Aucun défaut dur.**

## 4. Ponctuation finale — dont 4 sur un **lot inter-langues**

Restreint aux cellules où **FR et EN ponctuent tous deux** :

| Carte | Champ | Langues sans ponctuation finale |
|---|---|---|
| **1.2.3** | contexte | **es** · ar · fa · zh |
| **1.2.3** | enjeu | **es** · ar · fa · zh |
| **2.1.8** | suggestion | **es** · ar · zh |
| 2.2.1 | suggestion | pt (**et l'EN lui-même**) |
| **7.2.3** | contexte | **es seul** |

⇒ 4 des 5 cellules espagnoles relèvent d'une **signature de lot** partagée avec 3 autres
langues ; **7.2.3 est propre à l'espagnol**. Pour 2.2.1, c'est l'**EN** qui omet le point et
l'espagnol le suit — le défaut est en amont.

## 5. Observation

**1.2.3 `enjeu`** — l'espagnol rend « *Intentar disuadirlo* » (infinitif nu, sans sujet) là où
le FR (« Le baratineur doit tenter de l'en dissuader. ») et l'EN ("The smooth talker must try to
dissuade him.") font une **phrase complète avec sujet**. Perte du sujet, pas du sens : à
trancher en relecture native.

## 6. Verdict es

- Couverture **complète**, **0 défaut dur**, **0 contamination**.
- **1 cellule de ponctuation propre** (7.2.3) + **4 relevant du lot partagé**.
- 1 observation de complétude de phrase (§5).
- Les 38 signaux `=FR` / `=EN` sont **tous** des noms propres ou des cognats : c'est un
  résultat, pas un silence de l'instrument — l'instrument sait voir un vrai non-traduit, il en
  a vu ailleurs dans la campagne.

*po-2024*
