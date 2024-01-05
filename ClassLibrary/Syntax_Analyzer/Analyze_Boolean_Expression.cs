namespace ClassLibrary;

public partial class SyntaxAnalyzer
{
    private TokenType AnalyzeBooleanExpressionLv1()
    {
        TokenType leftSide = AnalyzeBooleanExpressionLv2();
        TokenType rightSide;

        while (currentToken.Type is TokenType.And || currentToken.Type is TokenType.Or)
        {
            if (currentToken.Type is TokenType.And)
            {

                Eat(TokenType.And);
                
                if (leftSide is not TokenType.Bool && leftSide is not TokenType.Temporal) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

                rightSide = AnalyzeBooleanExpressionLv2();

                if (rightSide is not TokenType.Bool && rightSide is not TokenType.Temporal) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

            }
            else
            {
                Eat(TokenType.Or);
                if (leftSide is not TokenType.Bool && leftSide is not TokenType.Temporal) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

                rightSide = AnalyzeBooleanExpressionLv2();

                if (rightSide is not TokenType.Bool && rightSide is not TokenType.Temporal) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

            }
            return TokenType.Bool;

        }
        return leftSide;

    }

    private TokenType AnalyzeBooleanExpressionLv2()//aqui aqui voy a revisar sintacticamente si los miembros de los operadores de comparacion son del tipo correspondiente(==, != , <=, >= ,< , > )
    {
        TokenType leftSide = ConcatString();

        if (currentToken.Type is TokenType.EqualsTo)
        {
            TokenType _operator = currentToken.Type;
            Eat(TokenType.EqualsTo);
            TokenType rightSide = ConcatString();
            if (leftSide != rightSide) Error($"Los tipos en ambos miembros de {_operator} deben de ser los mismos");
            return TokenType.Bool;
        }

        else if (currentToken.Type is TokenType.NotEquals)
        {
            TokenType _operator = currentToken.Type;
            Eat(TokenType.NotEquals);
            TokenType rightSide = ConcatString();
            if (leftSide != rightSide) Error("Los tipos en ambos miembros del operador != deben de ser los mismos");
            return TokenType.Bool;

        }
        else if (currentToken.Type is TokenType.LessThan || currentToken.Type is TokenType.LessOrEquals || currentToken.Type is TokenType.GreaterThan || currentToken.Type is TokenType.GreaterOrEquals)
        {
            TokenType _operator = currentToken.Type;
            Eat(currentToken.Type);
            if (leftSide != TokenType.Number && leftSide is not TokenType.Temporal) Error($"El miembro izquierdo de \"{_operator}\" tiene que ser de tipo \"Number\".");
            TokenType rightSide = ConcatString();
            if (rightSide != TokenType.Number && rightSide is not TokenType.Temporal) Error($"El miembro derecho de \"{_operator}\" tiene que ser de tipo \"Number\".");
            return TokenType.Bool;
        }

        return leftSide;

    }

}
