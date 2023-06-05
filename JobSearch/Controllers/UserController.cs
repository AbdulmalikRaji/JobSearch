using DatabaseLayer;
using JobSearch.Models;
using LanguageExt;
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
                    ModelState.AddModelError("Email Address", "Email Address Is Already Registered");
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
                    var user = new UserTable();
                user.Username = userMV.Username;
                user.Password = userMV.Password;
                user.EmailAddress = userMV.EmailAddress;
                user.UserTypeID = userMV.IsProvider == true ? 2 : 3;
                user.ContactNo = userMV.ContactNo;
                db.UserTables.Add(user);
                db.SaveChanges();

                if (userMV.IsProvider == true)
                {
                    
                        var company = new CompanyTable();
                        company.UserID = user.UserID;
                        company.EmailAddress = userMV.Company.EmailAddress;
                        company.CompanyName = userMV.Company.CompanyName;
                        company.ContactNo = userMV.Company.ContactNo;
                        company.Logo = "~/Content/assets/img/logo/logo.png";
                    company.Description = userMV.Company.Description;
                        db.CompanyTables.Add(company);
                        db.SaveChanges();


                    }
                    trans.Commit();
                    return RedirectToAction("Login");
                }

            }
                return View(userMV);
        }
    }
}