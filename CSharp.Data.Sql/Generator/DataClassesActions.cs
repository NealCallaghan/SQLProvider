namespace CSharp.Data.Sql.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Common;
    using Errors;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Schema;
    using Schema.Provider.SqlServer;
    using Util.Func;

    using static Schema.Provider.SqlServer.SqlServerSchemaProvider;
    using static Schema.TableUtilities;
    using static Common.SqlDataProviderUtilities;

    public static class DataClassesActions
    {
        private static readonly Dictionary<DatabaseType, Func<string, Task<Result<IReadOnlyCollection<Table>>>>> GetTablesFacDictionary
            =
            new()
            {
                {
                    DatabaseType.MsSqlServer,
                    connectionString =>
                    {
                        var connection = new DbConnectionAsync(connectionString);
                        return GetTablesFromConnectionStringAsync(connection,
                            () => new SchemaDataAdapter(connection));
                    }
                },
            };

        public static Func<ConnectionMetadata, Task<Result>> GenerateDataClasses(GeneratorExecutionContext context) =>
            async metaData =>
            {
                var (classMetaData, connectionString, databaseType) = metaData;

                var getTablesFromConnectionStringAsync = GetTablesFacDictionary[databaseType];

                var tablesResult = await getTablesFromConnectionStringAsync(connectionString);

                tablesResult
                    .OnSuccess(AddTablesToSource(context, classMetaData));

                return tablesResult;
            };

        public static Action<IReadOnlyCollection<Table>> AddTablesToSource(GeneratorExecutionContext context, ClassMetaData classMetaData) =>
            tables =>
            {
                var (nameSpace, classToExtend) = classMetaData;

                foreach (var table in tables)
                {
                    var dataTableText = ProvideDataClassTextFromTable(table);

                    var classWithNameSpace = GetClassWithNameSpace(dataTableText);

                    context.AddSource($"{table.TableName}.cs", SourceText.From(classWithNameSpace, Encoding.UTF8));
                }

                var classText = GetPartialSqlDataProviderClassSyntax(classToExtend, nameSpace, tables);
                context.AddSource($"{classToExtend}PartialExtension.cs", SourceText.From(classText, Encoding.UTF8));
            };

        public static Action<SyntaxErrorConcatenated> ReportMultipleSyntaxError(GeneratorExecutionContext context) => 
            syntaxErrors =>
            {
                foreach (var syntaxError in syntaxErrors.SyntaxErrors)
                    ReportSingleSyntaxError(context)(syntaxError);
            };

        public static Action<SyntaxError> ReportSingleSyntaxError(GeneratorExecutionContext context) => 
            syntaxError =>
            new DiagnosticDescriptor(
                    $"SyntaxError{syntaxError.ErrorMessage}",
                    "Sql Data Provider Syntax Error",
                    $"{syntaxError.ErrorMessage}",
                    "Syntax Error", DiagnosticSeverity.Error, true)
                .Tee(diagnostic => context.ReportDiagnostic(Diagnostic.Create(diagnostic, Location.None)));

        private static string GetClassWithNameSpace(string classCode) =>
            $@"namespace CSharp.Data.Sql
{{
    {classCode}
}}";
    }
}