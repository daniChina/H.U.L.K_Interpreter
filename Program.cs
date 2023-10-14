string entrada = Console.ReadLine()!;
Lexer lexer = new Lexer(entrada);
lexer.GetNextToken();
lexer.Show();

