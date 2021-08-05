using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Yano.Expression;
using Yano.Interface;

namespace Yano
{
    public class AstPrinter :IVisitor<string>
    {
        public string Print(IExpression expression)
        {
            return expression.Accept(this);
        }

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

        private string Parenthesize(string name, params IExpression[] expressions)
        {
            StringBuilder builder = new StringBuilder();
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
