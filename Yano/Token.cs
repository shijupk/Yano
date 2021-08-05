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
        public int Position { get; set; }

        public Token()
        {
        }

        public Token(TokenType type, string lexeme, object literal, int line, int position)
        {
            Type = type;
            Lexeme = lexeme;
            Literal = literal;
            Line = line;
            Position = position;
        }

        public string ToPrint()
        {
            return $"{Type} {Lexeme} {Literal}";
        }
    }
}