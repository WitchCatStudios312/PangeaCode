
namespace PangeaCode.Countries
{
    public class Mexico : ICountry
    {
        public string CountryCode => "MEX";

        public string CurrencyCode => "MXN";

        public decimal FlatRate => 0.024m;

        public override string ToString()
        {
            return "Mexico";
        }

    }
}
