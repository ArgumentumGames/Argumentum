# Item 5 — Sitemap préprod vide : le levier est la rangée portail `ContentLocalizationEnabled` (mesuré 25/10, restauré)

Pool #458, item 5 : « trouver en préprod le plus petit réglage qui rend des `<url>` sous
`ContentLocalizationEnabled=True` (mesure, puis geste préprod si réversible). Le résultat
conditionne l'étape go-live "activer les 7 langues". Avant et après, compter les `<loc>`
contre la baseline prod (18, web2). »

**Verdict : le plus petit réglage est la rangée `PortalSettings` (portail 1)
`ContentLocalizationEnabled` elle-même.** Une rangée, réversible, effet mesuré :
sitemap 0 → 18 `<loc>` (= baseline prod exactement), puis restauré à `True`
(0 `<loc>` à nouveau, re-mesuré). Non-régression 16 routes verte dans **les deux** états.

## 1. Mécanisme (source exacte du binaire servi — pas mémoire, pas master)

`DotNetNuke.dll` préprod = 10.3.2, `ProductVersion 10.3.2+Branch.master.Sha.4b0214b8…`.
Le fichier `CoreSitemapProvider.cs` a été relu **à ce SHA précis** (il diffère de master —
10 348 vs 11 933 octets ; master ne sert pas de référence ici). Chaîne d'appel mesurée sur
la même source : `web.config:913-918` (`<sitemap defaultProvider="coreSitemapProvider">`,
type `DotNetNuke.Services.Sitemap.CoreSitemapProvider, DotNetNuke`) → `SitemapHandler`
(`SiteMap.aspx`) → `SitemapBuilder.BuildSiteMap` → **une seule** invocation
`provider.GetUrls(portalId, PortalSettings.Current, "0.9")` — pas d'itération par culture
au niveau du builder ; le cache est par culture (`sitemap.<culture>.xml` sous
`Portals/1-System/Sitemap/`, validité `SitemapCacheDays=1`).

Le filtre décisif — `CoreSitemapProvider.GetUrls`, lignes 48-50 (source @ `4b0214b8`) :

```csharp
var tabs = TabController.Instance.GetTabsByPortal(portalId).Values
            .Where(t => (!t.IsSystem
                        && !ps.ContentLocalizationEnabled)                       // branche A
                || (languagePublished && t.CultureCode.Equals(currentLanguage))); // branche B
```

- Sous `CL=True` : la branche A est fermée pour toute la table ; la branche B n'accepte
  que les pages **codées culture**. Arbre préprod (mesuré 25/10, `Tabs` portail 1,
  non supprimées) : **29 pages `CultureCode` NULL** (22 visibles) + **1 page fr-FR
  invisible** ⇒ 0 URL pour toute culture. Ce n'est pas un stalage de cache : c'est le
  calcul courant.
- Sous `CL=False` : la branche A ouvre toutes les pages non-système ; les filtres internes
  (`IsDeleted`, `DisableLink`, `TabType.Normal`, dates, `IsTabPublic`,
  `includeHiddenPages` **par défaut True** — pas de rangée `SitemapIncludeHidden`,
  `HasBeenPublished`, `AllowIndex`) laissent passer **18 URL**.

