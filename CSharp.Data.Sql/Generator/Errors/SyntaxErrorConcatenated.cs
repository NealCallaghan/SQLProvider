namespace CSharp.Data.Sql.Generator.Errors
{
    using Util.Func;

    public class SyntaxErrorConcatenated : ResultError
    {
        public SyntaxErrorConcatenated(params SyntaxError[] syntaxErrors) =>
            SyntaxErrors = syntaxErrors;

        public SyntaxError[] SyntaxErrors { get; }
    }
}