// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Expression.cs
// ----------------------------------------------------------------------------------------

namespace Yano.Interface
{
    public abstract class DNDExpression: IExpression
    {
       public abstract T Accept<T>(IVisitor<T> visitor);
    }
}