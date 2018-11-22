using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TErm.Models;
using Term3.Helpers.Integration;

namespace Term3.Controllers
{
    public class ClasterViewController : Controller
    {
        // GET: ClasterView
        public ActionResult Clasters(int userID, string projectName)
        {
            List<ClasteringListModel> clastersList = new List<ClasteringListModel>();
            ServerRequests serverRequests = new ServerRequests();
            List<IssuesModel> issuesList = serverRequests.getIssues(userID, projectName);
            bool clasterIsExist = false;
            foreach (IssuesModel issue in issuesList)
            {
                if (clastersList.Count == 0)
                {
                    List<string> issuesInClaster = new List<string>() { issue.name};
                    clastersList.Add(new ClasteringListModel(issue.cluster_name, issue.estimate_time, issuesInClaster));
                }
                else
                {
                    foreach (ClasteringListModel claster in clastersList)
                    {
                        if (claster.clasterName == issue.cluster_name)
                        {
                            claster.taskList.Add(issue.name);
                            clasterIsExist = true;
                            break;
                        }
                    }
                    if (!clasterIsExist)
                    {
                        List<string> issuesInClaster = new List<string>() { issue.name };
                        clastersList.Add(new ClasteringListModel(issue.cluster_name, issue.estimate_time, issuesInClaster));                     
                    }
                    clasterIsExist = false;
                }
            }
            return View(clastersList);
        }
    }
}