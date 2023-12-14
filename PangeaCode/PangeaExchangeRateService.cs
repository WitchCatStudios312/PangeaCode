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

        #region Notes

        /*
         * Partner class encapsulate all logic to call the Partner API, validate the data, and return a model that will be returned
         * by all partners. Each conrete class can have data transformation methods if required to map to the PartnerRate object contract.
         * The idea being the main business logic will call methods on the interface, so no adjustment to the main logic will be needed when a new
         * Partner is added. It just requires a new concrete class and creator (and unit tests)
         * 
         * Each Partner will also have its own individual httpClient to handle all calls to their API.
         * I think this is cleaner, and each Partner API will have its own authentication and config requirements, and url.
         * Probably a static or singleton HttpClient instance with PooledConnectionLifetime set to the desired interval,
         * such as 2 minutes, depending on any expected DNS changes.
         * This solves both port exhaustion and DNS change issues.
         * Or we could use the httpClientFactory, but I am not sure yet if that overhead is necessary.
         * I would like to get more information about the expected shape of the various future partner APIs,
         * and also if we are only going to call API methods to get rates,
         * or if we anticipate making additional calls to the Partners' API for other reasons in the future
         * 
         * Formula class is also a factory, and inside this class is encapsulated the logic to take in the PartnerRate array,
         * run the specific calculation method, and transform the resulting data into a PangeaExchangeRate array.
         * 
         * TODO - unit tests
         * 
         * TODO - Ideally this below configuration information will be stored in a datbase with xref tables
         * for the configurations. The information would be returned to the factory methods
         * which would then create the objects based on the data returned.
         * I would like to allow greater flexibility for custom configurations by Partner/Country/Formula.
         * A many to many relationship between Partner and Country, and a 1-1 relationship between any combination
         * of Partner/Country - Formula, Partner - Formula, or Country - Formula, depending on the business requirements.
         * I would also like to add a flag in the xref table to allow the ability to turn Partners or whatever other config on/off.
         * In a production app I would want that kill switch functionality to be able to turn specific pieces on/off easily in case of fires.
         * I would like to ask more detailed questions about the buiness requirements to make a better judgement on the complexity
         * of the design required
         * 
         * Keeping those configurations in the db would allow for real time changes to configs without having to recompile the software
         * or publish updates. We would only have to deploy release if we edit code or added new classes.
         * I assume that those configurations would not be changed that often, so we could call the db at startup and populate a
         * dictionary which the application would reference. We could set a time limit to refresh the dictionary
         * after a certain amount of time to keep it fresh.
         * 
         * With a db we really wouldn't need a separate class per country, with the current data requirements for the Flat Rate Formula.
         * Not sure if this would change depending on the other kinds of formulas that would be added, or any future country specific logic.
         * Currently, I don't see a need for it after the implementation of the database. Just a general country object for passing data seems good enough.
         * 
         * The db I would have added if I had time for this would have been SQLite. Using a in memory database is not a good choice
         * for a production app because you'd lose everything on restart, and right now the data needs are simple enough that SQLite would work fine.
         * It is also easier to set up and run in the context of an interview code review since the db is just a text file I could include in the solution
        */

        #endregion

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
