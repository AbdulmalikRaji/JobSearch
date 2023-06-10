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
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            postJobMv.UserID = userid;
            postJobMv.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                var post = new PostJobTable();
                post.UserID = postJobMv.UserID;
                post.CompanyID = postJobMv.CompanyID;
                post.JobCategoryID = postJobMv.JobCategoryID;
                post.JobTitle = postJobMv.JobTitle;
                post.JobDescription = postJobMv.JobDescription;
                post.MinSalary = postJobMv.MinSalary;
                post.MaxSalary = postJobMv.MaxSalary;
                post.Location = postJobMv.Location;
                post.Vacancy = postJobMv.Vacancy;
                post.JobNatureID = postJobMv.JobNatureID;
                post.PostDate = DateTime.Now;
                post.ApplicationLastDate = postJobMv.ApplicationLastDate;
                post.LastDate = postJobMv.ApplicationLastDate;
                post.JobStatusID = 1;
                post.WebUrl = postJobMv.WebUrl;
                db.PostJobTables.Add(post);
                db.SaveChanges();
                return RedirectToAction("CompanyJobsList");
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
        public ActionResult CompanyJobsList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allposts = db.PostJobTables.ToList();

            return View(allposts);
        }
    }
}