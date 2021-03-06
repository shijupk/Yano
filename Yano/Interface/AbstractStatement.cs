// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: AbstractStatement.cs
// ----------------------------------------------------------------------------------------

namespace Yano.Interface
{
    public abstract class AbstractStatement
    {
        public abstract T Accept<T>(IStatementVisitor<T> statementVisitor);
    }
}