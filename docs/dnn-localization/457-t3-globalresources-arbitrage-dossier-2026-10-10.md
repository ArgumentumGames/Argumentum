# #457 T3 — dossier d'arbitrage `GlobalResources.<culture>.resx` : composition, surface réelle, amont

**Date :** 2026-10-10 · **Lane :** po-2024 (worker, `myia-po-2024`) · **Statut :** mesure seule, 0 écriture DNN, 0 appel payé, 0 resx produit.
**Question à laquelle ce dossier répond :** le statut porte « `GlobalResources` : 7 446 cellules (6 cultures) à
router », le doc T3 (`457-t3-ui-labels-measured.md` §3(b)) porte « arbitrage avant tout outillage ». Ce dossier
est **l'entrée d'arbitrage** que le second réclamait : il mesure de quoi les 7 446 cellules sont faites, ce que
le site en référence réellement, et ce que l'amont DNN couvre.

---

## 1. Composition des 1 241 entrées de base (les 6 familles à variante `fr-FR`)

Parse XML des bases EN/invariantes (l'arbre écarte de lui-même les 24 fantômes MSDN de #1846 — l'instrument
retrouve exactement 1 241, la réconciliation tient) :

| Famille | Base | Prose (≥ 2 mots) | Mot simple | `fr-FR` couvert | extras `fr-FR` |
|---|---:|---:|---:|---:|---:|
| `Exceptions` | 132 | 132 | 0 | 130/132 | 0 |
| `FileUpload` | 4 | 4 | 0 | 4/4 | 0 |
| `GlobalResources` | 182 | 139 | 43 | 179/182 | **67** |
| `List_Country` | 256 | 0 | **256** | 256/256 | 0 |
| `SharedResources` | 617 | 484 | 133 | 606/617 | **22** |
| `WebControls` | 50 | 37 | 13 | 50/50 | 5 |
| **Total** | **1 241** | **796 (64,1 %)** | **445 (35,9 %)** | **1 225/1 241 (98,7 %)** | **94** |

**Décomposition des 7 446 cellules** (1 241 × 6) :

| Tranche | Entrées | × 6 cultures | Nature |
|---|---:|---:|---|
| Prose + boutons | 985 (796 prose + 189 mots simples hors `List_Country`) | **5 910** | traduisable par un LLM — *plafond*, pas worklist |
| Noms de pays (`List_Country`) | 256 | **1 536** | données curatées — un paquet amont les fournit mieux qu'un LLM |

**La dérive `fr-FR` est un fait de version, et c'est un avertissement pour tout install futur** : le `fr-FR`
présent couvre 1 225/1 241 clés de base, en manque 16, et porte **94 clés que la base EN n'a pas** (67 dans
`GlobalResources`, 22 dans `SharedResources`, 5 dans `WebControls`). Un paquet installé sur une plateforme
d'une autre version laisse exactement ce genre d'écart.

## 2. La surface réellement référencée — quasi nulle côté Argumentum

