using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Exception;

namespace Yano
{
    class YanoInstance
    {
        private YanoClass _class;
        private IDictionary<string, object> _fields = new Dictionary<string, object>();

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
                return method;
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
