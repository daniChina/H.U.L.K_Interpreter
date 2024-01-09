namespace ClassLibrary;
public partial class Parser
{
    private int position;
    private Token currentToken;
    private int tokenIndex;

    public int counter;
    public List<(int, int)> IfElseMatches;
    List<Token> tokenList = new List<Token>();
    List<Dictionary<string, object>> Scope = new List<Dictionary<string, object>>();

    public Parser(List<Token> TokenList, List<Dictionary<string, object>> scope)
    {
        position = 0;
        tokenList = TokenList;
        currentToken = tokenList[position];
        tokenIndex = 0;
        Scope = scope;
        counter = scope.Count;
    }


   
    private void Eat(TokenType type)
    {
        if (currentToken.Type == type)
        {
            GetNextToken();
        }
        else Error($"Se esperaba el token \"{type}\", \"{currentToken}\" fue encontrado.");
    }

    private void GetNextToken()
    {
        if (tokenIndex < tokenList.Count)
        {
            tokenIndex++;
            currentToken = tokenList[tokenIndex];

        }
    }
    private void Error(string mensaje)
    {
        throw new Exception("Syntax Error : " + mensaje);
    }

    public object ParseExpression()
    {
        object result = ParseBooleanExpressionLv1();
        return result;
    }
     public int ElsePosition(int ifPosition)
    {
        int position = int.MinValue;
        int ifAmount = 1;
        int elseAmount = 0;
        for (int i = ifPosition + 1; i < tokenList.Count; i++)
        {
            if (tokenList.ElementAt(i).Type == TokenType.IfKeyWord)
            {
                ifAmount++;
            }
            if (tokenList.ElementAt(i).Type == TokenType.ElseKeyWord)
            {
                elseAmount++;

            }
            if (ifAmount == elseAmount)
            {
                position = i;
                return position;
            }

        }
        return position + 1;
    }
}