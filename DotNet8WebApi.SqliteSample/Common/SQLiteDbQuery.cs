using DotNet8WebApi.SqliteSample.Models.Blog;

namespace DotNet8WebApi.SqliteSample.Common;

public class SqliteDbQuery
{
    public static string CreateTableSql = 
    @"CREATE TABLE IF NOT EXISTS Tbl_Blog 
    (BlogId TEXT NOT NULL, 
    BlogTitle TEXT NOT NULL, 
    BlogAuthor TEXT NOT NULL, 
    BlogContent TEXT NOT NULL)";

    public static string GetAllSql = @"SELECT * FROM Tbl_Blog";

    public static string InsertSql (BlogModel reqModel) =>
    $@"INSERT INTO Tbl_Blog (BlogId, BlogTitle, BlogAuthor, BlogContent) 
    VALUES ('{reqModel.BlogId}', '{reqModel.BlogTitle}', '{reqModel.BlogAuthor}', '{reqModel.BlogContent}')";

    public static string NewInsertSql =
    @"INSERT INTO Tbl_Blog (BlogId, BlogTitle, BlogAuthor, BlogContent) 
    VALUES (@BlogId, @BlogTitle, @BlogAuthor, @BlogContent)";

    public static string UpdateSql = @"UPDATE Tbl_Blog SET BlogTitle = @BlogTitle WHERE BlogId = @BlogId";

    public static string DeleteSql = @"DELETE FROM Tbl_Blog WHERE BlogId = @BlogId";
}