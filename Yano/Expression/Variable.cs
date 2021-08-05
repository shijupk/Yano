// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Variable.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Variable : IExpression
    {
        public Token Name { get; set; }

        public Variable(Token name)
        {
            Name = name;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitVariableExpr(this);
        }
    }
}