using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class IssueBO
    {
        public Issue Issue { get; set; }
        public Dictionary<User, bool> UsersList { get; set; }

        public IssueBO()
        {
            UsersList = new Dictionary<User, bool>();
        }
    }
}
