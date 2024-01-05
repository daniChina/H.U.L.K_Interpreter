namespace ClassLibrary;
public partial class SyntaxAnalyzer
{
    public bool Exists()
    {//Aqui voy a revisar si la funcion que se va a ejecutar existe en el Diccionario que guarda a las funciones declaradas 
        switch (currentToken.GetName())
        {
            case "sin":
                return true;
            case "cos":
                return true;
            case "log":
                return true;
            case "exp":
                return true;
            case "sqrt":
                return true;
            case "print":
                return true;
        }

        return Global.Functions.ContainsKey(currentToken.GetName());
    }

    public void CheckArguments(string functionName)
    {
        Eat(TokenType.LeftParenthesis);
        List<TokenType> args = new List<TokenType>();
        args.Add(AnalyzeExpression());
        
        while (currentToken.Type is TokenType.Comma)
        {
            Eat(TokenType.Comma);
            args.Add(AnalyzeExpression());
        }
        
        if (Global.Functions[functionName].Arguments.Count != args.Count)
            Error($"La funcion {functionName} recibe {Global.Functions[functionName].Arguments.Count} argumentos y solo {args.Count} fueron dados.");
    }


}