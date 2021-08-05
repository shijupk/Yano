// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Print.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class Print : AbstractStatement
    {
        public IExpression Expression { get; set; }

        public Print(IExpression expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitPrintStmt(this);
        }
    }
}