// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Call.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Call : IExpression
    {
        public IList<IExpression> Arguments { get; set; }
        public IExpression Callee { get; set; }
        public Token Parenthesis { get; set; }

        public Call(IExpression callee, Token parenthesis, IList<IExpression> arguments)
        {
            Callee = callee;
            Parenthesis = parenthesis;
            Arguments = arguments;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitCallExpr(this);
        }
    }
}