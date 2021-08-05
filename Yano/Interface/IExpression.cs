// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: IExpression.cs
// ----------------------------------------------------------------------------------------

namespace Yano.Interface
{
    public interface IExpression
    {
         T Accept<T>(IVisitor<T> visitor);
    }
}