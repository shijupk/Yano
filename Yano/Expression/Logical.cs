// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Logical.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Logical : IExpression
    {
        public IExpression Left { get; set; }
        public Token Operator { get; set; }
        public IExpression Right { get; set; }

        public Logical(IExpression left, Token operatorToken, IExpression right)
        {
            Left = left;
            Operator = operatorToken;
            Right = right;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLogicalExpr(this);
        }
    }
}