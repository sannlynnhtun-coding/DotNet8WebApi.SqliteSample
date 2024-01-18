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
            return Ok(_sqliteService.Execute(SqliteDbQuery.CreateBlogTableQuery));
        }

        [HttpGet]
        public IActionResult GetList()
        {
            return Ok(_sqliteService.Query<BlogModel>(SqliteDbQuery.BlogListQuery));
        }

        [HttpPost]
        public IActionResult Create(BlogModel blog)
        {
            //BlogModel blog = new BlogModel();
            blog.BlogId = Ulid.NewUlid().ToString();
            //blog.BlogTitle = "Test";
            //blog.BlogAuthor = "Test";
            //blog.BlogContent = "Test";

            return Ok(_sqliteService.Execute(SqliteDbQuery.BlogCreateQuery, blog));
        }
    }
}