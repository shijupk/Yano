// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: AstPrinter.cs
// ----------------------------------------------------------------------------------------

#region

using System;
using System.Text;
using Yano.Expression;
using Yano.Interface;

#endregion

namespace Yano
{
    public class AstPrinter : IExpressionVisitor<string>
    {
        public string VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public string VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(expr.Operator.Lexeme,
                expr.Left, expr.Right);
        }

        public string VisitCallExpr(Call expr)
        {
            throw new NotImplementedException();
        }

        public string VisitGetExpr(Get expr)
        {
            throw new NotImplementedException();
        }

        public string VisitGroupingExpr(Grouping expr)
        {
            return Parenthesize("group", expr.Expression);
        }

        public string VisitLiteralExpr(Literal expr)
        {
            if (expr.Value == null)
            {
                return "nil";
            }

            return expr.Value.ToString();
        }

        public string VisitLogicalExpr(Logical expr)
        {
            throw new NotImplementedException();
        }

        public string VisitSetExpr(Set expr)
        {
            throw new NotImplementedException();
        }

        public string VisitSuperExpr(Super expr)
        {
            throw new NotImplementedException();
        }

        public string VisitThisExpr(This expr)
        {
            throw new NotImplementedException();
        }

        public string VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(expr.Operator.Lexeme, expr.Right);
        }

        public string VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }

        public string Print(IExpression expression)
        {
            return expression.Accept(this);
        }

        private string Parenthesize(string name, params IExpression[] expressions)
        {
            var builder = new StringBuilder();
            builder.Append("(").Append(name);
            foreach (var expr in expressions)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }

            builder.Append(")");
            return builder.ToString();
        }
    }
}