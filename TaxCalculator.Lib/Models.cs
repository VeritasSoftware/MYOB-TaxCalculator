namespace TaxCalculator.Lib
{
    public class TaxTable
    {
        public TaxSlab[]? Slabs { get; set; }
    }

    public class TaxSlab
    {
        public int Start { get; set; }
        public int End { get; set; }
        public decimal TaxPerDollar { get; set; }
    }

    public class MonthlyTaxReport
    {
        public string Name { get; set; }
        public decimal GrossMonthlyIncome { get; set; }
        public decimal MonthlyIncomeTax { get; set; }
        public decimal NetMonthlyIncome { get; set; }

        public void Print()
        {
            Console.WriteLine($@"Monthly Payslip for: ""{this.Name}""");
            Console.WriteLine($"Gross Monthly Income: ${this.GrossMonthlyIncome.ToString("#.##")}");
            Console.WriteLine($"Monthly Income Tax: ${this.MonthlyIncomeTax.ToString("#.##")}");
            Console.WriteLine($"Net Monthly Income: ${this.NetMonthlyIncome.ToString("#.##")}");
        }
    }
}
