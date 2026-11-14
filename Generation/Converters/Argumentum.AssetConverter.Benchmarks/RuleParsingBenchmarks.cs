using System;
using System.Globalization;
using System.IO;
using System.Text;
using Argumentum.AssetConverter.Entities;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace Argumentum.AssetConverter.Benchmarks;

/// <summary>
/// Benchmarks of the converter public API hot paths: multilingual rule CSV parsing
/// (CsvHelper path through CsvBase.LoadFromContent) and the timestamping pattern
/// used by the logger (DateTime.Now + format, the .NET 11 digest x2.2 target).
/// </summary>
[MemoryDiagnoser]
[Config(typeof(InProcessShortConfig))]
public class RuleParsingBenchmarks
{
    private const string Header = "pk,Text,Text_en,Text_ru,Text_pt,Text_es,Text_ar,Text_fa,Text_zh,PrintAndPlay";

    private const string RowFr =
        "Rules_{0:D2},« Le joueur actif peut jouer une carte Argument ; l'adversaire répond avec une Fallacy. »,"
        + "\"The active player may play an Argument card; the opponent answers with a Fallacy.\","
        + "\"Активный игрок может разыграть карту Аргумент; противник отвечает Клеветой.\","
        + "\"O jogador ativo pode jogar uma carta de Argumento; o oponente responde com uma Falácia.\","
        + "\"El jugador activo puede jugar una carta de Argumento; el oponente responde con una Falacia.\","
        + "\"يجوز للاعب النشط لعب بطاقة حجة؛ ويرد الخصم ببطاقة مغالطة.\","
        + "\"بازیکن فعال می‌تواند کارت استدلال را بازی کند؛ حریف با کارت مغالطه پاسخ می‌دهد.\","
        + "\"活跃玩家可以打出一张论点牌；对手用谬误牌回应。\",yes";

    private string _csv50 = "";
    private string _csv500 = "";

    [GlobalSetup]
    public void Setup()
    {
        // Redirect the converter's own file logger away from the repo tree and silence its
        // console output: LoadFromContent logs "Loaded N items" once per call (one locked
        // file append, documented as part of the measured API behavior).
        Logger.LogFile = Path.Combine(Path.GetTempPath(), "arg-bench-logger.log");
        Logger.LogInfo = false;

        _csv50 = BuildCsv(50);
        _csv500 = BuildCsv(500);
    }

    private static string BuildCsv(int rowCount)
    {
        var builder = new StringBuilder(Header.Length + (RowFr.Length * rowCount));
        builder.AppendLine(Header);
        for (int i = 0; i < rowCount; i++)
        {
            builder.AppendFormat(CultureInfo.InvariantCulture, RowFr, i + 1);
            builder.AppendLine();
        }

        return builder.ToString();
    }

    [Benchmark(Baseline = true, Description = "Rule.LoadFromContent (50 règles multilingues)")]
    public System.Collections.Generic.IList<Rule> Rule_LoadFromContent_50Rows() => Rule.LoadFromContent(_csv50);

    [Benchmark(Description = "Rule.LoadFromContent (500 règles multilingues)")]
    public System.Collections.Generic.IList<Rule> Rule_LoadFromContent_500Rows() => Rule.LoadFromContent(_csv500);

    [Benchmark(Description = "Timestamping DateTime.Now + yyyyMMdd-HHmmss x1000 (Logger/Archive pattern)")]
    public string DateTimeNow_Timestamping()
    {
        string last = "";
        for (int i = 0; i < 1000; i++)
        {
            last = DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
        }

        return last;
    }
}

/// <summary>
/// In-process config for Windows machines where the out-of-process benchmark child process
/// gets blocked by antivirus/Defender (exit code -2147450730). ShortRun job with enough
/// iterations to keep the relative error in the 10-25% range.
/// </summary>
public class InProcessShortConfig : ManualConfig
{
    public InProcessShortConfig()
    {
        AddJob(Job.ShortRun
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithInvocationCount(1)
            .WithUnrollFactor(1)
            .WithWarmupCount(5)
            .WithIterationCount(15));
    }
}
