namespace CSharp.Data.Sql.Common
{
    using System;
    using Util.Func;

    public class ConnectionSetAttribute : Attribute 
    {
        public string ConnectionString { get; init; }
        public DatabaseType DatabaseType { get; init; }

        internal static string ConnectionStringName() =>
            new ConnectionSetAttribute()
                .Map(x => nameof(x.ConnectionString));

        internal static string DatabaseTypeName() =>
            new ConnectionSetAttribute()
                .Map(x => nameof(x.DatabaseType));
    }

    public static class ConnectionSetAttributeUtilities
    {
        public static string GetConnectionSetAttributeClassSyntax() => 
            @$"//   Generated
    public class {nameof(ConnectionSetAttribute)} : System.Attribute 
    {{
        public string ConnectionString {{ get; init; }}
        
        public DatabaseType DatabaseType {{ get; init; }}
    }}";
    }
}