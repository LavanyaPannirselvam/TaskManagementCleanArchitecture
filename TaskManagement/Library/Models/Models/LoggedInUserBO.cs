using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class LoggedInUserBO
    {
        public User LoggedInUser { get; set; }
        public string WelcomeText { get; set; }
        public LoggedInUserBO()
        { }
    }
}
