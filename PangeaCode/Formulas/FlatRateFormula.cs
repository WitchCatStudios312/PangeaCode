using PangeaCode.Countries;
using PangeaCode.Models;

namespace PangeaCode.Formulas
{
    // Concrete Formula: FlatRate
    public class FlatRateFormula : IFormula
    {
        private PangeaExchangeRate[] GetFlatRates(ICountry country, PartnerRate[] partnerRates)
        {
            decimal ratemodifier = country.FlatRate;
            string currencyCode = country.CurrencyCode;
            string countryCode = country.CountryCode;

            PangeaExchangeRate[] pangeaRates = new PangeaExchangeRate[partnerRates.Length];

            for (int i = 0; i < partnerRates.Length; i++)
            {
                decimal rate = partnerRates[i].Rate + ratemodifier;

                PangeaExchangeRate p = new PangeaExchangeRate
                {
                    CurrencyCode = currencyCode,
                    CountryCode = countryCode,
                    PangeaRate = Math.Round(rate, 2),
                    PaymentMethod = partnerRates[i].PaymentMethod,
                    DeliveryMethod = partnerRates[i].DeliveryMethod,
                };
                pangeaRates[i] = p;
            }
            return pangeaRates;
        }

        public PangeaExchangeRate[] Calculate(ICountry country, PartnerRate[] partnerRates)
        {
            return GetFlatRates(country, partnerRates);
        }
    }
}
