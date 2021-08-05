// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: ExpressionStatement.cs
// ----------------------------------------------------------------------------------------

#region

using Yano.Interface;

#endregion

namespace Yano.Statement
{
    public class ExpressionStatement : AbstractStatement
    {
        public IExpression Expression { get; set; }

        public ExpressionStatement(IExpression expression)
        {
            Expression = expression;
        }

        public override T Accept<T>(IStatementVisitor<T> statementVisitor)
        {
            return statementVisitor.VisitExpressionStmt(this);
        }
    }
}