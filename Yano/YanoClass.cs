using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yano
{
    public class YanoClass : YannoCallable
    {
        public string Name { get; set; }
        private IDictionary<string, YanoFunction> _methods;

        public YanoClass(string name, IDictionary<string, YanoFunction> methods)
        {
            Name = name;
            _methods = methods;
        }

        public YanoFunction FindMethod(string name)
        {
            if (_methods.ContainsKey(name))
            {
                return _methods[name];
            }

            return null;
        }


        public int Arity()
        {
            return 0;
        }

        public object Call(Interpreter interpreter, IList<object> arguments)
        {
            YanoInstance instance = new YanoInstance(this);
            return instance;
        }

        public new string ToString()
        {
            return Name;
        }
    }
}
