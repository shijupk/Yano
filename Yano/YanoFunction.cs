// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: YanoFunction.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;
using Yano.Exception;
using Yano.Statement;

#endregion

namespace Yano
{
    public class YanoFunction : YannoCallable
    {
        private readonly Environment _closure;
        private readonly Function _declaration;

        public YanoFunction(Function function, Environment closure)
        {
            _declaration = function;
            _closure = closure;
        }

        public int Arity()
        {
            return _declaration.Parameters.Count;
        }

        public object Call(Interpreter interpreter, IList<object> arguments)
        {
            var environment = new Environment(_closure);
            for (var i = 0; i < _declaration.Parameters.Count; i++)
            {
                environment.Define(_declaration.Parameters[i].Lexeme, arguments[i]);
            }

            try
            {
                interpreter.ExecuteBlock(_declaration.Body, environment);
            }
            catch (ReturnAsException returnValue)
            {
                return returnValue.Value;
            }

            return null;
        }

        public new string ToString()
        {
            return $"<fn {_declaration.Name.Lexeme}>";
        }
    }
}