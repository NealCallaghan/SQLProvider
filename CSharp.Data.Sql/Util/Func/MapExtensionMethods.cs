namespace CSharp.Data.Sql.Util.Func
{
    using System;

    public static class MapExtensionMethods
    {
        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> func) =>
            func(@this);

        public static TIn Tee<TIn>(this TIn @this, Action<TIn> func)
        {
            func(@this);
            return @this;
        }
    }
}