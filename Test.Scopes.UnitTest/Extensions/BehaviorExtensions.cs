using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Scopes.UnitTest.Extensions
{
    public static class BehaviorExtensions
    {
        public static TOut Given<TOut>(Func<TOut> func)
            => func();

        public static TOut And<TIn, TOut>(this TIn @in, Func<TIn, TOut> func)
            => func(@in);

        public static TOut When<TIn, TOut>(this TIn @in, Func<TIn, TOut> func)
            => func(@in);

        public static TOut When<TOut>(this TOut @out, Action<TOut> func)
        {
            func(@out);
            return @out;
        }

        public static TOut Then<TOut>(this TOut @out, Action<TOut> func)
        {
            func(@out);
            return @out;
        }
    }
}
