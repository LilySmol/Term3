using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class IssuesModel
    {
        //public enum stateEnum { opened, closed }

        public int id { get; set; }          //общее id задачи
        public int iid { get; set; }         //id задачи
        public string title { get; set; }
        public string description { get; set; }
        //public stateEnum state { get; set; }         
        //public DateTime created_at { get; set; } 
        //public DateTime updated_at { get; set; } 
        public TimeStatsModel time_stats { get; set; }
    }
}