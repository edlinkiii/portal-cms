using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using Portal.CMS.Models;
using Newtonsoft.Json;

namespace Portal.CMS.Ajax
{
    [WebService]
    public class UploadFile : WebService
    {
        [WebMethod]
        public void UploadFileForArticle()
        {
            var postRequest = HttpContext.Current.Request;

            if (postRequest.Files.AllKeys.Any())
            {
                HttpPostedFile file = postRequest.Files["file"];

                if (file != null)
                {
                    FileUpload fileUpload = new FileUpload();
                    fileUpload.Name = file.FileName;
                    fileUpload.Type = file.ContentType;
                    fileUpload.Stream = file.InputStream;
                    using (BinaryReader binaryReader = new BinaryReader(fileUpload.Stream))
                    {
                        fileUpload.Contents = binaryReader.ReadBytes((int)fileUpload.Stream.Length);
                    }

                    fileUpload.CategoryID = Guid.Parse(postRequest.Params["categoryId"]);

                    fileUpload.ID = FileUploadData.UploadFile(fileUpload);

                    // ArticleFileUpload articleFileUpload = new ArticleFileUpload();
                    // articleFileUpload.ArticleID = Guid.Parse(postRequest.Params["articleId"]);
                    // articleFileUpload.FileID = fileUpload.ID;

                    // articleFileUpload.ID = FileUploadData.UploadArticleFile(articleFileUpload.ArticleID, articleFileUpload.FileID);

                    FileUpload returnData = new FileUpload() {
                        ID = fileUpload.ID,
                        Type = fileUpload.Type,
                        Name = fileUpload.Name
                    };

                    HttpResponse response = HttpContext.Current.Response;
                    response.Write(JsonConvert.SerializeObject(returnData));
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
    public class NewID
    {
        public Guid ID;
    }
}
