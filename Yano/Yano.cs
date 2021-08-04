// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Yano.cs
// ----------------------------------------------------------------------------------------

#region

using System;

#endregion

namespace Yano
{
    internal class Yano
    {
        public static bool HadError { get; private set; }

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
                Prompt();
            }
        }

        private static void Prompt()
        {
            while (true)
            {
                Console.Write("Yano > ");
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
            foreach (var token in tokens)
            {
                Console.WriteLine(token.ToPrint());
            }
        }

        public static void Error(int line, string message)
        {
            Report(line, "", message);
        }

        private static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line} Error {where}: {message}");
            HadError = true;
        }
    }
}