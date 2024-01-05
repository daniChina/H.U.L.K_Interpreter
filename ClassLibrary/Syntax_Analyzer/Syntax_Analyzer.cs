namespace ClassLibrary;
public partial class SyntaxAnalyzer
{
    int position;
    Token currentToken;
    int tokenIndex;
    int counter = -1;
    public List<(int, int)> IfElseMatches;
    (int, int) lastMatch = (-1, -1);
    bool EstoyAnalizando { get; set; }//Esto es para cuando vaya a analizar sintacticamente el cuerpo de una funcion
    List<Token> tokenList;
    List<Dictionary<string, TokenType>> Scope = new List<Dictionary<string, TokenType>>();

    public List<Token> FunctionBody = new List<Token>();

    // Dictionary<string, Function> FuncionesDeclaradas = new Dictionary<string, Function>();

    public SyntaxAnalyzer(List<Token> TokenList)
    {
        position = 0;
        tokenList = TokenList;
        currentToken = tokenList[position];
        tokenIndex = 0;
        IfElseMatches = new List<(int, int)>();
        EstoyAnalizando = false;

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

    private void Eat(TokenType expectedToken)
    {
        if (currentToken.Type == expectedToken)
        {
            GetNextToken();
        }
        else Error($"Se esperaba \"{expectedToken}\" en la columna {tokenIndex}.");
    }

    private void GetNextToken()
    {
        if (tokenIndex + 1 < tokenList.Count)
        {
            tokenIndex++;
            currentToken = tokenList[tokenIndex];
        }
    }
    private void Error(string mensaje)
    {
        throw new Exception("Syntax Error : " + mensaje);
    }

    public void StartAnalyzeExpression()
    {

        //  En este metodo voy a ver si el token en el que estoy es una funcion y si es , la declaro y la agrego al 
        //  diccionario de funciones.
        //  Si no es una funcion , entonces es una expresion y la analizo sintacticamente 

        if (currentToken.Type is TokenType.FunctionKeyWord) { FunctionDeclaration(); }
        else { AnalyzeExpression(); }
    }
    private TokenType AnalyzeExpression()
    {
        TokenType result = AnalyzeBooleanExpressionLv1();

        return result;
    }
}