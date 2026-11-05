# #458 — Résidu des langpacks, à blanc (2026-11-05)

> **Nature** : mesure du dépôt (lecture seule). Item 4 du pool ai-01 (c.6645521254).
> **Base** : `26f40e2b` (branche #2148 — arbre identique à master sur `DNNPlatform/`).
> **Instrument** : [`tools/458-langpack-residual.py`](../../tools/458-langpack-residual.py) —
> une famille = un fichier neutre ; résidu = familles **sans jumeau localisé dans le dépôt**.

## 1. Le résidu, par culture (MESURÉ)

| Culture | Familles suivies | Résidu fichiers | Résidu entrées (~) | Plus gros manque |
|---|---|---|---|---|
| fr-FR | 190 | 37 | 1 573 | NBrightBuy ×5, ResourceManager ×4 |
| es-ES | 192 | 77 | 6 714 | EditBar ×5, ResourceManager ×4 |
| ru-RU | 167 | 59 | 3 969 | **NBrightBuy ×25**, ResourceManager ×4 |
| zh-CN | 132 | 82 | 8 287 | **NBrightBuy ×25**, PersonaBar ×3 |
| ar-SA | 138 | 76 | 8 275 | **NBrightBuy ×25**, PersonaBar ×3 |
| fa-IR | 138 | 76 | 8 275 | **NBrightBuy ×25**, PersonaBar ×3 |
| pt-BR | 140 | **105** | **9 032** | **NBrightBuy ×25**, EditBar ×5 |

Contrôle croisé : es-ES « suivies » = **192** ici et « master resx: 192 » dans le build
#1915 au même commit — les deux instruments comptent le même objet.

**en-US n'est pas une culture de pack** : 3 fichiers `*.en-US.resx` sur 214 familles —
le **neutre est le texte anglais** (même constat que le geste 6 bis du runbook : il
n'existe pas de `GlobalResources.en-US.resx`).

## 2. Lecture — trois familles portent presque tout le résidu

1. **NBrightBuy (la boutique) ×25 fichiers** manque à **toutes** les cultures de pack —
   et même fr-FR n'en suit que 20/25. C'est la plus grosse masse et la seule commune à
   tous. ⚠️ Décision à prendre AVANT d'ouvrir le chantier : la boutique doit-elle être
   localisée, ou servie en fr/repli ? (Le site vend aujourd'hui en fr.)
2. **Console admin** (EditBar, PersonaBar, ResourceManager, CKE) — visible par les
   administrateurs, pas par les visiteurs. La relecture #2132 ne la voit pas.
3. **Le reste** : Auth services, Install, skins — fichiers isolés.

pt-BR est la culture la plus en retard (105 familles manquantes) ; le résidu 18/09
« 197 fichiers / 10 408 clés » datait d'avant les imports module-level #2111-#2113 —
il est tombé à 77-105 selon la culture, mais il n'est **pas** nul.

## 3. Ce que ce résidu n'est pas

- Il n'est pas **le résidu servi** : le site de test sert aussi les packs **amont
  installés le 12/10**, absents du dépôt (R8 du runbook) — une famille manquante ici
  peut être couverte sur le site de test. Seul le build + l'installation tranchent.
- Il n'est pas bloquant pour le build : `458-langpack-build.py --culture es-ES` passe au
  HEAD courant (192/192 familles **suivies**, dry-run OK) — le pack livrera ce que le
  dépôt suit ; les familles non suivies retombent sur le neutre anglais **ou** sur le
  pack amont déjà installé.
- Les entrées « ~ » comptent `name="` (quelques attributs hors `<data>` inclus) :
  à comparer entre lignes, pas à additionner avec un compte exact.

## 4. À trancher avant go-live (le tableau à blanc)

| # | Décision | Options | Recommandation po-2023 |
|---|---|---|---|
| 1 | NBrightBuy ×25, toutes cultures | (a) localiser la boutique (b) la servir en fr/repli (c) statuquo | **(b)** au go-live, localiser ensuite — la boutique n'est pas dans le périmètre #2132 et 25 familles ≈ un chantier entier |
| 2 | Console admin (EditBar/PersonaBar/ResourceManager) | (a) importer les amont dans le dépôt (b) laisser le repli neutre | **(a) pour ru/zh/ar/fa/es seulement si les packs amont existent déjà** (l'import #1903-model est le geste connu) ; sinon repli |
| 3 | pt-BR 105 familles | (a) importer l'amont pt-BR complet (b) livrer le pack partiel | **(a)** — c'est l'écart le plus creux et l'amont existe (modèle #1903) |

*po-2023 · mesure : `python tools/458-langpack-residual.py` (lecture seule)*
