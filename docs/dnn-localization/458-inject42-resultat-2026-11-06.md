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

Les 42 lignes sont des **insertions pures** : les supprimer restitue l'état initial (0 dim sur les 6 cibles). À exécuter en transaction, après vérification du compte :

```sql
BEGIN TRAN r;
-- garde : exactement 42 liens sur les ValueIds écrits
SELECT COUNT(*) FROM TsDynDataValueDimension WHERE ValueId BETWEEN 30525 AND 30566;  -- attendu 42
DELETE FROM TsDynDataValueDimension WHERE ValueId BETWEEN 30525 AND 30566;
DELETE FROM TsDynDataValue WHERE ValueId BETWEEN 30525 AND 30566;
-- garde post : 0 dim résiduelle sur les 6 entités/attributs
SELECT COUNT(*) FROM TsDynDataValueDimension d
  WHERE d.ValueId IN (SELECT ValueId FROM TsDynDataValue
    WHERE EntityId IN (9416,9414,9415,9417,9413,8818) AND AttributeId IN (1755,1545));  -- attendu 0
-- COMMIT r;  -- après contrôle
```

Puis recycle app-domain (mtime web.config), et rejouer la preuve de §4 : attendu retour à 42 `registre_avance`. Les bases dim-0 (ValueIds 22477, 22474, 22478, 22476, 22475, 22064) ne sont **jamais** dans le périmètre du rollback.

## 6. Comment vérifier que ce constat est mort

```powershell
python tools/458-eav-content-export.py --config DNNPlatform\web.config verify --csv docs/dnn-localization/eav-content.csv --classify
```

Attendu : 631/631, **0 écart**. Si 42 `registre_avance` réapparaissent : un rollback ou un restore a eu lieu — §5 en trace la forme.

*po-2023*
