using System;

namespace CurrencyRateChange.Service.Currency.Models
{
    public class Currency
    {
        public DateTime Date { set; get; }
        public string CurrencyCode { get; set; }
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
    }
}
