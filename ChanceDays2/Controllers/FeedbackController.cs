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
    public class FeedbackController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();
        
        //
        // GET: /Feedback/

        public ActionResult Index()
        {
            return View(db.Feedbacks.ToList());
        }

        /*
         * 
         * Add Feedback
         * 
         */
        public ActionResult AddFeedback(int projectid = 0)
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();

            Project project = db.Projects.Find(projectid);

            ProjectView feedbackProject = Mapper.Map<Project, ProjectView>(project);
            Mapper.Map(project.CurrentVersion, feedbackProject);

            return View(feedbackProject);
        }
        
        [HttpPost]
        public ActionResult AddFeedback(NewFeedback newFeedback)
        {
            Feedback feedback = new Feedback();
            feedback.Comment = newFeedback.comment;
            feedback.Poster = db.Users.Where(user => user.Username == newFeedback.poster).First();
            feedback.insertDate = DateTime.Now;

            feedback.owner = db.Projects.Find(newFeedback.parentProject);

            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Feedback successfully added!";
            return RedirectToAction("ViewFeedback", new { projectid = newFeedback.parentProject });
        }
        

        //
        // GET: /Project/Feedback/projectid
        //
        // Updated to v2.0 w/ audit trails
        public ActionResult ViewFeedback(int projectid = 0)
        {
            Mapper.CreateMap<ProjectVersion, ProjectView>();
            Mapper.CreateMap<Project, ProjectView>();

            Project project = db.Projects.Find(projectid);

            ProjectView feedbackProject = Mapper.Map<Project, ProjectView>(project);
            Mapper.Map(project.CurrentVersion, feedbackProject);

            feedbackProject.Feedbacks = db.Feedbacks.Where(b => b.owner.ProjectId.Equals(projectid)).ToList();

            //takes the comment and turns it into markedup html
            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;

            foreach (var feedback in feedbackProject.Feedbacks)
            {
                string MarkedUp = md.Transform(feedback.Comment);
                feedback.Comment = MarkedUp;
            }

            return View(feedbackProject);
        }

        //
        // GET: /Feedback/Details/5
        /*
        public ActionResult Details(int id = 0)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);


        }

        //
        // GET: /Feedback/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Feedback/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feedback);
        }

        //
        // GET: /Feedback/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        //
        // POST: /Feedback/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feedback);
        }

        //
        // GET: /Feedback/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        //
        // POST: /Feedback/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }*/
    }
}