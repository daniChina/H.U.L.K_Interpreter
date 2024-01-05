using ClassLibrary;

while (true)
{
    System.Console.Write(">");
    string text = Console.ReadLine()!;
    if (text.Length != 0)
    {
        try
        {

            Lexer lexer = new Lexer(text);
            List<Token> tokens = lexer.Tokenize();

            SyntaxAnalyzer analyzer = new SyntaxAnalyzer(tokens);
            analyzer.StartAnalyzeExpression();
            Parser parser = new Parser(tokens, new List<Dictionary<string, object>>());
            parser.IfElseMatches = analyzer.IfElseMatches;
            object parserResult = parser.ParseExpression();

            System.Console.WriteLine(parserResult);
        }
        catch (Exception mesage)
        {
            System.Console.WriteLine(mesage);
             continue;
        }

    }


}
