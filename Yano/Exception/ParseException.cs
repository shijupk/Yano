// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: ParseException.cs
// ----------------------------------------------------------------------------------------

#region

using System;

#endregion

namespace Yano.Exception
{
    public class ParseException : System.Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }

    public class RuntimeException : SystemException
    {
        public Token Token { get; set; }

        public RuntimeException(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}