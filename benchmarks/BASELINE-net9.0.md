# Baseline .NET 9 — bancs BenchmarkDotNet Argumentum (AssetConverter)

Baseline de référence pour la mesure des gains runtime .NET 10/11 (issue CoursIA #18770, fille de l'EPIC #18695). Banc : `Generation/Converters/Argumentum.AssetConverter.Benchmarks` (BenchmarkDotNet 0.14.0), exerçant l'**API publique** du convertisseur sur les hot paths identifiés par le digest .NET 11 : parsing de règles multilingues (CsvHelper) et timestamps `DateTime.Now`.

## Environnement de mesure

| Élément | Valeur |
| --- | --- |
| Commit mesuré | `da8a49a2` (pointe de `master`) |
| Runtime | .NET 9.0.20 (9.0.2026.41315), X64 RyuJIT AVX2 |
| SDK | 10.0.204 |
| BenchmarkDotNet | 0.14.0, InProcessEmitToolchain, Job.ShortRun 5 warmup / 15 itérations |
| Machine | Windows 11 Pro 10.0.26300 (poste de travail, GC client) |

Le convertisseur cible déjà `net9.0-windows` : la baseline est donc le TFM natif du projet, sans translation de framework.

## Résultats (net9.0.20)

| Benchmark | Mean | Error | Allocated |
| --- | ---: | ---: | ---: |
| `Rule.LoadFromContent` (50 règles multilingues) | 1 746,2 us | 107,7 us (6,2 %) | 164,05 KB |
| `Rule.LoadFromContent` (500 règles multilingues) | 4 716,2 us | 351,3 us (7,4 %) | 854,09 KB |
| Timestamping `DateTime.Now` + `yyyyMMdd-HHmmss` ×1000 (motif Logger/Archive) | 288,9 us | 29,1 us (10,1 %) | 57,66 KB |

Charge : règles multilingues réalistes (français, anglais, russe, portugais, espagnol, arabe, farsi, chinois) — texte non-ASCII traversant CsvHelper et la normalisation de diacritiques.

## Lecture des hot paths

- **Parsing de règles** : le coût par appel est dominé par une composante fixe (initialisation CsvHelper + enregistrement du ClassMap + un append fichier verrouillé dans `Logger.Log` en fin d'appel, comportement propre à l'API `LoadFromContent`). De 50 à 500 règles, le temps ne fait ×2,7 et l'allocation par règle passe de ~3,3 KB à ~1,7 KB : la composante fixe amortit, la marge runtime .NET 10/11 porte surtout sur les allocations du chemin CsvHelper (records `string` par colonne).
- **Timestamping** : ~289 ns par itération `DateTime.Now.ToString("yyyyMMdd-HHmmss")` — le motif exact de `Logger.ArchivePreviousLog`. C'est la cible ×2,2 annoncée par le digest .NET 11 sur `DateTime.Now` ; la part `ToString` (allocation de la chaîne formatée) bénéficiera en sus des améliorations du formateur.

## Contexte de fidélité pin/pointe

Le submodule CoursIA pointe `053257c7` (en retard de plusieurs centaines de commits sur `master`, l'écart portant surtout sur le contenu web/cartes ; la surface C# du convertisseur a aussi bougé — p. ex. `Argumentum.CsvValidator` présent au pin a été retiré depuis). Le banc suit l'API de la pointe `da8a49a2` (là où la PR merge) ; la mesure est reproductible au commit exact.

## Protocole de reproduction

```bash
dotnet build Generation/Converters/Argumentum.AssetConverter.Benchmarks -c Release
cd Generation/Converters/Argumentum.AssetConverter.Benchmarks/bin/Release/net9.0-windows
./Argumentum.AssetConverter.Benchmarks.exe -f "*"
```

La config `InProcessShortConfig` (in-process, 5 warmup / 15 itérations) est attachée par attribut — elle évite le blocage du processus enfant par Defender/antivirus sur Windows (exit -2147450730). Le GlobalSetup redirige `Logger.LogFile` vers `%TEMP%` et coupe `Logger.LogInfo` : la sortie console est silencieuse et l'append fichier (une fois par appel, comportement natif de l'API) reste mesuré tel quel.

## Limites

- Poste de travail unique (pas de multi-machine) ; ordres de grandeur et ratios allocation/temps, pas des valeurs absolues de référence hardware.
- `MinIterationTime` : minimums d'itération courts (1,6 ms / 4,3 ms / 267 us) — erreurs relatives 6-10 %, suffisantes pour des ratios ; augmenter les itérations si un point devient discriminant.
- Les charges sont des constantes déterministes (pas de graine aléatoire nécessaire).
