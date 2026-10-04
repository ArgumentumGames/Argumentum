# Campagne de fidélité Scénarios — **pt** (0 écriture)

**Mandat** : pool #458, dispatch c.5976781537. **Statut** : **0 écriture** — aucune cellule modifiée.

## 1. Instrument

Écran **multi-langue** comparant chaque cellule à **FR *et* EN**. Signaux : manque · identique
à une source · chiffre perdu/ajouté · ratio · ponctuation finale · latin résiduel.

⚠️ Le signal « latin résiduel » rendait **1168/1168** sur le portugais — il se déclenchait sur
**la langue elle-même**, en écriture latine : signal désactivé pour es/pt/en. Deux autres
pièges corrigés en route (ponctuation **par écriture** ; comparaison « cellule = FR » faussée
sur le bloc EN, qui rendait un faux par carte) : détail dans le dossier `ar`.

## 2. Couverture

**1169 / 1169 cellules** (167 × 7 champs rendus) — **0 manquante.**

## 3. Candidats (31) — verdict

| Signal | Cellules | Verdict |
|---|---|---|
| `=FR` (24) · `=EN` (24) | noms propres : *Veto*, *Gretel*, *Judas*, *Hades*, *Loki*, *Thor*, *Zeus*, *Dorothy*, *Alice*, *Moriarty*, *Sherlock Holmes*, *Don Juan*, *Pollock*, *Ergo sum*, *Casper*, *Titanic*, *Ross*, *Rachel*, *Charles VII*, *Louis XVI*… | **légitimes** |
| `=FR` seul | 2.1.3 *Shéhérazade* · 2.2.1 *Vade retro, Satanas.* · 7.1.1 *King size* | **légitimes** : graphie FR conservée, emprunt anglais courant en portugais |
| `=EN` seul | 1.1.2 *Cato* (FR « Caton ») · 2.2.1 *Jesus* (FR « Jésus ») · 2.3.4 *Professor Moriarty* · 2.3.5 *Don Juan* (FR « Dom Juan ») | **légitimes** : graphies portugaises correctes |
| `+CHIFFRE:3` | 4.3.7 titre « **3.ª** baliza » (FR « Troisième but », EN "Third Goal") | **légitime** : ordinaux en chiffres, usage courant |
| `PONCT` (2) | 1.1.1 enjeu · 6.3.2 suggestion | voir §4 |

**Aucun défaut dur.**

## 4. Ponctuation — le portugais suit **tantôt le FR, tantôt l'EN**

| Carte | Champ | FR | EN | pt | Lecture |
|---|---|---|---|---|---|
| **2.2.1** | suggestion | « Vade retro, Satanas**.** » | "Vade retro Satanas" (*sans point*) | « Vade retro, Satanas**.** » | le pt **ponctue** là où l'EN omet ⇒ **suit le FR** |
| **6.3.2** | suggestion | « …élire un traître**.** » | "…elect a traitor" (*sans point*) | « …eleger um traidor » (*sans point*) | le pt **omet** là où le FR ponctue ⇒ **suit l'EN** |
| **1.1.1** | enjeu | ponctué | ponctué | *sans point* | **propre au portugais** |

⇒ Confirmation mesurée que **la source de traduction n'est pas la même partout** : sur la même
langue et le même champ, le portugais recopie le FR une fois et l'EN l'autre.
**1.1.1 est la seule cellule de ponctuation imputable au portugais lui-même.**

Pour mémoire, la matrice inter-langues (cellules où FR **et** EN ponctuent) relève un lot
partagé — 1.2.3 (contexte, enjeu) en es/ar/fa/zh, 2.1.8 en es/ar/zh — **dont le portugais ne
fait pas partie** sur ces cartes.

## 5. Verdict pt

- Couverture **complète**, **0 défaut dur**, **0 contamination**.
- **1 cellule de ponctuation propre** (1.1.1) ; 6.3.2 est **héritée de l'EN**.
- Les 48 signaux `=FR` / `=EN` sont **tous** des noms propres, des graphies nationales
  correctes ou des emprunts : aucun non-traduit réel.

*po-2024*
