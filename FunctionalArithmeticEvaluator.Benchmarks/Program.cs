using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FunctionalArithmeticEvaluator.CSharp;
using Microsoft.FSharp.Collections;
using System;
using System.Text;

namespace FunctionalArithmeticEvaluator.Benchmarks
{
    public class Lexers
    {
        [Params(51, 501)]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(DateTime.Now.Millisecond);
            var sb = new StringBuilder();
            for (var i = 0; i < N; i++)
            {                
                if (i % 2 == 0)
                {
                    _ = sb.Append(random.Next(0, 10000).ToString());
                }
                else
                {
                    var opId = random.Next(5);
                    var op = opId switch
                    {
                        0 => "+",
                        1 => "-",
                        2 => "*",
                        3 => "/",
                        _ => "%",
                    };
                    _ = sb.Append(op);
                }
            }
            _input = sb.ToString();
        }

        private string _input;

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
