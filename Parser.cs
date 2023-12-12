using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hulk_Interpreter
{
    public class Parser
    {
        public int position;
        public Token currentToken;
        public int tokenIndex;

        List<Token> tokenList = new List<Token>();
        List<Dictionary<string, object>> ScopeLetIn = new List<Dictionary<string, object>>();
        public List<Token> Parameters = new List<Token>();
        public List<Token> FunctionBody = new List<Token>();
        List<Function> FuncionesDeclaradas = new List<Function>();
        List<Token> Variables = new List<Token>();


        public Parser(List<Token> TokenList)
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
        private void GetPastToken()
        {
            if (tokenIndex > 0)
            {
                tokenIndex--;
                currentToken = tokenList[tokenIndex];
            }
        }
        public object ParseExpression(){
            object result = ParseBooleanExpressionLv1();
            return result ;
        }


        public object ParseBooleanExpressionLv1()
        {
            object leftSide = ParseBooleanExpressionLv2();
            object rightSide;

            if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "&")
                {
                    Eat(TokenType.Operator);
                    rightSide = ParseBooleanExpressionLv2();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);
                        return left && right;
                    }
                }
                else if (currentToken.Value.ToString() == "|")
                {
                    Eat(TokenType.Operator);
                    rightSide = ParseBooleanExpressionLv2();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);
                        return left || right;
                    }
                }
                else return leftSide;
            }
            return leftSide;

        }

        public object ParseBooleanExpressionLv2()
        {
            object leftSide = ConcatString();
            if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "==")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);

                        return left == right;
                    }

                }
                else if (currentToken.Value.ToString() == "!=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);

                        return left != right;
                    }

                }

                else if (currentToken.Value.ToString() == "<")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left < right;
                    }
                }
                else if (currentToken.Value.ToString() == "<=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        if (left <= right) return true;
                        else return false;
                    }
                }

                else if (currentToken.Value.ToString() == ">")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left > right;
                    }

                }
                else if (currentToken.Value.ToString() == ">=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left >= right;
                    }

                }
                else return leftSide;

            }
            return leftSide;
        }
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

        public object ParseBaseExp()
        {
            object result = "";
            Token token = currentToken;
            if (currentToken.Type == TokenType.Text)
            {
                Eat(TokenType.Text);
                result = token.Value.ToString()!;
                return result;
            }
            else if (currentToken.Type == TokenType.Number)
            {
                Eat(TokenType.Number);
                result = (double)token.GetValue();
                return result;
            }
            else if (currentToken.GetType() == TokenType.Punctuator)
            {
                if (currentToken.GetValue().ToString() != "(") Error("se esperaba (");

                Eat(TokenType.Punctuator);
                result = ParseExpression(); //revisar aqui !!!
                if (currentToken.Value.ToString() != ")") Error("se esperaba )");
                Eat(TokenType.Punctuator);
                return result;
                //else saltar un error : se esperaba )
                // else System.Console.WriteLine("error : se esperaba )");


            }
            // if (currentToken.Type == TokenType.KeyWords)
            // {
            if (currentToken.Value.ToString() == "true")
            {
                Eat(TokenType.KeyWords);
                return true;
            }
            else if (currentToken.Value.ToString() == "false")
            {
                Eat(TokenType.KeyWords);
                return false;
            }
            // else System.Console.WriteLine("aqui tiene que saltar un error");
            // }
            else if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() != "!") Error("Error Semantico : Operador no valido");

                Eat(TokenType.Operator);
                object expression = ParseExpression();
                if (!(expression is bool)) Error("Se esperaba una expresion de tipo bool");

                bool left = Convert.ToBoolean(expression);
                return !left;

                // else System.Console.WriteLine("aqui tiene que saltar un error:expresion no valida ");
            }
            else if (currentToken.Type == TokenType.Identifier)
            {
                string Identifier = currentToken.Value.ToString()!;
                Eat(TokenType.Identifier);                                              
                ScopeLetIn.Reverse();
                foreach (var item in ScopeLetIn)
                {
                    if (item.ContainsKey(Identifier)) return item[Identifier];

                }
                // ScopeLetIn.Reverse();

            }

            else if (currentToken.Value.ToString() == "let") return ParseLetInExpression();
            else if (currentToken.Value.ToString() == "function") return FunctionDeclaration();
            return ParseIfElseExpression();
        }
        public void Show(Dictionary<string, object> Local)
        {
            System.Console.WriteLine("LocalScope tiene " + Local.Count + " elementos");
            foreach (var element in Local)
            {
                System.Console.WriteLine(element.Key + "   " + element.Value);
            }
        }
     

        public object ParseIfElseExpression()
        {

            if (currentToken.Value.ToString() == "if")
            {

                Eat(TokenType.KeyWords);
                if (!(currentToken.Value.ToString() == "(")) Error("se esperaba (");

                object ifExpression = ParseExpression();
                if (!(ifExpression is bool))
                {
                    Error("la expresion debe ser booleana");
                }
                bool expr = Convert.ToBoolean(ifExpression);
                object ifExpr = ParseExpression();
                if (!(currentToken.Value.ToString() == "else"))
                {
                    System.Console.WriteLine("se esperaba un else");
                }
                Eat(TokenType.KeyWords);
                object elseExpression = ParseExpression();
                if (expr)
                {
                    return ifExpr;
                }
                else return elseExpression;
            }
            return 1;

        }
        public object ParseLetInExpression()
        {
            int contador = 0;

            Dictionary<string, object> LocalScope = new Dictionary<string, object>();
            Eat(TokenType.KeyWords);

            if (!(currentToken.Type == TokenType.Identifier)) Error(" se esperaba un identificador ");
            string Identifier = currentToken.Value.ToString()!;
            Eat(TokenType.Identifier);

            if (!(currentToken.Value.ToString() == "=")) Error("se esperaba = ");
            Eat(TokenType.Operator);
            object expr = ParseExpression();

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

                object expr2 = ParseExpression();

                if (LocalScope.ContainsKey(Identifier2))
                {
                    LocalScope[Identifier2] = expr2;

                }
                else LocalScope.Add(Identifier2, expr2);
                contador++;
            }

            ScopeLetIn.Add(LocalScope);

            if (!(currentToken.Value.ToString() == "in")) Error("se esperaba la expresion in ");
            Eat(TokenType.KeyWords);

            object inExpr = ParseExpression();

            ScopeLetIn.Remove(LocalScope);
            return inExpr;
        }

        public object FunctionDeclaration()//este metodo lo voy a llamar si el token en el que estoy tiene como value la palabra function
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
            return function;

        }
        public void AddFunction(Function function)
        {
            if (FuncionesDeclaradas.Contains(function)) Error("No se puede agregar la funcion, ya existe otra con el mismo nombre");
            FuncionesDeclaradas.Add(function);

        }

        private void Error(string s)
        {
            throw new Exception(s);
        }

    }
}
