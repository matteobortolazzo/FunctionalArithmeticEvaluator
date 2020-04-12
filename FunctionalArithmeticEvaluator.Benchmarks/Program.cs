using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FunctionalArithmeticEvaluator.CSharp;
using Microsoft.FSharp.Collections;

namespace FunctionalArithmeticEvaluator.Benchmarks
{
    public class Lexers
    {
        private const string _input = "30 - 10 * 5 - 20 / 4 % 2";

        [Benchmark]
        public FSharpList<CSharpLexer.Token> CSharp() => CSharpLexer.Tokenize(_input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharp() => FSharpLexer.tokenize(_input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharpTailRec() => FSharpLexer.tailTokenize(_input);

        [Benchmark]
        public FSharpList<FSharpLexer.Token> FSharpTailRecChars() => FSharpLexer.tailTokenizeChar(_input);
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            _ = BenchmarkRunner.Run<Lexers>();
        }
    }
}
