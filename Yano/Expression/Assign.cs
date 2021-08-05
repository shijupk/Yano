// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Assign.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Assign : IExpression
    {
        public Token Name { get; set; }
        public IExpression Value { get; set; }

        public Assign(Token name, IExpression value)
        {
            Name = name;
            Value = value;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitAssignExpr(this);
        }
    }
}