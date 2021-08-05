// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Parser.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Exception;
using Yano.Expression;
using Yano.Interface;
using Yano.Statement;

#endregion

namespace Yano
{
    public class Parser
    {
        private readonly IList<Token> _tokens;
        private int _current;

        public Parser(IList<Token> tokens)
        {
            _tokens = tokens;
        }

        public IList<AbstractStatement> Parse()
        {
            IList<AbstractStatement> statements = new List<AbstractStatement>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private AbstractStatement Declaration()
        {
            try
            {
                if (Match(TokenType.FUN))
                {
                    return Function("function");
                }

                if (Match(TokenType.VAR))
                {
                    return VarDeclaration();
                }

                return Statement();
            }
            catch (ParseException)
            {
                Synchronize();
                return null;
            }
        }

        private Function Function(string kind)
        {
            var name = Consume(TokenType.IDENTIFIER, $"Expect {kind} name.");
            Consume(TokenType.LEFT_PAREN, $"Expect '(' after {kind} name");
            var parameters = new List<Token>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (parameters.Count >= 255)
                    {
                        RaiseError(Peek(), "Can't have more than 255 parameters.");
                    }

                    parameters.Add(Consume(TokenType.IDENTIFIER, "Expect parameter name."));
                } while (Match(TokenType.COMMA));
            }

            Consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters.");

            Consume(TokenType.LEFT_BRACE, $"Expect '{{' before {kind} body. ");
            var body = Block();
            return new Function(name, parameters, body);
        }

        private AbstractStatement VarDeclaration()
        {
            var name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

            IExpression initializer = null;
            if (Match(TokenType.EQUAL))
            {
                initializer = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after variable declaration.");
            return new Var(name, initializer);
        }

        private AbstractStatement Statement()
        {
            if (Match(TokenType.FOR))
            {
                return ForStatement();
            }

            if (Match(TokenType.IF))
            {
                return IfStatement();
            }

            if (Match(TokenType.PRINT))
            {
                return PrintStatement();
            }

            if (Match(TokenType.RETURN))
            {
                return ReturnStatement();
            }

            if (Match(TokenType.WHILE))
            {
                return WhileStatement();
            }

            if (Match(TokenType.LEFT_BRACE))
            {
                return new Block(Block());
            }

            return ExpressionStatement();
        }

        private AbstractStatement ReturnStatement()
        {
            var keyword = Previous();
            IExpression value = null;
            if (!Check(TokenType.SEMICOLON))
            {
                value = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after return value.");
            return new Return(keyword, value);
        }

        private AbstractStatement ForStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'for'.");
            AbstractStatement initializer;
            if (Match(TokenType.SEMICOLON))
            {
                initializer = null;
            }
            else if (Match(TokenType.VAR))
            {
                initializer = VarDeclaration();
            }
            else
            {
                initializer = ExpressionStatement();
            }

            IExpression condition = null;
            if (!Check(TokenType.SEMICOLON))
            {
                condition = Expression();
            }

            Consume(TokenType.SEMICOLON, "Expect ';' after loop condition.");

            IExpression increment = null;
            if (!Check(TokenType.RIGHT_PAREN))
            {
                increment = Expression();
            }

            Consume(TokenType.RIGHT_PAREN, "Expect ')' after for clauses.");
            var body = Statement();
            if (increment != null)
            {
                var list = new List<AbstractStatement> {body, new ExpressionStatement(increment)};
                body = new Block(list);
            }

            if (condition == null)
            {
                condition = new Literal(true);
            }

            body = new While(condition, body);

            if (initializer != null)
            {
                var list = new List<AbstractStatement> {initializer, body};
                body = new Block(list);
            }

            return body;
        }

        private AbstractStatement WhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'while'.");
            var condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after condition.");
            var body = Statement();
            return new While(condition, body);
        }

        private AbstractStatement IfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect '(' after 'if'.");
            var condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition.");
            var thenStatement = Statement();
            AbstractStatement elseStatement = null;
            if (Match(TokenType.ELSE))
            {
                elseStatement = Statement();
            }

