using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class IssuesModel
    {
        public int id { get; set; }          //общее id задачи
        public int issue_id { get; set; }         //id задачи
        public long project_id { get; set; }
        public string name { get; set; }
        public string desc { get; set; }
        public double spent_time { get; set; }
        public double estimate_time { get; set; }
        public string cluster_name { get; set; }

        public IssuesModel(int id, string title, string description, double spendTime, double estimateTime)
        {
            this.id = id;
            this.name = title;
            this.desc = description;
            this.spent_time = spendTime;
            this.estimate_time = estimateTime;
        }
    }
}