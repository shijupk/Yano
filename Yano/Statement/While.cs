// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: While.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class While : AbstractStatement
    {
        public IExpression Condition { get; set; }
        public AbstractStatement Body { get; set; }

        public While(IExpression condition, AbstractStatement body)
        {
            Condition = condition;
            Body = body;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitWhileStmt(this);
        }
    }
}