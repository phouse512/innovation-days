using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChanceDays2.Models;
using AutoMapper;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;

namespace ChanceDays2.Controllers
{
    public class LogEntryController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();

        public ActionResult Index(int projectid)
        {
            List<LogEntry> currentMessages = db.LogEntries.Where(b => b.ParentProject.Equals(projectid)).ToList();

            //takes the comment and turns it into html
            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;

            foreach (var messages in currentMessages)
            {
                string MarkedUp = md.Transform(messages.Content);
                messages.Content = MarkedUp;
            }

            return PartialView("_loadLogs", currentMessages);
        }

        //
        // POST: /LogEntry/Create

        [HttpPost]
        public ActionResult Create(CreateLogEntry logentry)
        {
            LogEntry newEntry = new LogEntry();
            newEntry.ParentProject = logentry.parentProject;
            newEntry.Content = logentry.content;
            newEntry.PostedOn = DateTime.Now;
            Debug.Write(logentry.posterName);
            string text =logentry.posterName.Replace("WARE", "WARE\\");
            newEntry.PostedBy = db.Users.Where(b => b.Username == text).First();

            if (ModelState.IsValid)
            {
                db.LogEntries.Add(newEntry);
                db.SaveChanges();
                return new EmptyResult();
            }

            return new EmptyResult();
        }
    }

    public class Chat : Hub
    {
        ProjectDBContext db = new ProjectDBContext();

        public void Send(string LogMessage, int parentProject, string posterName)
        {
            LogEntry newEntry = new LogEntry();
            newEntry.Content = LogMessage;
            newEntry.ParentProject = parentProject;
            string text = posterName.Replace("WARE", "WARE\\");
            newEntry.PostedBy = db.Users.Where(b => b.Username == text).First();
            newEntry.PostedOn = DateTime.Now;

            db.LogEntries.Add(newEntry);
            db.SaveChanges();

            var md = new MarkdownDeep.Markdown();
            md.ExtraMode = true;
            md.SafeMode = false;

            string MarkdownOutput = md.Transform(LogMessage);   
            string output = "<div style='display: none;' class='custom-panels panel panel-default'><div class='panel-body' style='margin-bottom: 12px;'>" + MarkdownOutput + "<br /><i class='pull-right' style='font-size: smaller;'>Posted by " + newEntry.PostedBy.FirstName + " " + newEntry.PostedBy.LastName + " on " + newEntry.PostedOn.ToShortDateString() + "</i></div></div>";
            Clients.All.send(output, parentProject);
        }

    }
}