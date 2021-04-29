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
    public class Status
    {
        public Guid ID;
        public Guid? ParentID;
        public string DisplayName;
    }
    public class StatusData
    {
        public static Status GetStatusByID(Guid statusId)
        {
            if (statusId == Guid.Empty)
            {
                throw new ArgumentException("StatusID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            Status status = new Status();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT ID, ParentID, DisplayName FROM Statuses WHERE ID=@StatusID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@StatusID", statusId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            status.ID = (Guid)(dt.Rows[0]["ID"]);
                            status.ParentID = (Guid)(dt.Rows[0]["ParentID"]);
                            status.DisplayName = dt.Rows[0]["DisplayName"] as string;
                        }
                    }
                }

                return status;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }
        public static List<Status> GetStatusesByParentID(Guid parentId)
        {
            if (parentId == Guid.Empty)
            {
                throw new ArgumentException("ParentID");
            }

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Status> statuses = new List<Status>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT ID, ParentID, DisplayName FROM Statuses WHERE ParentID=@ParentID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ParentID", parentId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Status status = new Status();
                                status.ID = (Guid)(dt.Rows[0]["ID"]);
                                status.ParentID = (Guid)(dt.Rows[0]["ParentID"]);
                                status.DisplayName = dt.Rows[0]["DisplayName"] as string;

                                statuses.Add(status);
                            }
                        }
                    }
                }

                return statuses;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }
        public static List<Status> GetStatuses()
        {
            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            List<Status> statuses = new List<Status>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    string sql = "SELECT ID, ParentID, DisplayName FROM Statuses";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            DataTable dt = new DataTable();
                            dt.Load(dr);

                            foreach (DataRow row in dt.Rows)
                            {
                                Status status = new Status();
                                status.ID = (Guid)(dt.Rows[0]["ID"]);
                                status.ParentID = (Guid)(dt.Rows[0]["ParentID"]);
                                status.DisplayName = dt.Rows[0]["DisplayName"] as string;

                                statuses.Add(status);
                            }
                        }
                    }
                }

                return statuses;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());

                return null;
            }
        }
    }
}
