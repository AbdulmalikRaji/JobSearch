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
                var user = db.UserTables.Where(u => u.Username == userLoginMV.Username && u.Password == userLoginMV.Password).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Enter correct Username/Password");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["Username"] = user.Username;
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
            Session["Username"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            Session["UserTypeID"] = string.Empty;

            return RedirectToAction("Index", "Home");
        }
    }
}