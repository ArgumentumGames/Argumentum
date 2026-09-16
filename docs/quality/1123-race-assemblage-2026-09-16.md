# #1123 — Dossier d'analyse de la race d'assemblage d'images (cause racine #1121)

**Auteur** : po-2023 (worker lane) · **Date** : 2026-09-16 · **Base** : `origin/master` `503e64c8`
**Nature** : **lecture seule** — aucune régénération, aucun fix, aucun test exécuté sur le pipeline.
Le dossier consolide : chemin de code **vérifié au jour**, séquence reproductible dérivée du code,
borne d'irreproductibilité, garde proposée. Le fix durable reste #1123 (post-tag, arbitrage A).

---

## §1 — Le défaut et son état au jour (ARMÉ)

Cause racine établie au verdict #1121 (20/08, Phase B — log lignes intercalées, `PokerCards_en.pdf`
1 XObject / 2 pages au lieu de 167, `PokerCards_ru` absent du disque, 80/80 « Generated » loggés,
harvests complets). L'assemblage d'images tourne en parallèle **sans verrou** et le
`File.Exists()` skip-existing + écriture ne sont pas synchronisés.

**État mesuré sur master `503e64c8` (16/09) — le défaut est armé par défaut :**

| élément | état | source |
|---|---|---|
| `EnableParallelism` | **`true`** (défaut) | `WebBasedGeneratorConfig.cs:31` |
| degré docs / langs | **4 / 2** (défauts) | `WebBasedGeneratorConfig.cs:66,68` |
| verrou assemblage images | **aucun** | `ImageFileGenerator.cs` (§2) |
| `pdfLock` | couvre `PdfManager.Generate*` en aval **seulement** | `WebBasedGenerator.cs:111` |

L'arbitrage A de #1121 (« la release passe par une régén sérialisée ») est un **choix de run**, pas
un changement de code : tout run pris avec les défauts ci-dessus rouvre la fenêtre de collision.

## §2 — Chemin de code vérifié au jour (n° de lignes actuels ; le body #1123 citait l'état d'août)

1. **`ImageFileGenerator.cs:42`** — `Parallel.ForEach(enabledDocs, …)` ×4 sur les documents ;
   **`:55`** — `Parallel.ForEach(targetLanguages, …)` ×2 imbriqué sur les langues.
2. **`GenerateFacesAndAssembleCard`** — itère les faces du harvest et appelle, pour chaque carte :
3. **`ImageHelper.cs:108` `LoadAndProcessImageUrl`** — **`:119` `File.Exists(imageFileName)`**
   skip-existing, puis dans le `else` : décodage Magick, `StripAlphaOnWhite`, écritures
   (`Write` original + densité) — **séquence Exists→Write sans aucun verrou**.
4. **`ImageHelper.cs:79` `GetImageFileName`** — le chemin est
   `{lang}/Images/density-{n}/{CardSetName}/{image}.{fmt}` : fonction de la **langue, du CardSet et
   de la densité — PAS du document**. Deux documents activés qui partagent un CardSet
   (mesuré 28/08 #1187 : Fallacies alimente TarotCards, FallaciesWeb, Fallacies-Print&Play)
   **écrivent le même fichier** pour une même langue.

⇒ La collision n'exige ni même document ni même langue côté parallélisme : elle exige **deux
couples (doc, lang) concurrents tombant sur le même (CardSet, langue, image)** — ce que le partage
inter-documents du §2.4 rend structurel.

## §3 — Séquence reproductible (dérivée du code — non exécutée, grain lecture seule)

**Préconditions** (toutes vraies par défaut) : `EnableParallelism=true` · ≥ 2 documents activés
partageant un CardSet (Fallacies ×3 documents) · arbre vierge (aucune image pré-existante → le
skip-existing ne court-circuite pas) · densités identiques.

**Fenêtre** : pour chaque image partagée, deux threads franchissent `File.Exists` (faux des deux
côtés), décodent, puis écrivent **le même chemin** concurremment. Suivant l'entrelacement :
écriture concurrente du même fichier (contenu corrompu ou tronqué), ou thread lecteur (Magick,
assemblage aval) lisant un fichier **à moitié écrit**.

**Conséquences observées le 20/08** (la preuve du verdict) : liste d'assemblage amputée (PDF
1 XObject / 2 pages), fichier final jamais materialisé (ru absent), succès **loggé quand même**
80/80 — le défaut est silencieux jusqu'à l'inspection du bundle.

