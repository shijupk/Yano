// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Var.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class Var : AbstractStatement
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