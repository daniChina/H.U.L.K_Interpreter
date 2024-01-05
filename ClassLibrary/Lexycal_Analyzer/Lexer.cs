namespace ClassLibrary;

// Un Lexer recorre el código y lo divide en tokens
public class Lexer
{
    #region  Lexer Object

    public readonly string sourceCode;
    private int currentPosition;

    public Lexer(string sourceCode)
    {
        this.sourceCode = sourceCode;
        currentPosition = 0;
    }

    #endregion

    #region Lexer Main Function: Tokenize

    // Primero se necesita dividir los Tokens
    // Entonces creo la funcion Tokenize,  que retorna una lista de Tokens.

    public List<Token> Tokenize()
    {
        // Inicializar la Lista
        List<Token> tokens = new();

        while (currentPosition < sourceCode.Length)
        {
            char currentChar = sourceCode[currentPosition];

            // Si es un espacio en blanco , lo salto y continuo el ciclo
            if (char.IsWhiteSpace(currentChar))
            {
                currentPosition++;
                continue;
            }

            // Agregar si es un identificador 
            else if (IsLetter(currentChar))
            {
                tokens.Add(IdKind());
            }

            // Agregar los string
            else if (currentChar == '"')
            {
                tokens.Add(String());
            }

            // Agregar si es un numero
            else if (char.IsDigit(currentChar))
            {
                tokens.Add(Number());
            }

            // Aagregar si es un operador 
            else if (IsOperator(currentChar))
            {
                tokens.Add(Operator());
            }

            // Agregar si es un punctuador
            else if (IsPunctuator(currentChar))
            {
                tokens.Add(Punctuator());
            }

           //En este caso serian los token que no cumplen ninguna de las condiciones anteriores
            else
            {
                tokens.Add(new CommonToken(TokenType.Unknown, currentChar.ToString()));
                Console.WriteLine($"!lexical error: \"{tokens.Last()}\" is not a valid token.");
                currentPosition++;
            }
        }

        if (tokens.Last().Type is not TokenType.Semicolon)
        {
            Console.WriteLine("!syntax error: expression must end with \";\".");
            throw new Exception();
        }
        return tokens;
    }

    #endregion

    #region TokenKind Adder Functions

    private Token Number()
    {

        string number = "";

        while ((currentPosition < sourceCode.Length) && (char.IsDigit(sourceCode[currentPosition]) || sourceCode[currentPosition] == '.'))
        {
            number += sourceCode[currentPosition];

            if (IsLetter(LookAhead(1)))
            {
                Console.WriteLine($"!lexical error: \"{number + LookAhead(1)}\" is not a valid token.");
                throw new Exception();
            }

            currentPosition++;
        }

        return new Data(TokenType.Number, Double.Parse(number));
    }

    private Token IdKind()
    {

        string idkind = "";

        while (currentPosition < sourceCode.Length && (IsLetterOrDigit(sourceCode[currentPosition]) || sourceCode[currentPosition] == '_'))
        {
            idkind += sourceCode[currentPosition];
            currentPosition++;
        }
         //en este caso verifico primero si es una palabra reservada 
        if (IsKeyWord(idkind))
        {
            return KeyWord(idkind);
        }

        else
        {
            //en este caso se crea un identificador
            return new CommonToken(TokenType.Identifier, idkind);
        }
    }

    private Token String()
    {
        currentPosition++;
        string str = "";

        while (currentPosition < sourceCode.Length && sourceCode[currentPosition] != '"')
        {
            str += sourceCode[currentPosition];
            currentPosition++;
        }

        MoveNext();
        return new Data(TokenType.String, str);
    }

