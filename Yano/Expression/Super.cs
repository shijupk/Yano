// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Super.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Expression
{
    public class Super : IExpression
    {
        public Token Keyword { get; set; }
        public Token Method { get; set; }

        public Super(Token keyword, Token method)
        {
            Keyword = keyword;
            Method = method;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitSuperExpr(this);
        }
    }
}