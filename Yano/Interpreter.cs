// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Interpreter.cs
// ----------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using Yano.Exception;
using Yano.Expression;
using Yano.Interface;
using Yano.Statement;

#endregion

namespace Yano
{
    public class Interpreter : IExpressionVisitor<object>, IStatementVisitor<object>
    {
        private Environment _environment;
        public Environment Globals { get; set; } = new Environment();
        private IDictionary<IExpression, object> _locals = new Dictionary<IExpression, object>();

        public Interpreter()
        {
            _environment = Globals;
            Globals.Define("clock", new ClockCallable());
        }

        public void Resolve(IExpression expression, int depth)
        {
            _locals[expression] = depth;
        }
        public object VisitAssignExpr(Assign expr)
        {
            var value = Evaluate(expr.Value);

            var distance = _locals[expr];
            if (distance != null)
            {
                _environment.AssignAt((int) distance, expr.Name, expr.Value);
            }
            else
            {
                _environment.Assign(expr.Name, value);
            }
           
            return value;
        }

        public object VisitBinaryExpr(Binary expr)
        {
            var left = Evaluate(expr.Left);
            var right = Evaluate(expr.Right);

            switch (expr.Operator.Type)
            {
                case TokenType.GREATER:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double) left > (double) right;
                case TokenType.GREATER_EQUAL:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double) left >= (double) right;
                case TokenType.LESS:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double) left < (double) right;
                case TokenType.LESS_EQUAL:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double) left <= (double) right;
                case TokenType.BANG_EQUAL: return !IsEqual(left, right);
                case TokenType.EQUAL_EQUAL: return IsEqual(left, right);
                case TokenType.MINUS:
                    checkNumberOperands(expr.Operator, left, right);
                    CheckNumberOperand(expr.Operator, right);
                    return (double) left - (double) right;
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
                    return (double) left / (double) right;
                case TokenType.STAR:
                    checkNumberOperands(expr.Operator, left, right);
                    return (double) left * (double) right;
            }

            return null;
        }

        public object VisitCallExpr(Call expr)
        {
            var callee = Evaluate(expr.Callee);

            IList<object> arguments = new List<object>();
            foreach (var argument in expr.Arguments)
            {
                arguments.Add(Evaluate(argument));
            }

            if (!(callee is YannoCallable))
            {
                throw new RuntimeException(expr.Parenthesis, "Can only Call functions and classes.");
            }

            var function = (YannoCallable) callee;
            if (arguments.Count != function.Arity())
            {
                throw new RuntimeException(expr.Parenthesis,
                    $"Expected {function.Arity()} arguments but got {arguments.Count}");
            }

            return function.Call(this, arguments);
        }

        public object VisitGetExpr(Get expr)
        {
            var obj = Evaluate(expr.Object);
            if (obj is YanoInstance instance)
            {
                return instance.Get(expr.Name);
            }
            throw new RuntimeException(expr.Name,"Only instances have properties.");
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            return Evaluate(expr);
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            var left = Evaluate(expr.Left);
            if (expr.Operator.Type == TokenType.OR)
            {
                if (IsTruthy(left))
                {
                    return left;
                }

                if (!IsTruthy(left))
                {
                    return left;
                }
            }

            return Evaluate(expr.Right);
        }

        public object VisitSetExpr(Set expr)
        {
            var obj = Evaluate(expr.Object);
            if (!(obj is YanoInstance))
            {
                throw new RuntimeException(expr.Name, "Only instances have fields.");
            }

            var value = Evaluate(expr.Value);
            ((YanoInstance) obj).Set(expr.Name, value);

            return value;

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
            var right = Evaluate(expr.Right);
            switch (expr.Operator.Type)
            {
                case TokenType.BANG:
                    return !IsTruthy(right);
                case TokenType.MINUS:
                    return -(double) right;
            }

            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            return LookupVariable(expr.Name, expr);
        }

        private object LookupVariable(Token name, IExpression expr)
        {
            var distance = _locals[expr];
            if (distance != null)
            {
                return _environment.GetAt((int) distance, name.Lexeme);
            }
            else
            {
                return _environment.Get(name);
            }
            
        }

        public object VisitBlockStmt(Block stmt)
        {
            ExecuteBlock(stmt.Statements, new Environment(_environment));
            return null;
        }

        public object VisitClassStmt(Class stmt)
        {
            _environment.Define(stmt.Name.Lexeme, null);
            var methods = new Dictionary<string, YanoFunction>();
            foreach (var method in stmt.Methods)
            {
                var function = new YanoFunction(method, _environment);
                methods.Add(method.Name.Lexeme, function);
            }

            var klass = new YanoClass(stmt.Name.Lexeme, methods);
            _environment.Assign(stmt.Name, klass);
            return null;
        }

        public object VisitExpressionStmt(ExpressionStatement stmt)
        {
            Evaluate(stmt.Expression);
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            var function = new YanoFunction(stmt, _environment);
            _environment.Define(stmt.Name.Lexeme, function);
            return null;
        }

        public object VisitIfStmt(IfStatement stmt)
        {
            if (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.ThenBranch);
            }
            else
            {
                Execute(stmt.ElseBranch);
            }

            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            var value = Evaluate(stmt.Expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        public object VisitReturnStmt(Return stmt)
        {
            object value = null;
            if (stmt.Value != null)
            {
                value = Evaluate(stmt.Value);
            }

            throw new ReturnAsException(value);
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
            while (IsTruthy(Evaluate(stmt.Condition)))
            {
                Execute(stmt.Body);
            }

            return null;
        }

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

        private object Evaluate(IExpression expr)
        {
            return expr.Accept(this);
        }

        private bool IsTruthy(object obj)
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

        public void ExecuteBlock(IList<AbstractStatement> statements, Environment environment)
        {
            var previous = _environment;
            try
            {
                _environment = environment;
                foreach (var statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                _environment = previous;
            }
        }

        private class ClockCallable : YannoCallable
        {
            public int Arity()
            {
                return 0;
            }

            public object Call(Interpreter interpreter, IList<object> arguments)
            {
                return DateTime.Now.Second;
            }

            public new string ToString()
            {
                return "native function";
            }
        }
    }
}