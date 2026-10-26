# Injection du préfixe de nom de carte Règle (`RuleMemoCardFileNamePrefix`) : 4 cellules, préprod

**Date :** 2026-10-25 · **Lane :** po-2023 · **Pool :** #458 item 3, traduction portée par #1979 · **Base :** `origin/master` `dc42437c`
**Portée :** préprod (`dnn.argumentum.myia.io`) uniquement. Prod non touchée.

---

## 1. Ce que l'item demandait, et ce qu'il restait réellement

L'item s'énonce « injection des 4 cellules `RuleMemoCardFileNamePrefix` (ru/ar/fa/zh) ». Comme pour
les apps 35, 49 et 59 ([`458-app49-injection-2026-10-25.md`](458-app49-injection-2026-10-25.md)),
la traduction existait déjà dans le registre relu `docs/dnn-localization/eav-content.csv` — c'est
**l'écriture en base qui manquait**. Mesuré avant tout geste, hors outil (lecture directe de
`TsDynDataValue` + `TsDynDataValueDimension`) :

| Mesure | Avant | Après |
| --- | --- | --- |
| Rangées de valeur sur `(entité, attribut)` | **4** | **8** |
| Dimensions portées | `7` en, `9` pt, `10` es | + `8` ru, `11` ar, `12` fa, `13` zh |
| Base (0 rangée de dimension) | 1 — `Argumentum - Règle` | 1, inchangée |

Entité `DEA9410D-274B-4A4E-B47C-7D5D00692348`, app **60**, type `App-Resources`, champ
`RuleMemoCardFileNamePrefix`. Dimensions 2sxc : `7=en-US`, `8=ru-RU`, `9=pt-BR`, `10=es-ES`,
`11=ar-SA`, `12=fa-IR`, `13=zh-CN`.

## 2. Deux sources en accord, exigé avant d'écrire

La valeur à écrire ne vient pas d'une seule copie. La garde compare **octet pour octet**, sur
`fr` **et** les 4 langues cibles :

