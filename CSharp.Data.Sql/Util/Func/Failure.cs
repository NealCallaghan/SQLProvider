namespace CSharp.Data.Sql.Util.Func
{
    using System;
    using System.Threading.Tasks;

    public interface IFailure<out TValue, out TError> : Result<TValue> where TError : ResultError { }

    public interface IFailure<out TError> : Result where TError : ResultError { }

    public class Failure<TError> : IFailure<TError> where TError : ResultError
    {
        public TError ResultError { get; init; }

        protected Failure() {}
    }

    public class Failure<TValue, TError> : Failure<TError>, IFailure<TValue, TError> where TError : ResultError
    {
        public static Result<TValue> Fail(TError resultError) =>
            new Failure<TValue, TError>(resultError);

        private Failure(TError error) =>
            ResultError = error;

        public Result<TOut> Then<TOut>(Func<TValue, Result<TOut>> func) =>
            new Failure<TOut, TError>(ResultError);

        public Task<Result> Then(Func<TValue, Task<Result>> func) =>
            Task.FromResult((Result)this);

        public Result<TValue> OnSuccess(Action<TValue> func) =>
            this;

        public Result<TValue> OnSuccess(Func<TValue, Task> func) =>
            this;
        
        public Result<TValue> OnError<TErrorType>(Action<TErrorType> func) where TErrorType : ResultError
        {
            if (ResultError is TErrorType errorType)
                func(errorType);

            return this;
        }
    }

    public static class FailureExtensions
    {
        public static Task<Result> OnError<TErrorType>(this Task<Result> @this, Action<TErrorType> func) where TErrorType : ResultError =>
            @this.ContinueWith(x =>
            {
                if (x.Result is Failure<TErrorType> f) func(f.ResultError);
                return x.Result;
            });
    }
}