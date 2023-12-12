using PangeaCode.Countries;
using PangeaCode.Models;

namespace PangeaCode.Formulas
{
    // Concrete Formula: FormulaB
    public class FormulaB : IFormula
    {
        /// <summary>
        /// FormulaB does not actually do anything to change the rates currently
        /// </summary>
        /// <param name="country"></param>
        /// <param name="partnerRates"></param>
        /// <returns></returns>
        private PangeaExchangeRate[] GetFormulaBRates(ICountry country, PartnerRate[] partnerRates)
        {
            string currencyCode = country.CurrencyCode;
            string countryCode = country.CountryCode;

            PangeaExchangeRate[] pangeaRates = new PangeaExchangeRate[partnerRates.Length];

            for (int i = 0; i < partnerRates.Length; i++)
            {
                PangeaExchangeRate p = new PangeaExchangeRate
                {
                    CurrencyCode = currencyCode,
                    CountryCode = countryCode,
                    PangeaRate = partnerRates[i].Rate,
                    PaymentMethod = partnerRates[i].PaymentMethod,
                    DeliveryMethod = partnerRates[i].DeliveryMethod,
                };
                pangeaRates[i] = p;
            }
            return pangeaRates;
        }

        public PangeaExchangeRate[] Calculate(ICountry country, PartnerRate[] partnerRates)
        {
            return GetFormulaBRates(country, partnerRates);
        }
    }
}