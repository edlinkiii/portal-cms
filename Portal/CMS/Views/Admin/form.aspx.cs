using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Portal.CMS.Models;
using Newtonsoft.Json;

namespace Portal.CMS.Views.Admin
{
    public partial class form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Success.Visible = false;

            if (!IsPostBack)
            {
                Category category = CategoryData.GetCategoryBySlug(Page.RouteData.Values["controller"].ToString());
                Guid articleId;
                Boolean IsNew;

                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    articleId = Guid.Parse(Page.RouteData.Values["id"].ToString());
                    IsNew = false;

                }
                else
                {
                    IsNew = true;
                    articleId = Guid.NewGuid();
                }

                Headline.Text = category.DisplayName + " :: " + (IsNew ? "Add New" : "Edit");
                LinkToDashboard.NavigateUrl = "/cms/admin/" + category.Slug.ToString() + "/dashboard";
                CategoryID.Value = category.ID.ToString();
                ArticleID.Value = articleId.ToString();
                AuthorID.Value = "4e695492-1d1a-4575-9f74-66c4e8421561"; // Ed
                IsAddNew.Value = IsNew.ToString();

                if (!IsNew)
                {
                    PopulateForm(articleId);
                }
            }
        }
        protected void PopulateForm(Guid id)
        {
            Article article = ArticleData.GetArticleByID(id);
            ArticleTitle.Text = article.Title;
            ArticleSubtitle.Text = article.Subtitle;
            ArticleBody.Text = article.Body;
            ArticleID.Value = article.ID.ToString();
            ArticleExcerpt.Text = article.Excerpt;
            ArticleFeaturedImage.Text = article.FeaturedImage;
            ArticleSlugDisplay.Text = article.Slug;
            ArticleSlug.Value = article.Slug;
            ArticlePublished.Text = article.Published?.ToString("yyyy-MM-dd");
            ArticlePublished.ReadOnly = !string.IsNullOrEmpty(article.Published?.ToString());

            List<ArticleTag> tags = TagData.GetArticleTagsByArticleID(article.ID);

            ArticleTagDisplay.InnerHtml = "";

            for (int i = 0; i < tags.Count; i++)
            {
                ArticleTagDisplay.InnerHtml += "<div class='tag-container' data-articletagid='" + tags[i].ID + "'><span class='tag-button'>" + tags[i].Tag.DisplayName + "</span><a class='tag-remove-button'></a></div>";
            }

            FileDropzone.InnerHtml = "";

            List<Models.FileUpload> articleFiles = FileUploadData.GetFilesForArticle(article.ID);

            for (int i = 0; i < articleFiles.Count; i++)
            {
                switch (articleFiles[i].Type)
                {
                    case "video/mp4":
                        FileDropzone.InnerHtml += "<video class='thumbnail'><source src='/vid/video/" + articleFiles[i].ID + "' type='" + articleFiles[i].Type + "' class='add-video'>Your browser does not support the video tag.</video>";
                        break;
                    case "image/gif":
                    default:
                        FileDropzone.InnerHtml += "<img src='/img/image/" + articleFiles[i].ID + "' class='thumbnail add-image'>";
                        break;
                }
                // FileDropzone.InnerHtml += "<img src='/img/image/" + articleFiles[i].ID + "' class='thumbnail'>";
                // "<video class='thumbnail'><source src='/vid/video/"+ id +"' type='"+ type +"'>Your browser does not support the video tag.</video>"
            }
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Article article = new Article();
            article.ID = Guid.Parse(ArticleID.Value);
            article.CategoryID = Guid.Parse(CategoryID.Value);
            article.AuthorID = Guid.Parse(AuthorID.Value);
            article.Title = ArticleTitle.Text;
            article.Subtitle = ArticleSubtitle.Text;
            article.Body = ArticleBody.Text;
            article.Excerpt = ArticleExcerpt.Text;
            article.FeaturedImage = ArticleFeaturedImage.Text;
            article.Slug = ArticleSlug.Value;
            article.Published = string.IsNullOrEmpty(ArticlePublished.Text) ? (DateTime?)null : DateTime.Parse(ArticlePublished.Text);

            Boolean isNew = Convert.ToBoolean(IsAddNew.Value);
            Boolean result = (isNew) ? ArticleData.InsertArticle(article) : ArticleData.UpdateArticle(article);

            if (result)
            {
                ArticleForm.Visible = false;
                Success.Visible = true;
            }
        }

        [WebMethod]
        public static List<Models.FileUpload> GetFileListForCategory(string categoryId)
        {
            Guid CategoryID = Guid.Parse(categoryId);

            List<Models.FileUpload> FileList = FileUploadData.GetFilesForCategory(CategoryID);

            return FileList;
        }

        [WebMethod]
        public static Models.FileUpload AddFileToArticle(string articleId, string fileId)
        {
            Models.FileUpload returnData = new Models.FileUpload()
            {
                ID = FileUploadData.UploadArticleFile(Guid.Parse(articleId), Guid.Parse(fileId))
            };

            return returnData;
        }

        [WebMethod]
        public static List<ArticleTag> AddTagToArticle(string categoryId, string articleId, string tagId, string tagText)
        {
            Guid CategoryID = Guid.Parse(categoryId);
            Guid ArticleID = Guid.Parse(articleId);

            Guid TagID = (tagId == "") ? TagData.AddTagToCategory(CategoryID, tagText) : Guid.Parse(tagId);

            TagData.AddTagToArticle(ArticleID, TagID);

            return TagData.GetArticleTagsByArticleID(ArticleID);
        }

        [WebMethod]
        public static List<ArticleTag> RemoveTagFromArticle(string articleId, string articleTagId)
        {
            Guid ArticleID = Guid.Parse(articleId);
            Guid ArticleTagID = Guid.Parse(articleTagId);

            TagData.RemoveTagFromArticle(ArticleTagID);

            return TagData.GetArticleTagsByArticleID(ArticleID);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public static List<Tag> FilterTagList(string categoryId = null, string input = null) // 
        {
            return TagData.FilterTagList(Guid.Parse(categoryId), input);
        }
    }
}
