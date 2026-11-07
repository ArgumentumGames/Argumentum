# #458 — injection ⑥ : 42 cellules EAV écrites, preuve tierce 0 écart

**Date :** 2026-11-06 11:23-11:31 (Paris) · **Lane :** po-2023 · **Contexte :** dispatch c.6672655541 item ⑥, registre #2164, méthode E1 (`458-contact-newsletter-injection-2026-11-04.md`).

## 1. Ce qui a été écrit

Les **42 cellules** du lot ⑥ : 5 `ViewConfiguration.Title` (app 52, vues **non câblées** — invisibles des visiteurs, mesuré §8 de `458-eav-70-cellules-2026-11-05.md`) + 1 `Slider.Title` « 50vh » (app 49), chacune × 7 dimensions (en=7, ru=8, pt=9, es=10, ar=11, fa=12, zh=13).

**Les bases dim-0 ne sont pas touchées** : la colonne `fr` du registre = SOURCE (valeur née anglaise pour 5 des 6 titres), elle reste telle quelle. L'injection corrige le **sélecteur de vues de l'UI d'édition 2sxc**, pas le rendu public.

Script : `.tmp/inject-42-cells.ps1` (scratch de lane, modes Verify/Rehearse/Apply) — payload, gardes et recette de §4 le rejouent à l'identique.

## 2. Gardes et répétition

| Contrôle | Résultat |
|---|---|
| Miroir base dim-0 = registre `fr` (`-ceq`, byte-faithful) | 6/6 |
| Anti-rejeu : 0 rangée dimensionnée pré-existante sur les 6 cibles | ✅ |
| ValueId annoncé ∈ EntityId × AttributeId | 6/6 |
| Rehearse (tx + ROLLBACK vérifié) | 42/42 byte-identiques, état initial restitué |
| Contrôle inverse du comparateur (attente mutée ≠) | ✅ |

## 3. Mesures de l'Apply (11:23)

- Transaction unique, UPDLOCK/HOLDLOCK sur les 6 valeurs cibles, 42 `INSERT` + 42 liens `TsDynDataValueDimension`.
- Appariement par contenu **avec consommation** (les 7 valeurs identiques « 50vh » reçoivent chacune leur lien, pas 7 liens sur la première).
- Relecture in-tx : **42/42 byte-identiques, 0 diff**, dim-0 intactes, contrôle inverse du comparateur OK.
- Seed `IDENT_CURRENT('TsDynDataValue')` : 30524 → 30566 (**Δ=42 exact**).
- Backup pré-geste : 25 rangées, `G:\Mon Drive\MyIA\Argumentum\backups-eav-2026-11-06\42cells-backup-20261106-112339.json`.
- Recycle app-domain : `mtime` web.config touché, **sha256 inchangé** (B59E9AFB…), jamais iisreset.

**Identifiants écrits** (relecture post-COMMIT, 42/42 liens présents) :

| Cible | EntityId | Dims 7→13 : ValueIds |
|---|---:|---|
| Frontpage with images | 9416 | 30525 – 30531 |
| Column without images | 9414 | 30532 – 30538 |
| News | 9415 | 30539 – 30545 |
| Archive news list | 9417 | 30546 – 30552 |
| By category filtered | 9413 | 30553 – 30559 |
| Slider 50vh | 8818 | 30560 – 30566 |

Contiguous 30525-30566 = 42, conforme au seed.

## 4. Preuve tierce (avant/après, même instrument)

```
python tools/458-eav-content-export.py --config DNNPlatform\web.config verify --csv <registre> --classify
```

| Passe | Registre | Écarts | registre_avance | base_avance | conflit |
|---|---|---:|---:|---:|---:|
| Avant (09:2x) | `origin/master` @ `3b611203` | 42 | 42 | 0 | 0 |
| Après (11:25) | idem | **0** | 0 | 0 | 0 |

Verdict après : « réimport à blanc = 0 valeur changée ». Les 42 écarts d'avant = exactement le payload ; rien d'autre n'a bougé (0 base_avance, 0 conflit dans les deux passes).

⚠️ **Piège d'instrument, noté** : une première passe contre le registre du **webroot `284b49b8`** (12 commits de retard) rendait « 0 écart » **avant** l'injection. Cause : le registre y porte ces 42 cellules **vides**, la base aussi → **vide-contre-vide**, pas une traduction. Toute preuve « avant/après » se mesure contre le **même registre**, et ce registre est celui qui **porte les valeurs**. Le registre est **large** : 631 enregistrements (1 clé + colonnes par langue), 4 867 lignes physiques (champs multilignes) — le compte de lignes du rapport = enregistrements, pas `wc -l`.

