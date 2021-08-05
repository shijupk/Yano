// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: This.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class This : IExpression
    {
        public Token Keyword { get; set; }

        public This(Token keyword)
        {
            Keyword = keyword;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitThisExpr(this);
        }
    }
}