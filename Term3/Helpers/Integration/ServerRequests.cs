using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using TErm.Helpers.Integration;
using TErm.Models;

namespace Term3.Helpers.Integration
{
    public class ServerRequests: Requests, IServerMethods
    {      
        public string baseUrl { get; set; }

        /// <summary>
        /// Добавить пользователя.
        /// </summary>
        public int addUser(string token, string userName)
        {
            var body = new NameValueCollection();
            body["token"] = token;
            body["username"] = userName;
            string response = post(baseUrl + "api/add-user", body);
            if (response.Equals("") || response.Contains("message"))
            {
                return 0;
            }
            UserModel responseObject = JsonConvert.DeserializeObject<UserModel>(response);           
            return responseObject.Id;
        }

        /// <summary>
        /// Получить список проектов пользователя по id.
        /// </summary>
        public List<ProjectModel> getProjects(int userId)
        {
            var body = new NameValueCollection();
            body["id"] = userId.ToString();
            string response = post(baseUrl + "api/get-projects", body);
            if (response.Equals("") || response.Contains("message"))
            {
                return null;
            }
            List<ProjectModel> projectsList = JsonConvert.DeserializeObject<List<ProjectModel>>(response);
            return projectsList;
        }

        /// <summary>
        /// Обновить список проектов пользователя по id.
        /// </summary>
        public bool updateProjects(int userId)
        {
            var body = new NameValueCollection();
            body["id"] = userId.ToString();
            string response = post(baseUrl + "api/update-user-projects", body);
            if (response.Equals(""))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получить список задач проекта по id пользователя и названию проекта.
        /// </summary>
        public List<IssuesModel> getIssues(int userId, string projectName)
        {
            var body = new NameValueCollection();
            body["id"] = userId.ToString();
            body["project_name"] = projectName;
            string response = post(baseUrl + "api/get-project-issues", body);
            if (response.Equals(""))
            {
                return null;
            }
            List<IssuesModel> issuesList = JsonConvert.DeserializeObject<List<IssuesModel>>(response);
            return issuesList;
        }

        /// <summary>
        /// Обновить оценочное время и кластер в списке задач проекта по id пользователя
        /// </summary>
        public bool updateIssues(int userId, List<IssuesModel> issuesList)
        {
            string json = "{\"user_id\":" + userId + ", \"issues\": [";
            for (int i = 0; i < issuesList.Count; i++)
            {
                json += "{\"id\":" + issuesList[i].id + ","
                    + "\"project_id\":" + issuesList[i].project_id + ","
                    + "\"cluster_name\":\"" + issuesList[i].cluster_name + "\","
                    + "\"estimate_time\":" + issuesList[i].estimate_time + "}";
                if (i != (issuesList.Count - 1))
                {
                    json += ",";
                }
            }
            json += "]}";
            string response = post(baseUrl + "api/update-issues", json);
            if (response.Equals(""))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Обновить оценочное время проекта по id пользователя и id проекта
        /// </summary>
        public bool updateProjectTime(int userId, int projectId, int? estimateTime)
        {
            string json = "{\"id\":" + projectId 
                + ", \"user_id\":" + userId 
                + ", \"estimate_time\":" + estimateTime 
                + "}";
            string response = post(baseUrl + "api/update-project", json);
            if (response.Equals(""))
            {
                return false;
            }
            return true;
        }
    }
}