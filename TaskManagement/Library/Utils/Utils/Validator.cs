using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Utils
{
    public static class Validator
    {
        public static bool ValidateEmail(string email)
        {
           string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|in)$";
           return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }
    }
}
