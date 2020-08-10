using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool userAuthenticated = false;
            if (!PayrollUser.AdminUserExist())
            {
                // There are no users currently, we need to create the admin user first


                // We just created this user, we do not need to reauthenticate them
                userAuthenticated = true;
            }

            if (!userAuthenticated)
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine().Trim();
                string passwordInput = GetPasswordInput();
            }

            Console.ReadLine();
        }

        private static string GetPasswordInput()
        {
            List<char> passwordList = new List<char>();
            Console.Write("Enter Password: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (passwordList.Count > 0)
                    {
                        passwordList.RemoveAt(passwordList.Count - 1);
                    }
                }
                else
                {
                    passwordList.Add(keyInfo.KeyChar);
                }

                keyInfo = Console.ReadKey(true);
            }

            return String.Join("", passwordList);
        }
    }
}
