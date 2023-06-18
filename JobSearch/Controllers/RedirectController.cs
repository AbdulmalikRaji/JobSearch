using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSearch.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RedirectToExternalUrl(string redirectUrl)
        {
            return Redirect(redirectUrl);
        }
    }
}