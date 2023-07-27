using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Models
{
    public class UserBO
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public UserBO() { }
        public UserBO(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            UserBO obj = other as UserBO;
            return Email == obj.Email;
        }
    }
}
