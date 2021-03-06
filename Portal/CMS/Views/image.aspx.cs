using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using Portal.CMS.Models;

namespace Portal.CMS.Views
{
    public partial class image1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Guid imgId = Guid.Parse(Page.RouteData.Values["imgId"] as string);

            FileUpload file = FileUploadData.DownloadFileStream(imgId);

            if (file.Type != "")
            {
                HttpResponse response = HttpContext.Current.Response;
                // response.Write(JsonConvert.SerializeObject(newId));
                response.ContentType = file.Type;

                if (file.Contents.Length > 0)
                {
                    Stream outputStream = response.OutputStream; // = file.Stream; //  BinaryWrite(file.Stream);
                    outputStream.Write(file.Contents, 0, file.Contents.Length);
                }
            }
        }
    }
}