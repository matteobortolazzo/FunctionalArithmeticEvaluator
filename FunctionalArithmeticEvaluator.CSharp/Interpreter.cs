using static FunctionalArithmeticEvaluator.CSharp.CSharpParser;

namespace FunctionalArithmeticEvaluator.CSharp
{
    public static class CSharpInterpreter
    {
        public static float Evaluate(this Node node)
        {
            if (node is FloatNode flNode)
            {
                return flNode.Value;
            }

            var opNode = node as OperationNode;

            var left = opNode.Left.Evaluate();
            var right = opNode.Right.Evaluate();

            return opNode.OperationType switch
            {
                OperationType.Sum      => left + right,
                OperationType.Subtract => left - right,
                OperationType.Multiply => left * right,
                OperationType.Divide   => left / right,
                _                      => left % right
            };
        }
    }
}
