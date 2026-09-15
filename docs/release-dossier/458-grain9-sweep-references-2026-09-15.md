# Pool #458 grain ⑨ — Sweep des références périmées (#1189/#1190, anciens noms de familles)

**Auteur** : po-2024 (worker) · **Date** : 2026-09-15 · **Base** : master `c089d526`
**Dispatch** : pool #458 c.5666260217 (grain ⑨) — « anciens noms de familles re-mesurés au CSV ;
exclure fichiers possédés par PR en vol ».
**Statut** : **SWEEP exécuté** — 4 erratum d'en-tête datés, 0 réécriture de fond, 0 CSV touché.

---

## §1 Méthode et périmètre

- **Re-mesure CSV d'abord** (le grain l'exige) : `Argumentum Virtues - Taxonomy.csv` sur `c089d526`
  → **8 familles, aucun ancien nom** (`Inférence maîtrisée`, `Sens quantitatif`, `Justesse lexicale`,
  `Argument pertinent`, `Argument valable`, `Honnêteté intellectuelle`, `Présentation intègre`,
  `Échange enrichissant`). Toute doc qui porte `Raisonnement valide`/`Rigueur mathématique`/
  `Langage exact` décrit donc un état antérieur au 7/08.
- Balayage `docs/**.md` + `README.md` + `CHANGELOG.md` : (a) marques IP des 5 cartes pseudonymisées
  #1189 (Harry Potter 2.3.2, Astérix 5.1.3, Matrix 5.2.2, Star Wars 5.2.4, LOTR 5.2.7 — remplacées
  le 28/08, 201 cellules, `e59ddf26`) ; (b) anciens noms de familles ; (c) descriptions du rendu
  Rules antérieures à #1190/#1202/#1206.
- **Exclusions appliquées** : fichiers possédés par PR en vol (#1381–#1386 : release-notes v2.0.0,
  CHANGELOG, CLAUDE.md, cards-catalog, guide validation éd. 3, RUNBOOK, articles v2.0.0, dossiers
  drift/tri/994-arbitrage) · dossiers d'archives (`Cards/**/Archive/`) · artefacts figés du
  `release-dossier/` (traces datées).

## §2 Résultats

| cible | vivants à corriger | action |
|---|---|---|
| **#1189 — marques IP** | 1 : `docs/quality/multilingual-drift-audit-2026-07.md` (citait pk 2.3.2 « Lord Voldemort » comme exemple courant de cognate overlap) | **erratum d'en-tête daté** — la mesure de juillet reste trace de juillet, exemples non recyclables |
| **Anciens noms de familles** | 3 : `docs/taxonomy/192-native-ratification-checklist.md`, `192-terminology-glossary-register.md`, `499-virtues-parity-closure.md` (libellés et comptes de juin) | **erratum d'en-tête datés** — renvoi aux noms courants re-mesurés + leçon #985 |
| **#1190 — rendu Rules** | 0 | **néant constaté** chez les vivants non-possédés (README décrit la structure, pas le rendu ; `visual-tests-release-gate.md` sans bannière/gouttière périmées) |
| Faux positifs écartés | — | « matrix » anglais (= matrice) dans `docs/dnn/**`, `docs/licensing/**` ; aucune marque réelle |

Les 7 `docs/taxonomy/499-aif-*-cluster.md` portent des slugs d'ancienne génération
(`rigmath`, `langage-definitions`) : contenu de chantier #499 figé, slugs = identité historique du
fichier — **non renommés** (renommer casserait les liens et falsifierait la trace).

## §3 Ce que ce sweep n'établit pas

⛔ Aucune réécriture de fond ni renommage · ⛔ aucun CSV/`Cards/` touché · ⛔ les fichiers possédés
par les PR en vol seront à re-balayer après merge (notamment si #1385/#1386 modifient des citations) ·
⛔ ne couvre pas les issue/PR bodies (traces datées par convention) ni les artefacts PDF générés.

---

*po-2024 — pool #458, grain ⑨. Le sweep date les traces ; il ne réécrit pas l'histoire.*
