// ---------------------------------------------------------------------------------------
// Copyright Shiju P K 2021
// 
// FILENAME: YannoCallable.cs
// ----------------------------------------------------------------------------------------

#region

using System.Collections.Generic;

#endregion

namespace Yano
{
    public interface YannoCallable
    {
        int Arity();
        object call(Interpreter interpreter, IList<object> arguments);
    }
}