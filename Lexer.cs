using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hulk_Interpreter
{
    public class Lexer
    {
        private string text;
        private int position;
        private char currentChar;

        // private Token currentToken ;
        public List<Token> TokensList = new List<Token>();

        public Lexer(string text)
        {
            this.text = text;
            this.position = 0;
            this.currentChar = text[position];
            //currentToken = null!;
        }
        public void Show()
        {

            foreach (Token item in TokensList)
            {
                item.Show();
            }
        }

        public void GetNextToken()
        {
            while (position < text.Length)
            {

                //si es un espacio en blanco salto y continuo el ciclo
                if (char.IsWhiteSpace(currentChar))
                {
                    SkipWhiteSpaces();
                    continue;
                }

                //verificar si es un numero
                else if (char.IsDigit(currentChar))
                {
                    string number = " ";
                    while (position < text.Length && char.IsDigit(currentChar))
                    {
                        number += currentChar;
                        // if (char.IsLetter(LookAhead(1))) System.Console.WriteLine("error de lexico");
                        GetNextPosition();
                    }
                    Token token = new Token(TokenType.Number, double.Parse(number));
                    TokensList.Add(token);


                }//Verificar los operadores
                else if (currentChar == '+' || currentChar == '-' || currentChar == '*' || currentChar == '/' || currentChar == '%' || currentChar == '@' || currentChar == '^' || currentChar == '&' || currentChar == '|')
                {
                    Token token = new Token(TokenType.Operator, currentChar);
                    TokensList.Add(token);
                    GetNextPosition();

                }
                else if (currentChar == '=')
                {
                    string s = currentChar.ToString();
                    if (LookAhead(position, '=', '>'))
                    {
                        s += currentChar;
                        Token token = new Token(TokenType.Operator, s);
                        TokensList.Add(token);
                        GetNextPosition();
                    }
                    else
                    {
                        Token token1 = new Token(TokenType.Operator, s);
                        TokensList.Add(token1);
                    }
                }
                else if (currentChar == '!')
                {
                    string s = currentChar.ToString();
                    if (LookAhead(position, '='))
                    {
                        s += currentChar;
                        Token token = new Token(TokenType.Operator, s);
                        TokensList.Add(token);
                        GetNextPosition();
                    }
                    else
                    {
                        Token token1 = new Token(TokenType.Operator, s);
                        TokensList.Add(token1);

                    }
                }
                else if (currentChar == '<')
                {
                    string s = currentChar.ToString();
                    if (LookAhead(position, '='))
                    {
                        s += currentChar;
                        Token token = new Token(TokenType.Operator, s);
                        TokensList.Add(token);
                        GetNextPosition();

                    }
                    else
                    {
                        Token token1 = new Token(TokenType.Operator, s);
                        TokensList.Add(token1);
                    }
                }
                else if (currentChar == '>')
                {
                    string s = currentChar.ToString();
                    if (LookAhead(position, '='))
                    {
                        s += currentChar;
                        Token token = new Token(TokenType.Operator, s);
                        TokensList.Add(token);
                        GetNextPosition();

                    }
                    else
                    {
                        Token token1 = new Token(TokenType.Operator, s);
                        TokensList.Add(token1);
                    }

                }
                //  verificar si es un simbolo de agrupacion o puntuacion
                else if (currentChar == '(' || currentChar == ')' || currentChar == ',')
                {
                    Token token = new Token(TokenType.Punctuator, currentChar);
                    TokensList.Add(token);
                    GetNextPosition();

                }
                //verificar si es alguna palabra reservada del lenguaje 
                else if (char.IsLetter(currentChar))
                {


                    string value = IsAWord();
                    if (IsAKeyword(value) == true)
                    {
                        if (value == "true" || value == "false")
                        {
                            Token token1 = new Token(TokenType.KeyWord_Boolean, value);
                            TokensList.Add(token1);
                        }

                        Token token = new Token(TokenType.KeyWords, value);
                        TokensList.Add(token);

                    }
                    else
                    {
                        Token token = new Token(TokenType.Identifier, value);
                        TokensList.Add(token);

                    }

                }
                else if (currentChar == '_')
                {
                    string value1 = IsAWord();
                    Token token = new Token(TokenType.Identifier, value1);

                }
                //verificar si la entada contiene algun texto y si es asi , crear un token tipo texto y agregarlo a la lista de tokens
                else if (currentChar == '"') TokensList.Add(IsAString());
                else if (currentChar == ';')
                {
                    TokensList.Add(new Token(TokenType.EndOfLine, currentChar));
                    GetNextPosition();
                }
                else Error("Lexical Error : token " + currentChar + " no valido");//aqui debe de saltar una excepcion

            }
            TokensList.Add(new Token(TokenType.EndOfToken, "EOT"));
        }

        private void SkipWhiteSpaces()
        {
            while (position < text.Length && char.IsWhiteSpace(currentChar))
            {
                GetNextPosition();
            }
        }


        private bool IsAKeyword(string s)
        {
            List<string> Id = new() { "function", "let", "in", "if", "else", "true", "false","sqrt","cos","sin","exp","log","rand", "print" };
            if (Id.Contains(s)) return true;
            return false;
        }
        private string IsAWord()
        {
            string s = "";
            while (position < text.Length && (char.IsLetterOrDigit(currentChar) || currentChar == '_'))
            {
                s += currentChar;
                GetNextPosition();

            }
            return s;
        }
        private Token IsAString()
        {
            GetNextPosition();
            string s = "";
            while (position < text.Length && currentChar != '"')
            {
                s += currentChar;
                GetNextPosition();
            }

            if (currentChar != '"')
            {
                Error("Se esperaba para concluir " + '"');

            }
            GetNextPosition();

            return new Token(TokenType.Text, s);
        }
        //Este metodo es para aumentar la posicion y actualizar el currentChar con la posicion 
        private void GetNextPosition()
        {
            position++;
            if (position < text.Length)
            {
                currentChar = text[position];
            }
        }
        private bool CanLookAhead(int next_positions)
        {
            int nextPos = position + next_positions;
            if (nextPos < text.Length)
            {
                return true;
            }
            return false;
        }
        private void LookAhead(int next_positions)
        {
            int nextPos = position + next_positions;
            if (CanLookAhead(next_positions))
            {
                currentChar = text[nextPos];
            }
            else currentChar = text[currentChar];

        }

        private bool LookAhead(int position, params char[] c)
        //este metodo recibe la posicion en la que estoy y los parametros con los que construyo un array para 
        {
            GetNextPosition();
            if (this.position != position && this.position < text.Length)
            {
                foreach (var item in c)
                {
                    if (currentChar == item)
                    {
                        System.Console.WriteLine(this.position);
                        System.Console.WriteLine(position);
                        return true;
                    }
                }
            }
            return false;
        }
        private void Error(string mensaje)
        {
            throw new Exception("Lexical Error : " + mensaje);
        }

    }
}