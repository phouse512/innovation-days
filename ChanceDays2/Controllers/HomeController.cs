using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChanceDays2.Models;
using AutoMapper;

namespace ChanceDays2.Controllers
{
    public class HomeController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();
        
        public ActionResult Index()
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();
            List<Project> topSix = db.Projects.Take(6).ToList();
            List<ProjectView> topSixViews = new List<ProjectView>(topSix.Count);

            for (int i = 0; i < topSix.Count; i++)
            {
                ProjectView currentProject = Mapper.Map<ProjectVersion, ProjectView>(topSix[i].CurrentVersion);
                Mapper.Map<Project, ProjectView>(topSix[i], currentProject);
                topSixViews.Add(currentProject);
            }

            
            ViewBag.Message = "Welcome to the Chance Days Manager!";

            return View(topSixViews);
        }
        /*
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }*/
    }
}
