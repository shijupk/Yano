// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Set.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Set : IExpression
    {
        public Token Keyword { get; set; }
        public Token Method { get; set; }

        public Set(Token keyword, Token method)
        {
            Keyword = keyword;
            Method = method;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitSetExpr(this);
        }
    }
}