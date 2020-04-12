module FSharpLexer 

type TokenType =
    | Operation = 0
    | Float     = 1
    
type Token = {
    Type: TokenType
    Value: string
}

[<Literal>]
let Operations = "+-*/%"
[<Literal>]
let Numbers = "1234567890."

let private isOperation (c: char) = Operations.Contains(c)

let private isNumber (c: char) = Numbers.Contains(c)

let rec private getfloatLenght input lenght =
    match input with
    | "" -> lenght
    | _ ->
        match input.[0] with
        | c when isNumber c -> getfloatLenght input.[1..] (lenght + 1)
        | _ -> lenght

let rec tokenize input =
    match input with
    | "" -> []
    | _ ->
        match input.[0] with
        | c when isOperation c ->
            let token = { Type = TokenType.Operation; Value = input.[0].ToString() }
            token :: (tokenize input.[1..])
        | c when isNumber c ->
            let floatLenght = getfloatLenght input 0
            let token = { Type = TokenType.Operation; Value = input.[0..(floatLenght - 1)] }
            token :: (tokenize input.[1..])
        | _ -> tokenize input.[1..]

let tailTokenize input =
    let rec tokenizeRec input tokens =
        match input with
        | "" -> List.rev tokens
        | _ ->
            match input.[0] with
            | c when isOperation c ->
                let token = { Type = TokenType.Operation; Value = input.[0].ToString() }
                tokenizeRec input.[1..] (token :: tokens)
            | c when isNumber c ->
                let floatLenght = getfloatLenght input 0
                let token = { Type = TokenType.Operation; Value = input.[0..(floatLenght - 1)] }
                tokenizeRec input.[floatLenght..]  (token :: tokens)
            | _ -> tokenizeRec input.[1..] tokens
    tokenizeRec input []

let tailTokenizeChar (input: string) =
    let rec tokenizeRec input visited floatChars =
        let flushFloat = 
            match floatChars with
            | [] -> visited           
            | _ -> 
                let floatToken = { Type = TokenType.Float; Value = new System.String(floatChars |> List.rev |> Array.ofList) }    
                floatToken :: visited
        match input with
        | head :: tail -> 
            match head with
            | c when isOperation c ->
                let operationToken = { Type = TokenType.Operation; Value = head.ToString() }
                tokenizeRec tail (operationToken :: flushFloat) []
            | c when isNumber c -> 
                tokenizeRec tail visited (c :: floatChars)
            | _ -> tokenizeRec tail visited floatChars
        | [] -> List.rev flushFloat
    tokenizeRec (input.ToCharArray() |> List.ofArray) [] []