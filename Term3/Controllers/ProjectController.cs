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
using TErm.Helpers.Integration;
using TErm.Models;
using Term3.Helpers.DataBase;

namespace Term3.Controllers
{
    public class ProjectController : Controller
    {
        private static int userId = 0;
        private static string projectName = "";
        private ProjectModel project = new ProjectModel();
        static Clustering clustering = new Clustering();
        static ResourceManager resource = new ResourceManager("TErm3.Resource", Assembly.GetExecutingAssembly());
        private Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Project
        public ActionResult Projects(int userID)
        {
            userId = userID;
            
            DataTable projectsTable = new DataTable();
            if (userID != 0) //пользователь есть в базе данных
            {
                projectsTable = DataBaseRequest.getProjects(userID);
            }
            project.projectsList = new List<SelectListItem>();
            for (int i = 0; i < projectsTable.Rows.Count; i++)
            {
                project.projectsList.Add(new SelectListItem { Text = projectsTable.Rows[i]["name"].ToString(), Value = projectsTable.Rows[i]["name"].ToString() });
            }
            return View(project);
        }

        [HttpPost]
        public ActionResult Projects(ProjectModel project)
        {
            this.project = project;
            //if (project.name != null)
            //{
            //    projectName = project.name;
            //}                

            //DataTable issuesTable = DataBaseRequest.getIssues(userId, projectName);
            //this.project.issuesList = new List<IssuesModel>();
            //double estimateTime = 0;
            //double spentTime = 0;
            //foreach (DataRow row in issuesTable.Rows)
            //{
            //    spentTime = Convert.ToDouble(row["spentTime"]) / 3600;
            //    estimateTime = Convert.ToDouble(row["estimateTime"]) / 3600;
            //    this.project.issuesList.Add(new IssuesModel(row["title"].ToString(), row["description"].ToString(), spentTime, estimateTime));
            //}

            //if (project.name == null) //необходимо спрогнозировать оценочное время задач
            //{                
            //    createClusters();
            //    prognosis();
            //}
            projectName = project.name;
            return RedirectToAction("Issues", "Issue", new { userID = userId, projectTitle = projectName });
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
        
        protected void prognosis()
        {
            if (userId != 0 && projectName != "")
            {
                InputDataConverter inputDataConverter = new InputDataConverter();
                foreach (IssuesModel issue in project.issuesList)
                {
                    Cluster clusterCenter = clustering.ClusterList[clustering.getNumberNearestCenter(inputDataConverter.convertToClusterObject(issue))];
                    issue.time_stats.time_estimate = clusterCenter.NearestObject.SpentTime / 3600;
                    logger.Info("Задача: " + issue.title + " Oтносится к кластеру: " + clusterCenter.NearestObject.Title);
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
            clustering = new Clustering(inputDataConverter.convertListToClusterObject(projectWithTestData.issuesList), 9);
            clustering.initializationClusterCenters();
            clustering.clustering();
        }
    }
}