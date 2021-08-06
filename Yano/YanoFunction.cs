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
        private readonly bool _isInitializer;

        public YanoFunction(Function function, Environment closure, bool isInitializer)
        {
            _declaration = function;
            _closure = closure;
            _isInitializer = isInitializer;
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
                if (_isInitializer)
                {
                    return _closure.GetAt(0, "this");
                }

                return returnValue.Value;
            }

            if (_isInitializer)
            {
                return _closure.GetAt(0, "this");
            }

            return null;
        }

        public YanoFunction Bind(YanoInstance instance)
        {
            var environment = new Environment(_closure);
            environment.Define("this", instance);
            return new YanoFunction(_declaration, environment, _isInitializer);
        }

        public new string ToString()
        {
            return $"<fn {_declaration.Name.Lexeme}>";
        }
    }
}