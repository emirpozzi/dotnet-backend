using Microsoft.AspNetCore.Mvc;

namespace congestion.calculator
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculation : ControllerBase
    {
        [HttpGet()]
        public string Get()
        {
            return "Welcome to Tax Calculation for VOLVO!";
        }

        [HttpGet("vehicle={vehicleType}&dates={datesList}")]
        public string Get(string vehicleType, string datesList)
        {
            var dates = DateParser.GetSortedDatesFromStringList(datesList);

            var calculator = new CongestionTaxCalculator(vehicleType, dates);

            var taxValue = calculator.GetTaxForDay();

            return $"{{Tax: {{value: {taxValue} }}}}";
        }
    }
}
