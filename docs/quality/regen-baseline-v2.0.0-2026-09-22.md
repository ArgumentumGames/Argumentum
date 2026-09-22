# Ligne de base de régénération v2.0.0 — signatures page-par-page du bundle de recette

**Date** : 2026-09-22 · **Lane** : po-2023 (worker) · **Base** : `origin/master` `64d1b92c`
**Grain** : ⓿ du pool v2 sur #458 (dispatch c.5773133629 — « prioritaire et fenêtré : c'est le seul
grain de ce pool qui devient impossible si on le repousse »).

**DoP/DoD du dispatch** : un fichier de signatures **par document × langue**, avec le **sha256 du PDF
source à côté** de chaque entrée ; contrôle = re-rejouer l'instrument sur **2 PDF au hasard** ⇒
signatures identiques au fichier committé.

---

## Pourquoi ce fichier existe

Le bundle de recette `review-v2.0.0-regen-20260921` (régén v2.0.0 soldée, #1465) est la référence
visuelle actuelle. Toute régénération future devra prouver qu'elle n'a **rien changé** (ou nommer
exactement quoi). `tools/pdf-page-signature.py` compare **deux** PDF ; ce qui manquait, c'était la
**première** moitié figée. Ce grain la capture **pendant que le bundle existe encore** : les PDF de
relecture sont un paquet éphémère sur le drive, pas un artefact versionné.

## Artefacts livrés

| Fichier | Rôle |
|---|---|
| `docs/quality/regen-baseline-v2.0.0.json` | le manifeste : 1 entrée par PDF (langue, octets, sha256 du PDF source, pages, signatures page-par-page) |
| `tools/pdf-page-signature-batch.py` | le driver de capture (`--check` rejoue et compare au manifeste) |

## Mesure

- **Bundle source** : `G:\Mon Drive\MyIA\Argumentum\Fallacies\Recette v2.0.0\review-v2.0.0-regen-20260921`
  (régén du 21/09/2026, base `c9290bab`, 80 PDF CMYK — 10 par langue × 8 langues : ar en es fa fr pt ru zh).
- **Capturé** : 80 PDF · 9 360 pages · 8 langues × 10 PDF × 1 170 pages (symétrie complète).
- **Instrument** : `tools/pdf-page-signature.py` incluant le fix smask du grain ⑦bis
  (PR #1509) — sha256 `6553b6150c18f9aab862a0903192b995bf9424995554214d54079e4129b1c56d`,
  **enregistré dans le manifeste** (`instrument_sha256`). Le driver importe `page_sig` depuis
  l'instrument livré — aucune redéfinition : la signature capturée aujourd'hui est exactement celle
  que la comparaison produira demain.
- **Smasks** : 0 smask sur les 1 709 images des 10 PDF **fr** (mesure dédiée ; le scan 8 langues a
  été interrompu pour ne pas disputer le débit du drive pendant la capture). La signature couvre
  les smasks depuis ⑦bis partout où ils existeraient — l'ordre de merge des deux grains est donc
  sans effet sur ce manifeste.

## Contrôle (DoD)

Re-jeu de l'instrument sur 2 PDF tirés au hasard, comparé au manifeste committé :

```
python tools/pdf-page-signature-batch.py --check docs/quality/regen-baseline-v2.0.0.json <pdf1> <pdf2>
```

Résultat mesuré avant commit (tirage seed 20260922) : `IDENTIQUE` ×2 —
`zh/Argumentum_Fallacies_Web_A0_zh.pdf` (1/1 page) et `fa/Argumentum_PokerCards_fa.pdf`
(334/334 pages), rc=0.

## N'établit pas

- **Ce que la signature ne couvre pas** reste ce que l'instrument déclare non-objectif : texte
  vectoriel, placement des images, métadonnées (voir PORTEE EXACTE en tête du module).
- Le manifeste ne prouve pas que le bundle est **correct** — seulement **quoi** il est, page par
  page. La QA visuelle (ai-01) reste l'organe du verdict de contenu.
- Les chemins `G:\…` du manifeste sont des **provenances** enregistrées, pas des dépendances : le
  re-jeu du contrôle marche depuis toute copie des PDF.

## Reproductibilité

```bash
# re-capturer (si un nouveau bundle devient la référence) :
python tools/pdf-page-signature-batch.py "<bundle_dir>" <out.json>
# contrôler ce manifeste contre 2 PDF quelconques du bundle :
python tools/pdf-page-signature-batch.py --check docs/quality/regen-baseline-v2.0.0.json pdf1 pdf2
```

---
*po-2023 (worker lane) — signale, ne déclare pas PASS · verdict : ai-01 & jsboige*
