using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace SyntaxAnalyzer
{
    public partial class SyntaxAnalyzer
    {
         public TokenType AnalyzeBooleanExpressionLv1()
        {
            TokenType leftSide = AnalyzeBooleanExpressionLv2();
            TokenType rightSide;

            if (currentToken.Value.ToString() == "&" || currentToken.Value.ToString() == "|")
            {

                Eat(TokenType.Operator);
                if (leftSide != TokenType.KeyWord_Boolean) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

                rightSide = AnalyzeBooleanExpressionLv2();

                if (rightSide != TokenType.KeyWord_Boolean) Error("Los tipos a ambos miembros del operador & tienen que ser Bool");

            }
            return leftSide;

        }

        public TokenType AnalyzeBooleanExpressionLv2()//aqui aqui voy a revisar sintacticamente si los miembros de los operadores de comparacion son del tipo correspondiente(==, != , <=, >= ,< , > )
        {
            TokenType leftSide = ConcatString();
            if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "==")
                {
                    Eat(TokenType.Operator);
                    TokenType rightSide = ConcatString();
                    if (leftSide != rightSide) Error("Los tipos en ambos miembros del operador == tienen que tener el mismo tipo");

                }
                else if (currentToken.Value.ToString() == "!=")
                {
                    Eat(TokenType.Operator);
                    TokenType rightSide = ConcatString();
                    if (leftSide != rightSide) Error("Los tipos en ambos miembros del operador != tienen que tener el mismo tipo");

                }

                else if (currentToken.Value.ToString() == "<" || currentToken.Value.ToString() == "<=" || currentToken.Value.ToString() == ">" || currentToken.Value.ToString() == ">=")
                {
                    Eat(TokenType.Operator);
                    if (leftSide != TokenType.Number) Error("El miembro izquierdo de " + currentToken.Value.ToString() + "tiene que ser de tipo Number");
                    TokenType rightSide = ConcatString();
                    if (rightSide != TokenType.Number) Error("El miembro derecho de " + currentToken.Value.ToString() + "tiene que ser de tipo Number");

                }
                else return leftSide;

            }

            return leftSide;
        }
        
    }
}