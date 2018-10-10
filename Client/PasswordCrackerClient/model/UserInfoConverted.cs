using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerClient.model
{
    [Serializable]
    public class UserInfoConverted
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserInfoConverted(string username, string password)
        {
            if(username == null)
            {
                throw new ArgumentNullException("username");
            }
            if(password == null)
            {
                throw new ArgumentNullException("password");
            }
            Username = username;
            Password = password;
        }

        public override string ToString()
        {
            return Username + ":" + Password;
        }
    }
}
