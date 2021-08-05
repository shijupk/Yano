// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: IExpressionVisitor.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Expression;

#endregion

namespace Yano.Interface
{
    public interface IExpressionVisitor<T>
    {
        T VisitAssignExpr(Assign expr);
        T VisitBinaryExpr(Binary expr);
        T VisitCallExpr(Call expr);
        T VisitGetExpr(Get expr);
        T VisitGroupingExpr(Grouping expr);
        T VisitLiteralExpr(Literal expr);
        T VisitLogicalExpr(Logical expr);
        T VisitSetExpr(Set expr);
        T VisitSuperExpr(Super expr);
        T VisitThisExpr(This expr);
        T VisitUnaryExpr(Unary expr);
        T VisitVariableExpr(Variable expr);
    }
}