// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: YanoInstance.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Exception;

#endregion

namespace Yano
{
    public class YanoInstance
    {
        private readonly YanoClass _class;
        private readonly IDictionary<string, object> _fields = new Dictionary<string, object>();

        public YanoInstance(YanoClass yClass)
        {
            _class = yClass;
        }

        public object Get(Token name)
        {
            if (_fields.ContainsKey(name.Lexeme))
            {
                return _fields[name.Lexeme];
            }

            var method = _class.FindMethod(name.Lexeme);
            if (method != null)
            {
                return method.Bind(this);
            }

            throw new RuntimeException(name, $"Undefined property '{name.Lexeme}'.");
        }

        public void Set(Token name, object value)
        {
            _fields[name.Lexeme] = value;
        }

        public new string ToString()
        {
            return $"{_class.Name} instance";
        }
    }
}