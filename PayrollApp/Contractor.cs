using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayrollApp
{
    class Contractor : Employee
    {
        private protected double overtimeRate { get; set; }
        private protected double overtimeHours { get; set; }

        public Contractor()
        {
            this.rate = 30.00;
        }

        public override void CalcTotalPay()
        {
            this.basePay = this.rate * this.hours;
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
    }
}
