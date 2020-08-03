using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
