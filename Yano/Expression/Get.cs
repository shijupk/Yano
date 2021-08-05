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
        public Token Token { get; set; }

        public Get(IExpression obj, Token token)
        {
            Object = obj;
            Token = token;
        }

        public T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitGetExpr(this);
        }
    }
}