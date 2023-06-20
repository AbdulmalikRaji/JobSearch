using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class JobApplicationMV
    {
        public int PostJobID { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Skills are required.")]
        public string Skills { get; set; }

        [Required(ErrorMessage = "Experience details are required.")]
        public string Experience { get; set; }
        [Required(ErrorMessage = "Education details are required.")]
        public string Education { get; set; }
        [Display(Name = "CV")]
        public HttpPostedFileBase CVFile { get; set; }

    }
}