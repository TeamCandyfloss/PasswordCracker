using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordCrackerClient.model
{
    [Serializable]
    public class UserInfo
    {
        public string Username { get; set; }
        public string EncryptedPasswordBase64 { get; set; }
        public byte[] EncryptedPassword { get; set; }

        public UserInfo(string username, string encryptedPassword)
        {
            if(username == null)
            {
                throw new ArgumentNullException("Username");
            }
            if(encryptedPassword == null)
            {
                throw new ArgumentNullException("encryptedPassword");
            }
            Username = username;
            EncryptedPasswordBase64 = encryptedPassword;
            EncryptedPassword = Convert.FromBase64String(encryptedPassword);
        }

        public override string ToString()
        {
            return Username + ":" + EncryptedPassword;
        }
    }
}
