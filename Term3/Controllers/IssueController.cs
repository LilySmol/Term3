using NLog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Web.Mvc;
using TErm.Helpers.Clustering;
using TErm.Models;
using Term3.Helpers.Integration;

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
            //уже обновлено
            userId = userID;
            project.name = projectTitle;          
            ServerRequests serverRequests = new ServerRequests();
            serverRequests.baseUrl = resource.GetString("baseUrl");
            project.issuesList = serverRequests.getIssues(userId, projectTitle);
            //исправить получение оценочного времени
            List<ProjectModel> projects = serverRequests.getProjects(userId);
            project.estimate_time = projects
                .Find(project => project.name == projectTitle)
                .estimate_time;
            project.id = projects
                .Find(project => project.name == projectTitle)
                .id;
            return View(project);
        }

        [HttpPost]
        public ActionResult Issues(string action)
        {
            if(action == "showClasters")
            {
                return RedirectToAction("Clasters", "ClasterView", new { userID = userId, projectName  = project.name});
            }
            createClusters();
            prognosisIssuesAndProjectTime();

            ServerRequests serverRequest = new ServerRequests();
            serverRequest.baseUrl = resource.GetString("baseUrl");
            serverRequest.updateIssues(userId, project.issuesList);
            serverRequest.updateProjectTime(userId, project.id, project.estimate_time);

            return View(project);
        }

        protected void prognosisIssuesAndProjectTime()
        {
            if (userId != 0 && project.name != "")
            {
                InputDataConverter inputDataConverter = new InputDataConverter();
                int nearestCenter = 0;
                int projectEstimateTime = 0;
                foreach (IssuesModel issue in project.issuesList)
                {
                    nearestCenter = clustering.getNumberNearestCenter(inputDataConverter.convertToClusterObject(issue));
                    Cluster clusterCenter = clustering.ClusterList[clustering.getNumberNearestCenter(inputDataConverter.convertToClusterObject(issue))];
                    issue.estimate_time = clusterCenter.NearestObject.SpentTime;
                    issue.cluster_name = clusterCenter.NearestObject.Title;
                    projectEstimateTime += issue.estimate_time;
                    project.estimate_time = projectEstimateTime;
                    logger.Info("Задача: " + issue.name + " Oтносится к кластеру: " + clusterCenter.NearestObject.Title);
                }                
            }
        }

        protected void createClusters()
        {
            string testProjectName = resource.GetString("testProjectName");
            int clastersCount = Convert.ToInt32(resource.GetString("clastersCount"));
            ServerRequests serverRequest = new ServerRequests();
            serverRequest.baseUrl = resource.GetString("baseUrl");
            List<IssuesModel> issuesList = serverRequest.getIssues(userId, testProjectName);
            InputDataConverter inputDataConverter = new InputDataConverter();
            clustering = new Clustering(inputDataConverter.convertListToClusterObject(issuesList), clastersCount);
            clustering.initializationClusterCenters();
            clustering.clustering();
        }
    }
}