using Portal.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            featuredPressReleases.InnerHtml = GetFeaturedArticles("press-release");
        }
        protected string GetFeaturedArticles(string controllerSlug)
        {
            Category category = CategoryData.GetCategoryBySlug(controllerSlug);

            // Headline.Text = category.DisplayName + " :: Dashboard";

            List<Article> articles = ArticleData.GetFeaturedArticlesByCategoryID(category.ID);

            string articleListHtml = "<ul>";

            for (int i = 0; i < articles.Count; i++)
            {
                articleListHtml += "<li data-id='" + articles[i].ID + "'>";
                articleListHtml += "  <a href='/cms/" + controllerSlug + "/" + articles[i].Slug + "'>" + articles[i].Title + "</a>";
                articleListHtml += "</li>";
            }

            articleListHtml += "</ul>";

            return articleListHtml;
        }
    }
}