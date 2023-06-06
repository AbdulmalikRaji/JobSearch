using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class UserLoginMV
    {
        [Required(ErrorMessage = "Required")]
        public String Username { get; set; }
        [Required(ErrorMessage = "Required")]
        public String Password { get; set; }
    }
}