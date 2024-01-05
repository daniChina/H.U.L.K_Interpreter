using System.Xml.XPath;

namespace ClassLibrary;

public partial class Parser
{
    public object ParseBooleanExpressionLv1()
    {
        object leftSide = ParseBooleanExpressionLv2();
        object rightSide;

        while (currentToken.Type is TokenType.And || currentToken.Type is TokenType.Or)
        {
            TokenType _operator = currentToken.Type;

            if (_operator is TokenType.And)
            {
                Eat(TokenType.And);
                if (leftSide is not bool) Error("Se esperaba un tipo Bool");
                rightSide = ParseBooleanExpressionLv2();
                if (rightSide is not bool) Error("Se esperaba un tipo Bool");
                leftSide = (bool)leftSide && (bool)rightSide;
            }

            else
            {
                Eat(TokenType.Or);
                if (leftSide is not bool) Error("Se esperaba un tipo Bool");
                rightSide = ParseBooleanExpressionLv2();
                if (rightSide is not bool) Error("Se esperaba un tipo Bool");
                leftSide = (bool)leftSide || (bool)rightSide;
            }

        }
        return leftSide;

    }

    public object ParseBooleanExpressionLv2()//aqui aqui voy a revisar sintacticamente si los miembros de los operadores de comparacion son del tipo correspondiente(==, != , <=, >= ,< , > )
    {
        object leftSide = ConcatString();
        object rightSide;

        switch (currentToken.Type)
        {
            case TokenType.EqualsTo:
                Eat(TokenType.EqualsTo);
                rightSide = ConcatString();
                if (leftSide.GetType() != rightSide.GetType()) Error("Los tipos en ambos miembros del operador == tienen que tener el mismo tipo");
                leftSide = Equals(leftSide,rightSide);
                break;

            case TokenType.NotEquals:
                Eat(TokenType.NotEquals);
                rightSide = ConcatString();
                if (leftSide.GetType() != rightSide.GetType()) Error("Los tipos en ambos miembros del operador != tienen que tener el mismo tipo");
                leftSide = !Equals(leftSide,rightSide);
                break;

            case TokenType.LessThan:
                Eat(currentToken.Type);
                if (leftSide is not double) Error($"El miembro izquierdo de \"{TokenType.LessThan}\" tiene que ser de tipo Number.");
                rightSide = ConcatString();
                if (rightSide is not double) Error($"El miembro derecho de \"{TokenType.LessThan}\" tiene que ser de tipo Number");
                leftSide = (double)leftSide < (double)rightSide;
                break;

            case TokenType.LessOrEquals:
                Eat(currentToken.Type);
                if (leftSide is not double) Error($"El miembro izquierdo de \"{TokenType.LessOrEquals}\" tiene que ser de tipo Number.");
                rightSide = ConcatString();
                if (rightSide is not double) Error($"El miembro derecho de \"{TokenType.LessOrEquals}\" tiene que ser de tipo Number");
                leftSide = (double)leftSide <= (double)rightSide;
                break;

            case TokenType.GreaterThan:
                Eat(currentToken.Type);
                if (leftSide is not double) Error($"El miembro izquierdo de \"{TokenType.GreaterThan}\" tiene que ser de tipo Number.");
                rightSide = ConcatString();
                if (rightSide is not double) Error($"El miembro derecho de \"{TokenType.GreaterThan}\" tiene que ser de tipo Number");
                leftSide = (double)leftSide > (double)rightSide;
                break;

            case TokenType.GreaterOrEquals:
                Eat(currentToken.Type);
                if (leftSide is not double) Error($"El miembro izquierdo de \"{TokenType.GreaterOrEquals}\" tiene que ser de tipo Number.");
                rightSide = ConcatString();
                if (rightSide is not double) Error($"El miembro derecho de \"{TokenType.GreaterOrEquals}\" tiene que ser de tipo Number");
                leftSide = (double)leftSide >= (double)rightSide;
                break;
        }

        return leftSide;
    }

}
