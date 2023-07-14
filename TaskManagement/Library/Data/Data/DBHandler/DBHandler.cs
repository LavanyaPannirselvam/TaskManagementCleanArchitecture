using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Data;
using TaskManagementLibrary.Data.DBAdapter;
using TaskManagementLibrary.Enums;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementLibrary.Models;
using Windows.System;
using Windows.UI.Xaml.Media;
using TaskManagementLibrary.Models.Enums;

namespace TaskManagementLibrary.Data.DBHandler
{
    public class DBHandler : IDBHandler
    {
        private IDBAdapter _adapter;
        
        public DBHandler()
        {
            _adapter = Domain.ServiceProvider.GetInstance().Services.GetService<IDBAdapter>();
        }

        #region Project section
        public int AddProject(Project project)
        {
            _adapter.Add(project);
            return project.Id;
        }

        public void DeleteProject(int projectId)
        {
            string query = "SELECT * FROM Project WHERE Id = @projectId";
            var toDelete = _adapter.GetFromQuery<Project>(query, projectId);
            _adapter.Delete(toDelete);
        }

        public Project GetProject(int projectId)
        {
            string query = "SELECT * FROM Project WHERE Id = @projectId";
            var toGet = _adapter.GetFromQuery<Project>(query, projectId);
            return toGet.FirstOrDefault();
        }

        public List<Project> ProjectsList(string userName,string userEmail)
        {
            var query = "SELECT * FROM Project WHERE CreatedBy IN (SELECT Name FROM User WHERE Name = @userName AND Email = @userEmail)";
            var list = _adapter.GetFromQuery<Project>(query,userName,userEmail);
            return list.ToList();
        }

        public void UpdateProject(Project project)
        {
            _adapter.Update(project);
        }

        public List<Tasks> AssignedTasksList(int projectId)
        {
            var query = "SELECT * FROM Tasks WHERE ProjectId = @projectId";
            var toGet = _adapter.GetFromQuery<Tasks>(query, projectId);
            return toGet;
        }

        
        #endregion

        #region Task section 
        public int AddTask(Tasks task)
        {
            _adapter.Add(task);
            return task.Id;
        }

        public void DeleteTask(int taskId)
        {
            string query = "SELECT * FROM Tasks WHERE Id = @taskId";
            var toDelete = _adapter.GetFromQuery<Tasks>(query, taskId).FirstOrDefault();
            _adapter.Delete(toDelete);
        }

        public Tasks GetTask(int taskId)
        {
            string query = "SELECT * FROM Tasks WHERE Id = @taskId";
            var toGet = _adapter.GetFromQuery<Tasks>(query,taskId);
            return toGet.FirstOrDefault();
        }

        public List<Tasks> TasksList()
        {
            string query = "SELECT * FROM Tasks";
            var list = _adapter.GetFromQuery<Tasks>(query);
            return list.ToList();
        }

       
        #endregion

        #region User section
        public void AddUser(Models.User user)
        {
            _adapter.Add(user);
        }

        public void DeleteUser(string email)
        {
            string query = "SELECT * FROM User WHERE Email = @email";
            var toDelete = _adapter.GetFromQuery<Models.User>(query, email);
            _adapter.Delete(toDelete);
        }

        public Models.User GetUser(int userId)
        {
            string query = "SELECT * FROM User WHERE UserId = @userId";
            var toGet = _adapter.GetFromQuery<Models.User>(query,userId);
            return toGet.FirstOrDefault();
        }

        public Models.User GetUser(string email)
        {
            string query = "SELECT * FROM User WHERE Email = @email";
            var user = _adapter.GetFromQuery<Models.User>(query, email).FirstOrDefault();
            return user;
        }

        public List<Models.User> UsersList()
        {
            string query = "SELECT * FROM User "; 
            var list = _adapter.GetFromQuery<Models.User>(query); 
            return list.ToList();
        }

        public bool CheckUser(string email)
        {
            var query = "SELECT * FROM USER WHERE EMAIL = @email";
            var result = _adapter.GetFromQuery<Models.User>(query,email);
            return result.Count != 0;
        }

        public List<Models.User> AssignedUsersList(int activityId,int activityType)
        {
            var query = "SELECT * FROM User JOIN Assignment ON User.UserId = Assignment.UserId WHERE Assignment.ActivityId = @activityId AND Assignment.Type = @activityType";
            var toGet = _adapter.GetFromQuery<Models.User>(query, activityId,activityType);
            return toGet;
        }
        #endregion

        #region Assignment Section
        public void AssignActivity(int userId, int activityId, ActivityType type)
        {
            Assignment assign = new Assignment(userId, activityId, type);
            _adapter.Update(assign);
        }

        public void DeassignActivity( int userId, int activityId, ActivityType type)
        {
            Assignment deassign = new Assignment(userId,activityId, ActivityType.PROJECT);
            _adapter.Delete(deassign);
        }
        #endregion

        #region User Credentials Section
        public bool CheckUserCredential(string email, string password)
        {
            string query = "SELECT * FROM UserCredential WHERE Email = @email AND Password = @password;";
            var result = _adapter.GetFromQuery<Models.User>(query, email,password);
            return (result.Count != 0);
               
        }

        public bool CheckIfAdmin(string userId)
        {
            string query = "SELECT * FROM User WHERE Email = @userId AND Role = 2 ";
            var result = _adapter.GetFromQuery<Models.User>(query,userId);
            return (result.Count != 0);
               
        }

        public bool CheckIfManager(string userId)
        {
            string query = "SELECT * FROM User WHERE Email = @userId AND Role = 1 ";
            var result = _adapter.GetFromQuery<Models.User>(query,userId);
            return (result.Count != 0);
              
        }

        public void AddUserCredential(UserCredential credential)
        {
            _adapter.Update(credential);
        }
        #endregion

        #region Issue section
        public int AddIssue(Issue issue)
        {
            _adapter.Add(issue);
            return issue.Id;
        }

        public void DeleteIssue(int issueId)
        {
            string query = "SELECT * FROM Issues WHERE Id = @issueId";
            var toDelete = _adapter.GetFromQuery<Issue>(query, issueId).FirstOrDefault();
            _adapter.Delete(toDelete);
        }

        public Issue GetIssue(int issueId)
        {
            string query = "SELECT * FROM Issues WHERE Id = @issueId";
            var toGet = _adapter.GetFromQuery<Issue>(query, issueId);
            return toGet.FirstOrDefault();
        }

        public List<Issue> IssuesList()
        {
            string query = "SELECT * FROM Issues";
            var list = _adapter.GetFromQuery<Issue>(query);
            return list.ToList();
        }
        #endregion
    }
}
//need to update all queries

