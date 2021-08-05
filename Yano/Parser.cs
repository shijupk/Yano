using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Yano.Exception;
using Yano.Expression;
using Yano.Interface;

namespace Yano
{
    public class Parser
    {
        private IList<Token> _tokens;
        private int _current = 0;

        public Parser(IList<Token> tokens)
        {
            _tokens = tokens;
        }

        public IExpression Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseException)
            {
                return null;
            }
        }
        private IExpression Expression()
        {
            return Equality();
        }

        /// <summary>
        /// equality       → comparison ( ( "!=" | "==" ) comparison )* ;
        /// </summary>
        /// <returns></returns>
        private IExpression Equality()
        {
            var expression = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token operatorToken = Previous();
                IExpression right = Comparison();
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
        /// comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
        /// </summary>
        /// <returns></returns>
        private IExpression Comparison()
        {
            IExpression teminalExpression = Term();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                var operatorToken = Previous();
                var rightExpression = Term();
                teminalExpression = new Binary(teminalExpression, operatorToken, rightExpression);
            }

            return teminalExpression;
        }

        /// <summary>
        /// addition and subtraction:
        /// </summary>
        /// <returns></returns>
        private IExpression Term()
        {
            IExpression expression = Factor();
            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                var operatorToken = Previous();
                IExpression rightExpression = Factor();
                expression = new Binary(expression,operatorToken, rightExpression);
            }

            return expression;
        }

        private IExpression Factor()
        {
            IExpression expression = Unary();
            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                var operatorToken = Previous();
                var rightExpression = Unary();
                expression = new Binary(expression, operatorToken, rightExpression);
            }

            return expression;
        }

        /// <summary>
        /// unary          → ( "!" | "-" ) unary
        ///                | primary ;
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

            return Primary();
        }

        /// <summary>
        /// primary        → NUMBER | STRING | "true" | "false" | "nil"
        ///                  | "(" expression ")" ;
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
                Advance();
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
