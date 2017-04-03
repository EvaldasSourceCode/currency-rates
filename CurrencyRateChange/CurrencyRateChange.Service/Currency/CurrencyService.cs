using System;
using System.Collections.Generic;
using CurrencyRateChange.Service.Currency.RestClients;
using System.Linq;
using System.Xml.Linq;
using CurrencyRateChange.Service.Currency.Comparers;
using System.Threading.Tasks;
using System.Globalization;

namespace CurrencyRateChange.Service.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILietuvosBankasRestClient currencyRestClient;

        public CurrencyService(ILietuvosBankasRestClient currencyRestClient)
        {
            this.currencyRestClient = currencyRestClient;
        }

        public async Task<IEnumerable<Models.CurrencyRateChange>> GetCurrencyRateChangesByDate(DateTime date)
        {
            var currencyRateChangesContent = await currencyRestClient.GetExchangeRatesByDate(date.ToString("yyyy-MM-dd"));

            var currencyRateChangesDayBeforeContent = await currencyRestClient.GetExchangeRatesByDate(date.AddDays(-1).ToString("yyyy-MM-dd"));
                        
            var currencyRateChanges = XDocument.Parse(currencyRateChangesContent).Element("ExchangeRates").Elements("item").Select(e => new Models.Currency { CurrencyCode = e.Element("currency").Value, Rate = decimal.Parse(e.Element("rate").Value, CultureInfo.InvariantCulture), Date = date, Quantity = int.Parse(e.Element("quantity").Value) });
                       
            var currencyRateChangesDayBefore = XDocument.Parse(currencyRateChangesDayBeforeContent).Element("ExchangeRates").Elements("item").Select(e => new Models.Currency { CurrencyCode = e.Element("currency").Value, Rate = decimal.Parse(e.Element("rate").Value, CultureInfo.InvariantCulture), Date = date.AddDays(-1), Quantity = int.Parse(e.Element("quantity").Value) });

            var result = currencyRateChanges
               .Join(currencyRateChangesDayBefore, d1 => d1, d2 => d2,
                   (d1, d2) => new Models.CurrencyRateChange
                   {
                       TodayCurrency = d1,
                       BeforeCurrency = d2
                   }, new CurrencyEqualityComparer()).ToList();

            return result.OrderByDescending(p => p.RateChange);
        }
    }
}
