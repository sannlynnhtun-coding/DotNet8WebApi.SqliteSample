using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public int Execute(string query, object parameters = null)
    {
        SQLiteCommand cmd = new SQLiteCommand(query, _connection);
        _connection.Open();
        if (parameters != null)
        {
            cmd.Parameters.AddRange(AddParameters(parameters).ToArray());
        }
        var result = cmd.ExecuteNonQuery();
        _connection.Close();
        return result;
    }

    public List<SQLiteParameter> AddParameters<T>(T obj)
    {
        List<SQLiteParameter> lst = new List<SQLiteParameter>();
        foreach (var property in obj.GetType().GetProperties())
        {
            lst.Add(new SQLiteParameter(property.Name, property.GetValue(obj) ?? DBNull.Value));
        }
        return lst;
    }
}
