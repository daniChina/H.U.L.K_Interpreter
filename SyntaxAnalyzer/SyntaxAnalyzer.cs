using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace SyntaxAnalyzer
{
    public partial class SyntaxAnalyzer
    {
        public int position;
        public Token currentToken;
        public int tokenIndex;
        List<Token> tokenList = new List<Token>();
        List<Dictionary<string, TokenType>> Scope = new List<Dictionary<string, TokenType>>();
        public List<Token> FunctionParameters = new List<Token>();
        public List<Token> FunctionBody = new List<Token>();
        List<Dictionary<string, object>> ScopeLetIn = new List<Dictionary<string, object>>();

        Dictionary<string, Function> FuncionesDeclaradas = new Dictionary<string, Function>();



        public SyntaxAnalyzer(List<Token> TokenList)
        {
            position = 0;
            tokenList = TokenList;
            currentToken = tokenList[position];
            tokenIndex = 0;

        }
        private void Eat(TokenType tokenType)
        {
            if (currentToken.Type != TokenType.EndOfLine && currentToken.GetType() == tokenType)
            {
                GetNextToken();

            }
            else Error(" Se esperaba el token" + tokenType);
        }

        private void GetNextToken()
        {
            if (tokenIndex < tokenList.Count)
            {
                tokenIndex++;
                currentToken = tokenList[tokenIndex];

            }
        }
        private void Error(string mensaje)
        {
            throw new Exception("Syntax Error : " + mensaje);
        }

        public TokenType AnalyzeExpression()
        {
            TokenType result = AnalyzeBooleanExpressionLv1();
            return result;
        }
    }
}