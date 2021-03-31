namespace CSharp.Data.Sql.Util.Func
{
    using System;
    using System.Threading.Tasks;

    public interface Result { }

    public interface Result<out TValue> : Result
    {
        Result<TOut> Then<TOut>(Func<TValue, Result<TOut>> func);

        Task<Result> Then(Func<TValue, Task<Result>> func);

        Result<TValue> OnSuccess(Action<TValue> func);

        Result<TValue> OnSuccess(Func<TValue, Task> func);

        Result<TValue> OnError<TError>(Action<TError> func) where TError : ResultError;
    }
}
