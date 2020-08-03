using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class FileReader
    {
        private string userFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + Path.DirectorySeparatorChar + "Payroll_Users.txt";
        private string payrollFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            + Path.DirectorySeparatorChar + "Payroll_Payroll.txt";

        private FileStream userStream;
        private FileStream payrollStream;

        private char separator = ',';

        public FileReader()
        {
            this.userStream = File.OpenWrite(this.userFile);
            this.payrollStream = File.OpenWrite(this.payrollFile);
        }

        ~FileReader()
        {
            this.userStream.Close();
            this.payrollStream.Close();
        }

        public PayrollUser GetUserByUsername(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            StreamReader reader = new StreamReader(this.userStream);
            List<string> format = PayrollUser.FORMAT;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                List<string> data = line.Split(this.separator).ToList<string>();
                if (!data.Contains(username))
                {
                    continue;
                }

                PayrollUser user = new PayrollUser();
                Type type = user.GetType();
                for (int i = 0, count = data.Count; i < count; ++i)
                {
                    MethodInfo methodInfo = type.GetMethod(format[i]);
                    Debug.Assert(methodInfo != null);

                    type.GetProperty(format[i]).SetValue(user, data[i]);
                }

                return user;
            }

            return null;
        }
    }
}
