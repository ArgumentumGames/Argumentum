# #458 grain 4 — le repli XSLT rendu visible : mesure, avertissement, résumé de fin de passe

**Date :** 2026-10-04 · **lane :** po-2024 · **dispatch :** c.5975630522 (DM `msg-20261004T020549`) · **branche :** `fix/458-xslt-fallback-visible` (depuis master `c373ffef`)

> Objet du grain (verbatim) : « D'abord **mesurer** : dans quels cas ce chemin est pris (FreeMind absent,
> export FreeMind sans fichier produit…) et ce que le journal en dit. Ensuite rendre la chose **visible** :
> avertissement, compté dans le résumé de fin de passe. Et dire si la passe doit échouer. Contrôle inverse :
> un export simulé sans SVG doit produire l'avertissement. »

---

## 1. Mesure — quand le chemin est pris

La chaîne de production des SVG mindmaps a deux étages, et le repli XSLT est le second :

```
CreateFreemindmap → TryAutomateSvgConversion
    ├─ Fallacies  (FallacyMindMapDocumentConfig.cs:446-449) : TryFreeMindSvgExport, POINT — pas de repli
    └─ Virtues    (VirtueMindMapDocumentConfig.cs:306-319)  : TryFreeMindSvgExport
                                                              → repli XSLT (TryXsltSvgConversion)
                                                              → invite manuelle si interactif
```

