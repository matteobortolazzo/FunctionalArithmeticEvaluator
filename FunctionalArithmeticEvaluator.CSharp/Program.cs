using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FunctionalArithmeticEvaluator.CSharp
{
    public class LexerBenchmarks
    {
        private const string _input = "30 - 10 * 5 + 5 % 5";
        private const int _n = 100000;

        [Benchmark]
        public void Tokenize()
        {
            for (var i = 0; i < _n; i++)
            {
                _input.Tokenize();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<LexerBenchmarks>();
        }
    }
}
