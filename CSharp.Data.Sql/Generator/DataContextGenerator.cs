namespace CSharp.Data.Sql.Generator
{
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Common;

    using static Common.ConnectionSetAttributeUtilities;
    using static Common.SqlDataProviderUtilities;
    using static Common.DatabaseTypeUtilities;

    using static NamespaceWrapper;

    [Generator]
    public class DataContextGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            //TODO
            //CHECK TO SEE IF MYATTRIBUTE IS ALREADY IN THE SOURCE
            //CHECK TO SEE IF DATACLASS IS ALREADY IN THE CONTEXT
            //IF IT ISN'T ADD THEM
        } //

        public void Execute(GeneratorExecutionContext context)
        {
            var enumToAdd = WrapInNameSpace(GetDatabaseTypeEnumSyntax());

            context.AddSource($"{nameof(DatabaseType)}.cs", SourceText.From(enumToAdd, Encoding.UTF8));

            var attributeToAdd = WrapInNameSpace(GetConnectionSetAttributeClassSyntax());

            context.AddSource($"{nameof(ConnectionSetAttribute)}.cs", SourceText.From(attributeToAdd, Encoding.UTF8));

            var classToInherit = WrapInNameSpace(GetSqlDataProviderClassSyntax());

            context.AddSource($"{nameof(SqlDataProvider)}.cs", SourceText.From(classToInherit, Encoding.UTF8));
        }
    }
}