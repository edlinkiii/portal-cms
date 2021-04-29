using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;
using System.Web.Services;

namespace Portal.CMS.Views
{
    public partial class articles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controllerSlug = Page.RouteData.Values["controller"] as string;

            Category category = CategoryData.GetCategoryBySlug(controllerSlug);

            // Headline.Text = category.DisplayName + " :: Dashboard";

            List<Article> articles = ArticleData.GetPublishedArticlesByCategoryID(category.ID);

            string articleListHtml = "";

            for (int i = 0; i < articles.Count; i++)
            {
                articleListHtml += "<article data-id='" + articles[i].ID + "'" + ((articles[i].IsFeatured) ? " class='is-featured'" : "") + ">";
                articleListHtml += "  <h1>" + articles[i].Title + "</h1>";
                articleListHtml += "  <h2>" + articles[i].Subtitle + "</h2>";
                articleListHtml += "  <p>" + articles[i].Excerpt + "</p>";
                articleListHtml += "  <div class='text-right'><a href='/cms/" + controllerSlug + "/" + articles[i].Slug + "' class='btn btn-primary'>Read More</a></div>";
                articleListHtml += "</article>";
            }

            ArticleListContainer.InnerHtml = articleListHtml;
        }
    }
}
