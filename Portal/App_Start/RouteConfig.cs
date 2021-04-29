using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace Portal
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            // settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
            routes.MapPageRoute(
                "cms-admin-dashboard",              // route name
                "cms/admin/{controller}/dashboard", // url w/ params
                "~/cms/views/admin/dashboard.aspx"        // page to handle it
            );
            routes.MapPageRoute(
                "cms-admin-add-new",                    // route name
                "cms/admin/{controller}/add-new",       // url w/ params
                "~/cms/views/admin/form.aspx"           // page to handle it
            );
            routes.MapPageRoute(
                "cms-admin-edit",                       // route name
                "cms/admin/{controller}/edit/{id}",     // url w/ params
                "~/cms/views/admin/form.aspx"           // page to handle it
            );
            routes.MapPageRoute(
                "cms-tag-article-list",                 // route name
                "cms/{controller}/tag/{tag}",           // url w/ params
                "~/cms/views/tag_search.aspx"           // page to handle it
            );
            routes.MapPageRoute(
                "cms-view-article",           // route name
                "cms/{controller}/{article}", // url w/ params
                "~/cms/views/article.aspx"          // page to handle it
            );
            routes.MapPageRoute(
                "cms-view-article-list",            // route name
                "cms/{controller}/",                // url w/ params
                "~/cms/views/articles.aspx"         // page to handle it
            );
            routes.MapPageRoute(
                "cms-view-db-image",            // route name
                "img/image/{imgId}/",           // url w/ params
                "~/cms/views/image.aspx"        // page to handle it
            );
            routes.MapPageRoute(
                "cms-view-db-video",            // route name
                "vid/video/{vidId}/",           // url w/ params
                "~/cms/views/video.aspx"        // page to handle it
            );
        }
    }
}
