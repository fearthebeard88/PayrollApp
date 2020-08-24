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
using System.Runtime.CompilerServices;

namespace PayrollApp
{
    class PayrollUser
    {
        private readonly static string userFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}"
            + $"{Path.DirectorySeparatorChar}Payroll_Users.txt";

        private static char separator = ',';

        private const int SALTSIZE = 20;
        private const int HASHSIZE = 20;
        private const int ROUNDS = 100000;

        public readonly static List<string> ROLES = new List<string>()
        {
            "EMPLOYEE", "CONTRACTOR", "MANAGER"
        };

        private readonly List<string> FORMAT = new List<string>()
        {
            "firstname", "lastname", "username", "role", "passwordHash"
        };

        public int id = -1;

        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string role { get; set; }

        /// <summary>
        /// base64 encoded string
        /// </summary>
        public string passwordHash { get; set; }

        public byte[] GetPasswordBytes(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALTSIZE]);
            //this.salt = Convert.ToBase64String(salt);

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, ROUNDS);
            byte[] hash = pbkdf2.GetBytes(HASHSIZE);

            byte[] passwordHash = new byte[HASHSIZE + SALTSIZE];
            salt.CopyTo(passwordHash, 0);
            hash.CopyTo(passwordHash, salt.Length);
            //this.passwordHash = Convert.ToBase64String(passwordHash);

            return passwordHash;
        }

        public bool ValidatePassword(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
            {
                // We do not accept empty passwords
                return false;
            }

            byte[] passwordHash = Convert.FromBase64String(this.passwordHash);
            byte[] salt = passwordHash.Take(SALTSIZE).ToArray();

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, ROUNDS);
            byte[] inputHash = pbkdf2.GetBytes(HASHSIZE);
            for (int i = 0; i < HASHSIZE; ++i)
            {
                if (passwordHash[i + SALTSIZE] != inputHash[i])
                {
                    // The salt is stored with the password which is why we need to skip
                    // those bytes in the comparison
                    return false;
                }
            }

            return true;
        }

        public string CreatePassword(string password)
        {
            byte[] passwordHashBytes = this.GetPasswordBytes(password);
            string passwordHash = Convert.ToBase64String(passwordHashBytes);
            this.passwordHash = passwordHash;
            return passwordHash;
        }

        public static PayrollUser GetUserByUsername(string username)
        {
            PayrollUser user = new PayrollUser();
            string[] userCollection = FileReader.GetData(PayrollUser.userFile);
            if (userCollection == null)
            {
                return null;
            }

            int id = 0;
            int index = user.GetUsernameIndex();
            foreach (string userRow in userCollection)
            {
                List<string> userProps = new List<string>(userRow.Split(PayrollUser.separator).ToList<string>());
                if (userProps[index] != username)
                {
                    ++id;
                    continue;
                }

                // We matched on the username, now we set properties on our object
                Type type = user.GetType();
                for (int i = 0, count = userProps.Count; i < count; ++i)
                {
                    string paramString = user.FORMAT[i];
                    PropertyInfo param = type.GetProperty(paramString);
                    param.SetValue(user, userProps[i]);
                }

                user.id = id;
                return user;
            }

            return null;
        }

        private int GetUsernameIndex()
        {
            Debug.Assert(this.FORMAT.Contains("username"));
            return this.FORMAT.IndexOf("username");
        }

        public static int GetTotalUsers()
        {
            return FileReader.GetSize(PayrollUser.userFile);
        }

        public static PayrollUser CreateUser(string username, string firstname, string lastname, string role, string password)
        {
            PayrollUser existingUser = PayrollUser.GetUserByUsername(username);
            if (existingUser != null)
            {
                // A user already exists with this username, these must be unique
                return null;
            }

            PayrollUser newUser = new PayrollUser();
            newUser.username = username;
            newUser.firstname = firstname;
            newUser.lastname = lastname;
            newUser.role = role;
            if (!PayrollUser.ROLES.Contains(role.ToUpper()))
            {
                // We were passed an invalid role
                return null;
            }

            newUser.CreatePassword(password);

            newUser.Save();

            return newUser;
        }

        public void Save()
        {
            List<string> dataList = new List<string>();
            Type type = this.GetType();
            foreach (string property in this.FORMAT)
            {
                PropertyInfo prop = type.GetProperty(property);
                string value = prop.GetValue(this).ToString();

                dataList.Add(value);
            }

            string dataString = String.Join(PayrollUser.separator.ToString(), dataList.ToArray());
            FileReader.AddData(dataString, PayrollUser.userFile);
        }

        public void Update()
        {
            Debug.Assert(this.id >= 0);
            List<string> dataList = new List<string>();
            Type type = this.GetType();
            foreach (string property in this.FORMAT)
            {
                PropertyInfo prop = type.GetProperty(property);
                string value = prop.GetValue(this).ToString();

                dataList.Add(value);
            }

            string dataString = String.Join(PayrollUser.separator.ToString(), dataList.ToArray());
            FileReader.UpdateData(dataString, PayrollUser.userFile, this.id);
        }

        public void Delete()
        {
            Debug.Assert(this.id >= 0);
            FileReader.RemoveData(PayrollUser.userFile, this.id);
        }
    }
}