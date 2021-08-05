// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Literal.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Literal : IExpression
    {
        public object Value { get; set; }

        public Literal(object value)
        {
            Value = value;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}