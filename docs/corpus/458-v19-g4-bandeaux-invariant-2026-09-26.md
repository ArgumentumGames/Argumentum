# Bandeaux imprimés ≠ titre de l'ancêtre — mesure 0-écriture (pool v19, grain 4a)

Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107).

Instrument : pour chaque carte du deck (colonne `carte` non vide, 175 cartes), chaque niveau
k ∈ {1, 2, 3} (`Famille`/`Sous-Famille`/`Soussousfamille` et équivalents par langue), chaque langue :
le bandeau de la carte contre `text_<lang>` de la rangée dont le `path` est le préfixe de longueur k.

**Chiffre de contrôle : 46 écarts** (fr 7, en 2, ru 12, pt 4, es 2, ar 2, fa 11, zh 6) —
conforme à la mesure ai-01 du 26/09. **Après le grain 3 (retours règle C, simulé en mémoire) :
42** (fr 3).

Trois motifs ressortent : (1) **28 des 46 écarts tiennent à deux ancêtres renommés après coup** — `3.2.1`
(renommé par #397 `5e2477b5` dans 7 langues, les bandeaux des cartes 636/638 n'ont pas suivi) et `7.3`
« Ad hominem » (14 écarts fa/ru) ; (2) **les 3 accents fr sont des titres accentués par #369 (`9d45b4f9`)
sans que les bandeaux suivent** — le côté à corriger est le bandeau (grain 4b, règle typographique) ;
(3) l'imprimé 2022/v3 de la famille `1.2.2` portait déjà « Sophisme naturaliste » comme bandeau —
corroboration supplémentaire du retour règle C de la 96 (grain 3).

## Les écarts, groupe par groupe

### Ancêtre `3.2.1` (PK 633) en ar — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_ar = «علاقة وهمية» vs ancêtre «علاقة غير مبررة»
  - bandeau : apparu («علاقة وهمية») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «علاقة وهمية» → «علاقة غير مبررة» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_ar : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_ar = «علاقة وهمية» vs ancêtre «علاقة غير مبررة»
  - bandeau : apparu («علاقة وهمية») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «علاقة وهمية» → «علاقة غير مبررة» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_ar : (vide/absent)
  - imprimé v3 Subsubfamily_ar : (vide/absent)

### Ancêtre `3.2.1` (PK 633) en en — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily = «Spurious relationship» vs ancêtre «Unfounded relationship»
  - bandeau : apparu («Spurious relationship») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Spurious relationship» → «Unfounded relationship» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily = «Spurious relationship» vs ancêtre «Unfounded relationship»
  - bandeau : apparu («Spurious relationship») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Spurious relationship» → «Unfounded relationship» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily : (vide/absent)
  - imprimé v3 Subsubfamily : (vide/absent)

### Ancêtre `3.2.1` (PK 633) en es — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_es = «Relación espuria» vs ancêtre «Relación infundada»
  - bandeau : apparu («Relación espuria») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «Relación espuria» → «Relación infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_es : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_es = «Relación espuria» vs ancêtre «Relación infundada»
  - bandeau : apparu («Relación espuria») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «Relación espuria» → «Relación infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_es : (vide/absent)
  - imprimé v3 Subsubfamily_es : (vide/absent)

### Ancêtre `3.1.3` (PK 621) en fa — 2 carte(s)

- carte 622 (`3.1.3.1`) Subsubfamily_fa = «انتقال ناصحیح» vs ancêtre «انتقال ناموجه»
  - bandeau : apparu («انتقال ناصحیح») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «انتقال ناصحیح» → «انتقال ناموجه» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_fa : (vide/absent)
  - imprimé v3 Subsubfamily_fa : (vide/absent)
- carte 625 (`3.1.3.2`) Subsubfamily_fa = «انتقال ناصحیح» vs ancêtre «انتقال ناموجه»
  - bandeau : apparu («انتقال ناصحیح») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «انتقال ناصحیح» → «انتقال ناموجه» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_fa : (vide/absent)

### Ancêtre `3.2.1` (PK 633) en fa — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_fa = «رابطه‌ی کاذب» vs ancêtre «رابطهٔ بی‌اساس»
  - bandeau : apparu («رابطه‌ی کاذب») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «رابطه‌ی کاذب» → «رابطهٔ بی‌اساس» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_fa : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_fa = «رابطه‌ی کاذب» vs ancêtre «رابطهٔ بی‌اساس»
  - bandeau : apparu («رابطه‌ی کاذب») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «رابطه‌ی کاذب» → «رابطهٔ بی‌اساس» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_fa : (vide/absent)
  - imprimé v3 Subsubfamily_fa : (vide/absent)

### Ancêtre `7.3` (PK 1360) en fa — 7 carte(s)

- carte 1361 (`7.3.1`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1371 (`7.3.2`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_fa : (vide/absent)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1398 (`7.3.3`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_fa : (vide/absent)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1362 (`7.3.1.1`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_fa : (vide/absent)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1365 (`7.3.1.2`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_fa : (vide/absent)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1373 (`7.3.2.1.1`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_fa : (vide/absent)
- carte 1388 (`7.3.2.3.2`) Subfamily_fa = «حمله به شخص» vs ancêtre «حمله شخصی»
  - bandeau : apparu («حمله به شخص») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «حمله به شخص» → «حمله شخصی» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_fa : (vide/absent)

### Ancêtre `1.2.2` (PK 96) en fr — 4 carte(s)

- carte 98 (`1.2.2.2`) Soussousfamille = «Sophisme naturaliste» vs ancêtre «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
  - imprimé 2022 Soussousfamille : «Sophisme naturaliste»
  - imprimé v3 Soussousfamille : «Sophisme naturaliste»
- carte 104 (`1.2.2.3`) Soussousfamille = «Sophisme naturaliste» vs ancêtre «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
  - imprimé 2022 Soussousfamille : «Sophisme naturaliste»
  - imprimé v3 Soussousfamille : «Sophisme naturaliste»
- carte 108 (`1.2.2.4`) Soussousfamille = «Sophisme naturaliste» vs ancêtre «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `62b561e7` 2025-07-27 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
  - imprimé 2022 Soussousfamille : «Sophisme naturaliste»
  - imprimé v3 Soussousfamille : «Sophisme naturaliste»
- carte 105 (`1.2.2.3.1`) Soussousfamille = «Sophisme naturaliste» vs ancêtre «Appel à la nature»
  - bandeau : apparu («Sophisme naturaliste») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Sophisme naturaliste» → «Appel à la nature» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)

### Ancêtre `5.3.2` (PK 855) en fr — 1 carte(s)

- carte 869 (`5.3.2.3.2.1`) Soussousfamille = «Equivoque» vs ancêtre «Équivoque»
  - bandeau : apparu («Equivoque») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Equivoque» → «Équivoque» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
  - imprimé 2022 Soussousfamille : «Ambiguïté sémantique»
  - imprimé v3 Soussousfamille : «Ambiguïté sémantique»

### Ancêtre `7.2.1` (PK 1313) en fr — 2 carte(s)

- carte 1314 (`7.2.1.1`) Soussousfamille = «Evasion» vs ancêtre «Évasion»
  - bandeau : apparu («Evasion») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Evasion» → «Évasion» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)
- carte 1330 (`7.2.1.2.2`) Soussousfamille = «Evasion» vs ancêtre «Évasion»
  - bandeau : apparu («Evasion») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Evasion» → «Évasion» @ data(fallacies): FR clarity + cascade dr (`9d45b4f9` 2026-05-28 PR #369)

### Ancêtre `3.1.3` (PK 621) en pt — 2 carte(s)

- carte 622 (`3.1.3.1`) Subsubfamily_pt = «Transferência Ilícita» vs ancêtre «Transferência ilícita»
  - bandeau : apparu («Transferência Ilícita») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Transferência Ilícita» → «Transferência ilícita» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_pt : (vide/absent)
  - imprimé v3 Subsubfamily_pt : (vide/absent)
- carte 625 (`3.1.3.2`) Subsubfamily_pt = «Transferência Ilícita» vs ancêtre «Transferência ilícita»
  - bandeau : apparu («Transferência Ilícita») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Transferência Ilícita» → «Transferência ilícita» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_pt : (vide/absent)

### Ancêtre `3.2.1` (PK 633) en pt — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_pt = «Relacionamento Espúrio» vs ancêtre «Relação infundada»
  - bandeau : apparu («Relacionamento Espúrio») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relacionamento Espúrio» → «Relação infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_pt : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_pt = «Relacionamento Espúrio» vs ancêtre «Relação infundada»
  - bandeau : apparu («Relacionamento Espúrio») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Relacionamento Espúrio» → «Relação infundada» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_pt : (vide/absent)
  - imprimé v3 Subsubfamily_pt : (vide/absent)

### Ancêtre `2.3.2` (PK 420) en ru — 3 carte(s)

- carte 421 (`2.3.2.1`) Subsubfamily_ru = «Игра престолов» vs ancêtre «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
  - imprimé v3 Subsubfamily_ru : (vide/absent)
- carte 432 (`2.3.2.2.1`) Subsubfamily_ru = «Игра престолов» vs ancêtre «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
  - imprimé v3 Subsubfamily_ru : (vide/absent)
- carte 492 (`2.3.2.3.4`) Subsubfamily_ru = «Игра престолов» vs ancêtre «Игра власти»
  - bandeau : apparu («Игра престолов») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Игра престолов» → «Игра власти» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)

### Ancêtre `3.2.1` (PK 633) en ru — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_ru = «Безосновательное отношение» vs ancêtre «Необоснованная связь»
  - bandeau : apparu («Безосновательное отношение») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Безосновательное отношение» → «Необоснованная связь» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_ru : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_ru = «Безосновательное отношение» vs ancêtre «Необоснованная связь»
  - bandeau : apparu («Безосновательное отношение») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «Безосновательное отношение» → «Необоснованная связь» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_ru : (vide/absent)
  - imprimé v3 Subsubfamily_ru : (vide/absent)

### Ancêtre `7.3` (PK 1360) en ru — 7 carte(s)

- carte 1361 (`7.3.1`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `62b561e7` 2025-07-27 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1371 (`7.3.2`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_ru : (vide/absent)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1398 (`7.3.3`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_ru : (vide/absent)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1362 (`7.3.1.1`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_ru : (vide/absent)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1365 (`7.3.1.2`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subfamily_ru : (vide/absent)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1373 (`7.3.2.1.1`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_ru : (vide/absent)
- carte 1388 (`7.3.2.3.2`) Subfamily_ru = «К человеку» vs ancêtre «Ad hominem»
  - bandeau : apparu («К человеку») @ `e8482fe5` 2025-07-02 sans PR
  - titre   : «К человеку» → «Ad hominem» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subfamily_ru : (vide/absent)

### Ancêtre `2.3.1` (PK 357) en zh — 4 carte(s)

- carte 358 (`2.3.1.1`) Subsubfamily_zh = «条件反射» vs ancêtre «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
  - imprimé 2022 Subsubfamily_zh : (vide/absent)
  - imprimé v3 Subsubfamily_zh : (vide/absent)
- carte 376 (`2.3.1.1.4`) Subsubfamily_zh = «条件反射» vs ancêtre «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
- carte 361 (`2.3.1.1.1.2`) Subsubfamily_zh = «条件反射» vs ancêtre «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)
  - imprimé v3 Subsubfamily_zh : (vide/absent)
- carte 362 (`2.3.1.1.1.2.1`) Subsubfamily_zh = «条件反射» vs ancêtre «条件作用»
  - bandeau : apparu («条件反射») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «条件反射» → «条件作用» @ data(multilang): salvage deterministic c (`95db4425` 2026-05-30 PR #396)

### Ancêtre `3.2.1` (PK 633) en zh — 2 carte(s)

- carte 636 (`3.2.1.3`) Subsubfamily_zh = «虚假关系» vs ancêtre «无根据的关系»
  - bandeau : apparu («虚假关系») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «虚假关系» → «无根据的关系» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé v3 Subsubfamily_zh : (vide/absent)
- carte 638 (`3.2.1.4.1`) Subsubfamily_zh = «虚假关系» vs ancêtre «无根据的关系»
  - bandeau : apparu («虚假关系») @ `74557ea6` 2026-03-14 sans PR
  - titre   : «虚假关系» → «无根据的关系» @ feat(dataset-updater): Part B scaffoldin (`5e2477b5` 2026-05-31 PR #397)
  - imprimé 2022 Subsubfamily_zh : (vide/absent)
  - imprimé v3 Subsubfamily_zh : (vide/absent)

## Synthèse par ancêtre (toutes langues)

| Ancêtre | PK | Titre fr courant | Langues touchées | Nb écarts |
|---|---|---|---|---:|
| `7.3` | 1360 | Ad hominem | fa, ru | 14 |
| `3.2.1` | 633 | Relation infondée | ar, en, es, fa, pt, ru, zh | 14 |
| `1.2.2` | 96 | Appel à la nature | fr | 4 |
| `2.3.1` | 357 | Conditionnement | zh | 4 |
| `3.1.3` | 621 | Transfert illicite | fa, pt | 4 |
| `2.3.2` | 420 | Jeu de pouvoir | ru | 3 |
| `7.2.1` | 1313 | Évasion | fr | 2 |
| `5.3.2` | 855 | Équivoque | fr | 1 |

## Ce que cette mesure n'établit pas

- Elle ne décide rien : proposition règle C cas par cas reste à ai-01 (grain (c) = garde après décisions).
- Les comparaisons ne portent que sur les bandeaux NON VIDES des cartes deck et les ancêtres à titre non vide ;
  un bandeau vide n'est pas un écart (il n'imprime rien).
- L'historique cite le dernier mouvement de chaque côté dans la fenêtre du fichier (93 commits) ; un côté
  « jamais bougé » peut l'avoir été avant l'import repo (2025-07-02).
- Le join imprimé ne couvre que les ères archivées en CSV (2022, v3) et leurs colonnes de bandeau d'alors ;
  une ère sans la colonne du bandeau mesuré est omise de la ligne.
- Les 3 accents (« Equivoque » carte 869, « Evasion » cartes 1314/1330) sont corrigés par la PR séparée du
  grain 4b (règle typographique permanente) ; ils restent comptés ici (mesure sur `9085635b`).

## Annexe — les cellules « self » hors compte (bandeau de niveau k = profondeur de la carte)

21 cellules supplémentaires différeraient si l'on comparait aussi le niveau propre de la carte :
pour une carte de profondeur k, la colonne de niveau k n'imprime pas comme un bandeau d'ancêtre (c'est la
zone de titre de la carte) — elles ne comptent pas dans les 46. Listées pour transparence, même famille
de défauts (dont 2 accents fr : « Equivoque » sur la carte `5.3.2`, « Evasion » sur la carte `7.2.1`).

- carte 1360 (`7.3`) Subfamily_ru = «К человеку» vs propre titre «Ad hominem»
- carte 1360 (`7.3`) Subfamily_fa = «حمله به شخص» vs propre titre «حمله شخصی»
- carte 247 (`2.1.3`) Subsubfamily_fa = «الشعر» vs propre titre «شعر»
- carte 357 (`2.3.1`) Subsubfamily_zh = «条件反射» vs propre titre «条件作用»
- carte 420 (`2.3.2`) Subsubfamily_ru = «Игра престолов» vs propre titre «Игра власти»
- carte 621 (`3.1.3`) Subsubfamily_pt = «Transferência Ilícita» vs propre titre «Transferência ilícita»
- carte 621 (`3.1.3`) Subsubfamily_fa = «انتقال ناصحیح» vs propre titre «انتقال ناموجه»
- carte 633 (`3.2.1`) Subsubfamily = «Spurious relationship» vs propre titre «Unfounded relationship»
- carte 633 (`3.2.1`) Subsubfamily_ru = «Безосновательное отношение» vs propre titre «Необоснованная связь»
- carte 633 (`3.2.1`) Subsubfamily_pt = «Relacionamento Espúrio» vs propre titre «Relação infundada»
- carte 633 (`3.2.1`) Subsubfamily_es = «Relación espuria» vs propre titre «Relación infundada»
- carte 633 (`3.2.1`) Subsubfamily_ar = «علاقة وهمية» vs propre titre «علاقة غير مبررة»
- carte 633 (`3.2.1`) Subsubfamily_fa = «رابطه‌ی کاذب» vs propre titre «رابطهٔ بی‌اساس»
- carte 633 (`3.2.1`) Subsubfamily_zh = «虚假关系» vs propre titre «无根据的关系»
- carte 719 (`4.1.3`) Subsubfamily = «Causalation» vs propre titre «Stork effect»
- carte 855 (`5.3.2`) Soussousfamille = «Equivoque» vs propre titre «Équivoque»
- carte 1282 (`7.1.1`) Subsubfamily = «Relativism» vs propre titre «Abusive relativism»
- carte 1282 (`7.1.1`) Subsubfamily_ru = «Чрезмерная относительность» vs propre titre «Злоупотребление релятивизмом»
- carte 1282 (`7.1.1`) Subsubfamily_ar = «نسبية معيبة» vs propre titre «النسبية التعسفية»
- carte 1282 (`7.1.1`) Subsubfamily_fa = «نسبی‌گرایی افراطی» vs propre titre «نسبی‌گرایی سوءاستفاده‌آمیز»
- carte 1313 (`7.2.1`) Soussousfamille = «Evasion» vs propre titre «Évasion»

## Annexe — instrument rejouable

```python
# Rejouer : python g4a_bandeaux.py (depuis la racine du repo, arbre master 9085635b)
# Grain 4a du pool v19 : mesure 0-ecriture de l'invariant « bandeau imprime = titre de
# l'ancetre ». Pour chaque carte du deck (colonne carte non vide), chaque niveau k in {1,2,3},
# chaque langue : bandeau(k) contre text_<lang> de la rangee dont le path est le prefixe de
# longueur k. Chiffre de controle attendu (ai-01) : 46 ecarts sur 9085635b ; 42 une fois le
# grain 3 (retours regle C) applique. Pour chaque ecart : historique des deux cotes et join
# imprimé Archive/2022 + v3 par path.
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
    """-> (rows by path, list) ; chaque rangee = dict des colonnes utiles."""
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

def measure(rows, include_self=False):
    """-> liste d'ecarts (dict) pour les cartes du deck.
    Ancetres STRICTS par defaut (k < profondeur de la carte) : le bandeau de niveau k
    d'une carte de profondeur k est la zone de SON titre, pas un bandeau d'ancetre —
    21 cellules self existent (annexe), elles ne comptent pas dans le chiffre imprime."""
    deck = [(p, r) for p, r in rows.items() if r.get("carte")]
    misses = []
    for p, r in sorted(deck, key=lambda x: (len(x[0].split(".")), [int(i) for i in x[0].split(".")])):
        segs = p.split(".")
        for k in (1, 2, 3):
            if k > len(segs) or (k == len(segs) and not include_self):
                continue
            anc_path = ".".join(segs[:k])
            anc = rows.get(anc_path)
            for lang in LANGS:
                band_col = BAND_COLS[lang][k - 1]
                band = r.get(band_col, "")
                title = (anc or {}).get("text_" + lang, "")
                if not band or not title:
                    continue
                if band != title:
                    misses.append({
                        "card_pk": r.get("PK"), "card_path": p, "k": k, "lang": lang,
                        "band_col": band_col, "band": band, "anc_path": anc_path,
                        "anc_pk": (anc or {}).get("PK"), "title": title,
                    })
    return misses

misses = measure(cur)
self_cells = measure(cur, include_self=True)
by_lang = collections.Counter(m["lang"] for m in misses)
print("ecarts par langue:", dict(by_lang), "| total", len(misses))
EXPECTED = {"fr": 7, "en": 2, "ru": 12, "pt": 4, "es": 2, "ar": 2, "fa": 11, "zh": 6}
assert dict(by_lang) == EXPECTED and len(misses) == 46, f"attendu {EXPECTED} total 46"
n_self_only = len(self_cells) - len(misses)
print(f"cellules self (bandeau de niveau k = profondeur de la carte, hors compte): {n_self_only}")

# Simulation grain 3 (retours regle C appliques en memoire) -> 42 attendus
sim = {p: dict(r) for p, r in cur.items()}
for pathv, lang, new in [("2.1.1.1", "fr", "Argument par la question"), ("2.1.1.1", "en", "Argument by question"),
                          ("2.1.1.1", "ru", "Аргумент через вопрос"), ("1.2.2", "fr", "Sophisme naturaliste"),
                          ("4.1.2.6", "ru", "Софизм игрока")]:
    sim[pathv]["text_" + lang] = new
misses_sim = measure(sim)
by_lang_sim = collections.Counter(m["lang"] for m in misses_sim)
print("apres grain 3 (simule):", dict(by_lang_sim), "| total", len(misses_sim))
assert len(misses_sim) == 42, f"42 attendus, obtenu {len(misses_sim)}"

# --- Historique des deux cotes pour chaque ecart (dernier mouvement de chaque cote) ---
def last_change(getter):
    """-> (idx, old) dernier commit ou getter(snapshot) a change de valeur ; None si jamais."""
    prev, found = None, None
    for j, (_, _, _, rows) in enumerate(snaps):
        v = getter(rows)
        if v != prev:
            found = (j, prev)
            prev = v
    return found  # (index du commit du changement, valeur d'avant)

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

# --- Join imprime (archives par path) ---
def load_archives():
    eras = {}
    for era, paths in {
        "2022": ["Cards/Fallacies/Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022.csv",
                 "Cards/Fallacies/Archive/2022/Argumentum Fallacies - Cards - edition fevrier 2022 - Print and Play.csv"],
        "v3":   ["Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards.csv",
                 "Cards/Fallacies/Archive/v3/Argumentum Fallacies - Cards Print and Play.csv"],
    }.items():
        bypath = {}
        for p in paths:
            try:
                with open(p, encoding="utf-8-sig", errors="replace") as f:
                    rd = csv.DictReader(f)
                    bands = [c for c in (rd.fieldnames or []) if c and c.lower() in
                             ("famille", "sous-famille", "soussousfamille", "sous_sous_famille")]
                    for r in rd:
                        pv = (r.get("path") or "").strip()
                        if pv and pv not in bypath:
                            bypath[pv] = {b: (r.get(b) or "").strip() for b in bands} or (r.get("text_fr") or r.get("Titre") or "").strip()
            except FileNotFoundError:
                pass
        eras[era] = bypath
    return eras

archives = load_archives()

L = []
A = L.append
A("# Bandeaux imprimés ≠ titre de l'ancêtre — mesure 0-écriture (pool v19, grain 4a)")
A("")
A("Arbre master `9085635b` (2026-09-26) · dispatch [#458 c.5848171107](https://github.com/ArgumentumGames/Argumentum/issues/458#issuecomment-5848171107).")
A("")
A("Instrument : pour chaque carte du deck (colonne `carte` non vide, 175 cartes), chaque niveau")
A("k ∈ {1, 2, 3} (`Famille`/`Sous-Famille`/`Soussousfamille` et équivalents par langue), chaque langue :")
A("le bandeau de la carte contre `text_<lang>` de la rangée dont le `path` est le préfixe de longueur k.")
A("")
A(f"**Chiffre de contrôle : {len(misses)} écarts** (fr {by_lang['fr']}, en {by_lang['en']}, ru {by_lang['ru']},"
  f" pt {by_lang['pt']}, es {by_lang['es']}, ar {by_lang['ar']}, fa {by_lang['fa']}, zh {by_lang['zh']}) —")
A("conforme à la mesure ai-01 du 26/09. **Après le grain 3 (retours règle C, simulé en mémoire) :")
A(f"{len(misses_sim)}** (fr {by_lang_sim.get('fr', 0)}).")
A("")
A("Trois motifs ressortent : (1) **28 des 46 écarts tiennent à deux ancêtres renommés après coup** — `3.2.1`")
A("(renommé par #397 `5e2477b5` dans 7 langues, les bandeaux des cartes 636/638 n'ont pas suivi) et `7.3`")
A("« Ad hominem » (14 écarts fa/ru) ; (2) **les 3 accents fr sont des titres accentués par #369 (`9d45b4f9`)")
A("sans que les bandeaux suivent** — le côté à corriger est le bandeau (grain 4b, règle typographique) ;")
A("(3) l'imprimé 2022/v3 de la famille `1.2.2` portait déjà « Sophisme naturaliste » comme bandeau —")
A("corroboration supplémentaire du retour règle C de la 96 (grain 3).")
A("")
A("## Les écarts, groupe par groupe")
A("")
groups = collections.defaultdict(list)
for d in details:
    m = d[0]
    key = (m["anc_path"], m["lang"])
    groups[key].append(d)
for (anc_path, lang), items in sorted(groups.items(), key=lambda kv: (kv[0][1], kv[0][0])):
    m0 = items[0][0]
    A(f"### Ancêtre `{anc_path}` (PK {m0['anc_pk']}) en {lang} — {len(items)} carte(s)")
    A("")
    for m, band_hist, title_hist in items:
        A(f"- carte {m['card_pk']} (`{m['card_path']}`) {m['band_col']} = «{m['band']}» vs ancêtre «{m['title']}»")
        A(f"  - bandeau : {band_hist}")
        A(f"  - titre   : {title_hist}")
        for era in ("2022", "v3"):
            ref = archives[era].get(m["card_path"])
            if isinstance(ref, dict):
                bandv = ref.get(m["band_col"], "")
                A(f"  - imprimé {era} {m['band_col']} : {('«' + bandv + '»') if bandv else '(vide/absent)'}")
    A("")
A("## Synthèse par ancêtre (toutes langues)")
A("")
A("| Ancêtre | PK | Titre fr courant | Langues touchées | Nb écarts |")
A("|---|---|---|---|---:|")
by_anc = collections.defaultdict(lambda: collections.Counter())
anc_langs = collections.defaultdict(set)
for d in details:
    m = d[0]
    by_anc[m["anc_path"]]["n"] += 1
    anc_langs[m["anc_path"]].add(m["lang"])
for anc_path, c in sorted(by_anc.items(), key=lambda kv: -kv[1]["n"]):
    row = cur.get(anc_path, {})
    A(f"| `{anc_path}` | {row.get('PK', '?')} | {row.get('text_fr', '?')} | {', '.join(sorted(anc_langs[anc_path]))} | {c['n']} |")
A("")
A("## Ce que cette mesure n'établit pas")
A("")
A("- Elle ne décide rien : proposition règle C cas par cas reste à ai-01 (grain (c) = garde après décisions).")
A("- Les comparaisons ne portent que sur les bandeaux NON VIDES des cartes deck et les ancêtres à titre non vide ;")
A("  un bandeau vide n'est pas un écart (il n'imprime rien).")
A("- L'historique cite le dernier mouvement de chaque côté dans la fenêtre du fichier (93 commits) ; un côté")
A("  « jamais bougé » peut l'avoir été avant l'import repo (2025-07-02).")
A("- Le join imprimé ne couvre que les ères archivées en CSV (2022, v3) et leurs colonnes de bandeau d'alors ;")
A("  une ère sans la colonne du bandeau mesuré est omise de la ligne.")
A("- Les 3 accents (« Equivoque » carte 869, « Evasion » cartes 1314/1330) sont corrigés par la PR séparée du")
A("  grain 4b (règle typographique permanente) ; ils restent comptés ici (mesure sur `9085635b`).")
A("")
A("## Annexe — les cellules « self » hors compte (bandeau de niveau k = profondeur de la carte)")
A("")
A(f"{n_self_only} cellules supplémentaires différeraient si l'on comparait aussi le niveau propre de la carte :")
A("pour une carte de profondeur k, la colonne de niveau k n'imprime pas comme un bandeau d'ancêtre (c'est la")
A("zone de titre de la carte) — elles ne comptent pas dans les 46. Listées pour transparence, même famille")
A("de défauts (dont 2 accents fr : « Equivoque » sur la carte `5.3.2`, « Evasion » sur la carte `7.2.1`).")
A("")
self_keys = {(m["card_pk"], m["k"], m["lang"]) for m in misses}
for m in self_cells:
    if (m["card_pk"], m["k"], m["lang"]) in self_keys:
        continue
    A(f"- carte {m['card_pk']} (`{m['card_path']}`) {m['band_col']} = «{m['band']}» vs propre titre «{m['title']}»")
A("")
A("## Annexe — instrument rejouable")
A("")
A("``\`python")
A(open(__file__, encoding="utf-8").read().replace("``\`", "``\\`"))
A("``\`")
A("")
A(f"*po-2024 — mesure, 0 écriture sur le corpus · grain 4a · {len(misses)} écarts mesurés, {len(misses_sim)} simulés*")

with open("docs/corpus/458-v19-g4-bandeaux-invariant-2026-09-26.md", "w", encoding="utf-8", newline="\n") as f:
    f.write("\n".join(L) + "\n")
print(f"dossier: {len(L)} lignes ; ancêtres distincts: {len(by_anc)}")

```

*po-2024 — mesure, 0 écriture sur le corpus · grain 4a · 46 écarts mesurés, 42 simulés*
