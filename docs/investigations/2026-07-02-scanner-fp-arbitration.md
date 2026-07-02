# Arbitrage des 7 faux positifs du scanner i18n — Argumentum

**Date** : 2026-07-02 · **Auteur** : po-2024 (worker) · **Décision** : jsboige (post-tag)
**Contexte** : post-#640 (refonte Rules i18n), le scanner `scan_translations.py` passe de 22 → 7 findings. Les 7 résiduels étaient classés « faux positifs documentés » sans arbitrage formel. Ce document fournit cet arbitrage.

## Verdict synthétique

**Les 7 sont tous légitimes (KEEP tel-quel).** Aucun n'est un défaut de traduction. La preuve structurelle : **le français lui-même (source canonique) garde plusieurs de ces termes non traduits** — les autres langues qui font de même sont donc cohérentes, pas contaminées. Le scanner déclenche sur un seuil mécanique (< 30 % de caractères CJK/cyrilliques) inadapté aux noms propres, emprunts et mnémoniques latins.

## Détail cellule par cellule

### Fallacies (taxonomie des sophismes)

| PK | Cellule | FR (canon) | Autres langues | Verdict |
|---|---|---|---|---|
| **30** | `text_ru` = `Credo quia absurdum` | `Preuve par l'absurde fallacieuse` | EN = `Credo quia absurdum` (latin), ZH = `荒谬的信念` (traduit) | **KEEP** — locution latine canonique (Tertullien). EN la garde aussi en latin. FR fait le choix descriptif (plus clair pour un public FR). Cohérence inter-langue acceptable : le latin est l'usage établi pour ce sophisme. |
| **475** | `text_zh` = `Gish Gallop` | `Gish gallop` **(emprunt non traduit !)** | EN = `Gish gallop`, RU = `Гиш-галоп` (translit) | **KEEP** — **le FR lui-même ne traduit pas** « Gish gallop » (emprunt consacré). ZH miroir = cohérent. *Polish optionnel* : ajouter une glose chinoise ex. « Gish Gallop（斯特劳斯曼轰炸）», mais pas un défaut. |
| **927** | `text_zh` = `Creepypasta` | `Creepypasta` **(emprunt non traduit !)** | EN = `Creepypasta`, RU = `Крипипаста` | **KEEP** — idem : FR et EN gardent l'emprunt. ZH cohérent. |
| **1363** | `text_zh` = `Whataboutism（什么主义）` | `Whataboutisme` (francisé) | EN = `Whataboutism`, RU = `Ватабаутизм` | **KEEP** — emprunt + glose chinoise. La glose « 什么主义 » (litt. « quoi-isme ») est la calque chinoise usuelle. *Polish optionnel* : « 诉诸虚伪 » (appeal to hypocrisy) est la forme plus canonique en rhétorique ZH, mais la forme actuelle est compréhensible. |

### Virtues (cartes de vertus)

| PK | Cellule | FR (canon) | Verdict |
|---|---|---|---|
| **107** | `title_zh` = `Celarent 三段论` | `Syllogisme Celarent` | **KEEP** — convention standard pour les mnémoniques latins de syllogismes (Celarent, Camestres, Barbara…). « 三段论 » = « syllogisme ». Le mnémonique latin se garde tel quel dans toutes les traditions logiques. (Note : RU est incohérent — `Celarent` latin vs `Каместрес` translittéré — mais c'est un défaut RU mineur, pas ZH.) |
| **112** | `title_zh` = `Camestres 三段论` | `Syllogisme Camestres` | **KEEP** — idem. |

### Scenarii (scénarios de jeu)

| PK | Cellule | FR (canon) | Autres | Verdict |
|---|---|---|---|---|
| **6.1.3** | `title_zh` = `Johnny Johnny` | `Johnny Johnny` **(identique !)** | PT = `Johnny no Panthéon`, EN = *(vide)* | **KEEP** — nom propre. ZH = FR (identique), normal pour un nom propre. *Observation annexe* : le titre EN est **vide** (devrait être `Johnny Johnny` ou `Johnny at the Pantheon`) — c'est un trou EN, pas un défaut ZH. La référence « Panthéon » est gardée par FR/PT mais perdue par EN/RU/ZH : arbitrage de cohérence mineur, hors-scope. |

## Recommandation pour le scanner

Le seuil `< 30 % CJK/cyrillique` est trop agressif pour les **titres courts** (cellules HIGH = noms de cartes). Options pour réduire le bruit futur (non bloquant) :
1. Exempter les cellules `< 12 caractères` du check script-ratio (trop court pour conclure).
2. Ajouter une liste blanche de racines latines / emprunts consacrés (Credo, Gish, Creepypasta, Whataboutism, Celarent, Camestres, Straw man…).
3. Croiser avec le FR : si le FR garde le terme non traduit, le ZH/RU qui fait de même n'est pas un défaut.

Aucune de ces améliorations n'est requise pour le tag — ce sont des nice-to-have post-release.

## Décision attendue

**jsboige** : confirmer « garder tel-quel » pour les 7 (défaut recommandé), ou demander le polish optionnel (gloses ZH sur 475/927/1363). Ces 7 ne bloquent **pas** le tag v0.9.0 — le matériel est publiable en l'état.

---

Relates #633, #640, #140.
