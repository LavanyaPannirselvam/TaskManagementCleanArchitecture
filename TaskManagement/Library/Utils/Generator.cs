using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementLibrary.Utils
{
    public static class Generator
    {
        public static string GeneratePassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(3,true));
            builder.Append(RandomNumber(100, 999));
            builder.Append(SpecialCharacter());
            return builder.ToString();
        }
        private static string RandomString(int size,bool lower)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lower)
            {
                var str = builder.ToString();
                string mostLower = str.Substring(0, 1) + str.Substring(1).ToLower();
                return mostLower;
            }
            return builder.ToString();
        }
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private static char SpecialCharacter()
        {
            string spl = "%!@#$%&*";
            Random random = new Random();
            return spl[random.Next(0, spl.Length)];
        }


    }
}

