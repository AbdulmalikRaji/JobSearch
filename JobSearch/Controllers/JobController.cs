using DatabaseLayer;
using JobSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSearch.Controllers
{
    public class JobController : Controller
    {
        private jobsearchDBEntities db = new jobsearchDBEntities();
        // GET: Job
        public ActionResult PostJob()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var job = new PostJobMV();
            ViewBag.JobCategoryID = new SelectList(
                db.JobCategoryTables.ToList(),
                    "JobCategoryID",
                    "JobCategory",
                    "O");
            ViewBag.JobNatureID = new SelectList(
                    db.JobNatureTables.ToList(),
                    "JobNatureID",
                    "JobNature",
                    "O");
            return View(job);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostJob(PostJobMV postJobMv)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            ViewBag.JobCategoryID = new SelectList(
                db.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                "O");
            ViewBag.JobNatureID = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                "O");
            return View(postJobMv);
        }
    }
}