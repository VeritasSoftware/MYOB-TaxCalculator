using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Lib;

namespace TaxCalculator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly ITaxCalculator _taxCalculator;

        public TaxCalculatorController(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        [HttpGet("{name}/{annualSalary}")]
        public async Task<IActionResult> Get(string name, decimal annualSalary)
        {
            var result = await _taxCalculator.CalculateAsync(name, annualSalary);

            return Ok(result);
        }
    }
}
