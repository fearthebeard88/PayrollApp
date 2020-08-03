using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Employee
    {
        public readonly static List<string> FORMAT = new List<string>()
        {
            "rate", "hours", "name", "basePay", "totalPay"
        };

        private protected double rate { get; set; }
        private protected double hours { get; set; }
        private protected string name { get; set; }
        private protected double basePay { get; set; }
        private protected double totalPay { get; set; }

        public Employee()
        {
            this.rate = 20.75;
        }

        public virtual double CalcTotalPay()
        {
            this.basePay = this.rate * this.hours;
            this.totalPay = this.basePay;
            return this.totalPay;
        }
    }
}
