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
using System.ComponentModel;

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
            string query = "SELECT Id FROM Project";
            var primaryKeysList = _adapter.GetFromQuery<Project>(query);
            if (primaryKeysList.Count == 0)
                project.Id = 1;
            else project.Id = primaryKeysList.Last().Id + 1;
            _adapter.Add(project);
            return project.Id;
        }

        public void DeleteProject(int projectId)
        {
            string query = "SELECT * FROM Project WHERE Id = @projectId";
            var toDelete = _adapter.GetFromQuery<Project>(query, projectId).FirstOrDefault();
            _adapter.Delete(toDelete);
        }

        public Project GetProject(int projectId)
        {
            string query = "SELECT * FROM Project WHERE Id = @projectId";
            return _adapter.GetFromQuery<Project>(query, projectId).FirstOrDefault();
        }

        public List<Project> ProjectsList(string userName,string userEmail)
        {
            var query = "SELECT * FROM Project WHERE CreatedBy IN (SELECT Name FROM User WHERE Name = @userName AND Email = @userEmail)";
            return _adapter.GetFromQuery<Project>(query,userName,userEmail);
        }

        public void UpdateProject(Project project)
        {
            _adapter.Update(project);
        }

        public List<Tasks> AssignedTasksListOfAProject(int projectId)
        {
            var query = "SELECT * FROM Tasks WHERE ProjectId = @projectId";
            return _adapter.GetFromQuery<Tasks>(query, projectId);
        }

        public List<Issue> AssignedIssuesListOfAProject(int projectId)
        {
            var query = "SELECT * FROM Issues WHERE ProjectId = @projectId";
            return _adapter.GetFromQuery<Issue>(query, projectId);
        }
        #endregion

        #region Task section 
        public int AddTask(Tasks task)
        {
            string query = "SELECT Id FROM Tasks";
            var primaryKeysList = _adapter.GetFromQuery<Tasks>(query);
            if (primaryKeysList.Count == 0)
                task.Id = 1;
            else task.Id = primaryKeysList.Last().Id + 1;
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
            return _adapter.GetFromQuery<Tasks>(query,taskId).FirstOrDefault();
        }

        public List<Tasks> TasksList()
        {
            string query = "SELECT * FROM Tasks";
            return _adapter.GetFromQuery<Tasks>(query);
        }

        public List<Tasks> AssignedTasksListOfCurrentUser(int userId)
        {
            string query = "SELECT * FROM Tasks WHERE Id IN (SELECT ActivityId FROM Assignment WHERE UserId = @userId AND Type = 1)";
            return _adapter.GetFromQuery<Tasks>(query,userId);
        }
       
        public List<Tasks> CreatedTasksListOfCurrentUser(string userName , string userEmail)
        {
            string query = "SELECT * FROM Tasks WHERE CreatedBy IN (SELECT Name FROM User WHERE Name = @userName AND Email = @userEmail)";
            return _adapter.GetFromQuery<Tasks>(query,userName,userEmail);
        }

        public List<Assignment> AssignedUsersListOfATask(int taskId)
        {
            var query = "SELECT * FROM Assignment WHERE ActivityId = @taskId and Type = 1";
            return _adapter.GetFromQuery<Assignment>(query, taskId);
        }
        #endregion

        #region User section
        public int AddUser(Models.User user)
        {
            string query = "SELECT UserId FROM User";
            var primaryKeysList = _adapter.GetFromQuery<Models.User>(query);
            if (primaryKeysList.Count == 0)
                user.UserId = 1;
            else user.UserId = primaryKeysList.Last().UserId + 1;
            _adapter.Add(user);
            return user.UserId;
        }

        public void DeleteUser(string email)
        {
            string query = "SELECT * FROM User WHERE Email = @email";
            var toDelete = _adapter.GetFromQuery<Models.User>(query, email).FirstOrDefault();
            _adapter.Delete(toDelete);
        }

        public Models.User GetUser(int userId)
        {
            string query = "SELECT * FROM User WHERE UserId = @userId";
            return _adapter.GetFromQuery<Models.User>(query,userId).FirstOrDefault();
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
            return _adapter.GetFromQuery<Models.User>(query); 
        }

        public bool CheckUser(string email)
        {
            var query = "SELECT * FROM USER WHERE EMAIL = @email";
            var result = _adapter.GetFromQuery<Models.User>(query,email);
            return result.Count == 0;
        }

        public List<Models.User> AssignedUsersList(int activityId,int activityType)
        {
            var query = "SELECT * FROM User JOIN Assignment ON User.UserId = Assignment.UserId WHERE Assignment.ActivityId = @activityId AND Assignment.Type = @activityType";
            return _adapter.GetFromQuery<Models.User>(query, activityId,activityType);
        }

        public List<Assignment> AssignmentsList(int userId)
        {
            var query = "SELECT * FROM Assignment WHERE UserId = @userId";
            return _adapter.GetFromQuery<Assignment>(query, userId);
        }

        public List<Models.User> MatchingUsers(string input)
        {
            var query = "SELECT Name , UserId FROM User WHERE Name LIKE @input";
            return _adapter.GetFromQuery<Models.User>(query,("%" + input + "%"));
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
            Assignment deassign = new Assignment(userId,activityId, type);
            _adapter.Delete(deassign);
        }

        public void RemoveAllAssignments(List<Assignment> assignments)
        {
            foreach(var item in assignments)
            {
                _adapter.Delete(item);
            }
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

        public void DeleteUserCredentials(string email)
        {
            var query = "SELECT * FROM UserCredential WHERE Email = @email";
            var toGet = _adapter.GetFromQuery<UserCredential>(query, email).FirstOrDefault();
            _adapter.Delete(toGet);
        }
        #endregion

        #region Issue section
        public int AddIssue(Issue issue)
        {
            string query = "SELECT Id FROM Issues";
            var primaryKeysList = _adapter.GetFromQuery<Issue>(query);
            if (primaryKeysList.Count == 0)
                issue.Id = 1;
            else issue.Id = primaryKeysList.Last().Id + 1;
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
            return _adapter.GetFromQuery<Issue>(query, issueId).FirstOrDefault();
        }

        public List<Issue> IssuesList()
        {
            string query = "SELECT * FROM Issues";
            return _adapter.GetFromQuery<Issue>(query);
        }

        public List<Assignment> AssignedUsersListOfAIssue(int issueId)
        {
            var query = "SELECT * FROM Assignment WHERE ActivityId = @issueId and Type = 2";
            return _adapter.GetFromQuery<Assignment>(query, issueId);
        }

        
        #endregion
    }
}

