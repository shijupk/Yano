using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yano.Interface
{
    public abstract class AbstractStatement
    {
        public abstract T Accept<T>(IStatementVisitor<T> statementVisitor);
    }
}
