using PangeaCode.Countries;
using PangeaCode.Formulas.Creators;
using PangeaCode.Models;
using PangeaCode.Partners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangeaCode
{
    /// <summary>
    /// This is the class that actually does the work
    /// Consolidate the logic into one call
    /// </summary>
    public static class PangeaExchangeRateService
    {

        #region Configuration Properties

        /// <summary>
        /// Dictionary where you can configure the type of formula to use for each country
        /// </summary>
        private static Dictionary<string, Configuration> countryConfigs = new Dictionary<string, Configuration>()
        {
            {"MEX", new Configuration(new Mexico(), new FlatRateCalucationEngine()) },
            {"GTM", new Configuration(new Guatemala(), new FlatRateCalucationEngine()) },
            {"IND", new Configuration(new India(), new FlatRateCalucationEngine()) },
            {"PHL", new Configuration(new Philippines(), new FlatRateCalucationEngine()) },
        };


        /// <summary>
        /// List of all available Partners
        /// </summary>
        private static List<IPartner> PartnerList = new List<IPartner>
        {
            new PartnerXYZ(),
        };

        #endregion

       
        /// <summary>
        /// The entrance point to the exchange rate service. 
        /// Takes in a CountryCode as a parameter and returns the Pangea Rates
        /// across all Partners for the Country-Formula configuration.
        /// This code is called in the API Controller Method,
        /// as well as inside a Console App which I used for quick testing duing development
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static async Task<PangeaExchangeRate[]> CalculateExchangeRates(string country)
        {
            if (!countryConfigs.ContainsKey(country.ToUpper()))
            {
                throw new KeyNotFoundException(string.Format("Country Code {0} is not supported", country.ToUpper()));
            }

            Configuration config = countryConfigs[country.ToUpper()];
            RateCalucationEngine rcEngine = config.Engine;

            List<Task<List<PartnerRate>>> tasks = new List<Task<List<PartnerRate>>>();
            foreach (IPartner partner in PartnerList)
            {
                tasks.Add(partner.GetPartnerRatesAsync(config.Country.CountryCode));
            }

            var results = await Task.WhenAll(tasks);

            List<PartnerRate> allPartnerRates = new List<PartnerRate>();
            for (int i = 0; i < results.Length; i++)
            {
                foreach (var list in results[i])
                {
                    allPartnerRates.Add(list);
                }
            }

            PangeaExchangeRate[] pangeaRates = rcEngine.CalculatePangeaRates(config.Country, allPartnerRates.ToArray());
            return pangeaRates;
        }
    }
}
