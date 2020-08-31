using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Contractor : Employee
    {
        private protected double overtimeRate { get; set; }
        private protected double overtimeHours { get; set; }

        public readonly new List<string> FORMAT = new List<string>()
        {
            "username", "rate", "hours", "name", "basePay"
        };

        public Contractor()
        {
            this.rate = 30.00;
        }

        public override double CalcTotalPay()
        {
            this.basePay = this.rate * this.hours;
            return this.basePay;
        }

        private double CalcOvertimeRate()
        {
            if (this.hours <= 40.00)
            {
                this.overtimeRate = 0.00;
                return this.overtimeRate;
            }

            this.overtimeHours = this.hours - 40.00;
            this.overtimeRate = (this.rate * 1.5) * this.overtimeHours;
            return this.overtimeRate;
        }

        public static Contractor GetContractorFromUser(PayrollUser user)
        {
            if (user.role.ToUpper() != "CONTRACTOR")
            {
                return null;
            }

            Contractor employee = new Contractor();

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
