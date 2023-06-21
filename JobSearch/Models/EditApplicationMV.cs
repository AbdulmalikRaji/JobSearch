using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class EditApplicationMV
    {
        public int JobSeekerID { get; set; }
        public int PostJobID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNo { get; set; }
        public string Skills { get; set; }
        public string ExperienceID { get; set; }
        public int UserID { get; set; }
        public System.DateTime ApplicationDate { get; set; }
        public string Education { get; set; }
        public string CVFilePath { get; set; }
        public string JobApplyStatus { get; set; }
    }
}