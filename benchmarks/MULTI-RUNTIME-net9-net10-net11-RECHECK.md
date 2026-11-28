# Contrôle du delta .NET 9 / 10 / 11-rc.3 — carré latin, 18 jambes espacées (issue CoursIA #18770)

Contrôle du document [MULTI-RUNTIME-net9-net10-net11.md](MULTI-RUNTIME-net9-net10-net11.md), qui concluait lui-même :

> « La tendance timestamping mérite des jambes espacées et répétées avant conclusion. »

Ce document répond à cette demande. Il ne rejoue pas les trois jambes d'origine : il rejoue **18 jambes** (6 répétitions × 3 runtimes) avec un **ordre de passage équilibré**, des **jambes espacées**, une **jambe de warm-up écartée**, et une lecture par **séparabilité** plutôt que par moyenne.

**Verdict en une phrase : la suite monotone 9 → 10 → 11 n'est pas reproduite.** net11 est **séparable** de net9 et de net10 sur le timestamping ; **net10 n'est pas séparable de net9** (les intervalles se recouvrent). Sur les deux axes de parsing, aucun runtime n'est séparable des deux autres. Les allocations sont identiques à moins de 1 KB près sur les trois runtimes — l'étendue intra-runtime étant elle-même de cet ordre.

## Ce que le protocole d'origine laissait en libre

Le document d'origine enchaîne **3 jambes dans un ordre fixe** (net9, puis net10, puis net11). Dans un ordre fixe, la position est **parfaitement confondue** avec le runtime : net9 est toujours en 1re position, net10 toujours en 2e, net11 toujours en 3e. Toute dérive de la machine d'une position à l'autre (état thermique, cache, charge de fond) est alors indiscernable d'un effet de runtime.

Le biais n'est pas hypothétique : **il est mesuré ici, sur ce banc**. Les moyennes par position de l'axe timestamping s'étalent de **238,3 µs** (1re position) à **330,2 µs** (3e position) pour un même runtime — soit **+38,6 %** d'écart entre positions, du même ordre de grandeur que l'écart net9 → net10 lui-même (**+6,9 %** en moyenne, et non séparable).

C'est pourquoi une réplication « 9,10,11 ×3 » peut lire un **net10 plus lent que net9** (moyennes observées lors d'une passe preliminaire : net9 217,9 / net10 264,7 / net11 119,9 µs) alors que l'ordre équilibré montre que **net10 et net9 sont indiscernables**. L'artefact de l'ordre, pas un effet de runtime.

## Environnement de mesure

