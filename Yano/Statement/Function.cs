// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Function.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class Function : AbstractStatement
    {
        public Token Name { get; set; }
        public IList<Token> Parameters { get; set; }
        public IList<AbstractStatement> Body { get; set; }

        public Function(Token name, IList<Token> parameters, IList<AbstractStatement> body)
        {
            Name = name;
            Parameters = parameters;
            Body = body;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitFunctionStmt(this);
        }
    }
}