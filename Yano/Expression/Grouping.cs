// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Grouping.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Grouping : IExpression
    {
        public IExpression Expression { get; set; }

        public Grouping(IExpression expression)
        {
            Expression = expression;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitGroupingExpr(this);
        }
    }
}