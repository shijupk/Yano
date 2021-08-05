// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: ReturnAsException.cs
// ----------------------------------------------------------------------------------------

namespace Yano.Exception
{
    public class ReturnAsException : System.Exception
    {
        public object Value { get; set; }

        public ReturnAsException(object value)
        {
            Value = value;
        }
    }
}