La baseline prod (`sitemap_prod.xml`, web2) : 18 `<loc>`, URL neutres sans préfixe
culture, aucun hreflang (l'émission d'alternates xhtml est **commentée** dans
`SitemapBuilder.AddUrl` — le sitemap DNN 10.3.2 n'émet jamais de hreflang). L'arbre
préprod étant l'import de l'arbre prod, les 18 URL préprod sous `False` = les 18 prod
(mêmes slugs ; une casse diffère par culture sur `actus`/`Actus`).

## 2. Protocole et mesures (toutes les valeurs relevées)

Annonce `[INFO]` dashboard AVANT le geste. Backup de la valeur au scratchpad
(`sm5_rollback.json`, jamais commis). Chaîne : UPDATE 1 rangée → readback → suppression
des 2 fichiers cache **nommés** (`sitemap.{fr-fr,en-us}.xml` — le cache du jour rejouait
sinon l'urlset vide) → recycle (touch `web.config` mtime, sha256 invariant) → warm borné.

| État | `ContentLocalizationEnabled` | sitemap fr (`/SiteMap.aspx`) | sitemap en (`/en/SiteMap.aspx`) | 16 routes (2×8 cultures) | `<html lang>` distincts |
|---|---|---|---|---|---|
| Avant | `True` | **0** `<loc>` (320 o) | 0 `<loc>` | 16/16 verts | 8 |
| Geste | `False` | **18** `<loc>` (3 729 o) | 18 `<loc>` | **16/16 verts** | **8** |
| Restauré | `True` | **0** `<loc>` (320 o) — preuve que la restauration prend effet | — | 16/16 verts | 8 |

Artefacts (scratchpad, non commis) : `sitemap_item5_pre.xml` (320 o),
`sitemap_item5_fr_false.xml` / `sitemap_item5_en_false.xml` (3 729 o),
`sitemap_item5_post_restore.xml` (320 o), `sm5_flip_false.ps1`, `sm5_measure.ps1`,
`sm5_restore_true.ps1`, `sm5_rollback.json`.

## 3. La servitude culture ne dépend PAS du drapeau portail

Sous `False`, les 8 cultures restent servies (16/16 routes 200, `news5=5` sur les 8,
`Error Showing Content`=0, 8 `<html lang>` distincts) : la résolution culture repose sur
les rangées `PortalAlias` culturelles (`/en` `/es` `/pt` `/ru` `/zh` `/ar` `/fa`), pas sur
la rangée portail `ContentLocalizationEnabled`. Le drapeau du 11/10 ne fait pas tourner
le site multilingue tel que construit — il ne ferme que la branche A du sitemap
(et les alternates de `GetPageUrl`).

## 4. ⚠️ Interaction mesurée : sitemap peuplé XOR hreflang du skin (#1989)

Sous `False`, le sitemap `/en` rend des URL **neutres** (aucun préfixe `/en/…`) :
`NavigateURL(tab, culture)` sur une page neutre ignore la culture quand `CL=False`.
Or l'émetteur hreflang du skin (#1989, mesuré 9/9 sous `True` avec préfixes
`/en/Actus`…) construit ses alternates **par ce même appel**
`NavigateURL(tabId, IsSuperTab, portal, "", culture)`. Sous `False` ses 8 alternates
pointeraient toutes vers l'URL neutre — hreflang cassé silencieusement.

**Le go-live « activer les 7 langues » hérite donc d'un choix exclusif, à arbitrer :**

- **a) `CL=True`** : sitemap vide (0 `<loc>`) + hreflang porté par le skin #1989.
- **b) `CL=False`** : sitemap peuplé (18 = prod) mais l'émetteur #1989 doit construire
  ses URL autrement que par `NavigateURL(…, culture)` (préfixe d'alias explicite), sinon
  hreflang dégénéré.
- **c)** Modèle pages localisées (`CultureCode` par page) : rendrait la branche B
  non vide sous `True` — 29 rangées à créer + revalidation complète, hors « plus petit
  réglage ».

L'option b) ne casse rien de mesuré aujourd'hui (servitude 8 cultures verte), mais
l'émetteur non plus n'est pas déployé (mesuré puis restauré, #1989) : les deux volets
sont en attente du même arbitrage go-live. **Aucun des deux états n'est perdu** — la
bascule est une rangée, mesurée dans les deux sens.

## 5. Pièges d'instrument rencontrés

- **Cache sitemap du jour** : `CacheIsValid()` rejoue `sitemap.<culture>.xml` tant qu'il
  a < 1 j — toute re-mesure après un geste DOIT supprimer les fichiers nommés d'abord,
  sinon elle relit l'ancien état (le nôtre datait de 16:08/16:13 le jour même).
- **`GetTypes()` sous pwsh 7** : `System.Web` n'existe pas en .NET Core — la réflexion
  sur `DotNetNuke.dll` se fait sous `powershell.exe` 5.1 (`-File`, pas de here-string
  inline dont le quoting se mange).
- **Source à SHA, pas à branche** : master de Dnn.Platform diffère du binaire
  (10 348 ≠ 11 933 octets) ; `ProductVersion` porte le SHA exact (`4b0214b8`) — lire à
  ce ref.
- L'unique page fr-FR codée culture (invisible) n'apparaît pas dans les 18 : filtrée en
  aval (non publique ou non publiée) — les 18 préprod = les 18 prod, entrée pour entrée.
