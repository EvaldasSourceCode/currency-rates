namespace CurrencyRateChange.Service.Currency.Models
{
    public class CurrencyRateChange
    {
        public string Currency => TodayCurrency.CurrencyCode;

        public Currency TodayCurrency { get; set; }
        public Currency BeforeCurrency { get; set; }

        public decimal RateChange => TodayCurrency.Rate - BeforeCurrency.Rate;
    }
}
