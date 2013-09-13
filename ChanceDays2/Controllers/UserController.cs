using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChanceDays2.Models;


namespace ChanceDays2.Controllers
{
    public class UserController : Controller
    {
        private ProjectDBContext db = new ProjectDBContext();

        //
        // GET: /User/
        //
        // Updated to v2.0 w/ audit trail
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
        
        //
        // GET: /User/Details/5
        // Updated to v2.0 w/ audit trail
        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET return a user from username
        //
        // Updated to v2.0 w/ audit trail
        //
        // This runs on every page load to grab the username, but if for some reason the username is not in the database, 
        // it adds the user to the database so he/she can edit his/her information (ie. add firstname, lastname, etc.)
        public ActionResult returnUser()
        {
            try
            {
                User user = db.Users.Where(b => b.Username.Equals(User.Identity.Name)).First();
                return PartialView(user);
            }
            catch (Exception ex)
            {
                User newUser = new User();
                newUser.Username = User.Identity.Name;

                db.Users.Add(newUser);
                db.SaveChanges();

                User newestUser = db.Users.Where(b => b.Username.Equals(User.Identity.Name)).First();
                TempData["NewUser"] = "Welcome to Innovation Days for the first time! Would you like to take some time to add some info to your profile?";
                TempData["Url"] = Url.Action("Edit", "User", new { id = newestUser.UserId, username = User.Identity.Name });
                return PartialView(newUser);
            }
        }

        //
        // Ajax Search of users by input search term, and output type (modal/normal table)
        //
        // Updated to v2.0 w/ audit trail
        [HttpGet]
        public ActionResult SearchAll(string searchTerm, int modalWindow)
        {
            string searchView;
            if (modalWindow == 1)
            {
                searchView = "_Search";
            }
            else
            {
                searchView = "_SearchAll";
            }
            string userInput = string.Empty;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                userInput = searchTerm.ToLower().Trim();
            }
            else
            {
                return PartialView(searchView, db.Users.ToList());
            }

            List<User> results = db.Users.Where(b => b.FirstName.ToLower().Contains(userInput)).ToList();
            results.AddRange(db.Users.Where(b => b.LastName.ToLower().Contains(userInput)).ToList());
            results.AddRange(db.Users.Where(b => b.Title.ToLower().Contains(userInput)).ToList());
            results = results.Distinct().ToList();

            return PartialView(searchView, results);
        }

        
        //
        // GET: /User/Edit/5
        //
        // Updated to v2.0 w/ audit trail
        public ActionResult Edit(int id = 0, string username = "default")
        {
            var query = (from t in db.Users select t);

            if (query == null)
            {
                return RedirectToAction("Index");
            }

            query = query.Where(item => item.UserId == id);

            if (query == null)
            {
                return RedirectToAction("Index");
            }
            query = query.Where(item => item.Username == username);

            if (query == null)
            {
                return RedirectToAction("Index");
            }
            User user = query.FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5
        //
        // Updated to v2.0 w/ audit trail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Profile successfully updated!";
                return RedirectToAction("Details", new { id = user.UserId });
            }
            return View(user);
        }

        //
        // GET: /User/Delete/5
        /*
        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        } */
    }
}