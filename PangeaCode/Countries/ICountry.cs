
namespace PangeaCode.Countries
{
    public interface ICountry
    {
        string CountryCode { get; }
        string CurrencyCode { get; }
        decimal FlatRate { get; }
    }
}
