namespace ClassLibrary;
public partial class SyntaxAnalyzer
{
    private TokenType ConcatString()//En el caso del operador de concatenacion no es necesario revisar cual es el tipo de Token pues es posible concatenar cualquier tipo
    {
        TokenType text = AnalyzeMathExpressionsLv1();
        if (currentToken.Type is TokenType.Concat)
        {
            Eat(TokenType.Concat);
           TokenType rightSide = AnalyzeMathExpressionsLv1();
        }
        return text;

    }
    private TokenType AnalyzeMathExpressionsLv1()//aqui voy a revisar sintacticamente si los miembros de la suma y la resta son de tipo Number
    {
        TokenType result = AnalyzeMathExpressionLv2();
        TokenType rightSide;

        while (currentToken.Type is TokenType.Addition || currentToken.Type is TokenType.Substraction)
        {
            if (currentToken.Type is TokenType.Addition)
            {
                Eat(TokenType.Addition);
                if (result is not TokenType.Number && result is not TokenType.Temporal) Error("El miembro izquierdo  de una suma tiene que ser de tipo Number");
                rightSide = AnalyzeMathExpressionLv2();
                if (rightSide is not TokenType.Number && rightSide is not TokenType.Temporal) Error("El miembro derecho de una suma tiene que ser de tipo Number");

            }
            else  
            {
                Eat(TokenType.Substraction);
                if (result is not TokenType.Number && result is not TokenType.Temporal) Error("El miembro izquierdo  de una resta tiene que ser de tipo Number");
                rightSide = AnalyzeMathExpressionLv3();
                if (rightSide is not TokenType.Number && rightSide is not TokenType.Temporal) Error("El miembro derecho de una resta tiene que ser de tipo Number");
            }
        }
        return result;
    }
    private TokenType AnalyzeMathExpressionLv2()//aqui voy a revisar sintacticamente si los miembros de la division y la multiplicacion son de tipo Number
    {
        TokenType result = AnalyzeMathExpressionLv3();
        TokenType rightSide;

        while (currentToken.Type is TokenType.Multiplication || currentToken.Type is TokenType.Division || currentToken.Type is TokenType.Modulus)
        {
            if (currentToken.Type is TokenType.Multiplication)
            {
                Eat(TokenType.Multiplication);
                if (result is not TokenType.Number && result is not TokenType.Temporal) Error("El miembro izquierdo  de un producto tiene que ser de tipo Number");
                rightSide = AnalyzeMathExpressionLv3();
                if (rightSide is not TokenType.Number && rightSide is not TokenType.Temporal) Error("El miembro derecho de un producto tiene que ser de tipo Number");
            }
            else if(currentToken.Type is TokenType.Division)
            {
                Eat(TokenType.Division);
                if (result is not TokenType.Number && result is not TokenType.Temporal) Error("El miembro izquierdo  de una division tiene que ser de tipo Number");
                rightSide = AnalyzeMathExpressionLv3();
                if (rightSide is not TokenType.Number && rightSide is not TokenType.Temporal) Error("El miembro derecho de una division tiene que ser de tipo Number");
            }
            else
            {
                Eat(TokenType.Modulus);
                if (result is not TokenType.Number && result is not TokenType.Temporal) Error("El miembro izquierdo  de una division tiene que ser de tipo Number");
                rightSide = AnalyzeMathExpressionLv3();
                if (rightSide is not TokenType.Number && rightSide is not TokenType.Temporal) Error("El miembro derecho de una division tiene que ser de tipo Number");
            }
        }
        return result;

    }



    private TokenType AnalyzeMathExpressionLv3()//aqui voy a revisar sintacticamente si los tokens son numericos para poder evaluar en la expresion con el operador de potencia
    {
        TokenType result = BaseExp();

        if (currentToken.Type is TokenType.Power)
        {
            if (result is not TokenType.Number && result is not TokenType.Temporal) Error("Antes de un simbolo de ^ se espera un tipo number");
            Eat(TokenType.Power);
            TokenType Exponente = AnalyzeMathExpressionLv3();
            if (Exponente is not TokenType.Number && Exponente is not TokenType.Temporal) Error("Despues de un simbolo de ^ se espera un tipo number");
        }

        return result;
    }
}