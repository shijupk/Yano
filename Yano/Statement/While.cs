using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class While: AbstractStatement
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
