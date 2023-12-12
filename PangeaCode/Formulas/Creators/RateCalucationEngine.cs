using PangeaCode.Countries;
using PangeaCode.Models;

namespace PangeaCode.Formulas.Creators
{
    // Creator
    public abstract class RateCalucationEngine
    {
        public abstract IFormula CreateFormula(); //FactoryMethod

        public PangeaExchangeRate[] CalculatePangeaRates(ICountry country, PartnerRate[] partnerRates)
        {
            IFormula formula = CreateFormula();
            return formula.Calculate(country, partnerRates);
        }
    }
}
