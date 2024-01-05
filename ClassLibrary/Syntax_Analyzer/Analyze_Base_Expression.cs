namespace ClassLibrary;
public partial class SyntaxAnalyzer
{
    private TokenType BaseExp()
    //aqui analizo las expresiones mas basicas 
    {
        TokenType result = TokenType.Temporal;

        switch (currentToken.Type)
        {
            case TokenType.String:
                Eat(TokenType.String);
                result = TokenType.String;
                break;

            case TokenType.Number:
                Eat(TokenType.Number);
                result = TokenType.Number;
                break;

            case TokenType.Bool:
                Eat(TokenType.Bool);
                result = TokenType.Bool;
                break;

            case TokenType.LeftParenthesis:
                Eat(TokenType.LeftParenthesis);
                result = AnalyzeExpression();
                Eat(TokenType.RightParenthesis);
                break;

            case TokenType.IfKeyWord:
                result = AnalyzeIfElseExpression();
                IfElseMatches.Add(lastMatch);
                break;

            case TokenType.LetKeyWord:
                result = AnalyzeLetInExpression();
                break;

            case TokenType.Identifier:
                if (CheckNext(TokenType.LeftParenthesis))
                {
                    string name = currentToken.GetName();
                    bool exists = Exists();
                    if (!exists) Error($"La función \"{name}\" no existe.");
                    Eat(TokenType.Identifier);
                    CheckArguments(name);
                    Eat(TokenType.RightParenthesis);
                    break;
                }

                // Existe la variable ???
                string identifierName = currentToken.GetName();
                Eat(TokenType.Identifier);
                bool flag = false;

                if (Scope[counter].ContainsKey(identifierName))
                {
                    result = Scope[counter][identifierName];
                    flag = true;
                    break;
                }


                if (!flag) Error($"La variable \"{identifierName}\" no existe en el contexto actual.");
                Scope.Remove(Scope.Last());
                break;

            default:
                System.Console.WriteLine($"Token inesperado: \"{currentToken}\".");
                throw new Exception();

                /* PendienteEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE */
                // case TokenType.Not:
                //     Eat(TokenType.Not);
                //     result = AnalyzeExpression();
                //     if (result != TokenType.KeyWord_Boolean) Error("Se esperaba una expresion de tipo bool");
                //     break;
        }
        return result;
    }

    bool CheckNext(TokenType type)
    {
        // System.Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        // System.Console.WriteLine(tokenList[tokenIndex+1].Type);
        return tokenIndex < tokenList.Count - 1 && tokenList[tokenIndex + 1].Type == type;
    }

    public TokenType AnalyzeIfElseExpression()
    {
        (int, int) match = new(tokenIndex, -1);
        Eat(TokenType.IfKeyWord);

        TokenType ifCondition = AnalyzeExpression();
        if (ifCondition is not TokenType.Bool) Error("La cláusula de un if tiene que ser una expresión booleana.");

        TokenType ifExpression = AnalyzeExpression();


        Eat(TokenType.ElseKeyWord);
        match.Item2 = tokenIndex - 1;
        IfElseMatches.Add(match);
        TokenType elseExpression = AnalyzeExpression();

        lastMatch.Item2 = tokenIndex;
        if (ifExpression != elseExpression && ifExpression is not TokenType.Temporal && elseExpression is not TokenType.Temporal) Error("El tipo de retorno de una expresion if-else tiene que ser el mismo");
        return ifExpression;
    }

    // public void FunctionDeclaration()//este metodo lo voy a llamar si el token en el que estoy tiene como value la palabra function
    // {
    //     Eat(TokenType.KeyWords);

    //     if (currentToken.Type != TokenType.Identifier) Error("La funcion debe tener un nombre para ser declarada");
    //     string name = currentToken.Value.ToString()!;
    //     Eat(TokenType.Identifier);

    //     if (currentToken.Value.ToString() != "(") Error("La funcion debe tener los parametros entre parentesis");
    //     Eat(TokenType.Punctuator);

    //     while (currentToken.Value.ToString() != ")")
    //     {

    //         if (currentToken.Type != TokenType.Identifier) Error("Se esperaba un identificador");

    //         if (FunctionParameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
    //         FunctionParameters.Add(currentToken);

    //         Eat(TokenType.Identifier);

    //         while (currentToken.Value.ToString() == ",")
    //         {
    //             Eat(TokenType.Punctuator);
    //             if (currentToken.Type != TokenType.Identifier) Error("Se esperaba un identificador");

    //             if (FunctionParameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
    //             FunctionParameters.Add(currentToken);

    //             Eat(TokenType.Identifier);
    //         }
    //     }

    //     if (currentToken.Value.ToString() != ")") Error("Se esperaba )");
    //     Eat(TokenType.Punctuator);


    //     if (currentToken.Value.ToString() != "=>") Error("El operador para declarar funciones tiene que ser =>");
    //     Eat(TokenType.Operator);

    //     while (currentToken.Type != TokenType.EndOfLine)
    //     {
    //         FunctionBody.Add(currentToken);
    //         GetNextToken();
    //     }

    //     if (currentToken.Type != TokenType.EndOfLine) Error("Toda expresion tiene que terminar con ;");
    //     FunctionBody.Add(currentToken);
    //     Eat(TokenType.EndOfLine);

    //     if (currentToken.Type != TokenType.EndOfToken) Error("Despues del ; no puede haber ninguna expresion ");
    //     FunctionBody.Add(currentToken);
    //     Eat(TokenType.EndOfToken);

    //     Function function = new Function(name, FunctionParameters, FunctionBody, TokenType.Null);//en principio el tipo de retorno es null porque no se cual es 
    //     AddFunction(function);
    // }
    // public void AddFunction(Function function)
    // {
    //     if (FuncionesDeclaradas.ContainsKey(function.Name)) Error("No se puede agregar la funcion, ya existe otra con el mismo nombre");
    //     FuncionesDeclaradas.Add(function.Name, function);

    // }

    public TokenType AnalyzeLetInExpression()
    {
        Eat(TokenType.LetKeyWord);
        Dictionary<string, TokenType> LocalScope = new Dictionary<string, TokenType>();

        Eat(TokenType.Identifier);
        string variableName = tokenList[tokenIndex - 1].GetName();

        Eat(TokenType.Equals);
        TokenType expr = AnalyzeExpression();

        if (LocalScope.ContainsKey(variableName))
            LocalScope[variableName] = expr;

        else LocalScope.Add(variableName, expr);

        while (currentToken.Type == TokenType.Comma)
        {
            Eat(TokenType.Comma);
            Eat(TokenType.Identifier);
            string Identifier2 = tokenList[tokenIndex - 1].GetName();
            Eat(TokenType.Equals);

            TokenType expr2 = AnalyzeExpression();

            if (LocalScope.ContainsKey(Identifier2))
            {
                LocalScope[Identifier2] = expr2;
            }

            else LocalScope.Add(Identifier2, expr2);
        }

        Scope.Add(LocalScope);
        counter++;

        Eat(TokenType.InKeyWord);

        TokenType inExpr = AnalyzeExpression();
        Scope.RemoveAt(counter);
        counter--;

        return inExpr;
    }
}


