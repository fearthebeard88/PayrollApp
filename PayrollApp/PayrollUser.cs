using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class PayrollUser
    {
        private const int SALTSIZE = 20;
        private const int HASHSIZE = 20;
        public readonly static List<string> FORMAT = new List<string>()
        {
            "username", "firstname", "lastname", "role", "passwordHash", "salt"
        };

        private protected string username { get; set; }
        private protected string firstname { get; set; }
        private protected string lastname { get; set; }
        private protected string role { get; set; }

        /// <summary>
        /// base64 encoded string
        /// </summary>
        private protected string passwordHash { get; set; }

        /// <summary>
        /// base64 encoded string
        /// </summary>
        private protected string salt { get; set; }

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
            byte[] salt = Encoding.UTF8.GetBytes(saltString);
            Rfc2898DeriveBytes hasher = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = hasher.GetBytes(HASHSIZE);
            byte[] passwordHash = new byte[SALTSIZE + HASHSIZE];
            Array.Copy(salt, passwordHash, salt.Length);
            Array.Copy(hash, passwordHash, hash.Length);
            return passwordHash;
        }
    }
}