    private Token Operator()
    {
        char _operator = sourceCode[currentPosition];

        if (_operator == '+')
        {
            MoveNext();
            return new CommonToken(TokenType.Addition, _operator.ToString());
        }

        else if (_operator == '-')
        {
            MoveNext();
            return new CommonToken(TokenType.Substraction, _operator.ToString());
        }

        else if (_operator == '*')
        {
            MoveNext();
            return new CommonToken(TokenType.Multiplication, _operator.ToString());
        }

        else if (_operator == '/')
        {
            MoveNext();
            return new CommonToken(TokenType.Division, _operator.ToString());
        }

        else if (_operator == '^')
        {
            MoveNext();
            return new CommonToken(TokenType.Power, _operator.ToString());
        }

        else if (_operator == '%')
        {
            MoveNext();
            return new CommonToken(TokenType.Modulus, _operator.ToString());
        }

        else if (_operator == '@')
        {
            MoveNext();
            return new CommonToken(TokenType.Concat, _operator.ToString());
        }

        else if (_operator == '<' && LookAhead(1) == '=')
        {
            MoveNext(2);
            return new CommonToken(TokenType.LessOrEquals, "<=");
        }

        else if (_operator == '<')
        {
            MoveNext();
            return new CommonToken(TokenType.LessThan, _operator.ToString());
        }

        else if (_operator == '>' && LookAhead(1) == '=')
        {
            MoveNext(2);
            return new CommonToken(TokenType.GreaterOrEquals, ">=");
        }

        else if (_operator == '>')
        {
            MoveNext();
            return new CommonToken(TokenType.GreaterThan, _operator.ToString());
        }

        else if (_operator == '!' && LookAhead(1) == '=')
        {
            MoveNext(2);
            return new CommonToken(TokenType.NotEquals, "!=");
        }

        else if (_operator == '!')
        {
            MoveNext();
            return new CommonToken(TokenType.Not, _operator.ToString());
        }

        else if (_operator == '=' && LookAhead(1) == '=')
        {
            MoveNext(2);
            return new CommonToken(TokenType.EqualsTo, "==");
        }

        else if (_operator == '=' && LookAhead(1) == '>')
        {
            MoveNext(2);
            return new CommonToken(TokenType.Arrow, "=>");
        }

        else if (_operator == '=')
        {
            MoveNext();
            return new CommonToken(TokenType.Equals, _operator.ToString());
        }

        else if (_operator == '&')
        {
            MoveNext();
            return new CommonToken(TokenType.And, _operator.ToString());
        }

        else
        {
            MoveNext();
            return new CommonToken(TokenType.Or, _operator.ToString());
        }
    }

    private Token Punctuator()
    {
        char punctuator = sourceCode[currentPosition];
        switch (punctuator)
        {
            case '(':
                MoveNext();
                return new CommonToken(TokenType.LeftParenthesis, punctuator.ToString());

            case ')':
                MoveNext();
                return new CommonToken(TokenType.RightParenthesis, punctuator.ToString());

            case ',':
                MoveNext();
                return new CommonToken(TokenType.Comma, punctuator.ToString());

            case ';':
                MoveNext();
                return new CommonToken(TokenType.Semicolon, punctuator.ToString());

            case ':':
                MoveNext();
                return new CommonToken(TokenType.Colon, punctuator.ToString());

            case '"':
                MoveNext();
                return new CommonToken(TokenType.Quote, punctuator.ToString());
            default:
                MoveNext();
                return new CommonToken(TokenType.FullStop, punctuator.ToString());
        }
    }

    private Token KeyWord(string idkind)
    {
        switch (idkind)
        {
            case "let":
                return new Keyword(TokenType.LetKeyWord);

            case "in":
                return new Keyword(TokenType.InKeyWord);

            case "function":
                return new Keyword(TokenType.FunctionKeyWord);

            case "true":
                return new Data(TokenType.Bool, true);

            case "false":
                return new Data(TokenType.Bool, false);

            case "if":
                return new Keyword(TokenType.IfKeyWord);

            default:
                return new Keyword(TokenType.ElseKeyWord);

        }
    }

    #endregion

    #region Utility Functions 

    private void MoveNext(int positions)
    {
        currentPosition += positions;
    }

    private void MoveNext()
    {
        currentPosition ++;
    }

    private char LookAhead(int positions)
    {
        if ((currentPosition + positions) >= sourceCode.Length)
        {
            return ' ';
        }

        return sourceCode[currentPosition + positions];
    }

    private static bool IsLetter(char c) => char.IsLetter(c) || c == '_';

    private static bool IsLetterOrDigit(char c) => char.IsLetterOrDigit(c) || c == '_';

    private static bool IsOperator(char currentChar)
    {
        List<char> Operators = new()
            {
                '+', '-', '*', '/', '^','%','@',
                '=','<','>','!','|','&'
            };

        return Operators.Contains(currentChar);
    }

    private static bool IsPunctuator(char currentChar)
    {
        List<char> Punctuators = new()
            {
                '(', ')', ';', ',','.','"'
            };

        return Punctuators.Contains(currentChar);
    }

    private static bool IsKeyWord(string idkind)
    {
        List<string> keywords = new()
            {
                "let", "function",  "else",
                "in" , "if",        "true",
                "false"
            };

        return keywords.Contains(idkind);
    }

    #endregion
}

