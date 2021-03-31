namespace CSharp.Data.Sql.Generator.Errors
{
    using Util.Func;

    public class SyntaxError : ResultError
    {
        public SyntaxError(string errorMessage) =>
            ErrorMessage = errorMessage;
        public string ErrorMessage { get; }
    }
}