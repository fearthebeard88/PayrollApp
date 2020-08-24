using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Employee
    {
        protected readonly static string payrollFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}"
            + $"{Path.DirectorySeparatorChar}Payroll_Payroll.txt";

        protected static char separator = ',';

        public readonly List<string> FORMAT = new List<string>()
        {
            "username", "rate", "hours", "name", "basePay", "totalPay"
        };

        private protected string username { get; set; }
        private protected double rate { get; set; }
        private protected double hours { get; set; }
        private protected string name { get; set; }
        private protected double basePay { get; set; }
        private protected double totalPay { get; set; }
        private protected int id { get; set; }

        public Employee()
        {
            this.rate = 20.75;
        }

        public virtual double AddHours(double hours)
        {
            this.hours += hours;
            return this.hours;
        }

        public virtual double CalcTotalPay()
        {
            this.basePay = this.rate * this.hours;
            this.totalPay = this.basePay;
            return this.totalPay;
        }

        public virtual int GetUsernameIndex()
        {
            Debug.Assert(this.FORMAT.Contains("username"));
            return this.FORMAT.IndexOf("username");
        }

        public static Employee GetEmployeeFromUser(PayrollUser user)
        {
            if (user.role.ToUpper() != "Employee")
            {
                return null;
            }

            Employee employee = new Employee();

            int id = 0;
            int index = employee.GetUsernameIndex();
            string[] employeeRows = FileReader.GetData(Employee.payrollFile);
            foreach (string employeeRow in employeeRows)
            {
                string[] dataArray = employeeRow.Split(Employee.separator);
                if (dataArray[index] != user.username)
                {
                    ++id;
                    continue;
                }

                // If we get here we have a match
                Type type = employee.GetType();
                for (int i = 0, count = employee.FORMAT.Count; i < count; ++i)
                {
                    string paramString = employee.FORMAT[i];
                    PropertyInfo property = type.GetProperty(paramString);
                    property.SetValue(employee, dataArray[i]);
                }

                employee.id = id;
                return employee;
            }

            return null;
        }
    }
}
