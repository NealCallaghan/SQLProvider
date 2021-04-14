namespace CSharp.Data.Sql.Generator
{
    public static class NamespaceWrapper
    {
        public static string WrapInNameSpace(string code) =>
$@"namespace CSharp.Data.Sql
{{
    {code}
}}";
    }
}