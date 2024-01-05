namespace ClassLibrary;
public partial class Parser
{
    private object ConcatString()//En el caso del operador de concatenacion no es necesario revisar cual es el tipo de Token pues es posible concatenar cualquier tipo
    {
        object text = ParseMathExpressionLv1();
        while (currentToken.Type is TokenType.Concat)
        {
            Eat(TokenType.Concat);
            text = text.ToString() + ParseMathExpressionLv1().ToString();
        }

        return text;
    }

    public object ParseMathExpressionLv1()//aqui voy a revisar sintacticamente si los miembros de la suma y la resta son de tipo Number
    {
        object result = ParseMathExpressionLv2();
        object rightSide;

        while (currentToken.Type is TokenType.Addition || currentToken.Type is TokenType.Substraction)
        {
            TokenType _operator = currentToken.Type;
            if (_operator is TokenType.Addition)
            {
                Eat(TokenType.Addition);
                if (result is not double) Error("Se esperaba un tipo numerico.");
                rightSide = ParseMathExpressionLv2();
                if (rightSide is not double) Error("Se esperaba un tipo numerico.");
                result = (double)result + (double)rightSide;

            }
            else
            {
                Eat(TokenType.Substraction);
                if (result is not double) Error("Se esperaba un tipo numerico.");
                rightSide = ParseMathExpressionLv2();
                if (rightSide is not double) Error("Se esperaba un tipo numerico.");
                result = (double)result - (double)rightSide;
            }
        }
        return result;
    }
    public object ParseMathExpressionLv2()//aqui voy a revisar sintacticamente si los miembros de la division y la multiplicacion son de tipo Number
    {
        object result = ParseMathExpressionLv3();
        object rightSide;

        while (currentToken.Type is TokenType.Multiplication || currentToken.Type is TokenType.Division || currentToken.Type is TokenType.Modulus)
        {
            TokenType _operator = currentToken.Type;
            if (_operator is TokenType.Multiplication)
            {
                Eat(TokenType.Multiplication);
                if (result is not double) Error("Se esperaba un tipo numerico.");
                rightSide = ParseMathExpressionLv3();
                if (rightSide is not double) Error("Se esperaba un tipo numerico.");
                result = (double)result * (double)rightSide;
            }

            else if (_operator is TokenType.Division)
            {
                Eat(TokenType.Division);
                if (result is not double) Error("Se esperaba un tipo numerico.");
                rightSide = ParseMathExpressionLv3();
                if (rightSide is not double) Error("Se esperaba un tipo numerico.");
                result = (double)result / (double)rightSide;
            }

            else
            {
                Eat(TokenType.Modulus);
                if (result is not double) Error("Se esperaba un tipo numerico.");
                rightSide = ParseMathExpressionLv3();
                if (rightSide is not double) Error("Se esperaba un tipo numerico.");
                result = (double)result % (double)rightSide;
            }
        }

        return result;
    }


    public object ParseMathExpressionLv3()//aqui voy a revisar sintacticamente si los tokens son numericos para poder evaluar en la expresion con el operador de potencia
    {
        object result = ParseBaseExp();


        while (currentToken.Type is TokenType.Power)
        {
            Eat(TokenType.Power);
            object exponente = ParseBaseExp();
            result = Math.Pow((double)result, (double)exponente);
        }

        return result;
    }
}
