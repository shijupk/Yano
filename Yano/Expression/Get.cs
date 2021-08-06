// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Get.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Get : IExpression
    {
        public IExpression Object { get; set; }
        public Token Name { get; set; }

        public Get(IExpression obj, Token token)
        {
            Object = obj;
            Name = token;
        }

        public T Accept<T>(IExpressionVisitor<T> expressionVisitor)
        {
            return expressionVisitor.VisitGetExpr(this);
        }
    }
}