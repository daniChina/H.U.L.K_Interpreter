using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace SyntaxAnalyzer
{
    public partial class SyntaxAnalyzer
    {
         public TokenType ConcatString()//En el caso del operador de concatenacion no es necesario revisar cual es el tipo de Token pues es posible concatenar cualquier tipo
        {
            TokenType text = AnalyzeMathExpressionsLv1();
            if (currentToken.Value.ToString() == "@")
            {
                Eat(TokenType.Operator);
            }
            return text;

        }
        public TokenType AnalyzeMathExpressionsLv1()//aqui voy a revisar sintacticamente si los miembros de la suma y la resta son de tipo Number
        {
            TokenType rightSide;
            TokenType result = AnalyzeMathExpressionLv2();

            while (currentToken.Type != TokenType.EndOfLine && (currentToken.Value.ToString() == "+" || currentToken.Value.ToString() == "-"))
            {
                if (currentToken.Value.ToString() == "+")
                {
                    Eat(TokenType.Operator);
                    if (result != TokenType.Number) Error("El miembro izquierdo  de una suma tiene que ser de tipo Number");
                    rightSide = AnalyzeMathExpressionLv2();
                    if (rightSide != TokenType.Number) Error("El miembro derecho de una suma tiene que ser de tipo Number");

                }
                else if (currentToken.Value.ToString() == "-")
                {
                    Eat(TokenType.Operator);
                    if (result != TokenType.Number) Error("El miembro izquierdo  de una resta tiene que ser de tipo Number");
                    rightSide = AnalyzeMathExpressionLv3();
                    if (rightSide != TokenType.Number) Error("El miembro derecho de una resta tiene que ser de tipo Number");
                }
                else return result; // este else es para los casos de los operadores que no sean + y -

            }
            return result;
        }
        public TokenType AnalyzeMathExpressionLv2()//aqui voy a revisar sintacticamente si los miembros de la division y la multiplicacion son de tipo Number
        {
            TokenType rightSide;
            TokenType result = AnalyzeMathExpressionLv3();

            while (currentToken.Type != TokenType.EndOfLine && (currentToken.Value.ToString() == "*" || currentToken.Value.ToString() == "/"))
            {
                if (currentToken.Value.ToString() == "*")
                {
                    Eat(TokenType.Operator);
                    if (result != TokenType.Number) Error("El miembro izquierdo  de un producto tiene que ser de tipo Number");
                    rightSide = AnalyzeMathExpressionLv3();
                    if (rightSide != TokenType.Number) Error("El miembro derecho de un producto tiene que ser de tipo Numbero");
                }
                else if (currentToken.Value.ToString() == "/")
                {
                    Eat(TokenType.Operator);
                    if (result != TokenType.Number) Error("El miembro izquierdo  de una division tiene que ser de tipo Number");
                    rightSide = AnalyzeMathExpressionLv3();
                    if (rightSide != TokenType.Number) Error("El miembro derecho de una division tiene que ser de tipo Number");
                }
                else return result;
            }
            return result;

        }



        public TokenType AnalyzeMathExpressionLv3()//aqui voy a revisar sintacticamente si los tokens son numericos para poder evaluar en la expresion con el operador de potencia
        {
            TokenType result = BaseExp();
            if (currentToken.Value.ToString() == "^")
            {
                if (result != TokenType.Number) Error("se esperaba un Token Number");
                Eat(TokenType.Operator);
                TokenType Exponente = BaseExp();
                if (Exponente != TokenType.Number) Error("se esperaba un Token Number");

            }

            return result;
        }
        
    }
}