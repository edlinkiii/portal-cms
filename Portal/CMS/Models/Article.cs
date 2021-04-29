using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace Portal.CMS.Models
{
    public class Article
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public Guid AuthorID { get; set; }
        // public Guid StatusID { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Body { get; set; }
        public string Excerpt { get; set; }
        public string FeaturedImage { get; set; }
        public string Slug { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Published { get; set; }
        public Author Author { get; set; }
        public Status Status { get; set; }
        public Boolean IsFeatured { get; set; }
        public Boolean IsArchived { get; set; }
    }
    public class ArticleData
    {
        public static string VerifySlugAvailibility(Guid categoryID, Guid articleID, string articleSlug, int ext = 1)
        {
            if (articleID == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }
            if (articleSlug == "")
            {
                throw new ArgumentException("Slug");
            }

            string testSlug = (ext > 1) ? articleSlug + "-" + ext : articleSlug;

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    // SELECT Slug from Articles WHERE 
                    string sql = "SELECT ID, Slug FROM articles WHERE ID != @ArticleID and CategoryID = @CategoryID and Slug = @Slug";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleID", articleID);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@Slug", testSlug);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            if (dt.Rows.Count > 0)
                            {
                                return VerifySlugAvailibility(categoryID, articleID, articleSlug, (ext + 1));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }


            return testSlug;
        }
        // select list
        public static List<Article> GetArticlesForDashboardByCategoryID(Guid categoryID, string orderBy = "a.Published DESC")
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Article> articles = new List<Article>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT a.ID as ID, a.Created as Created, a.Published as Published, a.Title as Title, a.AuthorID as AuthorID, au.DisplayName as AuthorName, a.Slug as Slug, a.IsFeatured as IsFeatured, a.IsArchived as IsArchived FROM Articles a LEFT OUTER JOIN Authors au ON a.AuthorID = au.ID WHERE a.CategoryID=@CategoryID ORDER BY " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Author author = new Author();
                                author.ID = (Guid)(row["AuthorID"]);
                                author.DisplayName = row["AuthorName"] as string;

                                Article article = new Article();

                                article.ID = (Guid)(row["ID"]);
                                article.Author = author;
                                article.Slug = row["Slug"] as string;
                                article.Title = row["Title"] as string;
                                article.Created = DateTime.Parse(row["Created"].ToString());
                                article.Published = row["Published"] as DateTime?;
                                article.IsFeatured = Convert.ToBoolean(row["IsFeatured"]);
                                article.IsArchived = Convert.ToBoolean(row["IsArchived"]);

                                articles.Add(article);
                            }
                        }
                    }

                    return articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        public static List<Article> GetArticlesByCategoryID(Guid categoryID, string orderBy = "a.Published DESC")
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Article> articles = new List<Article>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT a.ID as ID, a.Created as Created, a.Published as Published, a.Title as Title, a.Subtitle as Subtitle, a.Excerpt as Excerpt, a.AuthorID as AuthorID, au.DisplayName as AuthorName, a.Slug as Slug, a.IsFeatured as IsFeatured FROM Articles a LEFT OUTER JOIN Authors au ON a.AuthorID = au.ID WHERE a.CategoryID=@CategoryID AND a.IsArchived=0 AND a.IsDeleted=0 ORDER BY " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Author author = new Author();
                                author.ID = (Guid)(row["AuthorID"]);
                                author.DisplayName = row["AuthorName"] as string;

                                Article article = new Article();

                                article.ID = (Guid)(row["ID"]);
                                article.Author = author;
                                article.Slug = row["Slug"] as string;
                                article.Title = row["Title"] as string;
                                article.Subtitle = row["Subtitle"] as string;
                                article.Excerpt = row["Excerpt"] as string;
                                article.Created = DateTime.Parse(row["Created"].ToString());
                                article.Published = row["Published"] as DateTime?;
                                article.IsFeatured = Convert.ToBoolean(row["IsFeatured"]);

                                articles.Add(article);
                            }
                        }
                    }

                    return articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        public static List<Article> GetPublishedArticlesByCategoryID(Guid categoryID, string orderBy = "a.Published DESC")
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Article> articles = new List<Article>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT a.ID as ID, a.Created as Created, a.Published as Published, a.Title as Title, a.Subtitle as Subtitle, a.Excerpt as Excerpt, a.AuthorID as AuthorID, au.DisplayName as AuthorName, a.Slug as Slug, a.IsFeatured as IsFeatured FROM Articles a LEFT OUTER JOIN Authors au ON a.AuthorID = au.ID WHERE a.CategoryID=@CategoryID AND a.IsArchived=0 AND a.IsDeleted=0 AND a.Published IS NOT NULL AND a.Published < GETDATE() ORDER BY " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Author author = new Author();
                                author.ID = (Guid)(row["AuthorID"]);
                                author.DisplayName = row["AuthorName"] as string;

                                Article article = new Article();

                                article.ID = (Guid)(row["ID"]);
                                article.Author = author;
                                article.Slug = row["Slug"] as string;
                                article.Title = row["Title"] as string;
                                article.Subtitle = row["Subtitle"] as string;
                                article.Excerpt = row["Excerpt"] as string;
                                article.Created = DateTime.Parse(row["Created"].ToString());
                                article.Published = row["Published"] as DateTime?;
                                article.IsFeatured = Convert.ToBoolean(row["IsFeatured"]);

                                articles.Add(article);
                            }
                        }
                    }

                    return articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        public static List<Article> GetFeaturedArticlesByCategoryID(Guid categoryID, string orderBy = "a.Published DESC")
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Article> articles = new List<Article>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT a.ID as ID, a.Created as Created, a.Published as Published, a.Title as Title, a.Subtitle as Subtitle, a.Excerpt as Excerpt, a.AuthorID as AuthorID, au.DisplayName as AuthorName, a.Slug as Slug, a.IsFeatured as IsFeatured FROM Articles a LEFT OUTER JOIN Authors au ON a.AuthorID = au.ID WHERE a.CategoryID=@CategoryID AND a.IsArchived=0 AND a.IsFeatured=1 AND a.IsDeleted=0 AND a.Published IS NOT NULL AND a.Published < GETDATE() ORDER BY " + orderBy;

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Author author = new Author();
                                author.ID = (Guid)(row["AuthorID"]);
                                author.DisplayName = row["AuthorName"] as string;

                                Article article = new Article();

                                article.ID = (Guid)(row["ID"]);
                                article.Author = author;
                                article.Slug = row["Slug"] as string;
                                article.Title = row["Title"] as string;
                                article.Subtitle = row["Subtitle"] as string;
                                article.Excerpt = row["Excerpt"] as string;
                                article.Created = DateTime.Parse(row["Created"].ToString());
                                article.Published = row["Published"] as DateTime?;
                                article.IsFeatured = Convert.ToBoolean(row["IsFeatured"]);

                                articles.Add(article);
                            }
                        }
                    }

                    return articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        public static List<Article> GetPublishedArticlesByTagName(Guid categoryID, string tagName)
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            if (tagName == "")
            {
                throw new ArgumentException("TagName");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Article> articles = new List<Article>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql_1 = "SELECT ID FROM Tags  WHERE DisplayName=@TagName AND CategoryID=@CategoryID";
                    string sql_2 = "SELECT ArticleID FROM ArticleTags WHERE TagID IN(" + sql_1 + ")";
                    string sql_3 = "SELECT a.ID as ID, a.Created as Created, a.Published as Published, a.Title as Title, a.Subtitle as Subtitle, a.Excerpt as Excerpt, a.AuthorID as AuthorID, au.DisplayName as AuthorName, a.Slug as Slug, a.IsFeatured as IsFeatured FROM Articles a LEFT OUTER JOIN Authors au ON a.AuthorID = au.ID WHERE a.ID IN(" + sql_2 + ") AND a.IsArchived=0 AND a.IsDeleted=0 AND a.Published IS NOT NULL AND a.Published < GETDATE() ORDER BY a.Published DESC";

                    using (SqlCommand cmd = new SqlCommand(sql_3, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@TagName", tagName);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Author author = new Author();
                                author.ID = (Guid)(row["AuthorID"]);
                                author.DisplayName = row["AuthorName"] as string;

                                Article article = new Article();

                                article.ID = (Guid)(row["ID"]);
                                article.Author = author;
                                article.Slug = row["Slug"] as string;
                                article.Title = row["Title"] as string;
                                article.Subtitle = row["Subtitle"] as string;
                                article.Excerpt = row["Excerpt"] as string;
                                article.Created = DateTime.Parse(row["Created"].ToString());
                                article.Published = row["Published"] as DateTime?;
                                article.IsFeatured = Convert.ToBoolean(row["IsFeatured"]);

                                articles.Add(article);
                            }
                        }
                    }
                }

                return articles;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }

        // select
        public static Article GetArticleByID(Guid articleID)
        {
            if (articleID == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Article article = new Article();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM articles WHERE id=@ArticleID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleID", articleID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            article.ID = (Guid)(dt.Rows[0]["ID"]);
                            article.CategoryID = (Guid)(dt.Rows[0]["CategoryID"]);
                            article.Title = dt.Rows[0]["Title"] as string;
                            article.Subtitle = dt.Rows[0]["Subtitle"] as string;
                            article.Body = dt.Rows[0]["Body"] as string;
                            article.Excerpt = dt.Rows[0]["Excerpt"] as string;
                            article.FeaturedImage = dt.Rows[0]["FeaturedImage"] as string;
                            article.Slug = dt.Rows[0]["Slug"] as string;
                            article.Published = dt.Rows[0]["Published"] as DateTime?;
                            article.IsFeatured = Convert.ToBoolean(dt.Rows[0]["IsFeatured"]);
                            article.IsArchived = Convert.ToBoolean(dt.Rows[0]["IsArchived"]);
                        }
                    }

                    return article;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        // select
        public static Article GetArticleBySlug(string articleSlug = "")
        {
            if (articleSlug == "")
            {
                throw new ArgumentException("Slug");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Article article = new Article();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT ar.id as ID, ar.title as Title, ar.subtitle as Subtitle, ar.body as Body, ar.published as Published, ar.featuredimage as FeaturedImage, au.displayname as AuthorName FROM articles ar LEFT OUTER JOIN authors au ON ar.authorid = au.id WHERE ar.slug=@Slug ORDER BY ar.published DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Slug", articleSlug);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            article.ID = (Guid)(dt.Rows[0]["ID"]);
                            article.Title = dt.Rows[0]["Title"] as string;
                            article.Subtitle = dt.Rows[0]["Subtitle"] as string;
                            article.Body = dt.Rows[0]["Body"] as string;
                            article.Published = dt.Rows[0]["Published"] as DateTime?;
                            article.FeaturedImage = dt.Rows[0]["FeaturedImage"] as string;

                            Author author = new Author();
                            author.DisplayName = dt.Rows[0]["AuthorName"] as string;

                            article.Author = author;
                        }
                    }

                    return article;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }

        // update
        public static Boolean UpdateArticle(Article article)
        {
            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string articleId = article.ID.ToString();
            string categoryId = article.CategoryID.ToString();
            string authorId = article.AuthorID.ToString();
            string title = article.Title.ToString();
            string subtitle = article.Subtitle.ToString();
            string body = article.Body.ToString();
            string excerpt = article.Excerpt.ToString();
            string featuredImage = article.FeaturedImage.ToString();
            // string slug = article.Slug.ToString();
            string slug = VerifySlugAvailibility(article.CategoryID, article.ID, article.Slug.ToString());
            string published = string.IsNullOrEmpty(article.Published?.ToString()) ? "null" : "'" + article.Published?.ToString() + "'";

            string sql = "UPDATE articles SET CategoryID=@CategoryID, AuthorID=@AuthorID, Title=@Title, Subtitle=@Subtitle, Body=@Body, Excerpt=@Excerpt, FeaturedImage=@FeaturedImage, Slug=@Slug, Published=" + published + " WHERE ID=@ArticleID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                        cmd.Parameters.AddWithValue("@AuthorID", authorId);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Subtitle", subtitle);
                        cmd.Parameters.AddWithValue("@Body", body);
                        cmd.Parameters.AddWithValue("@Excerpt", excerpt);
                        cmd.Parameters.AddWithValue("@FeaturedImage", featuredImage);
                        cmd.Parameters.AddWithValue("@Slug", slug);
                        // cmd.Parameters.AddWithValue("@Published", published);
                        cmd.Parameters.AddWithValue("@ArticleID", articleId);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return false;
            }

            return true;
        }

        public static Article UpdateIsFeatured(Guid articleId, Boolean updateValue = false)
        {
            if (articleId == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string id = articleId.ToString();
            string value = updateValue.ToString();

            string sql = "UPDATE articles SET IsFeatured=@IsFeatured, IsArchived='False' WHERE ID=@ID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@IsFeatured", value);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return GetArticleByID(articleId);
        }

        public static Article UpdateIsArchived(Guid articleId, Boolean updateValue = false)
        {
            if (articleId == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string id = articleId.ToString();
            string value = updateValue.ToString();

            string sql = "UPDATE articles SET IsArchived=@IsArchived, IsFeatured='False' WHERE ID=@ID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@IsArchived", value);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return GetArticleByID(articleId);
        }

        // insert
        public static Boolean InsertArticle(Article article)
        {
            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Guid ArticleID = Guid.NewGuid();
            string articleId = ArticleID.ToString();
            string categoryId = article.CategoryID.ToString();
            string authorId = article.AuthorID.ToString();
            string title = article.Title.ToString();
            string subtitle = article.Subtitle.ToString();
            string body = article.Body.ToString();
            string excerpt = article.Excerpt.ToString();
            string featuredImage = article.FeaturedImage.ToString();
            string slug = VerifySlugAvailibility(article.CategoryID, ArticleID, article.Slug.ToString());
            string published = string.IsNullOrEmpty(article.Published?.ToString()) ? "null" : "'" + article.Published?.ToString() + "'";

            string sql = "INSERT INTO articles (ID, CategoryID, AuthorID, Title, Subtitle, Body, Excerpt, FeaturedImage, Slug, Published) values (@ArticleID, @CategoryID, @AuthorID, @Title, @Subtitle, @Body, @Excerpt, @FeaturedImage, @Slug, " + published + ")";

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleID", articleId);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                        cmd.Parameters.AddWithValue("@AuthorID", authorId);
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Subtitle", subtitle);
                        cmd.Parameters.AddWithValue("@Body", body);
                        cmd.Parameters.AddWithValue("@Excerpt", excerpt);
                        cmd.Parameters.AddWithValue("@FeaturedImage", featuredImage);
                        cmd.Parameters.AddWithValue("@Slug", slug);
                        // cmd.Parameters.AddWithValue("@Published", published);

                        cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return false;
            }

            return true;
        }
    }
}
