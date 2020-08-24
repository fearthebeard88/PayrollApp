using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                // There are command line arguments, we use these to create a user
                // Ensure there is enough information to create a user
                if (args.Count() != 5)
                {
                    Console.WriteLine("Not enough arguments provided.");
                    return;
                }

                string cmd_username = args[0];
                string cmd_firstname = args[1];
                string cmd_lastname = args[2];
                string cmd_role = args[3];
                string cmd_password = args[4];

                PayrollUser cmd_user = PayrollUser.CreateUser(cmd_username, cmd_firstname, cmd_lastname, cmd_role, cmd_password);
                if (cmd_user == null)
                {
                    Console.WriteLine("CreateUser unexpectedly returned null after attempting to create a new user");
                    return;
                }

                Console.WriteLine($"Successfully created new user {cmd_user.username}!");
                Console.ReadLine();
                return;
            }

            if (PayrollUser.GetTotalUsers() <= 0)
            {
                Console.WriteLine("There are no valid users, please provide arguments to create a new user");
                return;
            }

            // If we are here there are no command line arguments and there are valid users to load

            Console.Write("Username: ");
            string username = Console.ReadLine().Trim();
            string password = GetPasswordInput();

            PayrollUser user = PayrollUser.GetUserByUsername(username);
            if (user == null)
            {
                Console.WriteLine("Invalid credentials");
                return;
            }

            if (!user.ValidatePassword(password))
            {
                Console.WriteLine("Invalid credentials");
                return;
            }

            Console.Clear();
            Console.WriteLine($"Welcome {user.firstname}!\n");

            bool runAgain = false;
            do
            {
                string currentRole = user.role.ToUpper();
                switch (currentRole)
                {
                    case "EMPLOYEE":
                        Employee employee = PayrollUser.GetEmployeeFromUser(user);
                        break;
                    case "CONTRACTOR":
                        Contractor contractor = PayrollUser.GetEmployeeFromUser(user);
                        break;
                    case "MANAGER":
                        Manager manager = PayrollUser.GetEmployeeFromUser(user);
                        break;
                    default:
                        break;
                }

                Console.WriteLine("Run again? (y/n)");
                runAgain = Console.ReadLine().Trim() == "y";
            }
            while (runAgain);

            Console.ReadLine();
        }

        private static string GetPasswordInput()
        {
            List<char> passwordList = new List<char>();
            Console.Write("Password: ");
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
