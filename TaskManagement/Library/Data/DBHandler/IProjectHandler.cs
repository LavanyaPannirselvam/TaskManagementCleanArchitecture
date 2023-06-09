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
        void AddProject(Project project);
        void DeleteProject(int projectId);
        Project GetProject(int projectId);
        void UpdateProject(Project project);
        List<Project> ProjectsList();
    }
}
