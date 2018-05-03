using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TErm.Models;
using System.Resources;
using System.Reflection;
using TErm.Helpers.Integration;
using TErm.Helpers.Clustering;
using NLog;
using TErm.Helpers.DataBase;

namespace TErm.Controllers
{
    public class HomeController : Controller
    {
        static Clustering clustering = new Clustering();        
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            createClusters();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModel person)
        {
            int userId = 0;
            string privateToken = person.Token;
            string name = person.Name;
            List<ProjectModel> projectList = getProjectsList(privateToken, name);
            if (projectList != null)
            {
                UserModel.Projects = projectList;
                userId = DataBaseRequest.getUserId(person.Name, person.Token);
                if (userId == 0) // пользователя нужно добавить в базу
                {
                    userId = addUserData(person);
                }
            }  
            else
            {
                return View();
            }       
            return RedirectToAction("Projects", "Project", new { userID = userId});
        }

        protected List<ProjectModel> getProjectsList(string token, string user)
        {
            GitLabParser gitLabParser = new GitLabParser();            
            gitLabParser.baseUrl = resource.GetString("baseUrl");
            return gitLabParser.getProjectsListByPrivateToken(token, user);         
        }

        protected void prognosis(UserModel user)
        {
            var project = from p in UserModel.Projects
                          where p.id == user.IdProjectForPrognosis
                          select p;
            ProjectModel projectForPrognosis = project.ToList()[0];
            InputDataConverter inputDataConverter = new InputDataConverter();
            foreach (IssuesModel issue in projectForPrognosis.issuesList)
            {
                if (issue.time_stats.time_estimate == 0 && issue.time_stats.total_time_spent == 0)
                {
                    Cluster clusterCenter = clustering.ClusterList[clustering.getNumberNearestCenter(inputDataConverter.convertToClusterObject(issue))];
                    issue.time_stats.time_estimate = clusterCenter.NearestObject.SpentTime;
                    logger.Info("Задача: " + issue.title + " Oтносится к кластеру: " + clusterCenter.NearestObject.Title);
                }
            }
        }

        protected void createClusters()
        {
            int testProjectId = Convert.ToInt32(resource.GetString("testProjectId"));
            List<ProjectModel> projectList = getProjectsList(resource.GetString("testProjectToken"), resource.GetString("testProjectUser"));
            InputDataConverter inputDataConverter = new InputDataConverter();
            var projectSelected = from project in projectList
                                  where project.id == testProjectId
                                  select project;
            ProjectModel projectWithTestData = projectSelected.ToList()[0];
            clustering = new Clustering(inputDataConverter.convertListToClusterObject(projectWithTestData.issuesList), 9);
            clustering.initializationClusterCenters();
            clustering.clustering();
        }

        protected int addUserData(UserModel user)
        {
            DataBaseRequest.insertUser(user.Name, user.Token);
            int userId = DataBaseRequest.getUserId(user.Name, user.Token);
            foreach (ProjectModel project in UserModel.Projects)
            {
                DataBaseRequest.insertProject(project.id, project.description, project.name, userId);
                foreach (IssuesModel issue in project.issuesList)
                {
                    DataBaseRequest.insertIssue(issue.id, issue.iid, issue.title, issue.description, project.id, issue.time_stats.total_time_spent, issue.time_stats.time_estimate);
                }
            }
            return userId;
        }
    }
}