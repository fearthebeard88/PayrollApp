using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace PayrollApp
{
    class PayrollUser
    {
        private static string userFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + Path.DirectorySeparatorChar + "Payroll_Users.txt";
        private static string payrollFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + Path.DirectorySeparatorChar + "Payroll_Payroll.txt";
        private static char separator = ',';

        private const int SALTSIZE = 20;
        private const int HASHSIZE = 20;
        public readonly static List<string> FORMAT = new List<string>()
        {
            "username", "firstname", "lastname", "role", "passwordHash", "salt"
        };

        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string role { get; set; }

        /// <summary>
        /// base64 encoded string
        /// </summary>
        public string passwordHash { get; set; }

        /// <summary>
        /// base64 encoded string
        /// </summary>
        public string salt { get; set; }

        public string CreatePassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALTSIZE]);
            this.salt = Convert.ToBase64String(salt);
            byte[] passwordHash = this.GeneratePasswordHash(password, this.salt);
            string stringifiedPasswordHash = Convert.ToBase64String(passwordHash);
            this.passwordHash = stringifiedPasswordHash;
            return this.passwordHash;
        }

        private byte[] GeneratePasswordHash(string password, string saltString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = hasher.GetBytes(HASHSIZE);
            byte[] passwordHash = new byte[SALTSIZE + HASHSIZE];
            Array.Copy(salt, passwordHash, salt.Length);
            Array.Copy(hash, passwordHash, hash.Length);
            return passwordHash;
        }

        public static List<PayrollUser> GetUserByUsername(string username)
        {
            string[] userCollection = FileReader.GetData(PayrollUser.userFile);
            if (userCollection == null)
            {
                return null;
            }

            // Because we can have multiple users with the same username, we have return all the matches
            // instead of returning a single concrete user
            List<PayrollUser> userList = new List<PayrollUser>();
            foreach (string userRow in userCollection)
            {
                List<string> userProps = new List<string>(userRow.Split(PayrollUser.separator).ToList<string>());
                if (userProps[PayrollUser.FORMAT.IndexOf("username")] != username)
                {
                    continue;
                }

                // We have a username match, now we need to instantiate a user and set the values
                PayrollUser user = new PayrollUser();
                for (int i = 0, count = PayrollUser.FORMAT.Count; i < count; ++i)
                {
                    string propertyName = PayrollUser.FORMAT[i];
                    PropertyInfo prop = user.GetType().GetProperty(propertyName);
                    prop.SetValue(user, userProps[i]);
                }

                userList.Add(user);
            }

            if (userList.Count <= 0)
            {
                return null;
            }

            return userList;
        }

        public static bool AdminUserExist()
        {
            return FileReader.GetSize(PayrollUser.userFile) > 0;
        }

        public static PayrollUser CreateAdminUser(string username, string firstname, string lastname, string password)
        {
            string userInfo = "";
            ParameterInfo[] userParams = MethodInfo.GetCurrentMethod().GetParameters();
            string[] userValues = new string[PayrollUser.FORMAT.Count];
            for (int i = 0, count = PayrollUser.FORMAT.Count; i < count; ++i)
            {
                string prop = PayrollUser.FORMAT[i];
                ParameterInfo userParam = userParams[Array.IndexOf(userParams, prop)];
                
            }
        }
    }
}

