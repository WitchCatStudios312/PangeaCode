//using Microsoft.AspNetCore.Mvc;
//using PangeaCode.Formulas.Creators;
//using PangeaCode.Partners;
//using PangeaCode;
//using PangeaCode.Countries;

//namespace PangeaRateAPI.Controllers
//{
//    [ApiController]
//    [Route("api/exchange-rates")]
//    //[Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private readonly ILogger<WeatherForecastController> _logger;

//        public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;
//        }


//        /// <summary>
//        /// Dictionary where you can configure the type of formula to use for each country
//        /// </summary>
//        private static Dictionary<string, Configuration> countryConfigs = new Dictionary<string, Configuration>()
//        {
//            {"MEX", new Configuration(new Mexico(), FormulaEnum.FlatRate, new FlatRateCalucationEngine()) },
//            {"GTM", new Configuration(new Guatemala(), FormulaEnum.FormulaB, new FormulaBCalucationEngine()) },
//            {"IND", new Configuration(new India(), FormulaEnum.FormulaB, new FormulaBCalucationEngine()) },
//            {"PHL", new Configuration(new Philippines(), FormulaEnum.FlatRate, new FlatRateCalucationEngine()) },
//        };

//        private static List<IPartner> PartnerList = new List<IPartner>
//        {
//            new PartnerXYZ(),
//            new PartnerXYZ(),
//        };

//        [HttpGet]
//        public async Task<IActionResult> Get(string country)
//        {
//            if (!countryConfigs.ContainsKey(country))
//            {
//                return NotFound();
//            }

//            Configuration config = countryConfigs[country];
//            RateCalucationEngine rcEngine = config.Engine;


//            //We could makes all the calls to the various partner API's here like this
//            List<Task<List<PartnerRate>>> tasks = new List<Task<List<PartnerRate>>>();
//            //Add partner API's here (TODO: Implement feature flags to easily turn partners on/off)
//            foreach (IPartner partner in PartnerList)
//            {
//                tasks.Add(partner.GetPartnerRatesAsync(config.Country.CountryCode));
//            }

//            var results = await Task.WhenAll(tasks);

//            List<PartnerRate> allPartnerRates = new List<PartnerRate>();
//            for (int i = 0; i < results.Length; i++)
//            {
//                foreach (var list in results[i])
//                {
//                    allPartnerRates.Add(list);
//                }
//            }

//            PangeaExchangeRate[] pangeaRates = rcEngine.CalculatePangeaRates(config.Country, allPartnerRates.ToArray());

//            if (pangeaRates == null || pangeaRates.Length == 0)
//            {
//                return NotFound();
//            }
//            return Ok(pangeaRates);
//        }
//    }
    
//}
