using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Interface;

namespace Yano.Statement
{
    public class Return: AbstractStatement
    {
        public Token Keyword { get; set; }
        public IExpression Value { get; set; }

        public Return(Token keyword, IExpression value)
        {
            Keyword = keyword;
            Value = value;
        }
        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitReturnStmt(this);
        }
    }
}
