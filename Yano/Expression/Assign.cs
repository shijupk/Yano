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
    public class Assign: IExpression
    {
        public Token Name { get; set; }
        public IExpression Vale { get; set; }

        private Assign(Token name, IExpression value)
        {
            Name = name;
            Vale = value;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitAssignExpr(this);
        }
    }
}