// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Yano.cs
// ----------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Text;
using Yano.Exception;
using Yano.Expression;

#endregion

namespace Yano
{
    internal class Yano
    {
        public static char BoxTopLeft = '┌';
        public static char BoxTopRight = '┐';
        public static char BoxBottomLeft = '└';
        public static char BoxBottomRight = '┘';
        public static char BoxTop = '─';
        public static char BoxBottom = '─';
        public static char BoxLeft = '│';
        public static char BoxRight = '│';

        public static char BoxTopJoin = '┬';
        public static char BoxLeftJoin = '├';
        public static char BoxRightJoin = '┤';
        public static char BoxBottomJoin = '┴';
        public static bool HadError { get; private set; }
        public static bool HadRuntimeError { get; private set; } = false;

        private static  Interpreter _interpreter = new Interpreter();

        private static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                Console.WriteLine("Usage: Yano <script>");
                Environment.Exit(0);
            }
            else if (args.Length == 1)
            {
                //read file
            }
            else
            {
                //TestMethod();
                Prompt();
            }
        }

        private static void TestMethod()
        {
            var expr = new Binary(
                new Unary(
                    new Token(TokenType.MINUS, "-", null, 1, 1),
                    new Literal(123)),
                new Token(TokenType.STAR, "*", null, 1, 2),
                new Grouping(
                    new Literal(45.67)));

            //var expr = new Binary(new Literal(123), new Token(TokenType.STAR, "*", null, 1, 2), new Literal(123));


            var res = new AstPrinter().Print(expr);
            Console.WriteLine(res);

        }
        private static void Prompt()
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Yano");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(" > ");
                Console.ForegroundColor = ConsoleColor.Gray;
                var line = Console.ReadLine();
                if (line == null)
                {
                    break;
                }

                Run(line);
                if (HadError)
                {
                    HadError = false;
                }
            }
        }

        private static void Run(string line)
        {
            var scanner = new Scanner(line);
            var tokens = scanner.ScanTokens();
            PrintLex(tokens);
            if (!HadError)
            {
                var parser = new Parser(tokens);
                var expression = parser.Parse();
                if (!HadError)
                {
                    //var res = new AstPrinter().Print(expression);
                    var res = _interpreter.Interpret(expression);
                    Output(res);
                }

            }

        }

        private static void PrintLex(List<Token> tokens)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" {BoxTopLeft}{new string(BoxTop, 81)}{BoxTopRight}");
            Console.WriteLine(" {0} {1, -10}{2,-10}{3,-20}{4, -20}{5, -20}{6}", BoxLeft, "Line", "Position",
                "Token Type", "Lexeme", "Literal", BoxRight);
            Console.WriteLine($" {BoxLeftJoin}{new string(BoxBottom, 81)}{BoxRightJoin}");
            foreach (var token in tokens)
            {
                Console.WriteLine(" {0} {1,-10}{2,-10}{3,-20}{4, -20}{5, -20}{6}", BoxLeft, token.Line, token.Position,
                    token.Type, token.Lexeme, token.Literal, BoxRight);
            }

            Console.WriteLine($" {BoxBottomLeft}{new string(BoxBottom, 81)}{BoxBottomRight}");
            Console.ResetColor();
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        public static void Error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                Report(token.Line, "at end", message);
            }
            else
            {
                Report(token.Line, $"at '{token.Lexeme}' ", message);
            }

        }

        public static void RuntimeError(RuntimeException exp)
        {
            Report(exp.Token.Line, "", exp.Message);
            HadRuntimeError = true;
        }
        private static void Report(int line, string where, string message)
        {
            var content = $"line {line} Error {where}: {message}";
            var bannerMsg = "Error";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($" {BoxTopLeft}{bannerMsg}{new string(BoxTop, content.Length)}{BoxTopRight}");
            Console.WriteLine($" {BoxLeft}{content}{new string(' ', bannerMsg.Length)}{BoxRight}");
            Console.WriteLine(
                $" {BoxBottomLeft}{new string(BoxBottom, content.Length + bannerMsg.Length)}{BoxBottomRight}");
            HadError = true;
            Console.ResetColor();
        }

        private static void Output(string message)
        {
            var bannerMsg = "Result";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($" {BoxTopLeft}{bannerMsg}{new string(BoxTop, message.Length)}{BoxTopRight}");
            Console.WriteLine($" {BoxLeft}{message}{new string(' ', bannerMsg.Length)}{BoxRight}");
            Console.WriteLine(
                $" {BoxBottomLeft}{new string(BoxBottom, message.Length + bannerMsg.Length)}{BoxBottomRight}");
            HadError = false;
            Console.ResetColor();
        }
    }
}