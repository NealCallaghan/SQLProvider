namespace CSharp.Data.Sql.Util
{
    using System.Collections.Generic;
    using System.Text;

    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendCollection(this StringBuilder @this, IEnumerable<string> collection)
        {
            foreach (var item in collection)
            {
                @this.Append(item);
            }

            return @this;
        }
    }
}