using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Models;

namespace TaskManagementLibrary.Data.DBHandler
{
    public interface IIssueHandler
    {
        int AddIssue(Issue issue);
        void DeleteIssue(int issueId);
        Issue GetIssue(int issueId);
        List<Issue> IssuesList();
        List<Assignment> AssignedUsersListOfAIssue(int issueId);
    }
}
