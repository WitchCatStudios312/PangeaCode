using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PangeaCode.Models;

namespace PangeaCode.Partners
{
    public class PartnerXYZ : IPartner
    {
        
        /// <summary>
        /// This is only used for now since when we call the API CountryCode is the correct value.
        /// Currently, the data in the file only contains CurrencyCode so we need to do a quick lookup
        /// </summary>
        private Dictionary<string, string> xrefCodes = new Dictionary<string, string>
        {
            { "MEX", "MXN" },
            { "IND", "INR" },
            { "PHL", "PHP" },
            { "GTM", "GTQ" }
        };

        private static HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5204/weatherforecast"),
        };



        public async Task<List<PartnerRate>> GetPartnerRatesAsync(string countryCode)
        {
            string currencyCode = xrefCodes[countryCode];
            List<PartnerRate> rates = await Task.Run(() => GetPartnerXYZRatesFromFile(currencyCode));
            return rates;

            //List<PartnerRate> rates = await GetPartnerXYZRatesFromAPI(currencyCode);
            //return rates;
        }



        /// <summary>
        /// Calls a simple server API I created to return the results of the sample json string
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        private async Task<List<PartnerRate>> GetPartnerXYZRatesFromAPI(string currencyCode)
        {
            HttpResponseMessage result = await httpClient.GetAsync("/partnerxyz");
            var jsonResponse = await result.Content.ReadAsStringAsync();

            PartnerData partnerData = JsonSerializer.Deserialize<PartnerData>(jsonResponse)!;
            PartnerRate[] partnerRates = (PartnerRate[])partnerData.PartnerRates.Where(rate => rate.Currency == currencyCode).ToArray();
            return [.. ValidateMostRecentRates(partnerRates)];
        }

        /// <summary>
        /// Get the unadjusted rates from PartnerXYZ for a specific country using the currency code
        /// </summary>
        /// <param name="CurrencyCode"></param>
        /// <returns>PartnerRate[]</returns>
        private List<PartnerRate> GetPartnerXYZRatesFromFile(string currencyCode)
        {
            string fileName = @"C:\Users\lvanv\Desktop\Code Challenges\PangeaCode\PangeaCode\PartnerXYZRates.json";

            string jsonString = File.ReadAllText(fileName);
            PartnerData partnerData = JsonSerializer.Deserialize<PartnerData>(jsonString)!;
            PartnerRate[] partnerRates = (PartnerRate[])partnerData.PartnerRates.Where(rate => rate.Currency == currencyCode).ToArray();
            return [.. ValidateMostRecentRates(partnerRates)];
        }

        /// <summary>
        /// Only use the most recent exchange rate for each Delivery/Payment method pairing
        /// 
        /// NOTE: Currently we only have 1 partner so this method can live here.
        /// If in the future other partners can utilize this same method we can extract it out to a common utilities file
        /// </summary>
        /// <param name="partnerRates"></param>
        /// <param name="CurrencyCode"></param>
        /// <returns>PartnerRate[]</returns>
        private PartnerRate[] ValidateMostRecentRates(PartnerRate[] partnerRates)
        {
            var filteredRates = partnerRates.GroupBy(rate => new { rate.DeliveryMethod, rate.PaymentMethod })
                                            .Select(rate => rate.OrderByDescending(rate => rate.AcquiredDate).First()).ToArray();
            return filteredRates;
        }     
        
    }
}
