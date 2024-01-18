using DotNet8WebApi.SqliteSample.Common;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogV1Controller : ControllerBase
    {
        private readonly SQLiteService _connection;

        public BlogV1Controller(SQLiteService connection)
        {
            _connection = connection;
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            SQLiteConnection _connection = new SQLiteConnection();

            string createTableSql =
            @"CREATE TABLE IF NOT EXISTS Tbl_Blog 
            (BlogId TEXT NOT NULL, 
            BlogTitle TEXT NOT NULL, 
            BlogAuthor TEXT NOT NULL, 
            BlogContent TEXT NOT NULL)";

            SQLiteCommand createTableCommand = new SQLiteCommand(createTableSql, _connection);

            _connection.Open();

            var result = createTableCommand.ExecuteNonQuery();
            Console.WriteLine("Table created!");

            _connection.Close();

            return Ok(result);
        }
    }
}
