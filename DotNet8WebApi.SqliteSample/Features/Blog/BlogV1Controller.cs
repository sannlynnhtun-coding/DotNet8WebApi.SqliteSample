using DotNet8WebApi.SqliteSample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SQLite;
using System.Reflection.Metadata;

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

            #region Code Style 1

            List<BlogModel> lst = new List<BlogModel>();
            foreach (DataRow dr in dt.Rows)
            {
                BlogModel blog = new BlogModel();
                blog.BlogId = Convert.ToString(dr["BlogId"]);
                blog.BlogTitle = Convert.ToString(dr["BlogTitle"]);
                blog.BlogAuthor = Convert.ToString(dr["BlogAuthor"]);
                blog.BlogContent = Convert.ToString(dr["BlogContent"]);
                lst.Add(blog);
            }

            #endregion

            #region Code Style 2

            var lst2 = dt.AsEnumerable().Select(dr => new BlogModel
            {
                BlogId = Convert.ToString(dr["BlogId"]),
                BlogTitle = Convert.ToString(dr["BlogTitle"]),
                BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            }).ToList();

            #endregion

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

            DataRow dr = dt.Rows[0];
            BlogModel item = new BlogModel();
            item.BlogId = Convert.ToString(dr["BlogId"]);
            item.BlogTitle = Convert.ToString(dr["BlogTitle"]);
            item.BlogAuthor = Convert.ToString(dr["BlogAuthor"]);
            item.BlogContent = Convert.ToString(dr["BlogContent"]);
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
            return Ok(message);
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
            return Ok(message);
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
            return Ok(message);
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
