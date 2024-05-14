using Microsoft.Extensions.Configuration;

namespace TaxCalculator
{
    public interface ITaxCalculator
    {
        MonthlyTaxReport Calculate(string name, decimal annualSalary);
    }

    public class TaxCalculator : ITaxCalculator
    {
        private readonly ITaxTableService _taxTableService;

        public TaxCalculator(ITaxTableService taxTableService)
        {
            _taxTableService = taxTableService;
        }

        public MonthlyTaxReport Calculate(string name, decimal annualSalary)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new InvalidDataException("Please enter a name.");
            }

            if (annualSalary < 0)
            {
                throw new InvalidDataException("Please enter an annual salary greater than 0.");
            }

            var monthlyTaxReport = new MonthlyTaxReport { Name = name };

            monthlyTaxReport.GrossMonthlyIncome = CalculateGrossMonthlyIncome(annualSalary);

            monthlyTaxReport.MonthlyIncomeTax = CalculateMonthlyIncomeTax(annualSalary);

            monthlyTaxReport.NetMonthlyIncome = Math.Round(monthlyTaxReport.GrossMonthlyIncome - monthlyTaxReport.MonthlyIncomeTax, 2);

            return monthlyTaxReport;
        }

        private decimal CalculateGrossMonthlyIncome(decimal annualSalary)
        {
            return Math.Round(annualSalary / 12, 2);
        }

        private decimal CalculateMonthlyIncomeTax(decimal annualSalary)
        {
            if (_taxTableService.TaxTable == null || _taxTableService.TaxTable.Slabs == null)
            {
                throw new InvalidDataException("Invalid tax table.");
            }

            var taxTable = _taxTableService.TaxTable;

            var upperSlabIndex = taxTable.Slabs.ToList()
                                         .FindIndex(s => s.Start <= annualSalary && s.End >= annualSalary);

            decimal monthlyTax = 0;
            decimal yearlyTax = 0;

            if (upperSlabIndex >= 0)
            {
                int slabNo = 0;

                while (slabNo <= upperSlabIndex)
                {
                    var slab = taxTable.Slabs[slabNo];

                    var slabTax = (slabNo == upperSlabIndex
                                        ? (annualSalary - slab.Start + 1)
                                        : (slab.End - slab.Start + 1))
                                        * (slab.TaxPerDollar / 100);

                    yearlyTax = yearlyTax + slabTax;

                    slabNo++;
                }

                monthlyTax = yearlyTax / 12;
            }

            return Math.Round(monthlyTax, 2);
        }
    }
}
