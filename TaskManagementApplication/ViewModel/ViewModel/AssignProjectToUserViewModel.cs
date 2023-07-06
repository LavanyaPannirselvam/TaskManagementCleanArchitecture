using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementCleanArchitecture.ViewModel
{
    public class AssignProjectToUserViewModel
    {
    }

    public abstract class AssignProjectToUserViewModelBase : NotifyPropertyBase
    {
        public ObservableCollection<User> UsersList = new ObservableCollection<User>();
        public IAssignUser assignUser { get; set; }
        public abstract void AssignUserToProject(int userId);
    }

    public interface IAssignUser
    {
        void AssignUserToProject(int userId);
    }
}
