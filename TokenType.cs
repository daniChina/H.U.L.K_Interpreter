namespace Hulk_Interpreter
{
    public enum TokenType
    {
        Operator,// + , - , * , @ , / , ^
        Number,
        Text,
        Identifier,
        KeyWords, // "funtion" , "if " ,"else ", "let", "in ", "true " ,false "
        KeyWord_Boolean,
        Punctuator,// ( , ) , { , } , ; , ", " 
        EndOfLine,
        EndOfToken,
    }

    class Operator : Token
    {
        object manolito;
        public Operator(TokenType type, object value) : base(type, value)
        {
            manolito = value;
        }
    }
}