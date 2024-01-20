using DotNet8WebApi.SqliteSample.Common;
using DotNet8WebApi.SqliteSample.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using System.Data.SQLite;
using System.Reflection.Metadata;

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
        public IActionResult CreateBlogTable()
        {
            return Ok(_sqliteService.Execute(SqliteDbQuery.CreateBlogTableQuery));
        }

        [HttpGet]
        public IActionResult BlogList()
        {
            return Ok(_sqliteService.Query<BlogModel>(SqliteDbQuery.BlogListQuery));
        }

        [HttpGet("{id}")]
        public IActionResult BlogById(string id)
        {
            var model = new BlogModel
            {
                BlogId = id
            };
            return Ok(_sqliteService.Query<BlogModel>(SqliteDbQuery.BlogByIdQuery, model));
        }

        [HttpPost]
        public IActionResult BlogCreate(BlogModel blog)
        {
            blog.BlogId = Ulid.NewUlid().ToString();
            return Ok(_sqliteService.Execute(SqliteDbQuery.BlogCreateQuery, blog));
        }

        [HttpPut("{id}")]
        public IActionResult BlogUpdate(string id, BlogViewModel requestModel)
        {
            var blog = new BlogModel
            {
                BlogId = id,
                BlogAuthor = requestModel.BlogAuthor,
                BlogTitle = requestModel.BlogTitle,
                BlogContent = requestModel.BlogContent,
            };
            return Ok(_sqliteService.Execute(SqliteDbQuery.BlogUpdateQuery, blog));
        }

        [HttpPatch("{id}")]
        public IActionResult BlogPatch(string id, BlogViewModel requestModel)
        {
            var blog = new BlogModel();
            blog.BlogId = id;

            if (!string.IsNullOrEmpty(requestModel.BlogTitle))
            {
                blog.BlogTitle = requestModel.BlogTitle;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogAuthor))
            {
                blog.BlogAuthor = requestModel.BlogAuthor;
            }
            if (!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                blog.BlogContent = requestModel.BlogContent;
            }

            return Ok(_sqliteService.Execute(SqliteDbQuery.BlogPatchQuery, blog));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(string id)
        {
            var blog = new BlogModel
            {
                BlogId = id
            };

            return Ok(_sqliteService.Execute(SqliteDbQuery.BlogDeleteQuery, blog));
        }
    }
}