| Élément | Valeur |
| --- | --- |
| Commit mesuré | `7efc9bdf` (branche `feature/benchmarkdotnet-multiruntime`, PR #2054) |
| Binaire | `net9.0-windows` unique, mesuré sous chaque runtime par roll-forward |
| Runtimes | .NET **9.0.22** (9.0.2226.52218) · **10.0.14** (10.0.1426.52326) · **11.0.0-rc.3** (11.0.26.52348), X64 RyuJIT x86-64-v3 |
| SDK | 10.0.114 (build ; aucun SDK requis pour la mesure) |
| BenchmarkDotNet | 0.15.8, InProcessEmitToolchain, Job.ShortRun 5 warmup / 15 itérations, InvocationCount 1, UnrollFactor 1 |
| Machine | **Intel Core i9-14900KF** 3,20 GHz, Windows 11 (10.0.26300.9711), GC Workstation |
| Écart de machine | le document d'origine mesurait sur un **i7-13650HX** — les valeurs absolues ne sont **pas comparables** entre les deux documents ; seules les comparaisons **internes** à chaque document le sont |

Les trois murs du document d'origine (NU1201 sur `--runtimes net10.0`, process enfant ignorant `DOTNET_ROOT`, `DotNetSdkValidator` de BDN 0.15.8 sur un SDK 11-rc) sont inchangés et toujours valables : la recette retenue reste **jambes InProcess**, **apphost lancé directement**, runtime **épinglé par l'environnement**.

## Protocole

1. **Carré latin** — trois ordres cycliques, répétés deux fois :

   | Répétition | 1re position | 2e position | 3e position |
   | --- | --- | --- | --- |
   | 1 | net9 | net10 | net11 |
   | 2 | net10 | net11 | net9 |
   | 3 | net11 | net9 | net10 |
   | 4 | net9 | net10 | net11 |
   | 5 | net10 | net11 | net9 |
   | 6 | net11 | net9 | net10 |

   Chaque runtime occupe **chaque position exactement deux fois** : le biais de position se moyenne à travers les six répétitions.

2. **Jambes espacées** — 60 s entre deux jambes consécutives : une jambe ne doit pas être mesurée sur l'état laissé par la précédente.

3. **Une jambe de warm-up écartée** (`net9`) avant la première jambe mesurée.

4. **Attestation** — la ligne `[Host]` du rapport BDN de chaque jambe est lue et comparée au runtime attendu : **18 jambes, 0 écart d'attestation**.

5. **Lecture par séparabilité** — deux groupes ne sont distinguables que si `max(groupe A) < min(groupe B)`. Deux groupes qui se recouvrent ne sont **pas** séparés : c'est un constat d'**absence de séparation**, pas une preuve d'égalité.

## Résultats — axe timestamping (`DateTime.Now` + `yyyyMMdd-HHmmss` ×1000)

Six valeurs par runtime, en µs :

| Runtime | 6 valeurs (triées) | moyenne | médiane | min – max |
| --- | --- | ---: | ---: | --- |
| net9.0.22 | 171,8 · 172,5 · 221,8 · 323,7 · 366,7 · 384,4 | 273,5 | 272,8 | 171,8 – 384,4 |
| net10.0.14 | 222,7 · 253,9 · 281,5 · 298,8 · 336,0 · 361,7 | 292,4 | 290,1 | 222,7 – 361,7 |
| net11.0.0-rc.3 | 99,7 · 110,7 · 116,5 · 120,2 · 127,8 · 152,6 | **121,2** | 118,3 | 99,7 – 152,6 |

**Moyennes par position** (le biais de passage, mesuré et non supposé) :

| Runtime | 1re position | 2e position | 3e position |
| --- | ---: | ---: | ---: |
| net9 | 278,4 | 269,2 | 272,8 |
| net10 | **238,3** | 308,8 | **330,2** |
| net11 | 124,0 | 134,6 | 105,2 |

**Séparabilité** :

| Comparaison | Verdict | Motif |
| --- | --- | --- |
| net11 vs net9 | **SÉPARÉS** | max(net11) = 152,6 < min(net9) = 171,8 |
| net11 vs net10 | **SÉPARÉS** | max(net11) = 152,6 < min(net10) = 222,7 |
| net10 vs net9 | **CHEVAUCHEMENT** | 222,7 – 361,7 et 171,8 – 384,4 se recouvrent |

## Résultats — axes de parsing

| Axe | net9.0.22 | net10.0.14 | net11.0.0-rc.3 |
| --- | --- | --- | --- |
| `Rule.LoadFromContent` (50 règles) — moyenne (min – max) | 1 899,6 (1 142,3 – 2 870,8) µs | 2 006,3 (1 324,5 – 2 519,8) µs | 1 870,6 (1 360,3 – 2 240,6) µs |
| `Rule.LoadFromContent` (500 règles) — moyenne (min – max) | 5 124,6 (3 686,1 – 7 137,4) µs | 5 078,6 (4 053,4 – 6 190,2) µs | 4 934,9 (3 872,3 – 6 888,8) µs |
| Allocated (50 / 500 / timestamps) — moyenne | 162,73 / 854,48 / 57,54 KB | 162,66 / 854,62 / 57,38 KB | 162,54 / 854,82 / 57,76 KB |

**Séparabilité** : sur les deux axes de parsing, **aucune paire de runtimes n'est séparable** (les trois intervalles se recouvrent deux à deux). L'amplitude intra-runtime (jusqu'à ×2,5 sur l'axe 50 règles) domine largement l'écart inter-runtime.

## Lecture

- **Le gain net11 sur le timestamping est réel et séparable.** 121,2 µs en moyenne contre 273,5 (net9) et 292,4 (net10), et l'intervalle net11 **ne chevauche aucun** des deux autres : **−55,7 % vs net9**, **−58,5 % vs net10**. C'est la seule affirmation de ce document qui survit au protocole équilibré. Elle ne dit rien sur la taille exacte du gain — l'intervalle net9 s'étale sur un facteur 2,2, la dispersion de ce banc est forte.
- **Le gain net10 sur net9 n'est pas établi.** La suite monotone 565 → 381 → 245 µs du document d'origine **ne se reproduit pas** : net10 et net9 sont indiscernables. L'hypothèse « net10 serait déjà plus rapide que net9 » n'est **pas soutenue** par cette mesure. Le verdict est une **absence de séparation**, pas une preuve d'égalité : un gain net10 plus petit que la dispersion de ce banc resterait indétectable ici.
- **Aucun delta de parsing**, sur aucune paire de runtimes. Cohérent avec le document d'origine.
- **Allocations identiques** à moins de 1 KB près sur les trois runtimes (162,5 – 163,0 KB / 854,3 – 855,0 KB / 56,9 – 57,9 KB, l'étendue étant ici celle **intra**-runtime) : aucun changement de layout ni de comportement GC sur ces chemins. Confirmé sur 18 jambes indépendantes.
- **La variabilité intra-runtime est le fait le plus important du document.** L'axe 50 règles s'étale de 1 142 à 2 871 µs pour un **même** runtime : ×2,5. Toute affirmation d'amplitude sur ce banc doit être exprimée en **séparabilité**, pas en pourcentage de moyenne.

## Protocole de reproduction

Les scripts de ce contrôle sont versionnés à côté :

- [`recheck-multiruntime.ps1`](recheck-multiruntime.ps1) — la matrice équilibrée (18 jambes + warm-up, espacement 60 s, attestation `[Host]` par jambe) ;
- [`recheck-multiruntime-aggregate.py`](recheck-multiruntime-aggregate.py) — l'agrégation (moyennes par position, min/max/médiane, **test de séparabilité**) et le contrôle d'attestation.

```powershell
# 1. build (une fois)
dotnet build Generation/Converters/Argumentum.AssetConverter.Benchmarks -c Release

# 2. matrice equilibree (18 jambes espacees de 60 s, ~25 min)
pwsh -File benchmarks/recheck-multiruntime.ps1 -OutRoot <dossier de sortie>

# 3. agregation + verdict de separabilite
python benchmarks/recheck-multiruntime-aggregate.py <dossier de sortie>
```

Le root 11 isolé doit contenir **NETCore et WindowsDesktop** (le TFM `-windows` exige les deux ; un root incomplet fait retomber le probe silencieusement sur le root système). `DOTNET_ROLL_FORWARD=Major` (et non `LatestMajor`) suffit depuis un root 11-seul.

## Limites

- **Machine unique**, et **différente** de celle du document d'origine (i9-14900KF contre i7-13650HX) : les valeurs absolues ne sont pas comparables d'un document à l'autre ; seules les comparaisons internes le sont.
- **Six jambes par runtime** : le test de séparabilité est un critère de **triomphe** (les groupes ne se recouvrent pas) ; il ne donne pas de puissance statistique. Une absence de séparation à n = 6 avec une dispersion ×2,5 est **attendue** et ne prouve pas l'égalité.
- **11.0.0-rc.3 est une version informative**, pas la GA : à re-mesurer à la sortie.
- **Le roll-forward mesure le même binaire `net9.0-windows`** sous chaque runtime — chemin identique au déploiement, mais ce n'est pas une recompilation TFM net10/net11.
- Le **biais de position n'est pas nul** dans ce banc (±38,6 % d'écart entre positions sur l'axe timestamping) : il est **équilibré**, pas supprimé. Un protocole à **positions aléatoires** réduirait la variance résiduelle, au prix de ne plus garantir l'équilibre exact.
