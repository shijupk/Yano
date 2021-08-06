// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: Environment.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Exception;

#endregion

namespace Yano
{
    public class Environment
    {
        private readonly IDictionary<string, object> _values = new Dictionary<string, object>();

        public Environment Enclosing { get; set; }

        public Environment()
        {
        }

        public Environment(Environment enclosing)
        {
            Enclosing = enclosing;
        }

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

            if (Enclosing != null)
            {
                return Enclosing.Get(name);
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

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeException(name, $"Undefined variable '{name.Lexeme}'.");
        }

        public object GetAt(int distance, string name)
        {
            return Ancestor(distance)._values[name];
        }

        public void AssignAt(int distance, Token name, object value)
        {
            Ancestor(distance)._values[name.Lexeme] = value;
        }

        public Environment Ancestor(int distance)
        {
            var environment = this;

            for (var i = 0; i < distance; i++)
            {
                environment = environment.Enclosing;
            }

            return environment;
        }
    }
}