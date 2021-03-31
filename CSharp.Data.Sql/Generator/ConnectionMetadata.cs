namespace CSharp.Data.Sql.Generator
{
    using Common;

    public record ClassMetaData(string Namespace, string ClassToExtend);

    public record ConnectionMetadata(ClassMetaData ClassMetaData, string ConnectionString, DatabaseType DatabaseType);
}