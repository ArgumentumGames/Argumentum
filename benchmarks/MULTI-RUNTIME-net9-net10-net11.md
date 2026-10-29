# Delta runtime .NET 9 / 10 / 11-rc.3 — même binaire, même machine (issue CoursIA #18770, partie B)

Suite directe de la [baseline net9](BASELINE-net9.0.md) : le **même binaire** `net9.0-windows` est mesuré sous trois runtimes — 9.0.19 (TFM natif), 10.0.11 et 11.0.0-rc.3 (roll-forward) — sur une même machine, protocole identique par jambe. La question posée : le digest .NET 11 (gains annoncés sur `DateTime.Now` ×2,2 et le formateur) se voit-il sur les hot paths du convertisseur ?

## Environnement de mesure

| Élément | Valeur |
| --- | --- |
| Commit mesuré | `a51b0339` (branche de la PR #1705, sur `master`) |
| Binaire | `net9.0-windows` unique, mesuré sous chaque runtime par roll-forward |
| Runtimes | .NET 9.0.19 (9.0.1926.36724) · 10.0.11 (10.0.1126.37416) · 11.0.0-rc.3 (11.0.26.52348), X64 RyuJIT x86-64-v3 |
| SDK | 10.0.400 (build ; aucun SDK requis pour la mesure) |
| BenchmarkDotNet | 0.15.8, InProcessEmitToolchain, Job.ShortRun 5 warmup / 15 itérations |
| Machine | 13th Gen Intel Core i7-13650HX, Windows 11 Home 10.0.26300, GC Workstation |

## Murs mesurés — pourquoi le moniker et le process enfant sont exclus

Le pattern multi-runtime de semantic-fleet (CoursIA #107) ne se transpose pas tel quel sur un TFM `-windows`. Trois murs, chacun mesuré :

1. **NU1201 sur `--runtimes net10.0`** : BDN génère un projet de banc ciblant `net10.0` nu, incompatible avec `net9.0-windows7.0` (`NU1201: Argumentum.AssetConverter.Benchmarks est incompatible avec net10.0`).
2. **Le process enfant ignore `DOTNET_ROOT`** (mesuré deux fois, déterministe) : BDN spawn son enfant via le muxer `dotnet` du `PATH`, qui résout son propre répertoire d'abord — l'enfant reste sur 9.0.19 même quand le probe apphost résout correctement 11-rc.3 par `DOTNET_ROOT`.
3. **BDN 0.15.8 `DotNetSdkValidator` rejette un SDK 11-rc en `PATH`** : `NotImplementedException: GetRuntimeVersion not implemented for NotRecognized` en parsant `11.0.100-rc.3.26522.148` (l'énumération `RuntimeMoniker` plafonne à net10).

**Recette retenue** : jambes InProcess, lancement direct de l'apphost (qui, lui, honore `DOTNET_ROOT` — prouvé par probe), runtime épinglé par environnement. En InProcess le processus hôte EST le processus mesuré : l'attestation est double (probe `--info` ligne `[Host]` + ligne `Runtime=` de la jambe), et les trois jambes l'ont confirmée d'accord. Le commutateur `DOTNET_MULTI_RUNTIME=1` de la config reste la route process enfant (utile net9/net10).

## Résultats (3 jambes enchaînées, même protocole)

| Benchmark | net9.0.19 | net10.0.11 | net11.0.0-rc.3 |
| --- | ---: | ---: | ---: |
| `Rule.LoadFromContent` (50 règles) | 2 698,5 ± 566,5 us | 2 701,9 ± 634,8 us | 2 925,0 ± 670,0 us |
| `Rule.LoadFromContent` (500 règles) | 7 579,2 ± 682,3 us | 6 664,7 ± 1 068,7 us | 7 483,2 ± 718,0 us |
| Timestamping `DateTime.Now` ×1000 | 565,1 ± 174,4 us | 380,8 ± 30,3 us | 244,6 ± 29,5 us |
| Allocated (50 / 500 / timestamps) | 162,5 / 854,6 / 57,6 KB | 162,8 / 854,8 / 57,6 KB | 162,7 / 855,0 / 57,9 KB |

## Lecture

- **Parsing de règles : aucun delta runtime significatif.** Les trois jambes se chevauchent largement (erreurs ±9 à ±23 %, jambes de ~6 s enchaînées) ; net11 n'est ni plus rapide ni plus lent de façon séparable du bruit. Un jeu de contrôle en process enfant (protocole 5/15, jambes ~100 s) mesurait net9 = 2 919,5 ± 155,6 / 5 046,5 ± 225,8 us et net10 = 3 070,7 ± 153,1 / 5 079,3 ± 185,2 us — plat lui aussi.
- **Allocations identiques** au dixième de KB près sur les trois runtimes : aucun changement de layout ou de comportement GC sur ces chemins.
- **Timestamping : tendance non concluable.** La suite 565 → 381 → 245 us ressemble au gain annoncé par le digest, mais la jambe net9 porte ±31 % et le contrôle process enfant mesure net9 ≈ net10 ≈ 302-308 us — la tendance n'est pas stable d'un protocole à l'autre. net11 à 244,6 ± 29,5 us passe sous les deux mesures de contrôle ; suggestif, à re-mesurer (jambes espacées, plusieurs répétitions) avant toute affirmation.
- **Effet protocole mesuré** : 500 règles = 5 046 us (process enfant, jambes espacées) vs 7 579 us (InProcess enchaînées) sur net9 — les comparaisons ne se font qu'au sein d'un même protocole ; c'est le cas du tableau ci-dessus.

## Protocole de reproduction

```powershell
dotnet build Generation/Converters/Argumentum.AssetConverter.Benchmarks -c Release
$exe = "Generation/Converters/Argumentum.AssetConverter.Benchmarks/bin/Release/net9.0-windows/Argumentum.AssetConverter.Benchmarks.exe"

# Jambe net9 (runtime système par defaut)
& $exe --info   # ligne [Host] = runtime resolu, doit lire .NET 9.0.x
& $exe -f "*"

# Jambe net10 (roll-forward majeur sur le root systeme)
$env:DOTNET_ROLL_FORWARD = "LatestMajor"; Remove-Item Env:DOTNET_ROOT -ErrorAction SilentlyContinue
& $exe --info   # doit lire .NET 10.0.x
& $exe -f "*"

# Jambe net11 (root isole : Microsoft.NETCore.App + Microsoft.WindowsDesktop.App 11.0.0-rc.3)
$env:DOTNET_ROOT = "D:\dev\tools\dotnet11rc"; $env:DOTNET_ROLL_FORWARD = "Major"
& $exe --info   # doit lire .NET 11.0.0 (11.0.0-rc.3...)
& $exe -f "*"
```

Le root 11 isolé doit contenir **NETCore et WindowsDesktop** (le TFM `-windows` exige les deux ; un root incomplet fait retomber le probe silencieusement sur le root système). `DOTNET_ROLL_FORWARD=Major` (et non `LatestMajor`) suffit depuis un root 11-seul.

## Limites

- Machine unique, jambes uniques enchaînées : erreurs ±9 à ±31 % — ce document tranche des **écarts** runtime, pas des valeurs absolues (la baseline net9 de référence reste [BASELINE-net9.0.md](BASELINE-net9.0.md)).
- 11.0.0-rc.3 est une version informative, pas la GA : à re-mesurer à la sortie.
- Le roll-forward mesure le même binaire `net9.0-windows` sous chaque runtime — c'est le chemin identique au déploiement, mais pas une recompilation TFM net10/net11.
- La tendance timestamping mérite des jambes espacées et répétées avant conclusion.
