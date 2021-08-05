// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: IfStatement.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class IfStatement : AbstractStatement
    {
        public IExpression Condition { get; set; }
        public AbstractStatement ThenBranch { get; set; }
        public AbstractStatement ElseBranch { get; set; }

        public IfStatement(IExpression condition, AbstractStatement thenBranch, AbstractStatement elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitIfStmt(this);
        }
    }
}