**Reproduction déterministe en run réel** : non tentée (exigerait une régén — interdite par le
grain ; le DoD #1123 en fera la preuve de sortie du fix). Le témoin unitaire §5.3 vise à s'en
approcher sans pipeline.

## §4 — Borne d'irreproductibilité

- **Probabiliste par construction** : code `d03fa9f3` **identique** entre le bundle gated CLEAN
  (#1035) et le run worktree défectueux du 20/08 — établi au verdict.
- **1 occurrence connue** sur l'historique documenté des runs.
- Runs complets documentés **post-20/08 sans récidive** : 22/08 (80 PDF), 28/08, 06/09 (série
  A-E du discriminant #1338), 12/09 (DoD intégral 80/80, contrôle inverse) — **≥ 5 runs**.
- ⚠️ **Limite de la borne** : le mode de parallélisme effectif de ces runs n'est **pas
  re-mesurable ici** (logs archivés avec les arbres recyclés ; `.regen-final-worktree` purgé), et
  l'arbitrage A documente l'intention de **sérialiser les runs release** — l'absence de récidive
  sur des runs vraisemblablement sérialisés **ne dit rien** du comportement en mode parallèle.
  C'est une absence d'observation, pas une preuve d'absence : la borne ne referme pas le défaut.

## §5 — Garde proposée (proposition ; l'implémentation reste #1123)

1. **Verrou par chemin d'image** (`ConcurrentDictionary<string, SemaphoreSlim>` indexé par
   `imageFileName`, ou lock striping) autour de la séquence Exists→write de
   `LoadAndProcessImageUrl` — cible la ressource partagée exacte (le fichier), **préserve le
   parallélisme inter-couples** : conformité à l'anti-pendule du body #1123 (pas de lock global).
2. **Écriture atomique** : écrire vers un temporaire puis `File.Move` (sur Windows, replace
   existant) — durcit aussi le lecteur concurrent (jamais un fichier à moitié écrit visible).
3. **Témoin unitaire multi-thread** (sans régén) : test `Tests/` invoquant
   `LoadAndProcessImageUrl` N fois concurremment sur le **même chemin cible** (Magick local,
   images embarquées), vérifiant (a) intégrité du fichier final, (b) pas d'exception avalée.
   Rend la garde exécutable à chaque CI au lieu d'attendre un run.
4. **Filet post-mortem existant** : `PdfBundleIntegrity` (#1122) est **déjà mergée**
   (`Tests/PdfAssembly/PdfBundleIntegrity.cs` + `PdfBundleIntegrityLogicTests.cs` +
   `VisualTests/PdfBundleIntegrityTests.cs`) — détecte le symptôme (PDF tronqué / cartes
   manquantes) sur tout bundle généré ; le DoD #1123 l'exige sur la sortie du fix.

## §6 — Ce que ce dossier n'établit pas

⛔ Aucune reproduction exécutée (lecture seule, aucune régén) · aucun fix implémenté (post-tag,
#1123) · aucune mesure de temps avant/après (DoD #1123) · aucun verdict sur le mode parallèle
réel des runs passés (§4) · aucun verdict visuel (ai-01) · aucune décision (jsboige).

---

*Base `503e64c8` · instruments : `git grep`/`git show` sur blobs master, `gh api` issues #1121
(CLOSED 20/08) et #1123 (OPEN, 0 commentaire) · ⛔ aucune régén lourde, aucun test pipeline exécuté ·
verdict visuel : ai-01 · décisions : jsboige.*
