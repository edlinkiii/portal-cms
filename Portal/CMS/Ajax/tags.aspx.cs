using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;
using System.Web.Services;
using System.Web.Script.Services;

namespace Portal.CMS.Ajax
{
    public partial class tags : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static string TestAjax()
        {
            // string wtf = test;
            return "test";
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static List<Tag> FilterTagList(string categoryId, string input)
        {
            return TagData.FilterTagList(Guid.Parse(categoryId), input);
        }
    }
}
