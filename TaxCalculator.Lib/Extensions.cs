using Microsoft.Extensions.DependencyInjection;

namespace TaxCalculator.Lib
{
    public static class Extensions
    {
        public static IServiceCollection AddTaxCalculator(this IServiceCollection services)
        {
            services.AddSingleton<ITaxTableService, TaxTableService>();
            services.AddScoped<ITaxCalculator, TaxCalculator>();

            return services;
        }        
    }
}
