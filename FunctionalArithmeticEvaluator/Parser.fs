module FSharpParser

open FSharpLexer

type NodeType =
    | Sum      = 1
    | Subtract = 2
    | Multiply = 3
    | Divide   = 4
    | Module   = 5

type Node =
    | OperationNode of OperationNode
    | ValueNode of float
and OperationNode = { Type: NodeType; Right: Node; Left: Node;  }

let private getNodeType value =
    match value with
    | "+" -> NodeType.Sum
    | "-" -> NodeType.Subtract
    | "*" -> NodeType.Multiply
    | "/" -> NodeType.Divide
    | _ ->   NodeType.Module

let private getPrecendence value = 
    match value with
    | "+" | "-" -> 1
    | _ -> 2

let parse tokens =
    let rec parseRec (tokens: list<Token>) precedence visited op =
        match tokens with
        | head :: tail ->
            match head.Type with
            | TokenType.Float -> parseRec tail precedence (head :: visited) op
            | _  ->
                let tokenPrecendence = getPrecendence head.Value
                if (tokenPrecendence > precedence) then
                    parseRec tail precedence (head :: visited) op                    
                else 
                    let nodeType = getNodeType head.Value
                    parseRec tail precedence [] (fun left ->
                        parseRec visited precedence [] (fun right ->
                            op(OperationNode({ Type = nodeType; Right = right; Left = left }))))
        | _ -> 
            match visited with    
            | head :: [] -> op (head.Value |> float |> ValueNode)        
            | _ -> parseRec visited 2 []  op   
    parseRec (List.rev tokens) 1 [] (fun x -> x)