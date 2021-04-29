using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.IO;

namespace Portal.CMS.Models
{
    public class ArticleFileUpload
    {
        public Guid ID { get; set; }
        public Guid ArticleID { get; set; }
        public Guid FileID { get; set; }
    }
    public class FileUpload
    {
        public Guid ID { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public byte[] Contents { get; set; }
        public Stream Stream { get; set; }
        public DateTime Uploaded { get; set; }
    }
    public class FileUploadData
    {
        public static Guid UploadFile(FileUpload file)
        {
            if(file.ID != Guid.Empty && file.ID != null)
            {
                // file already exists
                return file.ID;
            }

            Guid tempId = Guid.NewGuid();

            string connectionString = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string sql = "INSERT INTO files (id, categoryid, uploadname, uploadtype, uploadcontents) VALUES (@ID, @CategoryID, @FileName, @FileType, @FileContents)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = tempId;
                cmd.Parameters.Add("@CategoryID", SqlDbType.UniqueIdentifier).Value = file.CategoryID;
                cmd.Parameters.Add("@FileName", SqlDbType.VarChar).Value = file.Name;
                cmd.Parameters.Add("@FileType", SqlDbType.VarChar).Value = file.Type;
                cmd.Parameters.Add("@FileContents", SqlDbType.VarBinary, file.Contents.Length).Value = file.Contents;

                try
                {
                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());

                    tempId = Guid.Empty;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }

            return tempId;
        }
        public static Guid UploadArticleFile(Guid articleId, Guid fileId, string caption="")
        {
            if (articleId == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }
            if (fileId == Guid.Empty)
            {
                throw new ArgumentException("FileID");
            }

            string connectionString = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql_select = "SELECT id FROM articlefiles WHERE articleid=@ArticleID AND fileid=@FileID";

                try
                {
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(sql_select, connection);

                    da.SelectCommand.Parameters.Add("@ArticleID", SqlDbType.UniqueIdentifier).Value = articleId;
                    da.SelectCommand.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier).Value = fileId;

                    DataSet ds = new DataSet();
                    da.Fill(ds, "af");
                    DataTable dt = ds.Tables["af"];

                    if(dt.Rows.Count > 0)
                    {
                        return (Guid)(dt.Rows[0]["ID"]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());

                    throw new Exception(e.ToString());
                }
                finally
                {
                    if (connection != null) connection.Close();
                }

            }

            Guid tempId = Guid.NewGuid();

            string sql = "INSERT INTO articlefiles (id, articleid, fileid, caption) VALUES (@ID, @ArticleID, @FileID, @Caption)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = tempId;
                cmd.Parameters.Add("@ArticleID", SqlDbType.UniqueIdentifier).Value = articleId;
                cmd.Parameters.Add("@FileID", SqlDbType.UniqueIdentifier).Value = fileId;
                cmd.Parameters.Add("@Caption", SqlDbType.VarChar).Value = caption;

                try
                {
                    connection.Open();

                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());

                    tempId = Guid.Empty;
                }
                finally
                {
                    if (connection != null) connection.Close();
                }

            }

            return tempId;
        }
        public static FileUpload DownloadFileStream(Guid imageId)
        {
            if (imageId == Guid.Empty)
            {
                throw new ArgumentException("imageId");
            }

            FileUpload file = new FileUpload() { ID = imageId };

            string connstring = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            SqlConnection conn = null;

            string sql = "SELECT * FROM files WHERE id = '" + imageId + "'";

            conn = new SqlConnection(connstring);

            try
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "files");
                DataTable dt = ds.Tables["files"];

                file.Contents = (byte[])dt.Rows[0]["UploadContents"];
                file.Type = dt.Rows[0]["UploadType"] as string;
                // file.Stream = new MemoryStream();
                // file.Stream.Write(file.Contents, 0, file.Contents.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
            finally
            {
                if (conn != null) conn.Close();
            }

            return file;
        }
        public static List<ArticleFileUpload> GetArticleFileUploads(Guid articleId)
        {
            if (articleId == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            List<ArticleFileUpload> articleFileUploads = new List<ArticleFileUpload>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string sql = "SELECT id, articleid, fileid, caption FROM articlefiles WHERE articleid='"+ articleId + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "files");
                    DataTable dt = ds.Tables["files"];

                    foreach (DataRow row in dt.Rows)
                    {
                        ArticleFileUpload articleFileUpload = new ArticleFileUpload();
                        articleFileUpload.ID = (Guid)(row["ID"]);
                        articleFileUpload.ArticleID = (Guid)(row["ArticleID"]);
                        articleFileUpload.FileID = (Guid)(row["FileID"]);

                        articleFileUploads.Add(articleFileUpload);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            return articleFileUploads;
        }
        public static List<FileUpload> GetFilesForArticle(Guid articleId)
        {
            if (articleId == Guid.Empty)
            {
                throw new ArgumentException("ArticleID");
            }

            List<FileUpload> files = new List<FileUpload>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string sql = "SELECT f.id, f.uploadtype FROM articlefiles a LEFT OUTER JOIN Files as f ON a.fileid = f.id WHERE a.articleid='" + articleId + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "files");
                    DataTable dt = ds.Tables["files"];

                    foreach (DataRow row in dt.Rows)
                    {
                        FileUpload file = new FileUpload();
                        file.ID = (Guid)(row["ID"]);
                        file.Type = row["UploadType"] as String;

                        files.Add(file);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            return files;
        }
        public static List<FileUpload> GetFilesForCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("CategoryID");
            }

            List<FileUpload> files = new List<FileUpload>();

            string connectionString = ConfigurationManager.ConnectionStrings["dbSqlLocalhost"].ConnectionString;

            string sql = "SELECT id, uploadtype FROM files WHERE categoryid='" + categoryId + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    da.Fill(ds, "files");
                    DataTable dt = ds.Tables["files"];

                    foreach (DataRow row in dt.Rows)
                    {
                        FileUpload file = new FileUpload();
                        file.ID = (Guid)(row["ID"]);
                        file.Type = row["UploadType"] as String;

                        files.Add(file);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                }
                finally
                {
                    if (connection != null) connection.Close();
                }
            }
            return files;
        }
    }
}
