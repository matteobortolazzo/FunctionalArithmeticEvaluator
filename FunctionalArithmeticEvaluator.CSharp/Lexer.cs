using Microsoft.FSharp.Collections;
using System.Diagnostics;

namespace FunctionalArithmeticEvaluator.CSharp
{    
    public static class CSharpLexer
    {
        public enum TokenType
        {
            Operation,
            Float
        }

        [DebuggerDisplay("{Value}")]
        public class Token
        {
            public Token(TokenType type, string value)
            {
                Type = type;
                Value = value;
            }

            public TokenType Type { get; }
            public string Value { get; }
        }

        private const string _operations = "+-*/%";
        private const string _numbers = "1234567890.";
        private static bool IsOperation(this char c) => _operations.Contains(c);
        private static bool IsNumber(this char c) => _numbers.Contains(c);

        private static int GetFloatLenght(this string input, int lenght)
        {
            if (input.Length == 0 || !input[0].IsNumber())
            {
                return lenght;
            }

            return GetFloatLenght(input[1..], lenght + 1);
        }

        public static FSharpList<Token> Tokenize(this string input)
        {
            if (input.Length == 0)
            {
                return ListModule.Empty<Token>();
            }

            var head = input[0];

            if (head.IsOperation())
            {
                var token = new Token(TokenType.Operation, head.ToString());
                return new FSharpList<Token>(token, input[1..].Tokenize());
            }

            if (head.IsNumber())
            {
                var floagLenght = input.GetFloatLenght(0);
                var token = new Token(TokenType.Float, input[0..floagLenght]);
                return new FSharpList<Token>(token, input[floagLenght..].Tokenize());
            }

            return input[1..].Tokenize();
        }
    }
}
