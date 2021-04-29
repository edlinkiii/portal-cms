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
    public class Category
    {
        public Guid ID { get; set; }
        public string DisplayName { get; set; }
        public string Slug { get; set; }
    }
    public class CategoryData
    {
        public static Category GetCategoryBySlug(string controllerSlug = "")
        {
            if (controllerSlug == "")
            {
                throw new ArgumentException("Slug");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                Category category = new Category();

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM categories WHERE slug=@Slug";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Slug", controllerSlug);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            category.ID = (Guid)(dt.Rows[0]["ID"]);
                            category.DisplayName = dt.Rows[0]["DisplayName"] as string;
                            category.Slug = dt.Rows[0]["Slug"] as string;
                        }
                    }
                }

                return category;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }
        public static Category GetCategoryByID(Guid categoryID)
        {
            if (categoryID == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            try
            {
                Category category = new Category();

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT * FROM categories WHERE id=@CategoryID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryID", categoryID);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            category.ID = (Guid)(dt.Rows[0]["ID"]);
                            category.DisplayName = dt.Rows[0]["DisplayName"] as string;
                            category.Slug = dt.Rows[0]["Slug"] as string;
                        }
                    }
                }

                return category;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }
    }
}
