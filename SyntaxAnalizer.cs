using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hulk_Interpreter
{
    public class SyntaxAnalizer
    {
        public int position;
        public Token currentToken;
        public int tokenIndex;
        List<Token> tokenList = new List<Token>();
        List<Dictionary<string, TokenType>> Scope = new List<Dictionary<string, TokenType>>();
        public List<Token> Parameters = new List<Token>();
        public List<Token> FunctionBody = new List<Token>();
        List<Function> FuncionesDeclaradas = new List<Function>();
        List<Token> estructuraSintactica = new List<Token>();



        public SyntaxAnalizer(List<Token> TokenList)
        {
            position = 0;
            tokenList = TokenList;
            currentToken = tokenList[position];
            tokenIndex = 0;

        }
        private void Eat(TokenType tokenType)
        {
            if (currentToken.Type != TokenType.EndOfLine && currentToken.GetType() == tokenType)
            {
                GetNextToken();

            }
            else Error(" Se esperaba el token" + tokenType);
        }

        private void GetNextToken()
        {
            if (tokenIndex < tokenList.Count)
            {
                tokenIndex++;
                currentToken = tokenList[tokenIndex];

            }
        }
        private void Error(string mensaje)
        {
            throw new Exception("Syntax Error : " + mensaje);
        }

        public TokenType AnalyzeExpression()
        {
            TokenType result = AnalyzeBooleanExpressionLv1();
            return result;
        }

        //Aqui voy a revisar sintacticamente las expresiones de tipo bool

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

        public TokenType BaseExp()
        //aqui analizo las expresiones mas basicas 
        {

            Token token = currentToken;
            if (currentToken.Type == TokenType.Text)
            {
                Eat(TokenType.Text);
                return TokenType.Text;
            }
            else if (currentToken.Type == TokenType.Number)
            {
                Eat(TokenType.Number);
                return TokenType.Number;
            }
            else if (currentToken.GetValue().ToString() == "(")
            {
                Eat(TokenType.Punctuator);
                TokenType result = AnalyzeBooleanExpressionLv1();
                if (currentToken.GetValue().ToString() != ")") Error("Se esperaba )");
                return result;

            }

            if (currentToken.Value.ToString() == "true")
            {
                Eat(TokenType.KeyWords);
                return TokenType.KeyWord_Boolean;
            }
            else if (currentToken.Value.ToString() == "false")
            {
                Eat(TokenType.KeyWords);
                return TokenType.KeyWord_Boolean;
            }
            return TokenType.EndOfToken;
        }

        public TokenType AnalyzeIfElseExpression()
        {
            Eat(TokenType.KeyWords);
            if (currentToken.Value.ToString() != "(") Error("se esperaba (");
            Eat(TokenType.Punctuator);

            TokenType ifCondition = AnalyzeExpression();
            if (ifCondition != TokenType.KeyWord_Boolean) Error("La condicion de un if tiene que ser de tipo booleano ");

            if (currentToken.Value.ToString()!=")") Error("Se esperaba )");
            Eat(TokenType.Punctuator);

            TokenType ifExpression = AnalyzeExpression();

            if (!(currentToken.Value.ToString() == "else"))  Error("Se eperaba un else");
            
            Eat(TokenType.KeyWords);
            TokenType elseExpression = AnalyzeExpression();
            
            if(ifExpression!= elseExpression ) Error("EL tipo de retorno de una expresion if-else tiene que ser el mismo");
            return ifExpression;
            
        }
        public void FunctionDeclaration()//este metodo lo voy a llamar si el token en el que estoy tiene como value la palabra function
        {
            Eat(TokenType.KeyWords);

            if (currentToken.Type != TokenType.Identifier) Error("La funcion debe tener un nombre para ser declarada");
            string name = currentToken.Value.ToString()!;
            Eat(TokenType.Identifier);

            if (currentToken.Value.ToString() != "(") Error("La funcion debe tener los parametros entre parentesis");
            Eat(TokenType.Punctuator);

            while (currentToken.Value.ToString() != ")")
            {

                if (currentToken.Type != TokenType.Identifier) Error("Se esperaba un identificador");

                if (Parameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
                Parameters.Add(currentToken);

                Eat(TokenType.Identifier);

                while (currentToken.Value.ToString() == ",")
                {
                    Eat(TokenType.Punctuator);
                    if (currentToken.Type != TokenType.Identifier) Error("Se esperaba un identificador");

                    if (Parameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
                    Parameters.Add(currentToken);

                    Eat(TokenType.Identifier);
                }
            }

            if (currentToken.Value.ToString() != ")") Error("Se esperaba )");
            Eat(TokenType.Punctuator);


            if (currentToken.Value.ToString() != "=>") Error("El operador para declarar funciones tiene que ser =>");
            Eat(TokenType.Operator);

            while (currentToken.Type != TokenType.EndOfLine)
            {
                FunctionBody.Add(currentToken);
                GetNextToken();
            }

            if (currentToken.Value.ToString() != ";") Error("Toda expresion tiene que terminar con ;");

            Function function = new Function(name, Parameters, FunctionBody);
        }
        public void AddFunction(Function function)
        {
            if (FuncionesDeclaradas.Contains(function)) Error("No se puede agregar la funcion, ya existe otra con el mismo nombre");
            FuncionesDeclaradas.Add(function);

        }


        public TokenType AnalyzeLetInExpression()
        {
            int contador = 0;

            Dictionary<string, TokenType> LocalScope = new Dictionary<string, TokenType>();
            Eat(TokenType.KeyWords);

            if (!(currentToken.Type == TokenType.Identifier)) Error(" Se esperaba un identificador ");
            string Identifier = currentToken.Value.ToString()!;
            Eat(TokenType.Identifier);

            if (!(currentToken.Value.ToString() == "=")) Error("se esperaba = ");
            Eat(TokenType.Operator);

            TokenType expr = AnalyzeExpression();

            if (LocalScope.ContainsKey(Identifier))
            {
                LocalScope[Identifier] = expr;

            }
            else LocalScope.Add(Identifier, expr);

            while (currentToken.Type != TokenType.EndOfLine && currentToken.Value.ToString() == ",")
            {
                Eat(TokenType.Punctuator);
                if (!(currentToken.Type == TokenType.Identifier)) Error("se esperaba un identificador");

                string Identifier2 = currentToken.Value.ToString()!;
                Eat(TokenType.Identifier);

                if (!(currentToken.Value.ToString() == "=")) Error("se esperaba = ");
                Eat(TokenType.Operator);

                TokenType expr2 = AnalyzeExpression();

                if (LocalScope.ContainsKey(Identifier2))
                {
                    LocalScope[Identifier2] = expr2;

                }
                else LocalScope.Add(Identifier2, expr2);
                contador++;
            }

            Scope.Add(LocalScope);

            if (currentToken.Value.ToString() != "in") Error("se esperaba la expresion in ");
            Eat(TokenType.KeyWords);

            TokenType inExpr = AnalyzeExpression();

            Scope.Remove(LocalScope);
            return inExpr;
        }
    }
}
