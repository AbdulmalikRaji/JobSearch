using DatabaseLayer;
using JobSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BCrypt.Net;



namespace JobSearch.Controllers
{
    public class UserController : Controller
    {
        private jobsearchDBEntities db = new jobsearchDBEntities();
        // GET: User
        public ActionResult NewUser()
        {
            return View(new UserMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV userMV)
        {
            if (ModelState.IsValid)
            {
                var checkuser = db.UserTables.Where(u => u.EmailAddress == userMV.EmailAddress).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("EmailAddress", "Email Address Is Already Registered");
                    return View(userMV);
                }
                checkuser = db.UserTables.Where(u =>u.Username == userMV.Username).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("Username", "Username Is Already Registered");
                    return View(userMV);
                }
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new UserTable();
                        user.Username = userMV.Username;
                        string salt = BCrypt.Net.BCrypt.GenerateSalt();
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userMV.Password, salt);
                        user.Password = hashedPassword;
                        user.EmailAddress = userMV.EmailAddress;
                        user.UserTypeID = userMV.IsProvider == true ? 2 : 3;
                        user.ContactNo = userMV.ContactNo;
                        db.UserTables.Add(user);
                        db.SaveChanges();

                        if (userMV.IsProvider == true)
                        {

                            var company = new CompanyTable();
                            
                            company.UserID = user.UserID;
                            if (string.IsNullOrEmpty(userMV.Company.EmailAddress)) 
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Required");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Required");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.PhoneNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.PhoneNo", "Required");
                                return View(userMV);
                            }
                            if (string.IsNullOrEmpty(userMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Required");
                                return View(userMV);
                            }
                            company.EmailAddress = userMV.Company.EmailAddress;
                            company.CompanyName = userMV.Company.CompanyName;
                            company.ContactNo = userMV.ContactNo;
                            company.PhoneNo = userMV.Company.PhoneNo;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            company.Description = userMV.Company.Description;
                            db.CompanyTables.Add(company);
                            db.SaveChanges();


                        }
                        trans.Commit();
                        return RedirectToAction("Login");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(String.Empty, "Please Provide Correct Details");
                        trans.Rollback();
                    }
                   
                }

            }
                return View(userMV);
        }
        public ActionResult Login() 
        {
            return View(new UserLoginMV());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMV userLoginMV)
        {
            if(ModelState.IsValid)
            {
                var user = db.UserTables.FirstOrDefault(u => u.EmailAddress == userLoginMV.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginMV.Password, user.Password))
                {
                    ModelState.AddModelError(String.Empty, "Enter correct Email/Password");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["Email"] = user.EmailAddress;
                Session["UserTypeID"] = user.UserTypeID;
                if(user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }

                return RedirectToAction("Index", "Home");
            }
            return View(userLoginMV);
        }
        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["Email"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AllUsers()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");

            }
            var users = db.UserTables.ToList();
            return View(users);

        }
        public ActionResult Forgot()
        {
            return View(new ForgotPasswordMv());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forgot(ForgotPasswordMv forgotPasswordMv)
        {
            var user = db.UserTables.Where(u => u.EmailAddress == forgotPasswordMv.Email).FirstOrDefault();
            if (user != null) 
            {
                ModelState.AddModelError(string.Empty, "Password Has been reset successfully");
            }
            else
            {
                ModelState.AddModelError("Email", "Email Address is Incorrect");
            }
            
            return View();
        }
        public ActionResult AppliedJobs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var userId = (int)Session["UserID"];
            var applications = db.JobSeekerTables.Where(j => j.UserID == userId).ToList();
            return View(applications);
        }
        public ActionResult EditApplication(int id)
        {
            var application = db.JobSeekerTables.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }

            // Check if the application was submitted within the last 24 hours
            var currentTime = DateTime.Now;
            var submissionTime = application.ApplicationDate;
            var timeDifference = currentTime - submissionTime;
            var hoursDifference = timeDifference.TotalHours;
            if (hoursDifference > 24)
            {
                ModelState.AddModelError(string.Empty, "Can only Edit application in First day of posting");

            }
            return View(application);
        }
        [HttpPost]
        public ActionResult UpdateApplication(JobSeekerTable updatedApplication)
        {
            var application = db.JobSeekerTables.Find(updatedApplication.JobSeekerID);
            if (application == null)
            {
                return HttpNotFound();
            }
            application.FirstName = updatedApplication.FirstName;
            application.LastName = updatedApplication.LastName;
            application.ContactNo = updatedApplication.ContactNo;
            application.EmailAddress = updatedApplication.EmailAddress;
            application.Skills = updatedApplication.Skills;
            application.ExperienceID = updatedApplication.ExperienceID;
            application.Education = updatedApplication.Education;
            // Update other fields as needed


            db.SaveChanges();

            return RedirectToAction("AppliedJobs");
        }
        public ActionResult DeleteApplication(int id)
        {
            var application = db.JobSeekerTables.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }

            // Delete the application from the database
            db.JobSeekerTables.Remove(application);
            db.SaveChanges();

            return RedirectToAction("AppliedJobs");
        }
    }
}