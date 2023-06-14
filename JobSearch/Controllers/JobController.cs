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
            var allposts = db.PostJobTables.Where(c=>c.CompanyID == companyid && c.UserID == userid).ToList();

            return View(allposts);
        }

        public ActionResult AddRequirements(int? id) 
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var details = db.JobRequirementDetailTables.Where(j=>j.PostJobID == id).ToList();
            if(details.Count() > 0)
            {
                details = details.OrderBy(r => r.JobRequirementID).ToList();   
            }
            var requirements = new JobRequirementsMV();
            requirements.Details = details;
            requirements.PostJobID = (int)id;
            ViewBag.JobRequirementID = new SelectList(db.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle", "0");
            return View(requirements);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRequirements(JobRequirementsMV jobRequirementsMV)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var requirements = new JobRequirementDetailTable();
            requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
            requirements.JobRequirementDetails = jobRequirementsMV.JobRequirementDetails;
            requirements.PostJobID = jobRequirementsMV.PostJobID;
            db.JobRequirementDetailTables.Add(requirements);
            db.SaveChanges();

            var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == requirements.PostJobID).ToList();
            if (details.Count() > 0)
            {
                details = details.OrderBy(r => r.JobRequirementID).ToList();
            }
            jobRequirementsMV.Details = details;
            ViewBag.JobRequirementID = new SelectList(db.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle", jobRequirementsMV.JobRequirementID);
            return View(jobRequirementsMV);
        }
    }
}