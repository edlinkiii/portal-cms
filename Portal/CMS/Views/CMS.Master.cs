using Portal.CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Portal
{
    public partial class sidebar : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controllerSlug = Page.RouteData.Values["controller"] as string;

            Category category = CategoryData.GetCategoryBySlug(controllerSlug);

            recentArticles.InnerHtml = GetPublishedArticles(category);

            mostTags.InnerHtml = GetPublishedTags(category);
        }
        protected string GetPublishedArticles(Category category)
        {
            List<Article> articles = ArticleData.GetPublishedArticlesByCategoryID(category.ID);

            string articleListHtml = "<ul>";

            for (int i = 0; i < articles.Count; i++)
            {
                articleListHtml += "<li data-id='" + articles[i].ID + "'>";
                articleListHtml += "  <a href='/cms/" + category.Slug + "/" + articles[i].Slug + "'>" + articles[i].Title + "</a>";
                articleListHtml += "</li>";

                if (i == 4) break;
            }

            articleListHtml += "</ul>";

            return articleListHtml;
        }
        protected string GetPublishedTags(Category category)
        {
            List<Tag> tags = TagData.GetTagsByCategoryID(category.ID);

            string tagListHtml = "<ul>";

            for (int i = 0; i < tags.Count; i++)
            {
                tagListHtml += "<li data-id='" + tags[i].ID + "'>";
                tagListHtml += "  <a href='/cms/" + category.Slug + "/tag/" + tags[i].DisplayName + "'>" + tags[i].DisplayName + "</a>";
                tagListHtml += "</li>";

                if (i == 4) break;
            }

            tagListHtml += "</ul>";

            return tagListHtml;
        }
    }
}