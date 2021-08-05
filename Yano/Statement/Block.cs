using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class Block : AbstractStatement
    {
        public IList<AbstractStatement> Statements { get; set; }

        public Block(IList<AbstractStatement> statements)
        {
            Statements = statements;
        }
        public override T Accept<T>(IStatementVisitor<T> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }
    }
}