**Site après recycle** : `/` 000 (60 s, cold-start recompilation) → rejoué après stabilisation : `/` **200** (0,25 s), `/Actus` **200** (0,14 s), `/en/Actus` **200** (14,4 s, premier hit). Aucun écart visiteur attendu : les 5 vues sont non câblées (mesuré, §8 du doc 70-cellules).

**Webroot** : avance rapide `284b49b8` → `3b611203` dans la même fenêtre (24 fichiers, docs/ + tools/ uniquement, 0 resx, 0 dll, 0 DNNPlatform — rien de servi ; les 72 entrées sales épinglées ne chevauchent pas le diff).

## 5. Rollback (si jamais nécessaire)

Les 42 lignes sont des **insertions pures** : les supprimer restitue l'état initial (0 dim sur les 6 cibles).

⚠️ **Un compte ne vaut pas une identité** (remarque 🟡 de la review #2171). `COUNT(*) = 42` sur une plage de `ValueId` ne prouve **pas** que ces 42 lignes sont encore celles de l'injection : après une restauration ou une réaffectation d'identités, une autre plage de 42 valeurs satisferait le compte, et le rollback supprimerait du contenu étranger. La garde ci-dessous **apparie chaque `ValueId`** au couple `EntityId`/`AttributeId` attendu, à sa dimension (7–13) et à sa valeur ; **un seul écart ⇒ abandon**, aucune suppression. La suppression elle-même est bornée à `#expected` (identité **et** valeur), plus jamais à une plage.

```sql
BEGIN TRAN r;

-- ── Garde bloquante : les 42 tuples attendus, explicites (aucune inférence de plage) ──
CREATE TABLE #expected (ValueId int PRIMARY KEY, EntityId int, AttributeId int, DimensionId int, Val nvarchar(200));
INSERT INTO #expected (ValueId, EntityId, AttributeId, DimensionId, Val) VALUES
 (30525,9416,1755, 7,N'Frontpage with images'), (30526,9416,1755, 8,N'Главная страница с изображениями'),
 (30527,9416,1755, 9,N'Página inicial com imagens'), (30528,9416,1755,10,N'Página de inicio con imágenes'),
 (30529,9416,1755,11,N'الصفحة الرئيسية مع الصور'), (30530,9416,1755,12,N'صفحه اصلی با تصاویر'),
 (30531,9416,1755,13,N'带图片的首页'),
 (30532,9414,1755, 7,N'Column without images'), (30533,9414,1755, 8,N'Колонка без изображений'),
 (30534,9414,1755, 9,N'Coluna sem imagens'), (30535,9414,1755,10,N'Columna sin imágenes'),
 (30536,9414,1755,11,N'عمود بدون صور'), (30537,9414,1755,12,N'ستون بدون تصویر'),
 (30538,9414,1755,13,N'不带图片的栏目'),
 (30539,9415,1755, 7,N'News'), (30540,9415,1755, 8,N'Новости'), (30541,9415,1755, 9,N'Notícias'),
 (30542,9415,1755,10,N'Noticias'), (30543,9415,1755,11,N'الأخبار'), (30544,9415,1755,12,N'اخبار'),
 (30545,9415,1755,13,N'新闻'),
 (30546,9417,1755, 7,N'Archive news list'), (30547,9417,1755, 8,N'Список архивных новостей'),
 (30548,9417,1755, 9,N'Lista de notícias do arquivo'), (30549,9417,1755,10,N'Lista de noticias del archivo'),
 (30550,9417,1755,11,N'قائمة أخبار الأرشيف'), (30551,9417,1755,12,N'فهرست اخبار آرشیو'),
 (30552,9417,1755,13,N'新闻存档列表'),
 (30553,9413,1755, 7,N'News list filtered by category'),
 (30554,9413,1755, 8,N'Список новостей, отфильтрованный по категории'),
 (30555,9413,1755, 9,N'Lista de notícias filtrada por categoria'),
 (30556,9413,1755,10,N'Lista de noticias filtrada por categoría'),
 (30557,9413,1755,11,N'قائمة الأخبار مفلترة حسب الفئة'),
 (30558,9413,1755,12,N'فهرست اخبار فیلترشده بر اساس دسته‌بندی'),
 (30559,9413,1755,13,N'按类别筛选的新闻列表'),
 (30560,8818,1545, 7,N'50vh'), (30561,8818,1545, 8,N'50vh'), (30562,8818,1545, 9,N'50vh'),
 (30563,8818,1545,10,N'50vh'), (30564,8818,1545,11,N'50vh'), (30565,8818,1545,12,N'50vh'),
 (30566,8818,1545,13,N'50vh');

-- Quatre agrégats ; attendus A=0, B=42, C=0, D=6. Un A>0, B<>42, C>0 ou D≠6 ⇒ abandon.
-- (D est borné à [0,6] par la clé de TsDynDataValue : D≠6 ≡ D<6.)
SELECT
 (SELECT COUNT(*) FROM #expected e
    LEFT JOIN TsDynDataValue v ON v.ValueId=e.ValueId AND v.EntityId=e.EntityId
                              AND v.AttributeId=e.AttributeId AND v.Value=e.Val
    LEFT JOIN TsDynDataValueDimension d ON d.ValueId=e.ValueId AND d.DimensionId=e.DimensionId
   WHERE v.ValueId IS NULL OR d.ValueId IS NULL)                      AS A_attendus_absents_ou_derives,
 (SELECT COUNT(*) FROM TsDynDataValueDimension
   WHERE ValueId BETWEEN 30525 AND 30566)                             AS B_liens_dans_la_plage,
 (SELECT COUNT(*) FROM TsDynDataValue v
   WHERE v.EntityId IN (9416,9414,9415,9417,9413,8818) AND v.AttributeId IN (1755,1545)
     AND v.ValueId NOT IN (SELECT ValueId FROM #expected)
     AND v.ValueId NOT IN (22477,22474,22478,22476,22475,22064))      AS C_valeurs_etrangeres,
 (SELECT COUNT(*) FROM (VALUES (22477,N'Frontpage with images'),(22474,N'Column without images'),
   (22475,N'By category filtered news list'),(22478,N'News'),(22476,N'Archive news list'),(22064,N'50vh'))
   b(ValueId,Val) JOIN TsDynDataValue v ON v.ValueId=b.ValueId AND v.Value=b.Val) AS D_bases_conformes_backup;

-- Abandon : si l'un des quatre n'est pas celui attendu, on annule et on ne supprime RIEN.
-- ⚠️ Le IF porte les QUATRE agrégats : A, B, C et D. (Correction review #2184 — le premier
--    jet ne testait que A, B, C : une base dim-0 dérivée (D=5) laissait passer la suppression.)
IF (SELECT COUNT(*) FROM #expected e
      LEFT JOIN TsDynDataValue v ON v.ValueId=e.ValueId AND v.EntityId=e.EntityId
                                AND v.AttributeId=e.AttributeId AND v.Value=e.Val
      LEFT JOIN TsDynDataValueDimension d ON d.ValueId=e.ValueId AND d.DimensionId=e.DimensionId
     WHERE v.ValueId IS NULL OR d.ValueId IS NULL) <> 0
   OR (SELECT COUNT(*) FROM TsDynDataValueDimension WHERE ValueId BETWEEN 30525 AND 30566) <> 42
   OR EXISTS (SELECT 1 FROM TsDynDataValue v
        WHERE v.EntityId IN (9416,9414,9415,9417,9413,8818) AND v.AttributeId IN (1755,1545)
          AND v.ValueId NOT IN (SELECT ValueId FROM #expected)
          AND v.ValueId NOT IN (22477,22474,22478,22476,22475,22064))
   OR (SELECT COUNT(*) FROM (VALUES (22477,N'Frontpage with images'),(22474,N'Column without images'),
       (22475,N'By category filtered news list'),(22478,N'News'),(22476,N'Archive news list'),(22064,N'50vh'))
       b(ValueId,Val) JOIN TsDynDataValue v ON v.ValueId=b.ValueId AND v.Value=b.Val) <> 6
BEGIN
    ROLLBACK TRAN r;
    THROW 50000, 'ROLLBACK ANNULE : la garde d''identite ne reconnait plus l''injection (drift). Aucune suppression.', 1;
END

-- ── Suppression bornée à #expected (identité ET valeur), jamais à une plage ──
-- ⚠️ Colonne de #expected = Val, colonne de TsDynDataValue = Value. Le premier jet écrivait
--    `e.Value=v.Val` (colonnes croisées) : erreur 207 après le premier DELETE, transaction
--    ouverte. (Correction review #2184.)
DELETE d FROM TsDynDataValueDimension d
  JOIN #expected e ON e.ValueId=d.ValueId AND e.DimensionId=d.DimensionId;
DELETE v FROM TsDynDataValue v
  JOIN #expected e ON e.ValueId=v.ValueId AND e.EntityId=v.EntityId
                  AND e.AttributeId=v.AttributeId AND e.Val=v.Value;

-- ── Contrôles post-suppression (ne remplacent pas la garde préalable) ──
SELECT (SELECT COUNT(*) FROM TsDynDataValueDimension
         WHERE ValueId IN (SELECT ValueId FROM TsDynDataValue
           WHERE EntityId IN (9416,9414,9415,9417,9413,8818) AND AttributeId IN (1755,1545))) AS dim_residuelles, -- attendu 0
       (SELECT COUNT(*) FROM TsDynDataValue
         WHERE ValueId IN (SELECT ValueId FROM #expected))                                       AS valeurs_residuelles; -- attendu 0

DROP TABLE #expected;
-- COMMIT TRAN r;  -- seulement après les contrôles ci-dessus
```

**Validation de la garde (06/11 16:4x, lecture seule)** — la requête a été exécutée sur la base vivante avec `#expected` en table **temporaire de session** : zéro écriture persistante, aucun `DELETE`, rollback non exécuté (⛔ ai-01). Mesures :

| Passe | A | B | C | D | Verdict |
|---|---:|---:|---:|---:|---|
| Payload réel | 0 | 42 | 0 | 6 | **GARDE PASSE** |
| Contrôle inverse : une valeur mutée (`Новости` → `Новости-MUTE`) | **1** | 42 | 0 | 6 | **BLOQUE** |
| Contrôle inverse : une dimension mutée (30560 : 7 → 8) | **1** | 42 | 0 | 6 | **BLOQUE** |
| Contrôle inverse (correction #2184) : une base dim-0 mutée (22064 : `50vh` → `50vh-MUTE`) | 0 | 42 | 0 | **5** | **BLOQUE** *(désormais : le `IF` teste D)* |

Les trois mutations donnent **BLOQUE** par des agrégats **différents** (A, A, D) : la garde ne tient pas sur un seul capteur. La mutation n'est pas une simulation : c'est le même SQL, seule l'attente change.

**Preuve en lecture seule des jointures de suppression (demandée par la review #2184)** — chacun des deux `DELETE` est rejoué en `SELECT COUNT(*)` sur **exactement les mêmes jointures** : le comptage lie les colonnes comme la suppression, sans rien écrire. Attendu **42 et 42** :

```sql
-- À exécuter en lecture seule, fenêtre de jour (10 h – 17 h), annoncé au dashboard.
SELECT COUNT(*) AS delete_d_toucherait
  FROM TsDynDataValueDimension d
  JOIN #expected e ON e.ValueId=d.ValueId AND d.DimensionId=e.DimensionId;        -- attendu 42

SELECT COUNT(*) AS delete_v_toucherait
  FROM TsDynDataValue v
  JOIN #expected e ON e.ValueId=v.ValueId AND e.EntityId=v.EntityId
                  AND e.AttributeId=v.AttributeId AND e.Val=v.Value;              -- attendu 42
```

✅ **Exécutés le 07/11 11:2x (lecture seule, fenêtre de jour, annoncés)** — `#expected` reconstruite en table **temporaire de session** depuis les lignes 74-100 ci-dessus (aucune écriture persistante, aucun `DELETE`, aucun rollback exécuté) :

| Mesure | Attendu | Obtenu |
|---|---:|---:|
| `delete_d_toucherait` (jointure dimension, identité complète) | 42 | **42** |
| `delete_v_toucherait` (identité **et** valeur) | 42 | **42** |
| Garde : A / B / C / D | 0 / 42 / 0 / 6 | **0 / 42 / 0 / 6** |

Chaque `DELETE` toucherait exactement les 42 lignes de l'injection : ni plus (aucune valeur étrangère dans le rayon, C=0), ni moins (aucun drift, A=0).

⚠️ Le bloc de preuve ci-dessus portait lui-même une coquille au moment de l'exécution — `e.DimensionId=e.DimensionId` (tautologie : la jointure se réduisait au seul `ValueId`, preuve affaiblie) — corrigée en `d.DimensionId=e.DimensionId` avant de lancer la mesure. Même famille que le `e.Value=v.Val` de la review : les colonnes croisées se glissent dans les requêtes de contrôle comme dans celles d'action. La ligne mesurée est la ligne corrigée.

Puis recycle app-domain (mtime web.config), et rejouer la preuve de §4 : attendu retour à 42 `registre_avance`. Les bases dim-0 (ValueIds 22477, 22474, 22478, 22476, 22475, 22064) ne sont **jamais** dans le périmètre du rollback — la colonne `D` de la garde les vérifie contre le backup avant tout geste.

## 6. Comment vérifier que ce constat est mort

```powershell
python tools/458-eav-content-export.py --config DNNPlatform\web.config verify --csv docs/dnn-localization/eav-content.csv --classify
```

Attendu : 631/631, **0 écart**. Si 42 `registre_avance` réapparaissent : un rollback ou un restore a eu lieu — §5 en trace la forme.

*po-2023*
