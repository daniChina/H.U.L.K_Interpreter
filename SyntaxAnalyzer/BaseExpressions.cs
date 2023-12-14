using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace SyntaxAnalyzer
{
    public partial class SyntaxAnalyzer
    {
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

            if (currentToken.Value.ToString() == "true" || currentToken.Value.ToString()=="false")
            {
                Eat(TokenType.KeyWords);
                return TokenType.KeyWord_Boolean;
            }
            else if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() != "!") Error("Error Semantico : Operador no valido");

                Eat(TokenType.Operator);
                TokenType expression = AnalyzeExpression();
                if (expression != TokenType.KeyWord_Boolean) Error("Se esperaba una expresion de tipo bool");
               return expression;
            }
            else if (currentToken.Value.ToString() == "let") return AnalyzeLetInExpression();
            else if (currentToken.Type == TokenType.Identifier)
            {
                string Identifier = currentToken.Value.ToString()!;
                Eat(TokenType.Identifier);
                ScopeLetIn.Reverse();
                foreach (var item in ScopeLetIn)
                {
                    if (item.ContainsKey(Identifier)) return item[Identifier];

                }
                ScopeLetIn.Reverse();

            }
            else if (currentToken.Value.ToString() == "function") return FunctionDeclaration();
            return AnalyzeIfElseExpression();
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

                if (FunctionParameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
                FunctionParameters.Add(currentToken);

                Eat(TokenType.Identifier);

                while (currentToken.Value.ToString() == ",")
                {
                    Eat(TokenType.Punctuator);
                    if (currentToken.Type != TokenType.Identifier) Error("Se esperaba un identificador");

                    if (FunctionParameters.Contains(currentToken)) Error("No se pueden pasar parametros con el mismo nombre");
                    FunctionParameters.Add(currentToken);

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

            if (currentToken.Type != TokenType.EndOfLine) Error("Toda expresion tiene que terminar con ;");
            FunctionBody.Add(currentToken);
            Eat(TokenType.EndOfLine);

            if (currentToken.Type != TokenType.EndOfToken) Error ("Despues del ; no puede haber ninguna expresion ");

            Function function = new Function(name, FunctionParameters, FunctionBody);
        }
        public void AddFunction(Function function)
        {
            if (FuncionesDeclaradas.ContainsKey(function.Name)) Error("No se puede agregar la funcion, ya existe otra con el mismo nombre");
            FuncionesDeclaradas.Add(function.Name,function);

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
