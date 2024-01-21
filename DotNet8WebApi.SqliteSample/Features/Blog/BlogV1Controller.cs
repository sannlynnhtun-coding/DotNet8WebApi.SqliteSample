using DotNet8WebApi.SqliteSample.Models;
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
            string query = "SELECT * FROM Tbl_Blog";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            connection.Close();

            var lst = JsonConvert.DeserializeObject<List<BlogModel>>(JsonConvert.SerializeObject(dt));
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult BlogGetById(string id)
        {
            string query = "SELECT * FROM Tbl_BLog WHERE BlogId = @BlogId";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            connection.Close();

            if (dt.Rows.Count == 0)
            {
                return NotFound("No Data Found.");
            }

            var lst = JsonConvert.DeserializeObject<List<BlogModel>>(JsonConvert.SerializeObject(dt));
            var item = lst![0];
            return Ok(item);
        }

        [HttpPost]
        public IActionResult BlogCreate(BlogModel blog)
        {
            string query = "INSERT INTO Tbl_BLog (BlogTitle, BlogAuthor,BlogContent ) VALUES (@BlogTitle, @BlogAuthor, @BlogContent)";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();
            blog.BlogId = Ulid.NewUlid().ToString();

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", blog.BlogId);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            var response = new
            {
                Message = message,
                Blog = blog,
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public IActionResult BlogPut(string id, BlogModel blog)
        {
            string query = @"UPDATE Tbl_Blog 
                     SET BlogTitle = @BlogTitle,
                         BlogAuthor = @BlogAuthor,
                         BlogContent = @BlogContent 
                     WHERE BlogId = @BlogId";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            cmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            cmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Update Successful." : "Update Failed.";
            var response = new
            {
                Message = message,
                Blog = blog,
            };

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult BlogPatch(string id, BlogModel blog)
        {
            string getQuery = "SELECT * FROM Tbl_Blog WHERE BlogId = @BlogId";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            SQLiteCommand getCmd = new SQLiteCommand(getQuery, connection);
            getCmd.Parameters.AddWithValue("@BlogId", id);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(getCmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                connection.Close();
                return NotFound("Blog not found");
            }

            var existingBlog = JsonConvert.DeserializeObject<List<BlogModel>>(JsonConvert.SerializeObject(dt))![0];

            string updateQuery = @"UPDATE Tbl_Blog 
                           SET BlogTitle = @BlogTitle,
                               BlogAuthor = @BlogAuthor,
                               BlogContent = @BlogContent 
                           WHERE BlogId = @BlogId";

            SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@BlogId", id);

            if (!string.IsNullOrEmpty(blog.BlogTitle))
            {
                updateCmd.Parameters.AddWithValue("@BlogTitle", blog.BlogTitle);
            }
            else
            {
                updateCmd.Parameters.AddWithValue("@BlogTitle", existingBlog.BlogTitle);
            }

            if (!string.IsNullOrEmpty(blog.BlogAuthor))
            {
                updateCmd.Parameters.AddWithValue("@BlogAuthor", blog.BlogAuthor);
            }
            else
            {
                updateCmd.Parameters.AddWithValue("@BlogAuthor", existingBlog.BlogAuthor);
            }

            if (!string.IsNullOrEmpty(blog.BlogContent))
            {
                updateCmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            }
            else
            {
                updateCmd.Parameters.AddWithValue("@BlogContent", existingBlog.BlogContent);
            }

            int result = updateCmd.ExecuteNonQuery();

            connection.Close();

            string message = result > 0 ? "Update Successful." : "Update Failed.";
            var response = new
            {
                Message = message,
                Blog = existingBlog,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult BlogDelete(string id)
        {
            string checkQuery = "SELECT * FROM Tbl_Blog WHERE BlogId = @BlogId";
            SQLiteConnection connection = new SQLiteConnection(_connectionStringBuilder.ConnectionString);
            connection.Open();

            SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection);
            checkCmd.Parameters.AddWithValue("@BlogId", id);

            SQLiteDataAdapter adapter = new SQLiteDataAdapter(checkCmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            if (dt.Rows.Count == 0)
            {
                connection.Close();
                return NotFound("Blog not found");
            }

            string deleteQuery = "DELETE FROM Tbl_Blog WHERE BlogId = @BlogId";
            SQLiteCommand cmd = new SQLiteCommand(deleteQuery, connection);
            cmd.Parameters.AddWithValue("@BlogId", id);
            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            return Ok(message);
        }
    }
}
