using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using System.Collections.Generic;
using System.Linq;

namespace AbcArbitrage.Homework.Configs
{
    public class BenchmarkConfig
    {
        /// <summary>
        /// Get default Benchmark config
        /// </summary>
        /// <returns>IConfig</returns>
        public static IConfig GetDefault()
        {
            return DefaultConfig.Instance;
        }

        /// <summary>
        /// Get constom Benchmark config
        /// </summary>
        /// <returns>IConfig</returns>
        public static IConfig GetConstom()
        {
            var job = new Job(Job.Default)
                .WithUnrollFactor(20) //number of time to invok the bench methon per iteration.
                .WithToolchain(InProcessEmitToolchain.Instance); //To avoid a conflict with the system anti-virus.
            return ManualConfig.CreateEmpty()
                //Jobs
                .AddJob(
                    job
                    .WithId("j-1")
                    .AsBaseline()
                    .WithRuntime(CoreRuntime.Core80)
                    .WithPlatform(Platform.X64)
                )
                .AddJob(
                    job
                    .WithId("j-2")
                    .WithRuntime(CoreRuntime.Core90)
                    .WithPlatform(Platform.X64)
                )
                //Diagnoser and output config
                .AddDiagnoser(MemoryDiagnoser.Default)
                .AddColumnProvider(DefaultColumnProviders.Instance)
                .AddLogger(ConsoleLogger.Default)
                .AddExporter(HtmlExporter.Default)
                .AddExporter(CsvExporter.Default)
                .AddAnalyser(GetAnalysers().ToArray());
        }

        /// <summary>
        /// Get analyse for the custom config
        /// </summary>
        /// <returns>IEnumerable<IAnalyser></returns>
        private static IEnumerable<IAnalyser> GetAnalysers()
        {
            yield return EnvironmentAnalyser.Default;
            yield return OutliersAnalyser.Default;
            yield return MinIterationTimeAnalyser.Default;
            yield return MultimodalDistributionAnalyzer.Default;
            yield return RuntimeErrorAnalyser.Default;
            yield return ZeroMeasurementAnalyser.Default;
            yield return BaselineCustomAnalyzer.Default;
        }
    }
}
