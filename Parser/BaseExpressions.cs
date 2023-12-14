using Hulk_Interpreter;
namespace Parser
{
    partial class Parser
    {
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
                // foreach (var elemt in ScopeLetIn)
                // {
                //     foreach (var kvp in elemt)
                //     {
                //         System.Console.WriteLine(elemt[kvp.Key]);
                //     }

                // }
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

    }

}