using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using TErm.Helpers.Clustering;
using TErm.Helpers.DataBase;
using TErm.Models;
using Term3.Helpers.DataBase;
using Term3.Helpers.Integration;
using Term3.Models;

namespace Term3.Controllers
{
    public class IssueController : Controller
    {
        static ProjectModel project = new ProjectModel();
        static Clustering clustering = new Clustering();
        private Logger logger = LogManager.GetCurrentClassLogger();
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());
        static int userId = 0;

        // GET: Issue
        public ActionResult Issues(int userID, string projectTitle)
        {
            userId = userID;
            project.name = projectTitle;
            ServerRequests serverRequests = new ServerRequests();
            project.issuesList = serverRequests.getIssues(userId, projectTitle);
            //исправить получение оценочного времени
            project.projectTime = 0;
            return View(project);
        }

        [HttpPost]
        public ActionResult Issues(string action)
        {
            if(action == "showClasters")
            {
                return RedirectToAction("Clasters", "ClasterView");
            }
            createClusters();
            prognosisLeadTime();
            foreach (IssuesModel issue in project.issuesList)
            {
                double estimateTime = issue.time_stats.time_estimate * 3600;
                DataBaseRequest.updateEstimateTime(issue.id, estimateTime);
            }
            DataBaseRequest.updateProjectTime(project.name, project.projectTime, userId);
            return View(project);
        }

        protected void prognosisLeadTime()
        {
            if (userId != 0 && project.name != "")
            {
                InputDataConverter inputDataConverter = new InputDataConverter();
                foreach (IssuesModel issue in project.issuesList)
                {
                    Cluster clusterCenter = clustering.ClusterList[clustering.getNumberNearestCenter(inputDataConverter.convertToClusterObject(issue))];
                    issue.time_stats.time_estimate = clusterCenter.NearestObject.SpentTime / 3600;
                    project.projectTime += issue.time_stats.time_estimate;
                    logger.Info("Задача: " + issue.name + " Oтносится к кластеру: " + clusterCenter.NearestObject.Title);
                }                
            }
        }

        protected void createClusters()
        {
            int testProjectId = Convert.ToInt32(resource.GetString("testProjectId"));
            List<ProjectModel> projectList = DataBaseHelper.getProjectsList(resource.GetString("testProjectToken"), resource.GetString("testProjectUser"));
            InputDataConverter inputDataConverter = new InputDataConverter();
            var projectSelected = from project in projectList
                                  where project.id == testProjectId
                                  select project;
            ProjectModel projectWithTestData = projectSelected.ToList()[0];
            clustering = new Clustering(inputDataConverter.convertListToClusterObject(projectWithTestData.issuesList), 11);
            clustering.initializationClusterCenters();
            clustering.clustering();
        }
    }
}