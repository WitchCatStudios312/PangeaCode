using Microsoft.AspNetCore.Mvc;
using PangeaCode.Formulas.Creators;
using PangeaCode.Partners;
using PangeaCode;
using PangeaCode.Countries;
using PangeaCode.Models;

namespace PangeaRateAPI.Controllers
{
    [ApiController]
    [Route("api/exchange-rates")]
    public class PangeaRateController : ControllerBase
    {
        private readonly ILogger<PangeaRateController> _logger;

        public PangeaRateController(ILogger<PangeaRateController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string country)
        {
            try
            {
                PangeaExchangeRate[] pangeaRates = await PangeaExchangeRateService.CalculateExchangeRates(country);
                if (pangeaRates == null || pangeaRates.Length == 0)
                {
                    return NotFound();
                }
                _logger.LogInformation(string.Format("Successfully returned rates for Country Code {0}", country));
                return Ok(pangeaRates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }

}
