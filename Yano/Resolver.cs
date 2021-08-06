using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Expression;
using Yano.Interface;
using Yano.Statement;

namespace Yano
{
    public class Resolver : IStatementVisitor<object>, IExpressionVisitor<object>
    {

        private enum FunctionType
        {
            NONE,
            FUNCTION,
            METHOD
        }

        private FunctionType _currentFunction = FunctionType.NONE;

        private Interpreter _interpreter;
        private Stack<IDictionary<string, bool>> _scopes = new Stack<IDictionary<string, bool>>();
        public Resolver(Interpreter interpreter)
        {
            _interpreter = interpreter;
        }
        public object VisitAssignExpr(Assign expr)
        {
            Resolve(expr.Value);
            ResolveLocal(expr, expr.Name);
            return null;
        }

        public object VisitBinaryExpr(Binary expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        public object VisitBlockStmt(Block stmt)
        {
            BeginScope();
            Resolve(stmt.Statements);
            EndScope();
            return null;
        }

        private void BeginScope()
        {
            _scopes.Push(new Dictionary<string, bool>());
        }

        private void EndScope()
        {
            _scopes.Pop();
        }

        public void Resolve(IList<AbstractStatement> statements)
        {
            foreach (var statement in statements)
            {
                Resolve(statement);
            }
        }

        private void Resolve(AbstractStatement statement)
        {
            statement.Accept(this);
        }

        private void Resolve(IExpression expression)
        {
            expression.Accept(this);
        }


        public object VisitCallExpr(Call expr)
        {
           Resolve(expr.Callee);
           foreach (var argument in expr.Arguments)
           {
               Resolve(argument);
           }

           return null;
        }

        public object VisitClassStmt(Class stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);

            foreach (var method in stmt.Methods)
            {
                var declaration = FunctionType.METHOD;
                ResolveFunction(method, declaration);
            }
            return null;
        }

        public object VisitExpressionStmt(ExpressionStatement stmt)
        {
            Resolve(stmt.Expression);
            return null;
        }

        public object VisitFunctionStmt(Function stmt)
        {
            Declare(stmt.Name);
            Define(stmt.Name);

            ResolveFunction(stmt, FunctionType.FUNCTION);
            return null;
        }

        private void ResolveFunction(Function function, FunctionType type)
        {
            var enclosingFunction = _currentFunction;
            _currentFunction = type;

            BeginScope();
            foreach (var parameter in function.Parameters)
            {
                Declare(parameter);
                Define(parameter);
            }
            Resolve(function.Body);
            EndScope();
            _currentFunction = enclosingFunction;
        }

        public object VisitGetExpr(Get expr)
        {
            Resolve(expr.Object);
            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            Resolve(expr.Expression);
            return null;
        }

        public object VisitIfStmt(IfStatement stmt)
        {
            Resolve(stmt.Condition);
            Resolve(stmt.ThenBranch);
            if (stmt.ElseBranch != null)
            {
                Resolve(stmt.ElseBranch);
            }

            return null;
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return null;
        }

        public object VisitLogicalExpr(Logical expr)
        {
            Resolve(expr.Left);
            Resolve(expr.Right);
            return null;
        }

        public object VisitPrintStmt(Print stmt)
        {
            Resolve(stmt.Expression);
            return null;
        }

        public object VisitReturnStmt(Return stmt)
        {
            if (_currentFunction == FunctionType.NONE)
            {
                Yano.Error(stmt.Keyword, "Can't return from top-level code.");
            }

            if (stmt.Value != null)
            {
                Resolve(stmt.Value);
            }

            return null;
        }

        public object VisitSetExpr(Set expr)
        {
            Resolve(expr.Value);
            Resolve(expr.Object);
            return null;
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
            Resolve(expr.Right);
            return null;
        }

        public object VisitVariableExpr(Variable expr)
        {
            if (_scopes.Count != 0 && _scopes.Peek()[expr.Name.Lexeme] == false)
            {
                Yano.Error(expr.Name, "Can't read local variable in its own initializer.");
            }

            ResolveLocal(expr, expr.Name);
            return null;
        }

        private void ResolveLocal(IExpression expr, Token name)
        {
            for (var i = _scopes.Count - 1; i >= 0; i--)
            {
                if (_scopes.ElementAt(i).ContainsKey(name.Lexeme))
                {
                    _interpreter.Resolve(expr, _scopes.Count - 1 - i);
                    return;
                }
            }
        }

        public object VisitVarStmt(Var stmt)
        {
            Declare(stmt.Name);
            if (stmt.Initializer != null)
            {
                Resolve(stmt.Initializer);
            }

            Define(stmt.Name);
            return null;
        }

        private void Define(Token name)
        {
            if (_scopes.Count == 0)
            {
                return;
            }

            var scope = _scopes.Peek();
            scope[name.Lexeme] = true;
        }

        private void Declare(Token name)
        {
            if (_scopes.Count == 0)
            {
                return;
            }

            var scope = _scopes.Peek();
            if (scope.ContainsKey(name.Lexeme))
            {
                Yano.Error(name,"Already a variable with this name in this scope.");
            }
            scope[name.Lexeme] = false;
        }

        public object VisitWhileStmt(While stmt)
        {
           Resolve(stmt.Condition);
           Resolve(stmt.Body);
           return null;
        }
    }
}
