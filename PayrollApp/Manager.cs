using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    // child : parent
    class Manager : Employee
    {
        private protected double allowances;

        public Manager()
        {
            this.rate = 50.00;
            this.allowances = 100.00;
        }

        public override double CalcTotalPay()
        {
            this.basePay = this.rate * this.hours;
            this.totalPay = this.basePay + this.allowances;
            return this.totalPay;
        }

        public bool AllowancePurchase(double price)
        {
            if (this.allowances - price < 0.00)
            {
                return false;
            }

            this.allowances -= price;
            return true;
        }

        public static Manager GetManagerFromUser(PayrollUser user)
        {
            if (user.role.ToUpper() != "MANAGER")
            {
                return null;
            }

            Manager manager = new Manager();

            int id = 0;
            int index = manager.GetUsernameIndex();
            string[] managerRows = FileReader.GetData(Manager.payrollFile);
            foreach (string employeeRow in managerRows)
            {
                string[] dataArray = employeeRow.Split(Manager.separator);
                if (dataArray[index] != user.username)
                {
                    ++id;
                    continue;
                }

                // If we get here we have a match
                Type type = manager.GetType();
                for (int i = 0, count = manager.FORMAT.Count; i < count; ++i)
                {
                    string paramString = manager.FORMAT[i];
                    PropertyInfo property = type.GetProperty(paramString);
                    property.SetValue(manager, dataArray[i]);
                }

                manager.id = id;
                return manager;
            }

            return null;
        }
    }
}
