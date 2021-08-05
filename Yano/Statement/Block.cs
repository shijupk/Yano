// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Block.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Interface;

#endregion

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