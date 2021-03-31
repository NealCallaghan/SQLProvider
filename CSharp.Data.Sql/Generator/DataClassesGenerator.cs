namespace CSharp.Data.Sql.Generator
{
    using Microsoft.CodeAnalysis;
    using Util.Func;

    using static DataClassesActions;

    [Generator]
    public class DataClassesGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) =>
            context.RegisterForSyntaxNotifications(() => new DataContextReceiver());

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not DataContextReceiver contextReceiver) return;

            foreach (var result in contextReceiver.Candidates)
            {
                result
                    .Then(GenerateDataClasses(context))
                    .OnError(ReportSingleSyntaxError(context))
                    .OnError(ReportMultipleSyntaxError(context));
            }
        }
    }
}