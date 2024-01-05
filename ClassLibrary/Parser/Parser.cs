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
    // Dictionary<string, Function> FuncionesDeclaradas = new Dictionary<string, Function>();

    public Parser(List<Token> TokenList, List<Dictionary<string, object>> scope)
    {
        position = 0;
        tokenList = TokenList;
        currentToken = tokenList[position];
        tokenIndex = 0;
        Scope = scope;
        counter = scope.Count;
    }


    // public SyntaxAnalyzer(List<Token> TokenList, Dictionary<string, TokenType> Variables, Dictionary<string, Function> Functions)
    // {
    //     //Este constructor solo se utiliza en el casos en que voy a procesar la funcion, 
    //     //que necesito que reciba las variables,
    //     // las funciones que existen, y 
    //     tokenList = TokenList;
    //     position = 0;
    //     int size = TokenList.Count();
    //     Scope = new List<Dictionary<string, TokenType>>();
    //     EstoyAnalizando = AreAllNul();//Para saber que en este caso estoy analizando la funcion

    //     Scope.Add(Variables);
    //     int variable_subset = 0;
    //     FuncionesDeclaradas = Functions;
    //     if (position != size)
    //     {
    //         currentToken = tokenList[position];
    //     }
    //     else
    //     {
    //         currentToken = null;
    //     }
    //     bool AreAllNul()
    //     {
    //         foreach (var item in Variables)
    //         {
    //             if (item.Value == TokenType.Null) return true;
    //         }

    //         return false;
    //     }
    // }

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