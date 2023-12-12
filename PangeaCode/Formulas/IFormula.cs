using PangeaCode.Countries;
using PangeaCode.Models;

namespace PangeaCode.Formulas
{
    public interface IFormula
    {
        PangeaExchangeRate[] Calculate(ICountry country, PartnerRate[] partnerRates);
    }
}
