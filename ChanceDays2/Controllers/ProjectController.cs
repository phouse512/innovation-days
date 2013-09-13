using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChanceDays2.Models;
using AutoMapper;

namespace ChanceDays2.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();

        //
        // GET: /Project/
        
        //updated to v2.0 with audit trail
        public ActionResult JoinTeam(int projectid = 1)
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();

            ProjectVersion currentVersion = db.Projects.Find(projectid).CurrentVersion;
            Project projectDetails = db.Projects.Find(projectid);

            ProjectView newProjectView = Mapper.Map<ProjectVersion, ProjectView>(currentVersion);
            Mapper.Map(projectDetails, newProjectView);

            List<Membership> memberships = db.Memberships.Where(b => b.Project.ProjectId == projectid).ToList();
            List<User> users = new List<User>();
            foreach (var membership in memberships)
            {
                users.Add(membership.Member);
            }
            newProjectView.Members = users;

            //begin constructing all member view
            List<User> allUsers = db.Users.ToList();
            foreach (User member in newProjectView.Members)
            {
                allUsers.Remove(member);
            }

            if (newProjectView == null)
            {
                return HttpNotFound("No project found");
            }

            var tuple = new Tuple<ProjectView, List<User>>(newProjectView, allUsers);
            return View(tuple);
        }
        
        //updated to v2.0 with audit trail
        public ActionResult AddUserToTeam(int projectid = 0, int userid = 0, string editorname = "default")
        {
            Mapper.CreateMap<ProjectVersion, ProjectVersion>();

            Project selectedProject = db.Projects.Find(projectid);
            ProjectVersion currentVersion = selectedProject.CurrentVersion;
            ProjectVersion newVersion = Mapper.Map<ProjectVersion, ProjectVersion>(currentVersion);

            //Edit changeLog, changeTime, changedBy
            User addUser = db.Users.Find(userid);
            newVersion.EditedBy = db.Users.Where(b=> b.Username == editorname).First();
            newVersion.EditedOn = DateTime.Now;
            newVersion.ChangeDescription = "added user, " + addUser.FirstName + " " + addUser.LastName;

            //set project to point to new version
            selectedProject.CurrentVersion = newVersion;

            if (currentVersion == null || addUser == null)
            {
                return HttpNotFound("No project found");
            }

            Membership newMembership = new Membership();
            newMembership.Member = addUser;
            newMembership.Project = selectedProject;
            db.Memberships.Add(newMembership);
            db.SaveChanges();

            TempData["SuccessMessage"] = addUser.FirstName + " " + addUser.LastName + " was successfully added to the team!";
            return RedirectToAction("Details", new { id = projectid });
        }
        
        //Updated to version 2.0 w/ audit trail
        public ActionResult RemoveUser(int projectid = 1)
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();

            ProjectVersion currentVersion = db.Projects.Find(projectid).CurrentVersion;
            Project projectDetails = db.Projects.Find(projectid);

            ProjectView newProjectView = Mapper.Map<ProjectVersion, ProjectView>(currentVersion);
            Mapper.Map(projectDetails, newProjectView);

            List<Membership> memberships = db.Memberships.Where(b => b.Project.ProjectId == projectid).ToList();
            List<User> users = new List<User>();
            foreach (var membership in memberships)
            {
                users.Add(membership.Member);
            }

            newProjectView.Members = users;

            if (currentVersion == null || projectDetails == null)
            {
                return HttpNotFound("No project or no users");
            }

            return View(newProjectView);
        }

        //Updated to version 2.0 w/ audit trail
        public ActionResult RemoveUserFromTeam(int projectid = 0, int userid = 0, string editorname = "default")
        {
            Mapper.CreateMap<ProjectVersion, ProjectVersion>();

            Project selectedProject = db.Projects.Find(projectid);
            ProjectVersion currentVersion = selectedProject.CurrentVersion;
            ProjectVersion newVersion = Mapper.Map<ProjectVersion, ProjectVersion>(currentVersion);

            //Edit changeLog, changeTime, changedBy
            User removeUser = db.Users.Find(userid);
            newVersion.EditedBy = db.Users.Where(b => b.Username == editorname).First();
            newVersion.EditedOn = DateTime.Now;
            newVersion.ChangeDescription = "removed user, " + removeUser.FirstName + " " + removeUser.LastName;

            selectedProject.CurrentVersion = newVersion;

            if (selectedProject == null || removeUser == null)
            {
                return HttpNotFound("No project or no users found");
            }

            Membership deleted = db.Memberships.Where(i => i.Project.ProjectId.Equals(projectid) && i.Member.UserId.Equals(userid)).First();
            db.Memberships.Remove(deleted);
            db.SaveChanges();

            TempData["SuccessMessage"] = removeUser.FirstName + " " + removeUser.LastName + " was successfully removed from the team!";
            return RedirectToAction("Details", new { id = projectid });
        }
        
        //Updated to v2.0 w/ audit trail
        public ActionResult Index()
        {
            return View();
        }
        
        //Updated to v2.0 w/ audit trail
        [HttpPost]
        public ActionResult SearchAll(string searchTerm, int requestedPage, int PageSize)
        {
            double pageSize = (double)PageSize;
            string userInput = string.Empty;
            List<Project> results;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                userInput = searchTerm.ToLower().Trim();
                results = db.Projects.Where(b => b.CurrentVersion.ProjectName.ToLower().Contains(userInput)).ToList();
                results.AddRange(db.Projects.Where(b => b.CurrentVersion.ChanceDay.ToLower().Contains(userInput)).ToList());
                results = results.Distinct().ToList();
            }
            else
            {
                results = db.Projects.ToList();
            }

            int pageNumbers = (int) Math.Ceiling(results.Count() / pageSize);
            int skipSize = (requestedPage - 1) * (int) pageSize;

            results = results.Skip(skipSize).Take((int)pageSize).ToList();

            PaginatedProjectSearchResults finalResults = new PaginatedProjectSearchResults();
            finalResults.currentPage = requestedPage;
            finalResults.totalPages = pageNumbers;

            //Convert results to projectView
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();
            
            ProjectVersion loopVersion = new ProjectVersion();
            List<ProjectView> projectViewResults = new List<ProjectView>();
            ProjectView newProjectView = new ProjectView();
            foreach (var singleProject in results)
            {
                loopVersion = singleProject.CurrentVersion;
                newProjectView = Mapper.Map<ProjectVersion, ProjectView>(loopVersion);
                Mapper.Map(singleProject, newProjectView);
                projectViewResults.Add(newProjectView);
            }

            finalResults.projectResults = projectViewResults;

            return PartialView("_SearchAll", finalResults);
        }

        //
        // GET: /Project/Details/5
        //
        //Updated to v2.0 w/ audit trail
        public ActionResult Details(int id = 0)
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();

            Project project = db.Projects.Find(id);
            ProjectVersion projectVersion = project.CurrentVersion;
            if (project == null)
            {
                return HttpNotFound();
            }

            ProjectView newProjectView = Mapper.Map<ProjectVersion, ProjectView>(projectVersion);
            Mapper.Map(project, newProjectView);

            List<Membership> memberships = db.Memberships.Where(b => b.Project.ProjectId == id).ToList();
            List<User> users = new List<User>();
            foreach (var membership in memberships)
            {
                users.Add(membership.Member);
            }
            newProjectView.Members = users;
            return View(newProjectView);
        }
        
        //
        // GET: /Project/Create
        //
        // Updated to v2.0 w/ audit trail
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create
        //
        // Updated to v2.0 w/ audit trail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectCreateView newProjectInfo)
        {
            Mapper.CreateMap<ProjectCreateView, ProjectVersion>();

            if (ModelState.IsValid)
            {
                Project newProject = new Project();
                User editor = db.Users.Where(b => b.Username == newProjectInfo.creatorName).First();
                newProject.CreatedBy = editor;
                newProject.CreatedOn = DateTime.Now;

                ProjectVersion newVersion = Mapper.Map<ProjectCreateView, ProjectVersion>(newProjectInfo);
                newVersion.ChangeDescription = "Project created";
                newVersion.EditedBy = editor;
                newVersion.EditedOn = DateTime.Now;
                
                



                newProject.CurrentVersion = newVersion;

                db.Projects.Add(newProject);
                db.ProjectVersions.Add(newVersion);
                db.SaveChanges();

                newVersion.parentId = newProject.ProjectId;
                db.SaveChanges();
            }
         
            return RedirectToAction("Index");
        }

        //
        // GET: /Project/Edit/5
        //
        // Updated to v2.0 w/ audit trail
        public ActionResult Edit(int projectid = 0)
        {
            Mapper.CreateMap<Project, ProjectView>();
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            
            Project project = db.Projects.Find(projectid);
            ProjectView editableProject = Mapper.Map<Project, ProjectView>(project);
            Mapper.Map(project.CurrentVersion, editableProject);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(editableProject);
        }

        //
        // POST: /Project/Edit/5
        //
        // Updated to v2.0 w/ audit trail
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectEditView unsavedProject)
        {
            Mapper.CreateMap<ProjectEditView, ProjectVersion>();
            if (ModelState.IsValid)
            {
                Project project = db.Projects.Find(unsavedProject.ProjectId);
                User editor = db.Users.Where(b => b.Username == unsavedProject.editorName).First();

                ProjectVersion newVersion = Mapper.Map<ProjectEditView, ProjectVersion>(unsavedProject);
                newVersion.ChangeDescription = "edited description";
                newVersion.EditedBy = editor;
                newVersion.EditedOn = DateTime.Now;
                newVersion.parentId = unsavedProject.ProjectId;

                project.CurrentVersion = newVersion;

                db.ProjectVersions.Add(newVersion);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your edits were saved!";
            }

            return RedirectToAction("Details", new { id = unsavedProject.ProjectId });
        }

        //
        // GET: /Project/Delete/5
        /*
        public ActionResult Delete(int id = 0)
        {
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //
        // POST: /Project/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        /*protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        } 
        */

        /*
         * 
         * Project Leader Editing - the following three controllers allow access and modification of the project leader positions, whether it be
         *                              remove, add, or modify
         *     Updated to v2.0 w/ audit trail                         
        */
        public ActionResult ModifyProjectLeaders(int projectid, int modifyType, int userid = 0, string editorname = "default")
        {
            Project project = db.Projects.Find(projectid);
            User user = new User();
            if (userid != 0)
            {
                user = db.Users.Find(userid);
            }

            string editor = editorname.Replace("/", @"\");

            Mapper.CreateMap<ProjectVersion, ProjectVersion>();
            ProjectVersion currentVersion = project.CurrentVersion;
            ProjectVersion newVersion = Mapper.Map<ProjectVersion, ProjectVersion>(currentVersion);

            //Edit changeLog, changeTime, changedBy
            newVersion.EditedBy = db.Users.Where(b => b.Username == editor).First();
            newVersion.EditedOn = DateTime.Now;
            string successString;

            //Change log distinction
            string changeType;
            if (userid == 0){
                changeType = "removed";
                successString = user.FirstName + " " + user.LastName + " was successfully removed ";
                user.FirstName = "";
                user.LastName = "";

            } else {
                changeType = "changed";
                successString = user.FirstName + " " + user.LastName + " was successfully added ";
            }  
            
            //Modify leader by modifyType
            if (modifyType == 0)
            {
                newVersion.Lead = user; 
                newVersion.ChangeDescription = changeType + " lead, " + user.FirstName + " " + user.LastName;
                successString += "as lead";
            } 
            else if (modifyType == 1)
            {
                newVersion.Owner = user;
                newVersion.ChangeDescription = changeType + " owner, " + user.FirstName + " " + user.LastName;
                successString += "as owner";
            }
            else if (modifyType == 2)
            {
                newVersion.Presenter = user;
                newVersion.ChangeDescription = changeType + " presenter, " + user.FirstName + " " + user.LastName;
                successString += "as presenter";
            }
            newVersion.parentId = projectid;

            project.CurrentVersion = newVersion;
            db.SaveChanges();

            TempData["SuccessMessage"] = successString;
            return RedirectToAction("Details", new { id = project.ProjectId });
        } 
    }
}