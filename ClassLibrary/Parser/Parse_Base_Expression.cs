namespace ClassLibrary;
public partial class Parser
{
    public object ParseBaseExp()
    //aqui parseo las expresiones mas basicas 
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
                break;

            case TokenType.LetKeyWord:
                result = ParseLetInExpression();
                break;

            case TokenType.Not:
                Eat(TokenType.Not);
                result = ParseExpression();
                if (result is Boolean){
                    bool resultado = !(bool)result;
                    return resultado ; 
                }
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
                if (identifierName == "PI")
                {
                    result = Math.PI;
                    break;
                }

                if (Scope[counter - 1].ContainsKey(identifierName))
                {
                    result = Scope[counter - 1][identifierName];
                    break;
                }

                break;
                
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


