using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class JobRequirementDetailMV
    {
        public JobRequirementDetailMV()
        {
            Details = new List<JobRequirementDetailMV>();
        }
        public int JobRequirementID { get; set; }
        public string JobRequirementDetails { get; set; }
        public List<JobRequirementDetailMV> Details { get; set; }
    }
}