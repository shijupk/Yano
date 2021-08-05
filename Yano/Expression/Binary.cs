// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Binary.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Binary : IExpression
    {
        public IExpression Left { get; set; }
        public Token Operator { get; set; }
        public IExpression Right { get; set; }

        public Binary(IExpression left, Token operatorToken, IExpression right)
        {
            Left = left;
            Operator = operatorToken;
            Right = right;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
}