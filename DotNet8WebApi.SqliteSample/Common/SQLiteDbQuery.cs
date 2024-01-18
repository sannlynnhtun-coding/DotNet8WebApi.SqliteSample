namespace DotNet8WebApi.SqliteSample.Common;

public class SQLiteDbQuery
{
    string createTableSql = 
    @"CREATE TABLE IF NOT EXISTS Tbl_Blog 
    (BlogId TEXT NOT NULL, 
    BlogTitle TEXT NOT NULL, 
    BlogAuthor TEXT NOT NULL, 
    BlogContent TEXT NOT NULL)";

    string insertSql =
    @"INSERT INTO Tbl_Blog (BlogId, BlogTitle, BlogAuthor, BlogContent) 
    VALUES (@BlogId, @BlogTitle, @BlogAuthor, @BlogContent)";

    string query = @"SELECT * FROM Tbl_Blog";

    string updateSql = @"UPDATE Tbl_Blog SET BlogTitle = @BlogTitle WHERE BlogId = @BlogId";

    string deleteSql = @"DELETE FROM Tbl_Blog WHERE BlogId = @BlogId";
}