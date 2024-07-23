namespace Homework_57_izida_kubekova
{
    public class Currency
    {
        public Currency(string currencyCode, string currencyName, double currencyRate)
        {
            CurrencyCode = currencyCode;
            CurrencyName = currencyName;
            CurrencyRate = currencyRate;
        }

        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public double CurrencyRate { get; set; }
        
    }
}