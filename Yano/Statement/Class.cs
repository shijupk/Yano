using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Expression;
using Yano.Interface;

namespace Yano.Statement
{
    public class Class : AbstractStatement
    {
        public Token Name { get; set; }
        public Variable SuperClass { get; set; }
        private IList<Function> Methods { get; set; }

        public Class(Token name, Variable superClass, IList<Function> methods)
        {
            Name = Name;
            SuperClass = superClass;
            Methods = methods;
        }
        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
           return statementVisitor.VisitClassStmt(this);
        }
    }
}
