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

            case TokenType.Not:
                Eat(TokenType.Not);
                result = AnalyzeExpression();
                if (result is not TokenType.Bool)
                {
                    Error("El operador ! solo se puede aplicar a tipos bool");
                }
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
                if (identifierName == "PI")
                {
                    result = TokenType.Number;
                    flag = true;
                    break;
                }
                else if (Scope[counter].ContainsKey(identifierName))
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

        }
        return result;
    }

    bool CheckNext(TokenType type)
    {

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
        // Scope.RemoveAt(counter-1);
        counter--;

        return inExpr;
    }
}


