using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yano.Exception
{
    public class ParseException: System.Exception
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
