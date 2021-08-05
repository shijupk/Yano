using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Exception;
using Yano.Expression;
using Yano.Interface;

namespace Yano
{
    public class Interpreter : IVisitor<Object>
    {
        public object VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public string Interpret(IExpression expr)
        {
            try
            {
                object value = Evaluate(expr);
                return Stringify(value);
            }
            catch (RuntimeException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private string Stringify(object obj)
        {
            if (obj == null)
            {
                return "nil";
            }

            if (obj is double)
            {
                var text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }

                return text;
            }

            return obj.ToString();
        }
        public object VisitBinaryExpr(Binary expr)
        {
            Object left = Evaluate(expr.Left);
            Object right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left <= (double)right;
                case TokenType.BANG_EQUAL: return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
                case TokenType.MINUS:
                    checkNumberOperands(expr.Operator, left, right);
                    CheckNumberOperand(expr.Operator, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double numLeft && right is double numRight)
                    {
                        return numLeft + numRight;
                    }
                    if (left is string strLeft && right is string strRight)
                    {
                        return strLeft + strRight;
                    }
                    throw new RuntimeException(expr.Operator,
                        "Operands must be two numbers or two strings.");
                case TokenType.SLASH:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double)left * (double)right;
            }

            return null;
        }

        private void CheckNumberOperand(Token opToken, object operand)
        {
            if (operand is double)
            {
                return;
            }

            throw new RuntimeException(opToken, "Operand must be number.");
        }
        private void checkNumberOperands(Token opToken,
            object left, object right)
        {
            if (left is double && right is double)
            {
                return;
            }

            throw new RuntimeException(opToken, "Operands must be numbers.");
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public object VisitCallExpr(Call expr)
        {
            throw new NotImplementedException();
        }

        public object VisitGetExpr(Get expr)
        {
            throw new NotImplementedException();
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr);
        }

        private object Evaluate(IExpression expr)
        {
            return expr.Accept(this);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            throw new NotImplementedException();
        }

        public object VisitSetExpr(Set expr)
        {
            throw new NotImplementedException();
        }

        public object VisitSuperExpr(Super expr)
        {
            throw new NotImplementedException();
        }

        public object VisitThisExpr(This expr)
        {
            throw new NotImplementedException();
        }

        public object VisitUnaryExpr(Unary expr)
        {
            Object right = Evaluate(expr.Right);
            switch (expr.Operator.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    return -(double) right;

            }

            return null;
        }

        private bool IsTruthy(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is bool b)
            {
                return b;
            }

            return true;
        }

        public object VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }
    }
}
