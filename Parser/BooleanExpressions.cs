using Hulk_Interpreter;
namespace Parser
{
     partial class Parser
    {
        public object ParseBooleanExpressionLv1()
        {
            object leftSide = ParseBooleanExpressionLv2();
            object rightSide;

            if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "&")
                {
                    Eat(TokenType.Operator);
                    rightSide = ParseBooleanExpressionLv2();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);
                        return left && right;
                    }
                }
                else if (currentToken.Value.ToString() == "|")
                {
                    Eat(TokenType.Operator);
                    rightSide = ParseBooleanExpressionLv2();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);
                        return left || right;
                    }
                }
                else return leftSide;
            }
            return leftSide;

        }

        public object ParseBooleanExpressionLv2()
        {
            object leftSide = ConcatString();
            if (currentToken.Type == TokenType.Operator)
            {
                if (currentToken.Value.ToString() == "==")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);

                        return left == right;
                    }

                }
                else if (currentToken.Value.ToString() == "!=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is bool && rightSide is bool)
                    {
                        bool left = Convert.ToBoolean(leftSide);
                        bool right = Convert.ToBoolean(rightSide);

                        return left != right;
                    }

                }

                else if (currentToken.Value.ToString() == "<")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left < right;
                    }
                }
                else if (currentToken.Value.ToString() == "<=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        if (left <= right) return true;
                        else return false;
                    }
                }

                else if (currentToken.Value.ToString() == ">")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left > right;
                    }

                }
                else if (currentToken.Value.ToString() == ">=")
                {
                    Eat(TokenType.Operator);
                    object rightSide = ConcatString();
                    if (leftSide is Double && rightSide is Double)
                    {
                        double left = Convert.ToDouble(leftSide);
                        double right = Convert.ToDouble(rightSide);
                        return left >= right;
                    }

                }
                else return leftSide;

            }
            return leftSide;
        }


    }
}