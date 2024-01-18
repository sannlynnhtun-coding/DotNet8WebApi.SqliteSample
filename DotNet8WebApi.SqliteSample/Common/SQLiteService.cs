using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Common;

public class SQLiteService
{
    private readonly SQLiteConnection _connection;

    public SQLiteService(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
    }

}
