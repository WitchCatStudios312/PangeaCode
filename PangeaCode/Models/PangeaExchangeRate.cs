namespace PangeaCode.Models
{
    public class PangeaExchangeRate
    {
        public PangeaExchangeRate() { }

        public string? CurrencyCode { get; internal set; }
        public string? CountryCode { get; internal set; }
        public decimal? PangeaRate { get; internal set; }
        public string? PaymentMethod { get; internal set; }
        public string? DeliveryMethod { get; internal set; }
    }
}