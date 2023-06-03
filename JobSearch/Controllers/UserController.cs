using DatabaseLayer;
using JobSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSearch.Controllers
{
    public class UserController : Controller
    {
        private jobsearchDBEntities db = new jobsearchDBEntities();
        // GET: User
        public ActionResult NewUser()
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1).ToList(), "UserTypeID", "UserType", "0");
            return View(new UserMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            ViewBag.UserTypeID = new SelectList(db.UserTypeTables.Where(u => u.UserTypeID != 1).ToList(), "UserTypeID", "UserType", userMV.UserTypeID);
            return View(new UserMV());
        }
    }
}