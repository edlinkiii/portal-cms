using Portal.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal.CMS.Views
{
    public partial class tag_search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controllerSlug = Page.RouteData.Values["controller"] as string;
            string tagName = Page.RouteData.Values["tag"] as string;

            Category category = CategoryData.GetCategoryBySlug(controllerSlug);


            List<Article> articles = ArticleData.GetPublishedArticlesByTagName(category.ID, tagName);

            SearchQuery.InnerHtml = tagName;
            ResultCount.InnerHtml = articles.Count.ToString();

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
