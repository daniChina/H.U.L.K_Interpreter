namespace ClassLibrary;
public partial class SyntaxAnalyzer
{
    int position;
    Token currentToken;
    int tokenIndex;
    int counter = -1;
    public List<(int, int)> IfElseMatches;
    (int, int) lastMatch = (-1, -1);
    List<Token> tokenList;
    List<Dictionary<string, TokenType>> Scope = new List<Dictionary<string, TokenType>>();

    public List<Token> FunctionBody = new List<Token>();
   

    public SyntaxAnalyzer(List<Token> TokenList)
    {
        position = 0;
        tokenList = TokenList;
        currentToken = tokenList[position];
        tokenIndex = 0;
        IfElseMatches = new List<(int, int)>();

    }
   
    private void Eat(TokenType expectedToken)
    {
        if (currentToken.Type == expectedToken)
        {
            GetNextToken();
        }
        else Error("Se esperaba el token +" + expectedToken);
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