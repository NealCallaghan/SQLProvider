namespace CSharp.Data.Sql.Schema.Provider.SqlServer
{
    using System.Data;
    using System.Threading.Tasks;

    public interface IDbConnectionAsync
    {
        public Task OpenAsync();
        public IDbConnection GetDbConnection();
    }
}