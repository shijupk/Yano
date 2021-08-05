using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class IfStatement:AbstractStatement
    {
        private IExpression Condition { get; set; }
        private AbstractStatement ThenBranch { get; set; }
        private AbstractStatement ElseBranch { get; set; }

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