            return new IfStatement(condition, thenStatement, elseStatement);
        }

        private IList<AbstractStatement> Block()
        {
            var statements = new List<AbstractStatement>();
            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                statements.Add(Declaration());
            }

            Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
            return statements;
        }

        private AbstractStatement PrintStatement()
        {
            var value = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after value.");
            return new Print(value);
        }

        private AbstractStatement ExpressionStatement()
        {
            var expression = Expression();
            Consume(TokenType.SEMICOLON, "Expect ';' after expression.");
            return new ExpressionStatement(expression);
        }

        private IExpression Expression()
        {
            return Assignment();
        }

        private IExpression Assignment()
        {
            var expression = Or();
            if (Match(TokenType.EQUAL))
            {
                var equals = Previous();
                var value = Assignment();

                if (expression is Variable)
                {
                    var name = ((Variable) expression).Name;
                    return new Assign(name, value);
                }

                RaiseError(equals, "Invalid assignment target.");
            }

            return expression;
        }

        private IExpression Or()
        {
            var expression = And();
            while (Match(TokenType.AND))
            {
                var opToken = Previous();
                var right = And();
                expression = new Logical(expression, opToken, right);
            }

            return expression;
        }

        private IExpression And()
        {
            var expression = Equality();
            while (Match(TokenType.AND))
            {
                var opToken = Previous();
                var right = Equality();
                expression = new Logical(expression, opToken, right);
            }

            return expression;
        }

        /// <summary>
        ///     equality       → comparison ( ( "!=" | "==" ) comparison )* ;
        /// </summary>
        /// <returns></returns>
        private IExpression Equality()
        {
            var expression = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                var operatorToken = Previous();
                var right = Comparison();
                expression = new Binary(expression, operatorToken, right);
            }

            return expression;
        }

        private bool Match(params TokenType[] tokenTypes)
        {
            foreach (var tokenType in tokenTypes)
            {
                if (Check(tokenType))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                _current++;
            }

            return Previous();
        }

        private bool Check(TokenType tokenType)
        {
            if (IsAtEnd())
            {
                return false;
            }

            return Peek().Type == tokenType;
        }

        private Token Peek()
        {
            return _tokens[_current];
        }

        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        private Token Previous()
        {
            return _tokens[_current - 1];
        }

        /// <summary>
        ///     comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
        /// </summary>
        /// <returns></returns>
        private IExpression Comparison()
        {
            var teminalExpression = Term();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                var operatorToken = Previous();
                var rightExpression = Term();
                teminalExpression = new Binary(teminalExpression, operatorToken, rightExpression);
            }

            return teminalExpression;
        }

        /// <summary>
        ///     addition and subtraction:
        /// </summary>
        /// <returns></returns>
        private IExpression Term()
        {
            var expression = Factor();
            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                var operatorToken = Previous();
                var rightExpression = Factor();
                expression = new Binary(expression, operatorToken, rightExpression);
            }

            return expression;
        }

        private IExpression Factor()
        {
            var expression = Unary();
            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                var operatorToken = Previous();
                var rightExpression = Unary();
                expression = new Binary(expression, operatorToken, rightExpression);
            }

            return expression;
        }

        /// <summary>
        ///     unary          → ( "!" | "-" ) unary
        ///     | primary ;
        /// </summary>
        /// <returns></returns>
        private IExpression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                var operatorToken = Previous();
                var rightExpression = Unary();
                return new Unary(operatorToken, rightExpression);
            }

            return Call();
        }

        private IExpression Call()
        {
            var expression = Primary();
            while (true)
            {
                if (Match(TokenType.LEFT_PAREN))
                {
                    expression = FinishCall(expression);
                }
                else
                {
                    break;
                }
            }

            return expression;
        }

        private IExpression FinishCall(IExpression callee)
        {
            IList<IExpression> arguments = new List<IExpression>();

            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    if (arguments.Count >= 255)
                    {
                        RaiseError(Peek(), "Can't have more than 255 arguments.");
                    }

                    arguments.Add(Expression());
                } while (Match(TokenType.COMMA));
            }

            var parenthesis = Consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments.");
            return new Call(callee, parenthesis, arguments);
        }

        /// <summary>
        ///     primary        → NUMBER | STRING | "true" | "false" | "nil"
        ///     | "(" expression ")" ;
        /// </summary>
        /// <returns></returns>
        private IExpression Primary()
        {
            if (Match(TokenType.FALSE))
            {
                return new Literal(false);
            }

            if (Match(TokenType.TRUE))
            {
                return new Literal(true);
            }

            if (Match(TokenType.NIL))
            {
                return new Literal(null);
            }

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Literal(Previous().Literal);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new Variable(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                var expression = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression");
                return new Grouping(expression);
            }

            throw RaiseError(Peek(), "Expect expression");
        }

        private Token Consume(TokenType tokenType, string message)
        {
            if (Check(tokenType))
            {
                return Advance();
            }

            throw RaiseError(Peek(), message);
        }

        private ParseException RaiseError(Token token, string message)
        {
            Yano.Error(token, message);
            return new ParseException(message);
        }

        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Type == TokenType.SEMICOLON)
                {
                    return;
                }

                switch (Peek().Type)
                {
                    case TokenType.CLASS:
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
                    case TokenType.RETURN:
                        return;
                }

                Advance();
            }
        }
    }
}