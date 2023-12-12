using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hulk_Interpreter
{
    public class Token
    {
        public TokenType Type;
        public object Value;

        public Token(TokenType type, object value)
        {
            this.Type = type;
            this.Value = value;
        }


        public new TokenType GetType()
        {
            return Type;
        }

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
}