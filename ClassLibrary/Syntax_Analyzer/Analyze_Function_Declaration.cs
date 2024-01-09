namespace ClassLibrary;

public partial class SyntaxAnalyzer
{
    private void FunctionDeclaration()
    {
        Function function = new Function(new List<Token>());

        Eat(TokenType.FunctionKeyWord);
        Eat(TokenType.Identifier);

        string functionName = tokenList[tokenIndex - 1].GetName();

        Eat(TokenType.LeftParenthesis);
        Eat(TokenType.Identifier);

        function.Arguments.Add((tokenList[tokenIndex - 1].GetName(), null!));

        while (currentToken.Type is TokenType.Comma)
        {
            Eat(TokenType.Comma);
            Eat(TokenType.Identifier);
            function.Arguments.Add((tokenList[tokenIndex-1].GetName(), null!));
        }

        Eat(TokenType.RightParenthesis);
        Eat(TokenType.Arrow);

        int index = tokenIndex;
        Dictionary<string,TokenType> functionScope = new Dictionary<string, TokenType>();

        foreach(var argument in function.Arguments)
            functionScope.Add(argument.Item1,TokenType.Temporal);

        Scope.Add(functionScope);
        counter++;
        
        Global.Functions.Add(functionName,function);

        AnalyzeExpression();

        for (var i = index; i < tokenList.Count; i++)
        {
            function.Body.Add(tokenList[i]);
        }
        function.Body.Add(new CommonToken(TokenType.Semicolon, ";"));

        Global.Functions[functionName] = function;
        
        counter--;
    }
}