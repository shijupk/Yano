using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Statement;

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
