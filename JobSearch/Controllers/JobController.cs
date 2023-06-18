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
        public ActionResult AllCompanyPendingJobs()
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
            if (allposts.Count() > 1)
            {
                allposts = allposts.OrderByDescending(c => c.PostJobID).ToList();
            }

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
            try
            {
                var requirements = new JobRequirementDetailTable();
                requirements.JobRequirementID = jobRequirementsMV.JobRequirementID;
                requirements.JobRequirementDetails = jobRequirementsMV.JobRequirementDetails;
                requirements.PostJobID = jobRequirementsMV.PostJobID;
                db.JobRequirementDetailTables.Add(requirements);
                db.SaveChanges();
                return RedirectToAction("AddRequirements", new { id = requirements.PostJobID });
            }
            catch (Exception)
            {
                var details = db.JobRequirementDetailTables.Where(j => j.PostJobID == jobRequirementsMV.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMV.Details = details;
                ModelState.AddModelError("JobRequirementID", "Required");
            }

            ViewBag.JobRequirementID = new SelectList(db.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle", jobRequirementsMV.JobRequirementID);
            return View(jobRequirementsMV);
        }

        public ActionResult DeleteRequirements(int? id)
        {
            var jobpostid = db.JobRequirementDetailTables.Find(id).PostJobID;
            var requirements = db.JobRequirementDetailTables.Find(id);
            db.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("AddRequirements", new { id = jobpostid });
        }
        public ActionResult DeleteJobPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            if (Convert.ToString(Session["UserTypeID"]) == "1")
            {
                return RedirectToAction("AllCompanyPendingJobs");

            }
            return RedirectToAction("CompanyJobsList");
        }
        public ActionResult ApprovedPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 2;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }
        public ActionResult CancelledPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var jobpost = db.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            db.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");
        }
        public ActionResult JobDetails(int? id)
        {
            var getpostjob = db.PostJobTables.Find(id);
            var postjob = new PostJobDetailsMV();
            postjob.PostJobID = getpostjob.PostJobID;
            postjob.Company = getpostjob.CompanyTable.CompanyName;
            postjob.JobCategory = getpostjob.JobCategoryTable.JobCategory;
            postjob.JobTitle = getpostjob.JobTitle;
            postjob.JobDescription = getpostjob.JobDescription;
            postjob.MinSalary = getpostjob.MinSalary;
            postjob.MaxSalary = getpostjob.MaxSalary;
            postjob.Location = getpostjob.Location;
            postjob.Vacancy = getpostjob.Vacancy;
            postjob.JobNature = getpostjob.JobNatureTable.JobNature;
            postjob.PostDate = getpostjob.PostDate;
            postjob.ApplicationLastDate = getpostjob.ApplicationLastDate;
            postjob.WebUrl = getpostjob.WebUrl;

            getpostjob.JobRequirementDetailTables = getpostjob.JobRequirementDetailTables.OrderBy(d => d.JobRequirementID).ToList();
            int jobrequirementid = 0;
            var jobrequirements = new JobRequirementMV();
            foreach (var detail in getpostjob.JobRequirementDetailTables)
            {
            var jobrequirementsdetails = new JobRequirementDetailMV();
                if(jobrequirementid == 0)
                {
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementsTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;
                } else if (jobrequirementid == detail.JobRequirementID) 
                {
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if(jobrequirementid != detail.JobRequirementID)
                {
                    postjob.Requirements.Add(jobrequirements);
                    jobrequirements = new JobRequirementMV();
                    jobrequirements.JobRequirementID = detail.JobRequirementID;
                    jobrequirements.JobRequirementTitle = detail.JobRequirementsTable.JobRequirementTitle;
                    jobrequirementsdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementsdetails.JobRequirementDetails = detail.JobRequirementDetails;
                    jobrequirements.Details.Add(jobrequirementsdetails);
                    jobrequirementid = detail.JobRequirementID;
                }
            }
            postjob.Requirements.Add(jobrequirements);
            return View(postjob);
        }
        public ActionResult FilterJob()
        {
            var obj = new FilterJobMV();
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2).ToList();
            obj.Result = result;
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
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FilterJob(FilterJobMV filterJobMV)
        {
            var date = DateTime.Now.Date;
            var result = db.PostJobTables.Where(r => r.ApplicationLastDate >= date && r.JobStatusID == 2 && (r.JobCategoryID == filterJobMV.JobCategoryID || r.JobNatureID == filterJobMV.JobNatureID)).ToList();
            filterJobMV.Result = result;

            ViewBag.JobCategoryID = new SelectList(
               db.JobCategoryTables.ToList(),
               "JobCategoryID",
               "JobCategory",
               filterJobMV.JobCategoryID);
            ViewBag.JobNatureID = new SelectList(
                db.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                filterJobMV.JobNatureID);
            return View(filterJobMV);
        }
    }
}