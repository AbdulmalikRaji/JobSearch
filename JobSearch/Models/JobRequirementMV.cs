using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobSearch.Models
{
    public class JobRequirementMV
    {
        public JobRequirementMV()
        {
            Details = new List<JobRequirementDetailMV>();
        }
        public int JobRequirementID { get; set; }
        public string JobRequirementTitle { get; set; }
        public List<JobRequirementDetailMV> Details { get; set; }
        
    }
}