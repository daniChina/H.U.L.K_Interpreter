public abstract class Token
{
    public TokenType Type { get; set; }

    public Token(TokenType type)
    {
        Type = type;
    }

    public TokenType GetKind() => Type;
    
    public void SetKind(TokenType kind) => Type = kind;

    public abstract string GetName();

    public abstract void SetName(string name);

    public abstract object GetValue();

    public abstract void SetValue(object value);


    public override string ToString() => $"{Type}";
}

public class Keyword : Token
{
    public Keyword(TokenType kind) : base(kind)
    {
        Type = kind;
    }

    public override string GetName() => Type.ToString();

    public override object GetValue() => throw new NotImplementedException();

    public override void SetName(string name) => throw new NotImplementedException();

    public override void SetValue(object value) => throw new NotImplementedException();

}

public class CommonToken : Token
{
    public string Representation { get; set; }

    public CommonToken(TokenType kind, string representation) : base(kind)
    {
        Representation = representation;
    }
    public override string GetName() => Representation;

    public override void SetName(string name) => Representation = name;

    public override object GetValue() => throw new NotImplementedException();

    public override void SetValue(object value) => throw new NotImplementedException();

    public override string ToString() => $"{base.Type}: {Representation}";
}

public class Data : Token
{
    public object Value { get; set; }

    public Data(TokenType kind, object value) : base(kind)
    {
        Value = value;
    }

    public override string GetName() => Value.ToString()!;

    public override void SetName(string name) => throw new NotImplementedException();

    public override object GetValue() => Value;

    public override void SetValue(object value) => Value = value;

    public override string ToString() => $"{base.Type}: {Value}";
}