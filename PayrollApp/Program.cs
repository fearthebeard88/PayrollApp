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
            FileReader resourceManager = new FileReader();

            Console.Write("Enter Username: ");
            string username = Console.ReadLine().Trim();
            
            PayrollUser user = resourceManager.GetUserByUsername(username);

            string passwordInput = GetPasswordInput();
            if (user == null)
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