Deux balayages, chacun avec témoin positif (un instrument sans témoin n'a aucun compte à rendre) :

| Périmètre balayé | Fichiers | Clés des 6 familles référencées |
|---|---:|---|
| **Argumentum** (`Portals/1` + skins, tout type de source) | 538 | **1** (`SharedResources` 1/617) |
| **Source DNNPlatform** (hors obj/bin/resx eux-mêmes) | 970 | **8** (5 `GlobalResources` + 3 `SharedResources` ; `admin`, `DesktopModules`, `Portals`) |

Les libellés que les skins affichent (`Privacy.Text`, `ProductView.*`, …) résolvent contre leurs
**`App_LocalResources` locaux** — une couche différente, dont la seule variante culturelle présent au dépôt est
**`de-DE`** (skins 2shineBS5 et Bootstrap 4 Instant). ⚠️ Le portail Argumentum lui-même (`Portals/1/2sxc`)
**ne porte aucun `.resx`** : ses chaînes vivent en DB (App Resources 2sxc) et en dur dans les gabarits —
c'est la tranche #490/#1849, déjà livrée.

**Limite déclarée** : le cœur DNN compilé (`DotNetNuke.dll`) référence lui aussi ces clés au runtime (pages de
connexion, pages d'erreur, menus de modules) — invisible au grep d'un dépôt qui n'a pas ce code. Le « 8 » est
un **plancher du source greppable**, pas la surface runtime. La surface runtime ne se mesure qu'au rendu —
et **aucune mesure au rendu n'existe** (constat déjà porté par le doc T3, toujours vrai).

## 3. L'amont : 8 cultures officielles, dont 2 des nôtres — et le `fr-FR` présent EST un paquet

Repos `dnnsoftware/Language-Pack-*` (balayage GitHub, 10/10) :

| Culture | Repo officiel | Dernier push |
|---|---|---|
| fr-FR | ✅ `Language-Pack-FR-FR` | 2026-01 |
| de-DE, nl-NL, it-IT, vi-VN | ✅ | 2026-05 · 2026-05 · 2020 · 2026-02 |
| **es-ES** | ✅ `Language-Pack-ES-ES` | **2026-05** |
| **pt-BR** / **pt-PT** | ✅ / ✅ | 2025-06 / 2024-03 (commit initial unique) |
| **ru, ar, fa, zh** | ❌ **aucun repo** | — |

Le paquet `es-ES` embarque `Resources/App_GlobalResources/{Exceptions, FileUpload, GlobalResources,
List_Country, SharedResources, WebControls, prompt}.es-ES.resx` **plus toute la couche locale DesktopModules**
— soit *strictement plus* que le périmètre T3 entier, `prompt` compris (la 7ᵉ famille, que le plan ne nomme
pas et que `fr-FR` n'a pas).

**Le `fr-FR` de l'arbre est un paquet officiel installé, pas une production maison** — trois signatures le
portent : `List_BannedPasswords-1.fr-FR.resx` et `List_ProfanityFilter-1.fr-FR.resx` existent **sans base EN**
(fichiers propres au paquet), `List_Country.fr-FR` couvre 256/256 complet, et `Install/Language/` est vide
(PlaceHolder.txt) — DNN nettoie le répertoire après installation. **Le mécanisme « installer un paquet amont »
est donc déjà la voie établie sur ce site** ; produire au LLM ce qu'un paquet fournit créerait une seconde
source là où la première existe.

## 4. Routage — ce que « 7 446 cellules à router » devient une fois mesuré

**Il n'y a rien à router aujourd'hui vers le pipeline de traduction.** Le nombre se décompose en gestes par
culture, dont deux sont des installs d'amont et quatre n'ont **pas de besoin mesuré** :

| Culture | Geste | Gate |
|---|---|---|
| **es** | installer le paquet officiel `es-ES` (couvre les 7 familles + couche DesktopModules locale, zéro LLM) | GO owner — geste d'install, lane préprod |
| **pt** | installer `pt-BR` **ou** `pt-PT` | **rejoint la question owner PT-PT/PT-BR déjà ouverte** — le choix de paquet et le dialecte du corpus doivent coïncider |
| **ru, ar, fa, zh** | ⛔ **aucun paquet amont n'existe**. Tenir le repli EN (comportement actuel, mesuré nulle part au rendu) **jusqu'à ce qu'une mesure au rendu démontre l'exposition** ; ne produire au LLM qu'alors, scopé aux entrées réellement affichées | mesure au rendu d'abord (culture activée + parcours visiteur), puis GO owner |

**Ce que ce dossier recommande** : la voie (b) — installs d'amont pour es et pt, statu quo documenté pour
ru/ar/fa/zh. La production LLM des 5 910 cellules « traduisables » n'est pas un grain prêt : le besoin n'est
pas mesuré (§2), et les 1 536 cellules `List_Country` ne devraient jamais passer par un LLM.

## 5. Ce que ce dossier n'établit pas

- **La surface runtime** : pages réellement servies à un visiteur ru/ar/fa/zh sur les chaînes du framework —
  aucune culture n'a été activée, aucune page ouverte. Le « 1 clé côté Argumentum / 8 côté source » est un
  plancher de grep, pas un verdict de rendu.
- **La compatibilité de version** des paquets avec le DNN installé (9.13.x) : existence et fraîcheur vérifiées,
  ciblage de version **non vérifié paquet par paquet** — et la dérive `fr-FR` (94 extras / 16 manquantes) montre
  ce que coûte un paquet posé sur une version qui n'est pas la sienne.
- La complétude des 6 langues dans les paquets tiers/community (seuls les repos `dnnsoftware` ont été balayés ;
  `store.dnnsoftware.com` héberge des paquets communautaires non balayés ici).
- « Prose » est une heuristique de compte de mots (≥ 2) — un tri fin par nature de chaîne reste à faire si un
  jour la production LLM est arbitrée.

## 6. Reproduction

```bash
# composition + couverture fr-FR (parse XML des 6 familles à variante)
python -I <scratchpad>/resx_compose.py
# surface référencée Argumentum (538 fichiers, témoin requis)
python -I <scratchpad>/resx_usage.py     # exit 2 si le témoin n'est pas vu
# surface référencée source DNNPlatform (970 fichiers, témoin requis)
python -I <scratchpad>/resx_usage2.py    # exit 2 si le témoin n'est pas vu
# amont : repos de paquets officiels par culture
gh api "search/repositories?q=org:dnnsoftware+Language-Pack&per_page=50" --jq '.items[] | "\(.full_name) [\(.pushed_at)]"'
gh api "repos/dnnsoftware/Language-Pack-ES-ES/git/trees/HEAD?recursive=1" --jq '[.tree[].path]'
```

---

## Sources

- `docs/dnn-localization/457-t3-ui-labels-measured.md` — la mesure T3 d'origine (§3(b) : « arbitrage avant tout outillage »)
- `DNNPlatform/App_GlobalResources/` — 16 fichiers, bases EN + variantes `fr-FR`
- Erratum #1846 (ouvert) — 6 familles à variante / 1 241 entrées, confirmé aujourd'hui par un second instrument
- Statut workspace 10/10 — « `GlobalResources` : 7 446 cellules (6 cultures) à router »
- `dnnsoftware/Language-Pack-*` (GitHub) — 8 cultures officielles, arbres et dates balayés le 10/10
