using Microsoft.Extensions.Configuration;

namespace TaxCalculator
{
    public interface ITaxTableService
    {
        TaxTable TaxTable { get; }
    }

    public class TaxTableService : ITaxTableService
    {
        public virtual TaxTable TaxTable { get; private set; } = new TaxTable();

        public TaxTableService(IConfiguration configuration)
        {
            this.TaxTable.Slabs = configuration.GetSection("TaxTable")
                                               .Get<TaxSlab[]>();
        }
    }
}
