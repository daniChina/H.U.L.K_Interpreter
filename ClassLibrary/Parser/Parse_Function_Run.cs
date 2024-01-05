namespace ClassLibrary;
public partial class Parser
{
    private object ParseFunction(string functionName)
    {
        Dictionary<string, object> arguments = new Dictionary<string, object>();
        var function = Global.Functions[functionName];
        for (int i = 0; i < function.Arguments.Count; i++)
        {
            arguments.Add(function.Arguments[i].Item1, ParseExpression());
            if(currentToken.Type==TokenType.Comma)
            {
             Eat(TokenType.Comma);
            }
        }
        Eat(TokenType.RightParenthesis);
        function.Scope = Scope;
        function.Scope.Add(arguments);
        return function.Evaluate();
    }
}