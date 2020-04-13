module FSharpInterpreter

open FSharpParser

let rec evaluate node =
    let rec evaluateRec node op =
        match node with
        | ValueNode(v) -> op v
        | OperationNode(b) ->
            match b.Type with
            | NodeType.Sum      -> evaluateRec b.Left (fun left -> evaluateRec b.Right (fun right -> op(left + right) ))
            | NodeType.Subtract -> evaluateRec b.Left (fun left -> evaluateRec b.Right (fun right -> op(left - right) ))
            | NodeType.Multiply -> evaluateRec b.Left (fun left -> evaluateRec b.Right (fun right -> op(left * right) ))
            | NodeType.Divide   -> evaluateRec b.Left (fun left -> evaluateRec b.Right (fun right -> op(left / right) ))
            | _                 -> evaluateRec b.Left (fun left -> evaluateRec b.Right (fun right -> op(left % right) ))
    evaluateRec node (fun x -> x)