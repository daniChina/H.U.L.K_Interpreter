using Hulk_Interpreter;
string  text = Console.ReadLine()!;
Lexer lexer = new Lexer (text);
lexer.GetNextToken();
 Parser parser = new Parser (lexer.TokensList);
object result = parser.ParseBooleanExpressionLv1(); 
SyntaxAnalizer analizer = new SyntaxAnalizer(lexer.TokensList);
//System.Console.WriteLine(analizer.AnalyzeExpression());
lexer.Show();
System.Console.WriteLine(result);




