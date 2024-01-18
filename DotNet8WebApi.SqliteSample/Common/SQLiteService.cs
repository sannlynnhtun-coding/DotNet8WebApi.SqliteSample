using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Common;

public class SQLiteService
{
    public void CreateMySqlConnection()
    {
        string connectionString =
           "Data Source=C:\\Users\\YAHO\\Desktop\\ACE Data System\\SqlLite\\Blog.db;Version=3;";
        SQLiteConnection connection = new SQLiteConnection(connectionString);
    }
}
