using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class Token
{
    public TokenType Type;
    public object Value;

    public Token(TokenType type, object value)
    {
        Type = type;
        Value = value;
    }


    public TokenType GetType() => Type;
    public object GetValue() => Value;

    public void Show()
    {
        System.Console.WriteLine("(" + Type + "," + Value + ")");
    }

    public override string ToString()
    {
        return $"Token({Type}, {Value})";
    }

}


