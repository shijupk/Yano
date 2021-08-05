using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Exception;
using Yano.Expression;
using Yano.Interface;
using Yano.Statement;

namespace Yano
{
    public class Interpreter : IExpressionVisitor<Object>, IStatementVisitor<Object>
    {
        private Environment _environment = new Environment();



        public void Interpret(IList<AbstractStatement> statements)
        {
            try
            {
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            catch (RuntimeException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void Execute(AbstractStatement statement)
        {
            statement.Accept(this);
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

        public object VisitAssignExpr(Assign expr)
        {
            object value = Evaluate(expr.Value);
            _environment.Assign(expr.Name, value);
            return value;
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
            return _environment.Get(expr.Name);
        }

        public object VisitBlockStmt(Block stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitClassStmt(Class stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitExpressionStmt(ExpressionStatement stmt)
        {
            Evaluate(stmt.Expression);
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitIfStmt(IfStatement stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitPrintStmt(Print stmt)
        {
            object value = Evaluate(stmt.Expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitReturnStmt(Return stmt)
        {
            throw new NotImplementedException();
        }

        public object VisitVarStmt(Var stmt)
        {
            object value = null;
            if (stmt.Initializer != null)
            {
                value = Evaluate(stmt.Initializer);
            }
            _environment.Define(stmt.Name.Lexeme, value);
            return null;
        }

        public object VisitWhileStmt(While stmt)
        {
            throw new NotImplementedException();
        }
    }
}
