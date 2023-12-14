using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hulk_Interpreter;

namespace Parser
{
    partial class Parser
    {

        public int position;
        public Token currentToken;
        public int tokenIndex;

        List<Token> tokenList = new List<Token>();
        List<Dictionary<string, object>> ScopeLetIn = new List<Dictionary<string, object>>();
        public List<Token> Parameters = new List<Token>();
        public List<Token> FunctionBody = new List<Token>();
        List<Function> FuncionesDeclaradas = new List<Function>();
        List<Token> Variables = new List<Token>();


        public Parser(List<Token> TokenList)
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
        private void GetPastToken()
        {
            if (tokenIndex > 0)
            {
                tokenIndex--;
                currentToken = tokenList[tokenIndex];
            }
        }
        private void Error(string s)
        {
            throw new Exception(s);
        }

        public object ParseExpression(){
            object result = ParseBooleanExpressionLv1();
            return result ;
        }



    }
}