1. la rangée du registre `eav-content.csv` (décision de #1979) ;
2. la ligne `res.RuleMemoCardFileNamePrefix` de `docs/dnn-localization/dnn-ui-strings.csv`.

Les deux portent la même chaîne sur les 5 langues. Un écart, et le script refuse : deux magasins
qui divergent sur *ce qu'il faut écrire* sont un défaut d'arbitrage, pas une donnée à deviner.

## 3. Le consommateur n'est pas la surface servie — et c'est mesuré, pas supposé

Le préfixe nomme les cartes de la règle du jeu (paquet, pas site). Sonde sur **16 routes servies**
(8 cultures × accueil + `Actus`), rendu frais par cache-buster :

| Sonde | Résultat |
| --- | --- |
| Nom de clé `RuleMemoCardFileNamePrefix` dans le HTML | **0** sur 16/16 |
| Une des 5 valeurs (`Argumentum - Règle` + 4 traductions) dans le HTML | **0** sur 16/16 |

⚠️ **Un zéro n'est une absence que si l'instrument pouvait voir un un.** Ici le contrôle inverse est
la même sonde sur la même page : elle compte `app-news5-details-link` = **5** sur les 8 routes
`Actus`. L'instrument lit donc bien le corps servi ; ces chaînes n'y sont simplement pas.

⇒ **Aucune preuve « à l'écran » n'est possible pour cette injection, par construction.** La preuve
est la relecture en base, plus la mesure indépendante du §5 — pas une capture de page.

## 4. Ce qui a été écrit, et sous quelles gardes

**4 valeurs** dans `TsDynDataValue` + **4 rangées** dans `TsDynDataValueDimension`, en **une
transaction tout-ou-rien** :

| Dimension | Langue | Valeur |
| --- | --- | --- |
| 8 | ru | `Argumentum - Правило` |
| 11 | ar | `Argumentum - قاعدة` |
| 12 | fa | `Argumentum - قانون` |
| 13 | zh | `Argumentum - 规则` |

Gardes, toutes assertées avant la première écriture :

- **addition pure** : dims `8/11/12/13` **absentes**, dims `7/9/10` **présentes** — sinon refus ;
- **identité du pivot** : la rangée sans dimension vaut le `fr` du registre, sensible à la casse ;
- **compte exact** : 4 rangées de valeur au total et **exactement 1** sans dimension. ⚠️ Première
  version du script : la requête de pivot rendait les **4** rangées (elle ne filtrait pas les
  traductions déjà présentes) et le script refusait avec « expected 1, found 4 » — le refus était
  correct, la requête ne l'était pas. Corrigée en sélectionnant les 4 et en assertant la décomposition ;
- **relecture dans la transaction** : 4/4 octet pour octet, sinon `ROLLBACK` complet ;
- **relecture après commit**, portée de commande séparée : **7/7** conformes (en/ru/pt/es/ar/fa/zh).

Recycle app-domain après commit (touch mtime `web.config`, **sha256 asserté inchangé**, mécanisme
`pl1180`) — jamais `iisreset`. Rollback nominatif : 4 `ValueId` en JSON du scratchpad de session,
**jamais committé**.

## 5. Preuve indépendante, après coup : l'écart registre ↔ base tombe de 4

`python tools/458-eav-content-export.py verify` mesure l'écart entre le registre et la base. Il
n'était pas à zéro avant le geste et **n'a pas à l'être après** (les deux magasins divergent dans
les deux sens — voir l'arbitrage ouvert du pool) :

| Mesure | Valeur |
| --- | --- |
| Écarts avant injection | **129** |
| Écarts après injection | **125** |
| Delta | **−4**, exactement les 4 cellules écrites |

C'est la preuve post-hoc : un instrument tiers, qui ne connaît pas le script d'injection, voit
disparaître précisément les 4 écarts concernés. Le `grep` sur `RuleMemoCardFileNamePrefix` dans la
sortie de `verify` ne rend plus rien — ces cellules ne sont plus des écarts.

## 6. Non-régression servie, après recycle

| Mesure | Résultat |
| --- | --- |
| HTTP 200 | **16/16** |
| `app-news5-details-link` = 5 (routes `Actus`) | **8/8** |
| `Error Showing Content` | **0** |
| `<html lang>` distincts | **8** (`fr-FR`, `en-US`, `es-ES`, `pt-BR`, `ru-RU`, `zh-CN`, `ar-SA`, `fa-IR`) |

⚠️ **Piège d'instrument rencontré pendant cette mesure** : `http://…/fr/` rend **404**, et cela se
lit à tort comme une panne. La culture par défaut est servie **à la racine** (`/`, `/Actus`) ; les
7 autres le sont sous leur code. Une sonde qui interroge `/fr/` fabrique un faux rouge — et un
« 404 uniforme sur toutes les cultures » n'est, lui, qu'une fenêtre de recycle. Les deux se
distinguent en regardant **si les autres cultures répondent** : ici 7/8 répondaient 200, donc
404 sur `/fr/` = structure de routage, pas indisponibilité.

## 7. Comment vérifier que ce constat est mort

```sql
-- doit rendre 8 lignes de valeur, dont 7 dimensionnées (dims 7..13)
SELECT d.DimensionId, CAST(v.Value AS NVARCHAR(MAX))
FROM TsDynDataValue v
JOIN TsDynDataValueDimension d ON d.ValueId = v.ValueId
JOIN TsDynDataEntity e ON e.EntityId = v.EntityId
JOIN TsDynDataAttribute a ON a.AttributeId = v.AttributeId
WHERE CONVERT(NVARCHAR(64), e.EntityGuid) = 'DEA9410D-274B-4A4E-B47C-7D5D00692348'
  AND a.StaticName = 'RuleMemoCardFileNamePrefix'
ORDER BY d.DimensionId;
```

Puis `python tools/458-eav-content-export.py verify` : l'écart doit être **125**, et non 129. Si un
`RuleMemoCardFileNamePrefix` réapparaît dans les écarts, l'injection a été défaite (restauration de
base) — c'est le signal, et il est nominatif.
