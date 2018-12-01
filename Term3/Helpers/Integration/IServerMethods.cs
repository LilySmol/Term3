using System.Collections.Generic;
using TErm.Models;

namespace TErm.Helpers.Integration
{
    interface IServerMethods
    {
        int addUser(string token, string userName);
        List<ProjectModel> getProjects(int userId);
        bool updateProjects(int userId);
        List<IssuesModel> getIssues(int userId, string projectName);
        bool updateIssues(int userId, List<IssuesModel> issuesList);
        bool updateProjectTime(int userId, int projectId, int? estimateTime);
    }
}
