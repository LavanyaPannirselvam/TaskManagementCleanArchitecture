using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;

namespace TaskManagementLibrary.Models
{
    public class UserCredential
    {
        [PrimaryKey]
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public UserCredential() { }

        public UserCredential(string email, string password, Role role)
        {
            Email = email;
            Password = password;
            Role = role;
        }
    }
}
