using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class UserMV
    {
        public UserMV()
        {
            Company = new CompanyMV();
        }
        public int UserID { get; set; }
        public int UserTypeID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public bool IsProvider { get; set; }
        public CompanyMV Company { get; set; }
    }
}