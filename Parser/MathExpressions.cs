using Hulk_Interpreter;
namespace Parser
{
    partial class Parser
    {
        public object ConcatString()//*
        {
            object text = ParseMathExpressionsLv1();
            while (currentToken.Type != TokenType.EndOfLine && currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "@")
                {
                    Eat(TokenType.Operator);
                    text = text.ToString() + ParseMathExpressionsLv1().ToString();//???????
                }
                else return text;
                System.Console.WriteLine("error : operador no valido");

            }
            return text;
        }

        public object ParseMathExpressionsLv1()//*
        {
            object rightSide;
            object result = ParseMathExpressionLv2();

            while (currentToken.Type != TokenType.EndOfLine && (currentToken.Type == TokenType.Operator))
            {
                if (currentToken.Value.ToString() == "+")
                {
                    Eat(TokenType.Operator);
                    if (!(result is double)) Error("Se esperaba un tipo numerico");
                    rightSide = ParseMathExpressionLv2();
                    if (!(rightSide is double)) Error("Se esperaba un tipo numerico");
                    System.Console.WriteLine("Antes de sumar");
                    System.Console.WriteLine((double)result);
                    System.Console.WriteLine((double)rightSide);
                    result = (double)result + (double)rightSide;
                    System.Console.WriteLine("La suma es " + result);
                }
                else if (currentToken.Value.ToString() == "-")
                {
                    Eat(TokenType.Operator);
                    if (!(result is double)) Error("Se esperaba un tipo numerico");
                    rightSide = ParseMathExpressionLv3();
                    if (!(rightSide is double)) Error("Se esperaba un tipo numerico");
                    result = (double)result - (double)rightSide;
                }
                else return result; // este else es para los casos de los operadores que no sean + y -

            }
            return result;
        }
        public object ParseMathExpressionLv2()
        {
            object rightSide;
            object result = ParseMathExpressionLv3();

            while (currentToken.Type != TokenType.EndOfLine && currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "*")
                {
                    Eat(TokenType.Operator);
                    if (!(result is double)) Error("Se esperaba un tipo numerico");
                    rightSide = ParseMathExpressionLv3();
                    if (!(rightSide is double)) Error("Se esperaba un tipo numerico");
                    result = (double)result * (double)rightSide;
                }
                else if (currentToken.Value.ToString() == "/")
                {
                    Eat(TokenType.Operator);
                    if (!(result is double)) Error("Se esperaba un tipo numerico");
                    rightSide = ParseMathExpressionLv3();
                    if (!(rightSide is double)) Error("Se esperaba un tipo numerico");
                    result = (double)result * (double)rightSide;

                }
                else return result;//ese else es para los casos en que los operadores sean de + y - 
            }
            return result;

        }

        public object ParseMathExpressionLv3()
        {
            object result = ParseBaseExp();
            while (currentToken.Type != TokenType.EndOfLine && (currentToken.Type == TokenType.Operator))
            {
                if (currentToken.Value.ToString() == "^")
                {
                    if (!(result is double)) Error("se esperaba un number");
                    Eat(TokenType.Operator);
                    object Exponente = ParseBaseExp();
                    if (!(Exponente is double)) Error("se esperaba un tipo numerico");

                    result = Math.Pow(Convert.ToDouble(result), Convert.ToDouble(Exponente));
                }
                else return result;
            }
            return result;
        }

    }
}