using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class ExpressionStatement : AbstractStatement
    {
        public IExpression Expression { get; set; }

        public ExpressionStatement(IExpression expression)
        {
            Expression = expression;
        }
        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitExpressionStmt(this);
        }
    }
}
