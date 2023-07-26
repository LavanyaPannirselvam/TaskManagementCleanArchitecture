using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementLibrary.Enums;
using SQLite;

namespace TaskManagementLibrary.Models
{
    public class User
    {
        //private readonly int id=1;
        public int UserId { get; set; }
        public string Name { get; set; }
        [PrimaryKey]
        public string Email { get; set; }
        public Role Role { get; set; }
        public User(string name, string email, Role role)
        {
            Name = name;
            Email = email;
            Role = role;
        }
        public User() { }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            User obj = other as User;
            return Email == obj.Email;
        }
    }
}
