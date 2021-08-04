// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Token.cs
// ----------------------------------------------------------------------------------------

namespace Yano
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Lexeme { get; set; }
        public object Literal { get; set; }

        public int Line { get; set; }

        public Token()
        {
        }

        public Token(TokenType type, string lexeme, object literal, int line)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
        }

        public string ToPrint()
        {
            return $"{Type} {Lexeme} {Literal}";
        }
    }
}