using System;
using AbcArbitrage.Homework.Configs;
using BenchmarkDotNet.Running;

namespace AbcArbitrage.Homework
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, BenchmarkConfig.GetConstom());

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