**Chaîne d'appel mesurée** : `TryXsltSvgConversion` n'est appelé en production que par **un seul point**,
`VirtueMindMapDocumentConfig.cs:312`. Le chemin Fallacies n'a **pas** de repli depuis `75a049d3`
(« Remove XSLT fallback from TryAutomateSvgConversion — it was overwriting valid Batik SVGs with
low-quality XSLT output when SendKeys appeared to fail », commit qui a par ailleurs restauré
l'automatisation FreeMind validée). Les modes `--regen-*` n'appellent ni l'un ni l'autre : ils injectent
dans des `content.svg` existants (`Program.cs:455-458`, `526-531`) — hors chaîne.

### 1.1 Les quatre cas d'entrée dans le repli (étage FreeMind en échec)

Tous ces cas étaient **déjà** avertis par `TryFreeMindSvgExportCore`, puis retournaient `false` —
donc tous convergent vers le repli côté Virtues :

| Cas | Ligne (master `c373ffef`) | Ce que le journal disait déjà |
|---|---|---|
| FreeMind absent (`config.FreeMindPath` invalide ET env `ARGUMENTUM_FREEMIND_PATH` absente) | `:573` | `[Warning] FreeMind not found (…)` |
| Fenêtre FreeMind introuvable après 90 s | `:632` | `[Warning] FreeMind window not found after 90s.` |
| SVG non détecté après les keystrokes (fichier absent ou pas plus récent) | `:692` | `[Warning] FreeMind SVG not detected at '…'` |
| Exception pendant l'export | `:711` | `[Warning] FreeMind SVG export error: …` |

### 1.2 Ce que le journal disait du repli lui-même — **avant** ce grain

| Étape | Ligne (`c373ffef`) | Niveau avant | Niveau après |
|---|---|---|---|
| Entrée dans le repli (côté Virtues) | `VirtueMindMapDocumentConfig.cs:311` | **Info** | **Warning** |
| Engagement effectif (stylesheet `mm2svg.xslt` trouvée) | `FallacyMindMapDocumentConfig.cs:756` — **la ligne nommée par le dispatch** | **Info** | **Warning** (nomme la carte) |
| **Artefact produit** | `FallacyMindMapDocumentConfig.cs:773` | **LogSuccess** | **Warning** (consigne de remplacement) |
| Stylesheet absente | `:751` | Warning | inchangé (aucun artefact) |
| Transformation vide | `:777` | Warning | inchangé (aucun artefact) |
| Exception XSLT | `:782` | Problem | inchangé (aucun artefact) |

Le défaut de visibilité était donc **dans la sortie du repli, pas dans son entrée** : les échecs FreeMind
étaient avertis, mais la production d'un artefact dégradé sortait sous `[Info] Using XSLT fallback…`
puis **`[Success] SVG via XSLT: …`** — un lecteur du journal voyait un succès. C'est cette famille qui a
laissé une carte blanche entrer dans #1740 (triplet zh Vertus de la re-dérivation 3).

### 1.3 Ce que produit ce chemin — la voie est morte, mesuré

| Objet | Taille | `x="NaN"` | `x=""` | `Processing node level` |
|---|---|---|---|---|
| `zh/Argumentum_Virtues_MindMap_zh.content.svg` @ `6a10c3ca` (rd3) et tête `559fd6fa` | **234 631 o** | **223** | 223 | 223 |
| Même fichier @ `9e765e4b`, `102495f1`, master (exports Batik) | **1 148 515 o** | 0 | 0 | 0 |

223 marqueurs = un par nœud : coordonnées jamais calculées, page blanche au rendu, **mais ids présents** —
les portes qui comptent les ids passent vertes dessus (constat de la porte d'arbre de #1740,
`MindmapXsltFallbackGateTests`). Reproduit **dans la garde de ce grain** sur une carte minuscule :
l'artefact produit (1 KB) porte bien `x="NaN"`.

---

## 2. Ce qui a été changé

| Fichier | Ligne (après) | Changement |
|---|---|---|
| `Mindmapper/FallacyMindMapDocumentConfig.cs` | `:722-770` | Signaux : `_xsltFallbackEngagements` / `_xsltFallbackArtifacts` (compteurs process-wide, `Interlocked`), `ResetXsltFallbackSignals()`, `BuildXsltFallbackSummaryLine(engagements, artifacts)` (fonction pure épinglée par la garde) |
| idem | `:798-799` | Engagement : incrément + `LogWarning` nommant la carte (ex-`Log` Info) |
| idem | `:816-817` | Artefact : incrément + `LogWarning` « degraded, dead path #184 » (ex-`LogSuccess`) |
| `Mindmapper/VirtueMindMapDocumentConfig.cs` | `:311-312` | Entrée dans le repli : `LogWarning` (ex-`Log` Info) |
| `AssetConverterConfig.cs` | `:596-620` | Fenêtre de comptage par passe + **résumé de fin de passe** (`LogMindmapXsltFallbackSummary`, `:775`) après LES DEUX créateurs — en mode `AsynchronousPipeline`, résumé émis par une continuation unique après `Task.WhenAll` (deux résumés s'entrelaceraient) |
| `Tests/MindmapGeneration/MindmapXsltFallbackSignalTests.cs` | nouveau | **Contrôle inverse** (2 faits) — voir §4 |

Le résumé se lit en trois formes, toutes mesurées (jamais « tout va bien » au-delà du constaté) :

- `0 artefact` → `Mindmap pass summary: XSLT fallback never engaged (no degraded fallback artifact).`
- engagé sans artefact → `… engaged N time(s) but produced no artifact (stylesheet missing or transform failed — see the warnings above).`
- artefact(s) → `… engaged N time(s) and PRODUCED M degraded SVG(s) (dead path #184 — NaN coordinates, blank render). Do not copy them into the repository: replace each with a FreeMind/Batik export.`

Le cas zéro **ne prétend pas** que tous les SVG viennent de Batik : le chemin Fallacies n'a pas de repli,
une panne FreeMind y laisse simplement aucun SVG (non compté ici) — la formulation s'en tient au mesuré.

---

## 3. Preuve — journal réel et mutations

Extrait du journal (`Logs/file_logger.log`) d'une exécution de la garde, cascade complète, **tous les
niveaux en `[Warning]`** :

```
[Warning] FreeMind not found (config.FreeMindPath='…\FreeMind-absent.exe', env ARGUMENTUM_FREEMIND_PATH unset or invalid). Skipping GUI export.
[Warning] FreeMind GUI unavailable, falling back to XSLT for fallback-signal-<guid>.mm
[Warning] Using XSLT fallback for SVG conversion of fallback-signal-<guid>.mm: …\Mindmapper\xslt\mm2svg.xslt
[Warning] SVG produced via XSLT fallback (degraded, dead path #184): …\fallback-signal-<guid>.svg (1 KB). Replace with a FreeMind/Batik export before committing.
```

Mutations falsifiantes (backup `cp` + restauration `sha256sum -c` — identités vérifiées
`f02b10b6…` (Virtue) et `9b8eb608…` (Fallacy)) :

| Mutation | Rouge attendu | Rouge obtenu |
|---|---|---|
| **M1** — niveau de l'entrée dans le repli (Virtue `:311`) revient à `Log` (Info) | assertion de niveau dans le journal | ✅ 1 rouge nommé — `Expected journal … to contain "[Warning] FreeMind GUI unavailable…"` |
| **M2** — incrément du compteur d'artefacts retiré | delta compteur nul | ✅ 1 rouge nommé — `…XsltFallbackArtifacts - artifactsBefore) to be greater than or equal to 1 … but found 0` |

---

## 4. Contrôle inverse

`MindmapXsltFallbackSignalTests.SimulatedFreeMindExportWithoutSvg_WarnsInJournal_AndCountsTheDegradedArtifact`
simule l'**export sans SVG** : FreeMind « configuré mais absent du disque » (`FreeMindPath` vers un
exécutable inexistant) — échec immédiat et déterministe, sans GUI ni attente de 90 s, indépendant de
l'environnement de la machine. Le test passe par le **point d'insertion de production** (chaîne Virtues,
réflexion — patron déjà utilisé dans `SvgConversionIntegrationTests`), pas par le repli appelé directement,
et éprouve : (1) le repli « réussit » (le piège), (2) l'artefact produit porte `x="NaN"` (famille voie
morte), (3) les deux compteurs avancent, (4) le journal porte les quatre avertissements, niveau compris.
Le nom de fichier du test porte un GUID : le journal est en append, une assertion sur un nom fixe serait
verte à vide dès la seconde exécution.

