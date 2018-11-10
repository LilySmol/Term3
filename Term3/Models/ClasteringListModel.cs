using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TErm.Models
{
    public class ClasteringListModel
    {
        public string clasterName { get; set; }
        public int clasterHours { get; set; }
        public List<string> taskList { get; set; }

        public ClasteringListModel(string clasterName, int clasterHours, List<string> taskList)
        {
            this.clasterName = clasterName;
            this.clasterHours = clasterHours;
            this.taskList = taskList;
        }
    }
}