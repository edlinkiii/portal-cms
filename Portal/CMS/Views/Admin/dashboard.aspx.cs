using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;
using System.Web.Services;

namespace Portal.CMS.Views.Admin
{
    public partial class dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controllerSlug = Page.RouteData.Values["controller"] as string;
            string order = Request.QueryString["order"];
            string dir = Request.QueryString["dir"];

            string down = "&#9660;";
            string up = "&#9650;";

            Category category = CategoryData.GetCategoryBySlug(controllerSlug);

            Headline.Text = category.DisplayName + " :: Dashboard";

            if (string.IsNullOrEmpty(order))
            {
                order = "a.Created";
            }

            if (string.IsNullOrEmpty(dir))
            {
                dir = "DESC";
            }

            string orderBy = order + " " + dir;

            string arrow = (dir == "DESC") ? down : up;

            List<Article> articles = ArticleData.GetArticlesForDashboardByCategoryID(category.ID, orderBy);

            string articleTableHeader = "";

            articleTableHeader = "<tr>";
            articleTableHeader += "  <th></th>";
            articleTableHeader += "  <th class='sortable' data-order='a.Title' data-dir='" + ((order == "a.Title" && dir == "ASC") ? "DESC" : "ASC") + "'>Title <span class='direction'>" + ((order == "a.Title") ? arrow : "") + "</span></th>";
            articleTableHeader += "  <th class='sortable' data-order='au.DisplayName' data-dir='" + ((order == "au.DisplayName" && dir == "ASC") ? "DESC" : "ASC") + "'>Author <span class='direction'>" + ((order == "au.DisplayName") ? arrow : "") + "</span></th>";
            articleTableHeader += "  <th class='sortable' data-order='a.Created' data-dir='" + ((order == "a.Created" && dir == "DESC") ? "ASC" : "DESC") + "'>Created <span class='direction'>" + ((order == "a.Created") ? arrow : "") + "</span></th>";
            articleTableHeader += "  <th class='sortable' data-order='a.Published' data-dir='" + ((order == "a.Published" && dir == "DESC") ? "ASC" : "DESC") + "'>Published <span class='direction'>" + ((order == "a.Published") ? arrow : "") + "</span></th>";
            articleTableHeader += "  <th>Featured</th>";
            articleTableHeader += "  <th>Archived</th>";
            articleTableHeader += "</tr>";

            ArticleTableHeader.InnerHtml = articleTableHeader;

            string articleTableBody = "";

            for (int i = 0; i < articles.Count; i++)
            {
                articleTableBody += "<tr>";
                articleTableBody += "  <td>";
                articleTableBody += "    <a href='/cms/" + controllerSlug + "/" + articles[i].Slug + "' class='btn btn-success btn-sm' role='button' title='View Article'>View</a>&nbsp;";
                articleTableBody += "    <a href='edit/" + articles[i].ID + "' class='btn btn-primary btn-sm' role='button' title='Add New Article'>Edit</a>";
                articleTableBody += "  </td>";
                articleTableBody += "  <td>" + articles[i].Title + "</td>";
                articleTableBody += "  <td>" + articles[i].Author.DisplayName + "</td>";
                articleTableBody += "  <td>" + articles[i].Created.ToString("MM/dd/yyyy") + "</td>";
                articleTableBody += "  <td>" + articles[i].Published?.ToString("MM/dd/yyyy") + "</td>";
                articleTableBody += "  <td>" + ((articles[i].Published?.ToString("MM/dd/yyyy") != null) ? "<label class='switch is-featured'><input data-id='" + articles[i].ID + "' type='checkbox'" + ((articles[i].IsFeatured) ? " checked" : "") + "><span class='slider round'></span></label>" : "") + "</td>";
                articleTableBody += "  <td>" + ((articles[i].Published?.ToString("MM/dd/yyyy") != null) ? "<label class='switch is-archived'><input data-id='" + articles[i].ID + "' type='checkbox'" + ((articles[i].IsArchived) ? " checked" : "") + "><span class='slider round'></span></label>" : "") + "</td>";
                articleTableBody += "</tr>";
                string test = articles[i].Published?.ToString("MM/dd/yyyy");
            }

            ArticleTableBody.InnerHtml = articleTableBody;
        }

        [WebMethod]
        public static Boolean TestAjax(string test)
        {
            string wtf = test;
            return true;
        }

        [WebMethod]
        public static Article SetIsFeatured(string articleId, Boolean isFeatured)
        {
            return ArticleData.UpdateIsFeatured(Guid.Parse(articleId), isFeatured);
        }

        [WebMethod]
        public static Article SetIsArchived(string articleId, Boolean isArchived)
        {
            return ArticleData.UpdateIsArchived(Guid.Parse(articleId), isArchived);
        }
    }
}
