// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Unary.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Unary : IExpression
    {
        public Token Operator { get; set; }
        public IExpression Right { get; set; }

        public Unary(Token operatorToken, IExpression right)
        {
            Operator = operatorToken;
            Right = right;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}