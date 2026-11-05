# Contrôles du jour J — ligne de base `preprod-baseline-2026-11-05`

- Cible : `https://dnn.argumentum.myia.io`
- Mesuré : 2026-11-05 01:29 (lecture seule, cache-buster `?cb=` sur chaque sonde, `-L`)

## 1. Routes (8 cultures × 4 surfaces)

| Langue | Surface | URL | Statut | `<html lang>` | Octets |
|---|---|---|---|---|---|
| fr | accueil | `/` | ✅ 200 | fr-FR | 85472 |
| fr | terms | `/terms` | ✅ 200 | fr-FR | 62001 |
| fr | privacy | `/privacy` | ✅ 200 | fr-FR | 58022 |
| fr | règles | `/Règles` | ✅ 200 | fr-FR | 50665 |
| en | accueil | `/en/` | ✅ 200 | en-US | 80645 |
| en | terms | `/en/terms` | ✅ 200 | en-US | 54573 |
| en | privacy | `/en/privacy` | ✅ 200 | en-US | 48845 |
| en | règles | `/en/Règles` | ✅ 200 | en-US | 46613 |
| es | accueil | `/es/` | ✅ 200 | es-ES | 84611 |
| es | terms | `/es/terms` | ✅ 200 | es-ES | 61121 |
| es | privacy | `/es/privacy` | ✅ 200 | es-ES | 56382 |
| es | règles | `/es/Règles` | ✅ 200 | es-ES | 50108 |
| pt | accueil | `/pt/` | ✅ 200 | pt-BR | 84214 |
| pt | terms | `/pt/terms` | ✅ 200 | pt-BR | 60354 |
| pt | privacy | `/pt/privacy` | ✅ 200 | pt-BR | 55047 |
| pt | règles | `/pt/Règles` | ✅ 200 | pt-BR | 49814 |
| ru | accueil | `/ru/` | ✅ 200 | ru-RU | 91737 |
| ru | terms | `/ru/terms` | ✅ 200 | ru-RU | 92210 |
| ru | privacy | `/ru/privacy` | ✅ 200 | ru-RU | 79242 |
| ru | règles | `/ru/Règles` | ✅ 200 | ru-RU | 53416 |
| zh | accueil | `/zh/` | ✅ 200 | zh-CN | 82482 |
| zh | terms | `/zh/terms` | ✅ 200 | zh-CN | 50485 |
| zh | privacy | `/zh/privacy` | ✅ 200 | zh-CN | 47300 |
| zh | règles | `/zh/Règles` | ✅ 200 | zh-CN | 48828 |
| ar | accueil | `/ar/` | ✅ 200 | ar-SA | 88375 |
| ar | terms | `/ar/terms` | ✅ 200 | ar-SA | 74191 |
| ar | privacy | `/ar/privacy` | ✅ 200 | ar-SA | 65965 |
| ar | règles | `/ar/Règles` | ✅ 200 | ar-SA | 52605 |
| fa | accueil | `/fa/` | ✅ 200 | fa-IR | 90366 |
| fa | terms | `/fa/terms` | ✅ 200 | fa-IR | 78882 |
| fa | privacy | `/fa/privacy` | ✅ 200 | fa-IR | 70848 |
| fa | règles | `/fa/Règles` | ✅ 200 | fa-IR | 53005 |

## 2. Texte légal anglais (les deux sens)

| Page | Statut | Marqueur propre à la page | Texte stock DNN | Verdict |
|---|---|---|---|---|
| `/en/terms` | 200 | « ARGUMENTUM SAS » ×1 | « AGREEMENT BETWEEN USER AND » ×0 | ✅ |
| `/en/privacy` | 200 | « The French version of this page is the authoritative version » ×1 | « AGREEMENT BETWEEN USER AND » ×0 | ✅ |

Lecture : le marqueur de la page doit être **présent** (≥ 1) et le texte **stock de DNN** absent (0).
Un marqueur à 0 avec un stock à 0 est une page vide, pas un succès.

⚠️ Le marqueur de `terms` est « ARGUMENTUM SAS » ; celui de `privacy` est la phrase de primauté du
français — la politique de confidentialité **ne nomme pas la société**, ni dans le neutre ni dans le
fr-FR (mesuré 05/11 sur les valeurs du dépôt). Un contrôle qui exigerait « ARGUMENTUM SAS » sur
`privacy` ne pourrait pas passer.

## 3. Compteurs de joueurs servis (#1502 / D1)

