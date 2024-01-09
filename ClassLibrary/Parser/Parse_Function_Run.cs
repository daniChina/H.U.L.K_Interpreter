namespace ClassLibrary;
public partial class Parser
{
    private object ParseFunction(string functionName)
    {
        switch (functionName)
        {
            case "sin":
                //Parsea la expresion coge el valor y pasaselo al metodo
                
                double numero1;
                if (!double.TryParse(ParseExpression().ToString(), out numero1)) Error("Se esperaba un tipo number");
                Eat(TokenType.RightParenthesis);
                return Math.Sin(numero1 * Math.PI / 180);
            case "cos":
                //Parsea la expresion coge el valor y pasaselo al metodo
                
                object numero2 = ParseExpression();
                Eat(TokenType.RightParenthesis);
                double number = (double)numero2 * Math.PI / 180;
                return Math.Cos(number);

            case "sqrt":
                //Parsea la expresion coge el valor y pasaselo al metodo
                
                object numero3 = ParseExpression();
                Eat(TokenType.RightParenthesis);
                return Math.Sqrt((double)numero3);

            case "exp":
                //Parsea la expresion coge el valor y pasaselo al metodo
                
                object numero4 = ParseExpression();
                Eat(TokenType.RightParenthesis);
                return Math.Pow(Math.E, (double)numero4);

            case "log":
                //Parsea la expresion coge el valor y pasaselo al metodo
                
                object basis = ParseExpression();
                Eat(TokenType.Comma);
                object argumento = ParseExpression();
                Eat(TokenType.RightParenthesis);

                return Math.Log((double)basis, (double)argumento);

            case "print":
                //Se toma como la funcion identidad
                
                object ToPrint = ParseExpression();
                Eat(TokenType.RightParenthesis);
                return ToPrint;

            case "rand":
                
                Eat(TokenType.RightParenthesis);
                Random random = new Random();
                return random.NextDouble();

            default:
                Dictionary<string, object> arguments = new Dictionary<string, object>();
                var function = Global.Functions[functionName];
                for (int i = 0; i < function.Arguments.Count; i++)
                {
                    arguments.Add(function.Arguments[i].Item1, ParseExpression());
                    if (currentToken.Type == TokenType.Comma)
                    {
                        Eat(TokenType.Comma);
                    }
                }
                Eat(TokenType.RightParenthesis);
                function.Scope = Scope;
                function.Scope.Add(arguments);
                return function.Evaluate();
        }
    }

}