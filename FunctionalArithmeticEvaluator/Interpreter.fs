module FSharpInterpreter

open FSharpParser

let rec evaluate node =
    match node with
    | ValueNode(v) -> v
    | OperationNode(b) ->
        let left = evaluate b.Left
        let right = evaluate b.Right
        match b.Type with
        | NodeType.Sum      -> left + right
        | NodeType.Subtract -> left - right
        | NodeType.Multiply -> left * right
        | NodeType.Divide   -> left / right
        | _                 -> left % right

let evaluateTail node =
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