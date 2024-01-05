namespace ClassLibrary;
public partial class Parser
{
    public object ParseBaseExp()
    //aqui analizo las expresiones mas basicas 
    {
        object result = null!;

        switch (currentToken.Type)
        {
            case TokenType.String:
                result = currentToken.GetName();
                Eat(TokenType.String);
                break;

            case TokenType.Number:
                result = currentToken.GetValue();
                Eat(TokenType.Number);
                break;

            case TokenType.Bool:
                result = currentToken.GetValue();
                Eat(TokenType.Bool);
                break;

            case TokenType.LeftParenthesis:
                Eat(TokenType.LeftParenthesis);
                result = ParseBooleanExpressionLv1();
                Eat(TokenType.RightParenthesis);
                break;

            case TokenType.IfKeyWord:
                result = ParseIfElseExpression();
                // foreach (var match in IfElseMatches)
                //     if (match.Item1 == -1)
                //     {
                //         tokenIndex = match.Item2;
                //         currentToken = tokenList[tokenIndex];
                //     }
                break;

            case TokenType.LetKeyWord:
                result = ParseLetInExpression();
                break;

            case TokenType.Identifier:
                // Es una funcion ???
                if (CheckNext(TokenType.LeftParenthesis))
                {
                    string functionName = currentToken.GetName();
                    Eat(TokenType.Identifier);
                    Eat(TokenType.LeftParenthesis);
                    result = ParseFunction(functionName);
                    return result;
                }
                // Existe la variable ???
                string identifierName = currentToken.GetName();
                Eat(TokenType.Identifier);

                if (Scope[counter - 1].ContainsKey(identifierName))
                {
                    result = Scope[counter - 1][identifierName];
                    break;
                }

                break;

                /* PendienteEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE */
                // case TokenType.Not:
                //     Eat(TokenType.Not);
                //     result = ParseExpression();
                //     if (result != TokenType.KeyWord_Boolean) Error("Se esperaba una expresion de tipo bool");
                //     break;
        }
        return result;
    }

    bool CheckNext(TokenType type)
    {
        return tokenIndex < tokenList.Count - 1 && tokenList[tokenIndex + 1].Type == type;
    }

    private object ParseIfElseExpression()
    {
        object ifResult = null!;
        int ifPosition = tokenIndex;
        Eat(TokenType.IfKeyWord);

        object ifCondition = ParseExpression();

        if (ifCondition is true)
            ifResult = ParseExpression();

        else
        {
            tokenIndex = ElsePosition(ifPosition);
            currentToken = tokenList[tokenIndex];
            Eat(TokenType.ElseKeyWord);
            ifResult = ParseExpression();
        }

        return ifResult;
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

    public object ParseLetInExpression()
    {
        Eat(TokenType.LetKeyWord);
        Dictionary<string, object> LocalScope = new Dictionary<string, object>();

        Eat(TokenType.Identifier);
        string variableName = tokenList[tokenIndex - 1].GetName();

        Eat(TokenType.Equals);
        object expr = ParseExpression();

        if (LocalScope.ContainsKey(variableName))
            LocalScope[variableName] = expr;

        else LocalScope.Add(variableName, expr);

        while (currentToken.Type == TokenType.Comma)
        {
            Eat(TokenType.Comma);
            Eat(TokenType.Identifier);
            string Identifier2 = tokenList[tokenIndex - 1].GetName();
            Eat(TokenType.Equals);

            object expr2 = ParseExpression();

            if (LocalScope.ContainsKey(Identifier2))
            {
                LocalScope[Identifier2] = expr2;
            }

            else LocalScope.Add(Identifier2, expr2);
        }

        Scope.Add(LocalScope);
        counter++;

        Eat(TokenType.InKeyWord);

        object inExpr = ParseExpression();
        Scope.RemoveAt(counter);
        counter--;

        return inExpr;
    }
}


