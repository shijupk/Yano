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
        public IExpression Object { get; set; }
        public Token Name { get; set; }
        public IExpression Value { get; set; }

        public Set(IExpression obj, Token name, IExpression value)
        {
            Object = obj;
            Name = name;
            Value = value;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitSetExpr(this);
        }
    }
}