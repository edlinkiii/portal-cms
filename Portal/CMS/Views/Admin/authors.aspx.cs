using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;

namespace Portal.CMS.Views.Admin
{
    public partial class authors : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controller = Page.RouteData.Values["controller"] as string;
        }
        protected void LoadAuthors(string controller) {
            // connect to database
            // run query
            // process results
            // output results
        }
    }
}
