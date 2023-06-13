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
        public void AddProject(Project project)
        {
            _adapter.Add(project);
        }

        public void DeleteProject(int projectId)
        {
            string query = "SELECT * FROM PROJECT WHERE Id = @projectId";
            var toDelete = _adapter.GetFromQuery<Project>(query, projectId);
            _adapter.Delete(toDelete);
        }

        public Project GetProject(int projectId)
        {
            return _adapter.GetList<Project>().Where(project => project.Id == projectId).FirstOrDefault();
        }

        public List<Project> ProjectsList()
        {
             return _adapter.GetList<Project>().ToList();
        }

        public void UpdateProject(Project project)
        {
            _adapter.Update(project);
        }
        #endregion

        #region Task section 
        public void AddTask(Tasks task)
        {
            _adapter.Add(task);
        }

        public void DeleteTask(int taskId)
        {
            string query = "SELECT * FROM TASKS WHERE Id = @taskId";
            var toDelete = _adapter.GetFromQuery<Tasks>(query, taskId);
            _adapter.Delete(toDelete);
        }

        public Tasks GetTask(int taskId)
        {
            return _adapter.GetList<Tasks>().Where(task => task.Id == taskId).FirstOrDefault();
        }

        public List<Tasks> TasksList()
        {
            return _adapter.GetList<Tasks>().ToList();
        }
        #endregion
        
        #region User section
        public void AddUser(Models.User user)
        {
            _adapter.Add(user);
        }

        public void DeleteUser(string email)
        {
            string query = "SELECT * FROM USER WHERE Email = @email";
            var toDelete = _adapter.GetFromQuery<Models.User>(query, email);
            _adapter.Delete(toDelete);
        }

        public Models.User GetUser(int userId)
        {
            return _adapter.GetList<Models.User>().Where(c => c.UserId == userId).FirstOrDefault();
        }

        public Models.User GetUser(string email)
        {
            return _adapter.GetList<Models.User>().Where(c => c.Email == email).FirstOrDefault();
        }

        public List<Models.User> UsersList()
        {
            return _adapter.GetList<Models.User>().ToList();
        }

        public bool CheckUser(string email)
        {
            var user = _adapter.GetList<Models.User>().Where(i => i.Email == email);
            return (user.Count() > 0);
        }

        public List<Models.User> GetAssignedUsers(int projectId)
        {
            var query = "SELECT USER.Name FROM ASSIGNMENT JOIN USER ON ASSIGNMENT.userId = USER.Id WHERE ASSIGNMENT.assignmentId = @projectId AND ASSIGNMENT.assignmentType = PROJECT";
            var toGet = _adapter.GetFromQuery<Models.User>(query, projectId);
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
        public bool CheckUserCredential(string userId, string password)
        {
            var result = _adapter.GetList<UserCredential>().Where(c => c.Email == userId && c.Password == password).FirstOrDefault();
            return (result != null);
        }

        public bool CheckIfAdmin(string userId)
        {
            var result = _adapter.GetList<UserCredential>().Where(c=> c.Email == userId && c.Role == Role.EMPLOYEE).FirstOrDefault();
            return (result != null);
        }

        public bool CheckIfManager(string userId)
        {
            var result = _adapter.GetList<UserCredential>().Where(c => c.Email == userId && c.Role == Role.MANAGER).FirstOrDefault();
            return (result != null);
        }

        public void AddUserCredential(UserCredential credential)
        {
            _adapter.Update(credential);
        }

        
        #endregion
    }
}

