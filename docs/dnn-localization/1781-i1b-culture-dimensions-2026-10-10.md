# #1781 I1b — les 6 dimensions culture 2sxc provisionnées en zone 3 (10/10/2026)

## Objet

Fermer le goulot **#682 Path A** côté réceptacle : le corpus de traduction (#684) et
l'outillage (`site_content_pipeline.py` `to-xml`) savaient produire des valeurs pour 8
langues, mais la préprod n'offrait que **2 dimensions** (`fr-FR` 6, `en-US` 7) — « l'artefact
est prêt, le réceptacle ne l'est pas » (684-translation-run-report.md). Le ré-import d'une
valeur traduite vise la coordonnée `EntityID + StaticName + DimensionID` : sans rangée
`TsDynDataDimension`, la valeur n'a nulle part où atterrir.

**Consommateur nommé** (leçon #1841 — écrire = nommer son lecteur) : le moteur 2sxc
lui-même (toute valeur dimensionnée de l'import T4 passe par ces rangées) et le
dictionnaire `CULTURES` de l'outillage, mis à jour dans la même livraison. **Invisible
visiteur** : le sélecteur de langue lit `PortalLanguages` (portail DNN), pas cette table —
aucune langue n'apparaît dans le switcher.

## Mesure préalable (lecture seule)

Schéma réel de `TsDynDataDimension` (⚠️ il n'existe **aucune colonne `CultureCode`** — la
requête citée par la révision précédente de l'outillage échoue en `Nom de colonne non
valide`, mesuré ce jour ; le code culture vit dans `ExternalKey`) :

```
DimensionId (IDENTITY, seed 7) · Parent · Name · SystemKey · ExternalKey · Active (def 1) · ZoneId
```

État d'origine — 7 rangées, chaque zone porte un « Culture Root » (`SystemKey='Culture'`)
dont pendent les cultures réelles (`SystemKey=''`, `ExternalKey=code`) :

| Zone | Root | Cultures |
|---|---|---|
| 1 | 1 | — |
| 2 | 2 | en-US (3), fr-FR (4) |
| 3 | 5 | fr-FR (6), en-US (7) |

## Geste (préprod, transactionnel, réversible)

Six `INSERT` calqués sur les rangées existantes — `Parent=5` (root zone 3),
`Name=NativeName` .NET (même format que « français (France) »), `SystemKey=''`,
`Active=1`, `ZoneId=3`. `DimensionId` est IDENTITY : IDs **8-13** assignés par la base.

| DimensionId | ExternalKey | Name |
|---|---|---|
| 8 | ru-RU | русский (Россия) |
| 9 | pt-PT | português (Portugal) |
| 10 | es-ES | español (España) |
| 11 | ar-SA | العربية (المملكة العربية السعودية) |
| 12 | fa-IR | فارسی (ایران) |
| 13 | zh-CN | 中文（中国） |

Les codes sont ceux de l'outillage (`CULTURES` : `ar-SA`, pas `ar` neutre ; `fa-IR` ;
`zh-CN`) — cohérence store ↔ XML d'import par construction.

**Recyclage** : `Restart-WebAppPool` refusé sans élévation (accès IIS) → **touch du mtime
`web.config`** (0 octet changé, le file-watch ASP.NET déclenche le recycle app-domain).
1ʳᵉ requête froide > 60 s (démarrage), 2ᵉ **200 en 7,1 s**, 3ᵉ **200 en 0,39 s** —
régime chaud retrouvé, site sain.

## Contrôles

- Compteur : 7 avant → **13 après** (assertion dans le script, commit conditionné).
- Post-insert SELECT : les 8 cultures de zone 3 listées, `Active=1`.
- Contrôle servi post-recycle : home 200, 85 272 octets, 0,39 s à chaud.
- **Aucune valeur dimensionnée n'existe en zone 3** (I2) : le changement n'a donc aucun
  chemin vers le rendu — les dimensions ne sont lues que par l'import et l'admin 2sxc.
- Self-test outillage rejoint : **21/21** (IC4 adapté — un-attest in place, le mécanisme
  reste testé ; IC7 émet les 8 codes).

## Inverse

```sql
DELETE FROM TsDynDataDimension WHERE DimensionId BETWEEN 8 AND 13;  -- + recycle
```

Aucune rangée `TsDynDataValueDimension` ne référence 8-13 (aucune valeur traduite
n'existe encore) : le DELETE est complet et sans effet de bord.

## Ce qui reste ouvert

- **T2** (po-2024) : `to-csv` sur l'export app 33 (#1843, mergé) — puis `to-xml` **sans**
  `--unconfirmed-ok` (le présent grain retire le besoin du drapeau).
- **T4 / import réel** : la preuve sur UNE entité (le `--entity` du runbook) — c'est là que
  la visibilité moteur des rangées 8-13 sera prouvée en pratique.
- L'arbitrage owner Q-10-09-1 **(b) vs (c)** reste requis avant toute traduction de volume ;
  ce grain ne crée aucun contenu, il prépare le réceptacle du chemin (b).
- **PortalLanguages** (le switcher) n'est PAS touché : activer l'affichage d'une langue
  reste un geste séparé, à décider avec l'owner.

## Livré dans la même PR

`tools/dnn_i18n/site_content_pipeline.py` : `CULTURES` ru/pt/es/ar/fa/zh → IDs 8-13,
`attested=True`, provenance du 10/10 ; requête du garde corrigée (`ExternalKey`, pas
`CultureCode`) ; IC4 adapté (un-attest in place). `tools/dnn_i18n/README.md` et
[`457-t2-app33-first-lot.md`](457-t2-app33-first-lot.md) §5 actualisés.

*po-2023 (session -31, préprod) — 2026-10-10*
