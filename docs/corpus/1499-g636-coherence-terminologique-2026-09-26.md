# #1499 grain 4 — PK 636 « Sophisme de régression » : cohérence terminologique (mesure, ⛔ 0 écriture)

**Date :** 2026-09-26 · **Lane :** po-2024 (worker) · **Dispatch :** pool v16 grain 4 ([#458 c.5842223755](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5842223755))
**Objet :** les 8 `desc_*` réécrites par [#1563](https://github.com/ArgumentumGames/Argumentum/pull/1563) — leur terme clé est-il celui qu'emploient **le titre de la même carte** et **ses sœurs de famille** ?
**Instrument :** `tools/1499-g636-coherence-termino.py` (`--self-test` 12/12)

## 0. Verdict

**7 langues cohérentes, 1 écart — et cet écart est un écart de FORME, pas de sens.** Le titre `fa` est le seul des huit à ne pas porter le terme « régression » (رگرسیون), là où les 7 autres (dont `ar`, qui porte **les deux**) l'emploient. Aucune `desc` ne se contredit, aucune ne porte un terme d'une autre famille que son titre — sauf `en`, où la description **définit** le terme du titre (c'est le texte de l'archive).

⭐ Le résultat le plus utile n'est pas dans la carte mais dans **sa sous-famille** : hors PK 636, le mot « régression » y désigne **un autre concept** (« régression infinie », le *regressus*) en `fr`, `ru`, `pt`, `es`, `zh` et `ar`. Deux concepts, un mot, dans le même groupe de 34 cartes. Détail §2.

⛔ **Aucune écriture, aucun appel API, aucune régénération** (périmètre du dispatch).

## 1. Une ligne par langue — les deux termes cités

| langue | terme du **titre** (`text_*`) | terme de la **desc** (`desc_*`) | verdict |
|---|---|---|---|
| `fr` | **régression** — « Sophisme de *régression* » | **aucun** — « …le résultat d'une *fluctuation tout à fait normale* » | **cohérent** |
| `en` | **regression** — « *Regression* fallacy » | **average** — « the natural *return* from an extreme to the *average* » | **cohérent** — la desc **définit** le terme du titre (§3) |
| `ru` | **регрессии** — « Софизм *регрессии* » | **aucun** — « …результатом естественных *колебаний* » | **cohérent** |
| `pt` | **regressão** — « Sofisma da *regressão* » | **aucun** — « …resultado de uma *flutuação completamente normal* » | **cohérent** |
| `es` | **regresión** — « Sofisma de *regresión* » | **aucun** — « …resultado de una *fluctuación completamente normal* » | **cohérent** |
| `ar` | **الانحدار + المتوسط** — « مغالطة *الانحدار نحو المتوسط* » | **aucun** — « …نتيجة *تقلب طبيعي* تماماً » | **cohérent** — le titre porte **les deux** termes |
| `fa` | **میانگین** (moyenne) — « مغالطهٔ *بازگشت به میانگین* » | **aucun** — « …نتیجه یک *نوسان کاملاً طبیعی* » | ⚠️ **écart de forme** — seul titre sans le terme **رگرسیون** |
| `zh` | **回归** — « *回归*谬误 » | **aucun** — « …完全正常*波动* » | **cohérent** |

**Ce que « cohérent » veut dire ici, et rien de plus :** la desc ne contredit pas le terme du titre, et son silence sur ce terme est **la pratique mesurée du deck** (§3) — pas une affirmation que la desc nomme le concept aussi bien qu'une autre le ferait.

## 2. Les sœurs de famille — deux mesures, deux résultats opposés

### 2a. Les 11 sœurs de sous-sous-famille (`Relation infondée`, 633–643) : **silence**

| langue | titres portant REG ou MEAN | descs portant REG ou MEAN |
|---|---|---|
| fr · en · ru · pt · es · ar · fa · zh | **1/11** — PK 636 seul | `en` **1/11** (PK 636) · les 7 autres **0/11** |

⇒ Les 10 autres cartes du groupe nomment d'autres concepts (corrélation/causalité, pêche aux données, tireur d'élite texan, comparaisons multiples, corrélation illusoire, illusion de regroupement…). **Il n'y a rien à quoi s'aligner** : PK 636 est la seule carte du groupe à nommer ce concept, dans les 8 langues. La question du dispatch (« est-il celui qu'emploient ses sœurs ? ») a donc une réponse qui est une **absence**, et cette absence est structurelle, pas un défaut.

### 2b. Les sœurs de **sous-famille** (`Mauvaise interprétation`, 34 cartes) : **collision de sens**

Hors PK 636, le mot « régression » y désigne le *regressus* de l'infini (« régression infinie »), **un autre concept** :

| langue | cartes hors 636 portant REG | verdict |
|---|---|---|
| `fr` | 661 · 662 · 664 (`Infini trompeur`) | ⚠️ **collision** |
| `ru` | 661 · 662 · 664 | ⚠️ **collision** |
| `pt` | 661 · 662 · 664 | ⚠️ **collision** |
| `es` | 661 · 662 · 664 | ⚠️ **collision** |
| `zh` | 661 · 662 | ⚠️ **collision** |
| `ar` | 661 | ⚠️ **collision** |
| `en` | **aucune** | ✅ les deux concepts sont distingués |
| `fa` | **aucune** | ✅ distingués (636 en MEAN, 661–664 sans REG) |

- **Ce n'est PAS un défaut introduit par #1563** : cette PR n'a écrit que des `desc_*` ; les titres 661/662/664 sont antérieurs et hors de son périmètre. Le fait est **mesuré pour être nommé**, pas pour être imputé.
- En `fr`, « régression » est un terme consacré dans les deux sens (régression vers la moyenne / régression à l'infini) : la collision est celle de la langue, pas de la traduction. `en` la résout lexicalement (*regression* vs *infinite regress*), `fa` aussi.
- ⛔ **Aucune écriture proposée** : renommer 661/662/664 toucherait 3 cartes × 8 langues, hors périmètre du grain et hors de sa décision.

## 3. La pratique du deck, mesurée — pourquoi « la desc ne reprend pas le titre » n'est pas un écart

Sur les 175 cartes, part des `desc_*` qui reprennent **au moins un mot de ≥5 lettres du titre de la même carte** :

| langue | fr | en | ru | pt | es | ar | fa | zh |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| reprise | 17,1 % | 18,9 % | 9,1 % | 20,0 % | 18,3 % | 11,4 % | 28,6 % | 0,6 % |

⇒ **71 à 99 % des définitions du deck ne reprennent PAS un mot de leur titre.** Qu'une `desc` de PK 636 ne redise pas « régression » est donc **la norme**, pas une anomalie — et c'est vrai aussi de `fr`, `ru` et `en`, dont les trois textes viennent de l'archive 2022 ([#1563](https://github.com/ArgumentumGames/Argumentum/pull/1563)) : les 5 retraductions ont suivi **la même forme que la source**, elles ne s'en sont pas écartées.

⭐ Sans ce chiffre, la question du dispatch se serait lue « 7 descs sur 8 ne nomment pas le terme clé ⇒ écart » — et c'eût été une mesure prise pour un verdict.

## 4. Limites, déclarées

- **`en`** — « mean » est ambigu (nom « moyenne » / verbe « signifier ») : MEAN ne cherche que `average`. Une desc qui dirait « the mean » n'est **pas** vue.
- **`pt` / `es`** — `média`/`media` sont ambigus (« moyenne » / « médias ») : les occurrences sont **imprimées avec leur phrase** par l'instrument, jamais comptées seules.
- **§3 compare des MOTS, pas des sens** : c'est une **borne basse** de la reprise terminologique (une desc peut dire la même chose sans réutiliser le mot), et le `zh` ne partage aucun radical latin (0,6 % — attendu).
- Les familles REG/MEAN sont des listes de motifs par langue **choisies pour ce concept** ; un titre qui nommerait le concept autrement ne serait pas vu.
- Mesure limitée à PK 636 et à ses deux fratries ; ⛔ aucune écriture, aucun appel payant.

## 5. Ce qui n'est PAS conclu

- Que la `fa` doive s'aligner : **écart de forme consigné**, la décision appartient à l'owner (⛔ aucun grain d'écriture proposé ici).
- Que les descs « perdent » le sens : leur formulation (« fluctuation tout à fait normale ») est celle de `fr`/`ru` **de l'archive 2022**, restaurée délibérément par #1563.
- Que la collision §2b appelle un renommage : elle est **nommée**, avec sa portée (3 cartes, 8 langues), pas dispatchée.

## 6. Exécutable

```bash
python tools/1499-g636-coherence-termino.py              # §A titre/desc · §B fratrie · §D collision · §C pratique
python tools/1499-g636-coherence-termino.py --self-test  # 12 cas littéraux
```
