using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChanceDays2.Models;
using AutoMapper;

namespace ChanceDays2.Controllers
{
    public class EditHistoryController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();

        //
        // GET: /EditHistory/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RecentChanges(int projectid = 0)
        {
            Project currentProject = db.Projects.Find(projectid);
            List<ProjectVersion> recentVersions = db.ProjectVersions.Where(b => b.parentId.Equals(projectid)).OrderByDescending(b => b.EditedOn).Take(3).ToList();

            return PartialView("_RecentChanges", recentVersions);
        }

        public ActionResult ViewEditHistory(int projectid = 0)
        {
            Project currentProject = db.Projects.Find(projectid);
       
            return View(currentProject);
        }

        public ActionResult PaginatedViewHistory(int projectid = 0, int requestedPage = 1, int PageSize = 10)
        {
            double pageSize = (double)PageSize;
            List<ProjectVersion> results;

            results = db.ProjectVersions.Where(b => b.parentId.Equals(projectid)).OrderByDescending(b => b.EditedOn).ToList();

            int pageNumbers = (int)Math.Ceiling(results.Count() / pageSize);
            int skipSize = (requestedPage - 1) * (int)pageSize;

            results = results.Skip(skipSize).Take((int)pageSize).ToList();

            PaginatedEditHistory editHistory = new PaginatedEditHistory();
            editHistory.currentPage = requestedPage;
            editHistory.totalPages = pageNumbers;

            editHistory.results = results;
            editHistory.ParentId = projectid;

            return PartialView("_PaginatedEditHistory", editHistory);
        }

        public ActionResult SingleViewEdit(int projectversionId = 0)
        {
            ProjectVersion viewVersion = db.ProjectVersions.Find(projectversionId);

            return View(viewVersion);
        }

        public ActionResult RestoreVersion(int projectversionId = 0, string editorname = "default")
        {
            Mapper.CreateMap<ProjectVersion, ProjectVersion>().ForMember(x => x.ProjectVersionId, o => o.Ignore());
   
            ProjectVersion restoreVersion = db.ProjectVersions.Find(projectversionId);

            ProjectVersion newVersion = new ProjectVersion();
            newVersion = Mapper.Map<ProjectVersion, ProjectVersion>(restoreVersion);

            string text = editorname.Replace("/", "\\");
            newVersion.EditedBy = db.Users.Where(b => b.Username.Equals(text)).First();
            newVersion.EditedOn = DateTime.Now;
            newVersion.ChangeDescription = "restored project to old snapshot";

            Project project = db.Projects.Find(restoreVersion.parentId);
 

            db.ProjectVersions.Add(newVersion);
            project.CurrentVersion = newVersion;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Successfully restored project to old version!";
            return RedirectToAction("Details", "Project", new { id = restoreVersion.parentId });
        }
    }
}
