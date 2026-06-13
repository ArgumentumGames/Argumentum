# Investigation — Memory / Perf Hotspot Audit (#29)

**Date** : 2026-06-14
**Issue** : [#29](https://github.com/ArgumentumGames/Argumentum/issues/29) — *perf(pipeline): consommation mémoire élevée / machine qui rame (rapport 2021)*
**Auteur** : ai-01 (Claude Opus)
**Branche** : `docs/29-memory-perf-investigation` (off master `d411704a`)
**Type** : DOC-ONLY — aucune modification de code/CSV/prompt/config.

> **Méthode** : grounding technique (lecture du code, chemins `fichier:ligne`), croisement avec le fix déjà mergé (#436), les commentaires de triage (#29 ai-01 2026-06-02 + po-2024/ai-01 2026-06-03) et l'historique git `*Magick*`/`*memory*`. Chaque affirmation est qualifiée **VÉRIFIÉ** (lu dans le code/git), **RAPPORTÉ** (issue/PR/commentaire/CLAUDE.md) ou **SUPPOSÉ** (déduction statique non confirmée par profiling).
>
> **Limite forte (à lire d'abord)** : **aucun run de profiling (`dotnet-trace` / `dotnet-counters` / dump mémoire) n'a été exécuté** pour cette investigation. Toutes les « scale-risk » ci-dessous sont des **hypothèses d'analyse statique**, pas des pics mesurés. Le seul chiffre quantitatif existant (« ~1,2 GB ») provient du message de commit #436 (RAPPORTÉ, non re-mesuré ici). Le but est de **prioriser** où un profiling ciblé et un fix délibéré devraient porter — **pas** d'appliquer un fix.

---

## 0. Résumé exécutif

- La cause **2021** (rapport d'origine) est **obsolète** : le profil mémoire a entièrement changé depuis la réécriture (Playwright + QuestPDF + Magick.NET). **RAPPORTÉ** (#29 commentaire ai-01 2026-06-02). L'issue reste pertinente comme **optimisation mémoire/streaming** sur les gros runs Release multi-langues.
- **#436 (commit `f61f727d`) a déjà fixé les 2 plus gros offenders Magick.NET** : la `MagickImageCollection` d'assemblage PDF (`PdfManager.cs:206`) et le `MagickImage` de traitement d'image (`ImageHelper.cs:106`) sont désormais en `using`. **VÉRIFIÉ** (diff du commit, cf. §1).
- Il reste **8 hotspots** identifiables statiquement, dont **3 LOW-risk safe-to-apply** (purs `using`/dispose sans état partagé) et **5 MED/HIGH** qui touchent le pool de pages Playwright, l'architecture de buffering image, ou exigent un changement de design (streaming). **VÉRIFIÉ** (chemins ci-dessous).
- L'offender résiduel **le plus probable** au pic n'est plus un `IDisposable` oublié, mais **deux patterns architecturaux** : (a) **toutes** les images d'un document Tarot sont décodées en `MagickImage` plein-résolution **en une fois** avant écriture du PDF (`PdfManager.cs:31/52-70/98-107`), et (b) le **Print&Play** charge **tous** les octets de **toutes** les cartes (recto + verso) en RAM simultanément (`PdfManager.cs:154-155` → `PrintAndPlayDocument`). Ces deux-là sont **MED/HIGH** (ré-architecture, pas un simple `using`). **VÉRIFIÉ**.
- **Bonne nouvelle (pas un hotspot)** : le `harvestDictionary` ne garde **pas** tous les harvests en RAM — il stocke des `Func<CardSetHarvest>` qui **rechargent depuis le disque à la demande** (`HarvestManager.cs:127,159` ; `ImageFileGenerator.cs:75`). Le pic « tous les data-URLs base64 de 8 langues en mémoire » n'a **pas** lieu. **VÉRIFIÉ**.
- **Pas de chargement de fonts custom** dans le pipeline (aucun `FontManager.RegisterFont`/`.ttf` côté C# — QuestPDF utilise les polices système). Le risque « shared fonts » évoqué dans le cadrage est donc **faible** côté code .NET ; les polices vivent dans CardPen (navigateur). **VÉRIFIÉ** (`grep FontManager|RegisterFont|\.ttf` = 0 hors DNN/tests).

---

## 1. Ce que #436 a déjà corrigé (baseline)

Commit `f61f727d` — *fix(#29): deterministic Magick.NET dispose in PdfManager + ImageHelper (#436)*. **VÉRIFIÉ** (`git show f61f727d`).

| Avant | Après | Effet | Qualif. |
|---|---|---|---|
| `targetFile.documentImages().Write(...)` (collection jamais disposée → finalizer GC) | `using var collection = targetFile.documentImages(); collection.Write(...)` (`PdfManager.cs:206-207`) | dispose en cascade de tous les `MagickImage` enfants après écriture | VÉRIFIÉ |
| `MagickImage imageFromEmbeddedUrl` via if/else, jamais `Dispose` | `using var imageFromEmbeddedUrl = imageUrl switch {…}` (`ImageHelper.cs:106-111`) | dispose déterministe de l'image après resize/CMYK/write | VÉRIFIÉ |

Le message de commit cite **« ~1,2 GB peak held until GC finalizer »** pour la collection PDF (Fallacies Tarot ~277 images). **RAPPORTÉ** (commit message ; **non re-mesuré ici**). Ces deux corrections sont la part « P0 » du triage #29 ; elles **ne couvrent pas** le pic d'assemblage lui-même (cf. H1/H2 ci-dessous) ni le pool Playwright (H3).

> Note de cohérence : le commentaire de triage #29 (po-2024, 2026-06-03) listait `ImageHelper.cs:19-23` (`LoadImageFromPath`) comme « Magick.NET non-disposé ». **C'est désormais résolu en amont** : `LoadImageFromPath` retourne toujours un `MagickImage`, mais son **unique appelant** (`LoadAndProcessImageUrl`) le capture en `using` depuis #436 (`ImageHelper.cs:106-111`). Le triage est donc **partiellement périmé** sur ce point précis. **VÉRIFIÉ**.

---

## 2. Table priorisée des hotspots

> Toutes les lignes ci-dessous sont sur master `d411704a`. « scale-risk » = hypothèse statique (8 langues × milliers d'images), **non profilée**. « apply-risk » = risque d'**appliquer** le fix (LOW = pur `using`/dispose, aucun état partagé ; MED/HIGH = touche pool Playwright / architecture buffering / design).

| # | Fichier:ligne | Ce qui est alloué | Scale-risk (hypothèse) | Fix recommandé | Apply-risk | Qualif. |
|---|---|---|---|---|---|---|
| **H1** | `PdfManager.cs:31`, `:52-70`, `:98-107`, `:121` | `new MagickImage(...)` pour **chaque** carte (recto+verso), agrégées dans un `List<MagickImage>`/`MagickImageCollection` **avant** `Write` | **HIGH** : Tarot complet = ~277+ images plein-résolution (PNG lossless/CMYK en Release) **toutes décodées en RAM simultanément** par document, × parallélisme docs (jusqu'à 4) | Streamer carte-par-carte vers le PDF, ou borner la taille de batch ; ou pré-redimensionner. Pas un simple `using`. | **HIGH** | VÉRIFIÉ (alloc) / SUPPOSÉ (pic) |
| **H2** | `PdfManager.cs:154-155` → `PrintAndPlayDocument.cs:16-17,61-66` | `frontImagesData` + `backImagesData` = `List<byte[]>` contenant **tous** les octets de **toutes** les cartes (recto ET verso) du document, gardés vivants pendant tout le rendu QuestPDF | **HIGH** : Print&Play Tarot Release = PNG lossless (`File.ReadAllBytes`, pas de JPEG) ; 2 × N cartes × ~taille PNG, **tout en RAM en même temps** | Lire les octets à la demande page-par-page (lazy) au lieu de tout précharger ; ou libérer `frontImagesData`/`backImagesData` après composition. Changement de design. | **MED** | VÉRIFIÉ |
| **H3** | `HarvestManager.cs:327` (`Freepages`), `:349-352` (`ReleasePage`), `:329-347` (`GetFreePage`) | `IPage` Chromium empilées dans `ConcurrentStack<IPage>`, **jamais `CloseAsync()` individuellement** ; seules libérées via fermeture du `_browser` (statique) à `DisposeAsync` (`:21-28`) | **MED** : pages résidentes = jusqu'au cap de parallélisme harvest (`MaxDegreeOfParallelismCardpen`=3 × `…Translations`=2 ⇒ ~6 pages vivantes), chacune avec son contexte/heap Chromium ; le pool n'est jamais purgé en cours de run | Fermer les pages excédentaires au-delà d'un seuil, ou `Close()` au lieu de `Push()` quand le pool dépasse le cap ; au minimum vider `Freepages` (`CloseAsync` chacune) avant la phase image. **Attention état partagé** (`_browser` static, parallélisme). | **MED** | VÉRIFIÉ (pas de Close) / SUPPOSÉ (pic) |
| **H4** | `PdfManager.cs:177-191` (`ConvertToJpeg`) | `new MagickImage(imageData)` en `using` (OK) **mais** `image.ToByteArray()` + l'`imageData` d'entrée + le retour coexistent ; appelé **N×2 fois** (recto+verso) en Debug via `Select(...).ToList()` (`:154-155`) | **MED** : pic transitoire = (PNG source + MagickImage décodé + JPEG sortie) × cartes matérialisées d'un coup par `.ToList()` | Le `using` est déjà correct ; le risque est le `.ToList()` matérialisant tout (cf. H2). Pas de fix `using` isolé utile ici. | **MED** | VÉRIFIÉ |
| **H5** | `PrintAndPlayDocument.cs:101` | `File.ReadAllBytes(imagePath)` du header **ré-lu à chaque page** (`ComposePage` appelé par page) | **LOW-MED** : header relu N fois (N = nb pages) au lieu d'1, octets dupliqués transitoirement par page | Lire le header **une seule fois** (champ/cache) et réutiliser le `byte[]`. Pur refactor local, pas d'état partagé. | **LOW** | VÉRIFIÉ |
| **H6** | `UtilityExtensions.cs:217` | `using var client = new HttpClient()` créé **par appel** de `GetDocumentPayload` (re-instancié à chaque téléchargement) | **LOW** : anti-pattern HttpClient (socket exhaustion sur runs longs) plus que mémoire pure ; dispose présent mais instanciation répétée | `static readonly HttpClient` partagé (recommandation .NET). **Attention** : changement de durée de vie ⇒ vérifier thread-safety (HttpClient l'est pour les appels). | **LOW** | VÉRIFIÉ |
| **H7** | `HarvestManager.cs:359` + `:411,437,451` | `List<string> consoleMessages` alloué et **passé** à `GenerateImages` mais **jamais écrit dedans** (paramètre mort) | **NÉGLIGEABLE** : allocation vide ; pas un risque mémoire, mais bruit | Supprimer le paramètre mort (nettoyage, hors-scope perf). | **LOW** | VÉRIFIÉ |
| **H8** | `ImageHelper.cs:42-52` | branche SVG embarqué : `MemoryStream` en `using` (OK) mais `new MagickImage(stream, …)` **retourné** depuis l'intérieur du `using (stream)` — l'image survit au stream (Magick copie le contenu), capturée en `using` par l'appelant (#436) | **LOW** : OK fonctionnellement ; le `MemoryStream` est disposé, l'image l'est par l'appelant | **Déjà correct** depuis #436 (appelant en `using`). Aucun fix requis. | — (rien à faire) | VÉRIFIÉ |

---

## 3. Shortlist « safe-to-apply-now » (LOW apply-risk)

Fixes purs `using`/dispose/réutilisation locale, **sans** toucher le lock QuestPDF, le pool Playwright partagé, ni l'architecture de buffering. À décider/actionner dans une étape **séparée et délibérée** (cette investigation n'applique rien).

1. **`PrintAndPlayDocument.cs:101`** — lire l'image de header **une fois** (champ mis en cache dans le ctor ou lazy `Lazy<byte[]>`) au lieu de `File.ReadAllBytes` à chaque `ComposePage`. *Bénéfice : supprime N re-lectures du header (N = nb pages).*
2. **`UtilityExtensions.cs:217`** — remplacer `using var client = new HttpClient()` par un `static readonly HttpClient` partagé (pattern .NET recommandé). *Bénéfice : évite l'épuisement de sockets / la pression GC sur runs avec téléchargements répétés.*
3. **`HarvestManager.cs:359`** — supprimer le paramètre mort `consoleMessages` (jamais écrit). *Bénéfice : nettoyage ; impact mémoire négligeable mais zéro risque.*

> H8 (`ImageHelper.cs:42-52`) est **déjà correct** depuis #436 — listé pour mémoire, **rien à appliquer**.

---

## 4. « Needs deliberate review » (MED / HIGH apply-risk)

À **ne pas** traiter comme de simples `using`. Chacun exige soit un changement d'architecture, soit un profiling préalable, soit touche un point thread-sensible documenté (lock QuestPDF, pool de pages, `_browser` statique).

- **H1 — Décodage en masse des `MagickImage` d'assemblage PDF** (`PdfManager.cs:31/52-70/98-107/121`) — **HIGH**.
  *Probable pire offender résiduel au pic.* Toutes les cartes d'un document sont décodées plein-résolution dans une `List<MagickImage>`/`MagickImageCollection` **avant** `Write`. #436 dispose la collection **après** ; il ne réduit **pas** le pic pendant l'assemblage. Fix = streaming carte-par-carte ou batch borné ⇒ change la logique de `GeneratePdfsFromImages` et des 4 builders. **Profiler d'abord** (confirmer que c'est bien le pic) avant tout refactor.

- **H2 — Préchargement total des octets Print&Play** (`PdfManager.cs:154-155` ; `PrintAndPlayDocument.cs:16-17,61-66`) — **MED**.
  `frontImagesData` + `backImagesData` = tous les `byte[]` de toutes les cartes en RAM simultanément, vivants pendant tout le rendu. En Release c'est du **PNG lossless** (pas de réduction JPEG). Fix = lecture lazy page-par-page ou libération après composition ⇒ touche le contrat `PrintAndPlayDocument` (IDocument QuestPDF). À cadrer avec le lock global QuestPDF.

- **H3 — Pool de pages Playwright jamais purgé** (`HarvestManager.cs:327,349-352,329-347`) — **MED**.
  Les pages sont `Push`-ées, jamais `Close`-ées en cours de run ; libérées seulement à la fermeture du `_browser` **statique** (`DisposeAsync`, `:21-28`). Touche un état **partagé entre instances** (`_browser` static) et le parallélisme harvest. Fix = fermer les pages au-delà du cap, ou purge avant la phase image — **avec soin** (concurrence, `_browser` partagé, `KeepBrowserOpen`).

- **H4 — Matérialisation `.ToList()` + conversion JPEG transitoire** (`PdfManager.cs:154-155,177-191`) — **MED**.
  Le `using` interne de `ConvertToJpeg` est correct ; le risque est le `.Select(...).ToList()` qui matérialise toutes les conversions d'un coup (corollaire de H2). À traiter conjointement avec H2 (lazy), pas isolément.

---

## 5. Cartographie du flux (rappel, pour cibler le profiling)

```
HarvestImages()  ──> harvestDictionary : Func<CardSetHarvest>   (LAZY, recharge disque — PAS de pic RAM ici)   VÉRIFIÉ
   HarvestManager.cs:72-97 / 127,159                               (data-URLs base64 sur DISQUE, pas tous en RAM)
        │
        ▼
GenerateDocumentImages()  ──> décode chaque data-URL en MagickImage, resize/CMYK, écrit le PNG/JPEG fichier
   ImageFileGenerator.cs:30-111 ; ImageHelper.cs:106 (using #436)  (1 image à la fois par thread — OK depuis #436)   VÉRIFIÉ
        │  parallélisme : MaxDegreeOfParallelismImageTranslations=2
        ▼
GenerateCardSetDocuments()  ──> lock global QuestPDF ; assemble le PDF
   WebBasedGenerator.cs:96-164 (Parallel.ForEach + lock pdfLock:111)
        ├─ Tarot : PdfManager.GenerateFacesOnly / AlternateFaceAndBack / BackFirstOneDocPerBack
        │      ▼ H1 : List<MagickImage> de TOUT le doc décodé avant Write   ◀── pic probable (HIGH)
        │        collection disposée APRÈS write (using #436) — ne baisse pas le pic d'assemblage
        └─ Print&Play : PdfManager.GeneratePrintAndPlay
               ▼ H2 : List<byte[]> recto+verso TOTAL préchargé   ◀── pic probable (MED, PNG lossless en Release)
```

**Points thread-sensibles à respecter dans tout fix** (RAPPORTÉ, CLAUDE.md) :
- **QuestPDF n'est pas thread-safe** → `lock (pdfLock)` global (`WebBasedGenerator.cs:111`). Un fix de streaming PDF doit rester **dans** ce lock.
- **`_browser` est statique** et partagé (`HarvestManager.cs:36`) ; le pool `Freepages` est par-instance (`:327`). Toucher H3 = raisonner concurrence + cycle de vie statique.
- **`Logger`** est thread-safe (locks) — pas un hotspot mémoire.

---

## 6. Limites & prochaine étape recommandée

- **Aucun profiling exécuté.** Les scale-risk H1/H2/H3 sont des **hypothèses statiques** ; le « ~1,2 GB » est **RAPPORTÉ** (commit #436), non re-mesuré. Toute décision de refactor MED/HIGH doit être **précédée** d'un `dotnet-trace`/dump sur un run **Release 1 langue** pour confirmer le pire offender (cf. next-step autorisé du triage #29, po-2024 2026-06-03).
- **Ordre suggéré** (si décision d'actionner) : (1) appliquer la shortlist LOW §3 (sûr, faible gain mais zéro risque) → (2) profiler Release 1 langue → (3) cibler H1 **ou** H2 selon le pic mesuré, en PR isolée avec ACK ai-01 et tests de non-régression (build + xUnit + visual).
- **Ne pas** régresser le bénéfice #436 : tout refactor de `GeneratePdfsFromImages` doit **conserver** le `using` de la collection.

---

*Investigation DOC-ONLY. Aucune ligne de code/CSV/config modifiée. Source de vérité = code sur master `d411704a` + diff #436.*
