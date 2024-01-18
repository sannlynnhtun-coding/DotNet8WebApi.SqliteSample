using DotNet8WebApi.SqliteSample.Common;
using DotNet8WebApi.SqliteSample.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;

namespace DotNet8WebApi.SqliteSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogV1Controller : ControllerBase
    {
        private readonly SqliteService _sqliteService;

        public BlogV1Controller(SqliteService sqliteService)
        {
            _sqliteService = sqliteService;
        }

        [HttpGet("Create-Table")]
        public IActionResult CreateTable()
        {
            string query =
            @"CREATE TABLE IF NOT EXISTS Tbl_Blog 
            (BlogId TEXT NOT NULL, 
            BlogTitle TEXT NOT NULL, 
            BlogAuthor TEXT NOT NULL, 
            BlogContent TEXT NOT NULL)";
            return Ok(_sqliteService.Execute(query));
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_sqliteService.Query<BlogModel>(""));
        }
    }
}