Le second fait épingle la fonction pure du résumé (0 / engagé-sans-artefact / artefact).

---

## 5. La passe doit-elle échouer ? — verdict

**Oui, à terme — et le premier remède est plus simple que l'échec : retirer le repli côté Virtues.**

1. La voie est **morte mesurée** (§1.3) : son artefact est inutilisable (coordonnées jamais calculées),
   et il porte les ids, donc il passe les portes de comptage. Produire un inutilisable sous une ligne de
   succès est exactement la classe « jeu silencieusement manquant » (#1179/#1177) dont le remède éprouvé
   dans ce dépôt est : résumé de fin de passe **et** échec (#613 : `[HARVEST-PARTIAL]` + `throw`, gardé
   par `ContinueOnHarvestSetFailure`).
2. L'alignement existe déjà : `75a049d3` a retiré ce même repli du chemin **Fallacies**, pour la même
   raison (« overwriting valid Batik SVGs »). La dissymétrie Fallacies/Virtues **est** le défaut.
3. **Option recommandée** — aligner Virtues sur Fallacies (suppression du repli) : la classe disparaît à
   la source ; une panne FreeMind n'écrit alors plus rien (et la question « faut-il échouer sur SVG
   manquant ? » se traite alors du même geste pour les deux chemins).
   **Option de repli** — garder le repli mais faire échouer la passe quand un artefact dégradé est sorti
   (patron #613), le résumé de ce grain en étant le compte.

**Non implémenté dans ce grain**, délibérément : retirer le repli change ce qu'une passe produit sur une
machine sans FreeMind (rien au lieu de déchets) — c'est un changement de sémantique de passe, en pleine
campagne de re-dérivation mindmaps, et c'est la question posée qui doit être tranchée d'abord. En attendant
l'arbitrage, trois filets tiennent : l'avertissement + le résumé (ce grain, live), la porte d'arbre de
#1740 (`MindmapXsltFallbackGateTests`, sur l'arbre committé), et la lecture visuelle.

---

## 6. Périmètre et filets voisins

- **Couvert** : la passe mindmaps de production (`ConverterMode.Mindmapper` → les deux créateurs),
  pour la seule voie qui peut produire un artefact dégradé (Virtues).
- **Hors périmètre, observé** : le chemin **Fallacies** n'a pas de repli — une panne FreeMind y laisse
  aucun SVG, non compté par ces signaux (le retour de `TryAutomateSvgConversion` est ignoré par
  `CreateFreemindmap`, `:313`) ; à traiter si l'option recommandée du §5 est retenue.
- **Filet voisin** : `MindmapXsltFallbackGateTests` (#1740) garde l'**arbre committé** (marqueurs) ;
  ce grain garde la **passe** (journal + résumé). Les deux sont complémentaires, aucun fichier de #1740
  n'est modifié (vérifié par `git merge-tree` avant push).
- **Garde du grain** : `MindmapXsltFallbackSignalTests` (2 faits), nouveau fichier — les deux fichiers de
  tests mindmap de #1740 (`MindmapNodeOrderGateTests`, `MindmapXsltFallbackGateTests`) restent intacts.