namespace ClassLibrary;
public class Function
{
    public List<(string, object)> Arguments { get; set; }
    public List<Token> Body { get; set; }
    public List<string> ArgumentsName { get; set; }
    public List<Dictionary<string, object>> Scope { get; set; }
    public Function(List<Token> body)
    {
        Arguments = new List<(string, object)>();
        Body = body;
    }

    public object Evaluate()
    {
        Parser parser = new Parser(Body, Scope);
        object result = parser.ParseExpression();
        return result;
    }
}