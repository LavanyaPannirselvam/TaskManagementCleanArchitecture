using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;


namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IProjectHandler
    {
        int AddProject(Project project);
        void DeleteProject(int projectId);
        Project GetProject(int projectId);
        void UpdateProject(Project project);
        List<Project> ProjectsList(string userName,string userEmail,int count,int skipCount);
        List<Tasks> AssignedTasksListOfAProject(int projectId, int count, int skipCount);
        List<Issue> AssignedIssuesListOfAProject(int projectId,int count,int skipCount);
    }
}
