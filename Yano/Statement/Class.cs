// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Class.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Expression;
using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class Class : AbstractStatement
    {
        public Token Name { get; set; }
        public Variable SuperClass { get; set; }
        public IList<Function> Methods { get; set; }

        public Class(Token name, Variable superClass, IList<Function> methods)
        {
            Name = name;
            SuperClass = superClass;
            Methods = methods;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitClassStmt(this);
        }
    }
}