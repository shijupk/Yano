using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yano.Exception;
using Yano.Expression;

namespace Yano
{
    public class Environment
    {
        private IDictionary<string, object> _values = new Dictionary<string, object>();

        public void Define(string name, object value)
        {
            _values[name] = value;
        }

        public object Get(Token name)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                return _values[name.Lexeme];
            }
            throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
        }

        public void Assign(Token name, object value)
        {
            if (_values.ContainsKey(name.Lexeme))
            {
                _values[name.Lexeme] = value;
                return;
            }

            throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
        }

    }
}
