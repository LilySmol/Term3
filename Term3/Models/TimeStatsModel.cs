using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class TimeStatsModel
    {
        public int time_estimate { get; set; }    //в секундах
        public int total_time_spent { get; set; } //в секундах
        //public string human_time_estimate { get; set; }
        //public string human_total_time_spent { get; set; }
    }
}