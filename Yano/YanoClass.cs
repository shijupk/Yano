// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: YanoClass.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;

#endregion

namespace Yano
{
    public class YanoClass : YannoCallable
    {
        private readonly IDictionary<string, YanoFunction> _methods;
        public string Name { get; set; }
        public YanoClass SuperClass { get; set; }

        public YanoClass(string name, YanoClass superClass, IDictionary<string, YanoFunction> methods)
        {
            Name = name;
            _methods = methods;
            SuperClass = superClass;
        }


        public int Arity()
        {
            var initializer = FindMethod("init");
            if (initializer == null)
            {
                return 0;
            }

            return initializer.Arity();
        }

        public object Call(Interpreter interpreter, IList<object> arguments)
        {
            var instance = new YanoInstance(this);
            var initializer = FindMethod("init");
            initializer?.Bind(instance).Call(interpreter, arguments);

            return instance;
        }

        public YanoFunction FindMethod(string name)
        {
            if (_methods.ContainsKey(name))
            {
                return _methods[name];
            }

            if (SuperClass != null)
            {
                return SuperClass.FindMethod(name);
            }

            return null;
        }

        public new string ToString()
        {
            return Name;
        }
    }
}