using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Common;

public class SqliteService
{
    private readonly SQLiteConnection _connection;

    public SqliteService(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
    }

    public List<T> Query<T>(string query)
    {
        SQLiteCommand cmd = new SQLiteCommand(query, _connection);
        _connection.Open();
        SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);
        _connection.Close();

        var lst = JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(dt));
        return lst!;
    }

    public int Execute(string query)
    {
        SQLiteCommand cmd = new SQLiteCommand(query, _connection);
        _connection.Open();
        var result = cmd.ExecuteNonQuery();
        _connection.Close();
        return result;
    }
}
