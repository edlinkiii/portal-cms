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
    public class Author
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string DisplayName { get; set; }
    }
    public class AuthorData
    {
        public static Author GetAuthorByID(Guid authorID)
        {
            if (authorID == Guid.Empty)
            {
                throw new ArgumentException("AuthorID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Author author = new Author();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM authors WHERE id=@AuthorID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@AuthorID", authorID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            author.ID = (Guid)(dt.Rows[0]["ID"]);
                            author.CategoryID = (Guid)(dt.Rows[0]["CategoryID"]);
                            author.DisplayName = dt.Rows[0]["DisplayName"] as string;
                        }
                    }

                    return author;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }
        public static List<Author> GetVisibleAuthorsByCategoryID(Guid categoryID)
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Author> authors = new List<Author>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM authors WHERE categoryid=@CategoryID AND isvisible = '1';";

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

                                author.ID = (Guid)(row["ID"]);
                                author.CategoryID = (Guid)(row["CategoryID"]);
                                author.DisplayName = row["DisplayName"] as string;

                                authors.Add(author);
                            }
                        }
                    }
                }

                return authors;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }

            return null;
        }
    }
}
