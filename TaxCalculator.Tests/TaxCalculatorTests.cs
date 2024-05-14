using Moq;

namespace TaxCalculator.Tests
{
    public class TaxCalculatorTests
    {
        public class TaxCalculatorTestData : TheoryData<string, decimal, MonthlyTaxReport>
        {
            public TaxCalculatorTestData()
            {
                Add("Mary Song", 60000, new MonthlyTaxReport { Name = "Mary Song", GrossMonthlyIncome = 5000, MonthlyIncomeTax = 500, NetMonthlyIncome = 4500 });
                Add("John Smith", 180000, new MonthlyTaxReport { Name = "John Smith", GrossMonthlyIncome = 15000, MonthlyIncomeTax = 3333.33M, NetMonthlyIncome = 11666.67M });
                Add("Jane Smith", 20000, new MonthlyTaxReport { Name = "Jane Smith", GrossMonthlyIncome = 1666.67M, MonthlyIncomeTax = 0, NetMonthlyIncome = 1666.67M });
            }
        }

        [Theory]
        [ClassData(typeof(TaxCalculatorTestData))]
        public void TaxCalulator_Pass(string name, decimal annualSalary, MonthlyTaxReport expectedTaxReport)
        {
            //Arrange
            var taxTable = new TaxTable
            {
                Slabs = new TaxSlab[]
                {
                    new TaxSlab { Start = 0, End = 20000, TaxPerDollar = 0 },
                    new TaxSlab { Start = 20001, End = 40000, TaxPerDollar = 10 },
                    new TaxSlab { Start = 40001, End = 80000, TaxPerDollar = 20 },
                    new TaxSlab { Start = 80001, End = 180000, TaxPerDollar = 30 },
                    new TaxSlab { Start = 180001, End = int.MaxValue, TaxPerDollar = 40 }
                }
            };            

            var mockTaxTableService = new Mock<ITaxTableService>();

            mockTaxTableService.Setup(x => x.TaxTable)
                               .Returns(taxTable);

            var taxCalculator = new TaxCalculator(mockTaxTableService.Object);

            //Act
            var taxReport = taxCalculator.Calculate(name, annualSalary);

            //Assert
            Assert.Equal(expectedTaxReport.GrossMonthlyIncome, taxReport.GrossMonthlyIncome);
            Assert.Equal(expectedTaxReport.MonthlyIncomeTax, taxReport.MonthlyIncomeTax);
            Assert.Equal(expectedTaxReport.NetMonthlyIncome, taxReport.NetMonthlyIncome);
        }
    }
}