public class Lexer
{
    private string text;
    private int position;
    private char currentChar;
    public List<Token> TokensList { get; }

    public Lexer(string text)
    {
        this.text = text;
        this.position = 0;
       this.currentChar = text[position];
        TokensList = new List<Token>();
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
            if (char.IsWhiteSpace(currentChar)) {
                GetNextPosition();
                continue;
            }

            //verificar si es un numero
            else if (char.IsDigit(currentChar))
            {
                string number = " ";
                while (position < text.Length && char.IsDigit(currentChar))
                {
                    number += currentChar;
                    GetNextPosition();
                }
                if (char.IsLetter(currentChar)) System.Console.WriteLine("error de lexico");
                Token token = new Token(TokenType.Number, double.Parse(number));
                TokensList.Add(token);

            }//Verificar los operadores
            else if (currentChar == '+' || currentChar == '-' || currentChar == '*' || currentChar == '/' || currentChar == '=' || currentChar == '@' || currentChar == '^')
            {
                Token token = new Token(TokenType.Operator, currentChar);
                TokensList.Add(token);
                GetNextPosition();

            }
            //  verificar si es un simbolo de agrupacion o puntuacion
            else if (currentChar == '(' || currentChar == ')' || currentChar == ';' || currentChar == ',' || currentChar == '{' || currentChar == '}')
            {
                Token token = new Token(TokenType.Punctuator, currentChar);
                TokensList.Add(token);
                GetNextPosition();

            }//verificar si es alguna palabra reservada del lenguaje 
            else if (char.IsLetter(currentChar))
            {


                string value = IsAWord();
                if (IsAKeyword(value) == true)
                {
                    Token token = new Token(TokenType.KeyWords, value);
                    TokensList.Add(token);

                }
                else
                {
                    Token token = new Token(TokenType.Identifier, value);
                    TokensList.Add(token);
                    position++;
                }

            }
            else if (currentChar == '_')
            {
                string value = IsAWord();
                Token token = new Token(TokenType.Identifier, value);

            }
            //verificar si la entada contiene algun texto y si es asi , crear un token tipo texto y agregarlo a la lista de tokens
            else if (currentChar == '"') TokensList.Add(IsAString());

        }
        TokensList.Add(new Token(TokenType.EndOfLine, null!));
    }



    private bool IsAKeyword(string s)
    {
        List<string> Id = new() { "function", "let", "in", "if ", "else", "true", "false" };
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
            System.Console.WriteLine("error lexico ");

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





}
