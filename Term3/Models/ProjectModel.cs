using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class ProjectModel
    {
        public int id { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public LinksModel _links { get; set; }
        public List<IssuesModel> issuesList { get; set; }
    }
}