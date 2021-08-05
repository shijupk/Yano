// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Scanner.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;

#endregion

namespace Yano
{
    public class Scanner
    {
        private static readonly Dictionary<string, TokenType> KeyWords = new Dictionary<string, TokenType>
        {
            {"and", TokenType.AND},
            {"class", TokenType.CLASS},
            {"else", TokenType.ELSE},
            {"false", TokenType.FALSE},
            {"for", TokenType.FOR},
            {"fun", TokenType.FUN},
            {"if", TokenType.IF},
            {"nil", TokenType.NIL},
            {"or", TokenType.OR},
            {"print", TokenType.PRINT},
            {"return", TokenType.RETURN},
            {"super", TokenType.SUPER},
            {"this", TokenType.THIS},
            {"true", TokenType.TRUE},
            {"var", TokenType.VAR},
            {"while", TokenType.WHILE}
        };

        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();

        private int _current;
        private int _line = 1;
        private int _start;

        public Scanner(string source)
        {
            _source = source;
        }


        private string GetToken(int startIndex, int currentIndex)
        {
            var subStringLength = currentIndex - startIndex;
            return _source.Substring(startIndex, subStringLength);
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                // We are at the beginning of the next lexeme.
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line, _start));
            return _tokens;
        }

        private void ScanToken()
        {
            var c = Advance();
            switch (c)
            {
                case '(':
                    AddToken(TokenType.LEFT_PAREN);
                    break;
                case ')':
                    AddToken(TokenType.RIGHT_PAREN);
                    break;
                case '{':
                    AddToken(TokenType.LEFT_BRACE);
                    break;
                case '}':
                    AddToken(TokenType.RIGHT_BRACE);
                    break;
                case ',':
                    AddToken(TokenType.COMMA);
                    break;
                case '.':
                    AddToken(TokenType.DOT);
                    break;
                case '-':
                    AddToken(TokenType.MINUS);
                    break;
                case '+':
                    AddToken(TokenType.PLUS);
                    break;
                case ';':
                    AddToken(TokenType.SEMICOLON);
                    break;
                case '*':
                    AddToken(TokenType.STAR);
                    break;
                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the line.
                        while (Peek() != '\n' && !IsAtEnd())
                        {
                            Advance();
                        }
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }

                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.
                    break;

                case '\n':
                    _line++;
                    break;
                case '"':
                    ScanString();
                    break;
                default:
                    if (IsDigit(c))
                    {
                        ScanNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        ScanIdentifier();
                    }
                    else
                    {
                        Yano.Error(_line, "Unexpected character.");
                    }

                    break;
            }
        }

        private void ScanIdentifier()
        {
            while (IsAlphaNumeric(Peek()))
            {
                Advance();
            }

            var text = GetToken(_start, _current);

            var type = TokenType.IDENTIFIER;
            if (KeyWords.ContainsKey(text))
            {
                type = KeyWords[text];
            }

            AddToken(type);
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAlpha(char c)
        {
            return c >= 'a' && c <= 'z' ||
                   c >= 'A' && c <= 'Z' ||
                   c == '_';
        }

        private void ScanNumber()
        {
            while (IsDigit(Peek()))
            {
                Advance();
            }

            // Look for a fractional part.
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."
                Advance();

                while (IsDigit(Peek()))
                {
                    Advance();
                }
            }

            AddToken(TokenType.NUMBER, double.Parse(GetToken(_start, _current)));
        }

        private char PeekNext()
        {
            return _current + 1 >= _source.Length ? '\0' : _source[_current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void ScanString()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n')
                {
                    _line++;
                }

                Advance();
            }

            if (IsAtEnd())
            {
                Yano.Error(_line, "Unterminated string.");
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            var value = GetToken(_start + 1, _current - 1);
            AddToken(TokenType.STRING, value);
        }

        private char Peek()
        {
            return IsAtEnd() ? '\0' : _source[_current];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd())
            {
                return false;
            }

            if (_source[_current] != expected)
            {
                return false;
            }

            _current++;
            return true;
        }

        private void AddToken(TokenType type, object literal = null)
        {
            var text = GetToken(_start, _current);
            _tokens.Add(new Token(type, text, literal, _line, _start));
        }

        private char Advance()
        {
            return _source[_current++];
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}