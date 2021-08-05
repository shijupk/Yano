// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: IStatementVisitor.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Statement;

#endregion

namespace Yano.Interface
{
    public interface IStatementVisitor<T>
    {
        T VisitBlockStmt(Block stmt);
        T VisitClassStmt(Class stmt);
        T VisitExpressionStmt(ExpressionStatement stmt);
        T VisitFunctionStmt(Function stmt);
        T VisitIfStmt(IfStatement stmt);
        T VisitPrintStmt(Print stmt);
        T VisitReturnStmt(Return stmt);
        T VisitVarStmt(Var stmt);
        T VisitWhileStmt(While stmt);
    }
}