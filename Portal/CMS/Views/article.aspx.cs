using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;

namespace Portal.CMS.Views
{
    public partial class article : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string controllerSlug = Page.RouteData.Values["controller"] as string;
            string articleSlug = Page.RouteData.Values["article"] as string;

            Article article = ArticleData.GetArticleBySlug(articleSlug);

            List<ArticleTag> tags = TagData.GetArticleTagsByArticleID(article.ID);

            ArticleImage.Text = (article.FeaturedImage != "") ? "<img src='" + article.FeaturedImage + "' />" : "";
            ArticleTitle.Text = article.Title;
            ArticleSubtitle.Text = article.Subtitle;
            ArticleBody.Text = article.Body;
            ArticleAuthor.Text = article.Author.DisplayName;
            ArticlePublishedDate.Text = article.Published?.ToString("MM/dd/yyyy");

            ArticleTagContainer.InnerHtml = "<ul id='tag-list'>";

            for (int i = 0; i < tags.Count; i++)
            {
                ArticleTagContainer.InnerHtml += "<li><a href='/cms/press-release/tag/" + tags[i].Tag.DisplayName + "'>" + tags[i].Tag.DisplayName + "</a></li>";
            } 
            
            ArticleTagContainer.InnerHtml += "</ul>";

            // -- ?? if user is an admin of this category
            ArticleEditButton.Text = "<a href='/cms/admin/" + controllerSlug + "/edit/" + article.ID + "' class='btn btn-primary btn-block' role='button' title='Add New Article'>Edit</a>";
        }
    }
}
