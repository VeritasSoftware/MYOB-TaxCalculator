// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaxCalculator;

var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true);
var config = builder.Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(config);
services.AddSingleton<ITaxTableService, TaxTableService>();
services.AddScoped<ITaxCalculator, TaxCalculator.TaxCalculator>();

var sp = services.BuildServiceProvider();
var running = true;

while (running)
{
    try
    {       
        Console.WriteLine("Tax Calculator!");
        Console.WriteLine("Please enter your name: ");

        var name = Console.ReadLine();

        Console.WriteLine("Please enter your annual salary: ");

        var annualSalaryStr = Console.ReadLine();

        decimal annualSalary = decimal.Parse(annualSalaryStr);

        var taxCalculator = sp.GetRequiredService<ITaxCalculator>();

        var monthlyTaxReport = taxCalculator.Calculate(name, annualSalary);

        Console.WriteLine(Environment.NewLine);
        monthlyTaxReport.Print();

        Console.WriteLine(Environment.NewLine);
        Console.WriteLine("Another? (Y/N)");
        var yesNo = Console.ReadLine();

        running = yesNo == "Y";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{ex.Message}");
    }

    Console.WriteLine(Environment.NewLine);
}
