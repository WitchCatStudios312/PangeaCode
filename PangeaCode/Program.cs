using PangeaCode;
using PangeaCode.Models;
public class Program
{
    public static async Task Main()
    {
        string selectedCountry = "MEX"; //This will act as the user/system input value for now

        PangeaExchangeRate[] pangeaRates = await PangeaExchangeRateService.CalculateExchangeRates(selectedCountry);

        PrintResults(pangeaRates);
    }

    private static void PrintResults(PangeaExchangeRate[] pangeaRates)
    {
        for (int i = 0; i < pangeaRates.Length; i++)
        {
            var rate = pangeaRates[i];
            Console.WriteLine($"CurrencyCode: {rate.CurrencyCode}");
            Console.WriteLine($"CountryCode: {rate.CountryCode}");
            Console.WriteLine($"PangeaRate: {rate.PangeaRate}");
            Console.WriteLine($"PaymentMethod: {rate.PaymentMethod}");
            Console.WriteLine($"DeliveryMethod: {rate.DeliveryMethod}");         
            Console.WriteLine();
        }
    }

}
