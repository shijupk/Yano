using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class Var: AbstractStatement
    {
        public Token Name { get; set; }
        public IExpression Initializer { get; set; }

        public Var(Token name, IExpression initializer)
        {
            Name = name;
            Initializer = initializer;
        }
        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitVarStmt(this);
        }

    }
}
