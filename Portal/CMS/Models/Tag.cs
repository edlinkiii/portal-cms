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
    public class Tag
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string DisplayName { get; set; }
    }
    public class ArticleTag
    {
        public Guid ID { get; set; }
        public Guid ArticleID { get; set; }
        public Guid TagID { get; set; }
        public Tag Tag { get; set; }
    }
    public class TagData
    {
        public static Tag GetTagByID(Guid tagID)
        {
            if (tagID == Guid.Empty)
            {
                throw new ArgumentException("TagID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Tag tag = new Tag();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM tags WHERE id=@TagID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TagID", tagID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            tag.ID = (Guid)(dt.Rows[0]["ID"]);
                            tag.CategoryID = (Guid)(dt.Rows[0]["CategoryID"]);
                            tag.DisplayName = dt.Rows[0]["DisplayName"] as string;
                        }
                    }
                }

                return tag;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }

        public static List<Tag> GetTagsByCategoryID(Guid categoryID)
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Tag> tags = new List<Tag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    // string sql = "SELECT * FROM tags WHERE categoryid=@CategoryID"; // way too simplistic

                    // get all (unique) tags of _published articles_ for this categoryid, order by how many there are
                    string sql = "SELECT atag.TagID as ID, tag.DisplayName as DisplayName, count(*) as Counts FROM ArticleTags atag INNER JOIN Tags tag ON atag.TagID = tag.ID WHERE ArticleID IN( SELECT ID FROM Articles WHERE CategoryID= @CategoryID AND IsArchived = 0 AND IsDeleted = 0 AND Published IS NOT NULL AND Published < GETDATE() ) GROUP BY atag.TagID, tag.DisplayName ORDER BY Counts DESC";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Tag tag = new Tag();

                                tag.ID = (Guid)(row["ID"]);
                                // tag.CategoryID = (Guid)(row["CategoryID"]);
                                tag.CategoryID = categoryID;
                                tag.DisplayName = row["DisplayName"] as string;

                                tags.Add(tag);
                            }
                        }
                    }
                }

                return tags;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }

        public static List<ArticleTag> GetArticleTagsByArticleID(Guid articleID)
        {
            if (articleID == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<ArticleTag> articleTags = new List<ArticleTag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT ar.id as ID, ar.tagid as TagID, ar.articleid as ArticleID, t.displayname as DisplayName, t.categoryid as CategoryID FROM articletags ar LEFT OUTER JOIN tags t ON ar.tagid = t.id WHERE articleid=@ArticleID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleID", articleID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Tag tag = new Tag();
                                tag.ID = (Guid)(row["TagID"]);
                                tag.CategoryID = (Guid)(row["CategoryID"]);
                                tag.DisplayName = row["DisplayName"] as string;

                                ArticleTag articleTag = new ArticleTag();
                                articleTag.ID = (Guid)(row["ID"]);
                                articleTag.ArticleID = (Guid)(row["ArticleID"]);
                                articleTag.TagID = (Guid)(row["TagID"]);
                                articleTag.Tag = tag;

                                articleTags.Add(articleTag);
                            }
                        }
                    }
                }

                return articleTags;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }

        public static List<Tag> FilterTagList(Guid categoryID, string input)
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Tag> tags = new List<Tag>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM Tags WHERE CategoryID=@CategoryID AND DisplayName like @Input";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                        cmd.Parameters.AddWithValue("@Input", "%" + input + "%");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Tag tag = new Tag();

                                tag.ID = (Guid)(row["ID"]);
                                tag.CategoryID = (Guid)(row["CategoryID"]);
                                tag.DisplayName = row["DisplayName"] as string;

                                tags.Add(tag);
                            }
                        }
                    }
                }

                return tags;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }

        public static Guid AddTagToCategory(Guid categoryID, string tagText)
        {
            if(string.IsNullOrEmpty(tagText))
            {
                throw new ArgumentException("tagText");
            }
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("categoryID");
            }

            Guid tagID = Guid.NewGuid();

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "INSERT INTO Tags (ID, CategoryID, DisplayName) VALUES (@TagID, @CategoryID, @TagText)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@TagID", tagID);
                        cmd.Parameters.AddWithValue("@TagText", tagText);
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        cmd.ExecuteReader();
                    }
                }

                return tagID;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return Guid.Empty;
            }
        }

        public static void AddTagToArticle(Guid articleID, Guid tagID)
        {
            if (articleID == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }
            if (tagID == Guid.Empty)
            {
                throw new ArgumentException("TagID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "INSERT INTO articletags (articleid, tagid) VALUES (@ArticleID, @TagID)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleID", articleID);
                        cmd.Parameters.AddWithValue("@TagID", tagID);

                        cmd.ExecuteReader();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                tagID = Guid.Empty;
            }
        }

        public static void RemoveTagFromArticle(Guid articleTagID)
        {
            if (articleTagID == Guid.Empty)
            {
                throw new ArgumentException("ArticleTagID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "DELETE FROM articletags WHERE id=@ArticleTagID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ArticleTagID", articleTagID);

                        cmd.ExecuteReader();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }
    }
}