| Langue | Statut | Intervalles | Nombres lus | Min==max | 3→10 | 4→10 | 4→8 |
|---|---|---|---|---|---|---|---|
| fr | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 6 | 1 | 0 | 0 |
| en | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 6 | 1 | 0 | 0 |
| es | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 6 | 1 | 0 | 0 |
| pt | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 6 | 1 | 0 | 0 |
| ru | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 1 | 1 | 0 | 0 |
| zh | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 6 | 1 | 0 | 0 |
| ar | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 5 | 1 | 0 | 0 |
| fa | 200 | 4 | 1→8, 1→20, 2→8, 3→10 | 5 | 1 | 0 | 0 |

Témoin : une page Règles servie porte au moins un compteur ; 0 est un signal d'instrument
avant d'être un résultat. La colonne 3→10 est la cible #1502 côté cartes.

## 4. Journal Application (EventLog)

- ✅ 0 **FileLoadException** : 0 occurrence(s)
- — **Could not load file or assembly** : 0 occurrence(s)
- — **2sxc** : 0 occurrence(s)
- Fenêtre : 24 h · **685** événements lus

**Verdict** : ✅ tous les contrôles durs passent.

## 5. Lecture de cette ligne de base

*(section écrite à la main — le script ne régénère que les §1-4 ci-dessus)*

**Ce qu'elle établit.** Sur la préprod au 05/11, les quatre contrôles du jour J passent :
32/32 routes servies en 200 avec la bonne culture (8 langues × accueil, terms, privacy, Règles),
les deux pages légales anglaises portent leur marqueur propre **et** aucun texte stock de DNN,
les 8 pages Règles portent le compteur **3→10** (la cible #1502, une fois par langue),
et la fenêtre 24 h du journal Application ne porte **aucune** `FileLoadException`.

**Ce qu'elle n'établit pas.** Le journal ne prouve pas l'absence future : 685 événements lus sur
24 h, dont 0 de la classe cherchée — le lecteur voit du contenu, donc le 0 est un vrai 0 **sur
cette fenêtre**, pas une garantie au-delà. Les compteurs 3→10 sont ceux de la **préprod** ; la
prod sert aujourd'hui une 3ᵉ génération et c'est sa propre mesure qui tranchera au jour J.

**Contrôles inverses (l'instrument doit savoir rougir).**

| Contrôle | Résultat |
|---|---|
| Script lancé sur une base qui n'existe pas (`/AUCUNE-ROUTE`) | **exit 1, 35 ❌** — statuts 404 détectés sur les 32 surfaces + 3 échecs de compteurs |
| Marqueur `privacy` avant recalibrage (« ARGUMENTUM SAS » exigé sur les 2 pages) | **❌ sur `/en/privacy`** — le rouge était réel : la page ne porte pas cette chaîne, et la valeur du dépôt non plus |
| Marqueur `terms` sur la même passe | ✅ ×1 — le compteur voit le 1 quand il y en a un |

**Une correction à porter au runbook.** L'étape 6 bis y prescrit, pour les deux pages, « contient
« ARGUMENTUM SAS » et plus « AGREEMENT BETWEEN USER AND » ». La seconde moitié tient ; la première
n'est **satisfiable que sur `terms`**. Mesuré sur les valeurs du dépôt (`App_GlobalResources/`) :

| Clé | Neutre (le seul servi pour l'anglais) | fr-FR |
|---|---|---|
| `MESSAGE_PORTAL_TERMS.Text` | « ARGUMENTUM SAS » **×1**, SIREN oui | ×1, SIREN oui |
| `MESSAGE_PORTAL_PRIVACY.Text` | « ARGUMENTUM SAS » **×0**, SIREN non | ×0, SIREN non |

Il n'existe **aucun** `GlobalResources.en-US.resx` — confirmé. La politique de confidentialité ne
nomme pas la société : son marqueur de santé est la phrase de primauté du français
(« The French version of this page is the authoritative version »), présente dans le neutre comme
dans le fr-FR. Un contrôle qui exigerait « ARGUMENTUM SAS » sur `privacy` serait rouge sur un site
sain — et le jour J, un rouge faux coûte une fenêtre.

## 6. Rejeu au jour J

```bash
# préprod (ligne de base) — hairpin sur la machine qui sert le site
python tools/1105-cutover-day-controls.py --resolve 127.0.0.1 --label preprod-<date>

# prod, après la bascule (lecture seule, cache-buster, -L)
python tools/1105-cutover-day-controls.py --base-url https://www.argumentum.games \
    --label prod-jour-j --out docs/quality/controles-jour-j-prod-<date>.md
```

Code de sortie 1 = au moins un contrôle dur en échec (route non 200, accueil dans la mauvaise
langue, page légale inversée, compteur Règles absent, `FileLoadException` présente). Un arrêt sur
`Get-WinEvent` rend le contrôle **non concluant** (❌), jamais vert.

*po-2023 · script : `tools/1105-cutover-day-controls.py`*
