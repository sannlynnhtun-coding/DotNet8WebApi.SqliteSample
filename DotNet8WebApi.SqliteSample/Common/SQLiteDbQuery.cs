using DotNet8WebApi.SqliteSample.Models.Blog;

namespace DotNet8WebApi.SqliteSample.Common;

public class SqliteDbQuery
{
    public static string CreateBlogTableQuery = 
    @"CREATE TABLE IF NOT EXISTS Tbl_Blog 
    (BlogId TEXT NOT NULL, 
    BlogTitle TEXT NOT NULL, 
    BlogAuthor TEXT NOT NULL, 
    BlogContent TEXT NOT NULL)";

    public static string BlogByIdQuery = @"SELECT * FROM Tbl_BLog WHERE BlogId = @BlogId";
    
    public static string BlogListQuery = @"SELECT * FROM Tbl_Blog";

    public static string BlogCreateQuery =
    @"INSERT INTO Tbl_Blog (BlogId, BlogTitle, BlogAuthor, BlogContent) 
    VALUES (@BlogId, @BlogTitle, @BlogAuthor, @BlogContent)";

    public static string BlogUpdateQuery = @"UPDATE Tbl_Blog SET 
                                            BlogTitle = @BlogTitle,
                                            BlogAuthor = @BlogAuthor,
                                            BlogContent = @BlogContent 
                                            WHERE BlogId = @BlogId";

    public static string BlogPatchQuery = @"UPDATE Tbl_Blog SET 
                                            BlogTitle = @BlogTitle,
                                            BlogContent = @BlogContent 
                                            WHERE BlogId = @BlogId";

    public static string BlogDeleteQuery = @"DELETE FROM Tbl_Blog WHERE BlogId = @BlogId";
}