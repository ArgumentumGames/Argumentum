# Injection du jeu RuleMemo harmonisé : 22 cellules UPDATE, préprod

**Date :** 2026-10-26 · **Lane :** po-2023 · **Pool :** #458 — item « injection RuleMemo », gate = merge #2011 (`7e53347b`) · **Base :** `origin/master` `e718870b`
**Portée :** préprod (`dnn.argumentum.myia.io`) uniquement. Prod non touchée.
Suite directe de [`458-rulememo-injection-2026-10-25.md`](458-rulememo-injection-2026-10-25.md) (le préfixe de nom de fichier, addition pure).

---

## 1. Ce que le geste aligne

#2011 a harmonisé **un terme mémo par langue** dans le canon (`dnn-ui-strings.csv`) et le pivot
(`eav-content.csv`, rangées `DEA9410D-274B-4A4E-B47C-7D5D00692348`, app **60**, type `App-Resources`).
La base préprod portait encore **l'ancien jeu machine** sur 22 cellules. Mesuré avant tout geste par
`tools/458-eav-content-export.py verify --classify` sur master `e718870b` : **51 conflits**, dont
**22 RuleMemo** (les 29 autres = 27 Game Rule « Reader/drawer », lot #2010 ouvert, + 2 app 33 `Text`).

`RuleMemoCardFileNamePrefix` n'est **pas touché** : déjà aligné par le geste du 25/10, et son
descripteur est épinglé « Règle »/« Rule » (#772).

## 2. Les 22 cellules écrites (UPDATE purs, aucune INSERT, aucune suppression)

| Champ | Langues | Exemples ancien → nouveau |
| --- | --- | --- |
| `RuleMemoCard` (h2) | fr, ru, pt, es, ar, fa, zh | `Carte Mémo`→`Carte mémo` · `Карта Мемо`→`Карточка-памятка` · `Carta de ajuda`→`Cartão-lembrete` · `Carta recordatorio`→`Tarjeta memo` · `提示卡`→`备忘卡` |
| `RuleMemoCardDownload` (bouton) | en, ru, pt, es, ar, fa, zh | `Download the memo card`→`Download memo card` · `Baixar a carta de ajuda`→`Baixar cartão-lembrete` |
| `RuleMemoInstructions` (Html.Raw) | les 8 | fr `carte de mémo`→`carte mémo` · les 7 cibles re-dérivées du canon, ré-emballées `<p>…&nbsp;</p>`, entités fr préservées |

Deux cellules de l'inventaire étaient déjà conformes et restent **inchangées** : `RuleMemoCard` [en]
(`Memo card`) et `RuleMemoCardDownload` [fr] (`Télécharger la carte mémo`) — l'inventaire complet
(24 rangées = 3 champs × base + dims 7-13) est sauvegardé avant écriture, valeurs incluses
(scratchpad de session, jamais committé). Dimensions : `fr`=base, `7`=en, `8`=ru, `9`=pt, `10`=es,
`11`=ar, `12`=fa, `13`=zh.

## 3. Gardes et transaction

Une transaction tout-ou-rien, chaque UPDATE précédé d'un `SELECT … WITH (UPDLOCK, HOLDLOCK)` qui
doit rendre **exactement** l'ancienne valeur attendue (comparaison binaire) sinon `ROLLBACK`
complet ; l'UPDATE porte lui-même `AND Value = @old` (double garde, `rows affected = 1` asserté) ;
relecture des 22 valeurs **dans** la transaction, octet pour octet ; relecture indépendante après
commit. Le plan est dérivé du CSV classé du `verify` (la valeur cible vient du pivot mergé, pas
d'une copie manuelle). Résultat : `COMMIT 22 updates, in-txn re-read 22/22 byte-identical`.

Recycle app-domain après commit (mtime `web.config`, **sha256 asserté inchangé**) — première requête
> 90 s = fenêtre de recycle, 200 revenu au poll suivant. Jamais `iisreset`.

## 4. Preuve tierce : l'écart registre ↔ base tombe d'exactement 22

| Mesure (`verify --classify`) | Avant | Après |
| --- | ---: | ---: |
| Conflits totaux | 51 | **29** |
| dont RuleMemo | 22 | **0** |
| dont Game Rule (`Content`/`Installation`/`Variants`) | 27 | 27 (lot #2010, hors périmètre) |
| dont app 33 `Text` | 2 | 2 (arbitrage ai-01) |

Un instrument qui ne connaît pas le script d'injection voit disparaître **précisément** les 22
cellules écrites — même logique de preuve que le geste du 25/10 (129 → 125).

## 5. Non-régression servie, après recycle (8 cultures, détail de règle `/Règles/details/l-ecole-des-menteurs/mid/602`)

| Culture | Racine | Détail | `Error Showing Content` | Nouvelles sondes | Ancien jeu |
| --- | --- | --- | --- | --- | --- |
| `/` | 200 | 200 | 0 | 3/3 (`Carte mémo`, `carte mémo suivante`, bouton) | absent |
| `/en` | 200 | 200 | 0 | 2/2 (`Download memo card`, `refer back to the rules easily`) | absent |
| `/es` | 200 | 200 | 0 | 3/3 (`Tarjeta memo` ×2, bouton) | absent |
| `/pt` | 200 | 200 | 0 | 3/3 (`Cartão-lembrete` ×2, bouton) | absent |
| `/ru` | 200 | 200 | 0 | 2/2 | absent |
| `/zh` | 200 | 200 | 0 | 2/2 (`备忘卡` h2 + instructions ; bloc extrait) | absent **du bloc mémo** |
| `/ar` | 200 | 200 | 0 | 2/2 | absent |
| `/fa` | 200 | 200 | 0 | 2/2 | absence non testable (voir §6) |

### 6. Deux pièges d'absence, refermés et nommés

1. **`fa` — l'ancien est préfixe du nouveau** : `کارت یادآور` (ancien) + `ی` = `کارت یادآوری`
   (nouveau). Une sonde « ancien absent de la page » échouerait **toujours**, même injection
   parfaite. L'absence n'y est pas testable ; la présence du nouveau + la relecture base la
   prouvent.
2. **`zh` — le mot ancien vit ailleurs** : `提示卡` apparaît 2× sur la page de détail **hors du bloc
   mémo**, dans la prose de la règle elle-même (« 提示卡对这一分类作了摘要 », liste de matériel) —
   cellules `Game Rule`, lot #2010, légitimement non touchées ici. Une absence **page entière**
   mélange les deux magasins ; l'absence se mesure **dans le bloc qui porte la chaîne** (du `<h2>`
   au bouton). C'est la même famille que « une égalité de texte visible ne distingue pas deux
   magasins » (#2014).

## 7. Comment vérifier que ce constat est mort

```sql
SELECT CASE WHEN d.DimensionId IS NULL THEN 0 ELSE d.DimensionId END AS Dim, CAST(v.Value AS NVARCHAR(200))
FROM TsDynDataValue v
JOIN TsDynDataEntity e ON e.EntityId = v.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId
LEFT JOIN TsDynDataValueDimension d ON d.ValueId = v.ValueId
WHERE CONVERT(NVARCHAR(64), e.EntityGuid) = 'DEA9410D-274B-4A4E-B47C-7D5D00692348'
  AND a.StaticName = 'RuleMemoCard'
ORDER BY 1;
-- dim 0 doit rendre 'Carte mémo' (bas de casse), dim 9 'Cartão-lembrete', dim 10 'Tarjeta memo'.
```

Puis `python tools/458-eav-content-export.py verify --classify` : l'écart doit être **29** et la
liste ne doit contenir **aucune** rangée `RuleMemo*`. Une réapparition = restauration de base ou
réécriture concurrente — signal nominatif.

Rollback : réécrire les 24 valeurs d'origine depuis la sauvegarde scratchpad du 26/10 12:10Z
(`rulememo-backup-20261026T1210.json`, jamais committée), même mécanisme transactionnel.
