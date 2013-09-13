using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ChanceDays2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Restore",
                url: "Project/RestoreVersion/{projectversionId}/{*editorname}",
                defaults: new { controller = "EditHistory", action = "RestoreVersion", projectversionid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ViewSnapshot",
                url: "Project/ViewSnapshot/{projectversionId}",
                defaults: new { controller = "EditHistory", action = "SingleViewEdit", projectversionId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ViewEditHistory",
                url: "Project/ViewEditHistory/{projectid}",
                defaults: new { controller = "EditHistory", action = "ViewEditHistory", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "EditHistory",
                url: "Project/EditHistory",
                defaults: new { controller = "EditHistory", action = "PaginatedViewHistory" }
            );

            routes.MapRoute(
                name: "EditProject",
                url: "Project/Edit/{projectid}",
                defaults: new { controller = "Project", action = "Edit", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ViewFeedback",
                url: "Project/ViewFeedback/{projectid}",
                defaults: new { controller = "Feedback", action = "ViewFeedback", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddFeedback",
                url: "Project/AddFeedback/{projectid}",
                defaults: new { controller = "Feedback", action = "AddFeedback", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "ModifyLeader",
                url: "Project/ModifyProjectLeaders/{projectid}/{modifyType}/{userid}/{*editorname}",
                defaults: new { controller = "Project", action = "ModifyProjectLeaders", userid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Details",
                url: "Project/Details/{id}",
                defaults: new { controller = "Project", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "RemoveUser",
                url: "Project/RemoveUser/{projectid}",
                defaults: new { controller = "Project", action = "RemoveUser", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddUser",
                url: "Project/JoinTeam/{projectid}",
                defaults: new { controller = "Project", action = "JoinTeam", projectid = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}