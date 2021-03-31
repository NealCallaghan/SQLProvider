namespace CSharp.Data.Sql.Common
{
    public enum DatabaseType
    {
        MsSqlServer = 0,
    }

    public static class DatabaseTypeUtilities
    {
        public static string GetDatabaseTypeEnumSyntax() => @$"
    // Generated
    public enum DatabaseType
    {{
        {DatabaseType.MsSqlServer} = 0,
    }}";
    }
}