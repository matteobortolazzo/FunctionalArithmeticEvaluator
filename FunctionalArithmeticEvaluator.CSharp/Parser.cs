using Microsoft.FSharp.Collections;
using static FunctionalArithmeticEvaluator.CSharp.CSharpLexer;

namespace FunctionalArithmeticEvaluator.CSharp
{
    public static class CSharpParser
    {
        public enum OperationType
        {
            Sum,
            Subtract,
            Multiply,
            Divide,
            Module
        }

        public abstract class Node { }
        public class FloatNode: Node
        {
            public FloatNode(float value)
            {
                Value = value;
            }

            public float Value { get; }
        }

        public class OperationNode : Node
        {
            public OperationNode(OperationType operationType, Node left, Node right)
            {
                OperationType = operationType;
                Left = left;
                Right = right;
            }

            public OperationType OperationType { get; }
            public Node Right { get; }
            public Node Left { get; }
        }

        private static FSharpList<Token> Empty => ListModule.Empty<Token>();

        private static OperationType ToType(this string op) => op switch
        {
            "+" => OperationType.Sum,
            "-" => OperationType.Subtract,
            "*" => OperationType.Multiply,
            "/" => OperationType.Divide,
            _ => OperationType.Module,
        };

        private static int GetPriority(this string op) => op == "+" || op == "-" ? 1 : 2;
        
        private static Node ParseRec(this FSharpList<Token> tokens, FSharpList<Token> visited, int priority)
        {
            Token head = tokens.Head;

            FSharpList<Token> Visit() => new FSharpList<Token>(head, visited);

            if (tokens.Length == 1)
            {
                if (visited.IsEmpty)
                {
                    return new FloatNode(float.Parse(head.Value));
                }

                return ListModule.Reverse(Visit()).ParseRec(Empty, 2);
            }

            if (head.Type == TokenType.Operation)
            {
                OperationType type = head.Value.ToType();
                if (head.Value.GetPriority() == priority)
                {
                    Node left = tokens.Tail.ParseRec(Empty, 1);
                    Node right = ListModule.Reverse(visited).ParseRec(Empty, 1);
                    return new OperationNode(type, left, right);
                }
            }

            return tokens.Tail.ParseRec(Visit(), priority);
        }

        public static Node Parse(this FSharpList<Token> tokens)
        {
            return ListModule.Reverse(tokens).ParseRec(Empty, 1);
        }
    }
}
