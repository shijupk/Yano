// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Return.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class Return : AbstractStatement
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