using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Features.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogV1Controller : ControllerBase
    {
        private readonly SQLiteConnectionStringBuilder _connectionStringBuilder;

        public BlogV1Controller()
        {
            _connectionStringBuilder = new SQLiteConnectionStringBuilder
            {
                DataSource = "Blog.db",
                Password = "sa@123",     
            };
        }

        [HttpGet]
        public IActionResult BlogList()
        {

            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "SELECT * FROM Tbl_Blog";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            connection.Close();

            var lst = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt));
            return Ok(lst);
        }

        [HttpGet("Id")]
        public IActionResult BlogGetById(string id)
        {
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();
            string query = "SELECT * FROM Tbl_BLog WHERE BlogId = @BlogId";

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            connection.Close();

            if (dt.Rows.Count == 0)
            {
                return NotFound ("No Data Found.");
            }

            var lst = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dt));

            return Ok(lst);

        }

    }
}
