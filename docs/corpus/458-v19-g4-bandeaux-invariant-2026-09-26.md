# Bandeaux imprimés ≠ titre de leur niveau — mesure 0-écriture (pool v19, grain 4a, corrigé)

Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107)
· correction demandée en [revue #1588](https://github.com/ArgumentumGames/Argumentum/pull/1588) et pool v20
[#458 c.5849450061](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5849450061).

Instrument : pour chaque carte du deck (colonne `carte` non vide, 175 cartes), chaque niveau k ∈ {1, 2, 3},
chaque langue : le bandeau de la carte contre `text_<lang>` de la rangée dont le `path` est le préfixe de
longueur k — **la rangée de ce niveau étant la carte elle-même quand k est sa profondeur**.

## Règle « imprimé », tirée du gabarit (lue dans le JSON, pas déduite)

`Argumentum_Fallacies_Face_fr.json`, clé `mustache` :

```handlebars
{{#if Sous-Famille}}
  <div class="header">
    <div class="supersetWrapper">
      <div class="famille" dir="auto">{{Famille}}</div>
    	{{#if Soussousfamille}}
        	<div class="sous_famille" dir="auto">
              {{Sous-Famille}} | <span class="Soussousfamille"> {{Soussousfamille}}</span>
      	</div>
    	{{/if}}
...
```

Donc : **`Famille` s'imprime si `Sous-Famille` est rempli ; `Sous-Famille` et `Soussousfamille` s'impriment
si `Soussousfamille` est rempli.** `template.Replace` convertit les noms de champ pour les 7 autres langues,
conditions `{{#if}}` comprises (`AssetConverterConfig.cs:146-160`) — la condition se lit sur les cellules de
la même langue. Une carte de profondeur 3 imprime donc **son propre nom** dans le bandeau (« … | Équivoque »)
au-dessus de son titre. La première version de ce dossier affirmait que le niveau propre « n'imprime pas comme
un bandeau d'ancêtre » : **c'était faux**, corrigé ici.

**Chiffre de contrôle : 65 bandeaux imprimés divergent** (fr 9, en 5, ru 15, pt 6, es 3, ar 4, fa 15, zh 8) —
**46 bandeaux d'ancêtres + 19 bandeaux de la carte elle-même** — conforme à la mesure ai-01 du 26/09.
Après #1587 (retours règle C, simulé en mémoire) : **61**.

Trois motifs ressortent : (1) **28 des 46 écarts d'ancêtres tiennent à deux ancêtres renommés après coup** — `3.2.1`
(renommé par #397 `5e2477b5` dans 7 langues, les bandeaux des cartes 636/638 n'ont pas suivi) et `7.3`
« Ad hominem » (14 écarts fa/ru) ; (2) **les 5 accents fr (3 ancêtres + 2 self imprimés) sont des titres accentués
par #369 (`9d45b4f9`) sans que les bandeaux suivent** — corrigés par #1589 (extension revue : les 2 cellules self
s'impriment aussi) ; (3) l'imprimé 2022/v3 de la famille `1.2.2` portait déjà « Sophisme naturaliste » comme bandeau —
corroboration supplémentaire du retour règle C de la 96 (#1587, mergé).

## Les écarts, groupe par groupe

### Niveau `3.2.1` (PK 633) en ar — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_ar = «علاقة وهمية» vs «علاقة غير مبررة»
  - bandeau : apparu («علاقة وهمية») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «علاقة وهمية» → «علاقة غير مبررة» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_ar = «علاقة وهمية» vs «علاقة غير مبررة»
  - bandeau : apparu («علاقة وهمية») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «علاقة وهمية» → «علاقة غير مبررة» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_ar = «علاقة وهمية» vs «علاقة غير مبررة»
  - bandeau : apparu («علاقة وهمية») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «علاقة وهمية» → «علاقة غير مبررة» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `7.1.1` (PK 1282) en ar — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 1282 (`7.1.1`) *(self)* Subsubfamily_ar = «نسبية معيبة» vs «النسبية التعسفية»
  - bandeau : apparu («نسبية معيبة») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «نسبية معيبة» → «النسبية التعسفية» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `3.2.1` (PK 633) en en — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily = «Spurious relationship» vs «Unfounded relationship»
  - bandeau : apparu («Spurious relationship») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Spurious relationship» → «Unfounded relationship» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily = «Spurious relationship» vs «Unfounded relationship»
  - bandeau : apparu («Spurious relationship») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Spurious relationship» → «Unfounded relationship» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily = «Spurious relationship» vs «Unfounded relationship»
  - bandeau : apparu («Spurious relationship») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Spurious relationship» → «Unfounded relationship» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `4.1.3` (PK 719) en en — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 719 (`4.1.3`) *(self)* Subsubfamily = «Causalation» vs «Stork effect»
  - bandeau : apparu («Causalation») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Causalation» → «Stork effect» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
### Niveau `7.1.1` (PK 1282) en en — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 1282 (`7.1.1`) *(self)* Subsubfamily = «Relativism» vs «Abusive relativism»
  - bandeau : apparu («Relativism») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relativism» → «Abusive relativism» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `3.2.1` (PK 633) en es — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_es = «Relación espuria» vs «Relación infundada»
  - bandeau : apparu («Relación espuria») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «Relación espuria» → «Relación infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_es = «Relación espuria» vs «Relación infundada»
  - bandeau : apparu («Relación espuria») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «Relación espuria» → «Relación infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_es = «Relación espuria» vs «Relación infundada»
  - bandeau : apparu («Relación espuria») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «Relación espuria» → «Relación infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `2.1.3` (PK 247) en fa — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 247 (`2.1.3`) *(self)* Subsubfamily_fa = «الشعر» vs «شعر»
  - bandeau : apparu («الشعر») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «الشعر» → «شعر» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
### Niveau `3.1.3` (PK 621) en fa — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 621 (`3.1.3`) *(self)* Subsubfamily_fa = «انتقال ناصحیح» vs «انتقال ناموجه»
  - bandeau : apparu («انتقال ناصحیح») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «انتقال ناصحیح» → «انتقال ناموجه» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 622 (`3.1.3.1`) Subsubfamily_fa = «انتقال ناصحیح» vs «انتقال ناموجه»
  - bandeau : apparu («انتقال ناصحیح») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «انتقال ناصحیح» → «انتقال ناموجه» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 625 (`3.1.3.2`) Subsubfamily_fa = «انتقال ناصحیح» vs «انتقال ناموجه»
  - bandeau : apparu («انتقال ناصحیح») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «انتقال ناصحیح» → «انتقال ناموجه» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `3.2.1` (PK 633) en fa — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_fa = «رابطه‌ی کاذب» vs «رابطهٔ بی‌اساس»
  - bandeau : apparu («رابطه‌ی کاذب») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «رابطه‌ی کاذب» → «رابطهٔ بی‌اساس» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_fa = «رابطه‌ی کاذب» vs «رابطهٔ بی‌اساس»
  - bandeau : apparu («رابطه‌ی کاذب») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «رابطه‌ی کاذب» → «رابطهٔ بی‌اساس» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_fa = «رابطه‌ی کاذب» vs «رابطهٔ بی‌اساس»
  - bandeau : apparu («رابطه‌ی کاذب») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «رابطه‌ی کاذب» → «رابطهٔ بی‌اساس» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `7.1.1` (PK 1282) en fa — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 1282 (`7.1.1`) *(self)* Subsubfamily_fa = «نسبی‌گرایی افراطی» vs «نسبی‌گرایی سوءاستفاده‌آمیز»
  - bandeau : apparu («نسبی‌گرایی افراطی») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «نسبی‌گرایی افراطی» → «نسبی‌گرایی سوءاستفاده‌آمیز» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `7.3` (PK 1360) en fa — 7 carte(s)

- carte 1361 (`7.3.1`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1371 (`7.3.2`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1398 (`7.3.3`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1362 (`7.3.1.1`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1365 (`7.3.1.2`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1373 (`7.3.2.1.1`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1388 (`7.3.2.3.2`) Subfamily_fa = «حمله به شخص» vs «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `1.2.2` (PK 96) en fr — 4 carte(s)

- carte 98 (`1.2.2.2`) Soussousfamille = «Sophisme naturaliste» vs «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 104 (`1.2.2.3`) Soussousfamille = «Sophisme naturaliste» vs «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 108 (`1.2.2.4`) Soussousfamille = «Sophisme naturaliste» vs «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `62b561e7` 2025-07-27 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 105 (`1.2.2.3.1`) Soussousfamille = «Sophisme naturaliste» vs «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
### Niveau `5.3.2` (PK 855) en fr — 2 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 855 (`5.3.2`) *(self)* Soussousfamille = «Equivoque» vs «Équivoque»
  - bandeau : apparu («Equivoque») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Equivoque» → «Équivoque» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 869 (`5.3.2.3.2.1`) Soussousfamille = «Equivoque» vs «Équivoque»
  - bandeau : apparu («Equivoque») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Equivoque» → «Équivoque» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
### Niveau `7.2.1` (PK 1313) en fr — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 1313 (`7.2.1`) *(self)* Soussousfamille = «Evasion» vs «Évasion»
  - bandeau : apparu («Evasion») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Evasion» → «Évasion» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 1314 (`7.2.1.1`) Soussousfamille = «Evasion» vs «Évasion»
  - bandeau : apparu («Evasion») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Evasion» → «Évasion» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 1330 (`7.2.1.2.2`) Soussousfamille = «Evasion» vs «Évasion»
  - bandeau : apparu («Evasion») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Evasion» → «Évasion» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
### Niveau `3.1.3` (PK 621) en pt — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 621 (`3.1.3`) *(self)* Subsubfamily_pt = «Transferência Ilícita» vs «Transferência ilícita»
  - bandeau : apparu («Transferência Ilícita») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Transferência Ilícita» → «Transferência ilícita» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 622 (`3.1.3.1`) Subsubfamily_pt = «Transferência Ilícita» vs «Transferência ilícita»
  - bandeau : apparu («Transferência Ilícita») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Transferência Ilícita» → «Transferência ilícita» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 625 (`3.1.3.2`) Subsubfamily_pt = «Transferência Ilícita» vs «Transferência ilícita»
  - bandeau : apparu («Transferência Ilícita») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Transferência Ilícita» → «Transferência ilícita» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `3.2.1` (PK 633) en pt — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_pt = «Relacionamento Espúrio» vs «Relação infundada»
  - bandeau : apparu («Relacionamento Espúrio») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relacionamento Espúrio» → «Relação infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_pt = «Relacionamento Espúrio» vs «Relação infundada»
  - bandeau : apparu («Relacionamento Espúrio») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relacionamento Espúrio» → «Relação infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_pt = «Relacionamento Espúrio» vs «Relação infundada»
  - bandeau : apparu («Relacionamento Espúrio») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relacionamento Espúrio» → «Relação infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `2.3.2` (PK 420) en ru — 4 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 420 (`2.3.2`) *(self)* Subsubfamily_ru = «Игра престолов» vs «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 421 (`2.3.2.1`) Subsubfamily_ru = «Игра престолов» vs «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 432 (`2.3.2.2.1`) Subsubfamily_ru = «Игра престолов» vs «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 492 (`2.3.2.3.4`) Subsubfamily_ru = «Игра престолов» vs «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
### Niveau `3.2.1` (PK 633) en ru — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_ru = «Безосновательное отношение» vs «Необоснованная связь»
  - bandeau : apparu («Безосновательное отношение») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Безосновательное отношение» → «Необоснованная связь» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_ru = «Безосновательное отношение» vs «Необоснованная связь»
  - bandeau : apparu («Безосновательное отношение») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Безосновательное отношение» → «Необоснованная связь» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_ru = «Безосновательное отношение» vs «Необоснованная связь»
  - bandeau : apparu («Безосновательное отношение») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Безосновательное отношение» → «Необоснованная связь» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `7.1.1` (PK 1282) en ru — 1 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 1282 (`7.1.1`) *(self)* Subsubfamily_ru = «Чрезмерная относительность» vs «Злоупотребление релятивизмом»
  - bandeau : apparu («Чрезмерная относительность») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Чрезмерная относительность» → «Злоупотребление релятивизмом» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `7.3` (PK 1360) en ru — 7 carte(s)

- carte 1361 (`7.3.1`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `62b561e7` 2025-07-27 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1371 (`7.3.2`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1398 (`7.3.3`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1362 (`7.3.1.1`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1365 (`7.3.1.2`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1373 (`7.3.2.1.1`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 1388 (`7.3.2.3.2`) Subfamily_ru = «К человеку» vs «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
### Niveau `2.3.1` (PK 357) en zh — 5 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 357 (`2.3.1`) *(self)* Subsubfamily_zh = «条件反射» vs «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 358 (`2.3.1.1`) Subsubfamily_zh = «条件反射» vs «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 376 (`2.3.1.1.4`) Subsubfamily_zh = «条件反射» vs «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 361 (`2.3.1.1.1.2`) Subsubfamily_zh = «条件反射» vs «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 362 (`2.3.1.1.1.2.1`) Subsubfamily_zh = «条件反射» vs «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
### Niveau `3.2.1` (PK 633) en zh — 3 carte(s) — **self** (la carte est la rangée de ce niveau)

- carte 633 (`3.2.1`) *(self)* Subsubfamily_zh = «虚假关系» vs «无根据的关系»
  - bandeau : apparu («虚假关系») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «虚假关系» → «无根据的关系» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 636 (`3.2.1.3`) Subsubfamily_zh = «虚假关系» vs «无根据的关系»
  - bandeau : apparu («虚假关系») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «虚假关系» → «无根据的关系» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
- carte 638 (`3.2.1.4.1`) Subsubfamily_zh = «虚假关系» vs «无根据的关系»
  - bandeau : apparu («虚假关系») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «虚假关系» → «无根据的关系» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)

## Synthèse par nœud de niveau (toutes langues)

| Niveau | PK | Titre fr courant | Langues touchées | Nb écarts | Self ? |
|---|---|---|---|---:|---|
| `3.2.1` | 633 | Relation infondée | ar, en, es, fa, pt, ru, zh | 21 | oui |
| `7.3` | 1360 | Ad hominem | fa, ru | 14 | — |
| `3.1.3` | 621 | Transfert illicite | fa, pt | 6 | oui |
| `2.3.1` | 357 | Conditionnement | zh | 5 | oui |
| `2.3.2` | 420 | Jeu de pouvoir | ru | 4 | oui |
| `7.1.1` | 1282 | Relativisme abusif | ar, en, fa, ru | 4 | oui |
| `1.2.2` | 96 | Appel à la nature | fr | 4 | — |
| `7.2.1` | 1313 | Évasion | fr | 3 | oui |
| `5.3.2` | 855 | Équivoque | fr | 2 | oui |
| `2.1.3` | 247 | Poésie | fa | 1 | oui |
| `4.1.3` | 719 | Effet cigogne | en | 1 | oui |

## Ce que cette mesure n'établit pas

- Elle ne décide rien : les décisions règle C par groupe sont rendues par ai-01 (pool v20, décision 1) ;
  l'exécution des 51 cellules est le grain ④ du pool v20, la garde ④c suit.
- Les comparaisons ne portent que sur les bandeaux NON VIDES des cartes deck et les rangées à titre non vide ;
  un bandeau vide n'est pas un écart (il n'imprime rien).
- La règle « imprimé » est **dérivée du gabarit**, vérifiée par ai-01 à l'œil sur la carte 5.3.2 du bundle n°3
  — la vérification en rendu reste au verdict visuel.
- L'historique cite le dernier mouvement de chaque côté dans la fenêtre du fichier (93 commits) ; un côté
  « jamais bougé » peut l'avoir été avant l'import repo (2025-07-02).
- Les 5 accents fr (869, 1314, 1330 ancêtres ; 855, 1313 self) sont corrigés par #1589 (extension revue) ;
  ils restent comptés ici (mesure sur `9085635b`, antérieure au merge).

## Annexe — les 2 cellules « self » divergentes NON imprimées

En plus des 65 imprimées, 2 cellules du niveau propre divergent mais **ne s'impriment pas** (règle gabarit :
la carte « Ad hominem » (`7.3`, PK 1360) est de profondeur 2 et sa cellule `Soussousfamille` est vide en ru
et fa → la ligne `sous_famille` ne s'imprime pas, seul `Famille` le fait) :

- carte 1360 (`7.3`) ru Subfamily_ru = «К человеку» vs propre titre «Ad hominem» (non imprimé)
- carte 1360 (`7.3`) fa Subfamily_fa = «حمله به شخص» vs propre titre «حمله شخصی» (non imprimé)

## Annexe — instrument rejouable

```python
# Rejouer : python g4a_bandeaux.py (depuis la racine du repo, arbre master 9085635b)
# Grain 4a du pool v19, CORRIGE (revue #1588, pool v20) : mesure 0-ecriture de
# l'invariant « bandeau imprime = titre de la rangee de ce niveau ». La regle
# « imprime » est tiree du GABARIT (Argumentum_Fallacies_Face_fr.json, verifie
# in extenso) : Famille s'imprime si Sous-Famille rempli ; Sous-Famille et
# Soussousfamille s'impriment si Soussousfamille rempli. Une carte de profondeur
# 3 imprime donc SON PROPRE NOM dans le bandeau. Chiffres de controle (ai-01) :
# 65 ecarts imprimes sur 9085635b (46 ancetres + 19 self) ; 2 cellules self
# divergentes non imprimees (1360 ru/fa, profondeur 2, Soussousfamille vide).
import subprocess, csv, io, collections, re, os

CSV_PATH = "Cards/Fallacies/Argumentum Fallacies - Taxonomy.csv"
LANGS = ["fr", "en", "ru", "pt", "es", "ar", "fa", "zh"]
BAND_COLS = {
    "fr": ["Famille", "Sous-Famille", "Soussousfamille"],
    "en": ["Family", "Subfamily", "Subsubfamily"],
    **{l: [f"Family_{l}", f"Subfamily_{l}", f"Subsubfamily_{l}"] for l in ["ru", "pt", "es", "ar", "fa", "zh"]},
}

def git(*args):
    return subprocess.run(["git", *args], capture_output=True, text=True, encoding="utf-8", errors="replace").stdout

commits = [l.split("|", 2) for l in git("log", "--reverse", "--format=%H|%ad|%s", "--date=short", "--", CSV_PATH).strip().splitlines()]

def parse(text):
    """-> (rows by path) ; chaque rangee = dict des colonnes utiles."""
    if not text.strip():
        return {}
    reader = csv.reader(io.StringIO(text))
    norm = [c.strip().lstrip("﻿").strip() for c in next(reader)]
    idx = {name: i for i, name in enumerate(norm)}
    cols = ["path", "PK", "carte"] + ["text_" + l for l in LANGS] + [c for l in LANGS for c in BAND_COLS[l]]
    need_cols = [c for c in cols if c in idx]
    need = max(idx[c] for c in need_cols)
    out = {}
    for row in reader:
        if not row or len(row) <= need:
            continue
        p = row[idx["path"]].strip()
        if not p:
            continue
        d = {c: row[idx[c]].strip() for c in need_cols}
        out[p] = d
    return out

snaps = [(h, d, s, parse(git("show", f"{h}:{CSV_PATH}"))) for h, d, s in commits]
cur = snaps[-1][3]

def prints(row, k, lang):
    """Regle tiree du gabarit (verifie in extenso dans le JSON, revue #1588) :
    {{#if Sous-Famille}} englobe <div class="famille">{{Famille}}</div> ;
    {{#if Soussousfamille}} englobe « {{Sous-Famille}} | {{Soussousfamille}} ».
    Donc : Famille (k=1) s'imprime si Sous-Famille rempli ; Sous-Famille (k=2)
    et Soussousfamille (k=3) s'impriment si Soussousfamille rempli.
    template.Replace convertit les noms de champ par langue, {{#if}} compris
    (AssetConverterConfig.cs:146-160) — la condition se lit sur les cellules
    de la meme langue."""
    sub, subsub = row.get(BAND_COLS[lang][1], ""), row.get(BAND_COLS[lang][2], "")
    return bool(sub.strip()) if k == 1 else bool(subsub.strip())

def measure(rows, strict_ancestors_only=False):
    """-> liste d'ecarts (dict) pour les cartes du deck, bandeaux IMPRIMES.
    Par defaut : niveaux 1..3 Y COMPRIS le niveau propre (une carte de
    profondeur k imprime son propre nom au niveau k si Soussousfamille rempli).
    strict_ancestors_only=True : l'ancienne mesure 46 (k < profondeur)."""
    deck = [(p, r) for p, r in rows.items() if r.get("carte")]
    misses = []
    for p, r in sorted(deck, key=lambda x: (len(x[0].split(".")), [int(i) for i in x[0].split(".")])):
        segs = p.split(".")
        for k in (1, 2, 3):
            if k > len(segs):
                continue
            if strict_ancestors_only and k == len(segs):
                continue
            anc_path = ".".join(segs[:k])
            anc = rows.get(anc_path)
            for lang in LANGS:
                band_col = BAND_COLS[lang][k - 1]
                band = r.get(band_col, "")
                title = (anc or {}).get("text_" + lang, "")
                if not band or not title:
                    continue
                if not prints(r, k, lang):
                    continue  # la cellule ne s'imprime pas (regle gabarit)
                if band != title:
                    misses.append({
                        "card_pk": r.get("PK"), "card_path": p, "k": k, "lang": lang, "self": k == len(segs),
                        "band_col": band_col, "band": band, "anc_path": anc_path,
                        "anc_pk": (anc or {}).get("PK"), "title": title,
                    })
    return misses

# cellules divergentes NON imprimees au niveau propre (annexe)
def self_nonprinted(rows):
    deck = [(p, r) for p, r in rows.items() if r.get("carte")]
    out = []
    for p, r in deck:
        segs = p.split(".")
        k = len(segs)
        if k > 3:
            continue
        anc = rows.get(p)
        for lang in LANGS:
            band_col = BAND_COLS[lang][k - 1]
            band = r.get(band_col, "")
            title = (anc or {}).get("text_" + lang, "")
            if band and title and not prints(r, k, lang) and band != title:
                out.append({"card_pk": r.get("PK"), "card_path": p, "k": k, "lang": lang,
                            "band_col": band_col, "band": band, "title": title})
    return out

misses = measure(cur)
strict = measure(cur, strict_ancestors_only=True)
by_lang = collections.Counter(m["lang"] for m in misses)
n_self = sum(1 for m in misses if m["self"])
n_anc = len(misses) - n_self
print("imprimés:", dict(by_lang), "| total", len(misses), f"({n_anc} ancêtres + {n_self} self)")
assert len(misses) == 65, f"contrôle ai-01 : 65 imprimés sur 9085635b, obtenu {len(misses)}"
assert n_anc == 46 and n_self == 19, f"46 ancêtres + 19 self attendus, obtenu {n_anc}+{n_self}"
strict_by_lang = collections.Counter(m["lang"] for m in strict)
assert len(strict) == 46 and strict_by_lang["fr"] == 7, "la mesure stricte précédente (46, fr 7) doit rester reproductible"

nonprinted = self_nonprinted(cur)
print("self divergents NON imprimés:", [(x["card_pk"], x["lang"]) for x in nonprinted])
assert len(nonprinted) == 2 and all(x["card_pk"] == "1360" for x in nonprinted), \
    "2 cellules self divergentes non imprimées attendues (1360 ru/fa)"

# Simulation #1587 (retours règle C, mergés en 7cf48616) -> attendu 61
sim = {p: dict(r) for p, r in cur.items()}
for pathv, lang, new in [("2.1.1.1", "fr", "Argument par la question"), ("2.1.1.1", "en", "Argument by question"),
                          ("2.1.1.1", "ru", "Аргумент через вопрос"), ("1.2.2", "fr", "Sophisme naturaliste"),
                          ("4.1.2.6", "ru", "Софизм игрока")]:
    sim[pathv]["text_" + lang] = new
misses_sim = measure(sim)
by_lang_sim = collections.Counter(m["lang"] for m in misses_sim)
print("après #1587 (simulé):", dict(by_lang_sim), "| total", len(misses_sim))
assert len(misses_sim) == 61, f"61 attendus après simulation #1587, obtenu {len(misses_sim)}"

# --- Historique des deux côtés pour chaque écart (dernier mouvement de chaque côté) ---
def last_change(getter):
    prev, found = None, None
    for j, (_, _, _, rows) in enumerate(snaps):
        v = getter(rows)
        if v != prev:
            found = (j, prev)
            prev = v
    return found

PR_RE = re.compile(r"\(#(\d+)\)\s*$")
def commit_desc(j):
    h, d, s = snaps[j][:3]
    m = re.search(PR_RE, s)
    return f"`{h[:8]}` {d} " + (f"PR #{m.group(1)}" if m else "sans PR")

details = []
for m in misses:
    cp, k, lang, bc = m["card_path"], m["k"], m["lang"], m["band_col"]
    band_chg = last_change(lambda rows, cp=cp, bc=bc: (rows.get(cp) or {}).get(bc, ""))
    title_chg = last_change(lambda rows, ap=m["anc_path"], lg=lang: (rows.get(ap) or {}).get("text_" + lg, ""))
    def side_desc(chg, current):
        if chg is None:
            return "jamais bougé dans la fenêtre"
        j, old = chg
        if old is None or old == "":
            return f"apparu («{current}») @ {commit_desc(j)}"
        return f"«{old}» → «{current}» @ {commits[j][2][:40]} ({commit_desc(j)})"
    details.append((m, side_desc(band_chg, m["band"]), side_desc(title_chg, m["title"])))

L = []
A = L.append
A("# Bandeaux imprimés ≠ titre de leur niveau — mesure 0-écriture (pool v19, grain 4a, corrigé)")
A("")
A("Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107)")
A("· correction demandée en [revue #1588](https://github.com/ArgumentumGames/Argumentum/pull/1588) et pool v20")
A("[#458 c.5849450061](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5849450061).")
A("")
A("Instrument : pour chaque carte du deck (colonne `carte` non vide, 175 cartes), chaque niveau k ∈ {1, 2, 3},")
A("chaque langue : le bandeau de la carte contre `text_<lang>` de la rangée dont le `path` est le préfixe de")
A("longueur k — **la rangée de ce niveau étant la carte elle-même quand k est sa profondeur**.")
A("")
A("## Règle « imprimé », tirée du gabarit (lue dans le JSON, pas déduite)")
A("")
A("`Argumentum_Fallacies_Face_fr.json`, clé `mustache` :")
A("")
A("``\`handlebars")
A("{{#if Sous-Famille}}")
A("  <div class=\"header\">")
A("    <div class=\"supersetWrapper\">")
A("      <div class=\"famille\" dir=\"auto\">{{Famille}}</div>")
A("    \t{{#if Soussousfamille}}")
A("        \t<div class=\"sous_famille\" dir=\"auto\">")
A("              {{Sous-Famille}} | <span class=\"Soussousfamille\"> {{Soussousfamille}}</span>")
A("      \t</div>")
A("    \t{{/if}}")
A("...")
A("``\`")
A("")
A("Donc : **`Famille` s'imprime si `Sous-Famille` est rempli ; `Sous-Famille` et `Soussousfamille` s'impriment")
A("si `Soussousfamille` est rempli.** `template.Replace` convertit les noms de champ pour les 7 autres langues,")
A("conditions `{{#if}}` comprises (`AssetConverterConfig.cs:146-160`) — la condition se lit sur les cellules de")
A("la même langue. Une carte de profondeur 3 imprime donc **son propre nom** dans le bandeau (« … | Équivoque »)")
A("au-dessus de son titre. La première version de ce dossier affirmait que le niveau propre « n'imprime pas comme")
A("un bandeau d'ancêtre » : **c'était faux**, corrigé ici.")
A("")
A(f"**Chiffre de contrôle : {len(misses)} bandeaux imprimés divergent** (fr {by_lang['fr']}, en {by_lang['en']}, ru {by_lang['ru']},"
  f" pt {by_lang['pt']}, es {by_lang['es']}, ar {by_lang['ar']}, fa {by_lang['fa']}, zh {by_lang['zh']}) —")
A(f"**{n_anc} bandeaux d'ancêtres + {n_self} bandeaux de la carte elle-même** — conforme à la mesure ai-01 du 26/09.")
A(f"Après #1587 (retours règle C, simulé en mémoire) : **{len(misses_sim)}**.")
A("")
A("Trois motifs ressortent : (1) **28 des 46 écarts d'ancêtres tiennent à deux ancêtres renommés après coup** — `3.2.1`")
A("(renommé par #397 `5e2477b5` dans 7 langues, les bandeaux des cartes 636/638 n'ont pas suivi) et `7.3`")
A("« Ad hominem » (14 écarts fa/ru) ; (2) **les 5 accents fr (3 ancêtres + 2 self imprimés) sont des titres accentués")
A("par #369 (`9d45b4f9`) sans que les bandeaux suivent** — corrigés par #1589 (extension revue : les 2 cellules self")
A("s'impriment aussi) ; (3) l'imprimé 2022/v3 de la famille `1.2.2` portait déjà « Sophisme naturaliste » comme bandeau —")
A("corroboration supplémentaire du retour règle C de la 96 (#1587, mergé).")
A("")
A("## Les écarts, groupe par groupe")
A("")
groups = collections.defaultdict(list)
for d in details:
    m = d[0]
    groups[(m["anc_path"], m["lang"])].append(d)
for (anc_path, lang), items in sorted(groups.items(), key=lambda kv: (kv[0][1], kv[0][0])):
    m0 = items[0][0]
    self_tag = " — **self** (la carte est la rangée de ce niveau)" if any(x[0]["self"] for x in items) else ""
    A(f"### Niveau `{anc_path}` (PK {m0['anc_pk']}) en {lang} — {len(items)} carte(s){self_tag}")
    A("")
    for m, band_hist, title_hist in items:
        self_mark = " *(self)*" if m["self"] else ""
        A(f"- carte {m['card_pk']} (`{m['card_path']}`){self_mark} {m['band_col']} = «{m['band']}» vs «{m['title']}»")
        A(f"  - bandeau : {band_hist}")
        A(f"  - titre   : {title_hist}")
A("")
A("## Synthèse par nœud de niveau (toutes langues)")
A("")
A("| Niveau | PK | Titre fr courant | Langues touchées | Nb écarts | Self ? |")
A("|---|---|---|---|---:|---|")
by_anc = collections.defaultdict(lambda: collections.Counter())
anc_langs = collections.defaultdict(set)
anc_self = collections.defaultdict(bool)
for d in details:
    m = d[0]
    by_anc[m["anc_path"]]["n"] += 1
    anc_langs[m["anc_path"]].add(m["lang"])
    anc_self[m["anc_path"]] |= m["self"]
for anc_path, c in sorted(by_anc.items(), key=lambda kv: -kv[1]["n"]):
    row = cur.get(anc_path, {})
    A(f"| `{anc_path}` | {row.get('PK', '?')} | {row.get('text_fr', '?')} | {', '.join(sorted(anc_langs[anc_path]))} | {c['n']} | {'oui' if anc_self[anc_path] else '—'} |")
A("")
A("## Ce que cette mesure n'établit pas")
A("")
A("- Elle ne décide rien : les décisions règle C par groupe sont rendues par ai-01 (pool v20, décision 1) ;")
A("  l'exécution des 51 cellules est le grain ④ du pool v20, la garde ④c suit.")
A("- Les comparaisons ne portent que sur les bandeaux NON VIDES des cartes deck et les rangées à titre non vide ;")
A("  un bandeau vide n'est pas un écart (il n'imprime rien).")
A("- La règle « imprimé » est **dérivée du gabarit**, vérifiée par ai-01 à l'œil sur la carte 5.3.2 du bundle n°3")
A("  — la vérification en rendu reste au verdict visuel.")
A("- L'historique cite le dernier mouvement de chaque côté dans la fenêtre du fichier (93 commits) ; un côté")
A("  « jamais bougé » peut l'avoir été avant l'import repo (2025-07-02).")
A("- Les 5 accents fr (869, 1314, 1330 ancêtres ; 855, 1313 self) sont corrigés par #1589 (extension revue) ;")
A("  ils restent comptés ici (mesure sur `9085635b`, antérieure au merge).")
A("")
A("## Annexe — les 2 cellules « self » divergentes NON imprimées")
A("")
A("En plus des 65 imprimées, 2 cellules du niveau propre divergent mais **ne s'impriment pas** (règle gabarit :")
A("la carte « Ad hominem » (`7.3`, PK 1360) est de profondeur 2 et sa cellule `Soussousfamille` est vide en ru")
A("et fa → la ligne `sous_famille` ne s'imprime pas, seul `Famille` le fait) :")
A("")
for x in nonprinted:
    A(f"- carte {x['card_pk']} (`{x['card_path']}`) {x['lang']} {x['band_col']} = «{x['band']}» vs propre titre «{x['title']}» (non imprimé)")
A("")
A("## Annexe — instrument rejouable")
A("")
A("``\`python")
A(open(__file__, encoding="utf-8").read().replace("``\`", "``\\`"))
A("``\`")
A("")
A(f"*po-2024 — mesure, 0 écriture sur le corpus · grain 4a corrigé · {len(misses)} écarts imprimés mesurés, {len(misses_sim)} simulés après #1587*")

os.makedirs("docs/corpus", exist_ok=True)
with open("docs/corpus/458-v19-g4-bandeaux-invariant-2026-09-26.md", "w", encoding="utf-8", newline="\n") as f:
    f.write("\n".join(L) + "\n")
print(f"dossier: {len(L)} lignes ; niveaux distincts: {len(by_anc)}")

```

*po-2024 — mesure, 0 écriture sur le corpus · grain 4a corrigé · 65 écarts imprimés mesurés, 61 simulés après #1587*
