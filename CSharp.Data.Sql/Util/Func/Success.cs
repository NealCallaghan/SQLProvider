namespace CSharp.Data.Sql.Util.Func
{
    using System;
    using System.Threading.Tasks;

    public class Success : Result { }

    public class Success<TValue> : Success, Result<TValue>
    {
        public TValue Value { get; }

        public static Result<TValue> Succeed(TValue value) =>
            new Success<TValue>(value);

        private Success(TValue value) =>
            Value = value;

        public Result<TOut> Then<TOut>(Func<TValue, Result<TOut>> func) =>
            func(Value);

        public Task<Result> Then(Func<TValue, Task<Result>> func) =>
            func(Value);

        public Result<TValue> OnSuccess(Action<TValue> func)
        {
            func(Value);

            return this;
        }

        public Result<TValue> OnSuccess(Func<TValue, Task> func)
        {
            func(Value);

            return this;
        }

        public Result<TValue> OnError<TError>(Action<TError> func) where TError : ResultError => 
            this;
    }
}