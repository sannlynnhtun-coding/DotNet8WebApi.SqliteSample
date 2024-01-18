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
            return Ok(_sqliteService.Execute(SqliteDbQuery.CreateTableSql));
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetList()
        {
            return Ok(_sqliteService.Query<BlogModel>(SqliteDbQuery.GetAllSql));
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create()
        {
            BlogModel blogModel = new BlogModel();
            blogModel.BlogId = Ulid.NewUlid().ToString();
            blogModel.BlogTitle = "Test";
            blogModel.BlogAuthor = "Test";
            blogModel.BlogContent = "Test";

            return Ok(_sqliteService.Execute(SqliteDbQuery.InsertSql(blogModel)));
        }
    }
}