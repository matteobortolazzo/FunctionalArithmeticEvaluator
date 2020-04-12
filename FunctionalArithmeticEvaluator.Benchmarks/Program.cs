using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FunctionalArithmeticEvaluator.CSharp;
using Microsoft.FSharp.Collections;

namespace FunctionalArithmeticEvaluator.Benchmarks
{
    public class Lexers
    {
        [Params(
            "",
            "30 - 10",
            "30 - 10 * 5 - 20 / 4 % 2",
            "30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40",
            "30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40 / 4.55 + 2",
            "30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40 / 4.55 + 2 + 30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40 / 4.55 + 2 + 30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40 / 4.55 + 2 + 30 - 10 * 5 - 20 / 4 % 2 - 4 * 10 + 7 * 40 / 4.55 + 2"
            )]
        public string Input { get; set; }


        [Benchmark]
        public FSharpList<CSharpLexer.Token> CSharp() => CSharpLexer.Tokenize(Input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharp() => FSharpLexer.tokenize(Input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharpTailRec() => FSharpLexer.tailTokenize(Input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharpTailRecChars() => FSharpLexer.tailTokenizeChar(Input);
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<Lexers>();
        }
    }
}
